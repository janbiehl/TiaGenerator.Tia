using System;
using System.Collections.Generic;
using System.IO;
using Siemens.Engineering;
using TiaGenerator.Tia.Extensions;
using TiaGenerator.Tia.Models;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia
{
	public class ProjectWrapper : IDisposable
	{
		private const string ProjectAlreadyPresent = "There is already a project open, close it first";
		
		/// <summary>
		/// The tia portal instance the project will be opened with
		/// </summary>
		private readonly TiaPortal _tiaPortal;
		/// <summary>
		/// The project that is used for operations on projects
		/// </summary>
		private Project? _project;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public ProjectWrapper()
		{
			_tiaPortal = new TiaPortal(TiaPortalMode.WithoutUserInterface);
		}

		/// <summary>
		/// Open a project tia-portal project file
		/// </summary>
		/// <param name="projectFilePath">The path to the TIA-Project</param>
		/// <param name="withUpgrade">Shall the project be upgraded</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="FileNotFoundException"></exception>
		/// <exception cref="Exception"></exception>
		public void OpenProject(
			string projectFilePath, 
			bool withUpgrade = true)
		{
			if (string.IsNullOrWhiteSpace(projectFilePath))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(projectFilePath));

			if (!File.Exists(projectFilePath))
				throw new FileNotFoundException("The file was not found", projectFilePath);
			
			_project = _tiaPortal.OpenProject(projectFilePath, withUpgrade);

			if (_project is null)
				throw new Exception("The project could not be opened");
		}

		/// <summary>
		/// Open a project from a TIA-Project archive
		/// </summary>
		/// <param name="archiveFilePath">The path to the TIA-Project-archive</param>
		/// <param name="projectTargetDirectoryPath">The directory where the project should be restored at</param>
		/// <param name="withUpgrade">Shall the project be upgraded</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="Exception"></exception>
		public void OpenProjectWithRestore(
			string archiveFilePath, 
			string projectTargetDirectoryPath, 
			bool withUpgrade = true)
		{
			if (_project != null)
				throw new InvalidOperationException(ProjectAlreadyPresent);
			
			if (string.IsNullOrWhiteSpace(archiveFilePath))
				throw new ArgumentException("Value cannot be null or whitespace.", 
					nameof(archiveFilePath));
			if (string.IsNullOrWhiteSpace(projectTargetDirectoryPath))
				throw new ArgumentException("Value cannot be null or whitespace.", 
					nameof(projectTargetDirectoryPath));

			_project = _tiaPortal.RestoreProjectArchive(archiveFilePath, projectTargetDirectoryPath, withUpgrade);
			
			if (_project is null)
				throw new Exception("The project could not be opened");
		}

		/// <summary>
		/// Get a list containing ever PLC-device that is inside the currently opened PLC-Device
		/// </summary>
		/// <returns>A list containing every plc device</returns>
		/// <exception cref="InvalidOperationException">No project opened</exception>
		public IList<PlcDevice> GetPlcDevices()
		{
			if (_project is null)
				throw new InvalidOperationException(
					"There is no open project, call OpenProject or OpenProjectWithRestore before any other call");

			var plcDevices = _project.FindAnyPlcDevices();

			if (plcDevices is null)
				throw new Exception("There was an error, no plc devices found");
			
			var result = new List<PlcDevice>(plcDevices.Count);

			foreach (var (device, deviceItem, plcSoftware) in plcDevices)
			{
				// gather the information an construct a plc device from it
				var plcDevice = PlcDevice.From(device, deviceItem, plcSoftware);

				// get the plc's tag data
				plcDevice.TagTables = PlcSoftwareUtils.GetAllTags(plcSoftware);

				// get the plc's block data
				plcDevice.Blocks = PlcSoftwareUtils.GetAllBlocks(plcSoftware);

				result.Add(plcDevice);
			}

			return result;
		}

		/// <summary>
		/// Create a project
		/// </summary>
		/// <param name="projectInfo">The information that will be used for creation</param>
		public void CreateProject(CreateProjectInfo projectInfo)
		{
			if (_project != null)
				throw new InvalidOperationException(ProjectAlreadyPresent);
			
			projectInfo.Validate();

			_project = _tiaPortal.CreateProject(projectInfo) ??
			           throw new Exception("There was an error, no project created");
		}
		
		
#region IDisposable

		public void Dispose()
		{
			_project?.Close();
			_tiaPortal?.Dispose();
		}

#endregion
	}
}