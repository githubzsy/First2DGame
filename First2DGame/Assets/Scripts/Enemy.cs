using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D Rigidbody2D;

    protected Collider2D Collider2D;

    protected Animator Animator;

    /// <summary>
    /// 死亡的音效
    /// </summary>
    protected AudioSource DeathAudioSource;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<CircleCollider2D>();
        DeathAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Death()
    {
        Collider2D.enabled = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// 被攻击了
    /// </summary>
    public void Attacked()
    {
        DeathAudioSource.Play();
        Animator.SetTrigger("death");
    }
}
