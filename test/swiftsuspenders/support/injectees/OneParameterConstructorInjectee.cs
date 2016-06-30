using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
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

