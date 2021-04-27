using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public Transform prince;
    public float range;
    public bool flee, facingLeft;
    public float speed;
    public float scale;
    public int health = 3;

    // Start is called before the first frame update
    private void Start()
    {
        facingLeft = true;
        flee = false;
        range = 2f;
        scale = 1;
        speed = 2f;
        prince = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(prince.position, transform.position) < range)
        {
            speed = 4f;
            Debug.Log("Fleeing!");
        }

        if (facingLeft)
        {
            transform.Translate(-1* speed * Time.deltaTime, 0.0f, 0.0f);
            transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
            transform.localScale = new Vector2(-scale, scale);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Edge")
        {
            facingLeft = !facingLeft;
            Debug.Log("Wizard turning");
        }
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0) Die();
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}