using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class OptionalOneRequiredParameterMethodInjectee
	{
		private Interface m_dependency;

		[Inject(true)]
		public void SetDependency(Interface dependency)
		{
			m_dependency = dependency;
		}

		public Interface GetDependency()
		{
			return m_dependency;
		}
	}
}

