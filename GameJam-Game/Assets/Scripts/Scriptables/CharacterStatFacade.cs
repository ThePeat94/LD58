using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Character Stat Facade", menuName = "Data/Character Stat Facade", order = 0)]
    public class CharacterStatFacade : ScriptableObject
    {
        [SerializeField] private CharacterStat m_hpStat;
        [SerializeField] private CharacterStat m_attackStat;
        [SerializeField] private CharacterStat m_defenseStat;
        [SerializeField] private CharacterStat m_likesStat;
        [SerializeField] private CharacterStat m_dislikesStat;
        [SerializeField] private CharacterStat m_superLikeStat;
        [SerializeField] private CharacterStat m_moneyStat;

        public CharacterStat HpStat => this.m_hpStat;
        public CharacterStat AttackStat => this.m_attackStat;
        public CharacterStat DefenseStat => this.m_defenseStat;
        public CharacterStat LikesStat => this.m_likesStat;
        public CharacterStat DislikesStat => this.m_dislikesStat;
        public CharacterStat SuperLikeStat => this.m_superLikeStat;
        public CharacterStat MoneyStat => this.m_moneyStat;
    }
}