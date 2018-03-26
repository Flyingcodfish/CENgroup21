using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField]
    private InputField input;

    public string GetInput(string name)
    {
        Debug.Log(name);
        return name;
    }
}
