using System.Collections.Generic;
using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Status.Types;
using Units.General;
using UnityEngine;
using Utilities;

namespace Status.General
{
	[Union(0, typeof(CorruptionData))]
	[Union(1, typeof(DecrepitudeData))] [Union(2, typeof(DemonicInfusionData))]
	[Union(3, typeof(FatigueData))] [Union(4, typeof(MasochistData))]
	[Union(5, typeof(MightData))] [Union(6, typeof(PerseveranceData))]
	[Union(7, typeof(PurityData))] [Union(8, typeof(TemptationData))]
	[Union(9, typeof(VulnerabilityData))] [Union(10, typeof(SigilData))]
	[Union(11, typeof(DivineBeaconData))] [Union(12, typeof(ResilienceData))]
	[Union(13, typeof(DivineInterventionData))] [Union(14, typeof(DefensiveStanceData))]
	[Union(15, typeof(RecklessData))] [Union(16, typeof(ThePriceData))]
	[Union(17, typeof(DarkBlessingData))] [Union(18, typeof(RiposteData))]
	[Union(19, typeof(BountyOfFaithData))] [Union(20, typeof(SwordMasteryData))]
	[Union(21, typeof(UnholyMightData))][Union(22, typeof(UnholyPerseveranceData))]
	[MessagePackObject(true)]
	public abstract class StatusData
	{
		public string Name;
		public Sprite Icon;
		public AudioClip AudioClip;
		[TextArea] public string Description;
		public BuffType BuffType = BuffType.Buff;

		[IgnoreMember]
		public TriggeredAction Trigger;

		public abstract StatusBase Initialize(Unit owner);

		public static List<StatusData> LoadDataList()
		{
			var list = Resources.Load<TextAsset>("Status/StatusList");
			return list ? PersistentJson.Create<StatusDataList>(list.text).Status : null;
		}
	}
}