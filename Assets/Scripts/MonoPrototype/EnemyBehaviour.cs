using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    [SerializeField]
    GameObject Bullet = null;

    [SerializeField]
    float RotationSpeed = 1f;

    [SerializeField]
    int ShootInterval = 10;

    float prevShootTime = 0;

    Vector3 prevRotation = new Vector3();
    void Start()
    {
        Debug.Log("Hello From enemy");

        prevRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        var dangeresBullet = DetectBullets();

        Vector3 diffRotation;
        if (dangeresBullet != null && false)
        {
            Debug.Log("Rotate to bullet");
            diffRotation = RotateTo(dangeresBullet.transform);
        }
        else
        {
            diffRotation = RotateTo(player.transform);
        }

        if (Mathf.Abs(diffRotation.x) < 10f &&  Mathf.Abs(diffRotation.y) < 10f && Mathf.Abs(diffRotation.z) < 10f)
        {
            Shut();
        }

        if (dangeresBullet != null)
        {
            Vector3 moveDirection = MoveFromBulletTrace(dangeresBullet);
            Debug.Log("New pos: " + moveDirection);

            transform.position = Vector3.Slerp(transform.position, moveDirection, Time.deltaTime * 1f);
        }

        prevRotation = transform.rotation.eulerAngles;
    }

    Vector3 RotateTo(Transform target)
    {
        Vector3 vectorToTarget = target.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 diffRotation = transform.rotation.eulerAngles - q.eulerAngles;

        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * RotationSpeed);
        
        return diffRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy collide");

        if (other.tag == "bullet")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }    
    }

    void Shut()
    {
        if (Time.unscaledTime - prevShootTime > ShootInterval)
        {
            GameObject newBullet = Instantiate(Bullet);
            //newBullet.transform.position = transform.position + new Vector3(0, 0.75f, 0);
            newBullet.transform.position = transform.localToWorldMatrix * new Vector4(1, 0, 0, 1f);
            newBullet.GetComponent<BulletBehaviour>().MovementOffset = transform.right;

            prevShootTime = Time.unscaledTime;
        }
    }

    Vector3 MoveFromBulletTrace(GameObject bullet)
    {
        float distance = (transform.position - bullet.transform.position).magnitude;

        float tna = 1f / distance;
        float tnb = (transform.position.y - bullet.transform.position.y)/(transform.position.x - bullet.transform.position.x);

        float l = Mathf.Sqrt(1 + distance * distance);

        float a = Mathf.Atan(tna);
        float b = Mathf.Atan(tnb);

        float x3 = l*Mathf.Cos(a + b);
        float y3 = l*Mathf.Sin(a + b);

        return new Vector3(bullet.transform.position.x + x3, bullet.transform.position.y + y3, 0);
    }

    GameObject DetectBullets()
    {
        var bullets = GameObject.FindGameObjectsWithTag("bullet");

        List<GameObject> dangerousBullets = new List<GameObject>();

        float minDistance = 100;
        GameObject dangerestBullet = null;

        foreach(var bullet in bullets)
        {
            Vector2 orig = new Vector2(bullet.transform.position.x, bullet.transform.position.y);
            Vector2 direction = new Vector2(bullet.GetComponent<BulletBehaviour>().MovementOffset.x, bullet.GetComponent<BulletBehaviour>().MovementOffset.y);
            
            Vector2 thisPos = new Vector2(transform.position.x,  transform.position.y);

            orig = orig + direction;
            
            RaycastHit2D hit = Physics2D.Raycast(orig, direction);

            if (hit.collider != null)
            {
                if (hit.transform.gameObject == gameObject)
                {
                    dangerousBullets.Add(bullet);

                    if (hit.distance < minDistance)
                    {
                        minDistance = hit.distance;

                        dangerestBullet = bullet;
                    }
                }
            }
        }

        return dangerestBullet;
    }
}
