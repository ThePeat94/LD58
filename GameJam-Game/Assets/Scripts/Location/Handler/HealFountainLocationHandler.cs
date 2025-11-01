using Nidavellir.Entity;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Location;

namespace Nidavellir.Location.Handler
{
    public class HealFountainLocationHandler : IEventLocationHandler
    {
        private readonly EntityStats m_playerStats;
        private readonly CharacterStat m_hpStat;
        private readonly HealFountainEventLocationData m_healFountainEventLocationData;
        private readonly LocationDraftManager m_locationDraftManager;

        public HealFountainLocationHandler(EntityStats playerStats, CharacterStat hpStat, HealFountainEventLocationData healFountainEventLocationData, LocationDraftManager locationDraftManager)
        {
            this.m_playerStats = playerStats;
            this.m_hpStat = hpStat;
            this.m_healFountainEventLocationData = healFountainEventLocationData;
            this.m_locationDraftManager = locationDraftManager;
        }
        
        public void Execute()
        {
            this.m_playerStats[this.m_hpStat].Add(this.m_healFountainEventLocationData.HealingAmount);
            this.m_locationDraftManager.SelectLocations();
        }
    }
}