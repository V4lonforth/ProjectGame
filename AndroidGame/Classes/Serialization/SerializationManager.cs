using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;

namespace AndroidGame.Serialization
{
    static class SerializationManager
    {
        const string infoPath = "Content/Info/";
        
        public static T[] LoadInfo<T>(string fileName)
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
    }
}