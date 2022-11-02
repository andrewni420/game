using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour, Damageable
{
    public enum intelligence
    {
        pos, vel, none
    }

    public AI enemyAI;
    protected Trajectory trajectory;
    protected State state;
    private float maxSpeed = 1F;
    private int health = 100;
    private int maxHealth = 100;
    private int damage = 20;
    private int knockback = 5;
    private float hitCooldown = 0;
    public intelligence intLevel;
    private Vector3 moveDirection;
    // private bool isInAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        state = new State();
        trajectory = new Trajectory(this.gameObject,maxSpeed);
        moveDirection = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
        trajectory.update();

        Vector3[] playerTrajectory = enemyAI.imputeTrajectory();
        

        Vector3[] nextMove = getNextMove(playerTrajectory);
        Debug.Log(nextMove[0]);
        if (nextMove[0] != moveDirection)
        {
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = hitCooldown == 0 ? nextMove[0] : transform.position;
            moveDirection = nextMove[0];
        }

        hitCooldown = Math.Max(hitCooldown - Time.fixedDeltaTime, 0);

        //transform.position += nextMove[0];
        transform.Rotate(nextMove[1]);

        // ////////////Kelly edit here///////////
        // did not have time to do anything with,
        // worked on making enemy disappear instead
        // ///
        // float random = UnityEngine.Random.Range(0F, 100F);
        // if (random < 25)
        // {
        //     float hi = 1;
        //     ///25% chance each fixedUpdate to play hit animation. Adjust as necessary
        // }

    }

    public int getHealth()
    {
        return health;
    }
    public void setHealth(int health)
    {
        this.health = health;
    }
    public void enforceMaxHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    

    public abstract Vector3[] getNextMove(Vector3[] position);
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerWeapon")
        {
            takeDamage(10);
        }
            
            // in future, access damage from other
            // hitAnimation();
    }

    // public void hitAnimation()
    // {
    //     isInAnimation = true;
    // }
    public void setHitCooldown(float time)
    {
        hitCooldown = time;
    }
    public void takeDamage(int damage)
    {
        health -= damage;
        checkHealth();
    }
    public void checkHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int getDamage()
    {
        return damage;
    }
    public int getKnockback()
    {
        return knockback;
    }
    
}