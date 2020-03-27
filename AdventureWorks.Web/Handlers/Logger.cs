using log4net;
using log4net.Config;

[assembly: XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace AdventureWorks.Web.Handlers
{
    public static class Logger
    {
        private static ILog log = LogManager.GetLogger("LOGGER");


        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}