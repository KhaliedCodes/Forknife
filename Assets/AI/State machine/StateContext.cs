using UnityEngine;

public class StateContext
{
    private IState _currentState;

    public IState GetCurrentState()
    {
        return _currentState;
    }

    public void SetState(IState state)
    {
        _currentState?.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void ExecuteState()
    {
        if (_currentState != null)
        {
            _currentState.Execute();
        }
    }
}
