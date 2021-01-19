using System;
using System.Collections.Generic;
using DevHive.Data.Interfaces.Models;

namespace DevHive.Data.Models
{
	public class Language : ILanguage
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<User> Users { get; set; }
	}
}
