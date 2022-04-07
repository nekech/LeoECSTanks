using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovementSpeed = 0.05f;
    public float RotationSpeed = 1f;

    [SerializeField]
    GameObject Bullet = null;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;

        if (Input.GetKey("w"))
        {
            newPosition = transform.position + new Vector3(0, MovementSpeed, 0);
        }
        if (Input.GetKey("s"))
        {
            newPosition = transform.position - new Vector3(0, MovementSpeed, 0);
        }
        if (Input.GetKey("d"))
        {
            newPosition = transform.position + new Vector3(MovementSpeed, 0, 0);
        }
        if (Input.GetKey("a"))
        {
            newPosition = transform.position - new Vector3(MovementSpeed, 0, 0);
        }

        transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * 1f);

        Vector3 newCameraPosition = newPosition;
        newCameraPosition.z = Camera.main.transform.position.z;

        Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, newCameraPosition, Time.deltaTime * 1f);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //TODO optimise calculation of rotation angle

        Vector3 vectorToTarget = mousePosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * RotationSpeed);

        if (Input.GetMouseButtonDown(0))
        {
            Shut();
        }
    }
    void Shut()
    {
        GameObject newBullet = Instantiate(Bullet);
        //newBullet.transform.position = transform.position + new Vector3(0, 0.75f, 0);
        newBullet.transform.position = transform.localToWorldMatrix * new Vector4(1, 0, 0, 1f);
        newBullet.GetComponent<BulletBehaviour>().MovementOffset = transform.right;
    }
}
