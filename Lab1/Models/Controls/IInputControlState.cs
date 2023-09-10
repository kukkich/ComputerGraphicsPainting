using SharpGL.WPF;
using System.Windows.Input;

namespace Lab1.Models.Controls;

public interface IInputControlState
{
    IInputControlState OnMouseHover(OpenGLControl glControl, MouseEventArgs e);
    IInputControlState OnMouseLeave(OpenGLControl glControl, MouseEventArgs e);
    IInputControlState OnMouseEnter(OpenGLControl glControl, MouseEventArgs e);
    IInputControlState OnClick(OpenGLControl glControl, MouseButtonEventArgs e);
    IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e);
}