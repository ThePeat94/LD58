using System.Collections.Generic;
using Nidavellir.Scriptables;

namespace Nidavellir.UI.Draft
{
    public class RuntimeEnemyInformation
    {
        public EnemyData BaseData { get; }
        public Dictionary<CharacterStat, int> Stats { get; }
        public int Power { get; set; }
        public bool SuperLiked { get; set; }
        
        public RuntimeEnemyInformation(EnemyData baseData, Dictionary<CharacterStat, int> stats, int power)
        {
            this.BaseData = baseData;
            this.Stats = stats;
            this.Power = power;
        }
    }
}