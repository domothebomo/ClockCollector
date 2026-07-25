using UnityEngine;

public class Rewind : ActionBase
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ActionTriggered(GameObject Target)
    {
        //SpiritBase targetComponent = Target.GetComponent<SpiritBase>();

        SpiritBase ownerComp = transform.parent.gameObject.GetComponent<SpiritBase>();
        ownerComp.HealDamage(10);

        base.ActionTriggered(Target);
    }
}
