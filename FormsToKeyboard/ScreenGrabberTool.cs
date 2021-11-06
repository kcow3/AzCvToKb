using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FormsToKeyboard
{
    public class ScreenGrabberTool
    {
        private Rectangle canvasBounds = Screen.GetBounds(Point.Empty);

        public void ShowGrabber()
        {
            using var screenGrabber = new ScreenGrabber();

            if (screenGrabber.ShowDialog() == DialogResult.OK)
            {
                canvasBounds = screenGrabber.GetRectangle();
            }
        }

        public Bitmap GetSnapShot()
        {
            using Image image = new Bitmap(canvasBounds.Width, canvasBounds.Height);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.CopyFromScreen(new Point(canvasBounds.Left, canvasBounds.Top), Point.Empty, canvasBounds.Size);
            }
            return new Bitmap(SetBorder(image, Color.Black, 1));
        }

        /// <summary>
        /// Saves file as .jpg
        /// You are required to provide a name like xxx.jpg, where xxx is the name of the file to save
        /// </summary>
        /// <param name="fileLocation">File location in the format xxx.jpg, where xxx is the name of the file to save</param>
        /// <param name="image">Bitmap image</param>
        public void SaveSnapShot(string fileLocation, Bitmap image)
        {
            image.Save($"{fileLocation}", ImageFormat.Jpeg);
        }

        private Image SetBorder(Image srcImg, Color color, int width)
        {
            // Create a copy of the image and graphics context
            Image dstImg = srcImg.Clone() as Image;
            Graphics g = Graphics.FromImage(dstImg);

            // Create the pen
            Pen pBorder = new Pen(color, width)
            {
                Alignment = PenAlignment.Center
            };

            // Draw
            g.DrawRectangle(pBorder, 0, 0, dstImg.Width - 1, dstImg.Height - 1);

            // Clean up
            pBorder.Dispose();
            g.Save();
            g.Dispose();

            // Return
            return dstImg;
        }
    }
}
