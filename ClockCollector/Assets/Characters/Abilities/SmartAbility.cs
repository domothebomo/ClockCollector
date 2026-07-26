using UnityEngine;

public class SmartAbility : AbilityBase
{

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionUseTrigger(GameObject action)
    {
        GameObject[] spirits;
        if (ownedSpirit.CompareTag("EnemySpirit"))
        {
            spirits = BattleManager.Instance.GetAllySpirits();
        }
        else
        {
            spirits = BattleManager.Instance.GetEnemySpirits();
        }
        foreach (GameObject spirit in spirits)
        {
            spirit.GetComponent<SpiritBase>().currentCooldown = spirit.GetComponent<SpiritBase>().currentCooldown - 0.5f;
        }
    }

    public override void TimeTrigger(GameObject action)
    {
        base.TimeTrigger(action);
    }

    public override void ReceiveHealthTrigger()
    {
        base.ReceiveHealthTrigger();
    }

 

    public override void ReceiveShieldTrigger()
    {
        base.ReceiveShieldTrigger();
    }

}
