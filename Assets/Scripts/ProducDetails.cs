using UnityEngine;
using UnityEngine.UI;

public class ProducDetails : MonoBehaviour
{
    public Button _hide;
    public Image _icon;
    public Text _title;
    public Text _price;
    public Text _desc;
    private Animator _anim;
    public Animator anim { get { return _anim; } }

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Details(Sprite sprite, string title, string price, string desc)
    {
        UnityEngine.Events.UnityAction HideProducts = () => { _anim.SetTrigger("Hide"); };
        _hide.onClick.AddListener(HideProducts);
        _desc.GetComponent<RectTransform>().offsetMax = new Vector2(_desc.GetComponent<RectTransform>().offsetMax.x, 0);
        _icon.sprite = sprite;
        _title.text = title;
        _price.text = price;
        _desc.text = desc;
    }
}