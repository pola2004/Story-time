using UnityEngine;

public class JumpState : State
{
    public LayerMask groundLayer = LayerMask.GetMask("Ground");
    private bool isTouchingGround;
    public float groundCheckRadius = 0.5f;
    public JumpState()
    {
        stateID = StateID.Jump;
    }

    public override void DoBeforeEntering(Animator animator)
    {
        isTouchingGround = true;
        animator.Play("Jump");
        startTime = Time.time;
    }
    public override void Act(Transform player)
    {
        if (Time.time < startTime + 0.5f)
            return;
        if (isTouchingGround)
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 50) * Time.deltaTime, ForceMode2D.Impulse);

        }
    }

    public override void Reason(Transform player)
    {

        if (Time.time < startTime + 0.5f)
            return;
        isTouchingGround = Physics2D.OverlapCircle(player.position, groundCheckRadius, groundLayer);
        if (isTouchingGround)
            player.GetComponent<Character>().SetTransition(Transition.Idle, player.GetComponent<Animator>());
    }
}