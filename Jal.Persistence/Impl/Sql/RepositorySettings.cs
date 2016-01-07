using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using Jal.Persistence.Interface;
using Jal.Settings.Interface;

namespace Jal.Persistence.Impl.Sql
{
    public class RepositorySettings : IRepositorySettings, ISupportInitialize 
    {
        private const string ConnectionStringsSectionName = "ConnectionStringsSectionName";

        protected readonly ISettingsExtractor SettingsExtractor;

        protected readonly ISectionExtractor SectionExtractor;

        public RepositorySettings(ISettingsExtractor settingsExtractor, ISectionExtractor sectionExtractor)
        {
            SettingsExtractor = settingsExtractor;
            SectionExtractor = sectionExtractor;
        }

        public string ConnectionStringAttributeName { get; set; }

        public string CommandTimeoutAttributeName { get; set; }

        public string ConnectionTimeoutAttributeName { get; set; }

        public string ApplicationNameAttributeName { get; set; }

        public string StatisticsEnabledAttributeName { get; set; }

        public string ConnectionString { get; private set; }

        public int CommandTimeout { get; private set; }

        public bool StatisticsEnabled { get; private set; }

        public virtual string LoadConnectionString()
        {
            var enviroment = SettingsExtractor.Get<string>(ConnectionStringsSectionName);

            var section = SectionExtractor.GetSection<ConnectionStringsSection>("connectionStrings" + enviroment);

            return section.ConnectionStrings[ConnectionStringAttributeName].ConnectionString;
        }

        public void BeginInit()
        {

            ConnectionString=LoadConnectionString();

            var builder = new SqlConnectionStringBuilder(ConnectionString);

            if (string.IsNullOrWhiteSpace(builder.ApplicationName))
            {
                var name = SettingsExtractor.Get<string>(ApplicationNameAttributeName,false, string.Empty);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    builder.ApplicationName = name;
                }
            }

            var connectTimeout = SettingsExtractor.Get<int>(ConnectionTimeoutAttributeName,false, 0);

            if (connectTimeout!=0)
            {
                builder.ConnectTimeout = connectTimeout;
            }

            ConnectionString = builder.ConnectionString;

            CommandTimeout = SettingsExtractor.Get<int>(CommandTimeoutAttributeName);

            StatisticsEnabled = SettingsExtractor.Get<bool>(StatisticsEnabledAttributeName, false, false);
            
        }

        public void EndInit()
        {
            
        }
    }
}