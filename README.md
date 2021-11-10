# Azure computer vision

A playground project written in Windows Forms (.NET Core 3.1) that uses Azure Computer Vision to perform Optical Character Recognition (OCR) on images in the Windows Clipboard.
The recognised text can then be copied, or sent to a predefined list of Windows processes via the Keyboard Buffer (SendKeys) method.

![OCR drawio](https://user-images.githubusercontent.com/27766536/141197092-1a1dc499-af5b-4a94-89a2-e01895390308.png)

## Azure setup

This project requires you to create a Computer Vision project on Microsoft Azure.
For more information on Azure Computer Vision, refer to [this](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/) link.

After you created a Computer Vision Project on Azure, navigate to the resource, and find the "Keys and Endpoint" option. You need two values from this section:
1. Key
2. Endpoint

Note down these values as you will require them when configuring Local User Secrets on your dev environment. See the image below for reference.

![image](https://user-images.githubusercontent.com/27766536/141188807-b7a05f83-b876-41e0-bd95-bb983407f782.png)

## Local setup

After pulling the code, you have to configure local user secrets. 

In Visual Studio, right-click on your project and click on "Manage User Secrets". This will open your secrets.json file. Paste the following code in your secrets file and enter your Azure Computer Vision key and endpoint mentioned before.

```json
{
  "cvApiEndpoint": "Your endpoint here",
  "cvApiKey": "Your key here"
}
```

For more information on user secrets, refer to [this documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)

## Config

The idea for this project was just to play with OCR capabilities of Azure. With this in mind, you can perform some simple operations using the Windows Forms application like:
* Take screenshots - [Credit](https://www.codeproject.com/Tips/988951/Custom-Snipping-Tool-using-Csharp-WinForms)
* Process text in screenshot
* Output text to process

To modify the precess list, modify the following code in `MainWindow.cs`:

```cs
ProcessList.DataSource = new List<string> { "N/A", "msedge", "chrome", "notepad" };
```

## Demo
![Demo](https://user-images.githubusercontent.com/27766536/141195460-afdb3287-6ba1-46f3-9cb5-38cba693a01a.gif)





