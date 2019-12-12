using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float speed;
    private float lastSpeed, distance;
    public float minFOV, maxFOV, damping, sensitivityDistance;
    private bool doOnce;

    void Start()
    {
        lastSpeed = speed;//uso lastSpeed para poder resetear la speed despues
        distance = maxFOV; //nos aseguramos de que al principo este en zoom out
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, distance, Time.deltaTime * damping);
    }

    public void Tick()
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityDistance;
        distance = Mathf.Clamp(distance, minFOV, maxFOV);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, distance, Time.deltaTime * damping);

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) ||
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
        }
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


}
