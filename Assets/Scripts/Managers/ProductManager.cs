using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public Feed feed;
    private bool _HasInternet = false;
    public bool HasInternet { get { return _HasInternet; } }
    private bool _DatabaseLoaded = false;
    public bool DatabaseLoaded { get { return _DatabaseLoaded; } }
    public Animator protectScreen;

    IEnumerator Start()
    {
        _DatabaseLoaded = false;
        WWW www = new WWW("https://itunes.apple.com/us/rss/topfreeapplications/limit=20/json");
        yield return www;
        if (www.error == null)
        {
            StartCoroutine(ProcessSimpleJson(www.data));

            _HasInternet = true;
        }
        else
        {
            feed = DataStorage.LoadFromFIle<Feed>("ProductsDB");

            for (int i = 0; i < feed.entry.Count; i++)
                Managers.Instance.Canvas.CreateContent(feed.entry[i]);

            _HasInternet = false;
            _DatabaseLoaded = true;
            if (DatabaseLoaded && protectScreen != null)
            {
                protectScreen.SetTrigger("Done");
                Destroy(protectScreen.gameObject, .50f);
            }
        }
    }

    private IEnumerator ProcessSimpleJson(string data)
    {
        var N = JSON.Parse(data);
        feed.author.name.label = N["feed"]["author"]["name"]["label"];
        feed.author.uri.label = N["feed"]["author"]["uri"]["label"];

        JSONArray newEntry = N["feed"]["entry"].AsArray;

        for (int i = 0; i < newEntry.Count; i++)
        {
            List<ImImage> list = new List<ImImage>();
            for (int q = 0; q < newEntry[i]["im:image"].AsArray.Count; q++)
            {
                WWW w = new WWW(newEntry[i]["im:image"][q]["label"]);
                yield return w;

                if (w.isDone)
                {
                    Sprite tempSprite = Sprite.Create(w.texture, new Rect(0, 0, w.texture.width, w.texture.height), new Vector2(0.5f, 0.5f));
                    string oldString = newEntry[i]["im:image"][q]["label"];
                    string newString = oldString.Substring(oldString.Length- 50);
                    tempSprite.name = newString;
                    list.Add(new ImImage(DataStorage.WriteTextureToPlayerPrefs(tempSprite.name, w.texture), GetAttributes(newEntry[i]["im:image"][q]["attributes"].AsObject)));
                }                
            }

            Entry tempEntry = new Entry(
                    new ImName(newEntry[i]["im:name"]["label"]),
                    list,
                    new Summary(newEntry[i]["summary"]["label"]),
                    new ImPrice(newEntry[i]["im:price"]["label"], GetAttributes(newEntry[i]["im:price"]["attributes"].AsObject)),
                    new ImContentType(GetAttributes(newEntry[i]["im:contentType"]["attributes"].AsObject)),
                    new Rights(newEntry[i]["rights"]["label"]),
                    new Title(newEntry[i]["title"]["label"]),
                    new Link(GetAttributes(newEntry[i]["link"]["attributes"].AsObject)),
                    new Id(newEntry[i]["id"]["label"], GetAttributes(newEntry[i]["id"]["attributes"].AsObject)),
                    new ImArtist(newEntry[i]["im:artist"]["label"], GetAttributes(newEntry[i]["im:artist"]["attributes"].AsObject)),
                    new Category(GetAttributes(newEntry[i]["category"]["attributes"].AsObject)),
                    new ImReleaseDate(newEntry[i]["im:releaseDate"]["label"], GetAttributes(newEntry[i]["im:releaseDate"]["attributes"].AsObject))
                );
            feed.entry.Add(tempEntry);
            Managers.Instance.Canvas.CreateContent(tempEntry);
        }

        feed.updated = new Updated(N["feed"]["updated"]["label"]);
        feed.rights = new Rights(N["feed"]["rights"]["label"]);
        feed.title = new Title(N["feed"]["title"]["label"]);
        feed.icon = new Icon(N["feed"]["icon"]["label"]);

        JSONArray newLink = N["feed"]["link"].AsArray;
        for (int i = 0; i < newLink.Count; i++)
        {
            feed.link.Add(new Link(GetAttributes(N["feed"]["link"][i]["attributes"].AsObject)));
        }

        feed.id = new Id(N["feed"]["id"]["label"], GetAttributes(N["feed"]["id"]["attributes"].AsObject));

        _DatabaseLoaded = true;
        DataStorage.SaveToFile<Feed>(feed, "ProductsDB");
        if (DatabaseLoaded && protectScreen != null)
        {
            protectScreen.SetTrigger("Done");
            Destroy(protectScreen.gameObject, 1f);
        }
    }

    private Attributes GetAttributes(JSONClass array)
    {
        Attributes newAtt = new Attributes();

        if (array["label"] != null || array["label"] != string.Empty || array["label"] != "")
            newAtt.SetLabel(array["label"]);

        if (array["rel"] != null || array["rel"] != string.Empty || array["rel"] != "")
            newAtt.SetRel(array["rel"]);

        if (array["type"] != null || array["type"] != string.Empty || array["type"] != "")
            newAtt.SetType(array["type"]);

        if (array["href"] != null || array["href"] != string.Empty || array["href"] != "")
            newAtt.SetHref(array["href"]);

        if (array["schema"] != null || array["schema"] != string.Empty || array["schema"] != "")
            newAtt.SetSchema(array["schema"]);

        if (array["term"] != null || array["term"] != string.Empty || array["term"] != "")
            newAtt.SetTerm(array["term"]);

        if (array["imId"] != null || array["imId"] != string.Empty || array["imId"] != "")
            newAtt.SetImId(array["imId"]);

        if (array["imBundleId"] != null || array["imBundleId"] != string.Empty || array["imBundleId"] != "")
            newAtt.SetImBundleId(array["imBundleId"]);

        if (array["amount"] != null || array["amount"] != string.Empty || array["amount"] != "")
            newAtt.SetAmount(array["amount"]);

        if (array["currency"] != null || array["currency"] != string.Empty || array["currency"] != "")
            newAtt.SetCurrency(array["currency"]);

        if (array["height"] != null || array["height"] != string.Empty || array["height"] != "")
            newAtt.SetHeight(array["height"]);

        return newAtt;
    }

    private string CheckString(string data)
    {
        if (data == null || data == "" || data == string.Empty)
            return null;
        else
            return data;
    }
}