using System;

namespace NotebookChoice.Models;

public class SelectedPc
{
    public string CPU { get; set; }
    public string GraphicsCard { get; set; }
    public string RAM { get; set; }
    public string Motherboard { get; set; }
    public string PowerSupply { get; set; }
    public string Storage { get; set; }
    public string CoolingSystem { get; set; }
    public string Case { get; set; }
    public Uri Image { get; set; }
}
