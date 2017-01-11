using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryData : MonoBehaviour
{
    [System.Serializable]
    public class EntryLayout
    {
        public GameObject area;
        public Image icon;
        public Text title;
        public Text price;
    }
    public EntryLayout listLayout, grillaLayout;
    public EntryMode mode;
    private EntryLayout layout;

    private List<Sprite> textures = new List<Sprite>();
    public bool isNull = true;

    void Awake()
    {
        if(Managers.Instance.IsPhone)
        {
            mode = EntryMode.Lista;
            layout = listLayout;
            grillaLayout.area.SetActive(false);
            GetComponent<RectTransform>().sizeDelta = new Vector2(0, 120);
        }
        else
        {
            mode = EntryMode.Grilla;
            layout = grillaLayout;
            listLayout.area.SetActive(false);
        }
    }

    public void SetData(Entry data)
    {
        for (int i = 0; i < data.imImage.Count; i++)
        {
            string oldString = data.imImage[i].label;
            string newString = oldString.Substring(oldString.Length - 50);

            Texture2D tex = DataStorage.ReadTextureFromPlayerPrefs(newString);

            Sprite tempSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            textures.Add(tempSprite);
        }

        if (textures.Count > 0)
            layout.icon.sprite = textures[0];

        layout.price.text = data.imPrice.attributes.amount + " " + data.imPrice.attributes.currency;

        if(mode == EntryMode.Lista)
            layout.title.text = data.title.label;

        UnityEngine.Events.UnityAction ShowProductDetails = () =>
        {
            Managers.Instance.Canvas.contentData.productDetails.anim.SetTrigger("Show");
            Managers.Instance.Canvas.contentData.productDetails.Details(
                textures[0],
                data.title.label,
                data.imPrice.attributes.amount + " " + data.imPrice.attributes.currency,
                data.summary.label
            );
        };

        GetComponent<Button>().onClick.AddListener(ShowProductDetails);
    }

    public void SetCleanData()
    {
        layout.icon.sprite = null;
        foreach (Text t in GetComponentsInChildren<Text>())
        {
            t.text = "";
        }
        GetComponent<Button>().enabled = false;
    }
}