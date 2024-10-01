using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Utils;
using Game;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseTarget", story: "[Agent] chases/attack target if detected by [Sensor] and [keepDistanceFromTarget] if not kamikaze", category: "Action", id: "77d414558db7ca861e427caefd6f6922")]
public partial class ChaseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Agent;
    [SerializeReference] public BlackboardVariable<Sensor> Sensor;
    [SerializeReference] public BlackboardVariable<float> KeepDistanceFromTarget;
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
            this.Agent.Value.OnTargetOnSight(this.Sensor.Value.Target.transform,
                                             this.KeepDistanceFromTarget.Value);
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

