using UnityEngine;

public class Villager : MonoBehaviour
{
    public bool facingRight;
    public float speed;
    public float scale;

    // Start is called before the first frame update
    private void Start()
    {
        facingRight = true;
        scale = 4;
    }

    // Update is called once per frame
    private void Update()
    {
        if (facingRight)
        {
            transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
            transform.localScale = new Vector2(scale, scale);
        }
        else {
            transform.Translate(-1 * speed * Time.deltaTime, 0.0f, 0.0f);
            transform.localScale = new Vector2(-scale, scale);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Edge") facingRight = !facingRight;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}