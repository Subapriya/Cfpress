<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsGrid.ascx.cs" Inherits="CFPress.CFPress.UmbracoMVCApplication.usercontrols.CommentsGrid" %>
<asp:DataGrid runat="server" ID ="Commentsdatagrid" AutoGenerateColumns="false" OnEditCommand="Commentsdatagrid_EditCommand" OnUpdateCommand="Commentsdatagrid_UpdateCommand" OnCancelCommand="Commentsdatagrid_RejectCommand" 
    EnableViewState="false">
   <Columns>
       <asp:EditCommandColumn
        EditText="Edit" CancelText="Reject"
                 UpdateText="Approve"
                 HeaderText="Edit item" ButtonType="PushButton" >
             
               <ItemStyle Wrap="False">
               </ItemStyle>

               <HeaderStyle Wrap="False">
               </HeaderStyle>
           </asp:EditCommandColumn>
                
          <asp:TemplateColumn HeaderText="NewsItemId">
         <ItemTemplate><%#DataBinder.Eval(Container.DataItem,"NewsItemId") %></ItemTemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn HeaderText="CommentText">
            <ItemTemplate><%#DataBinder.Eval(Container.DataItem,"CommentText") %></ItemTemplate>
       </asp:TemplateColumn>
        </Columns>
</asp:DataGrid>
<asp:Button ID="btnExportData" runat="server" Text="Export" />