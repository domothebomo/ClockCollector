using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritBase : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [SerializeField] GameObject[] knownMoves;

    TMP_Text statusText;

    SpriteRenderer spriteRenderer;

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

    }

    void UpdateUI()
    {
        statusText.text = (currentHealth +"/"+ maxHealth);
    }

    void TestAction()
    {
        Debug.Log("firing move!");
    }

    public void TickDamage()
    {
        currentHealth -= 1;
        UpdateUI();
    }
}
