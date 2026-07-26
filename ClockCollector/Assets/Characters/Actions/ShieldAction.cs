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
        SpiritBase ownerComp = transform.parent.gameObject.GetComponent<SpiritBase>();

        int shieldStrength = potency;
        ownerComp.abilityComp.ActionUseTrigger(gameObject);
        if (ownerComp.abilityComp.modifiedPotency != -1)
        {
            shieldStrength = ownerComp.abilityComp.modifiedPotency;
        }

        if (targetType == TargetType.SingleTarget && !ownerComp.abilityComp.splitActions)
        {
            ownerComp.AddShield(shieldStrength);
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
                spirit.GetComponent<SpiritBase>().AddShield(shieldStrength);
            }
        }

        base.ActionTriggered(Target);
    }
}
