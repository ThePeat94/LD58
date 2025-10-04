using System;
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
            
            var attackStat = this.m_ownEntityInformation.EntityStats[this.m_characterStatFacade.Attack];
            var healthStat = this.m_targetEntityInformation.EntityStats[this.m_characterStatFacade.Hp];

            if (attackStat is null || healthStat is null)
            {
                throw new InvalidOperationException("EntityAttacker missing required stats");
            }

            healthStat.UseResource(attackStat.CurrentValue);
            this.m_currentAttackFrame = this.m_attackFrames;
        }

        public void ResetAttack()
        {
            this.m_currentAttackFrame = this.m_attackFrames;
        }
    }
}