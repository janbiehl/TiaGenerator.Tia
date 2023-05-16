using System;
using System.IO;

namespace TiaGenerator.Tia.Models
{
	/// <summary>
	/// Information that will be used for TIA-Portal project creation
	/// </summary>
	public struct CreateProjectInfo
	{
		/// <summary>
		/// Who is responsible for this project
		/// </summary>
		public string? Author { get; set; }
		
		/// <summary>
		/// Description for the newly created project
		/// </summary>
		public string? Comment { get; set; }
		
		/// <summary>
		/// The name that the new project will get
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// The directory the project will be created in
		/// </summary>
		public string TargetDirectory { get; set; }

		/// <summary>
		/// Is simulation required for this Project. Can be set later on
		/// </summary>
		public bool EnableSimulation { get; set; }
		
		public void Validate()
		{
			if (string.IsNullOrWhiteSpace(TargetDirectory))
				throw new ArgumentException("Target directory may not be null, empty or whitespace", nameof(TargetDirectory));
			
			if (string.IsNullOrWhiteSpace(Name))
				throw new ArgumentException($"{nameof(Name)} may not be null, empty or whitespace", nameof(Name));

			if (!Directory.Exists(TargetDirectory))
				throw new DirectoryNotFoundException($"The target directory does not exist at path: '{TargetDirectory}'");

			var projectDirectory = TargetDirectory + @"\" + Name;

			if (Directory.Exists(projectDirectory))
				throw new InvalidOperationException($"There is already a project with the exact same name: '{projectDirectory}'");
		}
	}
}