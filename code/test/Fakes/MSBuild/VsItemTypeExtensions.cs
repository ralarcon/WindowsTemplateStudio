﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.Templates.Fakes
{
    public static class VsItemTypeExtensions
    {
        public static XElement GetXmlDefinition(this VsItemType itemType, string includePath)
        {
            switch (itemType)
            {
                case VsItemType.Compiled:
                    return GetCompileXElement(includePath, false);
                case VsItemType.CompiledWithDependant:
                    return GetCompileXElement(includePath, true);
                case VsItemType.XamlPage:
                    return GetXamlPageXElement(includePath);
                case VsItemType.Resource:
                    return GetResourceXElement(includePath);
                case VsItemType.Content:
                    return GetContentXElement(includePath);
                case VsItemType.None:
                    return GetNoneXElement(includePath);
                default:
                    return null;
            }
        }

        private static XElement GetCompileXElement(string includePath, bool dependentUpon)
        {
            var sb = new StringBuilder();
            sb.Append($"<Compile Include=\"{includePath}\"");

            if (dependentUpon)
            {
                string dependency = Path.GetFileNameWithoutExtension(includePath);
                sb.AppendLine(">");
                sb.AppendLine($"<DependentUpon>{dependency}</DependentUpon>");
                sb.AppendLine("</Compile>");
            }
            else
            {
                sb.AppendLine(" />");
            }

            var sr = new StringReader(sb.ToString());
            var itemElement = XElement.Load(sr);

            return itemElement;
        }

        private static XElement GetXamlPageXElement(string includePath)
        {
            var sb = new StringBuilder();
            sb.Append($"<Page Include=\"{includePath}\"");
            sb.AppendLine(">");
            sb.AppendLine($"<Generator>MSBuild:Compile</Generator>");
            sb.AppendLine($"<SubType>Designer</SubType>");
            sb.AppendLine("</Page>");

            var sr = new StringReader(sb.ToString());
            var itemElement = XElement.Load(sr);

            return itemElement;
        }

        private static XElement GetResourceXElement(string includePath)
        {
            var sb = new StringBuilder();
            sb.Append($"<PRIResource Include=\"{includePath}\" />");

            var itemElement = XElement.Parse(sb.ToString());

            return itemElement;
        }

        private static XElement GetContentXElement(string includePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<Content Include=\"{includePath}\" />");

            var sr = new StringReader(sb.ToString());
            var itemElement = XElement.Load(sr);

            return itemElement;
        }

        private static XElement GetNoneXElement(string includePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<None Include=\"{includePath}\" />");

            var sr = new StringReader(sb.ToString());
            var itemElement = XElement.Load(sr);

            return itemElement;
        }
    }
}
