using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class MoveTowards : Action
{
    public TreeBlackBoard blackBoard;

	private NavMeshAgent navA = null;
	public override void OnStart ( )
	{
		base.OnStart();
		Agent target = (Agent)blackBoard.Value.variables["Target"];
		navA = this.GetComponent<NavMeshAgent>();
		navA.SetDestination( target.gameObject.transform.position );

	}
	public override TaskStatus OnUpdate( )
	{
		if (!navA.pathPending)
		{
			if (navA.remainingDistance <= navA.stoppingDistance)
			{
				if (!navA.hasPath || navA.velocity.sqrMagnitude == 0f)
				{
					// Done
					return TaskStatus.Success;
				}
			}
		}
		return TaskStatus.Running;
	}

}