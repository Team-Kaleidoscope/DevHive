using Microsoft.Extensions.Options;

namespace DevHive.Services.Options
{
	public class JwtOptions
	{
		public JwtOptions(string secret)
		{
			this.Secret = secret;
		}

		public string Secret { get; init; }
	}
}
