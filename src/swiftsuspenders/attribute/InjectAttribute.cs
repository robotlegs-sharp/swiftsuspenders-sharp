using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Method,
	AllowMultiple = false,
	Inherited = true)]
public class Inject: Attribute
{
	private object[] _names;

	private bool _optional;

	public object[] names 
	{
		get 
		{
			return _names;
		}
	}

	public object name 
	{
		get 
		{
			if (names == null || names.Length == 0)
				return null;
			return names[0];
		}
	}

	public bool optional
	{
		get 
		{
			return _optional;
		}
	}
	public Inject(){}

	public Inject (bool optional)
	{
		_optional = optional;
	}

	public Inject (bool optional, params object[] names)
	{
		_optional = optional;
		_names = names;
	}

	public Inject (bool optional, Dictionary<string, object> extraParams, params object[] names)
	{
		_optional = optional;
		_names = names;
	}

	public Inject(params object[] names)
	{
		_names = names;
	}
}