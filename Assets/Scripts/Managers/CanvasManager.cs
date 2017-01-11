using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CanvasManager : MonoBehaviour
{ 
    #region Content
    [System.Serializable]
    public class ContentData
    {
        public EntryManager manager;
        public GameObject area;
        public ScrollSnapBase _top;
        public Transform _content;
        public ProducDetails productDetails;
        [Header("Classes")]
        public ReorderableList corridors;
        public ScrollSnapBase snapBase;
        public VerticalLayoutGroup Listas;
        public GridLayoutGroup Grillas;        
    }
    public ContentData phoneContent, tabletContent;

    #endregion

    // Prefabs
    [System.Serializable]
    public class CanvasPrefabs
    {
        public Button categoryPrefab;
        public Button categoryGroupTitle;
    }
    public CanvasPrefabs phonePrefabs, tabletPrefabs;
    private CanvasPrefabs prefabs;

    // Private vars
    private List<GameObject> categoryLayouts = new List<GameObject>();
    private List<string> categoryNames = new List<string>();
    private List<Button> _categories = new List<Button>();

    [HideInInspector]
    public int CurrentGridID = 0;

    private ContentData _contentData;
    public ContentData contentData { get { return _contentData; } }

    private int CategoryGroupButtonCount = -1;

    void Start()
    {
        if (Managers.Instance.IsPhone)
        {
            _contentData = phoneContent;
            prefabs = phonePrefabs;
            tabletContent.area.SetActive(false);
        }
        else
        {
            _contentData = tabletContent;
            prefabs = tabletPrefabs;
            phoneContent.area.SetActive(false);
        }

        UnityEngine.Events.UnityAction HideCorridors = () => { _contentData.corridors.GetComponent<Animator>().SetTrigger("Hide"); };
        GameObject.Find("HideCorridor").GetComponent<Button>().onClick.AddListener(HideCorridors);
    }

    void Update()
    {
        if (!Managers.Instance.productManager.DatabaseLoaded) return;

        if (_contentData.snapBase.CurrentPage != CurrentGridID)
        {
            CurrentGridID = _contentData.snapBase.CurrentPage;
            _contentData._top.GoToScreen(CurrentGridID);
        }
    }

    public void CreateContent(Entry _entry)
    {
        GameObject spawnPosition = null;
        GameObject tempGrid = null;

        if (categoryNames.Contains(_entry.category.attributes.label))
        {
            _contentData.snapBase.GetComponent<HorizontalScrollSnap>().UpdateLayout();
            _contentData._top.GetComponent<HorizontalScrollSnap>().UpdateLayout();

            for (int i = 0; i < categoryNames.Count; i++)
            {
                if (categoryNames[i] == _entry.category.attributes.label)
                {
                    if (_contentData.Grillas != null)
                    {
                        // ReorderableList list = categoryLayouts[i].GetComponent<ReorderableList>();
                        // Añadiendo entry
                        // spawnPosition = list.ContentLayout.gameObject;    
                        spawnPosition = categoryLayouts[i].gameObject;
                    }
                    else if (_contentData.Listas != null)
                    {
                        spawnPosition = categoryLayouts[i].gameObject;
                    }
                    tempGrid = categoryLayouts[i];
                }
            }
        }
        else
        {
            // Creando Grids si no existen
            GameObject prefab = null;
            if(_contentData.Grillas != null)
                prefab = _contentData.Grillas.gameObject;
            else if(_contentData.Listas != null)
                prefab = _contentData.Listas.gameObject;

            tempGrid = Instantiate(prefab, _contentData._content.position, Quaternion.identity) as GameObject;
            tempGrid.transform.SetParent(_contentData._content);
            tempGrid.GetComponent<CategoryManager>()._name = _entry.category.attributes.label;

            // Añadiendo grids al horizontal
            if (_contentData.snapBase != null)
            {
                _contentData._top.StartingScreen = CurrentGridID;
                _contentData.snapBase.StartingScreen = CurrentGridID;

                _contentData.snapBase.ChildObjects.Add(tempGrid);
            }

            // Ajustando scala y posicion del grid
            RectTransform rectTransform = tempGrid.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1,1,1);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            tempGrid.name = _entry.category.attributes.label;

            // Añadiendo a listados
            categoryNames.Add(tempGrid.name);            
            categoryLayouts.Add(tempGrid);

            // Creando botones de Categorias
            #region Category Buttons Function
            Button tempButton = Instantiate(prefabs.categoryPrefab, _contentData._top.GetComponent<ScrollRect>().content.position, Quaternion.identity) as Button;
            tempButton.transform.SetParent(_contentData._top.GetComponent<ScrollRect>().content);

            _contentData._top.ChildObjects.Add(tempButton.gameObject);

            tempButton.name = "Title: " + tempGrid.name;

            RectTransform tempButtonRect = tempButton.GetComponent<RectTransform>();
            tempButtonRect.localScale = new Vector3(1, 1, 1);
            tempButtonRect.sizeDelta = Vector2.zero;
            tempButtonRect.anchoredPosition = Vector2.zero;

            RectTransform tempButtonTextRect = tempButton.GetComponentInChildren<RectTransform>();
            tempButtonTextRect.localScale = new Vector3(1, 1, 1);
            tempButtonTextRect.sizeDelta = Vector2.zero;
            tempButtonTextRect.anchoredPosition = Vector2.zero;

            _categories.Add(tempButton);
            tempButton.GetComponentInChildren<Text>().text = tempGrid.name;

            UnityEngine.Events.UnityAction ShowCorridors = () => { _contentData.corridors.GetComponent<Animator>().SetTrigger("Show"); };
            tempButton.onClick.AddListener(ShowCorridors);

            Button corridorButton = Instantiate(prefabs.categoryGroupTitle, _contentData.corridors.ContentLayout.transform.position, Quaternion.identity) as Button;
            CategoryGroupButtonCount++;

            corridorButton.name = CategoryGroupButtonCount.ToString();

            UnityEngine.Events.UnityAction GoToCategory = () => {
                _contentData.snapBase.GoToScreen(System.Convert.ToInt16(corridorButton.name));
                _contentData.corridors.GetComponent<Animator>().SetTrigger("Hide");
            };
            
            corridorButton.onClick.AddListener(GoToCategory);

            corridorButton.transform.SetParent(_contentData.corridors.ContentLayout.transform);            

            RectTransform tempCorridoRect = corridorButton.GetComponent<RectTransform>();
            tempCorridoRect.localScale = new Vector3(1, 1, 1);
            tempCorridoRect.sizeDelta = new Vector2(0, 80);
            tempCorridoRect.anchoredPosition = Vector2.zero;

            RectTransform tempCorridoTextRect = corridorButton.GetComponentInChildren<RectTransform>();
            tempCorridoTextRect.localScale = new Vector3(1, 1, 1);
            tempCorridoTextRect.GetComponentInChildren<Text>().text = tempGrid.name;

            #endregion
            if (_contentData.Grillas != null)
            {
                // tempGrid.GetComponent<ReorderableList>().DraggableArea = GetComponent<RectTransform>();
                // Añadiendo entry
                // spawnPosition = tempGrid.GetComponent<ReorderableList>().ContentLayout.gameObject;
                spawnPosition = tempGrid;
            }
            else if (_contentData.Listas != null)
            {
                spawnPosition = tempGrid;
            }            
        }

        contentData.manager.CreateEntry(_entry, spawnPosition.transform, tempGrid.GetComponent<CategoryManager>());
    }
}