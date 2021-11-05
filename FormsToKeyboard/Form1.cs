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
        private static extern int SetForegroundWindow(IntPtr point);
        private static ComputerVisionClient _client;

        public Form1()
        {
            InitializeComponent();
            InitializeTestText();
            InitializeProcessList();
            InitializeAzureCv();
        }

        /// <summary>
        /// Read Azure CV api credentials from user secrets.
        /// Make sure your user secrets contains the following: "cvApiKey" and "cvApiEndpoint"
        /// </summary>
        private void InitializeAzureCv()
        {
            try
            {
                var config = new ConfigurationBuilder().AddUserSecrets<Form1>().Build();
                if (config == null)
                    return;

                var secretProvider = config.Providers.First();

                if (!secretProvider.TryGet("cvApiKey", out var apiKey))
                {
                    throw new Exception("Could not retrieve Azure CV API key");
                }
                if (!secretProvider.TryGet("cvApiEndpoint", out var apiEndpoint))
                {
                    throw new Exception("Could not retrieve Azure CV API endpoint");
                }
                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiEndpoint))
                {
                    throw new Exception("Incorrect Azure CV API credentials");
                }

                _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
                {
                    Endpoint = apiEndpoint
                };
            }
            catch (Exception)
            {
                MessageBox.Show($"Could not read user secrets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Initialize preview text
        /// </summary>
        private void InitializeTestText()
        {
            InputText.Text = "";
        }

        /// <summary>
        /// Provide the user with a standard list of processes to attache to.
        /// </summary>
        private void InitializeProcessList()
        {
            ProcessList.DataSource = new List<string> { "msedge", "chrome", "notepad" };
        }

        /// <summary>
        /// Perform OCR on image stream using Azure CV
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static async Task<string> GetTextFromImage(Stream stream)
        {
            var textHeaders = await _client.ReadInStreamAsync(stream);

            var operationId = textHeaders.OperationLocation.Split("/analyzeResults/")[1];
            Thread.Sleep(2000);

            ReadOperationResult results;
            do
            {
                results = await _client.GetReadResultAsync(Guid.Parse(operationId));
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
        private void SendStringToKeyboardBuffer(string s, int delay = 0)
        {
            var charactersToSend = s.Select(x => new string(x, 1)).ToArray();
            foreach (var character in charactersToSend)
            {
                SendKeys.SendWait(character.FormatStringForKeyboardBuffer());
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

            if (!Clipboard.GetImage().IsDimentionsOk())
            {
                MessageBox.Show($"Image must have be between 50x50 and 10000x10000 pixels", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var text = await GetTextFromImage(Clipboard.GetImage().ToStream());

            // Preview the text
            InputText.Text = text;

            // Send text to given process
            SendTextToProcess(ProcessList.SelectedValue as string, text, 50);
        }

        /// <summary>
        /// Attach to the given process and set the window to the foreground
        /// </summary>
        /// <param name="processName">Name of the process as found under processes in task manager</param>
        /// <param name="text">Text to send to the process</param>
        /// <param name="delay">Delay in ms between characters to send</param>
        private void SendTextToProcess(string processName, string text, int delay = 0)
        {
            try
            {
                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process == null)
                {
                    MessageBox.Show($"Could not attach to the process with name [{processName}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Open the given process in the foreground
                var window = process.MainWindowHandle;
                SetForegroundWindow(window);

                SendStringToKeyboardBuffer(text, delay);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error in sending text to process: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
