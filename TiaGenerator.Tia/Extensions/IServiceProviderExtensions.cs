using System;
using Siemens.Engineering;
using Siemens.Engineering.CustomIdentity;
using TiaGenerator.Tia.Utils;

namespace TiaGenerator.Tia.Extensions
{
	/// <summary>
	/// Extensions methods used for the IEngineeringServiceProvider
	/// </summary>
	public static class ServiceProviderExtensions
	{
		public static string? GetIdentifier(this IEngineeringServiceProvider engineeringServiceProvider)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			if (engineeringServiceProvider == null) 
				throw new ArgumentNullException(nameof(engineeringServiceProvider));
            
			var identityProvider = engineeringServiceProvider.GetService<CustomIdentityProvider>();

			return identityProvider?.Get(DeviceUtils.DeviceIdentifierKey);
		}

		public static void SetIdentifier(this IEngineeringServiceProvider engineeringServiceProvider, string identifier)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			if (engineeringServiceProvider == null) 
				throw new ArgumentNullException(nameof(engineeringServiceProvider));
            
			var identityProvider = engineeringServiceProvider.GetService<CustomIdentityProvider>();

			identityProvider?.Set(DeviceUtils.DeviceIdentifierKey, identifier); 
		}

		public static void SetIdentifier(this IEngineeringServiceProvider engineeringServiceProvider,
			ReadOnlySpan<byte> bytes)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			engineeringServiceProvider.SetIdentifier(bytes.ToString());        
		}
	}
}