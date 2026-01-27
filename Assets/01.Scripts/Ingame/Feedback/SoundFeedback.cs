using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private AudioSource _manualAudio;
    [SerializeField] private AudioSource _autoAudio;
    
    public void Play(ClickInfo clickInfo)
    {
        switch (clickInfo.Type)
        {
            case EClickType.Auto:
                _autoAudio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                _autoAudio.Play();
                break;
            
            case EClickType.Manual:
                _manualAudio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                _manualAudio.Play();
                break;
        }
    }
}
