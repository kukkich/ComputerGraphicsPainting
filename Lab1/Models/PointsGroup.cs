using System.Collections.Generic;
using System.Drawing;

namespace Lab1.Models;

public class PointsGroup
{
    public List<PointF> Points { get; set; } = new();
    public float[] Color { get; set; } = {0f, 0f, 0f};

    public PointsGroup() { }

    public PointsGroup(float[] color)
    {
        Color = color;
    }
}