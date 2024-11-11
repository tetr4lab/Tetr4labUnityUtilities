using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Tetr4lab.UnityEngine;
using Tetr4lab;
using System;

public class TestScreen2 : ScreenMode<TestScreen2>, IScreenMode {

    [SerializeField]
    private Text debugPanel = null;

    [SerializeField]
    private Button nextButoon = null;

    [SerializeField]
    private Button addButoon = null;

    public async Task<bool> InitAsync (GameObject parent, params object [] args) {
        Debug.Log ($"{parent.name} ({args.Length})");
        debugPanel ??= GetComponentInChildren<Text> ();
        var buttons = GetComponentsInChildren<Button> ();
        if (buttons.Length > 0) {
            addButoon ??= buttons [0];
        }
        if (buttons.Length > 1) {
            nextButoon ??= buttons [1];
        }
        if (nextButoon is not null) {
            nextButoon.onClick.RemoveListener (OnNextButtonDown);
            nextButoon.onClick.AddListener (OnNextButtonDown);
        }
        if (addButoon is not null) {
            addButoon.onClick.RemoveListener (OnAddButtonDown);
            addButoon.onClick.AddListener (OnAddButtonDown);
        }
        Debug.Log ($"{debugPanel.name} {addButoon.name} {nextButoon.name}");
        managed = await TestManaged.CreateAsync (gameObject);
        return debugPanel is not null;
    }

    private TestManaged managed = null;

    void Update () {
        debugPanel.text = string.Join ("\n", new [] {
            $"output to = '{debugPanel.name}'",
            $"add button = '{addButoon.name}'",
            $"next button = '{nextButoon.name}'",
        });
    }

    public async void OnAddButtonDown () {
        Debug.Log ($"OnAddButtonDown");
        managed = await TestManaged.CreateAsync (gameObject);
    }

    public void OnNextButtonDown () {
        Debug.Log ($"OnNextButtonDown");
        Destroy (gameObject);
    }
}

/// <summary>�Ǘ��ΏۃN���X</summary>
public sealed class TestManaged : MonoBehaviour {

    [SerializeField]
    private Text infoPanel = null;

    public DateTime Created;

    private float height;

    /// <summary>�Ǘ��N���X</summary>
    private static ManagedInstance<TestManaged> ManagedInstance { get; }
        = new ManagedInstance<TestManaged> (3, true);

    /// <summary>�C���X�^���X����</summary>
    /// <param name="parent">�R���e�i</param>
    /// <returns>�������ꂽ�C���X�^���X</returns>
    public static async Task<TestManaged> CreateAsync (GameObject parent) {
        var instance = await ManagedInstance.CreateAsync (parent);
        instance.infoPanel = instance.gameObject.GetComponentInChildren<Text> ();
        instance.Created = DateTime.Now;
        var rectTransform = instance.GetComponent<RectTransform> ();
        instance.height = rectTransform.rect.height;
        return instance;
    }

    /// <summary>�X�V</summary>
    private void Update () {
        if (infoPanel is not null) {
            var index = ManagedInstance.IndexOf (this);
            var rectTransform = infoPanel.GetComponent<RectTransform> ();
            var pos = rectTransform.localPosition;
            pos.y = height / 2 - 300 - index * 120;
            rectTransform.localPosition = pos;
            infoPanel.text = $"{index}: {pos.y}\n{Created:yyyy/mm/dd HH:mm:ss.ff}";
        }
    }

    /// <summary>�j��</summary>
    private void OnDestroy () {
        ManagedInstance.Instances.Remove (this);
    }

}



