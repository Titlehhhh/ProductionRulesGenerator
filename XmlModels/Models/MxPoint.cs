using System.Xml.Serialization;

namespace XmlModels;

[XmlRoot(ElementName="mxPoint")]
public class MxPoint { 

    [XmlAttribute(AttributeName="x")] 
    public double X { get; set; } 

    [XmlAttribute(AttributeName="y")] 
    public double Y { get; set; } 

    [XmlAttribute(AttributeName="as")] 
    public string? As { get; set; } 
}