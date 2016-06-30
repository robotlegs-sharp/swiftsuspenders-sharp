using System;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders.Support.Enums;

namespace SwiftSuspenders.Support.Injectees
{
	public class TwoNamedParametersMethodInjectee
	{
		private Clazz m_dependency;
		private Interface m_dependency2;

		[Inject(InjectEnum.NAMED_DEP, InjectEnum.NAMED_DEP_2)]
		public void SetDependency(Clazz dependency, Interface dependency2)
		{
			m_dependency = dependency;
			m_dependency2 = dependency2;
		}

		public Clazz GetDependency()
		{
			return m_dependency;
		}

		public Interface GetDependency2()
		{
			return m_dependency2;
		}
	}
}

