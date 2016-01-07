using System.Collections.Generic;
using System.Data;
using Jal.Converter.Impl;

namespace Jal.Persistence.Tests
{
    public class DataReaderListAccessoryTypeConverter : AbstractConverter<IDataReader, IList<AccessoryType>>
    {
        public override IList<AccessoryType> Convert(IDataReader reader)
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
            return result;
        }
    }
}
