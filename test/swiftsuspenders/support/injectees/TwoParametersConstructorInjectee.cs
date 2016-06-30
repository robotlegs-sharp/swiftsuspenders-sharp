using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class TwoParametersConstructorInjectee
	{
		private Clazz m_dependency;

		private string m_dependency2;

		[Inject]
		public TwoParametersConstructorInjectee(Clazz dependency, string dependency2)
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

