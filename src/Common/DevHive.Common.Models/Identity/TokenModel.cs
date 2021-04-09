namespace DevHive.Common.Models.Identity
{
	public class TokenModel
	{
		public TokenModel(string token)
		{
			this.Token = token;
		}

		public string Token { get; set; }
	}
}