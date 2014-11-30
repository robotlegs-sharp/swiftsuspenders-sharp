using System;
using NUnit.Framework;
using swiftsuspenders.mapping;
using swiftsuspenders.support.types;
using swiftsuspenders.dependencyproviders;
using System.Reflection;
using System.Collections.Generic;
using swiftsuspenders.errors;

namespace swiftsuspenders
{
	[TestFixture]
	public class InjectionMappingTests
	{
		private Injector injector;

		public InjectionMappingTests ()
		{
		}

		[SetUp]
		public void setup()
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
		public void ConfigIsInstantiated()
		{
			InjectionMapping config = new InjectionMapping (injector, new MappingId(typeof(Clazz), null));
			Assert.IsInstanceOf<InjectionMapping>(config);
		}

		[Test]
		public void MappingWithoutProviderEverSetUsesClassProvider()
		{
			InjectionMapping config = new InjectionMapping(injector, new MappingId(typeof(Clazz), null));
			Assert.IsInstanceOf<TypeProvider>(config.GetProvider());
		}

		[Test]
		public void InjectionMappingAsSingletonMethodCreatesSingletonProvider()
		{
			InjectionMapping config = new InjectionMapping(injector, new MappingId(typeof(Clazz), null));
			config.AsSingleton();
			Assert.IsInstanceOf<SingletonProvider>(config.GetProvider());
		}

		[Test]
		public void SameNamedSingletonIsReturnedOnSecondResponse()
		{
			SingletonProvider provider = new SingletonProvider(typeof(Clazz), injector);
			object returnedResponse = provider.Apply(null, injector, null);
			object secondResponse = provider.Apply(null, injector, null);
			Assert.AreEqual( returnedResponse, secondResponse );
		}

		[Test]
		public void SetProviderChangesTheProvider()
		{
			SingletonProvider provider1 = new SingletonProvider(typeof(Clazz), injector);
			TypeProvider provider2 = new TypeProvider(typeof(Clazz));
			InjectionMapping config = new InjectionMapping(injector, new MappingId(typeof(Clazz), null));
			config.ToProvider(provider1);
			Assert.AreEqual (config.GetProvider (), provider1);
			config.ToProvider(null);
			config.ToProvider(provider2);
			Assert.AreEqual (config.GetProvider (), provider2);
		}

		[Test]
		public void SealingAMappingMakesItSealed()
		{
			InjectionMapping config = new InjectionMapping(injector, new MappingId(typeof(Interface), null));
			config.Seal();
			Assert.True(config.isSealed);
		}

		[Test]
		public void SealingAMappingMakesItUnchangable()
		{
			InjectionMapping config = new InjectionMapping(injector, new MappingId(typeof(Interface), null));
			config.Seal();
			List<MethodInfo> testMethods = new List<MethodInfo> ();
			List<object[]> testMethodArguments = new List<object[]> ();

			testMethods.Add(config.GetType ().GetMethod ("AsSingleton"));
			testMethodArguments.Add (new object[1]{false});

			testMethods.Add(config.GetType ().GetMethod ("ToSingleton", new Type[]{typeof(Type), typeof(bool)}));
			testMethodArguments.Add (new object[2]{typeof(Clazz), false});

			testMethods.Add(config.GetType ().GetMethod ("ToType", new Type[]{typeof(Type)}));
			testMethodArguments.Add (new object[1]{typeof(Clazz)});

			testMethods.Add(config.GetType ().GetMethod ("ToValue"));
			testMethodArguments.Add (new object[3]{typeof(Clazz), false, false});

			testMethods.Add(config.GetType ().GetMethod ("ToProvider"));
			testMethodArguments.Add (new object[1]{null});

			testMethods.Add(config.GetType ().GetMethod ("Locally"));
			testMethodArguments.Add (null);

			testMethods.Add(config.GetType ().GetMethod ("Softly"));
			testMethodArguments.Add (null);

			List<MethodInfo> injectionExeptionMethods = new List<MethodInfo> ();

			for (int i = 0; i < testMethods.Count; i++) 
			{
				MethodInfo methodInfo = testMethods [i];
				try
				{
					methodInfo.Invoke (config, testMethodArguments [i]);
				}
				catch (Exception exception) 
				{
					if (exception.InnerException != null && exception.InnerException is InjectorException)
						injectionExeptionMethods.Add (methodInfo);
					else 
					{
						throw exception;
					}
				}
			}

			Assert.AreEqual (testMethods.Count, injectionExeptionMethods.Count);
			for (int i = 0; i < testMethods.Count; i++) 
			{
				Assert.AreEqual (testMethods [i], injectionExeptionMethods [i]);
			}
		}

		[Test, ExpectedException(typeof(InjectorException))]
		public void UnmappingASealedMappingThrows()
		{
			injector.Map(typeof(Interface)).Seal();
			injector.Unmap(typeof(Interface));
		}

		[Test, ExpectedException(typeof(InjectorException))]
		public void DoubleSealingAMappingThrows()
		{
			injector.Map(typeof(Interface)).Seal();
			injector.Map(typeof(Interface)).Seal();
		}

		[Test]
		public void SealReturnsAnUnsealingKeyObject()
		{
			InjectionMapping config = new InjectionMapping(injector, new MappingId(typeof(Interface), null));
			Assert.NotNull(config.Seal ());
		}

		[Test, ExpectedException(typeof(InjectorException))]
		public void UnsealingAMappingWithoutKeyThrows()
		{
			injector.Map(typeof(Interface)).Seal();
			injector.Map(typeof(Interface)).Unseal(null);
		}

		[Test, ExpectedException(typeof(InjectorException))]
		public void UnsealingAMappingWithWrongKeyThrows()
		{
			injector.Map(typeof(Interface)).Seal();
			injector.Map(typeof(Interface)).Unseal(new object());
		}

		[Test]
		public void UnsealingAMappingWithRightKeyMakesItChangable()
		{
			object key = injector.Map(typeof(Interface)).Seal();
			injector.Map(typeof(Interface)).Unseal(key);
			injector.Map(typeof(Interface)).Locally();
		}

		[Test]
		public void ValueMappingSupportsNullValue()
		{
			injector.Map(typeof(Interface)).ToValue(null);
			object instance = injector.GetInstance(typeof(Interface));
			Assert.Null (instance);
		}
	}
}

