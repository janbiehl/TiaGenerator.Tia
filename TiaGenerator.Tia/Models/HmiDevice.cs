using System.IO;
using Siemens.Engineering.Hmi;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;

namespace TiaGenerator.Tia.Models
{
	public class HmiDevice
	{
		public Device Device { get; }
		public DeviceItem DeviceItem { get; }
		public HmiTarget HmiSoftware { get; }

		public HmiDevice(Device device)
		{
			foreach (var deviceItem in device.DeviceItems)
			{
				var softwareContainer = deviceItem.GetService<SoftwareContainer>();

				if (softwareContainer?.Software is HmiTarget hmiSoftware)
				{
					Device = device;
					DeviceItem = deviceItem;
					HmiSoftware = hmiSoftware;
					break; // Exit the loop
				}
			}

			if (Device is null || DeviceItem is null || HmiSoftware is null)
				throw new InvalidDataException($"Device '{device.Name}' does not contain a HMI software.");
		}
		
		public HmiDevice(Device device, DeviceItem deviceItem, HmiTarget hmiSoftware)
		{
			Device = device;
			DeviceItem = deviceItem;
			HmiSoftware = hmiSoftware;
		}
	}
}