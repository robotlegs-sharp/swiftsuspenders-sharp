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
			Assert.NotNull (description.injectionPoints, "Reflector should have found an injection point");
			description.injectionPoints.ApplyInjection(injectee, typeof(OptionalClassInjectee), injector);
			Assert.Null(injectee.property, "Instance of Clazz should not have been injected for Clazz property");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOneParamMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(OneParameterMethodInjectee));
			OneParameterMethodInjectee injectee = new OneParameterMethodInjectee();
			injector.Map (typeof(Clazz));
			Assert.NotNull (description.injectionPoints, "Reflector should have found an injection point");
			description.injectionPoints.ApplyInjection(injectee, typeof(OneParameterMethodInjectee), injector);
			Assert.NotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOneNamedParamMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(OneNamedParameterMethodInjectee));
			OneNamedParameterMethodInjectee injectee = new OneNamedParameterMethodInjectee();
			injector.Map(typeof(Clazz), InjectEnum.NAMED_DEP);
			description.injectionPoints.ApplyInjection(injectee, typeof(OneNamedParameterMethodInjectee), injector);
			Assert.NotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForTwoNamedParamsMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(TwoNamedParametersMethodInjectee));
			TwoNamedParametersMethodInjectee injectee = new TwoNamedParametersMethodInjectee();
			injector.Map(typeof(Clazz), InjectEnum.NAMED_DEP);
			injector.Map(typeof(Interface), InjectEnum.NAMED_DEP_2).ToType(typeof(Clazz));
			description.injectionPoints.ApplyInjection(injectee, typeof(TwoNamedParametersMethodInjectee), injector);
			Assert.NotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
			Assert.NotNull(injectee.GetDependency2(), "Instance of Clazz should have been injected for Interface dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOneRequiredOneOptionalParamsMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(OneRequiredOneOptionalPropertyMethodInjectee));
			OneRequiredOneOptionalPropertyMethodInjectee  injectee = new OneRequiredOneOptionalPropertyMethodInjectee();
			injector.Map(typeof(Clazz));
			description.injectionPoints.ApplyInjection(injectee, typeof(OneRequiredOneOptionalPropertyMethodInjectee), injector);
			Assert.NotNull(injectee.GetDependency(), "Instance of Clazz should have been injected for Clazz dependency");
			Assert.Null(injectee.GetDependency2(), "Instance of Clazz should not have been injected for Interface dependency");
		}

		[Test]
		public void ReflectorCorrectlyCreatesInjectionPointForOptionalOneRequiredParamMethodInjection()
		{
			TypeDescription description = reflector.DescribeInjections(typeof(OptionalOneRequiredParameterMethodInjectee));
			OptionalOneRequiredParameterMethodInjectee injectee = new OptionalOneRequiredParameterMethodInjectee();
			description.injectionPoints.ApplyInjection(injectee, typeof(OptionalOneRequiredParameterMethodInjectee), injector);
			Assert.Null(injectee.GetDependency(), "Instance of Interface should not have been injected for Interface dependency");
		}

		[Test]
		public void ReflectorCreatesInjectionPointsForPostConstructMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(typeof(OrderedPostConstructInjectee)).injectionPoints;
			Assert.True(first != null && first.next != null  && first.next.next != null  && first.next.next.next != null , "Four injection points have been added");
		}

		[Test]
		public void ReflectorCorrectlySortsInjectionPointsForPostConstructMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(typeof(OrderedPostConstructInjectee)).injectionPoints;
			Assert.AreEqual(1, (first as PostConstructInjectionPoint).order, "First injection point has order '1'");
			Assert.AreEqual(2, (first.next as PostConstructInjectionPoint).order, "Second injection point has order '2'");
			Assert.AreEqual(3, (first.next.next as PostConstructInjectionPoint).order, "Third injection point has order '3'");
			Assert.AreEqual(int.MaxValue, (first.next.next.next as PostConstructInjectionPoint).order, "Fourth injection point has no order 'int.MaxValue'");
		}

		[Test]
		public void ReflectorCreatesInjectionPointsForPreDestroyMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(typeof(OrderedPreDestroyInjectee)).preDestroyMethods;
			Assert.True(first != null && first.next != null  && first.next.next != null  && first.next.next.next != null , "Four injection points have been added");
		}

		[Test]
		public void ReflectorCorrectlySortsInjectionPointsForPreDestroyMethods()
		{
			InjectionPoint first = reflector.DescribeInjections(typeof(OrderedPreDestroyInjectee)).preDestroyMethods;
			Assert.AreEqual(1, (first as PreDestroyInjectionPoint).order, "First injection point has order '1'");
			Assert.AreEqual(2, (first.next as PreDestroyInjectionPoint).order, "Second injection point has order '2'");
			Assert.AreEqual(3, (first.next.next as PreDestroyInjectionPoint).order, "Third injection point has order '3'");
			Assert.AreEqual(int.MaxValue, (first.next.next.next as PreDestroyInjectionPoint).order, "Fourth injection point has no order 'int.MaxValue'");
		}

		// Hmm, not sure if I want/can implement these features easily
		/*
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

