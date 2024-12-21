using System.Xml.Serialization;

namespace XmlModels;

[XmlRoot(ElementName="mxCell")]
public class MxCell { 

    [XmlAttribute(AttributeName="id")] 
    public string? Id { get; set; } 
    [XmlAttribute(AttributeName="value")] 
    public string? Value { get; set; } 
    
    [XmlAttribute(AttributeName="style")] 
    public string? Style { get; set; } 
    
    [XmlAttribute(AttributeName="parent")] 
    public string? Parent { get; set; } 

    [XmlAttribute(AttributeName="vertex")] 
    public int Vertex { get; set; } 
    
    [XmlAttribute(AttributeName="edge")] 
    public string? Edge { get; set; } 

    

    [XmlAttribute(AttributeName="source")] 
    public string? Source { get; set; } 
    
    [XmlAttribute(AttributeName="target")] 
    public string? Target { get; set; } 

    
    [XmlAttribute(AttributeName="connectable")] 
    public string? Connectable { get; set; } 
    [XmlElement(ElementName="mxGeometry")] 
    public MxGeometry? MxGeometry { get; set; } 

    

   

    

    
}