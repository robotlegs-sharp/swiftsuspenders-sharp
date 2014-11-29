using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
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

