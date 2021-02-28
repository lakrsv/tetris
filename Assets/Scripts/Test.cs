using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var tetroMino = Tetromino.FromBlueprint(Tetromino.Type.I);
        Debug.Log("Hello World!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
