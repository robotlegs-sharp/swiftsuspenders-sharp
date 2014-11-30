using System;

namespace swiftsuspenders.support.injectees.childinjectors
{
	public class RobotFoot
	{
		public RobotToes toes;

		public RobotFoot(RobotToes toes = null)
		{
			this.toes = toes;
		}
	}
}

