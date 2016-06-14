using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class UiMenu : MonoBehaviour
    {
        public GUIStyle CustomBoxMenu;
        public GUIStyle CustomButtonGame;
        public GUIStyle CustomButtonSettings;
        public GUIStyle CustomButtonExist;

        private static readonly float OffsetWidth = Screen.width/2.7f - 90;
        private static readonly float OffsetHeihgt = Screen.height/3f;

        public AudioClip SoundButton;

        void Awake()
        {
            CommonDataUi.SetAudioSource(SoundButton, gameObject);
        }

        private IEnumerator Terminate()
        {
            SoundTrack.PlaySound (SoundButton);
            yield return new WaitForSeconds(SoundButton.length);
            Application.Quit();
        }

        void OnGUI()
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", CustomBoxMenu);

            if (GUI.Button(new Rect(OffsetWidth, OffsetHeihgt - 100, 256, 128), "", CustomButtonGame))
            {
                StartCoroutine(CommonDataUi.SceneLoad("Game", SoundButton));
            }

            if (GUI.Button(new Rect(OffsetWidth, OffsetHeihgt + 40, 512, 128), "", CustomButtonSettings))
            {
                StartCoroutine(CommonDataUi.SceneLoad("Settings", SoundButton));
            }

            if (GUI.Button(new Rect(OffsetWidth, OffsetHeihgt + 180, 450, 128), "", CustomButtonExist))
            {
                StartCoroutine(Terminate());
            }

        }
    }
}
