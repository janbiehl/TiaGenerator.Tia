using System;
using Siemens.Engineering.SW.Blocks;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Models
{
	[Serializable]
	[Obsolete]
	public class PlcBlock
	{
		/// <summary>
		/// Name that is assigned to the block, must be unique in a plc
		/// </summary>
		public string Name { get; set; } = Constants.NotAssigned;
		/// <summary>
		/// Unique identifier for the block
		/// </summary>
		public Guid Guid { get; set; }
		/// <summary>
		/// The number that is assigned to this block
		/// </summary>
		public int Number { get; set; }
		public bool AutoNumber { get; set; }
		public ProgrammingLanguage Language { get; set; }
		public BlockType BlockType { get; set; }

		public string Path { get; set; } = string.Empty;

		public override string ToString()
		{
			var blockTypeShortName = PlcSoftwareUtils.GetBlockTypeShortName(BlockType);
			return $"{Name} [{blockTypeShortName}{Number}]" ;
		}
	}
}