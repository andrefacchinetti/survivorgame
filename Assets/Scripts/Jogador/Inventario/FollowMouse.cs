using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    public Vector3 pos, offset;
    private void Start() {
    }
    private void LateUpdate() {
        pos = Input.mousePosition + offset;
        transform.position = pos;
    }
}
