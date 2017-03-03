﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class IEnumerableExtensions
    {
        private const string MacroBeforeMode = "^^";
        private const string MacroStartGroup = "{{";
        private const string MarcoEndGroup = "}}";

        public static int SafeIndexOf(this IEnumerable<string> source, string item, int skip)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                return -1;
            }
            if (skip == -1)
            {
                skip = 0;
            }

            var actual = source.Skip(skip).ToList();

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    return skip + i;
                }
            }

            return -1;
        }

        public static IEnumerable<string> Merge(this IEnumerable<string> source, IEnumerable<string> merge)
        {
            int lastLineIndex = -1;
            var insertionBuffer = new List<string>();

            bool beforeMode = false;
            bool isInBlock = false;

            var _diffTrivia = FindDiffLeadingTrivia(source, merge);
            var result = source.ToList();

            foreach (var mergeLine in merge)
            {
                var currentLineIndex = -1;

                if (!isInBlock)
                {
                    currentLineIndex = result.SafeIndexOf(mergeLine.WithLeadingTrivia(_diffTrivia), lastLineIndex);
                }

                if (currentLineIndex > -1)
                {
                    if (insertionBuffer.Any())
                    {
                        var insertIndex = GetInsertLineIndex(currentLineIndex, lastLineIndex, beforeMode);
                        result.InsertRange(insertIndex, insertionBuffer);

                        if (beforeMode)
                        {
                            beforeMode = false;
                        }
                    }

                    lastLineIndex = currentLineIndex + insertionBuffer.Count;
                    insertionBuffer.Clear();
                }
                else
                {
                    if (mergeLine.Contains(MacroBeforeMode))
                    {
                        beforeMode = true;
                    }
                    else if (mergeLine.Contains(MacroStartGroup))
                    {
                        isInBlock = true;
                    }
                    else if (mergeLine.Contains(MarcoEndGroup))
                    {
                        isInBlock = false;
                    }
                    else
                    {
                        insertionBuffer.Add(mergeLine.WithLeadingTrivia(_diffTrivia));
                    }
                }
            }

            return result;
        }

        private static int GetInsertLineIndex(int currentLine, int lastLine, bool isBeforeMode)
        {
            if (isBeforeMode)
            {
                return currentLine;
            }
            else
            {
                return lastLine + 1;
            }
        }

        private static int FindDiffLeadingTrivia(IEnumerable<string> target, IEnumerable<string> merge)
        {
            if (!target.Any() || !merge.Any())
            {
                return 0;
            }

            var firstMerge = merge.First();
            var firstTarget = target.FirstOrDefault(t => t.Trim().Equals(firstMerge.Trim(), StringComparison.OrdinalIgnoreCase));

            if (firstTarget == null)
            {
                return 0;
            }

            return firstTarget.GetLeadingTrivia() - firstMerge.GetLeadingTrivia();
        }
    }
}