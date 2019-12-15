using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SensorySystem_Guard : MonoBehaviour
{

    [SerializeField]
    private CapsuleCollider Collider_SensorRange;

    [SerializeField]
    private float FrontDistance_SensorRange = 20;
    [SerializeField]
    private float SideDistance_SensorRange = 5;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Collider_SensorRange is null)
        {

        }
        else
        {
            Collider_SensorRange.height = FrontDistance_SensorRange;
            Collider_SensorRange.radius = SideDistance_SensorRange;
            Collider_SensorRange.center = new Vector3(0, 0, SideDistance_SensorRange);
            Collider_SensorRange.direction = 2;
        }
#endif

    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
