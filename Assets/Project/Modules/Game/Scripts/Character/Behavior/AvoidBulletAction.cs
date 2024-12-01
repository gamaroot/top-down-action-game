using System;
using Unity.Behavior;
using UnityEngine;
using Utils;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AvoidBullet", story: "[Agent] tries to avoid a bullet detected by the [Sensor]", category: "Action", id: "719154a09063fdbb721aeb3b6622a151")]
public partial class AvoidBulletAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<Sensor> Sensor;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (this.Sensor.Value.IncomingBullet != null)
        {
            Vector3 direction = (this.Sensor.Value.IncomingBullet.position - this.Agent.Value.transform.position).normalized;

            // Avoids the bullet by moving perpendicular to its direction
            var avoidDirection = Vector3.Cross(direction, Vector3.up);

            this.Agent.Value.Move(avoidDirection * this.Agent.Value.speed * Time.deltaTime);
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

