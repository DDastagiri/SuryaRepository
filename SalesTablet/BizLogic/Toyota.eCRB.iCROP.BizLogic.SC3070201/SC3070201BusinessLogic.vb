'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070201BusinessLogic.vb
'─────────────────────────────────────
'機能： 見積作成
'補足： 
'作成： 2011/12/01 TCS 葛西
'更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応
'更新： 2019/04/17 TS  村井 (FS)次世代タブレット新興国向けの性能評価
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3070201
Imports Toyota.eCRB.iCROP.DataAccess.SC3070201.SC3070201TableAdapter
Imports Toyota.eCRB.Estimate.Quotation
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports System.Globalization

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

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ 受注後工程利用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const C_USE_AFTER_ODR_PROC_FLG As String = "USE_AFTER_ODR_PROC_FLG"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

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

    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 START

    ''' <summary>
    ''' 基幹コード区分:店舗(2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_TYPE_BRANCH As String = "2"

    ''' <summary>
    ''' プログラム設定検索条件（プログラムコード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_PROGRAM_CD As String = "SC3070201"

    ''' <summary>
    ''' プログラム設定検索条件（設定セクション）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_SETTING_SECTION As String = "SC3070201"

    ''' <summary>
    ''' プログラム設定検索条件（設定キー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_SETTING_KEY As String = "DMS_CODE_MAP_BRN_CD"

    ''' <summary>
    ''' 使用基幹コード(基幹コード2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_MAP_DMS_CD_2 As String = "DMS_CD_2"

    ''' <summary>
    ''' 使用基幹コード(基幹コード3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_MAP_DMS_CD_3 As String = "DMS_CD_3"

    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 END

#End Region

#Region "メソッド"

    ''' <summary>
    ''' 初期表示データ取得（API使用）
    ''' </summary>
    ''' <param name="dtEstimateData">見積管理ID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>画面の初期表示データを取得する。（API使用）</remarks>
    Public Function GetEstimateInitialData(ByVal dtEstimateData As SC3070201DataSet.SC3070201ESTIMATEDATADataTable) As IC3070201DataSet

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateInitialData Start")

        ' 見積管理ID取得
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
    ''' 見積価格相談情報取得
    ''' </summary>
    ''' <param name="lngNoticeReqId">通知依頼ID (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積価格相談情報を取得する。</remarks>
    Public Function GetEstDiscountApproval(ByVal lngNoticeReqId As Long) As SC3070201DataSet.SC3070201EstDiscountApprovalDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialData Start")

        '価格相談情報取得
        Return SC3070201TableAdapter.GetAnswer(lngNoticeReqId)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialData End")
    End Function

    ''' <summary>
    ''' マネージャー回答送信前チェック
    ''' </summary>
    ''' <remarks>マネージャー回答送信前チェックを実施する。</remarks>
    Public Function GetManagerAnswerCheck(ByVal noticeReqId As Long) As SC3070201DataSet.SC3070201NoticeRequestDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetManagerAnswerCheck Start")

        '通知依頼情報テーブル検索
        Return SC3070201TableAdapter.GetAdviceStatus(noticeReqId)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetManagerAnswerCheck End")
    End Function

    ''' <summary>
    ''' CR活動結果取得
    ''' </summary>
    ''' <remarks>CR活動結果を取得する。</remarks>
    Public Function GetCRActresult(ByVal estimateId As Long) As SC3070201DataSet.SC3070201FllwUpBoxDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCRActresult Start")

        Return SC3070201TableAdapter.GetFollowupboxStatus(estimateId)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCRActresult End")
    End Function

    ''' <summary>
    ''' 契約状況取得
    ''' </summary>
    ''' <remarks>契約状況を取得する。</remarks>
    Public Function GetContract(ByVal estimateId As Long) As SC3070201DataSet.SC3070201ContractDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCRActresult Start")

        Return SC3070201TableAdapter.GetContractFlg(estimateId)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCRActresult End")
    End Function

    '2019/04/17 TS 村井 DEL (FS)次世代タブレット新興国向けの性能評価

    ''' <summary>
    ''' 見積管理ID取得
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="strcd"></param>
    ''' <param name="fllwUpBoxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEstimateId(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwUpBoxSeqNo As Decimal) As SC3070201DataSet.SC3070201EstimateIdDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId Start")
        Return SC3070201TableAdapter.GetEstimateId(fllwUpBoxSeqNo)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId Start")
    End Function

    ''' <summary>
    ''' 契約承認情報取得
    ''' </summary>
    ''' <param name="EstimateID">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetContractApproval(ByVal estimateId As Long) As SC3070201DataSet.SC3070201CONTRACTAPPROVALDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval Start")

        Dim Dt As SC3070201DataSet.SC3070201CONTRACTAPPROVALDataTable

        Dt = SC3070201TableAdapter.GetContractApproval(estimateId)

        Return Dt

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval End")

    End Function

    ''' <summary>
    ''' 注文承認ステータス更新
    ''' </summary>
    ''' <param name="estimateID">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateContractApprovalStatus(ByVal estimateId As Long, ByRef messageId As Long) As Boolean

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateContractApprovalStatus Start")

        Try
            SC3070201TableAdapter.EstimateinfoLock(estimateId)
        Catch ex As OracleExceptionEx
            Rollback = True
            messageId = 994
            Return False
        End Try

        Dim rslt As Long

        rslt = SC3070201TableAdapter.UpdateDelFlg(estimateId, StaffContext.Current.Account)

        If rslt < 1 Then
            Rollback = True
            Return False
        End If

        rslt = SC3070201TableAdapter.UpdateContractApprovalStatus(estimateId, StaffContext.Current.Account)

        If rslt < 1 Then
            Rollback = True
            Return False
        Else
            Return True
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateContractApprovalStatus End")

    End Function

    ''' <summary>
    ''' 基幹販売店情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDmsCodeMap() As SC3070201DataSet.SC3070201DMSCODEMAPDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval Start")

        '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 START

        Dim dmsCodeMap As New DmsCodeMap
        Dim drDmsCodeMap As DmsCodeMapDataSet.DMSCODEMAPRow = dmsCodeMap.GetDmsCodeMap(C_DMS_CODE_TYPE_BRANCH,
                                                                                       Trim(StaffContext.Current.DlrCD),
                                                                                       Trim(StaffContext.Current.BrnCD))
        '空のDataTableを用意
        Dim dtDmsCodeMap As New SC3070201DataSet.SC3070201DMSCODEMAPDataTable

        If Not drDmsCodeMap Is Nothing Then
            'プログラム設定より使用するDMS店舗コードを選択
            Dim programSettingV4 As New ProgramSettingV4
            Dim drProgramSettingV4 As ProgramSettingV4DataSet.PROGRAMSETTINGV4Row = _
                                      programSettingV4.GetProgramSettingV4(C_DMS_PROGRAM_SETTING_PROGRAM_CD,
                                                                           C_DMS_PROGRAM_SETTING_SETTING_SECTION,
                                                                           C_DMS_PROGRAM_SETTING_SETTING_KEY)

            Dim strBrnCd As String = String.Empty
            If Not drProgramSettingV4 Is Nothing Then
                Dim strProgramSettingV4Val As String = drProgramSettingV4.SETTING_VAL
                If C_DMS_CODE_MAP_DMS_CD_3.Equals(strProgramSettingV4Val) Then
                    strBrnCd = drDmsCodeMap.DMS_CD_3
                Else
                    strBrnCd = drDmsCodeMap.DMS_CD_2
                End If
            Else
                'プログラム設定が取得できなかった場合は基幹コード2を取得
                strBrnCd = drDmsCodeMap.DMS_CD_2
            End If

            'DMS店舗コードをDataTableへ設定
            Dim drDmsCd As SC3070201DataSet.SC3070201DMSCODEMAPRow = CType(dtDmsCodeMap.NewRow, SC3070201DataSet.SC3070201DMSCODEMAPRow)

            drDmsCd.DMS_CD_1 = drDmsCodeMap.DMS_CD_1
            drDmsCd.DMS_CD_2 = strBrnCd
            dtDmsCodeMap.Rows.Add(drDmsCd)

        End If

        Return dtDmsCodeMap

        '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 END

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval End")

    End Function

    ''' <summary>
    ''' 敬称取得
    ''' </summary>
    ''' <param name="cstKind"></param>
    ''' <param name="dlrCD"></param>
    ''' <param name="crCustID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCustNametitle(ByVal cstKind As String,
                                     ByVal dlrCD As String,
                                     ByVal crCustId As String) As SC3070201DataSet.SC3070201CustNametitleDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustNametitle Start")

        If cstKind.Equals(StrOrgCustflg) Then
            '1:自社客
            '自社客個人情報取得
            Return SC3070201TableAdapter.GetOrgCustomerNametitle(crCustId)

        ElseIf cstKind.Equals(StrNewCustflg) Then
            '2:未取引客
            '未取引客個人情報取得
            Return SC3070201TableAdapter.GetNewCustomerNametitle(crCustId)

        End If

        Return New SC3070201DataSet.SC3070201CustNametitleDataTable

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustNametitle End")
    End Function

    ''' <summary>
    ''' 敬称の設定値を取得
    ''' </summary>
    ''' <param name="sysenvDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積情報を登録する。</remarks>
    Public Function GetNameTitleSysenv(ByVal sysenvDataTbl As SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable) As SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNameTitleSysenv Start")

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

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNameTitleSysenv End")

        Return sysenvDataTbl

    End Function

    ''' <summary>
    ''' 顧客担当スタッフコード取得
    ''' </summary>
    ''' <param name="cst_id">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStaffCd(ByVal cst_id As Decimal) As String

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffCd Start")

        Dim dt As New SC3070201DataSet.SC3070201StaffCdDataTable

        dt = SC3070201TableAdapter.GetStaffCD(StaffContext.Current.DlrCD, cst_id)

        Dim account As String = Nothing

        account = CStr(dt.Rows(0).Item("SLS_PIC_STF_CD"))

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffCd End")

        Return account

    End Function

    ''' <summary>
    ''' 通知依頼取得
    ''' </summary>
    ''' <param name="noticeReqId">通知依頼ID</param>
    ''' <returns>通知依頼</returns>
    Public Function GetNoticeRequest(ByVal noticeReqId As Long) As IC3070201DataSet.IC3070201NoticeRequestRow
        Dim bizLogic As New IC3070201BusinessLogic()
        Return bizLogic.GetNoticeRequest(noticeReqId)
    End Function

    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    ''' <summary>
    ''' DMSID取得(自社客)
    ''' </summary>
    ''' <param name="originalId">自社客連番</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDmsIdOrg(ByVal originalId As String) As String

        Logger.Info("GetDmsIdOrg Start")

        Dim dt As SC3070201DataSet.SC3070201DmsIdDataTable = SC3070201TableAdapter.GetDmsIdOrg(originalId)

        If (dt.Rows.Count > 0) Then
            Return CType(dt.Item(0).CUSTCD, String)
        Else
            Return String.Empty
        End If

        Logger.Info("GetDmsIdOrg End")

    End Function

    ''' <summary>
    ''' DMSID取得(未取引客)
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="salesBkgNo">注文番号</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDmsIdNew(ByVal dlrcd As String, ByVal salesBkgNo As String) As String

        Logger.Info("GetDmsIdNew Start")

        Dim dt As SC3070201DataSet.SC3070201DmsIdDataTable = SC3070201TableAdapter.GetDmsIdNew(dlrcd, salesBkgNo)

        If (dt.Rows.Count > 0) Then
            Return CType(dt.Item(0).CUSTCD, String)
        Else
            Return String.Empty
        End If

        Logger.Info("GetDmsIdNew End")

    End Function
    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
    ''' <summary>
    ''' 受注後工程利用フラグ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <returns>受注後工程利用フラグ(0:利用しない、1:利用する)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterOdrProcFlg(ByVal dlrcd As String, ByVal brncd As String) As String
        Logger.Info("GetAfterOdrProcFlg Start")
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '①販売店≠'XXXXX'、店舗≠'XXX'（販売店コード・店舗コード該当）
        '②①実行でデータがなければ販売店≠'XXXXX'、店舗＝'XXX'販売店（販売店コードのみ該当）
        '③①②実行でデータがなければ販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし(デフォルト値)  
        Dim systemBiz As New SystemSettingDlr
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(dlrcd, brncd, C_USE_AFTER_ODR_PROC_FLG)

        'データそのものが取れなかった場合、取得した列に値が設定されていない場合はエラー
        If drSettingDlr Is Nothing Then
            Return Nothing
        End If

        Logger.Info("GetAfterOdrProcFlg End")

        Return drSettingDlr.SETTING_VAL
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
    End Function
    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
#End Region

End Class
