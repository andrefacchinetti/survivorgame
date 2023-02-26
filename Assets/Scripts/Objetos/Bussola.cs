using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bussola : MonoBehaviour
{

    private Compass compass;

    // Start is called before the first frame update
    void Start()
    {
        compass = Input.compass;
        compass.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float heading = compass.magneticHeading;
        transform.rotation = Quaternion.Euler(0, heading, 0);
    }
}
