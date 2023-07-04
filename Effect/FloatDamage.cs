using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatDamage : MonoBehaviour
{
    public float destroyTime;
    public static TextMesh textMesh;

    void Start()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMesh>();
        Destroy(gameObject, destroyTime);
    }
}
