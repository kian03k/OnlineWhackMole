using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace A4
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IService1
    {
        public static List<string> filesInfo;
        public static string path;
        public List<string> GetFilesInfo()
        {
            if (filesInfo == null)
            {
                filesInfo = new List<string>();
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloaded");
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                var q = dirInfo.EnumerateFiles();
                foreach(var f in q)
                {
                    filesInfo.Add(String.Format("{0},{1}",f.Name,f.Length));
                }
            }
            return filesInfo;

        }
        public Stream DownloadStream(string fileName)
        {
            string filePath=Path.Combine(path,fileName);
            try
            {
                FileStream imageFile=File.OpenRead(filePath);
                return imageFile;
            }
            catch(IOException ex)
            {
                throw ex;
            }
        }
        
    }
}
