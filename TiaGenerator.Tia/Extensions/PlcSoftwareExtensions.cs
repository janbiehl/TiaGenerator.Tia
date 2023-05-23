using System;
using System.Collections.Generic;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using TiaGenerator.Tia.Models;
using TiaGenerator.Tia.Utils;
using PlcBlock = TiaGenerator.Tia.Models.PlcBlock;

namespace TiaGenerator.Tia.Extensions
{
	public static class PlcSoftwareExtensions
	{
		// public static void EnumerateTagTables(this PlcSoftware plcSoftware)
		// {
		// 	PlcSoftwareUtils.EnumerateTagTables(plcSoftware);
		// }
		//
		// public static void EnumerateBlocks(this PlcSoftware plcSoftware)
		// {
		// 	PlcSoftwareUtils.EnumerateBlocks(plcSoftware);
		// }

		public static IList<PlcBlock> GetBlockInformation(this PlcSoftware plcSoftware)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));

			return PlcSoftwareUtils.GetAllBlocks(plcSoftware);
		}

		public static IList<PlcTagTable> GetTagTableInformation(this PlcSoftware plcSoftware)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();
			
			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));

			return PlcSoftwareUtils.GetAllTags(plcSoftware);
		}

		public static CompilerResult? Compile(this PlcSoftware plcSoftware)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));

			var compilableService = plcSoftware.GetService<ICompilable>();
			return compilableService?.Compile();
		}
		
		public static PlcBlockUserGroup GetOrCreateGroup(this PlcSoftware plcSoftware, string[] blockGroups)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return PlcSoftwareUtils.GetOrCreateBlockGroup(plcSoftware, blockGroups);
		}

		public static PlcBlockUserGroup GetOrCreateBlockGroup(this PlcSoftware plcSoftware, string groupName,
			PlcBlockUserGroupComposition? groupComposition = null)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return PlcSoftwareUtils.GetOrCreateBlockGroup(plcSoftware, groupName, groupComposition);
		}
	}
}