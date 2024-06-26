using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";
    public static SoundManager Instance { get; private set; }


    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;

    private float volume = 1f;
    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME , 1f);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.OnAnyPickedSomething += Instance_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipsRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipsRefsSO.objectDrops , baseCounter.transform.position);
    }

    private void Instance_OnPickSomething(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipsRefsSO.objectPickup, player.transform.position);
    }
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipsRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefsSO.deliveryFail , deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray , Vector3 position , float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0 , audioClipArray.Length)] , position , volume);
    }
    private void PlaySound(AudioClip audioClip , Vector3 position , float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip , position , volumeMultiplier * volume);
    }

    public void PlayFootstepsSound(Vector3 position , float volume)
    {
        PlaySound(audioClipsRefsSO.footstep , position , volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipsRefsSO.warning , Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipsRefsSO.warning, position);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME , volume);
        PlayerPrefs.Save();
    }

    public float GetVoulume()
    {
        return volume;
    }
}
