using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    Vector3 minimum;
    [SerializeField]
    Vector3 maximum;
    [SerializeField]
    GameObject TargetPrefab;
    [SerializeField]
    GameObject ParentObject;

    private Vector3 RandomPosition;

    public void Respawn() {
        float x = Random.Range(minimum.x, maximum.x);
        float y = Random.Range(minimum.y, maximum.y);
        float z = Random.Range(minimum.z, maximum.z);

        Vector3 RandomPosition = new Vector3(x, y, z);
        //Debug.Log(RandomPosition);

        GameObject target = Instantiate(TargetPrefab, RandomPosition, Quaternion.identity);

        target.transform.SetParent(ParentObject.transform, true);
    }

}
