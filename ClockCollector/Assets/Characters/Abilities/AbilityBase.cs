using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    public GameObject ownedSpirit;
    public SpriteRenderer spriteComp;

    //public int modifiedDamage;
    public float modifiedCooldown;
    //public int modifiedShield;
    public int modifiedPotency;
    public int modifiedTriggers;

    public bool bypassShields = false;
    public bool splitActions = false;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ActionUseTrigger(GameObject action)
    {
        modifiedCooldown = -1;
        modifiedTriggers = -1;
        modifiedPotency = -1;
    }

    public virtual void TimeTrigger(GameObject action)
    {

    }

    public virtual void ReceiveHealthTrigger()
    {

    }

    public virtual void DamagedTrigger(GameObject attacker)
    {

    }

    public virtual void ReceiveShieldTrigger()
    {

    }

}
