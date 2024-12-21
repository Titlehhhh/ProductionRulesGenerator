using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace XmlModels;

[XmlRoot(ElementName = "root")]
public class Root
{
    [XmlElement(ElementName = "mxCell")] public List<MxCell> MxCells { get; set; }
}