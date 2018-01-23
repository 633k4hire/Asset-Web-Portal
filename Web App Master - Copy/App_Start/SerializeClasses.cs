

namespace Serializers
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml.Serialization;

    //BINARY
    [Serializable]
    public class MemoryBinarySerializer
    {
        public byte[] WriteMemory()
        {            
            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, this);
                bytes = stream.ToArray();
            }
            return bytes;
        }
        public static object ReadMemory(byte[] bytes)
        {
            object obj = null;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                //stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                BinaryFormatter bf = new BinaryFormatter();
                obj = (object)bf.Deserialize(stream);
            }
            return obj;
        }
    }
    [Serializable]
    public class FileBinarySerializer
    {
        public void Write(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, this);
                }
            }
            catch { }
        }
        public static object Read(string path)
        {
            object obj = new object();
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                obj = (object)formatter.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return obj;
        }
    }
    [Serializable]
    public class BinarySerializer : FileBinarySerializer
    {
        public byte[] WriteMemory()
        {
            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, this);
                bytes = stream.ToArray();
            }
            return bytes;
        }
        public static object ReadMemory(byte[] bytes)
        {
            object obj = null;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                //stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                BinaryFormatter bf = new BinaryFormatter();
                obj = (object)bf.Deserialize(stream);
            }
            return obj;
        }
    }   

    //XML
    [Serializable]
    public class MemoryXmlSerializer
    {
        public class XmlAttributeEvent : EventArgs
        {
            public XmlAttributeEvent(XmlAttributeEventArgs obj = null)
            {
                if (obj != null)
                {
                    Data = obj;
                }
            }
            public XmlAttributeEventArgs Data { get; set; }
        }
        public class XmlNodeEvent : EventArgs
        {
            public XmlNodeEvent(XmlNodeEventArgs obj = null)
            {
                if (obj != null)
                {
                    Data = obj;
                }
            }
            public XmlNodeEventArgs Data { get; set; }
        }

        protected virtual void newXmlAttributeEvent(XmlAttributeEvent connectionEvent)
        {
            EventHandler<XmlAttributeEvent> handler = XmlAttributeEventOccured;
            if (handler != null)
            {
                handler(this, connectionEvent);
            }
        }
        public event EventHandler<XmlAttributeEvent> XmlAttributeEventOccured;

        protected virtual void newXmlNodeEvent(XmlNodeEvent connectionEvent)
        {
            EventHandler<XmlNodeEvent> handler = XmlNodeEventOccured;
            if (handler != null)
            {
                handler(this, connectionEvent);
            }
        }
        public event EventHandler<XmlNodeEvent> XmlNodeEventOccured;


        public byte[] WriteMemory(Type type)
        {
            
            byte[] bytes=null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer xmlSerial = new XmlSerializer(type);
                    xmlSerial.Serialize(stream, this);
                    bytes = stream.ToArray();
                }
            }
            catch { }
            return bytes;
        }
        public object ReadMemory(Type type, byte[] bytes)
        {
            object obj = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                serializer.UnknownNode += new
                XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new
                XmlAttributeEventHandler(serializer_UnknownAttribute);
                
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    obj = (object)serializer.Deserialize(stream);
                }
            }
            catch { }
            return obj;
        }
        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            newXmlNodeEvent(new XmlNodeEvent(e));
        }

        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            newXmlAttributeEvent(new XmlAttributeEvent(e));
        }
    }
    
}
