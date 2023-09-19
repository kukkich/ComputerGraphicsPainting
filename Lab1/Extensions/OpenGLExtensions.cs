using SharpGL;
using System.Windows.Media;

namespace Lab1.Extensions;

// ReSharper disable once InconsistentNaming
public static class OpenGLExtensions
{
    public static void Color(this OpenGL gl, Color color)
    {
        gl.Color(color.R, color.G, color.B, color.A);
    }
}