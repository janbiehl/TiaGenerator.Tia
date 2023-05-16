using System;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Models
{
	[Serializable]
	public class PlcTag
	{
		public string Name { get; set; } = Constants.NotAssigned;
		public string DataTypeName { get; set; } = Constants.NotAssigned;
		public string LogicalAddress { get; set; } = Constants.NotAssigned;
		public bool ExternalAccessible { get; set; }
		public bool ExternalVisible { get; set; }
		public bool ExternalWritable { get; set; }
		public string Comment { get; set; } = Constants.NotAssigned;

		public override string ToString() => Name;
	}
}