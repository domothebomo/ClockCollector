using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    private static BattleManager instance;
    
    public static BattleManager Instance {  get { return instance; } }

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

    bool battleActive = false;
    float battleTimer = 0.0f;

    GameObject[] allySpirits;
    GameObject[] enemySpirits;

    public CanvasGroup messageBox;
    public GameObject messageTemplate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        if (battleActive)
        {
            battleTimer += Time.deltaTime;
            if (battleTimer >= 1.0)
            {
                TickDown();
                battleTimer = 0.0f;
            }
        }
    }

    void StartBattle()
    {
        battleActive = true;

        allySpirits = GameObject.FindGameObjectsWithTag("AllySpirit");
        enemySpirits = GameObject.FindGameObjectsWithTag("EnemySpirit");
    }

    void TickDown()
    {
        foreach (GameObject spirit in allySpirits)
        {
            spirit.GetComponent<SpiritBase>().TickDamage();
        }
        foreach (GameObject spirit in enemySpirits)
        {
            spirit.GetComponent<SpiritBase>().TickDamage();
        }
    }

    public void DisplayStatusMessage(string message)
    {
        GameObject newMessage = Instantiate(messageTemplate, messageBox.transform);

        newMessage.GetComponent<TMP_Text>().text = message;
    }

    public void ClearAllySelection(GameObject newSelected)
    {
        foreach (GameObject spirit in allySpirits)
        {
            SpiritBase spiritComp = spirit.GetComponent<SpiritBase>();
            if (spirit != newSelected && spiritComp.selected)
            {
                spiritComp.ClearSelection();
            }
        }
    }

    public void ClearEnemySelection(GameObject newSelected)
    {
        foreach (GameObject spirit in enemySpirits)
        {
            SpiritBase spiritComp = spirit.GetComponent<SpiritBase>();
            if (spirit != newSelected && spiritComp.targeted)
            {
                spiritComp.ClearSelection();
            }
        }
    }

    public GameObject[] GetAllySpirits()
    {
        return allySpirits;
    }
}
