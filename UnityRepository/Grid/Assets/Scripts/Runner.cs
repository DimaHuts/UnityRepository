using UnityEngine;

public class Runner : MonoBehaviour
{
    // Use this for initialization
	void Start ()
	{
        Assets.Scripts.Grid.SetBackground();
        Assets.Scripts.Grid.InitializeGrid();
        Assets.Scripts.Grid.DrawGrid();
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Assets.Scripts.Grid.Complete());
        }
    }
}
