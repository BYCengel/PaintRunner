using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TestPlayer : MonoBehaviour
{

    [SerializeField] private GameObject dmgIndicatorUI;

    [SerializeField] private LayerMask platformsLayerMask;
    [SerializeField] private Score scoreText;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Transform backPoint;
    [SerializeField] private Transform frontPoint;
    private AudioManager audioManager;

    // Components
    private Rigidbody2D rigidBody2D;
    private Animator playerAnimator;
    private CapsuleCollider2D capsuleCollider2D;
    private Transform _transform;
    
    // Start End değişkenleri
    private bool started = false;
    private bool death = false;
    private bool notEnded = true;
    
    // Speed değişkenleri
    private float moveSpeed = 6f;
    [SerializeField] private float normalSpeed = 6f; //normal speed
    [SerializeField] private float slowSpeed = 1f; //normal speedin 1/6sı
    [SerializeField] private float fastSpeed = 9f; //normal speedin 3/2si
    [SerializeField] float movementSpeedScalingFactor = 0.005f;
    private CameraMovement cameraMovement;
    private int previousScore;

    private bool running = false;
      
    // Zıplama gücü
    private float jumpvelocity = 12f;

    // Skor
    private int score = 0;

    //Healthbar parameters
    private float timer;
    private float timeSinceLastRegen;
    
    // Air timer
    private bool air = false;
    private float onAirTime;

    [SerializeField] float regenAmount = 4f;
    //[SerializeField] float healthRegenerationCooldown = .3f;

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;

    //GameManager
    [SerializeField] private GameManager gameManager;
    
    // Spike hasarı
    private const int spikeDamage = 25;

    void Awake()
    {
        rigidBody2D = transform.GetComponent<Rigidbody2D>();
        capsuleCollider2D = transform.GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponentInChildren<Animator>();
        _transform = transform.GetComponent<Transform>();
        dmgIndicatorUI.SetActive(false);
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        healthBar = FindObjectOfType<HealthBar>();
        gameManager = FindObjectOfType<GameManager>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        
    }
    
    void Update()
    {
        if (!death)
        {
            // Zıplamak için 'W' basılır
            if(IsGrounded() && Input.GetKeyDown(KeyCode.W))
            {
                if (!running)
                {
                    int j = Random.Range(1, 5);
                    string jump = "jump" + j.ToString();
                    audioManager.Play(jump);
                }
                rigidBody2D.velocity = Vector2.up * jumpvelocity;
            }

            // Başlatmak için 'SPACE' basılır
            if (!started && Input.GetKeyDown(KeyCode.Space))
            {
                playerAnimator.SetBool("StartRunning", true);
                started = true;
                gameManager.CloseStartText();
            }
            
            // HEHE
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.OpenEscapeText();
            }
        
            // Hızlı yavaş koşma
            if (CheckLeft() && Input.GetKeyDown(KeyCode.A))// sol
            {
                running = true;
                audioManager.Play("slowrun");
            }
            else if (CheckLeft() && Input.GetKey(KeyCode.A))
            {
                moveSpeed = slowSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                running = false;
                audioManager.Stop("slowrun");
                moveSpeed = normalSpeed;
            }
            else if (CheckRight() && Input.GetKeyDown(KeyCode.D)) // sağ
            {
                running = true;
                audioManager.Play("fastrun");
            }
            else if (CheckRight() && Input.GetKey(KeyCode.D))
            {
                moveSpeed = fastSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                running = false;
                audioManager.Stop("fastrun");
                moveSpeed = normalSpeed;
            }
            ScaleMovementSpeeds();
        }
        
        //animations
        if (IsGrounded())
        {
            /*if (rigidBody2D.velocity.x == 0)
            { // idle
                playerAnimator.SetBool("jumping", false);
                playerAnimator.SetBool("running", false);
                playerAnimator.SetBool("idle", true);
                
            }
            else
            { // normal koşma
                playerAnimator.SetBool("jumping", false);
                playerAnimator.SetBool("running", true);
                playerAnimator.SetBool("idle", false);
            }*/

            if (moveSpeed == normalSpeed)
            {
                playerAnimator.SetFloat("RunSpeed", 6f);
            }
            else if (moveSpeed < normalSpeed)
            {
                playerAnimator.SetFloat("RunSpeed", 3f);
            }
            else if (moveSpeed > normalSpeed)
            {
                playerAnimator.SetFloat("RunSpeed", 8f);
            }
        }
        else
        { //zıplama
            
            
            playerAnimator.SetFloat("RunSpeed", 0f);
            if (rigidBody2D.velocity.y > 0.1f)
            {
                //yukarı
                playerAnimator.SetTrigger("Jump");
            }
            else if (rigidBody2D.velocity.y < -0.1f)
            {
                //aşşa
                playerAnimator.SetTrigger("Fall");
            }
            //playerAnimator.SetBool("jumping", true);
        }
        
        // Sürekli ileri hareket
        if (started)
        {
            //Debug.Log("HIZ = " + moveSpeed);
            Movement();
        }


        //if(timer - timeSinceLastRegen >= healthRegenerationCooldown)//regen after a set amount of time
        
        if(!death && currentHealth <= maxHealth){ //ölü değilse regenle
            healthBar.IncreaseHealth(regenAmount * Time.deltaTime);
            currentHealth += regenAmount * Time.deltaTime;
        }

        // Uzun süre havada kalındığında yere düşünce ses çıklarır
        if (!IsGrounded() && !air)
        {
            playerAnimator.SetBool("Land", false);
            onAirTime = Time.time;
            air = true;
        }
        else if(IsGrounded() && air)
        {
            playerAnimator.SetBool("Land", true);
            if (!running &&(Time.time - onAirTime) > 0.8f)
            {
                int d = Random.Range(1, 4);
                string drop = "drop" + d.ToString();
                audioManager.Play(drop);
            }
            air = false;
        }
    }

    private bool CheckLeft() // Sol tarafa gitme
    {
        float twoPoint = Vector3.Distance(backPoint.position, frontPoint.position);
        float toPlayer = Vector3.Distance(frontPoint.position, transform.position);
        if (twoPoint < toPlayer)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckRight() // Sağ tarafa gitme
    {
        float twoPoint = Vector3.Distance(frontPoint.position, backPoint.position);
        float toPlayer = Vector3.Distance(backPoint.position, transform.position);
        if (twoPoint < toPlayer)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    private void Movement() // İleri hareket
    {
        rigidBody2D.velocity = new Vector2(+moveSpeed, rigidBody2D.velocity.y);
        
        // Karakteri istenilen şekilde hareket ettirmek için burası commentden çıkarılıp üstteki satır commente alınır
        /*if (Input.GetKey(KeyCode.A))
        {
            rigidBody2D.velocity = new Vector2(-moveSpeed, rigidBody2D.velocity.y);
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                rigidBody2D.velocity = new Vector2(+moveSpeed, rigidBody2D.velocity.y);
            }
            else
            {
                //no keys pressed
                rigidBody2D.velocity = new Vector2(0, rigidBody2D.velocity.y);
            }
        }*/
    }
    private bool IsGrounded() // Karakterin sadece bir kere zıplaması için yere değip değmediği kontrol edilir
    {
        RaycastHit2D rayCastHit2D = Physics2D.BoxCast(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.size, 0f, Vector2.down, .5f, platformsLayerMask);
        return rayCastHit2D.collider != null;
    }

    public void Damage(float damage)
    {
        StartCoroutine(TriggerDmgIndicator());
        int d = Random.Range(1, 4);
        string dam = "damage" + d.ToString();
        audioManager.Play(dam);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0){
            death = true;
            Dead();
        }

    }

    private IEnumerator TriggerDmgIndicator()
    {
        dmgIndicatorUI.SetActive(true);
        yield return new WaitForSeconds(.1f);
        dmgIndicatorUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider2D) // Kameranın dışına çıkılmasınu kontrol eder. Çıkıldığında oyuncu ölür
    {
        if (collider2D.transform.tag == "Death" && !death)
        {
            StartCoroutine(TriggerDmgIndicator());
            death = true;
            Dead();
        }
    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {
        if (collider2D.transform.tag == "Spike" && !collider2D.transform.GetComponent<Spike>().GetUsed())
        {
            Damage(spikeDamage);
            collider2D.transform.GetComponent<Spike>().SetUsed(true);
        }
    }

    public Vector3 GetPosition()
    {
        return _transform.position;
    }

    public void Dead()
    {
        gameManager.SetIsDead(true);
        int d = Random.Range(1, 4);
        string dead = "death" + d.ToString();
        audioManager.Play(dead);
        Debug.Log("dead fonksiyonu");
        death = true;
        gameManager.ManageHighestScore();
    }

    private void ScaleMovementSpeeds(){
        
        float change = (gameManager.GetCurrentScore() * movementSpeedScalingFactor) - previousScore * movementSpeedScalingFactor;
        normalSpeed += change;
        slowSpeed = slowSpeed + (change/6);
        fastSpeed = fastSpeed + (change* 1.5f);
        cameraMovement.SetMoveSpeed(normalSpeed);
        previousScore = gameManager.GetCurrentScore();
        Debug.Log("ratios");
        Debug.Log("normal speed / slow speed = " + normalSpeed/slowSpeed);
        Debug.Log("fast speed / normal speed = " + fastSpeed/normalSpeed);
        Debug.Log("normal speed = " + normalSpeed + " camera speed = " + cameraMovement.GetCurrentSpeed());
    }

    public float GetMaxHealth(){
        return maxHealth;
    }

    public float GetCurrentHealth(){
        return currentHealth;
    }

    public bool GetStarted()
    {
        return started;
    }

    public bool GetDeath()
    {
        return death;
    }
}
