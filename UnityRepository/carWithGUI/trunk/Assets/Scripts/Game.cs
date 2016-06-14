using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        private static int _points;
        private const int CollisionWithWalls = -10;
        private const int TriggerWithRedBall = 20;
        private const int TriggerWithBlueBall = 40;
        private static readonly string[] NameBonus = {"BlueSphere", "RedSphere"};
        private static int _currentPositionBonus;
        private static readonly Vector3[] PositionBonus = {  new Vector3(-3.221828f, 0.198f, 0), new Vector3(4.25f, 0.48f, 0),
                                                             new Vector3(2.07f, 0.48f, 3.27f),   new Vector3(-1.41f, 0.321f, 0.48f),
                                                             new Vector3(-4.09f, 0.198f, -4.31f),  new Vector3(2.9f, 0.2f, -3.21f),
                                                             new Vector3(-0.79f, 0.2f, -0.74f)   
                                                           };

        public static string BonusName
        {
            get
            {
                var choice = Random.Range(0, 2);
            
                return NameBonus[choice];
            }
        }

        public static Vector3 BonusPosition()
        {
            if (_currentPositionBonus == PositionBonus.Length)
            {
                _currentPositionBonus = 0;
            }
            _currentPositionBonus++;
            return PositionBonus[_currentPositionBonus-1];
        }

        public static void GeneratedBonus()
        {
            var name = BonusName;
            var bonus = Instantiate(Resources.Load(name)) as GameObject;
            if (bonus == null) return;
            bonus.name = name;
            bonus.transform.position = BonusPosition();
        }

        private static void SetAndDisplayPoints(int value)
        {
            Point = value;
            Print();
        }

        public static int Point 
        {
            set { _points = value == 0 ? 0 : _points + value; }
            get { return _points; }
        }

        private static void Print()
        {
            Debug.Log("Count of Points " + _points);
        }

        private void OnCollisionEnter()
        {
            SetAndDisplayPoints(CollisionWithWalls);
        }

        private void OnTriggerEnter(Collider obj)
        {
            var nameObj = obj.gameObject.name;
            SetAndDisplayPoints(nameObj == "Hook" ? 0 : nameObj == "RedSphere" ? TriggerWithRedBall : TriggerWithBlueBall);
        }

        void Start()
        {
            GeneratedBonus();
            GeneratedBonus();
        }

    }
}
