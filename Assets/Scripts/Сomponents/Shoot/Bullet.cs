using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Bullet
{
    public Transform transform;
    public Rigidbody2D rigidBody;
    public Vector3 movement;
    public float speed;
    public bool destroy;
}
