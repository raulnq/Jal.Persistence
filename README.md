# Jal.Persistence
Just another library to interact with a relational database
## How to use?
Note: The Jal.Settings, Jal.Locator.CastleWindsor, Jal.Converter and Jal.AssemblyFinder library are needed.

Setup the AssemblyFinder library.

    var directory = AppDomain.CurrentDomain.BaseDirectory;
    AssemblyFinder.Impl.AssemblyFinder.Current = new AssemblyFinder.Impl.AssemblyFinder(directory);
    
Setup the Castle Windsor container.

    var container = new WindsorContainer();
  
Install the Jal.Locator.CastleWindsor library.

    container.Install(new ServiceLocatorInstaller());
    
Install the Jal.Converter library.

    container.Install(new ConverterInstaller());
    
Install the Jal.Settings library.

    container.Install(new SettingsInstaller());
    
Setup your connection.

    container.Install(new SqlRepositoryDatabaseInstaller("ConnectionName", "ConnectionString", "CommandTimeout"));
    
Setup your repositories to work with the defined connection.

    container.Install(new RepositoryInstaller("ConnectionName"));
    
Create your repository class

    public interface IAccessoryTypeRepository
    {
        AccessoryType Select(AccessoryType accessoryType);
    }
    
    public class AccessoryTypeRepository : AbstractRepository, IAccessoryTypeRepository
    {
        public AccessoryType Select(AccessoryType accessoryType)
        {
            return StoreProcedure("sp_AccessoryType_select_by_Name").
                Parameters
                (
                    p => p.Name("AccessoryType").VarChar(30).In().Value(accessoryType.Name)
                ).
                Query<AccessoryType>();
        }
        public AccessoryTypeRepository(IRepositoryContext context, IModelConverter modelConverter)
            : base(context, modelConverter)
        {
        }
    }
    
Tag the assembly container of the repository classes in order to be read by the library

    [assembly: AssemblyTag("Persistence")]
    
Create you converter class

    public class DataReaderAccessoryTypeConverter : AbstractConverter<IDataReader, AccessoryType>
    {
        public override AccessoryType Convert(IDataReader reader)
        {
            var result = new List<AccessoryType>();
            while (reader.Read())
            {
                var entity = new AccessoryType()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
                result.Add(entity);
            }
            return result.FirstOrDefault();
        }
    }
    
Tag the assembly container of the converter classes in order to be read by the library

    [assembly: AssemblyTag("Converter")]
    
Resolve a instance of the interface IAccessoryTypeRepository

    var accessoryTypeRepository = container.Resolve<IAccessoryTypeRepository>();
    
Use your repository class

    var result = accessoryTypeRepository.Select(new AccessoryType() {Name = "Remote"});
