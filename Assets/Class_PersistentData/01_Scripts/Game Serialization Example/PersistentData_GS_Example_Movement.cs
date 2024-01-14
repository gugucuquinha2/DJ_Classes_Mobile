using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this reference allow us to determine which classes are going to be saved/loaded through the "[Serializable]" header:
// - like the "PlayerData" class, in this case
using System;

public class PersistentData_GS_Example_Movement : MonoBehaviour
{
    private Transform camTransform;
    private Rigidbody2D rb;
    private bool bIsJumping = false;
    private float hor = 0;

    public float speed;
    public float camFollowSpeed;

    // a reference for the UI text displaying the current player's health
    public Text healthTxt;
    public PersistentData_GS_Example_Joystick joystick;

    // a reference for an instance of the class to be saved
    // this way we always know which instance will hold the player information so we can pass it on to our GameData class to be saved
    private PlayerData playerData;

    private void Awake()
    {
        // we assign the GameManager global reference to this script, so it can be globally accessible
        PersistentData_GS_Example_GlobalVarsManager.Instance.PlayerMovement = this;

        // before the game starts we create a new instance to be used of the PlayerData class
        // but only if we don't already have one created (through loading)
        // this way we make sure we only get 1 instance of this class, preventing errors associated with saving/loading the wrong instance of this class
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
        // we associate our instance of the PlayerData class in use to the one that's going to be saved and loaded at the GameData
        // now, every change we make to our local "levelData" variable, is going to automatically update the GameData's instance to be saved
        PersistentData_GS_Example_GlobalVarsManager.Instance.GameData.playerData = playerData;
    }

    // Start is called before the first frame update
    void Start()
    {
        // get necessary components
        rb = GetComponent<Rigidbody2D>();
        camTransform = Camera.main.transform;
        // update the UI
        healthTxt.text = "Health: " + playerData.health;
    }

    // method called whenever the game is loaded
    // this method is only concerned about the PlayerData, other systems should have a similar method to deal with their own data
    // again -  this way each part of the game takes care of its own data (modular approach) making it easier to manage and debug and it's scalable (if the game grows bigger)
    public void OnLoad(PlayerData _playerData)
    {
        // update the instance of the PlayerData class in use with the loaded one
        playerData = _playerData;
        // update the player's position - we place it a bit in front of the checkpoint so we don't trigger it again when loading the game
        transform.position = new Vector3(playerData.position.x + 2.5f, playerData.position.y);
        // update the UI with the loaded player health value
        healthTxt.text = "Health: " + playerData.health;
    }

    // update callbakc used for the player's movement logic
    void Update()
    {
        if (transform.position.y < 0.01f)
        {
            bIsJumping = false;

            // only allow controlling movement when not jumping
            hor = joystick.GetJoystickOutput().x;
        }

        Vector2 vel = rb.velocity;
        Vector3 translation = new Vector3(hor, 0, 0);

        if (joystick.GetJoystickOutput().y > 0.7f && !bIsJumping)
        {
            vel.y = 8;

            bIsJumping = true;
            hor *= 0.65f;
        }
        rb.velocity = vel;

        transform.position += translation * (speed * Time.deltaTime);

        // update camera position to smoothly follow player position
        Vector3 camPos = camTransform.position;
        camPos.x = Mathf.Lerp(camPos.x, transform.position.x, camFollowSpeed * Time.deltaTime);
        camTransform.position = camPos;
    }

    // trigger detection callback
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // if we collide with the enemy...
        if (collider.gameObject.CompareTag("Enemy"))
        {
            // ...we lose 10 health...
            playerData.health -= 10;
            // ...and update the health UI so the player can see it
            healthTxt.text = "Health: " + playerData.health;

        }
        // if we collide with a checkpoint
        else if (collider.gameObject.CompareTag("Checkpoint"))
        {
            // ...we update the level...
            PersistentData_GS_Example_GlobalVarsManager.Instance.GameManager.UpdateStage(true);
            // ...disable the checkpoint so we  can't go past it again
            collider.enabled = false;

            // at the moment the game saves, we store the player's position in the PlayerData instance, so it can also be saved
            playerData.position = transform.position;
            // call the method to save the game
            PersistentData_GS_Example_GlobalVarsManager.Instance.GameManager.Save();
        }
    }
}

// everytime we want to save a class, we need to make sure it can be serialized by adding the header "[Serializable]"
// having a specific class to hold all information we want to save/load is the most convenient and organized way to store only the information we need saving
[Serializable]
public class PlayerData
{
    // regarding the player, we want to save his remaining health and its current position
    // so we can start the game right where we left off
    public int health = 100;
    public Vector2 position;
}
