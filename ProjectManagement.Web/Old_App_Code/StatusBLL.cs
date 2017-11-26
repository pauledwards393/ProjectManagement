using ProjectManagement.Web;
using ProjectManagement.Web.ProjectTableAdapters;

/// <summary>
/// Summary description for StatusBLL
/// </summary>
public class StatusBLL
{
	public StatusBLL()
	{
	}

    private statusTableAdapter _adapter;
    public statusTableAdapter Adapter
    {
        get
        {
            if(_adapter!=null)
            {
                return _adapter;
            }
            else
            {
                return new statusTableAdapter();
            }
        }
        set
        {
            _adapter= value;
        }
    }

    public  Project.statusDataTable GetData()
    {
        return Adapter.GetData();
    }
}
