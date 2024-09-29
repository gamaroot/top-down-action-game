using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chain_Movement : MonoBehaviour
{
    public Transform Chain1; 
    public Transform Chain2; 
    public float speed = 2.0f; 
    private Vector3 ChainTarget;
    // Start is called before the first frame update
    void Start()
    {
        ChainTarget = Chain2.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, ChainTarget, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, ChainTarget) < 0.1f)
        {
            ChainTarget = ChainTarget == Chain1.position ? Chain2.position : Chain1.position;
        }
    }
}
