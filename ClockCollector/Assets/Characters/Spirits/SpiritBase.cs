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
    [SerializeField] Color defeatColor;

    bool trigger = false;

    float timer = 0.0f;

    public bool selected = false;
    public bool targeted = false;

    bool cooldownActive = false;
    float currentCooldown = 5.0f;
    float cooldownTimer = 0.0f;

    bool defeated = false;
    
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
        
        if (gameObject.tag == "EnemySpirit")
        {
            currentCooldown = Random.Range(2, 6);
            cooldownActive = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (defeated) { return; }

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

        if (gameObject.tag == "EnemySpirit" && !cooldownActive)
        {
            SelectEnemyAction();
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
                actionButton.GetComponentInChildren<TMP_Text>().text = knownActions[i].GetComponent<ActionBase>().actionName;

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
        if (defeated) { return; }

        if (selected || targeted)
        {
            ClearSelection();

            if (gameObject.tag == "EnemySpirit")
            {
                PlayerManager.Instance.clearTarget();
            }

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

        if (defeated) 
        { 
            spriteRenderer.color = defeatColor;
        }
        else if (gameObject.tag == "EnemySpirit")
        {
            spriteRenderer.color = enemyColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        UpdateActionUI();
    }

    void SelectEnemyAction()
    {
        int actionIndex = Random.Range(0, knownActions.Count);

        int targetIndex = Random.Range(0, BattleManager.Instance.GetAllySpirits().Length);

        GameObject target = BattleManager.Instance.GetAllySpirits()[targetIndex];

        //ActionBase actionComp = knownActions[actionIndex].GetComponent<ActionBase>();

        UseAction(actionIndex, target);
    }

    public void SelectPlayerAction(int actionIndex)
    {
        UseAction(actionIndex, PlayerManager.Instance.getTarget());
    }

    void UseAction(int actionIndex, GameObject target)
    {
        //action.GetComponent<ActionBase>().ActionTriggered(target);

        ActionBase actionComp = knownActions[actionIndex].GetComponent<ActionBase>();

        if (actionComp.OnCooldown())
        {
            return;
        }

        if (actionComp.requiresTarget && target == null)
        {
            return;
        }

        cooldownActive = true;
        currentCooldown = actionComp.actionCooldown;
        UpdateActionUI();

        actionComp.ActionTriggered(target);

        string statusMessage;
        if (actionComp.requiresTarget)
        {
            statusMessage = displayName + " used " + actionComp.actionName + " on " + target.GetComponent<SpiritBase>().displayName;
        }
        else
        {
            statusMessage = displayName + " used " + actionComp.actionName;
        }
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateUI();
    }

    public void HealDamage(int damage)
    {
        currentHealth += damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    void Die()
    {
        defeated = true;
        spriteRenderer.color = defeatColor;

        if (gameObject.tag == "EnemySpirit")
        {
            BattleManager.Instance.CheckEnemiesDefeated();
        }
    }

    public bool IsDefeated() { return defeated; }

}
