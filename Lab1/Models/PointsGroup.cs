using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace Lab1.Models;

public class PointsGroup
{
    public List<PointF> Points { get; set; } = new();
    public Color Color { get; set; } = Color.FromRgb(100, 149, 237);

    public PointsGroup() { }
    public PointsGroup(Color color)
    {
        Color = color;
    }
}