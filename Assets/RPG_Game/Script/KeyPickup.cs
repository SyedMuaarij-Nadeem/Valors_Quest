using UnityEngine;

namespace RPGGame
{
    public class KeyPickup : MonoBehaviour
    {
        private Level1Controller levelController;
        private bool isCollected = false;

        public AudioClip pickupSound;

        void Start()
        {
            levelController = FindFirstObjectByType<Level1Controller>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isCollected) return;

            if (other.CompareTag("Player"))
            {
                isCollected = true;

                // UI + counter update
                if (levelController != null)
                {
                    levelController.OnKeyCollected();
                }

                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                Destroy(gameObject);

                Debug.Log("Key Collected!");
            }
        }
    }
}