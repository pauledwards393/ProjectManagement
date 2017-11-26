using ProjectManagement.Web;
using ProjectManagement.Web.ProjectTableAdapters;

/// <summary>
/// Summary description for DepartmentBLL
/// </summary>
public class DepartmentBLL
{
    private DepartmentTableAdapter _adapter = null;
    public DepartmentTableAdapter Adapter
    {
        get
        {
            if (_adapter == null)
            {
                return new DepartmentTableAdapter();
            }
            return _adapter;
        }
        set
        {
            _adapter = value;
        }
    }
    public DepartmentBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Project.DepartmentDataTable GetData()
    {
        return Adapter.GetData();
    }
}
