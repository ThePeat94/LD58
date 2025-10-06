using System;
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

        public bool CanAttack
        {
            get;
            set;
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

        public void Initialize(EntityInformation ownEntityInformation, EntityInformation targetEntityInformation, CharacterStatFacade characterStatFacade)
        {
            this.m_ownEntityInformation = ownEntityInformation;
            this.m_targetEntityInformation = targetEntityInformation;
            this.m_characterStatFacade = characterStatFacade;
            
            this.m_attackFrames = this.m_ownEntityInformation.EntityStats[this.m_characterStatFacade.AtkSpeed]?.CurrentValue ?? 90;
            this.m_currentAttackFrame = this.m_attackFrames;
            
            this.m_ownEntityInformation.EntityStats[this.m_characterStatFacade.Hp].OnValueChanged += this.OnOwnHealthChanged;
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

            var extraAttack = 0;
            var relativeIncrease = 1f;
            if (this.m_playerUpgradeController is not null)
            {
                var enemyTags = this.m_targetEntityInformation.Tags;
                var upgrades = this.m_playerUpgradeController.PurchasedUpgrades;

                var affectTags = upgrades.Where(u => u.AffectedTags.Any(t => enemyTags.Contains(t))).ToList();

                foreach (var tag in affectTags)
                {
                    extraAttack += tag.AffectedStats
                        .Where(s => s.AffectedStat == this.m_characterStatFacade.Attack)
                        .Sum(s => s.IncreaseAmount);
                    
                    relativeIncrease += tag.AffectedStats
                        .Where(s => s.AffectedStat == this.m_characterStatFacade.Attack && s.RelativeIncreaseAmount > 1)
                        .Sum(s => 1 - s.RelativeIncreaseAmount);
                }
                Debug.Log($"Extra attack: {extraAttack}, relative increase: {relativeIncrease}");
            }
            
            var attackStat = this.m_ownEntityInformation.EntityStats[this.m_characterStatFacade.Attack];
            var healthStat = this.m_targetEntityInformation.EntityStats[this.m_characterStatFacade.Hp];
            var defenseStat = this.m_targetEntityInformation.EntityStats[this.m_characterStatFacade.Defense];

            if (attackStat is null || healthStat is null || defenseStat is null)
            {
                throw new InvalidOperationException("EntityAttacker missing required stats");
            }
            
            var totalAttack = (int)((extraAttack + attackStat.CurrentValue) * relativeIncrease);
            var damage = Math.Max(1, totalAttack - defenseStat.CurrentValue);

            healthStat.UseResource(damage);
            this.m_currentAttackFrame = this.m_attackFrames;
        }

        public void ResetAttack()
        {
            this.m_currentAttackFrame = this.m_attackFrames;
        }
    }
}