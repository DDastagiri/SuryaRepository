'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070205BusinessLogic.vb
'─────────────────────────────────────
'機能： 見積作成
'補足： 
'作成： 2013/11/27 TCS河原
'更新： 
'─────────────────────────────────────

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess

''' <summary>
''' SC3070205(Quotation)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
''' 

Public Class SC3070205BusinessLogic
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

    ''' <summary>
    ''' 通知タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum RequestTypeEnum
        Request
        Cancel
    End Enum

    ''' <summary>
    ''' 依頼種別（価格相談）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_PRICE As String = "02"

    ''' <summary>
    ''' ステータス（依頼）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_REQUEST As String = "1"

    ''' <summary>
    ''' ステータス（キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_CANCEL As String = "2"

    ''' <summary>
    ''' ステータス（受付）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_RECEIVE As String = "4"

    ''' <summary>
    ''' I/Fパラメータ　カテゴリータイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_PUSHCATEGORY As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_POSITION As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_TIME As Long = 3

    ''' <summary>
    ''' I/Fパラメータ　表示タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_TYPE As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_COLOR As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示時間数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_FUNCTION As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' I/Fパラメータ　アクション時関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_ACTFUNCTION As String = "icropScript.ui.openNoticeDialog()"

#End Region

#Region "メソッド"

    ''' <summary>
    ''' 初期表示データ取得（API使用）
    ''' </summary>
    ''' <param name="dtEstimateData">見積管理ID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>画面の初期表示データを取得する。（API使用）</remarks>
    Public Function GetEstimateInitialData(ByVal dtEstimateData As SC3070205DataSet.SC3070205ESTIMATEDATADataTable) As IC3070201DataSet

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateInitialData Start")

        '見積管理ID取得
        Dim lngEstimateId As Long
        lngEstimateId = CLng(dtEstimateData.Rows(0).Item("ESTIMATEID"))

        Dim bizLogicIC3070201 As IC3070201BusinessLogic
        bizLogicIC3070201 = New IC3070201BusinessLogic

        '見積情報取得I/F戻り値
        Dim dsGetEstimateDataSet As IC3070201DataSet

        '見積情報取得I/F
        dsGetEstimateDataSet = bizLogicIC3070201.GetEstimationInfo(lngEstimateId, Getallmode, 0)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateInitialData End")

        Return dsGetEstimateDataSet

    End Function

    ''' <summary>
    ''' 初期表示データ取得
    ''' </summary>
    ''' <param name="dtEstimateData">データテーブル (インプット)</param>
    ''' <param name="dsGetEstimate">データセット (インプット)</param>
    ''' <param name="lngNoticeReqId">通知依頼ID (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>画面の初期表示データを取得する。</remarks>
    Public Function GetInitialData(ByVal dtEstimateData As SC3070205DataSet.SC3070205ESTIMATEDATADataTable, _
                                   ByVal dsGetEstimate As IC3070201DataSet, _
                                   ByVal lngNoticeReqId As Long) As SC3070205DataSet

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialData Start")

        '販売店コード取得
        Dim strDlrcd As String
        strDlrcd = CStr(dtEstimateData.Rows(0).Item("DLRCD"))

        '見積情報
        Using dsEstimateExtraData As New SC3070205DataSet

            '取得データ格納用
            Dim dtOrgCustomer As SC3070205DataSet.SC3070205ORGCUSTOMERDataTable
            Dim dtNewCustomer As SC3070205DataSet.SC3070205NEWCUSTOMERDataTable
            Dim dtInsuranceComMast As SC3070205DataSet.SC3070205ESTINSUCOMMASTDataTable

            Dim dtInsuKindMast As SC3070205DataSet.SC3070205ESTINSUKINDMASTDataTable

            Dim dtFinanceComMast As SC3070205DataSet.SC3070205FINANCECOMMASTDataTable
            Dim dtModelPicture As SC3070205DataSet.SC3070205MODELPICTUREDataTable

            dsEstimateExtraData.Tables.Clear()
            If dsGetEstimate.IC3070201CustomerInfo.Rows.Count = CustomercountNew Then
                '見積り新規作成時

                '顧客情報取得
                If String.Equals(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND"), StrOrgCustflg) Then
                    '1:自社客
                    '自社客個人情報取得
                    dtOrgCustomer = SC3070205TableAdapter.GetOrgCustomer(CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")))
                    'データセットへ追加
                    dsEstimateExtraData.Tables.Add(dtOrgCustomer)

                ElseIf String.Equals(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND"), StrNewCustflg) Then
                    '2:未取引客
                    '未取引客個人情報取得
                    dtNewCustomer = SC3070205TableAdapter.GetNewCustomer(CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")))

                    'データセットへ追加
                    dsEstimateExtraData.Tables.Add(dtNewCustomer)

                End If

                '中古車情報を取得
                If dsGetEstimate.Tables.Item("IC3070201TradeincarInfo").Rows.Count = 0 AndAlso _
                    Not IsDBNull(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO")) Then
                    Dim dlrCd As String = CType(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("DLRCD"), String)
                    Dim strCd As String = CType(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("STRCD"), String)
                    Dim fuSeqNo As Long = CType(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO"), Long)
                    Dim estimateId As Long = CType(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("ESTIMATEID"), Long)

                    dsGetEstimate.Tables.Item("IC3070201TradeincarInfo").Merge(GetUcarAssessmentInfo(dlrCd, strCd, fuSeqNo, estimateId))
                End If

            End If

            '見積保険会社マスタ取得
            dtInsuranceComMast = SC3070205TableAdapter.GetEstInsuranceComMst(strDlrcd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtInsuranceComMast)

            '見積保険種別マスタ取得
            dtInsuKindMast = SC3070205TableAdapter.GetInsuKindMst(strDlrcd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtInsuKindMast)

            '融資会社マスタ取得
            dtFinanceComMast = SC3070205TableAdapter.GetFinanceComMst(strDlrcd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtFinanceComMast)

            'モデル写真取得
            Dim strModelCd As String
            Dim strColorCd As String

            strModelCd = CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELCD"))
            strColorCd = CStr(dsGetEstimate.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLORCD"))

            dtModelPicture = SC3070205TableAdapter.GetModelPicture(strModelCd, strColorCd)

            'データセットへ追加
            dsEstimateExtraData.Tables.Add(dtModelPicture)

            '通知依頼IDがセットされている場合のみ
            'If lngNoticeReqId <> 0 Then
            '    '価格相談情報取得
            '    Dim dtApprovalData As SC3070205DataSet.SC3070205EstDiscountApprovalDataTable
            '    dtApprovalData = SC3070205TableAdapter.GetAnswer(lngNoticeReqId)
            '    'データセットへ追加
            '    dsEstimateExtraData.Tables.Add(dtApprovalData)
            'End If

            Return dsEstimateExtraData

        End Using

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialData End")

    End Function

    ''' <summary>
    ''' メモ最大桁数取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>メモ最大桁数を取得する。</remarks>
    Public Function GetMemoMax() As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMemoMax Start")

        'メモ最大桁数取得戻り値
        Dim drSysEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        'ビジネスロジックオブジェクト
        Dim bizClass As SystemEnvSetting

        'ビジネスロジックオブジェクト作成
        bizClass = New SystemEnvSetting

        'メモ最大桁数取得
        drSysEnvSetting = bizClass.GetSystemEnvSetting(StrEstmemomax)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMemoMax End")

        Return drSysEnvSetting

    End Function

    ''' <summary>
    ''' 敬称の設定値を取得
    ''' </summary>
    ''' <param name="sysenvDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積情報を登録する。</remarks>
    Public Function GetNameTitleSysenv(ByVal sysenvDataTbl As SC3070205DataSet.SC3070205SYSTEMENVSETTINGDataTable) As SC3070205DataSet.SC3070205SYSTEMENVSETTINGDataTable

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNameTitleSysenv Start")

        Dim sysenvDataRow As SC3070205DataSet.SC3070205SYSTEMENVSETTINGRow

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

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNameTitleSysenv End")

        Return sysenvDataTbl

    End Function

    ''' <summary>
    ''' 見積情報保存
    ''' </summary>
    ''' <param name="dsRegEstimation">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積情報を登録する。</remarks>
    Public Function UpdateEstimation(ByVal dsRegEstimation As IC3070202DataSet) As IC3070202DataSet.IC3070202EstResultDataTable
        '    Dim bizLogicIC3070203 As IC3070203BusinessLogic
        '    bizLogicIC3070203 = New IC3070203BusinessLogic
        '
        '    '登録結果
        '    Dim dtResult As IC3070203DataSet.IC3070203EstResultDataTable
        '    '見積情報登録I/F
        '    dtResult = bizLogicIC3070203.SetEstimationInfo(dsRegEstimation, Updatedvs, Updallmode, Caroptionupddvs, 0)
        '
        '    Return dtResult

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstimation Start")

        Dim bizLogicIC3070202 As IC3070202BusinessLogic
        bizLogicIC3070202 = New IC3070202BusinessLogic

        '登録結果
        Dim dtResult As IC3070202DataSet.IC3070202EstResultDataTable
        '見積情報登録I/F
        dtResult = bizLogicIC3070202.SetEstimationInfo(dsRegEstimation, Updatedvs, Updallmode, Caroptionupddvs, 0)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstimation End")

        Return dtResult

    End Function

    ''' <summary>
    ''' 中古車査定取得情報
    ''' </summary>
    ''' <remarks>中古車査定情報を取得する。</remarks>
    Public Function GetUcarAssessmentInfo(ByVal dlrCD As String, _
                                          ByVal strCD As String, _
                                          ByVal fuSeqNo As Decimal, _
                                          ByVal estimateId As Long) As IC3070201DataSet.IC3070201TradeincarInfoDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetUcarAssessmentInfo Start")

        '中古車査定取得
        Dim dtUsedTradeInCar As SC3070205DataSet.SC3070205UCarAssessmentDataTable = SC3070205TableAdapter.GetUsedTradeinCar(fuSeqNo)

        Dim dtTradeInCar As New IC3070201DataSet.IC3070201TradeincarInfoDataTable
        Dim drTradeInCar As IC3070201DataSet.IC3070201TradeincarInfoRow

        Dim seqNo As Integer = 1

        For Each drUsedTradeInCar As SC3070205DataSet.SC3070205UCarAssessmentRow In dtUsedTradeInCar

            drTradeInCar = CType(dtTradeInCar.NewRow(), IC3070201DataSet.IC3070201TradeincarInfoRow)

            drTradeInCar.Item("ESTIMATEID") = estimateId
            drTradeInCar.Item("SEQNO") = seqNo
            drTradeInCar.Item("ASSESSMENTNO") = drUsedTradeInCar.Item("ASSESSMENTNO")
            drTradeInCar.Item("VEHICLENAME") = drUsedTradeInCar.Item("VEHICLENAME")
            drTradeInCar.Item("ASSESSEDPRICE") = drUsedTradeInCar.Item("APPRISAL_PRICE")

            dtTradeInCar.AddIC3070201TradeincarInfoRow(drTradeInCar)

            seqNo += 1
        Next

        dtUsedTradeInCar.Dispose()
        dtUsedTradeInCar = Nothing

        Return dtTradeInCar

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetUcarAssessmentInfo End")
    End Function

    ''' <summary>
    ''' CR活動結果取得
    ''' </summary>
    ''' <remarks>CR活動結果を取得する。</remarks>
    Public Function GetCRActresult(ByVal estimateId As Long) As SC3070205DataSet.SC3070205FllwUpBoxDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCRActresult Start")

        Return SC3070205TableAdapter.GetFollowupboxStatus(estimateId)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCRActresult End")
    End Function

    ' ''' <summary>
    ' ''' 見積管理ID取得
    ' ''' </summary>
    ' ''' <param name="dlrcd"></param>
    ' ''' <param name="strcd"></param>
    ' ''' <param name="fllwUpBoxSeqNo"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetEstimateId(ByVal dlrcd As String, _
    '                              ByVal strcd As String, _
    '                              ByVal fllwUpBoxSeqNo As Decimal) As SC3070205DataSet.SC3070205EstimateIdDataTable
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId Start")
    '    Return SC3070205TableAdapter.GetEstimateId(fllwUpBoxSeqNo)

    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId Start")
    'End Function

#End Region

End Class
