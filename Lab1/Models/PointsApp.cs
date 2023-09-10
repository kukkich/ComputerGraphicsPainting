using System;
using System.Collections.Generic;
using System.Threading;
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
    public byte[] Color { get; }

    private readonly List<IAction> _actions;
    private readonly List<IAction> _undoActions;

    private OpenGLControl _glControl = null!;
    private OpenGL _glContext = null!;
    public bool RenderScheduled = false;
    private readonly Timer _renderTimer;
    private readonly Dispatcher _dispatcher;

    public PointsApp(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;

        PointContext = new PointContext();
        _actions = new List<IAction>(5);
        _undoActions = new List<IAction>(5);

        InputControl = new InputControl(this);

        _renderTimer = new Timer(RenderTimerCallback, null, 0, 20);

        Color = new byte[] { 0, 1, 0 };
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
        ForceRender();
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

        _glContext.ClearColor(0.5f, 0.3f, 0.3f, 0.3f);
    }

    public void Render(OpenGLRoutedEventArgs args)
    {
        
    }

    public void ForceRender()
    {
        _dispatcher.Invoke(_glControl.DoRender);
    }

    private void RenderTimerCallback(object state)
    {
        if (!RenderScheduled) return;

        _dispatcher.Invoke(_glControl.DoRender);
        RenderScheduled = false;
    }
}