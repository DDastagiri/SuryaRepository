'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080101BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客検索一覧 (ビジネスロジック)
'補足： 
'作成： 2011/11/18 TCS 安田
'更新： 2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/12/03 TCS 森    Aカード情報相互連携開発
'更新： 2015/06/08 TCS 中村 TMT課題対応(#2)
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展  
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CustomerInfo.Search.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Text

''' <summary>
''' SC3080101(Customer List)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3080101BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' sales staff用 ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalesStaffId As Integer = 8

    ''' <summary>
    ''' serice adviser用 ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SericeAdviserId As Integer = 9

    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    ''' <summary>
    ''' 組織ID取得用：店舗内全組織ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AllOrg As String = "allOrg"

    ''' <summary>
    ''' 組織ID取得用：自チーム組織ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TeamOrg As String = "teamOrg"
    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END


    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstFacepicUploadurl As String = "FACEPIC_UPLOADURL"

    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    ''' <summary>
    ''' 業務権限フラグ（セールス）:業務権限あり
    ''' </summary>
    ''' <remarks></remarks>
    Public Const OrgnzSCFlgOn As String = "1"

    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

    '2015/06/08 TCS 中村 TMT課題対応(#2) START
    ''' <summary>
    ''' 区切り文字をシステム設定値より取得する際のキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RegNumDelimiterKey As String = "REG_NUM_DELIMITER"

    ''' <summary>
    ''' 区切り文字を分割する文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RegNumSplitter As Char = "*"c
    '2015/06/08 TCS 中村 TMT課題対応(#2) END

    ''' <summary>
    ''' 顧客一覧取得
    ''' </summary>
    ''' <param name="serchDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客一覧取得を取得する。</remarks>
    Public Shared Function GetCustomerList(ByVal serchDataTbl As SC3080101DataSet.SC3080101SerchDataTable) As SC3080101DataSet.SC3080101CustDataTable

        Dim serchDataRow As SC3080101DataSet.SC3080101SerchRow
        serchDataRow = serchDataTbl.Item(0)

        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
        '顧客一覧取得
        If (serchDataRow.SERCHTYPE = SC3080101TableAdapter.IdSerchTel Or _
            serchDataRow.SERCHTYPE = SC3080101TableAdapter.IdSerchSolId) Then
            '電話番号 or 国民番号で検索時
            '2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            Return SC3080101TableAdapter.GetTelSearchCustomerList(serchDataRow.DLRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING, _
                                        serchDataRow.SORTTYPE, _
                                        serchDataRow.SORTORDER, _
                                        serchDataRow.SERCHFLG, _
                                        serchDataRow.ORGNZ_ID)
            '2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
            ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
        Else
            '電話番号以外（名称・VIN・車両登録No）で検索時

            '2015/06/08 TCS 中村 TMT課題対応(#2) START
            '車両登録Noで検索時は検索文字間の"*"検索を可能にする
            If (serchDataRow.SERCHTYPE = SC3080101TableAdapter.IdSerchVclregno) Then
                serchDataRow.SERCHSTRING = RemoveRegNumDelimiter(serchDataRow.SERCHSTRING)
            End If
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("serchDataRow.SERCHSTRING = " + serchDataRow.SERCHSTRING)
            '2015/06/08 TCS 中村 TMT課題対応(#2) END

            ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
            Return SC3080101TableAdapter.GetCustomerList(serchDataRow.DLRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING, _
                                        serchDataRow.SORTTYPE, _
                                        serchDataRow.SORTORDER, _
                                        serchDataRow.ORGNZ_ID)
            ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
        End If

    End Function

    ''' <summary>
    ''' 顧客件数取得
    ''' </summary>
    ''' <param name="serchDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客件数</remarks>
    Public Shared Function GetCountCustomer(ByVal serchDataTbl As SC3080101DataSet.SC3080101SerchDataTable) As Integer

        Dim serchDataRow As SC3080101DataSet.SC3080101SerchRow
        serchDataRow = serchDataTbl.Item(0)

        '顧客件数取得
        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
        If (serchDataRow.SERCHTYPE = SC3080101TableAdapter.IdSerchTel _
            Or serchDataRow.SERCHTYPE = SC3080101TableAdapter.IdSerchSolId) Then
            '電話番号で検索時
            '2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            Return SC3080101TableAdapter.GetTelSearchCountCustomer(serchDataRow.DLRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING, _
                                        serchDataRow.SERCHFLG, _
                                        serchDataRow.ORGNZ_ID)
            '2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
            ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

        Else
            '電話番号以外（名称・VIN・車両登録No）で検索時

            '2015/06/08 TCS 中村 TMT課題対応(#2) START
            '車両登録Noで検索時は検索文字間の"*"検索を可能にする
            If (serchDataRow.SERCHTYPE = SC3080101TableAdapter.IdSerchVclregno) Then
                serchDataRow.SERCHSTRING = RemoveRegNumDelimiter(serchDataRow.SERCHSTRING)
            End If
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("serchDataRow.SERCHSTRING = " + serchDataRow.SERCHSTRING)
            '2015/06/08 TCS 中村 TMT課題対応(#2) END

            ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
            Return SC3080101TableAdapter.GetCountCustomer(serchDataRow.DLRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING, _
                                        serchDataRow.ORGNZ_ID)
            ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
        End If

    End Function

    '2015/06/08 TCS 中村 TMT課題対応(#2) START
    ''' <summary>
    ''' 車両登録No.文字列より、区切り文字を削除
    ''' </summary>
    ''' <param name="regNum">区切り文字削除対象文字列</param>
    ''' <returns>区切り文字削除後文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function RemoveRegNumDelimiter(ByVal regNum As String) As String

        'システム設定より区切り文字を取得
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        Dim systemBiz As New SystemSetting
        Dim sysRegNumDelimiter As String
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(RegNumDelimiterKey)
        If (dataRow Is Nothing) Then
            sysRegNumDelimiter = String.Empty
        Else
            sysRegNumDelimiter = dataRow.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        '区切り文字を検索文字列より削除
        If (Not sysRegNumDelimiter Is String.Empty) Then
            Dim regNumDelimiter() As String = sysRegNumDelimiter.Split(RegNumSplitter)
            For Each delimiter As String In regNumDelimiter
                regNum = regNum.Replace(delimiter, String.Empty)
            Next
        End If

        Return regNum
    End Function
    '2015/06/08 TCS 中村 TMT課題対応(#2) END

    ''' <summary>
    ''' sales staff権限情報取得
    ''' </summary>
    ''' <param name="serchDataTbl">データセット (インプット)</param>
    ''' <returns>sales staff用権限名</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSSName(ByVal serchDataTbl As SC3080101DataSet.SC3080101SerchDataTable) As String

        Dim serchDataRow As SC3080101DataSet.SC3080101SerchRow
        serchDataRow = serchDataTbl.Item(0)

        Dim tblOpera As SC3080101DataSet.SC3080101OperaTypeDataTable = _
                        SC3080101TableAdapter.GetOperaType(serchDataRow.DLRCD, SalesStaffId)

        If (tblOpera.Rows.Count > 0) Then
            Return tblOpera.Item(0).OPERATIONNAME
        Else
            Return "SC"
        End If

    End Function

    ''' <summary>
    ''' serice adviser権限情報取得
    ''' </summary>
    ''' <param name="serchDataTbl">データセット (インプット)</param>
    ''' <returns>serice adviser用権限名</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSAName(ByVal serchDataTbl As SC3080101DataSet.SC3080101SerchDataTable) As String

        Dim serchDataRow As SC3080101DataSet.SC3080101SerchRow
        serchDataRow = serchDataTbl.Item(0)

        Dim tblOpera As SC3080101DataSet.SC3080101OperaTypeDataTable = _
                        SC3080101TableAdapter.GetOperaType(serchDataRow.DLRCD, SericeAdviserId)

        If (tblOpera.Rows.Count > 0) Then
            Return tblOpera.Item(0).OPERATIONNAME
        Else
            Return "SA"
        End If

    End Function

    ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 自チーム配下の組織IDを取得する。
    ''' </summary>
    ''' <param name="orgnzId">自分が所属する組織ID</param>
    ''' <returns>自チーム配下の組織ID</returns>
    ''' <remarks>チームリーダーログイン時に利用</remarks>
    Public Shared Function GetMyTeamId(ByVal orgnzId As Decimal) As String

        Dim dt As SC3080101DataSet.SC3080101OrgDataTable = Nothing
        Dim myTeamList As New List(Of Decimal)
        Dim strRet As String = orgnzId.ToString()

        Try
            '同じ店舗内の全組織を取得
            dt = SC3080101TableAdapter.GetBranchSalesOrganizations()
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Try

            '再起処理にて下位セールス組織を収集
            GetMyTeamId(dt, orgnzId, myTeamList)
            For Each teamId As Decimal In myTeamList
                strRet &= "," & teamId.ToString()
            Next

        Finally
            If Not dt Is Nothing Then dt.Dispose()
            myTeamList.Clear()
        End Try

        Return strRet
    End Function

    ''' <summary>
    ''' 下位セールス組織を収集
    ''' </summary>
    ''' <param name="dt">店舗内全組織データ</param>
    ''' <param name="parentOrgnzId">親組織ID</param>
    ''' <param name="myTeamList">自チームIDリスト</param>
    ''' <remarks></remarks>
    Private Shared Sub getMyTeamId(ByRef dt As SC3080101DataSet.SC3080101OrgDataTable, ByVal parentOrgnzId As Decimal, ByRef myTeamList As List(Of Decimal))

        dt.DefaultView.RowFilter = "PARENT_ORGNZ_ID = " & parentOrgnzId.ToString()
        '下位組織無しのため現階層から抜ける
        If dt.DefaultView.Count = 0 Then Exit Sub

        For Each dvr As DataRowView In dt.DefaultView
            Dim dr As SC3080101DataSet.SC3080101OrgRow = CType(dvr.Row, SC3080101DataSet.SC3080101OrgRow)
            'セールス組織の場合、配下のチームとみなす
            If dr.ORGNZ_SC_FLG = "1" Then myTeamList.Add(dr.ORGNZ_ID)
            '再起処理にて下位セールス組織を収集
            GetMyTeamId(dt, dr.ORGNZ_ID, myTeamList)
        Next

    End Sub
    ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)取得
    ''' </summary>
    ''' <returns>顔写真の保存先フォルダ(Web向け)</returns>
    ''' <remarks>顔写真の保存先フォルダ(Web向け)取得</remarks>
    Public Shared ReadOnly Property GetImagePath As String
        Get
            '顔写真の保存先フォルダ(Web向け)取得
            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = sysEnv.GetSystemEnvSetting(ConstFacepicUploadurl)

            Return sysEnvRow.PARAMVALUE
        End Get
    End Property

End Class
