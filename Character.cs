using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public static Character mainCharacter;
    public Transform[] path;
    private FSMSystem fsm;
    private Animator animator;
    public Rigidbody2D characterRigiBody;
    private Vector3 originalPoint;
    public CinemachineVirtualCamera followCam;
    public CinemachineVirtualCamera followCam2;
    private void Awake()
    {
        originalPoint = transform.position;
        if (mainCharacter == null)
            mainCharacter = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        MakeFSM();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "End")
        {
            followCam.enabled = true;
            // SideScrolling.currentSideScrolling.SnapCamera();
        }
        else if (other.tag == "Start")
        {
            followCam2.transform.position = transform.position + new Vector3(7, 3.5f, -16);
            followCam.enabled = false;
        }
    }
    public void Reset()
    {
        transform.position = originalPoint;
        MakeFSM();
    }
    public void FixedUpdate()
    {

        fsm.CurrentState.Act(transform);
        fsm.CurrentState.Reason(transform);
    }
    private void MakeFSM()
    {
        IdleState idle = new IdleState();
        idle.AddTransition(Transition.Walk, StateID.Walk);
        idle.AddTransition(Transition.Jump, StateID.Jump);
        idle.AddTransition(Transition.Die, StateID.Die);

        WalkState walk = new WalkState(path);
        walk.AddTransition(Transition.Idle, StateID.Idle);
        walk.AddTransition(Transition.Jump, StateID.Jump);
        walk.AddTransition(Transition.Die, StateID.Die);

        JumpState jump = new JumpState();
        jump.AddTransition(Transition.Idle, StateID.Idle);
        jump.AddTransition(Transition.Die, StateID.Die);
        DeadState die = new DeadState();

        fsm = new FSMSystem();
        fsm.AddState(idle);
        fsm.AddState(walk);
        fsm.AddState(jump);
        fsm.AddState(die);
    }
    public void SetTransition(Transition t, Animator animator)
    {
        fsm.PerformTransition(t, animator);
    }

    public void DisplayDialogue()
    {
        Vector3 newPosition = new Vector3(transform.position.x + 1.5f, transform.position.y + 1.5f, transform.position.z);
        List<IEnumerator> dialogues = new List<IEnumerator>();

        dialogues.Add(DialogueSystem.instance.DisplayDialogue(newPosition, transform, "That is hard question, i need help with it", 3));
        dialogues.Add(DialogueSystem.instance.DisplayDialogue(newPosition, transform, DialogueSystem.instance.sprite1));

        StartCoroutine(DialogueSystem.instance.DisplayMultiDialogue(dialogues));
    }
}