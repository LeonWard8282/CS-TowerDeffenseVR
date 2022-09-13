using System.Collections.Generic;
using UnityEngine;



public class ObjectPool
{
    // Object pool will have a PoolableObject Prefab variable
    private PoolableObject prefab;
    // And it will have a list of poolabale object variable called availableobjects.  
    private List<PoolableObject> availableObjects;

    // private constructor that takes in a poolableObject Prefab and the in size. 
    private ObjectPool (PoolableObject Prefab, int size)
        {
        //this constructoc will make this.Prefab equal the prefab that is passed in. 
        this.prefab = Prefab;
        //This constructor will then go ahead and make a list of (x-Size) valled available Objects. 
        availableObjects = new List<PoolableObject>(size);
        }


    public static ObjectPool CreateInstance(PoolableObject prefab, int size)
    {
        ObjectPool pool = new ObjectPool(prefab, size);

        GameObject poolObject = new GameObject(prefab.name + "Pool");
        pool.CreateObjects(poolObject.transform, size);

        return pool;


    }

    private void CreateObjects( Transform parent, int Size)
    {
        for (int i = 0; i < Size; i++)
        { 

            PoolableObject poolableObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false);
        }

    }

    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        availableObjects.Add(poolableObject);

    }

    public PoolableObject GetObject()
    {
        if (availableObjects.Count > 0)
        {
            PoolableObject instance = availableObjects[0];
            availableObjects.RemoveAt(0);

            instance.gameObject.SetActive(true);

            return instance;

        }

        else
        {
            // couple of options to this
            //1. Return null - if you do not want to auto epand the size of the pol. Okay option if you are sure you will never configure pools wrong.  or if the pool not return gan object. 
            // this is sometimes okay to perfrom. 
            //2. Expand the size of the pool - the downside to doing this is if you expand the pool you will have to create new objects, which then create new garbage which then create sturtter. 

            Debug.LogError($"Could not get an object from pool \"{prefab.name} \"Pool. probably a configuration issue. ");
        return null;
        }
    }




}
