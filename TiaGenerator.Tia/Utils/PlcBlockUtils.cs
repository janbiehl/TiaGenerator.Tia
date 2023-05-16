using System;
using System.Collections.Generic;
using Siemens.Engineering.SW;
using TiaGenerator.Tia.Models;
using PlcBlock = Siemens.Engineering.SW.Blocks.PlcBlock;

namespace TiaGenerator.Tia.Utils
{
	public static class PlcBlockUtils
	{
		
		public static Fingerprints GetFingerprints(PlcBlock plcBlock)
		{
			// Get the fingerprint provider service
			var provider = plcBlock.GetService<FingerprintProvider>();
			// Get the actual fingerprints for this block
			IList<Fingerprint> fingerprints = provider.GetFingerprints();
			
			// Prepare the return value
			var result = new Fingerprints();
			
			foreach(var fingerprint in fingerprints)
			{
				switch (fingerprint.Id)
				{
					case FingerprintId.Code:
						result.Code = fingerprint.Value;
						break;
					case FingerprintId.Comments:
						result.Comments = fingerprint.Value;
						break;
					case FingerprintId.Interface:
						result.Interface = fingerprint.Value;
						break;
					case FingerprintId.LibraryType:
						result.LibraryType = fingerprint.Value;
						break;
					case FingerprintId.Texts:
						result.Texts = fingerprint.Value;
						break;
					case FingerprintId.Alarms:
						result.Alarms = fingerprint.Value;
						break;
					case FingerprintId.Supervisions:
						result.Supervision = fingerprint.Value;
						break;
					case FingerprintId.TechnologyObject:
						result.TechnologyObject = fingerprint.Value;
						break;
					case FingerprintId.Events:
						result.Events = fingerprint.Value;
						break;
					case FingerprintId.TextualInterface:
						result.TextualInterface = fingerprint.Value;
						break;
					case FingerprintId.Properties:
						result.Properties = fingerprint.Value;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			return result;
		} 
	}
}