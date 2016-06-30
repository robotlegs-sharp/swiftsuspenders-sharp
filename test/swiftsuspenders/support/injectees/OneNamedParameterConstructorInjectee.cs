using System;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders.Support.Enums;

namespace SwiftSuspenders.Support.Injectees
{
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

