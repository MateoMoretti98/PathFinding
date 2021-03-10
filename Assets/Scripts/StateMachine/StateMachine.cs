using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    State _currentState;

    List<State> _allStates = new List<State>();
    
    public void Update()
    {
        if(_currentState != null)
        {
            _currentState.OnExecute();
        }
    }

    public void LateUpdate()
    {
        if(_currentState != null)
        {
            _currentState.OnLateExecute();
        }
    }

    public void AddState(State s)
    {
        if(s != null)
        {
            _allStates.Add(s);
            if(_currentState == null)
            {
                _currentState = s;
            }
        }
    }

    public void SetState<T>()
    {
        foreach (var state in _allStates)
        {
            if (state.GetType() == typeof(T))
            {
                _currentState.OnSleep();
                _currentState = state;
                _currentState.OnAwake();
                return;
            }
        }
    }

    public bool IsActualState<T>()
    {
        return _currentState.GetType() == typeof(T);
    }
}
