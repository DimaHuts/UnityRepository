using UnityEngine;
using System.Collections;
using System.Text;
using DG.Tweening;

namespace Assets.Scripts
{
    ///<summary>
    /// Класс Grid предназначен для начальной инициализации игры
    ///</summary>
    public class Grid : MonoBehaviour
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

        /// <summary>
        /// метод возвращает сетку в которой будут равполагаться шары
        /// </summary>
        public static GameObject[,] GetGrid
        {
            get { return _grid; }
        }

        /// <summary>
        /// метод возвращает количество строк в сетке
        /// </summary>
        public static int GetRows
        {
            get { return Rows; }
        }

        /// <summary>
        /// метод возвращает количество столбцов в сетке
        /// </summary>
        public static int GetColumns
        {
            get { return Columns; }
        }

        /// <summary>
        /// метод генерирует случайным образом имя для будущего шара 
        /// </summary>
        public static string InternalElement
        {
            get
            {
                return InternalElements[Random.Range(0, 5)];
            }
        }

        /// <summary>
        /// метод генерирует шар и перемещает его в конкретную ячейку сетки
        /// </summary>
        /// <param name="grid">параметр который хранит ссылку на сетку</param>
        /// <param name="i">индекс столбца сетки</param>
        /// <param name="j">индекс строки сетки</param>
        public static void GeneratedArbitraryBall(GameObject[,] grid, int i, int j)
        {
            var nameInternalElement = InternalElement;
            var internalElement = Instantiate(Resources.Load(nameInternalElement)) as GameObject;
            if (internalElement != null)
            {
                internalElement.name = nameInternalElement;
                internalElement.transform.parent = grid[i, j].transform;
                internalElement.transform.DOMove(new Vector3(grid[i, j].transform.position.x, 
                                                             grid[i, j].transform.position.y), 1);
            }
        }

        /// <summary>
        /// метод непосредственно заполняющий всю сетку
        /// </summary>
        /// <returns></returns>
        public static IEnumerator Complete()
        {
            for (var j = Columns - 1; j >= 0; j--)
            {
                for (var i = Rows - 1; i >= 0; i--)
                {
                    GeneratedArbitraryBall(GetGrid, i, j);
                    yield return null;
                }
            }
        }

        /// <summary>
        /// метод устанавливает фон для сцены
        /// </summary>
        public static void SetBackground()
        {
            ((GameObject)Instantiate(Resources.Load(Background))).transform.localScale = new Vector3(3, 3);
        }

        /// <summary>
        /// метод инициализирует сетук
        /// </summary>
        /// <param name="rows">количество столбцов сетки</param>
        /// <param name="columns">количество строк сетки</param>
        public static void SetSizeGrid(int rows, int columns)
        {
            _grid = new GameObject[rows, columns];
        }

        /// <summary>
        /// метод устанавливает имч для каждой созданной ячейке в сетке
        /// </summary>
        /// <param name="i">координа ячейки - номер столбца</param>
        /// <param name="j">координата ячейки - номер строки</param>
        /// <returns></returns>
        private static string SetNameCell(int i, int j)
        {
            var lengthCellOfGrid = CellOfGrid.Length;
            return new StringBuilder(CellOfGrid).Insert(lengthCellOfGrid, "_").Insert(lengthCellOfGrid+1, i)
                                                                              .Insert(lengthCellOfGrid+2, j).ToString();
        }

        /// <summary>
        /// метод рисует сетку
        /// </summary>
        public static void DrawGrid()
        {
            for (var i = 0; i <= Rows - 1; i++)
            {
                for (var j = 0; j <= Columns - 1; j++)
                {
                    var cell = Instantiate(Resources.Load(CellOfGrid)) as GameObject;
                    if (cell == null) return;
                    cell.name = SetNameCell(i, j);
                    cell.transform.position = new Vector3(StartX + i * WideCell, StartY - j * WideCell);
                    _grid[i, j] = cell;
                }
            }
        }

        /// <summary>
        /// метод проверяет есть ли пустые ячейки в сетке
        /// </summary>
        /// <returns>если есть возвращает true, если нету то - false</returns>
        public static bool IsEmpty()
        {
            for (var i = 0; i <= Rows - 1; i++)
            {
                for (var j = 0; j <= Columns - 1; j++)
                {
                    if (_grid[i, j].transform.childCount == 0 || _grid[i, j].transform.GetChild(0).position != _grid[i, j].transform.position) return true;
                }
            }
            return false;
        }

    }
}
