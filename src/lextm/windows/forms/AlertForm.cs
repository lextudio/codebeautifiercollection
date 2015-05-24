// this is the alert form class.
// 		original file is copyrighted by Mukund Pujari.
// 		The file can be found on The Code Project in the article,
// 		''Some Cool Tips for .NET''.
//		This version is modified to suit my needs.
// Copyright (C) 2006  Lex Y. Li

// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lextm.Windows.Forms
{
	/// <summary>
	/// Alert form.
	/// </summary>
    /// <remarks>Something like the MSN messager's message.</remarks>
	public class AlertForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer fadeInTimer;
		private System.ComponentModel.IContainer components;
		int X;
		int Y;
		private System.Windows.Forms.Timer fadeOutTimer;
		private System.Windows.Forms.Timer waitTimer;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel llTip;
		/// <summary>
		/// Construstor.
		/// </summary>
		/// <param name="tip">Tip to display</param>
		private AlertForm(string tip)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			llTip.Text = tip;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code 
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertForm));
            this.fadeInTimer = new System.Windows.Forms.Timer(this.components);
            this.fadeOutTimer = new System.Windows.Forms.Timer(this.components);
            this.waitTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.llTip = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // fadeInTimer
            // 
            this.fadeInTimer.Enabled = true;
            this.fadeInTimer.Interval = 50;
            this.fadeInTimer.Tick += new System.EventHandler(this.fadeInTimer_Tick);
            // 
            // fadeOutTimer
            // 
            this.fadeOutTimer.Interval = 50;
            this.fadeOutTimer.Tick += new System.EventHandler(this.fadeOutTimer_Tick);
            // 
            // waitTimer
            // 
            this.waitTimer.Interval = 3000;
            this.waitTimer.Tick += new System.EventHandler(this.waitTimer_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(226, 78);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // llTip
            // 
            this.llTip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llTip.Location = new System.Drawing.Point(0, 78);
            this.llTip.Name = "llTip";
            this.llTip.Size = new System.Drawing.Size(226, 66);
            this.llTip.TabIndex = 2;
            this.llTip.TabStop = true;
            this.llTip.Text = "New Article has been added to Codeproject.com.Rate this article if you like it.";
            this.llTip.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AlertForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(226, 144);
            this.Controls.Add(this.llTip);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(232, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(232, 0);
            this.Name = "AlertForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CodeProject Alert";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        /// <summary>Show alert form.</summary>
		/// <param name="tip">Tip to display</param>
		public static void ShowForm(string tip) {
			AlertForm alert = new AlertForm(tip);
			alert.Show();
        }

		# region Code in Form Load  
		private void Form1_Load(object sender, System.EventArgs e)
		{
			/*The Screen.GetWorkingArea(control) will provide you the working
			area of a screen without the system tray area. The working area 
			depends on the resolution of your monitor*/
			
			X=Screen.GetWorkingArea(this).Width; // This line gives the width of working area
			Y=Screen.GetWorkingArea(this).Height; // This line gives the width of working area			
			this.Location=new Point(X-this.Width,Y+this.Height); // This line sets the initial location of the form
			
			fadeInTimer.Enabled=true;	
			fadeInTimer.Start();   // We'll start the timer which handles the opening of form.
		}
		
		# endregion

		# region Code for Opening the Form and displaying it for some time 
		private void fadeInTimer_Tick(object sender, System.EventArgs e)
		{
			/* The logic is that, first we will open the form below
			   taskbar at specified location and then in Timer's TICK event 
			   we'll just bring the form above taskbar to have the animated 
			   effect.
			  */
			int i = this.Location.Y; // First we will know form's Y-axis location
									 // which will act as upper limit for form's location
			if(i>Y-this.Height)		
			{				
				this.Location=new Point(X-this.Width,i-8);	// Here we just change the location
															// of form by 8 pixels(i-8)
			}
			else   // else we stop Timer1 and start Timer3 which holds the form for some time
			{
				fadeInTimer.Stop();
				fadeInTimer.Enabled=false;

				waitTimer.Start();
				waitTimer.Enabled=true;
			}
		}

		# endregion

		# region Code for Closing the Form  
		private void fadeOutTimer_Tick(object sender, System.EventArgs e)
		{
			/*
			   Here the logic is same as opening the form, only location value of Y-axis
			   is increased, so that form slowly hides behind Task Bar.
			 */
			waitTimer.Stop();
			waitTimer.Enabled=false;

			int i = this.Location.Y;
			if(i<Y)
			{
				this.Location=new Point(X-this.Width,i+8);	
			}
			else
			{
				fadeOutTimer.Stop();
				fadeOutTimer.Enabled=false;
				this.Close();				
			}
		}

		# endregion

		# region Start Closing the Form  
		private void waitTimer_Tick(object sender, System.EventArgs e)
		{
			fadeOutTimer.Start();          // Here we start Timer2 which handles closing of form
			fadeOutTimer.Enabled=true;
		}
		# endregion	    	
        
		
	}
}
