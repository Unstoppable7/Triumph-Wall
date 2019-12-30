using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class AlarmHandler : SingletonComponent<AlarmHandler>
{
    public struct Alarm
    {
        public Alarm(int id, bool single, double startedTime, double periodTime, UnityAction function)
        {
            this.id = id;
            this.single = single;
            this.startedTime = startedTime;
            this.periodTime = periodTime;
            this.finishTime = startedTime + periodTime;
            alarmEvent = new UnityEvent();
            alarmEvent.AddListener(function);
        }

        public Alarm(int id, bool single, double startedTime, double periodTime,ref UnityEvent _event)
        {
            this.id = id;
            this.single = single;
            this.startedTime = startedTime;
            this.periodTime = periodTime;
            this.finishTime = startedTime + periodTime;
            alarmEvent = _event;
        }

        public int id;
        public bool single;
        public double finishTime;
        public double startedTime;
        public double periodTime;
        public UnityEvent alarmEvent;
    }

    [SerializeField][ReadOnly]
    //Controla el tiempo de todas las alarmas
    double currentTime = 0;

    //Lista de todas las alarmas activas
    SortedList<double,List<Alarm>> alarmsSortedList = new SortedList<double, List<Alarm>>();

    [SerializeField][ReadOnly]
    //Lista de indices disponibles y usados
    List<bool> indexList;

    private void Awake()
    {
        SetUp();
    }

    /// <summary>
    /// Reinicia todo.
    /// </summary>
    public void SetUp()
    {
        currentTime = 0;
        alarmsSortedList = new SortedList<double, List<Alarm>>();
        indexList = new List<bool>();
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        CheckAlarm();
    }

    /// <summary>
    /// Método utilizado para añadir una nueva alarma.
    /// Devuelve la id del alarma para poder borrarla posteriormente.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="single"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public int AddAlarm(float period, bool single, UnityAction callBack)
    {

        int index = -1;
        if(indexList.Count == 0 || indexList.IndexOf(false) == -1)
        {
            index = indexList.Count;
            indexList.Add(true);
        }
        else
        {
            index = indexList.IndexOf(false);
            indexList[index] = true;
        }
        Alarm tempAlarm = new Alarm(index, single, currentTime, period, callBack);

        if(alarmsSortedList.IndexOfKey(tempAlarm.finishTime) == -1)
        {
            alarmsSortedList.Add(tempAlarm.finishTime, new List<Alarm>());
        }
        double keyIndex = alarmsSortedList.IndexOfKey(tempAlarm.finishTime);
        alarmsSortedList[tempAlarm.finishTime].Add(tempAlarm);

        return tempAlarm.id;
    }

    /// <summary>
    /// Quita un alarma a partir de la id dada por el AddAlarm().
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool RemoveAlarm(int id)
    {
        for (int i = 0; i < alarmsSortedList.Count; i++)
        {
            for (int j = 0; j < alarmsSortedList.Values[i].Count; j++)
            {
                if (alarmsSortedList.Values[i][j].id == id)
                {
                    alarmsSortedList.Values[i].RemoveAt(j);
                    if(alarmsSortedList.Values[i].Count == 0)
                    {
                        alarmsSortedList.RemoveAt(i);
                    }
                    indexList[id] = false;
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Interno.
    /// Se utiliza para añadir una alarma a partir de otra pre existente.
    /// </summary>
    /// <param name="alarm"></param>
    private void AddAlarm(Alarm alarm)
    {
        indexList[alarm.id] = true;//ESTO PODRÍA SER UN PROBLEMA
        Alarm tempAlarm = new Alarm(alarm.id, alarm.single, currentTime, alarm.periodTime,ref alarm.alarmEvent);

        if (alarmsSortedList.IndexOfKey(tempAlarm.finishTime) == -1)
        {
            alarmsSortedList.Add(tempAlarm.finishTime, new List<Alarm>());
        }
        double keyIndex = alarmsSortedList.IndexOfKey(tempAlarm.finishTime);
        alarmsSortedList[tempAlarm.finishTime].Add(tempAlarm);
    }

    /// <summary>
    /// Comprueba si la alarma más próxima tiene que activarse.
    /// </summary>
    private void CheckAlarm()
    {
        if(alarmsSortedList.Count == 0)
        {
            return;
        }
        else
        {
            List<Alarm> currentAlarm = new List<Alarm>(alarmsSortedList.Values[0]);

            if (currentAlarm[0].finishTime > currentTime)
            {
                return;
            }
            else
            {
                for (int i = 0; i < currentAlarm.Count; i++)
                {
                    currentAlarm[i].alarmEvent.Invoke();
                }

                for(int i = 0; i < currentAlarm.Count; i++)
                {

                    RemoveAlarm(currentAlarm[i].id);
                    //Si no es de una sola vez la volvemos a meter
                    if (!currentAlarm[i].single)
                    {
                        AddAlarm(currentAlarm[i]);
                    }
                }
                CheckAlarm();
            }
        }
    }
}

