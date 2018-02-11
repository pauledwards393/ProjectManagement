using System;
using System.Configuration;
using System.Data.SqlClient;
using ProjectManagement.Web;
using ProjectManagement.Web.ProjectTableAdapters;
using System.Data;

/// <summary>
/// Summary description for ProjectBLL
/// </summary>
public class ProjectBLL
{
    private ProjectTableAdapter _adapter = null;
    public ProjectTableAdapter Adapter
        
    {
        get
        {
            if(_adapter==null)
            {
                _adapter = new ProjectTableAdapter();
            }
            return _adapter;
        }
     
    }

    public ProjectBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public Project.ProjectDataTable GetDataBydepandStatus(int statusID, int depID)
    {
        Project.ProjectDataTable tabel = Adapter.GetDataByDepAndStatus(depID, statusID);
        return tabel;
    }
    
    public Project.ProjectDataTable GetDataByprojectCode(string projectcode)
    {
        Project.ProjectDataTable table = Adapter.GetDataByProjectCode(projectcode);
        foreach(Project.ProjectRow row in table.Rows)
        {
            row.Project_Code =
                ReplaceString(row.Project_Code, projectcode,
                              string.Format("<span style='background-color:yellow;'>{0}</span>", projectcode));

            row.AcceptChanges();
            table.AcceptChanges();

        }
        return table;

    }
    public Project.ProjectDataTable GetData()
    {
        Project.ProjectDataTable table = Adapter.GetData();
        return Adapter.GetData();
    }

    public Project.ProjectDataTable GetProjects(Int32? statusId, Int32? departmentId, Int32? sectorId, string projectSearchText)
    {
        return Adapter.GetProjects(statusId, departmentId, sectorId, projectSearchText);
    }

    public  int InsertBasics(string _pCode, int _status, int _dep, double _lat, double _lon, string _projectName)
    {
        int row = -1;
        row = Adapter.InsertBasics(_status, _dep, _lat,_lon, false,_projectName,_pCode, DateTime.Now);

        return row;
    }

    public Project.ProjectDataTable GetDataByStatus(int status_id)
    {
        return Adapter.GetDataByStatus(status_id);
    }

    public Project.ProjectDataTable GetSectors(int project_id)
    {
        return Adapter.GetSectors(project_id);
    }

    public  Project.ProjectDataTable getDatabyDepartment(int dep_id)
    {
        return Adapter.GetDataByDepartment(dep_id);
    }

    public void updateLatLng(int _projectid, double _lat, double _lon){
        Adapter.UpdateLatLng(_projectid, _lat, _lon);
    }

    public int InsertProject(string _projectcode, string _strstartdate,
                             string _strenddate, int _status, string _contact,
                                 string _address,string _city, int _department,
                            string _description, double _lat, double _lon)
    {
        DateTime _startdate = new DateTime();
        DateTime _enddate = new DateTime();
        if(!string.IsNullOrEmpty(_strstartdate))
        {
            try
            {
                string[] StartDateArray= _strstartdate.Split('/'); 
                int Day = int.Parse(StartDateArray[0]);
                int month = int.Parse(StartDateArray[1]);
                int year = int.Parse(StartDateArray[2]);
                _startdate = new DateTime(year,month,Day);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }
        else
        {
            _startdate=DateTime.MinValue;
        }
        if(!string.IsNullOrEmpty(_strenddate))
        {
            try
            {
                string[] EndDateArray = _strenddate.Split('/');
                int Day = int.Parse(EndDateArray[0]);
                int month = int.Parse(EndDateArray[1]);
                int year = int.Parse(EndDateArray[2]);
                _enddate = new DateTime(year,month,Day);
            }

            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
            
        }
        else
        {
            _enddate=DateTime.MaxValue;
        }
        int affectRow = Adapter.InsertQuery(_projectcode, _startdate, _enddate, _status, _contact, _address, _city, _department,
                            _description, _lat, _lon);
        return affectRow;
    }


    //public int updateProject (string _projectCode, int _status, string _address, string _city, int _dep, string _sectors, string _descript, int _projectID, string _projectName, DateTime? _startDate, DateTime? _endDate, string _contact, string _authority,string _ProejctManager,
    //    int countyId, int planningAuthorityId)
    //{
    //    Project.ProjectDataTable table = Adapter.GetProjectByID((short)_projectID);
    //    Project.ProjectRow row = (Project.ProjectRow)table.Rows[0];
    //    Adapter.UpdateQuery(_projectCode, _projectName, _startDate, _endDate, _contact, _address, _city, _descript, true,
    //                        _authority, _status, _ProejctManager, _dep, _projectID, countyId, planningAuthorityId);
    //    Adapter.ClearSectors(_projectID);
    //    string[] sectors = _sectors.Split(',');

    //    foreach (string _sectorID in sectors)
    //    {
    //        Adapter.AddSector(_projectID, Convert.ToInt32(_sectorID));
    //    }

    //    return 1;
    //}

    public int UpdateProject(ProjectManagement.Web.Models.Project project)
    {   
        string connectionString = ConfigurationManager.ConnectionStrings["MBProjectConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try {
                var cmd = new SqlCommand
                {
                    Connection = conn
                };

                conn.Open();

                // Update project
                cmd.CommandText = "UPDATE Project " +
                    "SET[Project Code] = @projectcode, [Project Name] = @projectname, StartDate = @startdate, EndDate = @enddate, Contact = @Contact, Address = @address, City = @city, Description = @description, Detailed = @detailed, " +
                    "Authority = @authority, StatusID = @StatusID, ProjectManager = @ProjectManager, DepartmentID = @DepartmentID, CountyId = @CountyId, PlanningAuthorityId = @PlanningAuthorityId " +
                    "WHERE(Project_ID = @project_id)";

                cmd.Parameters.AddWithValue("@project_id", project.Id);
                cmd.Parameters.AddWithValue("@projectcode", project.Code);
                cmd.Parameters.AddWithValue("@projectname", project.Name);
                cmd.Parameters.AddWithValue("@startdate", project.StartDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@enddate", project.EndDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Contact", project.Contact);
                cmd.Parameters.AddWithValue("@address", project.Address);
                cmd.Parameters.AddWithValue("@city", project.City);
                cmd.Parameters.AddWithValue("@description", project.Description);
                cmd.Parameters.AddWithValue("@detailed", project.Detailed);
                cmd.Parameters.AddWithValue("@authority", project.Authority);
                cmd.Parameters.AddWithValue("@StatusID", project.Status);
                cmd.Parameters.AddWithValue("@ProjectManager", project.ProjectManager);
                cmd.Parameters.AddWithValue("@DepartmentID", project.Department);
                cmd.Parameters.AddWithValue("@CountyId", project.CountyId);
                cmd.Parameters.AddWithValue("@PlanningAuthorityId", project.PlanningAuthorityId);
                                
                cmd.ExecuteNonQuery();
                
                // Delete current sectors associated with the project
                cmd.CommandText = "DELETE FROM ProjectSector WHERE Project_ID = @project_id";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@project_id", project.Id);

                cmd.ExecuteNonQuery();

                // Add new sectors
                var sectors = project.Sectors.Split(',');
                
                foreach (var sectorId in sectors)
                {
                    cmd.CommandText = "INSERT INTO ProjectSector (Project_ID, Sector_ID) VALUES (@project_id, @sector_id)";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@project_id", project.Id);
                    cmd.Parameters.AddWithValue("@sector_id", Convert.ToInt32(sectorId));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        return 1;
    }

    public void updateProjectStatus(int _projectID, int _status)
    {
        Adapter.UpdateStatus(_projectID, _status);
    }

    public void UpdateProjectSubmissions(Int32 projectId, DateTime? jobSheetSubmitted, DateTime? feeProposalSubmitted, DateTime? acceptanceOfServiceSubmitted)
    {
        Adapter.UpdateProjectSubmissions(jobSheetSubmitted, feeProposalSubmitted, acceptanceOfServiceSubmitted, projectId);
    }

    public Project.ProjectDataTable GetDataByID(string projectID)
    {
        return Adapter.GetProjectByID(Int16.Parse(projectID));

    }
    public static string ReplaceString(string strSource, string strReplace, string strReplaceWith)
    {
        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(strReplace, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        return r.Replace(strSource, strReplaceWith);
    }

    public void DeleteProject(int projectID)
    {
        Adapter.DeleteProject(projectID);
    }

    public bool ValidateProjectCode(int? projectId, string projectCode)
    {      
        bool isValid = true;

        String sql = "SELECT CASE WHEN NOT EXISTS " +
                                "(SELECT * FROM Project WHERE [Project Code] = @ProjectCode AND Project_ID != ISNULL(@ProjectId, 0) AND IsDeleted = 0) " +
                                "THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsValid";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MBProjectConnectionString"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ProjectId", ((object)projectId) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProjectCode", projectCode);           

            try
            {
                conn.Open();
                isValid = (bool)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }                     
        }

        return isValid;
    }
}
