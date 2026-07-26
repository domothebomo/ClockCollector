using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Transactions;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public static PlayerManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    GameObject targetedEnemy;

    List<GameObject> playerSpirits = new List<GameObject>();

    Camera cam;

    public UnlockScreen screenComp;

    public GameObject infoBox;
    public TMP_Text infoBoxTitle;
    public TMP_Text infoBoxDescription;

    GameObject hoveredObject;
    SpiritBase spiritComp;
    ActionButton buttonComp;

    int upgradeIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);

        cam = Camera.main;

        OpenUnlockScreen();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHovered();
    }

    void CheckHovered()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hoveredObject != hit.collider.gameObject)
        {
            Debug.Log(hit.collider.gameObject.name);
            hoveredObject = hit.collider.gameObject;
            UpdateInfoBox();
            Debug.Log("updating");
        }
        else if (hit.collider == null)
        {
            hoveredObject = null;
            HideInfoBox();
        }
    }

    void UpdateInfoBox()
    {
        spiritComp = hoveredObject.GetComponent<SpiritBase>();

        if (spiritComp != null)
        {
            infoBox.SetActive(true);
            infoBoxTitle.text = spiritComp.displayName;
            infoBoxDescription.text = spiritComp.abilityDescription;
            return;
        }

        buttonComp = hoveredObject.GetComponent<ActionButton>();

        if (buttonComp != null)
        {
            ActionBase actionComp;
            if (buttonComp.action != null)
            {
                actionComp = buttonComp.action.GetComponent<ActionBase>();
            }
            else
            {
                actionComp = buttonComp.ownedSpirit.GetComponent<SpiritBase>().GetKnownAction(buttonComp.actionIndex).GetComponent<ActionBase>();
            }

            infoBox.SetActive(true);
            infoBoxTitle.text = actionComp.actionName;

            string actionDescription = actionComp.actionDescription;

            switch (actionComp.statusEffect)
            {
                case ActionBase.StatusEffect.Damage:
                    actionDescription += "\n" + actionComp.triggers + "x" + actionComp.potency + " Damage";
                    break;
                case ActionBase.StatusEffect.Heal:
                    actionDescription += "\n" + actionComp.triggers + "x" + actionComp.potency + " Healing";
                    break;
                case ActionBase.StatusEffect.Shield:
                    actionDescription += "\n" + actionComp.triggers + "x" + actionComp.potency + " Shield";
                    break;
                case ActionBase.StatusEffect.Tank:
                    actionDescription += "\n"+ actionComp.potency + " Block";
                    break;
                case ActionBase.StatusEffect.Power:
                    actionDescription += "\n" + actionComp.potency + " Power";
                    break;
                default:
                    break;
                    
            }

            actionDescription += "\nCooldown: " + actionComp.actionCooldown + " seconds";
            infoBoxDescription.text = actionDescription;

            //infoBoxDescription.text = actionComp.actionDescription + "\nCooldown: " + actionComp.actionCooldown + " seconds";
            return;
        }
    }

    void HideInfoBox()
    {
        infoBox.SetActive(false);
    }

    public void RecruitSpirit(GameObject spirit)
    {
        playerSpirits.Add(spirit);

        spirit.tag = "AllySpirit";

        Debug.Log(playerSpirits.Count);

        spirit.transform.parent = transform;
        spirit.transform.position = BattleManager.Instance.allySpawns[playerSpirits.IndexOf(spirit)].transform.position;

        screenComp.choiceSelected(spirit);
        
        BattleManager.Instance.UpdateAllySpirits();
    }

    public void OpenUnlockScreen()
    {
        upgradeIndex = 0;
        if (playerSpirits.Count < 3)
        {
            screenComp.ActivateScreen(UnlockScreen.UnlockChoice.Spirit, null);
        }
        else
        {
            for (int i = 0; i < playerSpirits.Count; i++)
            {
                if (playerSpirits[i].GetComponent<SpiritBase>().GetKnownActions().Count < 2)
                {
                    upgradeIndex = i;
                    screenComp.ActivateScreen(UnlockScreen.UnlockChoice.Action, playerSpirits[i]);
                    return;
                }
            }
        }
    }

    public void ProgressUnlockScreen()
    {
        //upgradeIndex ++;
        while (upgradeIndex < 3 && upgradeIndex < playerSpirits.Count)
        {
            if (playerSpirits[upgradeIndex].GetComponent<SpiritBase>().GetKnownActions().Count < 2)
            {
                screenComp.ActivateScreen(UnlockScreen.UnlockChoice.Action, playerSpirits[upgradeIndex]);
                upgradeIndex++;
                return;
            }
            upgradeIndex++;
        }

        if (BattleManager.Instance.wave == 0 && playerSpirits.Count < 2)
        {
            OpenUnlockScreen();
        }
        else
        {
            screenComp.CloseScreen();
            BattleManager.Instance.ToggleButtonHidden(false);
        }

        //OpenUnlockScreen();
    }

    public void selectTarget(GameObject target)
    {
        targetedEnemy = target;
    }

    public void clearTarget() { targetedEnemy = null; }

    public GameObject getTarget() { return targetedEnemy; }
}
