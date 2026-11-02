using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarryableObj : MonoBehaviour
{
    public Rigidbody2D Rigidbody {get; private set;}

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
