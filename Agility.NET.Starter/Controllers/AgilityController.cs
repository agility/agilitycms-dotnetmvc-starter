using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.Data;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Agility.NET.Starter.Util.Helpers;
using Agility.Models;
using Agility.NET.FetchAPI.Models;


namespace Agility.NET.Starter.Controllers
{

	public class AgilityController : Controller
	{

		private readonly FetchApiService _fetchApiService;
		private readonly AppSettings _appSettings;
		private readonly IMemoryCache _cache;
		private readonly IWebHostEnvironment _env;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AgilityController(FetchApiService fetchApiService,
			 IOptions<AppSettings> appSettings,
			 IMemoryCache cache,
			 IWebHostEnvironment env,
			 IHttpContextAccessor httpContextAccessor)
		{
			_fetchApiService = fetchApiService;
			_cache = cache;
			_env = env;
			_appSettings = appSettings.Value;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IActionResult> RenderPage()
		{


			var path = HttpContext.Request.Path;
			Console.WriteLine("*** PATH: " + path);
			var sitemapPages =
				 await TransformerMiddlewareHelpers.GetOrCacheSitemapPages(
							_env, _appSettings, _fetchApiService, _httpContextAccessor, _cache);

			SitemapPage sitemapPage;

			sitemapPages.TryGetValue(path, out sitemapPage);

			var isPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext);

			if (sitemapPage == null)
			{
				Console.WriteLine("sitemapPage is null");
				//not found!
				return NotFound();
			}


			var getPageExpandedParameters = new GetPageParameters()
			{
				PageId = sitemapPage.PageID,
				Locale = sitemapPage.Locale,
				ExpandAllContentLinks = true,
				ContentLinkDepth = 0,
				IsPreview = isPreview
			};

			var page = await _fetchApiService.GetTypedPage(getPageExpandedParameters);

			if (page == null)
			{
				//not found!
				return NotFound();
			}

			AgilityPageModel pageModel = new AgilityPageModel()
			{
				PageResponse = page,
				Locale = sitemapPage.Locale,
				SitemapPage = sitemapPage
			};

			page.IsDynamicPage = page.Dynamic != null;

			if (page.IsDynamicPage)
			{
				page.Name = page.IsDynamicPage ? sitemapPage.Name : page.Name;
				RouteData.Values["dynamicFields"] = page.Dynamic;

				var getItemParameters = new GetItemParameters()
				{
					ContentId = sitemapPage.ContentID,
					Locale = sitemapPage.Locale,
					ContentLinkDepth = 3,
					ExpandAllContentLinks = true,
					IsPreview = isPreview
				};



				string contentItemStr = await _fetchApiService.GetContentItem(getItemParameters);
				if (!string.IsNullOrEmpty(contentItemStr))
				{
					var contentItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ContentItem>(contentItemStr);
					pageModel.DynamicPageItem = contentItem;
				}


			}



			//setup the caching for CDN
			Response.Headers["Vary"] = "Agility-Mode";

			if (!isPreview)
			{
				//Set the cache control headers - this is just an example, you can set this to whatever you need
				//we are caching the page for 60 seconds, and allowing it to be served stale for up to 1 day seconds while we revalidate it
				Response.Headers["Cache-Control"] = "public, s-maxage=60, stale-while-revalidate=86400";
				Response.Headers["Agility-Mode"] = "production";
			}
			else
			{
				Response.Headers["Cache-Control"] = "no-store";
				Response.Headers["Agility-Mode"] = "preview";
			}

			return View("/Views/AgilityPage.cshtml", pageModel);

		}
	}

}