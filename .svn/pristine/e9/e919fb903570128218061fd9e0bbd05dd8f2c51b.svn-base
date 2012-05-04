using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GBSFormatManager;

namespace FormatManager.Serializer
{
    /// <summary>
    /// Main class that define all the shared function to manipulate and manage the XML code
    /// </summary>
    public class XmlManager
    {
        /// <summary>
        /// Serializes the specified data to serialize.
        /// </summary>
        /// <param name="dataToSerialize">The data to serialize.</param>
        /// <param name="flow">The flow.</param>
        /// <exception cref="InvalidOperationException">In case a serializzation error occur</exception>
        static public void Serialize<TFormat>(TFormat dataToSerialize, ref Stream flow)
        {
            var serializer = new XmlSerializer(typeof(TFormat));
            serializer.Serialize(flow, dataToSerialize);
        }

        static public void Serialize<TFormat>(TFormat dataToSerialize, ref XmlWriter writer) where TFormat : class
        {
            if (dataToSerialize != null)
            {
                new XmlSerializer(typeof(TFormat)).Serialize(writer, dataToSerialize);
            }
        }

        /// <summary>
        /// Serializes the specified data to serialize.
        /// </summary>
        /// <param name="dataToSerialize">The data to serialize.</param>
        /// <param name="flow">The flow.</param>
        /// <exception cref="InvalidOperationException">If a serialization error occur</exception>
        static public void Serialize<TFormat>(TFormat dataToSerialize, ref FileStream flow)
        {
            var serializer = new XmlSerializer(typeof(TFormat));
            serializer.Serialize(flow, dataToSerialize);
        }

        /// <summary>
        /// Deserializes the specified data to deserialize.
        /// </summary>
        /// <typeparam name="TFormat">The type of the format.</typeparam>
        /// <param name="dataToDeserialize">The data to deserialize.</param>
        /// <param name="flow">The flow.</param>
        /// <returns>the intended object to be deserialized</returns>
        static public void Deserialize<TFormat>(ref TFormat dataToDeserialize, Stream flow)
        {
            var deserializer = new XmlSerializer(typeof(TFormat));
            dataToDeserialize = (TFormat)deserializer.Deserialize(flow);
        }

        public FileStream PrepareFileStream(String filePath, FileMode mode)
        {
            return new FileStream(filePath, mode);
        }
    }
}
