using System;
using System.Collections.Generic;
using System.IO;
using Siemens.Engineering;
using TiaGenerator.Tia.Models;

namespace TiaGenerator.Tia.Utils
{
    /// <summary>
    /// Utility functions for project interaction
    /// </summary>
    public static class ProjectUtils
    {
        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="tiaPortal">The tia portal instance to create the project with</param>
        /// <param name="projectInfo">The information that will be used for project creation</param>
        /// <exception cref="TiaException"></exception>
        public static Project CreateProject(TiaPortal tiaPortal, CreateProjectInfo projectInfo)
        {
            try
            {
                if (tiaPortal == null) throw new ArgumentNullException(nameof(tiaPortal));

                projectInfo.Validate();

                var targetDirectoryInfo = new DirectoryInfo(projectInfo.TargetDirectory);

                var createParameters = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Name", projectInfo.Name),
                    new KeyValuePair<string, object>("TargetDirectory", targetDirectoryInfo)
                };
                
                if (projectInfo.Author != null)
                    createParameters.Add(new KeyValuePair<string, object>("Author", projectInfo.Author));
                
                if (projectInfo.Comment != null)
                    createParameters.Add(new KeyValuePair<string, object>("Comment", projectInfo.Comment));
                
                var createdProject = (Project) ((IEngineeringComposition)tiaPortal.Projects).Create(typeof (Project), createParameters);

                // set initial simulation support
                createdProject.IsSimulationDuringBlockCompilationEnabled = projectInfo.EnableSimulation;
                
                return createdProject;
            }
            catch (Exception e)
            {
                throw new TiaException("The project could not be created...", e);
            }
        }

        /// <summary>
        /// Open a tia portal project 
        /// </summary>
        /// <param name="tiaPortal">The tia portal instance to create the project with</param>
        /// <param name="filePath">The path to the project file to open</param>
        /// <param name="withUpgrade">When true a project created with a old version, will be updated</param>
        /// <param name="umacDelegate">Delegate that will be used for authentication</param>
        /// <returns>The opened project or null</returns>
        /// <exception cref="TiaException"></exception>
        public static Project? OpenProject(TiaPortal tiaPortal, string filePath, bool withUpgrade = false, UmacDelegate? umacDelegate = null)
        {
            try
            {
                if (tiaPortal == null) throw new ArgumentNullException(nameof(tiaPortal));

                if (string.IsNullOrWhiteSpace(filePath))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));

                var fileInfo = new FileInfo(filePath);

                if (!fileInfo.Exists)
                    throw new InvalidOperationException("The file does not exist");
                
                return withUpgrade ? 
                    tiaPortal.Projects.OpenWithUpgrade(fileInfo, umacDelegate, ProjectOpenMode.Primary) : 
                    tiaPortal.Projects.Open(fileInfo, umacDelegate, ProjectOpenMode.Primary);
            }
            catch (Exception e)
            {
                throw new TiaException("The project could not be opened...", e);
            }
        }

        /// <summary>
        /// Save project changes when there are any
        /// </summary>
        /// <param name="project">The project to save</param>
        /// <exception cref="TiaException"></exception>
        public static void SaveProject(Project project)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));

                //if (project.IsModified)
                project.Save();
            }
            catch (Exception e)
            {
                throw new TiaException("Could not save project...", e);
            }
        }

        /// <summary>
        /// Save the project as a new project at a new location
        /// </summary>
        /// <param name="project">The project to duplicate</param>
        /// <param name="targetDirectoryPath">The directory where the copy of the project will be saved to</param>
        /// <exception cref="TiaException"></exception>
        public static void SaveProjectAsNew(Project project, string targetDirectoryPath)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));
                
                if (string.IsNullOrWhiteSpace(targetDirectoryPath))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(targetDirectoryPath));

                var directoryInfo = new DirectoryInfo(targetDirectoryPath);

                project.SaveAs(directoryInfo);
            }
            catch (Exception e)
            {
                throw new TiaException("Could not save the project as new..", e);
            }
        }

        /// <summary>
        /// Create a project archive to share or store the project. The original project will not be touched in any way.
        /// The archive name is valid to contain no file extension, but it is recommended to apply a file extension
        /// like '.zapXX'. 
        /// </summary>
        /// <remarks>
        /// File extension example: TIA-V16 will be '.zap16'
        /// </remarks>
        /// <param name="projectToArchive">
        /// The project that will be archived, it will not be changed. Nor will it be deleted from disk
        /// </param>
        /// <param name="archiveDirectory">The directory to archive the project to</param>
        /// <param name="archiveName">The file name for the project archive, extension is optionally but recommended</param>
        /// <param name="archiveMode">The mode for the creating of the project archive</param>
        /// <exception cref="TiaException"></exception>
        public static void CreateProjectArchive(Project projectToArchive, string archiveDirectory, string archiveName, 
            ProjectArchivationMode archiveMode = ProjectArchivationMode.DiscardRestorableDataAndCompressed)
        {
            try
            {
                if (projectToArchive == null) throw new ArgumentNullException(nameof(projectToArchive));

                if (string.IsNullOrWhiteSpace(archiveDirectory))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(archiveDirectory));

                if (string.IsNullOrWhiteSpace(archiveName))
                    throw new ArgumentException("Value cannot be null or whitespace", nameof(archiveName));

                if (projectToArchive.IsModified)
                    throw new InvalidOperationException(
                        "The project contains unsaved changes, save the project before creating a archive");
                
                var targetDirectoryInfo = new DirectoryInfo(archiveDirectory);
                projectToArchive.Archive(targetDirectoryInfo, archiveName, archiveMode);
            }
            catch (Exception e)
            {
                throw new TiaException("Could not create the project archive..", e);
            }
        }

        /// <summary>
        /// Retrieve a project from a project archive.
        /// </summary>
        /// <param name="tiaPortal">The portal instance to use for the retrieval</param>
        /// <param name="archiveFilePath">The path to the archive file</param>
        /// <param name="targetDirectory">The directory path where the restored project will be located at</param>
        /// <param name="withUpgrade">When true the project will be upgraded, when it was archived with a previous version of TIA-Portal</param>
        /// <param name="umacDelegate">The delegate that handles the login credentials</param>
        /// <exception cref="TiaException"></exception>
        public static Project RestoreProjectArchive(TiaPortal tiaPortal, string archiveFilePath, string targetDirectory, 
            bool withUpgrade = false, UmacDelegate? umacDelegate = null)
        {
            try
            {
                if (tiaPortal == null) throw new ArgumentNullException(nameof(tiaPortal));

                if (string.IsNullOrWhiteSpace(archiveFilePath))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(archiveFilePath));
                
                if (string.IsNullOrWhiteSpace(targetDirectory))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(targetDirectory));
                
                var sourceFileInfo = new FileInfo(archiveFilePath);
                var targetDirectoryInfo = new DirectoryInfo(targetDirectory);

                if (!sourceFileInfo.Exists)
                    throw new InvalidOperationException($"The archive file does not exist at path: '{archiveFilePath}'");

                if (!targetDirectoryInfo.Exists)
                    throw new DirectoryNotFoundException(
                        $"The target directory does not exist at path: '{targetDirectory}");

                if (!withUpgrade)
                {
                    return tiaPortal.Projects.Retrieve(sourceFileInfo, targetDirectoryInfo, umacDelegate);
                }
                else
                {
                    return tiaPortal.Projects.RetrieveWithUpgrade(sourceFileInfo, targetDirectoryInfo, umacDelegate);
                }
                
            }
            catch (Exception e)
            {
                throw new TiaException("Could not restore the project from a archive", e);
            }
        }
    }
}