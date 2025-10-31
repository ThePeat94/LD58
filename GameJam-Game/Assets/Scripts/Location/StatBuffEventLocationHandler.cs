using Nidavellir.Entity;
using Nidavellir.Scriptables.Location;

namespace Nidavellir.Location
{
    public class StatBuffEventLocationHandler : IEventLocationHandler
    {
        private readonly EntityStats m_playerStats;
        private readonly StatBuffEventLocationData  m_statBuffEventLocationData;
        private readonly LocationDraftManager m_locationDraftManager;

        public StatBuffEventLocationHandler(EntityStats playerStats, StatBuffEventLocationData statBuffEventLocationData, LocationDraftManager locationDraftManager)
        {
            this.m_playerStats = playerStats;
            this.m_statBuffEventLocationData = statBuffEventLocationData;
            this.m_locationDraftManager = locationDraftManager;
        }

        public void Execute()
        {
            this.m_playerStats.ApplyStatIncreases(this.m_statBuffEventLocationData.CharacterStatIncreases);
            this.m_locationDraftManager.SelectLocations();
        }
    }
}