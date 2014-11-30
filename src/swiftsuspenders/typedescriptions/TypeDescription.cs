using System;
using swiftsuspenders.errors;
using System.Reflection;
using swiftsuspenders.mapping;

namespace swiftsuspenders.typedescriptions
{
	public class TypeDescription
	{
		public ConstructorInjectionPoint ctor;
		public InjectionPoint injectionPoints;
		public PreDestroyInjectionPoint preDestroyMethods;
		private bool _postConstructAdded;

		public TypeDescription ()
		{
		}

		public TypeDescription SetConstructor(ConstructorInjectionPoint constructorInjectionPoint)
		{
			ctor = constructorInjectionPoint;
			return this;
		}

		public TypeDescription AddFieldInjection(MappingId mappingId, FieldInfo fieldInfo, bool optional = false)
		{
			if (_postConstructAdded)
				throw new InjectorException ("Can't add injection point after post construct method");
			AddInjectionPoint (new FieldInjectionPoint(mappingId, fieldInfo, optional));
			return this;
		}

		public TypeDescription AddPropertyInjection(MappingId mappingId, PropertyInfo propertyInfo, bool optional = false)
		{
			if (_postConstructAdded)
				throw new InjectorException ("Can't add injection point after post construct method");
			AddInjectionPoint (new PropertyInjectionPoint(mappingId, propertyInfo, optional));
			return this;
		}

		public TypeDescription AddMethodInjection(MethodInjectionPoint methodInjectionPoint)
		{
			if (_postConstructAdded)
				throw new InjectorException ("Can't add injection point after post construct method");
			AddInjectionPoint (methodInjectionPoint);
			return this;
		}

		public TypeDescription AddPostConstructMethod(PostConstructInjectionPoint postConstructInjectionPoint)
		{
			_postConstructAdded = true;
			AddInjectionPoint (postConstructInjectionPoint);
			return this;
		}

		public TypeDescription AddPreDestroyMethod(PreDestroyInjectionPoint preDestroyMethod)
		{
			if (preDestroyMethods != null)
			{
				preDestroyMethods.last.next = preDestroyMethod;
				preDestroyMethods.last = preDestroyMethod;
			}
			else
			{
				preDestroyMethods = preDestroyMethod;
				preDestroyMethods.last = preDestroyMethod;
			}
			return this;
		}

		public void AddInjectionPoint(InjectionPoint injectionPoint)
		{
			if (injectionPoints != null)
			{
				injectionPoints.last.next = injectionPoint;
				injectionPoints.last = injectionPoint;
			}
			else
			{
				injectionPoints = injectionPoint;
				injectionPoints.last = injectionPoint;
			}
		}
	}
}

