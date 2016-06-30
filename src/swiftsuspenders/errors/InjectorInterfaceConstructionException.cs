using System;

namespace SwiftSuspenders.Errors
{
	public class InjectorInterfaceConstructionException : InjectorException
	{
		public InjectorInterfaceConstructionException (string message) : base (message)
		{

		}
	}
}

