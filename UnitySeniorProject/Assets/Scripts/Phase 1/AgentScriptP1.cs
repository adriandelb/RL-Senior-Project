﻿using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Collections;
using Unity.MLAgents.Actuators;

public class AgentScriptP1 : Agent
{
    //Rigidbodies to control physics
    Rigidbody agentBodyRB;
    Rigidbody targetRB;

    //game objects to find physics objects in environment
    public GameObject target;
    public GameObject ground;
    public GameObject agent;

    [HideInInspector]
    public Bounds areaBounds;
    PushBlockSettings m_PushBlockSettings;

    //Academy
    EnvironmentParameters defaultParams;

    public void HitObject()
    {
        // We use a reward of 5.
        AddReward(5f);
        // By marking an agent as done AgentReset() will be called automatically.
        EndEpisode();
        Debug.Log("Agent touched block");
    }


    public Vector3 GetRandomSpawnPos()
    {
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = Random.Range(-areaBounds.extents.x * Random.Range(0.1f, 0.9f),
                areaBounds.extents.x * Random.Range(0.1f, 0.9f));

            var randomPosZ = Random.Range(-areaBounds.extents.z * Random.Range(0.1f, 0.9f),
                areaBounds.extents.z * Random.Range(0.1f, 0.9f));
            randomSpawnPos = ground.transform.localPosition + new Vector3(randomPosX, 1.35f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }


    public override void Initialize()
    {
        agentBodyRB = GetComponent<Rigidbody>();
        targetRB = target.GetComponent<Rigidbody>();
        areaBounds = ground.GetComponent<Collider>().bounds;

        defaultParams = Academy.Instance.EnvironmentParameters;
        Debug.Log("Initial");
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            HitObject();
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        AddReward(-1f / MaxStep);
        Debug.Log("Action Received");
    }

    void FixedUpdate()
    {
        if (transform.localPosition.y < 1)
        {
            Debug.Log("Fell");
            AddReward(-1f);
            EndEpisode();
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                dirToGo = transform.right * -0.75f;
                break;
            case 6:
                dirToGo = transform.right * 0.75f;
                break;
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        agentBodyRB.AddForce(dirToGo * 2f,
            ForceMode.VelocityChange);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }

    //What should happens when an episode begins
    public override void OnEpisodeBegin()
    {
        Debug.Log("Entered Episode");
        //reset the agent
        //velocity of agent
        agentBodyRB.velocity = Vector3.zero;
        agentBodyRB.angularVelocity = Vector3.zero;


        //start position
        agent.transform.localPosition = GetRandomSpawnPos();

        //Move target to new position
        target.transform.localPosition = GetRandomSpawnPos();

    }
}