using System;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders.Support.Enums;

namespace SwiftSuspenders.Support.Injectees
{
	public class MixedParametersConstructorInjectee
	{
		private Clazz m_dependency;
		private Clazz m_dependency2;
		private Interface m_dependency3;

		[Inject(InjectEnum.NAMED_DEP,null,InjectEnum.NAMED_DEP_2)]
		public MixedParametersConstructorInjectee(Clazz dependency, Clazz dependency2, Interface dependency3)
		{
			m_dependency = dependency;
			m_dependency2 = dependency2;
			m_dependency3 = dependency3;
		}

		public Clazz GetDependency()
		{
			return m_dependency;
		}
		public Clazz GetDependency2()
		{
			return m_dependency2;
		}
		public Interface GetDependency3()
		{
			return m_dependency3;
		}
	}

}

