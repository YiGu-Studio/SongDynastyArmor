using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace LPConfigView.Helper
{
    /// <summary>
    /// XML序列化公共处理类
    /// </summary>
    public static class XmlSerializeHelper
    {
        /// <summary>
        /// 将实体对象转换成XML
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">实体对象</param>
        public static void XmlSerialize<T>(T obj,string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    Type t = obj.GetType();
                    XmlSerializer serializer = new XmlSerializer(t);
                    serializer.Serialize(writer, obj);
                    writer.Close();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }



        public static bool DeserializeConfigObject<T>(ref T objConfig, string configFile, ref string strError)
        {
            objConfig = default(T);
            strError = string.Empty;
            if (!System.IO.File.Exists(configFile))
            {
                strError = string.Format("Config file not exist.\t{0}", configFile);
                return false;
            }
            try
            {
                Stream fileStream = new FileStream(configFile, FileMode.Open, FileAccess.Read);
                XmlReader xmlReader = new XmlTextReader(fileStream);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                objConfig = (T)xmlSerializer.Deserialize(xmlReader);
                xmlReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

     

    }
}
