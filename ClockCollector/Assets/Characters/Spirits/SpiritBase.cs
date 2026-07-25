using System.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpiritBase : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [SerializeField] string displayName;

    [SerializeField] GameObject[] startingActions;
    List<GameObject> knownActions = new List<GameObject>();

    TMP_Text statusText;

    SpriteRenderer spriteRenderer;

    [SerializeField] GameObject[] actionButtons;

    [SerializeField] Color selectColor;
    [SerializeField] Color enemyColor;
    [SerializeField] Color targetColor;

    bool trigger = false;

    float timer = 0.0f;

    public bool selected = false;
    public bool targeted = false;

    bool cooldownActive = false;
    float currentCooldown = 2.0f;
    float cooldownTimer = 0.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        statusText = gameObject.GetComponentInChildren<TMP_Text>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        foreach (GameObject action in startingActions)
        {
            LearnAction(action);
        }    

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 95 && gameObject.tag != "EnemySpirit" && knownActions.Count != 0 && trigger == false)
        {
            //UseAction(knownActions[0], GameObject.FindGameObjectWithTag("EnemySpirit"));
            trigger = true;
        }

        if (cooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= currentCooldown)
            {
                cooldownActive = false;
                cooldownTimer = 0.0f;
                UpdateActionUI();
            }
        }
    }

    void UpdateUI()
    {
        statusText.text = (currentHealth +"/"+ maxHealth);
    }

    void UpdateActionUI()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject actionButton = actionButtons[i];
            if (i >= knownActions.Count)
            {
                break;
            }

            if (selected)
            {
                actionButton.SetActive(true);
                actionButton.GetComponentInChildren<TMP_Text>().text = knownActions[i].GetComponent<PunchAction>().actionName;

                if (OnCooldown())
                {
                    actionButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    actionButton.GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                actionButton.SetActive(false);
            }
        }
    }

    public void LearnAction(GameObject actionClass)
    {
        GameObject newAction = Instantiate(actionClass);

        newAction.gameObject.transform.parent = gameObject.transform;
        knownActions.Add(newAction);
    }

    public void SelectSpirit()
    {
        if (selected || targeted)
        {
            ClearSelection();
            return;
        }

        if (gameObject.tag == "AllySpirit")
        {
            selected = true;
            spriteRenderer.color = selectColor;
            UpdateActionUI();
            BattleManager.Instance.ClearAllySelection(gameObject);
        }
        else if (gameObject.tag == "EnemySpirit")
        {
            targeted = true;
            spriteRenderer.color = targetColor;
            BattleManager.Instance.ClearEnemySelection(gameObject);
            PlayerManager.Instance.selectTarget(gameObject);
        }
    }

    public void ClearSelection()
    {
        selected = false;
        targeted = false;

        if (gameObject.tag == "EnemySpirit")
        {
            spriteRenderer.color = enemyColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        UpdateActionUI();
    }

    public void UseAction(int actionIndex)
    {
        //action.GetComponent<ActionBase>().ActionTriggered(target);

        ActionBase actionComp = knownActions[actionIndex].GetComponent<ActionBase>();

        if (actionComp.OnCooldown())
        {
            Debug.Log("on cooldown asshole");
            return;
        }

        cooldownActive = true;
        currentCooldown = actionComp.actionCooldown;
        UpdateActionUI();

        actionComp.ActionTriggered(PlayerManager.Instance.getTarget());

        string statusMessage = displayName + " used " + actionComp.actionName + " on " + PlayerManager.Instance.getTarget().GetComponent<SpiritBase>().displayName;

        BattleManager.Instance.DisplayStatusMessage(statusMessage);

        Debug.Log("firing move!");
    }

    public bool OnCooldown()
    {
        return cooldownActive;
    }

    public void TickDamage()
    {
        currentHealth -= 1;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();
    }
}
