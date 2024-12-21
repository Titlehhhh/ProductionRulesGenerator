using System.Xml.Serialization;

namespace XmlModels;

[XmlRoot(ElementName="mxGraphModel")]

public class MxGraphModel { 

    [XmlElement(ElementName="root")]
    public Root? Root { get; set; } 

    [XmlAttribute(AttributeName="dx")] 
    public double Dx { get; set; } 

    [XmlAttribute(AttributeName="dy")] 
    public double Dy { get; set; } 

    [XmlAttribute(AttributeName="grid")] 
    public double Grid { get; set; } 

    [XmlAttribute(AttributeName="gridSize")] 
    public double GridSize { get; set; } 

    [XmlAttribute(AttributeName="guides")] 
    public int Guides { get; set; } 

    [XmlAttribute(AttributeName="tooltips")] 
    public int Tooltips { get; set; } 

    [XmlAttribute(AttributeName="connect")] 
    public int Connect { get; set; } 

    [XmlAttribute(AttributeName="arrows")] 
    public int Arrows { get; set; } 

    [XmlAttribute(AttributeName="fold")] 
    public int Fold { get; set; } 

    [XmlAttribute(AttributeName="page")] 
    public int Page { get; set; } 

    [XmlAttribute(AttributeName="pageScale")] 
    public int PageScale { get; set; } 

    [XmlAttribute(AttributeName="pageWidth")] 
    public int PageWidth { get; set; } 

    [XmlAttribute(AttributeName="pageHeight")] 
    public int PageHeight { get; set; } 

    [XmlAttribute(AttributeName="background")] 
    public string? Background { get; set; } 

    [XmlAttribute(AttributeName="math")] 
    public double Math { get; set; } 

    [XmlAttribute(AttributeName="shadow")] 
    public double Shadow { get; set; } 
}