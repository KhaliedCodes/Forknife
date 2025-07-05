using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Flee from target", story: "[Agent] flee from [Target]", category: "Action/Navigation", id: "dfc7388534cbf4b471a566f68bea842e")]
public partial class FleeFromTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");


    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    private float m_PreviousStoppingDistance;
    private Vector3 m_ColliderAdjustedTargetPosition;
    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }
        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, Speed);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }



        float speed = Speed;



        Vector3 agentPosition = Agent.Value.transform.position;
        Vector3 toDestination = (Target.Value.transform.position - agentPosition) * -1;
        toDestination.y = 0.0f;
        toDestination.Normalize();
        agentPosition += toDestination * (speed * Time.deltaTime);
        Agent.Value.transform.position = agentPosition;

        // Look at the target.
        Agent.Value.transform.forward = toDestination;


        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, 0);
        }

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.stoppingDistance = m_PreviousStoppingDistance;
        }

        m_NavMeshAgent = null;
        m_Animator = null;
    }




}

