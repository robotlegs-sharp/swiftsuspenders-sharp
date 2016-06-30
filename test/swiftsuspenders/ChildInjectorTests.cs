using System;
using NUnit.Framework;
using SwiftSuspenders.Support.Injectees.childinjectors;
using SwiftSuspenders.Mapping;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders.Support.Injectees;
using SwiftSuspenders.Support.providers;

namespace SwiftSuspenders
{
	[TestFixture]
	public class ChildInjectorTests
	{
		private Injector injector;

		[SetUp]
		public void RunBeforeEachTest()
		{
			injector = new Injector();
		}

		[TearDown]
		public void Teardown()
		{
			Injector.PurgeInjectionPointsCache();
			injector = null;
		}

		[Test]
		public void InjectorCreatesChildInjector()
		{
			Assert.True(true);
			Injector childInjector = injector.CreateChildInjector();
			Assert.IsInstanceOf<Injector>(childInjector, "injector.createChildInjector should return an injector");
		}

		[Test]
		public void InjectorUsesChildInjectorForSpecifiedMapping()
		{
			injector.Map(typeof(RobotFoot));

			InjectionMapping leftFootMapping = injector.Map(typeof(RobotLeg), "leftLeg");
			Injector leftChildInjector = injector.CreateChildInjector();
			leftChildInjector.Map(typeof(RobotAnkle));
			leftChildInjector.Map(typeof(RobotFoot)).ToType(typeof(LeftRobotFoot));

			leftFootMapping.SetInjector(leftChildInjector);
			InjectionMapping rightFootMapping = injector.Map(typeof(RobotLeg), "rightLeg");
			Injector rightChildInjector = injector.CreateChildInjector();
			rightChildInjector.Map(typeof(RobotAnkle));
			rightChildInjector.Map(typeof(RobotFoot)).ToType(typeof(RightRobotFoot));
			rightFootMapping.SetInjector(rightChildInjector);

			RobotBody robotBody = injector.InstantiateUnmapped<RobotBody>();

			Assert.IsInstanceOf<RightRobotFoot>(robotBody.rightLeg.ankle.foot, "Right RobotLeg should have a RightRobotFoot");
			Assert.IsInstanceOf<LeftRobotFoot>(robotBody.leftLeg.ankle.foot, "Left RobotLeg should have a LeftRobotFoot");
		}

		[Test]
		public void ChildInjectorUsesParentForMissingMappings()
		{
			injector.Map(typeof(RobotFoot));
			injector.Map(typeof(RobotToes));

			InjectionMapping leftFootMapping = injector.Map(typeof(RobotLeg), "leftLeg");
			Injector leftChildInjector = injector.CreateChildInjector();
			leftChildInjector.Map(typeof(RobotAnkle));
			leftChildInjector.Map(typeof(RobotFoot)).ToType(typeof(LeftRobotFoot));
			leftFootMapping.SetInjector(leftChildInjector);

			InjectionMapping rightFootMapping = injector.Map(typeof(RobotLeg), "rightLeg");
			Injector rightChildInjector = injector.CreateChildInjector();
			rightChildInjector.Map(typeof(RobotAnkle));
			rightChildInjector.Map(typeof(RobotFoot)).ToType(typeof(RightRobotFoot));
			rightFootMapping.SetInjector(rightChildInjector);

			RobotBody robotBody = injector.InstantiateUnmapped<RobotBody>();

			Assert.IsInstanceOf<RobotToes>(robotBody.rightLeg.ankle.foot.toes, "Right RobotFoot should have toes");
			Assert.IsInstanceOf<RobotToes>(robotBody.leftLeg.ankle.foot.toes, "Right RobotFoot should have toes");
		}

		[Test]
		public void parentMappedSingletonGetsInitializedByParentWhenInvokedThroughChildInjector()
		{
			Clazz parentClazz = new Clazz();
			injector.Map(typeof(Clazz)).ToValue(parentClazz);
			injector.Map(typeof(ClassInjectee)).AsSingleton();
			Injector childInjector = injector.CreateChildInjector();
			Clazz childClazz = new Clazz();
			childInjector.Map(typeof(Clazz)).ToValue(childClazz);

			ClassInjectee classInjectee = childInjector.GetInstance<ClassInjectee>();

			Assert.AreEqual(classInjectee.property, parentClazz, "classInjectee.property is injected with value mapped in parent injector");
		}

		[Test]
		public void childInjectorDoesntReturnToParentAfterUsingParentInjectorForMissingMappings()
		{
			injector.Map(typeof(RobotAnkle));
			injector.Map(typeof(RobotFoot));
			injector.Map(typeof(RobotToes));

			InjectionMapping leftFootMapping = injector.Map(typeof(RobotLeg), "leftLeg");
			Injector leftChildInjector = injector.CreateChildInjector();
			leftChildInjector.Map(typeof(RobotFoot)).ToType(typeof(LeftRobotFoot));
			leftFootMapping.SetInjector(leftChildInjector);

			InjectionMapping rightFootMapping = injector.Map(typeof(RobotLeg), "rightLeg");
			Injector rightChildInjector = injector.CreateChildInjector();
			rightChildInjector.Map(typeof(RobotFoot)).ToType(typeof(RightRobotFoot));
			rightFootMapping.SetInjector(rightChildInjector);

			RobotBody robotBody = injector.InstantiateUnmapped<RobotBody>();

			Assert.IsInstanceOf<RightRobotFoot>(robotBody.rightLeg.ankle.foot, "Right RobotFoot should have RightRobotFoot");
			Assert.IsInstanceOf<LeftRobotFoot>(robotBody.leftLeg.ankle.foot, "Left RobotFoot should have LeftRobotFoot");
		}

		[Test]
		public void childInjectorHasMappingWhenExistsOnParentInjector()
		{
			Injector childInjector = injector.CreateChildInjector();
			Clazz class1 = new Clazz();
			injector.Map(typeof(Clazz)).ToValue(class1);  

			Assert.True(childInjector.Satisfies(typeof(Clazz)), "Child injector should return true for satisfies that exists on parent injector");
		}

		[Test]
		public void childInjectorDoesNotHaveMappingWhenDoesNotExistOnParentInjector()
		{
			Injector childInjector = injector.CreateChildInjector();

			Assert.False(childInjector.Satisfies<Interface>(), "Child injector should not return true for satisfies that does not exists on parent injector");
		}  

		[Test]
		public void grandChildInjectorSuppliesInjectionFromAncestor()
		{
			Injector childInjector;
			Injector grandChildInjector;
			ClassInjectee injectee = new ClassInjectee();
			injector.Map(typeof(Clazz)).ToSingleton<Clazz>();
			childInjector = injector.CreateChildInjector();
			grandChildInjector = childInjector.CreateChildInjector();

			grandChildInjector.InjectInto(injectee);

			Assert.IsInstanceOf<Clazz>(injectee.property, "injectee has been injected with Clazz instance from grandChildInjector"); 
		}

		[Test]
		public void injectorCanCreateChildInjectorDuringInjection()
		{
			injector.Map(typeof(Injector)).ToProvider(new ChildInjectorCreatingProvider());
			injector.Map(typeof(InjectorInjectee));
			injector.Map (typeof(NestedInjectorInjectee));
			InjectorInjectee injectee = injector.GetInstance<InjectorInjectee>();
			Assert.NotNull(injectee.injector, "Injection has been applied to injectorInjectee");
			Assert.AreEqual(injectee.injector.parentInjector, injector, "injectorInjectee.injector is child of main injector");
			Assert.AreEqual(injectee.nestedInjectee.injector.parentInjector.parentInjector, injector, "injectorInjectee.nestedInjectee is grandchild of main injector");
		}

		[Test]
		public void satisfies_with_fallbackProvider_trickles_down_to_children()
		{
			injector.fallbackProvider = new ProviderThatCanDoInterfaces(typeof(Clazz));
			Injector childInjector = injector.CreateChildInjector();
			Assert.True(childInjector.Satisfies<Clazz>());
		}

		[Test]
		public void GetInstance_with_fallbackProvider_trickles_down_to_children()
		{
			injector.fallbackProvider = new ProviderThatCanDoInterfaces(typeof(Clazz));
			Injector childInjector = injector.CreateChildInjector();
			Assert.NotNull(childInjector.GetInstance(typeof(Clazz)));
		}
	}
}

