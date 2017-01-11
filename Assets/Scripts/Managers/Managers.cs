using UnityEngine;

[RequireComponent(typeof(ProductManager))]
public class Managers : MonoBehaviour
{
    private static Managers _Instance;
    public static Managers Instance { get { return _Instance; } }
    private ProductManager _productManager;
    public ProductManager productManager { get { return _productManager; } }
    private CanvasManager _Canvas;
    public CanvasManager Canvas { get { return _Canvas; } }

    private bool _isPhone = true;
    public bool IsPhone { get { return _isPhone; } }

#if UNITY_EDITOR
    public bool isPhoneV2 = false;
#endif

    void Awake()
    {
        _Instance = this;
        _productManager = GetComponent<ProductManager>();
        _Canvas = FindObjectOfType<CanvasManager>() as CanvasManager;

#if UNITY_EDITOR
            _isPhone = isPhoneV2;
#elif UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            _isPhone = DeviceDiagonalSizeInInches();
            if(_isPhone)
                Screen.orientation = ScreenOrientation.Portrait;
            else
                Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
#endif
    }

    void LateUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.D))
            PlayerPrefs.DeleteAll();
#endif
    }

    private bool DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

#if UNITY_EDITOR
        Debug.Log("Getting device inches: " + diagonalInches);
#endif

        if (diagonalInches > 6.5f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}