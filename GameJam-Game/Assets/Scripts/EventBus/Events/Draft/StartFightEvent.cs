using System.Collections.Generic;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;

namespace Nidavellir.EventBus.Events.Draft
{
    public class StartFightEvent : IEvent
    {
        public StartFightEvent(List<RuntimeEnemyInformation> likedProfiles)
        {
            this.LikedProfiles = likedProfiles;
        }
        
        public List<RuntimeEnemyInformation> LikedProfiles { get; }
    }
}