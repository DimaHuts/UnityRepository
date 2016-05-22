using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Assets.Scripts
{
    class Grid : MonoBehaviour
    {
        private static GameObject[,] _grid;
        private static readonly string[] InternalElements =
            {"GreenBallNew", "BlueBallNew", "OrangeBallNew", "BlackBall","RedBallNew"};
        private const string Background = "Background_Sky";
        private const string CellOfGrid = "Cell";
        private const int Rows = 8;
        private const int Columns = 8;
        private const int StartX = 1;
        private const int StartY = 1;
        private const float WideCell = 0.48f;
        private static GameObject internalElement;

        public static string InternalElement
        {
            get
            {
                return InternalElements[Random.Range(0, 5)];
            }
        }

        public static IEnumerator Complete()
        {
            for (var j = Columns - 1; j >= 0; j--)
            {
                for (var i = Rows - 1; i >= 0; i--)
                {
                    var internalElement = Instantiate(Resources.Load(InternalElement)) as GameObject;
                    if (internalElement == null) continue;
                    internalElement.transform.parent = _grid[i, j].transform;
                    internalElement.transform.DOMove(new Vector3(_grid[i, j].transform.position.x,
                                                                 _grid[i, j].transform.position.y), 1);
                    yield return null;
                }
            }
        }

        public static void SetBackground()
        {
            ((GameObject)Instantiate(Resources.Load(Background))).transform.localScale = new Vector3(3, 3);
        }

        public static void InitializeGrid()
        {
            _grid = new GameObject[Rows, Columns];
        }

        public static void DrawGrid()
        {
            for (var i = 0; i <= Rows - 1; i++)
            {
                for (var j = 0; j <= Columns - 1; j++)
                {
                    var cell = Instantiate(Resources.Load(CellOfGrid)) as GameObject;
                    if (cell == null) continue;
                    cell.transform.position = new Vector3(StartX + i * WideCell, StartY - j * WideCell);
                    _grid[i, j] = cell;
                }
            }
        }
    }
}
