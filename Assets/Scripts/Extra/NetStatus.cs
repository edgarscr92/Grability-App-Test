using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetStatus : MonoBehaviour
{
    [System.Serializable]
    public class Layouts
    {
        public GameObject area, panel, modeOffline;
        public Button tryAgain, workOffline;
    }
    public Layouts phoneLayout, tabletLayout;
    public Layouts layout;
    private bool haveNet = false;
    private float checkTimeCD =0f;

    void Awake()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        StartCoroutine(checkInternetConnection());
    }

    void Start()
    {
        if (Managers.Instance.IsPhone)
        {
            layout = phoneLayout;
            tabletLayout.area.SetActive(false);
        }
        else
        {
            layout = tabletLayout;
            phoneLayout.area.SetActive(false);
        }

        UnityEngine.Events.UnityAction TryAgain = () =>
        {
            Start();
        };

        UnityEngine.Events.UnityAction offlineMode = () =>
        {
            layout.panel.SetActive(false);
            layout.modeOffline.SetActive(true);
        };

        UnityEngine.Events.UnityAction CheckStatusOfNetwork = () =>
        {
            StartCoroutine(checkInternetConnection());
        };

        layout.tryAgain.onClick.AddListener(TryAgain);
        layout.workOffline.onClick.AddListener(offlineMode);
        layout.modeOffline.GetComponent<Button>().onClick.AddListener(CheckStatusOfNetwork);
    }

    void LateUpdate()
    {
        checkTimeCD += Time.deltaTime;

        if (checkTimeCD >= 60f)
        {
            StartCoroutine(checkInternetConnection());
        }

        if (haveNet)
        {
            if (layout.modeOffline.activeSelf)
                layout.modeOffline.SetActive(false);
            if (layout.panel.activeSelf)
                layout.panel.SetActive(false);
        }
    }

    IEnumerator checkInternetConnection()
    {
        checkTimeCD = 0f;

        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.isDone && www.bytesDownloaded > 0)
        {
            haveNet = true;
            if (layout.panel.activeSelf)
            {
                layout.panel.SetActive(false);
                layout.modeOffline.SetActive(false);
            }
        }

        if (www.isDone && www.bytesDownloaded == 0)
        {
            haveNet = false;
            if (!layout.panel.activeSelf)
            {
                layout.panel.SetActive(true);
            }
        }
    }
}