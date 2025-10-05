using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Character Stat Facade", menuName = "Data/Character Stat Facade", order = 0)]
    public class CharacterStatFacade : ScriptableObject
    {
        [SerializeField] private CharacterStat m_hp;
        [SerializeField] private CharacterStat m_attack;
        [SerializeField] private CharacterStat m_defense;
        [SerializeField] private CharacterStat m_likes;
        [SerializeField] private CharacterStat m_dislikes;
        [SerializeField] private CharacterStat m_superLike;
        [SerializeField] private CharacterStat m_money;
        [SerializeField] private CharacterStat m_distance;
        [SerializeField] private CharacterStat m_round;
        [SerializeField] private CharacterStat m_rizz;
        [SerializeField] private CharacterStat m_atkSpeed;
        
        public CharacterStat Hp => this.m_hp;
        public CharacterStat Attack => this.m_attack;
        public CharacterStat Defense => this.m_defense;
        public CharacterStat Likes => this.m_likes;
        public CharacterStat Dislikes => this.m_dislikes;
        public CharacterStat SuperLike => this.m_superLike;
        public CharacterStat Money => this.m_money;
        public CharacterStat Distance => this.m_distance;
        public CharacterStat Round => this.m_round;
        public CharacterStat Rizz => this.m_rizz;
        public CharacterStat AtkSpeed => this.m_atkSpeed;
    }
}