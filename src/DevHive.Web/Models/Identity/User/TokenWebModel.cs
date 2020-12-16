namespace DevHive.Web.Models.Identity.User
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