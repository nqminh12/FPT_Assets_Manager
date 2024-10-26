using AssetsManagerLibrary.Models;
using AssetsManagerLibrary.Repositories;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using WPFFolderBrowser;
using Microsoft.VisualBasic.FileIO;


namespace AssetsManagerWPF.ViewModels
{
    public class ProjectManagementViewModel : INotifyPropertyChanged
    {
        private IProjectRepository projectRepository;
        private Project selectedProject;

        public ProjectManagementViewModel()
        {
            Projects = new ObservableCollection<Project>();
            LoadCommand = new DelegateCommand(LoadProjectList);
            DetailCommand = new DelegateCommand(DetailProject);
            InsertCommand = new DelegateCommand(InsertProject);
            UpdateCommand = new DelegateCommand(UpdateProject);
            DeleteCommand = new DelegateCommand(DeleteProject);
            CloseCommand = new DelegateCommand<Window>(CloseWindow);
            BrowseCommand = new DelegateCommand(BrowseForFolder);
        }

        public ProjectManagementViewModel(IProjectRepository repository) : this()
        {
            projectRepository = repository;
            LoadProjectList();
            selectedProject = new Project();
        }

        public ObservableCollection<Project> Projects { get; set; }

        public Project SelectedProject
        {
            get => selectedProject;
            set
            {
                selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand DetailCommand { get; }
        public ICommand BrowseCommand { get; }
        public ICommand InsertCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CloseCommand { get; }

        private void BrowseForFolder()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "elect a folder to save",
                Filter = "All files (*.*)|*.*",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedProject.Path = System.IO.Path.GetDirectoryName(dialog.FileName);
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private bool MoveProjectFolder(string sourcePath, string destinationPath)
        {
            try
            {
                if (Directory.Exists(sourcePath))
                {
                    if (Path.GetPathRoot(sourcePath) == Path.GetPathRoot(destinationPath))
                    {
                        Directory.Move(sourcePath, destinationPath);
                        return true;
                    }
                    else
                    {
                        if (CopyDirectory(sourcePath, destinationPath))
                        {
                            Directory.Delete(sourcePath, true);
                            MessageBox.Show("The project path and name have been successfully updated!",
                                            "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
                else
                {
                    MessageBox.Show("The source folder does not exist!",
                                    "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update the project path or name: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool CopyDirectory(string sourceDir, string destinationDir)
        {
            try
            {
                FileSystem.CopyDirectory(sourceDir, destinationDir, UIOption.AllDialogs);
                return true;

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Failed to copy the folder: {ex.Message}",
                //                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool UpdateProjectURLAndProjectName()
        {
            string selectedFolderPath = SelectedProject.Path;
            Project projectCheck = projectRepository.GetProjectByID(SelectedProject.ProjectId);
            string previousPath = projectCheck.Path;
            string previousProjectName = projectCheck.ProjectName;
            string currentProjectName = SelectedProject.ProjectName;
            string currentProjectPath = Path.Combine(previousPath, previousProjectName);
            string newProjectPath = Path.Combine(selectedFolderPath, previousProjectName);

            if (Directory.Exists(newProjectPath))
            {
                MessageBox.Show($"The folder {newProjectPath} already exists. Please select a different location!",
                                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!MoveProjectFolder(currentProjectPath, newProjectPath))
            {
                return false;
            }

            if (previousProjectName != currentProjectName)
            {
                Directory.Move(newProjectPath, Path.Combine(selectedFolderPath, currentProjectName));
                return true;
            }
            return false;
        }




        private void DetailProject()
        {
            if (projectRepository == null) return;
            if (SelectedProject == null || SelectedProject.ProjectId <= 0)
            {
                MessageBox.Show("Please choose project first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void LoadProjectList()
        {
            if (projectRepository == null) return;

            Projects.Clear();
            foreach (var project in projectRepository.GetProjects())
            {
                Projects.Add(project);
            }
        }

        private void CloseWindow(Window window)
        {
            window?.Close();
        }

        private void InsertProject()
        {
            if (SelectedProject != null && projectRepository != null)
            {
                try
                {
                    string projectDirectory = Path.Combine(SelectedProject.Path, SelectedProject.ProjectName);

                    if (Directory.Exists(projectDirectory))
                    {
                        MessageBox.Show($"The folder '{projectDirectory}' already exists! Please choose a different name or path.",
                                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Directory.CreateDirectory(projectDirectory);

                    SelectedProject.ProjectId = 0;
                    projectRepository.InsertProject(SelectedProject);

                    LoadProjectList();

                    MessageBox.Show("Project inserted and folder created successfully!",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to insert project or create folder: {ex.Message}",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill out project details before inserting!",
                                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateProject()
        {
            if (SelectedProject != null && projectRepository != null && SelectedProject.ProjectId > 0)
            {
                try
                {
                    if (UpdateProjectURLAndProjectName())
                    {
                        projectRepository.UpdateProject(SelectedProject);
                        LoadProjectList();
                        MessageBox.Show("Project updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to update project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"Choose prroject first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteProject()
        {
            if (SelectedProject != null && projectRepository != null && SelectedProject.ProjectId > 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete this project?",
                    "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (Directory.Exists(SelectedProject.Path))
                        {
                            projectRepository.DeleteProject(SelectedProject);
                            Directory.Delete(Path.Combine(SelectedProject.Path, SelectedProject.ProjectName), true);
                            LoadProjectList();
                            MessageBox.Show("Project deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Project doesn't exist!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show($"Choose prroject first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
