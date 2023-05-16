using Siemens.Engineering;

namespace TiaGenerator.Tia.Extensions
{
	public static class MultiLingualTextExtensions
	{
		/// <summary>
		/// Get the text that is stored at the lowest index
		/// </summary>
		/// <param name="multilingualText"></param>
		/// <returns></returns>
		public static string GetDefaultText(this MultilingualText multilingualText)
		{
			return multilingualText.Items[0].Text;
		}
	}
}