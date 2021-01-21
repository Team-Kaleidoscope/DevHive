using System.Security.Cryptography;
using System.Text;

namespace DevHive.Common.Models.Misc
{
	public static class PasswordModifications
	{
		public static string GeneratePasswordHash(string password)
		{
			return string.Join(string.Empty, SHA512.HashData(Encoding.ASCII.GetBytes(password)));
		}
	}
}
