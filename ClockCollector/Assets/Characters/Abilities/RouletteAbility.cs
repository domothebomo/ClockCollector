using UnityEngine;

public class RouletteAbility : AbilityBase
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

        if (action.GetComponent<ActionBase>().actionType == ActionBase.ActionType.Attack)
        {
            int gamba = Random.Range(1, 10);
            if (gamba == 1)
            {
                modifiedTriggers = (action.GetComponent<ActionBase>().triggers * 2);
            }
            else { modifiedTriggers = -1; }
            gamba = Random.Range(1, 5);
            if (gamba == 1)
            {
                modifiedPotency = ownedSpirit.GetComponent<ActionBase>().potency * 2;
            }
            else { modifiedPotency = -1; }
        }
        else
        {
            modifiedTriggers = -1;
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
