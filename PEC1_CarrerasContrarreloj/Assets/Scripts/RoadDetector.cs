using EasyRoads3Dv3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class RoadDetector : MonoBehaviour
{
    private CarController car;
    [SerializeField] float velocityReductionMultiplier = 0.5f;

    private void Start()
    {
        car = GetComponentInParent<CarController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Road"))
        {
            car.SetBackToRoadSettings();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            car.SetOutOfRoadSettings(velocityReductionMultiplier);
        }
    }
}
