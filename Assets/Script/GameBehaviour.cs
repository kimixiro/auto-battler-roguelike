using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBehaviour : MonoBehaviour
{   [SerializeField] public GameConfig data;
    [SerializeField]
    private List<GameObject> unitPrefab;
    [SerializeField]
    private List<GameObject> unitMonster;
    [SerializeField]
    private List<LvlScriptableObject> lvlCong;
    [SerializeField]
    private int _lvlCount;
    [SerializeField]
    private List<Transform> spawnUnit;
    [SerializeField]
    private List<Transform> spawnEnemy;
    
    public List<GameObject> poolUnit;
    public List<GameObject> poolMonster;
    [SerializeField]
    private GameObject _parentPoolUnit;
    [SerializeField]
    private GameObject _parentPoolMonster;

    public bool bootFin = false;
    public bool win;
    
    public UISelectUinit SelectUinit;
    public UIDefeat Defeat;
    public UiItems Items;
    
    
    

    public void Start()
    {
        if (_lvlCount == 1)
        {
            var lvl = lvlCong.Find(x => x.id == _lvlCount);
            _lvlCount++;
            
            foreach (var id in lvl.enemyPrefab)
            {
                SpawnEnemy(id);
            }
        
            foreach (var id in data.startPool)
            {
                SpawnUnit(id);
            }
        }
        
        Items.Fill(poolUnit,data.maxSlotUnit);
      //ъъ  SetMultiplier();
        bootFin = true;
        win = false;

    }

    public void LevelStart()
    {
        var lvl = lvlCong.Find(x => x.id == _lvlCount);
            _lvlCount++;
            
            Debug.Log("Start " + lvl.name);
            
            foreach (var id in lvl.enemyPrefab)
            {
                SpawnEnemy(id);
            }

        Items.Fill(poolUnit,data.maxSlotUnit);
        //ъъ  SetMultiplier();
        bootFin = true;
        win = false;
    }

    private void SetMultiplier()
    {
        foreach (var item in Items.items)
        {
            poolUnit.Find(x => x.GetComponent<IUnit>().UnitConfig.ClassUnit == item.ClassUnit).GetComponent<IUnit>().Multiplier = item.countInt;
        }
    }

    public void SpawnUnit(string id)
    {
       
        foreach (var t in unitPrefab)
        {
            if (id == t.GetComponent<IUnit>().UnitConfig.id)
            {
                var u =  Instantiate(t, new Vector3(0, 0, 0), Quaternion.identity,_parentPoolUnit.transform);
                u.GetComponent<IUnit>().Fill(this);
                poolUnit.Add(u);
            }
        }
        
        PlacementUnit();
    }


    void SpawnEnemy(string id)
    {
        foreach (var t in unitMonster)
        {
            if (id == t.GetComponent<IUnit>().UnitConfig.id)
            {
                var u =  Instantiate(t, new Vector3(0, 0, 0), Quaternion.identity,_parentPoolMonster.transform);
                u.GetComponent<IUnit>().Fill(this);
                poolMonster.Add(u);
            }
        }
        
        PlacementMonster();
    }
    
    void PlacementUnit()
    {
        for (var index = 0; index < poolUnit.Count; index++)
        {
            var unit = poolUnit[index];
           
            unit.transform.position = spawnUnit[index].position;
            unit.transform.rotation = spawnUnit[index].rotation;
        }
        
    }
    
    void PlacementMonster()
    {
        for (var index = 0; index < poolMonster.Count; index++)
        {
            var monster = poolMonster[index];
           
            monster.transform.position = spawnEnemy[index].position;
            monster.transform.rotation = spawnEnemy[index].rotation;
        }
    }

    public IUnit GetMonsterTarget(Transform unit)
    {
        if (!bootFin)
            return null;
        float oldDist = 100;
        IUnit targetMonster = null;
        var u = new IUnit();
        foreach (var t in poolMonster)
        {
            var dist = Vector3.Distance(unit.position, t.transform.position);
            if (dist<oldDist)
            {
                oldDist = dist;
                u = t.GetComponent<IUnit>();
            }
        }
        
        if (poolMonster.Count.Equals(0)&&!win)
        {
            win = true;
            SelectUinit.ShowSelectUnit();
        }
        else
        { 
            targetMonster = poolMonster[Random.Range(0, poolMonster.Count)].GetComponent<IUnit>();
        }

        
        return targetMonster;
    }
    
    public IUnit GetUnitTarget(Transform unit)
    {
        if (!bootFin)
            return null;
        
        float oldDist = 100;
        IUnit targetPlayer = null;
        var u = new IUnit();
        foreach (var t in poolUnit)
        {
            var dist = Vector3.Distance(unit.position, t.transform.position);
            if (dist<oldDist)
            {
                oldDist = dist;
                u = t.GetComponent<IUnit>();
            }
        }
        if (poolUnit.Count.Equals(0)&&!win)
        {
            win = true;
            Defeat.ShowDefeat();
        }
        else
        {
            targetPlayer = poolUnit[Random.Range(0, poolUnit.Count)].GetComponent<IUnit>();
        }
        return targetPlayer;
    }

    public void DestroyMonster(IUnit taget)
    {
        poolMonster.Remove(taget.gameObject);
        Destroy(taget.gameObject,1);
    }
    
    public void DestroyUnit(IUnit taget)
    {
        poolUnit.Remove(taget.gameObject);
        Items.Fill(poolUnit,data.maxSlotUnit);
      //  SetMultiplier();
        Destroy(taget.gameObject,1);
    }

    public void AddUnit(ClassUnit classUnit)
    {
        List<GameObject> poolChoice = new List<GameObject>();
        foreach (var t in unitPrefab)
        {
            if (classUnit == t.GetComponent<IUnit>().UnitConfig.ClassUnit)
            {
                poolChoice.Add(t);
            }
        }
        
        var gameObject = poolChoice[Random.Range(0, poolChoice.Count)];
        
        var u =  Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity,_parentPoolUnit.transform);
        u.GetComponent<IUnit>().Fill(this);
        poolUnit.Add(u);
        
        PlacementUnit();
        LevelStart();
    }
}
