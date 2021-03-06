﻿using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class OneParameterMethodInjectee
	{
		private Clazz m_dependency;

		[Inject]
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

