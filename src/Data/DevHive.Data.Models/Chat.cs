using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHive.Data.Models
{
	public class Chat
	{
		public HashSet<User> ChatMembers { get; set; }

		public List<Message> Messages { get; set; }

		public string ChatName { get; set; }
	}
}
