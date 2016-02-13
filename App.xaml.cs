using System;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WolframCortana
{
    sealed partial class App : Application
    {
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            Window.Current.Activate();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var voiceArgs = args as VoiceCommandActivatedEventArgs;

                var props = voiceArgs.Result.SemanticInterpretation.Properties;
                var query = props["query"][0];
                var escapedQuery = Uri.EscapeDataString(query);

                await Launcher.LaunchUriAsync(
                    new Uri(string.Format("wolframalpha://query?input={0}", escapedQuery)),
                    new LauncherOptions
                    {
                        FallbackUri = new Uri(string.Format("http://www.wolframalpha.com/input/?i={0}", escapedQuery)),
                    }
                );
            }
        }
    }
}
