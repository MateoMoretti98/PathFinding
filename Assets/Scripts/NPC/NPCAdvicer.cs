using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAdvicer : MonoBehaviour
{
	List<NPC> allRobots;
	int i = 1;

	private void Awake()
	{
		allRobots = new List<NPC>(FindObjectsOfType<NPC>());
		foreach (NPC robot in allRobots)
		{
			robot.OnPlayerSeenIt += AdviceRobots;
		}
	}

	public void AdviceRobots(NPC robotAdvicer)
	{

		foreach (NPC robot in allRobots)
		{
			if(robot != robotAdvicer)
			{
				if (!robot.GetStateMachine().IsActualState<NPCGoTo>() && !robot.GetStateMachine().IsActualState<NPCNotify>() && !robot.GetStateMachine().IsActualState<NPCShoot>())				
					robot.GetStateMachine().SetState<NPCGoTo>();
			}
		}
	}
}
