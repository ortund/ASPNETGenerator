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
        private static ResultPage GenerateCreatePage(Type type, string className)
        {
            var objectsVar = className.LowerFirstChar();
            var codeFile = new ResultPage
            {
                Name = "Create.aspx.cs"
            };
            var sb = new StringBuilder();
            sb.AppendLine("protected void Page_Load(object sender, EventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine(string.Empty);
            sb.AppendLine("protected async void SaveButton_Click(object sender, EventArgs e)");
            sb.AppendLine("{");
            sb.AppendLine("	try");
            sb.AppendLine("	{");
            sb.AppendLine($"		await Save{className}Async();");
            sb.AppendLine("		Response.Redirect(\"Default\");");
            sb.AppendLine("	}");
            sb.AppendLine("	catch (Exception ex)");
            sb.AppendLine("	{");
            sb.AppendLine("		ErrorLabel.Text = ex.Message;");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"private async Task<int> Save{className}Async()");
            sb.AppendLine("{");
            sb.AppendLine("	// Better to refactor this so that the value assignments are done in the constructor");
            sb.AppendLine("	// but the current version of the generator isn't smart enough for that.");

            // First loop creates variables that will be assigned to the properties in the second loop.
            //
            // This allows for more simplified object initialization when the user implements the generated code
            // since such simplified object initialization isn't possible in the generator.
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType == typeof(string))
                    sb.AppendLine($"	var {property.Name.ToLower()} = {property.Name}TextBox.Text;");
                else if (property.PropertyType == typeof(int))
                    sb.AppendLine("	int.TryParse({property.Name}TextBox.Text, out int {property.Name.ToLower()});");
                else if (property.PropertyType == typeof(DateTime))
                    sb.AppendLine($"	DateTime.TryParse({property.Name}DateTimePicker.SelectedDate.ToString(), out DateTime {property.Name.ToLower()});");
                else
                    sb.AppendLine($"	var {property.Name.ToLower()} = {property.Name}TextBox.Text;");
            }

            sb.AppendLine($"	var {objectsVar} = new {className}();");
            foreach (var property in type.GetProperties())
                sb.AppendLine($"	{objectsVar}.{property.Name} = {property.Name.ToLower()};");

            sb.AppendLine("    using (var context = new ApplicationDbContext())");
            sb.AppendLine("    {");
            sb.AppendLine($"        context.{className}s.Add({objectsVar});");
            sb.AppendLine("         await context.SaveChangesAsync();");
            sb.AppendLine("    }");
            sb.AppendLine($"	return {objectsVar}.Id;");
            sb.AppendLine("}");

            codeFile.Code = sb.ToString();
            return codeFile;
        }

        private static ResultPage GenerateCreateMarkup(Type type, string className)
        {
            var objectsVar = className.LowerFirstChar();
            var codeFile = new ResultPage
            {
                Name = "Create.aspx"
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
            sb.AppendLine("						<a href=\"javascript:history.go(-1)\">");
            sb.AppendLine("                         <i class=\"fas fa-angle-left\"></i>");
            sb.AppendLine($"                     </a>Adding a {className}");
            sb.AppendLine("					</div>");
            sb.AppendLine("					<div class=\"panel-body\">");
            sb.AppendLine("						<div class=\"row\">");
            sb.AppendLine("							<asp:Label runat=\"server\" ID=\"ErrorLabel\" ForeColor=\"Red\" />");
            sb.AppendLine("							<div class=\"form-horizontal\">");
            foreach (var property in type.GetProperties())
            {
                sb.AppendLine("								<div class=\"form-group\">");
                sb.AppendLine($"								    <label class=\"col-md-2 control-label\">{property.Name}</label>");
                sb.AppendLine("								    <div class=\"col-md-4\">");
                if (property.PropertyType == typeof(string))
                {
                    sb.AppendLine($"								        <asp:TextBox runat=\"server\" ID=\"{property.Name}TextBox\" CssClass=\"form-control\" />");
                }
                else if (property.PropertyType == typeof(int))
                {
                    sb.AppendLine($"								        <asp:TextBox runat=\"server\" ID=\"{property.Name}TextBox\" CssClass=\"form-control\" TextMode=\"Number\" />");
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    sb.AppendLine($"								        <telerik:RadDateTimePicker runat=\"server\" ID=\"{property.Name}DateTimePicker\" CssClass=\"form-control\" />");
                }
                else
                {
                    sb.AppendLine($"								        <asp:TextBox runat=\"server\" ID=\"{property.Name}TextBox\" CssClass=\"form-control\" />");
                }
                sb.AppendLine("								    </div>");
                sb.AppendLine("								</div>");
            }
            sb.AppendLine("								<div class=\"col-md-10 offset-md-2\">");
            sb.AppendLine("				    				<asp:Button runat=\"server\" ID=\"SaveButton\" CssClass=\"btn btn-primary\" Text=\"Save\" OnClick=\"SaveButton_Click\" />");
            sb.AppendLine("								</div>");
            sb.AppendLine("							</div");
            sb.AppendLine("						</div>");
            sb.AppendLine("					</div>");
            sb.AppendLine("				</div>");
            sb.AppendLine(string.Empty);
            sb.AppendLine("			</div>");
            sb.AppendLine("		</div>");
            sb.AppendLine("	</ContentTemplate>");
            sb.AppendLine("</asp:UpdatePanel>");
            codeFile.Code = sb.ToString();
            return codeFile;
        }
    }
}
