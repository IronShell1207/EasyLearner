using EasyLearner.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EasyLearner.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	internal class ViewModelLocator
	{
		public MainViewModel GetMainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();
	}
}
