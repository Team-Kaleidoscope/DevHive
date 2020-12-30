using System;
namespace DevHive.Data.Models
{
	public class Comment : IModel
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string Message { get; set; }
		public DateTime Date { get; set; }
	}
}