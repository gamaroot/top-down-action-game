using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Game;
using Utils;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StateDisplay", story: "[Agent] displays a message for [seconds] based on [IsTargetOnSight]", category: "Action", id: "f9a1c3e61cdfece9431ae149762fc5a1")]
public partial class StateDisplayAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<float> Seconds;
    [SerializeReference] public BlackboardVariable<bool> IsTargetOnSight;

    private bool _previousIsTargetOnSight;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        bool hasStateChanged = this.IsTargetOnSight.Value != this._previousIsTargetOnSight;
        if (hasStateChanged)
        {
            SpawnType spawnType = this.IsTargetOnSight.Value ? SpawnType.EXCLAMATION_MARK : SpawnType.QUESTION_MARK;
            UIFollower reactionUI = SpawnablePool.Spawn<UIFollower>(spawnType, this.Seconds);
            reactionUI.SetTarget(this.Agent.Value.transform);
            reactionUI.gameObject.SetActive(true);
        }
        this._previousIsTargetOnSight = this.IsTargetOnSight.Value;

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

