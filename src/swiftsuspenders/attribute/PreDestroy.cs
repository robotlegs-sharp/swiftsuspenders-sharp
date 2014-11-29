using System;
using swiftsuspenders;

public class PreDestroy: OrderedMethodAttribute
{
	public PreDestroy () : base()
	{
	}

	public PreDestroy (int order) : base (order)
	{
	}
}