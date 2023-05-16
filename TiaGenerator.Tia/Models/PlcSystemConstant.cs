using System;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Models
{
	[Serializable]
	public class PlcSystemConstant
	{
		public string Name { get; set; } = Constants.NotAssigned;
		public string DataTypeName { get; set; } = Constants.NotAssigned;
		public string Value { get; set; } = Constants.NotAssigned;

		public override string ToString() => Name;
	}
}