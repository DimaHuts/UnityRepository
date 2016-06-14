using UnityEngine;

namespace Assets.Scripts
{
    public class UiSettings : MonoBehaviour
    {
        public GUIStyle CustomBoxSettings;
        public GUIStyle CustomBoxSpeed;
        public GUIStyle CustomBoxBonus;
        public GUIStyle CustomBoxVolume;
        public GUIStyle CustomButtonMenu;

        private static bool _toggle10;
        private static bool _toggle15;
        private static bool _toggle20;
        private const string ToggleName1 = "10";
        private const string ToggleName2 = "15";
        private const string ToggleName3 = "20";

        private static int _selectedToolbarItem;
        private static readonly string[] ToolbarItems = {"0", "1", "2"};

        private static float _slider;

        private readonly static float BoxSettingsOffsetWidth = Screen.width/4f;
        private readonly static Vector2 BoxSize = new Vector2(Screen.width/1.6f, 70);
        private readonly static Vector2 ToggleSize = new Vector2(30, 20);
        private readonly static Vector2 ToolbarSize = new Vector2(100, 20);
        private readonly static Vector2 SliderSize = new Vector2(100, 20);
        private static readonly float BoxOffsetWidth = BoxSettingsOffsetWidth + 15;
        private const float SizeBetweenBoxes = 50;

        public AudioClip SoundButton;

        void OnGUI()
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", CustomBoxSettings);

            GUI.Box(new Rect(new Vector2(BoxOffsetWidth, SizeBetweenBoxes), BoxSize), "", CustomBoxSpeed);
            _toggle10 = GUI.Toggle( new Rect( 
                new Vector2(BoxSettingsOffsetWidth + 70, 90 ), ToggleSize ), _toggle10, ToggleName1);

            _toggle15 = GUI.Toggle( new Rect( 
                new Vector2(BoxSettingsOffsetWidth + 140, 90 ), ToggleSize ), _toggle15, ToggleName2);

            _toggle20 = GUI.Toggle( new Rect( 
                new Vector2(BoxSettingsOffsetWidth + 210, 90 ), ToggleSize ), _toggle20, ToggleName3);

            GUI.Box(new Rect(new Vector2(BoxOffsetWidth, 3*SizeBetweenBoxes), BoxSize), "", CustomBoxBonus);
            _selectedToolbarItem = GUI.Toolbar(new Rect( 
                new Vector2(BoxSettingsOffsetWidth + 130, 185), ToolbarSize ), _selectedToolbarItem, ToolbarItems);
       
            GUI.Box(new Rect(new Vector2(BoxOffsetWidth, 5*SizeBetweenBoxes), BoxSize), "", CustomBoxVolume);
            GUI.Label(new Rect(BoxSettingsOffsetWidth + 130, 280, 30, 20), _slider.ToString());
            _slider = GUI.HorizontalSlider(new Rect(
                new Vector2(BoxSettingsOffsetWidth + 160, 285), SliderSize), _slider, 0, 100);

            if (GUI.Button(new Rect(BoxOffsetWidth, Screen.height - 150, 128, 128), "", CustomButtonMenu))
            {
                StartCoroutine(CommonDataUi.SceneLoad("Menu", SoundButton));
            }

        }
    }
}
