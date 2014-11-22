using System;

namespace swiftsuspenders.errors
{
	public class InjectorInterfaceConstructionException : InjectorException
	{
		public InjectorInterfaceConstructionException (string message) : base (message)
		{

		}
	}
}

