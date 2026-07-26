using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockScreen : MonoBehaviour
{
    bool choosingSpirit;
    public enum UnlockChoice {Spirit, Action };

    public UnlockChoice unlockChoice;

    public GameObject[] choicePositions;

    GameObject[] options;

    GameObject learningSpirit;

    public TMP_Text headerText;

    List<int> usedIndices = new List<int>();

    public Animator animComp;

    public GameObject actionChoice;

    public Canvas canvasComp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //animComp = GetComponent<Animator>();

        //ActivateScreen(UnlockChoice.Spirit, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateScreen(UnlockChoice choice, GameObject spirit)
    {
        options = new GameObject[choicePositions.Length];
        unlockChoice = choice;

        if (choice == UnlockChoice.Action)
        {
            learningSpirit = spirit;
        }

        PopulateScreen();

        animComp.Play("UnlockScreen");
    }

    public void PopulateScreen()
    {

        usedIndices = new List<int>();

        if (unlockChoice == UnlockChoice.Spirit)
        {
            headerText.text = "Choose a chronogeist to join your party...";

            for (int i = 0; i < choicePositions.Length; i++)
            {
                int randomIndex = GetUnusedIndex(UnlockChoice.Spirit);

                options[i] = Instantiate(BattleManager.Instance.spiritPool[randomIndex]);
                options[i].transform.position = choicePositions[i].transform.position;
                options[i].transform.parent = transform;
                options[i].GetComponent<SpiritBase>().EnableRecruiting();
            }
        }
        else if (unlockChoice == UnlockChoice.Action)
        {
            headerText.text = learningSpirit.GetComponent<SpiritBase>().displayName + "...\nChoose an action to learn...";

            for (int i = 0; i < choicePositions.Length; i++)
            {
                int randomIndex;
                ActionButton actionOption;

                if (i == 0)
                {
                    randomIndex = Random.Range(0, BattleManager.Instance.attackPool.Length);
                    usedIndices.Add(randomIndex);
                    actionOption = Instantiate(actionChoice).GetComponentInChildren<ActionButton>();
                    actionOption.ownedSpirit = learningSpirit;
                    actionOption.action = BattleManager.Instance.attackPool[randomIndex];
                }
                else
                {
                    randomIndex = GetUnusedIndex(UnlockChoice.Action);
                    actionOption = Instantiate(actionChoice).GetComponentInChildren<ActionButton>();
                    actionOption.ownedSpirit = learningSpirit;
                    actionOption.action = BattleManager.Instance.actionPool[randomIndex];
                }

                actionOption.actionText.text = actionOption.action.GetComponent<ActionBase>().actionName;
                actionOption.unlockScreenComp = this;
                options[i] = actionOption.gameObject;

                options[i].transform.position = choicePositions[i].transform.position;
                options[i].transform.parent = canvasComp.transform;

            }
        }
    }

    int GetUnusedIndex(UnlockChoice choice)
    {
        int randomIndex = 0;

        if (choice == UnlockChoice.Spirit)
        {
            randomIndex = Random.Range(0, BattleManager.Instance.spiritPool.Length);
            while (usedIndices.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, BattleManager.Instance.spiritPool.Length);
            }
            usedIndices.Add(randomIndex);
        }
        else if (choice == UnlockChoice.Action)
        {
            randomIndex = Random.Range(0, BattleManager.Instance.actionPool.Length);
            while (usedIndices.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, BattleManager.Instance.actionPool.Length);
            }
            usedIndices.Add(randomIndex);
        }
        return randomIndex;
    }

    public void choiceSelected(GameObject choice)
    {
        if (unlockChoice == UnlockChoice.Spirit)
        {
            for (int i = 0; i < choicePositions.Length; i++)
            {
                if (options[i] != choice)
                {
                    Destroy(options[i]);
                }
            }
            //ActivateScreen(UnlockChoice.Action, choice);
        }
        else
        {
            for (int i = 0; i < choicePositions.Length; i++)
            {
                Destroy(options[i]);
            }
        }

        PlayerManager.Instance.ProgressUnlockScreen();
        //ActivateScreen(UnlockChoice.Action, choice);
        
    }

    public void CloseScreen()
    {
        animComp.Play("UnlockScreen2");
    }




}
