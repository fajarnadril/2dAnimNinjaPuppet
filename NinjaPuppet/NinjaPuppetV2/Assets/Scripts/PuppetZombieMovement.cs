using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetZombieMovement : MonoBehaviour {

    public GameObject Enemy;

    public Animator EnemyMovement;
    public Animation EnemyAnimation;

    public Vector3 LokasiEnemy;
    public Vector2 Flip;
    public float Speed;
    public float JumpForce;
    public float WaktuLompat;

    public float LandingTime;

    public float SpeedMultiplier = 1;
    public bool ResetSpeedMultiplier = true;

    public bool IyaJalan;
    public bool BisaLompat;
    public bool NotLanding;
    public bool DisableButton;
    public bool Attacking;

    // Use this for initialization
    void Start()
    {
        Flip = Enemy.transform.localScale;
        NotLanding = true;
        LandingTime = 0.5f;
        BisaLompat = true;
        DisableButton = false;
        Attacking = false;

    }

    // Update is called once per frame
    void Update()
    {
        LokasiEnemy = Enemy.transform.position;
        EnemyMovement.SetBool("Jalan", false);
        EnemyMovement.SetBool("Lari", false);
        EnemyMovement.SetBool("Attack1", false);

        //Despacito
        if (Input.GetKey(KeyCode.RightArrow) && DisableButton == false)
        {
            if (Enemy.transform.localScale.x < 0)
            {
                Flip.x = Flip.x * -1;
            }
            LokasiEnemy.x = LokasiEnemy.x + Speed * SpeedMultiplier * Time.deltaTime;
            Enemy.transform.position = LokasiEnemy;
            Enemy.transform.localScale = Flip;
            EnemyMovement.SetBool("Jalan", true);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && DisableButton == false)
        {
            if (Enemy.transform.localScale.x > 0)
            {
                Flip.x = Flip.x * -1;
            }
            LokasiEnemy.x = LokasiEnemy.x + -Speed * SpeedMultiplier * Time.deltaTime;
            Enemy.transform.position = LokasiEnemy;
            Enemy.transform.localScale = Flip;
            EnemyMovement.SetBool("Jalan", true);
        }
        if (Input.GetKey(KeyCode.UpArrow) && BisaLompat == true && DisableButton == false)
        {
            BisaLompat = false;
            EnemyMovement.SetBool("Lompat", true);
            EnemyMovement.SetBool("Mendarat", false);
            StartCoroutine("JumpWait");
            NotLanding = false;
            //DisableButton = true;
        }
        if (Input.GetKey(KeyCode.RightShift) && BisaLompat == true)
        {
            SpeedMultiplier = 2;
            EnemyMovement.SetBool("Lari", true);
        }
        if (EnemyMovement.GetBool("Lari") == false && BisaLompat == true)
        {
            SpeedMultiplier = 1;
        }

        if (Input.GetKeyDown(KeyCode.RightControl) && Attacking == false)
        {
            EnemyMovement.SetBool("Attack1", true);
            DisableButton = true;
            StartCoroutine("Attack1Wait");
            Attacking = true;
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            EnemyMovement.SetBool("Dead", true);
            StartCoroutine("DeadWait");
        }
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (NotLanding == false)
        {
            StartCoroutine("LandingWait");
        }
        DisableButton = false;
        EnemyMovement.SetBool("Mendarat", true);
        EnemyMovement.SetBool("Lompat", false);
    }

    public IEnumerator LandingWait()
    {
        yield return new WaitForSeconds(LandingTime);
        NotLanding = true;
        BisaLompat = true;
    }

    public IEnumerator Attack1Wait()
    {
        yield return new WaitForSeconds(0.7f);
        DisableButton = false;
        Attacking = false;
    }

    public IEnumerator JumpWait()
    {
        yield return new WaitForSeconds(0.1f);
        Enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    public IEnumerator DeadWait() {
        DisableButton = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
