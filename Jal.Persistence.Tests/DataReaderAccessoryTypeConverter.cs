using System.Collections.Generic;
using System.Data;
using System.Linq;
using Jal.Converter.Impl;

namespace Jal.Persistence.Tests
{
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
}
