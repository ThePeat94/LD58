namespace Nidavellir.EventArgs
{
    public class CharacterStatValueChangeEventArgs : System.EventArgs
    {
        public CharacterStatValueChangeEventArgs(int newValue, int oldValue)
        {
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }

        public int Delta => this.NewValue - this.OldValue;
        public int NewValue { get; }
        public int OldValue { get; }
    }
}