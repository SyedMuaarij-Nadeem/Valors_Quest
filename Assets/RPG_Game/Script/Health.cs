using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // YE LINE ZAROORI HAI: Slider ko control karne ke liye

namespace RPGGame
{
    public class Health : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;

        [Header("UI Reference")]
        public Slider healthBarSlider; // Inspector mein Muaarij wali bar yahan dalenge

        [Header("--- Audio ---")]
        public AudioClip deathSound;

        private Animator anim;
        private bool isDead = false;

        void Start()
        {
            currentHealth = maxHealth;
            anim = GetComponent<Animator>();

            // Shuru mein bar ko set karna
            if (healthBarSlider != null)
            {
                healthBarSlider.minValue = 0;
                healthBarSlider.maxValue = maxHealth;
                healthBarSlider.value = maxHealth;
            }
        }

        public void TakeDamage(int amount)
        {
            if (CompareTag("Player") && PlayerController.Instance != null && PlayerController.Instance.IsShieldActive)
            {
                Debug.Log("Player shielded! No damage taken.");
                return; // Yahan se return ho jaye ga, niche wali health kam karne ki logic nahi chalegi
            }
            if (isDead) return;

            currentHealth -= amount;
            Debug.Log(gameObject.name + " Health: " + currentHealth);

            // --- YE POINT SAB SE AHAM HAI ---
            // Is se aap ki bar har thapar (hit) ke baad thori thori niche giregi
            if (healthBarSlider != null)
            {
                healthBarSlider.value = currentHealth;
            }

            if (currentHealth <= 0) Die();
        }



        void Die()
        {
            if (isDead) return;
            isDead = true;

            if (anim != null) anim.SetTrigger("Die");

            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            if (gameObject.CompareTag("Player"))
            {
                // Player mar jaye to 2 sec baad restart
                Invoke("RestartGame", 2f);
            }
            else
            {
                // Dushman mar jaye to 3 sec baad destroy
                Destroy(gameObject, 3f);
            }
        }

        void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}