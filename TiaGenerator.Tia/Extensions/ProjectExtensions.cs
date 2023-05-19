using System.Collections.Generic;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using TiaGenerator.Tia.Models;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Extensions
{
	public static class ProjectExtensions
	{
		/// <summary>
		/// Save project changes when there are any
		/// </summary>
		/// <param name="project">The project to save</param>
		/// <exception cref="TiaException"></exception>
		public static void Save(this Project project)
		{
			ProjectUtils.SaveProject(project);
		}

		/// <summary>
		/// Save the project as a new project at a new location
		/// </summary>
		/// <param name="project">The project to duplicate</param>
		/// <param name="targetDirectory">The directory where the copy of the project will be saved to</param>
		/// <exception cref="TiaException"></exception>
		public static void SaveAsNew(this Project project, string targetDirectory)
		{
			ProjectUtils.SaveProjectAsNew(project, targetDirectory);
		}

		/// <summary>
		/// Create a project archive to share or store the project. The original project will not be touched in any way.
		/// The archive name is valid to contain no file extension, but it is recommended to apply a file extension
		/// like '.zapXX'. 
		/// </summary>
		/// <remarks>
		/// File extension example: TIA-V16 will be '.zap16'
		/// </remarks>
		/// <param name="project">
		/// The project that will be archived, it will not be changed. Nor will it be deleted from disk
		/// </param>
		/// <param name="archiveDirectory">The directory to archive the project to</param>
		/// <param name="archiveName">The file name for the project archive, extension is optionally but recommended</param>
		/// <param name="archiveMode">The mode for the creating of the project archive</param>
		/// <exception cref="TiaException"></exception>
		public static void CreateArchive(this Project project, string archiveDirectory, string archiveName,
			ProjectArchivationMode archiveMode = ProjectArchivationMode.DiscardRestorableDataAndCompressed)
		{
			ProjectUtils.CreateProjectArchive(project, archiveDirectory, archiveName, archiveMode);
		}

		/// <summary>
		/// Find the first PLC device in a project
		/// </summary>
		/// <param name="project">The project to search</param>
		/// <returns>Null, or a tuple containing the required information</returns>
		/// <exception cref="TiaException"></exception>
		public static (Device device, DeviceItem deviceItem, PlcSoftware plcSoftware)? FindFirstPlcDevice(
			this Project project)
		{
			return DeviceUtils.FindFirstPlcDevice(project);
		}

		/// <summary>
		/// Find any PLC devices in a project
		/// </summary>
		/// <param name="project">The project to search</param>
		/// <returns>Empty collection, or List of tuples containing the required information's</returns>
		/// <exception cref="TiaException"></exception>
		public static List<(Device device, DeviceItem deviceItem, PlcSoftware plcSoftware)>? FindAnyPlcDevices(
			this Project project)
		{
			return DeviceUtils.FindAnyPlcDevices(project);
		}

		/// <summary>
		/// Find the first HMI device in a project
		/// </summary>
		/// <param name="project">The project to search</param>
		/// <returns>Null, or a tuple containing the required information</returns>
		/// <exception cref="TiaException"></exception>
		public static HmiDevice? FindFirstHmiDevice(this Project project)
		{
			return DeviceUtils.FindFirstHmiDevice(project);
		}

		/// <summary>
		/// Find any HMI devices in a project
		/// </summary>
		/// <param name="project">The project to search</param>
		/// <returns>Empty collection, or List of tuples containing the required information's</returns>
		/// <exception cref="TiaException"></exception>
		public static List<HmiDevice> FindAnyHmiDevices(
			this Project project)
		{
			return DeviceUtils.FindAnyHmiDevices(project);
		}

		/// <summary>
		/// Create a new device
		/// </summary>
		/// <param name="project">The project to create the device in</param>
		/// <param name="typeIdentifier">Defines the device that is to be created</param>
		/// <param name="deviceRoot">The root name for hardware configuration view</param>
		/// <param name="hierarchyName">The name that will be displayed in the project hierarchy</param>
		/// <returns>The newly created device</returns>
		/// <exception cref="TiaException"></exception>
		public static Device CreateDevice(this Project? project, string typeIdentifier, string deviceRoot,
			string hierarchyName)
		{
			return DeviceUtils.CreateDevice(project, typeIdentifier, deviceRoot, hierarchyName);
		}

		public static void EnumerateDevices(this Project project)
		{
			DeviceUtils.EnumerateDevices(project);
		}
	}
}