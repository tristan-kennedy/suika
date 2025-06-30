using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Fruit> fruitLevels;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }
    public GameObject GetNextFruit(Fruit fruit)
    {
        int index = fruitLevels.FindIndex(f => f.fruitType == fruit.fruitType);
        if (index >= 0 && index < fruitLevels.Count - 1)
        {
            return fruitLevels[index + 1].gameObject;
        }

        return null; // Already maxed out, handle Watermelon + Watermelon here somehow eventually
    }

    public void TryMerge(Fruit a, Fruit b)
    {
        if (a.GetInstanceID() > b.GetInstanceID())
        {
            return; // Ensure we always merge in a consistent order
        }

        Vector2 midpoint = (a.transform.position + b.transform.position) / 2f;
        GameObject nextFruit = GetNextFruit(a);

        if (nextFruit != null)
        {
            GameObject newFruit = Instantiate(GetNextFruit(a), midpoint, Quaternion.identity);
        }

        // Optional: play particles/sound

        Destroy(a.gameObject);
        Destroy(b.gameObject);
    }
}
