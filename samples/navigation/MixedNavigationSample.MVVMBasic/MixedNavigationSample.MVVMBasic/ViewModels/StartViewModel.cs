﻿using System.Windows.Input;
using MixedNavigationSample.MVVMBasic.Helpers;
using MixedNavigationSample.MVVMBasic.Services;

namespace MixedNavigationSample.MVVMBasic.ViewModels
{
    public class StartViewModel : Observable
    {
        public ICommand StartCommand { get; set; }

        public StartViewModel()
        {
            StartCommand = new RelayCommand(OnStart);
        }

        private void OnStart()
        {
            //Navigating to a ShellPage, this will replaces NavigationService frame for an inner frame to change navigation handling.
            NavigationService.Navigate<Views.ShellPage>();

            //Navigating now to a HomePage, this will be the first navigation on a NavigationPane menu
            NavigationService.Navigate<Views.HomePage>();
        }
    }
}
