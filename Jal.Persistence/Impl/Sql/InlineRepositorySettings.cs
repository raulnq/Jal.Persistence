using System.Configuration;
using Jal.Settings.Interface;

namespace Jal.Persistence.Impl.Sql
{
    public class InlineRepositorySettings : RepositorySettings
    {
        public InlineRepositorySettings(ISettingsExtractor settingsExtractor, ISectionExtractor sectionExtractor)
            : base(settingsExtractor, sectionExtractor)
        {

        }

        public override string LoadConnectionString()
        {
            var section = SectionExtractor.GetSection<ConnectionStringsSection>("connectionStrings");

            return section.ConnectionStrings[ConnectionStringAttributeName].ConnectionString;
        }
    }
}