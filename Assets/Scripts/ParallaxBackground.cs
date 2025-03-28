using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public enum ParallaxType
    {
        None,
        Foreground,
        ForegroundDetails,
        Background,
        SkyDetails,
        Sky,
    }
    [SerializeField]  float SCROLL_WIDTH = 45f;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private ParallaxType _spriteType;
    private float savedSpeed = 0;

    public ParallaxType SpriteType { get => _spriteType; set => _spriteType = value; }

    public void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.x -= _scrollSpeed * Time.deltaTime;
        if(transform.position.x < -SCROLL_WIDTH)
            gameObject.transform.position = new Vector3(SCROLL_WIDTH, transform.position.y, transform.position.z);
        else
            transform.position = position;
    }
    public static void PauseAllParallax()
    {
        foreach (ParallaxBackground parallaxBackground in GameObject.FindObjectsOfType<ParallaxBackground>())
        {
            parallaxBackground.PauseParallax();
        }
    }
    public void PauseParallax()
    {
        savedSpeed = _scrollSpeed;
        _scrollSpeed = 0;
    }
    public static void ResumeAllParallax()
    {
        foreach (ParallaxBackground parallaxBackground in GameObject.FindObjectsOfType<ParallaxBackground>())
        {
            parallaxBackground.ResumeParallax();
        }
    }
    public void ResumeParallax()
    {
        _scrollSpeed = savedSpeed;
    }
}
