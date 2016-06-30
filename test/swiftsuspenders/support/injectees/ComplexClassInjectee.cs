using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class ComplexClassInjectee
	{
		[Inject]
		public ComplexClazz property { get; set; }
	}
}

