using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    StateMachine myStateMachine;     
    Animator _animator;          

    [Header("Others")]
    public GameObject head;             
    public LayerMask obstacles;       
    public List<Transform> _waypoints;              

    float _timingPatrol;            
    float _timingIdle;          
    float _iniTimingHearing;

    [Header("NPC Settings")]
    public float speed;
    public float speedFollowing;
    public float patrolTime;
    public float idleTime;
    public float viewAngle;
    public float viewDistance;
	public float shootDistance;
    public float hearDistance;
    public float timingHearing;
    public float rotTime;

    public event Action<NPC> OnPlayerSeenIt;
    List<DoneCCTVPlayerDetection> camerasDetection = new List<DoneCCTVPlayerDetection>();
    List<DoneLaserPlayerDetection> lazerDetection = new List<DoneLaserPlayerDetection>();

    private void Awake()
    {
        camerasDetection = new List<DoneCCTVPlayerDetection>(FindObjectsOfType<DoneCCTVPlayerDetection>());
        foreach (var item in camerasDetection)
        {
            item.OnPlayerDetected += SecurityDetectedPlayer;
        }

        lazerDetection = new List<DoneLaserPlayerDetection>(FindObjectsOfType<DoneLaserPlayerDetection>());
        foreach (var item in lazerDetection)
        {
            item.OnPlayerDetected += SecurityDetectedPlayer;
        }


        _timingPatrol = patrolTime;
        _timingIdle = idleTime;
        _iniTimingHearing = timingHearing;
        _animator = GetComponent<Animator>();

        FindObjectOfType<DonePlayerMovement>().OnShout += NPCHearShout;

        myStateMachine = new StateMachine();
        myStateMachine.AddState(new NPCPatrol(myStateMachine, _waypoints, this));
        myStateMachine.AddState(new NPCIdle(myStateMachine, this));
        myStateMachine.AddState(new NPCShoot(myStateMachine, this));
        myStateMachine.AddState(new NPCHear(myStateMachine, rotTime, this));
        myStateMachine.AddState(new NPCNotify(myStateMachine, FindObjectOfType<DonePlayerMovement>().transform, this));
        myStateMachine.AddState(new NPCGoTo(myStateMachine, this, FindObjectOfType<PathFinder>()));
    }

    private void Start()
    {
        myStateMachine.SetState<NPCPatrol>();
    }

    private void Update()
    {
        CheckPlayerOnView();
        CheckTimingPatrol();
        CheckTimingIdle();
        NPCHearingTimer();
        myStateMachine.Update();
    }

    private void NPCHearShout()
    {
        Vector3 playerPos = FindObjectOfType<DonePlayerMovement>().gameObject.transform.position;
        if (Vector3.Distance(playerPos, transform.position) < hearDistance)
        {
            myStateMachine.SetState<NPCHear>();
        }
    }

    void NPCHearingTimer()
    {
        if (myStateMachine.IsActualState<NPCHear>())
        {
            timingHearing -= Time.deltaTime;
            if (timingHearing <= 0)
                myStateMachine.SetState<NPCPatrol>();
        }
        else
        {
            timingHearing = _iniTimingHearing;
        }
    }

    public void SecurityDetectedPlayer(Vector3 here)
    {
        if (!myStateMachine.IsActualState<NPCGoTo>())
        {
            myStateMachine.SetState<NPCGoTo>();
            Debug.Log("STATE GO TO POINT");
        }
        return;
    }

    private void CheckPlayerOnView()
    {
        Transform player = FindObjectOfType<DonePlayerMovement>().gameObject.transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle && Vector3.Distance(transform.position, player.position) < 
            viewDistance && !Physics.Raycast(head.transform.position, dirToPlayer, viewDistance, obstacles))
        {
            if (CheckPlayerOnShootDistance())
                return;
            DoneLastPlayerSighting lastPlayerSighting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>();
            lastPlayerSighting.position = player.position;
            OnPlayerSeenIt(this);
            Debug.Log("Notify");
            if (!myStateMachine.IsActualState<NPCNotify>()) 
                myStateMachine.SetState<NPCNotify>();
            return;
        }
        else
        {
            if (myStateMachine.IsActualState<NPCNotify>())
                myStateMachine.SetState<NPCGoTo>();
        }
    }

	private bool CheckPlayerOnShootDistance()
	{
		Transform player = FindObjectOfType<DonePlayerMovement>().gameObject.transform;
		Vector3 dirToPlayer = (player.position - transform.position).normalized;
		if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
		{
			if (Vector3.Distance(transform.position, player.position) < shootDistance)
			{
				if (!Physics.Raycast(head.transform.position, dirToPlayer, shootDistance, obstacles))
				{
					if (!myStateMachine.IsActualState<NPCShoot>())
						myStateMachine.SetState<NPCShoot>();
					return true;
				}
			}
		}
		return false;
	}

    private void CheckTimingPatrol()
    {
        if (myStateMachine.IsActualState<NPCPatrol>())
        {
            patrolTime -= Time.deltaTime;
            if (patrolTime <= 0)
                myStateMachine.SetState<NPCIdle>();
        }
        else
        {
            patrolTime = _timingPatrol;
        }
    }

    private void CheckTimingIdle()
    {
        if (myStateMachine.IsActualState<NPCIdle>())
        {
            idleTime -= Time.deltaTime;
            if (idleTime <= 0)
                myStateMachine.SetState<NPCPatrol>();
        }
        else
        {
            idleTime = _timingIdle;
        }
    }

    public void SetVelocity()
    {
        _animator.SetFloat("Speed", speed);
    }

    public void SetVelocityFollowing()
    {
        _animator.SetFloat("Speed", speedFollowing);
    }

    public void SetNoVelocity()
    {
        _animator.SetFloat("Speed", 0);
    }

    public void Shoot()
    {
        _animator.SetLayerWeight(0, 0);
        _animator.SetLayerWeight(1, 1);
        _animator.SetBool("PlayerInSight", true);
        //FIX
        FindObjectOfType<DonePlayerHealth>().health = 0;
    }

	public StateMachine GetStateMachine()
	{
		return myStateMachine;
	}
}
