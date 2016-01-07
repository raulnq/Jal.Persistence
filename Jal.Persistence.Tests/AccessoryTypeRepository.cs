using System.Collections.Generic;
using Jal.Converter.Interface;
using Jal.Persistence.Impl;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Tests
{
    public class AccessoryTypeRepository : AbstractRepository, IAccessoryTypeRepository
    {
        public AccessoryType Select(AccessoryType accessoryType)
        {
            return StoredProcedure("sp_AccessoryType_select_by_Name")
                .Parameters
                (
                    p => p.Name("AccessoryType").VarChar(30).In().Value(accessoryType.Name)
                )
                .Query<AccessoryType>();
            /*
            var inputAction = new Action<IDbCommand>(command =>
                                                    {
                                                        InputParameterAdd.Varchar(command as SqlCommand, "AccessoryType", 30, accessoryType.Name);
                                                    });
            return Execute<AccessoryType>("sp_AccessoryType_select_by_Name", inputAction);
             */
        }

        public IList<AccessoryType> SelectAll()
        {
            /*
            var inputAction = new Action<IDbCommand>(command =>
            {
                OutputParameterAdd.Int(command as SqlCommand, "Count");
            });
            return Execute<IList<AccessoryType>>("sp_AccessoryType_select", inputAction);
            */
            return StoredProcedure("sp_AccessoryType_select")
                .Parameters
                (
                    p => p.Name("Count").Int().Out()
                )
                .Query<IList<AccessoryType>>();
        }

        public IList<AccessoryType> SelectAll(PageInfo pageInfo)
        {
            return StoredProcedure("sp_AccessoryType_select")
                .Parameters
                (
                    p => p.Name("Count").Int().Out().To<PageInfo, int>(x => x.Count)
                )
                .Query<IList<AccessoryType>, PageInfo>(pageInfo);
            /*
            var inputAction = new Action<IDbCommand>(command =>
            {
                OutputParameterAdd.Int(command as SqlCommand, "Count");
            });
            var outputAction = new Action<PageInfo, IDbCommand>((parameter, command) =>
            {
                pageInfo.Count = OutputParameterGet.Value<int>(command as SqlCommand, "Count");
            });
            var data = Execute<IList<AccessoryType>, PageInfo>("sp_AccessoryType_select", outputAction, pageInfo, inputAction);
            return data;
             */
        }

        public void Insert(string name)
        {
            /*
            var inputAction = new Action<IDbCommand>(command =>
            {
                InputParameterAdd.Varchar(command as SqlCommand, "AccessoryType", 30, name);
                OutputParameterAdd.Int(command as SqlCommand, "AccessoryTypeId");
            });
            ExecuteNonQuery("sp_AccessoryType_Insert", inputAction);
             */

            StoredProcedure("sp_AccessoryType_Insert")
                .Parameters(p =>
                            {
                                p.Name("AccessoryType").VarChar(30).In().Value(name);
                                p.Name("AccessoryTypeId").Int().Out();
                            })
                .Execute();
        }

        public AccessoryType Insert(AccessoryType accessoryType)
        {
            /*var inputAction = new Action<IDbCommand>(command =>
            {
                InputParameterAdd.Varchar(command as SqlCommand, "AccessoryType", 30, accessoryType.Name);
                OutputParameterAdd.Int(command as SqlCommand, "AccessoryTypeId");
            });

            var outputAction = new Action<AccessoryType, IDbCommand>((parameter, command) =>
            {
                parameter.Id = OutputParameterGet.Value<int>(command as SqlCommand, "AccessoryTypeId");
            });

            ExecuteNonQuery<AccessoryType>("sp_AccessoryType_Insert", outputAction, accessoryType, inputAction);

            return accessoryType;
            */
            StoredProcedure("sp_AccessoryType_Insert")
                .Parameters(p =>
                            {
                                p.Name("AccessoryType").VarChar(30).In().Value(accessoryType.Name);
                                p.Name("AccessoryTypeId").Int().Out().To<AccessoryType,int>(x=>x.Id);
                            })
                .Execute<AccessoryType>(accessoryType);

            return accessoryType; 
        }

        public AccessoryTypeRepository(IRepositoryContext context, IModelConverter modelConverter)
            : base(context, modelConverter)
        {

        }
    }
}
