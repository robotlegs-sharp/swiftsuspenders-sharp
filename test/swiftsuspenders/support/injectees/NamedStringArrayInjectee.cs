using System;

namespace SwiftSuspenders.Support.Injectees
{
	public class NamedStringArrayInjectee
	{
		[Inject("namedCollection")] public string[] ac;
	}
}

