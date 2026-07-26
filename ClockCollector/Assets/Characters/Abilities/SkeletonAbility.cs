using UnityEngine;

public class SkeletonAbility : AbilityBase
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
    }

    public override void TimeTrigger(GameObject action)
    {
        base.TimeTrigger(action);
    }

    public override void ReceiveHealthTrigger()
    {
        base.ReceiveHealthTrigger();
    }

    public override void DamagedTrigger(GameObject attacker, int damage)
    {
        attacker.GetComponent<SpiritBase>().TakeDamage(damage);
    }


    public override void ReceiveShieldTrigger()
    {
        base.ReceiveShieldTrigger();
    }

}
