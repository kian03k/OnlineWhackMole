using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService2”。
    [ServiceContract]
    public interface IService2
    {

        [OperationContract]
        int Register_Sql(string username,string password);

        [OperationContract]
        bool Login_Sql(string username, string password);

        [OperationContract]
        CompositeType querySql(string UserName);

    }
    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public List<SqlInfo> SqlinfoList { get; set; }

    }

}
