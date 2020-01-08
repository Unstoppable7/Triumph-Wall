using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CatchTarget : Action
{
	public TreeBlackBoard blackBoard;
	public Transform target;

	public override void OnStart ( )
	{
		base.OnStart();

		if(target == null)
			target = (Transform)blackBoard.Value.variables["Target"];


		target.SetParent( this.transform );
	}

	public override TaskStatus OnUpdate ( )
	{
		return TaskStatus.Success;
	}

	public override void OnEnd ( )
	{
		base.OnEnd();
		target = null;
	}
}
