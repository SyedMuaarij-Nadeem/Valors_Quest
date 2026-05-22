

using UnityEngine;
using UnityEngine.UI; // UI elements ke liye zaroori hai

namespace RPGGame
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController s_Instance;
        public static PlayerController Instance => s_Instance;

        [Header("Movement & Rotation")]
        public float speed = 5f;
        public float rotationSpeed = 600f;
        public float gravity = 20f;

        [Header("--- Audio ---")]
        public AudioClip attackSound;
        public AudioClip shieldSound;

        [Header("--- Shield Settings ---")]
        [Tooltip("Drag the 'ShieldGlobe' child object here")]
        public GameObject shieldGlobe;
        [Tooltip("Drag the UI Shield Button here")]
        public Button shieldButton;
        public float shieldDuration = 5f; // Kitni der shield active rahegi

        private CharacterController m_ChController;
        private PlayerInput m_PlayerInput;
        private Animator m_Animator;
        private float m_VerticalVelocity;
        private Transform currentTarget;

        // State variables for shield
        private bool m_IsShieldActive = false;
        public bool IsShieldActive => m_IsShieldActive; // Doosri scripts se read karne ke liye

        void Awake()
        {
            s_Instance = this;
            m_ChController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
        }

        void Start()
        {
            // Globe ko shuru mein hide rakhna hai
            if (shieldGlobe != null) shieldGlobe.SetActive(false);

            // Button par click listener lagana
            if (shieldButton != null)
            {
                shieldButton.onClick.AddListener(ActivateShield);
            }
        }

        void Update()
        {
            if (m_PlayerInput == null) return;

            Vector3 input = m_PlayerInput.MoveInput;

            if (m_ChController.isGrounded) m_VerticalVelocity = -2f;
            else m_VerticalVelocity -= gravity * Time.deltaTime;

            Vector3 moveDir = Vector3.zero;

            if (input.magnitude > 0.1f)
            {
                Vector3 camForward = Camera.main.transform.forward;
                camForward.y = 0;
                moveDir = Quaternion.LookRotation(camForward.normalized) * input.normalized;

                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            Vector3 finalMove = moveDir * speed;
            finalMove.y = m_VerticalVelocity;
            m_ChController.Move(finalMove * Time.deltaTime);

            if (m_Animator != null)
            {
                m_Animator.SetFloat("ForwardSpeed", input.magnitude * speed);
            }
        }

        // --- SHIELD LOGIC ---
        public void ActivateShield()
        {

            if (m_IsShieldActive) return; // Agar pehle se chal raha hai to dubara kaam na kare

            if (shieldSound != null)
            {
                AudioSource.PlayClipAtPoint(shieldSound, transform.position);
            }
            StartCoroutine(ShieldRoutine());
        }

        private System.Collections.IEnumerator ShieldRoutine()
        {
            m_IsShieldActive = true;
            if (shieldGlobe != null) shieldGlobe.SetActive(true);
            if (shieldButton != null) shieldButton.interactable = false; // Cooldown ke liye button disable

            Debug.Log("Shield Activated!");

            // Jitni duration set hai, utni der wait karega
            yield return new WaitForSeconds(shieldDuration);

            m_IsShieldActive = false;
            if (shieldGlobe != null) shieldGlobe.SetActive(false);
            if (shieldButton != null) shieldButton.interactable = true; // Dubara use ke liye enable

            Debug.Log("Shield Expired!");
        }

        // --- ATTACK FUNCTION ---
        public void PerformAttack()
        {
            FindNearestEnemy();

            if (attackSound != null)
            {
                AudioSource.PlayClipAtPoint(attackSound, transform.position);
            }

            if (currentTarget != null)
            {
                Vector3 dir = (currentTarget.position - transform.position).normalized;
                dir.y = 0;
                if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);

                float dist = Vector3.Distance(transform.position, currentTarget.position);
                if (dist <= 2.5f)
                {
                    Health h = currentTarget.GetComponent<Health>();
                    if (h != null) h.TakeDamage(25);
                }
            }

            if (m_Animator != null) m_Animator.SetTrigger("Attack");
        }

        void FindNearestEnemy()
        {
            Collider[] hit = Physics.OverlapSphere(transform.position, 6f);
            float minD = Mathf.Infinity;
            currentTarget = null;
            foreach (Collider c in hit)
            {
                if (c.CompareTag("Enemy"))
                {
                    float d = Vector3.Distance(transform.position, c.transform.position);
                    if (d < minD) { minD = d; currentTarget = c.transform; }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + transform.forward * 1.5f, 2f);
        }
    }
}