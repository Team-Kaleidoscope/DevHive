namespace DevHive.Web.Models.User
{
	public class TokenWebModel
	{
		public TokenWebModel(string token)
		{
			this.Token = token;
		}

		public string Token { get; set; }
	}
}
