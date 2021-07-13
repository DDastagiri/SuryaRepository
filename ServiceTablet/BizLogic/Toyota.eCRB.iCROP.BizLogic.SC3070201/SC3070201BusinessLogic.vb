Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3070201
Imports Toyota.eCRB.iCROP.DataAccess.SC3070201.SC3070201DataSetTableAdapters
Imports Toyota.eCRB.Estimate.Quotation
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess


''' <summary>
''' SC3070201(Quotation)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
''' 

Public Class SC3070201BusinessLogic
    Inherits BaseBusinessComponent


#Region "定数定義"

    ''' <summary>
    ''' 自社客/未取引客フラグ (１：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StrOrgCustflg As String = "1"

    ''' <summary>
    ''' 自社客/未取引客フラグ (２：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StrNewCustflg As String = "2"

    ''' <summary>
    ''' 見積情報取得I／F　実行モード (０：見積の全情報を取得)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Getallmode As Integer = 0


    ''' <summary>
    ''' 見積情報登録I／F　更新区分 (１：更新)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Updatedvs As Integer = 1

    ''' <summary>
    ''' 見積情報登録I／F　実行モード (０：見積の全情報を更新)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Updallmode As Integer = 0

    ''' <summary>
    ''' 見積情報取得I／F　車両オプション更新区分 (０：車両オプションを全て更新)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Caroptionupddvs As Integer = 0


    ''' <summary>
    ''' 見積顧客情報件数（見積新規作成時)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CustomercountNew As Integer = 0

    ''' <summary>
    ''' 車両購入税比率パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StrEstvcltaxratio As String = "EST_VCLTAX_RATIO"

    ''' <summary>
    ''' メモ最大桁数パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StrEstmemomax As String = "EST_MEMO_MAX"

    ''' <summary>
    ''' 敬称位置パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StrKeisyoZengo As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 敬称デフォルト値パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StrHonorificTitle As String = "HONORIFIC_TITLE"

#End Region


#Region "メソッド"


    ''' <summary>
    ''' 初期表示データ取得（API使用）
    ''' </summary>
    ''' <param name="dtEstimateData">見積管理ID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>画面の初期表示データを取得する。（API使用）</remarks>
    Public Function GetEstimateInitialData(ByVal dtEstimateData As SC3070201DataSet.SC3070201ESTIMATEDATADataTable) As IC3070201DataSet

        ' 見積管理ID取得
        Dim lngEstimateId As Long
        lngEstimateId = CLng(dtEstimateData.Rows(0).Item("ESTIMATEID"))

        Dim bizLogicIC3070201 As IC3070201BusinessLogic
        bizLogicIC3070201 = New IC3070201BusinessLogic

        '見積情報取得I/F戻り値
        Dim dsGetEstimateDataSet As IC3070201DataSet

        '見積情報取得I/F
        dsGetEstimateDataSet = bizLogicIC3070201.GetEstimationInfo(lngEstimateId, Getallmode)


        Return dsGetEstimateDataSet

    End Function



    ''' <summary>
    ''' 初期表示データ取得
    ''' </summary>
    ''' <param name="dtEstimateData">データテーブル (インプット)</param>
    ''' <param name="dsGetEstimate">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>画面の初期表示データを取得する。</remarks>
    Public Function GetInitialData(ByVal dtEstimateData As SC3070201DataSet.SC3070201ESTIMATEDATADataTable, ByVal dsGetEstimate As IC3070201DataSet) As SC3070201DataSet

        ' 販売店コード取得
        Dim strDlrcd As String
        strDlrcd = CStr(dtEstimateData.Rows(0).Item("DLRCD"))

        '見積情報
        Using dsEstimateExtraData As New SC3070201DataSet

            '取得データ格納用
            Dim dtOrgCustomer As SC3070201DataSet.SC3070201ORGCUSTOMERDataTable
            Dim dtNewCustomer As SC3070201DataSet.SC3070201NEWCUSTOMERDataTable
            Dim dtInsuranceComMast As SC3070201DataSet.SC3070201ESTINSUCOMMASTDataTable
            Dim dtInsuKindMast As SC3070201DataSet.SC3070201ESTINSUKINDMASTDataTable
            Dim dtFinanceComMast As SC3070201DataSet.SC3070201FINANCECOMMASTDataTable
            Dim dtModelPicture As SC3070201DataSet.SC3070201MODELPICTUREDataTable
            ' $99 Ken-Suzuki Add Start
            Dim dtVclPurchaseTax As SC3070201DataSet.SC3070201VCLPURCHASETAXMASTDataTable
            ' $99 Ken-Suzuki Add End

            dsEstimateExtraData.Tables.Clear()
            If dsGetEstimate.IC3070201CustomerInfo.Rows.Count = CustomercountNew Then
                '見積り新規作成時

                '顧客情報取得

                If String.Equals(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND"), StrOrgCustflg) Then
                    '1:自社客
                    '自社客個人情報取得
                    dtOrgCustomer = SC3070201DataTableTableAdapter.GetOrgCustomer(strDlrcd, CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")))

                    'データセットへ追加
                    dsEstimateExtraData.Tables.Add(dtOrgCustomer)


                ElseIf String.Equals(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND"), StrNewCustflg) Then
                    '2:未取引客
                    '未取引客個人情報取得
                    dtNewCustomer = SC3070201DataTableTableAdapter.GetNewCustomer(CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")))

                    'データセットへ追加
                    dsEstimateExtraData.Tables.Add(dtNewCustomer)

                End If

            End If


            '見積保険会社マスタ取得
            dtInsuranceComMast = SC3070201DataTableTableAdapter.GetEstInsuranceComMst(strDlrcd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtInsuranceComMast)

            '見積保険種別マスタ取得
            dtInsuKindMast = SC3070201DataTableTableAdapter.GetInsuKindMst(strDlrcd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtInsuKindMast)

            '融資会社マスタ取得
            dtFinanceComMast = SC3070201DataTableTableAdapter.GetFinanceComMst(strDlrcd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtFinanceComMast)

            'モデル写真取得
            Dim strSeriesCd As String
            Dim strModelCd As String
            Dim strColorCd As String

            strSeriesCd = CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("SERIESCD"))
            strModelCd = CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELCD"))
            strColorCd = CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLORCD"))


            dtModelPicture = SC3070201DataTableTableAdapter.GetModelPicture(strModelCd, strColorCd)


            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtModelPicture)

            '$99 Ken-Suzuki Add Start
            ' 見積車両購入税マスタ取得
            dtVclPurchaseTax = SC3070201DataTableTableAdapter.GetVclPurchaseTaxMst(strSeriesCd, strModelCd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtVclPurchaseTax)
            '$99 Ken-Suzuki Add End

            Return dsEstimateExtraData


        End Using

    End Function

    ''' <summary>
    ''' 車両購入税比率取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>車両購入税の比率を取得する。</remarks>
    Public Function GetEstimateVehicleTaxRatio() As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        '車両購入税比率取得戻り値
        Dim drSysEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        'ビジネスロジックオブジェクト
        Dim bizClass As SystemEnvSetting

        'ビジネスロジックオブジェクト作成
        bizClass = New SystemEnvSetting

        '車両購入税比率取得
        drSysEnvSetting = bizClass.GetSystemEnvSetting(StrEstvcltaxratio)

        Return drSysEnvSetting


    End Function

    ''' <summary>
    ''' メモ最大桁数取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>メモ最大桁数を取得する。</remarks>
    Public Function GetMemoMax() As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        '車両購入税比率取得戻り値
        Dim drSysEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        'ビジネスロジックオブジェクト
        Dim bizClass As SystemEnvSetting

        'ビジネスロジックオブジェクト作成
        bizClass = New SystemEnvSetting

        '車両購入税比率取得
        drSysEnvSetting = bizClass.GetSystemEnvSetting(StrEstmemomax)

        Return drSysEnvSetting


    End Function



    ''' <summary>
    ''' 敬称の設定値を取得
    ''' </summary>
    ''' <param name="sysenvDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積情報を登録する。</remarks>
    Public Function GetNameTitleSysenv(ByVal sysenvDataTbl As SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable) As SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable
        Dim sysenvDataRow As SC3070201DataSet.SC3070201SYSTEMENVSETTINGRow

        sysenvDataRow = sysenvDataTbl.Item(0)

        Dim sys As New SystemEnvSetting
        Dim sysPosition As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(StrKeisyoZengo)
        Dim sysDefoltTitle As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(StrHonorificTitle)

        '敬称の位置を取得
        If (sysPosition Is Nothing) Then
            sysenvDataRow.NAMETITLEPOSITION = ""
        Else
            sysenvDataRow.NAMETITLEPOSITION = sysPosition.PARAMVALUE
        End If

        ''敬称のデフォルト値を取得
        If (sysDefoltTitle Is Nothing) Then
            sysenvDataRow.DEFOLTNAMETITLE = ""
        Else
            sysenvDataRow.DEFOLTNAMETITLE = sysDefoltTitle.PARAMVALUE
        End If

        Return sysenvDataTbl

    End Function


    ''' <summary>
    ''' 見積情報保存
    ''' </summary>
    ''' <param name="dsRegEstimation">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積情報を登録する。</remarks>
    Public Function UpdateEstimation(ByVal dsRegEstimation As IC3070202DataSet) As IC3070202DataSet.IC3070202EstResultDataTable

        Dim bizLogicIC3070202 As IC3070202BusinessLogic
        bizLogicIC3070202 = New IC3070202BusinessLogic


        '登録結果
        Dim dtResult As IC3070202DataSet.IC3070202EstResultDataTable

        '見積情報登録I/F
        dtResult = bizLogicIC3070202.SetEstimationInfo(dsRegEstimation, Updatedvs, Updallmode, Caroptionupddvs)

        Return dtResult

    End Function


#End Region

End Class
