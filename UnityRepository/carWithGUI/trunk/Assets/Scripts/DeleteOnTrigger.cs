using UnityEngine;

namespace Assets.Scripts
{
    public class DeleteOnTrigger : MonoBehaviour
    {
	    private void OnTriggerExit()
        {
            Destroy(gameObject);
            Game.GeneratedBonus();
        }
    }
}
