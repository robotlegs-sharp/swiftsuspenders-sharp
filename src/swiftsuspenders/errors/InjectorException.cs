using System;

namespace SwiftSuspenders.Errors
{
	public class InjectorException : Exception
	{
		public InjectorException (string message) : base (message)
		{
		}
	}
}

