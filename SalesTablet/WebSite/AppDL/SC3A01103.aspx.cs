using System;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text;
//using log4net;
using System.Configuration;

using Toyota.eCRB.SystemFrameworks.Configuration;
using Toyota.eCRB.SystemFrameworks.Core;
using Toyota.eCRB.SystemFrameworks.Web;
using Toyota.eCRB.Common.Login.BizLogic;
using Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic;

public partial class AppDownLoad_SC3A01103 : System.Web.UI.Page
{

    //画面表示用
    public string pageTitle;
    public string placeholder01;
    public string placeholder02;

    //Web.Configから設定値を取得するためのキー
    private const string keyConfigXmlFileDir = "configXmlFileDir";
    private const string keyMobileConfigTemplate = "mobileConfigTemplate";
    private string configXmlFileDir;
    private string mobileConfigTemplate;

    const string APPLICATIONID = "SC3A01103";
    private const int id001 = 001;
    private const int id002 = 002;
    private const int id003 = 003;
    private const int id004 = 004;
    private const int id005 = 005;

    private const int id901 = 901;  //UserIDがない
    private const int id902 = 902;  //認証失敗

    /// <summary>
    /// ページロード時の処理
    /// </summary>
    /// <param name="sender">イベント発生元</param>
    /// <param name="e">イベントデータ</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Logger.Info("Page_Load Start");

        SC3A01103BusinessLogic bizLogic = new SC3A01103BusinessLogic();

        //Web.Configから取得
        GetConfig();

        //BusinessLogicへ値を設定
        bizLogic.configXmlFileDir = configXmlFileDir;
        bizLogic.mobileConfigTemplate = mobileConfigTemplate;

        //画面文言の設定
        setDisplayWord();

        Logger.Info("Page_Load End");
    }

    /// <summary>
    /// ダウンロードボタンクリック
    /// </summary>
    /// <param name="sender">イベント発生元</param>
    /// <param name="e">イベントデータ</param>
    protected void ButtonDownLoad_Click(object sender, EventArgs e)
    {
        Logger.Info("ButtonDownLoad_Click Start");

        string[] str = { "" };
        SC3A01103BusinessLogic bizLogic = new SC3A01103BusinessLogic();

        //Web.Configから取得
        GetConfig();

        //BusinessLogicへ値を設定
        bizLogic.configXmlFileDir = configXmlFileDir;
        bizLogic.mobileConfigTemplate = mobileConfigTemplate;

        //UserID
        string UserID = stfCd.Text;
        string Password = password.Text;

        Logger.Info("ButtonDownLoad_Click: DownLoad Request. UserID:[" + UserID + "]");

        //入力チェック
        if (string.IsNullOrEmpty(UserID))
        {
            //UserIDがない
            ShowMessageBox(id901, str);
            return;
        }
        if (string.IsNullOrEmpty(Password))
        {
            //Passwordがない
            ShowMessageBox(id901, str);
            return;
        }

        //認証
        Boolean AuthorizationCheck = false;
        AuthorizationCheck = bizLogic.Authorization(UserID, Password);
        if (!AuthorizationCheck)
        {
            //認証失敗
            ShowMessageBox(id902, str);
            return;
        }

        //認証OKの場合、ダウンロード開始
        if (AuthorizationCheck)
        {

            Logger.Info("ButtonDownLoad_Click: Start DownLoad [mobileConfig]");

            //ダウンロードファイルを取得
            string outPut = bizLogic.GetDownLoadFile_mobileConfig(UserID, Password);

            //Responseを作成
            Response.Clear();
            Response.ContentType = "application/x-apple-aspen-config";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + mobileConfigTemplate + "\"");
            Response.Write(outPut);
            Response.Flush();

            Logger.Info("ButtonDownLoad_Click: End DownLoad");
            Response.End();

        }
        Logger.Info("ButtonDownLoad_Click End");
    }

    /// <summary>
    /// アプリケーションのパスをWeb.configから取得
    /// </summary>
    protected void GetConfig()
    {
        Logger.Info("GetConfig Start");

        configXmlFileDir = System.Configuration.ConfigurationManager.AppSettings[keyConfigXmlFileDir];
        mobileConfigTemplate = System.Configuration.ConfigurationManager.AppSettings[keyMobileConfigTemplate];

        Logger.Info("GetConfig End");
    }

    /// <summary>
    /// 文言の設定
    /// </summary>
    protected void setDisplayWord()
    {
        Logger.Info("setDisplayWord Start");

        pageTitle = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id001));
        Label_Word001.Text = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id001));
        Label_Word002.Text = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id002));

        placeholder01 = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id003));
        stfCd.DataBind();
        placeholder02 = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id004));
        password.DataBind();

        ButtonDownLoad.Text = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id005));

        Logger.Info("setDisplayWord End");
    }

    /// <summary>
    /// メッセージボックスの表示
    /// </summary>
    /// <param name="wordNo"></param>
    /// <param name="wordParam"></param>
    protected void ShowMessageBox(int wordNo, params string[] wordParam)
    {
        Logger.Info("ShowMessageBox Start");

        string word = WebWordUtility.GetWord(wordNo);
        if (null != wordParam && 0 < wordParam.Length)
        {
            word = String.Format(CultureInfo.InvariantCulture, word, wordParam);
        }
        StringBuilder alert = new StringBuilder();

        alert.Append("<script type='text/javascript'>");
        alert.Append("  alert('" + System.Web.HttpUtility.JavaScriptStringEncode(word) + "')");
        alert.Append("</script>");

        System.Web.UI.ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(this.GetType(), "alert", alert.ToString());

        Logger.Info("ShowMessageBox End");
    }
}