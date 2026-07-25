using System.ComponentModel;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    public string actionName;
    public string actionDescription;

    public float actionCooldown = 2.0f;
    float cooldownTimer = 0.0f;
    bool cooldownActive = false;

    public bool requiresTarget = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        /*if (cooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= actionCooldown)
            {
                cooldownActive = false;
                cooldownTimer = 0.0f;
            }
        }*/
    }

    public bool OnCooldown()
    {
        return cooldownActive;
    }

    public virtual void ActionTriggered(GameObject Target)
    {
        //cooldownActive = true;
    }

}
