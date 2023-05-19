using System;

namespace TiaGenerator.Tia.Models
{
	[Serializable]
	public class Fingerprints
	{
		/// <summary>
		/// Considers all changes in the code inside the body of the block.
		/// It does not consider the compilation result.
		/// </summary>
		public string? Code { get; set; }
			
		/// <summary>
		/// Considers all changes in the interface of a block. Including start values of a DB
		/// </summary>
		public string? Interface { get; set; }

		/// <summary>
		/// Considers changes in the properties of a block. e.g. name, number
		/// </summary>
		public string? Properties { get; set; }
			
		/// <summary>
		/// Considers changes in the comments of a block.
		/// In case of OBs the fingerprint also changes when the list
		/// of available languages in project language setting changes
		/// </summary>
		public string? Comments { get; set; }
			
		/// <summary>
		/// Exists when a block is connected to a library type
		/// </summary>
		public string? LibraryType { get; set; }
			
		/// <summary>
		/// With V15 SP1 this fingerprint only exists for Graph blocks
		/// </summary>
		public string? Texts { get; set; }
			
		/// <summary>
		/// Exists when a block uses alarming.
		/// </summary>
		public string? Alarms { get; set; }
			
		/// <summary>
		/// Exists when a block contains supervision
		/// </summary>
		public string? Supervision { get; set; }
			
		/// <summary>
		/// Exists only for technology object DBs
		/// </summary>
		public string? TechnologyObject { get; set; }
			
		/// <summary>
		/// Exists only for OB
		/// </summary>
		public string? Events { get; set; }
			
		/// <summary>
		/// Exists when the block has a textual interface
		/// </summary>
		public string? TextualInterface { get; set; }
	}
}