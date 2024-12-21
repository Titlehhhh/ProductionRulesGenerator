using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlModels;

[XmlRoot(ElementName="mxGeometry")]
public class MxGeometry { 

	[XmlAttribute(AttributeName="x")] 
	public double X { get; set; } 

	[XmlAttribute(AttributeName="y")] 
	public double Y { get; set; } 

	[XmlAttribute(AttributeName="width")] 
	public double Width { get; set; } 

	[XmlAttribute(AttributeName="height")] 
	public double Height { get; set; } 

	[XmlAttribute(AttributeName="as")] 
	public string? As { get; set; } 

	[XmlElement(ElementName="mxPoint")] 
	public List<MxPoint> MxPoints { get; set; } 

	[XmlAttribute(AttributeName="relative")] 
	public string Relative { get; set; } 
}