using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class OneParameterMethodInjectee
	{
		private Clazz m_dependency;

		[Inject]
		public void SetDependency(Clazz dependency)
		{
			m_dependency = dependency;
		}

		public Clazz GetDependency()
		{
			return m_dependency;
		}
	}
}

