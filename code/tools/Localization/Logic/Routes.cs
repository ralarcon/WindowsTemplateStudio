﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Localization
{
    public static class Routes
    {
        internal const string ProjectTemplatePathCS = "code\\src\\ProjectTemplates\\CSharp.UWP.2017.Solution";
        internal const string ProjectTemplatePathVB = "code\\src\\ProjectTemplates\\VBNet.UWP.VS2017.Solution";
        internal const string ProjectTemplateFileCS = "CSharp.UWP.VS2017.Solution.vstemplate";
        internal const string ProjectTemplateFileVB = "VBNet.UWP.VS2017.Solution.vstemplate";
        internal const string ProjectTemplateFileNamePatternCS = "CSharp.UWP.VS2017.Solution.{0}.vstemplate";
        internal const string ProjectTemplateFileNamePatternVB = "VBNet.UWP.VS2017.Solution.{0}.vstemplate";

        internal const string CommandTemplateRootDirPath = "code\\src\\Installer.2017\\Commands";
        internal const string RelayCommandFile = "RelayCommandPackage.en-US.vsct";
        internal const string RelayCommandFileNamePattern = "RelayCommandPackage.{0}.vsct";
        internal const string VspackageFileNamePattern = "VSPackage.{0}.resx";

        internal const string RightClickFileSearchPattern = "*postaction.md";

        internal const string TemplatesRootDirPath = "templates";
        internal const string TemplatesPagesPatternPath = "Pages";
        internal const string TemplatesFeaturesPatternPath = "Features";

        internal const string TemplateConfigDir = ".template.config";
        internal const string TemplateDescriptionFile = "description.md";
        internal const string TemplateJsonFile = "template.json";

        internal const string WtsTemplatesRootDirPath = "templates\\_catalog";
        internal const string WtsProjectTypes = "projectTypes";
        internal const string WtsFrameworks = "frameworks";

        internal const string VsixRootDirPath = "code\\src\\Installer.2017";
        internal const string VsixLangDirPattern = "{0}\\Extension.vsixlangpack";
        internal const string VsixManifestFile = "source.extension.vsixmanifest";
        internal const string VsixLangpackFile = "Extension.vsixlangpack";
        internal const string VsixLangpackContent = @"<?xml version=""1.0"" encoding=""utf-8"" ?>  
<VsixLanguagePack Version = ""1.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema-lp/2010"" >  
  <LocalizedName>{0}</LocalizedName>
  <LocalizedDescription>{1}</LocalizedDescription>  
  <License>..\Content\EULA.{2}.rtf</License>
  <MoreInfoUrl>https://github.com/Microsoft/WindowsTemplateStudio/</MoreInfoUrl>  
</VsixLanguagePack>";

        internal const string ResourcesFilePath = "StringRes.resx";
        internal const string ResourcesFilePathPattern = "StringRes.{0}.resx";

        internal static string[] ResoureceDirectories { get; } =
        {
            "code\\src\\Core\\Resources",
            "code\\src\\Installer.2017\\Resources",
            "code\\src\\UI\\Resources"
        };

        // Validate Routes
        internal const string VsixValidatePath = "code\\src\\Installer.2017\\source.extension.vsixmanifest";
        internal const string ProjectTemplateFileNameValidateCS = "code\\src\\ProjectTemplates\\CSharp.UWP.2017.Solution\\CSharp.UWP.VS2017.Solution.vstemplate";
        internal const string ProjectTemplateFileNameValidateVB = "code\\src\\ProjectTemplates\\VBNet.UWP.2017.Solution\\VBNet.UWP.VS2017.Solution.vstemplate";
        internal const string RelayCommandFileNameValidate = "code\\src\\Installer.2017\\Commands\\RelayCommandPackage.en-US.vsct";
        internal const string VspackageFileNameValidate = "code\\src\\Installer.2017\\Commands\\VSPackage.en-US.resx";
        internal const string WtsProjectTypesValidate = "templates\\_catalog\\projectTypes.json";
        internal const string WtsFrameworksValidate = "templates\\_catalog\\frameworks.json";
    }
}
