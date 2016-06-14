using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class CommonDataUi : MonoBehaviour
    {
        public static IEnumerator SceneLoad(string scene, AudioClip soundButton)
        {
            SoundTrack.PlaySound(soundButton);
            yield return new WaitForSeconds(soundButton.length);
            SceneManager.LoadScene(scene);
        }

        public static void SetAudioSource(AudioClip soundButton, GameObject gameObject)
        {
            var audioSource = gameObject.GetComponent<AudioSource> ();
            audioSource.clip = soundButton;
        }
    }
}
