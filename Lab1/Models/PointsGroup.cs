using System.Collections.Generic;
using System.Drawing;

namespace Lab1.Models;

public class PointsGroup
{
    public List<PointF> Points { get; set; } = new();
    public float[] Color { get; set; } = { 0.1f, 0.3f, 0.6f };

    public PointsGroup() { }

    public PointsGroup(float[] color)
    {
        Color = color;
    }
}