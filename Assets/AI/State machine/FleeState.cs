using UnityEngine;
using UnityEngine.AI;

public class FleeState : IState
{

    private Transform _player;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Transform _transform;
    private Vector3 prevPosition;
    private float _speed = 7f;


    public FleeState(Transform player, NavMeshAgent navMeshAgent, Animator animator, Transform transform)
    {
        _player = player;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _transform = transform;
    }
    public void Enter()
    {
        _animator.SetBool("Run", true);
    }

    public void Execute()
    {
        var speed = _transform.position - prevPosition;
        prevPosition = _transform.position;
        var dir = Flee(_player.position - _transform.position);
        Vector3 fleePosition = _transform.position + dir * 10;
        _navMeshAgent.speed = _speed;
        _navMeshAgent.SetDestination(fleePosition);
    }

    public void Exit()
    {
        _animator.SetBool("Run", false);
        _navMeshAgent.speed = 3.5f;
        _navMeshAgent.ResetPath();
    }
    Vector3 Flee(Vector3 dir)
    {
        return -dir.normalized;
    }
}
