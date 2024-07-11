using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.Starter.Util.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.Shared
{
	public class RenderContentZone : ViewComponent
	{
		public IViewComponentResult Invoke(string zone, AgilityPageModel model)
		{

			//determine the page model to use based on the template name
			//
			ContentZoneRenderer contentZoneRenderer = new ContentZoneRenderer
			{
				Zone = zone,
				PageModel = model,
				ContentZone = model.PageResponse.Zones.FirstOrDefault(cz => cz.ReferenceName == zone)
			};

			//render it!
			//
			return View("~/Views/PageModels/ContentZone.cshtml", contentZoneRenderer);
		}
	}
}
