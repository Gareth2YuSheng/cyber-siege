using UnityEngine;

public class VKBulletScript : BulletScript
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // If enemy is virus enemy do double damage
        if (collision.gameObject.tag == "Virus Enemy" && collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hasCollided)
        {
            hasCollided = true;
            VirusScript virus = collision.gameObject.GetComponent<VirusScript>();
            SpecialEffect(virus);
            Destroy(gameObject);
        }
        // Else If enemy is normal enemy do normal damage
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hasCollided)
        {
            base.OnCollisionEnter2D(collision); // Let parent logic handle
        }
    }

    protected virtual void SpecialEffect(VirusScript virus)
    {
        virus.TakeSpecialisedDamage(damage * 2);
    }
}
