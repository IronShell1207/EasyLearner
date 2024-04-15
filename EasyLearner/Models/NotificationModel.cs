namespace EasyLearner.Models
{
	using System;

	public class NotificationModel
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime DateToShow { get; set; } = DateTime.Now;
		public string BigImagePreviewPath { get; set; }
		public string SmallImageIconPath { get; set; }

		public Action OnActivatedAction { get; set; }
	}
}