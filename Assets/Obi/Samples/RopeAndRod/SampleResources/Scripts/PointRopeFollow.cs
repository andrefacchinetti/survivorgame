using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRopeFollow : MonoBehaviour
{

    [SerializeField] Transform objFollowed;

    void LateUpdate()
    {
        this.transform.position = objFollowed.transform.position;
    }
}
