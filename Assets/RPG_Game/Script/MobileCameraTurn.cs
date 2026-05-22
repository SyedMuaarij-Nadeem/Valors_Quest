using UnityEngine;
using Unity.Cinemachine;

namespace RPGGame
{
    public class MobileCameraTurn : MonoBehaviour
    {
        [Header("Settings")]
        public float sensitivityX = 0.15f;
        public float sensitivityY = 0.15f;

        private CinemachineOrbitalFollow m_OrbitalFollow;
        private int m_RightFingerId = -1;

        void Start()
        {
            // Yeh khud hi scene mein se CinemachineCamera dhoond kar us ka OrbitalFollow nikal legi
            CinemachineCamera virtualCam = Object.FindFirstObjectByType<CinemachineCamera>();

            if (virtualCam != null)
            {
                m_OrbitalFollow = virtualCam.GetComponent<CinemachineOrbitalFollow>();
            }

            if (m_OrbitalFollow == null)
            {
                Debug.LogError("MobileCameraTurn: Scene mein CinemachineCamera ya OrbitalFollow component nahi mila!");
            }
        }

        void Update()
        {
            if (m_OrbitalFollow == null) return;

            // ─── 1. MOBILE TOUCH LOGIC ───
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    // Sirf tab touch register karo jab finger screen ke RIGHT HALF par touch ho
                    if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2)
                    {
                        m_RightFingerId = touch.fingerId;
                    }

                    // Agar ye wahi right side wali finger hai jo move ho rahi hai
                    if (touch.fingerId == m_RightFingerId)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            // Camera rotation update karna
                            m_OrbitalFollow.HorizontalAxis.Value += touch.deltaPosition.x * sensitivityX;
                            m_OrbitalFollow.VerticalAxis.Value -= touch.deltaPosition.y * sensitivityY;
                        }

                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            m_RightFingerId = -1;
                        }
                    }
                }
            }

            // ─── 2. UNITY EDITOR MOUSE LOGIC (For Testing) ───
#if UNITY_EDITOR
            else if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x > Screen.width / 2)
                {
                    float mouseX = Input.GetAxis("Mouse X");
                    float mouseY = Input.GetAxis("Mouse Y");

                    m_OrbitalFollow.HorizontalAxis.Value += mouseX * sensitivityX * 30f;
                    m_OrbitalFollow.VerticalAxis.Value -= mouseY * sensitivityY * 30f;
                }
            }
#endif
        }
    }
}