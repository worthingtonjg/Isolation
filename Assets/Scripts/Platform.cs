using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown() {
        transform.Rotate(0f, 90f, 0f);
        LevelGenerator.Instance.PlayClip("rotate");
    }
}
