'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3060101BusinessLogic.vb
'─────────────────────────────────────
'機能： 査定チェックシートビジネスロジック
'補足： 
'作成： 2011/11/29 KN 清水
'更新： 2012/03/19 KN 清水  【SALES_1B】SALES_1B UT(課題No.0023) TCV遷移対応
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $02
'─────────────────────────────────────


Imports System.Xml
Imports System.Text
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Imports System.Globalization
Imports System.Net
Imports Toyota.eCRB.Assessment.Assessment.DataAccess

''' <summary>
''' SC3060101(査定チェックシート)
''' ビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3060101BusinessLogic
    Inherits BaseBusinessComponent


#Region "メンバ変数"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private resultId_ As Integer
#End Region


#Region "プロパティ"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <value>終了コード</value>
    ''' <returns>終了コード</returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public Property ResultId As Integer
        Get
            Return resultId_
        End Get
        Set(ByVal value As Integer)
            resultId_ = value
        End Set
    End Property
#End Region

#Region "定数"

    ''' <summary>エラーコード：正常終了</summary>
    Public Const ErrorCodeFinish As Integer = 0
    ''' <summary>エラーコード：査定情報末存在</summary>
    Public Const ErrorCodeNoData As Integer = 1101
    ''' <summary>エラーコード：販売店コード未設定</summary>
    Public Const ErrorCodeDealerNoSet As Integer = 2001
    ''' <summary>エラーコード：店舗コード未設定</summary>
    Public Const ErrorCodeStoreNoSet As Integer = 2002
    ''' <summary>エラーコード：依頼ID未設定</summary>
    Public Const ErrorCodeRequestNoSet As Integer = 2003
    ''' <summary>エラーコード：査定No未設定</summary>
    Public Const ErrorCodeAssessmentNoSet As Integer = 2004
    ''' <summary>エラーコード：IF呼び出しエラー</summary>
    Public Const ErrorCodeSend As Integer = 9999
    ''' <summary>エラーコード：パラメータ未設定</summary>
    Public Const ErrorMsgParameterNoSet As String = "input param error "
    ''' <summary>エラーコード：IF呼び出しエラー</summary>
    Public Const ErrorMsgUcar As String = "call ucar error "

    ''' <summary>DataSet用 ロケール指定</summary>
    Private Const DataSetCulture As String = "ja-JP"
    ''' <summary>環境設定値１：U-MSのIF、URL</summary>
    Private Const EnvParameterName1 As String = "U-MS_ASSESSMENT_INFO_URL"
    ''' <summary>環境設定値２：U-MSの車両画像URL</summary>
    Private Const EnvParameterName2 As String = "U-MS_CAR_IMAGE_URL"
    ''' <summary>環境設定値３：U-MSのスタッフ画像URL</summary>
    Private Const EnvParameterName3 As String = "U-MS_STAFF_IMAGE_URL"
    ''' <summary>環境設定値４：U-MSのIF アカウント</summary>
    Private Const EnvParameterName4 As String = "U-MS_IF_ACCOUNT"
    ''' <summary>環境設定値５：U-MSのIF パスワード</summary>
    Private Const EnvParameterName5 As String = "U-MS_IF_PASSWORD"


    ''' <summary>開始ログ</summary>
    Private Const STARTLOG As String = "START "

    ''' <summary>終了ログ</summary>
    Private Const ENDLOG As String = "END "

    ''' <summary>終了ログRETURN</summary>
    Private Const ENDLOGRETURN As String = "RETURN "

    ''' <summary>
    ''' 1: 名前の前に敬称(主に英語圏)、2: 名前の後ろに敬称(中国など)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTKEISYOZENGO As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Native向け)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTFACEPICUPLOADPATH As String = "FACEPIC_UPLOADPATH"

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTFACEPICUPLOADURL As String = "FACEPIC_UPLOADURL"

    '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
    ' リクエストカテゴリ
    ' Walk-in
    Public Const RequestcategoryWalkin As String = "11"


    ' CR活動結果
    ' Hot
    Public Const CractresultHot As String = "21"
    ' Prospect
    Public Const CractresultProspect As String = "2"
    ' Success
    Public Const CractresultSuccess As String = "31"
    ' Giveup
    Public Const CractresultGiveup As String = "32"
    '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END


#End Region

    ''' <summary>
    ''' 中古車IF呼び出し
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="requestId">依頼ID</param>
    ''' <param name="assessmentNo">査定No</param>
    ''' <returns>出力XMLのDataSet</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetAssessmentInfo(ByVal dealerCode As String, ByVal storeCode As String, ByVal requestId As String, ByVal assessmentNo As String) As DataSet

        Const METHODNAME As String = "GetAssessmentInfo "
        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '引数チェック：販売店コード
        If IsNothing(dealerCode) Then
            Me.ResultId = ErrorCodeDealerNoSet
            'デバッグログ(終了)
            OutErrorLog(METHODNAME, ErrorMsgParameterNoSet & Me.ResultId)
            Return Nothing
        End If

        '引数チェック：店舗コード
        If IsNothing(storeCode) Then
            Me.ResultId = ErrorCodeStoreNoSet
            'デバッグログ(終了)
            OutErrorLog(METHODNAME, ErrorMsgParameterNoSet & Me.ResultId)
            Return Nothing
        End If

        '引数チェック：依頼ID
        If IsNothing(requestId) Then
            Me.ResultId = ErrorCodeRequestNoSet
            'デバッグログ(終了)
            OutErrorLog(METHODNAME, ErrorMsgParameterNoSet & Me.ResultId)
            Return Nothing
        End If

        '引数チェック：査定No
        If IsNothing(assessmentNo) Then
            Me.ResultId = ErrorCodeAssessmentNoSet
            'デバッグログ(終了)
            OutErrorLog(METHODNAME, ErrorMsgParameterNoSet & Me.ResultId)
            Return Nothing
        End If

        Dim ret As String = SendUcar(dealerCode, storeCode, requestId, assessmentNo)

        If IsNothing(ret) Then
            'デバッグログ(終了)
            Me.ResultId = ErrorCodeSend
            OutErrorLog(METHODNAME, ErrorMsgUcar & Me.ResultId)
            Return Nothing
        End If

        Dim culture As System.Globalization.CultureInfo
        culture = New System.Globalization.CultureInfo(DataSetCulture)

        Using ds As New System.Data.DataSet()
            ds.Locale = culture

            Using reader = New IO.StringReader(ret)
                ds.ReadXml(reader)
            End Using

            Using envTable As New DataTable("ENV")
                envTable.Locale = culture
                envTable.Columns.Add("PARAM")
                Dim row As DataRow = envTable.NewRow()

                row.Item("PARAM") = GetEnv(EnvParameterName3)
                envTable.Rows.Add(row)

                row = envTable.NewRow()
                row.Item("PARAM") = GetEnv(EnvParameterName2)
                envTable.Rows.Add(row)

                ds.Tables.Add(envTable)
            End Using

            '共通エラーコード取得
            Dim id As Integer = CInt(DirectCast(ds.Tables("Common").Rows(0).Item(0), String))
            Dim msg As String = DirectCast(ds.Tables("Common").Rows(0).Item(1), String)

            '正常でない場合
            If Not ErrorCodeFinish.Equals(id) Then
                'デバッグログ(終了)
                OutErrorLog(METHODNAME, id & msg)
                Me.ResultId = ErrorCodeSend
            End If
            'デバッグログ(終了)
            OutErrorLog(METHODNAME, CStr(id))
            Return ds
        End Using

    End Function

    ''' <summary>
    ''' 中古車IF呼び出し
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="requestId">依頼ID</param>
    ''' <param name="assessmentNo">査定No</param>
    ''' <returns>出力XMLの文字列</returns>
    ''' <remarks>
    ''' </remarks>
    Private Function SendUcar(ByVal dealerCode As String, ByVal storeCode As String, ByVal requestId As String, ByVal assessmentNo As String) As String

        Const METHODNAME As String = "SendUcar "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim webClient As SC3060101WebClient = New SC3060101WebClient(dealerCode, storeCode, requestId, assessmentNo)

        'システム環境設定から、中古車IFのURLを取得
        Dim strUrl As String
        strUrl = GetEnv(EnvParameterName1)
        'システム環境設定から、中古車IFのアカウントを取得
        Dim userId As String
        userId = GetEnv(EnvParameterName4)
        'システム環境設定から、中古車IFのパスワードを取得
        Dim password As String
        password = GetEnv(EnvParameterName5)

        'IFを呼び出す。
        Dim ret As String = ""

        Try

            Dim url As System.Uri = New System.Uri(strUrl)

            ret = webClient.SendRequest(url, userId, password, getSendDate())
        Catch ex As WebException

            'デバッグログ(終了)
            OutErrorLog(METHODNAME, ErrorCodeSend & ex.Message)
            Me.ResultId = ErrorCodeSend
            Return Nothing
        End Try

        Return ret

    End Function

    ''' <summary>
    ''' 現在日付取得
    ''' </summary>
    ''' <remarks>
    ''' IFにセットする現在日付
    ''' </remarks>
    Private Function getSendDate() As String

        Const METHODNAME As String = "getSendDate "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(CStr(DateTimeFunc.Now))
        Logger.Info(endLogInfo.ToString())

        Return CStr(DateTimeFunc.Now)

    End Function

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="returnCode">リターンコード</param>
    ''' <remarks>
    ''' 環境設定値
    ''' </remarks>
    Private Sub OutErrorLog(ByVal methodName As String, ByVal returnCode As String)

        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(MethodName)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(returnCode)
        Logger.Info(endLogInfo.ToString())

    End Sub

    ''' <summary>
    ''' 環境設定値取得
    ''' </summary>
    ''' <param name="paramaterName">環境設定値名</param>
    ''' <remarks>
    ''' 環境設定値
    ''' </remarks>
    Private Function GetEnv(ByVal paramaterName As String) As String

        Const METHODNAME As String = "GetEnv "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Dim ret As String

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(paramaterName)

        If sysEnvRow Is Nothing Then
            ret = ""
        Else
            ret = sysEnvRow.PARAMVALUE
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(ret)
        Logger.Info(endLogInfo.ToString())


        Return ret

    End Function

    ''' <summary>
    ''' 自社客取得
    ''' </summary>
    ''' <param name="inCustomerDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>自社客を取得する処理</remarks>
    Public Shared Function GetOrgCustomerData(ByVal inCustomerDataTbl As SC3060101DataSet.SC3060101ParameterDataTable) As SC3060101DataSet.SC3060101OrgCustomerDataTable

        Const METHODNAME As String = "GetOrgCustomerData "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim customerDataRow As SC3060101DataSet.SC3060101ParameterRow
        customerDataRow = CType(inCustomerDataTbl.Rows(0), SC3060101DataSet.SC3060101ParameterRow)
        '自社客取得
        Using outCustomerDataTbl = SC3060101TableAdapter.GetOrgCustomer(customerDataRow.DLRCD, customerDataRow.CRCUSTID)


            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            For Each drOutCustomerDataTbl In outCustomerDataTbl
                '敬称位置取得
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTKEISYOZENGO)
                drOutCustomerDataTbl.KEISYO_ZENGO = sysEnvRow.PARAMVALUE
            Next
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return outCustomerDataTbl
        End Using
    End Function

    ''' <summary>
    ''' 未取引客取得
    ''' </summary>
    ''' <param name="inCustomerDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>未取引客を取得する処理</remarks>
    Public Shared Function GetNewCustomerData(ByVal inCustomerDataTbl As SC3060101DataSet.SC3060101ParameterDataTable) As SC3060101DataSet.SC3060101NewCustomerDataTable

        Const METHODNAME As String = "GetNewCustomerData "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim customerDataRow As SC3060101DataSet.SC3060101ParameterRow
        customerDataRow = CType(inCustomerDataTbl.Rows(0), SC3060101DataSet.SC3060101ParameterRow)

        '未取引客取得
        Using outCustomerDataTbl = SC3060101TableAdapter.GetNewCustomer(customerDataRow.DLRCD, customerDataRow.CRCUSTID)

            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            For Each drOutCustomerDataTbl In outCustomerDataTbl
                '敬称位置取得
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTKEISYOZENGO)
                drOutCustomerDataTbl.KEISYO_ZENGO = sysEnvRow.PARAMVALUE
            Next
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return outCustomerDataTbl
        End Using
    End Function


    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <param name="datatableFrom"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatus(ByVal datatableFrom As SC3060101DataSet.SC3060101GetStatusFromDataTable) As SC3060101DataSet.SC3060101GetStatusToDataTable

        Const METHODNAME As String = "GetFollowupboxStatus "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
        Dim fllwupbox_seqno As Decimal              'Followupbox seqno
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END
        Dim datarowFrom As SC3060101DataSet.SC3060101GetStatusFromRow
        Dim result As SC3060101DataSet.SC3060101GetStatusToDataTable

        datarowFrom = CType(datatableFrom.Rows(0), SC3060101DataSet.SC3060101GetStatusFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' SQL発行
        result = SC3060101TableAdapter.GetFollowupboxStatus(dlrcd, strcd, fllwupbox_seqno)

        '編集
        For Each dr As SC3060101DataSet.SC3060101GetStatusToRow In result
            Select Case dr.CRACTRESULT
                Case CractresultSuccess, CractresultGiveup
                    dr.ENABLEFLG = False
                Case CractresultHot, CractresultProspect
                    dr.ENABLEFLG = True
                Case Else
                    If dr.REQCATEGORY = RequestcategoryWalkin Then
                        dr.ENABLEFLG = True
                    Else
                        dr.ENABLEFLG = False
                    End If
            End Select
        Next


        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

        Return result

    End Function

    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <param name="datatableFrom"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatusPast(ByVal datatableFrom As SC3060101DataSet.SC3060101GetStatusFromDataTable) As SC3060101DataSet.SC3060101GetStatusToDataTable

        Const METHODNAME As String = "GetFollowupboxStatusPast "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
        Dim fllwupbox_seqno As Decimal              'Followupbox seqno
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END
        Dim datarowFrom As SC3060101DataSet.SC3060101GetStatusFromRow
        Dim result As SC3060101DataSet.SC3060101GetStatusToDataTable

        datarowFrom = CType(datatableFrom.Rows(0), SC3060101DataSet.SC3060101GetStatusFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' SQL発行
        result = SC3060101TableAdapter.GetFollowupboxStatusPast(dlrcd, strcd, fllwupbox_seqno)

        '編集
        For Each dr As SC3060101DataSet.SC3060101GetStatusToRow In result
            Select Case dr.CRACTRESULT
                Case CractresultSuccess, CractresultGiveup
                    dr.ENABLEFLG = False
                Case CractresultHot, CractresultProspect
                    dr.ENABLEFLG = True
                Case Else
                    If dr.REQCATEGORY = RequestcategoryWalkin Then
                        dr.ENABLEFLG = True
                    Else
                        dr.ENABLEFLG = False
                    End If
            End Select
        Next


        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

        Return result

    End Function

    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 START
    ''' <summary>
    ''' 見積ID取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimatedId(ByVal datatableFrom As SC3060101DataSet.SC3060101GetEstimateidFromDataTable) As SC3060101DataSet.SC3060101GetEstimateidToDataTable
        Const METHODNAME As String = "GetEstimatedId "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード

        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
        Dim fllwupbox_seqno As Decimal              'Followupbox seqno
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

        Dim datatableEstimatedId As SC3060101DataSet.SC3060101GetEstimateidToDataTable
        dlrcd = datatableFrom(0).DLRCD
        strcd = datatableFrom(0).STRCD
        fllwupbox_seqno = datatableFrom(0).FLLWUPBOX_SEQNO
        ' SQL発行
        datatableEstimatedId = SC3060101TableAdapter.GetEstimateInfo(dlrcd, strcd, fllwupbox_seqno)

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

        ' 返却
        Return datatableEstimatedId

    End Function

    ''' <summary>
    ''' 契約状況取得
    ''' </summary>
    ''' <param name="EstimateInfoTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractFlg(ByVal EstimateInfoTbl As SC3060101DataSet.SC3060101ESTIMATEINFODataTable) As SC3060101DataSet.SC3060101ContractDataTable
        Const METHODNAME As String = "GetContractFlg "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim dr As SC3060101DataSet.SC3060101ESTIMATEINFORow = CType(EstimateInfoTbl.Rows(0), SC3060101DataSet.SC3060101ESTIMATEINFORow)

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

        '契約状況取得
        Return SC3060101TableAdapter.GetContractFlg(dr.ESTIMATEID)
    End Function
    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 END

End Class

