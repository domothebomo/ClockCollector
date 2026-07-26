using UnityEngine;

public class HealAction : ActionBase
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

        int healStrength = potency;
        ownerComp.abilityComp.ActionUseTrigger(gameObject);
        if (ownerComp.abilityComp.modifiedPotency != -1)
        {
            healStrength = ownerComp.abilityComp.modifiedPotency;
        }

        if (targetType == TargetType.SingleTarget && !ownerComp.abilityComp.splitActions)
        {
            ownerComp.HealDamage(healStrength);
        }
        else
        {
            GameObject[] Spirits;
            if (ownerComp.CompareTag("AllySpirit"))
            {
                Spirits = BattleManager.Instance.GetAllySpirits();
            }
            else
            {
                Spirits = BattleManager.Instance.GetEnemySpirits();
            }
            foreach (GameObject spirit in Spirits)
            {
                spirit.GetComponent<SpiritBase>().HealDamage(healStrength);
            }
        }



        base.ActionTriggered(Target);
    }
}
