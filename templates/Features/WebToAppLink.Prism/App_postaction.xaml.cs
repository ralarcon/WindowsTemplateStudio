﻿using Windows.UI.Xaml;
//^^
//{[{
using Windows.UI.Xaml.Controls;
using Prism.Windows.Navigation;
using Param_RootNamespace.Views;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
//^^
//{[{
        // See detailed documentation about Web to App link
        // https://docs.microsoft.com/en-us/windows/uwp/launch-resume/web-to-app-linking
        // TODO WTS: You must to update the Host Uri also on Package.appxmanifest
        private const string Host = "myapp.website.com";
        private const string Section1 = "/MySection1";
        private const string Section2 = "/MySection2";

//}]}
        public App()
        {
            InitializeComponent();
        }

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
//^^
//{[{
            if (args.Kind == ActivationKind.Protocol && ((ProtocolActivatedEventArgs)args)?.Uri?.Host == Host && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                switch (((ProtocolActivatedEventArgs)args).Uri.AbsolutePath)
                {
                    // Open the page in app that is equivalent to the section on the website.
                    case Section1:
                        // Use NavigationService to Navigate to MySection1Page
                        break;
                    case Section2:
                        // Use NavigationService to Navigate to MySection2Page
                        break;
                    default:
                        // Launch the application with default page.
                        // Use NavigationService to Navigate to MainPage
                        break;
                }
            }
//}]}

            return Task.CompletedTask;
        }
    }
}
