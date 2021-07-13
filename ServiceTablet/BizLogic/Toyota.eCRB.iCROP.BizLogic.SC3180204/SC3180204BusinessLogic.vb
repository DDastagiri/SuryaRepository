'-------------------------------------------------------------------------
'SC3180204BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：完成検査入力画面(ビジネスロジック)
'補足：
'作成：2014/02/28 AZ長代	初版作成
'更新：2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────
Option Explicit On
Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3180204.SC3180204DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3180204.SC3180204DataSetTableAdapter.SC3180204TableAdapter
Imports Toyota.eCRB.iCROP.DataAccess.SC3180204.SC3180204DataSetTableAdapter
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3180204

''' <summary>
''' チェックシートプレビュービジネスクラス
''' </summary>
''' <remarks></remarks>
Public Class SC3180204BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "規定値"

    Private Const AcceptanceTypeWalkin As String = "0"                          ' 受付区分（WalkIN）

    Private Const StallUseStatusWork As String = "02"                           ' ストール利用ステータス(作業中）

    Private Const UnsetStallId As Long = -1                                     ' ストールID（未設定時）
    Private Const UnsetJobDtlId As Long = -1                                    ' 現在のJobDtlID（未設定時）

    '2014/08/11 整理(使用していないものを削除)　START　↓↓↓
    'Private Const EventkeyCommonProces As String = "100"                        ' イベントキー（共通処理）
    'Private Const EventkeyRegistProces As String = "200"                        ' イベントキー（検査終了処理）
    ''2014/05/14 通知&PUSH処理追加　START　↓↓↓
    'Private Const EventkeyApproveProces As String = "200"                       ' イベントキー（検査承認処理）
    ''2014/05/14 通知&PUSH処理追加　END　　↑↑↑
    'Private Const EventkeyRejectProces As String = "300"                        ' イベントキー（検査否認処理）
    'Private Const EventkeyLastApproveProces As String = "201"                   ' イベントキー（最終検査承認処理）
    '2014/08/11 整理(使用していないものを削除)　END　　↑↑↑

    ''' <summary>最終チップ、且つ、Registerボタン押下時</summary>
    Private Const EventkeyLastRegistProces As String = "201"                    ' イベントキー（最終検査終了処理）


    ''' <summary>Sendボタン押下時(承認依頼)</summary>
    Private Const EventkeySendProces As String = "300"                          ' イベントキー（検査依頼処理）

    ''' <summary>最終チップ以外、且つ、Registerボタン押下時</summary>
    Private Const EventkeyFromPreview As String = "400"                         ' イベントキー（登録処理）

    Private Const NowStatusUnset As String = ""                                 ' 現在ステータス（未設定）
    Private Const NowStatusDefault As String = "0"                              ' 現在ステータス（未設定）
    Private Const NowStatusNow As String = "1"                                  ' 現在ステータス（現在）

    Private Const NowStatusApproved As String = "2"                             ' 現在ステータス（現在）

    Private Const RoStatusWork As Long = 60                                     ' RO_STATUS（作業中）
    Private Const RoStatusCompExaminationRequest As Long = 65                   ' RO_STATUS（完成検査依頼中）
    Private Const RoStatusCompExaminationComplate As Long = 70                  ' RO_STATUS（完成検査完了）
    Private Const RoStatusDeliveryWait As Long = 80                             ' RO_STATUS（納車準備待ち）
    Private Const RoStatusDeliveryWork As Long = 85                             ' RO_STATUS（納車作業中）

    Private Const RoStatusProcBeforeWork As Long = 1                            ' RO_STATUS（作業中前）
    Private Const RoStatusProcWorkToCompExaminationRequest As Long = 2          ' RO_STATUS（作業中～完成検査依頼中）
    Private Const RoStatusProcCompExaminationComplate As Long = 3               ' RO_STATUS（完成検査完了）
    Private Const RoStatusProcDeliveryWaitToDeliveryWork As Long = 4            ' RO_STATUS（納車準備待ち～納車作業中）
    Private Const RoStatusProcAfterDeliveried As Long = 5                       ' RO_STATUS（納車済み以降）

    Private Const DefaultItemCD As String = "                    "              ' ItemCD未設定値
    Private Const DefaultJobInspectId As String = "0"                           ' JobInstructID未設定値
    Private Const DefaultJobInspectSeq As Long = 0                              ' JobInstructSeq未設定値
    Private Const DefaultAlreadyReplace As Long = 0                             ' Replaced選択状態（未選択）
    Private Const DefaultAlreadyFix As Long = 0                                 ' Fixed選択状態（未選択）
    Private Const DefaultAlreadyClean As Long = 0                               ' Cleaned選択状態（未選択）
    Private Const DefaultAlreadySwap As Long = 0                                ' Swapped選択状態（未選択）
    Private Const DefaultBeforeText As Long = 0                                 ' Before入力内容（未入力値）
    Private Const DefaultAfterText As Long = 0                                  ' After入力内容（未入力値）

    Private Const InspectionUpdateSend As Long = 1                              '作業内容ステータス更新内容判定フラグ
    Private Const InspectionUpdateRegist As Long = 2                            '作業内容ステータス更新内容判定フラグ

    Private Const AllDealerCode As String = "XXXXX"                             ' 全販売店を意味するワイルドカード販売店コード
    Private Const AllBranchCode As String = "XXX"                               ' 全店舗を意味するワイルドカード店舗コード

    Private Const InspectionNeedFlgON As String = "1"                           ' 検査必要

    Private Const DefaultAlreadyReplaceInt As Integer = 0                       ' Replaced選択状態（未選択）
    Private Const InspecResltCodeReplaceInt As Integer = 3                      ' 点検結果Replaced
    Private Const DefaultPreviousReplaceMile As Decimal = -1                    ' 交換走行距離初期値
    Private Const FormatDbDateTime As String = "1900/01/01"                     ' 前回部品交換情報.前回交換日時初期値(年月日)

    Private Const ModeRegister As Integer = 0
    Private Const ModeSend As Integer = 1
    Private Const ModeAdditional As Integer = 3
    Private Const ModeSave As Integer = 4

    ''' <summary>点検項目表示順のソートキー</summary>
    Private Const SortKey_Inspec As String = "INSPEC_ITEM_DISP_SEQ, JOB_DTL_ID, DAIHYO_SEIBI DESC, JOB_INSTRUCT_ID, JOB_INSTRUCT_SEQ "

    ' ''' <summary>
    ' ''' 検査部位コード
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const PartCdEngine As String = "01"
    'Private Const PartCdInRoom As String = "02"
    'Private Const PartCdLeft As String = "03"
    'Private Const PartCdRight As String = "04"
    'Private Const PartCdUnder As String = "05"
    'Private Const PartCdTrunk As String = "06"
    Private Const PartCdNone As String = ""

    '共通関数の戻り値にて、継続する値
    '0:正常終了
    '-9000:ワーニング
    Private arySuccessList As Long() = {0, -9000}

    '2016/11/08 (TR-SVT-TMT-20160512-001) TBL_SERVICE_VISIT_MANAGEMENTにデータが無い場合はSA通知を送らない
    ''' <summary>
    '''    サービス来店者管理テーブル存在フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private isExistSvcVisitMng As Boolean = True

    '2019/06/27 TKM要件：型式対応 Start
    ''' <summary>
    ''' 型式使用フラグ取得用の[TB_M_SYSTEM_SETTING].[SETTING_NAME]値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysParaNameKatashikiUseFlg As String = "USE_FLG_KATASHIKI"

    ''' <summary>
    ''' マスタ登録状態フラグ（登録なし）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ExistFlag As String = "0"

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSystemSetting As String = "1101"
    '2019/06/27 TKM要件：型式対応 End
#End Region

#Region "メインロジック"

    ''' <summary>
    ''' ヘッダー情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>ヘッダー情報</returns>
    ''' <remarks></remarks>
    Public Function GetHederInfo(ByVal dlrCD As String, _
                                  ByVal brnCD As String, _
                                  ByVal roNum As String, _
                                  ByVal isExistActive As Boolean) As SC3180204HederInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim tableAdapter As New SC3180204TableAdapter
        Dim dtHeaderInfo As SC3180204HederInfoDataTable

        dtHeaderInfo = tableAdapter.GetDBHederInfo(dlrCD, brnCD, roNum, isExistActive)

        '2019/06/10 ジョブ名複数時対応 start
        If 1 < dtHeaderInfo.Count Then

            ' Null、ブランク、スペースのみを除いたジョブ名を取得
            Dim query = (From s In dtHeaderInfo.AsEnumerable _
                         Where Not s.IsSVC_CLASS_NAMENull() _
                         Select s.SVC_CLASS_NAME.Trim()).ToList()

            ' 重複削除（※大文字小文字違い、末尾スペース等は考慮しない）
            Dim names As IEnumerable(Of String) = query.Distinct()
            ' Null Or Empty削除
            names = (From s In names Where Not String.IsNullOrEmpty(s)).ToList()

            ' １件目を書き換える（※クライアント側で１件目のみを使用）
            dtHeaderInfo(0).SVC_CLASS_NAME = String.Join(" / ", names)

            ' クリア
            query = Nothing
            names = Nothing

        End If
        '2019/06/10 ジョブ名複数時対応 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return dtHeaderInfo

    End Function

#Region "2014/06/06 未使用になる為コメント"
    ' ''' <summary>
    ' ''' OperationItemsList(Inspection)情報取得
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="brnCD">店舗コード</param>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <param name="nowStatus">現在Stauts</param>
    ' ''' <param name="roStatus">ROStauts</param>
    ' ''' <returns>OperationItems情報</returns>
    ' ''' <remarks></remarks>
    'Public Function GetAllInspecCode(ByVal dlrCD As String, _
    '                              ByVal brnCD As String, _
    '                              ByVal roNum As String, _
    '                              ByRef nowStatus As String, _
    '                              ByRef roStatus As String, _
    '                              ByVal checkFlg As Boolean) As SC3180204InspectCodeDataTable

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim tableAdapter As New SC3180204TableAdapter
    '    Dim dtInspecCode As SC3180204InspectCodeDataTable
    '    Dim dtMainteCodeList As SC3180204MainteCodeListDataTable

    '    roStatus = ""

    '    '検索処理(チェック項目)
    '    dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, PartCdNone, checkFlg)

    '    'ROステータス取得のためにJobDtlIDを取得
    '    Dim JobDtlID As String = String.Empty
    '    If 0 < dtInspecCode.Count Then
    '        For idx = 0 To dtInspecCode.Count - 1
    '            If False = dtInspecCode(idx).IsNull("JOB_DTL_ID") Then
    '                JobDtlID = dtInspecCode(idx).JOB_DTL_ID.ToString.Trim
    '            End If
    '            '現在のステータスを保存
    '            If StallUseStatusWork = dtInspecCode(idx).STALL_USE_STATUS.ToString.Trim Then
    '                '現在
    '                If False = dtInspecCode(idx).IsNull("APROVAL_STATUS") Then
    '                    nowStatus = dtInspecCode(idx).APROVAL_STATUS.ToString.Trim
    '                End If
    '            End If
    '        Next
    '    End If
    '    ''JobDtlIDを条件にROステータスを取得
    '    If "" <> JobDtlID Then
    '        Dim dtRoState As SC3180204RoStateDataTable
    '        dtRoState = GetDBRoState(JobDtlID)
    '        If 0 < dtRoState.Count Then
    '            If False = dtRoState(0).IsNull("RO_STATUS") Then
    '                roStatus = dtRoState(0).RO_STATUS.ToString.Trim
    '            End If
    '        End If
    '    End If

    '    If "" = roStatus Then
    '        '検索処理(メンテナンス)
    '        dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum)

    '        'ROステータス取得のためにJobDtlIDを取得
    '        JobDtlID = ""
    '        If 0 < dtMainteCodeList.Count Then
    '            For idx = 0 To dtMainteCodeList.Count - 1
    '                If False = dtMainteCodeList(idx).IsNull("JOB_DTL_ID") Then
    '                    JobDtlID = dtMainteCodeList(idx).JOB_DTL_ID.ToString.Trim
    '                End If
    '                '現在のステータスを保存
    '                If StallUseStatusWork = dtMainteCodeList(idx).STALL_USE_STATUS.ToString.Trim Then
    '                    '現在
    '                    If False = dtMainteCodeList(idx).IsNull("APROVAL_STATUS") Then
    '                        nowStatus = dtMainteCodeList(idx).APROVAL_STATUS.ToString.Trim
    '                    End If
    '                End If
    '            Next
    '        End If
    '        ''JobDtlIDを条件にROステータスを取得
    '        If "" <> JobDtlID Then
    '            Dim dtRoState As SC3180204RoStateDataTable
    '            dtRoState = GetDBRoState(JobDtlID)
    '            If 0 < dtRoState.Count Then
    '                If False = dtRoState(0).IsNull("RO_STATUS") Then
    '                    roStatus = dtRoState(0).RO_STATUS.ToString.Trim
    '                End If
    '            End If
    '        End If
    '    End If

    '    '2014/05/30
    '    If roStatus = "" Then
    '        roStatus = "0"
    '    End If

    '    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '    '            , "JobDtlID=[{0}] nowStatus=[{1}] roStatus=[{2}]" _
    '    '            , JobDtlID _
    '    '            , nowStatus _
    '    '            , roStatus))

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '処理結果返却
    '    'Return dtInspecCode
    '    Return Nothing
    'End Function
#End Region

    ''' <summary>
    ''' OperationItemsList(Inspection)情報取得
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="specifyDlrCdFlgs">全販売店検索フラグセット</param>
    ''' <returns>OperationItems情報</returns>
    ''' <remarks></remarks>
    Public Function GetInspecCode(ByVal staffInfo As StaffContext, _
                                  ByVal dlrCD As String, _
                                  ByVal brnCD As String, _
                                  ByVal roNum As String, _
                                  ByVal specifyDlrCdFlgs As Dictionary(Of String, Boolean),
                                  ByRef argMergeDataTable As SC3180204InspectCodeMergeDataTable) As SC3180204InspectCodeDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180204TableAdapter
        Dim dtInspecCode As SC3180204InspectCodeDataTable
        Dim strInspecItemCD As String = String.Empty
        Dim intIdx As Integer = 0
        Dim nowStatus As String = ""

        '検索処理

        '2015/04/14 新販売店追加対応 start
        'dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, partCD, checkFlg)
        'dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, partCD)
        'dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, partCD, specifyDlrCdFlgs)
        '2019/12/02 NCN吉川 TKM要件：型式対応 Start
        '引数にisUseKatashiki追加
        dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs)

        '自販売店で値が取得できていない場合　
        If dtInspecCode.Rows.Count = 0 AndAlso specifyDlrCdFlgs("COMB_DLR_AND_BRN_EXIST") = True Then
            specifyDlrCdFlgs("COMB_DLR_AND_BRN_EXIST") = False
            dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
        End If

        '型式使用で値が取得できていない場合　
        If dtInspecCode.Rows.Count = 0 AndAlso specifyDlrCdFlgs("KATASHIKI_EXIST") = True Then
            'モデルコードで検索する
            specifyDlrCdFlgs("KATASHIKI_EXIST") = False
            dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
        End If
        '2019/12/02 NCN吉川 TKM要件：型式対応　End
        '2015/04/14 新販売店追加対応 end

        With dtInspecCode
            '表示入力制限
            intIdx = 0
            '2014/06/11 データ表示制御修正　Start
            'If 0 < .Count Then
            '    Do While .Count > intIdx
            '        If StallUseStatusWork = dtInspecCode(intIdx).STALL_USE_STATUS.ToString.Trim Then
            '            '2014/06/06 レスポンス対応と不具合対応　Start
            '            nowStatus = ""
            '            If False = dtInspecCode(intIdx).IsNull("APROVAL_STATUS") Then
            '                nowStatus = dtInspecCode(intIdx).APROVAL_STATUS.ToString.Trim
            '            End If
            '            '2014/06/06 レスポンス対応と不具合対応　End

            '            If (NowStatusUnset = nowStatus OrElse NowStatusDefault = nowStatus) And _
            '               (staffInfo.OpeCD = Operation.FM OrElse staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.SA OrElse staffInfo.OpeCD = Operation.SM) Then
            '                '入力作業途中のFM/CT,SA/SM
            '                ''削除
            '                .RemoveSC3180204InspectCodeRow(dtInspecCode(intIdx))
            '            ElseIf NowStatusNow = nowStatus And (staffInfo.OpeCD = Operation.SA OrElse staffInfo.OpeCD = Operation.SM) Then
            '                '入力作業途中のSA/SM
            '                ''削除
            '                .RemoveSC3180204InspectCodeRow(dtInspecCode(intIdx))
            '            Else
            '                intIdx += 1
            '            End If
            '        Else
            '            intIdx += 1
            '        End If
            '    Loop
            'End If
            If 0 < .Count Then
                Do While .Count > intIdx

                    nowStatus = ""

                    If dtInspecCode(intIdx).IsAPPROVAL_STATUSNull = False Then
                        nowStatus = dtInspecCode(intIdx).APPROVAL_STATUS.ToString.Trim
                    End If

                    Select Case staffInfo.OpeCD
                        Case Operation.SA, Operation.SM
                            If nowStatus <> NowStatusApproved Then
                                .RemoveSC3180204InspectCodeRow(dtInspecCode(intIdx))
                            Else
                                intIdx += 1
                            End If
                        Case Operation.FM, Operation.CT
                            If (NowStatusUnset = nowStatus OrElse NowStatusDefault = nowStatus) Then
                                .RemoveSC3180204InspectCodeRow(dtInspecCode(intIdx))
                            Else
                                intIdx += 1
                            End If
                        Case Else
                            intIdx += 1
                    End Select
                Loop
            End If
            '2014/06/11 データ表示制御修正　End

            '重複要素の削除
            intIdx = 0
            If 0 < .Count Then
                strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim()
                intIdx += 1
                Do While .Count > intIdx
                    Dim MergeDataRow As SC3180204InspectCodeMergeRow = DirectCast(argMergeDataTable.NewRow, SC3180204InspectCodeMergeRow)
                    '重複要素の削除
                    If strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim() Then

                        MergeDataRow.JOB_DTL_ID = dtInspecCode(intIdx).JOB_DTL_ID
                        MergeDataRow.RO_STATUS = dtInspecCode(intIdx).RO_STATUS
                        MergeDataRow.INSPECTION_STATUS = dtInspecCode(intIdx).INSPECTION_STATUS
                        argMergeDataTable.Rows.Add(MergeDataRow)
                        .RemoveSC3180204InspectCodeRow(dtInspecCode(intIdx))
                    Else
                        strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim()
                        intIdx += 1
                    End If
                Loop
            End If
        End With

        ''2014/08/14 ソート条件変更　Start
        Dim rtn As SC3180204InspectCodeDataTable = New SC3180204InspectCodeDataTable()

        Using dv As New DataView(dtInspecCode)
            dv.Sort = SortKey_Inspec
            rtn.Merge(dv.ToTable())

            dtInspecCode.Clear()
            dtInspecCode.Dispose()
            dtInspecCode = Nothing
        End Using
        ''2014/08/14 ソート条件変更　End

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return rtn

    End Function

    ''' <summary>
    ''' OperationCodeList(Maintenance)情報取得
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="specifyDlrCdFlgs">全販売店検索フラグセット</param>
    ''' <returns>OperationCode情報</returns>
    ''' <remarks></remarks>
    Public Function GetMainteCodeList(ByVal staffInfo As StaffContext, _
                                      ByVal dlrCD As String, _
                                      ByVal brnCD As String, _
                                      ByVal roNum As String, _
                                      ByVal specifyDlrCdFlgs As Dictionary(Of String, Boolean)) As SC3180204MainteCodeListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180204TableAdapter
        Dim dtMainteCodeList As SC3180204MainteCodeListDataTable
        Dim intIdx As Integer = 0
        Dim nowStatus As String = ""

        '検索処理
        'dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum)
        '2019/12/02 NCN 吉川　TKM要件：型式対応  Start
        dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum, specifyDlrCdFlgs)

        '型式使用で値が取得できていない場合　
        If dtMainteCodeList.Rows.Count = 0 AndAlso specifyDlrCdFlgs("KATASHIKI_EXIST") = True Then
            'モデル使用で再取得
            specifyDlrCdFlgs("KATASHIKI_EXIST") = False
            dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
        End If
        '2019/12/02 NCN 吉川　TKM要件：型式対応 End
        With dtMainteCodeList
            '現在のステータス取得できなかった場合(点検項目なし？)
            '2014/06/06 レスポンス対応と不具合対応　Start
            'If "" = nowStatus Then
            '    If 0 < .Count Then
            '        For intIdx = 0 To .Count - 1
            '            '現在のステータスを保存
            '            If StallUseStatusWork = dtMainteCodeList(intIdx).STALL_USE_STATUS.ToString.Trim Then
            '                '現在
            '                If False = dtMainteCodeList(intIdx).IsNull("APROVAL_STATUS") Then
            '                    nowStatus = dtMainteCodeList(intIdx).APROVAL_STATUS.ToString.Trim
            '                End If
            '            End If

            '        Next intIdx
            '    End If
            'End If
            '2014/06/06 レスポンス対応と不具合対応　End

            '表示入力制限
            intIdx = 0
            '2014/06/11 データ表示制御修正　Start
            'If 0 < .Count Then
            '    Do While .Count > intIdx
            '        If StallUseStatusWork = dtMainteCodeList(intIdx).STALL_USE_STATUS.ToString.Trim Then
            '            '2014/06/06 レスポンス対応と不具合対応　Start
            '            nowStatus = ""
            '            If False = dtMainteCodeList(intIdx).IsNull("APROVAL_STATUS") Then
            '                nowStatus = dtMainteCodeList(intIdx).APROVAL_STATUS.ToString.Trim
            '            End If
            '            '2014/06/06 レスポンス対応と不具合対応　End

            '            If (NowStatusUnset = nowStatus OrElse NowStatusDefault = nowStatus) And _
            '               (staffInfo.OpeCD = Operation.FM OrElse staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.SA OrElse staffInfo.OpeCD = Operation.SM) Then
            '                '入力作業途中のFM/CT,SA/SM
            '                ''削除
            '                .RemoveSC3180204MainteCodeListRow(dtMainteCodeList(intIdx))
            '            ElseIf NowStatusNow = nowStatus And (staffInfo.OpeCD = Operation.SA OrElse staffInfo.OpeCD = Operation.SM) Then
            '                '入力作業途中のSA/SM
            '                ''削除
            '                .RemoveSC3180204MainteCodeListRow(dtMainteCodeList(intIdx))
            '            Else
            '                intIdx += 1
            '            End If
            '        Else
            '            intIdx += 1
            '        End If
            '    Loop
            'End If
            If 0 < .Count Then
                Do While .Count > intIdx

                    nowStatus = ""

                    If dtMainteCodeList(intIdx).IsAPPROVAL_STATUSNull = False Then
                        nowStatus = dtMainteCodeList(intIdx).APPROVAL_STATUS.ToString.Trim
                    End If

                    Select Case staffInfo.OpeCD
                        Case Operation.SA, Operation.SM
                            If nowStatus <> NowStatusApproved Then
                                .RemoveSC3180204MainteCodeListRow(dtMainteCodeList(intIdx))
                            Else
                                intIdx += 1
                            End If
                        Case Operation.FM, Operation.CT
                            If (NowStatusUnset = nowStatus OrElse NowStatusDefault = nowStatus) Then
                                .RemoveSC3180204MainteCodeListRow(dtMainteCodeList(intIdx))
                            Else
                                intIdx += 1
                            End If
                        Case Else
                            intIdx += 1
                    End Select
                Loop
            End If
            '2014/06/11 データ表示制御修正　End

        End With


        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "nowStatus=[{0}]" _
        '            , nowStatus))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return dtMainteCodeList

    End Function

    ''' <summary>
    ''' RoStatusCheck情報判断
    ''' </summary>
    ''' <param name="roStatus">ROステータス</param>
    ''' <returns>OperationCode情報</returns>
    ''' <remarks></remarks>
    Public Function RoStatusCheck(ByVal roStatus As Long) As Long

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} START" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim lngResult As Long = 0

        '整備・点検実績データあり
        If RoStatusWork > Integer.Parse(roStatus) Then
            '整備・点検実績データなし:編集可(変更なし)
            lngResult = RoStatusProcBeforeWork
        ElseIf RoStatusWork = Integer.Parse(roStatus) OrElse RoStatusCompExaminationRequest = Integer.Parse(roStatus) Then
            '最終チップ承認前(60:作業中、65:完成検査依頼中):編集可(変更なし)
            lngResult = RoStatusProcWorkToCompExaminationRequest
        ElseIf RoStatusCompExaminationComplate = Integer.Parse(roStatus) Then
            '納車より前(70:完成検査完了まで):編集不可
            lngResult = RoStatusProcCompExaminationComplate
        ElseIf RoStatusDeliveryWait = Integer.Parse(roStatus) OrElse RoStatusDeliveryWork = Integer.Parse(roStatus) Then
            '納車より前(80:納車準備待ち、85:納車作業中):編集不可
            lngResult = RoStatusProcDeliveryWaitToDeliveryWork
        Else
            '納車より前(90:納車済み以降):編集不可
            lngResult = RoStatusProcAfterDeliveried
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "lngResult=[{0}]" _
        '            , lngResult))

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} END" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '判断結果返却
        Return lngResult

    End Function

    ''' <summary>
    ''' 完成検査結果データ登録(AdditionalJob)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlID">JobDtlID</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="vin">VIN</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function AdditionalJobLogic(ByVal dealerCD As String, _
                                           ByVal branchCD As String, _
                                           ByVal roNum As String, _
                                           ByVal jobDtlID As String, _
                                           ByVal decNowJobDtlID As Decimal, _
                                           ByVal decServiceID As Decimal, _
                                           ByVal decStallID As Decimal, _
                                           ByVal strAdviceContent As String, _
                                           ByVal dtInspecItem As SC3180204RegistInfoDataTable, _
                                           ByVal dtMaintenance As SC3180204RegistInfoDataTable, _
                                           ByVal strAccount As String, _
                                           ByVal strApplicationID As String, _
                                           ByVal aprovalReqAccount As String, _
                                           ByVal vin As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)

        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 start
        Try
            '画面データ登録
            blnResult = RegistDispData(dealerCD, _
                                       branchCD, _
                                       roNum, _
                                       strAdviceContent, _
                                       dtInspecItem, _
                                       dtMaintenance, _
                                       strAccount, _
                                       dtfUpdate, _
                                       aprovalReqAccount, _
                                       vin, _
                                       ModeAdditional)

            If False = blnResult Then
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
                Logger.Error(String.Format(CultureInfo.CurrentCulture, "Update error: result={0}", blnResult.ToString))
                Me.Rollback = True
                'Throw New ApplicationException
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end
            End If

        Catch ex As Exception
            Logger.Error(String.Format(CultureInfo.CurrentCulture, ex.Message))
            blnResult = False
            Me.Rollback = True
        End Try
        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 完成検査結果データ登録(Save)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">JobDtlID</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="vin">VIN</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SaveLogic(ByVal dealerCD As String, _
                                  ByVal branchCD As String, _
                                  ByVal roNum As String, _
                                  ByVal jobDtlId As String, _
                                  ByVal decNowJobDtlID As Decimal, _
                                  ByVal decServiceID As Decimal, _
                                  ByVal decStallID As Decimal, _
                                  ByVal strAdviceContent As String, _
                                  ByVal dtInspecItem As SC3180204RegistInfoDataTable, _
                                  ByVal dtMaintenance As SC3180204RegistInfoDataTable, _
                                  ByVal strAccount As String, _
                                  ByVal strApplicationID As String, _
                                  ByVal aprovalReqAccount As String, _
                                  ByVal vin As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)

        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 start
        Try
            '画面データ登録
            blnResult = RegistDispData(dealerCD, _
                                       branchCD, _
                                       roNum, _
                                       strAdviceContent, _
                                       dtInspecItem, _
                                       dtMaintenance, _
                                       strAccount, _
                                       dtfUpdate, _
                                       aprovalReqAccount, _
                                       vin, _
                                       ModeSave)

            If False = blnResult Then
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
                Logger.Error(String.Format(CultureInfo.CurrentCulture, "Update error: result={0}", blnResult.ToString))
                Me.Rollback = True
                'Throw New ApplicationException
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end
            End If

        Catch ex As Exception
            Logger.Error(String.Format(CultureInfo.CurrentCulture, ex.Message))
            blnResult = False
            Me.Rollback = True
        End Try
        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 end
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 完成検査結果データ登録(Register)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">JobDtlID</param>
    ''' <param name="saChipID">来店者実績連番</param>
    ''' <param name="basrezId">DMS予約ID</param>
    ''' <param name="seqNo">RO_JOB_SEQ</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="viewMode">ViewMode</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="blnFromPreviewFlg">PreviewフラグID</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks>2014/07/16　引数の順番変更</remarks>
    <EnableCommit()>
    Public Function RegisterLogic(ByVal dealerCD As String, _
                                      ByVal branchCD As String, _
                                      ByVal roNum As String, _
                                      ByVal jobDtlId As String, _
                                      ByVal viewMode As String, _
                                      ByVal vin As String, _
                                      ByVal saChipId As String, _
                                      ByVal basrezId As String, _
                                      ByVal seqNo As String, _
                                      ByRef decNowJobDtlID As Decimal, _
                                      ByVal decServiceID As Decimal, _
                                      ByRef decStallID As Decimal, _
                                      ByVal strAdviceContent As String, _
                                      ByVal dtInspecItem As SC3180204RegistInfoDataTable, _
                                      ByVal dtMaintenance As SC3180204RegistInfoDataTable, _
                                      ByVal strAccount As String, _
                                      ByVal strApplicationID As String, _
                                      ByVal blnFromPreviewFlg As Boolean, _
                                      ByVal operatorCd As Integer, _
                                      ByVal saAccountId As String, _
                                      ByVal aprovalReqAccount As String, _
                                      ByRef rtnGlobalResult As Long, _
                                      ByRef blnLastChipFlg As Boolean, _
                                      ByVal mergeDataTable As SC3180204InspectCodeMergeDataTable) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        'Dim blnResultOuter As Boolean = True
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)
        Dim strServiceINAdviceContent As String

        Dim objStaffContext As StaffContext = StaffContext.Current

        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 start
        Try
            '前ステータス取得 2014/05/08
            Dim prevStatus As String = JudgeChipStatus(decStallID)
            Dim prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable = Nothing
            prevJobStatus = JudgeJobStatus(jobDtlId)

            '画面データ登録
            blnResult = RegistDispData(dealerCD, _
                                       branchCD, _
                                       roNum, _
                                       strAdviceContent, _
                                       dtInspecItem, _
                                       dtMaintenance, _
                                       strAccount, _
                                       dtfUpdate, _
                                       aprovalReqAccount, _
                                       vin, _
                                       ModeRegister)

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                          , "blnResult [{0}]" _
            '                          , blnResult.ToString))

            ''アドバイス欄を完成検査結果データに反映
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                          , "Stall_use_id [{0}]: JOB_DTL_ID [{1}]" _
            '                          , decStallID _
            '                          , decNowJobDtlID))

            '仕様変更
            If True = blnFromPreviewFlg Then

                '処理結果返却
                Return blnResult
            End If

            If UnsetStallId = decNowJobDtlID Then
                'ストール利用ステータスが'02'の作業内容IDが取得できなかった時
                If True = IsNumeric(jobDtlId) Then
                    'セッションからJOB_DTL_IDが取得できた時 
                    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    '                          , "JOB_DTL chenge [{0}]" _
                    '                          , jobDtlId))
                    decNowJobDtlID = Decimal.Parse(jobDtlId)
                End If
            End If

            If True = blnResult Then
                strServiceINAdviceContent = strAdviceContent
                If String.Empty = strServiceINAdviceContent Then
                    strServiceINAdviceContent = " "
                End If

                blnResult = SetDBServiceINAdvice(decServiceID, _
                                                 strServiceINAdviceContent, _
                                                 strAccount, _
                                                 dtfUpdate)
            End If

            '共通関数実行結果値を初期化
            rtnGlobalResult = ActionResult.Success

            'グローバル連携
            If True = blnResult Then
                '登録処理が成功した時
                'ストール利用ステータスが'02'のデータがある場合はStallIdが'-1'以外

                If UnsetStallId <> decStallID Then

                    '1.作業終了処理
                    '2.チップステータス取得
                    '3.ステータス送信
                    '4.Job Dispath処理
                    rtnGlobalResult = Finish(dealerCD, _
                                            decServiceID, _
                                            decStallID, _
                                            strApplicationID)
                Else

                    Dim isExistMerge As Boolean = False
                    If mergeDataTable.Rows.Count > 0 Then

                        Dim mergeDataTableSelectedArray As Array = mergeDataTable.Select(String.Format("JOB_DTL_ID = {0}", jobDtlId))

                        If mergeDataTableSelectedArray.Length > 0 Then
                            'ROステータスが作業中であること
                            Dim mergeDataTableSelectedRow As SC3180204InspectCodeMergeRow = DirectCast(mergeDataTableSelectedArray(0), SC3180204InspectCodeMergeRow)
                            If mergeDataTableSelectedRow.RO_STATUS = RoStatusWork Or mergeDataTableSelectedRow.RO_STATUS = RoStatusCompExaminationRequest Then
                                isExistMerge = True
                            End If
                        End If
                    End If

                    If isExistMerge Then
                        'マージにより点検がない追加チップである場合も「作業終了」を行う
                        decStallID = GetStallUseId(decNowJobDtlID, dealerCD, branchCD)

                        rtnGlobalResult = Finish(dealerCD, _
                                                 decServiceID, _
                                                 decStallID, _
                                                 strApplicationID)

                    Else
                        If UnsetJobDtlId <> decNowJobDtlID Then
                            'JOB_DTL_IDが取得できた時 
                            'ストールIDの取得
                            decStallID = GetStallUseId(decNowJobDtlID, dealerCD, branchCD)
                            '自力基幹連携処理
                            '1.チップステータス取得
                            '2.ステータス送信
                            '3.Job Dispath処理
                            rtnGlobalResult = SelfFinish(dealerCD, _
                                                        decNowJobDtlID, _
                                                        decServiceID, _
                                                        decStallID, _
                                                        strApplicationID, _
                                                        prevStatus, _
                                                        prevJobStatus)
                        End If
                    End If
                End If

                'If True = blnFromPreviewFlg Then
                '    'Try
                '    '    NoticeProcessing(objStaffContext, _
                '    '                         saChipId, _
                '    '                         basrezId, _
                '    '                         roNum, _
                '    '                         seqNo, _
                '    '                         vin, _
                '    '                         viewMode, _
                '    '                         CStr(decNowJobDtlID), _
                '    '                         dtfUpdate, _
                '    '                         EventkeyRegistProces, _
                '    '                         "")
                '    'Catch ex As Exception
                '    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '    '                , "NoticeProcessing Exception:{0}" _
                '    '                , ex.Message))
                '    'End Try
                'End If

                '2014/07/17　最終チップフラグを参照渡しするように修正　START　↓↓↓
                '最終チップ判断
                blnLastChipFlg = GetDBChkLastChip(dealerCD, branchCD, roNum, decNowJobDtlID)
                '2014/07/17　最終チップフラグを参照渡しするように修正　END　　↑↑↑

                '    '通知処理
                '    If True = GetDBChkLastChip(dealerCD, _
                '                 branchCD, _
                '                 roNum, _
                '                 decNowJobDtlID) Then
                '        Try
                '            '2014/06/03 通知処理を承認入力から移植　Start
                '            'NoticeProcessing(objStaffContext, _
                '            '                 saChipId, _
                '            '                 basrezId, _
                '            '                 roNum, _
                '            '                 seqNo, _
                '            '                 vin, _
                '            '                 viewMode, _
                '            '                 CStr(decNowJobDtlID), _
                '            '                 dtfUpdate, _
                '            '                 EventkeyLastRegistProces, _
                '            '                  "", _
                '            '                 saAccountId, _
                '            '                 OperetionMode.Registration)
                '            NoticeProcessing(objStaffContext, _
                '                              saChipId, _
                '                              basrezId, _
                '                              roNum, _
                '                              seqNo, _
                '                              vin, _
                '                              viewMode, _
                '                              CStr(decNowJobDtlID), _
                '                              dtfUpdate, _
                '                              EventkeyLastRegistProces, _
                '                              aprovalReqAccount)
                '            '2014/06/03 通知処理を承認入力から移植　End
                '        Catch ex As Exception
                '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                        , "NoticeProcessing Exception:{0}" _
                '                        , ex.Message))
                '        End Try
                '    Else

                '        If False = blnFromPreviewFlg Then
                '            Try
                '                NoticeProcessing(objStaffContext, _
                '                               saChipId, _
                '                               basrezId, _
                '                               roNum, _
                '                               seqNo, _
                '                               vin, _
                '                               viewMode, _
                '                               CStr(decNowJobDtlID), _
                '                               dtfUpdate, _
                '                               EventkeyFromPreview, "")
                '            Catch ex As Exception
                '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                            , "NoticeProcessing Exception:{0}" _
                '                            , ex.Message))
                '            End Try
                '        End If

                '    End If


            End If

            If UnsetJobDtlId <> decNowJobDtlID Then
                ''JOB_DTL_IDが取得できた時
                'If True = blnResult Then

                ' 2015/5/1 強制納車対応  start
                'JOB_DTL_IDが取得できた時、且つ、エラーが発生していない場合
                If True = blnResult And _
                   arySuccessList.Contains(rtnGlobalResult) Then

                    '作業内容の完成検査ステータスを変更する(2:承認済にする処理)
                    blnResult = InspectionUpdate(InspectionUpdateRegist, _
                                                 dealerCD, _
                                                 decNowJobDtlID, _
                                                 decServiceID, _
                                                 "", _
                                                 strAccount, _
                                                 dtfUpdate)

                    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    '                          , "blnResult [{0}]" _
                    '                          , blnResult.ToString))
                End If
            End If

            'If False = blnResult Then
            '    Throw New ApplicationException
            'End If
            'エラーが発生した場合、ロールバックを実行して戻り値をFalse(エラー)に設定する
            If Not blnResult OrElse _
               Not arySuccessList.Contains(rtnGlobalResult) Then
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
                Logger.Error(String.Format(CultureInfo.CurrentCulture, "Update error: result={0}", blnResult.ToString))
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end

                Me.Rollback = True
                blnResult = False

            End If
            ' 2015/5/1 強制納車対応  end
        Catch ex As Exception
            Logger.Error(String.Format(CultureInfo.CurrentCulture, ex.Message))
            blnResult = False
            Me.Rollback = True
        End Try
        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    Public Function NoticeAfterRegisterLogic(ByVal dealerCD As String, _
                                  ByVal branchCD As String, _
                                  ByVal roNum As String, _
                                  ByVal jobDtlId As String, _
                                  ByVal viewMode As String, _
                                  ByVal vin As String, _
                                  ByVal saChipId As String, _
                                  ByVal basrezId As String, _
                                  ByVal seqNo As String, _
                                  ByRef decNowJobDtlID As Decimal, _
                                  ByVal decServiceID As Decimal, _
                                  ByRef decStallID As Decimal, _
                                  ByVal strAdviceContent As String, _
                                  ByVal dtInspecItem As SC3180204RegistInfoDataTable, _
                                  ByVal dtMaintenance As SC3180204RegistInfoDataTable, _
                                  ByVal strAccount As String, _
                                  ByVal strApplicationID As String, _
                                  ByVal blnFromPreviewFlg As Boolean, _
                                  ByVal operatorCd As Integer, _
                                  ByVal saAccountId As String, _
                                  ByVal aprovalReqAccount As String, _
                                  ByRef blnLastChipFlg As Boolean) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim objStaffContext As StaffContext = StaffContext.Current
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)
        Dim blnResult As Boolean = True

        '通知処理
        '2014/07/17　最終チップフラグを参照渡しするように修正　START　↓↓↓
        'If True = GetDBChkLastChip(dealerCD, _
        '                 branchCD, _
        '                 roNum, _
        '                 decNowJobDtlID) Then
        If True = blnLastChipFlg Then
            '2014/07/17　最終チップフラグを参照渡しするように修正　END　↑↑↑
            Try
                NoticeProcessing(objStaffContext, _
                                      saChipId, _
                                      basrezId, _
                                      roNum, _
                                      seqNo, _
                                      vin, _
                                      viewMode, _
                                      CStr(decNowJobDtlID), _
                                      dtfUpdate, _
                                      EventkeyLastRegistProces, _
                                      aprovalReqAccount)
                '2014/06/03 通知処理を承認入力から移植　End
            Catch ex As Exception
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "NoticeProcessing Exception:{0}" _
                            , ex.Message))
            End Try
        Else

            If False = blnFromPreviewFlg Then
                Try
                    NoticeProcessing(objStaffContext, _
                                   saChipId, _
                                   basrezId, _
                                   roNum, _
                                   seqNo, _
                                   vin, _
                                   viewMode, _
                                   CStr(decNowJobDtlID), _
                                   dtfUpdate, _
                                   EventkeyFromPreview, "")
                Catch ex As Exception
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                , "NoticeProcessing Exception:{0}" _
                                , ex.Message))
                End Try
            End If

        End If


        If UnsetJobDtlId <> decNowJobDtlID Then
            'JOB_DTL_IDが取得できた時
            '作業内容の完成検査ステータスを変更する(2:承認済にする処理)
            blnResult = InspectionUpdate(InspectionUpdateRegist, _
                                         dealerCD, _
                                         decNowJobDtlID, _
                                         decServiceID, _
                                         "", _
                                         strAccount, _
                                         dtfUpdate)

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                          , "blnResult [{0}]" _
            '                          , blnResult.ToString))
        End If

        ' 2019/1/25 ISSUE_0167対応 DEL start
        'If False = blnResult Then
        '    Throw New ApplicationException
        'End If
        ' 2019/1/25 ISSUE_0167対応 DEL end
        
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 完成検査結果データ登録(Send)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">JobDtlID</param>
    ''' <param name="viewMode">ViewMode</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="saChipId">来店者実績連番</param>
    ''' <param name="basrezId">DMS予約ID</param>
    ''' <param name="seqNo">RO_JOB_SEQ</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="strSendUser">送信先ユーザー</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SendLogic(ByVal dealerCD As String, _
                                  ByVal branchCD As String, _
                                  ByVal roNum As String, _
                                  ByVal jobDtlId As String, _
                                  ByVal viewMode As String, _
                                  ByVal vin As String, _
                                  ByVal saChipId As String, _
                                  ByVal basrezId As String, _
                                  ByVal seqNo As String, _
                                  ByRef decNowJobDtlID As Decimal, _
                                  ByVal decServiceID As Decimal, _
                                  ByRef decStallID As Decimal, _
                                  ByVal strAdviceContent As String, _
                                  ByVal dtInspecItem As SC3180204RegistInfoDataTable, _
                                  ByVal dtMaintenance As SC3180204RegistInfoDataTable, _
                                  ByVal strAccount As String, _
                                  ByVal strApplicationID As String, _
                                  ByVal strSendUser As String, _
                                  ByVal operatorCd As Integer, _
                                  ByVal aprovalReqAccount As String, _
                                  ByRef rtnGlobalResult As Long, _
                                  ByVal mergeDataTable As SC3180204InspectCodeMergeDataTable) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        'Dim blnResultOuter As Boolean = True
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)
        Dim strServiceINAdviceContent As String

        Dim objStaffContext As StaffContext = StaffContext.Current

        '前ステータス取得 2014/05/08
        Dim prevStatus As String = JudgeChipStatus(decStallID)
        Dim prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable = Nothing
        prevJobStatus = JudgeJobStatus(jobDtlId)

        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 start
        Try
            '画面データ登録
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "画面データ登録"))
            blnResult = RegistDispData(dealerCD, _
                                       branchCD, _
                                       roNum, _
                                       strAdviceContent, _
                                       dtInspecItem, _
                                       dtMaintenance, _
                                       strAccount, _
                                       dtfUpdate, _
                                       aprovalReqAccount, _
                                       vin, _
                                       ModeSend)

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                          , "blnResult [{0}]" _
            '                          , blnResult.ToString))

            'アドバイス欄を完成検査結果データに反映

            '承認者選択が必要な場合は承認者選択画面をポップアップ表示する

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                          , "ServiceinID[{0}]: Stall_use_id [{1}]: JOB_DTL_ID [{2}]" _
            '                          , decServiceID _
            '                          , decStallID _
            '                          , decNowJobDtlID))

            If UnsetStallId = decNowJobDtlID Then
                'ストール利用ステータスが'02'の作業内容IDが取得できなかった時
                If True = IsNumeric(jobDtlId) Then
                    'セッションからJOB_DTL_IDが取得できた時
                    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    '                          , "JOB_DTL chenge [{0}]" _
                    '                          , jobDtlId))
                    decNowJobDtlID = Decimal.Parse(jobDtlId)
                End If
            End If

            If True = blnResult Then
                strServiceINAdviceContent = strAdviceContent
                If String.Empty = strServiceINAdviceContent Then
                    strServiceINAdviceContent = " "
                End If

                blnResult = SetDBServiceINAdvice(decServiceID, _
                                                 strServiceINAdviceContent, _
                                                 strAccount, _
                                                 dtfUpdate)
            End If

            '共通関数実行結果値を初期化
            rtnGlobalResult = ActionResult.Success

            'グローバル連携
            If True = blnResult Then
                '登録処理が成功した時
                'ストール利用ステータスが'02'のデータがある場合はStallIdが'-1'以外
                If UnsetStallId <> decStallID Then
                    '1.作業終了処理
                    '2.チップステータス取得
                    '3.ステータス送信
                    '4.Job Dispath処理
                    rtnGlobalResult = Finish(dealerCD, _
                                            decServiceID, _
                                            decStallID, _
                                            strApplicationID)

                Else
                    Dim isExistMerge As Boolean = False
                    If mergeDataTable.Rows.Count > 0 Then

                        Dim mergeDataTableSelectedArray As Array = mergeDataTable.Select(String.Format("JOB_DTL_ID = {0}", jobDtlId))

                        If mergeDataTableSelectedArray.Length > 0 Then
                            'ROステータスが作業中であること
                            Dim mergeDataTableSelectedRow As SC3180204InspectCodeMergeRow = DirectCast(mergeDataTableSelectedArray(0), SC3180204InspectCodeMergeRow)
                            If mergeDataTableSelectedRow.RO_STATUS = RoStatusWork Or mergeDataTableSelectedRow.RO_STATUS = RoStatusCompExaminationRequest Then
                                isExistMerge = True
                            End If
                        End If
                    End If

                    If isExistMerge Then
                        'マージにより点検がない追加チップである場合も「作業終了」を行う
                        decStallID = GetStallUseId(decNowJobDtlID, dealerCD, branchCD)

                        rtnGlobalResult = Finish(dealerCD, _
                                                 decServiceID, _
                                                 decStallID, _
                                                 strApplicationID)

                    Else
                        If UnsetJobDtlId <> decNowJobDtlID Then

                            'JOB_DTL_IDが取得できた時 
                            'ストールIDの取得
                            decStallID = GetStallUseId(decNowJobDtlID, dealerCD, branchCD)
                            '自力基幹連携処理
                            '1.チップステータス取得
                            '2.ステータス送信
                            '3.Job Dispath処理
                            rtnGlobalResult = SelfFinish(dealerCD, _
                                                        decNowJobDtlID, _
                                                        decServiceID, _
                                                        decStallID, _
                                                        strApplicationID, _
                                                        prevStatus, _
                                                        prevJobStatus)
                        End If
                    End If
                End If

                '    If True = blnResultOuter Then
                '        If UnsetJobDtlId <> decNowJobDtlID Then
                '            'JOB_DTL_IDが取得できた時
                '            '通知処理
                '            Try
                '                 NoticeProcessing(objStaffContext, _
                '                                 saChipId, _
                '                                 basrezId, _
                '                                 roNum, _
                '                                 seqNo, _
                '                                 vin, _
                '                                 viewMode, _
                '                                 CStr(decNowJobDtlID), _
                '                                 dtfUpdate, _
                '                                 EventkeySendProces, _
                '                                 aprovalReqAccount)
                '                '2014/06/03 通知処理を承認入力から移植　End
                '            Catch ex As Exception
                '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                            , "NoticeProcessing Exception:{0}" _
                '                            , ex.Message))
                '            End Try

                '            ''Try
                '            ''    '2014/06/03 通知処理を承認入力から移植　Start
                '            ''    'NoticeProcessing(objStaffContext, _
                '            ''    '                 saChipId, _
                '            ''    '                 basrezId, _
                '            ''    '                 roNum, _
                '            ''    '                 seqNo, _
                '            ''    '                 vin, _
                '            ''    '                 viewMode, _
                '            ''    '                 CStr(decNowJobDtlID), _
                '            ''    '                 dtfUpdate, _
                '            ''    '                 EventkeyCommonProces, _
                '            ''    '                 "", _
                '            ''    '                 String.Empty, _
                '            ''    '                 OperetionMode.ApprovalRequest)
                '            ''    NoticeProcessing(objStaffContext, _
                '            ''                     saChipId, _
                '            ''                     basrezId, _
                '            ''                     roNum, _
                '            ''                     seqNo, _
                '            ''                     vin, _
                '            ''                     viewMode, _
                '            ''                     CStr(decNowJobDtlID), _
                '            ''                     dtfUpdate, _
                '            ''                     EventkeyCommonProces)
                '            ''    '2014/06/03 通知処理を承認入力から移植　End
                '            ''Catch ex As Exception
                '            ''    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '            ''                , "NoticeProcessing Exception:{0}" _
                '            ''                , ex.Message))
                '            ''End Try
                '        End If
                '    End If
            End If

            If UnsetJobDtlId <> decNowJobDtlID Then
                ''JOB_DTL_IDが取得できた時
                'If True = blnResult Then

                ' 2015/5/1 強制納車対応  start
                'JOB_DTL_IDが取得できた時、且つ、エラーが発生していない場合
                If True = blnResult And _
                   arySuccessList.Contains(rtnGlobalResult) Then

                    '作業内容の完成検査ステータスを変更する(1:承認依頼にする処理)
                    blnResult = InspectionUpdate(InspectionUpdateSend, _
                                                 dealerCD, _
                                                 decNowJobDtlID, _
                                                 decServiceID, _
                                                 strSendUser, _
                                                 strAccount, _
                                                 dtfUpdate)

                    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    '                          , "blnResult [{0}]" _
                    '                          , blnResult.ToString))
                End If
            End If

            'If False = blnResult Then
            '    Throw New ApplicationException
            'End If
            'エラーが発生した場合、ロールバックを実行して戻り値をFalse(エラー)に設定する
            If Not blnResult OrElse _
               Not arySuccessList.Contains(rtnGlobalResult) Then
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
                Logger.Error(String.Format(CultureInfo.CurrentCulture, "Update error: result={0}", blnResult.ToString))
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end

                Me.Rollback = True
                blnResult = False

            End If
            ' 2015/5/1 強制納車対応  end
        Catch ex As Exception
            Logger.Error(String.Format(CultureInfo.CurrentCulture, ex.Message))
            blnResult = False
            Me.Rollback = True
        End Try
        'TMT2販社 BTS310 新規登録時の例外処理追加 2015/04/06 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    Public Function NoticeAfterSendLogic(ByVal dealerCD As String, _
                              ByVal branchCD As String, _
                              ByVal roNum As String, _
                              ByVal jobDtlId As String, _
                              ByVal viewMode As String, _
                              ByVal vin As String, _
                              ByVal saChipId As String, _
                              ByVal basrezId As String, _
                              ByVal seqNo As String, _
                              ByRef decNowJobDtlID As Decimal, _
                              ByVal decServiceID As Decimal, _
                              ByRef decStallID As Decimal, _
                              ByVal strAdviceContent As String, _
                              ByVal dtInspecItem As SC3180204RegistInfoDataTable, _
                              ByVal dtMaintenance As SC3180204RegistInfoDataTable, _
                              ByVal strAccount As String, _
                              ByVal strApplicationID As String, _
                              ByVal strSendUser As String, _
                              ByVal operatorCd As Integer, _
                              ByVal aprovalReqAccount As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)

        Dim objStaffContext As StaffContext = StaffContext.Current
        Dim blnResultOuter As Boolean = True


        '画面データ登録

        If True = blnResultOuter Then
            If UnsetJobDtlId <> decNowJobDtlID Then
                'JOB_DTL_IDが取得できた時
                '通知処理
                Try
                    NoticeProcessing(objStaffContext, _
                                    saChipId, _
                                    basrezId, _
                                    roNum, _
                                    seqNo, _
                                    vin, _
                                    viewMode, _
                                    CStr(decNowJobDtlID), _
                                    dtfUpdate, _
                                    EventkeySendProces, _
                                    aprovalReqAccount)
                    '2014/06/03 通知処理を承認入力から移植　End
                Catch ex As Exception
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                , "NoticeProcessing Exception:{0}" _
                                , ex.Message))
                End Try

                ''Try
                ''    '2014/06/03 通知処理を承認入力から移植　Start
                ''    'NoticeProcessing(objStaffContext, _
                ''    '                 saChipId, _
                ''    '                 basrezId, _
                ''    '                 roNum, _
                ''    '                 seqNo, _
                ''    '                 vin, _
                ''    '                 viewMode, _
                ''    '                 CStr(decNowJobDtlID), _
                ''    '                 dtfUpdate, _
                ''    '                 EventkeyCommonProces, _
                ''    '                 "", _
                ''    '                 String.Empty, _
                ''    '                 OperetionMode.ApprovalRequest)
                ''    NoticeProcessing(objStaffContext, _
                ''                     saChipId, _
                ''                     basrezId, _
                ''                     roNum, _
                ''                     seqNo, _
                ''                     vin, _
                ''                     viewMode, _
                ''                     CStr(decNowJobDtlID), _
                ''                     dtfUpdate, _
                ''                     EventkeyCommonProces)
                ''    '2014/06/03 通知処理を承認入力から移植　End
                ''Catch ex As Exception
                ''    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                ''                , "NoticeProcessing Exception:{0}" _
                ''                , ex.Message))
                ''End Try
            End If
        End If

        If UnsetJobDtlId <> decNowJobDtlID Then
            'JOB_DTL_IDが取得できた時
            If True = blnResult Then
                '作業内容の完成検査ステータスを変更する(1:承認依頼にする処理)
                blnResult = InspectionUpdate(InspectionUpdateSend, _
                                             dealerCD, _
                                             decNowJobDtlID, _
                                             decServiceID, _
                                             strSendUser, _
                                             strAccount, _
                                             dtfUpdate)

                'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                          , "blnResult [{0}]" _
                '                          , blnResult.ToString))
            End If
        End If

        ' 2019/1/25 ISSUE_0167対応 DEL start
        'If False = blnResult Then
        '    Throw New ApplicationException
        'End If
        ' 2019/1/25 ISSUE_0167対応 DEL end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 画面データ登録
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="ButtonMode">ボタン種類</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function RegistDispData(ByVal dealerCD As String, _
                                   ByVal branchCD As String, _
                                   ByVal roNum As String, _
                                   ByVal strAdviceContent As String, _
                                   ByVal dtInspecItem As SC3180204RegistInfoDataTable, _
                                   ByVal dtMaintenance As SC3180204RegistInfoDataTable, _
                                   ByVal strAccount As String, _
                                   ByVal dtfUpdate As Date, _
                                   ByVal aprovalReqAccount As String, _
                                   ByVal vin As String, _
                                   ByVal ButtonMode As Integer) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim intDataIndex As Integer = 0
        Dim intListIndex As Integer = 0
        Dim dicJobDtlID As New Dictionary(Of String, Long)()

        '登録済みJobDtlIDリストの初期化
        dicJobDtlID.Clear()

        ' 検査必要フラグ取得(前回交換情報で使用)
        Dim inspectionNeedFlg As String = String.Empty
        If dtInspecItem.Count > 0 Then
            Dim tableAdapter As New SC3180204TableAdapter
            inspectionNeedFlg = tableAdapter.GetInspectionNeedFlg(dtInspecItem(0).JobDtlID)
        End If

        '前回部品交換情報リスト取得
        Dim tableAdapter1 As New SC3180204TableAdapter
        Dim prePartsReplaceDt As SC3180204PreviousPartsReplaceDataTable = New SC3180204PreviousPartsReplaceDataTable()
        prePartsReplaceDt = tableAdapter1.GetPreviousPartsReplace(vin)

        '部品交換情報差異比較用
        Dim editPartsReplaceArray As List(Of String) = New List(Of String)

        'InspectionItem
        For intDataIndex = 0 To dtInspecItem.Count - 1

            '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
            Dim blnRegisted As Boolean = False
            If True = dicJobDtlID.ContainsKey(dtInspecItem(intDataIndex).JobDtlID) Then
                '存在している(登録対象としない)
                blnRegisted = True
            Else
                '存在していない(登録対象とする)
                dicJobDtlID.Add(dtInspecItem(intDataIndex).JobDtlID, dtInspecItem(intDataIndex).RowLockVer)
            End If

            If False = blnRegisted And True = blnResult Then
                '未登録・未更新
                blnResult = SetDBCmpChkReslut(dealerCD, _
                                              branchCD, _
                                              dtInspecItem(intDataIndex).JobDtlID, _
                                              roNum, _
                                              dtInspecItem(intDataIndex).AprovalStatus, _
                                              strAdviceContent, _
                                              strAccount, _
                                              dtfUpdate, _
                                              dtInspecItem(intDataIndex).RowLockVer, _
                                              aprovalReqAccount)
            End If

            '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
            If True = blnResult Then
                '2014/06/02 Edit svcCdを引数に追加 Start
                'blnResult = SetDBCmpChkResultDetail(dealerCD, _
                '                                    branchCD, _
                '                                    dtInspecItem(intDataIndex).JobDtlID, _
                '                                    DefaultJobInspectId, _
                '                                    DefaultJobInspectSeq, _
                '                                    dtInspecItem(intDataIndex).ItemCD, _
                '                                    dtInspecItem(intDataIndex).ItemsCheck, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Replaced, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Fixed, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Cleaned, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Swapped, _
                '                                    dtInspecItem(intDataIndex).ItemsTextBefore, _
                '                                    dtInspecItem(intDataIndex).ItemsTextAfter, _
                '                                    strAccount, _
                '                                    dtfUpdate)
                '2014/06/02 Edit svcCdを引数に追加 End

                '2014/06/06 '2014/06/06 登録不具合のため引数変更　Start　Start
                'blnResult = SetDBCmpChkResultDetail(dealerCD, _
                '                                     branchCD, _
                '                                     dtInspecItem(intDataIndex).JobDtlID, _
                '                                     DefaultJobInspectId, _
                '                                     DefaultJobInspectSeq, _
                '                                     dtInspecItem(intDataIndex).ItemCD, _
                '                                     dtInspecItem(intDataIndex).SVC_CD, _
                '                                     dtInspecItem(intDataIndex).ItemsCheck, _
                '                                     dtInspecItem(intDataIndex).ItemsSelect_Replaced, _
                '                                     dtInspecItem(intDataIndex).ItemsSelect_Fixed, _
                '                                     dtInspecItem(intDataIndex).ItemsSelect_Cleaned, _
                '                                     dtInspecItem(intDataIndex).ItemsSelect_Swapped, _
                '                                     dtInspecItem(intDataIndex).ItemsTextBefore, _
                '                                     dtInspecItem(intDataIndex).ItemsTextAfter, _
                '                                     strAccount, _
                '                                     dtfUpdate)

                blnResult = SetDBCmpChkResultDetail(dealerCD, _
                                                    branchCD, _
                                                    dtInspecItem(intDataIndex).JobDtlID, _
                                                    dtInspecItem(intDataIndex).JobInstructID, _
                                                    dtInspecItem(intDataIndex).JobInstructSeq, _
                                                    dtInspecItem(intDataIndex).ItemCD, _
                                                    dtInspecItem(intDataIndex).SVC_CD, _
                                                    dtInspecItem(intDataIndex).ItemsCheck, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Replaced, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Fixed, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Cleaned, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Swapped, _
                                                    dtInspecItem(intDataIndex).ItemsTextBefore, _
                                                    dtInspecItem(intDataIndex).ItemsTextAfter, _
                                                    strAccount, _
                                                    dtfUpdate)
                '2014/06/06 '2014/06/06 登録不具合のため引数変更　Start　End

                ' 2017/2/17 ライフサイクル対応 前回部品交換情報を登録 Start
                If dtInspecItem(intDataIndex).ItemsSelect_Replaced <> DefaultAlreadyReplaceInt _
                    Or dtInspecItem(intDataIndex).ItemsCheck = InspecResltCodeReplaceInt Then

                    ' 差異比較の配列に追加
                    editPartsReplaceArray.Add(dtInspecItem(intDataIndex).ItemCD)

                    If blnResult Then
                        blnResult = SetPreviousPartsReplace(vin,
                                                dtInspecItem(intDataIndex).ItemCD, _
                                                dealerCD, _
                                                branchCD, _
                                                roNum, _
                                                inspectionNeedFlg, _
                                                dtfUpdate, _
                                                strAccount, _
                                                prePartsReplaceDt, _
                                                dtInspecItem(intDataIndex).JobDtlID, _
                                                ButtonMode)
                    End If
                End If
                ' 2017/2/17 ライフサイクル対応 前回部品交換情報を登録 End
            End If

            If False = blnResult Then
                'エラー発生のため終了
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
                Logger.Error(String.Format(CultureInfo.CurrentCulture, "Update error: result={0}", blnResult.ToString))
                'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end
                Exit For
            End If
        Next

        ' 2017/2/17 ライフサイクル対応 前回部品交換情報を削除・更新 Start
        If blnResult Then
            If dtInspecItem.Count > 0 Then
                blnResult = NotReplacePreviousParts(vin, roNum, editPartsReplaceArray, prePartsReplaceDt, strAccount, dtfUpdate)
            End If
        End If
        ' 2017/2/17 ライフサイクル対応 前回部品交換情報を削除・更新 End

        'Maintenance
        If True = blnResult Then
            For intDataIndex = 0 To dtMaintenance.Count - 1

                '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
                Dim blnRegisted As Boolean = False
                If True = dicJobDtlID.ContainsKey(dtMaintenance(intDataIndex).JobDtlID) Then
                    '存在している(登録対象としない)
                    blnRegisted = True
                Else
                    '存在していない(登録対象とする)
                    dicJobDtlID.Add(dtMaintenance(intDataIndex).JobDtlID, dtMaintenance(intDataIndex).RowLockVer)
                End If

                If False = blnRegisted And True = blnResult Then
                    blnResult = SetDBCmpChkReslut(dealerCD, _
                                                  branchCD, _
                                                  dtMaintenance(intDataIndex).JobDtlID, _
                                                  roNum, _
                                                  dtMaintenance(intDataIndex).AprovalStatus, _
                                                  strAdviceContent, _
                                                  strAccount, _
                                                  dtfUpdate, _
                                                  dtMaintenance(intDataIndex).RowLockVer, _
                                                  aprovalReqAccount)
                End If

                '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
                If True = blnResult Then
                    '2014/06/02 Edit svcCdを引数に追加 Start
                    'blnResult = SetDBCmpChkResultDetail(dealerCD, _
                    '                                    branchCD, _
                    '                                    dtMaintenance(intDataIndex).JobDtlID, _
                    '                                    dtMaintenance(intDataIndex).JobInstructID, _
                    '                                    dtMaintenance(intDataIndex).JobInstructSeq, _
                    '                                    DefaultItemCD, _
                    '                                    dtMaintenance(intDataIndex).ItemsCheck, _
                    '                                    DefaultAlreadyReplace, _
                    '                                    DefaultAlreadyFix, _
                    '                                    DefaultAlreadyClean, _
                    '                                    DefaultAlreadySwap, _
                    '                                    DefaultBeforeText, _
                    '                                    DefaultAfterText, _
                    '                                    strAccount, _
                    '                                    dtfUpdate)
                    blnResult = SetDBCmpChkResultDetail(dealerCD, _
                                                         branchCD, _
                                                         dtMaintenance(intDataIndex).JobDtlID, _
                                                         dtMaintenance(intDataIndex).JobInstructID, _
                                                         dtMaintenance(intDataIndex).JobInstructSeq, _
                                                         DefaultItemCD, _
                                                         String.Empty, _
                                                         dtMaintenance(intDataIndex).ItemsCheck, _
                                                         DefaultAlreadyReplace, _
                                                         DefaultAlreadyFix, _
                                                         DefaultAlreadyClean, _
                                                         DefaultAlreadySwap, _
                                                         DefaultBeforeText, _
                                                         DefaultAfterText, _
                                                         strAccount, _
                                                         dtfUpdate)
                    '2014/06/02 Edit svcCdを引数に追加 End

                End If

                If False = blnResult Then
                    'エラー発生のため終了
                    'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
                    Logger.Error(String.Format(CultureInfo.CurrentCulture, "Update error: result={0}", blnResult.ToString))
                    'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end
                    Exit For
                End If
            Next
        End If

        If True = blnResult Then

            For intListIndex = 0 To dicJobDtlID.Count - 1
                '行ロックバージョン更新処理
                If True = blnResult And -1 < dicJobDtlID.Values(intListIndex) Then
                    '行ロックバージョン更新
                    blnResult = SetDBInspectionLockUpt(dealerCD, _
                                                       branchCD, _
                                                       dicJobDtlID.Keys(intListIndex), _
                                                       dicJobDtlID.Values(intListIndex))
                End If

                If False = blnResult Then
                    'エラー発生のため終了
                    'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
                    Logger.Error(String.Format(CultureInfo.CurrentCulture, "Update error: result={0}", blnResult.ToString))
                    'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end
                    Exit For
                End If
            Next

            '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する Start
            '更新対象の合った場合のみ
            If True = blnResult And 0 < dicJobDtlID.Count Then
                'アドバイスを更新する
                blnResult = SetDBInspectionAdvice(dealerCD, _
                                                  branchCD, _
                                                  roNum, _
                                                  strAdviceContent, _
                                                  strAccount, _
                                                  dtfUpdate)
            End If
            '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する end


        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 登録処理(SetDBCmpChkResultUpt)
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="aprovalStatus">作業ステータス</param>
    ''' <param name="advicdContent">アドバイス(一時保存用)</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="updateTime">更新日</param>
    ''' <param name="lockVersion">ロックバージョン</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBCmpChkReslut(ByVal dlrCD As String, _
                                          ByVal brnCD As String, _
                                          ByVal jobDtlId As Decimal, _
                                          ByVal roNum As String, _
                                          ByVal aprovalStatus As Integer, _
                                          ByVal advicdContent As String, _
                                          ByVal accountName As String, _
                                          ByVal updateTime As Date, _
                                          ByVal lockVersion As Long, _
                                          ByVal aprovalReqAccount As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180204TableAdapter
        Dim dtTblCheck As SC3180204tblcheckDataTable

        '存在チェック
        dtTblCheck = tableAdapter.GetDBTableChk(dlrCD, brnCD, jobDtlId)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "exist record count : " & dtTblCheck(0).COUNT))

        If 0 = dtTblCheck(0).COUNT Then
            '検索結果なし(Insert)
            blnResult = tableAdapter.SetDBCmpChkReslutIns(dlrCD, _
                                                          brnCD, _
                                                          jobDtlId, _
                                                          roNum, _
                                                          aprovalStatus, _
                                                          advicdContent, _
                                                          accountName,
                                                          updateTime, _
                                                          aprovalReqAccount)
        Else

            'ヘッダロック処理
            SelectInspectionHeadLock(jobDtlId)

            '検索結果あり(Update)
            blnResult = tableAdapter.SetDBCmpChkResultUpt(dlrCD, _
                                                          brnCD, _
                                                          jobDtlId, _
                                                          roNum, _
                                                          aprovalStatus, _
                                                          accountName,
                                                          updateTime, _
                                                          lockVersion, _
                                                          aprovalReqAccount)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 登録処理(SetDBCmpChkResultDetailUpt)
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="jobInstructId">作業指示ID</param>
    ''' <param name="jobInstructSeq">作業指示枝番</param>
    ''' <param name="inspecItemCD">点検項目コード</param>
    ''' <param name="IinspecRsltCD">点検結果</param>
    ''' <param name="alreadyReplace">選択状態(replace)</param>
    ''' <param name="alreadyFixed">選択状態(fixed)</param>
    ''' <param name="alreadyCelaning">選択状態(celaning)</param>
    ''' <param name="alreadySwapped">選択状態(swapped)</param>
    ''' <param name="beforeText">作業値入力(Before)</param>
    ''' <param name="afterText">作業値入力(After)</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="updateTime">更新日</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks>'2014/06/02 Edit svcCdを引数に追加 Start</remarks>
    <EnableCommit()>
    Public Function SetDBCmpChkResultDetail(ByVal dlrCD As String, _
                                                ByVal brnCD As String, _
                                                ByVal jobDtlId As Decimal, _
                                                ByVal jobInstructId As String, _
                                                ByVal jobInstructSeq As Long, _
                                                ByVal inspecItemCD As String, _
                                                ByVal svcCd As String, _
                                                ByVal IinspecRsltCD As Long, _
                                                ByVal alreadyReplace As Long, _
                                                ByVal alreadyFixed As Long, _
                                                ByVal alreadyCelaning As Long, _
                                                ByVal alreadySwapped As Long, _
                                                ByVal beforeText As Decimal, _
                                                ByVal afterText As Decimal, _
                                                ByVal accountName As String, _
                                                ByVal updateTime As Date) As Boolean
        'Public Function SetDBCmpChkResultDetail(ByVal dlrCD As String, _
        '                                            ByVal brnCD As String, _
        '                                            ByVal jobDtlId As Decimal, _
        '                                            ByVal jobInstructId As String, _
        '                                            ByVal jobInstructSeq As Long, _
        '                                            ByVal inspecItemCD As String, _
        '                                            ByVal IinspecRsltCD As Long, _
        '                                            ByVal alreadyReplace As Long, _
        '                                            ByVal alreadyFixed As Long, _
        '                                            ByVal alreadyCelaning As Long, _
        '                                            ByVal alreadySwapped As Long, _
        '                                            ByVal beforeText As Decimal, _
        '                                            ByVal afterText As Decimal, _
        '                                            ByVal accountName As String, _
        '                                            ByVal updateTime As Date) As Boolean
        '2014/06/02 Edit svcCdを引数に追加 End

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180204TableAdapter
        Dim dtTblCheck As SC3180204tblcheckDataTable

        '存在チェック
        dtTblCheck = tableAdapter.GetDBTableChk(dlrCD, _
                                              brnCD, _
                                              jobDtlId, _
                                              jobInstructId, _
                                              jobInstructSeq, _
                                              inspecItemCD)

        If 0 = dtTblCheck(0).COUNT Then
            '検索結果なし(Insert)
            '2014/06/02 Edit svcCdを引数に追加 Start
            'blnResult = tableAdapter.SetDBCmpChkReslutDetailIns(dlrCD, _
            '                                                    brnCD, _
            '                                                    jobDtlId, _
            '                                                    jobInstructId, _
            '                                                    jobInstructSeq, _
            '                                                    inspecItemCD, _
            '                                                    IinspecRsltCD, _
            '                                                    alreadyReplace, _
            '                                                    alreadyFixed, _
            '                                                    alreadyCelaning, _
            '                                                    alreadySwapped, _
            '                                                    beforeText, _
            '                                                    afterText, _
            '                                                    accountName,
            '                                                    updateTime)
            blnResult = tableAdapter.SetDBCmpChkReslutDetailIns(dlrCD, _
                                                     brnCD, _
                                                     jobDtlId, _
                                                     jobInstructId, _
                                                     jobInstructSeq, _
                                                     inspecItemCD, _
                                                     svcCd, _
                                                     IinspecRsltCD, _
                                                     alreadyReplace, _
                                                     alreadyFixed, _
                                                     alreadyCelaning, _
                                                     alreadySwapped, _
                                                     beforeText, _
                                                     afterText, _
                                                     accountName,
                                                     updateTime)
            '2014/06/02 Edit svcCdを引数に追加　End

        Else
            '2014/06/04 明細更新なのにヘッダーをロックしているのでコメント Start
            '明細ロック処理
            'SelectInspectionHeadLock(jobDtlId)
            '2014/06/04 明細更新なのにヘッダーをロックしているのでコメント End

            '検索結果あり(Update)
            blnResult = tableAdapter.SetDBCmpChkResultDetailUpt(dlrCD, _
                                                                brnCD, _
                                                                jobDtlId, _
                                                                jobInstructId, _
                                                                jobInstructSeq, _
                                                                inspecItemCD, _
                                                                IinspecRsltCD, _
                                                                alreadyReplace, _
                                                                alreadyFixed, _
                                                                alreadyCelaning, _
                                                                alreadySwapped, _
                                                                beforeText, _
                                                                afterText, _
                                                                accountName,
                                                                updateTime)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 登録処理(SetDBInspection)
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="aprovalStaff">   </param>
    ''' <param name="updateFlg">   </param>
    ''' <param name="updateTime">更新日</param>
    ''' <param name="svcinId">サービスID</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBInspection(ByVal jobDtlId As Decimal, _
                                        ByVal accountName As String, _
                                        ByVal aprovalStaff As String,
                                        ByVal updateFlg As Integer, _
                                        ByVal updateTime As Date, _
                                        ByVal svcinId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180204TableAdapter

        'サービスインロック処理
        SelectSvcinLock(svcinId)

        Dim lockVersion As Long = Me.GetServiceInLock(svcinId)

        blnResult = tableAdapter.SetDBInspectionUpt(jobDtlId, _
                                                  accountName, _
                                                  aprovalStaff, _
                                                  updateFlg,
                                                  updateTime)

        If blnResult = True Then
            blnResult = tableAdapter.SetServiceInLockUpt(svcinId, _
                                                         lockVersion)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' アドバイス登録処理(SetDBServiceINAdviceComment)
    ''' </summary>
    ''' <param name="svcinId">サービスID</param>
    ''' <param name="advicdContent">アドバイス</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="updateTime">更新日</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBServiceINAdvice(ByVal svcinId As Decimal, _
                                             ByVal advicdContent As String, _
                                             ByVal accountName As String, _
                                             ByVal updateTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180204TableAdapter

        'サービスインロック処理
        SelectSvcinLock(svcinId)

        'サービスインロックバージョン取得
        Dim lockVersion As Long = Me.GetServiceInLock(svcinId)

        blnResult = tableAdapter.SetDBServiceINAdviceComment(svcinId, _
                                                                advicdContent, _
                                                                accountName, _
                                                                updateTime, _
                                                                lockVersion)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' ChkLastChip情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <returns>1:最終チップ　0:作業途中</returns>
    ''' <remarks></remarks>
    Public Function GetDBChkLastChip(ByVal dlrCD As String, _
                                     ByVal brnCD As String, _
                                     ByVal roNum As String, _
                                     ByVal jobDtlId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180204TableAdapter
        Dim dtChkLastChip As SC3180204ChkLastChipDataTable

        dtChkLastChip = tableAdapter.GetDBChkLastChip(dlrCD, brnCD, roNum, jobDtlId)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If dtChkLastChip(0).count.ToString = "1" Then
            blnResult = True
        Else
            blnResult = False
        End If

        Return blnResult

    End Function

    ''' <summary>
    ''' 行ロック更新処理(SetInspectionHeadLockUpt)
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="lockversion">ロックバージョン</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBInspectionLockUpt(ByVal dlrCD As String, _
                                                 ByVal brnCD As String, _
                                                 ByVal jobDtlId As Decimal, _
                                                 ByVal lockVersion As Long) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180204TableAdapter

        '完成検査ヘッダロック処理
        SelectInspectionHeadLock(jobDtlId)

        '完成検査ヘッダ行ロック更新処理
        blnResult = tableAdapter.SetInspectionHeadLockUpt(dlrCD, _
                                                          brnCD, _
                                                          jobDtlId, _
                                                          lockVersion)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 行ロックバージョン取得(GetServiceInLock)
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function GetServiceInLock(ByVal svcinId As Decimal) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180204TableAdapter

        'サービスイン 行ロックバージョンの取得
        Dim svcinLockVersion = tableAdapter.GetServiceLockVersion(svcinId)
        Dim lockVersion As Long

        If Not IsNothing(svcinLockVersion(0).ROW_LOCK_VERSION.ToString) Then
            lockVersion = Long.Parse(svcinLockVersion(0).ROW_LOCK_VERSION.ToString)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return lockVersion

    End Function

    ''' <summary>
    ''' ストール利用ID取得(GetServiceInLock)
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="dlrCD"></param>
    ''' <param name="brnCD"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStallUseId(ByVal jobDtlId As Decimal, ByVal dlrCD As String, ByVal brnCD As String) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180204TableAdapter

        'ストール利用IDの取得
        Dim stallUse = tableAdapter.GetStallUse(jobDtlId, dlrCD, brnCD)
        Dim stallUseId As Long

        If Not IsNothing(stallUse(0).STALL_USE_ID.ToString) Then
            stallUseId = Long.Parse(stallUse(0).STALL_USE_ID.ToString)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return stallUseId

    End Function

#End Region

    '2014/06/03 通知処理を承認入力から移植　Start
#Region "通知"
    '2014/06/03 通知処理を承認入力から移植　Start

    '#Region "通知用定数"

    '    ''' <summary>
    '    ''' 通知API用(カテゴリータイプ)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const NotifyPushCategory As String = "1"

    '    ''' <summary>
    '    ''' 通知API用(表示位置)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const NotifyPotisionType As String = "1"

    '    ''' <summary>
    '    ''' 通知API用(表示時間)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const NotifyTime As Integer = 3

    '    ''' <summary>
    '    ''' 通知API用(表示タイプ)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const NotifyDispType As String = "1"

    '    ''' <summary>
    '    ''' 通知API用(色)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const NotifyColor As String = "1"

    '    ''' <summary>
    '    ''' 通知API用(呼び出し関数)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const NotifyDispFunction As String = "icropScript.ui.setNotice()"

    '    ''' <summary>
    '    ''' 通知履歴のSessionValue(カンマ)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionValueKanma As String = ","

    '    ''' <summary>
    '    ''' 完成検入力認画面のリンク文字列
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const CheckPageLink As String = "<a id='SC31802040' Class='SC3180204' href='/Website/Pages/SC3180204.aspx' onclick='return ServiceLinkClick(event)'>"

    '    ''' <summary>
    '    ''' 完成検査承認画面のリンク文字列
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const ApprovePageLink As String = "<a id='SC31802010' Class='SC3180201' href='/Website/Pages/SC3180201.aspx' onclick='return ServiceLinkClick(event)'>"

    '    ''' <summary>
    '    ''' Aタグ終了文字列
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const EndLikTag As String = "</a>"

    '    ''' <summary>
    '    ''' 敬称利用区分("1"：後方)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const PositionTypeBack As String = "1"

    '    ''' <summary>
    '    ''' 敬称利用区分("2"：前方)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const PositionTypeFront As String = "2"

    '    ''' <summary>
    '    ''' イベントキーID
    '    ''' </summary>
    '    Private Enum EventKeyId

    '        ''' <summary>
    '        ''' 共通処理
    '        ''' </summary>
    '        CommonProces = 100

    '        ''' <summary>
    '        ''' 検査終了処理
    '        ''' </summary>
    '        RegistProces = 200

    '        ''' <summary>
    '        ''' 最終検査終了処理
    '        ''' </summary>
    '        LastRegistProces = 201

    '        ''' <summary>
    '        ''' 承認依頼処理
    '        ''' </summary>
    '        SendProces = 300

    '    End Enum

    '    ''' <summary>
    '    ''' メッセージID管理
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Enum MsgID
    '        ''' <summary>通知用文言("50"：完成検査承認)</summary>
    '        id50 = 50
    '        ''' <summary>通知用文言("51"：精算準備)</summary>
    '        id51 = 51
    '        ''' <summary>通知用文言("52"：完成検査否認)</summary>
    '        id52 = 52
    '    End Enum

    '    ''' <summary>
    '    ''' 画面遷移セッションキー(DMS販売店コード)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyDealerCode As String = "DealerCode,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(DMS店舗コード)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyBranchCode As String = "BranchCode,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(アカウント)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyAccount As String = "LoginUserID,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(来店実績連番)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyVistSequence As String = "SAChipID,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(DMS予約ID)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyResrveId As String = "BASREZID,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(RO番号)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyRepairorder As String = "R_O,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(RO作業連番)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeySequence As String = "SEQ_NO,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(RO番号)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyJobDtlId As String = "JOB_DTL_ID,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(VIN)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyVin As String = "VIN,String,"
    '    ''' <summary>
    '    ''' 画面遷移セッションキー(編集モード)
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const SessionKeyViewMode As String = "ViewMode,String,"

    '#End Region

    '#Region "Publicメソッド"

    '    ''' <summary>
    '    ''' 通知処理
    '    ''' </summary>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="inSaChip">   </param>
    '    ''' <param name="inBaserzId">   </param>
    '    ''' <param name="roNum">Ro番号</param>
    '    ''' <param name="inSeqNo">   </param>
    '    ''' <param name="inVin">    </param>
    '    ''' <param name="inViewMode">   </param>
    '    ''' <param name="jobDtlId">作業内容ID</param>
    '    ''' <param name="inPresentTime">現在日時</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <param name="inSendUser">送信先ユーザー</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' </history>
    '    Public Sub NoticeProcessing(ByVal inStaffInfo As StaffContext, _
    '                                ByVal inSaChip As String, _
    '                                ByVal inBaserzId As String, _
    '                                ByVal roNum As String, _
    '                                ByVal inSeqNo As String, _
    '                                ByVal inVin As String, _
    '                                ByVal inViewMode As String, _
    '                                ByVal jobDtlId As String, _
    '                                ByVal inPresentTime As DateTime, _
    '                                ByVal inEventKey As String, _
    '                                ByVal inSendUser As String, _
    '                                ByVal saAccountId As String, _
    '                                ByVal inOperetionType As OperetionMode)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} ROUMBER[{2}] PRESENTTIME:{3} EVENTKEY:{4}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , roNum, inPresentTime, inEventKey))

    '        '通知処理デバッグ用
    '        Dim NoticeFlg As Boolean = False
    '        If NoticeFlg = True Then

    '            'SC3180204TableAdapterのインスタンス
    '            Dim da As New SC3180204TableAdapter

    '            '通知送信用情報取得
    '            Dim dtNoticeProcessingInfo As SC3180204NoticeProcessingInfoDataTable = _
    '                da.GetNoticeProcessingInfo(roNum, _
    '                                           inStaffInfo.DlrCD, _
    '                                           inStaffInfo.BrnCD, _
    '                                           Decimal.Parse(jobDtlId))

    '            '通知送信用情報取得チェック
    '            If 0 < dtNoticeProcessingInfo.Count Then
    '                '取得できた場合

    '                'Rowに変換
    '                Dim rowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow = _
    '                    DirectCast(dtNoticeProcessingInfo.Rows(0), SC3180204NoticeProcessingInfoRow)

    '                '現在日時を設定
    '                rowNoticeProcessingInfo.PRESENTTIME = inPresentTime

    '                'イベント情報判定
    '                Select Case inEventKey

    '                    Case CType(EventKeyId.CommonProces, String)
    '                        '共通処理

    '                        '共通の実行
    '                        Me.NoticeMainProcessing(rowNoticeProcessingInfo, inStaffInfo, _
    '                                                 inSaChip, _
    '                                                 inBaserzId, _
    '                                                 roNum, _
    '                                                 inSeqNo, _
    '                                                 inVin, _
    '                                                 inViewMode, _
    '                                                 jobDtlId, _
    '                                                 EventKeyId.CommonProces, _
    '                                                 inSendUser, _
    '                                                 saAccountId, _
    '                                                 inOperetionType)

    '                    Case CType(EventKeyId.LastRegistProces, String)
    '                        '検査終了処理

    '                        '最終検査終了の実行
    '                        Me.NoticeMainProcessing(rowNoticeProcessingInfo, inStaffInfo, _
    '                                                 inSaChip, _
    '                                                 inBaserzId, _
    '                                                 roNum, _
    '                                                 inSeqNo, _
    '                                                 inVin, _
    '                                                 inViewMode, _
    '                                                 jobDtlId, _
    '                                                 EventKeyId.LastRegistProces, _
    '                                                 inSendUser, _
    '                                                 saAccountId, _
    '                                                 inOperetionType)

    '                    Case CType(EventKeyId.RegistProces, String)
    '                        '検査終了処理

    '                        '検査終了の実行
    '                        Me.NoticeMainProcessing(rowNoticeProcessingInfo, inStaffInfo, _
    '                                                 inSaChip, _
    '                                                 inBaserzId, _
    '                                                 roNum, _
    '                                                 inSeqNo, _
    '                                                 inVin, _
    '                                                 inViewMode, _
    '                                                 jobDtlId, _
    '                                                 EventKeyId.RegistProces, _
    '                                                 inSendUser, _
    '                                                 saAccountId, _
    '                                                 inOperetionType)

    '                    Case CType(EventKeyId.SendProces, String)
    '                        '承認依頼処理()

    '                        '承認依頼の実行
    '                        Me.NoticeMainProcessing(rowNoticeProcessingInfo, inStaffInfo, _
    '                                                 inSaChip, _
    '                                                 inBaserzId, _
    '                                                 roNum, _
    '                                                 inSeqNo, _
    '                                                 inVin, _
    '                                                 inViewMode, _
    '                                                 jobDtlId, _
    '                                                 EventKeyId.SendProces, _
    '                                                 inSendUser, _
    '                                                 saAccountId, _
    '                                                 inOperetionType)

    '                End Select

    '            Else
    '                '取得失敗

    '                'エラーログ
    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                           , "{0}.{1} GetNoticeProcessingInfo IS NOTHING" _
    '                           , Me.GetType.ToString _
    '                           , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '            End If
    '        End If

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    End Sub

    '#End Region

    '#Region "Privateメソッド"

    '    ''' <summary>
    '    ''' 通知メイン処理
    '    ''' </summary>
    '    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="inSaChip">   </param>
    '    ''' <param name="inBaserzId">   </param>
    '    ''' <param name="roNum">Ro番号</param>
    '    ''' <param name="inSeqNo">   </param>
    '    ''' <param name="inVin">    </param>
    '    ''' <param name="inViewMode">   </param>
    '    ''' <param name="jobDtlId">作業内容ID</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <param name="inAproveStaff">   </param>
    '    ''' <remarks></remarks>
    '    Private Sub NoticeMainProcessing(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
    '                                     ByVal inStaffInfo As StaffContext, _
    '                                     ByVal inSaChip As String, _
    '                                     ByVal inBaserzId As String, _
    '                                     ByVal roNum As String, _
    '                                     ByVal inSeqNo As String, _
    '                                     ByVal inVin As String, _
    '                                     ByVal inViewMode As String, _
    '                                     ByVal jobDtlId As String, _
    '                                     ByVal inEventKey As EventKeyId, _
    '                                     ByVal inAproveStaff As String, _
    '                                     ByVal saAccountId As String, _
    '                                     ByVal inOperetionType As OperetionMode)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START EVENTKEY:{2}" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                  , inEventKey))

    '        Dim account As List(Of XmlAccount) = New List(Of XmlAccount)()

    '        Select Case inEventKey

    '            Case EventKeyId.CommonProces

    '                '送信先アカウント情報設定
    '                account = Me.CreateAccount(inStaffInfo, inEventKey)

    '            Case EventKeyId.LastRegistProces, EventKeyId.RegistProces

    '                '送信先アカウント情報設定(個別)
    '                account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey)

    '                'Case EventKeyId.RegistProces

    '                '    '送信先アカウント情報設定(個別)
    '                '    account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey)

    '            Case EventKeyId.SendProces

    '                '送信先アカウント情報設定(個別)
    '                account = Me.CreateAccountSendParticular(inStaffInfo, inAproveStaff, inEventKey)

    '        End Select

    '        Notification(inStaffInfo.Account, inAproveStaff, inOperetionType, IIf(jobDtlId <> String.Empty, IdType.Yes, IdType.No))

    '        '通知履歴登録情報の設定
    '        Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(inRowNoticeProcessingInfo, inStaffInfo, _
    '                                                                       inSaChip, _
    '                                                                       inBaserzId, _
    '                                                                       roNum, _
    '                                                                       inSeqNo, _
    '                                                                       inVin, _
    '                                                                       inViewMode, _
    '                                                                       jobDtlId, _
    '                                                                       inEventKey)

    '        'Push情報作成処理の設定
    '        Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeProcessingInfo, inEventKey)

    '        '設定したものを格納し、通知APIをコール
    '        Using noticeData As New XmlNoticeData

    '            '現在時間データの格納
    '            noticeData.TransmissionDate = inRowNoticeProcessingInfo.PRESENTTIME
    '            '送信ユーザーデータ格納
    '            noticeData.AccountList.AddRange(account.ToArray)
    '            '通知履歴用のデータ格納
    '            noticeData.RequestNotice = requestNotice
    '            'Pushデータ格納
    '            noticeData.PushInfo = pushInfo

    '            '通知処理実行
    '            Using ic3040801Biz As New IC3040801BusinessLogic

    '                'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
    '                'Logger.Info(LogNoticeData(noticeData) &
    '                '            GetLogParam("noticeDisposalMode", CStr(NoticeDisposal.GeneralPurpose), True))

    '                '通知処理実行
    '                ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

    '            End Using
    '        End Using

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    End Sub

    '#End Region

    '#Region "通知独自部分"

    '    ''' <summary>
    '    ''' 送信先アカウント情報作成処理
    '    ''' </summary>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <returns>送信先アカウント情報リスト</returns>
    '    ''' <remarks></remarks>
    '    Private Function CreateAccount(ByVal inStaffInfo As StaffContext, _
    '                                   ByVal inEventKey As EventKeyId) As List(Of XmlAccount)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START EVENTKEY:{2}" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                  , inEventKey))

    '        '送信先アカウント情報リスト
    '        Dim accountList As New List(Of XmlAccount)

    '        'OperationCodeリスト
    '        Dim operationCodeList As New List(Of Long)

    '        'OperationCodeリストに権限"58"：FMを設定
    '        operationCodeList.Add(Operation.FM)

    '        'OperationCodeリストに権限"55"：CTを設定
    '        operationCodeList.Add(Operation.CT)

    '        'OperationCodeリストに権限"62"：CHTを設定
    '        operationCodeList.Add(Operation.CHT)

    '        'ユーザーステータス取得
    '        Using user As New IC3810601BusinessLogic

    '            'ユーザーステータス取得処理
    '            '各権限の全ユーザー情報取得
    '            Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
    '                user.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
    '                                             inStaffInfo.BrnCD, _
    '                                             operationCodeList)

    '            'オンラインユーザー分ループ
    '            For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

    '                '送信先アカウント情報 
    '                Using account As New XmlAccount

    '                    '受信先のアカウント設定
    '                    account.ToAccount = userRow.ACCOUNT

    '                    ''受信先アカウントログ出力
    '                    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    '          , "ACCOUNT [{0}] " _
    '                    '          , userRow.ACCOUNT))

    '                    '受信者名設定
    '                    account.ToAccountName = userRow.USERNAME

    '                    ''受信者名ログ出力
    '                    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    '          , "USERNAME [{0}] " _
    '                    '          , userRow.USERNAME))

    '                    '送信先アカウント情報リストに送信先アカウント情報を追加
    '                    accountList.Add(account)

    '                End Using

    '            Next

    '        End Using

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Return accountList


    '    End Function

    '    ''' <summary>
    '    ''' 送信先アカウント情報作成処理（個別）
    '    ''' </summary>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="roNum">Ro番号</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <returns>送信先アカウント情報リスト</returns>
    '    ''' <remarks></remarks>
    '    Private Function CreateAccountParticular(ByVal inStaffInfo As StaffContext, _
    '                                             ByVal roNum As String, _
    '                                             ByVal inEventKey As EventKeyId) As List(Of XmlAccount)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START EVENTKEY:{2}" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                  , inEventKey))

    '        '送信先アカウント情報リスト
    '        Dim accountList As New List(Of XmlAccount)

    '        'ユーザーステータス取得
    '        Dim tableAdapter As New SC3180204TableAdapter
    '        Dim user As SC3180204PicSaStfDataTable = _
    '            tableAdapter.GetPicSaStf(inStaffInfo.DlrCD, _
    '                                     inStaffInfo.BrnCD, _
    '                                     roNum)

    '        'ユーザーステータス取得処理
    '        Dim userDT As SC3180204PicSaStfRow =
    '            DirectCast(user.Rows(0), SC3180204PicSaStfRow)

    '        '送信先アカウント情報 
    '        Using account As New XmlAccount

    '            '受信先のアカウント設定
    '            account.ToAccount = userDT.PIC_SA_STF_CD

    '            '受信先アカウントログ出力
    '            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            '          , "ACCOUNT [{0}] " _
    '            '          , userDT.PIC_SA_STF_CD))

    '            '受信者名設定
    '            account.ToAccountName = userDT.USERNAME

    '            ''受信者名ログ出力
    '            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            '          , "USERNAME [{0}] " _
    '            '          , userDT.USERNAME))

    '            '送信先アカウント情報リストに送信先アカウント情報を追加
    '            accountList.Add(account)

    '        End Using
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Return accountList

    '    End Function

    '    ''' <summary>
    '    ''' 送信先アカウント情報作成処理（Send個別）
    '    ''' </summary>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="inSendUser">送信先ユーザー</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <returns>送信先アカウント情報リスト</returns>
    '    ''' <remarks></remarks>
    '    Private Function CreateAccountSendParticular(ByVal inStaffInfo As StaffContext, _
    '                                                 ByVal inSendUser As String, _
    '                                                 ByVal inEventKey As EventKeyId) As List(Of XmlAccount)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START EVENTKEY:{2}" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                  , inEventKey))

    '        '送信先アカウント情報リスト
    '        Dim accountList As New List(Of XmlAccount)

    '        'ユーザーステータス取得
    '        Dim tableAdapter As New SC3180204TableAdapter
    '        Dim user As SC3180204AproveStfDataTable = _
    '            tableAdapter.GetStfInfo(inSendUser)

    '        'ユーザーステータス取得処理
    '        Dim userdt As SC3180204AproveStfRow =
    '            DirectCast(user.Rows(0), SC3180204AproveStfRow)

    '        '送信先アカウント情報 
    '        Using account As New XmlAccount

    '            '受信先のアカウント設定
    '            account.ToAccount = userdt.ACCOUNT

    '            '受信先アカウントログ出力
    '            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            '          , "ACCOUNT [{0}] " _
    '            '          , userdt.ACCOUNT))

    '            '受信者名設定
    '            account.ToAccountName = userdt.USERNAME

    '            '受信者名ログ出力
    '            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            '          , "USERNAME [{0}] " _
    '            '          , userdt.USERNAME))


    '            '送信先アカウント情報リストに送信先アカウント情報を追加
    '            accountList.Add(account)

    '        End Using
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Return accountList

    '    End Function

    '    ''' <summary>
    '    ''' 通知履歴登録情報作成処理
    '    ''' </summary>
    '    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="inSaChip">   </param>
    '    ''' <param name="inBaserzId">   </param>
    '    ''' <param name="roNum">Ro番号</param>
    '    ''' <param name="inSeqNo">   </param>
    '    ''' <param name="inVin">    </param>
    '    ''' <param name="inViewMode">   </param>
    '    ''' <param name="jobDtlId">作業内容ID</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <returns>通知履歴登録情報</returns>
    '    ''' <remarks></remarks>
    '    Private Function CreateRequestNotice(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
    '                                         ByVal inStaffInfo As StaffContext, _
    '                                         ByVal inSaChip As String, _
    '                                         ByVal inBaserzId As String, _
    '                                         ByVal roNum As String, _
    '                                         ByVal inSeqNo As String, _
    '                                         ByVal inVin As String, _
    '                                         ByVal inViewMode As String, _
    '                                         ByVal jobDtlId As String, _
    '                                         ByVal inEventKey As EventKeyId) As XmlRequestNotice

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        'XmlRequestNoticeのインスタンス
    '        Using requestNotice As New XmlRequestNotice

    '            '販売店コード設定
    '            requestNotice.DealerCode = inStaffInfo.DlrCD

    '            '店舗コード設定
    '            requestNotice.StoreCode = inStaffInfo.BrnCD

    '            'スタッフコード(送信元)設定
    '            requestNotice.FromAccount = inStaffInfo.Account

    '            'スタッフ名(送信元)設定
    '            requestNotice.FromAccountName = inStaffInfo.UserName

    '            '通知履歴にリンクをつけるか判定
    '            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
    '            '通知履歴にリンクをつける
    '            Select Case inEventKey

    '                Case EventKeyId.CommonProces

    '                    requestNotice.Message = Space(1)

    '                Case Else

    '                    '通知履歴用メッセージ作成設定
    '                    requestNotice.Message = Me.CreateNoticeRequestMessage(inRowNoticeProcessingInfo, inEventKey)

    '            End Select

    '            'セッション設定値設定
    '            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowNoticeProcessingInfo, _
    '                                                                       inStaffInfo, _
    '                                                                       inSaChip, _
    '                                                                       inBaserzId, _
    '                                                                       roNum, _
    '                                                                       inSeqNo, _
    '                                                                       inVin, _
    '                                                                       inViewMode, _
    '                                                                       jobDtlId)

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} END" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '            Return requestNotice

    '        End Using

    '    End Function

    '    ''' <summary>
    '    ''' 通知履歴用メッセージ作成処理
    '    ''' </summary>
    '    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <returns>通知履歴用メッセージ情報</returns>
    '    ''' <history>
    '    ''' </history>
    '    ''' <remarks></remarks>
    '    Private Function CreateNoticeRequestMessage(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
    '                                                ByVal inEventKey As EventKeyId) As String

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        'メッセージ
    '        Dim workMessage As New StringBuilder

    '        'メッセージ組立処理

    '        'イベントごとに処置分岐
    '        Select Case inEventKey
    '            Case EventKeyId.SendProces
    '                '承認依頼処理

    '                '通知履歴にリンクをつける
    '                'Aタグを設定
    '                workMessage.Append(ApprovePageLink)

    '                '文言：承認依頼 設定
    '                workMessage.Append(WebWordUtility.GetWord(MsgID.id50))

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '            Case EventKeyId.LastRegistProces
    '                '最終検査終了処理

    '                '通知履歴にリンクをつける
    '                'Aタグを設定
    '                workMessage.Append(CheckPageLink)

    '                '文言：最終承認 設定
    '                workMessage.Append(WebWordUtility.GetWord(MsgID.id51))

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '            Case EventKeyId.RegistProces
    '                '結果変更報告

    '                '通知履歴にリンクをつける
    '                'Aタグを設定
    '                workMessage.Append(CheckPageLink)

    '                '文言：結果変更報告 設定
    '                workMessage.Append(WebWordUtility.GetWord(MsgID.id52))

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '        End Select

    '        'メッセージ組立：RO番号
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.RO_NUM) Then

    '            'RO番号を設定
    '            workMessage.Append(inRowNoticeProcessingInfo.RO_NUM)

    '            'メッセージ間にスペースの設定
    '            workMessage.Append(Space(1))

    '        End If

    '        'メッセージ組立：REG番号
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.REG_NO) Then

    '            'REG番号を設定
    '            workMessage.Append(inRowNoticeProcessingInfo.REG_NO)

    '            'メッセージ間にスペースの設定
    '            workMessage.Append(Space(1))

    '        End If

    '        'メッセージ組立：お客様名
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.CST_NAME) Then
    '            'お客様名がある場合

    '            '敬称利用区分チェック
    '            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
    '                '敬称を後方につける

    '                '顧客名を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

    '                '敬称を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
    '                '敬称を前方につける

    '                '敬称を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

    '                '顧客名を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '            Else
    '                '上記以外の場合

    '                '顧客名を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

    '            End If

    '        Else
    '            'お客様名がない場合

    '            '文言：お客様 設定
    '            workMessage.Append(Space(1))

    '        End If

    '        'メッセージ組立：商品名
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then

    '            '商品名を設定
    '            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

    '            'メッセージ間にスペースの設定
    '            workMessage.Append(Space(1))

    '        End If

    '        Select Case inEventKey
    '            Case EventKeyId.SendProces
    '                '承認依頼処理

    '                'Aタグ終了を設定
    '                workMessage.Append(EndLikTag)

    '            Case EventKeyId.LastRegistProces
    '                '最終検査終了処理

    '                'Aタグ終了を設定
    '                workMessage.Append(EndLikTag)

    '            Case EventKeyId.RegistProces
    '                '結果変更報告

    '                'Aタグ終了を設定
    '                workMessage.Append(EndLikTag)

    '        End Select

    '        '戻り値設定
    '        Dim notifyMessage As String = workMessage.ToString().TrimEnd

    '        ''送信メッセージログ出力
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '    , "MESSAGE [{0}]" _
    '        '    , notifyMessage))

    '        '開放処理
    '        workMessage = Nothing

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END MESSAGE = {2}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , notifyMessage))

    '        Return notifyMessage

    '    End Function

    '    ''' <summary>
    '    ''' 通知履歴用セッション情報作成メソッド
    '    ''' </summary>
    '    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="inSaChip">   </param>
    '    ''' <param name="inBaserzId">   </param>
    '    ''' <param name="roNum">Ro番号</param>
    '    ''' <param name="inSeqNo">   </param>
    '    ''' <param name="inVin">    </param>
    '    ''' <param name="inViewMode">   </param>
    '    ''' <param name="jobDtlId">作業内容ID</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' </history>
    '    Private Function CreateNoticeRequestSession(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
    '                                                ByVal inStaffInfo As StaffContext, _
    '                                               ByVal inSaChip As String, _
    '                                               ByVal inBaserzId As String, _
    '                                               ByVal roNum As String, _
    '                                               ByVal inSeqNo As String, _
    '                                               ByVal inVin As String, _
    '                                               ByVal inViewMode As String, _
    '                                               ByVal jobDtlId As String) As String

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Dim notifySession As String = String.Empty

    '        '通知用セッション情報作成処理
    '        notifySession = CreateCustomerSession(inRowNoticeProcessingInfo, _
    '                                               inStaffInfo, _
    '                                               inSaChip, _
    '                                               inBaserzId, _
    '                                               roNum, _
    '                                               inSeqNo, _
    '                                               inVin, _
    '                                               inViewMode, _
    '                                               jobDtlId)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Return notifySession

    '    End Function

    '    ''' <summary>
    '    ''' 通知用セッション情報作成メソッド
    '    ''' </summary>
    '    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    '    ''' <param name="inStaffInfo">ログイン情報</param>
    '    ''' <param name="inSaChip">   </param>
    '    ''' <param name="inBaserzId">   </param>
    '    ''' <param name="roNum">Ro番号</param>
    '    ''' <param name="inSeqNo">   </param>
    '    ''' <param name="inVin">    </param>
    '    ''' <param name="inViewMode">   </param>
    '    ''' <param name="jobDtlId">作業内容ID</param>
    '    ''' <returns>戻り値</returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' </history>
    '    Private Function CreateCustomerSession(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
    '                                           ByVal inStaffInfo As StaffContext, _
    '                                           ByVal inSaChip As String, _
    '                                           ByVal inBaserzId As String, _
    '                                           ByVal roNum As String, _
    '                                           ByVal inSeqNo As String, _
    '                                           ByVal inVin As String, _
    '                                           ByVal inViewMode As String, _
    '                                           ByVal jobDtlId As String) As String

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Dim workSession As New StringBuilder

    '        'DEARLERCODEの設定
    '        If Not String.IsNullOrEmpty(inStaffInfo.DlrCD.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyDealerCode, inStaffInfo.DlrCD.Trim)

    '        End If

    '        'BRANCHCODEの設定
    '        If Not String.IsNullOrEmpty(inStaffInfo.BrnCD.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyBranchCode, inStaffInfo.BrnCD.Trim)

    '        End If

    '        'ACCOUNTの設定
    '        If Not String.IsNullOrEmpty(inStaffInfo.Account.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyAccount, inStaffInfo.Account.Trim)

    '        End If

    '        'RO_NUMの設定
    '        If Not String.IsNullOrEmpty(roNum.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyRepairorder, roNum.Trim)

    '        End If

    '        'SEQ_NOの設定
    '        If Not String.IsNullOrEmpty(inSeqNo.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeySequence, inSeqNo.Trim)

    '        End If

    '        'JOB_DTL_IDの設定
    '        If Not String.IsNullOrEmpty(jobDtlId.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyJobDtlId, jobDtlId.Trim)

    '        End If

    '        'VINの設定
    '        If Not String.IsNullOrEmpty(inVin.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyVin, inVin.Trim)

    '        End If

    '        'VIEWMODEの設定
    '        If Not String.IsNullOrEmpty(inViewMode.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyViewMode, inViewMode.Trim)

    '        End If

    '        'SACHIPの設定
    '        If Not String.IsNullOrEmpty(inSaChip.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyVistSequence, inSaChip.Trim)

    '        End If

    '        'BASERZIDの設定
    '        If Not String.IsNullOrEmpty(inBaserzId.Trim) Then
    '            'VINがある場合は設定

    '            'VINのセッション値作成
    '            Me.SetSessionValueWord(workSession, SessionKeyResrveId, inBaserzId.Trim)

    '        End If

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Return workSession.ToString

    '    End Function

    '    ''' <summary>
    '    ''' SessionValue文字列作成
    '    ''' </summary>
    '    ''' <param name="workSession">追加元文字列</param>
    '    ''' <param name="SessionValueWord">追加するSESSIONKEY</param>
    '    ''' <param name="SessionValueData">追加するデータ</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function SetSessionValueWord(ByVal workSession As StringBuilder, _
    '                                         ByVal sessionValueWord As String, _
    '                                         ByVal sessionValueData As String) As StringBuilder


    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        'カンマの設定
    '        If workSession.Length <> 0 Then
    '            'データがある場合

    '            '「,」を結合する
    '            workSession.Append(SessionValueKanma)

    '        End If

    '        'セッションキーを設定
    '        workSession.Append(sessionValueWord)

    '        'セッション値を設定
    '        workSession.Append(sessionValueData)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        Return workSession

    '    End Function

    '    ''' <summary>
    '    ''' Push情報作成処理
    '    ''' </summary>
    '    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <returns>Push情報</returns>
    '    ''' <remarks></remarks>
    '    Private Function CreatePushInfo(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
    '                                    ByVal inEventKey As EventKeyId) As XmlPushInfo

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        'PUSH内容設定
    '        Using pushInfo As New XmlPushInfo

    '            'カテゴリータイプ設定
    '            pushInfo.PushCategory = NotifyPushCategory

    '            '表示位置設定
    '            pushInfo.PositionType = NotifyPotisionType

    '            '表示時間設定
    '            pushInfo.Time = NotifyTime

    '            '表示タイプ設定
    '            pushInfo.DisplayType = NotifyDispType

    '            Select Case inEventKey

    '                Case EventKeyId.CommonProces

    '                    'Push用メッセージ作成
    '                    pushInfo.DisplayContents = Space(1)

    '                Case EventKeyId.RegistProces, EventKeyId.LastRegistProces, EventKeyId.SendProces

    '                    'Push用メッセージ作成
    '                    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

    '                    'Case EventKeyId.LastRegistProces

    '                    '    'Push用メッセージ作成
    '                    '    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

    '                    'Case EventKeyId.SendProces

    '                    '    'Push用メッセージ作成
    '                    '    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

    '            End Select

    '            '色設定
    '            pushInfo.Color = NotifyColor

    '            '表示時関数設定
    '            pushInfo.DisplayFunction = NotifyDispFunction

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} END" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '            Return pushInfo

    '        End Using
    '    End Function

    '    ''' <summary>
    '    ''' Push用メッセージ作成処理
    '    ''' </summary>
    '    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    '    ''' <param name="inEventKey">イベント特定キー情報</param>
    '    ''' <returns>Puss用メッセージ文言</returns>
    '    ''' <history>
    '    ''' </history>
    '    ''' <remarks></remarks>
    '    Private Function CreatePusuMessage(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
    '                                       ByVal inEventKey As EventKeyId) As String

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        'メッセージ
    '        Dim workMessage As New StringBuilder

    '        'メッセージ組立処理

    '        'イベントごとに処置分岐
    '        Select Case inEventKey
    '            Case EventKeyId.CommonProces
    '                '共通処理

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '            Case EventKeyId.SendProces
    '                '承認依頼処理

    '                '文言：承認依頼 設定
    '                workMessage.Append(WebWordUtility.GetWord(MsgID.id50))

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '            Case EventKeyId.LastRegistProces

    '                '文言：最終承認 設定
    '                workMessage.Append(WebWordUtility.GetWord(MsgID.id51))

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '            Case EventKeyId.RegistProces
    '                '結果変更報告

    '                '文言：結果変更報告 設定
    '                workMessage.Append(WebWordUtility.GetWord(MsgID.id52))

    '                'メッセージ間にスペースの設定
    '                workMessage.Append(Space(1))

    '        End Select


    '        'メッセージ組立：RO番号
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.RO_NUM) Then

    '            'RO番号を設定
    '            workMessage.Append(inRowNoticeProcessingInfo.RO_NUM)

    '            'メッセージ間にスペースの設定
    '            workMessage.Append(Space(1))

    '        End If

    '        'メッセージ組立：REG番号
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.REG_NO) Then

    '            'REG番号を設定
    '            workMessage.Append(inRowNoticeProcessingInfo.REG_NO)

    '            'メッセージ間にスペースの設定
    '            workMessage.Append(Space(1))

    '        End If

    '        'メッセージ組立：お客様名
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.CST_NAME) Then
    '            'お客様名がある場合

    '            '敬称利用区分チェック
    '            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
    '                '敬称を後方につけつ

    '                '顧客名を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

    '                '敬称を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

    '            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
    '                '敬称を前方につける

    '                '敬称を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

    '                '顧客名を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

    '            Else
    '                '上記以外の場合

    '                '顧客名を設定
    '                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

    '            End If

    '            'メッセージ間にスペースの設定
    '            workMessage.Append(Space(1))

    '        Else
    '            'お客様名がない場合
    '            'メッセージ間にスペースの設定
    '            workMessage.Append(Space(1))

    '        End If

    '        'メッセージ組立：商品名
    '        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then
    '            '商品名がある場合

    '            '商品名を設定
    '            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

    '        End If

    '        '戻り値設定
    '        Dim notifyMessage As String = workMessage.ToString().TrimEnd


    '        '開放処理
    '        workMessage = Nothing

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END MESSAGE = {2}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , notifyMessage))

    '        Return notifyMessage

    '    End Function

    '#End Region

    '#Region "ログデータ加工処理"

    '    '    ''' <summary>
    '    '    ''' ログデータ（メソッド）
    '    '    ''' </summary>
    '    '    ''' <param name="methodName">メソッド名</param>
    '    '    ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
    '    '    ''' <returns>加工した文字列</returns>
    '    '    ''' <remarks></remarks>
    '    '    Private Function GetLogMethod(ByVal methodName As String,
    '    '                                  ByVal startEndFlag As Boolean) As String
    '    '        Dim sb As New StringBuilder
    '    '        With sb
    '    '            .Append("[")
    '    '            .Append(methodName)
    '    '            .Append("]")
    '    '            If startEndFlag Then
    '    '                .Append(" method start")
    '    '            Else
    '    '                .Append(" method end")
    '    '            End If
    '    '        End With
    '    '        Return sb.ToString
    '    '    End Function

    '    '    ''' <summary>
    '    '    ''' ログデータ（引数）
    '    '    ''' </summary>
    '    '    ''' <param name="paramName">引数名</param>
    '    '    ''' <param name="paramData">引数値</param>
    '    '    ''' <param name="kanmaFlag">True：引数名の前に「,」を表示、False：特になし</param>
    '    '    ''' <returns>加工した文字列</returns>
    '    '    ''' <remarks></remarks>
    '    '    Private Function GetLogParam(ByVal paramName As String,
    '    '                                 ByVal paramData As String,
    '    '                                 ByVal kanmaFlag As Boolean) As String
    '    '        Dim sb As New StringBuilder
    '    '        With sb
    '    '            If kanmaFlag Then
    '    '                .Append(",")
    '    '            End If
    '    '            .Append(paramName)
    '    '            .Append("=")
    '    '            .Append(paramData)
    '    '        End With
    '    '        Return sb.ToString
    '    '    End Function

    '    '#End Region

    '    '#Region "XmlNoticeDataログデータ加工処理"

    '    '    ''' <summary>
    '    '    ''' XmlNoticeDataログデータ加工処理
    '    '    ''' </summary>
    '    '    ''' <param name="xmlNoticeData">XmlNoticeDataクラス</param>
    '    '    ''' <returns>ログ情報</returns>
    '    '    ''' <remarks></remarks>
    '    '    Private Function LogNoticeData(ByVal xmlNoticeData As XmlNoticeData) As String
    '    '        Dim log As New StringBuilder
    '    '        With log
    '    '            '見やすくするために改行
    '    '            .AppendLine("")
    '    '            .AppendLine("000･･･")
    '    '            .AppendLine("<TransmissionDate>" & CStr(xmlNoticeData.TransmissionDate))
    '    '            .AppendLine("100･･･")
    '    '            For Each accountData In xmlNoticeData.AccountList
    '    '                .AppendLine("<ToAccount>" & accountData.ToAccount)
    '    '                .AppendLine("<ToClientID>" & accountData.ToClientId)
    '    '                .AppendLine("<ToAccountName>" & accountData.ToAccountName)
    '    '            Next
    '    '            .AppendLine("200･･･")
    '    '            .AppendLine("<DealerCode>" & xmlNoticeData.RequestNotice.DealerCode)
    '    '            .AppendLine("<StoreCode>" & xmlNoticeData.RequestNotice.StoreCode)
    '    '            .AppendLine("<RequestClass>" & xmlNoticeData.RequestNotice.RequestClass)
    '    '            .AppendLine("<Status>" & xmlNoticeData.RequestNotice.Status)
    '    '            .AppendLine("<RequestID>" & xmlNoticeData.RequestNotice.RequestId)
    '    '            .AppendLine("<RequestClassID>" & xmlNoticeData.RequestNotice.RequestClassId)
    '    '            .AppendLine("<FromAccount>" & xmlNoticeData.RequestNotice.FromAccount)
    '    '            .AppendLine("<FromClientID>" & xmlNoticeData.RequestNotice.FromClientId)
    '    '            .AppendLine("<FromAccountName>" & xmlNoticeData.RequestNotice.FromAccountName)
    '    '            .AppendLine("<CustomID>" & xmlNoticeData.RequestNotice.CustomId)
    '    '            .AppendLine("<CustomName>" & xmlNoticeData.RequestNotice.CustomName)
    '    '            .AppendLine("<CustomerClass>" & xmlNoticeData.RequestNotice.CustomerClass)
    '    '            .AppendLine("<CstKind>" & xmlNoticeData.RequestNotice.CustomerKind)
    '    '            .AppendLine("<Message>" & xmlNoticeData.RequestNotice.Message)
    '    '            .AppendLine("<SessionValue>" & xmlNoticeData.RequestNotice.SessionValue)
    '    '            .AppendLine("<SalesStaffCode>" & xmlNoticeData.RequestNotice.SalesStaffCode)
    '    '            .AppendLine("<VehicleSequenceNumber>" & xmlNoticeData.RequestNotice.VehicleSequenceNumber)
    '    '            .AppendLine("<FollowUpBoxStoreCode>" & xmlNoticeData.RequestNotice.FollowUpBoxStoreCode)
    '    '            .AppendLine("<FollowUpBoxNumber>" & xmlNoticeData.RequestNotice.FollowUpBoxNumber)
    '    '            ' $01 start step2開発
    '    '            .AppendLine("<CSPaperName>" & xmlNoticeData.RequestNotice.CSPaperName)
    '    '            ' $01 end   step2開発
    '    '            .AppendLine("300･･･")
    '    '            If Not IsNothing(xmlNoticeData.PushInfo) Then
    '    '                .AppendLine("<PushCategory>" & xmlNoticeData.PushInfo.PushCategory)
    '    '                .AppendLine("<PositionType>" & xmlNoticeData.PushInfo.PositionType)
    '    '                .AppendLine("<Time>" & xmlNoticeData.PushInfo.Time)
    '    '                .AppendLine("<DisplayType>" & xmlNoticeData.PushInfo.DisplayType)
    '    '                .AppendLine("<DisplayContents>" & xmlNoticeData.PushInfo.DisplayContents)
    '    '                .AppendLine("<Color>" & xmlNoticeData.PushInfo.Color)
    '    '                .AppendLine("<PopWidth>" & xmlNoticeData.PushInfo.PopWidth)
    '    '                .AppendLine("<PopHeight>" & xmlNoticeData.PushInfo.PopHeight)
    '    '                .AppendLine("<PopX>" & xmlNoticeData.PushInfo.PopX)
    '    '                .AppendLine("<PopY>" & xmlNoticeData.PushInfo.PopY)
    '    '                .AppendLine("<DisplayFunction>" & xmlNoticeData.PushInfo.DisplayFunction)
    '    '                .AppendLine("<ActionFunction>" & xmlNoticeData.PushInfo.ActionFunction)
    '    '            End If
    '    '        End With
    '    '        Return log.ToString
    '    '    End Function

    '#End Region

    '2014/06/03 通知処理を承認入力から移植　Start
#End Region

#Region "通知"

#Region "通知用定数"

    ''' <summary>
    ''' 通知API用(カテゴリータイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPushCategory As String = "1"

    ''' <summary>
    ''' 通知API用(表示位置)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPotisionType As String = "1"

    ''' <summary>
    ''' 通知API用(表示時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyTime As Integer = 3

    ''' <summary>
    ''' 通知API用(表示タイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispType As String = "1"

    ''' <summary>
    ''' 通知API用(色)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyColor As String = "1"

    ''' <summary>
    ''' 通知API用(呼び出し関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispFunction As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' 通知履歴のSessionValue(カンマ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueKanma As String = ","

    ''' <summary>
    ''' 自社客のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MyPageLink As String = "<a id='SC3180201' Class='SC3180201' href='/Website/Pages/SC3180201.aspx' onclick='return ServiceLinkClick(event)'>"

    '2014/05/13 通知&PUSH処理追加　START　↓↓↓

    ''' <summary>
    ''' R/Oプレビューのリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoPreviewLink As String = "<a id='{0}0' Class='{0}' href='/Website/Pages/{0}.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 顧客名のリンク文字列(車両用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerVclLink As String = "<a id='{0}1' Class='{0}' href='/Website/Pages/{0}.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 顧客名のリンク文字列(顧客名用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerCstLink As String = "<a id='{0}2' Class='{0}' href='/Website/Pages/{0}.aspx' onclick='return ServiceLinkClick(event)'>"
    '2014/05/13 通知&PUSH処理追加　END　↑↑↑

    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

    ''' <summary>
    ''' 敬称利用区分("1"：後方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBack As String = "1"

    ''' <summary>
    ''' 敬称利用区分("2"：前方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeFront As String = "2"

    ''' <summary>
    ''' イベントキーID
    ''' </summary>
    Private Enum EventKeyId

        ''' <summary>
        ''' 共通処理
        ''' </summary>
        CommonProces = 100

        ''' <summary>
        ''' 承認処理
        ''' </summary>
        AproveProces = 200

        ''' <summary>
        ''' 最終承認処理
        ''' </summary>
        LastAproveProces = 201

        ''' <summary>
        ''' 承認依頼処理
        ''' </summary>
        SendProces = 300

    End Enum

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        ''' <summary>通知用文言("50"：完成検査承認)</summary>
        id50 = 50
        ''' <summary>通知用文言("51"：精算準備)</summary>
        id51 = 51
        ''' <summary>通知用文言("52"：完成検査否認)</summary>
        id52 = 52
    End Enum

    ''' <summary>
    ''' 画面遷移セッションキー(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyDealerCode As String = "SessionKey.DealerCode,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyBranchCode As String = "SessionKey.BranchCode,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyAccount As String = "SessionKey.LoginUserID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyVistSequence As String = "SessionKey.SAChipID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyResrveId As String = "SessionKey.BASREZID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyRepairorder As String = "SessionKey.R_O,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeySequence As String = "SessionKey.SEQ_NO,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyJobDtlId As String = "SessionKey.JOB_DTL_ID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyVin As String = "SessionKey.VIN,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(編集モード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyViewMode As String = "SessionKey.ViewMode,String,"

    '2014/07/16　セッション情報作成処理変更　START　↓↓↓
    ''' <summary>
    ''' 通知履歴のSessionValue(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDearlerCode As String = "Session.Param1,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBranchCode As String = "Session.Param2,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueLoginUserID As String = "Session.Param3,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSAChipID As String = "Session.Param4,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBASREZID As String = "Session.Param5,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueR_O As String = "Session.Param6,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSEQ_NO As String = "Session.Param7,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVIN_NO As String = "Session.Param8,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueViewMode As String = "Session.Param9,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(フォーマット「0：プレビュー」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueFormat As String = "Session.Param10,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(入庫管理番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInNum As String = "Session.Param11,String,"

    ''' <summary>
    '''  通知履歴のSessionValue(入庫販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInDlrCd As String = "Session.Param12,String,"

    ''' <summary>
    '''  通知履歴のSessionValue(入庫店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInBrnCd As String = "Session.Param13,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「5：R/O参照」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDisp_Num As String = "Session.DISP_NUM,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(顧客のDMSID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDmsCstId As String = "SessionKey.DMS_CST_ID,String,"


    '2014/07/16　セッション情報作成処理変更　END　　↑↑↑

#End Region

    '2014/05/16 通知&PUSH処理追加　START　↓↓↓
#Region "遷移先画面取得Dictionary用定数"

    ''' <summary>
    ''' 文言DB番号キー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyWordNo As String = "WordNo"

    ''' <summary>
    ''' R/O番号URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyRoNoURL As String = "RoNoURL"

    ''' <summary>
    ''' 車両番号URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyRegNoURL As String = "RegNoURL"

    ''' <summary>
    ''' お客様名URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyCutomerURL As String = "CutomerURL"

    ''' <summary>
    ''' 商品名URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyMerchandiseURL As String = "MerchandiseURL"

    ''' <summary>
    ''' 画面リフレッシュキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyPushMethod As String = "PushMethod"

    ''' <summary>
    ''' SMB画面リフレッシュメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSMB As String = "RefreshSMB()"

    ''' <summary>
    ''' FMメイン画面リフレッシュメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshFMMain As String = "MainRefresh()"

    '2014/12/06 完成検査承認時SAへのPUSH対応 START ↓↓↓
    ''' <summary>
    ''' SAメイン画面リフレッシュメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAMain As String = "MainRefresh()"
    '2014/12/06 完成検査承認時SAへのPUSH対応 END ↑↑↑

    ''' <summary>
    ''' ページID：SC3010501
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PageIdSC3010501 As String = "SC3010501"

    ''' <summary>
    ''' ページID：SC3010501　DispNum：13
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PageIdSC3010501_13 As String = "SC3010501-13"

    ''' <summary>
    ''' ページID：SC3080225
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PageIdSC3080225 As String = "SC3080225"

    ''' <summary>
    ''' 値無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicNone As String = ""

#End Region

#Region "PUSH処理用メッセージID"

    ''' <summary>
    ''' 正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' スタッフ情報が0件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdAccountInfoIsNull As Integer = 1101

    ''' <summary>
    ''' 販売店コードに該当するマスタデータが存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdDealerInfoIsNull As Integer = 1102

    ''' <summary>
    ''' 店舗コードに該当するマスタデータが存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdBranchInfoIsNull As Integer = 1103

    ''' <summary>
    ''' Push送信に失敗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdPushSendFailed As Integer = 6001

    ''' <summary>
    ''' システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSystemError As Integer = 9999

#End Region
    '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

#Region "Publicメソッド"

    ''' <summary>
    ''' 通知処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Sub NoticeProcessing(ByVal inStaffInfo As StaffContext, _
                                ByVal inSaChip As String, _
                                ByVal inBaserzId As String, _
                                ByVal inRoNumber As String, _
                                ByVal inSeqNo As String, _
                                ByVal inVin As String, _
                                ByVal inViewMode As String, _
                                ByVal inJobDtlId As String, _
                                ByVal inPresentTime As DateTime, _
                                ByVal inEventKey As String, _
                                ByVal noticeTarget As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ROUMBER[{2}] PRESENTTIME:{3} EVENTKEY:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRoNumber, inPresentTime, inEventKey))

        'SC3140103DataTableAdapterのインスタンス
        Dim da As New SC3180204TableAdapter

        '2014/05/14 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Directory作成
        Dim ScreenTransitionDictionary As Dictionary(Of String, Dictionary(Of String, String)) = Me.CreateScreenTransitionDictionary
        '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

        '通知送信用情報取得
        Dim dtNoticeProcessingInfo As SC3180204NoticeProcessingInfoDataTable = _
            da.GetNoticeProcessingInfo(inRoNumber, _
                                       inStaffInfo.DlrCD, _
                                       inStaffInfo.BrnCD, _
                                       Decimal.Parse(inJobDtlId))

        '通知送信用情報取得チェック
        If 0 < dtNoticeProcessingInfo.Count Then
            '取得できた場合

            'Rowに変換
            Dim rowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow = _
                DirectCast(dtNoticeProcessingInfo.Rows(0), SC3180204NoticeProcessingInfoRow)

            '現在日時を設定
            rowNoticeProcessingInfo.PRESENTTIME = inPresentTime

            'イベント情報判定
            Select Case inEventKey

                Case CType(EventKeyId.CommonProces, String)
                    '共通処理

                    '共通の実行
                    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                            inStaffInfo, _
                                            inSaChip, _
                                            inBaserzId, _
                                            inRoNumber, _
                                            inSeqNo, _
                                            inVin, _
                                            inViewMode, _
                                            inJobDtlId, _
                                            EventKeyId.CommonProces, _
                                            ScreenTransitionDictionary, _
                                            "")

                Case EventkeyLastRegistProces
                    '最終承認処理

                    '' ''2014/05/14 通知&PUSH処理追加　START　↓↓↓
                    '' ''承認の実行（依頼者へ通知）
                    ' ''Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                    ' ''                        inStaffInfo, _
                    ' ''                        inSaChip, _
                    ' ''                        inBaserzId, _
                    ' ''                        inRoNumber, _
                    ' ''                        inSeqNo, _
                    ' ''                        inVin, _
                    ' ''                        inViewMode, _
                    ' ''                        inJobDtlId, _
                    ' ''                        EventkeyLastRegistProces, _
                    ' ''                        ScreenTransitionDictionary)
                    '' ''2014/05/14 通知&PUSH処理追加　END　　↑↑↑

                    '2016/11/08 (TR-SVT-TMT-20160512-001) TBL_SERVICE_VISIT_MANAGEMENTにデータが無い場合はSA通知を送らない
                    Dim tableAdapter As New SC3180204TableAdapter
                    isExistSvcVisitMng = tableAdapter.GetSvcVisitManagementExist(inRoNumber, _
                                                                                 inStaffInfo.DlrCD, _
                                                                                 inStaffInfo.BrnCD)

                    '最終承認の実行（SAへ通知）
                    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                            inStaffInfo, _
                                            inSaChip, _
                                            inBaserzId, _
                                            inRoNumber, _
                                            inSeqNo, _
                                            inVin, _
                                            inViewMode, _
                                            inJobDtlId, _
                                            EventkeyLastRegistProces, _
                                            ScreenTransitionDictionary, _
                                            "")

                    'Case CType(EventKeyId.AproveProces, String)
                    '    '承認処理

                    '    '2014/05/14 通知&PUSH処理追加　START　↓↓↓
                    '    '承認の実行（依頼者へ通知）
                    '    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                    '                            inStaffInfo, _
                    '                            inSaChip, _
                    '                            inBaserzId, _
                    '                            inRoNumber, _
                    '                            inSeqNo, _
                    '                            inVin, _
                    '                            inViewMode, _
                    '                            inJobDtlId, _
                    '                            EventKeyId.AproveProces, _
                    '                            ScreenTransitionDictionary)
                    '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

                Case EventkeySendProces

                    '承認依頼（依頼者へ通知）
                    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                            inStaffInfo, _
                                            inSaChip, _
                                            inBaserzId, _
                                            inRoNumber, _
                                            inSeqNo, _
                                            inVin, _
                                            inViewMode, _
                                            inJobDtlId, _
                                            EventkeySendProces, _
                                            ScreenTransitionDictionary, _
                                            noticeTarget)

            End Select

            '2014/08/11 イベントキー「200：承認処理」は使用されていない為、削除　START　↓↓↓
            ''2014/05/13 通知&PUSH処理追加　START　↓↓↓
            ''Push通知によるメイン画面リフレッシュ
            '
            'Dim operationCdList As New List(Of Decimal)
            '
            ''イベント特定キーが「201：最終承認処理」なら、「200：承認処理」としてDictionaryキーを作成する
            'Dim ChangeEventKey As String
            'If EventkeyLastApproveProces = inEventKey Then
            '    ChangeEventKey = EventkeyApproveProces
            'Else
            '    ChangeEventKey = inEventKey
            'End If

            Dim ChangeEventKey As String = inEventKey

            '①ChTとCTにPUSH処理
            Dim TransDicKey = CreateTransDicKey(inStaffInfo.OpeCD, Operation.CT, ChangeEventKey)
            '2014/08/11 イベントキー「200：承認処理」は使用されていない為、削除　END　　↑↑↑

            'Push通知によるメイン画面リフレッシュ
            Dim operationCdList As New List(Of Decimal)

            If ScreenTransitionDictionary.ContainsKey(TransDicKey) Then
                '作成したDictionaryキーが遷移先画面取得Dictionaryに存在する
                If Not String.IsNullOrWhiteSpace(ScreenTransitionDictionary(TransDicKey)(TransDicKeyPushMethod)) Then
                    '指定したキーにPushMethodが登録されている

                    '権限リストの作成
                    operationCdList.Clear()
                    'OperationCodeリストに権限"55"：CTを設定
                    operationCdList.Add(Operation.CT)
                    'OperationCodeリストに権限"62"：CHTを設定
                    operationCdList.Add(Operation.CHT)

                    'PUSH処理
                    Me.SendGateNotice(inStaffInfo.DlrCD _
                                      , inStaffInfo.BrnCD _
                                      , operationCdList _
                                      , ScreenTransitionDictionary(TransDicKey)(TransDicKeyPushMethod) _
                                      )
                End If
            End If

            '②FMにPUSH処理
            Dim FMTransDicKey As String = CreateTransDicKey(inStaffInfo.OpeCD, Operation.FM, ChangeEventKey)

            If ScreenTransitionDictionary.ContainsKey(FMTransDicKey) Then
                '作成したDictionaryキーが遷移先画面取得Dictionaryに存在する
                If Not String.IsNullOrWhiteSpace(ScreenTransitionDictionary(FMTransDicKey)(TransDicKeyPushMethod)) Then
                    '指定したキーにPushMethodが登録されている

                    '権限リストの作成
                    operationCdList.Clear()
                    'OperationCodeリストに権限"58"：FAを設定
                    operationCdList.Add(Operation.FM)

                    'PUSH処理
                    Me.SendGateNotice(inStaffInfo.DlrCD _
                                      , inStaffInfo.BrnCD _
                                      , operationCdList _
                                      , ScreenTransitionDictionary(FMTransDicKey)(TransDicKeyPushMethod) _
                                      )
                End If
            End If
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑


            '2016/11/08 (TR-SVT-TMT-20160512-001) TBL_SERVICE_VISIT_MANAGEMENTにデータが無い場合はSA通知を送らない
            If isExistSvcVisitMng Then
                '2014/12/06 SAにPUSH送信　START　↓↓↓

                '③SAにPUSH処理
                Dim SATransDicKey As String = CreateTransDicKey(inStaffInfo.OpeCD, Operation.SA, ChangeEventKey)

                If ScreenTransitionDictionary.ContainsKey(SATransDicKey) Then
                    '作成したDictionaryキーが遷移先画面取得Dictionaryに存在する
                    If Not String.IsNullOrWhiteSpace(ScreenTransitionDictionary(SATransDicKey)(TransDicKeyPushMethod)) Then
                        '指定したキーにPushMethodが登録されている

                        Dim tableAdapter As New SC3180204TableAdapter
                        Dim user As SC3180204PicSaStfDataTable = Nothing

                        '担当SAユーザーの取得
                        user = tableAdapter.GetPicSaStf(inStaffInfo.DlrCD, inStaffInfo.BrnCD, inRoNumber)
                        Dim userdt As SC3180204PicSaStfRow = DirectCast(user.Rows(0), SC3180204PicSaStfRow)

                        '担当SAへPUSH処理
                        Me.SendGatePush(userdt.PIC_SA_STF_CD, ScreenTransitionDictionary(SATransDicKey)(TransDicKeyPushMethod))

                    End If
                End If
                '2014/12/06 SAにPUSH送信　END　　↑↑↑

            End If

        Else
            '取得失敗

            'エラーログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} GetNoticeProcessingInfo IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inTransDic」を追加</remarks>
    Private Sub NoticeMainProcessing(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                     ByVal inStaffInfo As StaffContext, _
                                     ByVal inSaChip As String, _
                                     ByVal inBaserzId As String, _
                                     ByVal inRoNumber As String, _
                                     ByVal inSeqNo As String, _
                                     ByVal inVin As String, _
                                     ByVal inViewMode As String, _
                                     ByVal inJobDtlId As String, _
                                     ByVal inEventKey As EventKeyId, _
                                     ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String)), _
                                     ByVal noticeTarget As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))

        Dim account As List(Of XmlAccount) = New List(Of XmlAccount)()
        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '依頼者の権限コード
        Dim inClientCode As String = Nothing
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        Select Case inEventKey

            Case EventKeyId.CommonProces

                '送信先アカウント情報設定
                account = Me.CreateAccount(inStaffInfo, inEventKey)

            Case EventKeyId.LastAproveProces

                '送信先アカウント情報設定(個別)
                account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey, inJobDtlId, inClientCode, "")

                '2014/08/11 イベントキー「200：承認処理」は使用されていない為、削除　START　↓↓↓
                'Case EventKeyId.AproveProces
                '
                '    '2014/05/14 通知&PUSH処理追加　START　↓↓↓
                '    '送信先アカウント情報設定(個別)
                '    account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey, inJobDtlId, inClientCode, "")
                '    '2014/05/14 通知&PUSH処理追加　END　　↑↑↑
                '2014/08/11 イベントキー「200：承認処理」は使用されていない為、削除　END　　↑↑↑

            Case EventkeySendProces
                '承認依頼の場合
                account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey, inJobDtlId, inClientCode, noticeTarget)
        End Select

        If account.Count = 0 Then
            Exit Sub
        End If

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey As String = CreateTransDicKey(CType(inStaffInfo.OpeCD, String), inClientCode, CType(inEventKey, String))

        Logger.Info(String.Format("ScreenTransitionDictionary, Key:[{0}], ContainsKey:[{1}]" _
                                  , TransDicKey _
                                  , inTransDic.ContainsKey(TransDicKey)))
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '「操作者権限、依頼者権限、イベント特定キー」が遷移先画面取得Dictionaryに登録されていたら通知処理を実行する
        If inTransDic.ContainsKey(TransDicKey) Then

            '通知履歴登録情報の設定
            Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(inRowNoticeProcessingInfo, _
                                                                           inStaffInfo, _
                                                                           inSaChip, _
                                                                           inBaserzId, _
                                                                           inRoNumber, _
                                                                           inSeqNo, _
                                                                           inVin, _
                                                                           inViewMode, _
                                                                           inJobDtlId, _
                                                                           inEventKey, _
                                                                           inClientCode, _
                                                                           inTransDic)

            'Push情報作成処理の設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeProcessingInfo _
                                                            , inEventKey _
                                                            , CType(inStaffInfo.OpeCD, String) _
                                                            , inClientCode _
                                                            , inTransDic _
                                                            )

            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData

                '現在時間データの格納
                noticeData.TransmissionDate = inRowNoticeProcessingInfo.PRESENTTIME
                '送信ユーザーデータ格納
                noticeData.AccountList.AddRange(account.ToArray)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNotice
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))

                    Try
                        Logger.Info(LogNoticeData(noticeData) &
                                    GetLogParam("noticeDisposalMode", CStr(NoticeDisposal.GeneralPurpose), True))

                    Catch ex As Exception
                        Logger.Error(ex.ToString)

                    End Try

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using
            End Using

        End If
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "通知独自部分"

    ''' <summary>
    ''' 送信先アカウント情報作成処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>送信先アカウント情報リスト</returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal inStaffInfo As StaffContext, _
                                   ByVal inEventKey As EventKeyId) As List(Of XmlAccount)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))

        '送信先アカウント情報リスト
        Dim accountList As New List(Of XmlAccount)

        'OperationCodeリスト
        Dim operationCodeList As New List(Of Long)

        'OperationCodeリストに権限"55"：CTを設定
        operationCodeList.Add(Operation.CT)

        'OperationCodeリストに権限"62"：CHTを設定
        operationCodeList.Add(Operation.CHT)

        'ユーザーステータス取得
        Using user As New IC3810601BusinessLogic

            'ユーザーステータス取得処理
            '各権限の全ユーザー情報取得
            Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                user.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
                                             inStaffInfo.BrnCD, _
                                             operationCodeList)

            'オンラインユーザー分ループ
            For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                '送信先アカウント情報 
                Using account As New XmlAccount

                    '受信先のアカウント設定
                    account.ToAccount = userRow.ACCOUNT

                    '受信先アカウントログ出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "ACCOUNT [{0}] " _
                              , userRow.ACCOUNT))

                    '受信者名設定
                    account.ToAccountName = userRow.USERNAME

                    '受信者名ログ出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "USERNAME [{0}] " _
                              , userRow.USERNAME))


                    '送信先アカウント情報リストに送信先アカウント情報を追加
                    accountList.Add(account)

                End Using

            Next

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return accountList


    End Function

    ''' <summary>
    ''' 送信先アカウント情報作成処理（個別）
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inRoNum">Ro番号</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>送信先アカウント情報リスト</returns>
    ''' <remarks></remarks>
    Private Function CreateAccountParticular(ByVal inStaffInfo As StaffContext, _
                                             ByVal inRoNum As String, _
                                             ByVal inEventKey As EventKeyId, _
                                             ByVal inJobDtlId As String, _
                                             ByRef inAccountCD As String, _
                                             ByRef inTagerAccount As String) As List(Of XmlAccount)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))

        '送信先アカウント情報リスト
        Dim accountList As New List(Of XmlAccount)

        'ユーザーステータス取得
        Dim tableAdapter As New SC3180204TableAdapter

        '2014/05/14 通知&PUSH処理追加　START　↓↓↓
        Dim user As SC3180204PicClientDataTable = Nothing
        Dim user_ctcht As SC3180204PicClientDataTable = Nothing
        Dim userdt_opcd As SC3180204PicClientRow = Nothing

        Select Case inEventKey
            ''Case EventkeyApproveProces, EventkeyRejectProces
            ''    '承認処理、否認処理　→　依頼者に通知するため、依頼者情報を取得
            ''    user = _
            ''        tableAdapter.GetPicClient(inStaffInfo.DlrCD, _
            ''                                 inStaffInfo.BrnCD, _
            ''                                 inRoNum, _
            ''                                 inJobDtlId)
            Case EventkeyLastRegistProces
                '最終承認処理　→　SAに通知するため、SA情報を取得
                ' TODO: ★製造中：SAの項目テーブル不明のため、仮としてサービス入庫テーブルのSAスタッフコードを取得
                user = _
                    tableAdapter.GetPicSaStf(inStaffInfo.DlrCD, _
                                             inStaffInfo.BrnCD, _
                                             inRoNum, _
                                             inJobDtlId)

                userdt_opcd =
                    DirectCast(user.Rows(0), SC3180204PicClientRow)


                '2016/11/08 (TR-SVT-TMT-20160512-001) TBL_SERVICE_VISIT_MANAGEMENTにデータが無い場合はSA通知を送らない
                If Not isExistSvcVisitMng Then
                    'SAへ送らない為、SA情報をクリア
                    user = Nothing

                End If

                user_ctcht = tableAdapter.GetPicCtChtStf(inStaffInfo.DlrCD, _
                         inStaffInfo.BrnCD, _
                         inJobDtlId)

                If isExistSvcVisitMng Then

                    If user_ctcht.Rows.Count > 0 Then
                        user.Merge(user_ctcht)
                    End If

                Else

                    user = user_ctcht

                End If

            Case EventkeySendProces
                user = tableAdapter.GetPicAppStf(inTagerAccount)

                userdt_opcd =
                    DirectCast(user.Rows(0), SC3180204PicClientRow)

                user_ctcht = tableAdapter.GetPicCtChtStf(inStaffInfo.DlrCD, _
                         inStaffInfo.BrnCD, _
                         inJobDtlId, _
                         userdt_opcd.ACCOUNT)

                If user_ctcht.Rows.Count > 0 Then
                    user.Merge(user_ctcht)
                End If

        End Select

        inAccountCD = userdt_opcd.OPERATIONCODE

        'Dim user As SC3180204PicSaStfDataTable = _
        '    tableAdapter.GetPicSaStf(inStaffInfo.DlrCD, _
        '                             inStaffInfo.BrnCD, _
        '                             inRoNum)
        '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

        Dim userdt As SC3180204PicClientRow

        For user_cnt = 0 To user.Rows.Count - 1


            'ユーザーステータス取得処理
            '2014/05/16 通知&PUSH処理追加　START　↓↓↓
            userdt = DirectCast(user.Rows(user_cnt), SC3180204PicClientRow)
            'Dim userdt As SC3180204PicSaStfRow =
            '    DirectCast(user.Rows(0), SC3180204PicSaStfRow)
            '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

            '送信先アカウント情報 
            Using account As New XmlAccount

                '受信先のアカウント設定
                '2014/05/16 通知&PUSH処理追加　START　↓↓↓
                account.ToAccount = userdt.ACCOUNT
                'account.ToAccount = userdt.PIC_SA_STF_CD
                '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

                '受信先アカウントログ出力
                '2014/05/16 通知&PUSH処理追加　START　↓↓↓
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "ACCOUNT [{0}] " _
                          , userdt.ACCOUNT))
                'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '          , "ACCOUNT [{0}] " _
                '          , userdt.PIC_SA_STF_CD))
                '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

                '受信者名設定
                account.ToAccountName = userdt.USERNAME

                '受信者名ログ出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "USERNAME [{0}] " _
                          , userdt.USERNAME))

                '送信先アカウント情報リストに送信先アカウント情報を追加
                accountList.Add(account)

            End Using
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return accountList

    End Function

    ''' <summary>
    ''' 通知履歴登録情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inClientCD">送信先（依頼者）権限</param>
    ''' <param name="inTransDic" >遷移先画面取得Dictionary</param>
    ''' <returns>通知履歴登録情報</returns>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inClientCD」「inTransDic」を追加</remarks>
    Private Function CreateRequestNotice(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                         ByVal inStaffInfo As StaffContext, _
                                         ByVal inSaChip As String, _
                                         ByVal inBaserzId As String, _
                                         ByVal inRoNumber As String, _
                                         ByVal inSeqNo As String, _
                                         ByVal inVin As String, _
                                         ByVal inViewMode As String, _
                                         ByVal inJobDtlId As String, _
                                         ByVal inEventKey As EventKeyId, _
                                         ByVal inClientCD As String, _
                                         ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'XmlRequestNoticeのインスタンス
        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inStaffInfo.DlrCD

            '店舗コード設定
            requestNotice.StoreCode = inStaffInfo.BrnCD

            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffInfo.Account

            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffInfo.UserName

            ''顧客種別(リンク制御で使用)
            'Dim customerType As Integer = MessageType.MyCustomer

            '通知履歴にリンクをつけるか判定
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            Select Case inEventKey

                Case EventKeyId.CommonProces

                    requestNotice.Message = Space(1)

                Case Else


                    '通知履歴用メッセージ作成設定
                    requestNotice.Message = Me.CreateNoticeRequestMessage(inRowNoticeProcessingInfo _
                                                                          , inEventKey _
                                                                          , CType(inStaffInfo.OpeCD, String) _
                                                                          , inClientCD _
                                                                          , inTransDic)

            End Select

            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowNoticeProcessingInfo, _
                                                                       inStaffInfo, _
                                                                       inSaChip, _
                                                                       inBaserzId, _
                                                                       inRoNumber, _
                                                                       inSeqNo, _
                                                                       inVin, _
                                                                       inViewMode, _
                                                                       inJobDtlId, _
                                                                       inEventKey, _
                                                                       CType(inStaffInfo.OpeCD, String), _
                                                                       inClientCD, _
                                                                       inTransDic)


            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return requestNotice

        End Using

    End Function

    ''' <summary>
    ''' 通知履歴用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inOperationCode">送信元（操作者）権限コード</param>
    ''' <param name="inClientCode">送信先（依頼者）権限コード</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <returns>通知履歴用メッセージ情報</returns>
    ''' <history>
    ''' </history>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inOperationCode」「inClientCode」「inTransDic」追加</remarks>
    Private Function CreateNoticeRequestMessage(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                                ByVal inEventKey As EventKeyId, _
                                                ByVal inOperationCode As String, _
                                                ByVal inClientCode As String, _
                                                ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メッセージ
        Dim workMessage As New StringBuilder

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey As String = CreateTransDicKey(inOperationCode, inClientCode, CType(inEventKey, String))

        '作成したキーが遷移先画面取得Dictionaryしているか
        If Not inTransDic.ContainsKey(TransDicKey) Then
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END, Not Contain Key." _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        'メッセージ組立処理

        '通知履歴にリンクをつける
        '自社客のAタグを設定
        'workMessage.Append(MyPageLink)

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '文言設定
        If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyWordNo)) Then
            workMessage.Append(WebWordUtility.GetWord(CType(inTransDic(TransDicKey)(TransDicKeyWordNo), Decimal)))
            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))
        Else
            workMessage.Append(Space(3))
        End If

        ''イベントごとに処置分岐
        'Select Case inEventKey
        '    Case EventKeyId.AproveProces
        '        '承認処理

        '        '文言：承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id50))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(1))

        '    Case EventKeyId.LastAproveProces

        '        '文言：最終承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id51))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(1))

        '    Case EventKeyId.RejectProces
        '        '否認処理

        '        '文言：否認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id52))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(1))

        'End Select
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        'メッセージ組立：RO番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.RO_NUM) Then

            Dim RO_LinkID As String = String.Empty

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRoNoURL)) Then
                'R/OプレビューのAタグを設定
                '2014/07/16　セッション情報作成処理変更　START　↓↓↓
                'Dim RO_LinkID As String = String.Empty
                If inTransDic(TransDicKey)(TransDicKeyRoNoURL).Contains(PageIdSC3010501 & "-") Then
                    RO_LinkID = PageIdSC3010501
                Else
                    RO_LinkID = inTransDic(TransDicKey)(TransDicKeyRoNoURL)
                End If

                '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                '顧客詳細画面（SC3080225）で顧客IDが無い時はリンクIDを空白にする
                If RO_LinkID = PageIdSC3080225 Then
                    If String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        RO_LinkID = ""
                    End If
                End If

                'リンクIDがあればAタグを追加する
                If RO_LinkID <> "" Then
                    workMessage.Append(String.Format(RoPreviewLink, RO_LinkID))
                End If
                '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑
                'workMessage.Append(String.Format(RoPreviewLink, inTransDic(TransDicKey)(TransDicKeyRoNoURL)))
                '2014/07/16　セッション情報作成処理変更　END　　↑↑↑
            End If
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'RO番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.RO_NUM)

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
            'If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRoNoURL)) Then
            If RO_LinkID <> "" Then
                'Aタグ終了を設定
                workMessage.Append(EndLikTag)
            End If
            '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))

        End If

        'メッセージ組立：REG番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.REG_NO) Then

            Dim Reg_LinkID As String = String.Empty

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRegNoURL)) Then
                '車両情報のAタグを設定
                '2014/07/16　セッション情報作成処理変更　START　↓↓↓
                'Dim Reg_LinkID As String = String.Empty
                If inTransDic(TransDicKey)(TransDicKeyRegNoURL).Contains(PageIdSC3010501 & "-") Then
                    Reg_LinkID = PageIdSC3010501
                Else
                    Reg_LinkID = inTransDic(TransDicKey)(TransDicKeyRegNoURL)
                End If

                '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                '顧客詳細画面（SC3080225）で顧客IDが無い時はリンクIDを空白にする
                If Reg_LinkID = PageIdSC3080225 Then
                    If String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        Reg_LinkID = ""
                    End If
                End If

                'リンクIDがあればAタグを追加する
                If Reg_LinkID <> "" Then
                    workMessage.Append(String.Format(CustomerVclLink, Reg_LinkID))
                End If
                '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑

                'workMessage.Append(String.Format(CustomerCstLink, inTransDic(TransDicKey)(TransDicKeyRegNoURL)))
                '2014/07/16　セッション情報作成処理変更　END　　↑↑↑
            End If
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'REG番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.REG_NO)

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
            'If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRegNoURL)) Then
            If Reg_LinkID <> "" Then
                'Aタグ終了を設定
                workMessage.Append(EndLikTag)
            End If
            '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))

        End If

        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.CST_NAME) Then
            'お客様名がある場合

            Dim Cutomer_LinkID As String = String.Empty

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '顧客詳細のAタグを設定
            If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyCutomerURL)) Then
                '2014/07/16　セッション情報作成処理変更　START　↓↓↓
                'Dim Cutomer_LinkID As String = String.Empty
                If inTransDic(TransDicKey)(TransDicKeyCutomerURL).Contains(PageIdSC3010501 & "-") Then
                    Cutomer_LinkID = PageIdSC3010501
                Else
                    Cutomer_LinkID = inTransDic(TransDicKey)(TransDicKeyCutomerURL)
                End If

                '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                '顧客詳細画面（SC3080225）で顧客IDが無い時はリンクIDを空白にする
                If Cutomer_LinkID = PageIdSC3080225 Then
                    If String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        Cutomer_LinkID = ""
                    End If
                End If

                'リンクIDがあればAタグを追加する
                If Cutomer_LinkID <> "" Then
                    workMessage.Append(String.Format(CustomerCstLink, Cutomer_LinkID))
                End If
                '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑

                'workMessage.Append(String.Format(CustomerVclLink, inTransDic(TransDicKey)(TransDicKeyCutomerURL)))
                '2014/07/16　セッション情報作成処理変更　END　　↑↑↑
            End If
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につける

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                'メッセージ間にスペースの設定
                'workMessage.Append(Space(1))

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

                'メッセージ間にスペースの設定
                'workMessage.Append(Space(1))

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

            End If

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
            'If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyCutomerURL)) Then
            If Cutomer_LinkID <> "" Then
                'Aタグ終了を設定
                workMessage.Append(EndLikTag)
            End If
            '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑
            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

        Else
            'お客様名がない場合

            '文言：お客様 設定
            workMessage.Append(Space(1))

        End If

        'メッセージ組立：商品名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then

            '商品名を設定
            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))

        End If

        '通知履歴にリンクをつける
        'Aタグ終了を設定
        'workMessage.Append(EndLikTag)

        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd

        '送信メッセージログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "MESSAGE [{0}]" _
            , notifyMessage))


        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns></returns>
    ''' <remarks>2014/07/16　パラメータ追加（Dictionary用）</remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                                ByVal inStaffInfo As StaffContext, _
                                                ByVal inSaChip As String, _
                                                ByVal inBaserzId As String, _
                                                ByVal inRoNumber As String, _
                                                ByVal inSeqNo As String, _
                                                ByVal inVin As String, _
                                                ByVal inViewMode As String, _
                                                ByVal inJobDtlId As String, _
                                                ByVal inEventKey As EventKeyId, _
                                                ByVal inOperationCode As String, _
                                                ByVal inClientCode As String, _
                                                ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim notifySession As String = String.Empty

        '2014/07/16　セッション情報作成処理変更　START　↓↓↓
        '通知用セッション情報作成処理
        'notifySession = CreateCustomerSession(inRowNoticeProcessingInfo, _
        '                                      inStaffInfo, _
        '                                      inSaChip, _
        '                                      inBaserzId, _
        '                                      inRoNumber, _
        '                                      inSeqNo, _
        '                                      inVin, _
        '                                      inViewMode, _
        '                                      inJobDtlId)

        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey As String = CreateTransDicKey(inOperationCode, inClientCode, CType(inEventKey, String))

        '作成したキーが遷移先画面取得Dictionaryしているか確認する
        If Not inTransDic.ContainsKey(TransDicKey) Then
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END, Not Contain Key." _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If
        'RO番号、車両番号、顧客名の遷移先ページIDを取得する
        Dim LinkPageIds(2) As String
        LinkPageIds(0) = inTransDic(TransDicKey)(TransDicKeyRoNoURL)
        LinkPageIds(1) = inTransDic(TransDicKey)(TransDicKeyRegNoURL)
        LinkPageIds(2) = inTransDic(TransDicKey)(TransDicKeyCutomerURL)

        Dim workNotifySession As New StringBuilder

        For Each LinkPageId As String In LinkPageIds
            If Not String.IsNullOrEmpty(LinkPageId) Then
                'ページIDがSC3010501か確認
                If LinkPageId.Contains(PageIdSC3010501 & "-") Then
                    'ページIDがSC3010501
                    'DispNumを取り出す
                    Dim DispNum As String = Replace(LinkPageId, PageIdSC3010501 & "-", "")
                    'DispNumにあわせてセッションキーを設定する
                    Select Case DispNum
                        Case "13"
                            '通知用セッション情報作成処理（ROプレビュー用）
                            workNotifySession.Append(CreateRoPreviewSession(inRowNoticeProcessingInfo, _
                                                                            inStaffInfo, _
                                                                            inSaChip, _
                                                                            inBaserzId, _
                                                                            inRoNumber, _
                                                                            inSeqNo, _
                                                                            inVin, _
                                                                            inViewMode, _
                                                                            inJobDtlId, _
                                                                            DispNum))
                        Case Else
                            'その他
                    End Select
                    'ElseIf LinkPageId = "None" Then
                    '    workNotifySession.Append("None")
                ElseIf LinkPageId.Contains(PageIdSC3080225) Then
                    'ページIDがSC3080225（顧客詳細画面）
                    '通知用セッション情報作成処理
                    '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                    If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        workNotifySession.Append(CreateCustomerSession(inRowNoticeProcessingInfo, _
                                                                       inStaffInfo, _
                                                                       inSaChip, _
                                                                       inBaserzId, _
                                                                       inRoNumber, _
                                                                       inSeqNo, _
                                                                       inVin, _
                                                                       inViewMode, _
                                                                       inJobDtlId))
                    End If
                    '2014/08/06 顧客IDが無い時はリンクを作成しない　END　↑↑↑
                Else
                    'ページIDがSC3010501/SC3080225以外
                    '通知用セッション情報作成処理
                    workNotifySession.Append(CreateOtherSession(inRowNoticeProcessingInfo, _
                                                                   inStaffInfo, _
                                                                   inSaChip, _
                                                                   inBaserzId, _
                                                                   inRoNumber, _
                                                                   inSeqNo, _
                                                                   inVin, _
                                                                   inViewMode, _
                                                                   inJobDtlId))

                End If

            End If
            'タブで分ける
            workNotifySession.Append(vbTab)
        Next


        notifySession = workNotifySession.ToString
        '2014/07/16　セッション情報作成処理変更　END　　↑↑↑

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifySession

    End Function

    '2014/07/16　セッション情報作成処理変更　START　↓↓↓
    ''' <summary>
    ''' 通知用セッション情報作成メソッド（SC3080225　顧客詳細画面用）
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateCustomerSession(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                           ByVal inStaffInfo As StaffContext, _
                                           ByVal inSaChip As String, _
                                           ByVal inBaserzId As String, _
                                           ByVal inRoNumber As String, _
                                           ByVal inSeqNo As String, _
                                           ByVal inVin As String, _
                                           ByVal inViewMode As String, _
                                           ByVal inJobDtlId As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder


        '基幹顧客IDのセッション設定
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
            '基幹顧客IDがある場合は設定
            '基幹顧客IDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueDmsCstId, inRowNoticeProcessingInfo.DMS_CST_CD.Trim)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueDmsCstId, "")

        End If

        'VINの設定
        If Not String.IsNullOrEmpty(inVin.Trim) Then
            'VINがある場合は設定
            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyVin, inVin.Trim)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionKeyVin, "")

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function

    ''' <summary>
    ''' ROプレビュー遷移の通知用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateRoPreviewSession(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                           ByVal inStaffInfo As StaffContext, _
                                           ByVal inSaChip As String, _
                                           ByVal inBaserzId As String, _
                                           ByVal inRoNumber As String, _
                                           ByVal inSeqNo As String, _
                                           ByVal inVin As String, _
                                           ByVal inViewMode As String, _
                                           ByVal inJobDtlId As String, _
                                           ByVal DispNum As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = Me.GetDmsDealerData(inStaffInfo)

        'DMS情報のチェック
        If Not (IsNothing(dtDmsCodeMapDataTable)) Then
            '取得できた場合
            'DMS販売店コードのセッション設定
            Me.SetSessionValueWord(workSession, SessionValueDearlerCode, dtDmsCodeMapDataTable(0).CODE1)
            'DMS店舗コードのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBranchCode, dtDmsCodeMapDataTable(0).CODE2)
        Else
            '取得できなかった場合
            'DMS販売店コードのセッション設定
            Me.SetSessionValueWord(workSession, SessionValueDearlerCode, inStaffInfo.DlrCD)
            'DMS店舗コードのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBranchCode, inStaffInfo.BrnCD)
        End If

        'LoginUserIDのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueLoginUserID, inStaffInfo.Account)

        '来店管理番号のセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSAChipID, inSaChip)


        'BASREZIDの設定
        If Not String.IsNullOrEmpty(inBaserzId) Then
            'BASREZIDがある場合は設定
            'BASREZIDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, inBaserzId)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, "")
        End If

        'R_Oの設定
        If Not String.IsNullOrEmpty(inRoNumber) Then
            'R_Oがある場合は設定

            'R_Oのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueR_O, inRoNumber)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueR_O, "")
        End If

        'SEQ_NOの設定
        If Not String.IsNullOrEmpty(SessionValueSEQ_NO) Then
            'SEQ_NOがある場合は設定

            'SEQ_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, inSeqNo)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, "")
        End If

        'VIN_NOの設定
        If Not String.IsNullOrEmpty(inVin) Then
            'VIN_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, inVin)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, "")
        End If

        'ViewModeのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueViewMode, "0")

        'Formatのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueFormat, "0")

        '入庫管理番号のセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInNum, "")

        '入庫販売店コードのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInDlrCd, "")

        '入庫店舗コードのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInBrnCd, "")

        'DISP_NUMのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueDisp_Num, DispNum)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function

    ''' <summary>
    ''' 通知用セッション情報作成メソッド（その他）
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateOtherSession(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                           ByVal inStaffInfo As StaffContext, _
                                           ByVal inSaChip As String, _
                                           ByVal inBaserzId As String, _
                                           ByVal inRoNumber As String, _
                                           ByVal inSeqNo As String, _
                                           ByVal inVin As String, _
                                           ByVal inViewMode As String, _
                                           ByVal inJobDtlId As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'DEARLERCODEの設定
        If Not String.IsNullOrEmpty(inStaffInfo.DlrCD.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyDealerCode, inStaffInfo.DlrCD.Trim)

        End If

        'BRANCHCODEの設定
        If Not String.IsNullOrEmpty(inStaffInfo.BrnCD.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyBranchCode, inStaffInfo.BrnCD.Trim)

        End If

        'ACCOUNTの設定
        If Not String.IsNullOrEmpty(inStaffInfo.Account.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyAccount, inStaffInfo.Account.Trim)

        End If

        'RO_NUMの設定
        If Not String.IsNullOrEmpty(inRoNumber.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyRepairorder, inRoNumber.Trim)

        End If

        'SEQ_NOの設定
        If Not String.IsNullOrEmpty(inSeqNo.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeySequence, inSeqNo.Trim)

        End If

        'JOB_DTL_IDの設定
        If Not String.IsNullOrEmpty(inJobDtlId.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyJobDtlId, inJobDtlId.Trim)

        End If

        'VINの設定
        If Not String.IsNullOrEmpty(inVin.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyVin, inVin.Trim)

        End If

        'VIEWMODEの設定
        If Not String.IsNullOrEmpty(inViewMode.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyViewMode, inViewMode.Trim)

        End If

        'SACHIPの設定
        If Not String.IsNullOrEmpty(inSaChip.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyVistSequence, inSaChip.Trim)

        End If

        'BASERZIDの設定
        If Not String.IsNullOrEmpty(inBaserzId.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyResrveId, inBaserzId.Trim)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function
    '2014/07/16　セッション情報作成処理変更　END　　↑↑↑

    ''' <summary>
    ''' SessionValue文字列作成
    ''' </summary>
    ''' <param name="workSession">追加元文字列</param>
    ''' <param name="SessionValueWord">追加するSESSIONKEY</param>
    ''' <param name="SessionValueData">追加するデータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSessionValueWord(ByVal workSession As StringBuilder, _
                                         ByVal SessionValueWord As String, _
                                         ByVal SessionValueData As String) As StringBuilder

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'カンマの設定
        If workSession.Length <> 0 Then
            'データがある場合

            '「,」を結合する
            workSession.Append(SessionValueKanma)

        End If

        'セッションキーを設定
        workSession.Append(SessionValueWord)

        'セッション値を設定
        workSession.Append(SessionValueData)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession

    End Function

    ''' <summary>
    ''' Push情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inOperationCode">送信元（操作者）権限コード</param>
    ''' <param name="inClientCode">送信先（依頼者）権限コード</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <returns>Push情報</returns>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inOperationCode」「inClientCode」「inTransDic」追加</remarks>
    Private Function CreatePushInfo(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                    ByVal inEventKey As EventKeyId, _
                                    ByVal inOperationCode As String, _
                                    ByVal inClientCode As String, _
                                    ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As XmlPushInfo

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'PUSH内容設定
        Using pushInfo As New XmlPushInfo

            'カテゴリータイプ設定
            pushInfo.PushCategory = NotifyPushCategory

            '表示位置設定
            pushInfo.PositionType = NotifyPotisionType

            '表示時間設定
            pushInfo.Time = NotifyTime

            '表示タイプ設定
            pushInfo.DisplayType = NotifyDispType

            Select Case inEventKey

                Case EventKeyId.CommonProces

                    'Push用メッセージ作成
                    pushInfo.DisplayContents = Space(3)

                Case EventkeySendProces

                    'Push用メッセージ作成
                    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey, inOperationCode, inClientCode, inTransDic)

                Case EventkeyLastRegistProces

                    'Push用メッセージ作成
                    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey, inOperationCode, inClientCode, inTransDic)

                    'Case EventKeyId.LastAproveProces

                    '    'Push用メッセージ作成
                    '    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

                    'Case EventKeyId.RejectProces

                    '    'Push用メッセージ作成
                    '    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

            End Select

            '色設定
            pushInfo.Color = NotifyColor

            '表示時関数設定
            pushInfo.DisplayFunction = NotifyDispFunction

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return pushInfo

        End Using
    End Function

    ''' <summary>
    ''' Push用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inOperationCode">送信元（操作者）権限コード</param>
    ''' <param name="inClientCode">送信先（依頼者）権限コード</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <returns>Puss用メッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inOperationCode」「inClientCode」「inTransDic」追加</remarks>
    Private Function CreatePusuMessage(ByVal inRowNoticeProcessingInfo As SC3180204NoticeProcessingInfoRow, _
                                       ByVal inEventKey As EventKeyId, _
                                       ByVal inOperationCode As String, _
                                       ByVal inClientCode As String, _
                                       ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey = CreateTransDicKey(inOperationCode, inClientCode, CType(inEventKey, String))

        '作成したキーが遷移先画面取得Dictionaryしているか
        If Not inTransDic.ContainsKey(TransDicKey) Then
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END, Not Contain Key." _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        'メッセージ
        Dim workMessage As New StringBuilder

        'メッセージ組立処理

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyWordNo)) Then
            workMessage.Append(WebWordUtility.GetWord(CType(inTransDic(TransDicKey)(TransDicKeyWordNo), Decimal)))
            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))
        Else
            workMessage.Append(Space(3))
        End If

        ''イベントごとに処置分岐
        'Select Case inEventKey
        '    Case EventKeyId.CommonProces
        '        '共通処理

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))


        '    Case EventKeyId.AproveProces
        '        '承認処理

        '        '文言：承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id50))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))

        '    Case EventKeyId.LastAproveProces

        '        '文言：最終承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id51))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))

        '    Case EventKeyId.RejectProces
        '        '否認処理

        '        '文言：否認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id52))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))

        'End Select
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑


        'メッセージ組立：RO番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.RO_NUM) Then

            'RO番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.RO_NUM)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：REG番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.REG_NO) Then

            'REG番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.REG_NO)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If


        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.CST_NAME) Then
            'お客様名がある場合

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につけつ

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

            End If

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        Else
            'お客様名がない場合
            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：商品名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then
            '商品名がある場合

            '商品名を設定
            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

        End If


        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd


        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

#End Region

#Region "ログデータ加工処理"

    ''' <summary>
    ''' ログデータ（メソッド）
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function GetLogMethod(ByVal methodName As String,
                                  ByVal startEndFlag As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            .Append("[")
            .Append(methodName)
            .Append("]")
            If startEndFlag Then
                .Append(" method start")
            Else
                .Append(" method end")
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（引数）
    ''' </summary>
    ''' <param name="paramName">引数名</param>
    ''' <param name="paramData">引数値</param>
    ''' <param name="kanmaFlag">True：引数名の前に「,」を表示、False：特になし</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function GetLogParam(ByVal paramName As String,
                                 ByVal paramData As String,
                                 ByVal kanmaFlag As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            If kanmaFlag Then
                .Append(",")
            End If
            .Append(paramName)
            .Append("=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function

#End Region

#Region "XmlNoticeDataログデータ加工処理"

    ''' <summary>
    ''' XmlNoticeDataログデータ加工処理
    ''' </summary>
    ''' <param name="xmlNoticeData">XmlNoticeDataクラス</param>
    ''' <returns>ログ情報</returns>
    ''' <remarks></remarks>
    Private Function LogNoticeData(ByVal xmlNoticeData As XmlNoticeData) As String

        Dim log As New StringBuilder

        With log
            '見やすくするために改行
            .AppendLine("")
            .AppendLine("000･･･")
            .AppendLine("<TransmissionDate>" & CStr(xmlNoticeData.TransmissionDate))
            .AppendLine("100･･･")
            For Each accountData In xmlNoticeData.AccountList
                .AppendLine("<ToAccount>" & accountData.ToAccount)
                .AppendLine("<ToClientID>" & accountData.ToClientId)
                .AppendLine("<ToAccountName>" & accountData.ToAccountName)
            Next
            .AppendLine("200･･･")
            .AppendLine("<DealerCode>" & xmlNoticeData.RequestNotice.DealerCode)
            .AppendLine("<StoreCode>" & xmlNoticeData.RequestNotice.StoreCode)
            .AppendLine("<RequestClass>" & xmlNoticeData.RequestNotice.RequestClass)
            .AppendLine("<Status>" & xmlNoticeData.RequestNotice.Status)
            .AppendLine("<RequestID>" & xmlNoticeData.RequestNotice.RequestId)
            .AppendLine("<RequestClassID>" & xmlNoticeData.RequestNotice.RequestClassId)
            .AppendLine("<FromAccount>" & xmlNoticeData.RequestNotice.FromAccount)
            .AppendLine("<FromClientID>" & xmlNoticeData.RequestNotice.FromClientId)
            .AppendLine("<FromAccountName>" & xmlNoticeData.RequestNotice.FromAccountName)
            .AppendLine("<CustomID>" & xmlNoticeData.RequestNotice.CustomId)
            .AppendLine("<CustomName>" & xmlNoticeData.RequestNotice.CustomName)
            .AppendLine("<CustomerClass>" & xmlNoticeData.RequestNotice.CustomerClass)
            .AppendLine("<CstKind>" & xmlNoticeData.RequestNotice.CustomerKind)
            .AppendLine("<Message>" & xmlNoticeData.RequestNotice.Message)
            .AppendLine("<SessionValue>" & xmlNoticeData.RequestNotice.SessionValue)
            .AppendLine("<SalesStaffCode>" & xmlNoticeData.RequestNotice.SalesStaffCode)
            .AppendLine("<VehicleSequenceNumber>" & xmlNoticeData.RequestNotice.VehicleSequenceNumber)
            .AppendLine("<FollowUpBoxStoreCode>" & xmlNoticeData.RequestNotice.FollowUpBoxStoreCode)
            .AppendLine("<FollowUpBoxNumber>" & xmlNoticeData.RequestNotice.FollowUpBoxNumber)
            ' $01 start step2開発
            .AppendLine("<CSPaperName>" & xmlNoticeData.RequestNotice.CSPaperName)
            ' $01 end   step2開発
            .AppendLine("300･･･")
            If Not IsNothing(xmlNoticeData.PushInfo) Then
                .AppendLine("<PushCategory>" & xmlNoticeData.PushInfo.PushCategory)
                .AppendLine("<PositionType>" & xmlNoticeData.PushInfo.PositionType)
                .AppendLine("<Time>" & xmlNoticeData.PushInfo.Time)
                .AppendLine("<DisplayType>" & xmlNoticeData.PushInfo.DisplayType)
                .AppendLine("<DisplayContents>" & xmlNoticeData.PushInfo.DisplayContents)
                .AppendLine("<Color>" & xmlNoticeData.PushInfo.Color)
                .AppendLine("<PopWidth>" & xmlNoticeData.PushInfo.PopWidth)
                .AppendLine("<PopHeight>" & xmlNoticeData.PushInfo.PopHeight)
                .AppendLine("<PopX>" & xmlNoticeData.PushInfo.PopX)
                .AppendLine("<PopY>" & xmlNoticeData.PushInfo.PopY)
                .AppendLine("<DisplayFunction>" & xmlNoticeData.PushInfo.DisplayFunction)
                .AppendLine("<ActionFunction>" & xmlNoticeData.PushInfo.ActionFunction)
            End If
        End With
        Return log.ToString
    End Function

#End Region

    '2014/05/13 通知&PUSH処理追加　START　↓↓↓
#Region "Push送信（メイン画面リフレッシュ）"

    ''' <summary>
    ''' PUSH処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <returns>終了コード</returns>
    ''' <remarks>PUSH処理（メイン画面リフレッシュ）を送信する</remarks>
    <EnableCommit()>
    Public Function SendGateNotice(ByVal dealerCode As String _
                                   , ByVal storeCode As String _
                                   , ByVal operationCdList As List(Of Decimal) _
                                   , ByVal pushMethod As String) As Integer
        'SendGateNotice開始ログ出力
        Dim sendGateNoticeStartLogInfo As New StringBuilder
        sendGateNoticeStartLogInfo.Append("SendGateNotice_Start ")
        sendGateNoticeStartLogInfo.Append("dealerCode[" & dealerCode & "]")
        sendGateNoticeStartLogInfo.Append(",storeCode[" & storeCode & "]")
        sendGateNoticeStartLogInfo.Append(",pushMethod[" & pushMethod & "]")
        Logger.Info(sendGateNoticeStartLogInfo.ToString())

        '終了コード
        Dim resultId As Integer = MessageIdSuccess

        'Logger.Info("SendGateNotice_001 " & "MasterCheck_Start")

        ''マスターチェック
        'resultId = IsVaildMaster(dealerCode, storeCode)

        'If resultId <> MessageIdSuccess Then

        '    Logger.Error("SendGateNotice_002 " & "IsVaildMaster NG")

        '    'エラー出力
        '    Logger.Warn("ResultId : " & CStr(resultId))

        '    'チェックに引っかかっていたら返却
        '    Logger.Error("SendGateNotice_End Ret[" & CStr(resultId) & "]")
        '    Return resultId
        'End If

        'Logger.Info("SendGateNotice_003 " & "IsVaildMaster OK")

        'ユーザマスタから販売店コード、店舗コード、権限リストを条件にスタッフ情報を取得
        Dim users As Users = New Users
        'Dim operationCdList As New List(Of Decimal)

        ''OperationCodeリストに権限"55"：CTを設定
        'operationCdList.Add(Operation.CT)

        ''OperationCodeリストに権限"62"：CHTを設定
        'operationCdList.Add(Operation.CHT)

        'userData取得ログ出力
        Dim userDataStartLogInfo As New StringBuilder
        userDataStartLogInfo.Append("SendGateNotice_004 " & "Call_Start users.GetAllUser ")
        userDataStartLogInfo.Append("param1[" & dealerCode & "]")
        userDataStartLogInfo.Append(",param2[" & storeCode & "]")
        userDataStartLogInfo.Append(",param3[" & operationCdList.Item(0).ToString(CultureInfo.InvariantCulture()) & "]")
        Logger.Info(userDataStartLogInfo.ToString())

        '販売店コード、店舗コード、権限リストを元にスタッフ情報を取得
        Dim userData As UsersDataSet.USERSDataTable = users.GetAllUser(dealerCode, storeCode, operationCdList)

        'userData取得ログ出力
        Dim userDataEndLogInfo As New StringBuilder
        userDataEndLogInfo.Append("SendGateNotice_004 " & "Call_End users.GetAllUser ")
        userDataEndLogInfo.Append("Ret[" & userData.ToString & "]")
        Logger.Info(userDataEndLogInfo.ToString())

        'スタッフ情報チェック
        If userData.Count = 0 Then

            userDataStartLogInfo.Append("SendGateNotice_005 NotStaffInfo")

            'スタッフ情報が0件
            resultId = MessageIdAccountInfoIsNull

            'エラー出力
            Logger.Warn("ResultId : " & CStr(resultId))

            Logger.Error("SendGateNotice_End Ret[" & CStr(resultId) & "]")
            Return resultId
        End If

        'マスタチェック終了
        Logger.Info("SendGateNotice_006 MasterCheck_End")

        ''デバッグログ出力(来店日時取得開始)
        'Logger.Info("SendGateNotice_007 " & "Call_Start DateTimeFunc.Now Param[" & dealerCode & "]")

        ''日付管理機能から来店日時(現在日時)を販売店コードを元に取得
        'Dim visitTimeStamp As Date = DateTimeFunc.Now(dealerCode)

        ''デバッグログ出力(来店日時取得終了)
        'Logger.Info("SendGateNotice_008 " & "Call_End DateTimeFunc.Now Ret[" & visitTimeStamp & "]")

        'CTとCHTスタッフのアカウントを取得
        For Each target As UsersDataSet.USERSRow In userData

            'Push機能にて、スタッフ端末へ、ゲート通知送信命令を送信
            SendGatePush(target.ACCOUNT, pushMethod)
        Next

        '終了デバッグログ出力
        Dim sendGateNoticeEndLogInfo As New StringBuilder
        sendGateNoticeEndLogInfo.Append("SendGateNotice_End ")
        sendGateNoticeEndLogInfo.Append("Ret[" & CStr(resultId) & "]")
        Logger.Info(sendGateNoticeEndLogInfo.ToString())

        Return resultId
    End Function

#End Region

#Region "マスターデータチェック"

    ''' <summary>
    ''' マスターデータチェックメソッド
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <returns>チェック結果を終了コードで返却</returns>
    ''' <remarks></remarks>
    Private Function IsVaildMaster(ByVal dlrCd As String, ByVal strCd As String) As Integer

        'マスタチェック開始
        Dim startLogMaster As New StringBuilder
        startLogMaster.Append("IsVaildMaster_Start ")
        startLogMaster.Append("param1[" & dlrCd & "]")
        startLogMaster.Append(",param2[" & strCd & "]")
        Logger.Info(startLogMaster.ToString())

        '販売店コードの存在チェック
        Logger.Info("IsVaildMaster_001 Call_Start dealers.GetDealer Param[" & dlrCd & "]")
        Dim dealers As Dealer = New Dealer
        Dim dealerData As DealerDataSet.DEALERRow = dealers.GetDealer(dlrCd)

        '指定した販売店コードが取れなかった場合
        If dealerData Is Nothing Then

            Logger.Error("IsVaildMaster_002  dealerData Is Nothing")

            '終了ログ
            Logger.Error("IsVaildMaster_End Ret[" & MessageIdDealerInfoIsNull & "]")
            Return MessageIdDealerInfoIsNull
        End If
        Logger.Info("IsVaildMaster_001 Call_End dealers.GetDealer Ret[" & dealerData.ToString & "]")

        ''店舗コードの存在チェック
        'Dim storesLogMaster As New StringBuilder
        'storesLogMaster.Append("IsVaildMaster_003 Call_Start stores.GetBranch ")
        'storesLogMaster.Append("param1[" & dlrCd & "]")
        'storesLogMaster.Append(",param2[" & strCd & "]")
        'Logger.Info(storesLogMaster.ToString())
        'Dim stores As Branch = New Branch
        'Dim storesData As BranchDataSet.BRANCHRow = stores.GetBranch(dlrCd, strCd)

        ''指定した店舗コードが取れなかった場合
        'If storesData Is Nothing Then

        '    Logger.Info("IsVaildMaster_004  storesData Is Nothing")

        '    '終了ログ
        '    Logger.Info("IsVaildMaster_End Ret[" & MessageIdBranchInfoIsNull & "]")
        '    Return MessageIdBranchInfoIsNull
        'End If
        'Logger.Info("IsVaildMaster_003 Call_End stores.GetBranch Ret[" & storesData.ToString & "]")


        '終了ログ
        Logger.Info("IsVaildMaster_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess
    End Function

#End Region

#Region "Push送信実行"

    ''' <summary>
    ''' PUSH処理（画面リフレッシュ）を送信
    ''' </summary>
    ''' <param name="accountCd">アカウント</param>
    ''' <remarks>push送信を行う</remarks>
    Private Sub SendGatePush(ByVal accountCd As String, ByVal pushMethod As String)

        'デバッグログ出力(PUSH開始)
        Dim sendGatePushStartLogInfo As New StringBuilder
        sendGatePushStartLogInfo.Append("SendGatePush_Start ")
        sendGatePushStartLogInfo.Append("param1[" & accountCd & "]")
        sendGatePushStartLogInfo.Append(", pushMethod[" & pushMethod & "]")
        Logger.Info(sendGatePushStartLogInfo.ToString())

        'POST送信する文字列を作成する
        Dim postMsg As New StringBuilder
        With postMsg
            .Append("cat=action")
            .Append("&type=main")
            .Append("&sub=js")
            .Append("&uid=" & accountCd)
            .Append("&time=0")
            '.Append("&js1=sc3090301pushRecv()")
            .Append(String.Format("&js1={0}", pushMethod))
        End With

        'Push送信を行う
        Dim util As New VisitUtility
        util.SendPush(postMsg.ToString)

        'デバッグログ出力(PUSH終了)
        Logger.Info("SendGatePush_End")

    End Sub
#End Region

#Region "遷移先画面取得Dictionary"

    ''' <summary>
    ''' 遷移先画面取得Dictionaryを作成する
    ''' </summary>
    ''' <remarks>遷移先画面取得Dictionaryを作成する</remarks>
    Private Function CreateScreenTransitionDictionary() _
                                              As Dictionary(Of String, Dictionary(Of String, String))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim ScreenTransDic As Dictionary(Of String, Dictionary(Of String, String)) = New Dictionary(Of String, Dictionary(Of String, String))


        '送信元：送信先：操作
        'TC	FM	[承認依頼	有]:300	50	完成検査依頼 [R/O] [REG] [CUST NAME] [点検名称]	R/Oプレビュー画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.TEC, Operation.FM, EventkeySendProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshFMMain))

        'TC	CT	[承認依頼	有]:300	50	完成検査依頼 [R/O] [REG] [CUST NAME] [点検名称]	R/Oプレビュー画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.TEC, Operation.CT, EventkeySendProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))

        'TC	ChT	[承認依頼	有]:300	50	完成検査依頼 [R/O] [REG] [CUST NAME] [点検名称]	R/Oプレビュー画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.TEC, Operation.CHT, EventkeySendProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))



        'TC	SA	[登録	有]:201	51	清算準備 [R/O] [REG] [CUST NAME] [点検名称]	R/O作成画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.TEC, Operation.SA, EventkeyLastRegistProces), _
                           TransDicValue(CType(MsgID.id51, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSAMain)) '担当SAPUSH

        'TC	CT	[登録	有]:201
        ScreenTransDic.Add(CreateTransDicKey(Operation.TEC, Operation.CT, EventkeyLastRegistProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'TC	ChT	[登録	有]:201
        ScreenTransDic.Add(CreateTransDicKey(Operation.TEC, Operation.CHT, EventkeyLastRegistProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'TC	SA	[登録	有]:400		清算準備 [R/O] [REG] [CUST NAME] [点検名称]	R/O作成画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.TEC, Operation.SA, EventkeyFromPreview), _
                           TransDicValue(CType(MsgID.id51, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSAMain)) '担当SAPUSH



        'ChT	FM	[承認依頼	有]:300	50	完成検査依頼 [R/O] [REG] [CUST NAME] [点検名称]	R/Oプレビュー画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.FM, EventkeySendProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshFMMain))

        'ChT	CT	[承認依頼	有]:300	50	完成検査依頼 [R/O] [REG] [CUST NAME] [点検名称]	R/Oプレビュー画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CT, EventkeySendProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))

        'ChT	ChT	[承認依頼	有]:300	50	完成検査依頼 [R/O] [REG] [CUST NAME] [点検名称]	R/Oプレビュー画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CHT, EventkeySendProces), _
                   TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))



        'ChT	SA	[登録	有]:201	51	清算準備 [R/O] [REG] [CUST NAME] [点検名称]	R/O作成画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.SA, EventkeyLastRegistProces), _
                   TransDicValue(CType(MsgID.id51, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSAMain)) '担当SAPUSH

        'ChT	CT	[登録	有]:201
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CT, EventkeyLastRegistProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'ChT	ChT	[登録	有]:201
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CHT, EventkeyLastRegistProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))



        'ChT	SA	[登録	無]:400	52	完成検査結果変更 [R/O] [REG] [CUST NAME] [点検名称]	R/O作成画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.SA, EventkeyFromPreview), _
                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSAMain)) '担当SAPUSH

        'ChT	CT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'ChT	ChT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CHT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))



        'FM	SA	[登録	無]:400	52	完成検査結果変更 [R/O] [REG] [CUST NAME] [点検名称]	R/O作成画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.SA, EventkeyFromPreview), _
                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))

        'FM	CT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'FM	ChT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CHT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))



        'CT	SA	[登録	無]:400	52	完成検査結果変更 [R/O] [REG] [CUST NAME] [点検名称]	R/O作成画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.SA, EventkeyFromPreview), _
                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))

        'CT	CT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'CT	ChT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CHT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))



        'SM	SA	[登録	無]:400	52	完成検査結果変更 [R/O] [REG] [CUST NAME] [点検名称]	R/O作成画面	顧客詳細画面	
        ScreenTransDic.Add(CreateTransDicKey(Operation.SM, Operation.SA, EventkeyFromPreview), _
                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))

        'SM	CT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.SM, Operation.CT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'SM	ChT	[登録	無]:400
        ScreenTransDic.Add(CreateTransDicKey(Operation.SM, Operation.CHT, EventkeyFromPreview), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))


        '' FM： TC：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.TEC, EventkeyApproveProces), _
        '                   TransDicValue(CType(MsgID.id50, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        '' FM：ChT：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CHT, EventkeyApproveProces), _
        '                   TransDicValue(CType(MsgID.id50, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        '' FM： SA：最終承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.SA, EventkeyLastApproveProces), _
        '                   TransDicValue(CType(MsgID.id51, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        '' FM： FM：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.FM, EventkeyApproveProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        '' FM： CT：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CT, EventkeyApproveProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))
        '' FM： TC：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.TEC, EventkeyRejectProces), _
        '                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        '' FM：ChT：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CHT, EventkeyRejectProces), _
        '                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        '' FM： FM：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.FM, EventkeyRejectProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        '' FM： CT：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CT, EventkeyRejectProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        '' CT： TC：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.TEC, EventkeyApproveProces), _
        '                   TransDicValue(CType(MsgID.id50, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        '' CT：ChT：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CHT, EventkeyApproveProces), _
        '                   TransDicValue(CType(MsgID.id50, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        '' CT： SA：最終承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.SA, EventkeyLastApproveProces), _
        '                   TransDicValue(CType(MsgID.id51, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        '' CT： FM：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.FM, EventkeyApproveProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        '' CT： CT：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CT, EventkeyApproveProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))
        '' CT： TC：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.TEC, EventkeyRejectProces), _
        '                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        '' CT：ChT：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CHT, EventkeyRejectProces), _
        '                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        '' CT： FM：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.FM, EventkeyRejectProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        '' CT： CT：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CT, EventkeyRejectProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        ''ChT： TC：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.TEC, EventkeyApproveProces), _
        '                   TransDicValue(CType(MsgID.id50, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        ''ChT：ChT：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CHT, EventkeyApproveProces), _
        '                   TransDicValue(CType(MsgID.id50, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        ''ChT： SA：最終承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.SA, EventkeyLastApproveProces), _
        '                   TransDicValue(CType(MsgID.id51, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        ''ChT： FM：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.FM, EventkeyApproveProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        ''ChT： CT：承認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CT, EventkeyApproveProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))
        ''ChT： TC：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.TEC, EventkeyRejectProces), _
        '                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        ''ChT：ChT：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CHT, EventkeyRejectProces), _
        '                   TransDicValue(CType(MsgID.id52, String), PageIdSC3010501, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        ''ChT： FM：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.FM, EventkeyRejectProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        ''ChT： CT：否認
        'ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CT, EventkeyRejectProces), _
        '                   TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ScreenTransDic

    End Function

    ''' <summary>
    ''' 遷移先画面取得Dictionaryのキーを作成する
    ''' </summary>
    ''' <param name="OperationKey">送信元（操作者）権限コード</param>
    ''' <param name="ClientKey">送信先（依頼者）権限コード</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <remarks>遷移先画面取得Dictionaryのキーを作成する</remarks>
    Private Function CreateTransDicKey(ByVal OperationKey As String _
                                     , ByVal ClientKey As String _
                                     , ByVal inEventKey As String _
                                     ) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START, OperationKey:[{2}], ClientKey:[{3}], inEventKey:[{4}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , OperationKey _
                    , ClientKey _
                    , inEventKey))


        Dim ChangeEventKey As String
        ''イベント特定キーが「201：最終承認処理」なら、「200：承認処理」としてDictionaryキーを作成する
        'If EventkeyLastApproveProces = inEventKey Then
        '    ChangeEventKey = EventkeyApproveProces
        'Else
        '    ChangeEventKey = inEventKey
        'End If
        ChangeEventKey = inEventKey

        Dim DicKey As String = String.Format("{0};{1};{2}", OperationKey, ClientKey, ChangeEventKey)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , DicKey))

        Return DicKey

    End Function

    ''' <summary>
    ''' 遷移先画面取得Dictionaryに追加する
    ''' </summary>
    ''' <param name="WordNo">文言DB番号</param>
    ''' <param name="RoNoURL">R/Oリンク先ID</param>
    ''' <param name="RegNoURL">車両番号リンク先ID</param>
    ''' <param name="CutomerURL">お客様リンク先ID</param>
    ''' <param name="PushMethod">Push処理（画面更新）時のJavaScript関数名</param>
    ''' <remarks>遷移先画面取得Dictionaryに追加する</remarks>
    Private Function TransDicValue(ByVal WordNo As String _
                                 , ByVal RoNoURL As String _
                                 , ByVal RegNoURL As String _
                                 , ByVal CutomerURL As String _
                                 , ByVal PushMethod As String _
                                 ) As Dictionary(Of String, String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START, WordNo:[{2}], RoNoURL:[{3}], RegNoURL:[{4}], CutomerURL:[{5}], PushMethod:[{6}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , WordNo _
                    , RoNoURL _
                    , RegNoURL _
                    , CutomerURL _
                    , PushMethod))

        Dim DicValueInfo As Dictionary(Of String, String) = New Dictionary(Of String, String)
        DicValueInfo.Add(TransDicKeyWordNo, WordNo)
        DicValueInfo.Add(TransDicKeyRoNoURL, RoNoURL)
        DicValueInfo.Add(TransDicKeyRegNoURL, RegNoURL)
        DicValueInfo.Add(TransDicKeyCutomerURL, CutomerURL)
        DicValueInfo.Add(TransDicKeyPushMethod, PushMethod)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return_Count:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , DicValueInfo.Count))

        Return DicValueInfo

    End Function

#End Region
    '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

#End Region
    '2014/06/03 通知処理を承認入力から移植　End

#Region "ROステータス取得処理"
    '2014/06/06 未使用になったためコメント　Start
    ' ''' <summary>
    ' ''' ROステータス取得
    ' ''' </summary>
    ' ''' <param name="jobDtlId">作業内容ID</param>
    ' ''' <returns>ヘッダー情報</returns>
    ' ''' <remarks></remarks>
    'Public Function GetDBRoState(ByVal jobDtlId As String) As SC3180204RoStateDataTable

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '検索処理
    '    Dim tableAdapter As New SC3180204TableAdapter
    '    Dim dtROStatusInfo As SC3180204RoStateDataTable

    '    dtROStatusInfo = tableAdapter.GetDBRoState(jobDtlId)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '処理結果返却
    '    Return dtROStatusInfo

    'End Function
    '2014/06/06 未使用になったためコメント　End

#End Region

#Region "DMS情報取得"

    ''' <summary>
    ''' DMS情報取得
    ''' </summary>
    ''' <param name="inStaffInfo">sスタッフ情報</param>
    ''' <returns>DMS情報</returns>
    ''' <remarks></remarks>
    Public Function GetDmsDealerData(ByVal inStaffInfo As StaffContext) As DmsCodeMapDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New ServiceCommonClassBusinessLogic
            'DMS販売店データの取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                biz.GetIcropToDmsCode(inStaffInfo.DlrCD,
                                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                      inStaffInfo.DlrCD, _
                                      inStaffInfo.BrnCD, _
                                      String.Empty, _
                                      inStaffInfo.Account)

            If dtDmsCodeMapDataTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing
                '2015/04/14 新販売店追加対応 start
                '全販売店を含む複数取得の場合は1件目を参照する。エラーとしないようコメントアウト
                'ElseIf 1 < dtDmsCodeMapDataTable.Count Then
                '    'データが2件以上取得できた場合は一意に決定できないためエラー
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is sum data" _
                '                , Me.GetType.ToString _
                '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '    Return Nothing
                '2015/04/14 新販売店追加対応 end
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dtDmsCodeMapDataTable
            End If

        End Using
    End Function

    ''' <summary>
    ''' DMS情報取得
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <returns>DMS情報</returns>
    ''' <remarks></remarks>
    Public Function GetDmsDealerData(ByVal dealerCD As String, ByVal branchCD As String) As DmsCodeMapDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New ServiceCommonClassBusinessLogic
            'DMS販売店データの取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                biz.GetIcropToDmsCode(dealerCD,
                                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                      dealerCD, _
                                      branchCD, _
                                      String.Empty, _
                                      String.Empty)

            If dtDmsCodeMapDataTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dtDmsCodeMapDataTable
            End If

        End Using
    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "外部連携"

#Region "完成検査データ更新"
    ''' <summary>
    ''' 完成検査データ更新
    ''' </summary>
    ''' <param name="intUpdateStatus">更新区分</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="strSendUser">送信先ユーザー</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="dtfUpdate">更新日付</param>
    ''' <remarks>True:成功/False:失敗</remarks>
    Private Function InspectionUpdate(ByVal intUpdateStatus As Integer, _
                                      ByVal dealerCD As String, _
                                      ByVal decNowJobDtlID As Decimal, _
                                      ByVal decServiceID As Decimal, _
                                      ByVal strSendUser As String, _
                                      ByVal strAccount As String, _
                                      ByVal dtfUpdate As Date) As Boolean

        Dim update As Date = DateTimeFunc.Now(dealerCD)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "JOB_DTL_ID [{0}]" _
        '          , decNowJobDtlID.ToString))

        If True = blnResult Then
            blnResult = SetDBInspection(decNowJobDtlID, _
                                        strAccount, _
                                        strSendUser, _
                                        intUpdateStatus, _
                                        dtfUpdate, _
                                        decServiceID)
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "blnResult [{0}]" _
        '          , blnResult.ToString))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return blnResult
    End Function
#End Region

#Region "SMBチップ更新"
    ''' <summary>
    ''' SMBチップ更新 Finish
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <remarks>True:成功/False:失敗</remarks>
    Private Function Finish(ByVal dealerCD As String, _
                            ByVal decServiceID As Decimal, _
                            ByVal decStallID As Decimal, _
                            ByVal strApplicationID As String) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                          , "StallId [{0}] " _
        '                          , decStallID))

        Dim commonUtility As New TabletSMBCommonClassBusinessLogic
        Dim finishResult As Long = -1

        'サービス入庫 行ロックバージョン取得
        Dim lockVersion = GetServiceInLock(decServiceID)
        finishResult = commonUtility.Finish(decStallID, _
                                            DateTimeFunc.Now(dealerCD), _
                                            "0", _
                                            DateTimeFunc.Now(dealerCD), _
                                            lockVersion, _
                                            strApplicationID)
        If arySuccessList.Contains(finishResult) Then
            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} True END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
            'Return True
        Else
            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} False END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
            'Return False
        End If

        Return finishResult
    End Function
#End Region

#Region "基幹連携独自更新"

#Region "実行部分"
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <remarks>True:成功/False:失敗</remarks>
    Public Function SelfFinish(ByVal dealerCD As String, _
                               ByRef decNowJobDtlID As Decimal, _
                               ByVal decServiceID As Decimal, _
                               ByVal decStallID As Decimal, _
                               ByVal strApplicationID As String, _
                               ByVal prevStatus As String, _
                               ByVal prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable) As Boolean
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "Stall_use_id [{0}]:ServiceID [{1}]:JOB_DTL_ID [{2}]" _
        '          , decStallID _
        '          , decServiceID _
        '          , decNowJobDtlID))

        Dim finishResult As Long = -1

        finishResult = Finish_update(decStallID, decServiceID, decNowJobDtlID, strApplicationID, prevStatus, prevJobStatus)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "self_finish result [{0}]" _
                   , finishResult.ToString))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'If finishResult = 0 Then
        '    Return True
        'Else
        '    Return False
        'End If

        Return finishResult
    End Function
#End Region

#Region "更新部分"

    ''' <summary>
    ''' G10とG13を実行
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="serviceId">サービスID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Public Function Finish_update(ByVal stallUseId As Decimal, _
                                  ByVal serviceId As Decimal, _
                                  ByVal jobDtlId As Decimal, _
                                  ByVal systemId As String, _
                                  ByVal prevStatus As String, _
                                  ByVal prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable) As Long
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '***********************************************************************
        ' 1. いろいろな値を準備する
        '***********************************************************************

        '更新前のステータス取得
        Dim crntStatus As String = JudgeChipStatus(stallUseId)

        '***********************************************************************
        ' 2. 実行
        '***********************************************************************

        Dim dmsSendResult As Long
        '基幹側にステータス情報を送信(G13)
        Using ic3802601blc As New IC3802601BusinessLogic

            dmsSendResult = ic3802601blc.SendStatusInfo(serviceId, _
                                                        jobDtlId, _
                                                        stallUseId, _
                                                        prevStatus, _
                                                        crntStatus, _
                                                        0)
            If Not arySuccessList.Contains(dmsSendResult) Then
                'Return ActionResult.DmsLinkageError
                Return dmsSendResult
            End If
        End Using

        'If IsUseJobDispatch() Then
        '作業ステータスを取得する
        Dim crntJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable = JudgeJobStatus(jobDtlId)

        '基幹側にJobDispatch実績情報を送信(G10)
        Dim resultSendJobClock As Long = SendJobClockOnInfo(serviceId, _
                                                            jobDtlId, _
                                                            prevJobStatus, _
                                                            crntJobStatus)

        If Not arySuccessList.Contains(resultSendJobClock) Then
            'Return ActionResult.DmsLinkageError
            Return resultSendJobClock
        End If

        'End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If dmsSendResult <> ActionResult.Success Then
            'ワーニング返却
            Return dmsSendResult
        ElseIf resultSendJobClock <> ActionResult.Success Then
            'ワーニング返却
            Return resultSendJobClock
        Else
            ' 正常終了
            Return ActionResult.Success
        End If

    End Function

#End Region

#Region "JobDispatch送信"
    ''' <summary>
    ''' 基幹連携(JobDispatch実績情報送信処理)を行う(メイン)
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevJobStatus">更新前作業連携ステータス</param>
    ''' <param name="crntJobStatus">更新後作業連携ステータス</param>
    ''' <returns>ActionResult</returns>
    ''' <remarks></remarks>
    Public Function SendJobClockOnInfo(ByVal svcinId As Decimal, _
                                       ByVal jobDtlId As Decimal, _
                                       ByVal prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable, _
                                       ByVal crntJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable) As Long

        Using IC3802701Biz As New IC3802701BusinessLogic
            Dim dmsSendResult As Long
            dmsSendResult = IC3802701Biz.SendJobClockOnInfo(svcinId, _
                                                            jobDtlId, _
                                                            prevJobStatus, _
                                                            crntJobStatus)
            ' 2015/5/1 強制納車対応  start
            If Not arySuccessList.Contains(dmsSendResult) Then
                Return ActionResult.DmsLinkageError
            Else
                Return dmsSendResult
            End If
            ' 2015/5/1 強制納車対応  end
        End Using

    End Function
#End Region

#Region "各ステータス"

#Region "作業ステータス"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusWorking As String = "0"
    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusFinish As String = "1"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusStop As String = "2"

    ''' <summary>
    ''' 制御用作業ステータス：開始前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusBeforeWork As String = "101"
    ''' <summary>
    ''' 制御用作業ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusWorking As String = "102"
    ''' <summary>
    ''' 制御用作業ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusFinish As String = "103"
    ''' <summary>
    ''' 制御用作業ステータス：中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusStop As String = "104"
#End Region

#Region "サービスステータス"
    ''' <summary>
    ''' 未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNotCarin As String = "00"
    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNoShow As String = "01"

    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWorkOrderWait As String = "03"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStartwait As String = "04"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStart As String = "05"
    ''' <summary>
    ''' 次の作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNextStartWait As String = "06"
    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashWait As String = "07"
    ''' <summary>
    ''' 洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashStart As String = "08"
    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionWait As String = "09"
    ''' <summary>
    ''' 検査中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionStart As String = "10"
    ''' <summary>
    ''' 預かり中（DropOff）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDropOffCustomer As String = "11"
    ''' <summary>
    ''' 納車待ち（Waiting）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWaitingCustomer As String = "12"
    ''' <summary>
    ''' 納車済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDelivery As String = "13"
#End Region

#Region "チップステータス"
    ''' <summary>
    ''' 未入庫(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNotCarIn As String = "1"

    ''' <summary>
    ''' 未入庫(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedNotCarIn As String = "2"

    ''' <summary>
    ''' 作業開始待ち(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeWaitStart As String = "3"

    ''' <summary>
    ''' 作業開始待ち(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedWaitStart As String = "4"

    ''' <summary>
    ''' 仮置き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTemp As String = "5"

    ''' <summary>
    ''' 未来店客(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusNoshow As String = "6"

    ''' <summary>
    ''' 飛び込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWalkin As String = "7"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWorking As String = "8"

    ''' <summary>
    ''' 作業中断：部品欠品
    ''' </summary>
    Private Const ChipStatusStopForPartsStockout As String = "9"

    ''' <summary>
    ''' 作業中断：顧客連絡待ち
    ''' </summary>
    Private Const ChipStatusStopForWaitCustomer As String = "10"

    ''' <summary>
    ''' 作業中断：ストール待ち
    ''' </summary>
    Private Const ChipStatusStopForWaitStall As String = "11"

    ''' <summary>
    ''' 作業中断：その他
    ''' </summary>
    Private Const ChipStatusStopForOtherReason As String = "12"

    ''' <summary>
    ''' 作業中断：検査中断
    ''' </summary>
    Private Const ChipStatusStopForInspection As String = "13"

    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    Private Const ChipStatusWaitWash As String = "14"

    ''' <summary>
    ''' 洗車中
    ''' </summary>
    Private Const ChipStatusWashing As String = "15"

    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    Private Const ChipStatusWaitInspection As String = "16"

    ''' <summary>
    ''' 検査中
    ''' </summary>
    Private Const ChipStatusInspecting As String = "17"

    ''' <summary>
    ''' 預かり中
    ''' </summary>
    Private Const ChipStatusKeeping As String = "18"

    ''' <summary>
    ''' 納車待ち
    ''' </summary>
    Private Const ChipStatusWaitDelivery As String = "19"

    ''' <summary>
    ''' 作業完了
    ''' </summary>
    Private Const ChipStatusJobFinish As String = "20"

    ''' <summary>
    ''' 日跨ぎ終了
    ''' </summary>
    Private Const ChipStatusDateCrossEnd As String = "21"

    ''' <summary>
    ''' 納車済み
    ''' </summary>
    Private Const ChipStatusDeliveryEnd As String = "22"

    ''' <summary>
    ''' 未来店客(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNoShow As String = "24"

#End Region

#Region "予約ステータス"
    ''' <summary>
    ''' 仮予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResvStatusTentative As String = "0"

    ''' <summary>
    ''' 本予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResvStatusConfirmed As String = "1"
#End Region

#Region "ストール利用ステータス"
    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusWorkOrderWait As String = "00"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStartWait As String = "01"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStart As String = "02"
    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusFinish As String = "03"
    ''' <summary>
    ''' 作業計画の一部の作業が中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStartIncludeStopJob As String = "04"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStop As String = "05"
    ''' <summary>
    ''' 日跨ぎ終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusMidfinish As String = "06"
    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusNoshow As String = "07"
#End Region

#Region "中断理由区分"
    ''' <summary>
    ''' 部品欠品
    ''' </summary>
    Private Const StopReasonPartsStockOut As String = "01"
    ''' <summary>
    ''' お客様連絡待ち
    ''' </summary>
    Private Const StopReasonCustomerReportWaiting As String = "02"
    ''' <summary>
    ''' 検査不合格
    ''' </summary>
    Private Const StopReasonInspectionFailure As String = "03"
    ''' <summary>
    ''' その他
    ''' </summary>
    Private Const StopReasonOthers As String = "99"
#End Region

#Region "既定値"
    ''' <summary>
    ''' DB数値型の既定値（0）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultNumberValue As Long = 0
#End Region

#End Region

#Region "Jobステータス判定"

    ''' <summary>
    ''' 該チップに紐付く作業のステータスを取得する
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <returns>作業ステータステーブル</returns>
    ''' <remarks></remarks>
    Private Function JudgeJobStatus(ByVal jobDtlId As Decimal) As IC3802701DataSet.IC3802701JobStatusDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim jobStatusTable As TabletSMBCommonClassDataSet.TabletSmbCommonClassJobResultDataTable = Nothing
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            '作業単位でステータスを取得する
            jobStatusTable = ta.GetJobStatusByJob(jobDtlId)
        End Using

        '戻す用テーブル
        Using retJobStatusTable As New IC3802701DataSet.IC3802701JobStatusDataTable
            For Each jobStatusRow As TabletSMBCommonClassDataSet.TabletSmbCommonClassJobResultRow In jobStatusTable
                Dim retJobStatusRow As IC3802701DataSet.IC3802701JobStatusRow = retJobStatusTable.NewIC3802701JobStatusRow

                '値の設定
                retJobStatusRow.JOB_DTL_ID = jobStatusRow.JOB_DTL_ID
                retJobStatusRow.JOB_INSTRUCT_ID = jobStatusRow.JOB_INSTRUCT_ID
                retJobStatusRow.JOB_INSTRUCT_SEQ = jobStatusRow.JOB_INSTRUCT_SEQ

                '作業ステータスを設定する
                '作業前(実績テーブルに作業指示のレコードがないので、DBNULLだ)
                If jobStatusRow.IsJOB_STATUSNull Then
                    retJobStatusRow.JOB_STATUS = JobLinkStatusBeforeWork
                Else
                    'Linkテーブル用の作業ステータスに変更する
                    Select Case jobStatusRow.JOB_STATUS
                        Case JobStatusFinish
                            '終了
                            retJobStatusRow.JOB_STATUS = JobLinkStatusFinish
                        Case JobStatusWorking
                            '作業中
                            retJobStatusRow.JOB_STATUS = JobLinkStatusWorking
                        Case JobStatusStop
                            '中断
                            retJobStatusRow.JOB_STATUS = JobLinkStatusStop
                    End Select
                End If

                '一行追加
                retJobStatusTable.AddIC3802701JobStatusRow(retJobStatusRow)
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture _
                       , "{0}.{1} END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'ログ
            'Me.OutPutIFLog(retJobStatusTable, "IC3802701JobStatusDataTable:")

            Return retJobStatusTable
        End Using

    End Function

#End Region

#Region "チップステータス判定"
    ''' <summary>
    ''' チップのステータス判定
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 1：未入庫(仮予約)、2：未入庫(本予約)、3：作業開始待ち(仮予約)、
    ''' 4：作業開始待ち(本予約)、6：未来店客、7：飛び込み客、
    ''' 8：作業中、9：中断・部品欠品、10：中断・お客様連絡待ち、
    ''' 11：中断・ストール待機、12：中断・その他、13：中断・検査中断、
    ''' 14：洗車待ち、15：洗車中、16：検査待ち、17：検査中、18：預かり中、
    ''' 19：納車待ち、20：次の作業開始待ち、22：納車済み
    ''' </returns>
    ''' <remarks></remarks>
    Public Function JudgeChipStatus(ByVal stallUseId As Decimal) As String

        '戻り値
        Dim retValue As String = String.Empty

        'エラー発生フラグ
        Dim errorFlg As Boolean = False

        'チップエンティティ
        Dim chipEntityTable As New TabletSMBCommonClassDataSet.TabletSmbCommonClassChipEntityDataTable

        'TabletSMBCommonClassのテーブルアダプタークラスインスタンスを生成
        Using myTableAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter()
            'チップ情報取得
            chipEntityTable = myTableAdapter.GetChipEntity(stallUseId, 0)
            'Me.OutPutIFLog(chipEntityTable, "ChipEntityTable:") 'ログ
        End Using

        'チップ情報が取得できない場合はエラー
        If chipEntityTable.Count <= 0 Then
            errorFlg = True
            Return String.Empty
        End If

        'データ行の抜き出し
        Dim chipEntityRow As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipEntityRow _
            = DirectCast(chipEntityTable.Rows(0), TabletSMBCommonClassDataSet.TabletSmbCommonClassChipEntityRow)
        'サービスステータス
        Dim svcStatus As String = chipEntityRow.SVC_STATUS
        '予約ステータス
        Dim resvStatus As String = chipEntityRow.RESV_STATUS
        'ストール利用ステータス
        Dim stallUseStatus As String = chipEntityRow.STALL_USE_STATUS
        '中断理由区分
        Dim stopReasonType As String = chipEntityRow.STOP_REASON_TYPE
        '関連ストール非稼動ID
        Dim stallIdleId As Decimal = chipEntityRow.STALL_IDLE_ID
        '受付区分
        Dim acceptanceType As String = chipEntityRow.ACCEPTANCE_TYPE
        'ストールID
        Dim stallId As Decimal = chipEntityRow.STALL_ID
        'サービスステータスによって分岐
        Select Case svcStatus

            Case SvcStatusNotCarin
                'サービスステータス「00：未入庫」の場合
                retValue = Me.JudgeNotCarInStatus(resvStatus)

            Case SvcStatusNoShow
                'サービスステータス「01：未来店客」の場合
                retValue = Me.JudgeNoShowStatus(resvStatus, stallUseStatus)

            Case SvcStatusWorkOrderWait, SvcStatusStartwait, SvcStatusNextStartWait
                'サービスステータス「03：着工指示待ち」「04：作業開始待ち」「06：次の作業開始待ち」の場合
                retValue = Me.JudgeWaitStartStatus(resvStatus, stallUseStatus, stopReasonType, stallIdleId, acceptanceType, stallId)

            Case SvcStatusStart
                'サービスステータス「05：作業中」
                retValue = Me.JudgeStartStatus(stallUseStatus, stopReasonType, stallIdleId)

            Case SvcStatusCarWashWait
                'サービスステータス「07：洗車待ち」→チップステータス【14:洗車待ち】
                retValue = ChipStatusWaitWash

            Case SvcStatusCarWashStart
                'サービスステータス「08：洗車中」→チップステータス【15:洗車中】
                retValue = ChipStatusWashing

            Case SvcStatusInspectionWait
                'サービスステータス「09：検査待ち」→チップステータス【16:検査待ち】
                retValue = ChipStatusWaitInspection

            Case SvcStatusInspectionStart
                'サービスステータス「10：検査中」→チップステータス【17:検査中】
                retValue = ChipStatusInspecting

            Case SvcStatusDropOffCustomer
                'サービスステータス「11：預かり中」→チップステータス【18:預かり中】
                retValue = ChipStatusKeeping

            Case SvcStatusWaitingCustomer
                'サービスステータス「12：納車待ち」→チップステータス【19:納車待ち】
                retValue = ChipStatusWaitDelivery

            Case SvcStatusDelivery
                'サービスステータス「13：納車済み」→チップステータス【22:納車済み】
                retValue = ChipStatusDeliveryEnd

        End Select

        Return retValue

    End Function
#End Region

#Region "チップステータス"

    ''' <summary>
    ''' サービスステータス「00:未入庫」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeNotCarInStatus(ByVal resvStatus As String) As String

        '戻り値
        Dim retValue As String = String.Empty

        If resvStatus.Equals(ResvStatusTentative) Then
            'チップステータス【1：未入庫(仮予約)】
            retValue = ChipStatusTentativeNotCarIn
        Else
            'チップステータス【2：未入庫(本予約)】
            retValue = ChipStatusConfirmedNotCarIn
        End If

        Return retValue

    End Function

    ''' <summary>
    ''' サービスステータス「01：未来店客」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeNoShowStatus(ByVal resvStatus As String, ByVal stallUseStatus As String) As String

        '戻り値
        Dim retValue As String = String.Empty

        If stallUseStatus.Equals(StalluseStatusNoshow) Then
            'ストール利用ステータス「07：未来店客」の場合
            'チップステータス【6：未来店客】
            retValue = ChipStatusNoshow

        ElseIf stallUseStatus.Equals(StalluseStatusWorkOrderWait) _
        OrElse stallUseStatus.Equals(StalluseStatusStartWait) Then
            'ストール利用ステータス「00：着工指示待ち」または「01：作業開始待ち」の場合

            If resvStatus.Equals(ResvStatusTentative) Then
                'チップステータス【1：未入庫(仮予約)】
                retValue = ChipStatusTentativeNotCarIn
            Else
                'チップステータス【2：未入庫(本予約)】
                retValue = ChipStatusConfirmedNotCarIn
            End If
        End If

        Return retValue

    End Function

    ''' <summary>
    ''' サービスステータス「03:着工指示待ち」「04:作業開始待ち」「06:次の作業開始待ち」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <param name="acceptanceType">受付区分</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeWaitStartStatus(ByVal resvStatus As String, ByVal stallUseStatus As String, _
                                          ByVal stopReasonType As String, ByVal stallIdleId As Decimal, _
                                          ByVal acceptanceType As String, ByVal stallId As Decimal) As String

        '戻り値
        Dim retValue As String = String.Empty

        'ストール利用ステータスで分岐
        Select Case stallUseStatus

            Case StalluseStatusFinish
                'ストール利用ステータス「03：完了」の場合
                'チップステータス【20：次の作業開始待ち】
                retValue = ChipStatusJobFinish

            Case StalluseStatusStop
                'ストール利用ステータス「05：中断」の場合
                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合

                If acceptanceType.Equals(AcceptanceTypeWalkin) _
                AndAlso stallId = DefaultNumberValue Then
                    '受付区分が「1：Walk-in」、かつストールIDが未設定の場合
                    'チップステータス【7：Walk-in】
                    retValue = ChipStatusWalkin

                Else
                    If resvStatus.Equals(ResvStatusTentative) Then
                        'チップステータス【3：作業開始待ち(仮予約)】
                        retValue = ChipStatusTentativeWaitStart
                    Else
                        'チップステータス【4：作業開始待ち(本予約)】
                        retValue = ChipStatusConfirmedWaitStart
                    End If
                End If

        End Select

        Return retValue

    End Function

    ''' <summary>
    ''' ストール利用ステータス「05:中断」の場合のチップステータス判定
    ''' </summary>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 9：中断・部品欠品、10：中断・お客様連絡待ち、11：中断・ストール待機、
    ''' 12：中断・その他、13：中断・検査中断
    ''' </returns>
    ''' <remarks></remarks>
    Private Function JudgeStopStatus(ByVal stopReasonType As String, ByVal stallIdleId As Decimal) As String

        '戻り値
        Dim retValue As String = String.Empty

        If stallIdleId <> DefaultNumberValue Then
            '関連ストール非稼動IDが設定されている場合
            'チップステータス【11：作業中断(ストール待ち)】
            retValue = ChipStatusStopForWaitStall
        Else
            '中断理由区分で分岐
            Select Case stopReasonType
                Case StopReasonPartsStockOut
                    '中断理由区分「01：部品欠品」の場合
                    'チップステータス【09：作業中断(部品欠品)】
                    retValue = ChipStatusStopForPartsStockout

                Case StopReasonCustomerReportWaiting
                    '中断理由区分「02：お客様連絡待ち」の場合
                    'チップステータス【10：作業中断(お客様連絡待ち)】
                    retValue = ChipStatusStopForWaitCustomer

                Case StopReasonInspectionFailure
                    '中断理由区分「03：検査不合格」の場合
                    'チップステータス【13：作業中断(検査中断)】
                    retValue = ChipStatusStopForInspection

                Case StopReasonOthers
                    '中断理由区分が「99：その他」の場合
                    'チップステータス【12：作業中断(その他)】
                    retValue = ChipStatusStopForOtherReason

                Case Else
                    '中断理由区分が上記以外の場合
                    'チップステータス【12：作業中断(その他)】
                    retValue = ChipStatusStopForOtherReason
            End Select
        End If

        Return retValue

    End Function

    ''' <summary>
    ''' サービスステータス「05:作業中」の場合のチップステータス判定
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 4：作業開始待ち（本予約）、8：作業中、9：中断・部品欠品、
    ''' 10：中断・お客様連絡待ち、11：中断・ストール待機、12：中断・その他、13：中断・検査中断、20：作業完了
    ''' </returns>
    ''' <remarks></remarks>
    Private Function JudgeStartStatus(ByVal stallUseStatus As String, ByVal stopReasonType As String, ByVal stallIdleId As Decimal) As String

        '戻り値
        Dim retValue As String = String.Empty

        'ストール利用ステータスで分岐
        Select Case stallUseStatus

            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合
                'チップステータス【4：作業開始待ち(本予約)】
                retValue = ChipStatusConfirmedWaitStart

            Case StalluseStatusStart, StalluseStatusStartIncludeStopJob
                'ストール利用ステータス「02：作業中」「04：作業指示の一部の作業が中断」の場合
                'チップステータス【8：作業中】
                retValue = ChipStatusWorking

            Case StalluseStatusStop
                'ストール利用ステータス「05：中断」の場合
                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

            Case StalluseStatusFinish
                'ストール利用ステータス「03：完了」の場合
                'チップステータス【20：次の作業開始待ち】
                retValue = ChipStatusJobFinish

        End Select

        Return retValue

    End Function
#End Region

#Region "作業実績送信使用フラグ"
    ''' <summary>
    ''' 作業実績送信使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysParaNameJobDispatchUseFlg As String = "JOBDISPATCH_USE_FLG"
#End Region

#Region "Job Dispatch運用判定"
    ''' <summary>
    ''' Job Dispatch運用を行うか否かを設定
    ''' </summary>
    ''' <returns>作業実績送信使用フラグ True:使用</returns>
    ''' <remarks></remarks>
    Private Function IsUseJobDispatch() As Boolean
        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            'Job Dispatch運用フラグ
            Dim jobDispatchUseFlg As String = serviceCommonBiz.GetDlrSystemSettingValueBySettingName(SysParaNameJobDispatchUseFlg)

            If String.IsNullOrEmpty(jobDispatchUseFlg) Then
                Return False
            Else
                '使用の場合、trueを戻す
                If jobDispatchUseFlg.Trim().Equals("1") Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Using
    End Function

#End Region

#Region "OutPutIFLog"
    'Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

    '    If dt Is Nothing Then
    '        Return
    '    End If

    '    Logger.Error(ifName + " Result START " + " OutPutCount: " + (dt.Rows.Count).ToString(CultureInfo.InvariantCulture))

    '    Dim log As New Text.StringBuilder

    '    For j = 0 To dt.Rows.Count - 1

    '        log = New Text.StringBuilder()
    '        Dim dr As DataRow = dt.Rows(j)

    '        log.Append("RowNum: " + (j + 1).ToString(CultureInfo.InvariantCulture) + " -- ")

    '        For i = 0 To dt.Columns.Count - 1
    '            log.Append(dt.Columns(i).Caption)
    '            If IsDBNull(dr(i)) Then
    '                log.Append(" IS NULL")
    '            Else
    '                log.Append(" = ")
    '                log.Append(dr(i).ToString)
    '            End If

    '            If i <= dt.Columns.Count - 2 Then
    '                log.Append(", ")
    '            End If
    '        Next

    '        Logger.Error(log.ToString)
    '    Next

    '    Logger.Error(ifName + " Result END ")

    'End Sub
#End Region

#End Region

    ''操作
    'Enum OperetionMode
    '    ApprovalRequest = 0  '承認依頼
    '    Registration = 1     '登録
    '    Approval = 2         '承認
    '    Denial = 3           '否認
    'End Enum
    '
    ''Job_Dtl_Id
    'Enum IdType
    '    Yes = 0 '有
    '    No = 1  '無
    'End Enum
    '
    ''文言ID
    'Enum WordNo
    '    No53 = 53
    '    No54 = 54
    '    No55 = 55
    '    No56 = 56
    '    No57 = 57
    '    No58 = 58
    '    No59 = 59
    '    No60 = 60
    '    No61 = 61
    '    No62 = 62
    '    No63 = 63
    '    No64 = 64
    '    No65 = 65
    '    No66 = 66
    '    No67 = 67
    '    No68 = 68
    '    No69 = 69
    '    No70 = 70
    '    No71 = 71
    '    No72 = 72
    '    No73 = 73
    '    No74 = 74
    '    No75 = 75
    '    No76 = 76
    '    No77 = 77
    '    No78 = 78
    '    No79 = 79
    '    No80 = 80
    '
    'End Enum
    '
    'Dim NotificationTable As New DataTable
    'Dim PushTable As New DataTable
    '
    'Private Sub Notification(ByVal inSource As String, ByVal inDestination As String, ByVal inOperation As OperetionMode, ByVal inIDFlg As IdType)
    '
    '    Dim ds As New DataSet
    '    ds.Tables.Add(NotificationTable)
    '    ds.Tables.Add(PushTable)
    '
    '    CreatePushDataTable()
    '
    '    Dim NotificationRows() As Data.DataRow
    '    NotificationRows = NotificationTable.Select(String.Format(CultureInfo.CurrentCulture _
    '                , "Source={0} AND Destination={1} AND Operation={2} AND IDFlg={3}" _
    '                , inSource _
    '                , inDestination _
    '                , CInt(inOperation) _
    '                , CInt(inIDFlg)))
    '
    '    Dim PushRows() As Data.DataRow
    '    PushRows = PushTable.Select(String.Format(CultureInfo.CurrentCulture _
    '                , "Source={0} AND Destination={1} AND Operation={2} AND IDFlg={3}" _
    '                , inSource _
    '                , inDestination _
    '                , CInt(inOperation) _
    '                , CInt(inIDFlg)))
    '
    '
    '
    '
    'End Sub
    '
    'Private Sub CreatePushDataTable()
    '
    '    '通知用データ
    '    With NotificationTable
    '        .Columns.Add("Source", Type.GetType("System.String")) '元
    '        .Columns.Add("Destination", Type.GetType("System.String")) '先
    '        .Columns.Add("Operation", Type.GetType("System.String")) '操作
    '        .Columns.Add("IDFlg", Type.GetType("System.String")) 'JOB_DTL_IDフラグ"
    '        .Columns.Add("WordNo", Type.GetType("System.String")) '文言DB№"
    '        .Columns.Add("RO", Type.GetType("System.String")) 'R/O
    '        .Columns.Add("REG", Type.GetType("System.String")) 'REG
    '        .Columns.Add("CustName", Type.GetType("System.String")) 'CUSTNAME"
    '        .Columns.Add("PushMethod", Type.GetType("System.String")) 'PushMethod"
    '
    '        .Rows.Add(Operation.TEC, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, WordNo.No53, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.TEC, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, WordNo.No54, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.TEC, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, WordNo.No55, "SC3010501", "SC3080201", "SC3080201", "*")
    '
    '        .Rows.Add(Operation.TEC, Operation.SA, OperetionMode.Registration, IdType.Yes, WordNo.No56, "SC3010501", "SC3080201", "SC3080201", "")
    '
    '        .Rows.Add(Operation.CHT, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, WordNo.No57, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CHT, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, WordNo.No58, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, WordNo.No59, "SC3010501", "SC3080201", "SC3080201", "*")
    '
    '        .Rows.Add(Operation.CHT, Operation.SA, OperetionMode.Registration, IdType.Yes, WordNo.No60, "SC3010501", "SC3080201", "SC3080201", "")
    '        .Rows.Add(Operation.CHT, Operation.SA, OperetionMode.Registration, IdType.No, WordNo.No61, "SC3010501", "SC3080201", "SC3080201", "")
    '        .Rows.Add(Operation.FM, Operation.SA, OperetionMode.Registration, IdType.No, WordNo.No62, "SC3010501", "SC3080201", "SC3080201", "")
    '        .Rows.Add(Operation.CT, Operation.SA, OperetionMode.Registration, IdType.No, WordNo.No63, "SC3010501", "SC3080201", "SC3080201", "")
    '        .Rows.Add(Operation.SM, Operation.SA, OperetionMode.Registration, IdType.No, WordNo.No64, "SC3010501", "SC3080201", "SC3080201", "")
    '
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Approval, IdType.Yes, WordNo.No65, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Approval, IdType.Yes, WordNo.No66, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.FM, Operation.SA, OperetionMode.Approval, IdType.Yes, WordNo.No67, "SC3010501", "SC3080201", "SC3080201", "")
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Denial, IdType.Yes, WordNo.No68, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Denial, IdType.Yes, WordNo.No69, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Approval, IdType.Yes, WordNo.No70, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Approval, IdType.Yes, WordNo.No71, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CT, Operation.SA, OperetionMode.Approval, IdType.Yes, WordNo.No72, "SC3010501", "SC3080201", "SC3080201", "")
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Denial, IdType.Yes, WordNo.No73, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Denial, IdType.Yes, WordNo.No74, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Approval, IdType.Yes, WordNo.No75, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Approval, IdType.Yes, WordNo.No76, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CHT, Operation.SA, OperetionMode.Approval, IdType.Yes, WordNo.No77, "SC3010501", "SC3080201", "SC3080201", "")
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Denial, IdType.Yes, WordNo.No78, "SC3010501", "SC3080201", "SC3080201", "*")
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Denial, IdType.Yes, WordNo.No79, "SC3010501", "SC3080201", "SC3080201", "*")
    '    End With
    '
    '    'Push用データ
    '    With PushTable
    '        .Columns.Add("Source", Type.GetType("System.String")) '元
    '        .Columns.Add("Destination", Type.GetType("System.String")) '先
    '        .Columns.Add("Operation", Type.GetType("System.String")) '操作
    '        .Columns.Add("IDFlg", Type.GetType("System.String")) 'JOB_DTL_IDフラグ"
    '        .Columns.Add("Seq", Type.GetType("System.String")) '枝番
    '        .Columns.Add("Push", Type.GetType("System.String")) 'PushMethod
    '
    '        .Rows.Add(Operation.TEC, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.TEC, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.TEC, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.TEC, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.TEC, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.TEC, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.TEC, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.TEC, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.TEC, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CHT, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CHT, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CHT, Operation.FM, OperetionMode.ApprovalRequest, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CHT, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CHT, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CHT, Operation.CT, OperetionMode.ApprovalRequest, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.ApprovalRequest, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Approval, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Approval, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Approval, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Approval, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Approval, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Approval, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Denial, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Denial, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.FM, Operation.TEC, OperetionMode.Denial, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Denial, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Denial, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.FM, Operation.CHT, OperetionMode.Denial, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Approval, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Approval, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Approval, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Approval, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Approval, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Approval, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Denial, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Denial, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CT, Operation.TEC, OperetionMode.Denial, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Denial, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Denial, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CT, Operation.CHT, OperetionMode.Denial, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Approval, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Approval, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Approval, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Approval, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Approval, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Approval, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Denial, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Denial, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CHT, Operation.TEC, OperetionMode.Denial, IdType.Yes, 3, Operation.CHT)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Denial, IdType.Yes, 1, Operation.FM)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Denial, IdType.Yes, 2, Operation.CT)
    '        .Rows.Add(Operation.CHT, Operation.CHT, OperetionMode.Denial, IdType.Yes, 3, Operation.CHT)
    '    End With
    '
    'End Sub

#End Region

    '2014/06/03 InspectionHeadのカウント取得 STart
    Public Function SelectInspectionHeadCount(ByVal dealerCD As String, ByVal branchCD As String, ByVal roNum As String, ByVal isExistActive As Boolean) As Boolean

        Dim tableAdapter As New SC3180204TableAdapter
        'Dim cnt As Integer = tableAdapter.SelectInspectionHeadCount(inJobDtlId)
        Dim cnt As Integer = tableAdapter.SelectInspectionHeadCount(dealerCD, branchCD, roNum, isExistActive)
        Dim flg As Boolean = True

        If cnt = 0 Then
            flg = False
        End If

        '処理結果返却
        Return flg

    End Function
    '2014/06/03 InspectionHeadのカウント取得　End

    ''2014/06/13 仕様変更対応 Start
    ' ''' <summary>
    ' ''' データ件数チェック
    ' ''' </summary>
    ' ''' <param name="dealerCD">販売店コード</param>
    ' ''' <param name="branchCD">店舗コード</param>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function CheckDataCount(ByVal dealerCD As String, ByVal branchCD As String, ByVal roNum As String) As Boolean

    '    Dim tableAdapter As New SC3180204TableAdapter
    '    Dim result As Boolean = tableAdapter.CheckDataCount(dealerCD, branchCD, roNum)

    '    Return result
    'End Function
    ''2014/06/13 仕様変更対応 End

    '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 Start
    ''' <summary>
    ''' RO番号をキーに、[完成検査結果データ]テーブルに登録された[アドバイス]を取得する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>[完成検査結果データ].[アドバイス]</returns>
    ''' <remarks></remarks>
    Public Function GetAdviceContent(ByVal dealerCD As String, _
                                     ByVal branchCD As String, _
                                     ByVal roNum As String, _
                                     ByVal isExistActive As Boolean) As String

        Dim tableAdapter As New SC3180204TableAdapter

        Return tableAdapter.GetAdviceContent(dealerCD, branchCD, roNum, isExistActive)

    End Function
    '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 End

    '2014/12/10 [JobDispatch完成検査入力制御開発]対応　Start
    ''' <summary>
    ''' チップ単位で、全JOBが開始しているかどうかをチェックする
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="isExistActive">Active存在フラグ</param>
    ''' <returns>True：全JOBが開始している ／ False：開始していないJOBが存在する</returns>
    ''' <remarks></remarks>
    Public Function IsAllJobStartByChip(ByVal jobDtlId As String, ByVal isExistActive As Boolean) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtn As Boolean = False

        Dim tableAdapter As New SC3180204TableAdapter
        rtn = tableAdapter.IsAllJobStartByChip(jobDtlId, isExistActive)
        tableAdapter = Nothing

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result={2}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , rtn.ToString))

        Return rtn

    End Function
    '2014/12/10 [JobDispatch完成検査入力制御開発]対応　End

    '2020/02/13 NCN 小林 TKM要件：型式対応 Start
    ''' <summary>
    ''' マスタに販売店が登録されているか判定する
    ''' </summary>
    ''' <param name="strRoNum">R/O番号</param>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <param name="strBrnCd">店舗コード</param>
    ''' <returns>登録状態 TRANSACTION_EXIST : True or False , HISTORY_EXIST : True or False , MAINTE_CD_EXIST : True or False , KATASHIKI_EXIST : True or False, COMB_DLR_AND_BRN_EXIST : True or False</returns>
    ''' <remarks></remarks>
    Public Function GetDlrCdExistMst(ByVal strRoNum As String, ByVal strDlrCd As String, ByVal strBrnCd As String) As Dictionary(Of String, Boolean)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180204TableAdapter
        Dim dt As New DataTable

        '点検組み合わせマスタ検索
        dt = tableAdapter.GetDlrCdExistMst(strRoNum, _
                                           strDlrCd, _
                                           strBrnCd)

        Dim dict As New Dictionary(Of String, Boolean)
        dict.Add("TRANSACTION_EXIST", False)
        dict.Add("HISTORY_EXIST", False)
        dict.Add("MAINTE_CD_EXIST", False)
        dict.Add("KATASHIKI_EXIST", False)
        dict.Add("COMB_DLR_AND_BRN_EXIST", False)
        If dt.Rows.Count > 0 Then
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("COMB_DLR_AND_BRN_EXIST").ToString())) Then
                dict("COMB_DLR_AND_BRN_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("TRANSACTION_EXIST").ToString())) Then
                dict("TRANSACTION_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("HISTORY_EXIST").ToString())) Then
                dict("HISTORY_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("MAINTE_CD_EXIST").ToString())) Then
                dict("MAINTE_CD_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("KATASHIKI_EXIST").ToString())) Then
                dict("KATASHIKI_EXIST") = True
            End If
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=TRANSACTION_EXIST:{2}, HISTORY_EXIST:{3}, MAINTE_CD_EXIST:{4}, KATASHIKI_EXIST:{5}, COMB_DLR_AND_BRN_EXIST:{6}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dict("TRANSACTION_EXIST").ToString _
                  , dict("HISTORY_EXIST").ToString _
                  , dict("MAINTE_CD_EXIST").ToString _
                  , dict("KATASHIKI_EXIST").ToString _
                  , dict("COMB_DLR_AND_BRN_EXIST").ToString))
        Return dict
    End Function
    '2020/02/13 NCN 小林 TKM要件：型式対応 End

    '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する Start

    ''' <summary>
    ''' 行ロックバージョン取得(GetInspectionHeadLock)
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <returns>行ロックバージョン</returns>
    ''' <remarks></remarks>
    Public Function GetInspectionHeadLock(ByVal jobDtlId As Decimal) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180204TableAdapter

        '完成検査結果データ 行ロックバージョンの取得
        Dim headLockVersion = tableAdapter.GetHeadLockVersion(jobDtlId)
        Dim lockVersion As Long

        If Not IsNothing(headLockVersion(0).ROW_LOCK_VERSION.ToString) Then
            lockVersion = Long.Parse(headLockVersion(0).ROW_LOCK_VERSION.ToString)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return lockVersion

    End Function


    ''' <summary>
    ''' 完成検査結果データ.アドバイスの更新
    ''' RO番号単位でアドバイスを更新する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="adviceContent">アドバイスコメント</param>
    ''' <param name="accountName">更新アカウント</param>
    ''' <param name="updateTime">更新日付</param>
    ''' <returns>True：更新完了 ／ False：更新失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBInspectionAdvice(ByVal dealerCD As String, _
                                                 ByVal branchCD As String, _
                                                 ByVal roNum As String, _
                                                 ByVal adviceContent As String, _
                                                 ByVal accountName As String, _
                                                 ByVal updateTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim isUpdatedSuccess As Boolean
        Dim tableAdapter As New SC3180204TableAdapter

        'アドバイス更新対象リストの取得
        Dim AdviceJobData As SC3180204AdviceJobDataTable
        AdviceJobData = tableAdapter.SelectInspectionHeadList(dealerCD, branchCD, roNum)

        For intListIndex = 0 To AdviceJobData.Count - 1
            Dim jobDtlid As String = AdviceJobData(intListIndex).JOB_DTL_ID

            '完成検査結果データロック処理
            SelectInspectionHeadLock(jobDtlid)

            '完成検査結果データロックバージョンの取得
            Dim lockVersion As Long = Me.GetInspectionHeadLock(jobDtlid)

            '更新処理
            isUpdatedSuccess = tableAdapter.SetDBInspectionAdviceUpt(jobDtlid, _
                                                              adviceContent, _
                                                              accountName, _
                                                              updateTime, _
                                                              lockVersion)
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=Return:{2}, RO_NUM:{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , isUpdatedSuccess _
                  , roNum))

        '処理結果返却
        Return isUpdatedSuccess

    End Function
    '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する End

    '2017/2/16 ライフサイクル対応 走行距離を完成検査で登録する Start

    ''' <summary>
    ''' 前回部品交換情報登録処理
    ''' </summary>
    ''' <param name="vin">VIN</param>
    ''' <param name="inspecItemCd">点検項目コード</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="inspectionNeedFlg">検査必要フラグ</param>
    ''' <param name="updateTime">日時</param>
    ''' <param name="accountName">アカウント名</param>
    ''' <param name="prePartsReplaceDt">前回部品交換情報(取得条件VIN)</param>
    ''' <param name="jobDtlID">作業内容ID</param>
    ''' <param name="ButtonMode">ボタン押下種類</param>
    ''' <returns>登録成功：True／失敗：False</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetPreviousPartsReplace(ByVal vin As String, _
                                            ByVal inspecItemCd As String, _
                                            ByVal dealerCD As String, _
                                            ByVal branchCD As String, _
                                            ByVal roNum As String, _
                                            ByVal inspectionNeedFlg As String, _
                                            ByVal updateTime As Date, _
                                            ByVal accountName As String, _
                                            ByVal prePartsReplaceDt As SC3180204PreviousPartsReplaceDataTable, _
                                            ByVal jobDtlID As String, _
                                            ByVal ButtonMode As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim isSuccessSet As Boolean = False
        Dim tableAdapter As New SC3180204TableAdapter

        ' VINを条件に取得された前回部品交換情報を点検項目コードで絞込
        Dim prePartsReplaceArray As Array = prePartsReplaceDt.Select(String.Format("INSPEC_ITEM_CD = {0}", inspecItemCd))

        '走行距離取得
        Dim regMile As Decimal = Me.GetReplaceMile(dealerCD, branchCD, roNum)

        If prePartsReplaceArray.Length > 0 Then

            Dim prePartsReplaceRow As SC3180204PreviousPartsReplaceRow = DirectCast(prePartsReplaceArray(0), SC3180204PreviousPartsReplaceRow)

            '行ロック取得
            SelectPartsReplaceLock(vin, inspecItemCd)

            If roNum.Equals(prePartsReplaceRow.RO_NUM) Then
                ' RO番号が前回部品交換情報と一致する場合
                ' 手戻りと判断し、前回部品交換情報.前回交換走行距離等の更新は行わない
                '更新処理
                isSuccessSet = tableAdapter.SetPartsReplaceUpt(vin,
                                                               inspecItemCd,
                                                               regMile,
                                                               inspectionNeedFlg,
                                                               accountName,
                                                               updateTime,
                                                               CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))

            Else

                If InspectionNeedFlgON.Equals(inspectionNeedFlg) Then
                    If ButtonMode = ModeRegister Then
                        '完成検査承認が必要かつ、SAが新規でReplaceを選択した場合
                        '交換日時 = 承認日時
                        Dim appDatetimeStr As String = tableAdapter.GetInspectionApprovalDatetime(jobDtlID)
                        Dim appDatetimeDate As Date
                        If String.IsNullOrEmpty(appDatetimeStr) Then
                            '承認日時が取得できない状況は通常ありえない
                            appDatetimeDate = Date.Parse(FormatDbDateTime, CultureInfo.CurrentCulture)
                        Else
                            appDatetimeDate = Date.Parse(appDatetimeStr, CultureInfo.CurrentCulture)
                        End If
                        '更新処理
                        isSuccessSet = tableAdapter.SetPartsReplaceUpt(vin,
                                                                       inspecItemCd,
                                                                       roNum,
                                                                       regMile,
                                                                       appDatetimeDate,
                                                                       CType(prePartsReplaceRow.REPLACE_MILE, Decimal),
                                                                       Date.Parse(prePartsReplaceRow.REPLACE_DATE, CultureInfo.CurrentCulture),
                                                                       inspectionNeedFlg,
                                                                       accountName,
                                                                       updateTime,
                                                                       CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))
                    Else
                        'SA以外が更新を行う
                        '交換日時は更新しない
                        '更新処理
                        isSuccessSet = tableAdapter.SetPartsReplaceUpt(vin,
                                                                       inspecItemCd,
                                                                       roNum,
                                                                       regMile,
                                                                       Date.Parse(FormatDbDateTime, CultureInfo.CurrentCulture),
                                                                       CType(prePartsReplaceRow.REPLACE_MILE, Decimal),
                                                                       Date.Parse(prePartsReplaceRow.REPLACE_DATE, CultureInfo.CurrentCulture),
                                                                       inspectionNeedFlg,
                                                                       accountName,
                                                                       updateTime,
                                                                       CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))

                    End If

                Else
                    '完成検査承認が不要の場合
                    '交換日時 = HEAD.行更新日時
                    Dim rowDate As Date = Date.Parse(prePartsReplaceRow.REPLACE_DATE, CultureInfo.CurrentCulture)
                    '更新処理
                    isSuccessSet = tableAdapter.SetPartsReplaceUpt(vin,
                                                                   inspecItemCd,
                                                                   roNum,
                                                                   regMile,
                                                                   updateTime,
                                                                   CType(prePartsReplaceRow.REPLACE_MILE, Decimal),
                                                                   rowDate,
                                                                   inspectionNeedFlg,
                                                                   accountName,
                                                                   updateTime,
                                                                   CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))

                End If

            End If
        Else

            Dim appDatetimeDate As Date

            If InspectionNeedFlgON.Equals(inspectionNeedFlg) Then

                If ButtonMode = ModeRegister Then
                    '完成検査承認が必要かつ、Registerの場合
                    Dim appDatetimeStr As String = tableAdapter.GetInspectionApprovalDatetime(jobDtlID)
                    If String.IsNullOrEmpty(appDatetimeStr) Then
                        '承認日時が取得できない状況は通常ありえない
                        appDatetimeDate = Date.Parse(FormatDbDateTime, CultureInfo.CurrentCulture)
                    Else
                        appDatetimeDate = Date.Parse(appDatetimeStr, CultureInfo.CurrentCulture)
                    End If

                Else
                    '完成検査承認が必要かつ、Registe以外の場合
                    appDatetimeDate = Date.Parse(FormatDbDateTime, CultureInfo.CurrentCulture)
                End If

            Else

                '完成検査承認が不要の場合
                appDatetimeDate = updateTime

            End If
            isSuccessSet = tableAdapter.SetPartsReplaceIns(vin,
                                                           inspecItemCd,
                                                           roNum,
                                                           CType(regMile, Decimal),
                                                           appDatetimeDate,
                                                           accountName,
                                                           updateTime)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=Return:{2}, VIN:{3}, INSPEC_ITEM_CD:{4}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , isSuccessSet _
                  , vin _
                  , inspecItemCd))

        '処理結果返却
        Return isSuccessSet

    End Function

    ''' <summary>
    ''' 前回部品交換情報削除処理
    ''' </summary>
    ''' <param name="vin">VIN</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="editPartsReplaceArray">画面パラメータでReplace選択された点検項目コードリスト</param>
    ''' <param name="prePartsReplaceDt">前回部品交換情報(取得条件VIN)</param>
    ''' <param name="strAccount">行更新アカウント</param>
    ''' <param name="dtfUpdate">行更新日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function NotReplacePreviousParts(ByVal vin As String, _
                                            ByVal roNum As String, _
                                            ByVal editPartsReplaceArray As List(Of String), _
                                            ByVal prePartsReplaceDt As SC3180204PreviousPartsReplaceDataTable, _
                                            ByVal strAccount As String, _
                                            ByVal dtfUpdate As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim isSuccessSet As Boolean = True
        Dim tableAdapter As New SC3180204TableAdapter
        ' 点検項目がReplace→Replace以外になった場合
        For Each prePartsReplaceRow As SC3180204PreviousPartsReplaceRow In prePartsReplaceDt

            If Not editPartsReplaceArray.Contains(prePartsReplaceRow.INSPEC_ITEM_CD) Then
                '更新前DBにあって、更新対象にない

                Dim preDateCmp As Integer = DateTime.Compare(Date.Parse(prePartsReplaceRow.PREVIOUS_REPLACE_DATE, CultureInfo.InvariantCulture), _
                                                             Date.Parse(FormatDbDateTime, CultureInfo.InvariantCulture))

                If prePartsReplaceRow.RO_NUM.Equals(roNum) AndAlso
                    preDateCmp = 0 AndAlso
                    prePartsReplaceRow.PREVIOUS_REPLACE_MILE = DefaultPreviousReplaceMile Then

                    '初回交換時(削除)
                    isSuccessSet = tableAdapter.DelPreviousPartsReplace(vin, prePartsReplaceRow.INSPEC_ITEM_CD)

                ElseIf prePartsReplaceRow.RO_NUM.Equals(roNum) Then

                    'ロック
                    SelectPartsReplaceLock(vin, prePartsReplaceRow.INSPEC_ITEM_CD)
                    '2回目以降交換時(更新)
                    isSuccessSet = tableAdapter.SetDelPartsReplaceUpt(vin, _
                                                                      prePartsReplaceRow.INSPEC_ITEM_CD, _
                                                                      CType(prePartsReplaceRow.PREVIOUS_REPLACE_MILE, Decimal), _
                                                                      Date.Parse(prePartsReplaceRow.PREVIOUS_REPLACE_DATE, CultureInfo.CurrentCulture), _
                                                                      strAccount, _
                                                                      dtfUpdate, _
                                                                      CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))
                End If

            End If
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=Return:{2}, VIN:{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , isSuccessSet _
                  , vin))

        Return isSuccessSet

    End Function

#Region "走行距離の取得"
    ''' <summary>
    ''' 走行距離の取得
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>走行距離(取得失敗時は-1)</returns>
    ''' <remarks></remarks>
    Public Function GetReplaceMile(ByVal dealerCD As String, _
                                    ByVal branchCD As String, _
                                    ByVal roNum As String) As Decimal

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Const INIT_REG_MILE As Decimal = -1

        Dim DmsDataTable As DmsCodeMapDataTable = Me.GetDmsDealerData(dealerCD, branchCD)

        Dim DMS_BRN_CD As String
        If 0 < DmsDataTable.Count Then
            If Not DmsDataTable(0).IsCODE2Null AndAlso Not String.IsNullOrWhiteSpace(DmsDataTable(0).CODE2) Then
                'DMS変換後の店舗コードを取得
                DMS_BRN_CD = DmsDataTable(0).CODE2
            Else
                'DMS変換失敗時は変換前の店舗コードを入れる
                DMS_BRN_CD = branchCD
            End If
        Else
            'DMS変換取得失敗時は変換前の店舗コードを入れる
            DMS_BRN_CD = branchCD
        End If

        '入庫管理番号作成
        Dim strSVCIN_NUM As String = GetSVCIN_NUM(DMS_BRN_CD, roNum)

        '入庫履歴より走行距離を取得
        Dim tableAdapter As New SC3180204TableAdapter
        Dim replaceMileDt As PreviosReplacementMileageDataTable
        replaceMileDt = tableAdapter.GetPreviosReplacementMileage(dealerCD, strSVCIN_NUM)

        Dim regMile As Decimal
        If replaceMileDt.Count > 0 Then
            regMile = CType(replaceMileDt(0).REG_MILE, Decimal)
        Else
            regMile = INIT_REG_MILE
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END [Result=Return:{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , regMile))

        '処理結果返却
        Return regMile

    End Function


    ''' <summary>
    ''' 入庫管理番号取得
    ''' </summary>
    ''' <returns>入庫管理番号</returns>
    ''' <remarks>入庫管理番号の書式変換を行う</remarks>
    Public Function GetSVCIN_NUM(ByVal strBRN_CD As String, ByVal strRO_NUM As String) As String

        '開始ログの記録
        Logger.Info(String.Format("GetSVCIN_NUM_START, strBRN_CD:[{0}]", strBRN_CD))

        '①「販売店システム設定」より、「入庫管理番号利用フラグ」を取得する。
        Dim SVCIN_FLG As String = Me.GetDlrSystemSettingValueBySettingName("SVCIN_NUM_USE_FLG")

        '②「入庫管理番号利用フラグ」が０の場合、書式変換を行う
        Dim SVCIN_Num As String = String.Empty
        If Not String.IsNullOrWhiteSpace(SVCIN_FLG) Then
            If SVCIN_FLG = "0" Then
                Dim SVCIN_Format As String = Me.GetDlrSystemSettingValueBySettingName("SVCIN_NUM_FORMAT")
                If Not String.IsNullOrWhiteSpace(SVCIN_Format) Then
                    SVCIN_Num = Replace(Replace(SVCIN_Format, "[RO_NUM]", strRO_NUM), "[DMS_BRN_CD]", strBRN_CD)
                End If
            End If
        End If

        '終了ログの記録
        Logger.Info(String.Format("GetSVCIN_NUM_END, Return:[{0}]", SVCIN_Num))

        Return SVCIN_Num

    End Function

    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty

        'ログイン情報
        Dim userContext As StaffContext = StaffContext.Current


        '自分のテーブルアダプタークラスインスタンスを生成
        Dim ta As New SC3180204TableAdapter

        '販売店システム設定から取得
        Dim dt As SC3180204DataSet.SystemSettingDataTable _
                                = ta.GetDlrSystemSettingValue(userContext.DlrCD, _
                                                                          userContext.BrnCD, _
                                                                          AllDealerCode, _
                                                                          AllBranchCode, _
                                                                          settingName)

        If 0 < dt.Count Then
            '設定値を取得
            retValue = dt.Item(0).SETTING_VAL
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S OUT:{1}={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function

    '【***完成検査_排他制御***】 start
    ''' <summary>
    ''' 完成検査結果更新可能判定
    ''' </summary>
    ''' <param name="svcinRowLockVersion">行ロックバージョン</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <remarks>True:行ロックバージョン更新可能/False:行ロックバージョン更新不可</remarks>
    Public Function CheckUpdateFinalInspection(ByVal svcinRowLockVersion As Long,
                                   ByVal dealerCD As String,
                                   ByVal branchCD As String,
                                   ByVal roNum As String) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim Flg As Boolean = True
        Dim tableAdapter As New SC3180204TableAdapter
        Dim dt As DataTable


        dt = tableAdapter.GetAndLockServiceinRow(roNum, dealerCD, branchCD)

        If svcinRowLockVersion = Long.Parse(dt.Rows(0).Item("ROW_LOCK_VERSION").ToString) Then

        Else
            Flg = False
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END, Return:[{2}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , Flg))

        Return Flg
    End Function
    '【***完成検査_排他制御***】 end

#End Region


    '2017/2/16 ライフサイクル対応 走行距離を完成検査で登録する End

End Class
