using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Util
{
    [System.Serializable]
    public class StatRange
    {
        [SerializeField] private CharacterStat m_characterStat;
        [SerializeField, Range(1, 1000)] private int m_value;
    }
}