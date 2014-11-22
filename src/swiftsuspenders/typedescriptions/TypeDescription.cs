using System;

namespace swiftsuspenders.typedescriptions
{
	public class TypeDescription
	{
		public ConstructorInjectionPoint ctor;
		public InjectionPoint injectionPoints;
		public PreDestroyInjectionPoint preDestroyMethods;

		private bool _postConstructAdded;

		public TypeDescription ()
		{
		}

		public void AddInjectionPoint(InjectionPoint injectionPoint)
		{
			if (injectionPoints != null)
			{
				injectionPoints.last.next = injectionPoint;
				injectionPoints.last = injectionPoint;
			}
			else
			{
				injectionPoints = injectionPoint;
				injectionPoints.last = injectionPoint;
			}
		}
	}
}

