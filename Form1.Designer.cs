namespace Simple_Screenshot
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SummaryAssistActivator = new Button();
            textBox1 = new TextBox();
            SummaryLabel = new Label();
            Prereqs = new Label();
            PrereqButton = new Button();
            PCPrereqs = new CheckedListBox();
            label1 = new Label();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // SummaryAssistActivator
            // 
            SummaryAssistActivator.Location = new Point(238, 76);
            SummaryAssistActivator.Name = "SummaryAssistActivator";
            SummaryAssistActivator.Size = new Size(126, 23);
            SummaryAssistActivator.TabIndex = 3;
            SummaryAssistActivator.Text = "Add Summary";
            SummaryAssistActivator.UseVisualStyleBackColor = true;
            SummaryAssistActivator.Click += SummaryAssistActivatorButton1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(87, 77);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(145, 23);
            textBox1.TabIndex = 5;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // SummaryLabel
            // 
            SummaryLabel.AutoSize = true;
            SummaryLabel.Location = new Point(23, 81);
            SummaryLabel.Name = "SummaryLabel";
            SummaryLabel.Size = new Size(58, 15);
            SummaryLabel.TabIndex = 7;
            SummaryLabel.Text = "Summary";
            SummaryLabel.Click += label1_Click;
            // 
            // Prereqs
            // 
            Prereqs.AutoSize = true;
            Prereqs.Location = new Point(23, 125);
            Prereqs.Name = "Prereqs";
            Prereqs.Size = new Size(46, 15);
            Prereqs.TabIndex = 8;
            Prereqs.Text = "Prereqs";
            Prereqs.Click += label1_Click_1;
            // 
            // PrereqButton
            // 
            PrereqButton.Location = new Point(238, 121);
            PrereqButton.Name = "PrereqButton";
            PrereqButton.Size = new Size(126, 23);
            PrereqButton.TabIndex = 13;
            PrereqButton.Text = "Add Prereq";
            PrereqButton.UseVisualStyleBackColor = true;
            PrereqButton.Click += PrereqButton_Click;
            // 
            // PCPrereqs
            // 
            PCPrereqs.FormattingEnabled = true;
            PCPrereqs.Items.AddRange(new object[] { "PC-Main-Raw", "PC-Main-Final", "PC-TU-Raw", "PC-TU-Final" });
            PCPrereqs.Location = new Point(87, 125);
            PCPrereqs.Name = "PCPrereqs";
            PCPrereqs.Size = new Size(145, 94);
            PCPrereqs.TabIndex = 14;
            PCPrereqs.SelectedIndexChanged += checkedListBox1_PCPrereqs;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Bisque;
            label1.Location = new Point(645, 426);
            label1.Name = "label1";
            label1.Size = new Size(143, 15);
            label1.TabIndex = 15;
            label1.Text = "Screenshot: CTRL+Shift+J";
            label1.Click += label1_Click_2;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = SystemColors.ControlDarkDark;
            checkBox1.ForeColor = SystemColors.ControlLight;
            checkBox1.Location = new Point(706, 12);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(84, 19);
            checkBox1.TabIndex = 17;
            checkBox1.Text = "Dark Mode";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(800, 450);
            Controls.Add(checkBox1);
            Controls.Add(label1);
            Controls.Add(PCPrereqs);
            Controls.Add(PrereqButton);
            Controls.Add(Prereqs);
            Controls.Add(SummaryLabel);
            Controls.Add(textBox1);
            Controls.Add(SummaryAssistActivator);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button SummaryAssistActivator;
        private TextBox textBox1;
        private Label SummaryLabel;
        private Label Prereqs;
        private Button PrereqButton;
        private CheckedListBox PCPrereqs;
        private Label label1;
        private CheckBox checkBox1;
    }
}
