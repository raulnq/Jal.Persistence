using System.Collections.Generic;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Tests
{
    public interface IAccessoryTypeRepository : IRepositoryContextContainer
    {
        AccessoryType Select(AccessoryType accessoryType);

        void Insert(string name);

        AccessoryType Insert(AccessoryType accessoryType);

        IList<AccessoryType> SelectAll();

        IList<AccessoryType> SelectAll(PageInfo pageInfo);
    }
}
