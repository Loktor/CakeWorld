using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muncher : GravityBody
{
    AudioSource audioSource;
    public List<AudioClip> eatingSounds = new List<AudioClip>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("WorldJumpingArea"))
        {
            AudioSource.PlayClipAtPoint(eatingSounds[Random.Range(0, eatingSounds.Count)], transform.position);
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Cake"))
        {
            Physics2D.IgnoreCollision(coll.collider, GetComponent<Collider2D>());
        }
    }

}
