using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollision
{
    public void TakeDamage(int damage);
    public void DestroyByHeal();
}
