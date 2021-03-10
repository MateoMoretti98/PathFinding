using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHear : NPCState
{
    Vector3 _posPlayerHeared;
    float _timeToRotate;
    float t = 0;
    NPC _robot;

    public NPCHear(StateMachine sm, float timeToRotate, NPC npc) : base(sm)
    {
        _timeToRotate = timeToRotate;
        _robot = npc;
    }

    public void SetPosPlayerHeared(Vector3 pp)
    {
        _posPlayerHeared = pp;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        _robot.SetNoVelocity();
        t = 0;
        SetPosPlayerHeared(MonoBehaviour.FindObjectOfType<DonePlayerMovement>().transform.position);
    }

    public override void OnExecute()
    {
        base.OnExecute();
        t += Time.deltaTime;
        _robot.transform.forward = Vector3.Lerp(_robot.transform.forward, Vector3.Normalize(_posPlayerHeared - _robot.transform.position), t / _timeToRotate);
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
