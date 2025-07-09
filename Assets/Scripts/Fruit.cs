using UnityEngine;

public class Fruit : MonoBehaviour
{
    public string fruitType;
    public int points;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        FruitSpawner.Instance.updateCanDrop(gameObject);

        Fruit other = collision.gameObject.GetComponent<Fruit>();
        if (other == null) return;

        if (this.fruitType == other.fruitType)
        {
            GameManager.Instance.TryMerge(this, other);
        }
    }

}
