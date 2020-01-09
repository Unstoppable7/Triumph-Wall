using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SOs/ResourceController" )]
public class SO_ResourceController : SerializedScriptableObject
{

	[Title( "Positive Factors" )]
	[SerializeField]
	[HideInInspector]
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	public float happinesPoFactor = 0.4f;
	[SerializeField]
	[HideInInspector]
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	public float efficiencyPoFactor = 0.4f;
	[SerializeField]
	[FoldoutGroup( "Opinion Publica", Order = 0 )][Range(0,1)]
	public float daysWithoutCasualtiesPoFactor = 0.4f;

	[PropertyRange( 0, 1 )]
	[ShowInInspector]
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	[PropertyOrder( -1 )]
	private float HappinesPoFactor
	{
		get { return happinesPoFactor; }
		set
		{
			happinesPoFactor = value;
			efficiencyPoFactor = 1 - happinesPoFactor;
		}
	}
	[PropertyRange( 0, 1 )]
	[ShowInInspector]
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	[PropertyOrder( -1 )]
	private float EfficiencyPoFactor
	{
		get { return efficiencyPoFactor; }
		set
		{
			efficiencyPoFactor = value;
			happinesPoFactor = 1 - efficiencyPoFactor;
		}
	}



	[Title( "Negative Factors" )]
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	public float woundedDeportedPoPenalty = 0.005f;
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	public float greavousDeportedPoPenalty = 0.01f;
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	public float woundPoPenalty = 0.005f; //5 unidades por cada herido
	[FoldoutGroup( "Opinion Publica", Order = 0 )]
	public float deathPoPenalty = 0.3f; //300 unidade por cada muerto 


	[Title( "Dinero" )]
	[FoldoutGroup( "Dinero",Order = 0)]
	[PropertyOrder(-1)]
	public float governmentMoney = 0;//from Balance File

	[Title("Positive Factors", "Cuanto mas de estas cosas tengas MAS dinerito")]
	[PropertyRange( 0, 1 )]
	[ShowInInspector]
	[FoldoutGroup( "Dinero", Order = 0 )]
	[PropertyOrder(-1)]
	private float PublicOpinionMoneyFactor
	{
		get { return publicOpinionMoneyFactor; }
		set
		{
			publicOpinionMoneyFactor = value;
			efficiencyMoneyFactor = 1 - publicOpinionMoneyFactor;
		}
	}
	[PropertyRange( 0, 1 )]
	[ShowInInspector]
	[FoldoutGroup( "Dinero", Order = 0 )]
	[PropertyOrder(-1)]
	private float EfficiencyMoneyFactor
	{
		get { return efficiencyMoneyFactor; }
		set
		{
			efficiencyMoneyFactor = value;
			publicOpinionMoneyFactor = 1 - efficiencyMoneyFactor;
		}
	}

	[Title( "Negative Factors", "Cuanto mas de estas cosas tengas MENOS dinerito")]
	[FoldoutGroup( "Dinero", Order = 1 )]
	public float woundedMoneyPenalty = 0.005f; //5 euros por cada herido
	[FoldoutGroup( "Dinero", Order = 1 )]
	public float deathMoneyPenalty = 0.01f; //10 euros por cada muerto  


	[SerializeField]
	[HideInInspector]
	public float publicOpinionMoneyFactor = 0.3f;
	[SerializeField]
	[HideInInspector]
	public float efficiencyMoneyFactor = 0.7f;
}
