using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web.Configuration;
using System.IO;
using System.Xml;
//using log4net;

using Toyota.eCRB.SystemFrameworks.Configuration;
using Toyota.eCRB.SystemFrameworks.Core;
using Toyota.eCRB.SystemFrameworks.Web;
using Toyota.eCRB.Common.Login.BizLogic;
using Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic;


public partial class AppDownLoad_IC3A01101 : System.Web.UI.Page
{
    private const string moduleName = "IC3A01101.aspx";

    //Web.Configから設定値を取得するためのキー
    private const string keyApplicationDownLoadBaseURL = "applicationDownLoadBaseURL";
    private const string keyConfigXmlFileDir = "configXmlFileDir";
    private const string keyDefaultAppName = "defaultAppName";
    private const string keyRootingCheck = "rootingCheck";
    private string applicationDownLoadBaseURL;
    private string configXmlFileDir;
    private string defaultAppName;
    private string rootingCheck;

    /// <summary>
    /// ページロード時の処理
    /// </summary>
    /// <param name="sender">イベント発生元</param>
    /// <param name="e">イベントデータ</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Logger.Info("Page_Load Start");
        string appName, version, dealerCode;

        IC3A01101BusinessLogic bizLogic = new IC3A01101BusinessLogic();

        //Web.Configから取得
        GetConfig();

        //BusinessLogicへ値を設定
        bizLogic.configXmlFileDir = configXmlFileDir;
        bizLogic.applicationDownLoadBaseURL = applicationDownLoadBaseURL;

        //リクエストのBODYを読む(XMLの読込み）
        System.IO.StreamReader StreamRequest;
        StreamRequest = new System.IO.StreamReader(Request.InputStream);
        string StrBody = "";
        while (!StreamRequest.EndOfStream)
        {
            StrBody += StreamRequest.ReadLine();
        }

        Logger.Debug("RequestBody:" + StrBody);

        //バージョン
        version = bizLogic.GetPostRequestParam(StrBody, "version", "");
        //アプリ名
        appName = bizLogic.GetPostRequestParam(StrBody, "appName", defaultAppName);
        //販売店コード
        dealerCode = bizLogic.GetPostRequestParam(StrBody, "dealerCode", "");

        ////リクエストパラメータの取得
        ////バージョン
        //version = bizLogic.GetRequestParam(Request.QueryString["version"], "");
        ////アプリ名
        //appName = bizLogic.GetRequestParam(Request.QueryString["appName"], defaultAppName);
        ////販売店コード
        //dealerCode = bizLogic.GetRequestParam(Request.QueryString["dealerCode"], "");

        //最新版を検索
        string newestAppInfo = "";
        newestAppInfo = bizLogic.getNewestAppURL(version, dealerCode, appName);

        //レスポンス出力
        Response.Clear();
        Response.ContentType = "text/plain";
        Response.Write(newestAppInfo);
        Response.End();
        Logger.Info("Page_Load End");
    }

    /// <summary>
    /// アプリケーションのパスをWeb.configから取得
    /// </summary>
    protected void GetConfig()
    {
        Logger.Info("GetConfig Start");

        rootingCheck = System.Configuration.ConfigurationManager.AppSettings[keyRootingCheck];
        applicationDownLoadBaseURL = GetApplicationDownLoadBaseURL();

        configXmlFileDir = System.Configuration.ConfigurationManager.AppSettings[keyConfigXmlFileDir];
        defaultAppName = System.Configuration.ConfigurationManager.AppSettings[keyDefaultAppName];
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

}