using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 MovementOffset = new Vector3(0, 0, 0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        var rb = GetComponent<Rigidbody2D>();

        Vector2 offset2d = new Vector2(MovementOffset.x, MovementOffset.y);

        Vector2 newPosition = rb.position + offset2d * Time.deltaTime;

        
        if (newPosition.x > 10 || newPosition.x < -10 || newPosition.y > 10 || newPosition.y < -10)
        {
            Destroy(gameObject);
        }

        GetComponent<Rigidbody2D>().MovePosition(newPosition);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "bullet")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }    
    }


}
