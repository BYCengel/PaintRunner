using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireBullet : MonoBehaviour
{
    [SerializeField] private int side;
    [SerializeField] private Transform bullet;

    [SerializeField] private bool onSide;
    private Vector3 startingPosition;
    [SerializeField] private float movementLimit = 4f;
    private float movementSpeed = 5f;
    [SerializeField] private bool isMovingUp;
    [SerializeField] private bool isMovingRight;
    [SerializeField] private bool isOnRight;
    private bool doesNeedToMove;

    [SerializeField] private Transform cameraOrigin;
    [SerializeField] private float randomizationFactorForBullets = .5f;
    [SerializeField] private float onSideSpeedModifier = 3f;

    [SerializeField] private float minBulletSpawnTime = 1f;
    [SerializeField] private float maxBulletSPawnTime = 4f;

    [SerializeField] private float currentMinBulletSpawnTime = 2f;
    [SerializeField] private float currentMaxBulletSpawnTime = 8f;

    [SerializeField] private float bulletSpawnTimeScalingFactor = 0.005f;
    private GameManager gameManager;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        cameraOrigin = transform.parent.Find("Origin");
        startingPosition = transform.position;
        NextBullet();
    }
    private void Update() {
        Move();
    }

    private void BulletFire()
    {
        Fire();
        NextBullet();
    }
    
    private Vector3 CalculateShootingDirection(){
        Vector3 shootingDirection = (cameraOrigin.position - transform.position).normalized;
        Vector3 randomizedShootingDirection = new Vector3(shootingDirection.x +
            Random.Range(-randomizationFactorForBullets, randomizationFactorForBullets),
            shootingDirection.y + Random.Range(-randomizationFactorForBullets, randomizationFactorForBullets),
            shootingDirection.z + Random.Range(-randomizationFactorForBullets, randomizationFactorForBullets));
        return randomizedShootingDirection;
    }//mermi başına bir kere çağırmak lazım

    private float CalculateShootingAngle(Vector3 shootingDirection){
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        return angle;
    }//mermi başına bir kere çağırmak lazım

    private void Fire(){
        Transform instantiatedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector3 thisShootingDirection = CalculateShootingDirection();
        instantiatedBullet.GetComponent<Bullet>().Setup(thisShootingDirection, CalculateShootingAngle(thisShootingDirection) + 180);
        if(onSide && isOnRight){//sağdaysa
            float oldSpeed = instantiatedBullet.GetComponent<Bullet>().GetBulletSpeed();
            instantiatedBullet.GetComponent<Bullet>().SetBulletSpeed(oldSpeed - onSideSpeedModifier);
        }else if(onSide && !isOnRight){//soldaysa
            float oldSpeed = instantiatedBullet.GetComponent<Bullet>().GetBulletSpeed();
            instantiatedBullet.GetComponent<Bullet>().SetBulletSpeed(oldSpeed + onSideSpeedModifier);
        }
    }

    private void Move(){
        if(onSide){

            if(Mathf.Abs(Vector3.Distance(transform.position, startingPosition)) < movementLimit || doesNeedToMove){
                if(isMovingUp){
                    transform.position = new Vector2(transform.position.x, transform.position.y +
                 movementSpeed * Time.deltaTime);
                 doesNeedToMove = false;
                }else{
                    transform.position = new Vector2(transform.position.x, transform.position.y -
                 movementSpeed * Time.deltaTime);
                 doesNeedToMove = false;
                }
            }else{
                if(transform.position.y > startingPosition.y){//eğer üstündeyse
                    isMovingUp = false;
                    doesNeedToMove = true;
                }else{
                    isMovingUp = true;
                    doesNeedToMove = true;
                }
            }

        }else{

            if(Mathf.Abs(Vector3.Distance(transform.position, startingPosition)) < movementLimit || doesNeedToMove){
                if(isMovingRight){
                    transform.position = new Vector2(transform.position.x + movementSpeed * Time.deltaTime,
                 transform.position.y);
                 doesNeedToMove = false;
                }else{
                    transform.position = new Vector2(transform.position.x - movementSpeed * Time.deltaTime,
                 transform.position.y);
                 doesNeedToMove = false;
                }
            }else{
                if(transform.position.x > startingPosition.x){//eğer sağındaysa
                    isMovingRight = false;
                    doesNeedToMove = true;
                }else{
                    isMovingRight = true;
                    doesNeedToMove = true;
                }
            }

        }
    }

    private void NextBullet()
    {
        float rand = Random.Range(2f, 6f);
        Invoke("BulletFire",rand);
        ScaleBulletSpawnSpeed();
    }

    private void ScaleBulletSpawnSpeed(){
        if(currentMinBulletSpawnTime > minBulletSpawnTime){
            currentMinBulletSpawnTime -= bulletSpawnTimeScalingFactor * gameManager.GetCurrentScore();
        }
        if(currentMaxBulletSpawnTime > maxBulletSPawnTime){
            currentMaxBulletSpawnTime -= bulletSpawnTimeScalingFactor * gameManager.GetCurrentScore();
        }
    }

//LEGACY CODES

    /*if (side == 0)
        {
            Transform bulletTransform = Instantiate(bullet, transform.position, Quaternion.Euler(0,0,-90));
            Vector3 shootDir = Vector3.up;
            bulletTransform.GetComponent<Bullet>().Setup(shootDir);
        }
        else if (side == 1)
        {
            Transform bulletTransform = Instantiate(bullet, transform.position, Quaternion.Euler(0,0,-270));
            Vector3 shootDir = Vector3.down;
            bulletTransform.GetComponent<Bullet>().Setup(shootDir);
        }
        else if (side == 2)
        {
            Transform bulletTransform = Instantiate(bullet, transform.position, Quaternion.identity);
            Vector3 shootDir = Vector3.left;
            bulletTransform.GetComponent<Bullet>().Setup(shootDir);
        }
        else if (side == 3)
        {
            Transform bulletTransform = Instantiate(bullet, transform.position, Quaternion.Euler(0,0,180));
            Vector3 shootDir = Vector3.right;
            bulletTransform.GetComponent<Bullet>().Setup(shootDir);
        }*/
    
    
}
