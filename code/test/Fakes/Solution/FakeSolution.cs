﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Fakes
{
    public class FakeSolution
    {
        private const string GlobalSectionText = "GlobalSection(ProjectConfigurationPlatforms) = postSolution";

        private const string ConfigurationTemplate = @"		{0}.Debug|ARM.ActiveCfg = Debug|ARM
		{0}.Debug|ARM.Build.0 = Debug|ARM
		{0}.Debug|ARM.Deploy.0 = Debug|ARM
		{0}.Debug|x64.ActiveCfg = Debug|x64
		{0}.Debug|x64.Build.0 = Debug|x64
		{0}.Debug|x64.Deploy.0 = Debug|x64
		{0}.Debug|x86.ActiveCfg = Debug|x86
		{0}.Debug|x86.Build.0 = Debug|x86
		{0}.Debug|x86.Deploy.0 = Debug|x86
		{0}.Release|ARM.ActiveCfg = Release|ARM
		{0}.Release|ARM.Build.0 = Release|ARM
		{0}.Release|ARM.Deploy.0 = Release|ARM
		{0}.Release|x64.ActiveCfg = Release|x64
		{0}.Release|x64.Build.0 = Release|x64
		{0}.Release|x64.Deploy.0 = Release|x64
		{0}.Release|x86.ActiveCfg = Release|x86
		{0}.Release|x86.Build.0 = Release|x86
		{0}.Release|x86.Deploy.0 = Release|x86
";

        private const string ProjectTemplateCS = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{name}"", ""{name}\{name}.csproj"", ""{id}""
EndProject
";
        private const string ProjectTemplateVB = @"Project(""{7CF740F7-735F-48EA-8B7B-3FFA4902371C}"") = ""{name}"", ""{name}\{name}.vbproj"", ""{id}""
EndProject
";
        private readonly string _path;

        private FakeSolution(string path)
        {
            _path = path;
        }

        public static FakeSolution Create(string path)
        {
            var solutionTemplate = ReadTemplate();
            File.WriteAllText(path, solutionTemplate, Encoding.UTF8);

            return new FakeSolution(path);
        }

        public void AddProjectToSolution(string projectName, string projectGuid, bool isCSharp)
        {
            var slnContent = File.ReadAllText(_path);

            if (slnContent.IndexOf(projectName) == -1)
            {
                var globalIndex = slnContent.IndexOf("Global");
                var projectTemplate = isCSharp ? ProjectTemplateCS : ProjectTemplateVB;
                var projectContent = projectTemplate
                                            .Replace("{name}", projectName)
                                            .Replace("{id}", projectGuid);

                slnContent = slnContent.Insert(globalIndex, projectContent);

                var globalSectionIndex = slnContent.IndexOf(GlobalSectionText);
                var projectConfigContent = string.Format(ConfigurationTemplate, projectGuid);

                slnContent = slnContent.Insert(globalSectionIndex + GlobalSectionText.Length + 1, projectConfigContent);
            }

            File.WriteAllText(_path, slnContent, Encoding.UTF8);
        }

        private static string ReadTemplate()
        {
            return File.ReadAllText(@"Solution\SolutionTemplate.txt");
        }
    }
}
