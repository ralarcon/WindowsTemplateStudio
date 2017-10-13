﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommandLine;
using CommandLine.Text;

namespace Localization.Options
{
    public class CommandLineOptions
    {
        [VerbOption("ext", HelpText = "Extract localizable items for different cultures.")]
        public ExtractOptions ExtractOptions { get; set; }

        [VerbOption("gen", HelpText = "Generates Project Templates for different cultures.")]
        public GenerationOptions GenerationOptions { get; set; }

        [VerbOption("verify", HelpText = "Verify if exist localizable items for different cultures")]
        public VerifyOptions VerifyOptions { get; set; }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}
