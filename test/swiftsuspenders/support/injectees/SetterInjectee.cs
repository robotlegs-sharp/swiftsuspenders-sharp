using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class SetterInjectee
	{
		private Clazz m_property;

		[Inject]
		public Clazz property
		{
			set
			{
				m_property = value;
			}
			get
			{
				return m_property;
			}
		}
	}
}

