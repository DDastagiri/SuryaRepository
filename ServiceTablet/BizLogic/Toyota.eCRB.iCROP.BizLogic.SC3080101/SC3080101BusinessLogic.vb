Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.DataAccess.SC3080101
Imports Toyota.eCRB.iCROP.DataAccess.SC3080101.SC3080101DataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

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

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstFacepicUploadurl As String = "FACEPIC_UPLOADURL"

    ''' <summary>
    ''' 顧客一覧取得
    ''' </summary>
    ''' <param name="serchDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客一覧取得を取得する。</remarks>
    Public Shared Function GetCustomerList(ByVal serchDataTbl As SC3080101DataSet.SC3080101SerchDataTable) As SC3080101DataSet.SC3080101CustDataTable

        Dim serchDataRow As SC3080101DataSet.SC3080101SerchRow
        serchDataRow = serchDataTbl.Item(0)

        '顧客一覧取得
        If (serchDataRow.SERCHTYPE = SC3080101DataTableTableAdapter.IdSerchTel) Then
            '電話番号で検索時
            Return SC3080101DataTableTableAdapter.GetTelSearchCustomerList(serchDataRow.DLRCD, _
                                        serchDataRow.STRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING, _
                                        serchDataRow.SORTTYPE, _
                                        serchDataRow.SORTORDER)
        Else
            '電話番号以外（名称・VIN・車両登録No）で検索時
            Return SC3080101DataTableTableAdapter.GetCustomerList(serchDataRow.DLRCD, _
                                        serchDataRow.STRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING, _
                                        serchDataRow.SORTTYPE, _
                                        serchDataRow.SORTORDER)
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
        If (serchDataRow.SERCHTYPE = SC3080101DataTableTableAdapter.IdSerchTel) Then
            '電話番号で検索時
            Return SC3080101DataTableTableAdapter.GetTelSearchCountCustomer(serchDataRow.DLRCD, _
                                        serchDataRow.STRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING)

        Else
            '電話番号以外（名称・VIN・車両登録No）で検索時
            Return SC3080101DataTableTableAdapter.GetCountCustomer(serchDataRow.DLRCD, _
                                        serchDataRow.STRCD, _
                                        serchDataRow.SERCHDIRECTION, _
                                        serchDataRow.SERCHTYPE, _
                                        serchDataRow.SERCHSTRING)
        End If

    End Function

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
                        SC3080101DataTableTableAdapter.GetOperaType(serchDataRow.DLRCD, SalesStaffId)

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
                        SC3080101DataTableTableAdapter.GetOperaType(serchDataRow.DLRCD, SericeAdviserId)

        If (tblOpera.Rows.Count > 0) Then
            Return tblOpera.Item(0).OPERATIONNAME
        Else
            Return "SA"
        End If

    End Function

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
