using System;
using System.Configuration;
using System.Data.SqlClient;
using Jal.Persistence.Interface;
using Jal.Settings.Interface;

namespace Jal.Persistence.Impl.Sql
{
    public class InlineRepositorySettings : IRepositorySettings
    {
        public InlineRepositorySettings(ISettingsExtractor settingsExtractor, ISectionExtractor sectionExtractor,
            string connectionStringAttributeName, string commandTimeoutAttributeName, string connectionTimeoutAttributeName,
            string applicationNameAttributeName, string statisticsEnabledAttributeName)
        {

            var section = sectionExtractor.GetSection<ConnectionStringsSection>("connectionStrings");

            ConnectionString = section.ConnectionStrings[connectionStringAttributeName].ConnectionString;

            var builder = new SqlConnectionStringBuilder(ConnectionString);

            if (string.IsNullOrWhiteSpace(builder.ApplicationName))
            {
                var name = string.IsInterned(settingsExtractor.Get<string>(applicationNameAttributeName, false, string.Empty));

                if (!string.IsNullOrWhiteSpace(name))
                {
                    builder.ApplicationName = name;
                }
            }

            var connectTimeout = settingsExtractor.Get<int>(connectionTimeoutAttributeName, false, 0);

            if (connectTimeout != 0)
            {
                builder.ConnectTimeout = connectTimeout;
            }

            ConnectionString = builder.ConnectionString;

            CommandTimeout = settingsExtractor.Get<int>(commandTimeoutAttributeName);

            StatisticsEnabled = settingsExtractor.Get<bool>(statisticsEnabledAttributeName, false, false);
        }


        public string ConnectionString { get; set; }

        public int CommandTimeout { get; set; }

        public bool StatisticsEnabled { get; set; }

    }
}