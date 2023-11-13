using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CarController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SimulationManager))]
public class AutoParkAgent : Agent
{
    private Rigidbody _rigitBody;
    private CarController _controller;
    private SimulationManager _simulationManager;
    private ActionSegment<float>? _lastActions;
    private ParkingLot _nearestLot;

    public override void Initialize()
    {
        _rigitBody = GetComponent<Rigidbody>();
        _controller = GetComponent<CarController>();
        _simulationManager = GetComponent<SimulationManager>();
        _simulationManager.InitializeSimulation();
    }

    public override void OnEpisodeBegin()
    {
        _simulationManager.ResetSimulation();
        _simulationManager.InitializeSimulation();
        _nearestLot = null;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        _lastActions = actions.ContinuousActions;
        _controller.CurrentSteeringAngle = actions.ContinuousActions[0];
        _controller.CurrentAcceleration = actions.ContinuousActions[1];
        _controller.CurrentBrakeTorque = actions.ContinuousActions[2];
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    { 
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
        actions[2] = Input.GetAxis("Jump");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("barrier") || other.gameObject.CompareTag("car") ||
            other.gameObject.CompareTag("tree"))
        {
            AddReward(-0.01f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (_lastActions!= null && _simulationManager.InitComplete)
        {
            if(_nearestLot == null)
                _nearestLot = _simulationManager.GetRandomEmptyParkingSlot();
            Vector3 dirToTarget = (_nearestLot.transform.position - transform.position).normalized;
            sensor.AddObservation(transform.position.normalized);
            sensor.AddObservation(
                this.transform.InverseTransformPoint(_nearestLot.transform.position));
            sensor.AddObservation(
                this.transform.InverseTransformVector(_rigitBody.velocity.normalized));
            sensor.AddObservation(
                this.transform.InverseTransformDirection(dirToTarget));
            sensor.AddObservation(transform.forward);
            sensor.AddObservation(transform.right);
            // sensor.AddObservation(StepCount / MaxStep);
            float velocityAlignment = Vector3.Dot(dirToTarget, _rigitBody.velocity);
            AddReward(0.001f * velocityAlignment);
        }
        else
        {
            sensor.AddObservation(new float[18]);
        }
    }

    public IEnumerator JackpotReward(float bonus)
    {
        if(bonus > 0.2f)
            Debug.LogWarning("Jackpot hit! " + bonus);
        AddReward(0.2f + bonus);
        yield return new WaitForEndOfFrame();
        EndEpisode();
    }
}
