using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : NPCState
{
    NPC _robot;

    public NPCIdle(StateMachine sm, NPC robot) : base(sm)
    {
        _robot = robot;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        _robot.SetNoVelocity();
    }

    public override void OnExecute()
    {
        base.OnExecute();
    }

    public override void OnLateExecute()
    {
        base.OnLateExecute();
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }
}
