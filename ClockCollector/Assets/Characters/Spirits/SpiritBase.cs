using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritBase : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [SerializeField] GameObject[] knownActions;

    TMP_Text statusText;

    SpriteRenderer spriteRenderer;

    bool trigger = false;

    float timer = 0.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        statusText = gameObject.GetComponentInChildren<TMP_Text>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 95 && gameObject.tag != "EnemySpirit" && knownActions.Length != 0 && trigger == false)
        {
            UseAction(knownActions[0], GameObject.FindGameObjectWithTag("EnemySpirit"));
            trigger = true;
        }
    }

    void UpdateUI()
    {
        statusText.text = (currentHealth +"/"+ maxHealth);
    }

    void UseAction(GameObject action, GameObject target)
    {
        action.GetComponent<ActionBase>().ActionTriggered(target);
        Debug.Log("firing move!");
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
