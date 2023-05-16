using System;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Models
{
	[Serializable]
	public class PlcUserConstant
	{
		public string Name { get; set; } = Constants.NotAssigned;
		public string DataTypeName { get; set; } = Constants.NotAssigned;
		public string Value { get; set; } = Constants.NotAssigned;
		public string Comment { get; set; } = string.Empty;
	
		public override string ToString() => Name;
	}
}