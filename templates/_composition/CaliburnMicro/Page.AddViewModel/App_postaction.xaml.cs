﻿//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_ItemNamespace
{
    public sealed partial class App
    {
        protected override void Configure()
        {
            //^^
            //{[{       
            _container.PerRequest<wts.ItemNameViewModel>();
            //}]}
        }
    }
}
