using System;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders.Support.Enums;

namespace SwiftSuspenders.Support.Injectees
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

