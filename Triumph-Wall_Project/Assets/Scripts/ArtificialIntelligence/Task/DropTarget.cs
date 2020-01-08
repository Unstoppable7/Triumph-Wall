using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class DropTarget : Action
{
	public TreeBlackBoard blackBoard;
	private Transform target;
	public CentroDeRetencion sceneManager = null;

	public string newTargetTag = "";


	public override TaskStatus OnUpdate ( )
	{
		base.OnUpdate();
		if (target == null)
			target = (Transform)blackBoard.Value.variables["Target"];

		target.SetParent( null );


		if (string.IsNullOrEmpty( newTargetTag ))
			target.gameObject.SetActive( false );
		else
			target.gameObject.tag = newTargetTag;

		//NavMeshAgent navTarget = target.gameObject.GetComponent<NavMeshAgent>();

		//if (navTarget != null)
		//{
		//	navTarget.enabled = true;
		//}
		sceneManager.StartProcessInmigrant( target.GetComponent<Agent_Inmigrant>() );
		Debug.Log( "Dropped" );
		return TaskStatus.Failure;
	}

	public override void OnEnd ( )
	{
		target = null;
	}
}
