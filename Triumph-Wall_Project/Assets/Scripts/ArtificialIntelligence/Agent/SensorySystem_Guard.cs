using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SensorySystem_Guard : MonoBehaviour
{

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
#endif

    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
