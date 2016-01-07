﻿using System;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Persistence.Attributes;
using Jal.Persistence.Impl;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Installer
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        private readonly string _defaultDataBase;

        private readonly LifestyleType _lifestyleType;

        public RepositoryInstaller(string defaultDataBase, LifestyleType lifestyleType = LifestyleType.PerWebRequest)
        {
            _defaultDataBase = defaultDataBase;
            _lifestyleType = lifestyleType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Component.For<IRepositoryLogger>().ImplementedBy<NullRepositoryLogger>());

            container.Register(Component.For<IRepositoryCommand>().ImplementedBy<RepositoryCommand>());

            var assemblies = AssemblyFinder.Impl.AssemblyFinder.Current.GetAssemblies("Persistence");

            var defaultContextName = string.Format("{0}_context", _defaultDataBase);

            if (assemblies != null)
            {
                foreach (var assembly in assemblies)
                {
                    var descriptorDefault = Classes.FromAssembly(assembly).Where(type => !Attribute.IsDefined(type, typeof(DataBaseAttribute))).WithService.AllInterfaces()
                        .Configure(c =>
                        {
                            c.DependsOn(ServiceOverride.ForKey<IRepositoryContext>().Eq(defaultContextName));
                        }
                        );

                    var descriptor = Classes.FromAssembly(assembly).Where(type => Attribute.IsDefined(type, typeof(DataBaseAttribute))).WithService.AllInterfaces()
                        .Configure(
                            c =>
                            {
                                var databaseName = ((DataBaseAttribute)c.Implementation.GetCustomAttributes(typeof(DataBaseAttribute), false)[0]).Name;
                                var contextName = string.Format("{0}_context", databaseName);
                                c.DependsOn(ServiceOverride.ForKey<IRepositoryContext>().Eq(contextName));
                            }
                        );

                    if (_lifestyleType == LifestyleType.Transient)
                    {
                        container.Register(descriptorDefault.LifestyleTransient());

                        container.Register(descriptor.LifestyleTransient());
                    }

                    if (_lifestyleType == LifestyleType.PerWebRequest)
                    {
                        container.Register(descriptorDefault.LifestylePerWebRequest());

                        container.Register(descriptor.LifestylePerWebRequest());
                    }

                    if (_lifestyleType == LifestyleType.Singleton)
                    {
                        container.Register(descriptorDefault.LifestyleSingleton());

                        container.Register(descriptor.LifestyleSingleton());
                    }

                    if (_lifestyleType == LifestyleType.Scoped)
                    {
                        container.Register(descriptorDefault.LifestyleScoped());

                        container.Register(descriptor.LifestyleScoped());
                    }
                }
            }
        }
    }
}