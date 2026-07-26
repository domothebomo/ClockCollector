using TMPro;
using UnityEngine;

public class StatusMessage : MonoBehaviour
{

    public float lifeSpan = 3.0f;
    float lifeTimer = 0.0f;

    TMP_Text textComp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComp = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeSpan)
        {
            Destroy(gameObject);
        }

        Color tempColor = textComp.color;
        tempColor.a = 1 - (lifeTimer / lifeSpan);
        textComp.color = tempColor;
    }
}
