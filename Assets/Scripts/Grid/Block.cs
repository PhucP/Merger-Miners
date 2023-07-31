using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, ICollision
{
    [SerializeField] private int _collisionTime;

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
