using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.eCRB.SystemFrameworks.Core;

using System.IO;
using System.Xml;
using Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic;

public class IC3A01101BusinessLogic : BaseBusinessComponent
{

    /// <summary>
    /// xmlDirectriy
    /// </summary>
    public string configXmlFileDir { set; get; }

    /// <summary>
    /// applicationDownLoadBaseURL
    /// </summary>
    public string applicationDownLoadBaseURL { set; get; }

    /// <summary>
    /// リクエストパラメータの取得
    /// </summary>
    /// <returns></returns>
    public string GetPostRequestParam(string RequestBody, string key, string defaultvalue)
    {
        Logger.Info("GetPostRequestParam Start");

        string buffer = "";
        int keypos = RequestBody.IndexOf(key);
        if (keypos >= 0)
        {
            buffer = RequestBody.Substring(keypos + key.Length + 1);
        }

        keypos = buffer.IndexOf('&');
        if (keypos >= 0)
        {
            buffer = buffer.Substring(0, keypos);
        }

        Logger.Info("GetPostRequestParam End");
        return buffer;
    }

    /// <summary>
    /// バージョンチェック
    /// </summary>
    /// <param name="newVersion">新しいバージョン</param>
    /// <param name="oldVersion">古いバージョン</param>
    /// <returns>引数の新旧が正しければTrue</returns>
    private Boolean CheckVersion(string newVersion, string oldVersion) {

        Logger.Info("CheckVersion Start");
        string subVersion = "";

        //newVersionを.で切り分け
        if ((newVersion == null) || (newVersion.Length <= 0))        
        {
            subVersion = "0.0.0";
        }
        else 
        {
            subVersion = newVersion + ".0.0.0";
        }
        string[] arrnewVersionn = new string[3];
        arrnewVersionn = subVersion.Split('.');

        //oldVersionを.で切り分け
        if ((oldVersion == null) || (oldVersion.Length <= 0))
        {
            subVersion = "0.0.0";
        }
        else
        {
            subVersion = oldVersion + ".0.0.0";
        }
        string[] arroldVersion = new string[3];
        arroldVersion = subVersion.Split('.');

        if ((int.Parse(arrnewVersionn[0]) >  int.Parse(arroldVersion[0])) ||
            (int.Parse(arrnewVersionn[0]) == int.Parse(arroldVersion[0]) && int.Parse(arrnewVersionn[1]) > int.Parse(arroldVersion[1])) ||
            (int.Parse(arrnewVersionn[0]) == int.Parse(arroldVersion[0]) && int.Parse(arrnewVersionn[1]) == int.Parse(arroldVersion[1]) && int.Parse(arrnewVersionn[2]) > int.Parse(arroldVersion[2])))
        {
            Logger.Info("CheckVersion End");
            return true;
        }
        else
        {
            Logger.Info("CheckVersion End");
            return false;
        }
    }


    /// <summary>
    /// 最新端末アプリの情報を取得
    /// </summary>
    /// <param name="version">リクエストパラメータのバージョン</param>
    /// <param name="dealerCode">リクエストパラメータの販売店コード</param>
    /// <param name="appName">リクエストパラメータのアプリ名</param>
    /// <returns>最新端末アプリのダウンロードページ</returns>
    public string getNewestAppURL(string version, string dealerCode, string appName)
    {
        Logger.Info("getNewestAppURL Start");

        //string newestAppVersion = "";
        string newestAppFile = "";
        string[] xmlFiles = Directory.GetFiles(configXmlFileDir, "*.xml");

        string xmlVersion = "";
        string xmlDealerCode = "";
        string xmlAppName = "";
        string xmlReleaseDate = "";
        string xmlAppFile = "";
        DateTime dtToday = DateTime.Today;
        DateTime dtXmlReleaseDate;
        XmlTextReader reader;

        string newestversion = "0.0.0";

        //xmlサーチ
        foreach (string xmlFile in xmlFiles)
        {
            //XMLファイルを読込み
            reader = new XmlTextReader(xmlFile);
            while (reader.Read())
            {
                switch (reader.LocalName)
                {
                    case "AppName":
                        xmlAppName = reader.ReadString();
                        break;
                    case "ReleaseDate":
                        xmlReleaseDate = reader.ReadString();
                        break;
                    case "DealerCode":
                        xmlDealerCode = reader.ReadString();
                        break;
                    case "Version":
                        xmlVersion = reader.ReadString();
                        break;
                    case "AppFile":
                        xmlAppFile = reader.ReadString();
                        break;
                }
            }

            //XMLに設定されたアプリケーションの最新バージョンを検索
            //DealerCodeとAppNameが一致
            if (appName == xmlAppName && dealerCode == xmlDealerCode)
            {
                //公開日を超えている
                dtXmlReleaseDate = DateTimeFunc.FormatString("yyyy/MM/dd", xmlReleaseDate);
                //dtXmlReleaseDate = DateTime.ParseExact(xmlReleaseDate, "");// .Parse(xmlReleaseDate);
                if (dtXmlReleaseDate <= dtToday)
                {
                    //読み込んだバージョンが最新であればバッファに保存
                    if (CheckVersion(xmlVersion, newestversion))
                    {
                        //newestversionに代入
                        newestversion = xmlVersion;
                        //newestappfileに代入
                        newestAppFile = xmlAppFile;
                    }
                }
            }
        }

        //最新端末アプリのダウンロードページ
        if (CheckVersion(newestversion, version))
        {
            Logger.Info("getNewestAppURL End");
            return applicationDownLoadBaseURL + "/" + "SC3A01102.aspx" + "?dealerCode=" + dealerCode + "&appName=" + appName + "&version=" + newestversion;
        }
        else
        {
            Logger.Info("getNewestAppURL End");
            return "";
        }
    }

}
