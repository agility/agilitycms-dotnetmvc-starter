﻿using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.PageModules
{
    public class RichTextArea : ViewComponent
    {
        private readonly FetchApiService _fetchApiService;

        public RichTextArea(FetchApiService fetchApiService)
        {
            _fetchApiService = fetchApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var getParams = new GetItemParameters
            {
                ContentId = moduleModel.Model.Item.ContentID,
                Locale = moduleModel.Locale
            };
            var richTextArea = await _fetchApiService.GetTypedContentItem<Agility.Models.RichTextArea>(getParams);
            return View("/Views/PageModules/RichTextArea.cshtml", richTextArea);
        }
    }
}
