using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Drop", 0.5f);
            Destroy(gameObject, 3.5f);
        }
    }

    private void Drop()
    {
        rb.isKinematic = false;
    }
}