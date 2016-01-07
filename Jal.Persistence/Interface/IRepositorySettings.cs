namespace Jal.Persistence.Interface
{
    public interface IRepositorySettings
    {
        string ConnectionString { get; }

        int CommandTimeout { get; }

        bool StatisticsEnabled { get; }
    }
}
