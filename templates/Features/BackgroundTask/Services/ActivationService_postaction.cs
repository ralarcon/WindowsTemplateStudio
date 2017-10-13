﻿//{**
// These code blocks include the BackgroundTaskService Instance in the method `GetActivationHandlers()`
// and background task registration to the method `InitializeAsync()` in the ActivationService of your project.
//**}

using System;
//{[{
using Param_RootNamespace.Helpers;
//}]}

namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
            //{[{
            Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasks();
            //}]}
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            //{[{
            yield return Singleton<BackgroundTaskService>.Instance;
            //}]}
//{--{
            yield break;//}--}
        }
    }
}
