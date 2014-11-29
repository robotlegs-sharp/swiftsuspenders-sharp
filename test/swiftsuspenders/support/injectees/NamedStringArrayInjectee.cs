using System;

namespace swiftsuspenders.support.injectees
{
	public class NamedStringArrayInjectee
	{
		[Inject("namedCollection")] public string[] ac;
	}
}

