using System;
using Cysharp.Threading.Tasks;
using GameControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    private string _obstacle;
    private int _ranObs;
    private Vector3 _ranPos, _ranPosKillerWhale, _ranPosGlacier, _ranPosMonkFish;
    private Quaternion _ranRot;
    [SerializeField] private Transform seaOtter;
    [SerializeField] private float delay;
    [SerializeField] private Transform BossTransform;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1, Random.Range(1, 6));
        InvokeRepeating(nameof(SpawnObstacle_KillerWhale), 1, Random.Range(7, 11));
    }

    public void OnDisable()
    {
        CancelInvoke();


        BossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
    }

    private async UniTaskVoid SpawnObstacle()
    {
        _ranObs = Random.Range(0, 2);
        if (_ranObs == 0)
        {
            _obstacle = "Obstacle";
        }
        else if (_ranObs == 1)
        {
            _obstacle = "Obstacle_1";
        }
        else
        {
            _obstacle = "Obstacle_2";
        }

        var seaOtterPosition = seaOtter.position;
        _ranPos = new Vector3(seaOtterPosition.x + 20, Random.Range(-12, 5), 0);
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        ObjectPoolManager.Get(_obstacle, _ranPos, _ranRot);
    }

    private async UniTaskVoid SpawnObstacle_KillerWhale()
    {
        var seaOtterPosition = seaOtter.position;
        _ranPosKillerWhale = new Vector3(seaOtterPosition.x - 20,
            Random.Range(seaOtterPosition.y - 6, seaOtterPosition.y), 0);
        ObjectPoolManager.Get("NotifyObstacle", _ranPosKillerWhale);
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        ObjectPoolManager.Get("KillerWhale", _ranPosKillerWhale, _ranRot);
    }

    private async UniTaskVoid SpawnObstacle_Glacier()
    {
        var seaOtterPosition = seaOtter.position;
        _ranPosGlacier = new Vector3(seaOtterPosition.x + 20, 6.845004f, 0);
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        ObjectPoolManager.Get("Glacier", _ranPosGlacier, _ranRot);
    }

    public void InvokeGlacier()
    {
        InvokeRepeating(nameof(SpawnObstacle_Glacier), 0, 7);
    }

    private async UniTaskVoid SpawnObstacle_MonkFish()
    {
        var seaOtterPosition = seaOtter.position;
        _ranPosMonkFish = new Vector3(seaOtterPosition.x + 20,
            Random.Range(seaOtterPosition.y - 3, seaOtterPosition.y + 4), 0);
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        ObjectPoolManager.Get("Monkfish", _ranPosMonkFish, _ranRot);
    }

    public void InvokeMonkFish()
    {
        InvokeRepeating(nameof(SpawnObstacle_MonkFish), 1, Random.Range(6, 13));
    }

    public void InvokeBossAttack()
    {
        InvokeRepeating(nameof(SpawnObstacle_Boss_Attack), 1, Random.Range(5, 10));
    }

    public async UniTaskVoid SpawnObstacle_Boss_Attack()
    {
        for (int i = 0; i < 3; i++)
        {
            _ranObs = Random.Range(0, 2);
            if (_ranObs == 0)
            {
                _obstacle = "Obstacle_Boss";
            }
            else if (_ranObs == 1)
            {
                _obstacle = "Obstacle_Boss_1";
            }
            else
            {
                _obstacle = "Obstacle_Boss_2";
            }
            _ranPos = new Vector3(BossTransform.position.x - 3,
            Random.Range(BossTransform.position.y - 3, BossTransform.position.y + 4), 0);
            ObjectPoolManager.Get("NotifyObstacle", _ranPos);
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            // ranRot = Quaternion.Euler(0, 0, Random.Range(0, 360));
            ObjectPoolManager.Get(_obstacle, _ranPos, _ranRot);
        }
    }
    
}