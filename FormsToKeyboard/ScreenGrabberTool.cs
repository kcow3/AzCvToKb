using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FormsToKeyboard
{
    public class ScreenGrabberTool
    {
        private Rectangle screenShotArea = Screen.GetBounds(Point.Empty);

        public void ShowGrabber()
        {
            using var screenGrabber = new ScreenGrabberWindow();

            if (screenGrabber.ShowDialog() == DialogResult.OK)
            {
                var rectangle = screenGrabber.GetRectangle();

                if (rectangle.Width == 0 || rectangle.Height == 0) return;

                screenShotArea = screenGrabber.GetRectangle();
            }
        }

        public Bitmap GetSnapShot()
        {
            using Image image = new Bitmap(screenShotArea.Width, screenShotArea.Height);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.CopyFromScreen(new Point(screenShotArea.Left, screenShotArea.Top), Point.Empty, screenShotArea.Size);
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
