using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    MusicManager mm;
    void Start()
    {
        mm = FindAnyObjectByType<MusicManager>();
    }

    public void Click()
    {
        mm.SelectSFX();
    }
}
