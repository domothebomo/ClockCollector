using System.Runtime.InteropServices;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioSource menuTrack;
    [SerializeField] AudioSource battleTrack;
    
    [SerializeField] AudioSource selectSFX;
    [SerializeField] float lowerPitchBound = 0.8f;
    [SerializeField] float upperPitchBound = 1.2f;

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

    public void SelectSFX()
    {
        selectSFX.pitch = Random.Range(lowerPitchBound, upperPitchBound);
        selectSFX.Play();
    }
}
