using System;
using System.Collections.Generic;

namespace swiftsuspenders.support.injectees
{
	public class OrderedPostConstructInjectee
	{
		public List<int> loadOrder = new List<int>();

		[PostConstruct(2)]
		public void MethodTwo()
		{
			loadOrder.Add(2);
		}

		[PostConstruct]
		public void methodFour()
		{
			loadOrder.Add(4);
		}

		[PostConstruct(3)]
		public void MethodThree()
		{
			loadOrder.Add(3);
		}

		[PostConstruct(1)]
		public void MethodOne()
		{
			loadOrder.Add(1);
		}
	}
}

