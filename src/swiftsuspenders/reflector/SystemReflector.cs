using System;
using swiftsuspenders.typedescriptions;
using System.Reflection;

namespace swiftsuspenders.reflector
{
	public class SystemReflector : Reflector
	{
		private static Type _injectAttributeType;

		public SystemReflector ()
		{
			if (_injectAttributeType == null)
				_injectAttributeType = typeof(Inject);
		}

		public TypeDescription DescribeInjections (Type type)
		{
//			TypeDescription description = new TypeDescription(false);
			TypeDescription description = new TypeDescription();
//			description.ctor = new ConstructorInjectionPoint();
			AddConstructorInjectionPoint (description, type);
			AddPropertyInjectionPoints (description, type);
			AddFieldInjectionPoints (description, type);
			return description;
		}

		private void AddConstructorInjectionPoint(TypeDescription description, Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			ConstructorInfo constructorToInject = null;
			object[] keys = null;
			int maxParameters = -1;
			foreach (ConstructorInfo constructor in constructors)
			{
				object[] injections = constructor.GetCustomAttributes (_injectAttributeType, true);
				if (injections.Length > 0) 
				{
					Console.WriteLine ("Injections: " + injections.Length);
					Inject inject = injections[0] as Inject;
					keys = inject.names;
					constructorToInject = constructor;
					break;
				}

				if (constructor.GetParameters ().Length > maxParameters)
					constructorToInject = constructor;
			}

			description.ctor = new ConstructorInjectionPoint (constructorToInject, keys);
		}

		private void AddPropertyInjectionPoints(TypeDescription description, Type type)
		{
			PropertyInfo[] properties = type.GetProperties ();
			foreach (PropertyInfo property in properties) 
			{
				object[] injections = property.GetCustomAttributes (_injectAttributeType, true);
				if (injections.Length == 0)
					continue;

				Inject attr = injections [0] as Inject;
				object key = attr.name;
				object mappingId = key == null ? property.PropertyType as object : key as object;
				PropertyInjectionPoint injectionPoint = new PropertyInjectionPoint (mappingId, property, attr.optional); //injectParameters);
				description.AddInjectionPoint(injectionPoint);
			}
		}

		private void AddFieldInjectionPoints(TypeDescription description, Type type)
		{
			FieldInfo[] fields = type.GetFields ();
			foreach (FieldInfo field in fields) 
			{
				object[] injections = field.GetCustomAttributes(_injectAttributeType, true);
				if (injections.Length == 0)
					continue;

				Inject attr = injections [0] as Inject;
				object key = attr.name;
				object mappingId = key == null ? field.FieldType as object : key as object;
				FieldInjectionPoint injectionPoint = new FieldInjectionPoint (mappingId,
					field, attr.optional);// injectParameters.optional == 'true', injectParameters);
				description.AddInjectionPoint(injectionPoint);
			}
		}

		private void AddMethodInjectionPoints(TypeDescription description, Type type)
		{
			MethodInfo[] methods = type.GetMethods ();
			foreach (MethodInfo method in methods) 
			{
				object[] injections = method.GetCustomAttributes (_injectAttributeType, true);
				if (injections.Length == 0)
					continue;

				Inject attr = injections [0] as Inject;

				MethodInjectionPoint injectionPoint = new MethodInjectionPoint (method, attr.optional);
				description.AddInjectionPoint (injectionPoint);
			}
		}
	}
}

