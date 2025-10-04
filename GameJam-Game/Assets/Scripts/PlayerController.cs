using Nidavellir.Input;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController s_instance;
        private static readonly int s_isWalkingHash = Animator.StringToHash("IsWalking");

        [SerializeField] private PlayerData m_playerData;

        private Vector3 m_moveDirection;
        private CharacterController m_characterController;
        private InputProcessor m_inputProcessor;
        private Animator m_animator;


        public static PlayerController Instance => s_instance;
    
    
        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        
            this.m_inputProcessor = this.GetComponent<InputProcessor>();
            this.m_characterController = this.GetComponent<CharacterController>();
            this.m_animator = this.GetComponent<Animator>();
        }
    
        // Update is called once per frame
        void Update()
        {
        }

        private void LateUpdate()
        {
            this.UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            this.m_animator.SetBool(s_isWalkingHash, this.m_moveDirection != Physics.gravity);
        }


    }
}
