
namespace MyDemoProjects.Application.Infastructure.Hubs
{
    public class HubClientOptions
    {
        /// <summary>
        /// Create a new instance of HubClientOptions with its Default settings. 
        /// </summary>
        public static HubClientOptions Default => new HubClientOptions();
        public string HubUrl { get; set; } = String.Empty;
        public bool AddMessagePackProtocol { get; set; } = true;
        public bool LoggingEnabled  { get; set; } = true;

    }
}
