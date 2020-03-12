using Battle.Zones;
using Cards;
using Cards.General;
using Deck;
using Resting;
using Units.Enemy.General;
using Units.Player.General;
using Zenject;

namespace Battle.General
{
	public class BattleInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.Bind<Player>()
				.FromComponentInHierarchy(true)
				.AsSingle();


			Container
				.Bind<General.BattleTurnProcedure>()
				.FromComponentInHierarchy(true)
				.AsSingle();

			Container
				.Bind<RestMenu>()
				.FromComponentInHierarchy(true)
				.AsSingle();


			Container
				.Bind<Hand>()
				.AsSingle();

			Container
				.Bind<HandZone>()
				.FromComponentInHierarchy(true)
				.AsSingle();

			Container
				.Bind<UILineRenderer>()
				.FromComponentInHierarchy(true)
				.AsSingle();

			Container
				.Bind<DrawPile>()
				.AsSingle();
			
			Container
				.Bind<BlessingPile>()
				.AsSingle();

			Container
				.Bind<CardDeck>()
				.AsSingle();

			Container
				.Bind<BanishPile>()
				.AsSingle();

			Container
				.Bind<DiscardPile>()
				.AsSingle();

			Container
				.Bind<CardHandling>()
				.FromComponentInHierarchy(true)
				.AsSingle();

			Container
				.Bind<Encounter>()
				.AsSingle();
			
			Container
				.Bind<EnemyZone>()
				.FromComponentInHierarchy(true)
				.AsSingle();
		}
	}
}