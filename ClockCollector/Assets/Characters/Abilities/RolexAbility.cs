using UnityEngine;

public class RolexAbility : AbilityBase
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
        //modifiedCooldown = ownedSpirit.GetComponent<SpiritBase>().currentCooldown - 1;


        modifiedTriggers = -1;
        modifiedPotency = -1;
    }

    public override void TimeTrigger(GameObject action)
    {
        base.TimeTrigger(action);
    }

    public override void ReceiveHealthTrigger()
    {
        base.ReceiveHealthTrigger();
    }

    public override void DamagedTrigger(GameObject attacker)
    {
        base.DamagedTrigger(attacker);
    }

    public override void ReceiveShieldTrigger()
    {
        base.ReceiveShieldTrigger();
    }

}
