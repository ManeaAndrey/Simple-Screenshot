using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Simple_Screenshot.Services
{
    public class ThemeManager
    {
        private Dictionary<Control, (Color BackColor, Color ForeColor)> originalColors = new();
        private const int DarkModeBackgroundColor = 0x2D2D30; // Color.FromArgb(45, 45, 48)

        public void StoreOriginalColors(Control control)
        {
            originalColors[control] = (control.BackColor, control.ForeColor);
            foreach (Control child in control.Controls)
            {
                StoreOriginalColors(child);
            }
        }

        public void SwitchDarkMode(Control control, bool darkMode)
        {
            if (darkMode)
            {
                control.BackColor = Color.FromArgb(45, 45, 48);
                control.ForeColor = Color.White;
            }
            else
            {
                // Restore original colors
                if (originalColors.TryGetValue(control, out var colors))
                {
                    control.BackColor = colors.BackColor;
                    control.ForeColor = colors.ForeColor;
                }
                else
                {
                    control.BackColor = SystemColors.Control;
                    control.ForeColor = SystemColors.ControlText;
                }
            }
            foreach (Control child in control.Controls)
            {
                SwitchDarkMode(child, darkMode);
            }
        }
    }
}
