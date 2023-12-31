using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxSpawner : MonoBehaviour
{
    public GameObject healthBox;
    public int minHealthBoxes = 1;
    public int maxHealthBoxes = 4;

    public int minXPosition = -30;
    public int maxXPosition = 120;

    void Start() {
        
    int numberOfBoxes = Random.Range(minHealthBoxes, maxHealthBoxes);

    for(int i = 0; i < numberOfBoxes; i++) {
        Instantiate(healthBox, new Vector3(Random.Range(minXPosition, maxXPosition), 30, 0), Quaternion.Euler(-90, 0, 0));
    }
    }
}
