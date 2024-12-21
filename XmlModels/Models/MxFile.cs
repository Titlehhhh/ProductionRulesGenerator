using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace XmlModels;

[XmlRoot(ElementName="mxfile")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class MxFile { 
    [XmlElement(ElementName="diagram")] 
    public List<Diagram> Diagram { get; set; } 

    [XmlAttribute(AttributeName="host")] 
    public string? Host { get; set; } 

    [XmlAttribute(AttributeName="agent")] 
    public string? Agent { get; set; } 

    [XmlAttribute(AttributeName="version")] 
    public string? Version { get; set; } 

    [XmlAttribute(AttributeName="pages")] 
    public int Pages { get; set; } 
}