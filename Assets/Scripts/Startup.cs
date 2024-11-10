using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetr4lab;
using Tetr4lab.UI;

public class Startup : MonoBehaviour {
    async void Start () {
        while (this) {
            Debug.Log ($"Start {name}");
            await TestScreen.CreateAsync (gameObject);
            await TaskEx.DelayWhile (() => TestScreen.OnMode);
            if (!this) { break; }
            Debug.Log ($"Start2 {name}");
            await TestScreen2.CreateAsync (gameObject);
            await TaskEx.DelayWhile (() => TestScreen2.OnMode);
        }
    }
}
