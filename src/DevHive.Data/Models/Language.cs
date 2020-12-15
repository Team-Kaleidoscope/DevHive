using System;
namespace DevHive.Data.Models
{
	public class Language : IModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }	
	}
}
