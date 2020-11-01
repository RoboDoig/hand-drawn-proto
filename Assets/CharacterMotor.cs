using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMotor : MonoBehaviour
{

    NavMeshAgent navMeshAgent;
    Animator animator;
    Vector3 lastPosition;
    float currentSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate speed
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        // update animator
        animator.SetFloat("moveSpeed", currentSpeed);

        Debug.Log(currentSpeed);
    }


    public void SetDestination (Vector3 position) {
        navMeshAgent.SetDestination(position);
    }
}
