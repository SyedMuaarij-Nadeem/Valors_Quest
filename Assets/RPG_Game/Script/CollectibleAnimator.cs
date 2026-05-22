using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    // [Header("Rotation Settings")]
    // [SerializeField] private float rotationSpeed = 100f;
    // [SerializeField] private Vector3 rotationAxis = Vector3.up; // (0, 1, 0) yani Y-axis par ghumega

    // [Header("Hover Settings")]
    // [SerializeField] private bool enableHover = true;
    // [SerializeField] private float hoverSpeed = 2f;      // Floating ki speed
    // [SerializeField] private float hoverAmount = 0.2f;    // Kitna upar/neeche jaye

    // private Vector3 startPosition;

    // void Start()
    // {
    //     // Object ki shuruati position save kar rahe hain
    //     startPosition = transform.localPosition;
    // }

    // void Update()
    // {
    //     // 1. Rotation Effect
    //     // Time.deltaTime lagane se rotation har computer par smoothly aik jaisi chalegi
    //     transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);

    //     // 2. Hover (Floating) Effect
    //     if (enableHover)
    //     {
    //         float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
    //         transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    //     }
    // }


    [Header("Subtle Floating Settings")]
    [SerializeField] private float floatSpeed = 1.5f;   // Kitni speed se upar/neeche move kare (Ziyada tez nahi)
    [SerializeField] private float floatAmount = 0.1f;  // Kitna upar aur neeche jaye (0.1 bohot subtle hai)

    private Vector3 startPosition;

    void Start()
    {
        // Object ki shuruati position save kar rahe hain
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // Sirf Floating (Up/Down) Effect
        // Mathf.Sin se movement bohot smoothly wave ki tarah hoti hai
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}