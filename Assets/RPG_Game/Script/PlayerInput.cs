using UnityEngine;

namespace RPGGame
{
    public class PlayerInput : MonoBehaviour
    {
        // Inspector mein yahan Fixed Joystick drag karenge
        [SerializeField] private Joystick joystick;

        // Ye property doosri scripts (PlayerController) ko rasta batayegi
        public Vector3 MoveInput => new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // Check karne ke liye ke joystick hili hai ya nahi
        public bool IsMoveInput => MoveInput.magnitude > 0.1f;
    }
}