using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FormsToKeyboard
{
    public partial class ScreenGrabber : Form
    {
        private Point cursorStartLocation;
        private Point cursorCurrentLocation;
        private bool isCapturing = false;

        public ScreenGrabber()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            Opacity = 0.5;
            Cursor = Cursors.Cross;
            MouseDown += ScreenGrabber_MouseDown;
            MouseMove += ScreenGrabber_MouseMove;
            MouseUp += ScreenGrabber_MouseUp;
            Paint += ScreenGrabber_Paint;
            KeyDown += ScreenGrabber_KeyDown;
            DoubleBuffered = true;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle
            (
                Math.Min(cursorStartLocation.X, cursorCurrentLocation.X),
                Math.Min(cursorStartLocation.Y, cursorCurrentLocation.Y),
                Math.Abs(cursorStartLocation.X - cursorCurrentLocation.X),
                Math.Abs(cursorStartLocation.Y - cursorCurrentLocation.Y)
            );
        }

        private void ScreenGrabber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void ScreenGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            cursorCurrentLocation = e.Location;
            if (isCapturing) Invalidate();
        }

        private void ScreenGrabber_MouseUp(object sender, MouseEventArgs e)
        {

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ScreenGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            cursorCurrentLocation = cursorStartLocation = e.Location;
            isCapturing = true;
        }

        private void ScreenGrabber_Paint(object sender, PaintEventArgs e)
        {
            if (isCapturing) e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
        }



        //private void CaptureBtn_Click(object sender, System.EventArgs e)
        //{
        //    TakeScreenshot();
        //    DialogResult = DialogResult.OK;
        //    Close();
        //}

        //private void CancelBtn_Click(object sender, System.EventArgs e)
        //{
        //    DialogResult = DialogResult.Cancel;
        //    Close();
        //}

        private void TakeScreenshot()
        {
            //var b = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //var g = Graphics.FromImage(b);
            //g.CopyFromScreen(0, 0, 0, 0, b.Size);
            //b.Save($"D://Downloads//{Guid.NewGuid()}.jpg", ImageFormat.Jpeg);



            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save($"D://Downloads//{Guid.NewGuid()}.jpg", ImageFormat.Jpeg);
            }
        }
    }
}
