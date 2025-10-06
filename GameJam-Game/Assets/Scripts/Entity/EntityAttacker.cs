using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.EventArgs;
using Nidavellir.Player;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Entity
{
    public class EntityAttacker : MonoBehaviour
    {
        [SerializeField] private EntityInformation m_ownEntityInformation;
        private EntityInformation m_targetEntityInformation;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private PlayerUpgradeController m_playerUpgradeController;
        

        private int m_attackFrames = 90;
        private int m_currentAttackFrame = 90;

        private EntityMode m_entityMode = EntityMode.Player;

        public bool CanAttack
        {
            get;
            set;
        }
        
        public int AttackFrames => this.m_attackFrames;
        public int CurrentAttackFrame => this.m_currentAttackFrame;

        private void Awake()
        {
            if (this.m_playerUpgradeController is null)
            {
                this.m_playerUpgradeController = FindFirstObjectByType<PlayerUpgradeController>(FindObjectsInactive.Include);
            }
        }

        private void FixedUpdate()
        {
            if (!this.CanAttack)
            {
                return;
            }
            
            if (this.m_currentAttackFrame > 0)
            {
                this.m_currentAttackFrame--;
                return;
            }
            
            this.Attack();
        }

        public void Initialize(EntityInformation ownEntityInformation, EntityInformation targetEntityInformation, CharacterStatFacade characterStatFacade, EntityMode entityMode)
        {
            this.m_ownEntityInformation = ownEntityInformation;
            this.m_targetEntityInformation = targetEntityInformation;
            this.m_characterStatFacade = characterStatFacade;
            
            this.m_attackFrames = this.m_ownEntityInformation.EntityStats[this.m_characterStatFacade.AtkSpeed]?.CurrentValue ?? 90;
            this.m_currentAttackFrame = this.m_attackFrames;
            
            this.m_ownEntityInformation.EntityStats[this.m_characterStatFacade.Hp].OnValueChanged += this.OnOwnHealthChanged;
            this.m_entityMode = entityMode;
        }

        private void OnOwnHealthChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            if (e.NewValue > 0)
                return;
            
            this.CanAttack = false;
        }

        public void SetTarget(EntityInformation targetEntityInformation)
        {
            this.m_targetEntityInformation = targetEntityInformation;
        }

        private void Attack()
        {
            if (this.m_ownEntityInformation is null || this.m_targetEntityInformation is null)
            {
                throw new InvalidOperationException("EntityAttacker not initialized");
            }
            
            var extraAttackInfo = this.CalculateExtraAttack();
            var extraDefenseInfo = this.CalculateExtraDefense();
            
            var attackStat = this.m_ownEntityInformation.EntityStats[this.m_characterStatFacade.Attack];
            var healthStat = this.m_targetEntityInformation.EntityStats[this.m_characterStatFacade.Hp];
            var defenseStat = this.m_targetEntityInformation.EntityStats[this.m_characterStatFacade.Defense];

            if (attackStat is null || healthStat is null || defenseStat is null)
            {
                throw new InvalidOperationException("EntityAttacker missing required stats");
            }
            
            var totalAttack = (int)((extraAttackInfo.AbsoluteIncrease + attackStat.CurrentValue) * extraAttackInfo.RelativeIncrease);
            var totalDefense = (int)((extraDefenseInfo.AbsoluteIncrease + defenseStat.CurrentValue) * extraDefenseInfo.RelativeIncrease);
            var damage = Math.Max(1, totalAttack - totalDefense);

            healthStat.UseResource(damage);
            this.m_currentAttackFrame = this.m_attackFrames;
        }

        public void ResetAttack()
        {
            this.m_currentAttackFrame = this.m_attackFrames;
        }

        private ExtraStatInfo CalculateExtraAttack()
        {
            var extraAttack = 0;
            var relativeIncrease = 1f;
            if (this.m_playerUpgradeController is not null && this.m_entityMode == EntityMode.Player)
            {
                var enemyTags = this.m_targetEntityInformation.Tags;
                var upgrades = this.m_playerUpgradeController.PurchasedUpgrades;

                var affectTags = upgrades.Where(u => u.AffectedTags.Any(t => enemyTags.Contains(t))).ToList();
                var result = this.ExtractExtraStats(affectTags, this.m_characterStatFacade.Attack);
                return result;
            }
            
            return new ExtraStatInfo(extraAttack, relativeIncrease);
        }
        
        private ExtraStatInfo CalculateExtraDefense()
        {
            var extraDefense = 0;
            var relativeIncrease = 1f;
            if (this.m_playerUpgradeController is not null && this.m_entityMode == EntityMode.Enemy)
            {
                var enemyTags = this.m_ownEntityInformation.Tags;
                var upgrades = this.m_playerUpgradeController.PurchasedUpgrades;

                var affectTags = upgrades.Where(u => u.AffectedTags.Any(t => enemyTags.Contains(t))).ToList();
                var result = this.ExtractExtraStats(affectTags, this.m_characterStatFacade.Defense);
                return result;
            }
            
            return new ExtraStatInfo(extraDefense, relativeIncrease);
        }

        private ExtraStatInfo ExtractExtraStats(List<UpgradeData> upgrades, CharacterStat targetStat)
        {
            var absoluteExtra = 0;
            var relativeIncrease = 1f;
            foreach (var tag in upgrades)
            {
                absoluteExtra += tag.AffectedStats
                    .Where(s => s.AffectedStat == this.m_characterStatFacade.Defense)
                    .Sum(s => s.IncreaseAmount);
                    
                relativeIncrease += tag.AffectedStats
                    .Where(s => s.AffectedStat == this.m_characterStatFacade.Defense && s.RelativeIncreaseAmount > 1)
                    .Sum(s => 1 - s.RelativeIncreaseAmount);
            }
            
            return new ExtraStatInfo(absoluteExtra, relativeIncrease);
        }

        private class ExtraStatInfo
        {
            public int AbsoluteIncrease { get; }
            
            public float RelativeIncrease { get; }
            
            public ExtraStatInfo(int absoluteIncrease, float relativeIncrease)
            {
                this.AbsoluteIncrease = absoluteIncrease;
                this.RelativeIncrease = relativeIncrease;
            }
        }

        public enum EntityMode
        {
            Player,
            Enemy
        }
    }
}