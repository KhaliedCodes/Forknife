using UnityEngine;

public class DuckState : IState
{

    private Animator _animator;

    public DuckState(Animator animator)
    {
        _animator = animator;
    }
    public void Enter()
    {
        _animator.SetBool("Duck", true);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        _animator.SetBool("Duck", false);
    }
}
