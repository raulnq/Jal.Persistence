using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Persistence.Impl;
using Jal.Persistence.Impl.Sql;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Installer
{
    public class SqlRepositoryDatabaseInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        private readonly string _databaseName;

        private readonly bool _connectionStringInline;

        private readonly string _connectionStringAttributeName;

        private readonly string _commandTimeoutAttributeName;

        private readonly string _connectionTimeoutAttributeName;

        private readonly string _applicationNameAttributeName;

        private readonly string _statisticsEnabledAttributeName;

        private readonly LifestyleType _lifestyleType;

        public SqlRepositoryDatabaseInstaller(string databaseName, string connectionStringAttributeName, string commandTimeoutAttributeName, LifestyleType lifestyleType = LifestyleType.PerWebRequest, bool connectionStringInline = false, string applicationNameAttributeName = null, string statisticsEnabledAttributeName=null, string connectionTimeoutAttributeName = null)
        {
            _databaseName = databaseName;
            _connectionStringAttributeName = connectionStringAttributeName;
            _commandTimeoutAttributeName = commandTimeoutAttributeName;
            _lifestyleType = lifestyleType;
            _applicationNameAttributeName = applicationNameAttributeName;
            _connectionTimeoutAttributeName = connectionTimeoutAttributeName;
            _statisticsEnabledAttributeName = statisticsEnabledAttributeName;
            _connectionStringInline = connectionStringInline;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var settingName = string.Format("{0}_settings", _databaseName);

            var contextName = string.Format("{0}_context", _databaseName);

            //Setting Registration - Singleton
            if (_connectionStringInline)
            {
                container.Register(Component.For<IRepositorySettings>().ImplementedBy<InlineRepositorySettings>()
                .DependsOn(new
                {
                    connectionStringAttributeName = _connectionStringAttributeName ?? string.Empty,
                    commandTimeoutAttributeName = _commandTimeoutAttributeName ?? string.Empty,
                    connectionTimeoutAttributeName = _connectionTimeoutAttributeName ?? string.Empty,
                    applicationNameAttributeName = _applicationNameAttributeName ?? string.Empty,
                    statisticsEnabledAttributeName = _statisticsEnabledAttributeName ?? string.Empty
                })
                .Named(settingName));
            }
            else
            {
                container.Register(Component.For<IRepositorySettings>().ImplementedBy<RepositorySettings>()
                .DependsOn(new
                {
                    connectionStringAttributeName = _connectionStringAttributeName ?? string.Empty,
                    commandTimeoutAttributeName = _commandTimeoutAttributeName ?? string.Empty,
                    connectionTimeoutAttributeName = _connectionTimeoutAttributeName ?? string.Empty,
                    applicationNameAttributeName = _applicationNameAttributeName ?? string.Empty,
                    statisticsEnabledAttributeName = _statisticsEnabledAttributeName ?? string.Empty
                })
                .Named(settingName));
            }


            container.Register(Component.For<IRepositoryDatabase>().ImplementedBy<RepositoryDatabase>().DependsOn(ServiceOverride.ForKey<IRepositorySettings>().Eq(settingName)).Named(_databaseName));

            var descriptor = Component.For<IRepositoryContext>().ImplementedBy<RepositoryContext>().DependsOn(ServiceOverride.ForKey<IRepositoryDatabase>().Eq(_databaseName)).Named(contextName);

            //Repository Context Registration
            if (_lifestyleType == LifestyleType.PerWebRequest)
            {
                container.Register(descriptor.LifestylePerWebRequest());
            }

            if (_lifestyleType == LifestyleType.Transient)
            {
                container.Register(descriptor.LifestyleTransient());
            }

            if (_lifestyleType == LifestyleType.Singleton)
            {
                container.Register(descriptor);
            }

            if (_lifestyleType == LifestyleType.Scoped)
            {
                container.Register(descriptor.LifestyleScoped());
            }

        }

        #endregion
    }
}