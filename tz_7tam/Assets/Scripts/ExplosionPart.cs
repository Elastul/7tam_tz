using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPart : MonoBehaviour
{
    public void DestroyExplosionPart()
    {
        Destroy(transform.gameObject);
    }
}
