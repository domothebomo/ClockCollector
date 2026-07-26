using UnityEngine;

public class GrandfatherAbility : AbilityBase
{
    public Sprite altSprite;
    public Sprite defaultSprite;

    int hits = 0;

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
            if (hits >= 3)
            {
                hits = 0;
                modifiedPotency = action.GetComponent<ActionBase>().potency * 3;
                ownedSpirit.GetComponent<SpriteRenderer>().sprite = defaultSprite;
            }
            else
            {
                hits++;
                modifiedPotency = 0;

                if (hits >= 3)
                {
                    ownedSpirit.GetComponent<SpriteRenderer>().sprite = altSprite;
                }
            }
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
