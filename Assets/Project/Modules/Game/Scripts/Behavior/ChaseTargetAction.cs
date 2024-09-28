using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Utils;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseTarget", story: "[Agent] chases target if detected by [Sensor]", category: "Action", id: "77d414558db7ca861e427caefd6f6922")]
public partial class ChaseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<UnityEngine.AI.NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<Sensor> Sensor;
    [SerializeReference] public BlackboardVariable<bool> IsTargetOnSight;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        this.IsTargetOnSight.Value = this.Sensor.Value.Target != null;
        if (this.IsTargetOnSight.Value)
        {
            this.Agent.Value.SetDestination(this.Sensor.Value.Target.transform.position);
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

