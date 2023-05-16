using System;
using Siemens.Engineering.CustomIdentity;
using Siemens.Engineering.HW;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Extensions
{
	public static class DeviceItemExtensions
	{
		public static string? Identifier(this DeviceItem deviceItem)
		{
			if (deviceItem == null) throw new ArgumentNullException(nameof(deviceItem));
            
			var identityProvider = deviceItem.GetService<CustomIdentityProvider>();

			return identityProvider?.Get(DeviceUtils.DeviceIdentifierKey);
		}
	}
}