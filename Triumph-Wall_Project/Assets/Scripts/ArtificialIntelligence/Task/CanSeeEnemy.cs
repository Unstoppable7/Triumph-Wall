using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class CanSeeEnemy : Action
{
    public TreeBlackBoard blackBoard;

    public override TaskStatus OnUpdate()
    {
        List<Agent> enemiesInSight = (List<Agent>)blackBoard.Value.variables["enemiesInSight"];

        if (enemiesInSight.Count > 0)
        {
            Agent nearestEnemy = enemiesInSight[0];
            for(int i = 1; enemiesInSight.Count > i; i++)
            {
                Agent currentEnemy = enemiesInSight[i];
                float enemy0Distance = Vector3.Distance(nearestEnemy.transform.position, this.transform.position);
                float enemy1Distance = Vector3.Distance(currentEnemy.transform.position, this.transform.position);
                if(enemy0Distance < enemy1Distance)
                {

                }
                else
                {
                    nearestEnemy = currentEnemy;
                }
            }

            blackBoard.Value.variables["Target"] = nearestEnemy;
            return TaskStatus.Success;
        }

        blackBoard.Value.variables["Target"] = null;

        return TaskStatus.Failure;
    }
}
