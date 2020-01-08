using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using UnityEngine;

public class MoveTowards : Action
{
    public TreeBlackBoard blackBoard;
	public Transform target;

	private bool refreshTarget = false;
	private NavMeshAgent navA = null;
	public override void OnStart ( )
	{
		base.OnStart();

		if(target == null)
		{
			target = (Transform)blackBoard.Value.variables["Target"];
			refreshTarget = true;
		}

		navA = this.GetComponent<NavMeshAgent>();
		navA.SetDestination( target.position );

	}
	public override TaskStatus OnUpdate( )
	{
		if (!navA.pathPending)
		{
			if (navA.remainingDistance <= navA.stoppingDistance)
			{
				if (!navA.hasPath || navA.velocity.sqrMagnitude == 0f)
				{
					NavMeshAgent navTarget = target.gameObject.GetComponent<NavMeshAgent>();
					if (navTarget)
						navTarget.enabled = false;

					return TaskStatus.Success;
				}
			}
		}
		if (refreshTarget && target.position != navA.destination && navA.remainingDistance > navA.stoppingDistance)
		{
			navA.SetDestination( target.position );
		}
		return TaskStatus.Running;
	}

	
	public override void OnEnd ( )
	{
		base.OnEnd();
		navA.isStopped = true;
		navA.ResetPath();
		if (refreshTarget)
		{
			refreshTarget = false;
			target = null;
		}
	}

	public override void OnCollisionEnter (Collision collision)
	{
		base.OnCollisionEnter( collision );

	}

	public override void OnTriggerEnter (Collider other)
	{
		base.OnTriggerEnter( other );

	}

}