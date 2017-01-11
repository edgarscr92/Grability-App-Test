using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public EntryData entryPrefab;
    public int PageLimit;
    private bool AllLimitReached = false;
    // List 
    public List<EntryData> Entrys = new List<EntryData>();
    private List<EntryData> cleanEntry = new List<EntryData>();
    private List<CategoryManager> categoryManagers = new List<CategoryManager>();

    public void CreateEntry(Entry data, Transform spawn, CategoryManager categoryManager)
    {
        if (!categoryManagers.Contains(categoryManager))
            categoryManagers.Add(categoryManager);

        EntryData tempEntry = Instantiate(entryPrefab, spawn.position, Quaternion.identity) as EntryData;
        tempEntry.transform.SetParent(spawn);
        tempEntry.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        tempEntry.name = data.imName.label;

        tempEntry.SetData(data);
        Entrys.Add(tempEntry);

        categoryManager.UpdateData();
    }

    private void CreateCleanEntry(Transform spawn, CategoryManager categoryManager)
    {
        if (categoryManager.GetComponent<UnityEngine.UI.Extensions.ReorderableList>())
            spawn = categoryManager.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().ContentLayout.transform;
        else
            spawn = categoryManager.transform;

         EntryData tempEntry = Instantiate(entryPrefab, spawn.position, Quaternion.identity) as EntryData;
        tempEntry.transform.SetParent(spawn);
        tempEntry.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        tempEntry.name = "";

        tempEntry.SetCleanData();
        cleanEntry.Add(tempEntry);

        categoryManager.UpdateData();
    }

    void Update()
    {
        if (AllLimitReached == true) return;

        if (Managers.Instance.productManager.DatabaseLoaded)
        {
            for (int i = 0; i < categoryManagers.Count; i++)
            {
                if(!categoryManagers[i].LimitReached)
                {
                    CreateCleanEntry(null, categoryManagers[i]);
                }
            }
        }
    }
}