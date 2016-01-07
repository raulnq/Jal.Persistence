using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Logger.Installer
{
    public class RepositoryLoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IRepositoryLogger>().ImplementedBy<RepositoryLogger>().IsDefault());
        }

    }
}