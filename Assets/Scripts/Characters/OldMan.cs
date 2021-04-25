using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMan : MonoBehaviour
{
    public int dir;
    public float speed;
    public GameObject oldMan;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        oldMan.transform.Translate(dir * speed * Time.deltaTime, 0.0f, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            dir = dir * -1;
            Flip();
        }
    }

    private void Flip()
    {
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Die()
    {
        Destroy(oldMan.gameObject);
    }
}
