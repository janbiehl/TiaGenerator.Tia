using System;
using System.Collections.Generic;
using Siemens.Engineering.HW;
using Siemens.Engineering.SW;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Models
{
	[Serializable]
	public class PlcDevice
	{
		/// <summary>
		/// Unique identifier for a plc device
		/// </summary>
		public Guid Guid { get; set; }
		/// <summary>
		/// The name this object belongs to in the hierarchy
		/// </summary>
		public string DeviceName { get; set; } = Constants.NotAssigned;
		/// <summary>
		/// The name for this plc device in hierarchy
		/// </summary>
		public string DeviceItemName { get; set; } = Constants.NotAssigned;

		public string PlcSoftwareName { get; set; } = Constants.NotAssigned;
		/// <summary>
		/// Device group identifier for example S7-1500
		/// </summary>
		public string DeviceIdentifier { get; set; } = Constants.NotAssigned;
		/// <summary>
		/// The order number for the specific hardware
		/// </summary>
		public string TypeIdentifier { get; set; } = Constants.NotAssigned;
	
		/// <summary>
		/// Blocks that are inside this plc device, the hierarchy information is stored in the block itself
		/// </summary>
		public IList<PlcBlock> Blocks { get; set; } = Array.Empty<PlcBlock>();
		/// <summary>
		/// Tag tables that are present in the PLC
		/// </summary>
		public IList<PlcTagTable> TagTables { get; set; } = Array.Empty<PlcTagTable>();
		
		public override string ToString() => DeviceItemName;

		/// <summary>
		/// Gather the main information and get a PlcDevice from it. Will not contain any block or tag data
		/// </summary>
		/// <param name="device">The device where the plc lives in</param>
		/// <param name="deviceItem">The device item containing the plc software</param>
		/// <param name="plcSoftware">The plc software container</param>
		/// <returns>A PlcDevice containing the gathered information</returns>
		public static PlcDevice From(Device device, DeviceItem deviceItem, PlcSoftware plcSoftware)
		{
			return new PlcDevice()
			{
				Guid = Guid.NewGuid(),
				DeviceName = device.Name,
				DeviceItemName = deviceItem.Name,
				PlcSoftwareName = plcSoftware.Name,
				DeviceIdentifier = device.TypeIdentifier,
				TypeIdentifier = deviceItem.TypeIdentifier
			};
		}
	}
}