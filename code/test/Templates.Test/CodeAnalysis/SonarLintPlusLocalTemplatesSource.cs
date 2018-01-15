// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test
{
    public class SonarLintPlusLocalTemplatesSource : LocalTemplatesSource
    {
        public SonarLintPlusLocalTemplatesSource()
            : base("BuildSonarLint")
        {
        }

        public override void Extract(string source, string targetFolder)
        {
            base.Extract(source, targetFolder);

            SetSonarLintFeatureContent();
        }

        private void SetSonarLintFeatureContent()
        {
            string targetSonarLintFeaturePath = Path.Combine(FinalDestination, "Features", "SonarLintVB");
            if (Directory.Exists(targetSonarLintFeaturePath))
            {
                Fs.SafeDeleteDirectory(targetSonarLintFeaturePath);
            }

            Fs.CopyRecursive(@".\TestData\SonarLintVB", targetSonarLintFeaturePath, true);
        }
    }
}
