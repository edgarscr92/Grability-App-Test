using UnityEngine;

public class CategoryManager : MonoBehaviour
{
    public string _name = string.Empty;
    private int TotalEntry = 0;
    public bool LimitReached = false;
    private EntryManager entryManager;

    void Awake()
    {
        entryManager = FindObjectOfType<EntryManager>() as EntryManager;
    }

    public void UpdateData()
    {
        if (LimitReached)
            return;

        TotalEntry++;

        if (TotalEntry >= entryManager.PageLimit)
            LimitReached = true;
    }
}