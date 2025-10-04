using System;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Util
{
    [System.Serializable]
    public class InitialStatValue
    {
        [SerializeField] private CharacterStat m_characterStat;
        [SerializeField] private int m_value;
        [SerializeField] private int m_maxValue;

        public CharacterStat CharacterStat => this.m_characterStat;
        public int Value => this.m_value;
        public int MaxValue => this.m_maxValue;
    }
}