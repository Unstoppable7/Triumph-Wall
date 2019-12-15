using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float speed;
    private float lastSpeed, distance;
    public float minFOV, maxFOV, damping, sensitivityDistance;
    private bool doOnce;
    Vector2 dragOrigin;

    void Start()
    {
        lastSpeed = speed;//uso lastSpeed para poder resetear la speed despues
        distance = maxFOV; //nos aseguramos de que al principo este en zoom out
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, distance, Time.deltaTime * damping);
    }

    public void MoveX(float multiplier) //el multiplier controla la direccion de la camara
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed * multiplier, Space.World);
    }
    public void MoveZ(float multiplier)
    {
        transform.Translate(Vector3.forward* Time.deltaTime * speed * multiplier, Space.World);
    }

    public void ZoomInOut(float axis)
    {
        distance -= axis * sensitivityDistance;
        distance = Mathf.Clamp(distance, minFOV, maxFOV);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, distance, Time.deltaTime * damping);
    }

    public void MoveDrag(Vector2 actualMousePos)
    {
		//Borrarlo y ponerlo en la camara
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(actualMousePos);
		if (Physics.Raycast( ray, out hit ))
		{
			actualMousePos = new Vector2( hit.point.x, hit.point.z );
			Vector3 posD = actualMousePos - dragOrigin;
			Vector3 move = new Vector3( posD.x * 1, 0, posD.y * 1 );
			transform.Translate( -move, Space.World );
		}
    }

    public void StartDrag(Vector2 origin)
    {
        //dragOrigin = origin;
		//Borrarlo y ponerlo en la camara
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay( origin );
		if (Physics.Raycast( ray, out hit ))
		{
			dragOrigin = new Vector2( hit.point.x, hit.point.z );
		}
	}
}
