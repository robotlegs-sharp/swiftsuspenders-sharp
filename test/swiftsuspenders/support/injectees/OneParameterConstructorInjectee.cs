using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class OneParameterConstructorInjectee
	{
		private Clazz m_dependency;

		public OneParameterConstructorInjectee (Clazz dependency)
		{
			m_dependency = dependency;
		}

		public Clazz GetDependency()
		{
			return m_dependency;
		}
	}
}

