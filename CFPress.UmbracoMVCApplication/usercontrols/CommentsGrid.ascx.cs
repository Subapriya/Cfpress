using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFPress.UmbracoMVCApplication.Services;
using Umbraco.Core;

namespace CFPress.UmbracoMVCApplication.usercontrols
{
    public partial class CommentsGrid : System.Web.UI.UserControl
    {
        CFPress.UmbracoMVCApplication.Services.CommentsService commentsService = new CommentsService();
        DataTable commentsDataTable = new DataTable();
        DataView commentsDataview = new DataView();
        
        protected void Page_Load(object sender, EventArgs e)
        {
             commentsService.umbDatabase = ApplicationContext.Current.DatabaseContext.Database;
              if(!IsPostBack)
                {
                    this.InitializeControls();
                    Session["CommentsGridData"] = null;
                    this.GetCommentsDataSource();
                }

              BindCommentsGrid();
    
        }

         void btnExportData_Click(object sender, EventArgs e)
        {
            this.ExportGridData((DataTable)Commentsdatagrid.DataSource, "C:\\Users\\snageswaran\\Documents\\My Web Sites\\CFPress.UmbracoMVCApplication\\Exports\\Exportfile");
        }

        private void InitializeControls()
        {
            this.btnExportData.Click += btnExportData_Click;
            BuildCommentsDataTable();
                       
        }

        private void BuildCommentsDataTable()
        {
            commentsDataTable.Columns.Add(new DataColumn("CommentId"));
            commentsDataTable.Columns.Add(new DataColumn("MemberId"));
            commentsDataTable.Columns.Add(new DataColumn("NewsItemId"));
            commentsDataTable.Columns.Add(new DataColumn("CommentText"));

        }
        private void GetCommentsDataSource()
        {
            if (Session["CommentsGridData"] == null)
            {
                 //// show only comments in preapprove status
                var comments = commentsService.GetCommentsByCommentStatus(Enumerations.CommentStatus.PreApprove);
                foreach (CFPress.UmbracoMVCApplication.Pocos.Comments commentitem in comments)
                {

                    commentsDataTable.Rows.Add(commentitem.CommentId, commentitem.MemberId, commentitem.NewsItemUmbracoId, commentitem.CommentText);

                }
                commentsDataTable.AcceptChanges();
                Commentsdatagrid.DataSource = commentsDataTable;
                Commentsdatagrid.DataBind();
                
                Session["CommentsGridData"] = commentsDataTable;
            }

            
            BindCommentsGrid();
            

        }
        private void BindCommentsGrid()
        {
            commentsDataview = ((DataTable)Session["CommentsGridData"]).DefaultView;
            Commentsdatagrid.DataSource = commentsDataview;
            if (ViewState["EditItemIndex"] != null)
            {
                Commentsdatagrid.EditItemIndex = Convert.ToInt32(ViewState["EditItemIndex"]);
            }
            Commentsdatagrid.DataBind();
           
            
        }
      
        private void ExportGridData(DataTable dataTable, string filename)
        { 
            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            for (int j = 0; j < dataTable.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dataTable.Columns[j]) + "\t";
            stOutput += sHeaders + "\r\n";
            // Export data.
            for (int i = 0; i < dataTable.Rows.Count - 1; i++)
            {
                string stLine = "";
                foreach (DataColumn dc in dataTable.Columns)
                {
                    stLine = stLine.ToString() + Convert.ToString(dataTable.Rows[i][dc]) + "\t";
                    stOutput += stLine + "\r\n";
                }
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            filename = filename + DateTime.Now.ToString("ddMMyyyyHHmmssFFFF");
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        protected void Commentsdatagrid_EditCommand(object source, DataGridCommandEventArgs e)
        {
            Commentsdatagrid.EditItemIndex = e.Item.ItemIndex;
            ViewState["EditItemIndex"] = e.Item.ItemIndex;
            BindCommentsGrid();
        }

        protected void Commentsdatagrid_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            ////BindCommentsGrid();
            commentsDataTable = ((DataView)Commentsdatagrid.DataSource).ToTable();
            int commentId = Convert.ToInt32(commentsDataTable.Rows[e.Item.ItemIndex]["CommentId"]);
            if(commentsService.UpdateCommentStatus(commentId, Enumerations.CommentStatus.Approve))
            {
                //// if update was successful move the row back to edit and don't show the comment item anymore.
                Commentsdatagrid.EditItemIndex = e.Item.ItemIndex -1;
                ViewState["EditItemIndex"] = e.Item.ItemIndex - 1;
                commentsDataTable.Rows.RemoveAt(e.Item.ItemIndex);
                Session["CommentsGridData"] = commentsDataTable;
                BindCommentsGrid();
                
            }

            else
            {
                //// try and show error message that the approve failed.
                Commentsdatagrid.EditItemIndex = e.Item.ItemIndex;
                ViewState["EditItemIndex"] = e.Item.ItemIndex;
                BindCommentsGrid();
            }


        }

        protected void Commentsdatagrid_RejectCommand(object source, DataGridCommandEventArgs e)
        {
            ////BindCommentsGrid();
            commentsDataTable = ((DataView)Commentsdatagrid.DataSource).ToTable();
            int commentId = Convert.ToInt32(commentsDataTable.Rows[e.Item.ItemIndex]["CommentId"]);
            if (commentsService.UpdateCommentStatus(commentId, Enumerations.CommentStatus.Reject))
            {
                //// if update was successful move the row back to edit and don't show the comment item anymore.
                Commentsdatagrid.EditItemIndex = e.Item.ItemIndex - 1;
                ViewState["EditItemIndex"] = e.Item.ItemIndex - 1;
                commentsDataTable.Rows.RemoveAt(e.Item.ItemIndex);
                Session["CommentsGridData"] = commentsDataTable;
                BindCommentsGrid();
            }

            else
            {
                //// try and show error message that the approve failed.
                Commentsdatagrid.EditItemIndex = e.Item.ItemIndex;
                ViewState["EditItemIndex"] = e.Item.ItemIndex;
                BindCommentsGrid();
            }
        }  
    }
}