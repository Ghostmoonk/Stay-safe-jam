using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarmanBehaviour : MonoBehaviour
{
    [SerializeField] float length;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Cos(Time.time) * length, 0f); 
    }
}
