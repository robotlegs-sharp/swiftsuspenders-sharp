﻿using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class OneRequiredOneOptionalPropertyMethodInjectee
	{
		private Clazz m_dependency;
		private Interface m_dependency2;

		[Inject]
		public void SetDependency(Clazz dependency, Interface dependency2 = null)
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

