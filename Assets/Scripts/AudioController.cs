using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] AudioClip teamChangeAudio;
    [SerializeField] List<AudioClip> unitDiesAudio = new();

    AudioSource myAudioPlayer;

    private void Awake()
    {
        #region Singleton
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        #endregion
    }


    private void Start()
    {
        myAudioPlayer = GetComponent<AudioSource>();
    }

    public void PlayTeamChangeSound()
    {
        myAudioPlayer.PlayOneShot(teamChangeAudio, 1f);
    }

    public void PlayUnitDeathSound()
    {
        int rand = Random.Range(0, unitDiesAudio.Count);


        myAudioPlayer.PlayOneShot(unitDiesAudio[rand], 1f);
    }
}
