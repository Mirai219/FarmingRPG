using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    private Camera mainCamera;

    private AnimationOverrides animationOverrides;
    private List<CharacterAttribute> characterAttributeCustomisationList;
    [Tooltip("Should be populated in the prefab with the equipped item sprite renderer")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

    private CharacterAttribute armsCharacterAttribute;
    private CharacterAttribute toolCharacterAttribute;

    private float inputX;
    private float inputY;
    private bool isWaliking;
    private bool isRunning;
    private bool isIdle;
    private bool isCarrying = false;
    private ToolEffects toolEffects = ToolEffects.none;
    private bool isUsingToolRight;
    private bool isUsingToolLeft;
    private bool isUsingToolUp;
    private bool isUsingToolDown;
    private bool isLiftingToolRight;
    private bool isLiftingToolLeft;
    private bool isLiftingToolUp;
    private bool isLiftingToolDown;
    private bool isPickingRight;
    private bool isPickingLeft;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isSwingingToolRight;
    private bool isSwingingToolLeft;
    private bool isSwingingToolUp;
    private bool isSwingingToolDown;
    private bool idleUp;
    private bool idleDown;
    private bool idleLeft;
    private bool idleRight;

    private Rigidbody2D Rigidbody2D;

    private Direction PlayerDirection;
    
    private float movementSpeed;

    private bool _playerInputDisable = false;
    public bool playerInputDisable { get => _playerInputDisable; set => _playerInputDisable = value; }

    protected override void Awake()
    {
        base.Awake();

        Rigidbody2D = GetComponent<Rigidbody2D>();

        animationOverrides = GetComponentInChildren<AnimationOverrides>();

        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.Arms, PartVariantColor.none, PartVariantType.none);

        characterAttributeCustomisationList = new List<CharacterAttribute>();

        //get the reference of main camera
        mainCamera  = Camera.main;
    }

    private void Update()
    {
        #region PlayerInput

        if(!playerInputDisable)
        {
            PlayerTestInput();
            ResetAnimationTriggers();
            PlayerMovementInput();
            EventsHandler.MovementEventsHandler.CallMovementEvent(inputX, inputY,
                isWaliking, isRunning, isIdle, isCarrying,
                toolEffects,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                false, false, false, false);
        }

        #endregion PlayerInput
    }

    private void PlayerTestInput()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Test:Advance Minute");
            TimeManager.Instance.TestAdvanceGameMinute();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Test:Advance Day");
            TimeManager.Instance.TestAdvanceGameDay();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Test:Scene");
            SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene1_Farm.ToString(), transform.position);
        }

    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    private void PlayerMove()
    {
        Vector2 move =new Vector2(inputX*movementSpeed*Time.deltaTime, inputY * movementSpeed * Time.deltaTime);
        Rigidbody2D.MovePosition(Rigidbody2D.position + move);  
    }
    private void ResetAnimationTriggers()
    {
        toolEffects = ToolEffects.none;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
    }

    private void PlayerMovementInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if(inputX != 0 &&  inputY != 0)
        {
            inputX = inputX * 0.71f;
            inputY = inputY * 0.71f;
        }

        if(inputX != 0 || inputY != 0)
        {
            if(Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)) 
            {
                isRunning = false;
                isWaliking = true;
                isIdle = false;
                movementSpeed = Settings.walkingSpeed;
            }
            else
            {
                isRunning = true;
                isWaliking = false;
                isIdle = false;
                movementSpeed = Settings.runningSpeed;
            }
            

            if(inputX < 0)
            {
                PlayerDirection = Direction.left;
            }else if (inputX > 0)
            {
                PlayerDirection = Direction.right;
            }else if(inputY < 0)
            {
                PlayerDirection = Direction.down;
            }else
            {
                PlayerDirection= Direction.up;
            }
        }else if(inputX == 0 && inputY == 0)
        {
            isRunning = false;
            isWaliking = false;
            isIdle = true;
        }
    }

    public void DisablePlayerInput()
    {
      playerInputDisable = true;
    }
    public void EnablePlayerInput()
    {
        playerInputDisable = false;
    }
    public void DisablePlayerInputAndResetMovement()
    {
        DisablePlayerInput();
        ResetMovement();
        EventsHandler.MovementEventsHandler.CallMovementEvent(inputX, inputY,
                isWaliking, isRunning, isIdle, isCarrying,
                toolEffects,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                false, false, false, false);
    }

    private void ResetMovement()
    {
        inputX = 0;
        inputY = 0;
        isIdle = true;
        isWaliking = false;
        isRunning = false;
    }
    public Vector3 GetPlayerViewportPosition()
    {
        return mainCamera.WorldToViewportPoint(transform.position);
    }

    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

        if(itemDetails != null)
        {
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f,1f, 1f, 1f);

            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            //Debug.Log(armsCharacterAttribute.characterPart.ToString());
            characterAttributeCustomisationList.Clear();
            characterAttributeCustomisationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

            isCarrying = true;
        }
    }

    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f,1f,1f,0f);

        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        isCarrying = false;
    }
}
