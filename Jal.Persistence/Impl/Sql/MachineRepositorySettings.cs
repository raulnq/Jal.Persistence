using System.Configuration;
using System.Data.SqlClient;
using Jal.Persistence.Interface;
using Jal.Settings.Interface;

namespace Jal.Persistence.Impl.Sql
{
    public class MachineRepositorySettings : IRepositorySettings
    {
        protected const string ConnectionStringsSectionName = "ConnectionStringsSectionName";

        public MachineRepositorySettings(ISettingsExtractor settingsExtractor, ISectionExtractor sectionExtractor,
            string connectionStringAttributeName, string commandTimeoutAttributeName, string connectionTimeoutAttributeName = null,
            string applicationNameAttributeName = null, string statisticsEnabledAttributeName = null)
        {
            var enviroment = settingsExtractor.Get<string>(ConnectionStringsSectionName);

            var section = sectionExtractor.GetSection<ConnectionStringsSection>("connectionStrings" + enviroment);

            ConnectionString = section.ConnectionStrings[connectionStringAttributeName].ConnectionString;

            var builder = new SqlConnectionStringBuilder(ConnectionString);

            if (string.IsNullOrWhiteSpace(builder.ApplicationName))
            {
                var name = string.IsInterned(settingsExtractor.Get(applicationNameAttributeName, false, string.Empty));

                if (!string.IsNullOrWhiteSpace(name))
                {
                    builder.ApplicationName = name;
                }
            }

            var connectTimeout = settingsExtractor.Get(connectionTimeoutAttributeName, false, 0);

            if (connectTimeout != 0)
            {
                builder.ConnectTimeout = connectTimeout;
            }

            ConnectionString = builder.ConnectionString;

            CommandTimeout = settingsExtractor.Get<int>(commandTimeoutAttributeName);

            StatisticsEnabled = settingsExtractor.Get(statisticsEnabledAttributeName, false, false);
        }

        public string ConnectionString { get; set; }

        public int CommandTimeout { get; set; }

        public bool StatisticsEnabled { get; set; }
    }
}