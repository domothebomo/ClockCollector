using UnityEngine;

public class CosmosAbility : AbilityBase
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
        modifiedCooldown = ownedSpirit.GetComponent<SpiritBase>().currentCooldown / 2;
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
