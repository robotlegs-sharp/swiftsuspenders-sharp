using System;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders.Support.Enums;

namespace SwiftSuspenders.Support.Injectees
{
	public class TwoNamedParametersConstructorInjectee
	{
		private Clazz m_dependency;
		private string m_dependency2;

		[Inject(InjectEnum.NAMED_DEPENDENCY, InjectEnum.NAMED_DEPENDENCY_2)]
		public TwoNamedParametersConstructorInjectee(Clazz dependency, string dependency2)
		{
			m_dependency = dependency;
			m_dependency2 = dependency2;
		}

		public Clazz GetDependency()
		{
			return m_dependency;
		}

		public string GetDependency2()
		{
			return m_dependency2;
		}
	}
}

