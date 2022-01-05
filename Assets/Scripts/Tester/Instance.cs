using Project.Pool;
using System.Collections.Generic;
using UnityEngine;

//This class is placed on the script to showcase the use of the IPooled interface.
//Each IPooled object can have its own specific behaviour when it enters or leaves the pool.

public class Instance : MonoBehaviour, IPooled
{
    [field: SerializeField]
    private float _despawnDelay { get; set; } = 1.5f;


    private ClassPooler<GameObject> _pooler { get; set; }
    private List<Instance> _spawnedObjs { get; set; }
    private string _pooledObjName { get; set; }
    private float _despawnCounter { get; set; }



    public void Init(ClassPooler<GameObject> pooler, List<Instance> spawnedObjs)
    {
        _pooler = pooler;
        _pooledObjName = name.Replace("(Clone)", null);
        _spawnedObjs = spawnedObjs;
    }

    public void UpdateMe()
    {
        _despawnCounter += Time.deltaTime;

        if (_despawnCounter >= _despawnDelay)
        {
            _despawnCounter = 0f;
            _pooler.ReturnToPool(gameObject, _pooledObjName);
            _spawnedObjs.Remove(this);
        }

    }


    public void OnEnqueued()
    {
        gameObject.SetActive(false);
    }

    public void OnDequeued()
    {
        gameObject.SetActive(true);
    }
}
