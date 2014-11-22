using System;

namespace swiftsuspenders.errors
{
	public class InjectorException : Exception
	{
		public InjectorException (string message) : base (message)
		{
		}
	}
}

