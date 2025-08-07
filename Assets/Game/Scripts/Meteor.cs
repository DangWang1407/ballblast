using UnityEngine;
using TMPro;

public class Meteor : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected int health;

    [SerializeField] protected TMP_Text textHealth;
    [SerializeField] protected float jumpForce;


    private void Start()
    {
        UpdateUI();
        rb.velocity = Vector2.right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Cannon"))
        {
            
        }

        if (other.tag.Equals("Missile"))
        {
            TakeDamage(1);
            Missiles.Instance.DestroyMissile(other.gameObject);
        }

        if (other.tag.Equals("Walls"))
        {
            float posX = transform.position.x;
            if (posX > 0)
            {
                rb.AddForce(Vector2.left * 8f, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.right * 8f, ForceMode2D.Impulse);
            }
        }
        if (other.tag.Equals("Ground"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        textHealth.text = health.ToString();
        if (health <= 0)
        {
            Die();
        }
        UpdateUI();
    }

    void Die()
    {
        // Handle the death of the meteor, e.g., play an animation, destroy the object, etc.
        Destroy(gameObject);
    }

    private void UpdateUI()
    {
        textHealth.text = health.ToString();
    }
}