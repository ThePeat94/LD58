using System;
using System.Collections.Generic;
using Nidavellir.Entity;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Scaling;
using Nidavellir.UI.Draft;
using UnityEngine;
using Random = System.Random;

namespace Nidavellir
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private PowerCalculationData m_powerCalculationData;

        private StatController m_roundController;
        private StatController m_rizzController;
        private StatController m_likeController;

        private void Start()
        {
            this.m_roundController = this.m_playerStats[this.m_characterStatFacade.Round];
            this.m_rizzController = this.m_playerStats[this.m_characterStatFacade.Rizz];
            this.m_likeController = this.m_playerStats[this.m_characterStatFacade.Likes];
        }
        
        public RuntimeEnemyInformation CreateEnemy(EnemyData enemyData)
        {
            var d = this.GetDifficultyFactor();
            var calculatedStats = new Dictionary<CharacterStat, int>();
            foreach (var scalableStat in enemyData.ScalableStats.ScalableStats)
            {
                var mult = this.SafeEvaluate(scalableStat.ScalingFactor, d, 1f);
                var varianceAmp = this.SafeEvaluate(scalableStat.VarianceFactor, UnityEngine.Random.value, 0f);
                var variance = UnityEngine.Random.Range(-varianceAmp, varianceAmp);
                var value = Mathf.RoundToInt(Mathf.Max(scalableStat.MinValue, scalableStat.BaseValue * mult * (1f + variance)));
                
                calculatedStats.Add(scalableStat.Stat, value);
            }

            var power = this.CalculatePower(calculatedStats);
            var bounty = this.CalculateBounty(power);
            
            Debug.Log($"Created enemy {enemyData.Name} with power {power} and bounty {bounty}");

            calculatedStats.Add(this.m_characterStatFacade.Money, Mathf.RoundToInt(bounty));
            calculatedStats.Add(this.m_characterStatFacade.Distance, UnityEngine.Random.Range(4, 20));
            
            return new RuntimeEnemyInformation(
                enemyData,
                calculatedStats, 
                Mathf.FloorToInt(power)
            );
        }
        
        public void AmplifyEnemyForSuperlike(RuntimeEnemyInformation enemy)
        {
            var amplifier = this.m_powerCalculationData.SuperlikeAmplifier;
            var amplifiedStats = new Dictionary<CharacterStat, int>();
            foreach (var (stat, value) in enemy.Stats)
            {
                if (stat == this.m_characterStatFacade.Money || stat == this.m_characterStatFacade.Distance)
                {
                    amplifiedStats[stat] = value;
                    continue;
                }

                // atk speed is in frames, amplifying it would make the enemy attack slower, so we reduce it instead
                if (stat == this.m_characterStatFacade.AtkSpeed)
                {
                    var reducedValue = Mathf.RoundToInt(value / amplifier);
                    amplifiedStats[stat] = Mathf.Max(1, reducedValue);
                    continue;
                    
                }
                
                var amplifiedValue = Mathf.RoundToInt(value * amplifier);
                amplifiedStats[stat] = amplifiedValue;
            }

            var amplifiedPower = this.CalculatePower(amplifiedStats);
            var amplifiedBounty = this.CalculateBounty(amplifiedPower);
            amplifiedStats[this.m_characterStatFacade.Money] = Mathf.RoundToInt(amplifiedBounty);
            Debug.Log($"Amplified enemy {enemy.BaseData.Name} to power {amplifiedPower} with bounty {amplifiedBounty}");
            
            enemy.Stats.Clear();
            foreach (var (stat, value) in amplifiedStats)
            {
                enemy.Stats[stat] = value;
            }
            enemy.Power = Mathf.FloorToInt(amplifiedPower);
            enemy.SuperLiked = true;
        }

        private float GetDifficultyFactor()
        {
            var t = Mathf.Max(0, this.m_roundController.CurrentValue - 1);
            var round = 1f + 0.12f * t + 0.04f * t * t;
            var rizz = 1f + 0.25f * this.m_rizzController.CurrentValue;
            var d = round * rizz;
            Debug.Log($"Difficulty: {d}");
            return d;
        }

        private float SafeEvaluate(AnimationCurve animationCurve, float x, float fallback)
        {
            if (animationCurve is null || animationCurve.keys == null || animationCurve.keys.Length == 0)
            {
                return fallback;
            }
            
            return animationCurve.Evaluate(x);
        }

        private float CalculateBounty(float power)
        {
            
            var jitter = this.m_powerCalculationData.BountyJitter.Evaluate(UnityEngine.Random.value);
            var rndJitter = UnityEngine.Random.Range(-jitter, jitter);
            return this.m_powerCalculationData.BountyPerPower * power * (1f + rndJitter);
        }
        
        private float CalculatePower(Dictionary<CharacterStat, int> stats)
        {
            var power = 0f;
            foreach (var statWeight in this.m_powerCalculationData.StatWeights)
            {
                if (!stats.TryGetValue(statWeight.Stat, out var statValue))
                {
                    continue;
                }

                if (statWeight.Stat == this.m_characterStatFacade.Defense)
                {
                    power += Mathf.Lerp(statValue, statValue * 0.85f, 0.25f);                
                }
                else
                {
                    power += statWeight.Weight * statValue;
                }
            }

            return power;
        }
    }
}