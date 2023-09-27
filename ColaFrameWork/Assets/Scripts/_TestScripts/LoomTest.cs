using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using ColaFramework.Foundation;

public class LoomTest : MonoBehaviour
{

    private Text _text;
	// Use this for initialization
	void Start ()
	{
        ColaLoom.Initialize();
        _text = this.transform.Find("Text").GetComponent<Text>();
        ColaLoom.RunAsync(UpdateUI);
    }

    private void UpdateUI()
    {
        ColaLoom.QueueOnMainThread(() =>
        {
            this._text.text = Time.realtimeSinceStartup.ToString();
            ColaFramework.Timer.RunPerSecond((time) =>
            {
                this._text.text = Time.realtimeSinceStartup.ToString();
            }, null);
        });

    }
}
