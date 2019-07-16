using CRUDGenerator.Extensions;
using CRUDGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDGenerator
{
    public static partial class Generator
    {
        private static ResultPage GenerateDefaultPage(Type type, string className)
        {
            var objectsVar = className.LowerFirstChar();
            var codeFile = new ResultPage
            {
                Name = "Default.aspx.cs"
            };

            var sb = new StringBuilder();
            sb.AppendLine("protected async void Page_Load(object sender, EventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine("	try");
            sb.AppendLine("	{");
            sb.AppendLine("		if (!isPostBack)");
            sb.AppendLine("		{");
            sb.AppendLine("			await BindGridviewAsync();");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine("	catch (Exception ex)");
            sb.AppendLine("	{");
            sb.AppendLine("		ErrorLabel.Text = ex.Message;");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected async void BindGridviewAsync()");
            sb.AppendLine("{");
            sb.AppendLine("	ItemsGrid.AllowPaging = true;");
            sb.AppendLine("	ItemsGrid.PageSize = 15;");
            sb.AppendLine("	ItemsGrid.AllowSorting = true;");
            sb.AppendLine(string.Empty);
            sb.AppendLine("	ViewState[\"SortExpression\"] = ViewState[\"SortExpression\"] ?? \"Id ASC\";");
            sb.AppendLine(string.Empty);
            sb.AppendLine("	using (var context = new ApplicationDbContext())");
            sb.AppendLine("	{");
            sb.AppendLine("		var {className}s = context.{className}s.Where(x => !x.Deleted);");
            sb.AppendLine(string.Empty);
            sb.AppendLine("		// To add Viewmodel mapping, use .Select(x => new <ViewModel> {})");
            sb.AppendLine("		// and manullay map the model properties from the selection to the Viewmodel.");
            sb.AppendLine("		var data = await {className}s.SortDataBy(ViewState[\"SortExpression\"].ToString()).ToListAsync();");
            sb.AppendLine(string.Empty);
            sb.AppendLine("		ItemsGrid.DataSource = data;");
            sb.AppendLine("		ItemsGrid.DataBind();");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected async void ItemsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine("	try");
            sb.AppendLine("	{");
            sb.AppendLine("		ItemsGrid.PageIndex = e.NewPageIndex;");
            sb.AppendLine("		await BindGridviewAsync();");
            sb.AppendLine("	}");
            sb.AppendLine("	catch (Exception ex)");
            sb.AppendLine("	{");
            sb.AppendLine("		ErrorLabel.Text = ex.Message;");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected async void ItemsGrid_Sorting(object sender, GridViewPageEventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine("	// We're not going to try...catch this because its just ViewState manipulation.");
            sb.AppendLine("	var expression = ViewState[\"SortExpression\"].ToString().Split(' ');");
            sb.AppendLine("	if (expression[0] == e.SortExpression)");
            sb.AppendLine("	{");
            sb.AppendLine("		if (expression[1] == \"ASC\")");
            sb.AppendLine("		{");
            sb.AppendLine("			ViewState[\"SortExpression\"} = e.SortExpression + \" DESC\";");
            sb.AppendLine("		}");
            sb.AppendLine("		else");
            sb.AppendLine("		{");
            sb.AppendLine("			ViewState[\"SortExpression\"] = e.SortExpression + \" ASC\";");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine("	else");
            sb.AppendLine("	{");
            sb.AppendLine("		ViewState[\"SortExpression\"] = e.SortExpression + \" ASC\";");
            sb.AppendLine("	}");
            sb.AppendLine("	await BindGridviewAsync();");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected async void ItemsGrid_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine("	try");
            sb.AppendLine("	{");
            sb.AppendLine($"		var {className}Id = ItemsGrid.DataKeys[e.NewSelectedIndex].Values[\"Id\"];");
            sb.AppendLine($"		Session[\"{className}Id\"] = {className}Id;");
            sb.AppendLine("	}");
            sb.AppendLine("	catch (Exception ex)");
            sb.AppendLine("	{");
            sb.AppendLine("		ErrorLabel.Text = ex.Message;");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("private void ItemsGrid_RowDataBound(object sender, GridViewRowEventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine("	try");
            sb.AppendLine("	{");
            sb.AppendLine("		if (e.Row.RowType != DataControlRowType.DataRow) return;");
            sb.AppendLine(string.Empty);
            sb.AppendLine("		var deleteButton = e.Row.Cells[2].Controls[0] as LinkButton;");
            sb.AppendLine($"		deleteButton.OnClientClick = \"return confirm('Are you sure you want to delete this {objectsVar}?');\";");
            sb.AppendLine("	}");
            sb.AppendLine("	catch (Exception ex)");
            sb.AppendLine("	{");
            sb.AppendLine("		ErrorLabel.Text = ex.Message;");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected async void ItemsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine("	try");
            sb.AppendLine("	{");
            sb.AppendLine($"		int.TryParse(ItemsGrid.DataKeys[e.RowIndex].Values[\"Id\"].ToString(), out int {objectsVar}Id);");
            sb.AppendLine($"		context.{className}s.Single(x => x.Id == {objectsVar}Id).Deleted = true;");
            sb.AppendLine("		await context.SaveChangesAsync();");
            sb.AppendLine($"		Load{className}s(ItemsGrid.PageIndex);");
            sb.AppendLine("	}");
            sb.AppendLine("	catch (Exception ex)");
            sb.AppendLine("	{");
            sb.AppendLine("		ErrorLabel.Text = ex.Message;");
            sb.AppendLine("	}");
            sb.AppendLine("}");

            codeFile.Code = sb.ToString();
            return codeFile;
        }

        public static ResultPage GenerateDefaultMarkup(Type type, string className)
        {
            var objectsVar = className.LowerFirstChar();
            var codeFile = new ResultPage
            {
                Name = "Default.aspx"
            };

            var sb = new StringBuilder();
            sb.AppendLine("<!-- Please remember to make the page async by adding \"Async=\"true\"\" to the Page directive. -->");
            sb.AppendLine("<asp:UpdateProgress runat=\"server\" ID=\"UpdateProgress\" AssociatedUpdatePanelID=\"UpdatePanel\">");
            sb.AppendLine("	<ProgressTemplate>");
            sb.AppendLine("		<uc:ProgressLoader runat=\"server\" id=\"ProgressLoader1\" />");
            sb.AppendLine("	</ProgressTemplate>");
            sb.AppendLine("</asp:UpdateProgress>");
            sb.AppendLine(string.Empty);
            sb.AppendLine("<asp:UpdatePanel runat=\"server\" ID=\"UpdatePanel\">");
            sb.AppendLine("	<ContentTemplate>");
            sb.AppendLine("		<div class=\"row\">");
            sb.AppendLine("			<div class=\"col-md-12\">");
            sb.AppendLine(string.Empty);
            sb.AppendLine("				<div class=\"panel panel-primary\">");
            sb.AppendLine("					<div class=\"panel-heading\">");
            sb.AppendLine($"						<span class=\"glyphicon glyphicon-list\"></span>{className}s");
            sb.AppendLine("					</div>");
            sb.AppendLine("					<div class=\"panel-body\">");
            sb.AppendLine("						<nav class=\"navbar navbar-default\">");
            sb.AppendLine("							<div class=\"collapse navbar-collapse\">");
            sb.AppendLine("								<ul class=\"nav navbar-nav\">");
            sb.AppendLine("									<li><a href=\"Create\">Add New</a></li>");
            sb.AppendLine("								</ul>");
            sb.AppendLine("							</div>");
            sb.AppendLine("						</nav>");
            sb.AppendLine("						<div class=\"row\">");
            sb.AppendLine("							<asp:Label runat=\"server\" ID=\"ErrorLabel\" ForeColor=\"Red\" />");
            sb.AppendLine("						</div>");
            sb.AppendLine("						<asp:Panel runat=\"server\" ID=\"LicensesPanel\">");
            sb.AppendLine("							<asp:GridView runat=\"server\" ID=\"ItemsGrid\" DataKeyNames=\"Id\"");
            sb.AppendLine("								AutoGenerateColumns=\"false\"");
            sb.AppendLine("								CssClass=\"table table-striped table-bordered table-hover\"");
            sb.AppendLine("								HeaderStyle-CssClass=\"thead-dark\"");
            sb.AppendLine("								OnRowDataBound=\"ItemsGrid_RowDataBound\"");
            sb.AppendLine("								OnSelectedIndexChanging=\"ItemsGrid_SelectedIndexChanging\"");
            sb.AppendLine("								OnRowDeleting=\"ItemsGrid_RowDeleting\"");
            sb.AppendLine("								EmptyDataText=\"There are no codes to manage.\"");
            sb.AppendLine("								PageSize=\"10\" AllowPaging=\"true\"");
            sb.AppendLine("								PagerSettings-Position=\"TopAndBottom\"");
            sb.AppendLine("								PagerSettings-Visible=\"true\"");
            sb.AppendLine("								PagerSettings-Mode=\"Numeric\"");
            sb.AppendLine("								OnPageIndexChanging=\"ItemsGrid_PageIndexChanging\">");
            sb.AppendLine("								<Columns>");
            sb.AppendLine("									<asp:CommandField ShowSelectButton=\"true\" SelectText=\"<i class='fas fa-edit'></i>\" ItemStyle-Width=\"35px\" />");
            sb.AppendLine("									<asp:CommandField ShowDeleteButton=\"true\" DeleteText=\"<i class='fas fa-trash-alt'></i>\" ItemStyle-Width=\"35px\" />");
            sb.AppendLine("									<!-- Add data columns here -->");
            sb.AppendLine("								</Columns>");
            sb.AppendLine("							</asp:GridView>");
            sb.AppendLine("						</asp:Panel>");
            sb.AppendLine("					</div>");
            sb.AppendLine("				</div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("				");
            sb.AppendLine("           </div>");
            sb.AppendLine(string.Empty);
            sb.AppendLine("		<div id=\"snackbar\">");
            sb.AppendLine("			<asp:Label runat=\"server\" ID=\"Snack\" />");
            sb.AppendLine("		</div>");
            sb.AppendLine("	</ContentTemplate>");
            sb.AppendLine("</asp:UpdatePanel>");

            codeFile.Code = sb.ToString();
            return codeFile;
        }
    }
}
