using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Screenshot.Services
{
    public class ScreenshotService : IDisposable
    {
        private OcrService? ocrService;
        private readonly string tessDataPath;
        private bool disposed = false;
        private const float TargetDpi = 300f;
        private const float ScaleFactor = TargetDpi / 96f; // Standard screen DPI is 96

        public ScreenshotService()
        {
            tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            InitializeOcr();
        }

        private void InitializeOcr()
        {
            try
            {
                ocrService = new OcrService(tessDataPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OCR initialization failed: {ex.Message}");
            }
        }

        public void TakeScreenshot()
        {
            var bounds = SystemInformation.VirtualScreen;
            using var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            }

            var pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var dir = Path.Combine(pictures, "SimpleScreenshots");
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            bitmap.Save(file, ImageFormat.Png);

            ShowNotification("Screenshot saved", file);
        }

        public async Task TakeScreenshotWithTextExtractionAsync()
        {
            if (ocrService == null)
            {
                MessageBox.Show("OCR service failed to initialize. Check that tessdata folder exists with eng.traineddata file.", "OCR Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TakeScreenshot();
                return;
            }

            var bounds = SystemInformation.VirtualScreen;
            using var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            }

            var pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var dir = Path.Combine(pictures, "SimpleScreenshots");
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            
            try
            {
                bitmap.Save(file, ImageFormat.Png);
                System.Diagnostics.Debug.WriteLine($"[OCR] Screenshot saved to: {file}");
            }
            catch (Exception saveEx)
            {
                MessageBox.Show($"Failed to save screenshot: {saveEx.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"[OCR] Upscaling screenshot to 300 DPI...");
                using var upscaledBitmap = UpscaleTo300Dpi(bitmap);
                
                System.Diagnostics.Debug.WriteLine($"[OCR] Converting screenshot to greyscale for OCR...");
                using var greyscaleBitmap = ConvertToGreyscale(upscaledBitmap);
                
                System.Diagnostics.Debug.WriteLine($"[OCR] Starting text extraction for: {file}");
                var extractedText = await ocrService.ExtractTextFromBitmapAsync(greyscaleBitmap);
                System.Diagnostics.Debug.WriteLine($"[OCR] Text extracted. Length: {extractedText?.Length ?? 0} characters");
                
                if (string.IsNullOrEmpty(extractedText))
                {
                    MessageBox.Show("OCR returned empty text. Check if the screenshot contains readable text.", "Empty OCR Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                ocrService.SaveExtractedText(extractedText, file);
                var textFile = Path.ChangeExtension(file, ".txt");
                
                if (File.Exists(textFile))
                {
                    System.Diagnostics.Debug.WriteLine($"[OCR] Text file created successfully: {textFile}");
                    ShowNotification("Screenshot & text extracted", file);
                }
                else
                {
                    MessageBox.Show($"Text file was not created at: {textFile}", "File Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[OCR] FAILED - {ex.GetType().Name}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[OCR] Inner exception: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");
                }
                MessageBox.Show($"OCR extraction failed:\n\n{ex.Message}\n\nStack: {ex.StackTrace}", "OCR Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowNotification("Screenshot saved (OCR failed)", file);
            }
        }

        /// <summary>
        /// Upscales the bitmap to 300 DPI using high-quality interpolation.
        /// Standard screen DPI is 96, so this scales by approximately 3.125x.
        /// </summary>
        private Bitmap UpscaleTo300Dpi(Bitmap original)
        {
            var newWidth = (int)(original.Width * ScaleFactor);
            var newHeight = (int)(original.Height * ScaleFactor);
            
            System.Diagnostics.Debug.WriteLine($"[OCR] Upscaling from {original.Width}x{original.Height} to {newWidth}x{newHeight}");
            
            var upscaled = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
            upscaled.SetResolution(TargetDpi, TargetDpi);
            
            using (var g = Graphics.FromImage(upscaled))
            {
                // Use highest quality interpolation
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                
                g.DrawImage(original, new Rectangle(0, 0, newWidth, newHeight),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
            }

            return upscaled;
        }

        /// <summary>
        /// Converts a bitmap to greyscale for improved OCR accuracy.
        /// </summary>
        private Bitmap ConvertToGreyscale(Bitmap original)
        {
            var greyscale = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            greyscale.SetResolution(original.HorizontalResolution, original.VerticalResolution);
            
            using (var g = Graphics.FromImage(greyscale))
            {
                // Create a greyscale color matrix
                var colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                    new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                    new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                using var imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix);
                
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            return greyscale;
        }

        private void ShowNotification(string title, string message)
        {
            var ni = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Application,
                BalloonTipTitle = title,
                BalloonTipText = message
            };
            ni.ShowBalloonTip(3000);

            var cleanupTimer = new System.Windows.Forms.Timer { Interval = 4000 };
            cleanupTimer.Tick += (s, e) =>
            {
                cleanupTimer.Stop();
                cleanupTimer.Dispose();
                ni.Dispose();
            };
            cleanupTimer.Start();
        }

        public void Dispose()
        {
            if (disposed)
                return;

            ocrService?.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }

        ~ScreenshotService()
        {
            Dispose();
        }
    }
}
