using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using EasyLearner.Models;
using EasyLearner.ViewModels;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Serilog;

namespace EasyLearner.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class NotifyService
	{
		private DispatcherTimer _notifyTimer;
		/// <summary>
		/// Сервис по уведомлению.
		/// </summary>
		private static ToastNotifier _toastNotifier;

		private static string _bigImageWithTextsNofityAtTopXmlTemplate =>
			"<toast>\r\n\r\n    <visual>\r\n        <binding template=\"ToastGeneric\">\r\n            <text hint-maxLines=\"1\" id=\"1\"></text>\r\n            <text id=\"2\">Check out where we camped last weekend! It was incredible, wish you could have come on the backpacking trip!</text>\r\n            <image id=\"1\" placement=\"appLogoOverride\" hint-crop=\"circle\" src=\"https://unsplash.it/64?image=1027\"/>\r\n            <image id=\"2\" placement=\"hero\" src=\"https://unsplash.it/360/180?image=1043\"/>\r\n        </binding>\r\n    </visual>\r\n\r\n    \r\n\r\n</toast>";

		/// <summary>
		/// Создает отложенное напоминание с изображением.
		/// </summary>
		/// <param name="title">Заголовок.</param>
		/// <param name="description">Описание.</param>
		/// <param name="dateTime">Время показа.</param>
		/// <param name="imagePath">Путь к изображению.</param>
		/// <param name="snoozeInterval">Время на откладывание напоминания.</param>
		/// <param name="snoozeCount">Количество доступных откладываний.</param>
		public  void CreateScheduledNotifyWithImage(NotificationModel notify)
		{
			try
			{
				if (_toastNotifier == null)
				{
					_toastNotifier = ToastNotificationManager.CreateToastNotifier();
				}
				XmlDocument toastXml = new XmlDocument();
				toastXml.LoadXml(_bigImageWithTextsNofityAtTopXmlTemplate);
				var textNodes = toastXml.GetElementsByTagName("text");
				textNodes[0].InnerText = notify.Title;
				textNodes[1].InnerText = notify.Description;

				// todo: image
				//var imageElements = toastXml.GetElementsByTagName("image");
				//((XmlElement)imageElements[0]).SetAttribute("src", notify.SmallImageIconPath);
				//((XmlElement)imageElements[1]).SetAttribute("src", notify.BigImagePreviewPath);

				//string xml = toastXml.GetXml();

				var scheduledNotification = new ScheduledToastNotification(toastXml, notify.DateToShow.AddSeconds(2));

				if (_toastNotifier == null)
				{
					_toastNotifier = ToastNotificationManager.CreateToastNotifier();
				}

				_toastNotifier.AddToSchedule(scheduledNotification);
			}
			catch (Exception ex)
			{
				Log.Logger.Error(ex, ex.Message);
				Crashes.TrackError(ex);
			}
		}
		/// <summary>
		/// Инициализирует экземпляр <see cref="NotifyService"/>.
		/// </summary>
		public NotifyService()
		{
			_notifyTimer = new DispatcherTimer(); 
			_notifyTimer.Interval = TimeSpan.FromMinutes(5);
			_notifyTimer.Tick += _notifyTimer_Tick;
		
		}

		public void Init()
		{
			_notifyTimer_Tick(null, null);
			_notifyTimer.Start();
		}

		private void _notifyTimer_Tick(object sender, object e)
		{
			var mainVm = App.ServiceProvider.GetRequiredService<MainViewModel>();
			var words = mainVm.LearningStrings;

			if (words.Any())
			{
				var rand = new Random();
				int randomIndex = rand.Next(0, words.Count);
				var randomWord = words[randomIndex];

				var notifyModel = new NotificationModel();
				notifyModel.Title = randomWord.Value;
				notifyModel.Description = randomWord.Translate;
				CreateScheduledNotifyWithImage(notifyModel);
			}
		}
	}
}
