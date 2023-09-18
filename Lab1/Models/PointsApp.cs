using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Lab1.Extensions;
using Lab1.Models.Actions;
using Lab1.Models.Controls;
using Lab1.ViewModels;
using SharpGL;
using SharpGL.WPF;
using Color = System.Windows.Media.Color;

// ReSharper disable PossibleMultipleEnumeration

namespace Lab1.Models;

public class PointsApp
{
    public PointContext PointContext { get; }
    public InputControl InputControl { get; }

    public AppState State
    {
        get => _state;
        set
        {
            if (value == AppState.Initial)
            {
                PointContext.Unselect();
            }
            else if (_state is AppState.Initial)
            {
                PointContext.ReturnSelection();
            }
            _state = value;
            _view.State = value.ToString();
        }
    }
    private AppState _state;

    private readonly List<IAction> _actions;
    private readonly List<IAction> _undoActions;

    private OpenGLControl _glControl = null!;
    private OpenGL _glContext = null!;
    private readonly Dispatcher _dispatcher;
    
    private bool _renderScheduled = false;
    private readonly Timer _renderTimer;

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

        State = AppState.Initial;
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

        _renderScheduled = true;
    }

    public void UndoAction()
    {
        if (_actions.Count == 0) return;

        var action = _actions[^1];
        action.Undo();
        _actions.RemoveAt(_actions.Count - 1);
        _undoActions.Add(action);

        InputControl.ForceChangeState(State);

        _renderScheduled = true;
    }

    public void RedoAction()
    {
        if (_undoActions.Count <= 0) return;

        var action = _undoActions[^1];
        action.Do();
        _undoActions.RemoveAt(_undoActions.Count - 1);
        _actions.Add(action);

        InputControl.ForceChangeState(State);

        _renderScheduled = true;
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
        OpenGL gl = args.OpenGL;
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        void DrawPoints(IEnumerable<PointF> points)
        {
            foreach (var p in points)
            {
                gl.Vertex(p.X, p.Y);
            }
        }

        void DrawCursor()
        {
            gl.Vertex(
                PointContext.Cursor.Position.X,
                PointContext.Cursor.Position.Y
            );
        }

        void DrawOutline(IEnumerable<PointF> points)
        {
            // TODO Удалить когда PointContext.CurrentGroup
            // станет возвращать выделенную группу
            // и null если не выделена

            if (State == AppState.Initial) return;

            gl.LineWidth(3);
            gl.PointSize(5);

            #region Контур

            gl.Begin(OpenGL.GL_LINE_LOOP);
            gl.Color(new float[] { 0, 0, 0, 0.1f });
            DrawPoints(points);
            if (State == AppState.PointPlacement)
            {
                DrawCursor();
            }

            gl.End();

            #endregion

            #region Вершины

            gl.Begin(OpenGL.GL_POINTS);
            gl.Color(new float[] { 1f, 1f, 1f, 0.6f });
            DrawPoints(points);
            if (State == AppState.PointPlacement)
            {
                DrawCursor();
            }

            gl.End();

            #endregion

            gl.LineWidth(1);
            gl.PointSize(1);
        }

        void HighlightClosestPoint(PointF? hoveredPoint)
        {
            if (hoveredPoint is null) return;

            gl.PointSize(9);

            gl.Begin(OpenGL.GL_POINTS);
            gl.Color(new[] { 0f, 1f, 0f, 0.6f });

            gl.Vertex(hoveredPoint.Value.X, hoveredPoint.Value.Y);

            gl.End();

            gl.PointSize(1);
        }

        void DrawEditingGroup()
        {
            var group = PointContext.CurrentGroup!;

            gl.Begin(OpenGL.GL_TRIANGLE_FAN);
            gl.Color(group.Color);

            var points = group.Points.Select((p, index) =>
                index ==  PointContext.EditingPointIndex!
                    ? PointContext.Cursor.Position
                    : p
            );
            DrawPoints(points);
            gl.End();

            DrawOutline(points);

            HighlightClosestPoint(PointContext.Cursor.Position);
        }

        foreach (var group in PointContext.Groups.Where(x => x != PointContext.CurrentGroup))
        {
            gl.Begin(OpenGL.GL_TRIANGLE_FAN);
            gl.Color(group.Color);

            DrawPoints(group.Points);

            gl.End();
        }

        var currentGroup = PointContext.CurrentGroup;
        if (currentGroup is not null)
        {
            if (State == AppState.PointEditing)
            {
                DrawEditingGroup();
            }
            else
            {
                gl.Begin(OpenGL.GL_TRIANGLE_FAN);
                gl.Color(currentGroup.Color);

                DrawPoints(currentGroup.Points);
                if (State == AppState.PointPlacement)
                {
                    DrawCursor();
                }
                gl.End();

                DrawOutline(currentGroup.Points);
            }

            if (State == AppState.SelectingPointToEdit)
            {
                HighlightClosestPoint(PointContext.ClosestPoint);
            }
        }
    }

    public void ForceRender()
    {
        _renderScheduled = true;
    }

    public void SelectNewColor(Color newColor)
    {
        _view.ColorPicker.SelectedColor = newColor;
        PointContext.SelectNewColor(newColor);
    }

    private void RenderTimerCallback(object state)
    {
        if (!_renderScheduled) return;

        _dispatcher.Invoke(_glControl.DoRender);
        _renderScheduled = false;
    }
}