using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shovel : MonoBehaviour, ICollision
{
    [SerializeField] private int _collisionTime;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        //move the shovel
        rb.AddForce(Vector3.up * 8, ForceMode.Impulse);

        var block = other.gameObject.GetComponentInParent<ICollision>();
        if (block != null)
        {
            DecreaseCollisionTime();
            block.DecreaseCollisionTime();
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, 65 * Time.deltaTime);
    }

    public void DecreaseCollisionTime()
    {
        _collisionTime--;
        DestroyByCollisionTime();
    }

    public void DestroyByCollisionTime()
    {
        if (_collisionTime < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
