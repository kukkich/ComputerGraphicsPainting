using SharpGL.WPF;
using System.Windows.Input;

namespace Lab1.Models.Controls;

public interface IInputControlState
{
    void OnMouseHover(OpenGLControl glControl, MouseEventArgs e);
    void OnMouseLeave(OpenGLControl glControl, MouseEventArgs e);
    void OnMouseEnter(OpenGLControl glControl, MouseEventArgs e);
    void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e);
    void OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e);
    void OnUndoButton();
    void OnRedoButton();
    void OnEditModeToggle();
}