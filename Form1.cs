using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Simple_Screenshot
{
    public partial class Form1 : Form
    {
        private const int HOTKEY_ID = 1;
        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;

        string? SummaryAssist;
        int[] Prereq;
        bool DarkMode = false;
        bool IsOn = false;


        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            FormClosing += Form1_FormClosing;

        }

        private void Form1_Load(object? sender, EventArgs e)
        {

            var ok = RegisterHotKey(Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, (uint)Keys.J);
            if (!ok)
            {
                MessageBox.Show("Failed to register hotkey Ctrl+Shift+J", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(Handle, HOTKEY_ID);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                try
                {
                    TakeScreenshot();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error taking screenshot: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            base.WndProc(ref m);
        }

        private void TakeScreenshot()
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


            var ni = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Application,
                BalloonTipTitle = "Screenshot saved",
                BalloonTipText = file
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            SummaryAssist = textBox1.Text;



        }

        private void SummaryAssistActivatorButton1_Click(object sender, EventArgs e)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dir = Path.Combine(documents, "Summary");
            File.WriteAllText(dir, SummaryAssist);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_PCPrereqs(object sender, EventArgs e)
        {
            Prereq = PCPrereqs.CheckedIndices.Cast<int>().ToArray();


        }
        private void PrereqButton_Click(object sender, EventArgs e)
        {

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dir = Path.Combine(documents, "Prereqs");

            foreach (int index in Prereq)
            {

                File.WriteAllText(dir, PCPrereqs.Items[index].ToString());

            }


        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

      
        private void SwitchDarkMode(Control control, bool darkMode)
        {
            if (darkMode)
            {
                control.BackColor = Color.FromArgb(45, 45, 48);
                control.ForeColor = Color.White;
            }
            else
            {
                control.BackColor = SystemColors.Control;
                control.ForeColor = SystemColors.ControlText;
            }
            foreach (Control child in control.Controls)
            {
                SwitchDarkMode(child, darkMode);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DarkMode = !DarkMode;
            IsOn = !IsOn;
            SwitchDarkMode(this, DarkMode);
        }
    }
}
