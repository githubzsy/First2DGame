using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D Rigidbody2D;

    protected Collider2D Collider2D;

    protected Animator Animator;
    
    [Tooltip("死亡时的效果")]
    public GameObject DeathPrefab;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 被攻击了
    /// </summary>
    public void Attacked()
    {
        Instantiate(DeathPrefab, transform.position, Quaternion.identity);
        SoundManager.EnemyDeadAudio();
        Destroy(gameObject);
    }
}
