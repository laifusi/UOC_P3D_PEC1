using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class RoadDetector : MonoBehaviour
{
    private CarController car;                                  // CarController
    [SerializeField] float velocityReductionMultiplier = 0.5f;  // Reduction that will be applied to the car's Torque

    private void Start()
    {
        car = GetComponentInParent<CarController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the wheel enters the road, we set the settings back to normal
        if(other.CompareTag("Road"))
        {
            car.SetBackToRoadSettings();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the wheel exits the road, we reduce the car's Torque
        if (other.CompareTag("Road"))
        {
            car.SetOutOfRoadSettings(velocityReductionMultiplier);
        }
    }
}
