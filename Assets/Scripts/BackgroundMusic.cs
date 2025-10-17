using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this background music
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] List<AudioClip> songs;
    [SerializeField] AudioSource audioSource;


    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SettingData.Instance != null);

        Application.runInBackground = true;
        songs = Resources.LoadAll<AudioClip>("Songs").ToList();

        audioSource.volume = SettingData.Instance.gameSetting.song_Sound / 100;

        if (songs.Count == 0) yield return null;
        audioSource.PlayOneShot(songs[Random.Range(0, songs.Count - 1)]);
    }

    private void Update()
    {
        audioSource.volume = SettingData.Instance.gameSetting.song_Sound / 100;
        if (songs.Count == 0) return;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(songs[Random.Range(0, songs.Count - 1)]);
        }
    }
}
