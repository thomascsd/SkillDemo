<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        this.RegisterRoutes(RouteTable.Routes);
    }

    private void RegisterRoutes(RouteCollection routes)
    {
        routes.MapPageRoute("ques", "QuestionnaireList", "~/Questionnaire/QuesList.aspx");
        routes.MapPageRoute("showques", "ShowQuestionnaire", "~/Questionnaire/ShowQues.aspx");
        routes.MapPageRoute("form", "FormList", "~/DefinedForm/FormList.aspx");
        routes.MapPageRoute("showform", "ShowForm", "~/DefinedForm/ShowForm.aspx");
    }



    void Session_End(object sender, EventArgs e)
    {
        // 工作階段結束時執行的程式碼。 
        // 注意: 只有在 Web.config 檔將 sessionstate 模式設定為 InProc 時，
        // 才會引發 Session_End 事件。如果將工作階段模式設定為 StateServer 
        // 或 SQLServer，就不會引發這個事件。

    }
       
</script>
