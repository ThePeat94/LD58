using Nidavellir.Entity;
using Nidavellir.Scriptables.Location;

namespace Nidavellir.Location
{
    public class StatBuffEventLocationHandler : IEventLocationHandler
    {
        private readonly EntityStats m_playerStats;
        private readonly StatBuffEventLocationData  m_statBuffEventLocationData;

        public StatBuffEventLocationHandler(EntityStats playerStats, StatBuffEventLocationData statBuffEventLocationData)
        {
            this.m_playerStats = playerStats;
            this.m_statBuffEventLocationData = statBuffEventLocationData;
        }
        
        public void Execute()
        {
            this.m_playerStats.ApplyStatIncreases(this.m_statBuffEventLocationData.CharacterStatIncreases);
        }
    }
}