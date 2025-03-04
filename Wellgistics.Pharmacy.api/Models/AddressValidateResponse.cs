using System.Xml.Serialization;

namespace Wellgistics.Pharmacy.api.Models
{
    [XmlRoot("AddressValidateResponse")]
    public class AddressValidateResponse
    {
        [XmlElement("Address")]
        public Address Address { get; set; }
    }

    public class Address
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        // Success case properties
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip5 { get; set; }
        public string Zip4 { get; set; }
        public string DeliveryPoint { get; set; }
        public string CarrierRoute { get; set; }
        public string Footnotes { get; set; }
        public string DPVConfirmation { get; set; }
        public string DPVCMRA { get; set; }
        public string DPVFootnotes { get; set; }
        public string Business { get; set; }
        public string CentralDeliveryPoint { get; set; }
        public string Vacant { get; set; }

        // Error case properties
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Number { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string HelpFile { get; set; }
        public string HelpContext { get; set; }
    }
}
