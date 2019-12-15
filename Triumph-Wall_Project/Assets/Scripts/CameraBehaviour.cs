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

    public void Tick()
    {
       /* if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            if (doOnce) //se que esta condicion podria ir arriba, pero es mas limpio asi que no tenerla 40 veces
            {
                speed /= 2f;
                doOnce = !doOnce;
            }

        }
        else
        {
            speed = lastSpeed;
            doOnce = !doOnce;
        }*/

        

        //MoveDrag();
    }
    void Update()
    {
       

    }

    public void MoveX(float multiplier) //el multiplier controla la direccion de la camara
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed * multiplier);
    }
    public void MoveZ(float multiplier)
    {
        transform.Translate(Vector3.up* Time.deltaTime * speed * multiplier);
    }

    public void ZoomInOut(float axis)
    {
        distance -= axis * sensitivityDistance;
        distance = Mathf.Clamp(distance, minFOV, maxFOV);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, distance, Time.deltaTime * damping);
    }

    public void MoveDrag(Vector2 actualMousePos)
    {
     
        Vector3 pos = Camera.main.ScreenToViewportPoint(actualMousePos - dragOrigin);
        Vector3 move = new Vector3(pos.x * speed/10,  pos.y * speed/10, pos.z * speed/10);

        transform.Translate(move);            
       
    }

    public void StartDrag(Vector2 origin)
    {
        dragOrigin = origin;
    }

}
