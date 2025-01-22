using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    [Header("Map")]
    public Transform player;
    public float currentDis = 0f;
    public float limitedDis = 100f;
    public float respawnDis = 166f;
    
    // Update is called once per frame
    protected void FixedUpdate()
    {
        Spawning();
        getDis();
    }
    
    protected void Spawning()
    {
        if (this.currentDis < limitedDis) return; Debug.Log("A Weird Horizon Appears");
        Vector2 pos = transform.position;
        pos.x += this.respawnDis;
        transform.position = pos;
    }

    protected virtual void getDis()
    {
        this.currentDis = this.player.position.x - transform.position.x;
    }
}
