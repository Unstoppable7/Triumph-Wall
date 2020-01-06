﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BehaviorDesigner.Runtime;

public class SensorySystem : MonoBehaviour
{
    [Title("Main Variables")]
    [SerializeField]
    float alarmTime = 0.2f;
    [SerializeField]
    LayerMask collisionLayer;
    [SerializeField]
    bool searchForAgents = true;

    [SerializeField][FoldoutGroup("External Sensor")]
    private float ExternalSensor_Range = 40;
    [SerializeField][FoldoutGroup("External Sensor")]
    private int ExternalSensor_FOV = 30;

    [SerializeField][FoldoutGroup("Inner Sensor")]
    private float InnerSensor_Range = 25;
    [SerializeField][FoldoutGroup("Inner Sensor")]
    private int InnerSensor_FOV = 130;

    [Title("Special")]
    [SerializeField]
    private float SpecialSensor_Range = 5;
    [Title("Lists")]
    [ShowInInspector]
    private List<Agent> agentsInSight = new List<Agent>();
    [ShowInInspector]
    private List<Agent> agentInSensorRange = new List<Agent>();
    [ShowInInspector]
    private Collider[] collidersInSensorRange;

    BlackBoard blackBoard;

    Coroutine co_SearchAgents;
    WaitForSecondsRealtime _alarmTime;

    private void Awake()
    {
        if(this.gameObject.GetComponent<Agent>() is null)
        {
            Debug.LogError("No Agent attached to:" + this.name);
        }
        else
        {
            blackBoard = this.gameObject.GetComponent<BlackBoard>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        co_SearchAgents = StartCoroutine("CO_SearchAgents");
        Reset();
    }

    public void Reset()
    {
        _alarmTime = new WaitForSecondsRealtime(alarmTime);
        collisionLayer = LayerMask.GetMask("Agent");
    }

    IEnumerator CO_SearchAgents()
    {
        do
        {
            SearchAgents();
            yield return  _alarmTime;
        } while (searchForAgents);
    }

    private void SearchAgents()
    {
        CheckAgentsInsideSphere();
    }

    private void CheckAgentsInsideSphere()
    {
        collidersInSensorRange = Physics.OverlapSphere(transform.position, ExternalSensor_Range, collisionLayer);
        if (collidersInSensorRange.Length == 0)
        {
            return;
        }

        agentInSensorRange.Clear();

        for (int i = 0; i < collidersInSensorRange.Length; i++)
        {
            if(!collidersInSensorRange[i].GetComponentInParent<Agent>())
            {
                
            }
            else
            {

                if (GameObject.ReferenceEquals(collidersInSensorRange[i].transform.parent.gameObject, this.gameObject))
                {

                }
                else
                {
                    agentInSensorRange.Add(collidersInSensorRange[i].GetComponentInParent<Agent>());
                }
            }
        }

        if(agentInSensorRange.Count > 0)
        {
            CheckAgentsInSight();
        }
    }

    private void CheckAgentsInSight()
    {
        agentsInSight.Clear();
		Agent currentAgent;
        Vector3 innerAngleZero = Vector3.Normalize(Quaternion.AngleAxis(InnerSensor_FOV / 2.0f, this.transform.up) * this.transform.forward);
        Vector3 externalAngleZero = Vector3.Normalize(Quaternion.AngleAxis(ExternalSensor_FOV / 2.0f, this.transform.up) * this.transform.forward);
        Vector3 agentAngle = Vector3.zero;
        float distanceToAgent = Mathf.Infinity;

        for (int i = 0; i < agentInSensorRange.Count; i++)
        {
            currentAgent = agentInSensorRange[i];
            agentAngle = Vector3.Normalize(currentAgent.transform.position - this.transform.position);
            distanceToAgent = Vector3.Distance(currentAgent.transform.position, this.transform.position);

            if (distanceToAgent < SpecialSensor_Range)
            {
                //Se encuentra dentro de la esfera pequeña
                agentsInSight.Add(currentAgent);
                agentInSensorRange.RemoveAt(i);
                i--;
            }
            else
            if(Mathf.Acos(Vector3.Dot(this.transform.forward,agentAngle)) <= Mathf.Deg2Rad * (ExternalSensor_FOV * 0.5f))
            {
                //Se encuentran dentro del FOV exterior
                agentsInSight.Add(currentAgent);
                agentInSensorRange.RemoveAt(i);
                i--;
            }
            else
            if(Mathf.Acos(Vector3.Dot(this.transform.forward, agentAngle)) <= Mathf.Deg2Rad * (InnerSensor_FOV * 0.5f)
                && distanceToAgent < InnerSensor_Range)
            {
                //Se encuentran dentro del FOV interior y a rango
                agentsInSight.Add(currentAgent);
                agentInSensorRange.RemoveAt(i);
                i--;
            }
        }

        blackBoard.variables["agentsInSight"] = agentsInSight;
    }

    private void OnDrawGizmos()
    {
        //Vector3 dirLeft = Quaternion.AngleAxis(-(ExternalSensor_FOV / 2.0f), transform.up) * transform.forward;
        //Vector3 dirRight = Quaternion.AngleAxis((ExternalSensor_FOV / 2.0f), transform.up) * transform.forward;
        //Gizmos.DrawLine(transform.position, transform.position + dirLeft * ExternalSensor_Range);
        //Gizmos.DrawLine(transform.position, transform.position + dirRight * ExternalSensor_Range);

        //int anglePerSubdivision = 1;
        //int subDivisions = (int)ExternalSensor_FOV / anglePerSubdivision;

        //for (int i = 0; i < subDivisions; i++)
        //{
        //    dirLeft = Quaternion.AngleAxis((ExternalSensor_FOV / 2.0f) - (i * anglePerSubdivision), transform.up) * transform.forward;
        //    dirRight = Quaternion.AngleAxis((ExternalSensor_FOV / 2.0f) - ((i + 1) * anglePerSubdivision), transform.up) * transform.forward;
        //    Gizmos.DrawLine(transform.position + dirRight * ExternalSensor_Range, transform.position + dirLeft * ExternalSensor_Range);
        //}

        //dirLeft = Quaternion.AngleAxis(-(InternalSensor_FOV / 2.0f), transform.up) * transform.forward;
        //dirRight = Quaternion.AngleAxis((InternalSensor_FOV / 2.0f), transform.up) * transform.forward;
        //Gizmos.DrawLine(transform.position, transform.position + dirLeft * InternalSensor_Range);
        //Gizmos.DrawLine(transform.position, transform.position + dirRight * InternalSensor_Range);

        //anglePerSubdivision = 1;
        //subDivisions = (int)InternalSensor_FOV / anglePerSubdivision;

        //for (int i = 0; i < subDivisions; i++)
        //{
        //    dirLeft = Quaternion.AngleAxis((InternalSensor_FOV / 2.0f) - (i * anglePerSubdivision), transform.up) * transform.forward;
        //    dirRight = Quaternion.AngleAxis((InternalSensor_FOV / 2.0f) - ((i + 1) * anglePerSubdivision), transform.up) * transform.forward;
        //    Gizmos.DrawLine(transform.position + dirRight * InternalSensor_Range, transform.position + dirLeft * InternalSensor_Range);
        //}
        //Gizmos.DrawWireSphere(transform.position, SpecialSensor_Range);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 dirLeft = Quaternion.AngleAxis(-(ExternalSensor_FOV / 2.0f), transform.up) * transform.forward;
        Vector3 dirRight = Quaternion.AngleAxis((ExternalSensor_FOV / 2.0f), transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + dirLeft * ExternalSensor_Range);
        Gizmos.DrawLine(transform.position, transform.position + dirRight * ExternalSensor_Range);

        int anglePerSubdivision = 1;
        int subDivisions = (int)ExternalSensor_FOV / anglePerSubdivision;

        for (int i = 0; i < subDivisions; i++)
        {
            dirLeft = Quaternion.AngleAxis((ExternalSensor_FOV / 2.0f) - (i * anglePerSubdivision), transform.up) * transform.forward;
            dirRight = Quaternion.AngleAxis((ExternalSensor_FOV / 2.0f) - ((i + 1) * anglePerSubdivision), transform.up) * transform.forward;
            Gizmos.DrawLine(transform.position + dirRight * ExternalSensor_Range, transform.position + dirLeft * ExternalSensor_Range);
        }

        dirLeft = Quaternion.AngleAxis(-(InnerSensor_FOV / 2.0f), transform.up) * transform.forward;
        dirRight = Quaternion.AngleAxis((InnerSensor_FOV / 2.0f), transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + dirLeft * InnerSensor_Range);
        Gizmos.DrawLine(transform.position, transform.position + dirRight * InnerSensor_Range);

        anglePerSubdivision = 1;
        subDivisions = (int)InnerSensor_FOV / anglePerSubdivision;

        for (int i = 0; i < subDivisions; i++)
        {
            dirLeft = Quaternion.AngleAxis((InnerSensor_FOV / 2.0f) - (i * anglePerSubdivision), transform.up) * transform.forward;
            dirRight = Quaternion.AngleAxis((InnerSensor_FOV / 2.0f) - ((i + 1) * anglePerSubdivision), transform.up) * transform.forward;
            Gizmos.DrawLine(transform.position + dirRight * InnerSensor_Range, transform.position + dirLeft * InnerSensor_Range);
        }
        Gizmos.DrawWireSphere(transform.position, SpecialSensor_Range);
    }
}
