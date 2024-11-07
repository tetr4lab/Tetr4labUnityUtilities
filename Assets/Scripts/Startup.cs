using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tetr4lab.Utilities;

public class Startup : MonoBehaviour {

    [SerializeField]
    private Text debugPanel = null;

    private string Text = "";

    void Start () {
        debugPanel ??= GetComponentInChildren<Text>();
        Debug.Log ($"{debugPanel.name}");
        Text = "Test.txt".LoadStreamingText ();
    }

    void Update () {
        debugPanel.text = string.Join ("\n", new [] {
            $"output to = '{debugPanel.name}'",
            $"network available = {Tetr4labUtility.IsNetworkAvailable}",
            Text,
        });
    }
}
