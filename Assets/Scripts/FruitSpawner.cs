using UnityEngine;
using UnityEngine.InputSystem;

public class FruitSpawner : MonoBehaviour
{
    public static FruitSpawner Instance;
    [SerializeField] private GameObject heldFruit;
    [SerializeField] private LineRenderer guideLine;
    [SerializeField] private LayerMask collisionLayers = -1; // What layers to check for collision
    [SerializeField] private float minX = -2.5f;
    [SerializeField] private float maxX = 2.5f;
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 movementInput;
    private Vector3 targetPosition;
    private bool canDrop = true;
    private GameObject currentFruit;
    private GameObject nextFruit;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    void Start()
    {
        // Initialize the target position
        targetPosition = transform.position;

        currentFruit = getFruitToSpawn();
        nextFruit = getFruitToSpawn();
        SpriteRenderer fruitSprite = currentFruit.GetComponent<SpriteRenderer>();
        SpriteRenderer heldFruitRenderer = heldFruit.GetComponent<SpriteRenderer>();
        heldFruitRenderer.sprite = fruitSprite.sprite;

        // Setup the guideline
        SetupGuideline();
    }

    void Update()
    {
        if (movementInput != Vector2.zero)
        {
            targetPosition += new Vector3(movementInput.x, 0f, 0f) * Time.deltaTime * 5f;
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

        // Update guideline every frame
        UpdateGuideline();
    }

    private void SetupGuideline()
    {
        if (guideLine == null)
        {
            // Create LineRenderer if not assigned
            guideLine = gameObject.AddComponent<LineRenderer>();
        }

        // Configure the line renderer
        guideLine.material = new Material(Shader.Find("Sprites/Default"));
        guideLine.startWidth = 0.02f;
        guideLine.endWidth = 0.02f;
        guideLine.positionCount = 2;
        guideLine.useWorldSpace = true;
        guideLine.sortingOrder = 1; // Make sure it renders in front
    }

    private void UpdateGuideline()
    {
        if (guideLine == null || heldFruit == null) return;

        Vector3 startPosition = heldFruit.transform.position;
        Vector3 endPosition = startPosition;

        // Cast a ray downward from the held fruit
        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, Mathf.Infinity, collisionLayers);

        if (hit.collider != null)
        {
            endPosition = hit.point;
        }
        else
        {
            // If no collision, extend the line down a reasonable distance
            endPosition = startPosition + Vector3.down * 10f;
        }

        // Set the line positions
        guideLine.SetPosition(0, startPosition);
        guideLine.SetPosition(1, endPosition);

        // Hide the guideline when we can't drop
        guideLine.enabled = canDrop;
    }

    public void updateCanDrop(GameObject callingFruit)
    {
        if (callingFruit != currentFruit) return;
        currentFruit = nextFruit;
        nextFruit = getFruitToSpawn();
        SpriteRenderer fruitSprite = currentFruit.GetComponent<SpriteRenderer>();
        SpriteRenderer heldFruitRenderer = heldFruit.GetComponent<SpriteRenderer>();
        heldFruitRenderer.sprite = fruitSprite.sprite;
        canDrop = true;
    }

    private GameObject getFruitToSpawn()
    {
        int randomIndex = Random.Range(0, 5);
        return GameManager.Instance.fruitLevels[randomIndex].gameObject;
    }

    public void dropFruit(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !canDrop) return;
        Vector3 spawnPosition = heldFruit.transform.position;
        currentFruit = Instantiate(currentFruit, spawnPosition, Quaternion.identity);
        heldFruit.GetComponent<SpriteRenderer>().sprite = null;
        canDrop = false;
    }

    public void onMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }
}