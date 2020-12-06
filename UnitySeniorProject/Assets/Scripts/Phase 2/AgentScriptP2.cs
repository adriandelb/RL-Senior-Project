using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Collections;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class AgentScriptP2 : Agent
{
    //Rigidbodies to control physics
    Rigidbody agentBodyRB;
    Rigidbody targetRB;

    //game objects to find physics objects in environment
    public GameObject target;
    public GameObject ground;
    public GameObject agent;

    public List <GameObject> Walls;
    private int sizeOfList;
    private GameObject currentWall;
 

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

    private Bounds objbounds;
    public Vector3 GetRandomSpawnPos(GameObject obj)
    {
        objbounds = obj.GetComponent<Collider>().bounds;
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = Random.Range(-areaBounds.extents.x * Random.Range(0.1f, 0.9f),
                areaBounds.extents.x * Random.Range(0.1f, 0.9f));

            var randomPosZ = Random.Range(-areaBounds.extents.z * Random.Range(0.1f, 0.9f),
                areaBounds.extents.z * Random.Range(0.1f, 0.9f));
            randomSpawnPos = ground.transform.localPosition + new Vector3(randomPosX, 1.35f, randomPosZ);

            if (Physics.CheckBox(randomSpawnPos,new Vector3(2.5f,0.1f,2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }
    
    public Vector3 randomWallLane(GameObject Obj)
    {
        var spawn = Vector3.zero;
        var z = Random.Range(-7, 4);
        var y = Obj.transform.localPosition.y;
        var x = Obj.transform.localPosition.x;
        spawn = new Vector3(x, y, z);
        return spawn; 
    }
    public void SpawnWalls()
    {
       
        sizeOfList = Walls.Count;
        for (int i = 0; i < sizeOfList; i++)
        {
            currentWall = Walls[i];
            currentWall.transform.localPosition = randomWallLane(currentWall);
        }  
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
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.transform.localPosition);
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        AddReward(-1f / MaxStep);
        Debug.Log("Action Received");
    }

    void OnCollisionStay(Collision collision)
    {
        //Reduce reward when it starys touching a wall
        if (collision.gameObject.CompareTag( "Wall"))
        {
            AddReward(-0.01f);
            Debug.Log("Hit wall");
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            HitObject();
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y < 0)
        {
            Debug.Log("Fell");
            AddReward(-1f);
            EndEpisode();
        }

        if (target.transform.position.y > 4 || transform.position.y > 3)
        {
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
        //velocity of agents
        agentBodyRB.velocity = Vector3.zero;
        agentBodyRB.angularVelocity = Vector3.zero;


        Debug.Log("Ground Area" + ground.transform.localPosition);
        SpawnWalls();

        //start position
        agent.transform.localPosition = GetRandomSpawnPos(agent);
        //agent.transform.localPosition = new Vector3(-5.85f, 1.072695f, -1.39f);
        //Move target to new position
        target.transform.localPosition = GetRandomSpawnPos(target);

        //target.transform.localPosition = new Vector3(10.64f, 1.797695f, -3.54f);
    }
}
