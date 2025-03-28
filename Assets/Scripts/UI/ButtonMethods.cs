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
    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<Animator>().Play("ENABLE");
    }
    public void Button_Close(GameObject popUp)
    {
        Destroy(popUp);
    }
}
