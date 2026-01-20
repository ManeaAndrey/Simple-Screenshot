using System;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Forms;
using Simple_Screenshot.Services;

namespace Simple_Screenshot
{
    public partial class Form1 : Form
    {
        string? SummaryAssist;
        int[] Prereq;
        bool DarkMode = false;
        bool IsOn = false;

        private readonly ScreenshotService screenshotService = new();
        private ThemeManager themeManager = new();
        private FileService fileService = new();

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            // Store original colors before any mode switching
            themeManager.StoreOriginalColors(this);

            // Check tessdata folder exists
            var tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            var engFile = Path.Combine(tessDataPath, "eng.traineddata");
            
            if (!File.Exists(engFile))
            {
                MessageBox.Show(
                    $"Tesseract language file not found!\n\n" +
                    $"Expected path: {engFile}\n\n" +
                    $"Please download eng.traineddata and place it in the tessdata folder.",
                    "Tesseract Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            var ok = HotKeyManager.RegisterHotkey(Handle, Keys.J, controlModifier: true, shiftModifier: true);
            if (!ok)
            {
                MessageBox.Show("Failed to register hotkey Ctrl+Shift+J", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            HotKeyManager.UnregisterHotkey(Handle);
            screenshotService?.Dispose();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == HotKeyManager.GetHotKeyMessage() && m.WParam.ToInt32() == HotKeyManager.GetHotkeyId())
            {
                try
                {
                    _ = screenshotService.TakeScreenshotWithTextExtractionAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error taking screenshot: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            base.WndProc(ref m);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SummaryAssist = textBox1.Text;
        }

        private void SummaryAssistActivatorButton1_Click(object sender, EventArgs e)
        {
            fileService.SaveSummary(SummaryAssist);
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
            var prerequisites = Prereq.Select(index => PCPrereqs.Items[index].ToString()).ToArray();
            fileService.SavePrerequisites(prerequisites);
        }

        private void label1_Click_2(object sender, EventArgs e)
        {
        }

        private void SwitchDarkMode(Control control, bool darkMode)
        {
            themeManager.SwitchDarkMode(control, darkMode);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DarkMode = !DarkMode;
            IsOn = !IsOn;
            SwitchDarkMode(this, DarkMode);
        }
    }
}
