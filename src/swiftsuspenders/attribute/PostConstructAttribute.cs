using System;
using swiftsuspenders;

public class PostConstruct: OrderedMethodAttribute
{
	public PostConstruct () : base()
	{
	}

	public PostConstruct (int order) : base (order)
	{
	}
}