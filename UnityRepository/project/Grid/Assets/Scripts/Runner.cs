using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// класс который запускает игру
    /// </summary>
    class Runner : MonoBehaviour
    {
        void Start ()
	    {
            Grid.SetBackground();
            Grid.SetSizeGrid(Grid.GetRows, Grid.GetColumns);
            Grid.DrawGrid();
            StartCoroutine(Grid.Complete());
        }
	
	    void Update ()
	    {
	        StartCoroutine( Game.MonitoringChangeCells() );
	    }
    }
}
