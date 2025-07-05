using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Death", story: "[health] less than or equal 0", category: "Conditions", id: "b9feb46c919c28265d9285dc9ae4bb32")]
public partial class CheckDeathCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> Health;

    public override bool IsTrue()
    {
        return Health.Value <= 0;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
