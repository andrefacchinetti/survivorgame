using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    public Canvas canvas;
    public RawImage rawImage;
    public Vector2 pos;
    private void Start() {
        rawImage = gameObject.GetComponent<RawImage>();
    }
    private void LateUpdate() {
        transform.position = Input.mousePosition;
    }
}
