﻿using Lab1.Models.Controls.States;
using SharpGL.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace Lab1.Models.Controls;

public class InputControl : IInputControlState
{
    private readonly PointsApp _app;
    private IInputControlState _state;

    public InputControl(PointsApp app)
    {
        _app = app;
        _state = new InitialState(app);
    }

    public void OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        _state.OnMouseHover(glControl, e);
    }

    public void OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        _state.OnMouseLeave(glControl, e);
    }

    public void OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        _state.OnMouseEnter(glControl, e);
    }

    public void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _state.OnLeftClick(glControl, e);
    }

    public void OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _state.OnRightClick(glControl, e);
    }

    public void OnUndoButton()
    {
        _state.OnUndoButton();
    }

    public void OnRedoButton()
    {
        _state.OnRedoButton();
    }

    public void OnEditModeToggle()
    {
        _state.OnEditModeToggle();
    }

    public void OnCurrentGroupChanged(int newGroupIndex)
    {
        try
        {
            _state.OnCurrentGroupChanged(newGroupIndex);
        }
        catch (InvalidOperationException e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void ForceChangeState(AppState state)
    {
        _state = state switch
        {
            AppState.Initial => new InitialState(_app),
            AppState.PointPlacement => new PointPlacementState(_app),
            AppState.PointEditing => new PointEditingState(_app),
            AppState.SelectingPointToEdit => new SelectingPointToEditState(_app),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}