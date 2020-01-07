using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : Conditional
{
    public TreeBlackBoard blackBoard;
	private List<Transform> path = null;
	private NavMeshAgent navA = null;
	private int pathIndex = 0;

	public override void OnStart ( )
	{
		base.OnStart();

		path = (List<Transform>)blackBoard.Value.variables["Ruta"];
		navA = this.GetComponent<NavMeshAgent>();
		if(path.Count > 0)
		{
			Transform target;
			Transform nearestPoint = path[0];

			for (int i = 1; path.Count > i; i++)
			{
				Transform currentEnemy = path[i];
				float enemy0Distance = Vector3.Distance( nearestPoint.position, this.transform.position );
				float enemy1Distance = Vector3.Distance( currentEnemy.position, this.transform.position );
				if (enemy0Distance < enemy1Distance)
				{

				}
				else
				{
					nearestPoint = currentEnemy;
					pathIndex = i;
				}
			}
			target = nearestPoint;
			navA.SetDestination( target.position );
		}

	}
	public override TaskStatus OnUpdate()
	{
		path = (List<Transform>)blackBoard.Value.variables["Ruta"];
		if (path.Count > 0)
		{
			if (!navA.pathPending)
			{
				if (navA.remainingDistance <= navA.stoppingDistance)
				{
					if (!navA.hasPath || navA.velocity.sqrMagnitude == 0f)
					{
						//nextPoint yes
						pathIndex++;
						pathIndex = pathIndex % path.Count;
						navA.SetDestination( path[pathIndex].position );
						return TaskStatus.Running;
					}
				}
			}
			return TaskStatus.Running;
		}

		return TaskStatus.Failure;
    }

	public override void OnEnd ( )
	{
		base.OnEnd();
		navA.isStopped = true;
		navA.ResetPath();
	}
}
