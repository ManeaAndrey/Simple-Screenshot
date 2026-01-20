using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Tesseract;

namespace Simple_Screenshot.Services
{
    public class OcrService : IDisposable
    {
        private TesseractEngine? engine;
        private readonly string tessDataPath;
        private bool disposed = false;

        public OcrService(string tessDataPath)
        {
            this.tessDataPath = tessDataPath;
            InitializeEngine();
        }

        private void InitializeEngine()
        {
            try
            {
                if (!Directory.Exists(tessDataPath))
                {
                    throw new DirectoryNotFoundException($"Tesseract data directory not found at: {tessDataPath}");
                }

                var engFile = Path.Combine(tessDataPath, "eng.traineddata");
                if (!File.Exists(engFile))
                {
                    throw new FileNotFoundException($"Language file not found at: {engFile}");
                }

                engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to initialize Tesseract engine. Ensure tessdata folder exists at: {tessDataPath}",
                    ex);
            }
        }

        /// <summary>
        /// Extracts text from a bitmap image using OCR.
        /// </summary>
        public async Task<string> ExtractTextFromBitmapAsync(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            if (engine == null)
                throw new InvalidOperationException("Tesseract engine not initialized.");

            if (disposed)
                throw new ObjectDisposedException(nameof(OcrService));

            return await Task.Run(() =>
            {
                try
                {
                    using (var page = engine.Process(bitmap))
                    {
                        var text = page.GetText();
                        return text;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to extract text from bitmap.", ex);
                }
            });
        }

        /// <summary>
        /// Extracts text from an image file using OCR.
        /// </summary>
        public async Task<string> ExtractTextFromFileAsync(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException($"Image file not found: {imagePath}");

            using var bitmap = new Bitmap(imagePath);
            return await ExtractTextFromBitmapAsync(bitmap);
        }

        /// <summary>
        /// Saves extracted text to a file alongside the screenshot.
        /// </summary>
        public void SaveExtractedText(string extractedText, string screenshotPath)
        {
            if (string.IsNullOrEmpty(extractedText))
                return;

            try
            {
                var textFilePath = Path.ChangeExtension(screenshotPath, ".txt");
                File.WriteAllText(textFilePath, extractedText);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save extracted text.", ex);
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            engine?.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }

        ~OcrService()
        {
            Dispose();
        }
    }
}