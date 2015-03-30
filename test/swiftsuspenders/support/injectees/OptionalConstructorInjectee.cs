using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class OptionalConstructorInjectee
	{
		public int param1;

		public string param2;

		[Inject(true)]
		public OptionalConstructorInjectee (int param1, string param2)
		{
			this.param1 = param1;
			this.param2 = param2;
		}
	}
}

