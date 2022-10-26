using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    public string TargetPlayerTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        print(other.tag);
        print(TargetPlayerTag);
        if(other.tag != TargetPlayerTag) return;

        var player = other.gameObject.GetComponent<Player>();
        player.ReachedEnd = true;
        player.state = "Home";
        
        other.transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        LevelGenerator.Instance.PlayClip("winpuzzle");

        // disable this endpoint
        enabled = false;
    }
}
