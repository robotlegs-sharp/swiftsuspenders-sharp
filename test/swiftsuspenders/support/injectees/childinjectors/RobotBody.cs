using System;

namespace swiftsuspenders.support.injectees.childinjectors
{
	public class RobotBody
	{
		[Inject("leftLeg")]
		public RobotLeg leftLeg;

		[Inject("rightLeg")]
		public RobotLeg rightLeg;
	}
}

