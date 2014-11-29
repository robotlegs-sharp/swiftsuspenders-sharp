using System;
using swiftsuspenders.support.types;
using swiftsuspenders.support.enums;

namespace swiftsuspenders.support.injectees
{
	public class OneNamedParameterMethodInjectee
	{
		private Clazz m_dependency;

		[Inject(InjectEnum.NAMED_DEP)]
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

