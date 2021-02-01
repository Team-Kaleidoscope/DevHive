using System;
using System.ComponentModel.DataAnnotations.Schema;
using DevHive.Data.Models;

namespace DevHive.Data.RelationModels
{
	[Table("UserRates")]
	public class UserRate
	{
		public Guid Id { get; set; }

		public User User { get; set; }

		public bool Rate { get; set; }
	}
}
