﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Timeoff
{
    public static class ModelExtensions
    {
        public static string? ToLink(this ResultModels.PagerResult pager, IUrlHelper url, int page)
        {
            var ctx = new UrlActionContext
            {
                Values = new RouteValueDictionary(pager.QueryParameters)
                {
                    { "page", page }
                }
            };

            return url.Action(ctx);
        }
    }
}