using System;
using SwiftSuspenders.TypeDescriptions;
using System.Reflection;
using System.Collections.Generic;
using SwiftSuspenders.Errors;
using SwiftSuspenders.Mapping;

namespace SwiftSuspenders.Reflector
{
	public class SystemReflector : Reflector
	{
		private static readonly Type INJECT_ATTRIBUTE_TYPE = typeof(Inject);
		private static readonly Type POST_CONSTRUCT_ATTRIBUTE_TYPE = typeof(PostConstruct);
		private static readonly Type PRE_DESTROY_ATTRIBUTE_TYPE = typeof(PreDestroy);

		public TypeDescription DescribeInjections (Type type)
		{
			TypeDescription description = new TypeDescription();
			AddConstructorInjectionPoint (description, type);
			AddPropertyInjectionPoints (description, type);
			AddFieldInjectionPoints (description, type);
			MethodInfo[] methods = type.GetMethods ();
			AddMethodInjectionPoints (description, methods);
			AddPostConstructMethodPoints (description, methods);
			AddPreDestroyMethodPoints (description, methods);
			return description;
		}

		private void AddConstructorInjectionPoint(TypeDescription description, Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			ConstructorInfo constructorToInject = null;
			object[] keys = null;
			int maxParameters = -1;
			bool optional = false;
			foreach (ConstructorInfo constructor in constructors)
			{
				object[] injections = constructor.GetCustomAttributes (INJECT_ATTRIBUTE_TYPE, true);
				if (injections.Length > 0) 
				{
					Inject inject = injections[0] as Inject;
					keys = inject.names;
					optional = true;
					constructorToInject = constructor;
					break;
				}

				if (constructor.GetParameters ().Length > maxParameters)
				{
					constructorToInject = constructor;
					maxParameters = constructor.GetParameters ().Length;
				}
			}
			if (constructorToInject != null)
				description.ctor = new ConstructorInjectionPoint (constructorToInject, keys, optional);
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
				MappingId mappingId = new MappingId (property.PropertyType, key);
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
				MappingId mappingId = new MappingId (field.FieldType, key);
				FieldInjectionPoint injectionPoint = new FieldInjectionPoint (mappingId,
					field, attr.optional);// injectParameters.optional == 'true', injectParameters);
				description.AddInjectionPoint(injectionPoint);
			}
		}

		private void AddMethodInjectionPoints(TypeDescription description, MethodInfo[] methods)
		{
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

		private void AddPostConstructMethodPoints(TypeDescription description, MethodInfo[] methods)
		{
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

		private void AddPreDestroyMethodPoints(TypeDescription description, MethodInfo[] methods)
		{
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

