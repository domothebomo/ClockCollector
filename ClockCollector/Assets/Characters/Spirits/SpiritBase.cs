using System.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection.Metadata.Ecma335;

public class SpiritBase : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    int shieldHealth = 0;
    int powerStacks = 0;
    int blockStacks = 0;

    [SerializeField] public string displayName;
    [SerializeField] public string abilityDescription;

    [SerializeField] GameObject[] startingActions;
    List<GameObject> knownActions = new List<GameObject>();

    public TMP_Text statusText;
    public TMP_Text shieldText;

    public GameObject powerIcon;
    public TMP_Text powerCount;
    public GameObject blockIcon;
    public TMP_Text blockCount;

    SpriteRenderer spriteRenderer;

    [SerializeField] GameObject[] actionButtons;

    [SerializeField] Color selectColor;
    [SerializeField] Color enemyColor;
    [SerializeField] Color targetColor;
    [SerializeField] Color defeatColor;

    [SerializeField] GameObject cooldownMeter;
    [SerializeField] Image meterFill;


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

        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        foreach (GameObject action in startingActions)
        {
            LearnAction(action);
        }
        
        if (gameObject.tag == "EnemySpirit")
        {
            InitializeEnemy();
        }

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (defeated) { return; }

        if (cooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            meterFill.fillAmount = cooldownTimer / currentCooldown;

            if (cooldownTimer >= currentCooldown)
            {
                cooldownActive = false;
                cooldownTimer = 0.0f;
                cooldownMeter.SetActive(false);
                UpdateActionUI();
            }
        }

        if (gameObject.tag == "EnemySpirit" && !cooldownActive)
        {
            SelectEnemyAction();
        }
    }

    public void InitializeEnemy()
    {
        gameObject.tag = "EnemySpirit";

        currentCooldown = Random.Range(2, 6);
        cooldownActive = true;
        cooldownMeter.SetActive(true);
        UpdateEnemySprite();
    }

    void UpdateEnemySprite()
    {
        spriteRenderer.color = enemyColor;
        spriteRenderer.flipX = true;
        cooldownMeter.transform.localPosition = new Vector3(-90.0f,  -150.0f, 0.0f);
    }

    void UpdateUI()
    {
        statusText.text = (currentHealth +"/"+ maxHealth);

        if (shieldHealth > 0)
        {
            shieldText.gameObject.SetActive(true);
            shieldText.text = "+" + shieldHealth;
        }
        else
        {
            shieldText.gameObject.SetActive(false);
        }

        if (powerStacks > 0)
        {
            powerIcon.SetActive(true);
            powerCount.text = powerStacks.ToString();
        }
        else
        {
            powerIcon.SetActive(false);
        }

        if (blockStacks > 0)
        {
            blockIcon.SetActive(true);
            blockCount.text = blockStacks.ToString();
        }
        else
        {
            blockIcon.SetActive(false);
        }
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

    public GameObject GetKnownAction(int actionIndex)
    {
        return knownActions[actionIndex];
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
        cooldownMeter.SetActive(true);
        currentCooldown = actionComp.actionCooldown;
        UpdateActionUI();
        
        GameObject finalTarget = DetermineTarget(actionComp, target);

        for (int i = 0; i < actionComp.triggers; i++)
        {

            actionComp.ActionTriggered(finalTarget);
        }

        if (actionComp.actionType == ActionBase.ActionType.Attack && powerStacks > 0)
        {
            AddPower(-1);
        }

        string statusMessage;
        if (actionComp.requiresTarget)
        {
            if (finalTarget != target)
            {
                statusMessage = displayName + "'s " + actionComp.actionName + " was blocked by " + finalTarget.GetComponent<SpiritBase>().displayName;
            }
            else
            {
                statusMessage = displayName + " used " + actionComp.actionName + " on " + target.GetComponent<SpiritBase>().displayName;
            }
        }
        else
        {
            statusMessage = displayName + " used " + actionComp.actionName;
        }
        BattleManager.Instance.DisplayStatusMessage(statusMessage);

        Debug.Log("firing move!");
    }

    public GameObject DetermineTarget(ActionBase action, GameObject currentTarget)
    {
        if (!action.requiresTarget || currentTarget.GetComponent<SpiritBase>().GetBlockStacks() > 0)
        {
            return currentTarget;
        }
        else
        {
            GameObject[] spirits;
            if (currentTarget.CompareTag("EnemySpirit"))
            {
                spirits = BattleManager.Instance.GetEnemySpirits();
            }
            else
            {
                spirits = BattleManager.Instance.GetAllySpirits();
            }
            foreach (GameObject spirit in spirits)
            {
                if (spirit.GetComponent<SpiritBase>().GetBlockStacks() > 0)
                {
                    spirit.GetComponent<SpiritBase>().AddBlock(-1);
                    return spirit;
                }
            }

        }

        return currentTarget;
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
        int remainingDamage = damage;
        if (shieldHealth > 0)
        {
            if (shieldHealth < damage)
            {
                remainingDamage = damage - shieldHealth;
            }
            else
            {
                remainingDamage = 0;
            }

            shieldHealth -= damage;
            shieldHealth = Mathf.Max(0, shieldHealth);
        }

        currentHealth -= remainingDamage;
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

    public void AddShield(int shield)
    {
        shieldHealth += shield;
        UpdateUI();
    }

    public void AddPower(int power)
    {
        powerStacks += power;
        UpdateUI();
    }

    public void AddBlock(int block)
    {
        blockStacks += block;
        UpdateUI();
    }

    public int GetPowerStacks()
    {
        return powerStacks;
    }

    public int GetBlockStacks()
    {
        return blockStacks;
    }

    void Die()
    {
        ClearSelection();

        defeated = true;
        spriteRenderer.color = defeatColor;
        cooldownMeter.SetActive(false);

        AddPower(-GetPowerStacks());
        AddBlock(-GetBlockStacks());
        AddShield(-shieldHealth);

        if (gameObject.tag == "EnemySpirit")
        {
            if (PlayerManager.Instance.getTarget() == gameObject)
            {
                PlayerManager.Instance.clearTarget();
            }

            BattleManager.Instance.CheckEnemiesDefeated();
        }
    }

    public bool IsDefeated() { return defeated; }

}
