'-------------------------------------------------------------------------
'IC3190402WebServiceClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：部品ステータス情報取得用関数
'補足：
'作成：2014/XX/XX NEC 村瀬
'更新：2017/03/16 NSK A.Minagawa TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 $01
'更新：2019/05/21 NSK M.Sakamoto 18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策
'更新：2019/11/05 NSK M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 $02
'更新：2020/02/18 NSK S.Imaizumi TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される $03
'更新：2020/08/06 NSK S.Natsume TR-SVT-TMT-20200710-001 ログへ出力される文字が多すぎるために発生するエラー $04
'─────────────────────────────────────
Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Reflection
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.PartsManagement.PSMonitor.DataAccess
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Public Class IC3190402BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
#Region "PublicConst"
    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0

    ''' <summary>
    ''' XML戻り値解析失敗
    ''' </summary>
    ''' <remarks></remarks>
    Public Const XmlErr As String = "-1"
#End Region

#Region "PrivateConst"
    ''' <summary>
    ''' WebService名(IC3A09918)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsWebServiceID As String = "IC3A09918"

    ''' <summary>
    ''' WebService(IC3A09918)メソッド名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsWebServiceMethod As String = "GetPartsStatus"

    ''' <summary>
    ''' WebService(IC3A09918)引数名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsWebServiceArgument As String = "xsData="

    ''' <summary>
    ''' WebServiceURL(IC3A09918)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsWebServiceURL As String = "LINK_URL_PARTS_STATUS"

    ''' <summary>
    ''' WebService ヘッダーコメント削除置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsXmlReplace As String = "<?xml version=""1.0"" encoding=""utf-16""?>"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsEncodingUTF8 As String = "UTF-8"

    ''' <summary>
    ''' 送信方法(POST)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsPost As String = "POST"

    ''' <summary>
    ''' ContentType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsContentTypeString As String = "application/x-www-form-urlencoded"


    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsDateFormat As String = "dd/MM/yyyy HH:mm:ss"

    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsAllDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 全店舗を意味するワイルドカード店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsAllBranchCode As String = "XXX"

#Region "ログ文言"
    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsLogStart As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsLogEnd As String = "End"
#End Region

#End Region

#End Region

#Region "列挙体"

    ''' <summary>
    ''' 基幹コード区分
    ''' </summary>
    Public Enum DmsCodeType

        ''' <summary>
        ''' 区分なし
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        DealerCode = 1

        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        BranchCode = 2

        ''' <summary>
        ''' ストールID
        ''' </summary>
        ''' <remarks></remarks>
        StallId = 3

        ''' <summary>
        ''' 顧客分類
        ''' </summary>
        ''' <remarks></remarks>
        CustomerClass = 4

        ''' <summary>
        ''' 作業ステータス
        ''' </summary>
        ''' <remarks></remarks>
        WorkStatus = 5

        ''' <summary>
        ''' 中断理由区分
        ''' </summary>
        ''' <remarks></remarks>
        JobStopReasonType = 6

        ''' <summary>
        ''' チップステータス
        ''' </summary>
        ''' <remarks></remarks>
        ChipStatus = 7

        ''' <summary>
        ''' 希望連絡時間帯
        ''' </summary>
        ''' <remarks></remarks>
        ContactTimeZone = 8

        ''' <summary>
        ''' メーカー区分
        ''' </summary>
        ''' <remarks></remarks>
        MakerType = 9

    End Enum

    ''' <summary>
    ''' 返却結果コード
    ''' </summary>
    Private Enum ReturnCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrTimeout = 6001

        ''' <summary>
        ''' DMS側エラー発生
        ''' </summary>
        ErrDms = 6002

        ''' <summary>
        ''' その他エラー
        ''' </summary>
        ErrOther = 6003

    End Enum

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 部品ステータス情報取得WebService呼出処理
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>WebService処理結果。Nothingの場合はXML解析エラー発生する原因となる</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function CallGetPartsSearchInfoWebService(ByVal inXmlClass As PartsSearchXmlDocumentClass) As String

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        '開始ログの出力
        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogStart))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'XML戻り値用DataTable
        Dim dtWebServiceResult As New IC3190402DataSet.PartsSearchResultDataTable

        'XML戻り値用DataRow
        Dim rowWebServiceResult As IC3190402DataSet.PartsSearchResultRow = dtWebServiceResult.NewPartsSearchResultRow

        Try
            'WebServiceURLの取得
            Dim envSettingRow As String = String.Empty
            envSettingRow = GetDlrSystemSettingValueBySettingName(ConsWebServiceURL)

            'URLの取得確認
            If String.IsNullOrEmpty(envSettingRow) Then
                'URL取得失敗

                rowWebServiceResult.ResultCode = ReturnCode.ErrOther

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DlrEnvSetting == NOTHING OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rowWebServiceResult.ResultCode))

                Return ""

            End If

            'WebServiceのURLを作成
            Dim createUrl As String = String.Empty

            '取得結果の末尾の文字列と一致するかどうかを判断する
            If envSettingRow.EndsWith(ConsWebServiceMethod) = False Then
                createUrl = String.Concat(envSettingRow, "/", ConsWebServiceMethod)
            Else
                createUrl = envSettingRow
            End If

            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "DEBUG:createUrl=" & createUrl))
            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            'WebService送信用XML作成処理
            Dim sendXml As String = CreateXml(inXmlClass)

            'XMLのヘッダー部分を削除
            sendXml = sendXml.Replace(ConsXmlReplace, String.Empty)

            '送信XMLをエンコードし引数に指定
            sendXml = String.Concat(ConsWebServiceArgument, HttpUtility.UrlEncode(sendXml))

            'WebService送受信処理
            Dim resultString As String = CallWebServiceSite(sendXml, createUrl)

            ' $03 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される START
            ''返却された文字列をデコード
            'resultString = HttpUtility.HtmlDecode(resultString)
            ' $03 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される END

            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmln.*=""[^""]*"".")

            'XML名前空間を除去
            resultString = regex.Replace(resultString, Space(0))

            ''終了ログの出力
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "{0}.{1} OUT:resultXmlValue.ResultCode = {2}" _
            '            , Me.GetType.ToString _
            '            , MethodBase.GetCurrentMethod.Name _
            '            , rowWebServiceResult.ResultCode))

            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            '終了ログの出力
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '            , "{0}.{1} {2}" _
            '            , Me.GetType.ToString _
            '            , MethodBase.GetCurrentMethod.Name _
            '            , ConsLogEnd))
            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            Return resultString

        Catch ex As System.Net.WebException
            'WebServiceエラー

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService(ex) = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

            If ex.Status = WebExceptionStatus.Timeout Then

                Dim drTemp As IC3190402DataSet.PartsSearchResultRow = dtWebServiceResult.NewPartsSearchResultRow
                drTemp.ResultCode = ReturnCode.ErrTimeout

                dtWebServiceResult.AddPartsSearchResultRow(drTemp)

            Else

                Dim drTemp As IC3190402DataSet.PartsSearchResultRow = dtWebServiceResult.NewPartsSearchResultRow
                drTemp.ResultCode = ReturnCode.ErrOther

                dtWebServiceResult.AddPartsSearchResultRow(drTemp)

            End If

            Return ""

        Catch ex2 As System.Exception

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService(ex2) = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex2.Message))

            Dim drTemp As IC3190402DataSet.PartsSearchResultRow = dtWebServiceResult.NewPartsSearchResultRow
            drTemp.ResultCode = ReturnCode.ErrOther

            dtWebServiceResult.AddPartsSearchResultRow(drTemp)

            Return ""

        End Try

    End Function

    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.InvariantCulture _
        '                          , "{0} {1} SETTINGNAME={2}" _
        '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                          , ConsLogStart _
        '                          , settingName))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '戻り値
        Dim retValue As String = String.Empty

        'ログイン情報
        Dim userContext As StaffContext = StaffContext.Current

        '販売店システム設定テーブルから取得
        '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 START
        'Using dt As SC3190402DataSet.SystemSettingDataTable _
        '                        = SC3190402DataSet.GetDlrSystemSettingValue(userContext.DlrCD, _
        '                                                                  userContext.BrnCD, _
        '                                                                  ConsAllDealerCode, _
        '                                                                  ConsAllBranchCode, _
        '                                                                  settingName)
        '    If 0 < dt.Count Then
        '        '設定値を取得
        '        retValue = dt.Item(0).SETTING_VAL
        '    Else
        '        retValue = ""
        '    End If

        'End Using

        Dim systemSettingDlr As New SystemSettingDlr

        Dim row As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow _
                                = systemSettingDlr.GetEnvSetting(userContext.DlrCD, _
                                                                          userContext.BrnCD, _
                                                                          settingName)
        If row IsNot Nothing Then
            '設定値を取得
            retValue = row.SETTING_VAL
        Else
            retValue = ""
        End If
        '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 END

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2} OUT:retValue = {3}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogEnd _
        '            , retValue))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START


        Return retValue

    End Function

    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $02
    ''' <summary>
    ''' システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSystemSettingValueBySettingName(ByVal settingName As String) As String

        '戻り値
        Dim retValue As String = String.Empty

        '販売店システム設定テーブルから取得
        'S.Natsume TR-SVT-TMT-20200710-001 ログへ出力される文字が多すぎるために発生するエラー START $04
        'Using dt As SC3190402DataSet.SystemSettingDataTable _
        '                        = SC3190402DataSet.GetSystemSettingValue(settingName)

        '    If dt.Rows.Count > 0 Then
        '        '設定値を取得
        '        retValue = dt.Item(0).SETTING_VAL
        '    Else
        '        retValue = ""
        '    End If
        'End Using
        Dim systemSetting As New SystemSetting

        Dim row As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow _
            = systemSetting.GetSystemSetting(settingName)

        If row IsNot Nothing Then
            '設定値を取得
            retValue = CStr(row.SETTING_VAL)
        Else
            retValue = ""
        End If

        'S.Natsume TR-SVT-TMT-20200710-001 ログへ出力される文字が多すぎるために発生するエラー END $04

        Return retValue

    End Function
    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $02

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <remarks>基幹コード情報ROW</remarks>
    ''' <history>
    ''' </history>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As IC3190402DataSet.DmsCodeMapRow

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} " _
        '          , Me.GetType.ToString _
        '          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '          , ConsLogStart _
        '          , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START

        '基幹コードへ変換処理
        Using dtDmsCodeMap As IC3190402DataSet.DmsCodeMapDataTable = _
            Me.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                        DmsCodeType.BranchCode, _
                                        inStaffInfo.DlrCD, _
                                        inStaffInfo.BrnCD, _
                                        String.Empty, _
                                        inStaffInfo.Account)
            '基幹コード情報Row
            Dim rowDmsCodeMap As IC3190402DataSet.DmsCodeMapRow

            '基幹コードへ変換処理結果チェック
            If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
                '基幹コードへ変換処理成功

                'Rowに変換
                rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), IC3190402DataSet.DmsCodeMapRow)

                '基幹アカウントチェック
                If rowDmsCodeMap.IsACCOUNTNull Then
                    '値無し

                    '空文字を設定する
                    '基幹アカウント
                    rowDmsCodeMap.ACCOUNT = String.Empty

                End If

                '基幹販売店コードチェック
                If rowDmsCodeMap.IsCODE1Null Then
                    '値無し

                    '空文字を設定する
                    '基幹販売店コード
                    rowDmsCodeMap.CODE1 = String.Empty

                End If

                '基幹店舗コードチェック
                If rowDmsCodeMap.IsCODE2Null Then
                    '値無し

                    '空文字を設定する
                    '基幹店舗コード
                    rowDmsCodeMap.CODE2 = String.Empty

                End If

            Else
                '基幹コードへ変換処理成功失敗

                '新しいRowを作成
                rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, IC3190402DataSet.DmsCodeMapRow)

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If

            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            '終了ログ
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '           , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
            '           , Me.GetType.ToString _
            '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '           , ConsLogEnd _
            '           , dtDmsCodeMap.Count))
            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '結果返却
            Return rowDmsCodeMap
        End Using

    End Function

    ''' <summary>
    ''' i-CROP→DMSの値に変換された値を取得する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="dmsCodeType">基幹コード区分</param>
    ''' <param name="icropCD1">iCROPコード1</param>
    ''' <param name="icropCD2">iCROPコード2</param>
    ''' <param name="icropCD3">iCROPコード3</param>
    ''' <param name="account">アカウント</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 基幹コード区分(1～7)によって、引数に設定する値が異なる
    ''' ※ここに全て記載すると非常に長くなるため、TB_M_DMS_CODE_MAPのテーブル定義書を参照して下さい
    ''' </remarks>
    Public Function GetIcropToDmsCode(ByVal dealerCD As String, _
                                      ByVal dmsCodeType As DmsCodeType, _
                                      ByVal icropCD1 As String, _
                                      ByVal icropCD2 As String, _
                                      ByVal icropCD3 As String, _
                                      Optional ByVal account As String = "") As IC3190402DataSet.DmsCodeMapDataTable

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} ", _
        '                          Me.GetType.ToString, _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          ConsLogStart, _
        '                          dealerCD, _
        '                          CType(dmsCodeType, Integer), _
        '                          icropCD1, _
        '                          icropCD2, _
        '                          icropCD3))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '戻り値
        Using dt As IC3190402DataSet.DmsCodeMapDataTable =
                                        IC3190402DataSet.GetIcropToDmsCode(ConsAllDealerCode, _
                                                                        dealerCD, _
                                                                        dmsCodeType, _
                                                                        icropCD1, _
                                                                        icropCD2, _
                                                                        icropCD3)

            If dt.Count <= 0 Then
                'データが取得できない場合
                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} WARN：No data found. ", _
                                           Me.GetType.ToString, _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
            End If

            'アカウント情報と取得項目のチェック
            If Not (String.IsNullOrEmpty(account)) AndAlso _
               (dmsCodeType = dmsCodeType.DealerCode OrElse _
               dmsCodeType = dmsCodeType.BranchCode OrElse _
               dmsCodeType = dmsCodeType.StallId) Then
                'アカウントが存在する場合且つ、販売店・店舗・ストールの情報を取得する場合
                '変換したアカウントを格納
                dt(0).ACCOUNT = account.Split(CChar("@"))(0)

            End If

            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
            '                          "{0}.{1} {2} QUERY:COUNT = {3}" _
            '                          , Me.GetType.ToString _
            '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                          , ConsLogEnd _
            '                          , dt.Count))
            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            Return dt
        End Using
    End Function

#End Region

#Region "Privateメソッド"

#Region "XML作成"

    ''' <summary>
    ''' XML作成(メイン)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XMLString</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXml(ByVal inXmlClass As PartsSearchXmlDocumentClass) As String

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログの出力
        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogStart))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END


        'XMLのHeadTagの作成処理
        inXmlClass = CreateHeadTag(inXmlClass)

        'テキストWriter
        Using writer As New StringWriter(CultureInfo.InvariantCulture)

            'XMLシリアライザー型の設定
            Dim serializer As New XmlSerializer(GetType(PartsSearchXmlDocumentClass))

            'XmlDocumentClassをXML化
            serializer.Serialize(writer, inXmlClass)

            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmlns:xs.=""[^""]*""")

            'XML名前空間を除去
            Dim stringXml As String = regex.Replace(writer.ToString, Space(0))

            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            ''終了ログの出力
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '            , "{0}.{1} {2} OUT:retValue = {3}" _
            '            , Me.GetType.ToString _
            '            , MethodBase.GetCurrentMethod.Name _
            '            , ConsLogEnd _
            '            , stringXml))
            '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            Return stringXml

        End Using

    End Function

    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XML作成用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTag(ByVal inXmlClass As PartsSearchXmlDocumentClass) As PartsSearchXmlDocumentClass

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogStart))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'メッセージID
        inXmlClass.Head.MessageId = ConsWebServiceID

        '国コード
        inXmlClass.Head.CountryCode = EnvironmentSetting.CountryCode

        '基幹SYSTEM識別コード(0固定)
        inXmlClass.Head.LinkSystemCode = "0"

        'トランスミッションDATE
        'inXmlClass.Head.TransmissionDate = DateTimeFunc.FormatDate(1, DateTimeFunc.Now)
        inXmlClass.Head.TransmissionDate = Format(DateTimeFunc.Now, ConsDateFormat)

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2} OUT:retValue = {3}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogEnd _
        '            , inXmlClass.ToString))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return inXmlClass


    End Function

#End Region


#Region "XML送受信"

    ''' <summary>
    ''' WebServiceのサイトを呼出
    ''' WebServiceを送信し結果を受信する
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="WebServiceUrl">送信先URL</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function CallWebServiceSite(ByVal sendXml As String, ByVal webServiceUrl As String) As String

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログの出力
        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2} SENDXML:{3} URL:{4}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogStart _
        '            , sendXml, webServiceUrl))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END


        '文字コードを指定する
        Dim enc As System.Text.Encoding = _
            System.Text.Encoding.GetEncoding(ConsEncodingUTF8)

        'バイト型配列に変換
        Dim postDataBytes As Byte() = _
            System.Text.Encoding.UTF8.GetBytes(sendXml)

        'WebRequestの作成
        Dim req As WebRequest = WebRequest.Create(webServiceUrl)

        'メソッドにPOSTを指定
        req.Method = ConsPost

        'ContentType指定(固定)
        req.ContentType = ConsContentTypeString

        'POST送信するデータの長さを指定
        req.ContentLength = postDataBytes.Length

        '送信タイムアウト設定(10秒)
        req.Timeout = 10000

        'データをPOST送信するためのStreamを取得
        Using reqStream As Stream = req.GetRequestStream()

            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)

        End Using

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim resultResponse As WebResponse = req.GetResponse()

        '応答データを受信するためのStreamを取得
        Dim resultStream As Stream = resultResponse.GetResponseStream()

        '返却文字列
        Dim resultString As String

        '受信して表示
        Using resultReader As New StreamReader(resultStream, enc)

            '返却文字列を取得
            resultString = resultReader.ReadToEnd()

        End Using

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログの出力
        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2} OUT:retValue = {3}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogEnd _
        '            , resultString))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return resultString

    End Function

    ''' <summary>
    ''' Tagから値を取得
    ''' </summary>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <param name="tagName">Tag名</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetTagValue(ByVal resultXmlNode As XmlNode, _
                                 ByVal tagName As String) As String

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2} IN:TAGNAME = {3}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogStart _
        '            , tagName))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '処理結果
        Dim resultValue As String = XmlErr

        'タグの取得
        Dim selectNodeList As XmlNodeList = resultXmlNode.SelectNodes(tagName)

        'タグの確認
        If selectNodeList Is Nothing OrElse selectNodeList.Count = 0 Then
            '取得失敗

            'エラーログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err GET {2} VALUE = Nothing" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , tagName))

            'コードに-1を設定
            Return XmlErr

        End If

        '値の取得
        resultValue = selectNodeList.Item(0).InnerText.Trim

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} {2} OUT:retValue = {3}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ConsLogEnd _
        '            , resultValue))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return resultValue

    End Function

#End Region
#End Region


    ''' <summary>
    ''' IDisposable.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
    End Sub

End Class
