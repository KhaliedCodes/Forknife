using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Frog : Enemy
{
    public List<Transform> Waypoints;
    private NavMeshAgent NavMeshAgent;
    public Trigger FleeTrigger;
    public Trigger DuckTrigger;

    void Start()
    {
        base.Start();
        FleeTrigger.EnteredTrigger += OnFleeTriggerEnter;
        FleeTrigger.ExitedTrigger += OnFleeTriggerExit;
        DuckTrigger.EnteredTrigger += OnDuckTriggerEnter;
        DuckTrigger.ExitedTrigger += OnDuckTriggerExit;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        _context.SetState(new PatrolState(Waypoints, Animator, NavMeshAgent, transform));
    }

    void Update()
    {
        base.Update();
        _context.ExecuteState();

    }



    void OnDuckTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _context.GetCurrentState() is not DeathState)
        {
            _context.SetState(new DuckState(Animator));
            Debug.Log("Entered outer collider");
        }

    }

    void OnDuckTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _context.GetCurrentState() is not DeathState)
        {
            _context.SetState(new PatrolState(Waypoints, Animator, NavMeshAgent, transform));
            Debug.Log("Exited outer collider");
        }
    }

    void OnFleeTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player") && _context.GetCurrentState() is not DeathState)
        {
            _context.SetState(new FleeState(player.transform, NavMeshAgent, Animator, transform));
            Debug.Log("Entered inner collider");
        }
    }
    void OnFleeTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _context.GetCurrentState() is not DeathState)
        {
            _context.SetState(new DuckState(Animator));
            Debug.Log("Entered outer collider");
        }
    }
}
