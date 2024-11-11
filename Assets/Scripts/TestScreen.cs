using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Tetr4lab.UnityEngine;

public class TestScreen : ScreenMode<TestScreen>, IScreenMode {

    [SerializeField]
    private Text debugPanel = null;

    [SerializeField]
    private Button nextButoon = null;

    public Task<bool> InitAsync (GameObject parent, params object [] args) {
        Debug.Log ($"{parent.name} ({args.Length})");
        debugPanel ??= GetComponentInChildren<Text> ();
        nextButoon ??= GetComponentInChildren<Button> ();
        if (nextButoon is not null) {
            nextButoon.onClick.RemoveListener (OnClickNextButton);
            nextButoon.onClick.AddListener (OnClickNextButton);
        }
        Debug.Log ($"{debugPanel.name} {nextButoon.name}");
        Text = "Test.txt".LoadStreamingText ();
        return Task.FromResult (debugPanel is not null);
    }

    private string Text = "";

    void Update () {
        debugPanel.text = string.Join ("\n", new [] {
            $"output to = '{debugPanel.name}'",
            $"next button = '{nextButoon.name}'",
            $"network available = {Tetr4labUtility.IsNetworkAvailable}",
            Text,
        });
    }

    public void OnClickNextButton () {
        Debug.Log ($"OnClickNextButton");
        Destroy (gameObject);
    }

}
