using System;

namespace TiaGenerator.Tia
{
	public class TiaException : Exception
	{
		public TiaException()
		{
        
		}

		public TiaException(string message) : base(message)
		{
		}

		public TiaException(string message, Exception inner) : base(message, inner)
		{
        
		}
	}
}