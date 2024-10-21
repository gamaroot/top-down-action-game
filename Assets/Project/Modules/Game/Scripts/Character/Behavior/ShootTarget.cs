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
        this.IsTargetOnSight.Value = this.Sensor.Value.IsTargetOnSight;
        if (this.IsTargetOnSight.Value)
        {
            if (this.Sensor.Value.IsTargetBehindObstacle)
            {
                this.Agent.Value.OnMove(this.Sensor.Value.BestShootingPosition);
            }
            else
            {
                Vector3 point = this.Sensor.Value.Target.transform.position;

                this.Agent.Value.OnMove(point);
                this.Agent.Value.FaceTarget(point);
                this.Agent.Value.OnAttack();
            }
        }
        else
        {
            this.Agent.Value.OnTargetLost();
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

