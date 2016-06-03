using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// класс описывает процесс игры
    /// </summary>
    public class Game : MonoBehaviour
    {
        /// <summary>
        /// метод который находит 3 и больше подряд стоящих шаров в столбцах и строках 
        /// </summary>
        /// <param name="grid">переменная которая хранит ссылку на сетку</param>
        /// <param name="rows">количество строк в сетке</param>
        /// <param name="columns">количество столбцов сетке</param>
        /// <param name="inverse">параметр который определяет цикл проводить по строкам или по столбцам</param>
        private static void FindSameBalls(GameObject[,] grid, int rows, int columns, bool inverse = true)
        {
            for (var i = 0; i <= rows - 1; i++)
            {
                for (var j = 0; j <= columns - 1; j++)
                {
                    var count = 1;
                    for (var k = j+1; k <= columns-1; k++)
                    {
                        
                        if (( inverse ?  grid[i, j].transform.GetChild(0).gameObject.name == grid[i, k].transform.GetChild(0).gameObject.name
                                      :  grid[j, i].transform.GetChild(0).gameObject.name == grid[k, i].transform.GetChild(0).gameObject.name
                           ))
                        {
                            count ++;
                            if (k != rows-1) continue;
                            DestroySameBall(count, i, j, inverse, grid);
                            j += count-1;
                        }
                        else
                        {
                            DestroySameBall(count, i, j, inverse, grid);
                            if (j + count >= columns - 1)
                            {
                                DestroySameBall(count, i, j, inverse, grid);
                                j = columns + 10;
                            }
                            k = columns + 10;
                            j += count-1;
                        }
                    } 
                }
            }
        }

        /// <summary>
        /// метод который непосредственно удаляет шары
        /// </summary>
        /// <param name="count">количество шаров которое надо удалить</param>
        /// <param name="i">столбец в котором находятся шары на удаление</param>
        /// <param name="j">строки в которых находятся шары на удаление</param>
        /// <param name="inverse">параметр который определяет цикл проводить по строкам или по столбцам</param>
        /// <param name="grid">переменная которая хранит ссылку на сетку</param>
        public static void DestroySameBall(int count, int i, int j, bool inverse, GameObject[,] grid)
        {
            if (count < 3) return;
            if (inverse)
            {
                for (var n = j; n <= j + count - 1; n++)
                {
                    Destroy(grid[i, n].transform.GetChild(0).gameObject, 0);
                }
            }
            else
            {
                for (var n = j; n <= j + count - 1; n++)
                {
                    Destroy(grid[n, i].transform.GetChild(0).gameObject, 0);
                }
            }
        }

        /// <summary>
        /// метод который обновляет сетку после уничтожения одинаковых шаров
        /// </summary>
        /// <param name="grid">переменная которая хранит ссылку на сетку</param>
        /// <param name="rows">количество строк в сетке</param>
        /// <param name="columns">количество столбцов сетке</param>
        public static void UpdateGrid(GameObject[,] grid, int rows, int columns)
        {
            for (var i = 0; i <= columns - 1; i++)
            {
                for (var j = rows - 1; j >= 1; j--)
                {
                    //собираем информацию по каждому столбцу об элементах которые надо передвинуть
                    var listNotNull = new ArrayList();
                    if (grid[i, j].transform.childCount != 0) continue;
                    for (var k = j - 1; k >= 0; k--)
                    {
                        if (grid[i, k].transform.childCount != 0)
                        {
                            listNotNull.Add(k);
                        }
                    }

                    //если такие элементы существуют то они перемещаются
                    if (listNotNull.Count == 0) continue;
                    var count = 0;
                    foreach (var index in listNotNull)
                    {
                        var moveElement = grid[i, (int) index].transform.GetChild(0).gameObject;
                        moveElement.transform.SetParent(grid[i, j - count].transform, true);
                        moveElement.transform
                            .DOMove( new Vector3(grid[i, j - count].transform.position.x,
                                                 grid[i, j - count].transform.position.y), 0.7f);
                        count++;
                    }
                }
            }

            //после это определяем остались ли в верху столбца пустые ячейки если да то генерируются элементы
            for (var x = 0; x <= columns - 1; x++)
            {
                for (var y = 0; y <= rows - 1; y++)
                {
                    if (grid[x, y].transform.childCount != 0) continue;
                    Grid.GeneratedArbitraryBall(grid, x, y);
                }
            }
        }

        /// <summary>
        /// метод который контролирует процесс игры
        /// </summary>
        public static IEnumerator MonitoringChangeCells()
        {
            if (Grid.IsEmpty()) yield break;
            FindSameBalls(Grid.GetGrid, Grid.GetRows, Grid.GetColumns, false);
            FindSameBalls(Grid.GetGrid, Grid.GetRows, Grid.GetColumns);
            yield return new WaitForSeconds(0.5f);

            UpdateGrid(Grid.GetGrid, Grid.GetRows, Grid.GetColumns);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
