using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FormsToKeyboard
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        private static string _apiEndpoint = "";
        private static string _apiKey = "";


        public Form1()
        {
            InitializeComponent();
            InitializeTestText();
            InitializeProcessList();
            InitializeUserSecrets();
        }

        private void InitializeUserSecrets()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Form1>().Build();
            if (config == null)
                return;

            var secretProvider = config.Providers.First();
            if (secretProvider.TryGet("cvApiKey", out var apiKey))
            {
                _apiKey = apiKey;
            }
            if (secretProvider.TryGet("cvApiEndpoint", out var apiEndpoint))
            {
                _apiEndpoint = apiEndpoint;
            }
        }

        private void InitializeTestText()
        {
            InputText.Text = "";
        }

        private void InitializeProcessList()
        {
            ProcessList.DataSource = new List<string> { "msedge", "chrome", "notepad" };
        }


        private static async Task<string> ReadFileLocal(ComputerVisionClient client, Stream stream)
        {
            var textHeaders = await client.ReadInStreamAsync(stream);

            var operationId = textHeaders.OperationLocation.Split("/analyzeResults/")[1];
            Thread.Sleep(2000);

            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            var result = new StringBuilder();
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    result.Append(line.Text);
                    result.Append(" ");
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Send the input string to kb buffer and apply delay between characters
        /// </summary>
        /// <param name="s">String to send</param>
        /// <param name="delay">Delay in ms</param>
        private void SendStringToKeyboardBuffer(string s, int delay = 25)
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

        private async void ClipboardBtn_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsImage())
            {
                MessageBox.Show($"No image in clipboard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                var stream = new MemoryStream();
                var image = Clipboard.GetImage();
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;

                ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_apiKey))
                {
                    Endpoint = _apiEndpoint
                };

                var result = await ReadFileLocal(client, stream);

                var process = Process.GetProcessesByName(ProcessList.SelectedValue as string).FirstOrDefault();
                if (process == null)
                {
                    MessageBox.Show($"Could not attach to the process with name [{ProcessList.SelectedValue as string}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var window = process.MainWindowHandle;
                SetForegroundWindow(window);


                InputText.Text = result;

                SendStringToKeyboardBuffer(result);

            }
        }
    }
}
