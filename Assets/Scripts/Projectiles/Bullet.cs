using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;

    private void Awake()
    {
        moveSpeed = 15f;
    }

    public void Setup(Vector3 shootDir, float shootAngle)
    {
        transform.eulerAngles = new Vector3(0, 0, shootAngle);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(shootDir * moveSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Draw")
        {
            particle.Play();
            // mermiler çizime çarpınca yok olur
            Destroy(gameObject);
        }
        TestPlayer player = collider.GetComponent<TestPlayer>();
        if (player != null)
        {
            particle.Play();
            //hedefe vurdu
            player.Damage(damage);
            Destroy(gameObject);
        }
    }

    public void SetBulletSpeed(float value){
        moveSpeed = value;
    }

    public float GetBulletSpeed(){
        return moveSpeed;
    }
}
