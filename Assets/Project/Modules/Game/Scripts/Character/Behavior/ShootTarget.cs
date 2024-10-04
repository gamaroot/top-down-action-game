using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Utils;
using Game;
using System.Drawing;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ShootTarget", story: "[Agent] shoots target if [IsTargetOnSight] when detected by [Sensor]", category: "Action", id: "77d414558db7ca861e427caefd6f6923")]
public partial class ShootTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyShooter> Agent;
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
            Vector3 point;
            if (this.Sensor.Value.IsTargetBehindObstacle)
            {
                // Reposition to shoot the target
                point = this.Sensor.Value.BestShootingPosition;
            }
            else
            {
                point = this.Sensor.Value.Target.transform.position;
                this.Agent.Value.OnAttack(this.Sensor.Value.Target.transform.position);
            }
            this.Agent.Value.FaceTarget(point);
            this.Agent.Value.OnMove(point);
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}
