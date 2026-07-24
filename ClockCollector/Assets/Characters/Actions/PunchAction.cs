using UnityEngine;

public class PunchAction : ActionBase
{
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionTriggered(GameObject Target)
    {
        SpiritBase targetComponent = Target.GetComponent<SpiritBase>();

        targetComponent.TakeDamage(5);
    }
}
