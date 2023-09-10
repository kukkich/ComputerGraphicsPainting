using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using Lab1.Models.Actions;
using Lab1.Models.Controls;
using SharpGL;
using SharpGL.WPF;

namespace Lab1.Models;

public class PointsApp
{
    public PointContext PointContext { get; }
    public InputControl InputControl { get; }

    private readonly List<IAction> _actions;
    private readonly List<IAction> _undoActions;

    private OpenGLControl _glControl;
    private OpenGL _glContext;
    private bool _renderScheduled = false;
    private readonly Timer _renderTimer;
    private readonly byte[] _color;
    private readonly Dispatcher _dispatcher;

    public PointsApp(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;

        PointContext = new PointContext();
        _actions = new List<IAction>(5);
        _undoActions = new List<IAction>(5);

        InputControl = new InputControl(PointContext);

        _renderTimer = new Timer(RenderTimerCallback, null, 0, 20);

        _color = new byte[] { 0, 1, 0 };
    }

    public void PushAction(IAction action)
    {
        if (_actions.Count == 5)
        {
            _actions.RemoveAt(0);
        }
        _undoActions.Clear();
        _actions.Add(action);
        action.Do();
    }

    public void UndoAction()
    {
        var action = _actions[^1];
        action.Undo();
        _actions.RemoveAt(_actions.Count - 1);
        _undoActions.Add(action);
    }

    public void RedoAction()
    {
        if (_undoActions.Count > 0)
        {
            var action = _undoActions[^1];
            action.Do();
            _undoActions.RemoveAt(_undoActions.Count - 1);
            _actions.Add(action);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    // ReSharper disable once InconsistentNaming
    public void InitOpenGL(OpenGLControl gl, OpenGLRoutedEventArgs args)
    {
        _glControl = gl;
        _glContext = args.OpenGL;

        _glContext.ClearColor(0.3f, 0.3f, 0.3f, 0.3f);
    }

    public void Render(OpenGLRoutedEventArgs args)
    {
        var points = PointContext.CurrentGroup;

        var gl = args.OpenGL;
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
        gl.Color(_color);

        foreach (var p in points)
        {
            gl.Vertex(p.X, p.Y);
        }

        if (PointContext.CursorPosition is not null)
        {
            gl.Vertex(
                PointContext.CursorPosition.Value.X,
                PointContext.CursorPosition.Value.Y
            );
        }

        gl.End();
    }

    private void RenderTimerCallback(object state)
    {
        if (!_renderScheduled) return;

        _dispatcher.Invoke(_glControl.DoRender);
        _renderScheduled = false;
    }
}