using System;

namespace SwiftSuspenders
{
	[AttributeUsage(AttributeTargets.Method, 
		AllowMultiple = false,
		Inherited = true)]
	public class OrderedMethodAttribute : Attribute
	{
		private int _order;

		public int order
		{
			get 
			{
				return _order;
			}
		}

		public OrderedMethodAttribute (int order = int.MaxValue)
		{
			_order = order;
		}
	}
}

