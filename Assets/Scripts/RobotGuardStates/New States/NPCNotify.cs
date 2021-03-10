using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCNotify : NPCState
{
    Transform _player;
    float _timeFollowing;
    NPC _robot;

    public NPCNotify(StateMachine sm, Transform player, NPC robot) : base(sm)
    {
        _player = player;
		_robot = robot;
    }

    public override void OnAwake()
    {
        base.OnAwake();
		_robot.SetVelocityFollowing();
    }

    public override void OnExecute()
    {
        base.OnExecute();
		_robot.transform.forward = Vector3.Lerp(_robot.transform.forward, 
            Vector3.Normalize(_player.transform.position - _robot.transform.position), Time.deltaTime * _robot.rotTime);
    }

    public override void OnLateExecute()
    {
        base.OnLateExecute();
    }

    public override void OnSleep()
    {
        base.OnSleep();
		_robot.SetNoVelocity();
	}
}
