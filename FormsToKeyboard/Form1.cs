using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FormsToKeyboard
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        public Form1()
        {
            InitializeComponent();
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            // Try to get the window
            try
            {
                var processName = "notepad";
                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process == null)
                {
                    MessageBox.Show($"Could not attach to the process with name [{processName}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var window = process.MainWindowHandle;
                SetForegroundWindow(window);

                if (string.IsNullOrEmpty(InputText.Text))
                {
                    MessageBox.Show($"Please enter some text to send", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SendStringToKeyboardBuffer(InputText.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("We ran into an error when trying to send text to the selected process...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Send the input string to kb buffer and apply delay between characters
        /// </summary>
        /// <param name="s">String to send</param>
        /// <param name="delay">Delay in ms</param>
        private void SendStringToKeyboardBuffer(string s, int delay = 0)
        {
            var charactersToSend = s.Select(x => new string(x, 1)).ToArray();
            foreach (var character in charactersToSend)
            {
                SendKeys.SendWait(character);
                Thread.Sleep(delay);
            }
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            InputText.Text = "";
        }
    }
}
