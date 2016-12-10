using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Testura.Android.Util.Extensions;

namespace Testura.Android.Device.Configurations
{
    /// <summary>
    /// Load configuration from a json-file
    /// </summary>
    public class JsonConfigurationLoader : IConfigurationLoader
    {
        private const string FileName = "testura.configuration.json";

        /// <summary>
        /// Try to load the default configuration. If we can't find file or fail to
        /// load we return an empty configuration object.
        /// </summary>
        /// <returns>The loaded configuration</returns>
        public DeviceConfiguration LoadConfiguration()
        {
            try
            {
                var configurationText = File.ReadAllText(Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), FileName));
                var configuration = JsonConvert.DeserializeObject<DeviceConfiguration>(configurationText);
                return configuration;
            }
            catch (IOException)
            {
                return new DeviceConfiguration();
            }
        }
    }
}
