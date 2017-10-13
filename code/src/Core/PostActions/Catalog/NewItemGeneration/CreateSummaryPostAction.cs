﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CreateSummaryPostAction : PostAction<TempGenerationResult>
    {
        public CreateSummaryPostAction(TempGenerationResult config)
            : base(config)
        {
        }

        public override void Execute()
        {
            var fileName = GetFileName();
            if (_config.SyncGeneration)
            {
                var newFiles = BuildNewFilesSection(StringRes.SyncSummarySectionNewFiles);

                var modifiedFiles = BuildMergeFileSection(StringRes.SyncSummarySectionModifiedFiles, StringRes.SyncSummaryTemplateModifiedFile, GenContext.Current.MergeFilesFromProject.Where(f => !GenContext.Current.FailedMergePostActions.Any(m => m.FileName == f.Key)));
                var failedMergeFiles = BuildMergeFileSection(StringRes.SyncSummarySectionFailedMergeFiles, StringRes.SyncSummaryTemplateFailedMerges, GenContext.Current.MergeFilesFromProject.Where(f => GenContext.Current.FailedMergePostActions.Any(m => m.FileName == f.Key)));
                var conflictingFiles = BuildConflictingFilesSection(StringRes.SyncSummarySectionConflictingFiles);

                File.WriteAllText(fileName, string.Format(StringRes.SyncSummaryTemplate, GenContext.Current.OutputPath, newFiles, modifiedFiles, failedMergeFiles, conflictingFiles));
            }
            else
            {
                var newFiles = BuildNewFilesSection(StringRes.SyncInstructionsSectionNewFiles);
                var modifiedFiles = BuildMergeFileSection(StringRes.SyncInstructionsSectionModifiedFiles, StringRes.SyncInstructionsTemplateModifiedFile, GenContext.Current.MergeFilesFromProject);
                var conflictingFiles = BuildConflictingFilesSection(StringRes.SyncInstructionsSectionConflictingFiles);
                var unchangedFiles = BuildUnchangedFilesSection(StringRes.SyncInstructionsSectionUnchangedFiles);

                File.WriteAllText(fileName, string.Format(StringRes.SyncInstructionsTemplate, GenContext.Current.OutputPath, newFiles, modifiedFiles, conflictingFiles, unchangedFiles));
            }

            GenContext.Current.FilesToOpen.Add(fileName);

            GenContext.Current.FailedMergePostActions.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
        }

        private string GetFileName()
        {
            if (_config.SyncGeneration)
            {
                return Path.Combine(GenContext.Current.OutputPath, StringRes.SyncSummaryFileName);
            }
            else
            {
                return Path.Combine(GenContext.Current.OutputPath, StringRes.SyncInstructionsFileName);
            }
        }

        private string BuildMergeFileSection(string sectionTemplate, string modifiedFileTemplate, IEnumerable<KeyValuePair<string, List<MergeInfo>>> mergeFiles)
        {
            if (mergeFiles.Any())
            {
                var sb = new StringBuilder();

                foreach (var mergeFile in mergeFiles)
                {
                    sb.AppendLine(GetMergeFileDescription(mergeFile, modifiedFileTemplate));
                }

                return string.Format(sectionTemplate, sb.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetMergeFileDescription(KeyValuePair<string, List<Merge.MergeInfo>> mergeFile, string modifiedFileTemplate)
        {
            var sb = new StringBuilder();

            foreach (var mergeInfo in mergeFile.Value)
            {
                sb.AppendLine($"```{mergeInfo.Format}");
                sb.AppendLine(mergeInfo.PostActionCode);
                sb.AppendLine("```");
            }

            return string.Format(modifiedFileTemplate, mergeFile.Key, sb.ToString(), GetMergeResult(mergeFile));
        }

        private string GetMergeResult(KeyValuePair<string, List<Merge.MergeInfo>> mergeFile)
        {
            if (!GenContext.Current.FailedMergePostActions.Any(w => w.FileName == mergeFile.Key))
            {
                return GetLinkToFile(mergeFile.Key);
            }
            else
            {
                var sb = new StringBuilder();
                var failedMergePostActions = GenContext.Current.FailedMergePostActions.Where(w => w.FileName == mergeFile.Key);

                foreach (var failedMergePostAction in failedMergePostActions)
                {
                    sb.AppendLine();
                    sb.AppendLine($"* {failedMergePostAction.Description}");
                }

                return sb.ToString();
            }
        }

        private string BuildNewFilesSection(string sectionTemplate)
        {
            if (_config.NewFiles.Any())
            {
                return string.Format(sectionTemplate, GetFileList(_config.NewFiles));
            }
            else
            {
                return string.Empty;
            }
        }

        private string BuildUnchangedFilesSection(string sectionTemplate)
        {
            if (_config.UnchangedFiles.Any())
            {
                return string.Format(sectionTemplate, GetFileList(_config.UnchangedFiles));
            }
            else
            {
                return string.Empty;
            }
        }

        private string BuildConflictingFilesSection(string sectionTemplate)
        {
            if (_config.ConflictingFiles.Any())
            {
                var sb = new StringBuilder();
                foreach (var conflictFile in _config.ConflictingFiles)
                {
                    sb.AppendLine(GetCompareLink(conflictFile));
                }

                return string.Format(sectionTemplate, sb.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private StringBuilder GetFileList(List<string> files)
        {
            var fileList = new StringBuilder();
            foreach (var file in files)
            {
                fileList.AppendLine($"* {GetLinkToFile(file)}");
            }

            return fileList;
        }

        private string GetLinkToFile(string fileName)
        {
            if (_config.SyncGeneration)
            {
                var filePath = Path.Combine(GenContext.Current.ProjectPath, fileName);
                return $"[{fileName}]({FormatFilePath(filePath)})";
            }
            else
            {
                return $"[{fileName}]({fileName})";
            }
        }

        private static string FormatFilePath(string filePath)
        {
            return $"about:/{filePath.Replace(" ", "%20").Replace(@"\", "/")}";
        }

        private string GetCompareLink(string fileName)
        {
            var filePath = Path.Combine(GenContext.Current.ProjectPath, fileName);
            return $"* {StringRes.SyncSummaryTempGenerationFile}: [{fileName}]({fileName}), {StringRes.SyncSummaryProjectFile}: [{fileName}]({FormatFilePath(filePath)})";
        }
    }
}
