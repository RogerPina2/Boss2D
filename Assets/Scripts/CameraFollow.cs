using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float speed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 endPos = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, endPos, speed);
        transform.position = smoothed;
    }
}
