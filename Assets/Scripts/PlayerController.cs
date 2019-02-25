using UnityEngine;
using System;

/// <summary>
/// Handles controlling the player...
///
/// And for the observer pattern, this is the subject class.
/// It holds the delegates and notifies all interested observers when
/// [player falls off] or the [game is won]
/// 
/// Other way of implementing the observer pattern would have been to
/// create Observer class and Subject class seperately... PlayerController
/// would have initialized new Subject(), and added new Observer() "GameResetObserver" as listener.
/// Then if [player falls off] or the [game is won] we would call subject.Notify() which
/// would have triggered OnNotify on the "GameResetObserver"...
/// </summary>
public class PlayerController : MonoBehaviour
{
    public GameObject PlayerPrefab;

    private Renderer myRenderer;
    private Rigidbody myRigidbody;
    private CommandFactory inputCmdFactory;

    private Material normalMaterial;
    private Material chargeMaterial;
    private Material chargeFullMaterial;
    private Material superMaterial;

    private int count;
    private int charge;

    private bool isJumping;
    private bool isSuperJumping;
    private bool isCharging;
    private bool isCharged;

    private const int MaxCharge = 50;
    private const int CountToWin = 10;

    private const string MoveCommandKey = "movec";
    private const string JumpCommandKey = "jumpc";
    private const string SuperJumpCommandKey = "jumpsc";

    // "The class which has the delegates is the subject"
    public event Action OnPlayerFellOff;
    public event Action OnGameWon;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myRigidbody = GetComponent<Rigidbody>();

        LoadMaterials();
        RegisterInputCommands();

        count = 0;
        charge = 0;

        isJumping = false;
        isSuperJumping = false;
        isCharging = false;
        isCharged = false;
    }

    void FixedUpdate()
    {
        ListenMoveInput();

        ListenJumpInput();

        CheckIfPlayerFellOf();

        ListenSuperJumpInput();
    }

    /// <summary>
    /// Registers all input commands for the 
    /// </summary>
    private void RegisterInputCommands()
    {
        inputCmdFactory = new CommandFactory();
        inputCmdFactory.RegisterRbCommand<MoveCommand>(MoveCommandKey);
        inputCmdFactory.RegisterRbCommand<JumpCommand>(JumpCommandKey);
        inputCmdFactory.RegisterRbCommand<SuperJumpCommand>(SuperJumpCommandKey);
    }

    /// <summary>
    /// Loads materials used for player
    /// </summary>
    private void LoadMaterials()
    {
        normalMaterial = Resources.Load("Materials/Player", typeof(Material)) as Material;
        chargeMaterial = Resources.Load("Materials/PlayerCharge", typeof(Material)) as Material;
        chargeFullMaterial = Resources.Load("Materials/PlayerChargeFull", typeof(Material)) as Material;
        superMaterial = Resources.Load("Materials/PlayerSuper", typeof(Material)) as Material;
    }

    /// <summary>
    /// Listens for move input
    /// </summary>
    private void ListenMoveInput()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            inputCmdFactory.ExecuteRbCommand(MoveCommandKey, myRigidbody);
        }
    }

    /// <summary>
    /// Listens for jump input
    /// </summary>
    private void ListenJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isSuperJumping && !isCharging && !isCharged)
        {
            inputCmdFactory.ExecuteRbCommand(JumpCommandKey, myRigidbody);
            isJumping = true;
        }
    }

    /// <summary>
    /// Listens for super jump input
    /// </summary>
    private void ListenSuperJumpInput()
    {
        if (isJumping || isSuperJumping)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (isCharged)
            {
                inputCmdFactory.ExecuteRbCommand(SuperJumpCommandKey, myRigidbody);
                ChangeMaterial(superMaterial);

                isSuperJumping = true;
                isCharged = false;
            }
            else if (isCharging)
            {
                ChangeMaterial(normalMaterial);
                isCharging = false;
            }

            charge = 0;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCharging = true;
            ChangeMaterial(chargeMaterial);
        }
        else if (Input.GetKey(KeyCode.LeftControl) && isCharging)
        {
            if (charge < MaxCharge)
            {
                charge++;
            }
            else
            {
                isCharging = false;
                isCharged = true;
                ChangeMaterial(chargeFullMaterial);
            }
        }
    }

    /// <summary>
    /// Checks if the player has fell of 
    /// </summary>
    private void CheckIfPlayerFellOf()
    {
        if (myRigidbody.transform.position.y < 0)
        {
            if (OnPlayerFellOff != null)
            {
                OnPlayerFellOff();
            }
        }
    }

    /// <summary>
    /// Increases amount of objects picked up
    /// and checks if the game has been won
    /// </summary>
    private void IncreasePickUpCount()
    {
        count++;

        if (count >= CountToWin)
        {
            if (OnGameWon != null)
            {
                OnGameWon();
            }
        }
    }

    void ChangeMaterial(Material material)
    {
        Material[] mats = myRenderer.materials;
        mats[0] = material;
        myRenderer.materials = mats;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isJumping)
            {
                isJumping = false;
            }
            else if (isSuperJumping)
            {
                isSuperJumping = false;
                ChangeMaterial(normalMaterial);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            IncreasePickUpCount();
        }
    }
}
