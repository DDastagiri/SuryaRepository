using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.eCRB.SystemFrameworks.Core;

using System.IO;
using System.Xml;
using Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic;

public class SC3A01102BusinessLogic : BaseBusinessComponent
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
    /// applicationBaseDir
    /// </summary>
    public string applicationBaseDir { set; get; }

    /// <summary>
    /// applicationBaseURL
    /// </summary>
    public string applicationBaseURL { set; get; }

    /// <summary>
    /// maxConnections
    /// </summary>
    public int maxConnections { set; get; }

    /// <summary>
    /// downLoadCountFile
    /// </summary>
    public string downLoadCountFile { set; get; }

    /// <summary>
    /// targetAppFile
    /// </summary>
    public string targetAppFile { set; get; }

    /// <summary>
    /// targetVersion
    /// </summary>
    public string targetVersion { set; get; }


    /// <summary>
    /// リクエストパラメータのチェックと取得
    /// </summary>
    /// <param name="param">リクエストパラメータ</param>
    /// <param name="defaultValue">デフォルト値</param>
    /// <returns></returns>
    public string GetRequestParam(string param, string defaultValue)
    {
        Logger.Info("GetRequestParam Start");

        //リクエストパラメータの取得 (バージョン)
        if (param != null && param.Length > 0)
        {
            Logger.Info("GetRequestParam End");
            return param;
        }
        else
        {
            Logger.Info("GetRequestParam End");
            return defaultValue;
        }
    }

    /// <summary>
    /// 認証処理
    /// </summary>
    /// <param name="UserID">ユーザID</param>
    /// <param name="Password">パスワード</param>
    /// <returns></returns>
    public Boolean Authorization(string UserID, string Password)
    {
        Logger.Info("Authorization Start");
        Users aUs = new Users();

        //ユーザ情報の取得
        Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.UsersDataSet.USERSRow UserInfo;
        UserInfo = aUs.GetUser(UserID, "0");
        if (UserInfo == null){
            Logger.Info("Authorization End");
            return false;
        }

        //パスワードの確認
        string DataBasePassword = (string)UserInfo["PASSWORD"];
        if (DataBasePassword != null){
            Logger.Info("Authorization End");
            return Password == DataBasePassword;
        }else{
            Logger.Info("Authorization End");
            return false;
        }
    }



    /// <summary>
    /// バージョンチェック
    /// </summary>
    /// <param name="oldVersion">古いバージョン</param>
    /// <param name="newVersion">新しいバージョン</param>
    /// <returns>引数の新旧が正しければTrue</returns>
    public Boolean CheckVersion(string oldVersion, string newVersion) {

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

        if ((int.Parse(arrnewVersionn[0]) > int.Parse(arroldVersion[0])) ||
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
    /// バージョンチェック(Equals)
    /// </summary>
    /// <param name="oldVersion">古いバージョン</param>
    /// <param name="newVersion">新しいバージョン</param>
    /// <returns>引数のバージョンが等しければTrue</returns>
    public Boolean CheckVersionEquals(string oldVersion, string newVersion)
    {
        Logger.Info("CheckVersionEquals Start");
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

        if ((int.Parse(arrnewVersionn[0]) == int.Parse(arroldVersion[0]) 
            && int.Parse(arrnewVersionn[1]) == int.Parse(arroldVersion[1]) 
            && int.Parse(arrnewVersionn[2]) == int.Parse(arroldVersion[2])))
        {
            Logger.Info("CheckVersionEquals End");
            return true;
        }
        else
        {
            Logger.Info("CheckVersionEquals End");
            return false;
        }
    }

    /// <summary>
    /// 対象ダウンロードファイルのパスを取得
    /// </summary>
    /// <param name="version">バージョン</param>
    /// <param name="dealerCode">販売店コード</param>
    /// <param name="appName">アプリ名</param>
    /// <returns></returns>
    public Boolean getTargetAppInfo(string version, string appName)
    {
        Logger.Info("getTargetAppInfo Start");

        //プロパティリセット
        targetVersion = "";
        targetAppFile = "";

        string[] xmlFiles = Directory.GetFiles(configXmlFileDir, "*.xml");

        XmlTextReader reader;
        string xmlVersion = "";
        string xmlDealerCode = "";
        string xmlAppName = "";
        string xmlReleaseDate = "";
        string xmlAppFile = "";

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

            //AppNameが一致
            if (appName == xmlAppName)
            {
                if (version == "")
                {
                    //バージョンが書かれていない場合
                    //最新バージョンファイル名を取得
                    if (string.IsNullOrEmpty(targetVersion))
                    {
                            //プロパティへ設定
                            targetVersion = xmlVersion;
                            targetAppFile = xmlAppFile;
                    }
                    else
                    {
                        if (CheckVersion(targetVersion, xmlVersion))
                        {
                            //プロパティへ設定
                            targetVersion = xmlVersion;
                            targetAppFile = xmlAppFile;
                        }
                    }
                }
                else
                {
                    //バージョンが書かれている場合
                    //対象バージョンファイル名を取得
                    if (CheckVersionEquals(xmlVersion, version))
                    {
                        //プロパティへ設定
                        targetVersion = version;
                        targetAppFile = xmlAppFile;
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(targetVersion) || string.IsNullOrEmpty(targetAppFile))
        {
            Logger.Info("getTargetAppInfo End");
            return false;
        }
        else
        {
            Logger.Info("getTargetAppInfo End");
            return true;
        }

    }

    /// <summary>
    /// ダウンロード対象のファイルを取得(iPadアプリ用)
    /// </summary>
    /// <param name="appName">ダウンロード対象アプリ名</param>
    /// <param name="targetVersion">ダウンロード対象バージョン</param>
    /// <param name="targetAppFile">ダウンロード対象ファイル名</param>
    /// <returns></returns>
    public string GetDownLoadFile_plist(string appName, string targetVersion, string targetAppFile)
    {
        Logger.Info("GetDownLoadFile_plist Start");

        string outPut = "";

        //ファイルを読込み
        string TargetAppFilePath = Path.Combine(@applicationBaseDir, appName, targetVersion, targetAppFile);

        //実際ファイル存在チェック
        if (!File.Exists(TargetAppFilePath))
        {
            //エラー記録
            Logger.Error("File Not Found. " + TargetAppFilePath);
            Logger.Info("GetDownLoadFile_plist End");
            return "";
        }

        try
        {
            //ファイル存在チェック
            if (File.Exists(TargetAppFilePath))
            {
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                outPut = System.IO.File.ReadAllText(TargetAppFilePath, enc);

                outPut = outPut.Replace("localhost", applicationDownLoadBaseURL + "/" + applicationBaseURL + "/" + appName + "/" + targetVersion + "/" + targetAppFile.Replace(".plist", ".ipa"));

                //Unix用に改行コードを置き換え
                outPut = outPut.Replace("\r\n", "\n");

            }
            else
            {
                Logger.Error("Template File Not Found.");
            }

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }

        Logger.Info("GetDownLoadFile_plist End");
        return outPut;
    }


    /// <summary>
    /// ダウンロードカウンタのチェックとカウントアップ
    /// </summary>
    /// <returns></returns>
    public Boolean checkDownLoadCount()
    {
        Logger.Info("checkDownLoadCount Start");

        try
        {
            string todayString = DateTime.Today.ToString("yyyy/MM/dd");
            int counter = 1;

            //カウントファイル読込み
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            String countData = System.IO.File.ReadAllText(downLoadCountFile, enc);
            if (!string.IsNullOrEmpty(countData))
            {
                if (countData.Contains(","))
                {
                    String[] arrCountData = countData.Split(',');

                    //日付チェック
                    if (todayString.Equals(arrCountData[0]))
                    {
                        //カウントのチェック
                        counter = int.Parse(arrCountData[1]);
                        if (maxConnections <= counter)
                        {
                            Logger.Error("[Inspection]checkDownLoadCount: Count Over:" + counter.ToString());
                            Logger.Info("checkDownLoadCount End");
                            return false;
                        }

                        //カウントをインクリメント
                        Logger.Error("[Inspection]checkDownLoadCount: CounterIncrement :" + counter.ToString() + "+1");
                        counter++;
                    }
                    else
                    {
                        //日付が変わっている場合はリセット
                    }
                }
            }

            //Shift_JISのファイルに書き込む。
            countData = todayString + "," + counter.ToString();
            Logger.Debug("checkDownLoadCount: FileWrite:" + countData);
            System.IO.File.WriteAllText(downLoadCountFile, countData, enc);

        }
        catch (Exception ex)
        {
            Logger.Error("checkDownLoadCount", ex);
        }

        Logger.Info("checkDownLoadCount End");
        return true;

    }

    /// <summary>
    /// ダウンロードカウンタのチェックとカウントアップ
    /// </summary>
    /// <returns></returns>
    public void downLoadCountDown()
    {
        Logger.Info("downLoadCountDown Start");

        try
        {
            string todayString = DateTime.Today.ToString("yyyy/MM/dd");
            int counter = 0;

            //カウントファイル読込み
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            String countData = System.IO.File.ReadAllText(downLoadCountFile, enc);
            if (!string.IsNullOrEmpty(countData))
            {
                if (countData.Contains(","))
                {
                    String[] arrCountData = countData.Split(',');

                    //日付チェック
                    if (todayString.Equals(arrCountData[0]))
                    {
                        //カウントをデクリメント
                        counter = int.Parse(arrCountData[1]);
                        counter--;
                    }
                    else
                    {
                        //日付が変わっている場合はリセット
                        counter = 0;
                    }
                }
            }

            //ファイルに書き込む。
            countData = todayString + "," + counter.ToString();
            Logger.Debug("downLoadCountDown: FileWrite:" + countData);
            System.IO.File.WriteAllText(downLoadCountFile, countData, enc);

        }
        catch (Exception ex)
        {
            Logger.Error("downLoadCountDown", ex);
        }
        Logger.Info("downLoadCountDown Start");

    }

}
