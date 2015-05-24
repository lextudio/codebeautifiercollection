using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using BeWise.Common.Utils;
using Lextm.LeXDK;

namespace ArtCSB
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	class WinForm_AutoSave : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkBox_AutoSave;
		private System.Windows.Forms.NumericUpDown numericUpDown_Delay;
		private System.Windows.Forms.Button button_Ok;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Panel panel_Sound;
		private System.Windows.Forms.Button button_OpenWav;
		private System.Windows.Forms.TextBox textBox_Wav;
		private System.Windows.Forms.NumericUpDown numericUpDown_Frequency;
		private System.Windows.Forms.Button button_TestSound;
		private System.Windows.Forms.CheckBox checkBox_PlaySound;
		private System.Windows.Forms.NumericUpDown numericUpDown_Duration;
		private System.Windows.Forms.Label label_Duration;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioButton_Beep;
		private System.Windows.Forms.RadioButton radioButton_Wav;
		private Vista_Api.OpenFileDialog openFileDialog_Wav;
		private System.Windows.Forms.Panel panel_BeepAttributes;
		private System.Windows.Forms.Panel panel_AutoSaveDelay;
		private System.Windows.Forms.RadioButton radioButton_Minutes;
		private System.Windows.Forms.RadioButton radioButton_Seconds;
		private System.Windows.Forms.Panel panel_WavAttributes;
		private System.Windows.Forms.Button button_About;

		internal WinForm_AutoSave()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinForm_AutoSave));
			this.checkBox_AutoSave = new System.Windows.Forms.CheckBox();
			this.button_Ok = new System.Windows.Forms.Button();
			this.button_Cancel = new System.Windows.Forms.Button();
			this.panel_Sound = new System.Windows.Forms.Panel();
			this.panel_WavAttributes = new System.Windows.Forms.Panel();
			this.button_OpenWav = new System.Windows.Forms.Button();
			this.textBox_Wav = new System.Windows.Forms.TextBox();
			this.panel_BeepAttributes = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label_Duration = new System.Windows.Forms.Label();
			this.numericUpDown_Duration = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_Frequency = new System.Windows.Forms.NumericUpDown();
			this.radioButton_Wav = new System.Windows.Forms.RadioButton();
			this.radioButton_Beep = new System.Windows.Forms.RadioButton();
			this.button_TestSound = new System.Windows.Forms.Button();
			this.checkBox_PlaySound = new System.Windows.Forms.CheckBox();
			this.openFileDialog_Wav1 = new System.Windows.Forms.OpenFileDialog();
			this.panel_AutoSaveDelay = new System.Windows.Forms.Panel();
			this.radioButton_Seconds = new System.Windows.Forms.RadioButton();
			this.radioButton_Minutes = new System.Windows.Forms.RadioButton();
			this.numericUpDown_Delay = new System.Windows.Forms.NumericUpDown();
			this.button_About = new System.Windows.Forms.Button();
			this.openFileDialog_Wav = new Vista_Api.OpenFileDialog();
			this.panel_Sound.SuspendLayout();
			this.panel_WavAttributes.SuspendLayout();
			this.panel_BeepAttributes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Duration)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Frequency)).BeginInit();
			this.panel_AutoSaveDelay.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Delay)).BeginInit();
			this.SuspendLayout();
			// 
			// checkBox_AutoSave
			// 
			this.checkBox_AutoSave.Location = new System.Drawing.Point(19, 26);
			this.checkBox_AutoSave.Name = "checkBox_AutoSave";
			this.checkBox_AutoSave.Size = new System.Drawing.Size(250, 17);
			this.checkBox_AutoSave.TabIndex = 0;
			this.checkBox_AutoSave.Text = "automatically save IDE project every";
			this.checkBox_AutoSave.CheckedChanged += new System.EventHandler(this.checkBox_AutoSave_CheckedChanged);
			// 
			// button_Ok
			// 
			this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Ok.Location = new System.Drawing.Point(374, 258);
			this.button_Ok.Name = "button_Ok";
			this.button_Ok.Size = new System.Drawing.Size(90, 25);
			this.button_Ok.TabIndex = 3;
			this.button_Ok.Text = "OK";
			this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
			// 
			// button_Cancel
			// 
			this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_Cancel.Location = new System.Drawing.Point(499, 258);
			this.button_Cancel.Name = "button_Cancel";
			this.button_Cancel.Size = new System.Drawing.Size(90, 25);
			this.button_Cancel.TabIndex = 4;
			this.button_Cancel.Text = "Cancel";
			this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
			// 
			// panel_Sound
			// 
			this.panel_Sound.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel_Sound.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel_Sound.Controls.Add(this.panel_WavAttributes);
			this.panel_Sound.Controls.Add(this.panel_BeepAttributes);
			this.panel_Sound.Controls.Add(this.radioButton_Wav);
			this.panel_Sound.Controls.Add(this.radioButton_Beep);
			this.panel_Sound.Controls.Add(this.button_TestSound);
			this.panel_Sound.Enabled = false;
			this.panel_Sound.Location = new System.Drawing.Point(10, 103);
			this.panel_Sound.Name = "panel_Sound";
			this.panel_Sound.Size = new System.Drawing.Size(585, 138);
			this.panel_Sound.TabIndex = 16;
			// 
			// panel_WavAttributes
			// 
			this.panel_WavAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel_WavAttributes.Controls.Add(this.button_OpenWav);
			this.panel_WavAttributes.Controls.Add(this.textBox_Wav);
			this.panel_WavAttributes.Enabled = false;
			this.panel_WavAttributes.Location = new System.Drawing.Point(96, 86);
			this.panel_WavAttributes.Name = "panel_WavAttributes";
			this.panel_WavAttributes.Size = new System.Drawing.Size(480, 43);
			this.panel_WavAttributes.TabIndex = 27;
			// 
			// button_OpenWav
			// 
			this.button_OpenWav.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_OpenWav.Location = new System.Drawing.Point(241, 9);
			this.button_OpenWav.Name = "button_OpenWav";
			this.button_OpenWav.Size = new System.Drawing.Size(125, 24);
			this.button_OpenWav.TabIndex = 27;
			this.button_OpenWav.Text = "Open wav-file";
			this.button_OpenWav.Click += new System.EventHandler(this.button_OpenWav_Click);
			// 
			// textBox_Wav
			// 
			this.textBox_Wav.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox_Wav.Location = new System.Drawing.Point(11, 9);
			this.textBox_Wav.Name = "textBox_Wav";
			this.textBox_Wav.Size = new System.Drawing.Size(228, 21);
			this.textBox_Wav.TabIndex = 26;
			// 
			// panel_BeepAttributes
			// 
			this.panel_BeepAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel_BeepAttributes.Controls.Add(this.label3);
			this.panel_BeepAttributes.Controls.Add(this.label_Duration);
			this.panel_BeepAttributes.Controls.Add(this.numericUpDown_Duration);
			this.panel_BeepAttributes.Controls.Add(this.numericUpDown_Frequency);
			this.panel_BeepAttributes.Location = new System.Drawing.Point(96, 39);
			this.panel_BeepAttributes.Name = "panel_BeepAttributes";
			this.panel_BeepAttributes.Size = new System.Drawing.Size(480, 34);
			this.panel_BeepAttributes.TabIndex = 26;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(250, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(134, 17);
			this.label3.TabIndex = 25;
			this.label3.Text = "Beep frequency (Hz):";
			// 
			// label_Duration
			// 
			this.label_Duration.Location = new System.Drawing.Point(10, 9);
			this.label_Duration.Name = "label_Duration";
			this.label_Duration.Size = new System.Drawing.Size(126, 17);
			this.label_Duration.TabIndex = 24;
			this.label_Duration.Text = "Beep duration (ms):";
			// 
			// numericUpDown_Duration
			// 
			this.numericUpDown_Duration.Increment = new decimal(new int[] {
									10,
									0,
									0,
									0});
			this.numericUpDown_Duration.Location = new System.Drawing.Point(144, 9);
			this.numericUpDown_Duration.Maximum = new decimal(new int[] {
									5000,
									0,
									0,
									0});
			this.numericUpDown_Duration.Minimum = new decimal(new int[] {
									10,
									0,
									0,
									0});
			this.numericUpDown_Duration.Name = "numericUpDown_Duration";
			this.numericUpDown_Duration.Size = new System.Drawing.Size(77, 21);
			this.numericUpDown_Duration.TabIndex = 23;
			this.numericUpDown_Duration.Value = new decimal(new int[] {
									500,
									0,
									0,
									0});
			// 
			// numericUpDown_Frequency
			// 
			this.numericUpDown_Frequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown_Frequency.Increment = new decimal(new int[] {
									500,
									0,
									0,
									0});
			this.numericUpDown_Frequency.Location = new System.Drawing.Point(394, 9);
			this.numericUpDown_Frequency.Maximum = new decimal(new int[] {
									20000,
									0,
									0,
									0});
			this.numericUpDown_Frequency.Minimum = new decimal(new int[] {
									500,
									0,
									0,
									0});
			this.numericUpDown_Frequency.Name = "numericUpDown_Frequency";
			this.numericUpDown_Frequency.Size = new System.Drawing.Size(76, 21);
			this.numericUpDown_Frequency.TabIndex = 22;
			this.numericUpDown_Frequency.Value = new decimal(new int[] {
									5000,
									0,
									0,
									0});
			// 
			// radioButton_Wav
			// 
			this.radioButton_Wav.Location = new System.Drawing.Point(10, 95);
			this.radioButton_Wav.Name = "radioButton_Wav";
			this.radioButton_Wav.Size = new System.Drawing.Size(86, 26);
			this.radioButton_Wav.TabIndex = 23;
			this.radioButton_Wav.Text = "Wav-file";
			this.radioButton_Wav.CheckedChanged += new System.EventHandler(this.radioButton_Wav_CheckedChanged);
			// 
			// radioButton_Beep
			// 
			this.radioButton_Beep.Checked = true;
			this.radioButton_Beep.Location = new System.Drawing.Point(10, 43);
			this.radioButton_Beep.Name = "radioButton_Beep";
			this.radioButton_Beep.Size = new System.Drawing.Size(96, 26);
			this.radioButton_Beep.TabIndex = 22;
			this.radioButton_Beep.TabStop = true;
			this.radioButton_Beep.Text = "Beep";
			this.radioButton_Beep.CheckedChanged += new System.EventHandler(this.radioButton_Beep_CheckedChanged);
			// 
			// button_TestSound
			// 
			this.button_TestSound.Location = new System.Drawing.Point(10, 9);
			this.button_TestSound.Name = "button_TestSound";
			this.button_TestSound.Size = new System.Drawing.Size(90, 24);
			this.button_TestSound.TabIndex = 17;
			this.button_TestSound.Text = "Test sound";
			this.button_TestSound.Click += new System.EventHandler(this.button_TestSound_Click);
			// 
			// checkBox_PlaySound
			// 
			this.checkBox_PlaySound.Enabled = false;
			this.checkBox_PlaySound.Location = new System.Drawing.Point(19, 69);
			this.checkBox_PlaySound.Name = "checkBox_PlaySound";
			this.checkBox_PlaySound.Size = new System.Drawing.Size(183, 26);
			this.checkBox_PlaySound.TabIndex = 17;
			this.checkBox_PlaySound.Text = "Play sound during saving";
			this.checkBox_PlaySound.CheckedChanged += new System.EventHandler(this.checkBox_PlaySound_CheckedChanged);
			// 
			// openFileDialog_Wav1
			// 
			this.openFileDialog_Wav1.DefaultExt = "WAV";
			this.openFileDialog_Wav1.Filter = "Wav-files|*.wav|All files|*.*";
			// 
			// panel_AutoSaveDelay
			// 
			this.panel_AutoSaveDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel_AutoSaveDelay.Controls.Add(this.radioButton_Seconds);
			this.panel_AutoSaveDelay.Controls.Add(this.radioButton_Minutes);
			this.panel_AutoSaveDelay.Controls.Add(this.numericUpDown_Delay);
			this.panel_AutoSaveDelay.Enabled = false;
			this.panel_AutoSaveDelay.Location = new System.Drawing.Point(259, 9);
			this.panel_AutoSaveDelay.Name = "panel_AutoSaveDelay";
			this.panel_AutoSaveDelay.Size = new System.Drawing.Size(336, 69);
			this.panel_AutoSaveDelay.TabIndex = 20;
			// 
			// radioButton_Seconds
			// 
			this.radioButton_Seconds.Location = new System.Drawing.Point(76, 33);
			this.radioButton_Seconds.Name = "radioButton_Seconds";
			this.radioButton_Seconds.Size = new System.Drawing.Size(124, 18);
			this.radioButton_Seconds.TabIndex = 22;
			this.radioButton_Seconds.Text = "second(s)";
			// 
			// radioButton_Minutes
			// 
			this.radioButton_Minutes.Checked = true;
			this.radioButton_Minutes.Location = new System.Drawing.Point(76, 10);
			this.radioButton_Minutes.Name = "radioButton_Minutes";
			this.radioButton_Minutes.Size = new System.Drawing.Size(124, 17);
			this.radioButton_Minutes.TabIndex = 21;
			this.radioButton_Minutes.TabStop = true;
			this.radioButton_Minutes.Text = "minute(s)";
			// 
			// numericUpDown_Delay
			// 
			this.numericUpDown_Delay.Location = new System.Drawing.Point(8, 18);
			this.numericUpDown_Delay.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericUpDown_Delay.Name = "numericUpDown_Delay";
			this.numericUpDown_Delay.Size = new System.Drawing.Size(59, 21);
			this.numericUpDown_Delay.TabIndex = 20;
			this.numericUpDown_Delay.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// button_About
			// 
			this.button_About.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button_About.Location = new System.Drawing.Point(10, 258);
			this.button_About.Name = "button_About";
			this.button_About.Size = new System.Drawing.Size(90, 25);
			this.button_About.TabIndex = 21;
			this.button_About.Text = "About";
			this.button_About.Click += new System.EventHandler(this.button_About_Click);
			// 
			// openFileDialog_Wav
			// 
			this.openFileDialog_Wav.FileName = "openFileDialog1";
			this.openFileDialog_Wav.Filter = null;
			// 
			// WinForm_AutoSave
			// 
			this.AcceptButton = this.button_Ok;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.button_Cancel;
			this.ClientSize = new System.Drawing.Size(602, 292);
			this.Controls.Add(this.button_About);
			this.Controls.Add(this.panel_AutoSaveDelay);
			this.Controls.Add(this.checkBox_PlaySound);
			this.Controls.Add(this.panel_Sound);
			this.Controls.Add(this.button_Cancel);
			this.Controls.Add(this.button_Ok);
			this.Controls.Add(this.checkBox_AutoSave);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WinForm_AutoSave";
			this.Text = "Auto-Save Options";
			this.Load += new System.EventHandler(this.WinForm_AutoSave_Load);
			this.panel_Sound.ResumeLayout(false);
			this.panel_WavAttributes.ResumeLayout(false);
			this.panel_WavAttributes.PerformLayout();
			this.panel_BeepAttributes.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Duration)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Frequency)).EndInit();
			this.panel_AutoSaveDelay.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Delay)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.OpenFileDialog openFileDialog_Wav1;
		#endregion

		private void button_TestSound_Click(object sender, System.EventArgs e)
		{

			if (radioButton_Beep.Checked){
				bool result = WinAPI.Beep((uint)numericUpDown_Frequency.Value,(uint)numericUpDown_Duration.Value);
			}else{
				try{
					WinAPI.PlaySound(textBox_Wav.Text);
				}catch{
					Lextm.Windows.Forms.MessageBoxFactory.Error(
						String.Format("Can't play {0}",textBox_Wav.Text));
				}
			}
		}

		private void checkBox_AutoSave_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBox_AutoSave.Checked){
				panel_AutoSaveDelay.Enabled = true;
				checkBox_PlaySound.Enabled = true;
			}else{
				panel_AutoSaveDelay.Enabled = false;
				panel_Sound.Enabled = false;
				checkBox_PlaySound.Enabled = false;
				checkBox_PlaySound.Checked = false;
			}
		}

		private void checkBox_PlaySound_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBox_PlaySound.Checked){
				panel_Sound.Enabled = true;

			}else{
				panel_Sound.Enabled = false;
			}

		}

		private void radioButton_Beep_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButton_Beep.Checked){
				panel_BeepAttributes.Enabled = true;
				panel_WavAttributes.Enabled = false;
			}
		}

		private void radioButton_Wav_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButton_Wav.Checked){
				panel_WavAttributes.Enabled = true;
				panel_BeepAttributes.Enabled = false;
			}
		}

		private void button_OpenWav_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog_Wav.ShowDialog() == DialogResult.OK){
				textBox_Wav.Text = openFileDialog_Wav.FileName;
			}
		}
//		private Lextm.ArtCSB.Feature.AutoSaveFeature.OptionsType MyOps =
//			Lextm.ArtCSB.Feature.AutoSaveFeature.getInstance().Options as
//			Lextm.ArtCSB.Feature.AutoSaveFeature.OptionsType;
		
		private void WinForm_AutoSave_Load(object sender, System.EventArgs e)
		{
			checkBox_AutoSave.Checked = (bool)PropertyRegistry.Get("AutoSave", false);
			numericUpDown_Delay.Value = (int)PropertyRegistry.Get("AutoSave_Delay", (int)1);
			radioButton_Minutes.Checked = (bool)PropertyRegistry.Get("AutoSave_DelayInMinutes", true);
			radioButton_Seconds.Checked = !(bool)PropertyRegistry.Get("AutoSave_DelayInMinutes");
			checkBox_PlaySound.Checked = (bool)PropertyRegistry.Get("AutoSave_PlaySound", false);
			radioButton_Beep.Checked = (bool)PropertyRegistry.Get("AutoSave_Beep", true);
			numericUpDown_Duration.Value = (int)PropertyRegistry.Get("AutoSave_BeepDurationMs", 500);
			numericUpDown_Frequency.Value = (int)PropertyRegistry.Get("AutoSave_BeepFrequency", 5000);
			textBox_Wav.Text = (string)PropertyRegistry.Get("AutoSave_Wav", null);
		}

		private void button_Ok_Click(object sender, System.EventArgs e)
		{
			PropertyRegistry.Set("AutoSave", checkBox_AutoSave.Checked);
			PropertyRegistry.Set("AutoSave_Delay", Convert.ToInt32(numericUpDown_Delay.Value));
			PropertyRegistry.Set("AutoSave_DelayInMinutes", radioButton_Minutes.Checked);
			PropertyRegistry.Set("AutoSave_PlaySound", checkBox_PlaySound.Checked);
			PropertyRegistry.Set("AutoSave_Beep", radioButton_Beep.Checked);
			PropertyRegistry.Set("AutoSave_BeepDurationMs", Convert.ToInt32(numericUpDown_Duration.Value));
			PropertyRegistry.Set("AutoSave_BeepFrequency", Convert.ToInt32(numericUpDown_Frequency.Value));
			PropertyRegistry.Set("AutoSave_Wav", textBox_Wav.Text);
			PropertyRegistry.Flush();
			//FeatureRegistry.GetFeature("Lextm.ArtCSB.Feature.AutoSaveFeature").SavePreferences();
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void button_Cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void button_About_Click(object sender, System.EventArgs e) {
			WinForm_About  dlgAbt = new WinForm_About();
			if (this.TopMost) {dlgAbt.TopMost = true;}
			dlgAbt.ShowDialog();
			dlgAbt.Dispose();
		}

	}
}
