﻿//{[{
using Param_ItemNamespace.Views;
//}]}
namespace Param_ItemNamespace.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
//{[{
        // By default, this handler expects URIs of the format 'wtsapp:sample?secret={value}'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            if (args.Uri.AbsolutePath.ToLowerInvariant().Equals("sample"))
            {
                var secret = "<<I-HAVE-NO-SECRETS>>";

                try
                {
                    if (args.Uri.Query != null)
                    {
                        // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                        var decoder = new Windows.Foundation.WwwFormUrlDecoder(args.Uri.Query);

                        secret = decoder.GetFirstValueByName("secret");
                    }
                }
                catch (Exception)
                {
                    // NullReferenceException if the URI doesn't contain a query
                    // ArgumentException if the query doesn't contain a param called 'secret'
                }

                // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                NavigationService.Navigate(typeof(Views.wts.ItemNameExamplePage), secret);
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(typeof(Views.PivotPage));
            }

            await Task.CompletedTask;
        }

//}]}
    }
}
