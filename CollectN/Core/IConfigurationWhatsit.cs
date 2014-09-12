namespace CollectN.Core
{
    public interface IConfigurationWhatsit
    {

        void Munge(ApplicationConfiguration config, ConfigurationFile configFile);
    }
}