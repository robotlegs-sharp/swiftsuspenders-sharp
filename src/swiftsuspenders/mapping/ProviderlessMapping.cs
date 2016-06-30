using System;
using SwiftSuspenders.DependencyProviders;

namespace SwiftSuspenders.Mapping
{
	public interface ProviderlessMapping
	{
		UnsealedMapping ToType(Type type);

		UnsealedMapping ToValue(object value, bool autoInject = false, bool destroyOnUnmap = false);

		UnsealedMapping ToSingleton(Type type, bool initializeImmediately = false);

		UnsealedMapping AsSingleton(bool initializeImmediately = false);

		UnsealedMapping ToProvider(DependencyProvider provider);

		object Seal();
	}
}

