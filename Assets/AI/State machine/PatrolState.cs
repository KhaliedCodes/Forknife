using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private List<Transform> _waypoints;
    private int _currentWaypointIndex;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Vector3 prevPosition;
    public PatrolState(List<Transform> waypoints, Animator animator, NavMeshAgent navMeshAgent, Transform transform)

    {
        _waypoints = waypoints;
        _currentWaypointIndex = 0;
        _animator = animator;
        _navMeshAgent = navMeshAgent;
        _transform = transform;
    }
    public void Enter()
    {
        _animator.SetBool("Walk", true);
        prevPosition = _transform.position;
        if (_waypoints.Count > 0)
        {
            MoveToNextTarget();
        }
    }

    public void Execute()
    {
        var speed = _transform.position - prevPosition;
        prevPosition = _transform.position;
        if (speed.magnitude == 0)
        {
            _animator.SetBool("Walk", false);
        }
        else
        {
            _animator.SetBool("Walk", true);
        }
        // _animator.SetFloat("Speed", speed.magnitude);
        if (_navMeshAgent.remainingDistance < 0.1f)
        {
            if (Time.time % 5 < 0.1f)
            {
                MoveToNextTarget();
            }
        }
    }

    public void Exit()
    {
        _animator.SetBool("Walk", false);
        _navMeshAgent.ResetPath();
    }


    void MoveToNextTarget()
    {
        _navMeshAgent.SetDestination(_waypoints.ElementAt(_currentWaypointIndex).position);
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;

    }
}
