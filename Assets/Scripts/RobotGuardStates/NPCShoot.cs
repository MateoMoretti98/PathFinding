using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShoot : NPCState
{
    NPC _robot;

    public NPCShoot(StateMachine sm, NPC robot) : base(sm)
    {
        _robot = robot;
    }

    public override void OnAwake()
    {       
        base.OnAwake();
        _robot.transform.forward = (MonoBehaviour.FindObjectOfType<DonePlayerMovement>().gameObject.transform.position - _robot.transform.position).normalized;
        _robot.Shoot();
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
