using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClickBall : MonoBehaviour
    {
	    
        /// <summary>
        /// метод вызывается каждый раз когда пользователь кликает по шару на сетке
        /// метод должен установить пару шаров которые будут меняться местами
        /// переменные для этих шаров описаны в классе UserAction 
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnMouseDown()
        {
            if (UserAction.ChoicedToMove == null)
            {
                UserAction.ChoicedToMove = gameObject;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                if (Math.Abs(UserAction.ChoicedToMove.transform.position.x).Equals(Math.Abs(gameObject.transform.position.x))) 
                {
                    if (Math.Abs(UserAction.ChoicedToMove.transform.position.y).Equals(Math.Abs(gameObject.transform.position.y))) 
                    {
                        yield break;
                    }
                }
                UserAction.MoveToBack = gameObject;
                yield return new WaitForSeconds(0.5f);
            }
            UserAction.ReplaceBalls();
        }
    }
}
