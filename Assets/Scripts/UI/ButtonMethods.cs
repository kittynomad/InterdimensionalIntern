/*
 * ButtonMethods.cs
 * Marlow Greenan
 * 2/22/2025
 * 
 * A random assortment of buttons.
 */
using UnityEngine;

public class ButtonMethods : MonoBehaviour
{
    public void Button_Close(GameObject popUp)
    {
        Destroy(popUp);
    }
}
