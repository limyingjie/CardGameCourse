using UnityEngine;
using System.Collections;

public class StatsEffectTest : MonoBehaviour {

    public GameObject DamagePrefab;
    public static StatsEffectTest Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            StatsEffect.CreateStatsEffect(transform.position, Random.Range(-7, 7), Random.Range(-7, 7));
    }
}
