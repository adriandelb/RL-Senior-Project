using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Security.Cryptography;
using System.Collections.Specialized;
using Unity.MLAgents.Actuators;

public class AgentScript : Agent
{
    //Rigidbodies to control physics
    Rigidbody agentBodyRB;
    Rigidbody targetRB;

    //game objects to find physics objects in environment
    public GameObject target;
    public GameObject platform;
    public GameObject agent;

    //Variable to get the area of the platform object
    public Bounds areaBounds;

    //Academy
    EnvironmentParameters defaultParams;

    //WHta should happens when an episode begins
    public override void OnEpisodeBegin()
    {
        //reset the agent
        //velocity of agent
        agentBodyRB.velocity = Vector3.zero;
        agentBodyRB.angularVelocity = Vector3.zero;

        //start position
        agent.transform.position = new Vector3(-7,0.625f,0);

        //Move target to new position
        target.transform.position = GetRandomSpawnPos();
    }

    public Vector3 GetRandomSpawnPos()
    {
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = Random.Range(-areaBounds.extents.x * Random.Range(0.1f,0.9f),
                areaBounds.extents.x * Random.Range(0.1f, 0.9f));

            var randomPosZ = Random.Range(-areaBounds.extents.z * Random.Range(0.1f, 0.9f),
                areaBounds.extents.z * Random.Range(0.1f, 0.9f));
            randomSpawnPos = platform.transform.position + new Vector3(randomPosX , 1.35f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }




   

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        AddReward(-1f / MaxStep);


        if (agent.transform.position.y < 0)
        {
            EndEpisode();
        }
        Debug.Log("Action Rece");
    }

    void OnCollisionEnter(Collision collision)
    {
        if(gameObject.CompareTag("Target"))
        {
            AddReward(5f);
            Debug.Log("Action coll");
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
        agentBodyRB.AddForce(dirToGo * 10f,
            ForceMode.VelocityChange);

    }

    public override void Initialize()
    {
        agentBodyRB = agent.GetComponent<Rigidbody>();
        targetRB = target.GetComponent<Rigidbody>();
        areaBounds = platform.GetComponent<Collider>().bounds;

        defaultParams = Academy.Instance.EnvironmentParameters;
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

        Debug.Log("Heuristic");
    }

}
