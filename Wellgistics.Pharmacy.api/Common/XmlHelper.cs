using System.Xml.Serialization;

namespace Wellgistics.Pharmacy.api.Common
{
    public class XmlHelper
    {
        public static T DeserializeXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        public static string ObjectToXml(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());

            using (var stringWriter = new StringWriter())
            {
                using (var writer = new System.Xml.XmlTextWriter(stringWriter))
                {
                    writer.Formatting = System.Xml.Formatting.Indented;
                    serializer.Serialize(writer, obj);
                    return stringWriter.ToString();
                }
            }
        }
        public static string ObjectToXmlWithoutDeclaration(object obj)
        {
            // Get the XML string from the existing ObjectToXml method
            string xml = ObjectToXml(obj);

            // Remove the XML declaration if it exists
            int declarationEndIndex = xml.IndexOf("?>");
            if (declarationEndIndex >= 0)
            {
                // Remove the XML declaration part (everything before and including the '?>')
                xml = xml.Substring(declarationEndIndex + 2).TrimStart();
            }

            return xml;
        }
    }
}
