using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            InputText.Text = "This is some test text";
        }

        private void InitializeProcessList()
        {
            ProcessList.DataSource = new List<string> { "notepad", "chrome", "msedge" };
        }

        private async void SendBtn_Click(object sender, EventArgs e)
        {
            // Try to get the window
            try
            {
                ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_apiKey))
                {
                    Endpoint = _apiEndpoint
                };

                //await ReadFileUrl(client, "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png");
                var result = await ReadFileLocal(client, "TypingTest2.png");


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
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        private static async Task<string> ReadFileLocal(ComputerVisionClient client, string localFile)
        {
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile));

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
            return result.ToString().Remove(result.Length - 1);
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
