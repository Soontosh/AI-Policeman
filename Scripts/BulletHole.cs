using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 1);
    }
}
