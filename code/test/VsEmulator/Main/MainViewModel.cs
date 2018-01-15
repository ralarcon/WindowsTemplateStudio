﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.VsEmulator.LoadProject;
using Microsoft.Templates.VsEmulator.NewProject;
using Microsoft.Templates.VsEmulator.TemplatesContent;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.VsEmulator.Main
{
    public class MainViewModel : Observable, IContextProvider
    {
        private readonly MainView _host;

        private string _language;

        public MainViewModel(MainView host)
        {
            _host = host;
            _wizardVersion = "0.0.0.0";
            _templatesVersion = "0.0.0.0";
        }

        public string ProjectName { get; private set; }

        public string OutputPath { get; private set; }

        public string ProjectPath { get; private set; }

        private bool _forceLocalTemplatesRefresh = true;

        public bool ForceLocalTemplatesRefresh
        {
            get => _forceLocalTemplatesRefresh;
            set => SetProperty(ref _forceLocalTemplatesRefresh, value);
        }

        public List<string> ProjectItems { get; } = new List<string>();

        public List<FailedMergePostAction> FailedMergePostActions { get; } = new List<FailedMergePostAction>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; } = new Dictionary<ProjectMetricsEnum, double>();

        public RelayCommand NewCSharpProjectCommand => new RelayCommand(NewCSharpProject);

        public RelayCommand NewVisualBasicProjectCommand => new RelayCommand(NewVisualBasicProject);

        public RelayCommand LoadProjectCommand => new RelayCommand(LoadProject);

        public RelayCommand OpenInVsCommand => new RelayCommand(OpenInVs);

        public RelayCommand OpenInVsCodeCommand => new RelayCommand(OpenInVsCode);

        public RelayCommand OpenInExplorerCommand => new RelayCommand(OpenInExplorer);

        public RelayCommand OpenTempInExplorerCommand => new RelayCommand(OpenTempInExplorer);

        public RelayCommand ConfigureVersionsCommand => new RelayCommand(ConfigureVersions);

        public RelayCommand AddNewFeatureCommand => new RelayCommand(AddNewFeature);

        public RelayCommand AddNewPageCommand => new RelayCommand(AddNewPage);

        private string _state;

        public string State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        private string _log;

        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }

        private Visibility _isProjectLoaded;

        public Visibility IsProjectLoaded
        {
            get => _isProjectLoaded;
            set => SetProperty(ref _isProjectLoaded, value);
        }

        private Visibility _isWtsProject;

        public Visibility IsWtsProject
        {
            get => _isWtsProject;
            set => SetProperty(ref _isWtsProject, value);
        }

        public Visibility TempFolderAvailable
        {
            get
            {
                return HasContent(GetTempGenerationFolder()) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private string _wizardVersion;

        public string WizardVersion
        {
            get => _wizardVersion;
            set => SetProperty(ref _wizardVersion, value);
        }

        private string _templatesVersion;

        public string TemplatesVersion
        {
            get => _templatesVersion;
            set => SetProperty(ref _templatesVersion, value);
        }

        private string _solutionName;

        public string SolutionName
        {
            get => _solutionName;
            set
            {
                SetProperty(ref _solutionName, value);

                if (string.IsNullOrEmpty(value))
                {
                    IsProjectLoaded = Visibility.Hidden;
                }
                else
                {
                    IsProjectLoaded = Visibility.Visible;
                }
            }
        }

        public string SolutionPath { get; set; }

        public void Initialize()
        {
            SolutionName = null;
        }

        private void NewCSharpProject()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                await NewProjectAsync(ProgrammingLanguages.CSharp);
            });
        }

        private void NewVisualBasicProject()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                await NewProjectAsync(ProgrammingLanguages.VisualBasic);
            });
        }

        private async Task NewProjectAsync(string language)
        {
            _language = language;
            ConfigureGenContext(ForceLocalTemplatesRefresh);

            try
            {
                var newProjectInfo = ShowNewProjectDialog();

                if (!string.IsNullOrEmpty(newProjectInfo.name))
                {
                    var projectPath = Path.Combine(newProjectInfo.location, newProjectInfo.name, newProjectInfo.name);

                    GenContext.Current = this;

                    var userSelection = NewProjectGenController.Instance.GetUserSelection(_language);

                    if (userSelection != null)
                    {
                        ProjectName = newProjectInfo.name;
                        ProjectPath = projectPath;
                        OutputPath = projectPath;

                        ClearContext();
                        SolutionName = null;

                        await NewProjectGenController.Instance.GenerateProjectAsync(userSelection);

                        GenContext.ToolBox.Shell.ShowStatusBarMessage("Project created!!!");

                        SolutionName = newProjectInfo.name;
                        SolutionPath = ((FakeGenShell)GenContext.ToolBox.Shell).SolutionPath;
                        OnPropertyChanged(nameof(TempFolderAvailable));
                    }
                }
            }
            catch (WizardBackoutException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }

        private void AddNewFeature()
        {
            ConfigureGenContext(ForceLocalTemplatesRefresh);

            OutputPath = GenContext.GetTempGenerationPath(GenContext.Current.ProjectName);
            ClearContext();

            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewFeature(GenContext.InitializedLanguage);

                if (userSelection != null)
                {
                    NewItemGenController.Instance.FinishGeneration(userSelection);
                    OnPropertyChanged(nameof(TempFolderAvailable));
                    GenContext.ToolBox.Shell.ShowStatusBarMessage("Item created!!!");
                }
            }
            catch (WizardBackoutException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }

        private void ClearContext()
        {
            ProjectItems.Clear();
            MergeFilesFromProject.Clear();
            FailedMergePostActions.Clear();
            FilesToOpen.Clear();
        }

        private void AddNewPage()
        {
            ConfigureGenContext(ForceLocalTemplatesRefresh);

            OutputPath = GenContext.GetTempGenerationPath(GenContext.Current.ProjectName);
            ClearContext();
            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewPage(GenContext.InitializedLanguage);

                if (userSelection != null)
                {
                    NewItemGenController.Instance.FinishGeneration(userSelection);
                    OnPropertyChanged(nameof(TempFolderAvailable));
                    GenContext.ToolBox.Shell.ShowStatusBarMessage("Item created!!!");
                }
            }
            catch (WizardBackoutException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }

        private void LoadProject()
        {
            var loadProjectInfo = ShowLoadProjectDialog();

            if (!string.IsNullOrEmpty(loadProjectInfo))
            {
                SolutionPath = loadProjectInfo;
                SolutionName = Path.GetFileNameWithoutExtension(SolutionPath);

                var solutionDirectory = Path.GetDirectoryName(SolutionPath);
                var projFile = Directory.EnumerateFiles(solutionDirectory, "*.csproj", SearchOption.AllDirectories)
                        .Union(Directory.EnumerateFiles(solutionDirectory, "*.vbproj", SearchOption.AllDirectories)).FirstOrDefault();

                _language = Path.GetExtension(projFile) == ".vbproj" ? ProgrammingLanguages.VisualBasic : ProgrammingLanguages.CSharp;

                ConfigureGenContext(ForceLocalTemplatesRefresh);

                GenContext.Current = this;

                ProjectName = Path.GetFileNameWithoutExtension(projFile);
                ProjectPath = Path.GetDirectoryName(projFile);
                OutputPath = ProjectPath;
                IsWtsProject = GenContext.ToolBox.Shell.GetActiveProjectIsWts() ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(TempFolderAvailable));
                ClearContext();
            }
        }

        private void ConfigureVersions()
        {
            var dialog = new TemplatesContentView(WizardVersion, TemplatesVersion)
            {
                Owner = _host
            };

            var dialogRes = dialog.ShowDialog();

            if (dialogRes.HasValue && dialogRes.Value)
            {
                WizardVersion = dialog.ViewModel.Result.WizardVersion;
                TemplatesVersion = dialog.ViewModel.Result.TemplatesVersion;
                ConfigureGenContext(ForceLocalTemplatesRefresh);
            }
        }

        private void OpenInVs()
        {
            if (!string.IsNullOrEmpty(SolutionPath))
            {
                System.Diagnostics.Process.Start(SolutionPath);
            }
        }

        private void OpenInVsCode()
        {
            if (!string.IsNullOrEmpty(SolutionPath))
            {
                System.Diagnostics.Process.Start("code", $@"--new-window ""{Path.GetDirectoryName(SolutionPath)}""");
            }
        }

        private void OpenInExplorer()
        {
            if (!string.IsNullOrEmpty(SolutionPath))
            {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(SolutionPath));
            }
        }

        private void OpenTempInExplorer()
        {
            var tempGenerationPath = GetTempGenerationFolder();
            if (HasContent(tempGenerationPath))
            {
                System.Diagnostics.Process.Start(tempGenerationPath);
            }
        }

        private static string GetTempGenerationFolder()
        {
            if (GenContext.ToolBox == null || GenContext.ToolBox.Shell == null)
            {
                return string.Empty;
            }

            var path = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);

            Guid guid = GenContext.ToolBox.Shell.GetVsProjectId();
            return Path.Combine(path, guid.ToString());
        }

        private static bool HasContent(string tempPath)
        {
            return !string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath) && Directory.EnumerateDirectories(tempPath).Any();
        }

        [SuppressMessage("StyleCop", "SA1008", Justification = "StyleCop doesn't understand C#7 tuple return types yet.")]
        private (string name, string solutionName, string location) ShowNewProjectDialog()
        {
            var dialog = new NewProjectView()
            {
                Owner = _host
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return (dialog.ViewModel.Name, dialog.ViewModel.SolutionName, dialog.ViewModel.Location);
            }

            return (null, null, null);
        }

        private string ShowLoadProjectDialog()
        {
            var dialog = new LoadProjectView(SolutionPath)
            {
                Owner = _host
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return dialog.ViewModel.SolutionPath;
            }

            return string.Empty;
        }

        private void SetState(string message)
        {
            State = message;
            DoEvents();
        }

        private void AddLog(string message)
        {
            Log += message + Environment.NewLine;
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                _host.logScroll.ScrollToEnd();
            });
        }

        private void ConfigureGenContext(bool forceLocalTemplatesRefresh)
        {
            GenContext.Bootstrap(
                new LocalTemplatesSource(WizardVersion, TemplatesVersion, forceLocalTemplatesRefresh),
                new FakeGenShell(_language, msg => SetState(msg), l => AddLog(l), _host),
                new Version(WizardVersion),
                _language);

            CleanUpNotUsedContentVersions();
        }

        [SuppressMessage(
            "Usage",
            "VSTHRD001:Use Await JoinableTaskFactory.SwitchToMainThreadAsync() to switch to the UI thread",
            Justification = "Not applying this rule to this method as it was specifically desgned as is.")]
        public void DoEvents()
        {
            var frame = new DispatcherFrame(true);

            SendOrPostCallback method = (arg) =>
            {
                var f = arg as DispatcherFrame;
                f.Continue = false;
            };

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, method, frame);
            Dispatcher.PushFrame(frame);
        }

        private void CleanUpNotUsedContentVersions()
        {
            if (_wizardVersion == "0.0.0.0" && _templatesVersion == "0.0.0.0")
            {
                var templatesFolder = GetTemplatesFolder();
                if (Directory.Exists(templatesFolder))
                {
                    var dirs = Directory.EnumerateDirectories(templatesFolder);
                    foreach (var dir in dirs)
                    {
                        if (!dir.EndsWith("0.0.0.0", StringComparison.OrdinalIgnoreCase))
                        {
                            Fs.SafeDeleteDirectory(dir);
                        }
                    }
                }
            }
        }

        private string GetTemplatesFolder()
        {
            var templatesSource = new LocalTemplatesSource(_wizardVersion, _templatesVersion);
            var templatesSync = new TemplatesSynchronization(templatesSource, new Version(_wizardVersion));
            string currentTemplatesFolder = templatesSync.CurrentTemplatesFolder;

            return currentTemplatesFolder;
        }
    }
}
