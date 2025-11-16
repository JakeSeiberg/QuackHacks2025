using UnityEngine;

public class bossMovement : MonoBehaviour
{
    [Header("Figure-8 Settings")]
    public float speed = 1.0f;        // controls how fast it moves along the curve
    public float amplitudeX = 2.0f;   // horizontal size of the figure-8
    public float amplitudeY = 1.0f;   // vertical size of the figure-8
    public float timeOffset = 0f;     // useful to desync multiple bosses

    private Vector3 centerPosition;

    void Start()
    {
        // store starting position as the center of the pattern
        centerPosition = transform.position;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Use Time.time (not Time.deltaTime) so the curve is parameterized by absolute time
        float t = (Time.time + timeOffset) * speed;

        // Parametric figure-8 (Lissajous-like)
        float x = Mathf.Sin(t) * amplitudeX;
        float y = Mathf.Sin(t * 2f) * amplitudeY; // 2x frequency gives figure-8 shape

        // Set position directly (don't accumulate)
        transform.position = centerPosition + new Vector3(x, y, 0f);
    }
}