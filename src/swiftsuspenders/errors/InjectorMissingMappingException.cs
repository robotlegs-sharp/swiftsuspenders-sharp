using System;

namespace swiftsuspenders.errors
{
	public class InjectorMissingMappingException : InjectorException
	{
		public InjectorMissingMappingException (string message) : base (message)
		{

		}
	}
}

