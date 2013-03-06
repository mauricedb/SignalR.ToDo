using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using SignalR.ToDo.Configuration;

namespace SignalR.ToDo
{
    public static class SignalRConfig
    {
        public static void RegisterHubs()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new SignalRContractResolver()
            };
            var jsonNetSerializer = new JsonNetSerializer(serializerSettings);
            GlobalHost.DependencyResolver.Register(typeof (IJsonSerializer), () => jsonNetSerializer);

            GlobalHost.HubPipeline.AddModule(new ExceptionNotifierHubPipelineModule());

            RouteTable.Routes.MapHubs();
        }
    }

}
