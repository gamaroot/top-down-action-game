using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Utils;
using Game;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseTargetAsKamikaze", story: "[Agent] chases target if [IsTargetOnSight] when detected by [Sensor]", category: "Action", id: "77d414558db7ca861e427caefd6f6922")]
public partial class ChaseTargetAsKamikazeAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyKamikaze> Agent;
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
            Vector3 targetPosition = this.Sensor.Value.Target.transform.position;
            float distance = Vector3.Distance(this.Agent.Value.transform.position, targetPosition);
            if (distance <= 1f)
            {
                this.Agent.Value.OnCloseToTarget();
            }
            else
            {
                this.Agent.Value.OnMove(targetPosition);
            }
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

