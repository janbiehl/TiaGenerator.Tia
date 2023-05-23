using System;
using Siemens.Engineering.CustomIdentity;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using TiaGenerator.Tia.Models;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Extensions
{
	public static class DeviceExtensions
	{
		public static string? Identifier(this Device device)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			if (device == null) throw new ArgumentNullException(nameof(device));
            
			var identityProvider = device.GetService<CustomIdentityProvider>();

			return identityProvider?.Get(DeviceUtils.DeviceIdentifierKey);
		}
	}
}