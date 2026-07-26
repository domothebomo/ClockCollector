using UnityEngine;

public class BlockAction : ActionBase
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
        if (targetType == TargetType.SingleTarget)
        {
            ownerComp.AddBlock(potency);
        }
        else if (targetType == TargetType.AreaOfEffect)
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
                spirit.GetComponent<SpiritBase>().AddBlock(potency);
            }
        }



        base.ActionTriggered(Target);
    }
}
