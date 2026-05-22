using UnityEngine;
using UnityEngine.AI;

namespace RPGGame
{
    public class BanditBehaviour : MonoBehaviour
    {
        [Header("Speed Settings")]
        public float patrolSpeed = 2.0f;
        public float chaseSpeed = 5.5f;

        [Header("Detection Settings")]
        public float detectionRadius = 10.0f; // Kitni door se dhoondega
        public float chaseLimit = 15.0f;     // "HOME" se kitni door tak peecha karega
        public float attackRange = 1.8f;

        public Transform[] waypoints;
        private int currentWaypointIndex = 0;
        private Vector3 spawnPoint; // Dushman ka "Ghar"

        private NavMeshAgent agent;
        private Animator anim;
        private float nextAttackTime = 0f;
        private float attackCooldown = 1.5f;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            spawnPoint = transform.position; // Game shuru hote hi "Ghar" yaad kar lo
        }

        void Update()
        {
            if (PlayerController.Instance == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);

            // --- NEW LEASH LOGIC ---
            // Check karein ke dushman apne "Spawn Point" se kitni door nikal gaya hai
            float distanceFromHome = Vector3.Distance(transform.position, spawnPoint);

            // CASE A: Agar player dushman ke ghar se boht door hai ya dushman khud hadd se baahir hai
            if (distanceFromHome > chaseLimit || distanceToPlayer > detectionRadius)
            {
                PatrolLevel(); // Wapis pehre par jao
            }
            // CASE B: Agar player attack range mein hai
            else if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            // CASE C: Agar player detection radius mein hai (aur dushman ghar ke kareeb hai)
            else
            {
                ChasePlayer();
            }

            if (anim != null) anim.SetFloat("ForwardSpeed", agent.velocity.magnitude);
        }

        void PatrolLevel()
        {
            if (waypoints == null || waypoints.Length < 2)
            {
                // Agar points nahi hain to wapis "Ghar" (Spawn Point) jao
                agent.SetDestination(spawnPoint);
                return;
            }
            agent.isStopped = false;
            agent.speed = patrolSpeed;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            if (!agent.pathPending && agent.remainingDistance < 0.6f)
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        void ChasePlayer()
        {
            agent.isStopped = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(PlayerController.Instance.transform.position);
        }

        void AttackPlayer()
        {
            agent.isStopped = true;
            // Face Player
            Vector3 lookDir = (PlayerController.Instance.transform.position - transform.position).normalized;
            lookDir.y = 0;
            if (lookDir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 0.2f);

            if (Time.time >= nextAttackTime)
            {
                if (anim != null) anim.SetTrigger("Attack");
                Health pHealth = PlayerController.Instance.GetComponent<Health>();
                if (pHealth != null) pHealth.TakeDamage(10);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}