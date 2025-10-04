using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Util
{
    [System.Serializable]
    public class StatRange
    {
        [SerializeField] private CharacterStat m_characterStat;
        [SerializeField] private int m_value;

        public CharacterStat CharacterStat => this.m_characterStat;
        public int Value => this.m_value;
    }
}