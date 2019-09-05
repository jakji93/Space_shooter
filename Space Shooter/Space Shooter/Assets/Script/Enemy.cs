using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject laser;
    [SerializeField] float projectileSpeed = 6f;

    [Header("Visual")]
    [SerializeField] GameObject VFXEffect;
    [SerializeField] float VFXDuration = 1f;

    [Header("Sound")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathVolume = 0.7f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserVolume = 0.25f;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        var laserObject = Instantiate(laser, transform.position, Quaternion.identity);
        laserObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        damageDealer.Hit();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        if (health <= 100)
        {
            EnemyDestroied();
        }
    }

    private void EnemyDestroied()
    {
        Destroy(gameObject);
        var effect = Instantiate(VFXEffect, transform.position, Quaternion.identity);
        Destroy(effect, VFXDuration);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
        gameSession.AddToScore(scoreValue);
    }
}
