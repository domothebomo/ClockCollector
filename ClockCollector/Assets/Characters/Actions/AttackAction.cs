using UnityEngine;

public class AttackAction : ActionBase
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
        SpiritBase ownerComp = transform.parent.gameObject.GetComponent<SpiritBase>();

        int damage = potency + ownerComp.GetPowerStacks();

        if (targetType == TargetType.SingleTarget)
        {
            SpiritBase targetComponent = Target.GetComponent<SpiritBase>();
            targetComponent.TakeDamage(damage);
        }
        else if (targetType == TargetType.AreaOfEffect)
        {
            GameObject[] Spirits;
            if (ownerComp.CompareTag("AllySpirit"))
            {
                Spirits = BattleManager.Instance.GetEnemySpirits();
            } 
            else
            {
                Spirits = BattleManager.Instance.GetAllySpirits();
            }
            foreach (GameObject spirit in Spirits)
            {
                spirit.GetComponent<SpiritBase>().TakeDamage(damage);
            }

        }

        base.ActionTriggered(Target);
    }
}
