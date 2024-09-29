using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XDropDown
{
    public class XToolStripDropDown : System.Windows.Forms.ToolStripDropDown
    {
        private System.Windows.Forms.Control m_popedContainer;

        private ToolStripControlHost m_host;

        private bool m_fade = true;

        public XToolStripDropDown(System.Windows.Forms.Control popedControl)
        {
            InitializeComponent();
            if (popedControl == null)
                throw new ArgumentNullException("content");

            this.m_popedContainer = popedControl;

            this.m_fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;

            this.m_host = new ToolStripControlHost(popedControl);
            m_host.AutoSize = false;//make it take the same room as the poped control

            Padding = Margin = m_host.Padding = m_host.Margin = Padding.Empty;

            popedControl.Location = Point.Empty;

            this.Items.Add(m_host);

            popedControl.Disposed += delegate (object sender, EventArgs e)
            {
                popedControl = null;
                Dispose(true);// this popup container will be disposed immediately after disposion of the contained control
            };
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {//prevent alt from closing it and allow alt+menumonic to work
            if ((keyData & Keys.Alt) == Keys.Alt)
                return false;

            return base.ProcessDialogKey(keyData);
        }

        public void Show(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            Show(control, control.ClientRectangle);
        }

        public void Show(UserControl control)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            Show(control, control.ClientRectangle);
        }


        public void Show(Form f, Point p)
        {
            this.Show(f, new Rectangle(p, new Size(0, 0)));

        }

        private void Show(Control control, Rectangle area)
        {
            if (control == null)
                throw new ArgumentNullException("control");


            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));

            Rectangle screen = Screen.FromControl(control).WorkingArea;

            if (location.X + Size.Width > (screen.Left + screen.Width))
                location.X = (screen.Left + screen.Width) - Size.Width;

            if (location.Y + Size.Height > (screen.Top + screen.Height))
                location.Y -= Size.Height + area.Height;

            location = control.PointToClient(location);

            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }


    }
}
