namespace DevHive.Services.Models.Identity.User
{
	public class TokenServiceModel
	{
		public TokenServiceModel(string token)
		{
			this.Token = token;
		}

		public string Token { get; set; }
	}
}