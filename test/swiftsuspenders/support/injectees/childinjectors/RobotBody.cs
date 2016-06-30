using System;

namespace SwiftSuspenders.Support.Injectees.childinjectors
{
	public class RobotBody
	{
		[Inject("leftLeg")]
		public RobotLeg leftLeg;

		[Inject("rightLeg")]
		public RobotLeg rightLeg;
	}
}

