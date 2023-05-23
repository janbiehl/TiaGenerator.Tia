using System.Diagnostics;
// ReSharper disable MemberCanBePrivate.Global

namespace TiaGenerator.Tia
{
	/// <summary>
	/// Tracing for this library
	/// </summary>
	public static class Tracing
	{
		public const string InstrumentationName = "TiaGenerator.Tia";
		public const string InstrumentationVersion = "0.0.01";

		internal static readonly ActivitySource? ActivitySource;
			
		static Tracing()
		{
			ActivitySource = new ActivitySource(InstrumentationName, InstrumentationVersion);	
		}
	}
}