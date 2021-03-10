using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : NPCState
{
    List<Transform> _waypoints;

    int _currentIndex = 0;

    float _minRange = 0.6f;
    bool _goingBack = false;

    NPC _robot;

    public NPCPatrol(StateMachine sm, List<Transform> waypoints, NPC robot) : base(sm)
    {
        _waypoints = new List<Transform>(waypoints);
        _robot = robot;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        _robot.SetVelocity();
    }

    public override void OnExecute()
    {
        base.OnExecute();
        FollowWaypoints();
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

    private void FollowWaypoints()
    {
        if(_robot == null)
            return;

        float distanceToChangeDir = Vector3.Distance(_waypoints[_currentIndex].position, _robot.transform.position);
        if(distanceToChangeDir <= _minRange) ChangeWaypoint();

        Vector3 dir = (_waypoints[_currentIndex].position - _robot.transform.position).normalized;
        dir.y = 0;
        _robot.transform.forward = dir;
    }

    private void ChangeWaypoint()
    {
        if (_goingBack)
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = 0;
                _goingBack = false;
            }
        }
        else
        {
            _currentIndex++;
            if (_currentIndex >= _waypoints.Count)
            {
                _currentIndex = _waypoints.Count - 1;
                _goingBack = true;
            }
        }
    }
}
