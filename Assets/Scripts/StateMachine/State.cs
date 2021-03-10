using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine _sm;

    public State(StateMachine sm)
    {
        _sm = sm;
    }

    public abstract void OnAwake();
    public abstract void OnExecute();
    public abstract void OnLateExecute();
    public abstract void OnSleep();



}
