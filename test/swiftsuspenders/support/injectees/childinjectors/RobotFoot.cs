using System;

namespace SwiftSuspenders.Support.Injectees.childinjectors
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

