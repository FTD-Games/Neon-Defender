using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public List<ObjectPoolWithType> allPools = new List<ObjectPoolWithType>();

    public void ClearAllPools()
    {
        if (allPools.Count > 0)
        {
            foreach (var pool in allPools)
            {
                pool.ClearPool();
            }
        }
    }

    public GameObject GetAvailableObject(Enums.E_RequestableObject requestedObject, Enums.E_Monster targetMonster = Enums.E_Monster.None) => GetPoolsOfType(requestedObject).FirstOrDefault(p => p.requestMonster == targetMonster).GetAvailableObject();

    public void ReAddToAvailablePool(GameObject finishedObject, Enums.E_RequestableObject finishedType, Enums.E_Monster targetMonster = Enums.E_Monster.None)
    {
        finishedObject.transform.SetParent(null);
        var foundPool = GetPoolsOfType(finishedType).FirstOrDefault(p => p.requestMonster == targetMonster);
        if (!foundPool.UsedPool().Contains(finishedObject))
        {
            Debug.LogWarning($"Object '{finishedObject}' with type '{finishedType}' lost in used pool?");
            return;
        }
        switch (finishedType)
        {
            case Enums.E_RequestableObject.Monster:
                ReAddWithTime(foundPool, finishedObject, 0);
                break;
            case Enums.E_RequestableObject.ExpOrb:
                ReAddWithTime(foundPool, finishedObject, 0);
                break;
            default:
                ReAdd(foundPool, finishedObject);
                break;
        }
    }

    private void ReAddWithTime(ObjectPoolWithType pool, GameObject finishedObj, float time) => StartCoroutine(ReAddAfterTime(pool, finishedObj, time));

    private void ReAdd(ObjectPoolWithType pool, GameObject finishedObj)
    {
        pool.UsedPool().Remove(finishedObj);
        pool.AvailablePool().Enqueue(finishedObj);
        finishedObj.SetActive(false);
    }

    private IEnumerator ReAddAfterTime(ObjectPoolWithType pool, GameObject finishedObj, float time)
    {
        yield return new WaitForSeconds(time);
        ReAdd(pool, finishedObj);
    }

    private List<ObjectPoolWithType> GetPoolsOfType(Enums.E_RequestableObject incReqType)
    {
        var foundPools = allPools.Where(x => x.requestableObject == incReqType);
        if (foundPools == null || foundPools.Count() == 0)
        {
            Debug.LogWarning($"Pool not found for {incReqType}. Assign new pool in inspector.");
            return null;
        }
        return foundPools.ToList();
    }

    /// <summary>
    /// A pool with objects of target type.
    /// </summary>
    [Serializable]
    public class ObjectPoolWithType
    {
        public int preLoadAmount;
        public int MyCurrentPoolSize { get; set; }
        private readonly Queue<GameObject> _availablePool = new();
        private readonly HashSet<GameObject> _usedPool = new();
        public GameObject myPrefab;
        public Enums.E_Monster requestMonster;
        public Enums.E_RequestableObject requestableObject;

        public Queue<GameObject> AvailablePool() => _availablePool;

        public HashSet<GameObject> UsedPool() => _usedPool;

        public void ClearPool()
        {
            _usedPool.Clear();
            _availablePool.Clear();
            MyCurrentPoolSize = 0;
            if (preLoadAmount > 0)
            {
                for (int i = 0; i < preLoadAmount; i++)
                {
                    var finishedObj = CreateAndUseNew();
                    UsedPool().Remove(finishedObj);
                    AvailablePool().Enqueue(finishedObj);
                    finishedObj.SetActive(false);
                }
            }
        }

        public GameObject GetAvailableObject()
        {
            if (!_availablePool.Any()) return CreateAndUseNew(); // check if there is NOT an available object -> create new / for usage
            GameObject availableObject = _availablePool.Dequeue(); // get the first next available object
            _usedPool.Add(availableObject); // put object in used pool
            availableObject.SetActive(true); // set the object active -> set start stats ?
            return availableObject;
        }

        private GameObject CreateAndUseNew()
        {
            MyCurrentPoolSize++;
            GameObject newObject = GetObject();
            _usedPool.Add(newObject); // add directly to used pool
            return newObject; // send for usage
        }

        private GameObject GetObject() => Instantiate(myPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity, null);
    }
}