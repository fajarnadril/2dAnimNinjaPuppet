using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public GameObject Character;

    public Animator CharacterMovement;
    public Animation CharacterAnimation;

    public Vector3 LokasiCharacter;
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

	// Use this for initialization
	void Start ()
    {
        Flip = Character.transform.localScale;
        NotLanding = true;
        LandingTime = 0.5f;
        BisaLompat = true;
        DisableButton = false;
    }
	
	// Update is called once per frame
	void Update () {
        LokasiCharacter = Character.transform.position;
        CharacterMovement.SetBool("Jalan", false);
        CharacterMovement.SetBool("Lari", false);
        //Despacito
        if (Input.GetKey(KeyCode.D) && DisableButton == false )
        {
            if (Character.transform.localScale.x > 0)
            {
                Flip.x = Flip.x * -1;
            }
            LokasiCharacter.x = LokasiCharacter.x + Speed*SpeedMultiplier*Time.deltaTime;
            Character.transform.position = LokasiCharacter;
            Character.transform.localScale = Flip;
            CharacterMovement.SetBool("Jalan", true);
        }
        if (Input.GetKey(KeyCode.A) && DisableButton == false)
        {
            if(Character.transform.localScale.x < 0) {
                Flip.x = Flip.x * -1;
            }
            LokasiCharacter.x = LokasiCharacter.x + -Speed*SpeedMultiplier* Time.deltaTime;
            Character.transform.position = LokasiCharacter;
            Character.transform.localScale = Flip;
            CharacterMovement.SetBool("Jalan", true);
        }
        if (Input.GetKey(KeyCode.W) && BisaLompat == true && CharacterAnimation.IsPlaying("Run"))
        {
            Character.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            BisaLompat = false;
            CharacterMovement.SetBool("Lompat", true);
            CharacterMovement.SetBool("Mendarat", false);
            NotLanding = false;
            DisableButton = true;
        }
        if (Input.GetKey(KeyCode.LeftShift) && BisaLompat == true) {
            SpeedMultiplier = 2;
            CharacterMovement.SetBool("Lari", true);
        }
        if (CharacterMovement.GetBool("Lari") == false && BisaLompat == true)
        {
            SpeedMultiplier = 1;
        }
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (NotLanding == false)
        {
            StartCoroutine("LandingWait");
        }
        DisableButton = false;
        CharacterMovement.SetBool("Mendarat", true);
        CharacterMovement.SetBool("Lompat", false);
    }

    public IEnumerator LandingWait() {
        yield return new WaitForSeconds(LandingTime);
        NotLanding = true;
        BisaLompat = true;
    }
}
