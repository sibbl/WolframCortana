using System;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace WolframCortana
{
    public sealed partial class MainPage : Page
    {
        private const string VoiceCommandPath = "VoiceCommands.xml";

        public MainPage()
        {
            this.InitializeComponent();

            InstallVoiceCommandDefinitions();
        }

        private async void InstallVoiceCommandDefinitions()
        {
            var firstTime = VoiceCommandDefinitionManager.InstalledCommandDefinitions.Count == 0;
            MessageText.Text = firstTime ? "Integrating into Cortana..." : "Updating Cortana integration...";
            try
            {
                var storageFile =
                    await StorageFile.GetFileFromApplicationUriAsync(
                        new Uri("ms-appx:///" + VoiceCommandPath));
                await VoiceCommandDefinitionManager
                        .InstallCommandDefinitionsFromStorageFileAsync(storageFile);
                MessageText.Text = firstTime ? "Cortana integration is ready." : "Cortana integration is up to date.";
            }
            catch (Exception e)
            {
                MessageText.Text = "Cortana, we have a problem.";
            }
        }
    }
}
