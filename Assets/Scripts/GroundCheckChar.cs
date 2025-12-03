using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckChar : MonoBehaviour
{
    MovementLogic logicmovement;

    private void Start()
    {
        logicmovement = this.GetComponentInParent<MovementLogic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Touch The Ground");
        logicmovement.groundedchanger();

    }
    
}
