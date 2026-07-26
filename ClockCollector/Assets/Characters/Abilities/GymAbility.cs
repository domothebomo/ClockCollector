using UnityEngine;

public class GymAbility : AbilityBase
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
        if (ownedSpirit.GetComponent<SpiritBase>().GetBlockStacks() > 0 && action.GetComponent<ActionBase>().statusEffect == ActionBase.StatusEffect.Shield)
        {
            modifiedPotency = (int) (action.GetComponent<ActionBase>().potency * 1.5);
        }
        else
        {
            modifiedPotency = -1;
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
