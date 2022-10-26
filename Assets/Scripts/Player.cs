using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool ReachedEnd;
    public float moveSpeed = 2f;
    public float gravity = 1f;
    public string state = "Isolated";
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ReachedEnd) return;

        var h = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var v = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        var direction = Vector3.down * gravity * Time.deltaTime;
        direction += Vector3.right * -h;
        direction += Vector3.forward * -v;
        controller.Move(direction);

        if(transform.position.y < -5)
        {
            gameObject.SetActive(false);
            state = "Lost";
            LevelGenerator.Instance.PlayClip("pew");
        }
    }

    private void OnMouseDown() 
    {
        LevelGenerator.Instance.Players.ForEach(p => p.enabled = false);
        enabled = true;
    }
}
