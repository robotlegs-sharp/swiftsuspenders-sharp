using System;
using swiftsuspenders.support.types;
using swiftsuspenders.support.enums;

namespace swiftsuspenders.support.injectees
{
	//[Inject(InjectEnum.NAMED_DEPENDENCY)]
	public class OneNamedParameterConstructorInjectee
	{
		private Clazz m_dependency;

		[Inject(InjectEnum.NAMED_DEPENDENCY)]
		public OneNamedParameterConstructorInjectee (Clazz dependency)
		{
			m_dependency = dependency;
		}

		public Clazz GetDependency()
		{
			return m_dependency;
		}
	}
}

