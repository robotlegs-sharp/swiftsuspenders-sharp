using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class OptionalOneRequiredParameterMethodInjectee
	{
		private Interface m_dependency;

		[Inject(true)]
		public void SetDependency(Interface dependency)
		{
			m_dependency = dependency;
		}

		public Interface GetDependency()
		{
			return m_dependency;
		}
	}
}

