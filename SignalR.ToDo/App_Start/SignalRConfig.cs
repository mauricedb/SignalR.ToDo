﻿using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using SignalR.ToDo.Models;
using SignalR.ToDo.Utils;

namespace SignalR.ToDo
{
    public static class SignalRConfig
    {
        public static void RegisterHubs()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new FilteredCamelCasePropertyNamesContractResolver
                {
                    TypesToInclude = 
                    { 
                        typeof(TodoListDto), 
                        typeof(TodoItemDto), 
                    }
                }
            };
            var jsonNetSerializer = new JsonNetSerializer(serializerSettings);
            GlobalHost.DependencyResolver.Register(typeof(IJsonSerializer), () => jsonNetSerializer);

            RouteTable.Routes.MapHubs();
        }
    }

}
