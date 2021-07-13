'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240301DataSet.vb
'─────────────────────────────────────
'機能：タブレットSMBサブチップボックスのデータセット
'補足： 
'作成：2013/05/17 TMEJ 丁 タブレット版SMB機能開発(工程管理)
'更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致
'更新：2015/07/16 TMEJ 河原 TMT_N/W問題緊急対応_SQL007チューニング対応
'更新：2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除）
'更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新：2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization

Partial Class SC3240301DataSet

    Partial Class SC3240301ChipCountDataTable

    End Class

End Class

Namespace SC3240301DataSetTableAdapters

    Public Class SC3240301StallInfoDataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        ''' <summary>
        ''' 完成検査承認待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_INSPECTION_APPROVAL_WAIT = "1"
        ''' <summary>
        ''' キャンセルフラグ 　0:有効　
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_NOT_CANCEL = "0"
        ''' <summary>
        ''' キャンセルフラグ   1:キャンセル
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_CANCEL = "1"

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        ''' <summary>
        ''' 仮置きフラグ 　0:仮置きでない
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_NOT_TEMP As String = "0"
        ''' <summary>
        ''' 仮置きフラグ 　1:仮置き　
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_TEMP As String = "1"
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 着工指示フラグ   0:未指示
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_INSTRUCT_FLG_NOT = "0"
        ''' <summary>
        ''' 着工指示フラグ   1:指示済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_INSTRUCT_FLG = "1"
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        ''' <summary>
        ''' ハイフン
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_HYPHEN As String = "-"
        ''' <summary>
        ''' 基本型式(ALL)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_BASETYPEALL As String = "X"
        ''' <summary>
        ''' サービスステータス：納車待ち（Waiting）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_WAITINGDELI As String = "12"
        ''' <summary>
        ''' サービスステータス：預かり中（DropOff）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_DROPOFF As String = "11"
        ''' <summary>
        ''' サービスステータス：洗車待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_CARWASHWAIT As String = "07"
        ''' <summary>
        ''' サービスステータス：洗車中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_CARWASHSTART As String = "08"

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        ''' <summary>
        ''' サービスステータス：未入庫
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_NOTCARIN As String = "00"
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        ''' <summary>
        ''' サービスステータス：未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_NOSHOW As String = "01"

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' サービスステータス：キャンセル（画面上から関連するチップが全て消えている）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_CANCEL As String = "02"

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        ''' <summary>
        ''' サービスステータス：着工指示待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_STARTWORKINSTRUCTWAIT As String = "03"
        ''' <summary>
        ''' サービスステータス：作業開始待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_STARTWORKWAIT As String = "04"
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        ''' <summary>
        ''' サービスステータス：納車済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_DELI As String = "13"

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        ''' <summary>
        ''' サービスステータス：作業中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_WORKING As String = "05"
        ''' <summary>
        ''' サービスステータス：次の作業開始待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_NEXTSTARTWAIT As String = "06"
        ''' <summary>
        ''' サービスステータス：検査待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SVC_STATUS_INSPECTIONWAIT As String = "09"
        ''' <summary>
        ''' 洗車必要フラグ　1:洗車必要
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_CARWASHNEED As String = "1"
        ''' <summary>
        ''' 中断
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_STALLUSE_STATUS_STOP As String = "05"
        ''' <summary>
        ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_STALLUSE_STATUS_NOSHOW As String = "07"

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' ROステータス:FM承認待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_RO_STATUS_WAITFMAPP As String = "20"
        ''' <summary>
        ''' ROステータス:お客様承認
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_RO_STATUS_CSTAPPROVED As String = "50"
        ''' <summary>
        ''' ROステータス:作業中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_RO_STATUS_WORKING As String = "60"
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        ''' <summary>
        ''' RO番号デフォルト値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_RO_NUM As String = " "

        ''' <summary>
        ''' RO連番デフォルト値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_RO_SEQ As Decimal = -1

        ''' <summary>
        ''' 日付最小値文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DateMinValue As String = "1900-01-01 00:00:00"
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
        '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>
        ''' P/Lマークフラグ（0：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
        '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

#End Region

        ''' <summary>
        ''' サブチップエリアの納車待ちチップ一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDeliverdChipList(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. dealerCode={1}, branchCode={2}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dealerCode, _
                                      branchCode))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_001 */ ")
                .AppendLine("    T13.SVCIN_ID ")
                .AppendLine("  , T13.DLR_CD ")
                .AppendLine("  , T13.BRN_CD ")
                .AppendLine("  , T13.RO_NUM ")
                .AppendLine("  , T13.CST_ID ")
                .AppendLine("  , T13.VCL_ID ")
                .AppendLine("  , T13.CST_VCL_TYPE  ")
                .AppendLine("  , T13.TLM_CONTRACT_FLG ")
                .AppendLine("  , T13.ACCEPTANCE_TYPE  ")
                .AppendLine("  , T13.PICK_DELI_TYPE ")
                .AppendLine("  , T13.CARWASH_NEED_FLG  ")
                .AppendLine("  , T13.RESV_STATUS ")
                .AppendLine("  , T13.SVC_STATUS ")
                .AppendLine("  , T13.SCHE_SVCIN_DATETIME ")
                .AppendLine("  , T13.SCHE_DELI_DATETIME ")
                .AppendLine("  , T13.RSLT_SVCIN_DATETIME ")
                .AppendLine("  , T13.RSLT_DELI_DATETIME ")
                .AppendLine("  , T13.ROW_UPDATE_DATETIME ")
                .AppendLine("  , T13.ROW_LOCK_VERSION ")
                .AppendLine("  , T13.JOB_DTL_ID ")
                .AppendLine("  , T13.INSPECTION_NEED_FLG ")
                .AppendLine("  , T13.CANCEL_FLG ")
                .AppendLine("  , T13.STALL_USE_ID ")
                .AppendLine("  , T13.STALL_ID ")
                .AppendLine("  , T13.TEMP_FLG ")
                .AppendLine("  , T13.STALL_USE_STATUS ")
                .AppendLine("  , T13.SCHE_START_DATETIME ")
                .AppendLine("  , T13.SCHE_END_DATETIME ")
                .AppendLine("  , T13.SCHE_WORKTIME ")
                .AppendLine("  , T13.REST_FLG ")
                .AppendLine("  , T13.RSLT_START_DATETIME ")
                .AppendLine("  , T13.PRMS_END_DATETIME ")
                .AppendLine("  , T13.RSLT_END_DATETIME ")
                .AppendLine("  , T13.RSLT_WORKTIME ")
                .AppendLine("  , T13.STOP_REASON_TYPE ")
                .AppendLine("  , T13.CST_NAME ")
                .AppendLine("  , T13.VCL_VIN ")
                .AppendLine("  , T13.MODEL_NAME ")
                .AppendLine("  , T13.REG_NUM ")
                .AppendLine("  , T13.CARWASH_RSLT_ID ")
                .AppendLine("  , T13.CW_RSLT_START_DATETIME ")
                .AppendLine("  , T13.CW_RSLT_END_DATETIME ")
                .AppendLine("  , T13.SVC_CLASS_NAME ")
                .AppendLine("  , T13.SVC_CLASS_NAME_ENG ")
                .AppendLine("  , T13.UPPER_DISP ")
                .AppendLine("  , T13.LOWER_DISP ")
                .AppendLine("  , T13.INSPECTION_RSLT_ID ")
                .AppendLine("  , T13.IS_RSLT_START_DATETIME ")
                .AppendLine("  , T13.IS_RSLT_END_DATETIME ")
                .AppendLine("  , T13.PASSING_FLG ")
                .AppendLine("  , T13.STF_CD ")
                .AppendLine("  , T13.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("  , T13.IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine(" FROM ( ")
                .AppendLine("   SELECT ")
                .AppendLine("       T1.SVCIN_ID ")
                .AppendLine("    ,  T1.DLR_CD ")
                .AppendLine("    ,  T1.BRN_CD ")
                .AppendLine("    ,  T1.RO_NUM ")
                .AppendLine("    ,  T1.CST_ID ")
                .AppendLine("    ,  T1.VCL_ID ")
                .AppendLine("    ,  T1.CST_VCL_TYPE ")
                .AppendLine("    ,  T1.TLM_CONTRACT_FLG ")
                .AppendLine("    ,  T1.ACCEPTANCE_TYPE ")
                .AppendLine("    ,  T1.PICK_DELI_TYPE ")
                .AppendLine("    ,  T1.CARWASH_NEED_FLG ")
                .AppendLine("    ,  T1.RESV_STATUS ")
                .AppendLine("    ,  T1.SVC_STATUS ")
                .AppendLine("    ,  T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("    ,  T1.SCHE_DELI_DATETIME ")
                .AppendLine("    ,  T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("    ,  T1.RSLT_DELI_DATETIME ")
                .AppendLine("    ,  T1.ROW_UPDATE_DATETIME ")
                .AppendLine("    ,  T1.ROW_LOCK_VERSION ")
                .AppendLine("    ,  T2.JOB_DTL_ID ")
                .AppendLine("    ,  T2.INSPECTION_NEED_FLG ")
                .AppendLine("    ,  T2.CANCEL_FLG ")
                .AppendLine("    ,  T3.STALL_USE_ID ")
                .AppendLine("    ,  T3.STALL_ID ")
                .AppendLine("    ,  T3.TEMP_FLG ")
                .AppendLine("    ,  T3.STALL_USE_STATUS ")
                .AppendLine("    ,  T3.SCHE_START_DATETIME ")
                .AppendLine("    ,  T3.SCHE_END_DATETIME ")
                .AppendLine("    ,  T3.SCHE_WORKTIME ")
                .AppendLine("    ,  T3.REST_FLG  ")
                .AppendLine("    ,  T3.RSLT_START_DATETIME ")
                .AppendLine("    ,  T3.PRMS_END_DATETIME  ")
                .AppendLine("    ,  T3.RSLT_END_DATETIME ")
                .AppendLine("    ,  T3.RSLT_WORKTIME  ")
                .AppendLine("    ,  T3.STOP_REASON_TYPE ")
                .AppendLine("    ,  T4.CST_NAME ")
                .AppendLine("    ,  T5.VCL_VIN ")
                .AppendLine("    ,  T6.MODEL_NAME ")
                .AppendLine("    ,  T7.REG_NUM ")
                .AppendLine("    ,  T8.CARWASH_RSLT_ID ")
                .AppendLine("    ,  T8.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("    ,  T8.RSLT_END_DATETIME AS CW_RSLT_END_DATETIME ")
                .AppendLine("    ,  T9.SVC_CLASS_NAME ")
                .AppendLine("    ,  T9.SVC_CLASS_NAME_ENG ")
                .AppendLine("    ,  T10.UPPER_DISP ")
                .AppendLine("    ,  T10.LOWER_DISP ")
                .AppendLine("    ,  T11.INSPECTION_RSLT_ID ")
                .AppendLine("    ,  T11.RSLT_START_DATETIME AS IS_RSLT_START_DATETIME ")
                .AppendLine("    ,  T11.RSLT_END_DATETIME AS IS_RSLT_END_DATETIME ")
                .AppendLine("    ,  T11.PASSING_FLG ")
                .AppendLine("    ,  T12.STF_CD ")
                .AppendLine("    ,  T12.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("    ,  NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("      TB_T_SERVICEIN T1 ")
                .AppendLine("    , TB_T_JOB_DTL T2 ")
                .AppendLine("    , TB_T_STALL_USE T3 ")
                .AppendLine("    , TB_M_CUSTOMER T4 ")
                .AppendLine("    , TB_M_VEHICLE T5 ")
                .AppendLine("    , TB_M_MODEL T6 ")
                .AppendLine("    , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("    , TB_T_CARWASH_RESULT T8 ")
                .AppendLine("    , TB_M_SERVICE_CLASS T9 ")
                .AppendLine("    , TB_M_MERCHANDISE T10 ")
                .AppendLine("    , TB_T_INSPECTION_RESULT T11 ")
                .AppendLine("    , TB_M_STAFF T12 ")
                .AppendLine("   WHERE ")
                .AppendLine("        T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("    AND T1.CST_ID = T4.CST_ID(+) ")
                .AppendLine("    AND T1.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("    AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("    AND T1.VCL_ID = T7.VCL_ID(+) ")
                .AppendLine("    AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("    AND T1.SVCIN_ID = T8.SVCIN_ID(+) ")
                .AppendLine("    AND T2.SVC_CLASS_ID = T9.SVC_CLASS_ID(+) ")
                .AppendLine("    AND T2.MERC_ID = T10.MERC_ID (+) ")
                .AppendLine("    AND T3.JOB_DTL_ID = T11.JOB_DTL_ID (+) ")
                .AppendLine("    AND T2.UPDATE_STF_CD = T12.STF_CD(+) ")
                .AppendLine("    AND T1.DLR_CD = :DLRCD ")
                .AppendLine("    AND T1.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("    AND T2.DLR_CD = :DLRCD ")
                .AppendLine("    AND T2.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("    AND T1.SVC_STATUS IN (:SVC_STATUS1,:SVC_STATUS2) ")
                .AppendLine("    AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("    AND ( T2.SVCIN_ID, T3.RSLT_END_DATETIME ) = ( ")
                .AppendLine("                       SELECT ")
                .AppendLine("                            T17.SVCIN_ID, MAX(T15.RSLT_END_DATETIME) ")
                .AppendLine("                       FROM ")
                .AppendLine("                            TB_T_SERVICEIN T17, TB_T_JOB_DTL T14, TB_T_STALL_USE T15 ")
                .AppendLine("                       WHERE ")
                .AppendLine("                            T17.SVCIN_ID = T14.SVCIN_ID ")
                .AppendLine("                        AND T14.JOB_DTL_ID = T15.JOB_DTL_ID ")
                .AppendLine("                        AND T17.SVCIN_ID = T1.SVCIN_ID ")
                .AppendLine("                        AND T14.CANCEL_FLG=:CANCEL_FLG ")
                .AppendLine("                        GROUP BY T17.SVCIN_ID ")
                .AppendLine("                       ) ")
                .AppendLine("    AND T3.STALL_USE_ID=( ")
                .AppendLine("                        SELECT ")
                .AppendLine("                            MAX(STALL_USE_ID) ")
                .AppendLine("                        FROM ")
                .AppendLine("                            TB_T_STALL_USE T16 ")
                .AppendLine("                        WHERE ")
                .AppendLine("                            T16.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
                .AppendLine(" ) T13 ")
                .AppendLine(" ORDER BY ")
                .AppendLine("   DECODE(T13.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T13.SCHE_DELI_DATETIME) ASC ")
                .AppendLine(" , T13.ACCEPTANCE_TYPE ASC  ")
                .AppendLine(" , T13.PICK_DELI_TYPE ASC  ")
                .AppendLine(" , NVL(T13.CW_RSLT_END_DATETIME,T13.RSLT_END_DATETIME) ASC ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubChipInfoDataTable)("SC3240301_001")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_WAITINGDELI)                     ' 納車待ち（Waiting）
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_DROPOFF)                     ' 預かり中（DropOff）
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアの洗車チップ一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCarWashChipList(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_S. dealerCode={1}, branchCode={2}", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                          dealerCode, _
                          branchCode))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_002 */ ")
                .AppendLine("    T13.SVCIN_ID ")
                .AppendLine("  , T13.DLR_CD ")
                .AppendLine("  , T13.BRN_CD ")
                .AppendLine("  , T13.RO_NUM ")
                .AppendLine("  , T13.CST_ID ")
                .AppendLine("  , T13.VCL_ID ")
                .AppendLine("  , T13.CST_VCL_TYPE  ")
                .AppendLine("  , T13.TLM_CONTRACT_FLG ")
                .AppendLine("  , T13.ACCEPTANCE_TYPE  ")
                .AppendLine("  , T13.PICK_DELI_TYPE ")
                .AppendLine("  , T13.CARWASH_NEED_FLG  ")
                .AppendLine("  , T13.RESV_STATUS ")
                .AppendLine("  , T13.SVC_STATUS ")
                .AppendLine("  , T13.SCHE_SVCIN_DATETIME ")
                .AppendLine("  , T13.SCHE_DELI_DATETIME ")
                .AppendLine("  , T13.RSLT_SVCIN_DATETIME ")
                .AppendLine("  , T13.RSLT_DELI_DATETIME ")
                .AppendLine("  , T13.ROW_UPDATE_DATETIME ")
                .AppendLine("  , T13.ROW_LOCK_VERSION ")
                .AppendLine("  , T13.JOB_DTL_ID ")
                .AppendLine("  , T13.INSPECTION_NEED_FLG ")
                .AppendLine("  , T13.CANCEL_FLG ")
                .AppendLine("  , T13.STALL_USE_ID ")
                .AppendLine("  , T13.STALL_ID ")
                .AppendLine("  , T13.TEMP_FLG ")
                .AppendLine("  , T13.STALL_USE_STATUS ")
                .AppendLine("  , T13.SCHE_START_DATETIME ")
                .AppendLine("  , T13.SCHE_END_DATETIME ")
                .AppendLine("  , T13.SCHE_WORKTIME ")
                .AppendLine("  , T13.REST_FLG ")
                .AppendLine("  , T13.RSLT_START_DATETIME ")
                .AppendLine("  , T13.PRMS_END_DATETIME ")
                .AppendLine("  , T13.RSLT_END_DATETIME ")
                .AppendLine("  , T13.RSLT_WORKTIME ")
                .AppendLine("  , T13.STOP_REASON_TYPE ")
                .AppendLine("  , T13.CST_NAME ")
                .AppendLine("  , T13.VCL_VIN ")
                .AppendLine("  , T13.MODEL_NAME ")
                .AppendLine("  , T13.REG_NUM ")
                .AppendLine("  , T13.CARWASH_RSLT_ID ")
                .AppendLine("  , T13.CW_RSLT_START_DATETIME ")
                .AppendLine("  , T13.CW_RSLT_END_DATETIME ")
                .AppendLine("  , T13.SVC_CLASS_NAME ")
                .AppendLine("  , T13.SVC_CLASS_NAME_ENG ")
                .AppendLine("  , T13.UPPER_DISP ")
                .AppendLine("  , T13.LOWER_DISP ")
                .AppendLine("  , T13.INSPECTION_RSLT_ID ")
                .AppendLine("  , T13.IS_RSLT_START_DATETIME ")
                .AppendLine("  , T13.IS_RSLT_END_DATETIME ")
                .AppendLine("  , T13.PASSING_FLG ")
                .AppendLine("  , T13.STF_CD ")
                .AppendLine("  , T13.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("  , T13.IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine(" FROM ( ")
                .AppendLine("   SELECT ")
                .AppendLine("      T1.SVCIN_ID ")
                .AppendLine("    , T1.DLR_CD ")
                .AppendLine("    , T1.BRN_CD ")
                .AppendLine("    , T1.RO_NUM ")
                .AppendLine("    , T1.CST_ID ")
                .AppendLine("    , T1.VCL_ID ")
                .AppendLine("    , T1.CST_VCL_TYPE ")
                .AppendLine("    , T1.TLM_CONTRACT_FLG ")
                .AppendLine("    , T1.ACCEPTANCE_TYPE ")
                .AppendLine("    , T1.PICK_DELI_TYPE ")
                .AppendLine("    , T1.CARWASH_NEED_FLG ")
                .AppendLine("    , T1.RESV_STATUS ")
                .AppendLine("    , T1.SVC_STATUS ")
                .AppendLine("    , T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("    , T1.SCHE_DELI_DATETIME ")
                .AppendLine("    , T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("    , T1.RSLT_DELI_DATETIME ")
                .AppendLine("    , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("    , T1.ROW_LOCK_VERSION ")
                .AppendLine("    , T2.JOB_DTL_ID ")
                .AppendLine("    , T2.INSPECTION_NEED_FLG ")
                .AppendLine("    , T2.CANCEL_FLG ")
                .AppendLine("    , T3.STALL_USE_ID ")
                .AppendLine("    , T3.STALL_ID ")
                .AppendLine("    , T3.TEMP_FLG ")
                .AppendLine("    , T3.STALL_USE_STATUS ")
                .AppendLine("    , T3.SCHE_START_DATETIME ")
                .AppendLine("    , T3.SCHE_END_DATETIME ")
                .AppendLine("    , T3.SCHE_WORKTIME ")
                .AppendLine("    , T3.REST_FLG  ")
                .AppendLine("    , T3.RSLT_START_DATETIME ")
                .AppendLine("    , T3.PRMS_END_DATETIME  ")
                .AppendLine("    , T3.RSLT_END_DATETIME ")
                .AppendLine("    , T3.RSLT_WORKTIME  ")
                .AppendLine("    , T3.STOP_REASON_TYPE ")
                .AppendLine("    , T4.CST_NAME ")
                .AppendLine("    , T5.VCL_VIN ")
                .AppendLine("    , T6.MODEL_NAME ")
                .AppendLine("    , T7.REG_NUM ")
                .AppendLine("    , T8.CARWASH_RSLT_ID ")
                .AppendLine("    , T8.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("    , T8.RSLT_END_DATETIME AS CW_RSLT_END_DATETIME ")
                .AppendLine("    , T9.SVC_CLASS_NAME ")
                .AppendLine("    , T9.SVC_CLASS_NAME_ENG ")
                .AppendLine("    , T10.UPPER_DISP ")
                .AppendLine("    , T10.LOWER_DISP ")
                .AppendLine("    , T11.INSPECTION_RSLT_ID ")
                .AppendLine("    , T11.RSLT_START_DATETIME AS IS_RSLT_START_DATETIME ")
                .AppendLine("    , T11.RSLT_END_DATETIME AS IS_RSLT_END_DATETIME ")
                .AppendLine("    , T11.PASSING_FLG ")
                .AppendLine("    , T12.STF_CD ")
                .AppendLine("    , T12.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("    , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("      TB_T_SERVICEIN T1 ")
                .AppendLine("    , TB_T_JOB_DTL T2 ")
                .AppendLine("    , TB_T_STALL_USE T3 ")
                .AppendLine("    , TB_M_CUSTOMER T4 ")
                .AppendLine("    , TB_M_VEHICLE T5 ")
                .AppendLine("    , TB_M_MODEL T6 ")
                .AppendLine("    , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("    , TB_T_CARWASH_RESULT T8 ")
                .AppendLine("    , TB_M_SERVICE_CLASS T9 ")
                .AppendLine("    , TB_M_MERCHANDISE T10 ")
                .AppendLine("    , TB_T_INSPECTION_RESULT T11 ")
                .AppendLine("    , TB_M_STAFF T12 ")
                .AppendLine("   WHERE ")
                .AppendLine("         T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("     AND T1.CST_ID = T4.CST_ID(+) ")
                .AppendLine("     AND T1.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("     AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("     AND T1.VCL_ID = T7.VCL_ID(+) ")
                .AppendLine("     AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("     AND T1.SVCIN_ID = T8.SVCIN_ID(+) ")
                .AppendLine("     AND T2.SVC_CLASS_ID = T9.SVC_CLASS_ID(+) ")
                .AppendLine("     AND T2.MERC_ID = T10.MERC_ID (+) ")
                .AppendLine("     AND T3.JOB_DTL_ID = T11.JOB_DTL_ID (+) ")
                .AppendLine("     AND T2.UPDATE_STF_CD = T12.STF_CD(+) ")
                .AppendLine("     AND T1.DLR_CD = :DLRCD ")
                .AppendLine("     AND T1.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("     AND T2.DLR_CD = :DLRCD ")
                .AppendLine("     AND T2.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("     AND T1.SVC_STATUS IN (:SVC_STATUS1,:SVC_STATUS2) ")
                .AppendLine("     AND T1.CARWASH_NEED_FLG = :CARWASH_NEED_FLG ")
                .AppendLine("     AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("     AND ( T2.SVCIN_ID, T3.RSLT_END_DATETIME ) = ( ")
                .AppendLine("                       SELECT ")
                .AppendLine("                            T17.SVCIN_ID, MAX(T15.RSLT_END_DATETIME) ")
                .AppendLine("                       FROM ")
                .AppendLine("                            TB_T_SERVICEIN T17, TB_T_JOB_DTL T14, TB_T_STALL_USE T15 ")
                .AppendLine("                       WHERE ")
                .AppendLine("                            T17.SVCIN_ID = T14.SVCIN_ID ")
                .AppendLine("                        AND T14.JOB_DTL_ID = T15.JOB_DTL_ID ")
                .AppendLine("                        AND T17.SVCIN_ID = T1.SVCIN_ID ")
                .AppendLine("                        AND T14.CANCEL_FLG=:CANCEL_FLG ")
                .AppendLine("                        GROUP BY T17.SVCIN_ID ")
                .AppendLine("                       ) ")
                .AppendLine("     AND T3.STALL_USE_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                        MAX(STALL_USE_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                        TB_T_STALL_USE T16 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                        T16.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
                .AppendLine(" ) T13 ")
                .AppendLine(" ORDER BY ")
                .AppendLine("   DECODE(T13.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T13.SCHE_DELI_DATETIME) ASC ")
                .AppendLine(" , T13.ACCEPTANCE_TYPE ASC ")
                .AppendLine(" , T13.PICK_DELI_TYPE ASC ")
                .AppendLine(" , DECODE(T13.RSLT_END_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T13.RSLT_END_DATETIME) ASC ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubChipInfoDataTable)("SC3240301_002")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_CARWASHWAIT)                     ' 洗車待ち
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_CARWASHSTART)                     ' 洗車中
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG", OracleDbType.NVarchar2, C_CARWASHNEED)                     ' 洗車必要フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアのNoShowチップ一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        Public Function GetNoShowChipList(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0}_S. dealerCode={1}, branchCode={2}", _
              System.Reflection.MethodBase.GetCurrentMethod.Name, _
              dealerCode, _
              branchCode))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_003 */ ")
                .AppendLine("    T14.SVCIN_ID ")
                .AppendLine("  , T14.DLR_CD ")
                .AppendLine("  , T14.BRN_CD ")
                .AppendLine("  , T14.RO_NUM ")
                .AppendLine("  , T14.CST_ID ")
                .AppendLine("  , T14.VCL_ID ")
                .AppendLine("  , T14.CST_VCL_TYPE  ")
                .AppendLine("  , T14.TLM_CONTRACT_FLG ")
                .AppendLine("  , T14.ACCEPTANCE_TYPE  ")
                .AppendLine("  , T14.PICK_DELI_TYPE ")
                .AppendLine("  , T14.CARWASH_NEED_FLG  ")
                .AppendLine("  , T14.RESV_STATUS ")
                .AppendLine("  , T14.SVC_STATUS ")
                .AppendLine("  , T14.SCHE_SVCIN_DATETIME ")
                .AppendLine("  , T14.SCHE_DELI_DATETIME ")
                .AppendLine("  , T14.RSLT_SVCIN_DATETIME ")
                .AppendLine("  , T14.RSLT_DELI_DATETIME ")
                .AppendLine("  , T14.ROW_UPDATE_DATETIME ")
                .AppendLine("  , T14.ROW_LOCK_VERSION ")
                .AppendLine("  , T14.JOB_DTL_ID ")
                .AppendLine("  , T14.INSPECTION_NEED_FLG ")
                .AppendLine("  , T14.CANCEL_FLG ")
                .AppendLine("  , T14.STALL_USE_ID ")
                .AppendLine("  , T14.STALL_ID ")
                .AppendLine("  , T14.TEMP_FLG ")
                .AppendLine("  , T14.STALL_USE_STATUS ")
                .AppendLine("  , T14.SCHE_START_DATETIME ")
                .AppendLine("  , T14.SCHE_END_DATETIME ")
                .AppendLine("  , T14.SCHE_WORKTIME ")
                .AppendLine("  , T14.REST_FLG ")
                .AppendLine("  , T14.RSLT_START_DATETIME ")
                .AppendLine("  , T14.PRMS_END_DATETIME ")
                .AppendLine("  , T14.RSLT_END_DATETIME ")
                .AppendLine("  , T14.RSLT_WORKTIME ")
                .AppendLine("  , T14.STOP_REASON_TYPE ")
                .AppendLine("  , T14.CST_NAME ")
                .AppendLine("  , T14.VCL_VIN ")
                .AppendLine("  , T14.MODEL_NAME ")
                .AppendLine("  , T14.REG_NUM ")
                .AppendLine("  , T14.CARWASH_RSLT_ID ")
                .AppendLine("  , T14.CW_RSLT_START_DATETIME ")
                .AppendLine("  , T14.CW_RSLT_END_DATETIME ")
                .AppendLine("  , T14.SVC_CLASS_NAME ")
                .AppendLine("  , T14.SVC_CLASS_NAME_ENG ")
                .AppendLine("  , T14.UPPER_DISP ")
                .AppendLine("  , T14.LOWER_DISP ")
                .AppendLine("  , T14.INSPECTION_RSLT_ID ")
                .AppendLine("  , T14.IS_RSLT_START_DATETIME ")
                .AppendLine("  , T14.IS_RSLT_END_DATETIME ")
                .AppendLine("  , T14.PASSING_FLG ")
                .AppendLine("  , T14.STF_CD ")
                .AppendLine("  , T14.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("  , T14.IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine(" FROM ( ")
                .AppendLine("   SELECT ")
                .AppendLine("       T1.SVCIN_ID ")
                .AppendLine("     , T1.DLR_CD ")
                .AppendLine("     , T1.BRN_CD ")
                .AppendLine("     , T1.RO_NUM ")
                .AppendLine("     , T1.CST_ID ")
                .AppendLine("     , T1.VCL_ID ")
                .AppendLine("     , T1.CST_VCL_TYPE ")
                .AppendLine("     , T1.TLM_CONTRACT_FLG ")
                .AppendLine("     , T1.ACCEPTANCE_TYPE ")
                .AppendLine("     , T1.PICK_DELI_TYPE ")
                .AppendLine("     , T1.CARWASH_NEED_FLG ")
                .AppendLine("     , T1.RESV_STATUS ")
                .AppendLine("     , T1.SVC_STATUS ")
                .AppendLine("     , T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("     , T1.SCHE_DELI_DATETIME ")
                .AppendLine("     , T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("     , T1.RSLT_DELI_DATETIME ")
                .AppendLine("     , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("     , T1.ROW_LOCK_VERSION ")
                .AppendLine("     , T2.JOB_DTL_ID ")
                .AppendLine("     , T2.INSPECTION_NEED_FLG ")
                .AppendLine("     , T2.CANCEL_FLG ")
                .AppendLine("     , T3.STALL_USE_ID ")
                .AppendLine("     , T3.STALL_ID ")
                .AppendLine("     , T3.TEMP_FLG ")
                .AppendLine("     , T3.STALL_USE_STATUS ")
                .AppendLine("     , T3.SCHE_START_DATETIME ")
                .AppendLine("     , T3.SCHE_END_DATETIME ")
                .AppendLine("     , T3.SCHE_WORKTIME ")
                .AppendLine("     , T3.REST_FLG  ")
                .AppendLine("     , T3.RSLT_START_DATETIME ")
                .AppendLine("     , T3.PRMS_END_DATETIME  ")
                .AppendLine("     , T3.RSLT_END_DATETIME ")
                .AppendLine("     , T3.RSLT_WORKTIME  ")
                .AppendLine("     , T3.STOP_REASON_TYPE ")
                .AppendLine("     , T4.CST_NAME ")
                .AppendLine("     , T5.VCL_VIN ")
                .AppendLine("     , T6.MODEL_NAME ")
                .AppendLine("     , T7.REG_NUM ")
                .AppendLine("     , T8.CARWASH_RSLT_ID ")
                .AppendLine("     , T8.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("     , T8.RSLT_END_DATETIME AS CW_RSLT_END_DATETIME ")
                .AppendLine("     , T9.SVC_CLASS_NAME ")
                .AppendLine("     , T9.SVC_CLASS_NAME_ENG ")
                .AppendLine("     , T10.UPPER_DISP ")
                .AppendLine("     , T10.LOWER_DISP ")
                .AppendLine("     , T11.INSPECTION_RSLT_ID ")
                .AppendLine("     , T11.RSLT_START_DATETIME AS IS_RSLT_START_DATETIME ")
                .AppendLine("     , T11.RSLT_END_DATETIME AS IS_RSLT_END_DATETIME ")
                .AppendLine("     , T11.PASSING_FLG ")
                .AppendLine("     , T12.STF_CD ")
                .AppendLine("     , T12.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("     , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("       TB_T_SERVICEIN T1 ")
                .AppendLine("     , TB_T_JOB_DTL T2 ")
                .AppendLine("     , TB_T_STALL_USE T3 ")
                .AppendLine("     , TB_M_CUSTOMER T4 ")
                .AppendLine("     , TB_M_VEHICLE T5 ")
                .AppendLine("     , TB_M_MODEL T6 ")
                .AppendLine("     , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("     , TB_T_CARWASH_RESULT T8 ")
                .AppendLine("     , TB_M_SERVICE_CLASS T9 ")
                .AppendLine("     , TB_M_MERCHANDISE T10 ")
                .AppendLine("     , TB_T_INSPECTION_RESULT T11 ")
                .AppendLine("     , TB_M_STAFF T12 ")
                .AppendLine("   WHERE ")
                .AppendLine("         T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("     AND T1.CST_ID = T4.CST_ID(+) ")
                .AppendLine("     AND T1.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("     AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("     AND T1.VCL_ID = T7.VCL_ID(+) ")
                .AppendLine("     AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("     AND T1.SVCIN_ID = T8.SVCIN_ID(+) ")
                .AppendLine("     AND T2.SVC_CLASS_ID = T9.SVC_CLASS_ID(+) ")
                .AppendLine("     AND T2.MERC_ID = T10.MERC_ID (+) ")
                .AppendLine("     AND T3.JOB_DTL_ID = T11.JOB_DTL_ID (+) ")
                .AppendLine("     AND T2.UPDATE_STF_CD = T12.STF_CD(+) ")
                .AppendLine("     AND T1.DLR_CD = :DLRCD ")
                .AppendLine("     AND T1.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("     AND T2.DLR_CD = :DLRCD ")
                .AppendLine("     AND T2.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("     AND T1.SVC_STATUS = :SVC_STATUS  ")
                .AppendLine("     AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("     AND T3.STALL_USE_STATUS = :STALL_USE_STATUS ")
                .AppendLine("     AND T3.STALL_USE_ID=( ")
                .AppendLine("                       SELECT ")
                .AppendLine("                           MAX(STALL_USE_ID) ")
                .AppendLine("                       FROM ")
                .AppendLine("                           TB_T_STALL_USE T13 ")
                .AppendLine("                       WHERE ")
                .AppendLine("                           T13.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                           ) ")
                .AppendLine(" ) T14 ")
                .AppendLine(" ORDER BY ")
                .AppendLine("     DECODE(T14.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T14.SCHE_DELI_DATETIME) ASC ")
                .AppendLine("   , T14.ACCEPTANCE_TYPE ASC ")
                .AppendLine("   , T14.PICK_DELI_TYPE ASC ")
                .AppendLine("   , T14.SVCIN_ID ASC ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubChipInfoDataTable)("SC3240301_003")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, C_SVC_STATUS_NOSHOW)                     ' サービスステータス
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, C_STALLUSE_STATUS_NOSHOW)                     ' ストール利用ステータス
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアの中断チップ一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        Public Function GetStopChipList(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                     "{0}_S. dealerCode={1}, branchCode={2}", _
                                     System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                     dealerCode, _
                                     branchCode))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_004 */ ")
                .AppendLine("    T14.SVCIN_ID ")
                .AppendLine("  , T14.DLR_CD ")
                .AppendLine("  , T14.BRN_CD ")
                .AppendLine("  , T14.RO_NUM ")
                .AppendLine("  , T14.CST_ID ")
                .AppendLine("  , T14.VCL_ID ")
                .AppendLine("  , T14.CST_VCL_TYPE  ")
                .AppendLine("  , T14.TLM_CONTRACT_FLG ")
                .AppendLine("  , T14.ACCEPTANCE_TYPE  ")
                .AppendLine("  , T14.PICK_DELI_TYPE ")
                .AppendLine("  , T14.CARWASH_NEED_FLG  ")
                .AppendLine("  , T14.RESV_STATUS ")
                .AppendLine("  , T14.SVC_STATUS ")
                .AppendLine("  , T14.SCHE_SVCIN_DATETIME ")
                .AppendLine("  , T14.SCHE_DELI_DATETIME ")
                .AppendLine("  , T14.RSLT_SVCIN_DATETIME ")
                .AppendLine("  , T14.RSLT_DELI_DATETIME ")
                .AppendLine("  , T14.ROW_UPDATE_DATETIME ")
                .AppendLine("  , T14.ROW_LOCK_VERSION ")
                .AppendLine("  , T14.JOB_DTL_ID ")
                .AppendLine("  , T14.INSPECTION_NEED_FLG ")
                .AppendLine("  , T14.CANCEL_FLG ")
                .AppendLine("  , T14.STALL_USE_ID ")
                .AppendLine("  , T14.STALL_ID ")
                .AppendLine("  , T14.TEMP_FLG ")
                .AppendLine("  , T14.STALL_USE_STATUS ")
                .AppendLine("  , T14.SCHE_START_DATETIME ")
                .AppendLine("  , T14.SCHE_END_DATETIME ")
                .AppendLine("  , T14.SCHE_WORKTIME ")
                .AppendLine("  , T14.REST_FLG ")
                .AppendLine("  , T14.RSLT_START_DATETIME ")
                .AppendLine("  , T14.PRMS_END_DATETIME ")
                .AppendLine("  , T14.RSLT_END_DATETIME ")
                .AppendLine("  , T14.RSLT_WORKTIME ")
                .AppendLine("  , T14.STOP_REASON_TYPE ")
                .AppendLine("  , T14.CST_NAME ")
                .AppendLine("  , T14.VCL_VIN ")
                .AppendLine("  , T14.MODEL_NAME ")
                .AppendLine("  , T14.REG_NUM ")
                .AppendLine("  , T14.CARWASH_RSLT_ID ")
                .AppendLine("  , T14.CW_RSLT_START_DATETIME ")
                .AppendLine("  , T14.CW_RSLT_END_DATETIME ")
                .AppendLine("  , T14.SVC_CLASS_NAME ")
                .AppendLine("  , T14.SVC_CLASS_NAME_ENG ")
                .AppendLine("  , T14.UPPER_DISP ")
                .AppendLine("  , T14.LOWER_DISP ")
                .AppendLine("  , T14.INSPECTION_RSLT_ID ")
                .AppendLine("  , T14.IS_RSLT_START_DATETIME ")
                .AppendLine("  , T14.IS_RSLT_END_DATETIME ")
                .AppendLine("  , T14.PASSING_FLG ")
                .AppendLine("  , T14.STF_CD ")
                .AppendLine("  , T14.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("  , T14.IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine(" FROM ( ")
                .AppendLine("   SELECT ")
                .AppendLine("       T1.SVCIN_ID ")
                .AppendLine("     , T1.DLR_CD ")
                .AppendLine("     , T1.BRN_CD ")
                .AppendLine("     , T1.RO_NUM ")
                .AppendLine("     , T1.CST_ID ")
                .AppendLine("     , T1.VCL_ID ")
                .AppendLine("     , T1.CST_VCL_TYPE ")
                .AppendLine("     , T1.TLM_CONTRACT_FLG ")
                .AppendLine("     , T1.ACCEPTANCE_TYPE ")
                .AppendLine("     , T1.PICK_DELI_TYPE ")
                .AppendLine("     , T1.CARWASH_NEED_FLG ")
                .AppendLine("     , T1.RESV_STATUS ")
                .AppendLine("     , T1.SVC_STATUS ")
                .AppendLine("     , T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("     , T1.SCHE_DELI_DATETIME ")
                .AppendLine("     , T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("     , T1.RSLT_DELI_DATETIME ")
                .AppendLine("     , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("     , T1.ROW_LOCK_VERSION ")
                .AppendLine("     , T2.JOB_DTL_ID ")
                .AppendLine("     , T2.INSPECTION_NEED_FLG ")
                .AppendLine("     , T2.CANCEL_FLG ")
                .AppendLine("     , T3.STALL_USE_ID ")
                .AppendLine("     , T3.STALL_ID ")
                .AppendLine("     , T3.TEMP_FLG ")
                .AppendLine("     , T3.STALL_USE_STATUS ")
                .AppendLine("     , T3.SCHE_START_DATETIME ")
                .AppendLine("     , T3.SCHE_END_DATETIME ")
                .AppendLine("     , T3.SCHE_WORKTIME ")
                .AppendLine("     , T3.REST_FLG  ")
                .AppendLine("     , T3.RSLT_START_DATETIME ")
                .AppendLine("     , T3.PRMS_END_DATETIME  ")
                .AppendLine("     , T3.RSLT_END_DATETIME ")
                .AppendLine("     , T3.RSLT_WORKTIME  ")
                .AppendLine("     , T3.STOP_REASON_TYPE ")
                .AppendLine("     , T4.CST_NAME ")
                .AppendLine("     , T5.VCL_VIN ")
                .AppendLine("     , T6.MODEL_NAME ")
                .AppendLine("     , T7.REG_NUM ")
                .AppendLine("     , T8.CARWASH_RSLT_ID ")
                .AppendLine("     , T8.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("     , T8.RSLT_END_DATETIME AS CW_RSLT_END_DATETIME ")
                .AppendLine("     , T9.SVC_CLASS_NAME ")
                .AppendLine("     , T9.SVC_CLASS_NAME_ENG ")
                .AppendLine("     , T10.UPPER_DISP ")
                .AppendLine("     , T10.LOWER_DISP ")
                .AppendLine("     , T11.INSPECTION_RSLT_ID ")
                .AppendLine("     , T11.RSLT_START_DATETIME AS IS_RSLT_START_DATETIME ")
                .AppendLine("     , T11.RSLT_END_DATETIME AS IS_RSLT_END_DATETIME ")
                .AppendLine("     , T11.PASSING_FLG ")
                .AppendLine("     , T12.STF_CD ")
                .AppendLine("     , T12.STF_NAME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("     , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("       TB_T_SERVICEIN T1 ")
                .AppendLine("     , TB_T_JOB_DTL T2 ")
                .AppendLine("     , TB_T_STALL_USE T3 ")
                .AppendLine("     , TB_M_CUSTOMER T4 ")
                .AppendLine("     , TB_M_VEHICLE T5 ")
                .AppendLine("     , TB_M_MODEL T6 ")
                .AppendLine("     , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("     , TB_T_CARWASH_RESULT T8 ")
                .AppendLine("     , TB_M_SERVICE_CLASS T9 ")
                .AppendLine("     , TB_M_MERCHANDISE T10 ")
                .AppendLine("     , TB_T_INSPECTION_RESULT T11 ")
                .AppendLine("     , TB_M_STAFF T12 ")
                .AppendLine("   WHERE ")
                .AppendLine("         T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("     AND T1.CST_ID = T4.CST_ID(+) ")
                .AppendLine("     AND T1.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("     AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("     AND T1.VCL_ID = T7.VCL_ID(+) ")
                .AppendLine("     AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("     AND T1.SVCIN_ID = T8.SVCIN_ID(+) ")
                .AppendLine("     AND T2.SVC_CLASS_ID = T9.SVC_CLASS_ID(+) ")
                .AppendLine("     AND T2.MERC_ID = T10.MERC_ID (+) ")
                .AppendLine("     AND T3.JOB_DTL_ID = T11.JOB_DTL_ID (+) ")
                .AppendLine("     AND T2.UPDATE_STF_CD = T12.STF_CD(+) ")
                .AppendLine("     AND T1.DLR_CD = :DLRCD ")
                .AppendLine("     AND T1.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("     AND T1.SVC_STATUS NOT IN (:SVC_STATUS1,:SVC_STATUS2) ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("     AND T2.DLR_CD = :DLRCD ")
                .AppendLine("     AND T2.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("     AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("     AND T3.STALL_USE_STATUS = :STALL_USE_STATUS ")
                .AppendLine("     AND T3.STALL_USE_ID=( ")
                .AppendLine("                       SELECT ")
                .AppendLine("                           MAX(STALL_USE_ID) ")
                .AppendLine("                       FROM ")
                .AppendLine("                           TB_T_STALL_USE T13 ")
                .AppendLine("                       WHERE ")
                .AppendLine("                           T13.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                           ) ")
                .AppendLine(" ) T14 ")
                .AppendLine(" ORDER BY ")
                .AppendLine("    DECODE(T14.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T14.SCHE_DELI_DATETIME) ASC ")
                .AppendLine("  , T14.ACCEPTANCE_TYPE ASC ")
                .AppendLine("  , T14.PICK_DELI_TYPE ASC ")
                .AppendLine("  , DECODE(T14.RSLT_END_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T14.RSLT_END_DATETIME) ASC ")
                .AppendLine("  , T14.SVCIN_ID ASC ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubChipInfoDataTable)("SC3240301_004")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, C_STALLUSE_STATUS_STOP) 'ストール利用ステータス
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_CANCEL) 'サービスステータス　キャンセル
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_DELI) 'サービスステータス　納車済み
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアの完成検査チップ一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCompletedInspectionChipList(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                         "{0}_S. dealerCode={1}, branchCode={2}", _
                         System.Reflection.MethodBase.GetCurrentMethod.Name, _
                         dealerCode, _
                         branchCode))

            Dim sql As New StringBuilder

            ' SQL文の作成       
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_005 */ ")
                .AppendLine("    T14.SVCIN_ID ")
                .AppendLine("  , T14.DLR_CD ")
                .AppendLine("  , T14.BRN_CD ")
                .AppendLine("  , T14.RO_NUM ")
                .AppendLine("  , T14.CST_ID ")
                .AppendLine("  , T14.VCL_ID ")
                .AppendLine("  , T14.CST_VCL_TYPE  ")
                .AppendLine("  , T14.TLM_CONTRACT_FLG ")
                .AppendLine("  , T14.ACCEPTANCE_TYPE  ")
                .AppendLine("  , T14.PICK_DELI_TYPE ")
                .AppendLine("  , T14.CARWASH_NEED_FLG  ")
                .AppendLine("  , T14.RESV_STATUS ")
                .AppendLine("  , T14.SVC_STATUS ")
                .AppendLine("  , T14.SCHE_SVCIN_DATETIME ")
                .AppendLine("  , T14.SCHE_DELI_DATETIME ")
                .AppendLine("  , T14.RSLT_SVCIN_DATETIME ")
                .AppendLine("  , T14.RSLT_DELI_DATETIME ")
                .AppendLine("  , T14.ROW_UPDATE_DATETIME ")
                .AppendLine("  , T14.ROW_LOCK_VERSION ")
                .AppendLine("  , T14.JOB_DTL_ID ")
                .AppendLine("  , T14.DMS_JOB_DTL_ID ")
                .AppendLine("  , T14.INSPECTION_NEED_FLG ")
                .AppendLine("  , T14.CANCEL_FLG ")
                .AppendLine("  , T14.STALL_USE_ID ")
                .AppendLine("  , T14.STALL_ID ")
                .AppendLine("  , T14.TEMP_FLG ")
                .AppendLine("  , T14.STALL_USE_STATUS ")
                .AppendLine("  , T14.SCHE_START_DATETIME ")
                .AppendLine("  , T14.SCHE_END_DATETIME ")
                .AppendLine("  , T14.SCHE_WORKTIME ")
                .AppendLine("  , T14.REST_FLG ")
                .AppendLine("  , T14.RSLT_START_DATETIME ")
                .AppendLine("  , T14.PRMS_END_DATETIME ")
                .AppendLine("  , T14.RSLT_END_DATETIME ")
                .AppendLine("  , T14.RSLT_WORKTIME ")
                .AppendLine("  , T14.STOP_REASON_TYPE ")
                .AppendLine("  , T14.CST_NAME ")
                .AppendLine("  , T14.VCL_VIN ")
                .AppendLine("  , T14.MODEL_NAME ")
                .AppendLine("  , T14.REG_NUM ")
                .AppendLine("  , T14.CARWASH_RSLT_ID ")
                .AppendLine("  , T14.CW_RSLT_START_DATETIME ")
                .AppendLine("  , T14.CW_RSLT_END_DATETIME ")
                .AppendLine("  , T14.SVC_CLASS_NAME ")
                .AppendLine("  , T14.SVC_CLASS_NAME_ENG ")
                .AppendLine("  , T14.UPPER_DISP ")
                .AppendLine("  , T14.LOWER_DISP ")
                .AppendLine("  , T14.INSPECTION_RSLT_ID ")
                .AppendLine("  , T14.IS_RSLT_START_DATETIME ")
                .AppendLine("  , T14.IS_RSLT_END_DATETIME ")
                .AppendLine("  , T14.PASSING_FLG ")
                .AppendLine("  , T14.STF_CD ")
                .AppendLine("  , T14.STF_NAME ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("  , T14.VISIT_ID ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("  , T14.IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine(" FROM ( ")
                .AppendLine("   SELECT ")
                .AppendLine("      T1.SVCIN_ID ")
                .AppendLine("    , T1.DLR_CD ")
                .AppendLine("    , T1.BRN_CD ")
                .AppendLine("    , T1.RO_NUM ")
                .AppendLine("    , T1.CST_ID ")
                .AppendLine("    , T1.VCL_ID ")
                .AppendLine("    , T1.CST_VCL_TYPE ")
                .AppendLine("    , T1.TLM_CONTRACT_FLG ")
                .AppendLine("    , T1.ACCEPTANCE_TYPE ")
                .AppendLine("    , T1.PICK_DELI_TYPE ")
                .AppendLine("    , T1.CARWASH_NEED_FLG ")
                .AppendLine("    , T1.RESV_STATUS ")
                .AppendLine("    , T1.SVC_STATUS ")
                .AppendLine("    , T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("    , T1.SCHE_DELI_DATETIME ")
                .AppendLine("    , T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("    , T1.RSLT_DELI_DATETIME ")
                .AppendLine("    , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("    , T1.ROW_LOCK_VERSION ")
                .AppendLine("    , T2.JOB_DTL_ID ")
                .AppendLine("    , T2.DMS_JOB_DTL_ID ")
                .AppendLine("    , T2.INSPECTION_NEED_FLG ")
                .AppendLine("    , T2.CANCEL_FLG ")
                .AppendLine("    , T3.STALL_USE_ID ")
                .AppendLine("    , T3.STALL_ID ")
                .AppendLine("    , T3.TEMP_FLG ")
                .AppendLine("    , T3.STALL_USE_STATUS ")
                .AppendLine("    , T3.SCHE_START_DATETIME ")
                .AppendLine("    , T3.SCHE_END_DATETIME ")
                .AppendLine("    , T3.SCHE_WORKTIME ")
                .AppendLine("    , T3.REST_FLG  ")
                .AppendLine("    , T3.RSLT_START_DATETIME ")
                .AppendLine("    , T3.PRMS_END_DATETIME  ")
                .AppendLine("    , T3.RSLT_END_DATETIME ")
                .AppendLine("    , T3.RSLT_WORKTIME  ")
                .AppendLine("    , T3.STOP_REASON_TYPE ")
                .AppendLine("    , T4.CST_NAME ")
                .AppendLine("    , T5.VCL_VIN ")
                .AppendLine("    , T6.MODEL_NAME ")
                .AppendLine("    , T7.REG_NUM ")
                .AppendLine("    , T8.CARWASH_RSLT_ID ")
                .AppendLine("    , T8.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("    , T8.RSLT_END_DATETIME AS CW_RSLT_END_DATETIME ")
                .AppendLine("    , T9.SVC_CLASS_NAME ")
                .AppendLine("    , T9.SVC_CLASS_NAME_ENG ")
                .AppendLine("    , T10.UPPER_DISP ")
                .AppendLine("    , T10.LOWER_DISP ")
                .AppendLine("    , T11.INSPECTION_RSLT_ID ")
                .AppendLine("    , T11.RSLT_START_DATETIME AS IS_RSLT_START_DATETIME ")
                .AppendLine("    , T11.RSLT_END_DATETIME AS IS_RSLT_END_DATETIME ")
                .AppendLine("    , T11.PASSING_FLG ")
                .AppendLine("    , T12.STF_CD ")
                .AppendLine("    , T12.STF_NAME ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("    , T15.VISIT_ID ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("    , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("      TB_T_SERVICEIN T1 ")
                .AppendLine("    , TB_T_JOB_DTL T2 ")
                .AppendLine("    , TB_T_STALL_USE T3 ")
                .AppendLine("    , TB_M_CUSTOMER T4 ")
                .AppendLine("    , TB_M_VEHICLE T5 ")
                .AppendLine("    , TB_M_MODEL T6 ")
                .AppendLine("    , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("    , TB_T_CARWASH_RESULT T8 ")
                .AppendLine("    , TB_M_SERVICE_CLASS T9 ")
                .AppendLine("    , TB_M_MERCHANDISE T10 ")
                .AppendLine("    , TB_T_INSPECTION_RESULT T11 ")
                .AppendLine("    , TB_M_STAFF T12 ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("    , TB_T_RO_INFO T15 ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("   WHERE ")
                .AppendLine("        T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("    AND T1.CST_ID = T4.CST_ID(+) ")
                .AppendLine("    AND T1.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("    AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("    AND T1.VCL_ID = T7.VCL_ID(+) ")
                .AppendLine("    AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("    AND T1.SVCIN_ID = T8.SVCIN_ID(+) ")
                .AppendLine("    AND T2.SVC_CLASS_ID = T9.SVC_CLASS_ID(+) ")
                .AppendLine("    AND T2.MERC_ID = T10.MERC_ID (+) ")
                .AppendLine("    AND T3.JOB_DTL_ID = T11.JOB_DTL_ID (+) ")
                .AppendLine("    AND T2.UPDATE_STF_CD = T12.STF_CD(+) ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("    AND T1.SVCIN_ID = T15.SVCIN_ID ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("    AND T1.DLR_CD = :DLRCD ")
                .AppendLine("    AND T1.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("    AND T2.DLR_CD = :DLRCD ")
                .AppendLine("    AND T2.BRN_CD = :STRCD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("    AND T1.SVC_STATUS IN (:SVC_STATUS1, :SVC_STATUS2, :SVC_STATUS3) ")
                .AppendLine("    AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("    AND T2.INSPECTION_STATUS = :INSPECTION_STATUS ")
                .AppendLine("    AND T3.STALL_USE_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                       MAX(STALL_USE_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                       TB_T_STALL_USE T13 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                       T13.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '複数レコードが発生するので、親チップの来店実績番号とする
                .AppendLine("    AND T15.RO_SEQ=0 ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine(" ) T14 ")
                .AppendLine(" ORDER BY ")
                .AppendLine("    DECODE(T14.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T14.SCHE_DELI_DATETIME) ASC ")
                .AppendLine("  , T14.ACCEPTANCE_TYPE ASC ")
                .AppendLine("  , T14.PICK_DELI_TYPE ASC ")
                .AppendLine("  , DECODE(T14.RSLT_END_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),T14.PRMS_END_DATETIME,T14.RSLT_END_DATETIME) ASC ")
                .AppendLine("  , T14.SVCIN_ID ASC ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubChipInfoDataTable)("SC3240301_005")

                query.CommandText = sql.ToString()

                ' バインド変数定義      
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                                ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                                ' 店舗コード
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_WORKING)                ' サービスステータス：05(作業中)
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_NEXTSTARTWAIT)          ' サービスステータス：06(次の作業開始待ち)
                query.AddParameterWithTypeValue("SVC_STATUS3", OracleDbType.NVarchar2, C_SVC_STATUS_INSPECTIONWAIT)         ' サービスステータス：09(検査待ち)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                         ' キャンセルフラグ：0(有効)
                query.AddParameterWithTypeValue("INSPECTION_STATUS", OracleDbType.NVarchar2, C_INSPECTION_APPROVAL_WAIT)    ' 完成検査ステータス：1(完成検査承認待ち)
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                ' 最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアの追加作業チップ一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        '''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        Public Function GetAddWorkChipList(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable
            'Public Function GetAddWorkChipList(ByVal inroNumlist As List(Of String), _
            '                           ByVal dealerCode As String, _
            '                              ByVal branchCode As String) As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                         "{0}_S. dealerCode={1}, branchCode={2}", _
                         System.Reflection.MethodBase.GetCurrentMethod.Name, _
                         dealerCode, _
                         branchCode))

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            '' R/O番号取得文字列
            'Dim subSql As New StringBuilder
            'Dim roNumName As String
            'Dim count As Integer = 0
            'subSql.Append(" ( T1.RO_NUM IN ( ")
            '' DBSelectQueryインスタンス生成
            'Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable)("SC3240301_006")
            '    For Each roNum In inroNumlist
            '        If String.IsNullOrWhiteSpace(roNum) Then
            '            Continue For
            '        End If
            '        ' SQL作成
            '        roNumName = String.Format(CultureInfo.CurrentCulture, "RO_NUM{0}", count)
            '        If count >= 1000 AndAlso count Mod 1000 = 0 Then
            '            subSql.Append(" ) OR ( T1.RO_NUM IN ( ")
            '            subSql.Append(String.Format(CultureInfo.CurrentCulture, ":{0} ", roNumName))
            '        ElseIf count = 0 Then
            '            subSql.Append(String.Format(CultureInfo.CurrentCulture, ":{0} ", roNumName))
            '        Else
            '            subSql.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", roNumName))
            '        End If
            '        ' パラメータ作成
            '        query.AddParameterWithTypeValue(roNumName, OracleDbType.NVarchar2, roNum) 'RO番号
            '        count = count + 1
            '    Next
            '    subSql.Append(" ) )")
            '    Dim sql As New StringBuilder
            '    ' SQL文の作成
            '    With sql
            '        .Append(" SELECT ")
            '        .Append("  /* SC3240301_006 */ ")
            '        .Append("    T8.SVCIN_ID ")
            '        .Append("  , T8.DLR_CD ")
            '        .Append("  , T8.BRN_CD ")
            '        .Append("  , T8.RO_NUM ")
            '        .Append("  , T8.CST_ID ")
            '        .Append("  , T8.VCL_ID ")
            '        .Append("  , T8.CST_VCL_TYPE  ")
            '        .Append("  , T8.TLM_CONTRACT_FLG ")
            '        .Append("  , T8.ACCEPTANCE_TYPE  ")
            '        .Append("  , T8.PICK_DELI_TYPE ")
            '        .Append("  , T8.CARWASH_NEED_FLG  ")
            '        .Append("  , T8.RESV_STATUS ")
            '        .Append("  , T8.SVC_STATUS ")
            '        .Append("  , T8.SCHE_SVCIN_DATETIME ")
            '        .Append("  , T8.RSLT_SVCIN_DATETIME ")
            '        .Append("  , T8.RSLT_DELI_DATETIME ")
            '        .Append("  , T8.ROW_UPDATE_DATETIME ")
            '        .Append("  , T8.ROW_LOCK_VERSION ")
            '        .Append("  , T8.JOB_DTL_ID ")
            '        .Append("  , T8.CANCEL_FLG ")
            '        .Append("  , T8.CST_NAME ")
            '        .Append("  , T8.VCL_VIN ")
            '        .Append("  , T8.MODEL_NAME ")
            '        .Append("  , T8.REG_NUM ")
            '        .Append(" FROM ( ")
            '        .Append("   SELECT ")
            '        .Append("      T1.SVCIN_ID ")
            '        .Append("    , T1.DLR_CD ")
            '        .Append("    , T1.BRN_CD ")
            '        .Append("    , T1.RO_NUM ")
            '        .Append("    , T1.CST_ID ")
            '        .Append("    , T1.VCL_ID ")
            '        .Append("    , T1.CST_VCL_TYPE ")
            '        .Append("    , T1.TLM_CONTRACT_FLG ")
            '        .Append("    , T1.ACCEPTANCE_TYPE ")
            '        .Append("    , T1.PICK_DELI_TYPE ")
            '        .Append("    , T1.CARWASH_NEED_FLG ")
            '        .Append("    , T1.RESV_STATUS ")
            '        .Append("    , T1.SVC_STATUS ")
            '        .Append("    , T1.SCHE_SVCIN_DATETIME ")
            '        .Append("    , T1.RSLT_SVCIN_DATETIME ")
            '        .Append("    , T1.RSLT_DELI_DATETIME ")
            '        .Append("    , T1.ROW_UPDATE_DATETIME ")
            '        .Append("    , T1.ROW_LOCK_VERSION ")
            '        .Append("    , T2.JOB_DTL_ID ")
            '        .Append("    , T2.CANCEL_FLG ")
            '        .Append("    , T4.CST_NAME ")
            '        .Append("    , T5.VCL_VIN ")
            '        .Append("    , T6.MODEL_NAME ")
            '        .Append("    , T7.REG_NUM ")
            '        .Append("   FROM ")
            '        .Append("      TB_T_SERVICEIN T1 ")
            '        .Append("    , TB_T_JOB_DTL T2 ")
            '        .Append("    , TB_M_CUSTOMER T4 ")
            '        .Append("    , TB_M_VEHICLE T5 ")
            '        .Append("    , TB_M_MODEL T6 ")
            '        .Append("    , TB_M_VEHICLE_DLR T7 ")
            '        .Append("   WHERE ")
            '        .Append("        T1.SVCIN_ID = T2.SVCIN_ID ")
            '        .Append("    AND T1.CST_ID = T4.CST_ID(+) ")
            '        .Append("    AND T1.VCL_ID = T5.VCL_ID (+) ")
            '        .Append("    AND T5.MODEL_CD = T6.MODEL_CD (+) ")
            '        .Append("    AND T1.VCL_ID = T7.VCL_ID(+) ")
            '        .Append("    AND T1.DLR_CD = T7.DLR_CD (+) ")
            '        .Append("    AND T2.DLR_CD = :DLRCD ")
            '        .Append("    AND T2.BRN_CD = :STRCD ")
            '        .Append("    AND T2.JOB_DTL_ID=( ")
            '        .Append("                   SELECT ")
            '        .Append("                        MIN(JOB_DTL_ID) ")
            '        .Append("                   FROM ")
            '        .Append("                        TB_T_JOB_DTL T8 ")
            '        .Append("                   WHERE ")
            '        .Append("                        T8.SVCIN_ID=T1.SVCIN_ID ")
            '        .Append("                    AND T8.CANCEL_FLG=:CANCEL_FLG ")
            '        .Append("                       ) ")
            '        .Append("    AND ")
            '        .Append(subSql.ToString)
            '        .Append(" ) T8 ")
            '        .Append(" ORDER BY ")
            '        .Append("    T8.ACCEPTANCE_TYPE ASC ")
            '        .Append("  , T8.PICK_DELI_TYPE ASC ")
            '    End With
            '    query.CommandText = sql.ToString()
            '    ' バインド変数定義
            '    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
            '    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
            '    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ

            '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            '    ' 検索結果の返却
            '    Return query.GetData()
            'End Using

            Dim sql As New StringBuilder
            ' SQL文の作成
            With sql
                .AppendLine("   SELECT ")
                .AppendLine("  　　/* SC3240301_006 */ ")
                .AppendLine("      T1.SVCIN_ID ")
                .AppendLine("    , T1.DLR_CD ")
                .AppendLine("    , T1.BRN_CD ")
                .AppendLine("    , T1.CST_ID ")
                .AppendLine("    , T1.VCL_ID ")
                .AppendLine("    , T1.CST_VCL_TYPE ")
                .AppendLine("    , T1.TLM_CONTRACT_FLG ")
                .AppendLine("    , T1.ACCEPTANCE_TYPE ")
                .AppendLine("    , T1.PICK_DELI_TYPE ")
                .AppendLine("    , T1.CARWASH_NEED_FLG ")
                .AppendLine("    , T1.RESV_STATUS ")
                .AppendLine("    , T1.SVC_STATUS ")
                .AppendLine("    , T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("    , T1.SCHE_DELI_DATETIME  ")
                .AppendLine("    , T1.RSLT_SVCIN_DATETIME  ")
                .AppendLine("    , T1.RSLT_DELI_DATETIME ")
                .AppendLine("    , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("    , T1.ROW_LOCK_VERSION ")
                .AppendLine("    , T2.JOB_DTL_ID ")
                .AppendLine("    , T2.DMS_JOB_DTL_ID ")
                .AppendLine("    , T2.CANCEL_FLG ")
                .AppendLine("    , T2.INSPECTION_NEED_FLG ")
                .AppendLine("    , T3.STALL_USE_ID ")
                .AppendLine("    , T3.STALL_ID ")
                .AppendLine("    , T3.REST_FLG ")
                .AppendLine("    , T3.TEMP_FLG ")
                .AppendLine("    , T3.STALL_USE_STATUS ")
                .AppendLine("    , T3.SCHE_START_DATETIME ")
                .AppendLine("    , T3.SCHE_END_DATETIME ")
                .AppendLine("    , T3.SCHE_WORKTIME ")
                .AppendLine("    , T3.RSLT_START_DATETIME ")
                .AppendLine("    , T3.RSLT_END_DATETIME ")
                .AppendLine("    , T3.PRMS_END_DATETIME ")
                .AppendLine("    , T4.CST_NAME ")
                .AppendLine("    , T5.VCL_VIN ")
                .AppendLine("    , T6.MODEL_NAME ")
                .AppendLine("    , T7.REG_NUM ")
                .AppendLine("    , T8.SVC_CLASS_NAME ")
                .AppendLine("    , T8.SVC_CLASS_NAME_ENG ")
                .AppendLine("    , T9.UPPER_DISP ")
                .AppendLine("    , T9.LOWER_DISP ")
                .AppendLine("    , T10.RO_NUM ")
                .AppendLine("    , T10.RO_SEQ AS RO_JOB_SEQ ")
                .AppendLine("    , T10.VISIT_ID ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("    , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("      TB_T_SERVICEIN T1 ")
                .AppendLine("    , TB_T_JOB_DTL T2 ")
                .AppendLine("    , TB_T_STALL_USE T3 ")
                .AppendLine("    , TB_M_CUSTOMER T4 ")
                .AppendLine("    , TB_M_VEHICLE T5 ")
                .AppendLine("    , TB_M_MODEL T6 ")
                .AppendLine("    , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("    , TB_M_SERVICE_CLASS T8 ")
                .AppendLine("    , TB_M_MERCHANDISE T9 ")
                .AppendLine("    , TB_T_RO_INFO T10 ")
                .AppendLine("   WHERE ")
                .AppendLine("         T1.SVCIN_ID = T2.SVCIN_ID  ")
                .AppendLine("     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID  ")
                .AppendLine("     AND T1.CST_ID = T4.CST_ID(+) ")
                .AppendLine("     AND T1.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("     AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("     AND T1.VCL_ID = T7.VCL_ID(+) ")
                .AppendLine("     AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("     AND T2.SVC_CLASS_ID = T8.SVC_CLASS_ID(+) ")
                .AppendLine("     AND T2.MERC_ID = T9.MERC_ID(+) ")
                .AppendLine("     AND T1.SVCIN_ID = T10.SVCIN_ID ")
                .AppendLine("     AND T1.RO_NUM <>' ' ")
                .AppendLine("     AND T1.DLR_CD = :DLRCD ")
                .AppendLine("     AND T1.BRN_CD = :STRCD ")
                'コスト改善
                .AppendLine("     AND T2.DLR_CD = :DLRCD ")
                .AppendLine("     AND T2.BRN_CD = :STRCD ")

                .AppendLine("     AND T2.JOB_DTL_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                        MIN(T11.JOB_DTL_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                        TB_T_JOB_DTL T11 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                        T11.SVCIN_ID=T1.SVCIN_ID ")
                .AppendLine("                        AND T11.CANCEL_FLG=:CANCEL_FLG ")
                .AppendLine("                       ) ")
                .AppendLine("     AND T3.STALL_USE_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                       MAX(STALL_USE_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                       TB_T_STALL_USE T12 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                       T12.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                        ) ")
                .AppendLine("     AND T10.RO_STATUS=:RO_STATUS ")
                .AppendLine("     AND EXISTS( ")
                .AppendLine("        SELECT ")
                .AppendLine("           1 ")
                .AppendLine("        FROM ")
                .AppendLine("           TB_T_JOB_DTL T14 ")
                .AppendLine("        WHERE ")
                .AppendLine("           T14.SVCIN_ID=T1.SVCIN_ID ")
                .AppendLine("           AND T14.CANCEL_FLG=:CANCEL_FLG ")
                .AppendLine("           ) ")
                .AppendLine("   ORDER BY ")
                .AppendLine("      T1.SCHE_DELI_DATETIME ASC ")
                .AppendLine("    , T1.ACCEPTANCE_TYPE ASC ")
                .AppendLine("    , T1.PICK_DELI_TYPE ASC ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubChipInfoDataTable)("SC3240301_006")
                query.CommandText = sql.ToString()
                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, C_RO_STATUS_WAITFMAPP)    ' ROステータス(「20」FM承認待ち)
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                ' 検索結果の返却
                Return query.GetData()
            End Using
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        End Function

        ''' <summary>
        ''' サブチップエリアの受付チップ一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 2015/07/16 TMEJ 河原 TMT_N/W問題緊急対応_SQL007チューニング対応
        ''' </remarks>
        ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' Public Function GetReceptionChipList(ByVal inroNumlist As List(Of String), _
        '''                                      ByVal dealerCode As String, _
        '''                                      ByVal branchCode As String) As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
        Public Function GetReceptionChipList(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                         "{0}_S. dealerCode={1}, branchCode={2}", _
                         System.Reflection.MethodBase.GetCurrentMethod.Name, _
                         dealerCode, _
                         branchCode))

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            '' R/O番号取得文字列
            'Dim subSql As New StringBuilder
            'Dim roNumName As String
            'Dim count As Integer = 0
            'subSql.Append(" ( T1.RO_NUM IN ( ")
            '' DBSelectQueryインスタンス生成
            'Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable)("SC3240301_007")
            '    For Each roNum In inroNumlist
            '        If String.IsNullOrWhiteSpace(roNum) Then
            '            Continue For
            '        End If
            '        ' SQL作成
            '        roNumName = String.Format(CultureInfo.CurrentCulture, "RO_NUM{0}", count)
            '        If count >= 1000 AndAlso count Mod 1000 = 0 Then
            '            subSql.Append(" ) OR ( T1.RO_NUM IN ( ")
            '            subSql.Append(String.Format(CultureInfo.CurrentCulture, ":{0} ", roNumName))
            '        ElseIf count = 0 Then
            '            subSql.Append(String.Format(CultureInfo.CurrentCulture, ":{0} ", roNumName))
            '        Else
            '            subSql.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", roNumName))
            '        End If
            '        ' パラメータ作成
            '        query.AddParameterWithTypeValue(roNumName, OracleDbType.NVarchar2, roNum) 'RO番号
            '        count = count + 1
            '    Next
            '    subSql.Append(" ) )")
            '    Dim sql As New StringBuilder
            '    ' SQL文の作成
            '    With sql
            '        .Append(" SELECT ")
            '        .Append("  /* SC3240301_007 */ ")
            '        .Append("    T10.SVCIN_ID ")
            '        .Append("  , T10.DLR_CD ")
            '        .Append("  , T10.BRN_CD ")
            '        .Append("  , T10.RO_NUM ")
            '        .Append("  , T10.CST_ID ")
            '        .Append("  , T10.VCL_ID ")
            '        .Append("  , T10.CST_VCL_TYPE  ")
            '        .Append("  , T10.TLM_CONTRACT_FLG ")
            '        .Append("  , T10.ACCEPTANCE_TYPE  ")
            '        .Append("  , T10.PICK_DELI_TYPE ")
            '        .Append("  , T10.CARWASH_NEED_FLG  ")
            '        .Append("  , T10.RESV_STATUS ")
            '        .Append("  , T10.SVC_STATUS ")
            '        .Append("  , T10.SCHE_SVCIN_DATETIME ")
            '        .Append("  , T10.PARENTS_SCHE_DELI_DATETIME ")
            '        .Append("  , T10.RSLT_SVCIN_DATETIME ")
            '        .Append("  , T10.RSLT_DELI_DATETIME ")
            '        .Append("  , T10.ROW_UPDATE_DATETIME ")
            '        .Append("  , T10.ROW_LOCK_VERSION ")
            '        .Append("  , T10.JOB_DTL_ID ")
            '        .Append("  , T10.PARENTS_RO_JOB_SEQ ")
            '        .Append("  , T10.CANCEL_FLG ")
            '        .Append("  , T10.STALL_USE_ID ")
            '        .Append("  , T10.STALL_ID ")
            '        .Append("  , T10.REST_FLG ")
            '        .Append("  , T10.SCHE_START_DATETIME ")
            '        .Append("  , T10.SCHE_END_DATETIME ")
            '        .Append("  , T10.SCHE_WORKTIME ")
            '        .Append("  , T10.RSLT_START_DATETIME ")
            '        .Append("  , T10.RSLT_END_DATETIME ")
            '        .Append("  , T10.CST_NAME ")
            '        .Append("  , T10.VCL_VIN ")
            '        .Append("  , T10.MODEL_NAME ")
            '        .Append("  , T10.REG_NUM ")
            '        .Append(" FROM ( ")
            '        .Append("   SELECT ")
            '        .Append("      T1.SVCIN_ID ")
            '        .Append("    , T1.DLR_CD ")
            '        .Append("    , T1.BRN_CD ")
            '        .Append("    , T1.RO_NUM ")
            '        .Append("    , T1.CST_ID ")
            '        .Append("    , T1.VCL_ID ")
            '        .Append("    , T1.CST_VCL_TYPE ")
            '        .Append("    , T1.TLM_CONTRACT_FLG ")
            '        .Append("    , T1.ACCEPTANCE_TYPE ")
            '        .Append("    , T1.PICK_DELI_TYPE ")
            '        .Append("    , T1.CARWASH_NEED_FLG ")
            '        .Append("    , T1.RESV_STATUS ")
            '        .Append("    , T1.SVC_STATUS ")
            '        .Append("    , T1.SCHE_SVCIN_DATETIME ")
            '        .Append("    , T1.SCHE_DELI_DATETIME AS PARENTS_SCHE_DELI_DATETIME ")
            '        .Append("    , T1.RSLT_SVCIN_DATETIME  ")
            '        .Append("    , T1.RSLT_DELI_DATETIME ")
            '        .Append("    , T1.ROW_UPDATE_DATETIME ")
            '        .Append("    , T1.ROW_LOCK_VERSION ")
            '        .Append("    , T2.JOB_DTL_ID ")
            '        .Append("    , T2.RO_JOB_SEQ AS PARENTS_RO_JOB_SEQ ")
            '        .Append("    , T2.CANCEL_FLG ")
            '        .Append("    , T3.STALL_USE_ID ")
            '        .Append("    , T3.STALL_ID ")
            '        .Append("    , T3.REST_FLG ")
            '        .Append("    , T3.SCHE_START_DATETIME ")
            '        .Append("    , T3.SCHE_END_DATETIME ")
            '        .Append("    , T3.SCHE_WORKTIME ")
            '        .Append("    , T3.RSLT_START_DATETIME ")
            '        .Append("    , T3.RSLT_END_DATETIME ")
            '        .Append("    , T4.CST_NAME ")
            '        .Append("    , T5.VCL_VIN ")
            '        .Append("    , T6.MODEL_NAME ")
            '        .Append("    , T7.REG_NUM ")
            '        .Append("   FROM ")
            '        .Append("      TB_T_SERVICEIN T1 ")
            '        .Append("    , TB_T_JOB_DTL T2 ")
            '        .Append("    , TB_T_STALL_USE T3 ")
            '        .Append("    , TB_M_CUSTOMER T4 ")
            '        .Append("    , TB_M_VEHICLE T5 ")
            '        .Append("    , TB_M_MODEL T6 ")
            '        .Append("    , TB_M_VEHICLE_DLR T7 ")
            '        .Append("   WHERE ")
            '        .Append("        T1.SVCIN_ID = T2.SVCIN_ID  ")
            '        .Append("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID  ")
            '        .Append("    AND T1.CST_ID = T4.CST_ID(+) ")
            '        .Append("    AND T1.VCL_ID = T5.VCL_ID (+) ")
            '        .Append("    AND T5.MODEL_CD = T6.MODEL_CD (+) ")
            '        .Append("    AND T1.VCL_ID = T7.VCL_ID(+) ")
            '        .Append("    AND T1.DLR_CD = T7.DLR_CD (+) ")
            '        .Append("    AND T1.DLR_CD = :DLRCD ")
            '        .Append("    AND T1.BRN_CD = :STRCD ")
            '        .Append("    AND T2.JOB_DTL_ID=( ")
            '        .Append("                   SELECT ")
            '        .Append("                        MIN(JOB_DTL_ID) ")
            '        .Append("                   FROM ")
            '        .Append("                        TB_T_JOB_DTL T8 ")
            '        .Append("                   WHERE ")
            '        .Append("                        T8.SVCIN_ID=T1.SVCIN_ID ")
            '        .Append("                    AND T8.CANCEL_FLG=:CANCEL_FLG ")
            '        .Append("                       ) ")
            '        .Append("    AND T3.STALL_USE_ID=( ")
            '        .Append("                   SELECT ")
            '        .Append("                       MAX(STALL_USE_ID) ")
            '        .Append("                   FROM ")
            '        .Append("                       TB_T_STALL_USE T9 ")
            '        .Append("                   WHERE ")
            '        .Append("                       T9.JOB_DTL_ID=T2.JOB_DTL_ID ")
            '        .Append("                        ) ")
            '        .Append("    AND ")
            '        .Append(subSql.ToString)
            '        .Append(" ) T10 ")
            '        .Append(" ORDER BY ")
            '        .Append("    T10.ACCEPTANCE_TYPE ASC ")
            '        .Append("  , T10.PICK_DELI_TYPE ASC ")
            '    End With
            '    query.CommandText = sql.ToString()
            '    ' バインド変数定義
            '    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
            '    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
            '    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ


            '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            '    ' 検索結果の返却
            '    Return query.GetData()
            'End Using

            Dim sql As New StringBuilder
            ' SQL文の作成
            With sql
                .AppendLine("   SELECT /* SC3240301_007 */ ")
                .AppendLine("          T1.SVCIN_ID ")
                .AppendLine("        , T1.DLR_CD ")
                .AppendLine("        , T1.BRN_CD ")
                .AppendLine("        , T1.CST_ID ")
                .AppendLine("        , T1.VCL_ID ")
                .AppendLine("        , T1.CST_VCL_TYPE ")
                .AppendLine("        , T1.TLM_CONTRACT_FLG ")
                .AppendLine("        , T1.ACCEPTANCE_TYPE ")
                .AppendLine("        , T1.PICK_DELI_TYPE ")
                .AppendLine("        , T1.CARWASH_NEED_FLG ")
                .AppendLine("        , T1.RESV_STATUS ")
                .AppendLine("        , T1.SVC_STATUS ")
                .AppendLine("        , T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("        , T1.SCHE_DELI_DATETIME  ")
                .AppendLine("        , T1.RSLT_SVCIN_DATETIME  ")
                .AppendLine("        , T1.RSLT_DELI_DATETIME ")
                .AppendLine("        , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("        , T1.ROW_LOCK_VERSION ")
                .AppendLine("        , T2.JOB_DTL_ID ")
                .AppendLine("        , T2.CANCEL_FLG ")
                .AppendLine("        , T2.INSPECTION_NEED_FLG ")
                '2017/10/16 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("        , T2.INSPECTION_STATUS ")
                '2017/10/16 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("        , T3.STALL_USE_ID ")
                .AppendLine("        , T3.STALL_ID ")
                .AppendLine("        , T3.REST_FLG ")

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                '.AppendLine("        , T3.TEMP_FLG ")
                .AppendLine("        , :TEMP_FLG_NOT AS TEMP_FLG ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                .AppendLine("        , T3.STALL_USE_STATUS ")
                .AppendLine("        , T3.SCHE_START_DATETIME ")
                .AppendLine("        , T3.SCHE_END_DATETIME ")
                .AppendLine("        , T3.SCHE_WORKTIME ")
                .AppendLine("        , T3.RSLT_START_DATETIME ")
                .AppendLine("        , T3.RSLT_END_DATETIME ")
                .AppendLine("        , T3.PRMS_END_DATETIME ")
                .AppendLine("        , T4.CST_NAME ")
                .AppendLine("        , T5.VCL_VIN ")
                .AppendLine("        , T6.MODEL_NAME ")
                .AppendLine("        , T7.REG_NUM ")
                .AppendLine("        , T8.SVC_CLASS_NAME ")
                .AppendLine("        , T8.SVC_CLASS_NAME_ENG ")
                .AppendLine("        , T9.UPPER_DISP ")
                .AppendLine("        , T9.LOWER_DISP ")
                .AppendLine("        , T10.RO_NUM ")
                .AppendLine("        , T10.RO_SEQ AS RO_JOB_SEQ ")
                .AppendLine("        , T10.RO_APPROVAL_DATETIME AS CUST_CONFIRMDATE ")

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("        , :DEFAULT_DATE As SCHE_START_DATETIME_SORT ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("        , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("     FROM ")

                '2015/07/16 TMEJ 河原 TMT_N/W問題緊急対応_SQL007チューニング対応 START

                '.AppendLine("          TB_T_SERVICEIN T1 ")
                '.AppendLine("        , TB_T_JOB_DTL T2 ")
                '.AppendLine("        , TB_T_STALL_USE T3 ")
                '.AppendLine("        , TB_M_CUSTOMER T4 ")
                '.AppendLine("        , TB_M_VEHICLE T5 ")
                '.AppendLine("        , TB_M_MODEL T6 ")
                '.AppendLine("        , TB_M_VEHICLE_DLR T7 ")
                '.AppendLine("        , TB_M_SERVICE_CLASS T8 ")
                '.AppendLine("        , TB_M_MERCHANDISE T9 ")
                '.AppendLine("        , TB_T_RO_INFO T10 ")
                '.AppendLine("    WHERE ")
                '.AppendLine("          T1.SVCIN_ID = T2.SVCIN_ID ")
                '.AppendLine("      AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                '.AppendLine("      AND T1.CST_ID = T4.CST_ID(+) ")
                '.AppendLine("      AND T1.VCL_ID = T5.VCL_ID (+) ")
                '.AppendLine("      AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                '.AppendLine("      AND T1.VCL_ID = T7.VCL_ID(+) ")
                '.AppendLine("      AND T1.DLR_CD = T7.DLR_CD (+) ")
                '.AppendLine("      AND T2.SVC_CLASS_ID = T8.SVC_CLASS_ID(+) ")
                '.AppendLine("      AND T2.MERC_ID = T9.MERC_ID(+) ")
                '.AppendLine("      AND T1.SVCIN_ID = T10.SVCIN_ID ")
                '.AppendLine("      AND T1.RO_NUM <> ' ' ")
                '.AppendLine("      AND T1.DLR_CD = :DLRCD ")
                '.AppendLine("      AND T1.BRN_CD = :STRCD ")
                ''コスト改善
                '.AppendLine("      AND T2.DLR_CD = :DLRCD ")
                '.AppendLine("      AND T2.BRN_CD = :STRCD ")

                'SQLチューニング
                '内容：
                '①表結合の順番を変更
                '②ストール利用、RO情報の条件に販売店コード、店舗コードを追加

                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("        , TB_T_JOB_DTL T2 ")
                .AppendLine("        , TB_T_STALL_USE T3 ")
                .AppendLine("        , TB_T_RO_INFO T10 ")
                .AppendLine("        , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("        , TB_M_VEHICLE T5 ")
                .AppendLine("        , TB_M_MODEL T6 ")
                .AppendLine("        , TB_M_CUSTOMER T4 ")
                .AppendLine("        , TB_M_SERVICE_CLASS T8 ")
                .AppendLine("        , TB_M_MERCHANDISE T9 ")
                .AppendLine("    WHERE ")
                .AppendLine("          T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("      AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("      AND T1.SVCIN_ID = T10.SVCIN_ID ")
                .AppendLine("      AND T1.VCL_ID = T7.VCL_ID (+) ")
                .AppendLine("      AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("      AND T7.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("      AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("      AND T1.CST_ID = T4.CST_ID (+) ")
                .AppendLine("      AND T2.SVC_CLASS_ID = T8.SVC_CLASS_ID (+) ")
                .AppendLine("      AND T2.MERC_ID = T9.MERC_ID (+) ")
                .AppendLine("      AND T1.RO_NUM <> ' ' ")
                .AppendLine("      AND T1.DLR_CD = :DLRCD ")
                .AppendLine("      AND T1.BRN_CD = :STRCD ")
                .AppendLine("      AND T2.DLR_CD = :DLRCD ")
                .AppendLine("      AND T2.BRN_CD = :STRCD ")
                .AppendLine("      AND T3.DLR_CD = :DLRCD ")
                .AppendLine("      AND T3.BRN_CD = :STRCD ")
                .AppendLine("      AND T10.DLR_CD = :DLRCD ")
                .AppendLine("      AND T10.BRN_CD = :STRCD ")

                '2015/07/16 TMEJ 河原 TMT_N/W問題緊急対応_SQL007チューニング対応 END

                .AppendLine("      AND T2.JOB_DTL_ID = ")
                .AppendLine("          ( SELECT ")
                .AppendLine("                   MIN(T11.JOB_DTL_ID) ")
                .AppendLine("              FROM ")
                .AppendLine("                   TB_T_JOB_DTL T11 ")
                .AppendLine("             WHERE ")
                .AppendLine("                   T11.SVCIN_ID = T1.SVCIN_ID ")
                .AppendLine("               AND T11.CANCEL_FLG = :CANCEL_FLG ) ")
                .AppendLine("      AND T3.STALL_USE_ID = ")
                .AppendLine("          ( SELECT ")
                .AppendLine("                   MAX(STALL_USE_ID) ")
                .AppendLine("              FROM ")
                .AppendLine("                   TB_T_STALL_USE T12 ")
                .AppendLine("             WHERE ")
                .AppendLine("                   T12.JOB_DTL_ID = T2.JOB_DTL_ID ) ")

                '.AppendLine("    AND T10.RO_STATUS=:RO_STATUS ")
                .AppendLine("      AND T10.RO_STATUS IN (:RO_STATUS_50, :RO_STATUS_60) ")

                .AppendLine("      AND EXISTS ")
                .AppendLine("          ( SELECT ")
                .AppendLine("                   1 ")
                .AppendLine("              FROM ")
                .AppendLine("                   TB_T_JOB_INSTRUCT T13 ")

                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

                .AppendLine("                 , TB_T_JOB_DTL T15 ")
                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("                 , TB_T_STALL_USE T16 ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                .AppendLine("             WHERE ")
                .AppendLine("                   T13.RO_NUM = T10.RO_NUM ")
                .AppendLine("               AND T13.RO_SEQ = T10.RO_SEQ ")

                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

                .AppendLine("               AND T13.JOB_DTL_ID = T15.JOB_DTL_ID ")

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("               AND T13.JOB_DTL_ID = T16.JOB_DTL_ID ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                .AppendLine("               AND T15.DLR_CD = :DLRCD ")
                .AppendLine("               AND T15.BRN_CD = :STRCD ")
                .AppendLine("               AND T15.CANCEL_FLG = :CANCEL_FLG ")

                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                '.AppendLine("               AND T13.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG ) ")
                '.AppendLine("      AND EXISTS ")
                '.AppendLine("          ( SELECT ")
                '.AppendLine("                   1 ")
                '.AppendLine("              FROM ")
                '.AppendLine("                   TB_T_JOB_DTL T14 ")
                '.AppendLine("             WHERE ")
                '.AppendLine("                   T14.SVCIN_ID = T1.SVCIN_ID ")
                '.AppendLine("               AND T14.CANCEL_FLG = :CANCEL_FLG ) ")
                '.AppendLine(" ORDER BY ")
                '.AppendLine("          T10.RO_APPROVAL_DATETIME ASC ")
                '.AppendLine("        , T1.ACCEPTANCE_TYPE ASC ")
                '.AppendLine("        , T1.PICK_DELI_TYPE ASC ")

                .AppendLine("               AND T13.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG ")
                .AppendLine("               AND EXISTS ")
                .AppendLine("                   ( SELECT ")
                .AppendLine("                            1 ")
                .AppendLine("                       FROM ")
                .AppendLine("                            TB_T_STALL_USE T17 ")
                .AppendLine("                      WHERE ")
                .AppendLine("                            T17.JOB_DTL_ID = T15.JOB_DTL_ID ")
                .AppendLine("                       HAVING ")
                .AppendLine("                            MAX(T17.STALL_USE_ID) = T16.STALL_USE_ID ) ")
                .AppendLine("                            AND T16.TEMP_FLG = :TEMP_FLG_OFF ) ")

                .AppendLine(" UNION ALL ")
                .AppendLine(" SELECT  ")
                .AppendLine("          T1.SVCIN_ID ")
                .AppendLine("        , T1.DLR_CD ")
                .AppendLine("        , T1.BRN_CD ")
                .AppendLine("        , T1.CST_ID ")
                .AppendLine("        , T1.VCL_ID ")
                .AppendLine("        , T1.CST_VCL_TYPE ")
                .AppendLine("        , T1.TLM_CONTRACT_FLG ")
                .AppendLine("        , T1.ACCEPTANCE_TYPE ")
                .AppendLine("        , T1.PICK_DELI_TYPE ")
                .AppendLine("        , T1.CARWASH_NEED_FLG ")
                .AppendLine("        , T1.RESV_STATUS ")
                .AppendLine("        , T1.SVC_STATUS ")
                .AppendLine("        , T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("        , T1.SCHE_DELI_DATETIME  ")
                .AppendLine("        , T1.RSLT_SVCIN_DATETIME  ")
                .AppendLine("        , T1.RSLT_DELI_DATETIME ")
                .AppendLine("        , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("        , T1.ROW_LOCK_VERSION ")
                .AppendLine("        , T2.JOB_DTL_ID ")
                .AppendLine("        , T2.CANCEL_FLG ")
                .AppendLine("        , T2.INSPECTION_NEED_FLG ")
                '2017/10/16 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("        , T2.INSPECTION_STATUS ")
                '2017/10/16 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("        , T3.STALL_USE_ID ")
                .AppendLine("        , T3.STALL_ID ")
                .AppendLine("        , T3.REST_FLG ")
                .AppendLine("        , T3.TEMP_FLG ")
                .AppendLine("        , T3.STALL_USE_STATUS ")
                .AppendLine("        , T3.SCHE_START_DATETIME ")
                .AppendLine("        , T3.SCHE_END_DATETIME ")
                .AppendLine("        , T3.SCHE_WORKTIME ")
                .AppendLine("        , T3.RSLT_START_DATETIME ")
                .AppendLine("        , T3.RSLT_END_DATETIME ")
                .AppendLine("        , T3.PRMS_END_DATETIME ")
                .AppendLine("        , T4.CST_NAME ")
                .AppendLine("        , T5.VCL_VIN ")
                .AppendLine("        , T6.MODEL_NAME ")
                .AppendLine("        , T7.REG_NUM ")
                .AppendLine("        , T8.SVC_CLASS_NAME ")
                .AppendLine("        , T8.SVC_CLASS_NAME_ENG ")
                .AppendLine("        , T9.UPPER_DISP ")
                .AppendLine("        , T9.LOWER_DISP ")

                '更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("        , T1.RO_NUM ")
                '更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                .AppendLine("        , :DEFAULT_RO_SEQ AS  RO_JOB_SEQ ")
                .AppendLine("        , CASE T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("          WHEN :DEFAULT_DATE THEN :MAXDATE ")
                .AppendLine("          ELSE  T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("          END AS CUST_CONFIRMDATE ")
                .AppendLine("        , T3.SCHE_START_DATETIME AS SCHE_START_DATETIME_SORT ")

                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("        , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine(" FROM  ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("        , TB_T_JOB_DTL T2 ")
                .AppendLine("        , TB_T_STALL_USE T3 ")
                .AppendLine("        , TB_M_CUSTOMER T4 ")
                .AppendLine("        , TB_M_VEHICLE T5 ")
                .AppendLine("        , TB_M_MODEL T6 ")
                .AppendLine("        , TB_M_VEHICLE_DLR T7 ")
                .AppendLine("        , TB_M_SERVICE_CLASS T8 ")
                .AppendLine("        , TB_M_MERCHANDISE T9 ")
                .AppendLine(" WHERE ")
                .AppendLine("          T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("       AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("       AND T1.CST_ID = T4.CST_ID (+) ")
                .AppendLine("       AND T1.VCL_ID = T5.VCL_ID (+) ")
                .AppendLine("       AND T5.MODEL_CD = T6.MODEL_CD (+) ")
                .AppendLine("       AND T1.VCL_ID = T7.VCL_ID (+) ")
                .AppendLine("       AND T1.DLR_CD = T7.DLR_CD (+) ")
                .AppendLine("       AND T2.SVC_CLASS_ID = T8.SVC_CLASS_ID (+) ")
                .AppendLine("       AND T2.MERC_ID = T9.MERC_ID (+) ")
                .AppendLine("       AND T1.DLR_CD = :DLRCD ")
                .AppendLine("       AND T1.BRN_CD = :STRCD ")
                .AppendLine("       AND T2.CANCEL_FLG = :CANCEL_FLG_OFF ")
                .AppendLine("       AND T1.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01, :SVC_STATUS_03, ")
                .AppendLine("                               :SVC_STATUS_04, :SVC_STATUS_05, :SVC_STATUS_06) ")
                .AppendLine("       AND EXISTS ")
                .AppendLine("           ( SELECT 1")
                .AppendLine("             FROM ")
                .AppendLine("                 TB_T_STALL_USE T11 ")
                .AppendLine("             WHERE ")
                .AppendLine("                 T11.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("             HAVING ")
                .AppendLine("                 MAX(T11.STALL_USE_ID) = T3.STALL_USE_ID) ")
                .AppendLine("       AND T3.TEMP_FLG = :TEMP_FLG ")

                .AppendLine(" ORDER BY ")
                .AppendLine("          CUST_CONFIRMDATE ")
                .AppendLine("        , SCHE_START_DATETIME_SORT ")
                .AppendLine("        , JOB_DTL_ID ")
            End With
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301SubChipInfoDataTable)("SC3240301_007")
                query.CommandText = sql.ToString()
                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, C_INSTRUCT_FLG_NOT)   ' 着手指示フラグ(「0」未着工指示)
                'query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, C_RO_STATUS_CSTAPPROVED)                     ' ROステータス(「50」作業開始待ち)
                query.AddParameterWithTypeValue("RO_STATUS_50", OracleDbType.NVarchar2, C_RO_STATUS_CSTAPPROVED)        ' ROステータス(「50」作業開始待ち)
                query.AddParameterWithTypeValue("RO_STATUS_60", OracleDbType.NVarchar2, C_RO_STATUS_WORKING)            ' ROステータス(「60」作業中)

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                query.AddParameterWithTypeValue("DEFAULT_DATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))  'デフォルト日時
                query.AddParameterWithTypeValue("TEMP_FLG_OFF", OracleDbType.NVarchar2, C_NOT_TEMP)

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                'query.AddParameterWithTypeValue("DEFAULT_RO_NUM", OracleDbType.NVarchar2, C_RO_NUM)
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                query.AddParameterWithTypeValue("DEFAULT_RO_SEQ", OracleDbType.Decimal, C_RO_SEQ)
                query.AddParameterWithTypeValue("CANCEL_FLG_OFF", OracleDbType.NVarchar2, C_NOT_CANCEL)
                query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, C_TEMP)
                query.AddParameterWithTypeValue("TEMP_FLG_NOT", OracleDbType.NVarchar2, C_NOT_TEMP)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, C_SVC_STATUS_NOTCARIN)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, C_SVC_STATUS_NOSHOW)
                query.AddParameterWithTypeValue("SVC_STATUS_03", OracleDbType.NVarchar2, C_SVC_STATUS_STARTWORKINSTRUCTWAIT)
                query.AddParameterWithTypeValue("SVC_STATUS_04", OracleDbType.NVarchar2, C_SVC_STATUS_STARTWORKWAIT)
                query.AddParameterWithTypeValue("SVC_STATUS_05", OracleDbType.NVarchar2, C_SVC_STATUS_WORKING)
                query.AddParameterWithTypeValue("SVC_STATUS_06", OracleDbType.NVarchar2, C_SVC_STATUS_NEXTSTARTWAIT)
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                ' 検索結果の返却
                Return query.GetData()
            End Using
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        End Function

        ''' <summary>
        ''' サブチップエリアの中断ボタンの情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStopButtonInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
             "{0}_S. dealerCode={1}, branchCode={2}, inNowDate={3}", _
             System.Reflection.MethodBase.GetCurrentMethod.Name, _
             dealerCode, _
             branchCode, _
             inNowDate))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_008 */ ")
                .AppendLine("     T1.SVCIN_ID ")
                .AppendLine("  ,  CASE WHEN DECODE(T1.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T1.SCHE_DELI_DATETIME)>:NOW THEN 0 ELSE 1 END AS LATEFLG ")
                .AppendLine(" FROM ")
                .AppendLine("     TB_T_SERVICEIN T1 ")
                .AppendLine("  ,  TB_T_JOB_DTL  T2 ")
                .AppendLine("  ,  TB_T_STALL_USE  T3 ")
                .AppendLine(" WHERE  ")
                .AppendLine("      T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("  AND T1.SVC_STATUS NOT IN (:SVC_STATUS1,:SVC_STATUS2) ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("  AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T2.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("  AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("  AND T3.STALL_USE_STATUS = :STALL_USE_STATUS ")
                .AppendLine("  AND T3.STALL_USE_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                       MAX(STALL_USE_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                       TB_T_STALL_USE T4 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                       T4.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301ChipCountDataTable)("SC3240301_008")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, C_STALLUSE_STATUS_STOP) 'ストール利用ステータス
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_CANCEL) 'サービスステータス　キャンセル
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_DELI) 'サービスステータス　納車済み
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                '現在日時
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアのNoShowボタンの情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNoShowButtonInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
             "{0}_S. dealerCode={1}, branchCode={2}, inNowDate={3}", _
             System.Reflection.MethodBase.GetCurrentMethod.Name, _
             dealerCode, _
             branchCode, _
             inNowDate))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_009 */ ")
                .AppendLine("    T1.SVCIN_ID ")
                .AppendLine("  , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T1.SCHE_DELI_DATETIME)>:NOW THEN 0 ELSE 1 END AS LATEFLG ")
                .AppendLine(" FROM ")
                .AppendLine("    TB_T_SERVICEIN T1 ")
                .AppendLine("  , TB_T_JOB_DTL  T2 ")
                .AppendLine("  , TB_T_STALL_USE  T3 ")
                .AppendLine(" WHERE  ")
                .AppendLine("      T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("  AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T2.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("  AND T1.SVC_STATUS = :SVC_STATUS  ")
                .AppendLine("  AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("  AND T3.STALL_USE_STATUS = :STALL_USE_STATUS ")
                .AppendLine("  AND T3.STALL_USE_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                       MAX(STALL_USE_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                       TB_T_STALL_USE T4 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                       T4.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301ChipCountDataTable)("SC3240301_009")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, C_SVC_STATUS_NOSHOW)                     ' サービスステータス
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, C_STALLUSE_STATUS_NOSHOW)                     ' ストール利用ステータス
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                '現在日時
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアの納車待ちボタンの情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDeliverdButtonInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                         "{0}_S. dealerCode={1}, branchCode={2}, inNowDate={3}", _
                         System.Reflection.MethodBase.GetCurrentMethod.Name, _
                         dealerCode, _
                         branchCode, _
                         inNowDate))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_010 */ ")
                .AppendLine("    T1.SVCIN_ID ")
                .AppendLine("  , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T1.SCHE_DELI_DATETIME)>:NOW THEN 0 ELSE 1 END AS LATEFLG ")
                .AppendLine(" FROM ")
                .AppendLine("   TB_T_SERVICEIN T1 ")
                .AppendLine("  ,TB_T_JOB_DTL  T2 ")
                .AppendLine("  ,TB_T_STALL_USE  T3 ")
                .AppendLine(" WHERE  ")
                .AppendLine("      T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("　AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("　AND T2.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("  AND T1.SVC_STATUS IN (:SVC_STATUS1,:SVC_STATUS2) ")
                .AppendLine("  AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("  AND T2.JOB_DTL_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                        MAX(JOB_DTL_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                        TB_T_JOB_DTL T14 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                        T14.SVCIN_ID=T1.SVCIN_ID ")
                .AppendLine("                    AND T14.CANCEL_FLG=:CANCEL_FLG ")
                .AppendLine("                   ) ")
                .AppendLine("  AND T3.STALL_USE_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                        MAX(STALL_USE_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                        TB_T_STALL_USE T15 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                        T15.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301ChipCountDataTable)("SC3240301_010")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_WAITINGDELI)                     ' 納車待ち（Waiting）
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_DROPOFF)                     ' 預かり中（DropOff）
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                '現在日時
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップボックスの洗車ボタンの情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCarWashButtonInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                         "{0}_S. dealerCode={1}, branchCode={2}, inNowDate={3}", _
                         System.Reflection.MethodBase.GetCurrentMethod.Name, _
                         dealerCode, _
                         branchCode, _
                         inNowDate))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_011 */ ")
                .AppendLine("    T1.SVCIN_ID ")
                .AppendLine("  , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T1.SCHE_DELI_DATETIME)>:NOW THEN 0 ELSE 1 END AS LATEFLG ")
                .AppendLine(" FROM ")
                .AppendLine("   TB_T_SERVICEIN T1 ")
                .AppendLine("  ,TB_T_JOB_DTL  T2 ")
                .AppendLine("  ,TB_T_STALL_USE  T3 ")
                .AppendLine(" WHERE  ")
                .AppendLine("      T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("　AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("　AND T2.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("  AND T1.SVC_STATUS IN (:SVC_STATUS1,:SVC_STATUS2) ")
                .AppendLine("  AND T1.CARWASH_NEED_FLG = :CARWASH_NEED_FLG ")
                .AppendLine("  AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("  AND T2.JOB_DTL_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                        MAX(JOB_DTL_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                        TB_T_JOB_DTL T14 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                        T14.SVCIN_ID=T1.SVCIN_ID ")
                .AppendLine("                    AND T14.CANCEL_FLG=:CANCEL_FLG ")
                .AppendLine("                    ) ")
                .AppendLine("  AND T3.STALL_USE_ID=( ")
                .AppendLine("                   SELECT ")
                .AppendLine("                       MAX(STALL_USE_ID) ")
                .AppendLine("                   FROM ")
                .AppendLine("                       TB_T_STALL_USE T15 ")
                .AppendLine("                   WHERE ")
                .AppendLine("                       T15.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301ChipCountDataTable)("SC3240301_011")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_CARWASHWAIT)                     ' 洗車待ち
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_CARWASHSTART)                     ' 洗車中
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG", OracleDbType.NVarchar2, C_CARWASHNEED)                     ' 洗車必要フラグ
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                '現在日時
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアの完成検査ボタンの情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCompletedInspectionButtonInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                         "{0}_S. dealerCode={1}, branchCode={2}, inNowDate={3}", _
                         System.Reflection.MethodBase.GetCurrentMethod.Name, _
                         dealerCode, _
                         branchCode, _
                         inNowDate))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3240301_012 */ ")
                .AppendLine("    T1.SVCIN_ID ")
                .AppendLine("  , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T1.SCHE_DELI_DATETIME)>:NOW THEN 0 ELSE 1 END AS LATEFLG ")
                .AppendLine(" FROM ")
                .AppendLine("   TB_T_SERVICEIN T1 ")
                .AppendLine("  ,TB_T_JOB_DTL  T2 ")
                .AppendLine("  ,TB_T_STALL_USE  T3 ")
                .AppendLine("  ,TB_T_RO_INFO  T4 ")
                .AppendLine(" WHERE  ")
                .AppendLine("      T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("  AND T1.SVCIN_ID = T4.SVCIN_ID ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'コスト改善
                .AppendLine("  AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T2.BRN_CD = :BRN_CD ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("  AND T1.SVC_STATUS IN (:SVC_STATUS1, :SVC_STATUS2, :SVC_STATUS3) ")
                .AppendLine("  AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("  AND T2.INSPECTION_STATUS = :INSPECTION_STATUS  ")
                .AppendLine("  AND T3.STALL_USE_ID=( ")
                .AppendLine("                       SELECT ")
                .AppendLine("                           MAX(STALL_USE_ID) ")
                .AppendLine("                       FROM ")
                .AppendLine("                           TB_T_STALL_USE T5 ")
                .AppendLine("                       WHERE ")
                .AppendLine("                           T5.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                       ) ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("  AND T4.RO_SEQ=0 ")
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301ChipCountDataTable)("SC3240301_012")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                               ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                               ' 店舗コード
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, C_SVC_STATUS_WORKING)                ' サービスステータス：05(作業中)
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, C_SVC_STATUS_NEXTSTARTWAIT)          ' サービスステータス：06(次の作業開始待ち)
                query.AddParameterWithTypeValue("SVC_STATUS3", OracleDbType.NVarchar2, C_SVC_STATUS_INSPECTIONWAIT)         ' サービスステータス：09(検査待ち)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                         ' キャンセルフラグ
                query.AddParameterWithTypeValue("INSPECTION_STATUS", OracleDbType.NVarchar2, C_INSPECTION_APPROVAL_WAIT)    ' 完成検査承認待ちフラグ
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                        ' 現在日時
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                ' 最大日時
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' サブチップエリアの受付ボタンの情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除）
        ''' </remarks>
        ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        Public Function GetReceptionButtonInfo(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable
            'Public Function GetReceptionAddWorkButtonInfo(ByVal dealerCode As String, _
            '                                   ByVal branchCode As String, _
            '                                   ByVal inroNumlist As List(Of String), _
            '                                   ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
             "{0}_S. dealerCode={1}, branchCode={2}, inNowDate={3}", _
             System.Reflection.MethodBase.GetCurrentMethod.Name, _
             dealerCode, _
             branchCode, _
             inNowDate))

            ' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            '' R/O番号取得文字列
            'Dim subSql As New StringBuilder
            'Dim roNumName As String
            'Dim count As Integer = 0
            'subSql.AppendLine(" ( T1.RO_NUM IN ( ")

            'For Each roNum In inroNumlist
            '    ' SQL作成
            '    roNumName = String.Format(CultureInfo.CurrentCulture, "RO_NUM{0}", count)

            '    If count >= 1000 AndAlso count Mod 1000 = 0 Then
            '        subSql.AppendLine(" ) OR ( T1.RO_NUM IN ( ")
            '        subSql.AppendLine(String.Format(CultureInfo.CurrentCulture, ":{0} ", roNumName))
            '    ElseIf count = 0 Then
            '        subSql.AppendLine(String.Format(CultureInfo.CurrentCulture, ":{0} ", roNumName))
            '    Else
            '        subSql.AppendLine(String.Format(CultureInfo.CurrentCulture, ", :{0} ", roNumName))
            '    End If

            '    ' パラメータ作成
            '    query.AddParameterWithTypeValue(roNumName, OracleDbType.NVarchar2, roNum) 'RO番号

            '    count = count + 1
            'Next
            'subSql.AppendLine(" ) )")
            'Dim sql As New StringBuilder

            '' SQL文の作成
            'With sql
            '    .AppendLine(" SELECT ")
            '    .AppendLine("  /* SC3240301_013 */ ")
            '    .AppendLine("    T1.SVCIN_ID ")
            '    .AppendLine("  , T1.RO_NUM ")
            '    .AppendLine("  , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T1.SCHE_DELI_DATETIME)>:NOW THEN 0 ELSE 1 END AS LATEFLG ")
            '    .AppendLine("   FROM ")
            '    .AppendLine("      TB_T_SERVICEIN T1 ")
            '    .AppendLine("    , TB_T_JOB_DTL T2 ")
            '    .AppendLine("    , TB_T_STALL_USE T3 ")
            '    .AppendLine("   WHERE ")
            '    .AppendLine("        T1.SVCIN_ID = T2.SVCIN_ID (+) ")
            '    .AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID (+) ")
            '    .AppendLine("    AND T1.DLR_CD = :DLR_CD ")
            '    .AppendLine("    AND T1.BRN_CD = :BRN_CD ")
            '    .AppendLine("    AND T2.JOB_DTL_ID=( ")
            '    .AppendLine("                   SELECT ")
            '    .AppendLine("                        MIN(JOB_DTL_ID) ")
            '    .AppendLine("                   FROM ")
            '    .AppendLine("                        TB_T_JOB_DTL T8 ")
            '    .AppendLine("                   WHERE ")
            '    .AppendLine("                        T8.SVCIN_ID=T1.SVCIN_ID ")
            '    .AppendLine("                    AND T8.CANCEL_FLG=:CANCEL_FLG ")
            '    .AppendLine("                       ) ")
            '    .AppendLine("    AND T3.STALL_USE_ID=( ")
            '    .AppendLine("                   SELECT ")
            '    .AppendLine("                       MAX(STALL_USE_ID) ")
            '    .AppendLine("                   FROM ")
            '    .AppendLine("                       TB_T_STALL_USE T5 ")
            '    .AppendLine("                   WHERE ")
            '    .AppendLine("                       T5.JOB_DTL_ID=T2.JOB_DTL_ID ")
            '    .AppendLine("                        ) ")
            '    .AppendLine("  AND ")
            '    .AppendLine(subSql.ToString)
            'End With
            'query.CommandText = sql.ToString()

            '' バインド変数定義
            'query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
            'query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
            'query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                '現在日時
            'query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
            'query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
            ' SQL文の作成
            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT /* SC3240301_013 */ ")
                .AppendLine("        T1.SVCIN_ID ")
                .AppendLine("      , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME, TO_DATE('19000101000000','YYYYMMDDHH24MISS'), :MAXDATE, T1.SCHE_DELI_DATETIME) > :NOW THEN 0 ELSE 1 END AS LATEFLG ")
                .AppendLine("      , T4.RO_APPROVAL_DATETIME AS CUST_CONFIRMDATE ")
                .AppendLine("      , T4.RO_NUM ")
                .AppendLine("      , T4.RO_SEQ AS RO_JOB_SEQ ")

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("      , :C_TEMP_FLG_NOT AS TEMP_FLG ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN T1 ")
                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） START
                '.AppendLine("      , TB_T_JOB_DTL T2 ")
                '.AppendLine("      , TB_T_STALL_USE T3 ")
                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） END
                .AppendLine("      , TB_T_RO_INFO T4 ")
                .AppendLine("  WHERE ")
                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） START
                '.AppendLine("        T1.SVCIN_ID = T2.SVCIN_ID ")
                '.AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                '.AppendLine("    AND T1.SVCIN_ID=T4.SVCIN_ID ")
                .AppendLine("        T1.SVCIN_ID = T4.SVCIN_ID ")
                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） END
                .AppendLine("    AND T1.RO_NUM <> ' ' ")
                .AppendLine("    AND T1.DLR_CD = :DLRCD ")
                .AppendLine("    AND T1.BRN_CD = :STRCD ")
                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） START
                '.AppendLine("    AND T2.DLR_CD = :DLRCD ")
                '.AppendLine("    AND T2.BRN_CD = :STRCD ")
                .AppendLine("    AND T4.DLR_CD = :DLRCD ")
                .AppendLine("    AND T4.BRN_CD = :STRCD ")

                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） START
                '.AppendLine("    AND T2.JOB_DTL_ID = ")
                '.AppendLine("        ( SELECT ")
                '.AppendLine("                 MIN(T5.JOB_DTL_ID) ")
                '.AppendLine("            FROM ")
                '.AppendLine("                 TB_T_JOB_DTL T5 ")
                '.AppendLine("           WHERE ")
                '.AppendLine("                 T5.SVCIN_ID = T1.SVCIN_ID ")
                '.AppendLine("             AND T5.CANCEL_FLG = :CANCEL_FLG ) ")
                '.AppendLine("    AND T3.STALL_USE_ID = ")
                '.AppendLine("        ( SELECT ")
                '.AppendLine("                 MAX(STALL_USE_ID) ")
                '.AppendLine("            FROM ")
                '.AppendLine("                 TB_T_STALL_USE T6 ")
                '.AppendLine("           WHERE ")
                '.AppendLine("                 T6.JOB_DTL_ID = T2.JOB_DTL_ID ) ")
                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） END

                '.AppendLine("    AND T4.RO_STATUS = :RO_STATUS_RECEPTION ")
                .AppendLine("    AND T4.RO_STATUS IN (:RO_STATUS_50, :RO_STATUS_60) ")

                .AppendLine("    AND EXISTS ")
                .AppendLine("        ( SELECT ")
                .AppendLine("                 1 ")
                .AppendLine("            FROM ")
                .AppendLine("                 TB_T_JOB_INSTRUCT T7 ")

                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

                .AppendLine("               , TB_T_JOB_DTL T9 ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("               , TB_T_STALL_USE T10 ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

                .AppendLine("           WHERE ")
                .AppendLine("                 T7.RO_NUM = T4.RO_NUM ")
                .AppendLine("             AND T7.RO_SEQ = T4.RO_SEQ ")

                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

                .AppendLine("             AND T7.JOB_DTL_ID = T9.JOB_DTL_ID ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("             AND T7.JOB_DTL_ID = T10.JOB_DTL_ID ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                .AppendLine("             AND T9.DLR_CD = :DLRCD ")
                .AppendLine("             AND T9.BRN_CD = :STRCD ")
                .AppendLine("             AND T9.CANCEL_FLG = :CANCEL_FLG ")

                '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                '.AppendLine("             AND T7.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG ) ")
                .AppendLine("             AND T7.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG  ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） START
                '.AppendLine("    AND EXISTS ")
                '.AppendLine("        ( SELECT ")
                '.AppendLine("                 1 ")
                '.AppendLine("            FROM ")
                '.AppendLine("                 TB_T_JOB_DTL T8 ")
                '.AppendLine("           WHERE ")
                '.AppendLine("                 T8.SVCIN_ID = T1.SVCIN_ID ")
                '.AppendLine("             AND T8.CANCEL_FLG = :CANCEL_FLG ) ")
                '2015/10/02 TM 小牟禮 SQL013のチューニング対応（不要なexists句を削除） END

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("             AND EXISTS ")
                .AppendLine("                   ( SELECT 1 ")
                .AppendLine("                     FROM ")
                .AppendLine("                        TB_T_STALL_USE T11 ")
                .AppendLine("                     WHERE ")
                .AppendLine("                         T11.JOB_DTL_ID = T10.JOB_DTL_ID ")
                .AppendLine("                     HAVING ")
                .AppendLine("                         MAX(T11.STALL_USE_ID) = T10.STALL_USE_ID) ")
                .AppendLine("             AND T10.TEMP_FLG = :C_TEMP_FLG_NOT ) ")

                .AppendLine(" UNION ALL ")

                .AppendLine(" SELECT  ")
                .AppendLine("        T1.SVCIN_ID ")
                .AppendLine("      , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME, TO_DATE('19000101000000','YYYYMMDDHH24MISS'), :MAXDATE, T1.SCHE_DELI_DATETIME) > :NOW THEN 0 ELSE 1 END AS LATEFLG ")
                .AppendLine("      , :DEFAULT_DATE AS CUST_CONFIRMDATE ")
                .AppendLine("      , :DEFAULT_RO_NUM AS RO_NUM ")
                .AppendLine("      , :DEFAULT_RO_SEQ AS RO_SEQ ")
                .AppendLine("      , T2.TEMP_FLG ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN T1 ")
                .AppendLine("      , TB_T_STALL_USE T2 ")
                .AppendLine("      , TB_T_JOB_DTL T3 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.SVCIN_ID = T3.SVCIN_ID ")
                .AppendLine("    AND T3.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .AppendLine("    AND T1.DLR_CD = :DLRCD ")
                .AppendLine("    AND T1.BRN_CD = :STRCD ")
                .AppendLine("    AND T3.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("    AND T1.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01, :SVC_STATUS_03, ")
                .AppendLine("                           :SVC_STATUS_04, :SVC_STATUS_05, :SVC_STATUS_06) ")
                .AppendLine("    AND EXISTS ")
                .AppendLine("           ( SELECT 1 ")
                .AppendLine("               FROM  ")
                .AppendLine("                   TB_T_STALL_USE T4 ")
                .AppendLine("               WHERE ")
                .AppendLine("                   T4.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .AppendLine("               HAVING ")
                .AppendLine("                   MAX(T4.STALL_USE_ID) = T2.STALL_USE_ID ) ")
                .AppendLine("   AND T2.TEMP_FLG = :C_TEMP_FLG ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START

            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301ChipCountDataTable)("SC3240301_013")
                query.CommandText = sql.ToString()
                ' バインド変数定義
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                '現在日時
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, C_INSTRUCT_FLG_NOT)   ' 着手指示フラグ(「0」未着工指示)
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                'query.AddParameterWithTypeValue("RO_STATUS_RECEPTION", OracleDbType.NVarchar2, C_RO_STATUS_CSTAPPROVED)                     ' ROステータス(「50」お客様承認)
                query.AddParameterWithTypeValue("RO_STATUS_50", OracleDbType.NVarchar2, C_RO_STATUS_CSTAPPROVED)        ' ROステータス(「50」作業開始待ち)
                query.AddParameterWithTypeValue("RO_STATUS_60", OracleDbType.NVarchar2, C_RO_STATUS_WORKING)            ' ROステータス(「60」作業中)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                query.AddParameterWithTypeValue("C_TEMP_FLG", OracleDbType.NVarchar2, C_TEMP)   ' 仮置きフラグ(「1」仮置き)
                query.AddParameterWithTypeValue("C_TEMP_FLG_NOT", OracleDbType.NVarchar2, C_NOT_TEMP)   ' 仮置きフラグ(「0」仮置きではない)
                query.AddParameterWithTypeValue("DEFAULT_DATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture)) '初期設定日時
                query.AddParameterWithTypeValue("DEFAULT_RO_NUM", OracleDbType.NVarchar2, C_RO_NUM)  ' RO番号(初期値)
                query.AddParameterWithTypeValue("DEFAULT_RO_SEQ", OracleDbType.Decimal, C_RO_SEQ) ' RO連番(初期値)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, C_SVC_STATUS_NOTCARIN)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, C_SVC_STATUS_NOSHOW)
                query.AddParameterWithTypeValue("SVC_STATUS_03", OracleDbType.NVarchar2, C_SVC_STATUS_STARTWORKINSTRUCTWAIT)
                query.AddParameterWithTypeValue("SVC_STATUS_04", OracleDbType.NVarchar2, C_SVC_STATUS_STARTWORKWAIT)
                query.AddParameterWithTypeValue("SVC_STATUS_05", OracleDbType.NVarchar2, C_SVC_STATUS_WORKING)
                query.AddParameterWithTypeValue("SVC_STATUS_06", OracleDbType.NVarchar2, C_SVC_STATUS_NEXTSTARTWAIT)
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' サブチップエリアの追加作業ボタンの情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAddWorkButtonInfo(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal inNowDate As Date) As SC3240301DataSet.SC3240301ChipCountDataTable
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
             "{0}_S. dealerCode={1}, branchCode={2}, inNowDate={3}", _
             System.Reflection.MethodBase.GetCurrentMethod.Name, _
             dealerCode, _
             branchCode, _
             inNowDate))


            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301ChipCountDataTable)("SC3240301_014")

                ' SQL文の作成
                Dim sql As New StringBuilder

                With sql
                    .AppendLine(" SELECT ")
                    .AppendLine("  /* SC3240301_014 */ ")
                    .AppendLine("    T1.SVCIN_ID ")
                    .AppendLine("  , CASE WHEN DECODE(T1.SCHE_DELI_DATETIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS'),:MAXDATE,T1.SCHE_DELI_DATETIME)>:NOW THEN 0 ELSE 1 END AS LATEFLG ")
                    .AppendLine("  , T4.RO_NUM ")
                    .AppendLine("  , T4.RO_SEQ AS RO_JOB_SEQ ")
                    .AppendLine("   FROM ")
                    .AppendLine("      TB_T_SERVICEIN T1 ")
                    .AppendLine("    , TB_T_JOB_DTL T2 ")
                    .AppendLine("    , TB_T_STALL_USE T3 ")
                    .AppendLine("    , TB_T_RO_INFO T4 ")
                    .AppendLine("   WHERE ")
                    .AppendLine("        T1.SVCIN_ID = T2.SVCIN_ID  ")
                    .AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID  ")
                    .AppendLine("    AND T1.SVCIN_ID=T4.SVCIN_ID ")
                    .AppendLine("    AND T1.RO_NUM <>' ' ")
                    .AppendLine("    AND T1.DLR_CD = :DLRCD ")
                    .AppendLine("    AND T1.BRN_CD = :STRCD ")
                    .AppendLine("    AND T2.DLR_CD = :DLRCD ")
                    .AppendLine("    AND T2.BRN_CD = :STRCD ")
                    .AppendLine("    AND T2.JOB_DTL_ID=( ")
                    .AppendLine("                   SELECT ")
                    .AppendLine("                        MIN(T5.JOB_DTL_ID) ")
                    .AppendLine("                   FROM ")
                    .AppendLine("                        TB_T_JOB_DTL T5 ")
                    .AppendLine("                   WHERE ")
                    .AppendLine("                        T5.SVCIN_ID=T1.SVCIN_ID ")
                    .AppendLine("                        AND T5.CANCEL_FLG=:CANCEL_FLG ")
                    .AppendLine("                       ) ")
                    .AppendLine("    AND T3.STALL_USE_ID=( ")
                    .AppendLine("                   SELECT ")
                    .AppendLine("                       MAX(STALL_USE_ID) ")
                    .AppendLine("                   FROM ")
                    .AppendLine("                       TB_T_STALL_USE T6 ")
                    .AppendLine("                   WHERE ")
                    .AppendLine("                       T6.JOB_DTL_ID=T2.JOB_DTL_ID ")
                    .AppendLine("                        ) ")
                    .AppendLine("    AND T4.RO_STATUS=:RO_STATUS_ADD ")
                    .AppendLine("    AND EXISTS( ")
                    .AppendLine("        SELECT ")
                    .AppendLine("           1 ")
                    .AppendLine("        FROM ")
                    .AppendLine("           TB_T_JOB_DTL T7 ")
                    .AppendLine("        WHERE ")
                    .AppendLine("           T7.SVCIN_ID=T1.SVCIN_ID ")
                    .AppendLine("           AND T7.CANCEL_FLG=:CANCEL_FLG ")
                    .AppendLine("           ) ")
                End With
                query.CommandText = sql.ToString()
                ' バインド変数定義
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, inNowDate)                                '現在日時
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)                                '最大日時
                query.AddParameterWithTypeValue("RO_STATUS_ADD", OracleDbType.NVarchar2, C_RO_STATUS_WAITFMAPP)                     ' ROステータス(「20」FM承認待ち)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' チップに紐付いているRO情報取得
        ''' </summary>
        ''' <param name="dtSubChipInfo">サブチップ情報データテーブル</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetJobInstructInfo(ByVal dtSubChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable) _
                                                As SC3240301DataSet.SC3240301JobInstructInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
             "{0}_S.", _
             System.Reflection.MethodBase.GetCurrentMethod.Name))

            ' JobDtlId取得文字列
            Dim subSql As New StringBuilder
            Dim JobDtlIdName As String
            Dim count As Integer = 0
            subSql.Append(" ( T1.JOB_DTL_ID IN ( ")
            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3240301DataSet.SC3240301JobInstructInfoDataTable)("SC3240301_015")
                For Each drSubChipInfo As SC3240301DataSet.SC3240301SubChipInfoRow In dtSubChipInfo
                    If drSubChipInfo.IsJOB_DTL_IDNull Then
                        Continue For
                    End If
                    ' SQL作成
                    JobDtlIdName = String.Format(CultureInfo.CurrentCulture, "JOB_DTL_ID{0}", count)
                    If count >= 1000 AndAlso count Mod 1000 = 0 Then
                        subSql.AppendLine(" ) OR ( T1.JOB_DTL_ID IN ( ")
                        subSql.AppendLine(String.Format(CultureInfo.CurrentCulture, ":{0} ", JobDtlIdName))
                    ElseIf count = 0 Then
                        subSql.AppendLine(String.Format(CultureInfo.CurrentCulture, ":{0} ", JobDtlIdName))
                    Else
                        subSql.AppendLine(String.Format(CultureInfo.CurrentCulture, ", :{0} ", JobDtlIdName))
                    End If
                    ' パラメータ作成
                    query.AddParameterWithTypeValue(JobDtlIdName, OracleDbType.Decimal, drSubChipInfo.JOB_DTL_ID) '作業内容ID
                    count = count + 1
                Next
                If String.IsNullOrEmpty(subSql.ToString) Then
                    subSql.AppendLine(" 1<>1 ")
                End If

                subSql.Append(" ) )")
                Dim sql As New StringBuilder
                ' SQL文の作成
                With sql
                    .AppendLine(" SELECT ")
                    .AppendLine("  /* SC3240301_015 */ ")
                    .AppendLine("    T1.JOB_DTL_ID ")
                    .AppendLine("  , T1.RO_NUM ")
                    .AppendLine("  , T1.RO_SEQ AS RO_JOB_SEQ ")
                    .AppendLine(" FROM  ")
                    .AppendLine("      TB_T_JOB_INSTRUCT T1 ")
                    .AppendLine(" WHERE ")
                    .AppendLine(subSql.ToString)
                    .AppendLine("    AND T1.STARTWORK_INSTRUCT_FLG=:STARTWORK_INSTRUCT_FLG ")
                    .AppendLine(" ORDER BY ")
                    .AppendLine("    T1.JOB_DTL_ID ")
                End With
                query.CommandText = sql.ToString()

                'パラメタ作成
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, C_INSTRUCT_FLG)   ' 着手指示フラグ(「1」着工指示済)

                Dim dt As SC3240301DataSet.SC3240301JobInstructInfoDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E, COUNT={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))
                ' 検索結果の返却
                Return dt
            End Using

        End Function

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    End Class

End Namespace

