﻿//{**
//This code block adds the wts.ItemNameViewModel to the _secondaryItems of the ShellViewModel.
//**}
//{[{
using Param_ItemNamespace.Helpers;
//}]}
namespace Param_ItemNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private void PopulateNavItems()
        {
            //^^
            //{[{
            _secondaryItems.Add(new ShellNavigationItem("Shell_wts.ItemName".GetLocalized(), Symbol.Setting, typeof(wts.ItemNameViewModel).FullName));
            //}]}
        }
    }
}
