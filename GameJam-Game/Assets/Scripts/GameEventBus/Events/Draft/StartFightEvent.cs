using System.Collections.Generic;
using Nidavellir.Scriptables;

namespace Nidavellir.GameEventBus.Events.Draft
{
    public class StartFightEvent : IEvent
    {
        public StartFightEvent(List<EnemyData> likedProfiles)
        {
            this.LikedProfiles = likedProfiles;
        }
        
        public List<EnemyData> LikedProfiles { get; }
    }
}