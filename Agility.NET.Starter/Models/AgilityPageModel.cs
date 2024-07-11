using System;
using System.Collections.Generic;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.API;

namespace Agility.Models
{

	public class AgilityPageModel
	{
		/**
		 * The current page
		 */
		public PageResponse PageResponse { get; set; }

		/**
		 * The current page's locale
		 */
		public string Locale { get; set; }

		/**
		 * The sitemap node for the current page
		 */
		public SitemapPage SitemapPage { get; set; }


		/**
		 * The current page's dynamic page item (it's a dynamic page)
		 */
		public ContentItem DynamicPageItem { get; set; }
	}
}