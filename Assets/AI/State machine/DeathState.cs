using UnityEngine;

public class DeathState : IState
{
    private Animator _animator;
    public DeathState(Animator animator)
    {
        _animator = animator;
    }
    public void Enter()
    {
        Debug.Log("da5alt el death nafso");
        _animator.SetTrigger("Die");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
