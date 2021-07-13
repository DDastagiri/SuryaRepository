using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.eCRB.SystemFrameworks.Core;

using System.IO;
using System.Xml;
using Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic;

public class SC3A01103BusinessLogic : BaseBusinessComponent
{

    /// <summary>
    /// xmlDirectriy
    /// </summary>
    public string configXmlFileDir { set; get; }

    /// <summary>
    /// mobileConfigTemplate
    /// </summary>
    public string mobileConfigTemplate { set; get; }

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
        if (UserInfo == null)
        {
            return false;
        }

        //パスワードの確認
        string DataBasePassword = (string)UserInfo["PASSWORD"];
        if (DataBasePassword != null)
        {
            Logger.Info("Authorization End");
            return Password == DataBasePassword;
        }
        else
        {
            Logger.Info("Authorization End");
            return false;
        }
    }

    /// <summary>
    /// ダウンロード対象のファイルを取得(mobileConfig用)
    /// </summary>
    /// <param name="appName">ダウンロード対象アプリ名</param>
    /// <param name="targetVersion">ダウンロード対象バージョン</param>
    /// <param name="targetAppFile">ダウンロード対象ファイル名</param>
    /// <returns></returns>
    public string GetDownLoadFile_mobileConfig(string userID, string password)
    {
        Logger.Info("GetDownLoadFile_mobileConfig Start");

        string outPut = "";

        //ファイルを読込み
        string TargetAppFilePath = Path.Combine(@configXmlFileDir, mobileConfigTemplate);

        //実際ファイル存在チェック
        if (!File.Exists(TargetAppFilePath))
        {
            //エラー記録
            Logger.Error("File Not Found. " + TargetAppFilePath);
            return "";
        }

        try
        {
            //ファイル存在チェック
            if (File.Exists(TargetAppFilePath))
            {

                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                outPut = System.IO.File.ReadAllText(TargetAppFilePath, enc);

                //文字列置換
                outPut = outPut.Replace("<!--userID-->", userID);
                outPut = outPut.Replace("<!--password-->", password);

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

        Logger.Info("GetDownLoadFile_mobileConfig End");
        return outPut;
    }
}
