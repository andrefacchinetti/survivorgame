using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class CableProceduralSimple : MonoBehaviour
{

	LineRenderer line;

	// The Start of the cable will be the transform of the Gameobject that has this component.
	// The Transform of the Gameobject where the End of the cable is. This needs to be assigned in the inspector.
	[SerializeField] Transform endPointTransform;

	// Number of points per meter
	[SerializeField, Tooltip("Number of points per unit length, using the straight line from the start to the end transform.")] float pointDensity = 3;

	// How much the cable will sag by.
	[SerializeField] float sagAmplitude = 1;

	// How much wind will move the cable.
	[SerializeField] float swayMultiplier = 1;
	[SerializeField] float swayXMultiplier = 1;
	[SerializeField] float swayYMultiplier = .5f;
	// How fast the cable will go back and forth per second.
	[SerializeField] float swayFrequency = 1;

	// These are used later for calculations
	int pointsInLineRenderer;
	
	Vector3 sagDirection;
	float swayValue;



	void Start () 
	{
		line = GetComponent<LineRenderer>();

		if (!endPointTransform)
		{
			Debug.LogError("No Endpoint Transform assigned to Cable_Procedural component attached to " + gameObject.name);
			return;
		}
		transform.forward = (endPointTransform.position - transform.position).normalized;
		pointsInLineRenderer = Mathf.FloorToInt(pointDensity * (endPointTransform.position - transform.position).magnitude);
		line.positionCount = pointsInLineRenderer;
		sagDirection = Physics.gravity.normalized;
	}
	


	void Update () 
	{
		transform.forward = (endPointTransform.position - transform.position).normalized;
		pointsInLineRenderer = Mathf.FloorToInt(pointDensity * (endPointTransform.position - transform.position).magnitude);
		line.positionCount = pointsInLineRenderer;
		sagDirection = Physics.gravity.normalized;
		Draw();
	}



	void Draw()
	{
		if (!endPointTransform)
		{
			return;
		}

		// What point is being calculated
		int i = 0;

		swayValue += swayFrequency * Time.deltaTime;

		// Clamp the wind value to stay within a cirlce's radian limits.
		if(swayValue > Mathf.PI * 2){swayValue = 0;}
		if(swayValue < 0){swayValue = Mathf.PI * 2;}


		while(i < pointsInLineRenderer)
		{
			// This is the fraction of where we are in the cable and it accounts for arrays starting at zero.
			float pointForCalcs = (float)i / (pointsInLineRenderer - 1);
			// This is what gives the cable a curve and makes the wind move the center the most.
			float effectAtPointMultiplier = Mathf.Sin(pointForCalcs * Mathf.PI);

			// Calculate the position of the current point i
			Vector3 pointPosition = (endPointTransform.position - transform.position) * pointForCalcs;
			// Calculate the sag vector for the current point i
			Vector3 sagAtPoint = sagDirection * sagAmplitude;
			// Calculate the sway vector for the current point i
			Vector3 swayAtPoint = swayMultiplier * transform.TransformDirection( new Vector3(Mathf.Sin(swayValue) * swayXMultiplier, Mathf.Cos(2 * swayValue + Mathf.PI) * .5f * swayYMultiplier, 0));
			// Calculate the waving due to wind for the current point i 

			// Calculate the postion with Sag.
			Vector3 currentPointsPosition = 
				transform.position + 
				pointPosition + 
				(swayAtPoint + 
					Vector3.ClampMagnitude(sagAtPoint, sagAmplitude)) * effectAtPointMultiplier;
		

			// Set point
			line.SetPosition(i, currentPointsPosition);
			i++;
		}
	}
}
