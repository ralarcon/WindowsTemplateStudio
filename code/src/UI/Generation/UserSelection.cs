﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI
{
    public enum ItemGenerationType
    {
        None,
        Generate,
        GenerateAndMerge
    }

    public class UserSelection
    {
        public string ProjectType { get; set; }
        public string Framework { get; set; }
        public string HomeName { get; set; }
        public string Language { get; set; }
        public ItemGenerationType ItemGenerationType { get; set; } = ItemGenerationType.None;
        public List<(string name, ITemplateInfo template)> Pages { get; } = new List<(string name, ITemplateInfo template)>();
        public List<(string name, ITemplateInfo template)> Features { get; } = new List<(string name, ITemplateInfo template)>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Language))
            {
                sb.AppendFormat("Language: '{0}'", Language);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(ProjectType))
            {
                sb.AppendFormat("ProjectType: '{0}'", ProjectType);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(Framework))
            {
                sb.AppendFormat("Framework: '{0}'", Framework);
                sb.AppendLine();
            }

            if (Pages.Any())
            {
                sb.AppendFormat("Pages: '{0}'", string.Join(", ", Pages.Select(p => $"{p.name} - {p.template.Name}").ToArray()));
                sb.AppendLine();
            }

            if (Features.Any())
            {
                sb.AppendFormat("Features: '{0}'", string.Join(", ", Features.Select(p => $"{p.name} - {p.template.Name}").ToArray()));
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
