using System.Web.Routing;

namespace SignalR.ToDo
{
    public static class SignalRConfig
    {
        public static void RegisterHubs()
        {
            RouteTable.Routes.MapHubs();
        }
    }
}
