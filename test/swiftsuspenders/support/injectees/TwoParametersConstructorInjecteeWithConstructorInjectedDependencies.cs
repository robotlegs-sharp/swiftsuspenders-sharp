using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class TwoParametersConstructorInjecteeWithConstructorInjectedDependencies
	{
		private OneParameterConstructorInjectee m_dependency1;

		private TwoParametersConstructorInjectee m_dependency2;

		[Inject]
		public TwoParametersConstructorInjecteeWithConstructorInjectedDependencies(OneParameterConstructorInjectee dependency1, TwoParametersConstructorInjectee dependency2)
		{
			m_dependency1 = dependency1;
			m_dependency2 = dependency2;
		}

		public OneParameterConstructorInjectee GetDependency1()
		{
			return m_dependency1;
		}

		public TwoParametersConstructorInjectee GetDependency2()
		{
			return m_dependency2;
		}
	}
}

