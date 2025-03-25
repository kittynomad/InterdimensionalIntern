using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]  float SCROLL_WIDTH = 45f;
    [SerializeField] private float _scrollSpeed;
    public void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.x -= _scrollSpeed * Time.deltaTime;
        if(transform.position.x < -SCROLL_WIDTH)
        {
            gameObject.transform.position = new Vector3(SCROLL_WIDTH, transform.position.y, transform.position.z);
            Debug.Log("Re-enter");
        }
        else
            transform.position = position;
    }
}
