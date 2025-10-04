using Nidavellir.GameState;

namespace Nidavellir.GameEventBus.Events
{
    public class GameStateChangedEvent : IEvent
    {
        public GameStateChangedEvent(State newState)
        {
            this.NewState = newState;
        }

        public State NewState { get; }
    }
}