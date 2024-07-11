using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.Shared
{
	public class SEO : ViewComponent
	{
		public Task<IViewComponentResult> InvokeAsync(AgilityPageModel model)
		{

			return Task.Run<IViewComponentResult>(() => View("/Views/Shared/SEO.cshtml", model));
		}
	}
}
