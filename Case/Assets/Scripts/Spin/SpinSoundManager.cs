using UnityEngine;

public class SpinSoundManager : MonoBehaviour
{
    #region Variables

    [Header("Spin Sound")]
    [SerializeField] private AudioSource spinWheelSound;
    private const float SpinSoundLowPitch = .7f;
    private const float SpinSoundHighPitch = 1f;

    #endregion

    #region Methods

    public void PlaySpinSound()
    {
        spinWheelSound.Play();
        spinWheelSound.pitch = SpinSoundLowPitch;
    }

    public void StopSpinSound()
    {
        spinWheelSound.Stop();
    }
    
    public void AdjustSpinSoundPitch(float angle)
    {
        var targetPitch = (Mathf.Abs(angle) < 90f) ? SpinSoundLowPitch : SpinSoundHighPitch;
        spinWheelSound.pitch = Mathf.Lerp(spinWheelSound.pitch, targetPitch, Time.deltaTime * 2f);
    }

    #endregion
    

}
