using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;
using System.Collections.Generic;
using System.Net.Mail;
using System.Data.OleDb;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Table = iTextSharp.text.Table;
using ProjectManagement.Web;
using ProjectManagement.Web.Providers;
using Newtonsoft.Json;
using System.Linq;

public partial class Detail : System.Web.UI.Page
{
    private ProjectBLL projectBLL = new ProjectBLL();

    const String ExcelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=No;'";

    protected Boolean HasJobSheet
    {
        get
        {
            return ViewState["HasJobSheet"] != null && (Boolean)ViewState["HasJobSheet"];
        }
        set
        {
            ViewState["HasJobSheet"] = value;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(Button1);

        if (!IsPostBack)
        {
            var mode = Request.Params["Mode"];
            
            switch (mode)
            {
                case "View":
                    DetailsView2.ChangeMode(DetailsViewMode.ReadOnly);
                    break;
                case "Entry":
                    DetailsView2.ChangeMode(DetailsViewMode.Edit);
                    break;
                default:
                    DetailsView2.ChangeMode(DetailsViewMode.Insert);
                    DetailsView2.FooterRow.Visible = false;
                    break;
            }

            DetailsView2Databinding();
        }
        
        lblEmailSuccess.Visible = lblSaveError.Visible = false;        
    }

    private void DetailsView2Databinding()
    {
        string ProjectID = Request.Params["ProjectID"];

        if (!string.IsNullOrEmpty(ProjectID))
        {
            Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);

            DetailsView2.DataSource = p;

            GenerateJobSheet(p);

            Dictionary<String, Int16> FieldColumns = new Dictionary<String, Int16>();
            FieldColumns.Add( "AddedAt", 34 );
            FieldColumns.Add( "JobSheetSubmitted", 35 );
            FieldColumns.Add( "FeeProposalSubmitted", 36 );
            FieldColumns.Add( "AcceptanceOfServiceSubmitted", 37 );

            foreach (KeyValuePair<String, Int16> item in FieldColumns)
                DetailsView2.Fields[item.Value].Visible = p.Rows[0][item.Key] != DBNull.Value;

            DetailsView2.DataBind();

            HasJobSheet = p.Rows[0]["JobSheetSubmitted"] != DBNull.Value;
            JobSheetMandatoryMarker.Visible = !HasJobSheet;
        }

        DetailsView2.CssClass = DetailsView2.CurrentMode.ToString().ToLower();
    }

    protected void DetailsView2_DataBound(object sender, EventArgs e)
    {
        if (DetailsView2.CurrentMode == DetailsViewMode.Insert)
        {
            var lblLat = (Label)DetailsView2.FindControl("LblLat");
            var lblLng = (Label)DetailsView2.FindControl("LblLng");

            lblLat.Text = Request.Params["lat"];
            lblLng.Text = Request.Params["lng"];
        }
    }

    private void GenerateJobSheet(Project.ProjectDataTable p)
    {

        Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;
        TxtJobCode.Text = "";
        TxtManager.Text = "";
        TxtDetails.Text = "";
        TxtAddress.Text = "";
        TxtJobCode.Text = row.Project_Code;
        if (!row.IsContactNull())
        {
            TxtContact.Text = row.Contact;
        }
        TxtDate.Text = DateTime.Today.ToShortDateString();
        if (!row.IsDescriptionNull())
            TxtDetails.Text = row.Description;

        if (!row.IsAddressNull())
        {
            TxtAddress.Text = row.Address;
        }
        if (!row.IsCityNull())
        {
            TxtAddress.Text += "\n" + row.City;
        }
        if (!row.IsProjectManagerNull())
        {
            TxtManager.Text = row.ProjectManager;
        }


    }

    protected void DetailsView2_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        DetailsView2.ChangeMode(e.NewMode);
        DetailsView2Databinding();
    }

    protected void DetailsView2_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        DetailsView2.ChangeMode(DetailsViewMode.ReadOnly);
        DetailsView2.DataBind();
    }

    protected void DDLSector_DataBound(object sender, EventArgs e)
    {
        Label lblSector = (Label)DetailsView2.FindControl("lblSector");
        System.Web.UI.WebControls.ListBox ddlSector = (System.Web.UI.WebControls.ListBox)DetailsView2.FindControl("ddlSector");
        string[] arrSectors = lblSector.Text.Split(',');
        foreach (string szSector in arrSectors)
        {
            for (int i = 0; i < ddlSector.Items.Count; i++)
            {
                if (ddlSector.Items[i].Text == szSector.Trim())
                    ddlSector.Items[i].Selected = true;
            }
        }
    }

    private string GetLabelFieldValue(string name)
    {
        return ((Label)DetailsView2.FindControl(name)).Text.Trim();
    }

    private string GetTextFieldValue(string name)
    {
        return ((TextBox)DetailsView2.FindControl(name)).Text.Trim();
    }

    private ProjectManagement.Web.Models.Project PopulateProject()
    {
        Label LblId = (Label)DetailsView2.FindControl("LBLProjectID");
        TextBox TxtProjectname = (TextBox)DetailsView2.FindControl("TxtPorjectname");
        DropDownList DDLstatus = (DropDownList)DetailsView2.FindControl("DDlstatus");
        DropDownList DDLDepartment = (DropDownList)DetailsView2.FindControl("DDLDepartment");
        TextBox TxtContact = (TextBox)DetailsView2.FindControl("TxtContact");
        TextBox TxtCity = (TextBox)DetailsView2.FindControl("TxtCity");
        CheckBox ChkDetailed = (CheckBox)DetailsView2.FindControl("ChkDetailed");
        TextBox TxtDescription = (TextBox)DetailsView2.FindControl("TxtDescription");
        TextBox TxtProjectManager = (TextBox)DetailsView2.FindControl("TxtManager");
        DropDownList DDLCounty = (DropDownList)DetailsView2.FindControl("DDLCounty");
        TextBox TxtPlanningAuthority = (TextBox)DetailsView2.FindControl("txtPlanningAuthority");
        Label LblLat = (Label)DetailsView2.FindControl("LblLat");
        Label LblLng = (Label)DetailsView2.FindControl("LblLng");

        // Client address
        var clientAddressId = GetLabelFieldValue("LblClientAddressId");
        var clientAddress = new ProjectManagement.Web.Models.Address
        {
            AddressLine1 = GetTextFieldValue("TxtClientAddressLine1"),
            AddressLine2 = GetTextFieldValue("TxtClientAddressLine2"),
            CompanyName = GetTextFieldValue("TxtClientCompanyName"),
            County = GetTextFieldValue("TxtClientCounty"),
            Id = !string.IsNullOrWhiteSpace(clientAddressId) ? int.Parse(clientAddressId) : (int?)null,
            Postcode = GetTextFieldValue("TxtClientPostcode"),
            TownOrCity = GetTextFieldValue("TxtClientTownOrCity")
        };

        // Invoice address
        var invoiceAddressId = GetLabelFieldValue("LblInvoiceAddressId");
        var invoiceAddress = new ProjectManagement.Web.Models.Address
        {
            AddressLine1 = GetTextFieldValue("TxtInvoiceAddressLine1"),
            AddressLine2 = GetTextFieldValue("TxtInvoiceAddressLine2"),
            CompanyName = GetTextFieldValue("TxtInvoiceCompanyName"),
            County = GetTextFieldValue("TxtInvoiceCounty"),
            Id = !string.IsNullOrWhiteSpace(invoiceAddressId) ? int.Parse(invoiceAddressId) : (int?)null,
            Postcode = GetTextFieldValue("TxtInvoicePostcode"),
            TownOrCity = GetTextFieldValue("TxtInvoiceTownOrCity")
        };

        // Project
        var project = new ProjectManagement.Web.Models.Project
        {
            Address = GetTextFieldValue("TxtAddress"),
            City = TxtCity.Text,
            ClientAddress = clientAddress, // New
            Code = GetTextFieldValue("TxtProjectCode"),
            Contact = TxtContact.Text,
            CountyId = int.Parse(DDLCounty.SelectedValue),
            Department = int.Parse(DDLDepartment.SelectedValue),
            Description = TxtDescription.Text,
            Detailed = ChkDetailed.Checked,
            Id = !string.IsNullOrWhiteSpace(LblId.Text) ? int.Parse(LblId.Text) : (int?)null,
            Introducer = GetTextFieldValue("TxtIntroducer"), // New
            InvoiceAddress = invoiceAddress, // New
            InvoiceContact = GetTextFieldValue("TxtInvoiceContact"), // New
            Latitude = !string.IsNullOrWhiteSpace(LblLat.Text) ? double.Parse(LblLat.Text) : (double?)null,
            Longitude = !string.IsNullOrWhiteSpace(LblLng.Text) ? double.Parse(LblLng.Text) : (double?)null,
            Name = TxtProjectname.Text,
            PlanningAuthorityId = int.Parse(TxtPlanningAuthority.Text),
            ProjectManager = TxtProjectManager.Text,
            Status = int.Parse(DDLstatus.SelectedValue),
        };

        TextBox TxtStartDate = (TextBox)DetailsView2.FindControl("TxtStartdate");
        TextBox TxtEndDate = (TextBox)DetailsView2.FindControl("TxtEndDate");

        if (!string.IsNullOrEmpty(TxtStartDate.Text))
        {
            string[] date = TxtStartDate.Text.Split('/');
            project.StartDate = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));

        }
        if (!string.IsNullOrEmpty(TxtEndDate.Text))
        {
            string[] date = TxtEndDate.Text.Split('/');
            project.EndDate = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));
        }

        ListBox DDLSector = (ListBox)DetailsView2.FindControl("DDLSector");

        project.Sectors = DDLSector
            .GetSelectedIndices()
            .Select(index => Convert.ToInt32(DDLSector.Items[index].Value))
            .ToList();

        return project;
    }

    protected void DetailsView2_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        var project = PopulateProject();

        if (project.Latitude == null || project.Longitude == null) return;

        if (!projectBLL.ValidateProjectCode(project.Id, project.Code))
        {
            lblSaveError.Text = "* Your chosen project code already exists, please choose another.";
            lblSaveError.Visible = true;
            return;
        }

        var message = projectBLL.AddOrUpdateProject(project);

        if (string.IsNullOrWhiteSpace(message))
        {
            RedirectToMap();
        }
        else
        {
            lblSaveError.Text = "* " + message;
            lblSaveError.Visible = true;
        }
    }
    
    protected void DetailsView2_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        var project = PopulateProject();

        if (!projectBLL.ValidateProjectCode(project.Id, project.Code))
        {
            lblSaveError.Text = "* Your chosen project code already exists, please choose another.";
            lblSaveError.Visible = true;
            return;
        }

        var message = projectBLL.AddOrUpdateProject(project);

        if (string.IsNullOrWhiteSpace(message))
        {
            DetailsView2.ChangeMode(DetailsViewMode.ReadOnly);
            DetailsView2Databinding();
        } else
        {
            lblSaveError.Text = "* " + message;
            lblSaveError.Visible = true;
        }
    }

    protected void DetailsView2_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "GoBack")
        {
            RedirectToMap();
        }
    }
    
    protected void DeleteProject_Click(object sender, EventArgs e)
    {
        String projectId = ((Label)DetailsView2.FindControl("LBLProjectID")).Text;
        projectBLL.DeleteProject(Int32.Parse(projectId));
        Response.Redirect("map.aspx?delete=1");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Document doc = new Document();
        string path = Server.MapPath("PDFs");

        PdfWriter.GetInstance(doc, new FileStream(path + "/jobsheet.pdf", FileMode.Create));
        doc.Open();

        Table table = new Table(4);
        table.TableFitsPage = true;
        table.CellsFitPage = true;
        table.BorderWidthBottom = 1;
        table.BorderWidthLeft = 1;
        table.BorderWidthRight = 1;
        table.BorderWidthTop = 1;

        table.BorderColor = Color.BLACK;
        table.Cellpadding = 2;
        table.Cellspacing = 2;

        Cell cell = new Cell("NEW PROJECT DETAILS"); cell.Header = true;
        cell.SetHorizontalAlignment("Center");
        cell.Colspan = 4;
        table.AddCell(cell);

        cell = new Cell("JOB DETAILS"); cell.Header = true;
        cell.BackgroundColor = Color.GRAY;
        cell.SetHorizontalAlignment("Center");
        cell.Colspan = 4;
        table.AddCell(cell);
        // row 2
        //cell = new Cell("Job Status");
        //cell.BackgroundColor = Color.LIGHT_GRAY;
        //table.AddCell(cell);
        // row 3
        cell = new Cell("Job Code");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtJobCode.Text);
        table.AddCell(cell);

        cell = new Cell("Job No");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtJobNo.Text);
        table.AddCell(cell);

        // row 4
        cell = new Cell("Job Details");
        cell.Colspan = 2;
        cell.SetHorizontalAlignment("Center");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);



        cell = new Cell("for");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(Txtfor.Text);
        table.AddCell(cell);

        // row 6
        cell = new Cell(TxtDetails.Text);
        cell.Colspan = 4;
        table.AddCell(cell);

        // row 7
        cell = new Cell("Client Details");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        cell.Header = true;
        cell.SetHorizontalAlignment("Center");
        cell.Colspan = 2;
        table.AddCell(cell);



        cell = new Cell("Ref");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtRef.Text);
        table.AddCell(cell);

        // row 6

        cell = new Cell("Client");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtClient.Text);

        table.AddCell(cell);

        cell = new Cell("Order No");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtOrderNo.Text);
        table.AddCell(cell);

        // row 7

        cell = new Cell("Contact");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtContact.Text);
        table.AddCell(cell);

        cell = new Cell("Invoice Contact");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceContact.Text);
        table.AddCell(cell);

        //row8
        cell = new Cell("Address");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(true);
        table.AddCell(cell);

        cell = new Cell("Invoice Address");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(true);
        table.AddCell(cell);

        // row 9 
        cell = new Cell(TxtAddress.Text);
        cell.Colspan = 2;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceAddress.Text);
        cell.Colspan = 2;
        table.AddCell(cell);

        // row 10

        cell = new Cell("Tel");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtTel.Text);
        table.AddCell(cell);

        cell = new Cell("Tel");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceTel.Text);
        table.AddCell(cell);

        // row 11
        cell = new Cell("Fax");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtFax.Text);
        table.AddCell(cell);

        cell = new Cell("Fax");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceFax.Text);
        table.AddCell(cell);

        // row 12

        cell = new Cell("CONTRACT/ORDER DETAILS");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        cell.Colspan = 4;
        cell.SetHorizontalAlignment("Center");
        cell.Header = true;
        table.AddCell(cell);

        //row 13
        cell = new Cell("Estimated Hours");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtHours.Text);
        table.AddCell(cell);

        cell = new Cell("Estimated Completion Date or Final Invoice Date");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtCompletionDate.Text);
        table.AddCell(cell);

        //row 14
        cell = new Cell("Estimated Fee/OrderValue");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtOrderValue.Text);
        table.AddCell(cell);

        cell = new Cell(true);
        table.AddCell(cell);
        cell = new Cell(true);
        table.AddCell(cell);

        //row 15
        cell = new Cell("Variation Orders");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        cell.Colspan = 4;
        cell.SetHorizontalAlignment("Center");
        table.AddCell(cell);

        //row 16
        cell = new Cell(TxtVariationOrders.Text);
        cell.Colspan = 4;
        table.AddCell(cell);

        // row 17

        cell = new Cell("Project Manager");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtManager.Text);
        table.AddCell(cell);

        cell = new Cell("Director");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtDirector.Text);
        table.AddCell(cell);

        // row 18

        cell = new Cell(true);
        cell.Colspan = 2;
        table.AddCell(cell);


        cell = new Cell("Date");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);
        cell = new Cell(TxtDate.Text);
        table.AddCell(cell);

        try
        {
            doc.Add(table);
        }
        catch (Exception ex)
        {
            //Display parser errors in PDF.
            //Parser errors will also be wisible in Debug.Output window in VS
            Paragraph paragraph = new Paragraph("Error! " + ex.Message);
            paragraph.SetAlignment("center");
            Chunk text = paragraph.Chunks[0] as Chunk;

            if (text != null)
            {
                text.Font.Color = Color.RED;
            }
            doc.Add(paragraph);

        }
        finally
        {
            doc.Close();
            //Response.ContentType = "application/pdf";
            //Response.Redirect("PDFs/jobsheet.pdf");
            //Response.End();
            //ClientScriptManager cs = this.ClientScriptManager();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenPDF", "window.open('PDFs/Jobsheet.pdf','_blank');", true);


        }
    }

    protected void UploadJobSheet_Click(object sender, EventArgs e)
    {
        Boolean jobSheetIncluded = FileJobSheet.PostedFile.ContentLength > 0;
        String submittedBy = SubmittedBy.Text.Trim();

        if ((!HasJobSheet && !jobSheetIncluded) || String.IsNullOrEmpty(submittedBy))
            return;
        
        Boolean originalFeeProposalIncluded = FileOriginalFeeProposal.PostedFile.ContentLength > 0;
        Boolean acceptanceOfServiceIncluded = FileAcceptanceOfService.PostedFile.ContentLength > 0;

        System.Collections.Generic.List<Attachment> attachments = new System.Collections.Generic.List<Attachment>();

        if (jobSheetIncluded)
            attachments.Add(new Attachment(FileJobSheet.PostedFile.InputStream, FileJobSheet.PostedFile.FileName));

        if (originalFeeProposalIncluded)
            attachments.Add(new Attachment(FileOriginalFeeProposal.PostedFile.InputStream, FileOriginalFeeProposal.PostedFile.FileName));

        if (acceptanceOfServiceIncluded)
            attachments.Add(new Attachment(FileAcceptanceOfService.PostedFile.InputStream, FileAcceptanceOfService.PostedFile.FileName));

        String comments = ProjectDetailsComments.Text.Trim();

        // Don't send email if no information has been provided
        if (attachments.Count == 0 && String.IsNullOrEmpty(comments)) return;

        String[] bodyParameters = { submittedBy, comments };

        EmailService.SendEmail("jobSheet", bodyParameters, attachments);

        // Update database with submission dates
        String projectId = ((Label)DetailsView2.FindControl("LBLProjectID")).Text;
        DateTime? now = DateTime.Now;
        projectBLL.UpdateProjectSubmissions(Int32.Parse(projectId), jobSheetIncluded ? now : null, originalFeeProposalIncluded ? now : null, acceptanceOfServiceIncluded ? now : null);

        DetailsView2Databinding();

        lblEmailSuccess.Visible = true;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    private List<string> GetAddressFieldNames(string prefix, Project.ProjectRow row)
    {
        return new string[] {
            prefix + "CompanyName",
            prefix + "AddressLine1",
            prefix + "AddressLine2",
            prefix + "TownOrCity",
            prefix +  "County",
            prefix + "Postcode"
        }
        .Select(field => row[field].ToString())
        .Where(field => !string.IsNullOrWhiteSpace(field))
        .ToList();
    }

    protected void BtnSheet_Click(object sender, EventArgs e)
    {
        string ProjectID = Request.QueryString["projectID"];
        Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);
        Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;

        String originalJobSheetPath = Server.MapPath("JobSheet_2017-08-08.xls");
        String modifiedJobSheetPath = Server.MapPath("newJobsheet1.xls");

        File.Copy(originalJobSheetPath, modifiedJobSheetPath, true);

        String conn = String.Format(ExcelConnectionString, modifiedJobSheetPath);
        String updateQuery = "update [Project Sheet${0}:{0}] set F1 = \"{1}\"";

        using (OleDbConnection connection = new OleDbConnection(conn))
        {
            connection.Open();

            OleDbCommand cmd = new OleDbCommand
            {
                Connection = connection
            };
            
            if (!row.IsProject_CodeNull())
            {
                cmd.CommandText = String.Format(updateQuery, "G5", row.Project_Code);
                cmd.ExecuteNonQuery();
            }

            var introducer = row["Introducer"].ToString();

            if (!string.IsNullOrWhiteSpace(introducer))
            {
                cmd.CommandText = String.Format(updateQuery, "G7", introducer);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsStatusNull())
            {
                cmd.CommandText = String.Format(updateQuery, "G11", row.Status.Trim());
                cmd.ExecuteNonQuery();
            }

            if (!row.IsDescriptionNull())
            {
                cmd.CommandText = String.Format(updateQuery, "G13", row.Description);
                cmd.ExecuteNonQuery();
            }

            var clientCompanyName = row["ClientCompanyName"].ToString();

            if (!string.IsNullOrWhiteSpace(clientCompanyName))
            {
                cmd.CommandText = String.Format(updateQuery, "G18", clientCompanyName);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsContactNull())
            {
                cmd.CommandText = String.Format(updateQuery, "A21", row.Contact);
                cmd.ExecuteNonQuery();
            }
                   
            var clientAddressFields = GetAddressFieldNames("Client", row);
            var rowIndex = 24;

            foreach (var field in clientAddressFields)
            {
                cmd.CommandText = String.Format(updateQuery, "A" + rowIndex.ToString(), field);
                cmd.ExecuteNonQuery();

                rowIndex++;
            }

            var invoiceContact = row["InvoiceContact"].ToString();

            if (!string.IsNullOrWhiteSpace(invoiceContact))
            {
                cmd.CommandText = String.Format(updateQuery, "G21", invoiceContact);
                cmd.ExecuteNonQuery();
            }

            var invoiceAddressFields = GetAddressFieldNames("Invoice", row);
            rowIndex = 24;

            foreach (var field in invoiceAddressFields)
            {
                cmd.CommandText = String.Format(updateQuery, "G" + rowIndex.ToString(), field);
                cmd.ExecuteNonQuery();

                rowIndex++;
            }
            
            connection.Close();
        }

        Response.Redirect("newJobsheet1.xls");
    }

    [System.Web.Services.WebMethod]
    public static bool ValidateDeletePassword(string password)
    {
        return password == ConfigurationManager.AppSettings["DeletePassword"];
    }

    [System.Web.Services.WebMethod]
    public static string FetchPlanningAuthoritiesForCounty(int countyId)
    {
        var planningAuthorities = PlanningAuthorityProvider.GetPlanningAuthoritiesByCounty(countyId);
        return JsonConvert.SerializeObject(planningAuthorities);
    }

    private void RedirectToMap()
    {
        string lat = ((Label)DetailsView2.FindControl("LblLat")).Text;
        string lng = ((Label)DetailsView2.FindControl("LblLng")).Text;
        string projectcode = ((Label)DetailsView2.FindControl("LblCode")).Text;
        Response.Redirect(string.Format("map.aspx?lat={0}&lng={1}&code={2}", lat, lng, projectcode), false);
    }
}
