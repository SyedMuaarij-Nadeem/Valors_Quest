// using UnityEngine;
// using Unity.Cinemachine;

// namespace RPGGame
// {
//     public class CameraController : MonoBehaviour
//     {
//         [SerializeField] private CinemachineInputAxisController inputController;

//         void Update()
//         {
//             // Right Click (1) check karna
//             bool isRotating = Input.GetMouseButton(1);

//             if (inputController != null)
//             {
//                 // Ye line sab se aham hai: Mouse signal ko block ya allow karna
//                 inputController.enabled = isRotating;

//                 // Lock the cursor when rotating (Optional but Pro look)
//                 if (isRotating)
//                 {
//                     Cursor.lockState = CursorLockMode.Locked;
//                 }
//                 else
//                 {
//                     Cursor.lockState = CursorLockMode.None;
//                 }
//             }
//         }
//     }
// }


using UnityEngine;
using Unity.Cinemachine;

namespace RPGGame
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineInputAxisController m_InputController;

        void Start()
        {
            // Puraani scripts ko manually drag karne ki zaroorat nahi
            // Yeh khud hi scene mein se CinemachineInputAxisController dhoond legi
            m_InputController = Object.FindFirstObjectByType<CinemachineInputAxisController>();

            if (m_InputController == null)
            {
                Debug.LogWarning("CameraController: Scene mein CinemachineInputAxisController nahi mila!");
            }
        }

        void Update()
        {
            if (m_InputController == null) return;

            // 1. PC check: Right Click hold hai ya nahi
            bool isRotatingPC = Input.GetMouseButton(1);

            // 2. Mobile check: Kya screen par koi touch chal raha hai?
            bool isTouchingMobile = Input.touchCount > 0;

            // Agar PC par right click ho YA mobile par touch ho raha ho, 
            // to built-in InputController ko DISABLE kar do taake hamari script block na ho.
            if (isRotatingPC || isTouchingMobile)
            {
                m_InputController.enabled = false;
            }
            else
            {
                m_InputController.enabled = true;
            }

            // PC Editor ke liye Cursor settings
#if UNITY_EDITOR
            if (isRotatingPC)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
#endif
        }
    }
}