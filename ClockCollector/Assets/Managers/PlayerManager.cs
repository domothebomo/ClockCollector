using TMPro;
using UnityEngine;

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

    Camera cam;

    public GameObject infoBox;
    public TMP_Text infoBoxTitle;
    public TMP_Text infoBoxDescription;

    GameObject hoveredObject;
    SpiritBase spiritComp;
    ActionButton buttonComp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);

        cam = Camera.main;
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
            ActionBase actionComp = buttonComp.ownedSpirit.GetComponent<SpiritBase>().GetKnownAction(buttonComp.actionIndex).GetComponent<ActionBase>();

            infoBox.SetActive(true);
            infoBoxTitle.text = actionComp.actionName;
            infoBoxDescription.text = actionComp.actionDescription + "\nCooldown: " + actionComp.actionCooldown + " seconds";
            return;
        }
    }

    void HideInfoBox()
    {
        infoBox.SetActive(false);
    }

    public void selectTarget(GameObject target)
    {
        targetedEnemy = target;
    }

    public void clearTarget() { targetedEnemy = null; }

    public GameObject getTarget() { return targetedEnemy; }
}
