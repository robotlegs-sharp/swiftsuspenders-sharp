using System;

namespace SwiftSuspenders.Support.Injectees.childinjectors
{
	public class InjectorInjectee
	{
		[Inject] public Injector injector;
		public NestedInjectorInjectee nestedInjectee;

		[PostConstruct]
		public void CreateAnotherChildInjector()
		{
			nestedInjectee = injector.GetInstance<NestedInjectorInjectee>();
		}
	}
}

