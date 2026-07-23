using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritBase : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [SerializeField] GameObject[] knownMoves;

    TMP_Text statusText;

    float timer = 0.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        statusText = gameObject.GetComponentInChildren<TMP_Text>();
        if (statusText != null )
        {
            Debug.Log("UI found");
        }
        else
        {
            Debug.Log("not found");
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0;
            TestAction();
            currentHealth -= 1;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        statusText.text = (currentHealth +"/"+ maxHealth);
    }

    void TestAction()
    {
        Debug.Log("firing move!");
    }
}
