using System;
using SwiftSuspenders;

public class PreDestroy: OrderedMethodAttribute
{
	public PreDestroy () : base()
	{
	}

	public PreDestroy (int order) : base (order)
	{
	}
}