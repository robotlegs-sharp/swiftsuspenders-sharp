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
			description.ctor = new ConstructorInjectionPoint();
			AddPropertyInjectionPoints (description, type);
			AddFieldInjectionPoints (description, type);
			return description;
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
				PropertyInjectionPoint injectionPoint = new PropertyInjectionPoint(mappingId,
					property, false);// injectParameters.optional == 'true', injectParameters);
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
					field, false);// injectParameters.optional == 'true', injectParameters);
				description.AddInjectionPoint(injectionPoint);
			}
		}
	}
}

