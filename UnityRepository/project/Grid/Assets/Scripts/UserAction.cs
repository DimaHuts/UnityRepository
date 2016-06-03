using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// класс который управляет действимя пользователя
    /// </summary>
    public class UserAction : MonoBehaviour
    {
        //переменные которые будут меняться после действий пользователя
	    private static GameObject _choicedToMove;
        private static GameObject _moveToBack;

        public static GameObject ChoicedToMove 
        {
            get { return _choicedToMove; }
            set { _choicedToMove = value; }
        }

        public static GameObject MoveToBack
        {
            get { return _moveToBack; }
            set { _moveToBack = value; } 
        }

        /// <summary>
        /// метод который обнуляет предыдущую пару шаров
        /// </summary>
        private static void SetNullGameObject()
        {
            ChoicedToMove = null;
            MoveToBack = null;
        }

        /// <summary>
        /// метод который меняет местами шары
        /// </summary>
        public static void ReplaceBalls()
        {
            if (MoveToBack == null || ChoicedToMove == null) return;

            var parentChoicedToMove = ChoicedToMove.transform.parent;
            var parentMoveToBack = MoveToBack.transform.parent;

            ChoicedToMove.transform.SetParent(parentMoveToBack, true);
            ChoicedToMove.transform.DOMove(new Vector3(parentMoveToBack.position.x, parentMoveToBack.position.y), 0.5f);

            MoveToBack.transform.SetParent(parentChoicedToMove, true);
            MoveToBack.transform.DOMove(new Vector3(parentChoicedToMove.position.x, parentChoicedToMove.position.y), 0.5f);

            SetNullGameObject();
        }
    }

}
