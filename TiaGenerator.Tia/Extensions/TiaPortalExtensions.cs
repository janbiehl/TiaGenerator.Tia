using Siemens.Engineering;
using TiaGenerator.Tia.Models;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Extensions
{
	/// <summary>
	/// Extension methods for the <see cref="TiaPortal"/> class
	/// </summary>
	public static class TiaPortalExtensions
	{
		/// <summary>
		/// Create a new project
		/// </summary>
		/// <param name="tiaPortal">The tia portal instance to create the project with</param>
		/// <param name="projectInfo">The information that will be used for project creation</param>
		/// <exception cref="TiaException"></exception>
		public static Project CreateProject(this TiaPortal tiaPortal, CreateProjectInfo projectInfo)
		{
			return ProjectUtils.CreateProject(tiaPortal, projectInfo);
		}

		/// <summary>
		/// Open a tia portal project 
		/// </summary>
		/// <param name="tiaPortal">The tia portal instance to create the project with</param>
		/// <param name="filePath">The path to the project file to open</param>
		/// <param name="withUpgrade">When true a project created with a old version, will be updated</param>
		/// <param name="umacDelegate">Delegate that handles the login</param>
		/// <returns>The opened project or null</returns>
		/// <exception cref="TiaException"></exception>
		public static Project? OpenProject(this TiaPortal tiaPortal, string filePath, bool withUpgrade = false, UmacDelegate? umacDelegate = null)
		{
			return ProjectUtils.OpenProject(tiaPortal, filePath, withUpgrade, umacDelegate);
		}

		/// <summary>
		/// Retrieve a project from a project archive.
		/// </summary>
		/// <param name="tiaPortal">The portal instance to use for the retrieval</param>
		/// <param name="archiveFilePath">The path to the archive file</param>
		/// <param name="targetDirectory">The directory path where the restored project will be located at</param>
		/// <param name="withUpgrade">When true the project will be upgraded, when it was archived with a previous version of TIA-Portal</param>
		/// <param name="umacDelegate">Delegate that handles the login</param>
		/// <exception cref="TiaException"></exception>
		public static Project? RestoreProjectArchive(this TiaPortal tiaPortal, string archiveFilePath, string targetDirectory,
			bool withUpgrade = false, UmacDelegate? umacDelegate = null)
		{
			return ProjectUtils.RestoreProjectArchive(tiaPortal, archiveFilePath, targetDirectory, withUpgrade, umacDelegate);
		}
	}
}