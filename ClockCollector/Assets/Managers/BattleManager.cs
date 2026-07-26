using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    public int wave = 0;

    public GameObject[] spiritPool;
    public GameObject[] actionPool;
    public GameObject[] attackPool;
    public GameObject bossSpirit;

    public GameObject[] enemySpawns;
    public GameObject[] allySpawns;

    public CanvasGroup messageBox;
    public GameObject messageTemplate;

    public GameObject progressButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        //PrepareBattle();
        messageBox.gameObject.SetActive(false);
        ToggleButtonHidden(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (battleActive)
        {
            battleTimer += Time.deltaTime;
            if (battleTimer >= 1.2)
            {
                TickDown();
                battleTimer = 0.0f;
            }
        }
    }
    
    public void PrepareBattle()
    {
        ToggleButtonHidden(true);

        int enemyCount = 1;
        int moveCount = 2;
        bool spawnBoss = false;

        if (wave == 1)
        {
            enemyCount = 2;
        } else if (wave == 2)
        {
            enemyCount = 3;
        } else if (wave == 3)
        {
            enemyCount = 3;
            spawnBoss = true;
        }

        enemySpirits = new GameObject[enemyCount];

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject newEnemy;
            
            if (i == 2 && spawnBoss)
            {
                newEnemy = Instantiate(bossSpirit);
            }
            else
            {
                int spawnIndex = Random.Range(0, spiritPool.Length);
                newEnemy = Instantiate(spiritPool[spawnIndex]);
            }

            newEnemy.transform.position = enemySpawns[i].transform.position;

            for (int j = 0; j < moveCount; j++)
            {
                int actionIndex;
                if (j == 0)
                {
                    actionIndex = Random.Range(0, attackPool.Length);
                    newEnemy.GetComponent<SpiritBase>().LearnAction(attackPool[actionIndex]);
                }
                else
                {
                    actionIndex = Random.Range(0, actionPool.Length);
                    newEnemy.GetComponent<SpiritBase>().LearnAction(actionPool[actionIndex]);
                }
            }

            newEnemy.GetComponent<SpiritBase>().InitializeEnemy();
            //enemySpirits[i] = newEnemy;

        }

        StartBattle();
    }

    void StartBattle()
    {
        messageBox.gameObject.SetActive(true);
        battleActive = true;

        allySpirits = GameObject.FindGameObjectsWithTag("AllySpirit");
        enemySpirits = GameObject.FindGameObjectsWithTag("EnemySpirit");
    }

    public void ToggleButtonHidden(bool hidden)
    {
        progressButton.SetActive(!hidden);
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

    public void UpdateAllySpirits()
    {
        allySpirits = GameObject.FindGameObjectsWithTag("AllySpirit");
    }

    public void UpdateEnemySpirits()
    {
        enemySpirits = GameObject.FindGameObjectsWithTag("EnemySpirit");
    }

    public GameObject[] GetAllySpirits()
    {
        return allySpirits;
    }

    public GameObject[] GetEnemySpirits()
    {
        return enemySpirits;
    }

    public void CheckEnemiesDefeated()
    {
        foreach (GameObject enemy in enemySpirits)
        {
            if (!enemy.GetComponent<SpiritBase>().IsDefeated())
            {
                return;
            }
        }

        EndBattle(true);
    }

    public void EndBattle(bool victory)
    {

        ClearAllySelection(null);
        ClearEnemySelection(null);

        battleActive = false;

        messageBox.gameObject.SetActive(false);

        if (!victory)
        {
            return;
        }

        DestroyEnemies();

        wave++;
        PlayerManager.Instance.OpenUnlockScreen();
    }  

    void DestroyEnemies()
    {
        foreach(GameObject enemy in enemySpirits)
        {
            Destroy(enemy);
        }
    }
    
    public bool IsBattleActive()
    {
        return battleActive;
    }
}
