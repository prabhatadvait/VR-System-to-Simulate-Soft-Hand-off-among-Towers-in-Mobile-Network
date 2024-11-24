using UnityEngine;

public class NetworkTowerManager : MonoBehaviour
{
    public GameObject car; // Reference to the car object
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    public float switchDistanceThreshold = 50f; // Distance threshold to switch towers
    private GameObject[] networkTowers; // Array to hold the tower GameObjects
    private int currentTowerIndex = -1; // Index to track the current tower

    void Start()
    {
        // Find all objects in the scene with the tag "towers"
        networkTowers = GameObject.FindGameObjectsWithTag("towers");

        if (networkTowers.Length == 0)
        {
            Debug.LogError("No network towers found in the scene with the tag 'towers'!");
            return;
        }

        // Find the closest tower at the start and connect to it
        currentTowerIndex = FindClosestTowerIndex();
        Debug.Log("Starting connection to tower: " + networkTowers[currentTowerIndex].name);
    }

    void FixedUpdate()
    {
        // Get the position of the car
        Vector3 carPosition = car.transform.position;

        // Get the position of the current tower
        Vector3 currentTowerPosition = networkTowers[currentTowerIndex].transform.position;
        
        // Calculate the height of the current tower
        float towerHeight = networkTowers[currentTowerIndex].transform.localScale.y;

        // Set the end position of the line to the top of the tower
        Vector3 towerTopPosition = new Vector3(currentTowerPosition.x, currentTowerPosition.y + towerHeight / 2, currentTowerPosition.z);
        
        // Update the line positions
        lineRenderer.SetPosition(0, carPosition);
        lineRenderer.SetPosition(1, towerTopPosition);
        
        // Calculate the distance to the current tower
        float distanceToCurrentTower = Vector3.Distance(carPosition, towerTopPosition);

        // Adjust line width based on distance
        lineRenderer.startWidth = Mathf.Lerp(0.1f, 0.5f, 1 - (distanceToCurrentTower / switchDistanceThreshold));
        lineRenderer.endWidth = lineRenderer.startWidth;

        // Check if the car is far enough from the current tower to switch
        if (distanceToCurrentTower > switchDistanceThreshold)
        {
            // Find the next closest tower
            int closestTowerIndex = FindClosestTowerIndex();

            if (closestTowerIndex != currentTowerIndex)
            {
                currentTowerIndex = closestTowerIndex;
                Debug.Log("Switched connection to tower: " + networkTowers[currentTowerIndex].name);
            }
        }
    }

    // Method to find the closest tower to the car
    int FindClosestTowerIndex()
    {
        int closestIndex = 0;
        float closestDistance = Mathf.Infinity;
        Vector3 carPosition = car.transform.position;

        // Loop through all towers to find the closest one
        for (int i = 0; i < networkTowers.Length; i++)
        {
            float distanceToTower = Vector3.Distance(carPosition, networkTowers[i].transform.position);
            if (distanceToTower < closestDistance)
            {
                closestDistance = distanceToTower;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}