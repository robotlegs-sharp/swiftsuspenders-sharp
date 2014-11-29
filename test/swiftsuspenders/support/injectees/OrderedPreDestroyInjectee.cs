using System;
using System.Collections.Generic;

namespace swiftsuspenderssharptest
{
	public class OrderedPreDestroyInjectee
	{
		public List<int> loadOrder = new List<int>();

		[PreDestroy(2)]
		public void MethodTwo()
		{
			loadOrder.Add(2);
		}

		[PreDestroy]
		public void methodFour()
		{
			loadOrder.Add(4);
		}

		[PreDestroy(3)]
		public void MethodThree()
		{
			loadOrder.Add(3);
		}

		[PreDestroy(1)]
		public void MethodOne()
		{
			loadOrder.Add(1);
		}
	}
}

