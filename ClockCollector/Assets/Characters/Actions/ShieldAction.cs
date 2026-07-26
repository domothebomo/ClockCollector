using UnityEngine;

public class ShieldAction : ActionBase
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
        if (targetType == TargetType.SingleTarget)
        {
            SpiritBase ownerComp = transform.parent.gameObject.GetComponent<SpiritBase>();
            ownerComp.AddShield(potency);
        }
        else if (targetType == TargetType.AreaOfEffect)
        {
            GameObject[] allySpirits = BattleManager.Instance.GetAllySpirits();
            foreach (GameObject spirit in allySpirits)
            {
                spirit.GetComponent<SpiritBase>().AddShield(potency);
            }
        }

        base.ActionTriggered(Target);
    }
}
