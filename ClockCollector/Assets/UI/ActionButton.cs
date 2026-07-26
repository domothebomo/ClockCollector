using TMPro;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public GameObject ownedSpirit;
    public int actionIndex;

    public GameObject action;
    public TMP_Text actionText;

    public UnlockScreen unlockScreenComp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LearnAction()
    {
        ownedSpirit.GetComponent<SpiritBase>().LearnAction(action);

        unlockScreenComp.choiceSelected(null);
    }
}
