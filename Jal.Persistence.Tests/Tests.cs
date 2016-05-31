using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Transactions;
using Castle.Core;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jal.Converter.Impl;
using Jal.Converter.Installer;
using Jal.Converter.Interface;
using Jal.Locator.CastleWindsor.Installer;
using Jal.Locator.Impl;
using Jal.Persistence.Impl;
using Jal.Persistence.Impl.Sql;
using Jal.Persistence.Installer;
using Jal.Persistence.Interface;
using Jal.Persistence.Logger.Installer;
using Jal.Settings.Impl;
using Jal.Settings.Installer;
using NUnit.Framework;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Jal.Persistence.Tests
{
    [TestFixture]
    public class Tests
    {
        private IWindsorContainer _container;

        [SetUp]
        public void SetUp()
        {
            var setting = SettingsExtractor.Builder.Create;
            var section = SectionExtractor.Builder.Create;
            var locator = ServiceLocator.Builder.Create as ServiceLocator;
            locator.Register(typeof(IConverter<IDataReader, IList<AccessoryType>>), new DataReaderListAccessoryTypeConverter());
            locator.Register(typeof(IConverter<IDataReader, AccessoryType>), new DataReaderAccessoryTypeConverter());        
            var converter = ModelConverter.Builder.UseFactory(locator).Create;

            var command = RepositoryCommand.Builder.Create;
            var repositorySettings = new MachineRepositorySettings(setting, section, "OTS_1_0_ConnectionString", "OTS_1_0_CommandTimeout", "", "", "");
            ////var logger = new NullRepositoryLogger();
            var repositoryDatabase = RepositoryDatabase.Builder.UseSettings(repositorySettings).Create;
            var context = RepositoryContext.Builder.UseDatabase(repositoryDatabase).Create;

            


            var a = new AccessoryTypeRepository(context, converter, command);

            var result = a.Select(new AccessoryType() { Name = "Remote" });

            AssemblyFinder.Impl.AssemblyFinder.Current = AssemblyFinder.Impl.AssemblyFinder.Builder.UsePath(TestContext.CurrentContext.TestDirectory).Create;
            _container = new WindsorContainer();
            //_container.Kernel.Resolver.AddSubResolver(new LoggerSubDependencyResolver());
            _container.Install(new ConverterInstaller(AssemblyFinder.Impl.AssemblyFinder.Current.GetAssemblies("Converter")));
            _container.Install(new SettingsInstaller());
            _container.Install(new SqlRepositoryDatabaseMachineInstaller("DirectvDs", "OTS_1_0_ConnectionString", "OTS_1_0_CommandTimeout", LifestyleType.Scoped));
            _container.Install(new RepositoryInstaller("DirectvDs", AssemblyFinder.Impl.AssemblyFinder.Current.GetAssemblies("Repository"), LifestyleType.Scoped));
            //_container.Install(new RepositoryLoggerInstaller());
            _container.Install(new ServiceLocatorInstaller());
            //_container.Register(Component.For<IRepositoryLogger>().ImplementedBy<ConsoleLogger>().IsDefault());
            
        }

        [Test]
        public void Select_With_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var result = accessoryTypeRepository.Select(new AccessoryType() {Name = "Remote"});
                Assert.AreEqual(1, result.Id);
            }
        }

        [Test]
        public void Select_WithOutput_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var result = accessoryTypeRepository.Insert(new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) });
                Assert.AreNotEqual(0, result.Id);
            }
        }

        [Test]
        public void Select_WithOutOutput_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                accessoryTypeRepository.Insert(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            }
        }

        [Test]
        public void SelectAll_With_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var result = accessoryTypeRepository.SelectAll();
                Assert.AreNotEqual(0, result.Count);
            }
        }

        [Test]
        public void SelectAll_WithPageInfo_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var pageInfo = new PageInfo();
                var result = accessoryTypeRepository.SelectAll(pageInfo);
                Assert.AreNotEqual(0, result.Count);
                Assert.AreNotEqual(0, pageInfo.Count);
            }
        }

        [Test]
        public void Select_WithMultipleCalls_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var result = accessoryTypeRepository.Insert(new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) });
                Assert.AreNotEqual(0, result.Id);
                result = accessoryTypeRepository.Insert(new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) });
                Assert.AreNotEqual(0, result.Id);
                result = accessoryTypeRepository.Insert(new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) });
                Assert.AreNotEqual(0, result.Id);
            }
        }

        [Test]
        public void Select_WithMultipleCallsDiferentsRepositories_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository1 = _container.Resolve<IAccessoryTypeRepository>();
                var accessoryTypeRepository2 = _container.Resolve<IAccessoryTypeRepository>();
                var accessoryTypeRepository3 = _container.Resolve<IAccessoryTypeRepository>();
                var result = accessoryTypeRepository1.Insert(new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) });
                Assert.AreNotEqual(0, result.Id);
                result = accessoryTypeRepository2.Insert(new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) });
                Assert.AreNotEqual(0, result.Id);
                result = accessoryTypeRepository3.Insert(new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) });
                Assert.AreNotEqual(0, result.Id);
                _container.Release(accessoryTypeRepository1);
                _container.Release(accessoryTypeRepository2);
                _container.Release(accessoryTypeRepository3);
            }
        }

        [Test]
        public void Select_WithTransactionScope_Commit()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() {Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)};
                using (var scope = new TransactionScope(TransactionScopeOption.Required,new TransactionOptions() {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    using (var c = accessoryTypeRepository.Context.CreateConnection())
                    {
                        c.Open();
                        var result = accessoryTypeRepository.Insert(parameter);
                        Assert.AreNotEqual(0, result.Id);
                        scope.Complete();
                    }
                }
                var data = accessoryTypeRepository.Select(parameter);
                Assert.AreNotEqual(0, data.Id);
            }
        }

        [Test]
        public void Select_WithBeginTransaction_Commit()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                using (var c=accessoryTypeRepository.Context.CreateConnection())
                {
                    c.Open();
                    using (var t = c.BeginTransaction())
                    {
                        var result = accessoryTypeRepository.Insert(parameter);
                        Assert.AreNotEqual(0, result.Id);
                        t.Commit();
                    }
                }
                var data = accessoryTypeRepository.Select(parameter);
                Assert.AreNotEqual(0, data.Id);
            }
        }

        [Test]
        public void Select_WithTransactionScopeDifferentRepositories_Commit()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository1 = _container.Resolve<IAccessoryTypeRepository>();
                var accessoryTypeRepository2 = _container.Resolve<IAccessoryTypeRepository>();               
                var parameter1 = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                Thread.Sleep(100);
                var parameter2 = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var c = accessoryTypeRepository2.Context.CreateConnection())
                    {
                        c.Open();
                        var result = accessoryTypeRepository1.Insert(parameter1);
                        Assert.AreNotEqual(0, result.Id);

                        result = accessoryTypeRepository2.Insert(parameter2);
                        Assert.AreNotEqual(0, result.Id);

                        scope.Complete();
                    }
                }

                var data = accessoryTypeRepository2.Select(parameter1);
                Assert.AreNotEqual(0, data.Id);

                data = accessoryTypeRepository1.Select(parameter2);
                Assert.AreNotEqual(0, data.Id);
            }
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Select_WithDoubleInnerCreateConnection_Exception()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };

                using (var c1 = accessoryTypeRepository.Context.CreateConnection())
                {
                    using (var c2 = accessoryTypeRepository.Context.CreateConnection())
                    {
                        
                    }
                }
            }
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Select_WithDoubleInnerBeginTransacton_Exception()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();

                using (var c1 = accessoryTypeRepository.Context.CreateConnection())
                {
                    c1.Open();
                    using (var t1 = c1.BeginTransaction())
                    {
                        using (var t2 = c1.BeginTransaction())
                        {

                        }
                    }
                }
            }
        }


        [Test]
        public void Select_WithDoubleBeginTransacton_Exception()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();

                using (var c1 = accessoryTypeRepository.Context.CreateConnection())
                {
                    c1.Open();
                    using (var t1 = c1.BeginTransaction())
                    {
                        t1.Commit();
                    }

                    using (var t2 = c1.BeginTransaction())
                    {
                        t2.Commit();
                    }
                }
            }
        }

        [Test]
        public void Select_WithDoubleCreateConnection_Success()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };

                using (var c1 = accessoryTypeRepository.Context.CreateConnection())
                {
                    c1.Open();
                    var result = accessoryTypeRepository.Insert(parameter);
                    Assert.AreNotEqual(0, result.Id);
                }

                parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };

                using (var c2 = accessoryTypeRepository.Context.CreateConnection())
                {
                    c2.Open();
                    var result = accessoryTypeRepository.Insert(parameter);
                    Assert.AreNotEqual(0, result.Id);
                }
            }
        }



        [Test]
        public void Select_WithTransactionScopeDifferentRepositories_Rollback()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository1 = _container.Resolve<IAccessoryTypeRepository>();
                var accessoryTypeRepository2 = _container.Resolve<IAccessoryTypeRepository>();
                var parameter1 = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                Thread.Sleep(100);
                var parameter2 = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.DefaultTimeout} ))
                {
                    using (var c = accessoryTypeRepository2.Context.CreateConnection())
                    {
                        c.Open();
                        var result = accessoryTypeRepository1.Insert(parameter1);
                        Assert.AreNotEqual(0, result.Id);

                        result = accessoryTypeRepository1.Select(parameter1);
                        Assert.AreNotEqual(null, result);

                        result = accessoryTypeRepository2.Insert(parameter2);
                        Assert.AreNotEqual(0, result.Id);

                        result = accessoryTypeRepository2.Select(parameter2);
                        Assert.AreNotEqual(null, result);
                    }
                }
                var data = accessoryTypeRepository2.Select(parameter1);
                Assert.AreEqual(null, data);

                data = accessoryTypeRepository1.Select(parameter2);
                Assert.AreEqual(null, data);
            }
        }

        [Test]
        public void Select_WithBeginTransactionDifferentRepositories_Rollback()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository1 = _container.Resolve<IAccessoryTypeRepository>();
                var accessoryTypeRepository2 = _container.Resolve<IAccessoryTypeRepository>();
                var parameter1 = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                Thread.Sleep(100);
                var parameter2 = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };

                using (var c = accessoryTypeRepository2.Context.CreateConnection())
                {
                    c.Open();
                    using (var t = c.BeginTransaction())
                    {
                        var result = accessoryTypeRepository1.Insert(parameter1);
                        Assert.AreNotEqual(0, result.Id);

                        result = accessoryTypeRepository1.Select(parameter1);
                        Assert.AreNotEqual(null, result);

                        result = accessoryTypeRepository2.Insert(parameter2);
                        Assert.AreNotEqual(0, result.Id);

                        result = accessoryTypeRepository2.Select(parameter2);
                        Assert.AreNotEqual(null, result);

                        t.Rollback();
                    }
                }

                var data = accessoryTypeRepository2.Select(parameter1);
                Assert.AreEqual(null, data);

                data = accessoryTypeRepository1.Select(parameter2);
                Assert.AreEqual(null, data);
            }
        }

        [Test]
        public void Select_WithTransactionScope_Rollback()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var c = accessoryTypeRepository.Context.CreateConnection())
                    {
                        c.Open();
                        var result = accessoryTypeRepository.Insert(parameter);
                        Assert.AreNotEqual(0, result.Id);

                        result = accessoryTypeRepository.Select(parameter);
                        Assert.AreNotEqual(null, result);
                    }
                }
                var data = accessoryTypeRepository.Select(parameter);
                Assert.AreEqual(null, data);
            }
        }

        [Test]
        public void Select_WithBeginTransaction_Rollback()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };

                using (var c = accessoryTypeRepository.Context.CreateConnection())
                {
                    c.Open();
                    using (var t = c.BeginTransaction())
                    {
                        var result = accessoryTypeRepository.Insert(parameter);
                        Assert.AreNotEqual(0, result.Id);

                        result = accessoryTypeRepository.Select(parameter);
                        Assert.AreNotEqual(null, result);

                        t.Rollback();
                    }
                }

                var data = accessoryTypeRepository.Select(parameter);
                Assert.AreEqual(null, data);
            }
        }

        [Test]
        public void Select_WithTransactionScopeAndException_Rollback()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                try
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        using (var c = accessoryTypeRepository.Context.CreateConnection())
                        {
                            c.Open();
                            var result = accessoryTypeRepository.Insert(parameter);
                            Assert.AreNotEqual(0, result.Id);

                            result = accessoryTypeRepository.Select(parameter);
                            Assert.AreNotEqual(null, result);
                        }
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                var data = accessoryTypeRepository.Select(parameter);
                Assert.AreEqual(null, data);
            }
        }

        [Test]
        public void Select_WithBeginTransactionAndClose_Rollback()
        {
            using (_container.BeginScope())
            {
                var accessoryTypeRepository = _container.Resolve<IAccessoryTypeRepository>();
                var parameter = new AccessoryType() { Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                try
                {
                    using (var c = accessoryTypeRepository.Context.CreateConnection())
                    {
                        c.Open();
                        using (var t = c.BeginTransaction())
                        {
                            var result = accessoryTypeRepository.Insert(parameter);
                            Assert.AreNotEqual(0, result.Id);

                            result = accessoryTypeRepository.Select(parameter);
                            Assert.AreNotEqual(null, result);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                    // ignored
                }

                var data = accessoryTypeRepository.Select(parameter);
                Assert.AreEqual(null, data);
            }
        }
    }
}

