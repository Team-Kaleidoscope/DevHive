using Microsoft.Extensions.Options;

namespace DevHive.Services.Options
{
	public class JWTOptions
	{
		public JWTOptions(string secret)
		{
			this.Secret = secret;
		}

		public string Secret { get; init; }
	}
}
