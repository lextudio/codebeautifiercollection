/*
_____________________________________
© Pedro Miguel C. Cardoso 2007.
All rights reserved.
http://pmcchp.com/

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions are met:

1) Redistributions of source code must retain the above copyright notice, 
   this list of conditions and the following disclaimer. 
2) Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 
3) Neither the name of the ORGANIZATION nor the names of its contributors
   may be used to endorse or promote products derived from this software
   without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/
using System.ComponentModel;
using System.Drawing;
using System;
using System.Windows.Forms;
using Vista_Api.Dialog.Native;
namespace Vista_Api
{
    public partial class CommandLink : System.Windows.Forms.Button
    {

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (NativeMethods.IsVistaOrLater)
                    cp.Style |= NativeMethods.BS_COMMANDLINK;
                return cp;
            }
        }

        #region " Declarations "

        private DisplayStyle mDisplayStyle=DisplayStyle.Arrow;
        private bool mMouseOver;
        private bool mActivated;
        private Color textColor;
        private Color activeTextColor;
        private string _text;
        private string _description;

        public event EventHandler Selected;

        public enum DisplayStyle
        {
            Arrow,
            Shield,
        }

        #endregion

        #region " Constructors "

        public CommandLink()
        {
            InitializeComponent();
            // This call is required by the Windows Form Designer.
            if (NativeMethods.IsVistaOrLater)
            {
                base.FlatStyle = FlatStyle.System;
                lblDescription.Visible = false;
                lblDescription.Visible = false;
                picIcon.Visible = false;
            }
            else
            {
                base.FlatStyle = FlatStyle.Standard;
                MouseEnter += new EventHandler(CommandLink_MouseEnter);
                MouseLeave += new EventHandler(CommandLink_MouseLeave);
                lblDescription.MouseLeave += new EventHandler(CommandLink_MouseLeave);
                lblText.MouseLeave += new EventHandler(CommandLink_MouseLeave);
                picIcon.MouseLeave += new EventHandler(CommandLink_MouseLeave);
                
                Click+=new EventHandler(CommandLink_Click);
                lblDescription.Click += new EventHandler(lblDescription_Click);
                lblText.Click += new EventHandler(lblDescription_Click);
                picIcon.Click += new EventHandler(lblDescription_Click);
                picIcon.Image = new Bitmap(GetType(), "Images.restarrow.gif");
                 
                // Add any initialization after the InitializeComponent() call.
                if (this.Parent!=null)
                    this.BackColor = this.Parent.BackColor;

                textColor = Color.FromArgb(21, 28, 85);
                activeTextColor = Color.FromArgb(7, 74, 229);

                lblText.ForeColor = textColor;
                lblDescription.ForeColor = textColor;

                this.MouseOver = false;
                this.Activated = false;
            }
        }

        void lblDescription_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        public CommandLink(DisplayStyle style)
            : this()
        {
            this.Style = style;
        }

        public CommandLink(DisplayStyle style, string text)
            : this(style)
        {
            this.Text = text;
        }

        public CommandLink(DisplayStyle style, string text, string description)
            : this(style, text)
        {
            this.Description = description;
        }

        #endregion

        #region " Public Properties "

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        [DefaultValue("Lorem Ipsum")]
        public override string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (NativeMethods.IsVistaOrLater)
                    base.Text = value;
                else
                    lblText.Text = value;
                _text = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        [DefaultValue("Lorem Ipsum")]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (NativeMethods.IsVistaOrLater)
                {
                    NativeMethods.SendMessage(this.Handle,
                     NativeMethods.BCM_SETNOTE,
                     IntPtr.Zero, value);
                }
                else
                {
                    lblDescription.Text = value;
                }
                _description = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        [DefaultValue(DisplayStyle.Arrow)]
        public DisplayStyle Style
        {
            get
            {
                return mDisplayStyle;
            }
            set
            {
                bool bInv = false;
                if (mDisplayStyle != value)
                {
                    bInv = true;
                }
                mDisplayStyle = value;
                if (bInv)
                {
                    if (NativeMethods.IsVistaOrLater)
                    {
                        if (mDisplayStyle == DisplayStyle.Shield)
                            NativeMethods.SendMessage(this.Handle, NativeMethods.BCM_SETSHIELD, 0, 1);
                    }
                    else
                    {
                        base.FlatStyle = FlatStyle.Standard;

                        switch (mDisplayStyle)
                        {
                            case DisplayStyle.Arrow:
                                if (mMouseOver == true)
                                {
                                    picIcon.Image = new Bitmap(GetType(), "Images.selarrow.gif");
                                }
                                else
                                {
                                    picIcon.Image = new Bitmap(GetType(), "Images.restarrow.gif");
                                }

                                break;
                            case DisplayStyle.Shield:
                                picIcon.Image = new Bitmap(GetType(), "Images.shield.gif");
                                break;
                            default:
                                picIcon.Image = null;
                                break;
                        }
                        this.Invalidate();
                    }
                }
            }
        }

        #endregion

        #region " Public Methods "

        public void ActivateChanged(bool activate)
        {
            Activated = activate;
        }

        #endregion

        #region " Hidden Properties "

        private bool Default
        {
            get
            {
                return (object.ReferenceEquals(this.FindForm().AcceptButton, this));
            }
        }

        private bool MouseOver
        {
            get
            {
                return mMouseOver;
            }
            set
            {
                bool bInv = false;
                if (mMouseOver != value)
                {
                    bInv = true;
                }
                mMouseOver = value;
                if (!NativeMethods.IsVistaOrLater && bInv)
                {
                    if (mMouseOver == true)
                    {
                        lblText.ForeColor = activeTextColor;
                        lblDescription.ForeColor = activeTextColor;
                        if (Style == DisplayStyle.Arrow)
                        {
                            picIcon.Image = new Bitmap(GetType(), "Images.selarrow.gif");
                        }
                    }
                    else
                    {
                        lblText.ForeColor = textColor;
                        lblDescription.ForeColor = textColor;
                        if (Style == DisplayStyle.Arrow)
                        {
                            picIcon.Image = new Bitmap(GetType(), "Images.restarrow.gif");
                        }
                    }
                    this.Invalidate();
                }
            }
        }

        private bool Activated
        {
            get
            {
                return mActivated;
            }
            set
            {
                bool bInv = false;
                if (mActivated != value)
                {
                    bInv = true;
                }
                mActivated = value;
                if (bInv)
                {
                    this.Invalidate();
                }
            }
        }

        #endregion

        #region " Overridden Events "

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (NativeMethods.IsVistaOrLater)
            {
                base.OnPaint(e);
            }
            else
            {
                if ( base.FlatStyle != FlatStyle.Standard)
                    base.FlatStyle = FlatStyle.Standard;

                Pen oPen = null;

                Rectangle r = this.ClientRectangle;
                Brush oBrush = null;

                r.Width -= 1;
                r.Height -= 1;

                if (MouseOver)
                {
                    //the mouse is over is, draw the hover border
                    oPen = new Pen(Color.FromArgb(198, 198, 198));
                    oBrush = new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, this.BackColor, Color.FromArgb(246, 246, 246), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                }
                else if (Activated && Default)
                {
                    //draw the blue border
                    oPen = new Pen(Color.FromArgb(198, 244, 255));
                    r.Width -= 2;
                    r.Height -= 2;
                    r.X += 1;
                    r.Y += 1;
                }

                if (oBrush == null)
                {
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), this.ClientRectangle);
                }
                else
                {
                    e.Graphics.FillRectangle(oBrush, this.ClientRectangle);
                    oBrush.Dispose();
                }
                if (oPen != null)
                {
                    oPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                    //top line
                    e.Graphics.DrawLine(oPen, r.X + 2, r.Y, r.X + r.Width - 2, r.Y);
                    //dot
                    e.Graphics.DrawLine(oPen, r.X + r.Width - 2, r.Y, r.X + r.Width, r.Y + 2);

                    //rhs
                    e.Graphics.DrawLine(oPen, r.X + r.Width, r.Y + 2, r.X + r.Width, r.Y + r.Height - 2);
                    //dot
                    e.Graphics.DrawLine(oPen, r.X + r.Width, r.Y + r.Height - 2, r.X + r.Width - 2, r.Y + r.Height);

                    //bottom
                    e.Graphics.DrawLine(oPen, r.X + r.Width - 2, r.Y + r.Height, r.X + 2, r.Y + r.Height);
                    //dot
                    e.Graphics.DrawLine(oPen, r.X + 2, r.Y + r.Height, r.X, r.Y + r.Height - 2);

                    //lhs
                    e.Graphics.DrawLine(oPen, r.X, r.Y + r.Height - 2, r.X, r.Y + 2);
                    //dot
                    e.Graphics.DrawLine(oPen, r.X, r.Y + 2, r.X + 2, r.Y);

                    oPen.Dispose();
                }
            }
        }

        #endregion

        #region " Events Handlers "

        private void CommandLink_Click(object sender, System.EventArgs e)
        {
            if (Selected != null)
            {
                Selected(this, EventArgs.Empty);
            }
        }

        private void CommandLink_MouseEnter(object sender, System.EventArgs e)
        {
            MouseOver = true;
        }

        private void CommandLink_MouseLeave(object sender, System.EventArgs e)
        {
            Point p = this.PointToClient(Control.MousePosition);
            MouseOver = this.ClientRectangle.Contains(p);
            this.Invalidate();
        }

        #endregion

    }
}