namespace Testura.Android.Device.Configurations
{
    public interface IConfigurationLoader
    {
        /// <summary>
        /// Load the test configuration
        /// </summary>
        /// <returns>The loaded configuration</returns>
        DeviceConfiguration LoadConfiguration();
    }
}
