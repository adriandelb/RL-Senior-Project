using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Collections;
using Unity.MLAgents.Actuators;

public class AgentScript : Agent
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
            var randomPosX = Random.Range(-areaBounds.extents.x * Random.Range(0.1f,0.9f),
                areaBounds.extents.x * Random.Range(0.1f, 0.9f));

            var randomPosZ = Random.Range(-areaBounds.extents.z * Random.Range(0.1f, 0.9f),
                areaBounds.extents.z * Random.Range(0.1f, 0.9f));
            randomSpawnPos = ground.transform.position + new Vector3(randomPosX , 1.35f, randomPosZ);
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
    
    //Observations needed by agent:
    //Position of the object
    //Position of the agent

    public override void OnActionReceived(float[] vectorAction)
    {
        MoveAgent(vectorAction);
        AddReward(-1f / MaxStep);
        Debug.Log("Action Received");

        if(transform.position.y < 0)
        {
            Debug.Log("Fell");
            AddReward(-1f);
            EndEpisode();
        }
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = Mathf.FloorToInt(act[0]);

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

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2;
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
        agent.transform.eulerAngles = new Vector3(
            agent.transform.eulerAngles.x,
            agent.transform.eulerAngles.y,
            agent.transform.eulerAngles.z
        );

        //start position
        agent.transform.position = new Vector3(-7, 0.625f, 0);

        //Move target to new position
        target.transform.position = new Vector3(6.43f, 1.35f, 0);
    }






}
