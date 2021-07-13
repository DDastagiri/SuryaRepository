'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3040401BusinessLogic.vb
'─────────────────────────────────────
'機能： CalDAV連携バッチ
'補足： 
'作成： 2011/12/01 KN   梅村
'更新： 2012/02/13 KN   梅村    【SALES_1A】ログ出力の不具合修正
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $02
'更新： 2014/06/05 TMEJ y.gotoh 受注後フォロー機能開発 $03
'更新： 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'─────────────────────────────────────

Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.iCROP.DataAccess.MC3040401.MC3040401DataSet
Imports Toyota.eCRB.iCROP.DataAccess.MC3040401.MC3040401DataSetTableAdapters
Imports Toyota.eCRB.iCROP.DataAccess.MC3040401
Imports System.Globalization

Public Class MC3040401BussinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    '$03 受注後フォロー機能開発 START
    ''' <summary>
    ''' 終了コード(0)
    ''' </summary>
    ''' <remarks>正常終了</remarks>
    Private Const C_MESSAGE_0 As Integer = 0


    ''' <summary>
    ''' エラーコード(100)
    ''' </summary>
    ''' <remarks>処理中にException発生</remarks>
    Private Const C_MESSAGE_100 As Integer = 100
    '$03 受注後フォロー機能開発 END

    ''' <summary>
    ''' エラーコード(901)
    ''' </summary>
    ''' <remarks>バッチ開始日時取得エラー</remarks>
    Private Const C_MESSAGE_901 As Integer = 901

    ''' <summary>
    ''' エラーコード(902)
    ''' </summary>
    ''' <remarks>前回バッチ起動日時取得エラー</remarks>
    Private Const C_MESSAGE_902 As Integer = 902

    ''' <summary>
    ''' エラーコード(903)
    ''' </summary>
    ''' <remarks>再登録用の未登録スケジュール情報取得エラー</remarks>
    Private Const C_MESSAGE_903 As Integer = 903

    ''' <summary>
    ''' エラーコード(904)
    ''' </summary>
    ''' <remarks>割当済みに変更された活動担当スタッフ情報の取得エラー</remarks>
    Private Const C_MESSAGE_904 As Integer = 904

    ''' <summary>
    ''' エラーコード(905)
    ''' </summary>
    ''' <remarks>割当済みに変更された受付担当スタッフ情報の取得エラー</remarks>
    Private Const C_MESSAGE_905 As Integer = 905

    ''' <summary>
    ''' エラーコード(906)
    ''' </summary>
    ''' <remarks>スケジュール情報に反映が必要なサービス関連情報の取得エラー</remarks>
    Private Const C_MESSAGE_906 As Integer = 906

    ''' <summary>
    ''' エラーコード(907)
    ''' </summary>
    ''' <remarks>作成済みスケジュール情報の活動担当スタッフ変更情報の取得エラー</remarks>
    Private Const C_MESSAGE_907 As Integer = 907

    ''' <summary>
    ''' エラーコード(908)
    ''' </summary>
    ''' <remarks>作成済みスケジュール情報の受付担当スタッフ変更情報の取得エラー</remarks>
    Private Const C_MESSAGE_908 As Integer = 908

    ''' <summary>
    ''' エラーコード(915)
    ''' </summary>
    ''' <remarks>顧客敬称の取得エラー</remarks>
    Private Const C_MESSAGE_915 As Integer = 915

    ''' <summary>
    ''' エラーコード(916)
    ''' </summary>
    ''' <remarks>敬称位置の取得エラー</remarks>
    Private Const C_MESSAGE_916 As Integer = 916

    ''' <summary>
    ''' エラーコード(917)
    ''' </summary>
    ''' <remarks>接触方法名の取得エラー</remarks>
    Private Const C_MESSAGE_917 As Integer = 917

    ''' <summary>
    ''' エラーコード(918)
    ''' </summary>
    ''' <remarks>言語区分の取得エラー</remarks>
    Private Const C_MESSAGE_918 As Integer = 918

    ''' <summary>
    ''' エラーコード(919)
    ''' </summary>
    ''' <remarks>サービス名の取得エラー</remarks>
    Private Const C_MESSAGE_919 As Integer = 919

    ''' <summary>
    ''' エラーコード(920)
    ''' </summary>
    ''' <remarks>商品名の取得エラー</remarks>
    Private Const C_MESSAGE_920 As Integer = 920

    ''' <summary>
    ''' エラーコード(922)
    ''' </summary>
    ''' <remarks>WEBサービス送信エラー</remarks>
    Private Const C_MESSAGE_922 As Integer = 922

    ''' <summary>
    ''' エラーコード(923)
    ''' </summary>
    ''' <remarks>未登録スケジュール情報の過去データ削除エラー</remarks>
    Private Const C_MESSAGE_923 As Integer = 923

    ''' <summary>
    ''' エラーコード(924)
    ''' </summary>
    ''' <remarks>前回バッチ起動日時取得エラー</remarks>
    Private Const C_MESSAGE_924 As Integer = 924

    ''' <summary>
    ''' エラーコード(931)
    ''' </summary>
    ''' <remarks>顧客名変更情報の取得処理エラー</remarks>
    Private Const C_MESSAGE_931 As Integer = 931

    ''' <summary>
    ''' エラーコード(932)
    ''' </summary>
    ''' <remarks>削除された顧客情報の取得エラー</remarks>
    Private Const C_MESSAGE_932 As Integer = 932

    ''' <summary>
    ''' エラーコード(999)
    ''' </summary>
    ''' <remarks>WebExceptionエラー</remarks>
    Private Const C_MESSAGE_999 As Integer = 999

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM = "MC3040401"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_MESSAGE_ID = "IC3040403"

    ''' <summary>
    ''' SYSTEM識別コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_LINK_SYSTEM_CODE As String = "0"

    ''' <summary>
    ''' WebサービスのURL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_CALDAV_WEBSERVICE_URL As String = "CALDAV_WEBSERVICE_URL"

    ''' <summary>
    ''' 販売店共通コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_COMMON_DEALERCODE As String = "XXXXX"

    ''' <summary>
    ''' 敬称位置取得用パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_KEISYO_POSISION As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' スケジュール関連の文言取得用パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SCHEDULE As String = "SCHEDULE"

    ''' <summary>
    ''' 来店フォロー名取得用キー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SCHEDULE_KEY1 As Integer = 1

    ''' <summary>
    ''' 仮予約名取得用キー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SCHEDULE_KEY2 As Integer = 2

    ''' <summary>
    ''' DataSet TableAdapters
    ''' </summary>
    ''' <remarks></remarks>
    Private da As MC3040401ScheduleDataSetTableAdapters

    ''' <summary>
    ''' 更新用のスケジュール情報
    ''' </summary>
    ''' <remarks></remarks>
    Private scheduleInfoDataTable As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable

    ''' <summary>
    ''' バッチ処理開始日時情報
    ''' </summary>
    ''' <remarks></remarks>
    Private batchProcInfoDataTable As MC3040401DataSet.MC3040401BatchProcInfoDataTable = New MC3040401DataSet.MC3040401BatchProcInfoDataTable

    ''' <summary>
    ''' 接触方法名情報
    ''' </summary>
    ''' <remarks></remarks>
    Private contactNameDataTable As MC3040401DataSet.MC3040401ContactNameInfoDataTable = New MC3040401DataSet.MC3040401ContactNameInfoDataTable

    ''' <summary>
    ''' サービス名情報
    ''' </summary>
    ''' <remarks></remarks>
    Private serviceNameDataTable As MC3040401DataSet.MC3040401ServiceNameInfoDataTable = New MC3040401DataSet.MC3040401ServiceNameInfoDataTable

    ''' <summary>
    ''' 商品名情報
    ''' </summary>
    ''' <remarks></remarks>
    Private merchandiseNameDataTable As MC3040401DataSet.MC3040401MerchandiseNameInfoDataTable = New MC3040401DataSet.MC3040401MerchandiseNameInfoDataTable

    ''' <summary>
    ''' 顧客敬称情報
    ''' </summary>
    ''' <remarks></remarks>
    Private nameTitleDataTable As MC3040401DataSet.MC3040401NameTitleInfoDataTable = New MC3040401DataSet.MC3040401NameTitleInfoDataTable
    '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END

    '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' BatchSetting セクション 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SETTING_SECTION As String = "SETTINGINFO"

    ''' <summary>
    ''' BatchSetting キー 英語表記販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KEY_USEENGLISHDLRCD As String = "UseEnglishDlrCd"

    ''' <summary>
    ''' BatchSetting String デフォルト値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_STRING_VALUE As String = ""

    ''' <summary>
    ''' 言語区分：現地語
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LANG_DIV_LOCAL As String = "1"

    ''' <summary>
    ''' 言語区分：英語
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LANG_DIV_ENG As String = "0"
    '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

    ''' <summary>
    ''' BatchSetting キー 送信最大件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KEY_SENDMAXCOUNT As String = "SendMaxCount"

    ''' <summary>
    ''' BatchSetting キー 送信最大件数(デフォルト)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEND_MAXCOUNT_DEFAULT As Integer = 500

    '$03 受注後フォロー機能開発 START
    ''' <summary>
    ''' BatchSetting キー タイムアウト時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KEY_TIMEOUTPERIOD As String = "TimeoutPeriod"

    ''' <summary>
    ''' 受注前後区分(受注前)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ODR_BEFORE_AFTER_DIV_BEFORE As String = "1"

    ''' <summary>
    ''' 受注前後区分(受注後)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ODR_BEFORE_AFTER_DIV_AFTER As String = "2"

    ''' <summary>
    ''' 半角空白
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BLANK_STRING As String = " "
    '$03 受注後フォロー機能開発 END
#End Region

#Region "変数"
    ''' <summary>
    ''' メッセージコード
    ''' </summary>
    ''' <remarks></remarks>
    Private messageCode As Integer = C_MESSAGE_0

#End Region

'$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
#Region "現在日時の取得処理"

    '''-------------------------------------------------------
    ''' <summary>
    ''' 現在日時の取得処理
    ''' </summary>
    ''' <param name="batchStartDateTime">バッチ起動日時</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
    ''' </History>
    '''------------------------------------------------------- 
    <EnableCommit()>
    Public Function GetBatchStartDateTime(ByRef batchStartDateTime As DateTime) As Integer

        Dim resultCode As Integer = 0

        messageCode = C_MESSAGE_0

        Try
            Logger.Info(C_SYSTEM & " GetBatchStartDateTime Start")

            '現在日時(バッチ開始日時)の取得
            messageCode = C_MESSAGE_901
            batchStartDateTime = DateTimeFunc.Now()
            Logger.Info(C_SYSTEM & " Get Now:" + batchStartDateTime)

            Logger.Info(C_SYSTEM & " GetBatchStartDateTime End")

            Return resultCode

        Catch ex As Exception

            'ログ
            Logger.Error("ORDER_BEFORE_AFTER_DIV:GET_BATCH_START_DATETIME")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" + ex.StackTrace)
            Return C_MESSAGE_100

        End Try

    End Function

#End Region

#Region "前回バッチ起動日時取得"

    '''-------------------------------------------------------
    ''' <summary>
    ''' 前回バッチ起動日時取得
    ''' </summary>
    ''' <param name="lastProcDate">前回バッチ起動日時</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2019/04/23 NSK a.ohhira 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
    ''' </History>
    '''------------------------------------------------------- 
    <EnableCommit()>
    Public Function GetLastProcDate(ByRef lastProcDate As DateTime) As Integer

        Dim resultCode As Integer = 0

        messageCode = C_MESSAGE_0

        Try
            Logger.Info(C_SYSTEM & " GetLastProcDate Start")
            da = New MC3040401ScheduleDataSetTableAdapters

            '前回バッチ起動日時の取得
            messageCode = C_MESSAGE_902
            batchProcInfoDataTable = da.SelectLastProcInfo()
            If batchProcInfoDataTable.Count = 0 OrElse batchProcInfoDataTable.Item(0).IsLASTPROCDATETIMENull Then
                'ログ
                Logger.Error("ORDER_BEFORE_AFTER_DIV:GET_LAST_PROC_DATE")
                Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()))
                Return C_MESSAGE_100
            Else
                lastProcDate = DateTime.Parse(batchProcInfoDataTable.Item(0).LASTPROCDATETIME, CultureInfo.InvariantCulture())
            End If
            Logger.Info(C_SYSTEM & " Before ProcDate:" + lastProcDate)

            Logger.Info(C_SYSTEM & " GetLastProcDate End")

            Return resultCode

        Catch ex As Exception

            'ログ
            Logger.Error("ORDER_BEFORE_AFTER_DIV:GET_LAST_PROC_DATE")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" + ex.StackTrace)
            Return C_MESSAGE_100

        End Try

    End Function

#End Region

#Region "未登録スケジュール情報の過去データ削除"

    '''-------------------------------------------------------
    ''' <summary>
    ''' 未登録スケジュール情報の過去データ削除
    ''' </summary>
    ''' <remarks></remarks>
    ''' <param name="batchStartDateTime">バッチ起動日時</param>
    ''' <History>
    '''  2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
    ''' </History>
    '''------------------------------------------------------- 
    <EnableCommit()>
    Public Function DeleteUnregistScheduleInfo(ByVal batchStartDateTime As DateTime) As Integer

        Dim resultCode As Integer = 0

        messageCode = C_MESSAGE_0

        Try
            Logger.Info(C_SYSTEM & " DeleteUnregistScheduleInfo Start")

            'DataSetTableAdapters
            da = New MC3040401ScheduleDataSetTableAdapters

            '未登録スケジュール情報テーブルから過去データ(昨日までのデータ)を削除
            messageCode = C_MESSAGE_923

            da.DeleteUnregistScheduleInfo(batchStartDateTime)

            Logger.Info(C_SYSTEM & " DeleteUnregistScheduleInfo End")

            Return resultCode

        Catch ex As Exception

            'ログ
            Logger.Error("ORDER_BEFORE_AFTER_DIV:DELETEUNREGISTSCHEDULEINFO")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" + ex.StackTrace)
            Return C_MESSAGE_100

        End Try

    End Function

#End Region
    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

#Region "スケジュール情報送信処理"

    '''-------------------------------------------------------
    ''' <summary>
    ''' スケジュール情報送信処理
    ''' </summary>
    ''' <remarks></remarks>
    ''' <param name="batchStartDateTime">バッチ起動日時</param>
    ''' <param name="lastProcDate">前回バッチ起動日時</param>
    ''' <History>
    '''  2012/02/13 KN 梅村 【SALES_1A】ログ出力の不具合修正
    '''  2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
    '''  2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </History>
    '''------------------------------------------------------- 
    <EnableCommit()>
    Public Function SendScheduleInfo(ByVal batchStartDateTime As DateTime, ByVal lastProcDate As DateTime) As Integer
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'Public Function SendScheduleInfo() As Integer
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'Dim batchStartDateTime As DateTime
        'Dim lastProcDate As DateTime
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Dim resultCode As Integer = 0

        messageCode = C_MESSAGE_0

        Try
            Logger.Info(C_SYSTEM & " SendScheduleInfo Start")

            'DataSetTableAdapters
            da = New MC3040401ScheduleDataSetTableAdapters

            '更新用のスケジュール情報
            scheduleInfoDataTable = New MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable

            '現在日時(バッチ開始日時)の取得
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'messageCode = C_MESSAGE_901
            'batchStartDateTime = DateTimeFunc.Now()
            'Logger.Info(C_SYSTEM & " Get Now:" + batchStartDateTime)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            '前回バッチ起動日時の取得
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'messageCode = C_MESSAGE_902
            'batchProcInfoDataTable = da.SelectLastProcInfo()
            'If batchProcInfoDataTable.Count = 0 OrElse batchProcInfoDataTable.Item(0).IsLASTPROCDATETIMENull Then
            'ログ
            '    Logger.Error("ORDER_BEFORE_AFTER_DIV:BEFORE")
            '    Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()))
            '    Return C_MESSAGE_100
            'Else
            '    lastProcDate = DateTime.Parse(batchProcInfoDataTable.Item(0).LASTPROCDATETIME, CultureInfo.InvariantCulture())
            'End If
            'Logger.Info(C_SYSTEM & " Before ProcDate:" + lastProcDate)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            'スケジュール情報の取得
            scheduleInfoDataTable = CreateScheduleInfoDataTable(lastProcDate)

            '更新データが1件以上存在する場合のみ、WEBサービス呼び出し処理を実施
            '更新処理区分を元にXMLを作成
            If scheduleInfoDataTable.Rows.Count > 0 Then

                Dim totalUpdateCount As Decimal = 0
                Dim loopFlg As Boolean = True
                Dim rowIndex As Integer = 0
                ' 1回の送信で最大件数を取得
                Dim sendMaxCount As Integer = GetSendMaxCount()
                Logger.Info(C_SYSTEM & " sendMaxCount " & sendMaxCount)

                Do While loopFlg

                    '送信Xmlの作成
                    Dim xmlDoc As New System.Xml.XmlDocument
                    Dim xmlDecl As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)

                    xmlDoc.AppendChild(xmlDecl)

                    Dim rootElement As XmlElement = xmlDoc.CreateElement("RegistSchedule")
                    xmlDoc.AppendChild(rootElement)

                    'Headの作成
                    Dim headElement As XmlElement = xmlDoc.CreateElement("Head")
                    rootElement.AppendChild(headElement)

                    SetNode(xmlDoc, headElement, "MessageID", C_MESSAGE_ID)
                    SetNode(xmlDoc, headElement, "CountryCode", EnvironmentSetting.CountryCode)
                    SetNode(xmlDoc, headElement, "LinkSystemCode", C_LINK_SYSTEM_CODE)
                    SetNode(xmlDoc, headElement, "TransmissionDate", batchStartDateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                    'detailタグを作成
                    Dim UpdateCount As Integer
                    UpdateCount = CreateDetailTag(scheduleInfoDataTable, xmlDoc, rootElement, batchStartDateTime, sendMaxCount, rowIndex)

                    '更新件数
                    Logger.Info(C_SYSTEM + " Update Count:" + UpdateCount.ToString(CultureInfo.InvariantCulture))
                    totalUpdateCount = totalUpdateCount + UpdateCount

                    If UpdateCount > 0 Then

                        'WEBサービス送信処理
                        messageCode = C_MESSAGE_922
                        Dim dlrEnv As DealerEnvSetting = New DealerEnvSetting
                        Dim url As DlrEnvSettingDataSet.DLRENVSETTINGRow = dlrEnv.GetEnvSetting(C_COMMON_DEALERCODE, C_CALDAV_WEBSERVICE_URL)

                        Dim sendString As String
                        Dim returnString As String
                        Dim xmlResult As XmlDocument = New XmlDocument

                        'XML名前空間を除去
                        Logger.Info(xmlDoc.OuterXml)
                        Dim regex As Regex = New Regex(" xmlns=""[^""]*""")
                        sendString = regex.Replace(xmlDoc.OuterXml, Space(0))

                        sendString = "xsData=" + HttpUtility.UrlEncode(sendString)

                        'Web参照
                        '$03 受注後フォロー機能開発 START
                        returnString = CallWebServiceSite(sendString, url.PARAMVALUE + "/RegistSchedule")
                        '$03 受注後フォロー機能開発 END
                        xmlResult.LoadXml(returnString)

                        For Each resuleIdNode As XmlNode In xmlResult.SelectNodes("descendant::Common")
                            If Not "0".Equals(resuleIdNode.SelectSingleNode("ResultId").InnerXml) Then
                                'ログ
                                Logger.Info("ORDER_BEFORE_AFTER_DIV:BEFORE")
                                Logger.Info("Web Service Error:" & resuleIdNode.SelectSingleNode("ResultId").InnerXml)
                            End If
                        Next
                    Else
                        loopFlg = False
                    End If
                Loop

                Logger.Info(C_SYSTEM + "Total Update Count:" + totalUpdateCount.ToString(CultureInfo.InvariantCulture))
            End If

            '$03 受注後フォロー機能開発 START
            '未登録スケジュール情報テーブルから過去データ(昨日までのデータ)を削除
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'messageCode = C_MESSAGE_923

            'da.DeleteUnregistScheduleInfo(batchStartDateTime, ODR_BEFORE_AFTER_DIV_BEFORE)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            '前回バッチ起動日時の更新
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'messageCode = C_MESSAGE_924

            'da.UpdateBatchDateTimeInfo(batchStartDateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()),
            '               batchStartDateTime)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
            '$03 受注後フォロー機能開発 END

            Logger.Info(C_SYSTEM & " SendScheduleInfo End")

            Return resultCode

            ' 2012/02/13 KN 梅村 【SALES_1A】ログ出力の不具合修正 START
        Catch ex As System.Net.WebException

            Logger.Error("ORDER_BEFORE_AFTER_DIV:BEFORE")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" _
                         + "System.Net.WebException(It is not connected to the webservice(IC3040403)):" + ex.StackTrace)
            Return C_MESSAGE_999
            ' 2012/02/13 KN 梅村 【SALES_1A】ログ出力の不具合修正 END
        Catch ex As Exception

            'ログ
            Logger.Error("ORDER_BEFORE_AFTER_DIV:BEFORE")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" + ex.StackTrace)
            Return C_MESSAGE_100

        End Try

    End Function

#End Region

#Region "スケジュール情報送信処理(受注後)"

    '$03 受注後フォロー機能開発 START

    ''' <summary>
    ''' スケジュール情報送信処理(受注後)
    ''' </summary>
    ''' <param name="batchStartDateTime">バッチ起動日時</param>
    ''' <param name="lastProcDate">前回バッチ起動日時</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
    '''  2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </History>
    <EnableCommit()>
    Public Function SendScheduleInfoAfterProcess(ByVal batchStartDateTime As DateTime, ByVal lastProcDate As DateTime) As Integer
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'Public Function SendScheduleInfoAfterProcess() As Integer
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'Dim batchStartDateTime As DateTime
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Dim resultCode As Integer = 0

        messageCode = C_MESSAGE_0

        Try
            Logger.Info(C_SYSTEM & " SendScheduleInfoAfterProcess Start")

            'DataSetTableAdapters
            da = New MC3040401ScheduleDataSetTableAdapters

            '更新用のスケジュール情報
            scheduleInfoDataTable = New MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable

            '現在日時(バッチ開始日時)の取得
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'messageCode = C_MESSAGE_901
            'batchStartDateTime = DateTimeFunc.Now()
            'Logger.Info(C_SYSTEM & " Get Now:" + batchStartDateTime)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            'スケジュール情報の取得
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'scheduleInfoDataTable = CreateScheduleInfoDataTableAfterProcess()
            scheduleInfoDataTable = CreateScheduleInfoDataTableAfterProcess(lastProcDate)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            '更新データが1件以上存在する場合のみ、WEBサービス呼び出し処理を実施
            '更新処理区分を元にXMLを作成
            If scheduleInfoDataTable.Rows.Count > 0 Then

                Dim totalUpdateCount As Decimal = 0
                Dim loopFlg As Boolean = True
                Dim rowIndex As Integer = 0
                ' 1回の送信で最大件数を取得
                Dim sendMaxCount As Integer = GetSendMaxCount()
                Logger.Info(C_SYSTEM & " sendMaxCount " & sendMaxCount)

                Do While loopFlg

                    '送信Xmlの作成
                    Dim xmlDoc As New System.Xml.XmlDocument
                    Dim xmlDecl As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)

                    xmlDoc.AppendChild(xmlDecl)

                    Dim rootElement As XmlElement = xmlDoc.CreateElement("RegistAfterOrderSchedule")
                    xmlDoc.AppendChild(rootElement)

                    'Headの作成
                    Dim headElement As XmlElement = xmlDoc.CreateElement("Head")
                    rootElement.AppendChild(headElement)

                    SetNode(xmlDoc, headElement, "MessageID", C_MESSAGE_ID)
                    SetNode(xmlDoc, headElement, "CountryCode", EnvironmentSetting.CountryCode)
                    SetNode(xmlDoc, headElement, "LinkSystemCode", C_LINK_SYSTEM_CODE)
                    SetNode(xmlDoc, headElement, "TransmissionDate", batchStartDateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                    'detailタグを作成
                    Dim UpdateCount As Integer
                    UpdateCount = CreateAfterProcessDetailTag(scheduleInfoDataTable, xmlDoc, rootElement, batchStartDateTime, sendMaxCount, rowIndex)

                    '更新件数
                    Logger.Info(C_SYSTEM + " Update Count:" + UpdateCount.ToString(CultureInfo.InvariantCulture))
                    totalUpdateCount = totalUpdateCount + UpdateCount

                    If UpdateCount > 0 Then

                        'WEBサービス送信処理
                        messageCode = C_MESSAGE_922
                        Dim dlrEnv As DealerEnvSetting = New DealerEnvSetting
                        Dim url As DlrEnvSettingDataSet.DLRENVSETTINGRow = dlrEnv.GetEnvSetting(C_COMMON_DEALERCODE, C_CALDAV_WEBSERVICE_URL)

                        Dim sendString As String
                        Dim returnString As String
                        Dim xmlResult As XmlDocument = New XmlDocument

                        'XML名前空間を除去
                        Logger.Info(xmlDoc.OuterXml)
                        Dim regex As Regex = New Regex(" xmlns=""[^""]*""")
                        sendString = regex.Replace(xmlDoc.OuterXml, Space(0))

                        sendString = "xsData=" + HttpUtility.UrlEncode(sendString)

                        'Web参照
                        returnString = CallWebServiceSite(sendString, url.PARAMVALUE + "/RegistAfterOrderSchedule")
                        xmlResult.LoadXml(returnString)

                        For Each resuleIdNode As XmlNode In xmlResult.SelectNodes("descendant::Common")
                            If Not "0".Equals(resuleIdNode.SelectSingleNode("ResultId").InnerXml) Then
                                'ログ
                                Logger.Info("ORDER_BEFORE_AFTER_DIV:AFTER")
                                Logger.Info("Web Service Error:" & resuleIdNode.SelectSingleNode("ResultId").InnerXml)
                            End If
                        Next
                    Else
                        loopFlg = False
                    End If
                Loop

                Logger.Info(C_SYSTEM + "Total Update Count:" + totalUpdateCount.ToString(CultureInfo.InvariantCulture))
            End If

            '未登録スケジュール情報テーブルから過去データ(昨日までのデータ)を削除
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'messageCode = C_MESSAGE_923
            'da.DeleteUnregistScheduleInfo(batchStartDateTime, ODR_BEFORE_AFTER_DIV_AFTER)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START

            Logger.Info(C_SYSTEM & " SendScheduleInfoAfterProcess End")

            Return resultCode

        Catch ex As System.Net.WebException

            Logger.Error("ORDER_BEFORE_AFTER_DIV:AFTER")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" _
                         + "System.Net.WebException(It is not connected to the webservice(IC3040403)):" + ex.StackTrace)
            Return C_MESSAGE_999

        Catch ex As Exception

            'ログ
            Logger.Error("ORDER_BEFORE_AFTER_DIV:AFTER")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" + ex.StackTrace)
            Return C_MESSAGE_100

        End Try

    End Function

    '$03 受注後フォロー機能開発 END

#End Region

'$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
#Region "前回バッチ起動日時更新"

    '''-------------------------------------------------------
    ''' <summary>
    ''' 前回バッチ起動日時更新
    ''' </summary>
    ''' <param name="batchStartDateTime">バッチ起動日時</param>
    ''' <param name="lastProcDate">前回バッチ起動日時</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
    ''' </History>
    '''------------------------------------------------------- 
    <EnableCommit()>
    Public Function UpdateLastProcDate(ByVal batchStartDateTime As DateTime, ByVal lastProcDate As DateTime) As Integer

        Dim resultCode As Integer = 0

        messageCode = C_MESSAGE_0

        Try
            Logger.Info(C_SYSTEM & " UpdateLastProcDate Start")
            '前回バッチ起動日時の更新
            messageCode = C_MESSAGE_924

            da.UpdateBatchDateTimeInfo(batchStartDateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()),
                           batchStartDateTime)
            Logger.Info(C_SYSTEM & " UpdateLastProcDate End")

            Return resultCode

        Catch ex As Exception

            'ログ
            Logger.Error("ORDER_BEFORE_AFTER_DIV:UPDATE_LAST_PROC_DATE")
            Logger.Error(messageCode.ToString(CultureInfo.InvariantCulture()) + ":" + ex.StackTrace)
            Return C_MESSAGE_100

        End Try

    End Function

#End Region
'$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

#Region "DBから各スケジュール情報を取得"

    ''' <summary>
    ''' DBから各スケジュール情報を取得
    ''' </summary>
    ''' <param name="lastProcDate">前回バッチ起動日時</param>
    ''' <returns>スケジュール情報</returns>
    ''' <remarks></remarks>
    ''' <History>
    '''  2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
    ''' </History>
    Private Function CreateScheduleInfoDataTable(ByVal lastProcDate As DateTime) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
        '$03 受注後フォロー機能開発 START

        Dim odrBeforeAfterDiv As String = ODR_BEFORE_AFTER_DIV_BEFORE

        '新規登録パターン

        '再登録用の未登録スケジュール情報の取得 (更新処理区分:1)
        messageCode = C_MESSAGE_903
        scheduleInfoDataTable.Merge(da.SelectUnregistScheduleInfo(odrBeforeAfterDiv))
        Logger.Info("1." + C_SYSTEM + "SelectUnregistScheduleInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        '割当済みに変更された活動担当スタッフ情報の取得 (更新処理区分:2)
        messageCode = C_MESSAGE_904
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectAllocatedSalesStaffInfo())
        scheduleInfoDataTable.Merge(da.SelectAllocatedSalesStaffInfo(lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("2." + C_SYSTEM & "SelectAllocatedSalesStaffInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        '割当済みに変更された受付担当スタッフ情報の取得 (更新処理区分:3)
        messageCode = C_MESSAGE_905
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectAllocatedSavicesStaffInfo())
        scheduleInfoDataTable.Merge(da.SelectAllocatedSavicesStaffInfo(lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("3." + C_SYSTEM & "SelectAllocatedSavicesStaffInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        'スケジュール情報に反映が必要なサービス関連情報の取得 (更新処理区分:4)
        messageCode = C_MESSAGE_906
        scheduleInfoDataTable.Merge(da.SelectUpdateSarviceInfo(lastProcDate))
        Logger.Info("4." + C_SYSTEM & "SelectUpdateSarviceInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))


        '更新パターン

        '作成済みスケジュール情報の活動担当スタッフ変更情報の取得 (更新処理区分:5)
        messageCode = C_MESSAGE_907
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectUpdateSalesStaffInfo())
        scheduleInfoDataTable.Merge(da.SelectUpdateSalesStaffInfo(lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("5." + C_SYSTEM & "SelectUpdateSalesStaffInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        '作成済みスケジュール情報のサービススタッフ変更情報の取得 (更新処理区分:6)
        messageCode = C_MESSAGE_908
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectUpdateSavicesStaffInfo())
        scheduleInfoDataTable.Merge(da.SelectUpdateSavicesStaffInfo(lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("6." + C_SYSTEM & "SelectUpdateSavicesStaffInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))


        '顧客名変更情報の取得 (更新処理区分:7)
        messageCode = C_MESSAGE_931
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectUpdateCustInfo(odrBeforeAfterDiv))
        scheduleInfoDataTable.Merge(da.SelectUpdateCustInfo(odrBeforeAfterDiv, lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("7." + C_SYSTEM & "SelectUpdateCustInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))


        '削除パターン

        '削除された顧客情報の取得 (更新処理区分:10)
        messageCode = C_MESSAGE_932
        scheduleInfoDataTable.Merge(da.SelectDeleteCustInfo(odrBeforeAfterDiv))
        Logger.Info("8." + C_SYSTEM & "SelectDeleteCustInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        Return scheduleInfoDataTable

        '$03 受注後フォロー機能開発 END
    End Function

#End Region

#Region "DBから各スケジュール情報を取得(受注後)"

    ''' <summary>
    ''' DBから各スケジュール情報を取得(受注後)
    ''' </summary>
    ''' <param name="lastProcDate">前回バッチ起動日時</param>
    ''' <returns>スケジュール情報</returns>
    ''' <remarks></remarks>
    Private Function CreateScheduleInfoDataTableAfterProcess(ByVal lastProcDate As DateTime) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'Private Function CreateScheduleInfoDataTableAfterProcess() As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Dim odrBeforeAfterDiv As String = ODR_BEFORE_AFTER_DIV_AFTER

        '新規登録パターン

        '再登録用の未登録スケジュール情報の取得 (更新処理区分:1)
        messageCode = C_MESSAGE_903
        scheduleInfoDataTable.Merge(da.SelectUnregistScheduleInfo(odrBeforeAfterDiv))
        Logger.Info("1." + C_SYSTEM + "SelectUnregistScheduleInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        '割当済みに変更された活動担当スタッフ情報の取得 (更新処理区分:2)
        messageCode = C_MESSAGE_904
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectAllocatedSalesStaffInfoAfterProcess())
        scheduleInfoDataTable.Merge(da.SelectAllocatedSalesStaffInfoAfterProcess(lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("2." + C_SYSTEM & "SelectAllocatedSalesStaffInfoAfterProcess Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))


        '更新パターン

        '作成済みスケジュール情報の活動担当スタッフ変更情報の取得 (更新処理区分:5)
        messageCode = C_MESSAGE_907
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectUpdateSalesStaffInfoAfterProcess())
        scheduleInfoDataTable.Merge(da.SelectUpdateSalesStaffInfoAfterProcess(lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("5." + C_SYSTEM & "SelectUpdateSalesStaffInfoAfterProcess Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        '顧客名変更情報の取得 (更新処理区分:7)
        messageCode = C_MESSAGE_931
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
        'scheduleInfoDataTable.Merge(da.SelectUpdateCustInfo(odrBeforeAfterDiv))
        scheduleInfoDataTable.Merge(da.SelectUpdateCustInfo(odrBeforeAfterDiv, lastProcDate))
        '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
        Logger.Info("7." + C_SYSTEM & "SelectUpdateCustInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))


        '削除パターン

        '削除された顧客情報の取得 (更新処理区分:10)
        messageCode = C_MESSAGE_932
        scheduleInfoDataTable.Merge(da.SelectDeleteCustInfo(odrBeforeAfterDiv))
        Logger.Info("8." + C_SYSTEM & "SelectDeleteCustInfo Merge:" + scheduleInfoDataTable.Rows.Count.ToString(CultureInfo.InvariantCulture))

        Return scheduleInfoDataTable

        '$03 受注後フォロー機能開発 END
    End Function

#End Region

#Region "WebServiceのサイト呼び出し"
    ''' <summary>
    ''' WebServiceのサイトを呼び出す
    ''' </summary>
    ''' <param name="postData">送信文字列</param>
    ''' <param name="WebServiceUrl">送信先アドレス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CallWebServiceSite(ByVal postData As String, ByVal WebServiceUrl As String) As String
        '文字コードを指定する
        Dim enc As System.Text.Encoding = _
            System.Text.Encoding.GetEncoding("UTF-8")

        'バイト型配列に変換
        Dim postDataBytes As Byte() = _
            System.Text.Encoding.ASCII.GetBytes(postData)

        'WebRequestの作成
        '$03 受注後フォロー機能開発 START
        Dim req As System.Net.WebRequest = _
            System.Net.WebRequest.Create(WebServiceUrl)
        '$03 受注後フォロー機能開発 END

        'メソッドにPOSTを指定
        req.Method = "POST"
        'ContentTypeを"application/x-www-form-urlencoded"にする
        req.ContentType = "application/x-www-form-urlencoded"
        'POST送信するデータの長さを指定
        req.ContentLength = postDataBytes.Length

        '$03 受注後フォロー機能開発 START
        'タイムアウト時間の設定
        Dim timeoutPeriod As Integer = GetTimeoutPeriod()
        If Not timeoutPeriod.Equals(0) Then
            req.Timeout = timeoutPeriod
        End If
        '$03 受注後フォロー機能開発 END

        'データをPOST送信するためのStreamを取得
        Dim reqStream As System.IO.Stream = req.GetRequestStream()
        '送信するデータを書き込む
        reqStream.Write(postDataBytes, 0, postDataBytes.Length)
        reqStream.Close()

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim res As System.Net.WebResponse = req.GetResponse()
        '応答データを受信するためのStreamを取得
        Dim resStream As System.IO.Stream = res.GetResponseStream()
        '受信して表示
        Dim sr As New System.IO.StreamReader(resStream, enc)

        '返却文字列を取得
        Dim returnString As String = sr.ReadToEnd()

        '閉じる
        sr.Close()

        Return returnString
    End Function

#End Region

#Region "タイトル取得"
    ''' <summary>
    ''' タイトル取得
    ''' </summary>
    ''' <param name="scheduleInfoRow">スケジュール情報</param>
    ''' <param name="isExistChild">子存在フラグ</param>
    ''' <returns>タイトル</returns>
    ''' <remarks></remarks>
    Private Function GetSummary(ByVal scheduleInfoRow As MC3040401DataSet.MC3040401UpdateScheduleInfoRow, _
                                ByVal isExistChild As Boolean) As String

        Dim summary As String = ""

        '顧客敬称を取得
        messageCode = C_MESSAGE_915
        Dim nameTitle As String = ""
        nameTitleDataTable = New MC3040401DataSet.MC3040401NameTitleInfoDataTable

        '$03 受注後フォロー機能開発 START
        nameTitleDataTable = da.GetNameTitleCustomer(scheduleInfoRow.CUSTID)
        '$03 受注後フォロー機能開発 END

        If nameTitleDataTable.Count > 0 Then
            nameTitle = nameTitleDataTable.Item(0).NAMETITLE
        Else
            nameTitle = ""
        End If


        '敬称位置を取得
        messageCode = C_MESSAGE_916
        Dim sysEnv As SystemEnvSetting = New SystemEnvSetting
        Dim nameTitlePosition As String = ""
        Dim nameTitlePositionRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysEnv.GetSystemEnvSetting(C_KEISYO_POSISION)
        nameTitlePosition = nameTitlePositionRow.PARAMVALUE

        If "0".Equals(scheduleInfoRow.SCHEDULEDIV) Then

            '来店予約のタイトル設定
            summary = GetActivitySummary(scheduleInfoRow,
                                            nameTitle,
                                            nameTitlePosition,
                                            isExistChild)
            ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 START
        ElseIf "1".Equals(scheduleInfoRow.SCHEDULEDIV) Then
            ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 END
            '入庫予約のタイトル設定
            summary = GetReservationSummary(scheduleInfoRow,
                                            nameTitle,
                                            nameTitlePosition)
            ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 START
        ElseIf "2".Equals(scheduleInfoRow.SCHEDULEDIV) Then
            '受注後工程のタイトル設定
            summary = GetAfterProcessSummary(scheduleInfoRow,
                                             nameTitle,
                                             nameTitlePosition)
            ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 END
        End If

        Return summary


    End Function

#End Region

#Region "活動名のタイトル取得"


    ''' <summary>
    ''' '活動名のタイトル取得
    ''' </summary>
    ''' <param name="scheduleInfoRow">スケジュール情報</param>
    ''' <param name="nameTitle">敬称</param>
    ''' <param name="nameTitlePosition">敬称位置</param>
    ''' <param name="isExistChild">子存在フラグ</param>
    ''' <returns>活動名のタイトル</returns>
    ''' <remarks></remarks>
    Private Function GetActivitySummary(ByVal scheduleInfoRow As MC3040401DataSet.MC3040401UpdateScheduleInfoRow,
                                        ByVal nameTitle As String,
                                        ByVal nameTitlePosition As String,
                                        ByVal isExistChild As Boolean) As String

        Dim summary As String = ""

        '来店予約、来店フォローの判定
        If isExistChild Then

            '来店フォロー名を取得
            Dim followName As String = da.GetWord(C_SCHEDULE, C_SCHEDULE_KEY1)

            '$03 受注後フォロー機能開発 START

            'タイトル文字列取得
            If "1".Equals(nameTitlePosition) Then
                '敬称を顧客名の前につけて、タイトルを作成
                If Not String.IsNullOrEmpty(nameTitle) _
                    AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                    summary = summary + nameTitle
                End If
                summary = summary + scheduleInfoRow.CUSTNAME

            Else
                '敬称を顧客名の後につけて、タイトルを作成
                summary = summary + scheduleInfoRow.CUSTNAME
                If Not String.IsNullOrEmpty(nameTitle) _
                    AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                    summary = summary + nameTitle
                End If

            End If

            '来店フォロー名を連結
            If Not String.IsNullOrEmpty(followName) _
                AndAlso Not BLANK_STRING.Equals(followName) Then
                summary = summary + " " + followName
            End If

        Else

            '接触方法名称を取得
            messageCode = C_MESSAGE_917
            Dim contactName As String = ""
            contactNameDataTable = New MC3040401DataSet.MC3040401ContactNameInfoDataTable
            Try
                contactNameDataTable = da.GetContactName(scheduleInfoRow.CONTACTNO)
                If contactNameDataTable.Count > 0 Then
                    contactName = contactNameDataTable.Item(0).CONTACT
                End If
            Catch ex As OracleExceptionEx
                'ログ出力
                Logger.Error("ORDER_BEFORE_AFTER_DIV:BEFORE")
                Logger.Error(ex.Message)
            Finally
                contactNameDataTable.Dispose()
            End Try

            'タイトル文字列取得
            If "1".Equals(nameTitlePosition) Then
                '敬称を顧客名の前につけて、タイトルを作成
                If Not String.IsNullOrEmpty(nameTitle) _
                    AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                    summary = summary + nameTitle
                End If
                summary = summary + scheduleInfoRow.CUSTNAME

            Else
                '敬称を顧客名の後につけて、タイトルを作成
                summary = summary + scheduleInfoRow.CUSTNAME
                If Not String.IsNullOrEmpty(nameTitle) _
                    AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                    summary = summary + nameTitle
                End If

            End If

            '接触方法名を連結
            If Not String.IsNullOrEmpty(contactName) _
                AndAlso Not BLANK_STRING.Equals(contactName) Then
                summary = summary + " " + contactName
            End If
        End If
        '$03 受注後フォロー機能開発 END

        Return summary


    End Function

#End Region

#Region "入庫予約名のタイトル取得"

    ''' <summary>
    ''' 入庫予約名のタイトル取得
    ''' </summary>
    ''' <param name="scheduleInfoRow">スケジュール情報</param>
    ''' <param name="nameTitle">敬称</param>
    ''' <param name="nameTitlePosition">敬称位置</param>
    ''' <returns>入庫予約名のタイトル</returns>
    ''' <remarks></remarks>
    Private Function GetReservationSummary(ByVal scheduleInfoRow As MC3040401DataSet.MC3040401UpdateScheduleInfoRow,
                                           ByVal nameTitle As String,
                                           ByVal nameTitlePosition As String) As String

        Dim summary As String = ""

        '言語区分を取得
        messageCode = C_MESSAGE_918
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
        Dim languageDivision As String = GetLanguageDivision(scheduleInfoRow.DLRCD)
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

        'サービス名を取得
        messageCode = C_MESSAGE_919
        Dim serviceName As String = ""
        serviceNameDataTable = New MC3040401DataSet.MC3040401ServiceNameInfoDataTable

        serviceNameDataTable = da.GetServiceName(scheduleInfoRow.DLRCD,
                                                scheduleInfoRow.STRCD,
                                                scheduleInfoRow.SERVICECODE)
        If serviceNameDataTable.Count > 0 Then
            If "1".Equals(languageDivision) Then
                If Not serviceNameDataTable.Item(0).IsSVCORGNAMENull Then
                    serviceName = serviceNameDataTable.Item(0).SVCORGNAME
                End If
            Else
                If Not serviceNameDataTable.Item(0).IsSVCENGNAMENull Then
                    serviceName = serviceNameDataTable.Item(0).SVCENGNAME
                End If
            End If
        End If

        '商品名を取得
        messageCode = C_MESSAGE_920
        Dim merchandiseName As String = ""
        ' 入力値がある場合
        If Not String.IsNullOrEmpty(Trim(scheduleInfoRow.MERCHANDISECD)) Then
            Dim merchandiseCode As Decimal
            ' 入力値が数値である場合
            If Decimal.TryParse(scheduleInfoRow.MERCHANDISECD, merchandiseCode) Then
                merchandiseNameDataTable = New MC3040401DataSet.MC3040401MerchandiseNameInfoDataTable
                merchandiseNameDataTable = da.GetMerchandiseName(merchandiseCode)
                If merchandiseNameDataTable.Count > 0 Then
                    If "1".Equals(languageDivision) Then
                        If Not merchandiseNameDataTable.Item(0).IsMERCHANDISENAME_EXNull Then
                            merchandiseName = merchandiseNameDataTable.Item(0).MERCHANDISENAME_EX
                        End If
                    Else
                        If Not merchandiseNameDataTable.Item(0).IsMERCHANDISENAME_ENGNull Then
                            merchandiseName = merchandiseNameDataTable.Item(0).MERCHANDISENAME_ENG
                        End If
                    End If
                End If
            End If
        End If

        If "2".Equals(scheduleInfoRow.REZSTATUS) Then
            '仮予約の表示名を取得
            Dim reservationName As String = da.GetWord(C_SCHEDULE, C_SCHEDULE_KEY2)
            summary = summary + reservationName + " "
        End If

        '$03 受注後フォロー機能開発 START
        If "1".Equals(nameTitlePosition) Then
            '敬称を顧客名の前につけて、タイトルを作成
            If Not String.IsNullOrEmpty(nameTitle) _
                AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                summary = summary + nameTitle
            End If
            summary = summary + scheduleInfoRow.CUSTNAME

        Else
            '敬称を顧客名の後につけて、タイトルを作成
            summary = summary + scheduleInfoRow.CUSTNAME
            If Not String.IsNullOrEmpty(nameTitle) _
                AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                summary = summary + nameTitle
            End If

        End If

        'サービス名を連結
        If Not String.IsNullOrEmpty(serviceName) _
            AndAlso Not BLANK_STRING.Equals(serviceName) Then
            summary = summary + " " + serviceName
        End If

        '商品名を連結
        If Not String.IsNullOrEmpty(merchandiseName) _
            AndAlso Not BLANK_STRING.Equals(merchandiseName) Then
            summary = summary + " " + merchandiseName
        End If
        '$03 受注後フォロー機能開発 END

        Return summary


    End Function

#End Region

#Region "言語区分の取得"
    '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' 言語区分の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <returns>1:現地語/0:英語</returns>
    ''' <remarks></remarks>
    Private Function GetLanguageDivision(ByRef dealerCode) As String

        Dim returnValue As String = LANG_DIV_LOCAL
        Dim useEnglishDlrCd As String = String.Empty
        Try
            ' 設定ファイルより英語表記にする販売店コードを取得する。
            useEnglishDlrCd = BatchSetting.GetValue(SETTING_SECTION, KEY_USEENGLISHDLRCD, DEFAULT_STRING_VALUE)
        Catch ex As ArgumentException
            Return returnValue
        End Try

        If (String.IsNullOrEmpty(useEnglishDlrCd)) Then
            Return returnValue
        End If
        Dim useEnglishDlrCdList As String() = useEnglishDlrCd.Split(","c)
        For Each stData As String In useEnglishDlrCdList
            If (dealerCode.Equals(Trim(stData))) Then
                returnValue = LANG_DIV_ENG
            End If
        Next stData
        Return returnValue
    End Function
    '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

#End Region

#Region "受注後工程のタイトル取得"

    ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 START
    ''' <summary>
    ''' 受注後工程のタイトル取得
    ''' </summary>
    ''' <param name="scheduleInfoRow">スケジュール情報</param>
    ''' <param name="nameTitle">敬称</param>
    ''' <param name="nameTitlePosition">敬称位置</param>
    ''' <returns>受注後工程のタイトル</returns>
    ''' <remarks></remarks>
    Private Function GetAfterProcessSummary(ByVal scheduleInfoRow As MC3040401DataSet.MC3040401UpdateScheduleInfoRow,
                                            ByVal nameTitle As String,
                                            ByVal nameTitlePosition As String) As String

        Dim summary As String = ""

        '$03 受注後フォロー機能開発 START
        If "1".Equals(nameTitlePosition) Then
            '敬称を顧客名の前につけて、タイトルを作成
            If Not String.IsNullOrEmpty(nameTitle) _
                AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                summary = summary + nameTitle
            End If
            summary = summary + scheduleInfoRow.CUSTNAME

        Else
            '敬称を顧客名の後につけて、タイトルを作成
            summary = summary + scheduleInfoRow.CUSTNAME
            If Not String.IsNullOrEmpty(nameTitle) _
                AndAlso Not BLANK_STRING.Equals(nameTitle) Then
                summary = summary + nameTitle
            End If

        End If

        If Not String.IsNullOrEmpty(scheduleInfoRow.CONTACT_NAME) _
            AndAlso Not BLANK_STRING.Equals(scheduleInfoRow.CONTACT_NAME) Then
            summary = summary + " " + scheduleInfoRow.CONTACT_NAME
        End If
        If Not String.IsNullOrEmpty(scheduleInfoRow.ACT_ODR_NAME) _
            AndAlso Not BLANK_STRING.Equals(scheduleInfoRow.ACT_ODR_NAME) Then
            summary = summary + "(" + scheduleInfoRow.ACT_ODR_NAME + ")"
        End If
        '$03 受注後フォロー機能開発 END

        Return summary


    End Function
    ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 END

#End Region

#Region "XMLタグ作成"
    ''' <summary>
    ''' XMLタグを作成する
    ''' </summary>
    ''' <param name="xmlDoc">XMLオブジェクト</param>
    ''' <param name="rootNode">ルートノード</param>
    ''' <param name="elementName">タグ名</param>
    ''' <param name="elementValue">セット値</param>
    Private Sub SetNode(ByVal xmlDoc As System.Xml.XmlDocument, ByVal rootNode As XmlElement, ByVal elementName As String, ByVal elementValue As String)

        Dim crtElement As XmlElement
        crtElement = xmlDoc.CreateElement(elementName)

        crtElement.InnerText = elementValue
        rootNode.AppendChild(crtElement)

    End Sub

    ''' <summary>
    ''' XMLタグを作成する(空タグは作成しない)
    ''' </summary>
    ''' <param name="xmlDoc">XMLオブジェクト</param>
    ''' <param name="rootNode">ルートノード</param>
    ''' <param name="elementName">タグ名</param>
    ''' <param name="elementValue">セット値</param>
    Private Sub SetNodeEmptyTagNotCreate(ByVal xmlDoc As System.Xml.XmlDocument, ByVal rootNode As XmlElement, ByVal elementName As String, ByVal elementValue As String)

        If Not String.IsNullOrEmpty(elementValue) Then
            Dim crtElement As XmlElement
            crtElement = xmlDoc.CreateElement(elementName)

            crtElement.InnerText = elementValue
            rootNode.AppendChild(crtElement)
        End If
    End Sub

#End Region

#Region "IDisposable Support"

    ''' <summary>
    ''' Disposeメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        'Dispose(True)
        ' This object will be cleaned up by the Dispose method.
        ' Therefore, you should call GC.SupressFinalize to
        ' take this object off the finalization queue 
        ' and prevent finalization code for this object
        ' from executing a second time.
        Dispose(True)
        GC.SuppressFinalize(Me)

    End Sub

    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

        If disposing Then

            da.Dispose()
            batchProcInfoDataTable.Dispose()
            scheduleInfoDataTable.Dispose()
            contactNameDataTable.Dispose()
            serviceNameDataTable.Dispose()
            merchandiseNameDataTable.Dispose()
            nameTitleDataTable.Dispose()

            da = Nothing
            batchProcInfoDataTable = Nothing
            scheduleInfoDataTable = Nothing
            contactNameDataTable = Nothing
            serviceNameDataTable = Nothing
            merchandiseNameDataTable = Nothing
            nameTitleDataTable = Nothing

        End If

    End Sub

#End Region

#Region "Detailタグ作成"

    ''' <summary>
    ''' Detailタグを作成
    ''' </summary>
    ''' <param name="scheduleInfoTable">スケジュール情報</param>
    ''' <param name="xmlDoc">作成XMLドキュメント</param>
    ''' <param name="rootElement"></param>
    ''' <param name="batchStartDateTime"></param>
    ''' <remarks></remarks>
    Private Function CreateDetailTag(ByVal scheduleInfoTable As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable, _
                                     ByRef xmlDoc As XmlDocument, _
                                     ByRef rootElement As XmlElement, _
                                     ByVal batchStartDateTime As DateTime, _
                                     ByVal sendMaxCount As Integer, _
                                     ByRef rowIndex As Integer) As Integer

        '$03 受注後フォロー機能開発 START
        Dim UpdateCount As Integer = 0
        '$03 受注後フォロー機能開発 END

        ' 2012/04/24 KN 梅村 【SALES_2】受注後工程フォロー対応 START
        Dim parentInfoRow As MC3040401UpdateScheduleInfoRow = Nothing
        ' 2012/04/24 KN 梅村 【SALES_2】受注後工程フォロー対応 END

        For i As Integer = rowIndex To scheduleInfoTable.Rows.Count - 1

            Dim scheduleInfoRow As MC3040401UpdateScheduleInfoRow = scheduleInfoTable.Rows(i)

            'Detailの作成
            Dim detailElement As XmlElement = xmlDoc.CreateElement("Detail")
            rootElement.AppendChild(detailElement)

            'Commonの作成
            Dim commonElement As XmlElement = xmlDoc.CreateElement("Common")

            SetNode(xmlDoc, commonElement, "DealerCode", scheduleInfoRow.DLRCD)
            SetNode(xmlDoc, commonElement, "BranchCode", scheduleInfoRow.STRCD)
            SetNode(xmlDoc, commonElement, "ScheduleDiv", scheduleInfoRow.SCHEDULEDIV)
            SetNode(xmlDoc, commonElement, "ScheduleID", scheduleInfoRow.SCHEDULEID)
            SetNode(xmlDoc, commonElement, "ActionType", scheduleInfoRow.ACTIONTYPE)
            If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Then
                SetNode(xmlDoc, commonElement, "ActivityCreateStaff", scheduleInfoRow.ACTCREATESTAFFCD)
            Else
                SetNode(xmlDoc, commonElement, "ActivityCreateStaff", C_SYSTEM)
            End If

            detailElement.AppendChild(commonElement)

            'ScheduleInfoの作成
            If Not "5".Equals(scheduleInfoRow.UPDATEPROCDIV) And _
               Not "6".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                Dim scheduleInfoElement As XmlElement = xmlDoc.CreateElement("ScheduleInfo")

                If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
                   "2".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
                   "3".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
                   "4".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                    '新規追加パターン
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerDiv", scheduleInfoRow.CUSTDIV)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerCode", scheduleInfoRow.CUSTID)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "DmsID", scheduleInfoRow.DMSID)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerName", scheduleInfoRow.CUSTNAME)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "ReceptionDiv", scheduleInfoRow.RECEPTIONDIV)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "ServiceCode", scheduleInfoRow.SERVICECODE)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "MerchandiseCd", scheduleInfoRow.MERCHANDISECD)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "StrStatus", scheduleInfoRow.STRSTATUS)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "RezStatus", scheduleInfoRow.REZSTATUS)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CompletionDiv", scheduleInfoRow.COMPLETEFLG)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CompletionDate", scheduleInfoRow.COMPLETEDATE)

                    '$03 受注後フォロー機能開発 START
                ElseIf "7".Equals(scheduleInfoRow.UPDATEPROCDIV) Then
                    '$03 受注後フォロー機能開発 END

                    '顧客名が変更されているパターン
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerName", scheduleInfoRow.CUSTNAME)

                    '$03 受注後フォロー機能開発 START
                ElseIf "8".Equals(scheduleInfoRow.UPDATEPROCDIV) Then
                    '$03 受注後フォロー機能開発 END

                    '顧客情報が削除されているパターン
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "DeleteDate", batchStartDateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                End If

                detailElement.AppendChild(scheduleInfoElement)
            End If

            Dim isExistChild As Boolean = False

            '$03 受注後フォロー機能開発 START
            '子レコードが存在するか判定
            If scheduleInfoTable.Rows.Count > i + 1 Then

                parentInfoRow = scheduleInfoTable.Rows(i + 1)

                If scheduleInfoRow.DLRCD.Equals(parentInfoRow.DLRCD) AndAlso _
                    scheduleInfoRow.STRCD.Equals(parentInfoRow.STRCD) AndAlso _
                    scheduleInfoRow.SCHEDULEDIV.Equals(parentInfoRow.SCHEDULEDIV) AndAlso _
                    scheduleInfoRow.SCHEDULEID.Equals(parentInfoRow.SCHEDULEID) AndAlso _
                    "2".Equals(parentInfoRow.PARENTDIV) Then

                    isExistChild = True
                End If
            End If

            'scheduleタグの作成
            CreateScheduleTag(scheduleInfoRow, xmlDoc, detailElement, isExistChild)

            ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 START
            UpdateCount = UpdateCount + 1

            '来店予約、入庫予約で親子レコードの場合、次のscheduleタグを作成する(Detailタグを作らない)。
            If isExistChild Then
                CreateScheduleTag(parentInfoRow, xmlDoc, detailElement, False)
                i = i + 1
                UpdateCount = UpdateCount + 1
            End If

            '$03 受注後フォロー機能開発 END

            ' 最大送信件数が指定されており、送信数が最大送信件数以上の場合ループ処理を抜ける
            rowIndex = i
            If sendMaxCount <> 0 AndAlso UpdateCount = sendMaxCount Then
                Exit For
            End If
        Next

        Logger.Info(C_SYSTEM & " CreateDetailTag End " & rowIndex)
        rowIndex = rowIndex + 1
        Return UpdateCount

    End Function

#End Region

#Region "スケジュールタグ作成"

    ''' <summary>
    ''' スケジュールタグ作成
    ''' </summary>
    ''' <param name="scheduleInfoRow">作成スケジュール情報</param>
    ''' <param name="xmlDoc">追加対象XML</param>
    ''' <param name="detailElement">Detailタグ</param>
    ''' <param name="isExistChild">子存在フラグ</param>
    ''' <remarks></remarks>
    Private Sub CreateScheduleTag(ByVal scheduleInfoRow As MC3040401DataSet.MC3040401UpdateScheduleInfoRow, _
                                  ByRef xmlDoc As XmlDocument, _
                                  ByRef detailElement As XmlElement,
                                  ByVal isExistChild As Boolean)
        'Scheduleの作成
        '$03 受注後フォロー機能開発 START
        If Not "8".Equals(scheduleInfoRow.UPDATEPROCDIV) Then
            '$03 受注後フォロー機能開発 END

            Dim scheduleElement As XmlElement = xmlDoc.CreateElement("Schedule")


            'スケジュール作成区分
            ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 START
            If "3".Equals(scheduleInfoRow.ACTIONTYPE) Then
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "CreateScheduleDiv", "3")
            Else
                If Not String.IsNullOrEmpty(scheduleInfoRow.STARTTIME) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "CreateScheduleDiv", "1")
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "CreateScheduleDiv", "2")
                End If
            End If
            ' 2012/02/29 KN 梅村 【SALES_2】受注後工程フォロー対応 END

            If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "2".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "3".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "4".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '新規追加パターン
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ParentDiv", scheduleInfoRow.PARENTDIV)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffBranchCode", scheduleInfoRow.ACTSTAFFSTRCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffCode", scheduleInfoRow.ACTSTAFFCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ReceptionStaffBranchCode", scheduleInfoRow.RECSTAFFSTRCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ReceptionStaffCode", scheduleInfoRow.RECSTAFFCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ContactNo", scheduleInfoRow.CONTACTNO)

                If "4".Equals(scheduleInfoRow.UPDATEPROCDIV) Then
                    Dim summaryAdd As String = GetSummary(scheduleInfoRow, isExistChild)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "Summary", summaryAdd)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "Summary", scheduleInfoRow.SUMMARY)
                End If

                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "StartTime", scheduleInfoRow.STARTTIME)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "EndTime", scheduleInfoRow.ENDTIME)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "Memo", scheduleInfoRow.MEMO)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "XiCropColor", scheduleInfoRow.BACKGROUNDCOLOR)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "TodoID", scheduleInfoRow.TODOID)

                '$03 受注後フォロー機能開発 START
                If BLANK_STRING.Equals(scheduleInfoRow.CONTACT_NAME) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ContactName", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ContactName", scheduleInfoRow.CONTACT_NAME)
                End If

                If BLANK_STRING.Equals(scheduleInfoRow.ODR_DIV) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", scheduleInfoRow.ODR_DIV)
                End If
                '$03 受注後フォロー機能開発 END

            ElseIf "5".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '活動担当スタッフが変更されているパターン
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                'SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffBranchCode", scheduleInfoRow.ACTSTAFFSTRCD)
                'SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffCode", scheduleInfoRow.ACTSTAFFCD)
                SetNode(xmlDoc, scheduleElement, "ActivityStaffBranchCode", scheduleInfoRow.ACTSTAFFSTRCD)
                SetNode(xmlDoc, scheduleElement, "ActivityStaffCode", scheduleInfoRow.ACTSTAFFCD)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "TodoID", scheduleInfoRow.TODOID)
                '$03 受注後フォロー機能開発 START
                If BLANK_STRING.Equals(scheduleInfoRow.ODR_DIV) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", scheduleInfoRow.ODR_DIV)
                End If
                '$03 受注後フォロー機能開発 END

            ElseIf "6".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '受付担当スタッフが変更されているパターン
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                'SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ReceptionStaffBranchCode", scheduleInfoRow.RECSTAFFSTRCD)
                'SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ReceptionStaffCode", scheduleInfoRow.RECSTAFFCD)
                SetNode(xmlDoc, scheduleElement, "ReceptionStaffBranchCode", scheduleInfoRow.RECSTAFFSTRCD)
                SetNode(xmlDoc, scheduleElement, "ReceptionStaffCode", scheduleInfoRow.RECSTAFFCD)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "TodoID", scheduleInfoRow.TODOID)
                '$03 受注後フォロー機能開発 START
                If BLANK_STRING.Equals(scheduleInfoRow.ODR_DIV) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", scheduleInfoRow.ODR_DIV)
                End If
                '$03 受注後フォロー機能開発 END

                '$03 受注後フォロー機能開発 START
            ElseIf "7".Equals(scheduleInfoRow.UPDATEPROCDIV) Then
                '$03 受注後フォロー機能開発 END

                '顧客名が変更されているパターン
                Dim summary As String = GetSummary(scheduleInfoRow, isExistChild)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "Summary", summary)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "TodoID", scheduleInfoRow.TODOID)
                '$03 受注後フォロー機能開発 START
                If BLANK_STRING.Equals(scheduleInfoRow.ODR_DIV) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", scheduleInfoRow.ODR_DIV)
                End If
                '$03 受注後フォロー機能開発 END
            End If


            detailElement.AppendChild(scheduleElement)

            'Alarmの作成
            If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "2".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "3".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "4".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '新規追加パターン
                If Not String.IsNullOrEmpty(scheduleInfoRow.ALARMNO) Then
                    Dim alarmElement As XmlElement = xmlDoc.CreateElement("Alarm")
                    SetNodeEmptyTagNotCreate(xmlDoc, alarmElement, "Trigger", scheduleInfoRow.ALARMNO)
                    scheduleElement.AppendChild(alarmElement)
                End If
            End If
        End If
    End Sub


#End Region

#Region "Detailタグを作成(受注後工程)"

    '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 START
    ''' <summary>
    ''' Detailタグを作成(受注後工程)
    ''' </summary>
    ''' <param name="scheduleInfoTable">スケジュール情報</param>
    ''' <param name="xmlDoc">作成XMLドキュメント</param>
    ''' <param name="rootElement"></param>
    ''' <remarks></remarks>
    Private Function CreateAfterProcessDetailTag(ByVal scheduleInfoTable As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable, _
                                                 ByRef xmlDoc As XmlDocument, _
                                                 ByRef rootElement As XmlElement, _
                                                 ByVal batchStartDateTime As DateTime, _
                                                 ByVal sendMaxCount As Integer, _
                                                 ByRef rowIndex As Integer) As Integer

        Dim UpdateCount As Integer = 0

        '$03 受注後フォロー機能開発 START

        For i As Integer = rowIndex To scheduleInfoTable.Rows.Count - 1

            Dim scheduleInfoRow As MC3040401UpdateScheduleInfoRow = scheduleInfoTable.Rows(i)

            'Detailの作成
            Dim detailElement As XmlElement = xmlDoc.CreateElement("Detail")
            rootElement.AppendChild(detailElement)

            'Commonの作成
            Dim commonElement As XmlElement = xmlDoc.CreateElement("Common")

            SetNode(xmlDoc, commonElement, "DealerCode", scheduleInfoRow.DLRCD)
            SetNode(xmlDoc, commonElement, "BranchCode", scheduleInfoRow.STRCD)
            SetNode(xmlDoc, commonElement, "ScheduleID", scheduleInfoRow.SCHEDULEID)
            SetNode(xmlDoc, commonElement, "ActionType", scheduleInfoRow.ACTIONTYPE)
            If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Then
                SetNode(xmlDoc, commonElement, "ActivityCreateStaff", scheduleInfoRow.ACTCREATESTAFFCD)
            Else
                SetNode(xmlDoc, commonElement, "ActivityCreateStaff", C_SYSTEM)
            End If

            detailElement.AppendChild(commonElement)

            'ScheduleInfoの作成
            If Not "5".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                Dim scheduleInfoElement As XmlElement = xmlDoc.CreateElement("ScheduleInfo")

                If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
                   "2".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                    '新規追加パターン
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerDiv", scheduleInfoRow.CUSTDIV)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerCode", scheduleInfoRow.CUSTID)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "DmsID", scheduleInfoRow.DMSID)
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerName", scheduleInfoRow.CUSTNAME)

                ElseIf "7".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                    '顧客名が変更されているパターン
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "CustomerName", scheduleInfoRow.CUSTNAME)

                ElseIf "8".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                    '顧客情報が削除されているパターン
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleInfoElement, "DeleteDate", batchStartDateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                End If

                detailElement.AppendChild(scheduleInfoElement)
            End If

            'scheduleタグの作成
            CreateAfterProcessScheduleTag(scheduleInfoRow, xmlDoc, detailElement)

            UpdateCount = UpdateCount + 1

            ' 最大送信件数が指定されており、送信数が最大送信件数以上の場合ループ処理を抜ける
            rowIndex = i
            If sendMaxCount <> 0 AndAlso UpdateCount = sendMaxCount Then
                Exit For
            End If
        Next

        Logger.Info(C_SYSTEM & " CreateDetailTag End " & rowIndex)
        rowIndex = rowIndex + 1
        Return UpdateCount

        '$03 受注後フォロー機能開発 END
    End Function
    '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END

#End Region

#Region "スケジュールタグ作成(受注後工程)"

    '$03 受注後フォロー機能開発 START
    ''' <summary>
    ''' スケジュールタグ作成(受注後工程)
    ''' </summary>
    ''' <param name="scheduleInfoRow">作成スケジュール情報</param>
    ''' <param name="xmlDoc">追加対象XML</param>
    ''' <param name="detailElement"></param>
    ''' <remarks></remarks>
    Private Sub CreateAfterProcessScheduleTag(ByVal scheduleInfoRow As MC3040401DataSet.MC3040401UpdateScheduleInfoRow, _
                                  ByRef xmlDoc As XmlDocument, _
                                  ByRef detailElement As XmlElement)
        'Scheduleの作成
        If Not "8".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

            Dim scheduleElement As XmlElement = xmlDoc.CreateElement("Schedule")

            'スケジュール作成区分
            SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "CreateScheduleDiv", "2")

            If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "2".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '新規追加パターン
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffBranchCode", scheduleInfoRow.ACTSTAFFSTRCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffCode", scheduleInfoRow.ACTSTAFFCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ReceptionStaffBranchCode", scheduleInfoRow.RECSTAFFSTRCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ReceptionStaffCode", scheduleInfoRow.RECSTAFFCD)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ContactNo", scheduleInfoRow.CONTACTNO)

                If BLANK_STRING.Equals(scheduleInfoRow.CONTACT_NAME) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ContactName", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ContactName", scheduleInfoRow.CONTACT_NAME)
                End If

                If BLANK_STRING.Equals(scheduleInfoRow.ACT_ODR_NAME) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActOdrName", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActOdrName", scheduleInfoRow.ACT_ODR_NAME)
                End If

                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "Summary", scheduleInfoRow.SUMMARY)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "StartTime", scheduleInfoRow.STARTTIME)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "EndTime", scheduleInfoRow.ENDTIME)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "Memo", scheduleInfoRow.MEMO)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "XiCropColor", scheduleInfoRow.BACKGROUNDCOLOR)

                If BLANK_STRING.Equals(scheduleInfoRow.ODR_DIV) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", scheduleInfoRow.ODR_DIV)
                End If

                If BLANK_STRING.Equals(scheduleInfoRow.AFTER_ODR_ACT_ID) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "AfterOdrActID", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "AfterOdrActID", scheduleInfoRow.AFTER_ODR_ACT_ID)
                End If

                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "TodoID", scheduleInfoRow.TODOID)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ProcessDiv", scheduleInfoRow.PROCESSDIV)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ResultDate", scheduleInfoRow.RESULTDATE)

            ElseIf "5".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '活動担当スタッフが変更されているパターン
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                'SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffBranchCode", scheduleInfoRow.ACTSTAFFSTRCD)
                'SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ActivityStaffCode", scheduleInfoRow.ACTSTAFFCD)
                SetNode(xmlDoc, scheduleElement, "ActivityStaffBranchCode", scheduleInfoRow.ACTSTAFFSTRCD)
                SetNode(xmlDoc, scheduleElement, "ActivityStaffCode", scheduleInfoRow.ACTSTAFFCD)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                If BLANK_STRING.Equals(scheduleInfoRow.ODR_DIV) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", scheduleInfoRow.ODR_DIV)
                End If

                If BLANK_STRING.Equals(scheduleInfoRow.AFTER_ODR_ACT_ID) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "AfterOdrActID", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "AfterOdrActID", scheduleInfoRow.AFTER_ODR_ACT_ID)
                End If

                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "TodoID", scheduleInfoRow.TODOID)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ProcessDiv", scheduleInfoRow.PROCESSDIV)

            ElseIf "7".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '顧客名が変更されているパターン
                Dim summary As String = GetSummary(scheduleInfoRow, False)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "Summary", summary)

                If BLANK_STRING.Equals(scheduleInfoRow.ODR_DIV) Then
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", String.Empty)
                Else
                    SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "OdrDiv", scheduleInfoRow.ODR_DIV)
                End If

                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "AfterOdrActID", scheduleInfoRow.AFTER_ODR_ACT_ID)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "TodoID", scheduleInfoRow.TODOID)
                SetNodeEmptyTagNotCreate(xmlDoc, scheduleElement, "ProcessDiv", scheduleInfoRow.PROCESSDIV)
            End If

            detailElement.AppendChild(scheduleElement)

            'Alarmの作成
            If "1".Equals(scheduleInfoRow.UPDATEPROCDIV) Or _
               "2".Equals(scheduleInfoRow.UPDATEPROCDIV) Then

                '新規追加パターン
                If Not String.IsNullOrEmpty(scheduleInfoRow.ALARMNO) Then
                    Dim alarmElement As XmlElement = xmlDoc.CreateElement("Alarm")
                    SetNodeEmptyTagNotCreate(xmlDoc, alarmElement, "Trigger", scheduleInfoRow.ALARMNO)
                    scheduleElement.AppendChild(alarmElement)
                End If
            End If
        End If
    End Sub
    '$03 受注後フォロー機能開発 END

#End Region

#Region "送信最大件数の取得"

    ''' <summary>
    ''' 送信最大件数の取得
    ''' </summary>
    ''' <returns>送信最大件数</returns>
    ''' <remarks></remarks>
    Private Function GetSendMaxCount() As Integer
        Dim returnValue As Integer = 0
        Dim sendMaxCount As String = String.Empty
        Try
            ' 設定ファイルより送信最大件数を取得する。
            sendMaxCount = BatchSetting.GetValue(SETTING_SECTION, KEY_SENDMAXCOUNT, "0")
        Catch ex As ArgumentException
            Return SEND_MAXCOUNT_DEFAULT
        End Try

        If Integer.TryParse(sendMaxCount, returnValue) Then
            If returnValue = 0 Then
                Return SEND_MAXCOUNT_DEFAULT
            Else
                Return returnValue
            End If
        End If
        Return SEND_MAXCOUNT_DEFAULT
    End Function

#End Region

#Region "タイムアウト時間の取得"

    '$03 受注後フォロー機能開発 START
    ''' <summary>
    ''' タイムアウト時間の取得
    ''' </summary>
    ''' <returns>タイムアウト時間(設定ファイルから取得できない場合は0)</returns>
    ''' <remarks></remarks>
    Private Function GetTimeoutPeriod() As Integer
        Dim returnValue As Integer = 0
        Dim timeoutPeriod As String = String.Empty
        Try
            ' 設定ファイルよりタイムアウト時間を取得する。
            timeoutPeriod = BatchSetting.GetValue(SETTING_SECTION, KEY_TIMEOUTPERIOD, "0")
        Catch ex As ArgumentException
            Return 0
        End Try

        If Integer.TryParse(timeoutPeriod, returnValue) Then

            Return returnValue

        End If
        Return 0
    End Function
    '$03 受注後フォロー機能開発 END

#End Region

End Class
