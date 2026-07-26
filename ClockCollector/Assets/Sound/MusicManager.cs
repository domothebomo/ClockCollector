using System.Runtime.InteropServices;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioSource menuTrack;
    [SerializeField] AudioSource battleTrack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void StartBattle()
    {
        menuTrack.Pause();
        battleTrack.Play();
    }

    public void StopBattle()
    {
        battleTrack.Stop();
        menuTrack.UnPause();
    }
}
