using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject prefab;
    protected Stack<GameObject> inactive;
    public int capacity = 0;

    public virtual void Awake()
    {
        inactive = new Stack<GameObject>();

        for (int i = 0; i < capacity; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            this.inactive.Push(obj);
        }
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject spawned = null;
        if (inactive.Count <= 0)
        {
            this.Expand(1);
        }
        spawned = inactive.Pop();
        spawned.SetActive(true);
        spawned.transform.SetPositionAndRotation(position, rotation);
        return spawned;
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        inactive.Push(obj);
    }

    public void Expand(int quantity)
    {
        this.capacity += quantity;
        for (int i = 0; i < quantity; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            inactive.Push(obj);
        }
    }
}
