﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core.Gen;

using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergePostAction : PostAction<MergeConfiguration>
    {
        public MergePostAction(MergeConfiguration config)
            : base(config)
        {
        }

        public override void Execute()
        {
            string originalFilePath = GetFilePath();
            if (!File.Exists(originalFilePath))
            {
                if (_config.FailOnError )
                {
                    throw new FileNotFoundException(string.Format(StringRes.MergeFileNotFoundExceptionMessage, _config.FilePath));
                }
                else
                {
                    AddFailedMergePostActionsFileNotFound(originalFilePath);
                    File.Delete(_config.FilePath);
                    return;
                }
            }

            var source = File.ReadAllLines(originalFilePath).ToList();
            var merge = File.ReadAllLines(_config.FilePath).ToList();

            IEnumerable<string> result = source.HandleRemovals(merge);
            result = result.Merge(merge.RemoveRemovals(), out string errorLine);

            if (errorLine != string.Empty)
            {
                if (_config.FailOnError)
                {
                    throw new InvalidDataException(string.Format(StringRes.MergeLineNotFoundExceptionMessage, errorLine, originalFilePath));
                }
                else
                {
                    AddFailedMergePostActionsAddLineNotFound(originalFilePath, errorLine);
                }
            }
            else
            {
                Fs.EnsureFileEditable(originalFilePath);
                File.WriteAllLines(originalFilePath, result, Encoding.UTF8);

                // REFRESH PROJECT TO UN-DIRTY IT
                if ((Path.GetExtension(_config.FilePath).Equals(".csproj", StringComparison.OrdinalIgnoreCase)
                   || Path.GetExtension(_config.FilePath).Equals(".vbproj", StringComparison.OrdinalIgnoreCase))
                  && (GenContext.Current.OutputPath == GenContext.Current.ProjectPath))
                {
                    Gen.GenContext.ToolBox.Shell.RefreshProject();
                }
            }

            File.Delete(_config.FilePath);
        }

        protected void AddFailedMergePostActions(string originalFilePath, MergeFailureType mergeFailureType, string description)
        {
            var sourceFileName = GetRelativePath(originalFilePath);
            var postactionFileName = GetRelativePath(_config.FilePath);

            var failedFileName = GetFailedPostActionFileName();
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostAction(sourceFileName, _config.FilePath, failedFileName, description, mergeFailureType));
            File.Copy(_config.FilePath, failedFileName, true);
        }

        protected string GetRelativePath(string path)
        {
            return path.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);
        }

        private void AddFailedMergePostActionsFileNotFound(string originalFilePath)
        {
            var description = string.Format(StringRes.FailedMergePostActionFileNotFound, GetRelativePath(originalFilePath));
            AddFailedMergePostActions(originalFilePath,  MergeFailureType.FileNotFound, description);
        }

        private void AddFailedMergePostActionsAddLineNotFound(string originalFilePath, string errorLine)
        {
            var description = string.Format(StringRes.FailedMergePostActionLineNotFound, errorLine.Trim(), GetRelativePath(originalFilePath));
            AddFailedMergePostActions(originalFilePath, MergeFailureType.LineNotFound, description);
        }

        private string GetFailedPostActionFileName()
        {
            var newFileName = Path.GetFileNameWithoutExtension(_config.FilePath).Replace(MergeConfiguration.Suffix, MergeConfiguration.NewSuffix);
            var folder = Path.GetDirectoryName(_config.FilePath);
            var extension = Path.GetExtension(_config.FilePath);

            var validator = new List<Validator>
            {
                new FileExistsValidator(Path.GetDirectoryName(_config.FilePath))
            };

            newFileName = Naming.Infer(newFileName, validator);
            return Path.Combine(folder, newFileName + extension);
        }

        private string GetFilePath()
        {
            if (Path.GetFileName(_config.FilePath).StartsWith(MergeConfiguration.Extension, StringComparison.InvariantCultureIgnoreCase))
            {
                var extension = Path.GetExtension(_config.FilePath);
                var directory = Path.GetDirectoryName(_config.FilePath);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(MergeConfiguration.Suffix));
            }
            else
            {
                var path = Regex.Replace(_config.FilePath, MergeConfiguration.PostactionRegex, ".");

                return path;
            }
        }
    }
}
