using System;

namespace DevHive.Data.Models
{
	public class Technology : IModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
