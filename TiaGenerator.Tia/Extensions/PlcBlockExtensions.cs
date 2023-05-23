using System;
using System.Collections.Generic;
using System.IO;
using Siemens.Engineering;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using TiaGenerator.Tia.Models;
using TiaGenerator.Tia.Utils;
using PlcBlock = Siemens.Engineering.SW.Blocks.PlcBlock;

namespace TiaGenerator.Tia.Extensions
{
	/// <summary>
	/// Extension methods for plc blocks
	/// </summary>
	public static class PlcBlockExtensions
	{
		/// <summary>
		/// Is the plc block from type function 
		/// </summary>
		/// <param name="plcBlock">The block to check</param>
		/// <returns>True, when the block is a function</returns>
		public static bool IsFc(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock is FC;
		}
		
		/// <summary>
		/// Is the plc block from type function block
		/// </summary>
		/// <param name="plcBlock">The block to check</param>
		/// <returns>True, when the block is a function block</returns>
		public static bool IsFb(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock is FB;
		}
		
		/// <summary>
		/// Is the plc block from type organization block
		/// </summary>
		/// <param name="plcBlock">The block to check</param>
		/// <returns>True, when the block is a organization block</returns>
		public static bool IsOb(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock is OB;
		}
		
		/// <summary>
		/// Is the plc block from type data block (global)
		/// </summary>
		/// <param name="plcBlock">The block to check</param>
		/// <returns>True, when the block is a global data block</returns>
		public static bool IsDb(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock is GlobalDB;
		}
		
		/// <summary>
		/// Is the plc block from type data block (instance)
		/// </summary>
		/// <param name="plcBlock">The block to check</param>
		/// <returns>True, when the block is a instance data block</returns>
		public static bool IsInstanceDb(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock is InstanceDB;
		}
		
		/// <summary>
		/// Is the plc block from type data block (array)
		/// </summary>
		/// <param name="plcBlock">The block to check</param>
		/// <returns>True, when the block is a array data block</returns>
		public static bool IsArrayDb(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock is ArrayDB;
		}

		/// <summary>
		/// Get the short identifier for a blocks type (FB, FC, OB...)
		/// </summary>
		/// <param name="plcBlock">The block to get the type for</param>
		/// <returns>The short identifier for a blocks type</returns>
		/// <exception cref="ArgumentOutOfRangeException">Not supported block type</exception>
		[Obsolete("Use GetBlockType() + BlockUtils.GetBlockTypeShort() instead", true)]
		public static string GetBlockIdentifier(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock switch
			{
				FC _ => "FC",
				FB _ => "FB",
				OB _ => "OB",
				GlobalDB _ => "DB",
				InstanceDB _ => "iDB",
				ArrayDB _ => "aDB",
				_ => throw new ArgumentOutOfRangeException( nameof(plcBlock),"The type of plc block is not supported")
			};
		}
		
		/// <summary>
		/// Get the type of block (FB, FC, Ob...)
		/// </summary>
		/// <param name="plcBlock">The block to get the type for</param>
		/// <returns>Enum entry that defines the type of block</returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static BlockType GetBlockType(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			return plcBlock switch
			{
				FC _ => BlockType.Fc,
				FB _ => BlockType.Fb,
				OB _ => BlockType.Ob,
				GlobalDB _ => BlockType.Db,
				InstanceDB _ => BlockType.Idb,
				ArrayDB _ => BlockType.Adb,
				_ => throw new ArgumentOutOfRangeException(nameof(plcBlock), "The type of plc block is not supported")
			};
		}

		/// <summary>
		/// Export a non know-how protected plc block to SIMATIC ML file (XML)
		/// </summary>
		/// <param name="plcBlock"></param>
		/// <param name="filePath"></param>
		/// <param name="exportOptions"></param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="TiaException"></exception>
		public static FileInfo ExportToFile(this PlcBlock plcBlock, string filePath, 
			ExportOptions exportOptions = ExportOptions.WithDefaults | ExportOptions.WithReadOnly)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			if (plcBlock == null) throw new ArgumentNullException(nameof(plcBlock));
			if (string.IsNullOrWhiteSpace(filePath))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));

			var fileInfo = new FileInfo(filePath);
			
			try
			{
				if (plcBlock.IsKnowHowProtected)
				{
					throw new NotSupportedException("The block is know how protected, this is currently not supported");
				}
				
				plcBlock.Export(fileInfo, exportOptions);
				return fileInfo;
			}
			catch (Exception e)
			{
				throw new TiaException("There was an error while exporting the plc block", e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blockComposition"></param>
		/// <param name="filePath"></param>
		/// <param name="importOptions"></param>
		/// <param name="swImportOptions"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="TiaException"></exception>
		public static IList<PlcBlock> ImportBlocksFromFile(this PlcBlockComposition blockComposition, string filePath, 
			ImportOptions importOptions, SWImportOptions swImportOptions)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();

			if (blockComposition == null) 
				throw new ArgumentNullException(nameof(blockComposition));
			
			if (string.IsNullOrWhiteSpace(filePath))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));

			var blockFile = new FileInfo(filePath);
			
			if (!blockFile.Exists)
				throw new InvalidOperationException($"The file does not exist at path: '{filePath}'");
			
			try
			{
				return blockComposition.Import(blockFile, importOptions, swImportOptions);
			}
			catch (Exception e)
			{
				throw new TiaException("There was an error while importing the plc blocks from file", e);
			}
		}
		
		public static Fingerprints GetFingerprints(this PlcBlock plcBlock)
		{
			using var activity = Tracing.ActivitySource?.StartActivity();


			return PlcBlockUtils.GetFingerprints(plcBlock);
		}
		
		/*[Obsolete("Use GetFingerprints() instead", true)]
		public static (string codeFingerprint, string interfaceFingerprint) GetFingerprints(this PlcBlock plcBlock)
		{
			if (plcBlock == null) throw new ArgumentNullException(nameof(plcBlock));

			var fingerprintProvider = plcBlock.GetService<FingerprintProvider>();
			IList<Fingerprint> fingerprints = fingerprintProvider.GetFingerprints();

			var codeFingerprint = string.Empty;
			var interfaceFingerprint = string.Empty;
			
			foreach (var fingerprint in fingerprints)
			{
				switch (fingerprint.Id)
				{
					case FingerprintId.Code:
						codeFingerprint = fingerprint.Value;
						break;
					case FingerprintId.Interface:
						interfaceFingerprint = fingerprint.Value;
						break;
				}
			}

			if (plcBlock.IsDb())
				// DB's do not have a code fingerprint so we are setting it to an empty string
				codeFingerprint = string.Empty;

			return (codeFingerprint, interfaceFingerprint);
		}*/
	}
}