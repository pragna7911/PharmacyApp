using System.Xml.Serialization;

namespace Wellgistics.Pharmacy.api.Models
{
    public class AddressValidateRequest
    {
        [XmlAttribute("USERID")]
        public string USERID { get; set; }

        public string Revision { get; set; }
        public AddressModel Address { get; set; }
    }

    public class AddressModel
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip5 { get; set; }
        public string Zip4 { get; set; }
    }
}
