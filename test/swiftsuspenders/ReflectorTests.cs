using System;
using NUnit.Framework;
using swiftsuspenders;
using swiftsuspenders.reflector;
using swiftsuspenders.typedescriptions;
using swiftsuspenders.support.types;
using swiftsuspenders.support.injectees;
using swiftsuspenders.support.enums;

namespace swiftsuspenders
{
	public abstract class ReflectorTests
	{
		protected Reflector reflector;
		protected Injector injector;

		public ReflectorTests ()
		{

		}

//		[Test]
//		public void ReflectorReturnsNoParamsCtorInjectionPointForNoParamsCtor()
//		{
//			InjectionPoint injectionPoint = reflector.DescribeInjections(Clazz).ctor;
//			Assert.True (injectionPoint is NoParamsConstructorInjectionPoint, "reflector-returned injectionPoint is no-params ctor injectionPoint");
//		}

		[Test]
		public void ReflectorReturnsCorrectCtorInjectionPointForParamsCtor()
		{
			Assert.True (true);
			InjectionPoint injectionPoint = reflector.DescribeInjections(typeof(OneParameterConstructorInjectee)).ctor;
			Assert.True(injectionPoint is ConstructorInjectionPoint, "reflector-returned injectionPoint is ctor injectionPoint");
		}

		[Test]
		public void ReflectorReturnsCorrectCtorInjectionPointForNamedParamsCtor()
		{
			ConstructorInjectionPoint injectionPoint = reflector.DescribeInjections(typeof(OneNamedParameterConstructorInjectee)).ctor;
			Assert.True(injectionPoint is ConstructorInjectionPoint, "reflector-returned injectionPoint is ctor injectionPoint");
			injector.Map(typeof(Clazz), InjectEnum.NAMED_DEPENDENCY).ToType(typeof(Clazz));
			OneNamedParameterConstructorInjectee injectee = injectionPoint.CreateInstance(typeof(OneNamedParameterConstructorInjectee), injector) as OneNamedParameterConstructorInjectee;
			Assert.NotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for named Clazz parameter");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForUnnamedPropertyInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(InterfaceInjectee));
			Assert.IsInstanceOf<PropertyInjectionPoint> (description.injectionPoints);
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForNamedPropertyInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(NamedInterfaceInjectee));
			Assert.IsInstanceOf<PropertyInjectionPoint> (description.injectionPoints);
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOptionalPropertyInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(OptionalClassInjectee));
			OptionalClassInjectee injectee = new OptionalClassInjectee();
			description.injectionPoints.ApplyInjection(injectee, typeof(OptionalClassInjectee), injector);
			Assert.Null(injectee.property, "Instance of Clazz should not have been injected for Clazz property");
		}

		/*
		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOneParamMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(OneParameterMethodInjectee);
			OneParameterMethodInjectee injectee = new OneParameterMethodInjectee();
			injector.Map(Clazz);
			description.injectionPoints.ApplyInjection(injectee, OneParameterMethodInjectee, injector);
			Assert.NotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOneNamedParamMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(OneNamedParameterMethodInjectee);
			OneNamedParameterMethodInjectee injectee = new OneNamedParameterMethodInjectee();
			injector.Map(Clazz, InjectEnum.NAMED_DEP);
			description.injectionPoints.ApplyInjection(injectee, OneNamedParameterMethodInjectee, injector);
			Assert.NotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForTwoNamedParamsMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(TwoNamedParametersMethodInjectee);
			TwoNamedParametersMethodInjectee injectee = new TwoNamedParametersMethodInjectee();
			injector.Map(Clazz, InjectEnum.NAMED_DEP);
			injector.Map(Interface, InjectEnum.NAMED_DEP_2).ToType(Clazz);
			description.injectionPoints.ApplyInjection(injectee, TwoNamedParametersMethodInjectee, injector);
			Assert.assertNotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
			Assert.assertNotNull(injectee.GetDependency2(), "Instance of Clazz should have been injected for Interface dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOneRequiredOneOptionalParamsMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(OneRequiredOneOptionalPropertyMethodInjectee);
			OneRequiredOneOptionalPropertyMethodInjectee  injectee = new OneRequiredOneOptionalPropertyMethodInjectee();
			injector.Map(Clazz);
			description.injectionPoints.ApplyInjection(injectee, OneRequiredOneOptionalPropertyMethodInjectee, injector);
			Assert.assertNotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
			Assert.assertNull(injectee.GetDependency2(), "Instance of Clazz should not have been injected for Interface dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOptionalOneRequiredParamMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(OptionalOneRequiredParameterMethodInjectee);
			OptionalOneRequiredParameterMethodInjectee injectee = new OptionalOneRequiredParameterMethodInjectee();
			description.injectionPoints.ApplyInjection(injectee, OptionalOneRequiredParameterMethodInjectee, injector);
			Assert.assertNull(injectee.GetDependency(), "Instance of Clazz should not have been injected for Clazz dependency");
		}

		[Test]
		public void ReflectorCreatesInjectionPointsForPostConstructMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(OrderedPostConstructInjectee).injectionPoints;
			Assert.assertTrue(first && first.next && first.next.next && first.next.next.next, "Four injection points have been added");
		}

		[Test]
		public void ReflectorCorrectlySortsInjectionPointsForPostConstructMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(OrderedPostConstructInjectee).injectionPoints;
			Assert.assertEquals('First injection point has order "1"', 1,
				PostConstructInjectionPoint(first).order);
			Assert.assertEquals('Second injection point has order "2"', 2,
				PostConstructInjectionPoint(first.next).order);
			Assert.assertEquals('Third injection point has order "3"', 3,
				PostConstructInjectionPoint(first.next.next).order);
			Assert.assertEquals('Fourth injection point has no order "int.MAX_VALUE"', int.MAX_VALUE,
				PostConstructInjectionPoint(first.next.next.next).order);
		}

		[Test]
		public void ReflectorCreatesInjectionPointsForPreDestroyMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(OrderedPreDestroyInjectee).preDestroyMethods;
			Assert.True(first && first.next && first.next.next && first.next.next.next, "Four injection points have been added");
		}

		[Test]
		public void ReflectorCorrectlySortsInjectionPointsForPreDestroyMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(OrderedPreDestroyInjectee).preDestroyMethods;
			Assert.assertEquals('First injection point has order "1"', 1,
				PreDestroyInjectionPoint(first).order);
			Assert.assertEquals('Second injection point has order "2"', 2,
				PreDestroyInjectionPoint(first.next).order);
			Assert.assertEquals('Third injection point has order "3"', 3,
				PreDestroyInjectionPoint(first.next.next).order);
			Assert.assertEquals('Fourth injection point has no order "int.MAX_VALUE"', int.MAX_VALUE,
				PreDestroyInjectionPoint(first.next.next.next).order);
		}

		[Test]
		public void ReflectorStoresUnknownInjectParameters()
		{
			InjectionPoint first = reflector.DescribeInjections(UnknownInjectParametersInjectee).injectionPoints;
			assertThat(first.injectParameters, hasProperties(
				{optional:"true",name:'test',param1:"true",param2:'str',param3:"123"}));
		}

		[Test]
		public void ReflectorStoresUnknownInjectParametersListAsCSV()
		{
			InjectionPoint first = reflector.describeInjections(UnknownInjectParametersListInjectee).injectionPoints;
			assertThat(first.injectParameters, hasProperties({param:"true,str,123"}));
		}

		[Test]
		public void ReflectorFindsPostConstructMethodVars()
		{
			const first : PostConstructInjectionPoint = PostConstructInjectionPoint(
				reflector.describeInjections(PostConstructVarInjectee).injectionPoints);
			assertThat(first, notNullValue());
		}

		[Test]
		public void ReflectorFindsPostConstructMethodGetters()
		{
			PostConstructInjectionPoint first = PostConstructInjectionPoint(reflector.DescribeInjections(PostConstructGetterInjectee).injectionPoints);
			assertThat(first, notNullValue());
		}
		//*/
	}
}

