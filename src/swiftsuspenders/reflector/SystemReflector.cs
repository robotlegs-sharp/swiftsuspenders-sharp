using System;
using swiftsuspenders.typedescriptions;
using System.Reflection;
using System.Collections.Generic;
using swiftsuspenders.errors;

namespace swiftsuspenders.reflector
{
	public class SystemReflector : Reflector
	{
		private static readonly Type INJECT_ATTRIBUTE_TYPE = typeof(Inject);
		private static readonly Type POST_CONSTRUCT_ATTRIBUTE_TYPE = typeof(PostConstruct);
		private static readonly Type PRE_DESTROY_ATTRIBUTE_TYPE = typeof(PreDestroy);

		public TypeDescription DescribeInjections (Type type)
		{
//			TypeDescription description = new TypeDescription(false);
			TypeDescription description = new TypeDescription();
//			description.ctor = new ConstructorInjectionPoint();
			AddConstructorInjectionPoint (description, type);
			AddPropertyInjectionPoints (description, type);
			AddFieldInjectionPoints (description, type);
			AddMethodInjectionPoints (description, type);
			AddPostConstructMethodPoints (description, type);
			AddPreDestroyMethodPoints (description, type);
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
				object[] injections = constructor.GetCustomAttributes (INJECT_ATTRIBUTE_TYPE, true);
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
			if (constructorToInject != null)
				description.ctor = new ConstructorInjectionPoint (constructorToInject, keys);
		}

		private void AddPropertyInjectionPoints(TypeDescription description, Type type)
		{
			PropertyInfo[] properties = type.GetProperties ();
			foreach (PropertyInfo property in properties) 
			{
				object[] injections = property.GetCustomAttributes (INJECT_ATTRIBUTE_TYPE, true);
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
				object[] injections = field.GetCustomAttributes(INJECT_ATTRIBUTE_TYPE, true);
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
				object[] injections = method.GetCustomAttributes (INJECT_ATTRIBUTE_TYPE, true);
				if (injections.Length == 0)
					continue;

				Inject attr = injections [0] as Inject;

				MethodInjectionPoint injectionPoint = new MethodInjectionPoint (method, attr.names, attr.optional);
				description.AddInjectionPoint (injectionPoint);
			}
		}

		private void AddPostConstructMethodPoints(TypeDescription description, Type type)
		{
			MethodInfo[] methods = type.GetMethods ();

			List<OrderedInjectionPoint> orderedInjectionPoints = new List<OrderedInjectionPoint> ();
			foreach (MethodInfo method in methods) 
			{
				object[] injections = method.GetCustomAttributes (POST_CONSTRUCT_ATTRIBUTE_TYPE, true);
				if (injections.Length == 0)
					continue;

				PostConstruct attr = injections [0] as PostConstruct;

				PostConstructInjectionPoint injectionPoint = new PostConstructInjectionPoint (attr.order, method);
				orderedInjectionPoints.Add (injectionPoint);
			}

			orderedInjectionPoints.Sort (SortInjectionPoints);
			foreach (OrderedInjectionPoint point in orderedInjectionPoints) 
				description.AddInjectionPoint (point);
		}

		private void AddPreDestroyMethodPoints(TypeDescription description, Type type)
		{
			MethodInfo[] methods = type.GetMethods ();

			List<OrderedInjectionPoint> orderedInjectionPoints = new List<OrderedInjectionPoint> ();
			foreach (MethodInfo method in methods) 
			{
				object[] injections = method.GetCustomAttributes (PRE_DESTROY_ATTRIBUTE_TYPE, true);
				if (injections.Length == 0)
					continue;

				PreDestroy attr = injections [0] as PreDestroy;

				PreDestroyInjectionPoint injectionPoint = new PreDestroyInjectionPoint (attr.order, method);
				orderedInjectionPoints.Add (injectionPoint);
			}

			orderedInjectionPoints.Sort (SortInjectionPoints);
			foreach (PreDestroyInjectionPoint point in orderedInjectionPoints) 
				description.AddPreDestroyMethod (point);
		}

		private static int SortInjectionPoints(OrderedInjectionPoint a, OrderedInjectionPoint b)
		{
			return a.order > b.order ? 1 : a.order < b.order ? -1 : 0;
		}
	}
}

