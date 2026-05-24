using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Service
{
    //需要服务端实现的协定
    [ServiceContract(Namespace = "WcfChat",
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IChatServiceCallback))]
    public interface IService
    {
        [OperationContract(IsOneWay = true)]
        void Login();

        [OperationContract(IsOneWay = true)]
        void Logout(string userName);

        [OperationContract(IsOneWay = true)]
        void SitDown(string userName, int index, int side);

        [OperationContract(IsOneWay = true)]
        void GetUp(int index, int side);

        [OperationContract(IsOneWay = true)]
        void Start(string userName, int index, int side);

        [OperationContract(IsOneWay = true)]
        void SetDot(int index, int id);

        [OperationContract(IsOneWay = true)]
        void Talk(int index, string userName, string message);
        [OperationContract(IsOneWay = true)]
        void randomplace(int index);//随机函数设置地鼠出没
        [OperationContract(IsOneWay = true)]
        void userset(int index, string username,int clickid);

        [OperationContract(IsOneWay = true)]
        void TalkToSomebody(string sender, string receiver, string message);

        #region 数据操作
 

        #endregion

    }

    //需要客户端实现的接口
    public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ShowLogin(string loginUserName, int maxTables);

        [OperationContract(IsOneWay = true)]
        void ShowLogout(string userName);

        [OperationContract(IsOneWay = true)]
        void ShowSitDown(string userName, int side);

        [OperationContract(IsOneWay = true)]
        void ShowGetUp(int side);

        [OperationContract(IsOneWay = true)]
        void ShowStart(int side);

        [OperationContract(IsOneWay = true)]
        void ShowTalk(string messageTpye, string userName, string message);

        [OperationContract(IsOneWay = true)]//放置地鼠
        void ShowSetDot(int id);

        [OperationContract(IsOneWay = true)]
        void GameStart();

        [OperationContract(IsOneWay = true)]
        void GameWin(string message);

        [OperationContract(IsOneWay = true)]
        void UpdateTablesInfo(string tablesInfo, int userCount);

        [OperationContract(IsOneWay = true)]
        void Gotscore(string message,string message1,string message3);
        [OperationContract(IsOneWay = true)]
        void ResetGrid();

        [OperationContract(IsOneWay = true)]
        void InitUsersInfo(string UsersInfo);//初始化用户信息
        [OperationContract(IsOneWay = true)]
        void ShowStar(int number);
    }
}
