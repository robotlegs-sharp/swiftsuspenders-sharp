using System;
using NUnit.Framework;
using SwiftSuspenders.Reflector;
using SwiftSuspenders;

namespace SwiftSuspenders
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

