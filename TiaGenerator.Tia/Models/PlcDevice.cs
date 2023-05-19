using System.IO;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;

namespace TiaGenerator.Tia.Models
{
	public class PlcDevice
	{
		public Device Device { get; }
		public DeviceItem DeviceItem { get; }
		public PlcSoftware PlcSoftware { get; }

		public PlcDevice(Device device)
		{
			foreach (var deviceItem in device.DeviceItems)
			{
				var softwareContainer = deviceItem.GetService<SoftwareContainer>();

				if (!(softwareContainer?.Software is PlcSoftware plcSoftware)) continue;
				
				Device = device;
				DeviceItem = deviceItem;
				PlcSoftware = plcSoftware;
				break; // Exit the loop
			}

			if (Device is null || DeviceItem is null || PlcSoftware is null)
				throw new InvalidDataException($"Device '{device.Name}' does not contain a PLC software.");
		}
		
		public PlcDevice(Device device, DeviceItem deviceItem, PlcSoftware plcSoftware)
		{
			Device = device;
			DeviceItem = deviceItem;
			PlcSoftware = plcSoftware;
		}
	}
}