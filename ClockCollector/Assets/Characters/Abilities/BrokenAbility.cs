using System.Threading;
using UnityEngine;

public class BrokenAbility : AbilityBase
{
    float timer = 0;
    bool boosted = false;

    public Sprite altSprite;
    public Sprite defaultSprite;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (boosted)
        {
            if (timer >= 5)
            {
                boosted = false;
                ownedSpirit.GetComponent<SpriteRenderer>().sprite = defaultSprite;
                timer = 0;
            }
        }
        if (timer >= 10)
        {
            boosted = true;
            ownedSpirit.GetComponent<SpriteRenderer>().sprite = altSprite;
            timer = 0;
        }
    }

    public override void ActionUseTrigger(GameObject action)
    {
        if (boosted && action.GetComponent<ActionBase>().actionType == ActionBase.ActionType.Attack)
        {
            modifiedPotency = action.GetComponent<ActionBase>().potency * 2;
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
