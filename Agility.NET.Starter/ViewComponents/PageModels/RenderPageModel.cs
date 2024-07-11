using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.Starter.Util.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.Shared
{
	public class RenderPageModel : ViewComponent
	{
		public IViewComponentResult Invoke(AgilityPageModel model)
		{

			//determine the page model to use based on the template name
			var templateName = model.PageResponse.TemplateName;
			var template = $"~/Views/PageModels/{templateName.Replace(" ", string.Empty)}.cshtml";

			//render it!
			return View(template, model);
		}
	}
}
