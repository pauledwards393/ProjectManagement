<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" Inherits="Detail" Title="Mayer Brown Project Management System (Beta)"
    EnableEventValidation="false" Codebehind="Detail.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="Server">
    <link rel="Stylesheet" type="text/css" href="_Asset/bootstrap.css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div runat="server" id="ViewPanel">
        <fieldset title="Project Detail">
            <legend style="font-size: medium; font-weight: bold">Project Detail</legend>
            <ajax:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            </ajax:ScriptManager>
            <!--<ajax:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>-->
                    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                        <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                            <HeaderTemplate>
                                Project Detail
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:Label ID="lblEmailSuccess" runat="server" CssClass="success" Text="* Email sent successfully" Visible="false"></asp:Label>
                                <asp:Label ID="lblSaveError" runat="server" CssClass="error" Visible="false"></asp:Label>

                                <asp:ValidationSummary ID="DetailsValidationSummary" runat="server" DisplayMode ="List" CssClass="details-validation-summary" />

                                <asp:DetailsView ID="DetailsView2" runat="server" AutoGenerateRows="False"
                                    DataKeyNames="Project_ID"
                                    AutoGenerateEditButton="False"
                                    OnModeChanging="DetailsView2_ModeChanging" OnItemCommand="DetailsView2_ItemCommand"
                                    OnItemUpdated="DetailsView2_ItemUpdated" OnItemUpdating="DetailsView2_ItemUpdating"
                                    OnItemInserting="DetailsView2_ItemInserting" OnDataBound="DetailsView2_DataBound" ClientIDMode="Static">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <FieldHeaderStyle Font-Bold="True" Width="300px" />
                                    <Fields>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Project_ID") %>' ID="LBLProjectID"></asp:Label>
												<asp:Label runat="server" Text='<%# Eval("StatusID") %>' ID="LblPreviousStatus"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project Code" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Project Code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtProjectCode" runat="server" Text='<%# Eval("Project Code") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtProjectCode" ID="ProjectCodeValidator" runat="server" ErrorMessage="* Please add a project code" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Project_ID" HeaderText="Project_ID" InsertVisible="False"
                                            Visible="False" ReadOnly="True" SortExpression="Project_ID" />
                                        <asp:TemplateField HeaderText="Project Name" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Project Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtPorjectname" Text='<%# Eval("Project Name") %>'>
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtPorjectname" ID="ProjectNameValidator" runat="server" ErrorMessage="* Please add a project name" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLStartDate" runat="server" Text='<%# Eval("StartDate","{0:d}")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtStartdate" runat="server" Text='<%# Eval("StartDate","{0:d}")  %>'></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="TxtStartdate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtStartdate" ID="StartDateValidator" runat="server" ErrorMessage="* Please enter a start date" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Date">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLEndDate" runat="server" Text='<%# Eval("EndDate","{0:d}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtEndDate" runat="server" Text='<%# Eval("EndDate","{0:d}")%>'></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" TargetControlID="TxtEndDate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DDLStatus" runat="server" DataSourceID="StatusDataSource"
                                                    SelectedValue='<%# Eval("StatusID") %>' DataTextField="Status" DataValueField="Status_ID">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="StatusDataSource" runat="server" SelectMethod="GetData"
                                                    TypeName="StatusBLL" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DropDownList ID="DDLStatus" runat="server" DataSourceID="StatusDataSource"
                                                    DataTextField="Status" DataValueField="Status_ID">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="StatusDataSource" runat="server" SelectMethod="GetData"
                                                    TypeName="StatusBLL" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLDepartment" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DDLDepartment" runat="server" DataSourceID="DepartmentDataSourceEdit"
                                                    DataTextField="Name" DataValueField="Id" SelectedValue='<%# Eval("DepartmentID") %>'>
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="DepartmentDataSourceEdit" runat="server" SelectMethod="GetDepartmentsForEdit"
                                                        TypeName="ProjectManagement.Web.Providers.DepartmentProvider" OnSelecting="DepartmentDataSourceEdit_Selecting" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DropDownList ID="DDLDepartment" runat="server" DataSourceID="DepartmentDataSourceInsert"
                                                    DataTextField="Name" DataValueField="Id">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="DepartmentDataSourceInsert" runat="server" SelectMethod="GetDepartmentsForInsert"
                                                        TypeName="ProjectManagement.Web.Providers.DepartmentProvider" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sector" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLSector" runat="server" Text='<%# Eval("SectorList") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSector" runat="server" Text='<%# Eval("SectorList") %>' Visible="false"></asp:Label>
                                                <asp:ListBox ID="DDLSector" SelectionMode="Multiple" runat="server" DataSourceID="SqlDataSource5"
                                                    CssClass="MySelect" DataTextField="Name" OnDataBound="DDLSector_DataBound" DataValueField="Sector_ID"></asp:ListBox>
                                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>"
                                                    SelectCommand="SELECT [Sector_ID], [Name] FROM [Sector]"></asp:SqlDataSource>
                                                <asp:RequiredFieldValidator ControlToValidate="DDLSector" ID="SectorValidator" runat="server" ErrorMessage="* Please choose at least one sector" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Client Type" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ClientType")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DDLClientType" runat="server" DataSourceID="ClientTypeDataSource" SelectedValue='<%# Eval("ClientTypeId") %>'
                                                    DataTextField="Name" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true">
                                                    <Items>
                                                       <asp:ListItem Text="-- Select client type --" Value="" />
                                                    </Items>
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="ClientTypeDataSource" runat="server" SelectMethod="GetClientTypes"
                                                    TypeName="ProjectManagement.Web.Providers.ClientTypeProvider" />
                                                <asp:RequiredFieldValidator ControlToValidate="DDLClientType" ID="ValClientType" runat="server" ErrorMessage="* Please add a client type" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project Manager[MBL]">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ProjectManager") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtManager" Text='<%#  Eval("ProjectManager")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Introducer" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label ID="LblIntroducer" runat="server" Text='<%# Eval("Introducer") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtIntroducer" Text='<%#  Eval("Introducer")%>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtIntroducer" ID="ValIntroducer" runat="server" ErrorMessage="* Please add an introducer" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client Contact" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Contact") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtContact" Text='<%# Eval("Contact") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtContact" ID="ValClientContact" runat="server" ErrorMessage="* Please add a client contact" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="LblClientAddressId" Text='<%# Eval("ClientAddressId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client company" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ClientCompanyName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtClientCompanyName" Text='<%# Eval("ClientCompanyName") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtClientCompanyName" ID="ValClientCompany" runat="server" ErrorMessage="* Please add a client company name" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client address line 1" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ClientAddressLine1") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtClientAddressLine1" Text='<%# Eval("ClientAddressLine1") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtClientAddressLine1" ID="ValClientAddressLine1" runat="server" ErrorMessage="* Please add a client address line 1" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client address line 2">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ClientAddressLine2") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtClientAddressLine2" Text='<%# Eval("ClientAddressLine2") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client town/city" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ClientTownOrCity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtClientTownOrCity" Text='<%# Eval("ClientTownOrCity") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtClientTownOrCity" ID="ValClientTownOrCity" runat="server" ErrorMessage="* Please add a client town or city" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client county">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ClientCounty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtClientCounty" Text='<%# Eval("ClientCounty") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client postcode" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ClientPostcode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtClientPostcode" Text='<%# Eval("ClientPostcode") %>' MaxLength="10"></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtClientPostcode" ID="ValClientPostcode" runat="server" ErrorMessage="* Please add a client postcode" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice contact">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("InvoiceContact") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtInvoiceContact" Text='<%# Eval("InvoiceContact") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="LblInvoiceAddressId" Text='<%# Eval("InvoiceAddressId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice company">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("InvoiceCompanyName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtInvoiceCompanyName" Text='<%# Eval("InvoiceCompanyName") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice address line 1">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("InvoiceAddressLine1") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtInvoiceAddressLine1" Text='<%# Eval("InvoiceAddressLine1") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice address line 2">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("InvoiceAddressLine2") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtInvoiceAddressLine2" Text='<%# Eval("InvoiceAddressLine2") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice town/city">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("InvoiceTownOrCity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtInvoiceTownOrCity" Text='<%# Eval("InvoiceTownOrCity") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice county">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("InvoiceCounty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtInvoiceCounty" Text='<%# Eval("InvoiceCounty") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice postcode">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("InvoicePostcode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtInvoicePostcode" Text='<%# Eval("InvoicePostcode") %>' MaxLength="10"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project County">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("County")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DDLCounty" runat="server" DataSourceID="CountyDataSource" SelectedValue='<%# Eval("CountyId") %>'
                                                    DataTextField="Name" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true">
                                                    <Items>
                                                       <asp:ListItem Text="-- Select project county --" Value="" />
                                                    </Items>
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="CountyDataSource" runat="server" SelectMethod="GetCounties"
                                                    TypeName="ProjectManagement.Web.Providers.CountyProvider" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project Local Planning Authority">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("PlanningAuthority")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DDLPlanningAuthority" runat="server" ClientIDMode="Static">
                                                </asp:DropDownList>
                                                <asp:TextBox runat="server" ID="txtPlanningAuthority" Value='<%# Eval("PlanningAuthorityId") %>' ClientIDMode="Static" CssClass="hidden"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project Town/City">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ProjectCity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtProjectCity" Text='<%# Eval("ProjectCity") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detailed">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Detailed") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox runat="server" ID="ChkDetailed" Checked='<%# Eval("Detailed") %>' />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:CheckBox runat="server" ID="ChkDetailed" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCode" runat="server" Text='<%# Bind("Project_ID") %>'></asp:Label>
                                                <asp:Label ID="LblLat" runat="server" Text='<%# Bind("lat") %>'> </asp:Label>
                                                <asp:Label ID="LblLng" runat="server" Text='<%# Bind("lon") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="mandatory">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="10" Text='<%# Eval("Description") %>'
                                                    ID="TxtDescription"></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="TxtDescription" ID="ValDescription" runat="server" ErrorMessage="* Please add a description" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Created on" DataField="AddedAt" ReadOnly="true" DataFormatString="{0:d}" ItemStyle-CssClass="readonly" Visible="false" />
                                        <asp:BoundField HeaderText="Job Sheet" DataField="JobSheetSubmitted" ReadOnly="true" DataFormatString="{0:d}" ItemStyle-CssClass="readonly" Visible="false" />
                                        <asp:BoundField HeaderText="Original Fee Proposal" DataField="FeeProposalSubmitted" ReadOnly="true" DataFormatString="{0:d}" ItemStyle-CssClass="readonly" Visible="false" />
                                        <asp:BoundField HeaderText="Acceptance of Service" DataField="AcceptanceOfServiceSubmitted" ReadOnly="true" DataFormatString="{0:d}" ItemStyle-CssClass="readonly" Visible="false" />
                                    </Fields>
                                    <Fields>                                        
                                        <asp:TemplateField ShowHeader="false" ItemStyle-BackColor="#D1DDF1">
                                            <ItemTemplate>
                                                <asp:Button ID="EditProject" runat="server" Text="Edit" CommandName="Edit"></asp:Button>
                                                <button type="button" data-toggle="modal" data-target="#delete-confirmation">Delete</button>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button ID="UpdateProject" runat="server" Text="Update" CommandName="Update"></asp:Button>
                                                <asp:Button ID="CancelEditProject" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false"></asp:Button>
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:Button ID="CreateProject" runat="server" Text="Save" CommandName="Insert"></asp:Button>
                                                <asp:Button ID="CancelCreateProject" runat="server" Text="Cancel" CommandName="GoBack" CausesValidation="false"></asp:Button>
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                    <FooterTemplate>
                                        <asp:Button CommandName="GoBack" Text="Go Back" runat="server" ID="BtnBack" CausesValidation="false" />
                                        <asp:Button CommandName="JobSheet" Text="Job Sheet" runat="server" ID="BtnSheet" OnClick="BtnSheet_Click" />
                                        <button type="button" data-toggle="modal" data-target="#upload-job-sheet">Submit Job Sheet/Fee/Acceptance</button>
                                    </FooterTemplate>
                                </asp:DetailsView>
                                &nbsp;
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel runat="server" Visible="false" HeaderText="" Enabled="false" EnableViewState="false"
                            ID="TabPanel2">
                            <ContentTemplate>
                                <table id="Table1" class="jobsheet" runat="server">
                                    <tr>
                                        <td colspan="4" class="heading1">New Project Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="heading2">Job Detail
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 26px">Job Code
                                        </td>
                                        <td style="height: 26px">
                                            <asp:TextBox ID="TxtJobCode" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="height: 26px">Job No
                                        </td>
                                        <td style="width: 25%; height: 26px">
                                            <asp:TextBox ID="TxtJobNo" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Job Details
                                        </td>
                                        <td></td>
                                        <td>For
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="Txtfor" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="TxtDetails" TextMode="MultiLine" Rows="3" runat="server" Width="547px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading2">Client Details
                                        </td>
                                        <td></td>
                                        <td>Ref
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtRef" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Client
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtClient" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Order No
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtOrderNo" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 26px">Contact
                                        </td>
                                        <td style="height: 26px">
                                            <asp:TextBox ID="TxtContact" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="height: 26px">Invoice Contact
                                        </td>
                                        <td style="width: 25%; height: 26px;">
                                            <asp:TextBox ID="TxtInvoiceContact" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Address:
                                        </td>
                                        <td></td>
                                        <td>Invoice Address
                                        </td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtAddress" runat="server" TextMode="MultiLine" Width="272px" Rows="5"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtInvoiceAddress" runat="server" TextMode="MultiLine" Width="269px"
                                                Rows="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tel
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtTel" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Tel
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtInvoiceTel" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Fax
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFax" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Fax
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtInvoiceFax" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="heading2">Contract / Order Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 51px">Estimated Hours
                                        </td>
                                        <td style="height: 51px">
                                            <asp:TextBox ID="TxtHours" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="height: 51px">Estimated Completion Date/<br />
                                            Finanl Invoice Date
                                        </td>
                                        <td style="width: 25%; height: 51px">
                                            <asp:TextBox ID="TxtCompletionDate" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td>Estimated Fee/
                                            <br />
                                            Order Value
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtOrderValue" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td>Variation Orders
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="TxtVariationOrders" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Project Manager
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtManager" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td>Director
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtDirector" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>Date
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtDate" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <asp:Button ID="Button1" runat="server" Text="Generate PDF" OnClick="Button1_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                <!--</ContentTemplate>
            </ajax:UpdatePanel>-->
            <br />
        </fieldset>
    </div>
    <div id="editpanel" runat="server">
    </div>

    <div class="modal fade" id="delete-confirmation" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog delete-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Delete confirmation</h4>
                </div>
                <div class="modal-body">
                    <p>Are you sure you want to delete this project? If so, please enter the password below.</p>
                    <label for="DeletePassword" data-validate="true">
                        <span class="validation-label required">* Required field</span>
                        <span class="validation-label invalid">* Incorrect password</span>
                    </label>
                    <asp:Textbox TextMode="Password" ID="DeletePassword" runat="server" CssClass="form-control" placeholder="Delete password..." />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" onclick="validateDelete()">OK</button>
                    <asp:Button ID="DeleteProject" Style="display: none;" runat="server" OnClick="DeleteProject_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="upload-job-sheet" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Submit Project Details</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group first">
                        <label for="FileJobSheet" data-validate="<%= (!HasJobSheet).ToString().ToLower() %>">
                            <asp:Label ID="JobSheetMandatoryMarker" runat="server" Text="* "></asp:Label>
                            Job Sheet
                            <span class="validation-label required">* Required field</span>
                        </label>
                        <asp:FileUpload ID="FileJobSheet" runat="server" />
                    </div>     
                    <div class="form-group">
                        <label for="FileOriginalFeeProposal">
                            Original Fee Proposal 
                        </label>
                        <asp:FileUpload ID="FileOriginalFeeProposal" runat="server" />
                    </div>    
                    <div class="form-group">
                        <label for="FileAcceptanceOfService">
                            Client Acceptance of Service
                        </label>
                        <asp:FileUpload ID="FileAcceptanceOfService" runat="server" />
                    </div>   
                    <div class="form-group">
                        <label for="SubmittedBy" data-validate="true">
                            * Submitted by
                            <span class="validation-label required">* Required field</span>
                        </label>
                        <asp:Textbox  ID="SubmittedBy" runat="server" CssClass="form-control" placeholder="Insert your name..." />
                    </div>
                    <div class="form-group last">
                        <label for="ProjectDetailsComments">
                            Additional Comments
                        </label>
                        <asp:TextBox ID="ProjectDetailsComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Insert any additional comments..." />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button ID="UploadJobSheet" CssClass="btn btn-primary" runat="server" OnClick="UploadJobSheet_Click" OnClientClick="return validateUpload()" Text="Email Project Details" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="ContentScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <script type="text/javascript">

        $('#upload-job-sheet, #delete-confirmation').on('show.bs.modal', function (e) {
            $('.validation-label').hide();
            $('input.form-control, textarea.form-control').val('');
        })

        function validateModalRequired(modalId) {
            $('.validation-label').hide();

            var $modal = $('#' + modalId);
            var fieldsToValidate = $modal.find('label[data-validate="true"]');
            var hasErrors = false;

            $.each(fieldsToValidate, function (index, value) {
                var id = $(value).attr('for');
                var inputElement = $('input[id$="' + id + '"]');
                var validationLabel = inputElement.parent().find('.validation-label.required');

                if (inputElement.val().trim() === '') {
                    validationLabel.fadeIn();
                    hasErrors = true;
                }                    
                else {
                    validationLabel.hide();
                }                    
            });

            return !hasErrors;
        }

        function validateUpload() {
            return validateModalRequired('upload-job-sheet');
        }

        function validateDelete() {
            var isValid = validateModalRequired('delete-confirmation');

            if (!isValid) return;

            var passwordClientId = '<%= DeletePassword.ClientID %>';
            var buttonClientId = '<%= DeleteProject.ClientID %>';

            var deletePassword = document.getElementById(passwordClientId).value;

            PageMethods.ValidateDeletePassword(deletePassword, function (result) {
                // If password is correct, delete project else show validation
                if (result) {
                    __doPostBack('<%= DeleteProject.UniqueID %>', '')
                }
                else {
                    $('#delete-confirmation .validation-label.required').fadeOut();
                    $('#delete-confirmation .validation-label.invalid').fadeIn();
                }
            });      
        }

        function handleDeleteEnter(e) {
            if (e.which == 13 || e.keyCode == 13) {
                validateDelete();
                return false;
            }
            return true;
        }

        $('#aspnetForm').on('submit', function (e) {
            // Don't allow the default form to submit if the delete confirmation is open
            var isDeleteModalOpen = $("#delete-confirmation").data()['bs.modal'].isShown;
            return !isDeleteModalOpen;
        });

        $('#<%= DeletePassword.ClientID %>').on('keydown', handleDeleteEnter);

        //$('#upload-job-sheet').modal('show');

        (function () {
            var ddCounty = document.getElementById('DDLCounty');
            var ddPlanningAuthority = document.getElementById('DDLPlanningAuthority');
            var txtPlanningAuthority = document.getElementById('txtPlanningAuthority');
      
            if (!ddCounty || !ddPlanningAuthority || !txtPlanningAuthority) return;

            var handleChangePlanningAuthority = function (e) {
                txtPlanningAuthority.value = e.target.value;
            };

            var handleChangeCounty = function (e, isInitialLoad) {
                ddPlanningAuthority.options.length = 0;
                
                var countyId = e.target.value;

                if (countyId === '') {
                    txtPlanningAuthority.value = '';
                    return;
                }

                PageMethods.FetchPlanningAuthoritiesForCounty(parseInt(countyId, 10), function (response) {
                    var planningAuthorities = JSON.parse(response);
                    
                    var appendOption = function (planningAuthority) {
                        var option = document.createElement('option');
                        var value = planningAuthority.id.toString();

                        option.value = value;
                        option.innerHTML = planningAuthority.name;

                        ddPlanningAuthority.appendChild(option);
                    };

                    appendOption({ id: '', name: '-- Please select --' });

                    planningAuthorities.forEach(appendOption);

                    var planningAuthorityId = txtPlanningAuthority.value;

                    if (isInitialLoad && planningAuthorityId) {
                        console.log(planningAuthorityId);
                        ddPlanningAuthority.value = planningAuthorityId;
                    } else {
                        txtPlanningAuthority.value = '';
                    }
                });
            };
                     
            ddCounty.addEventListener('change', handleChangeCounty, false);
            ddPlanningAuthority.addEventListener('change', handleChangePlanningAuthority, false);

            if (ddCounty.value) {
                handleChangeCounty({ target: ddCounty }, true);
            }            
        }());

    </script>
</asp:Content>
