using Microsoft.Extensions.Options;

namespace Data.Models.Options 
{
	public class JWTOptions
	{
		public JWTOptions(string secret)
		{
			this.Secret = secret;
		}

		public string Secret { get; set; }
	}
}
