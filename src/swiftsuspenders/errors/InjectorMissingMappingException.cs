using System;

namespace SwiftSuspenders.Errors
{
	public class InjectorMissingMappingException : InjectorException
	{
		public InjectorMissingMappingException (string message) : base (message)
		{

		}
	}
}

