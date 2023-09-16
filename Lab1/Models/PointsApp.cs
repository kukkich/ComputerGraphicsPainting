using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using Lab1.Models.Actions;
using Lab1.Models.Controls;
using Lab1.ViewModels;
using SharpGL;
using SharpGL.WPF;

namespace Lab1.Models;

public class PointsApp
{
    public PointContext PointContext { get; }
    public InputControl InputControl { get; }
    public float[] Color { get; }
    public AppState State
    {
        get => _state;
        set
        {
            _state = value;
            StateString = value.ToString();
            _view.State = StateString;
        }
    }

    public string StateString { get; set; }

    private AppState _state;

    private readonly List<IAction> _actions;
    private readonly List<IAction> _undoActions;

    private OpenGLControl _glControl = null!;
    private OpenGL _glContext = null!;
    public bool RenderScheduled = false;
    private readonly Timer _renderTimer;
    private readonly Dispatcher _dispatcher;
    private readonly PointsAppView _view;

    public PointsApp(Dispatcher dispatcher, PointsAppView view)
    {
        _dispatcher = dispatcher;
        _view = view;

        PointContext = new PointContext(view);
        _actions = new List<IAction>(5);
        _undoActions = new List<IAction>(5);

        InputControl = new InputControl(this);

        _renderTimer = new Timer(RenderTimerCallback!, null, 0, 20);

        Color = new float[] { 0.1f, 0.3f, 0.6f };
    }

    public void PushAction(IAction action)
    {
        if (_actions.Count == 10)
        {
            _actions.RemoveAt(0);
        }
        _undoActions.Clear();
        _actions.Add(action);
        action.Do();

        InputControl.ForceChangeState(State);

        RenderScheduled = true;
    }

    public void UndoAction()
    {
        if (_actions.Count == 0)
        {
            return;
        }
        var action = _actions[^1];
        action.Undo();
        _actions.RemoveAt(_actions.Count - 1);
        _undoActions.Add(action);

        InputControl.ForceChangeState(State);

        RenderScheduled = true;
    }

    public void RedoAction()
    {
        if (_undoActions.Count > 0)
        {
            var action = _undoActions[^1];
            action.Do();
            _undoActions.RemoveAt(_undoActions.Count - 1);
            _actions.Add(action);

            InputControl.ForceChangeState(State);

            RenderScheduled = true;
        }
    }

    // ReSharper disable once InconsistentNaming
    public void InitOpenGL(OpenGLControl gl, OpenGLRoutedEventArgs args)
    {
        _glControl = gl;
        _glContext = args.OpenGL;

        gl.RenderTrigger = RenderTrigger.TimerBased;
        _glContext.ClearColor(0.4f, 0.4f, 0.5f, 0.3f);
    }

    public void Render(OpenGLRoutedEventArgs args)
    {
        var gl = args.OpenGL;
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        PointF? hoveredPoint = null;

        for (var i = 0; i < PointContext.Groups.Count; i++)
        {
            var group = PointContext.Groups[i];

            gl.Begin(OpenGL.GL_TRIANGLE_FAN);
            gl.Color(Color);

            foreach (var p in group.Points)
            {
                if (State == AppState.SelectingPointToEdit)
                {
                    var cursor = PointContext.Cursor.Position;
                    if (MathF.Abs(cursor.X - p.X) < 5e-2
                        && MathF.Abs(cursor.Y - p.Y) < 5e-2)
                    {
                        hoveredPoint = p;
                    }
                }
                gl.Vertex(p.X, p.Y);
            }

            if (State == AppState.PointPlacement && i == PointContext.Groups.Count - 1)
            {
                gl.Vertex(
                    PointContext.Cursor.Position.X,
                    PointContext.Cursor.Position.Y
                );
            }
            gl.End();
            if (i == PointContext.Groups.Count - 1 && State != AppState.Initial)
            {
                gl.LineWidth(3);
                gl.PointSize(5);

                #region Контур

                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(new float[] { 0, 0, 0, 0.1f });
                foreach (var p in group.Points)
                {
                    gl.Vertex(p.X, p.Y);
                }
                if (State == AppState.PointPlacement)
                {
                    gl.Vertex(
                        PointContext.Cursor.Position.X,
                        PointContext.Cursor.Position.Y
                    );
                }
                gl.End();

                #endregion

                #region Вершины

                gl.Begin(OpenGL.GL_POINTS);
                gl.Color(new float[] { 1f, 1f, 1f, 0.6f });
                foreach (var p in group.Points)
                {
                    gl.Vertex(p.X, p.Y);
                }
                if (State == AppState.PointPlacement)
                {
                    gl.Vertex(
                        PointContext.Cursor.Position.X,
                        PointContext.Cursor.Position.Y
                    );
                }
                gl.End();

                #endregion

                gl.LineWidth(1);
                gl.PointSize(1);
                gl.Color(Color);
            }
        }

        if (hoveredPoint is not null)
        {
            gl.PointSize(9);
            gl.Color(new float[] { 0f, 1f, 0f, 0.6f });

            gl.Begin(OpenGL.GL_POINTS);

            gl.Vertex(hoveredPoint.Value.X, hoveredPoint.Value.Y);

            gl.End();

            gl.PointSize(9);
            gl.Color(Color);

        }
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