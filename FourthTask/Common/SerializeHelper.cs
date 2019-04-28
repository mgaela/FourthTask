using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Configuration;

namespace FourthTask.Common
{
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化数据地址
        /// </summary>
        public static string SerializeDataPath = ConfigurationManager.AppSettings["SerializeDataPath"];
        /// <summary>
        /// XML序列化器
        /// </summary>
        public List<Programmer> XmlSerialize()
        {

            //使用XML序列化对象
            string fileName = Path.Combine(SerializeDataPath, @"Plots.xml");//文件名称与路径
            using (Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Programmer>));//创建XML序列化器，需要指定对象的类型
                //使用XML反序列化对象
                fStream.Position = 0;//重置流位置
                List<Programmer> pList = pList = (List<Programmer>)xmlFormat.Deserialize(fStream);
                return pList;
            }
        }
        [Serializable]  
        public class Programmer
        {
            public string Name { get; set; }
            public string Plot { get; set; }
        }
    }
}
