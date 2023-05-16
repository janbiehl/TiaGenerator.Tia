namespace TiaGenerator.Tia
{
	/// <summary>
	/// Defines which type (Ob, FB, FC...) a block is 
	/// </summary>
	public enum BlockType
	{
		/// <summary>
		/// No defined block type
		/// </summary>
		Undefined,
		/// <summary>
		/// Organization block
		/// </summary>
		Ob,
		/// <summary>
		/// Function block
		/// </summary>
		Fb,
		/// <summary>
		/// Function
		/// </summary>
		Fc,
		/// <summary>
		/// Global data block
		/// </summary>
		Db,
		/// <summary>
		/// Instance data block
		/// </summary>
		Idb,
		/// <summary>
		/// Array data block
		/// </summary>
		Adb
	}
}