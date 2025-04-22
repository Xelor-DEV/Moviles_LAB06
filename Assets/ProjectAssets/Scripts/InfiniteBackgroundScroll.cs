using UnityEngine;

public class InfiniteBackgroundScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    private float spriteWidth;
    private float leftCamEdge;
    private float rightCamEdge;

    void Start()
    {
        Camera cam = Camera.main;
        leftCamEdge = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        rightCamEdge = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        float currentRightEdge = transform.position.x + (spriteWidth / 2);

        if (currentRightEdge < leftCamEdge)
        {
            float newX = rightCamEdge + (spriteWidth / 2);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}