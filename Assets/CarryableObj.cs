using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarryableObj : MonoBehaviour
{
    public Rigidbody2D Rigidbody {get; private set;}
    public bool IsThrown { get; private set; }



    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetThrown(bool thrown)
    {
        IsThrown = thrown;
    }
}
