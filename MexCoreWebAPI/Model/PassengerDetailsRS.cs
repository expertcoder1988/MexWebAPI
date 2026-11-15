using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MalikExpressMVC.Models.XML.PNR
{


    [XmlRoot(ElementName = "PartyId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class PartyId
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "From", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class From
    {
        [XmlElement(ElementName = "PartyId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public PartyId PartyId { get; set; }
    }

    [XmlRoot(ElementName = "To", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class To
    {
        [XmlElement(ElementName = "PartyId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public PartyId PartyId { get; set; }
    }

    [XmlRoot(ElementName = "Service", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class Service
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "MessageData", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class MessageData
    {
        [XmlElement(ElementName = "MessageId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string MessageId { get; set; }
        [XmlElement(ElementName = "Timestamp", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Timestamp { get; set; }
        [XmlElement(ElementName = "RefToMessageId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string RefToMessageId { get; set; }
    }

    [XmlRoot(ElementName = "MessageHeader", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class MessageHeader
    {
        [XmlElement(ElementName = "From", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public From From { get; set; }
        [XmlElement(ElementName = "To", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public To To { get; set; }
        [XmlElement(ElementName = "CPAId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string CPAId { get; set; }
        [XmlElement(ElementName = "ConversationId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string ConversationId { get; set; }
        [XmlElement(ElementName = "Service", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public Service Service { get; set; }
        [XmlElement(ElementName = "Action", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Action { get; set; }
        [XmlElement(ElementName = "MessageData", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public MessageData MessageData { get; set; }
        [XmlAttribute(AttributeName = "eb", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Eb { get; set; }
        [XmlAttribute(AttributeName = "version", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "mustUnderstand", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public string MustUnderstand { get; set; }
    }

    [XmlRoot(ElementName = "BinarySecurityToken", Namespace = "http://schemas.xmlsoap.org/ws/2002/12/secext")]
    public class BinarySecurityToken
    {
        [XmlAttribute(AttributeName = "valueType")]
        public string ValueType { get; set; }
        [XmlAttribute(AttributeName = "EncodingType")]
        public string EncodingType { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Security", Namespace = "http://schemas.xmlsoap.org/ws/2002/12/secext")]
    public class Security
    {
        [XmlElement(ElementName = "BinarySecurityToken", Namespace = "http://schemas.xmlsoap.org/ws/2002/12/secext")]
        public BinarySecurityToken BinarySecurityToken { get; set; }
        [XmlAttribute(AttributeName = "wsse", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Wsse { get; set; }
    }

    [XmlRoot(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Header
    {
        [XmlElement(ElementName = "MessageHeader", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public MessageHeader MessageHeader { get; set; }
        [XmlElement(ElementName = "Security", Namespace = "http://schemas.xmlsoap.org/ws/2002/12/secext")]
        public Security Security { get; set; }
    }

    [XmlRoot(ElementName = "Success", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
    public class Success
    {
        [XmlAttribute(AttributeName = "timeStamp")]
        public string TimeStamp { get; set; }
    }

    [XmlRoot(ElementName = "Message", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
    public class Message
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SystemSpecificResults", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
    public class SystemSpecificResults
    {
        [XmlElement(ElementName = "Message", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
        public List<Message> Message { get; set; }
    }

    [XmlRoot(ElementName = "Warning", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
    public class Warning
    {
        [XmlElement(ElementName = "SystemSpecificResults", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
        public SystemSpecificResults SystemSpecificResults { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "timeStamp")]
        public string TimeStamp { get; set; }
    }

    [XmlRoot(ElementName = "ApplicationResults", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
    public class ApplicationResults
    {
        [XmlElement(ElementName = "Success", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
        public Success Success { get; set; }
        [XmlElement(ElementName = "Warning", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
        public Warning Warning { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
    }

    [XmlRoot(ElementName = "ItineraryRef", Namespace = "http://services.sabre.com/sp/pd/v3_2")]
    public class ItineraryRef
    {
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "PassengerDetailsRS", Namespace = "http://services.sabre.com/sp/pd/v3_2")]
    public class PassengerDetailsRS
    {
        [XmlElement(ElementName = "ApplicationResults", Namespace = "http://services.sabre.com/STL_Payload/v02_01")]
        public ApplicationResults ApplicationResults { get; set; }
        [XmlElement(ElementName = "ItineraryRef", Namespace = "http://services.sabre.com/sp/pd/v3_2")]
        public ItineraryRef ItineraryRef { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "PassengerDetailsRS", Namespace = "http://services.sabre.com/sp/pd/v3_2")]
        public PassengerDetailsRS PassengerDetailsRS { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "soap-env", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soapenv { get; set; }
    }
   
}