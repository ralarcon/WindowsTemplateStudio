﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Runtime.InteropServices;
using CommandLine;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("TemplateValidator")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("TemplateValidator")]
[assembly: AssemblyCopyright("Copyright © .NET Foundation and Contributors 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("365acd5e-0ffe-45a5-80b3-c0873b6bdb08")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: AssemblyLicense("Licensed to the.NET Foundation under one or more agreements.", "The .NET Foundation licenses this file to you under the MIT license.", "See the LICENSE file in the project root for more information.")]

[assembly: AssemblyUsage(
    "Usage: TemplateValidator -f \"../../Templates/Pages/Blank/.template.config/template.json\" ",
    "       TemplateValidator -d \"C:\\GitHub\\WindowsTemplateStudio\\templates\" ",
    "       TemplateValidator -d \"C:\\GitHub\\WTS\\templates\" \"C:\\MyWtsTemplates\" ")]
