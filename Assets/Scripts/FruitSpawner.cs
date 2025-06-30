using UnityEngine;
using UnityEngine.InputSystem;

public class FruitSpawner : MonoBehaviour
{
    public static FruitSpawner Instance;

    [SerializeField] private GameObject heldFruit;

    private Vector2 movementInput;
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
        currentFruit = getFruitToSpawn();
        nextFruit = getFruitToSpawn();
        SpriteRenderer fruitSprite = currentFruit.GetComponent<SpriteRenderer>();
        SpriteRenderer heldFruitRenderer = heldFruit.GetComponent<SpriteRenderer>();
        heldFruitRenderer.sprite = fruitSprite.sprite;
    }

    void Update()
    {
        if (movementInput != Vector2.zero)
        {
            Vector3 move = new Vector3(movementInput.x, 0f, 0f);
            transform.position += move * Time.deltaTime * 5f;
        }
    }

    public void updateCanDrop(GameObject callingFruit)
    {
        Debug.Log("Updating canDrop from " + callingFruit.name);
        Debug.Log("Current fruit is " + currentFruit.name);
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