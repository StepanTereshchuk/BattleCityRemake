using UnityEngine;

public class AnimationController 
{
    private Animator _animator;
    private int _move = Animator.StringToHash("Moving");
    private int _invincible = Animator.StringToHash("Invincibility");
    private int _explode = Animator.StringToHash("Explode");
    private int _spawn = Animator.StringToHash("Spawning");

    public AnimationController(Animator animator)
    {
        _animator = animator;
    }
    public void Spawn()
    {
        _animator.SetTrigger(_spawn);
    }
    public void Move(bool choice)
    {
        _animator.SetBool(_move, choice);
    }
    public void Explode()
    {
        _animator.SetTrigger(_explode);
    }
    public void MakeInvincible()
    {
        _animator.SetTrigger(_invincible);
    }
    //fire ?
}
