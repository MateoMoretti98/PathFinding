using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGoTo : NPCState
{
	Stack<Node> myPath = new Stack<Node>();
	PathFinder pathfinder;
	Node nearNode;
	Node nearLastSeenPlayerNode;
    NPC _robot;

	List<Transform> _waypoints = new List<Transform>();

	int _currentIndex = 0;
	float _minRange = 0.6f;
	bool _goingBack = false;

	public NPCGoTo(StateMachine sm, NPC robot, PathFinder pathfinder) : base(sm)
	{
		this.pathfinder = pathfinder;
		_robot = robot;
	}

	public override void OnAwake()
	{
		base.OnAwake();
		_currentIndex = 0;

		SetNearNode();
		SetNearNodeFromPlayer();
		myPath.Clear();
		myPath = pathfinder.GetPath(nearNode, nearLastSeenPlayerNode);

		_waypoints.Clear();
		while (myPath.Count > 0)
		{
			var t = myPath.Pop().transform;
			_waypoints.Add(t);
		}
		_robot.SetVelocityFollowing();
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

	public void SetNearNodeFromPlayer()
	{
		Vector3 playerPos = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>().position;
		nearLastSeenPlayerNode = pathfinder.allNodes[0];
		foreach (Node node in pathfinder.allNodes)
		{
			if (Vector3.Distance(playerPos, node.transform.position) < Vector3.Distance(playerPos, nearLastSeenPlayerNode.transform.position))
				nearLastSeenPlayerNode = node;
		}
	}

	private void SetNearNode()
	{
		nearNode = pathfinder.allNodes[0];
		foreach (Node node in pathfinder.allNodes)
		{
			if(Vector3.Distance(_robot.transform.position, node.transform.position) < Vector3.Distance(_robot.transform.position, nearNode.transform.position))
				nearNode = node;
		}
	}

	private void FollowWaypoints()
	{
		if (_robot == null)
			return;

		float distanceToChangeDir = Vector3.Distance(_waypoints[_currentIndex].position, _robot.transform.position);
		if (distanceToChangeDir <= _minRange)
			ChangeWaypoint();

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
				_sm.SetState<NPCPatrol>();
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
