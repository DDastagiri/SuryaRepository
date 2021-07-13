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

public partial class AppDownLoad_SC3A01102 : System.Web.UI.Page
{
    private const string moduleName = "SC3A01102.aspx";


    //画面表示用
    public string pageTitle;
    public string appName;
    public string version;
    public string placeholder01;
    public string placeholder02;

    //Web.Configから設定値を取得するためのキー
    private const string keyApplicationDownLoadBaseURL = "applicationDownLoadBaseURL";
    private const string keyApplicationBaseDir = "applicationBaseDir";
    private const string keyApplicationBaseURL = "applicationBaseURL";
    private const string keyConfigXmlFileDir = "configXmlFileDir";
    private const string keydefaultAppName = "defaultAppName";
    private const string keyMaxConnections = "maxConnections";
    private const string keyDownLoadCountFile = "downLoadCountFile";
    private const string keyRootingCheck = "rootingCheck";
    private string applicationDownLoadBaseURL;
    private string applicationBaseDir;
    private string applicationBaseURL;
    private string configXmlFileDir;
    private string defaultAppName;
    private int maxConnections;
    private string downLoadCountFile;
    private string rootingCheck;

    const string APPLICATIONID = "SC3A01102";
    private const int id001 = 001;
    private const int id002 = 002;
    private const int id003 = 003;
    private const int id004 = 004;
    private const int id005 = 005;
    private const int id006 = 006;
    private const int id007 = 007;
    private const int id008 = 008;

    private const int id901 = 901;  //UserIDがない
    private const int id902 = 902;  //認証失敗
    private const int id903 = 903;  //ダウンロード数オーバー

    //plistファイルの拡張子
    public const string plistExt = ".plist";

    /// <summary>
    /// ページロード時の処理
    /// </summary>
    /// <param name="sender">イベント発生元</param>
    /// <param name="e">イベントデータ</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Logger.Info("Page_Load Start");

        string[] str = { "" };
        SC3A01102BusinessLogic bizLogic = new SC3A01102BusinessLogic();

        //Web.Configから取得
        GetConfig();

        //BusinessLogicへ値を設定
        bizLogic.configXmlFileDir = configXmlFileDir;
        bizLogic.applicationBaseDir = applicationBaseDir;
        bizLogic.applicationDownLoadBaseURL = applicationDownLoadBaseURL;
        bizLogic.applicationBaseURL = applicationBaseURL;
        bizLogic.maxConnections = maxConnections;
        bizLogic.downLoadCountFile = downLoadCountFile;

        //画面文言の設定
        setDisplayWord();

        //PathInfoが付与されている場合はplistファイルのダウンロード
        if (!string.IsNullOrEmpty(Request.PathInfo))
        {
            string PathInfo = Request.PathInfo;
            string[] arrPathInfo = PathInfo.Split('/');

            //アプリ名
            appName = arrPathInfo[1];
            //バージョン
            version = arrPathInfo[2];
            //ファイル名
            string fileName = arrPathInfo[3] + plistExt;

            //ダウンロード数のチェックとカウントアップ
            if (!bizLogic.checkDownLoadCount())
            {
                Logger.Info("Page_Load: Count Over." + appName + " " + version + " " + fileName);
                ShowMessageBox(id903, str);
                Logger.Info("Page_Load End");
                return;
            }

            try
            {
                //plistの場合
                //ダウンロードファイルを取得
                string outPut = bizLogic.GetDownLoadFile_plist(appName, version, fileName);

                //Responseを作成
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.Write(outPut);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                Logger.Error("Page_Load", ex);
            }
            finally
            {
                //ダウンロード数のカウントダウン
                bizLogic.downLoadCountDown();
                Logger.Info("Page_Load: End DownLoad");
                Response.End();
            }
        }
        else
        {
            //リクエストパラメータを取得
            //バージョン
            version = bizLogic.GetRequestParam(Request.QueryString["version"], "");
            //アプリ名
            appName = bizLogic.GetRequestParam(Request.QueryString["appName"], defaultAppName);
        }
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
        SC3A01102BusinessLogic bizLogic = new SC3A01102BusinessLogic();

        //Web.Configから取得
        GetConfig();

        //BusinessLogicへ値を設定
        bizLogic.configXmlFileDir = configXmlFileDir;
        bizLogic.applicationBaseDir = applicationBaseDir;
        bizLogic.applicationDownLoadBaseURL = applicationDownLoadBaseURL;
        bizLogic.applicationBaseURL = applicationBaseURL;
        bizLogic.maxConnections = maxConnections;
        bizLogic.downLoadCountFile = downLoadCountFile;

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
            Logger.Info("ButtonDownLoad_Click Start");
            return;
        }

        //認証OKの場合、ダウンロード開始
        if (AuthorizationCheck)
        {

            Logger.Info("ButtonDownLoad_Click: DownLoad Request. appName:[" + appName + "] version:[" + version + "]");

            //ダウンロード対象のファイル情報を取得
            Boolean checkTargetAppInfo = bizLogic.getTargetAppInfo(version, appName);
            if (checkTargetAppInfo)
            {
                string targetAppFile = bizLogic.targetAppFile;
                string targetVersion = bizLogic.targetVersion;
                Logger.Info("ButtonDownLoad_Click: DownLoad Target. appFile:[" + targetAppFile + "] version:[" + targetVersion + "]");

                if (targetAppFile.Contains(plistExt))
                {

                    Logger.Info("ButtonDownLoad_Click: Start DownLoad [plist]");

                    //iOSアプリ(.plist)の場合はURLスキームが必要な為、JavaScriptでリダイレクトしてForm_Loadで行う。
                    string url = "";
                    url += url + "itms-services://?action=download-manifest&url=";
                    url += applicationDownLoadBaseURL + "/";
                    url += "SC3A01102.aspx/" + appName + "/" + targetVersion + "/" + targetAppFile.Replace(plistExt, "");
                    JavaScriptRedirect(url);

                }
                else
                {

                    //ダウンロード数のチェックとカウントアップ
                    if (!bizLogic.checkDownLoadCount())
                    {
                        Logger.Info("ButtonDownLoad_Click: Count Over." + UserID + " " + targetAppFile + " " + targetVersion);
                        ShowMessageBox(id903, str);
                        Logger.Info("ButtonDownLoad_Click End");
                        return;
                    }

                    try
                    {
                        //plist以外の場合
                        //ファイルを読込み
                        string TargetAppFilePath = Path.Combine(@applicationBaseDir, appName, targetVersion, targetAppFile);

                        Logger.Info("ButtonDownLoad_Click: Start DownLoad [exe]");

                        //Responseを作成
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + targetAppFile + "\"");
                        Response.WriteFile(TargetAppFilePath);
                        Response.Flush();

                    }
                    catch (Exception ex)
                    {
                        Logger.Error("ButtonDownLoad_Click", ex);
                    }
                    finally
                    {
                        //ダウンロード数のカウントダウン
                        bizLogic.downLoadCountDown();
                        Logger.Info("ButtonDownLoad_Click: End DownLoad");
                        Response.End();
                    }

                }
            }
        }
        Logger.Info("ButtonDownLoad_Click End");
    }

    /// <summary>
    /// アプリケーションのパスをWeb.configから取得
    /// </summary>
    protected void GetConfig()
    {
        Logger.Info("GetConfig Start");

        rootingCheck = System.Configuration.ConfigurationManager.AppSettings[keyRootingCheck];

        applicationDownLoadBaseURL = GetApplicationDownLoadBaseURL();
        applicationBaseDir = System.Configuration.ConfigurationManager.AppSettings[keyApplicationBaseDir];
        applicationBaseURL = System.Configuration.ConfigurationManager.AppSettings[keyApplicationBaseURL];
        configXmlFileDir = System.Configuration.ConfigurationManager.AppSettings[keyConfigXmlFileDir];
        defaultAppName = System.Configuration.ConfigurationManager.AppSettings[keydefaultAppName];

        maxConnections = int.Parse(System.Configuration.ConfigurationManager.AppSettings[keyMaxConnections]);
        downLoadCountFile = System.Configuration.ConfigurationManager.AppSettings[keyDownLoadCountFile];

        Logger.Info("GetConfig End");
    }

    /// <summary>
    /// アプリケーションダウンロードディレクトリのURLを取得
    /// </summary>
    /// <returns></returns>
    protected string GetApplicationDownLoadBaseURL()
    {
        Logger.Info("GetApplicationDownLoadBaseURL Start");
        string reqUrl = Request.Url.ToString();
        int slpos = reqUrl.LastIndexOf("/" + moduleName);
        reqUrl = reqUrl.Substring(0, slpos);

        //Big-IPを通したリクエストの場合、Port番号を削除
        if (rootingCheck == "1")
        {
            int pdpos = reqUrl.IndexOf(':', 6);
            slpos = reqUrl.IndexOf('/', pdpos);
            reqUrl = reqUrl.Substring(0, pdpos) + "" + reqUrl.Substring(slpos);
        }

        Logger.Info("GetApplicationDownLoadBaseURL End");
        return reqUrl;
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
        Label_Word003.Text = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id003));
        Label_Word005.Text = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id005));

        placeholder01 = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id006));
        stfCd.DataBind();
        placeholder02 = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id007));
        password.DataBind();

        ButtonDownLoad.Text = System.Web.HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, id008));

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

    /// <summary>
    /// JavaScriptリダイレクトの実行
    /// </summary>
    /// <param name="url">リダイレクト先</param>
    protected void JavaScriptRedirect(string url)
    {
        Logger.Info("JavaScriptRedirect Start");
        StringBuilder redirecturl = new StringBuilder();

        redirecturl.Append("<script type='text/javascript'>");
        redirecturl.Append("  location.href='" + url + "';");
        redirecturl.Append("</script>");

        System.Web.UI.ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(this.GetType(), "alert", redirecturl.ToString());

        Logger.Info("JavaScriptRedirect End");
    }
}