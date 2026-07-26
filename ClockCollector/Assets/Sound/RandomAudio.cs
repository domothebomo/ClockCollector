using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    [SerializeField] AudioSource hitSFX;
    [SerializeField] AudioSource buffSFX;
    [SerializeField] AudioSource deathSFX;
    [SerializeField] AudioSource selectSFX;
    [SerializeField] AudioSource rewindSFX;

    [SerializeField] float lowerPitchBound = 0.8f;
    [SerializeField] float upperPitchBound = 1.2f;

    public void AttackSFX()
    {
        hitSFX.pitch = Random.Range(lowerPitchBound, upperPitchBound);
        hitSFX.Play();
    }

    public void BuffSFX()
    {
        buffSFX.pitch = Random.Range(lowerPitchBound, upperPitchBound);
        buffSFX.Play();
    }

    public void DeathSFX()
    {
        deathSFX.pitch = Random.Range(lowerPitchBound, upperPitchBound);
        deathSFX.Play();
    }

    public void SelectSFX()
    {
        selectSFX.pitch = Random.Range(lowerPitchBound, upperPitchBound);
        selectSFX.Play();
    }

    public void RewindSFX()
    {
        rewindSFX.pitch = Random.Range(lowerPitchBound, upperPitchBound);
        rewindSFX.Play();
    }
}
