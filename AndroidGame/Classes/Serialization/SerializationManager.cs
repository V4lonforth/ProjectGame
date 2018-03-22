using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;

namespace AndroidGame.Serialization
{
    public class SerializationManager
    {
        const string infoPath = "Content/Info/";
        
        public T[] LoadInfo<T>(string fileName)
        {
            using (Stream stream = Game.Activity.Assets.Open(infoPath + fileName + ".xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]));
                T[] tInfo = (T[])xmlSerializer.Deserialize(stream);
                foreach (T projectileInfo in tInfo)
                    if (typeof(ISerializationInfo).IsAssignableFrom(typeof(T)))
                        ((ISerializationInfo)projectileInfo).Initialize();
                return tInfo;
            }
        }

        public void Save<T>(string fileName, T[] data)
        {
            using (StreamWriter stream = new StreamWriter(infoPath + fileName + ".xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]));
                xmlSerializer.Serialize(stream, data);
            }
        }
    }
}