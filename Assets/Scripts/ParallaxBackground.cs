using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]  float SCROLL_WIDTH = 45f;
    [SerializeField] private float _scrollSpeed;
    public void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x -= _scrollSpeed * Time.deltaTime;
        if(transform.position.x < -SCROLL_WIDTH)
            Offscreen(ref pos);
        transform.position = pos;
    }
    public virtual void Offscreen(ref Vector3 pos)
    {
        pos.x += (2*SCROLL_WIDTH);

    }
}
