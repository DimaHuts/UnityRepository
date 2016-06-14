using UnityEngine;

namespace Assets.Scripts
{
    public class UiGame : MonoBehaviour
    {
        public GUIStyle CustomButtonMenu;
        public AudioClip SoundButton;
        private const float BtnMenuWidth = 64;
        private const float BtnMenuHeight = 64;

        void Awake()
        {
            CommonDataUi.SetAudioSource(SoundButton, gameObject);
        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 130, 100, 20), "Очки: " + Game.Point);
            if (GUI.Button(new Rect(6, Screen.height-BtnMenuHeight - 6, BtnMenuWidth, BtnMenuHeight), "", CustomButtonMenu))
            {
                StartCoroutine(CommonDataUi.SceneLoad("Menu", SoundButton));
            }
        }
    }
}
