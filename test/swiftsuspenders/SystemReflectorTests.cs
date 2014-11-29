using System;
using NUnit.Framework;
using swiftsuspenders.reflector;
using swiftsuspenders;

namespace swiftsuspenders
{
	[TestFixture]
	public class SystemReflectorTests : ReflectorTests
	{
		[SetUp]
		public void setup()
		{
			reflector = new SystemReflector();
			injector = new Injector();
		}
	}
}

