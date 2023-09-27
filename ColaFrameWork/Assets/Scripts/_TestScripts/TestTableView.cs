using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using ColaFramework;

public class TestTableView : MonoBehaviour
{

    private UITableView tableView;

    // Use this for initialization
    void Start()
    {
        InitTableView();
    }

    void InitTableView()
    {
        tableView = GetComponent<UITableView>();
        tableView.onCellInit = Refresh;
        tableView.Reload(true);
    }

    private void Refresh(UITableView tableView, UITableViewCell cell)
    {
        Text text = cell.cacheGameObject.GetComponentByPath<Text>("Text");
        text.text = "cell_" + cell.index;
    }
}
