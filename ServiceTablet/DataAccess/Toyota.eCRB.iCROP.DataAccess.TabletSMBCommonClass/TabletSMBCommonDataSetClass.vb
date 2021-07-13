'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'TabletSMBCommonClassDataSet.vb
'─────────────────────────────────────
'機能： タブレットSMB共通関数のデータセット
'補足： 
'作成： 2013/06/05 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新： 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応
'更新： 2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
'更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新： 2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応
'更新： 2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）
'更新： 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新： 2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
'更新： 2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
'更新： 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/06/15 TMEJ 小澤 TR-SVT-TMT-20150612-001「納車済みのチップが突然、SAメイン画面に表示された」対応
'更新： 2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応
'更新： 2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応
'更新： 2016/11/24 NSK 竹中 TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet
'更新： 2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
'更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新： 2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない
'更新： 2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2018/11/26 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新： 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet


Namespace TabletSMBCommonClassDataSetTableAdapters
    Public Class TabletSMBCommonClassDataAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        ''' <summary>
        ''' 基本型式(ALL)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BaseTypeAll As String = "X"
        ''' <summary>
        ''' ハイフン
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Hyphen As String = "-"
        ''' <summary>
        ''' 商品マスタ　使用中フラグ(使用中)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InUse As String = "1"
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 数値 0
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Zero As Long = 0
        ''' <summary>
        ''' 数値 1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const One As Long = 1
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 作業ステータス：中断
        ''' </summary>
        ''' <remarks></remarks>
        Private Const JobStatusStop As String = "2"
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 権限コード：TC
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OPERATIONCODE_TC As Long = 14

        ''' <summary>
        ''' 権限コード：Cht
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OPERATIONCODE_CHT As Long = 62
        '2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
        ''' <summary>
        ''' キャンセルフラグ 　0:有効　
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_NOT_CANCEL = "0"
        '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
        ''' <summary>
        ''' 
        ''' 検査必要フラグ 　1:有効　　
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INSPECTION_NEED_FLG_1 = "1"

        ''' <summary>
        ''' 完成検査ステータス 　2:完成検査承認済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INSPECTION_STATUS_2 = "2"

        ''' <summary>
        ''' ROステータス   99：キャンセル
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_STATUS_99 = "99"
        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        ''' <summary>
        ''' ストール利用ステータス 　00:着工指示待ち　
        ''' </summary>
        ''' <remarks></remarks>
        Private Const STALL_USE_STATUS_WAIT As String = "00"
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"
        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        Private Const IconFlagOff = "0"
        '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

#End Region

#Region "特定情報取得"
        ''' <summary>
        ''' 店舗稼動時間情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        Public Function GetBranchOperatingHours(ByVal dealerCode As String, _
                                                ByVal branchCode As String) As TabletSmbCommonClassBranchOperatingHoursDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_001 */ ")
                .AppendLine("        SVC_JOB_START_TIME ")
                .AppendLine("      , SVC_JOB_END_TIME ")
                .AppendLine("   FROM TB_M_BRANCH_DETAIL ")
                .AppendLine("  WHERE DLR_CD = :DLR_CD ")
                .AppendLine("    AND BRN_CD = :BRN_CD ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassBranchOperatingHoursDataTable)("TABLETSMBCOMMONCLASS_001")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' 日跨ぎ終了チップのストール利用ID取得
        ''' </summary>
        ''' <param name="dlrCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <returns></returns>
        Public Function GetContainsMidfinishChip(ByVal dlrCode As String, ByVal brnCode As String, ByVal svcInId As Decimal) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcInId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_002 */ ")
                .AppendLine("        TSTAUSE.STALL_USE_ID COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("      , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("  WHERE ")
                .AppendLine("        TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("    AND TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("    AND TSRVIN.SVCIN_ID = :SVCIN_ID ")
                .AppendLine("    AND TSTAUSE.STALL_USE_STATUS = N'06' ")
                .AppendLine("    AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine("    AND TSTAUSE.BRN_CD = :BRN_CD ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_002")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' サービス入庫IDでストール利用ステータスを取得します
        ''' </summary>
        ''' <param name="dlrCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="serviceinId">サービス入庫ID</param>
        ''' <returns>ストール利用ステータスリスト</returns>
        ''' <remarks></remarks>
        Public Function GetlStallUseStatusListBySvcInId(ByVal dlrCode As String, ByVal brnCode As String, ByVal serviceinId As Decimal) As TabletSmbCommonClassStringValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. serviceinId={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, serviceinId))
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_003 */ ")
                .AppendLine("        TSTAUSE.STALL_USE_STATUS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("      , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("  WHERE ")
                .AppendLine("        TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("    AND TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("    AND TSRVIN.SVCIN_ID = :SVCIN_ID ")
                .AppendLine("    AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine("    AND TSTAUSE.BRN_CD = :BRN_CD ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_003")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, serviceinId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 削除されてない作業内容IDを取得する
        ''' </summary>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <returns>作業内容ID</returns>
        ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' Public Function GetCanceledJobDtlId(ByVal svcinId As Decimal) As TabletSmbCommonClassCanceledJobInfoDataTable
        Public Function GetNotCanceledJobDtlId(ByVal svcinId As Decimal) As TabletSmbCommonClassCanceledJobInfoDataTable
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_036 */ ")
                .AppendLine("        T1.JOB_DTL_ID  ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("       ,T1.RO_JOB_SEQ  ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("       ,T2.STALL_USE_ID    ")
                .AppendLine("       ,T2.STALL_ID    ")
                .AppendLine("       ,T2.SCHE_START_DATETIME    ")
                .AppendLine("       ,T2.SCHE_END_DATETIME    ")
                .AppendLine("   FROM  TB_T_JOB_DTL T1 ")
                .AppendLine("        ,TB_T_STALL_USE T2  ")
                .AppendLine("  WHERE T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("    AND T1.CANCEL_FLG = N'1' ")
                .AppendLine("    AND T1.CANCEL_FLG = N'0' ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("    AND T2.STALL_USE_STATUS = N'01' ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("    AND T1.RO_JOB_SEQ >= 0 ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("    AND T1.SVCIN_ID = :SVCIN_ID ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("    AND EXISTS ")
                .AppendLine("           ( ")
                .AppendLine("              SELECT  ")
                .AppendLine("                   S1.JOB_DTL_ID  ")
                .AppendLine("               FROM  ")
                .AppendLine("                   TB_T_JOB_INSTRUCT S1  ")
                .AppendLine("               WHERE  ")
                .AppendLine("                    S1.JOB_DTL_ID = T1.JOB_DTL_ID  ")
                .AppendLine("                AND S1.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_ON  ")
                .AppendLine("           ) ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassCanceledJobInfoDataTable)("TABLETSMBCOMMONCLASS_036")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' 削除済みの作業内容IDを取得する
        ''' </summary>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <returns></returns>
        Public Function GetCanceledJobDtlIdList(ByVal svcInId As Decimal) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. svcInId={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      svcInId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_041 */ ")
                .AppendLine("        JOB_DTL_ID COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_DTL ")
                .AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                .AppendLine("    AND CANCEL_FLG = N'1' ")
            End With

            Dim getTable As TabletSmbCommonClassNumberValueDataTable

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_041")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)

                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                        "{0}_E RowCount={1}", _
                        System.Reflection.MethodBase.GetCurrentMethod.Name, _
                        getTable.Rows.Count))

            Return getTable

        End Function

        '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
        ''' <summary>
        ''' 指定サービス入庫IDに紐づく最大のストール利用IDを取得する
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>最大のストール利用IDデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetMaxStallUseIdGroupByServiceId(ByVal inServiceInId As Decimal, _
                                                         ByVal inDealerCode As String, _
                                                         ByVal inBranchCode As String) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_Start. inServiceInId={1}, inDealerCode={2}, inBranchCode={3}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inServiceInId, _
                                      inDealerCode, _
                                      inBranchCode))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_072 */ ")
                .AppendLine("        MAX(TSTAUSE.STALL_USE_ID) AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN  ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL   ")
                .AppendLine("      , TB_T_STALL_USE TSTAUSE   ")
                .AppendLine("  WHERE  ")
                .AppendLine("        TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID  ")
                .AppendLine("    AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID   ")
                .AppendLine("    AND TJOBDTL.CANCEL_FLG = :CANCEL_FLG    ")
                .AppendLine("    AND TSRVIN.SVCIN_ID = :SVCIN_ID   ")
                .AppendLine("    AND TSTAUSE.DLR_CD = :DLR_CD    ")
                .AppendLine("    AND TSTAUSE.BRN_CD = :BRN_CD   ")
            End With

            Dim dtStallUseId As TabletSmbCommonClassNumberValueDataTable

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_072")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, C_NOT_CANCEL) '0:有効　

                dtStallUseId = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_End. RowCount={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dtStallUseId.Rows.Count))

            Return dtStallUseId

        End Function
        '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

#End Region

#Region "各エンティティ取得"
        ''' <summary>
        ''' チップエンティティを取得する
        ''' </summary>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <param name="nType">取得タイプ:1洗車時間、検査時間を含めて取得</param>
        ''' <returns></returns>
        Public Function GetChipEntity(ByVal stallUseId As Decimal, ByVal nType As Short) As TabletSmbCommonClassChipEntityDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. STALL_USE_ID={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, stallUseId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_004 */ ")
                .AppendLine("        TSRVIN.SVCIN_ID ")
                .AppendLine("      , TSRVIN.CST_ID ")
                .AppendLine("      , TSRVIN.DLR_CD ")
                .AppendLine("      , TSRVIN.BRN_CD ")
                .AppendLine("      , TSRVIN.SVC_STATUS ")
                .AppendLine("      , TSRVIN.RESV_STATUS ")
                .AppendLine("      , TSRVIN.CARWASH_NEED_FLG ")
                .AppendLine("      , TSRVIN.PICK_DELI_TYPE ")
                .AppendLine("      , TSRVIN.ACCEPTANCE_TYPE ")
                .AppendLine("      , TSRVIN.RO_NUM ")
                .AppendLine("      , TSRVIN.SCHE_SVCIN_DATETIME ")
                .AppendLine("      , TSRVIN.SCHE_DELI_DATETIME ")
                .AppendLine("      , TSRVIN.ROW_LOCK_VERSION ")
                .AppendLine("      , TJOBDTL.JOB_DTL_ID ")
                .AppendLine("      , TJOBDTL.INSPECTION_NEED_FLG ")
                .AppendLine("      , TJOBDTL.SVC_CLASS_ID ")
                .AppendLine("      , TJOBDTL.MERC_ID ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("      , TJOBDTL.RO_JOB_SEQ ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("      , TJOBDTL.INSPECTION_STATUS ")
                .AppendLine("      , TSTAUSE.STALL_USE_ID ")
                .AppendLine("      , TSTAUSE.JOB_ID ")
                .AppendLine("      , TSTAUSE.STALL_USE_STATUS ")
                .AppendLine("      , TSTAUSE.TEMP_FLG ")
                .AppendLine("      , TSTAUSE.REST_FLG ")
                .AppendLine("      , TSTAUSE.PARTS_FLG ")
                .AppendLine("      , TSTAUSE.STALL_ID ")
                .AppendLine("      , TSTAUSE.SCHE_START_DATETIME ")
                .AppendLine("      , TSTAUSE.SCHE_END_DATETIME ")
                .AppendLine("      , TSTAUSE.SCHE_WORKTIME ")
                .AppendLine("      , TSTAUSE.RSLT_START_DATETIME ")
                .AppendLine("      , TSTAUSE.RSLT_END_DATETIME ")
                .AppendLine("      , TSTAUSE.RSLT_WORKTIME ")
                .AppendLine("      , TSTAUSE.PRMS_END_DATETIME ")
                .AppendLine("      , TSTAUSE.STALL_IDLE_ID ")
                .AppendLine("      , TSTAUSE.STOP_REASON_TYPE ")
                .AppendLine("      , TSTAUSE.STOP_MEMO ")
                .AppendLine("      , TSTAUSE.UPDATE_DATETIME ")
                .AppendLine("      , TSTAUSE.UPDATE_STF_CD ")
                'GetChipEntityが頻繁でコールされた、それで、できるだけ少ないテーブルを検索する
                'タイプ1の場合、洗車、検査実績テーブルとjoinする
                If nType = 1 Then
                    .AppendLine("      , TCWRES.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                    .AppendLine("      , TCWRES.RSLT_END_DATETIME   AS CW_RSLT_END_DATETIME ")
                    .AppendLine("      , TINSRES.RSLT_START_DATETIME AS IS_RSLT_START_DATETIME ")
                    .AppendLine("      , TINSRES.RSLT_END_DATETIME   AS IS_RSLT_END_DATETIME ")
                End If
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("      , TB_T_STALL_USE TSTAUSE ")
                If nType = 1 Then
                    .AppendLine("      , TB_T_CARWASH_RESULT TCWRES ")
                    .AppendLine("      , TB_T_INSPECTION_RESULT TINSRES ")
                End If
                .AppendLine("  WHERE ")
                .AppendLine("        TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("    AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                If nType = 1 Then
                    .AppendLine("    AND TSRVIN.SVCIN_ID = TCWRES.SVCIN_ID(+) ")
                    .AppendLine("    AND TSTAUSE.JOB_DTL_ID = TINSRES.JOB_DTL_ID(+) ")
                End If
                .AppendLine("    AND TSTAUSE.STALL_USE_ID = :STALL_USE_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassChipEntityDataTable)("TABLETSMBCOMMONCLASS_004")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' 遅れ見込み情報を取得する
        ''' </summary>
        ''' <param name="svcinIdList">サービス入庫ID</param>
        ''' <returns></returns>
        Public Function GetDeliDelayInfo(ByVal svcinIdList As List(Of Decimal)) As TabletSmbCommonClassDeliDelayDateDataTable

            'サービス入庫IDがない場合、空白テーブルを戻す
            If IsNothing(svcinIdList) OrElse svcinIdList.Count = 0 Then
                Return New TabletSmbCommonClassDeliDelayDateDataTable
            End If

            'サービス入庫IDを「svcinid1,svcinid2,…svcinidN」のstringに変更する
            Dim sbSvcinId As New StringBuilder
            For Each svcinId As String In svcinIdList
                sbSvcinId.Append(svcinId)
                sbSvcinId.Append(",")
            Next

            Dim strSvcinIdList As String = sbSvcinId.ToString()
            '最後のコンマを削除する
            strSvcinIdList = strSvcinIdList.Substring(0, strSvcinIdList.Length - 1)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SVCIN_ID={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, strSvcinIdList))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_005 */ ")
                .AppendLine("        T1.SVCIN_ID ")
                .AppendLine("      , T1.SVC_STATUS ")
                .AppendLine("      , T1.RO_NUM ")
                .AppendLine("      , T1.CARWASH_NEED_FLG ")
                .AppendLine("      , T1.SCHE_DELI_DATETIME ")
                .AppendLine("      , T2.RSLT_START_DATETIME AS CARWASH_START_DATETIME ")
                .AppendLine("      , T2.RSLT_END_DATETIME   AS CARWASH_END_DATETIME ")
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("      , CASE  ")
                .AppendLine("            WHEN T1.INVOICE_PREP_COMPL_DATETIME = :MINDATE THEN :MINVALUE ")
                .AppendLine("            ELSE T1.INVOICE_PREP_COMPL_DATETIME  ")
                .AppendLine("        END AS INVOICE_PRINT_DATETIME ")
                .AppendLine("      , T3.REMAINING_INSPECTION_TYPE")
                .AppendLine("      , T4.MAX_RO_STATUS AS MAX_RO_STATUS")
                .AppendLine("      , T4.MIN_RO_STATUS AS MIN_RO_STATUS")
                .AppendLine("      , CASE  ")
                .AppendLine("            WHEN T3.MAX_INSPECTION_DATETIME = :MINDATE THEN :MINVALUE ")
                .AppendLine("            ELSE T3.MAX_INSPECTION_DATETIME ")
                .AppendLine("        END AS MAX_INSPECTION_DATETIME ")
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN T1 ")
                .AppendLine("      , TB_T_CARWASH_RESULT T2 ")
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("      , ( ")
                .AppendLine("          SELECT ")
                .AppendLine("                 M1.SVCIN_ID ")
                .AppendLine("                ,MIN(DECODE(M1.INSPECTION_NEED_FLG, 1, M1.INSPECTION_STATUS, 2)) AS REMAINING_INSPECTION_TYPE ")
                .AppendLine("                ,MAX(M1.INSPECTION_APPROVAL_DATETIME) AS MAX_INSPECTION_DATETIME ")
                .AppendLine("            FROM ")
                .AppendLine("                 TB_T_JOB_DTL M1")
                .AppendLine("           WHERE M1.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("        GROUP BY M1.SVCIN_ID ")
                .AppendLine("        ) T3")
                .AppendLine("      , ( ")
                .AppendLine("          SELECT ")
                .AppendLine("                 U1.SVCIN_ID")
                .AppendLine("               , MAX(U1.RO_STATUS) AS MAX_RO_STATUS ")
                .AppendLine("               , MIN(U1.RO_STATUS) AS MIN_RO_STATUS ")
                .AppendLine("            FROM ")
                .AppendLine("                 TB_T_RO_INFO U1")
                .AppendLine("           WHERE U1.RO_STATUS <> :RO_STATUS_99 ")
                .AppendLine("        GROUP BY U1.SVCIN_ID")
                .AppendLine("        ) T4")
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("  WHERE ")
                .AppendLine("        T1.SVCIN_ID = T2.SVCIN_ID(+) ")
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("    AND T1.SVCIN_ID = T3.SVCIN_ID(+) ")
                .AppendLine("    AND T1.SVCIN_ID = T4.SVCIN_ID(+)")
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("    AND T1.SVCIN_ID IN ( ")
                .AppendLine(strSvcinIdList)
                .AppendLine("                       ) ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassDeliDelayDateDataTable)("TABLETSMBCOMMONCLASS_005")
                query.CommandText = sql.ToString()
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("MINVALUE", OracleDbType.Date, Date.MinValue)
                query.AddParameterWithTypeValue("RO_STATUS_99", OracleDbType.NVarchar2, RO_STATUS_99)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, C_NOT_CANCEL)
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

#Region "チップ情報取得"
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>
        ' ''' 当日のストールチップ情報を取得する
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="stallStartTime">稼働時間From</param>
        ' ''' <param name="stallEndTime">稼働時間To</param>
        ' ''' <param name="stallIdList">ストールIDリスト</param>
        ' ''' <param name="theTime">この日時後変更があったチップを取得</param>
        ' ''' <returns></returns>
        'Public Function GetAllStallChip(ByVal dealerCode As String, _
        '                                ByVal branchCode As String, _
        '                                ByVal stallStartTime As Date, _
        '                                ByVal stallEndTime As Date, _
        '                                Optional ByVal stallIdList As List(Of Decimal) = Nothing, _
        '                                Optional ByVal theTime As Date = Nothing) As TabletSmbCommonClassStallChipInfoDataTable

        ''' <summary>
        ''' 当日のストールチップ情報を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallStartTime">稼働時間From</param>
        ''' <param name="stallEndTime">稼働時間To</param>
        ''' <param name="stallIdList">ストールIDリスト</param>
        ''' <param name="theTime">この日時後変更があったチップを取得</param>
        ''' <param name="svcinIdUpdatedWithRoinfo">RO情報テーブルに変更されたサービス入庫ID</param>
        ''' <returns></returns>
        Public Function GetAllStallChip(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal stallStartTime As Date, _
                                        ByVal stallEndTime As Date, _
                                        Optional ByVal stallIdList As List(Of Decimal) = Nothing, _
                                        Optional ByVal theTime As Date = Nothing, _
                                        Optional ByVal svcinIdUpdatedWithRoinfo As String = "") As TabletSmbCommonClassStallChipInfoDataTable
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Dim bIsStallId = True
            'ストールIDを「stallid1,stallid2,…stallidN」のstringに変更する
            Dim sbStallist As New StringBuilder
            Dim strStallList As String = ""
            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(stallIdList) OrElse stallIdList.Count = 0 Then
                bIsStallId = False
            Else
                For Each stallId As String In stallIdList
                    sbStallist.Append(stallId)
                    sbStallist.Append(",")
                Next
                strStallList = sbStallist.ToString()
                '最後のコンマを削除する
                strStallList = strStallList.Substring(0, strStallList.Length - 1)
            End If

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2},stallIdList={3}, stallStartTime={4}, stallEndTime={5}" _
            '              , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strStallList, stallStartTime, stallEndTime))
            If theTime <> CDate(Nothing) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2},stallIdList={3}, stallStartTime={4}, stallEndTime={5}, theTime={6}, svcinIdUpdatedWithRoinfo={7}" _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strStallList, stallStartTime, stallEndTime, theTime, svcinIdUpdatedWithRoinfo))

            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2},stallIdList={3}, stallStartTime={4}, stallEndTime={5}" _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strStallList, stallStartTime, stallEndTime))
            End If
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_006 */ ")
                .AppendLine("        SVCIN_ID ")
                .AppendLine("      , DLR_CD ")
                .AppendLine("      , BRN_CD ")
                .AppendLine("      , CST_ID ")
                .AppendLine("      , VCL_ID ")
                .AppendLine("      , CST_VCL_TYPE ")
                .AppendLine("      , TLM_CONTRACT_FLG ")
                .AppendLine("      , ACCEPTANCE_TYPE  ")
                .AppendLine("      , PICK_DELI_TYPE ")
                .AppendLine("      , PARTS_FLG ")
                .AppendLine("      , CARWASH_NEED_FLG ")
                .AppendLine("      , RESV_STATUS ")
                .AppendLine("      , SVC_STATUS ")
                .AppendLine("      , SCHE_SVCIN_DATETIME ")
                .AppendLine("      , SCHE_DELI_DATETIME ")
                .AppendLine("      , RSLT_SVCIN_DATETIME ")
                .AppendLine("      , RSLT_DELI_DATETIME ")
                .AppendLine("      , ROW_UPDATE_DATETIME ")
                .AppendLine("      , ROW_LOCK_VERSION ")
                .AppendLine("      , RO_NUM ")
                .AppendLine("      , JOB_DTL_ID ")
                .AppendLine("      , INSPECTION_NEED_FLG ")
                .AppendLine("      , INSPECTION_STATUS ")
                .AppendLine("      , CANCEL_FLG ")
                .AppendLine("      , STALL_USE_ID ")
                .AppendLine("      , STALL_ID ")
                .AppendLine("      , TEMP_FLG ")
                .AppendLine("      , STALL_USE_STATUS ")
                .AppendLine("      , SCHE_START_DATETIME ")
                .AppendLine("      , SCHE_END_DATETIME ")
                .AppendLine("      , SCHE_WORKTIME ")
                .AppendLine("      , REST_FLG ")
                .AppendLine("      , RSLT_START_DATETIME ")
                .AppendLine("      , PRMS_END_DATETIME ")
                .AppendLine("      , RSLT_END_DATETIME ")
                .AppendLine("      , RSLT_WORKTIME ")
                .AppendLine("      , STOP_REASON_TYPE ")
                .AppendLine("      , VCL_VIN ")
                .AppendLine("      , MODEL_NAME ")
                .AppendLine("      , REG_NUM ")
                .AppendLine("      , CARWASH_RSLT_ID ")
                .AppendLine("      , CW_RSLT_START_DATETIME ")
                .AppendLine("      , CW_RSLT_END_DATETIME ")
                .AppendLine("      , SVC_CLASS_NAME ")
                .AppendLine("      , SVC_CLASS_NAME_ENG ")
                .AppendLine("      , UPPER_DISP ")
                .AppendLine("      , LOWER_DISP ")
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("      , IMP_VCL_FLG")
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("      , RO_JOB_SEQ ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("   FROM ( ")
                .AppendLine("          SELECT ")
                .AppendLine("                 TSRVIN.SVCIN_ID ")
                .AppendLine("               , TSRVIN.DLR_CD ")
                .AppendLine("               , TSRVIN.BRN_CD ")
                .AppendLine("               , TSRVIN.CST_ID ")
                .AppendLine("               , TSRVIN.VCL_ID ")
                .AppendLine("               , TSRVIN.CST_VCL_TYPE ")
                .AppendLine("               , TSRVIN.TLM_CONTRACT_FLG ")
                .AppendLine("               , TSRVIN.ACCEPTANCE_TYPE ")
                .AppendLine("               , TSRVIN.PICK_DELI_TYPE ")
                .AppendLine("               , TSRVIN.CARWASH_NEED_FLG ")
                .AppendLine("               , TSRVIN.RESV_STATUS ")
                .AppendLine("               , TSRVIN.SVC_STATUS ")
                .AppendLine("               , TSRVIN.SCHE_SVCIN_DATETIME ")
                .AppendLine("               , TSRVIN.SCHE_DELI_DATETIME ")
                .AppendLine("               , TSRVIN.RSLT_SVCIN_DATETIME ")
                .AppendLine("               , TSRVIN.RSLT_DELI_DATETIME ")
                .AppendLine("               , TSRVIN.ROW_UPDATE_DATETIME ")
                .AppendLine("               , TSRVIN.ROW_LOCK_VERSION ")
                .AppendLine("               , TSRVIN.RO_NUM ")
                .AppendLine("               , TJOBDTL.JOB_DTL_ID ")
                .AppendLine("               , TJOBDTL.INSPECTION_NEED_FLG ")
                .AppendLine("               , TJOBDTL.INSPECTION_STATUS ")
                .AppendLine("               , TJOBDTL.CANCEL_FLG ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("               , TJOBDTL.RO_JOB_SEQ ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("               , TSTAUSE.STALL_USE_ID ")
                .AppendLine("               , TSTAUSE.STALL_ID ")
                .AppendLine("               , TSTAUSE.TEMP_FLG ")
                .AppendLine("               , TSTAUSE.PARTS_FLG ")
                .AppendLine("               , TSTAUSE.STALL_USE_STATUS ")
                .AppendLine("               , TSTAUSE.SCHE_START_DATETIME ")
                .AppendLine("               , TSTAUSE.SCHE_END_DATETIME ")
                .AppendLine("               , TSTAUSE.SCHE_WORKTIME ")
                .AppendLine("               , TSTAUSE.REST_FLG ")
                .AppendLine("               , TSTAUSE.RSLT_START_DATETIME ")
                .AppendLine("               , TSTAUSE.PRMS_END_DATETIME ")
                .AppendLine("               , TSTAUSE.RSLT_END_DATETIME ")
                .AppendLine("               , TSTAUSE.RSLT_WORKTIME ")
                .AppendLine("               , TSTAUSE.STOP_REASON_TYPE ")
                .AppendLine("               , MVCL.VCL_VIN ")
                .AppendLine("               , MMOD.MODEL_NAME ")
                .AppendLine("               , MVCLDLR.REG_NUM ")
                .AppendLine("               , TCWRES.CARWASH_RSLT_ID ")
                .AppendLine("               , TCWRES.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("               , TCWRES.RSLT_END_DATETIME   AS CW_RSLT_END_DATETIME ")
                .AppendLine("               , MSRVCLS.SVC_CLASS_NAME ")
                .AppendLine("               , MSRVCLS.SVC_CLASS_NAME_ENG ")
                .AppendLine("               , MMERC.UPPER_DISP ")
                .AppendLine("               , MMERC.LOWER_DISP ")
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("               , NVL(TRIM(MVCLDLR.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                .AppendLine("            FROM ")
                .AppendLine("                 TB_T_SERVICEIN TSRVIN ")
                .AppendLine("               , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("               , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("               , TB_M_VEHICLE MVCL ")
                .AppendLine("               , TB_M_MODEL MMOD ")
                .AppendLine("               , TB_M_VEHICLE_DLR MVCLDLR ")
                .AppendLine("               , TB_T_CARWASH_RESULT TCWRES ")
                .AppendLine("               , TB_M_SERVICE_CLASS MSRVCLS ")
                .AppendLine("               , TB_M_MERCHANDISE MMERC ")
                .AppendLine("           WHERE TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("             AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                '2016/11/17 NSK 竹中  TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet 性能対応 START
                .AppendLine("             AND TSRVIN.VCL_ID = MVCL.VCL_ID ")
                '2016/11/17 NSK 竹中  TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet 性能対応 END
                .AppendLine("             AND MVCL.MODEL_CD = MMOD.MODEL_CD(+) ")
                '2016/11/17 NSK 竹中  TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet 性能対応 START
                .AppendLine("             AND TSRVIN.VCL_ID = MVCLDLR.VCL_ID ")
                .AppendLine("             AND TSRVIN.DLR_CD = MVCLDLR.DLR_CD ")
                '2016/11/17 NSK 竹中  TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet 性能対応 END
                .AppendLine("             AND TSRVIN.SVCIN_ID = TCWRES.SVCIN_ID(+) ")
                .AppendLine("             AND TJOBDTL.SVC_CLASS_ID = MSRVCLS.SVC_CLASS_ID(+) ")
                .AppendLine("             AND TJOBDTL.MERC_ID = MMERC.MERC_ID(+) ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("             AND TSRVIN.SVC_STATUS <> N'02' ")
                '.AppendLine("             AND TJOBDTL.CANCEL_FLG = N'0' ")
                If theTime = CDate(Nothing) Then
                    .AppendLine("             AND TSRVIN.SVC_STATUS <> N'02' ")
                    .AppendLine("             AND TJOBDTL.CANCEL_FLG = N'0' ")
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("             AND TSRVIN.DLR_CD = :DLRCD ")
                .AppendLine("             AND TSRVIN.BRN_CD = :STRCD ")
                'theTimeがNothing以外の場合
                If theTime <> CDate(Nothing) Then
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    '.AppendLine("             AND TSRVIN.ROW_UPDATE_DATETIME >= :THETIME ")
                    '差分更新の間にROステータス変更があるサービスがあれば
                    If Not String.IsNullOrEmpty(svcinIdUpdatedWithRoinfo) Then
                        .AppendLine("    AND ( ")
                        .AppendLine("               TSRVIN.ROW_UPDATE_DATETIME >= :THETIME ")
                        .AppendLine("           OR  TSRVIN.SVCIN_ID IN ( ")
                        .AppendLine(svcinIdUpdatedWithRoinfo)
                        .AppendLine("                                   ) ")
                        '2016/11/17 NSK 竹中  TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet START
                        '.AppendLine("        ) ")
                        '2016/11/17 NSK 竹中 TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet END
                    Else
                        '2016/11/17 NSK 竹中  TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet START
                        '.AppendLine("             AND TSRVIN.ROW_UPDATE_DATETIME >= :THETIME ")
                        .AppendLine("             AND ( ")
                        .AppendLine("                     TSRVIN.ROW_UPDATE_DATETIME >= :THETIME")
                        '2016/11/17 NSK 竹中 TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet END
                    End If
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    '2016/11/17 NSK 竹中  TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet START
                    .AppendLine("             OR MVCLDLR.ROW_UPDATE_DATETIME >= :THETIME ")
                    .AppendLine("        ) ")
                    '2016/11/17 NSK 竹中 TR-SVT-TMT-20160301-001 Reg no not update from i-CROP to Tablet END
                End If
                .AppendLine("             AND EXISTS ( ")
                .AppendLine("                            SELECT TJOBDTL.SVCIN_ID ")
                .AppendLine("                              FROM TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("                                 , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("                             WHERE TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("                               AND TJOBDTL.DLR_CD = :DLRCD ")
                .AppendLine("                               AND TJOBDTL.BRN_CD = :STRCD ")
                .AppendLine("                               AND TSTAUSE.DLR_CD = :DLRCD ")
                .AppendLine("                               AND TSTAUSE.BRN_CD = :STRCD ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("                               AND TJOBDTL.CANCEL_FLG = N'0' ")
                If theTime = CDate(Nothing) Then
                    .AppendLine("                               AND TJOBDTL.CANCEL_FLG = N'0' ")
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                'ストールがあれば
                If bIsStallId Then
                    .AppendLine("                               AND TSTAUSE.STALL_ID IN ( ")
                    .AppendLine(strStallList)
                    .AppendLine("                                                       ) ")
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                If theTime = CDate(Nothing) Then
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    .AppendLine("                          AND ( ")
                    .AppendLine("							       ( ")
                    .AppendLine("                                          TSTAUSE.SCHE_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                    .AppendLine("                                      AND ( ")
                    .AppendLine("	                                          ( ")
                    .AppendLine("                                                   TSTAUSE.RSLT_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                    .AppendLine("                                               AND TSTAUSE.RSLT_START_DATETIME < TO_DATE(:ENDDATE,'YYYYMMDDHH24MISS') ")
                    .AppendLine("                                               AND ( ")
                    .AppendLine("                                                       ( ")
                    .AppendLine("                                                            TSTAUSE.RSLT_END_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                    .AppendLine("                                                        AND TSTAUSE.PRMS_END_DATETIME > TO_DATE(:STARTDATE,'YYYYMMDDHH24MISS') ")
                    .AppendLine("                                                       ) ")
                    .AppendLine("                                                    OR ( ")
                    .AppendLine("                                                            TSTAUSE.RSLT_END_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                    .AppendLine("                                                        AND TSTAUSE.RSLT_END_DATETIME > TO_DATE(:STARTDATE,'YYYYMMDDHH24MISS') ")
                    .AppendLine("                                                       ) ")
                    .AppendLine("                                                   ) ")
                    .AppendLine("                                              ) ")
                    .AppendLine("                                           OR ( ")
                    .AppendLine("                                                   TSTAUSE.RSLT_START_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                    .AppendLine("                                               AND TSTAUSE.SCHE_START_DATETIME < TO_DATE(:ENDDATE,'YYYYMMDDHH24MISS') ")
                    .AppendLine("                                               AND TSTAUSE.SCHE_END_DATETIME > TO_DATE(:STARTDATE,'YYYYMMDDHH24MISS') ")
                    .AppendLine("                                              ) ")
                    .AppendLine("                                          ) ")
                    .AppendLine("                                   ) ")
                    .AppendLine("                                OR TSTAUSE.SCHE_START_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                    .AppendLine("                              ) ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("                          AND TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("                        ) ")
                .AppendLine("         ) ETB3CHIPINFO ")
                .AppendLine(" ORDER BY ")
                .AppendLine("          SVCIN_ID ")
                .AppendLine("        , JOB_DTL_ID ")
                .AppendLine("        , STALL_USE_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStallChipInfoDataTable)("TABLETSMBCOMMONCLASS_006")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'query.AddParameterWithTypeValue("STARTDATE", OracleDbType.NVarchar2, Format(stallStartTime, "yyyyMMddHHmmss"))
                'query.AddParameterWithTypeValue("ENDDATE", OracleDbType.NVarchar2, Format(stallEndTime, "yyyyMMddHHmmss"))
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                'theTimeがNothing以外の場合
                If theTime <> CDate(Nothing) Then
                    query.AddParameterWithTypeValue("THETIME", OracleDbType.Date, theTime)
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                Else
                    'query.AddParameterWithTypeValue("STARTDATE", OracleDbType.NVarchar2, Format(stallStartTime, "yyyyMMddHHmmss"))
                    'query.AddParameterWithTypeValue("ENDDATE", OracleDbType.NVarchar2, Format(stallEndTime, "yyyyMMddHHmmss"))
                    query.AddParameterWithTypeValue("STARTDATE", OracleDbType.NVarchar2, stallStartTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                    query.AddParameterWithTypeValue("ENDDATE", OracleDbType.NVarchar2, stallEndTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>
        ' ''' サービス入庫IDにより、関連チップを全て取得する
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="svcinIdList">サービス入庫IDリスト</param>
        ' ''' <returns>関連チップ情報</returns>
        'Public Function GetStallChipBySvcinId(ByVal dealerCode As String, _
        '                                      ByVal branchCode As String, _
        '                                      ByVal svcinIdList As List(Of Decimal)) As TabletSmbCommonClassStallChipInfoDataTable

        ''' <summary>
        ''' サービス入庫IDにより、関連チップを全て取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="svcinIdList">サービス入庫IDリスト</param>
        ''' <param name="cancelChipFlg">キャンセルチップフラグ True:キャンセルしたチップがいる</param>
        ''' <returns>関連チップ情報</returns>
        Public Function GetStallChipBySvcinId(ByVal dealerCode As String, _
                                              ByVal branchCode As String, _
                                              ByVal svcinIdList As List(Of Decimal), _
                                              Optional ByVal cancelChipFlg As Boolean = False) As TabletSmbCommonClassStallChipInfoDataTable
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            'サービス入庫IDを「svcinid1,svcinid2,…svcinidN」のstringに変更する
            Dim sbSvcinIdList As New StringBuilder
            Dim strSvcinIdList As String = ""
            'サービス入庫IDがない場合、空白テーブルを戻す
            If IsNothing(svcinIdList) OrElse svcinIdList.Count = 0 Then
                Return New TabletSmbCommonClassStallChipInfoDataTable
            Else
                For Each stallId As String In svcinIdList
                    sbSvcinIdList.Append(stallId)
                    sbSvcinIdList.Append(",")
                Next
                strSvcinIdList = sbSvcinIdList.ToString()
                '最後のコンマを削除する
                strSvcinIdList = strSvcinIdList.Substring(0, strSvcinIdList.Length - 1)
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, svcinIdList={3}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strSvcinIdList))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_037 */ ")
                .AppendLine("        SVCIN_ID ")
                .AppendLine("      , DLR_CD ")
                .AppendLine("      , BRN_CD ")
                .AppendLine("      , CST_ID ")
                .AppendLine("      , VCL_ID ")
                .AppendLine("      , CST_VCL_TYPE ")
                .AppendLine("      , TLM_CONTRACT_FLG ")
                .AppendLine("      , ACCEPTANCE_TYPE  ")
                .AppendLine("      , PICK_DELI_TYPE ")
                .AppendLine("      , PARTS_FLG ")
                .AppendLine("      , CARWASH_NEED_FLG ")
                .AppendLine("      , RESV_STATUS ")
                .AppendLine("      , SVC_STATUS ")
                .AppendLine("      , SCHE_SVCIN_DATETIME ")
                .AppendLine("      , SCHE_DELI_DATETIME ")
                .AppendLine("      , RSLT_SVCIN_DATETIME ")
                .AppendLine("      , RSLT_DELI_DATETIME ")
                .AppendLine("      , ROW_UPDATE_DATETIME ")
                .AppendLine("      , ROW_LOCK_VERSION ")
                .AppendLine("      , RO_NUM ")
                .AppendLine("      , JOB_DTL_ID ")
                .AppendLine("      , INSPECTION_NEED_FLG ")
                .AppendLine("      , INSPECTION_STATUS ")
                .AppendLine("      , CANCEL_FLG ")
                .AppendLine("      , STALL_USE_ID ")
                .AppendLine("      , STALL_ID ")
                .AppendLine("      , TEMP_FLG ")
                .AppendLine("      , STALL_USE_STATUS ")
                .AppendLine("      , SCHE_START_DATETIME ")
                .AppendLine("      , SCHE_END_DATETIME ")
                .AppendLine("      , SCHE_WORKTIME ")
                .AppendLine("      , REST_FLG ")
                .AppendLine("      , RSLT_START_DATETIME ")
                .AppendLine("      , PRMS_END_DATETIME ")
                .AppendLine("      , RSLT_END_DATETIME ")
                .AppendLine("      , RSLT_WORKTIME ")
                .AppendLine("      , STOP_REASON_TYPE ")
                .AppendLine("      , VCL_VIN ")
                .AppendLine("      , MODEL_NAME ")
                .AppendLine("      , REG_NUM ")
                .AppendLine("      , CARWASH_RSLT_ID ")
                .AppendLine("      , CW_RSLT_START_DATETIME ")
                .AppendLine("      , CW_RSLT_END_DATETIME ")
                .AppendLine("      , SVC_CLASS_NAME ")
                .AppendLine("      , SVC_CLASS_NAME_ENG ")
                .AppendLine("      , UPPER_DISP ")
                .AppendLine("      , LOWER_DISP ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("      , RO_JOB_SEQ ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("   FROM ( ")
                .AppendLine("          SELECT ")
                .AppendLine("                 TSRVIN.SVCIN_ID ")
                .AppendLine("               , TSRVIN.DLR_CD ")
                .AppendLine("               , TSRVIN.BRN_CD ")
                .AppendLine("               , TSRVIN.CST_ID ")
                .AppendLine("               , TSRVIN.VCL_ID ")
                .AppendLine("               , TSRVIN.CST_VCL_TYPE ")
                .AppendLine("               , TSRVIN.TLM_CONTRACT_FLG ")
                .AppendLine("               , TSRVIN.ACCEPTANCE_TYPE ")
                .AppendLine("               , TSRVIN.PICK_DELI_TYPE ")
                .AppendLine("               , TSRVIN.CARWASH_NEED_FLG ")
                .AppendLine("               , TSRVIN.RESV_STATUS ")
                .AppendLine("               , TSRVIN.SVC_STATUS ")
                .AppendLine("               , TSRVIN.SCHE_SVCIN_DATETIME ")
                .AppendLine("               , TSRVIN.SCHE_DELI_DATETIME ")
                .AppendLine("               , TSRVIN.RSLT_SVCIN_DATETIME ")
                .AppendLine("               , TSRVIN.RSLT_DELI_DATETIME ")
                .AppendLine("               , TSRVIN.ROW_UPDATE_DATETIME ")
                .AppendLine("               , TSRVIN.ROW_LOCK_VERSION ")
                .AppendLine("               , TSRVIN.RO_NUM ")
                .AppendLine("               , TJOBDTL.JOB_DTL_ID ")
                .AppendLine("               , TJOBDTL.INSPECTION_NEED_FLG ")
                .AppendLine("               , TJOBDTL.INSPECTION_STATUS ")
                .AppendLine("               , TJOBDTL.CANCEL_FLG ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("               , TJOBDTL.RO_JOB_SEQ ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("               , TSTAUSE.STALL_USE_ID ")
                .AppendLine("               , TSTAUSE.STALL_ID ")
                .AppendLine("               , TSTAUSE.TEMP_FLG ")
                .AppendLine("               , TSTAUSE.PARTS_FLG ")
                .AppendLine("               , TSTAUSE.STALL_USE_STATUS ")
                .AppendLine("               , TSTAUSE.SCHE_START_DATETIME ")
                .AppendLine("               , TSTAUSE.SCHE_END_DATETIME ")
                .AppendLine("               , TSTAUSE.SCHE_WORKTIME ")
                .AppendLine("               , TSTAUSE.REST_FLG ")
                .AppendLine("               , TSTAUSE.RSLT_START_DATETIME ")
                .AppendLine("               , TSTAUSE.PRMS_END_DATETIME ")
                .AppendLine("               , TSTAUSE.RSLT_END_DATETIME ")
                .AppendLine("               , TSTAUSE.RSLT_WORKTIME ")
                .AppendLine("               , TSTAUSE.STOP_REASON_TYPE ")
                .AppendLine("               , MVCL.VCL_VIN ")
                .AppendLine("               , MMOD.MODEL_NAME ")
                .AppendLine("               , MVCLDLR.REG_NUM ")
                .AppendLine("               , TCWRES.CARWASH_RSLT_ID ")
                .AppendLine("               , TCWRES.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("               , TCWRES.RSLT_END_DATETIME   AS CW_RSLT_END_DATETIME ")
                .AppendLine("               , MSRVCLS.SVC_CLASS_NAME ")
                .AppendLine("               , MSRVCLS.SVC_CLASS_NAME_ENG ")
                .AppendLine("               , MMERC.UPPER_DISP ")
                .AppendLine("               , MMERC.LOWER_DISP ")
                .AppendLine("            FROM ")
                .AppendLine("                 TB_T_SERVICEIN TSRVIN ")
                .AppendLine("               , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("               , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("               , TB_M_VEHICLE MVCL ")
                .AppendLine("               , TB_M_MODEL MMOD ")
                .AppendLine("               , TB_M_VEHICLE_DLR MVCLDLR ")
                .AppendLine("               , TB_T_CARWASH_RESULT TCWRES ")
                .AppendLine("               , TB_M_SERVICE_CLASS MSRVCLS ")
                .AppendLine("               , TB_M_MERCHANDISE MMERC ")
                .AppendLine("           WHERE TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("             AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("             AND TSRVIN.VCL_ID = MVCL.VCL_ID(+) ")
                .AppendLine("             AND MVCL.MODEL_CD = MMOD.MODEL_CD(+) ")
                .AppendLine("             AND TSRVIN.VCL_ID = MVCLDLR.VCL_ID(+) ")
                .AppendLine("             AND TSRVIN.DLR_CD = MVCLDLR.DLR_CD(+) ")
                .AppendLine("             AND TSRVIN.SVCIN_ID = TCWRES.SVCIN_ID(+) ")
                .AppendLine("             AND TJOBDTL.SVC_CLASS_ID = MSRVCLS.SVC_CLASS_ID(+) ")
                .AppendLine("             AND TJOBDTL.MERC_ID = MMERC.MERC_ID(+) ")
                .AppendLine("             AND TSRVIN.DLR_CD = :DLRCD ")
                .AppendLine("             AND TSRVIN.BRN_CD = :STRCD ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'キャンセルしたチップが要らない場合（詳細画面から）
                If Not cancelChipFlg Then
                    .AppendLine("         AND TJOBDTL.CANCEL_FLG = N'0' ")
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("             AND TSRVIN.SVCIN_ID IN ( ")
                .AppendLine(strSvcinIdList)
                .AppendLine("                                    ) ")
                .AppendLine("             ) ")
                .AppendLine(" ORDER BY ")
                .AppendLine("          SVCIN_ID ")
                .AppendLine("        , JOB_DTL_ID ")
                .AppendLine("        , STALL_USE_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStallChipInfoDataTable)("TABLETSMBCOMMONCLASS_037")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function


        ''' <summary>
        ''' 仮仮チップの情報を取得する
        ''' </summary>
        ''' <param name="stallList">ストールIDリスト</param>
        ''' <param name="dtNow">現在日時</param>
        ''' <param name="dtStallStartTime">営業開始時間</param>
        ''' <param name="dtStallEndTime">営業終了時間</param>
        ''' <returns>仮仮チップの情報</returns>
        Public Function GetKariKariChipByStallId(ByVal stallList As List(Of Decimal), _
                                                 ByVal dtNow As Date, _
                                                 ByVal dtStallStartTime As Date, _
                                                 ByVal dtStallEndTime As Date) As TabletSmbCommonClassKariKariChipInfoDataTable

            'ストールIDを「stallid1,stallid2,…stallidN」のstringに変更する
            Dim sbStallIdList As New StringBuilder
            Dim strStallIdList As String = ""
            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(stallList) OrElse stallList.Count = 0 Then
                Return New TabletSmbCommonClassKariKariChipInfoDataTable
            Else
                For Each stallId As String In stallList
                    sbStallIdList.Append(stallId)
                    sbStallIdList.Append(",")
                Next
                strStallIdList = sbStallIdList.ToString()
                '最後のコンマを削除する
                strStallIdList = strStallIdList.Substring(0, strStallIdList.Length - 1)
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallIdList={1}, dtStallStartTime={2}, dtStallEndTime={3}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, strStallIdList, dtStallStartTime, dtStallEndTime))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_038 */ ")
                .AppendLine("        T1.SVCIN_TEMP_RESV_ID ")
                .AppendLine("      , T1.SVCIN_ID ")
                .AppendLine("      , T1.JOB_DTL_ID ")
                .AppendLine("      , T1.STALL_ID ")
                .AppendLine("      , T1.SCHE_START_DATETIME START_DATETIME ")
                .AppendLine("      , T1.SCHE_END_DATETIME END_DATETIME ")
                .AppendLine("      , T1.SCHE_WORKTIME WORKTIME ")
                .AppendLine("      , T1.UPDATE_STF_CD  ")
                .AppendLine("      , T2.STF_NAME ")
                .AppendLine("      , T1.ROW_UPDATE_DATETIME ")
                .AppendLine("      , T1.ROW_LOCK_VERSION ")
                .AppendLine("   FROM  ")
                .AppendLine("        TB_T_SVCIN_TEMP_RESV T1 ")
                .AppendLine("      , TB_M_STAFF T2 ")
                .AppendLine("   WHERE  ")
                .AppendLine("       T1.UPDATE_STF_CD = T2.STF_CD(+) ")
                .AppendLine("       AND T1.STALL_ID IN ( ")
                .AppendLine(strStallIdList)
                .AppendLine("                           ) ")
                .AppendLine("       AND T1.ROW_UPDATE_DATETIME  > :BEFOUREONEHOUR ")
                .AppendLine("       AND ((T1.SCHE_START_DATETIME >= :WORKSTARTTIME ")
                .AppendLine("             AND T1.SCHE_START_DATETIME < :WORKENDTIME) ")
                .AppendLine("         OR (T1.SCHE_END_DATETIME > :WORKSTARTTIME ")
                .AppendLine("             AND T1.SCHE_END_DATETIME <= :WORKENDTIME)) ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassKariKariChipInfoDataTable)("TABLETSMBCOMMONCLASS_038")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("BEFOUREONEHOUR", OracleDbType.Date, dtNow.AddHours(-1))
                query.AddParameterWithTypeValue("WORKSTARTTIME", OracleDbType.Date, dtStallStartTime)
                query.AddParameterWithTypeValue("WORKENDTIME", OracleDbType.Date, dtStallEndTime)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 差分リフレッシュの間に変更があるRO情報のサービス入庫IDを取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="theTime">前回差分リフレッシュした日時</param>
        ''' <returns>RO情報のサービス入庫ID</returns>
        ''' <remarks>
        ''' RO情報テーブルからデータを取得時、
        ''' ROステータスが99以外のデータを取得する。
        ''' ここで、99の判断条件はいらない。
        ''' 原因はRO情報テーブルに更新があれば、工程管理画面上にチップを更新させる
        ''' それで、99(ROキャンセル)と関係ない
        ''' </remarks>
        Public Function GetSvcinIdByDiffRefresh(ByVal dealerCode As String, _
                                                ByVal branchCode As String, _
                                                ByVal theTime As Date) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, theTime={3}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, theTime))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_063 */  ")
                .AppendLine("        DISTINCT (T1.SVCIN_ID) AS COL1 ")
                .AppendLine("   FROM  ")
                .AppendLine("        TB_T_RO_INFO T1  ")
                .AppendLine("   WHERE  ")
                .AppendLine("        T1.DLR_CD = :DLR_CD ")
                .AppendLine("    AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("    AND T1.ROW_UPDATE_DATETIME >= :THETIME  ")
                .AppendLine("    AND T1.SVCIN_ID <> 0  ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_063")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("THETIME", OracleDbType.Date, theTime)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

        ''' <summary>
        ''' サービス入庫テーブル情報取得
        ''' </summary>
        ''' <param name="inServiceinId">サービス入庫ID</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>サービス入庫情報</returns>
        ''' <remarks>サービス入庫IDよりサービス入庫テーブルの情報をすべて取得する処理</remarks>
        ''' <history>
        ''' </history>
        Public Function GetServiceinInfo(ByVal inServiceinId As Decimal, _
                                         ByVal inDealerCode As String, _
                                         ByVal inBranchCode As String) As TabletSmbCommonClassServiceinInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inServiceinId:{2};inDealerCode:{3};inBranchCode:{4};" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceinId.ToString(CultureInfo.CurrentCulture) _
                        , inDealerCode _
                        , inBranchCode))

            'データ格納用
            Dim dt As TabletSmbCommonClassServiceinInfoDataTable

            'SQL作成
            Dim sql As New StringBuilder
            With sql
                .AppendLine("SELECT /* TABLETSMBCOMMONCLASS_071 */ ")
                .AppendLine("       SVCIN_ID ")
                .AppendLine("      ,DLR_CD ")
                .AppendLine("      ,BRN_CD ")
                .AppendLine("      ,RO_NUM ")
                .AppendLine("      ,CST_ID ")
                .AppendLine("      ,VCL_ID ")
                .AppendLine("      ,CST_VCL_TYPE ")
                .AppendLine("      ,SVCIN_MILE ")
                .AppendLine("      ,TLM_CONTRACT_FLG ")
                .AppendLine("      ,ACCEPTANCE_TYPE ")
                .AppendLine("      ,PICK_DELI_TYPE ")
                .AppendLine("      ,CARWASH_NEED_FLG ")
                .AppendLine("      ,SMS_TRANSMISSION_FLG ")
                .AppendLine("      ,RESV_STATUS ")
                .AppendLine("      ,SVC_STATUS ")
                .AppendLine("      ,SCHE_SVCIN_DATETIME ")
                .AppendLine("      ,SCHE_DELI_DATETIME ")
                .AppendLine("      ,RSLT_SVCIN_DATETIME ")
                .AppendLine("      ,INVOICE_PREP_COMPL_DATETIME ")
                .AppendLine("      ,RSLT_DELI_DATETIME ")
                .AppendLine("      ,SVCIN_CREATE_TYPE ")
                .AppendLine("      ,PIC_SA_STF_CD ")
                .AppendLine("      ,NOSHOW_FLLW_FLG ")
                .AppendLine("      ,ADD_JOB_ADVICE ")
                .AppendLine("      ,NEXT_SVCIN_INSPECTION_ADVICE ")
                .AppendLine("      ,CONTACT_PERSON_NAME ")
                .AppendLine("      ,CONTACT_PHONE ")
                .AppendLine("      ,CREATE_DATETIME ")
                .AppendLine("      ,CREATE_STF_CD ")
                .AppendLine("      ,UPDATE_DATETIME ")
                .AppendLine("      ,UPDATE_STF_CD ")
                .AppendLine("      ,ROW_CREATE_DATETIME ")
                .AppendLine("      ,ROW_CREATE_ACCOUNT ")
                .AppendLine("      ,ROW_CREATE_FUNCTION ")
                .AppendLine("      ,ROW_UPDATE_DATETIME ")
                .AppendLine("      ,ROW_UPDATE_ACCOUNT ")
                .AppendLine("      ,ROW_UPDATE_FUNCTION ")
                .AppendLine("      ,ROW_LOCK_VERSION ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_SERVICEIN ")
                .AppendLine(" WHERE ")
                .AppendLine("       SVCIN_ID = :SVCIN_ID ")
                .AppendLine("   AND DLR_CD = :DLR_CD ")
                .AppendLine("   AND BRN_CD = :BRN_CD ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassServiceinInfoDataTable)("TABLETSMBCOMMONCLASS_071")
                query.CommandText = sql.ToString()

                'バインド設定
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceinId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)

                'SQL実行
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
        End Function

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

#End Region

#Region "関連チップ情報取得"

        ''' <summary>
        ''' 関連チップの情報を取得
        ''' </summary>
        ''' <param name="svcInIdList">サービス入庫ID</param>
        ''' <returns>関連チップの情報テーブル</returns>
        Public Function GetAllRelationChipInfo(ByVal svcInIdList As List(Of Decimal)) As TabletSmbCommonClassRelationChipInfoDataDataTable

            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(svcInIdList) OrElse svcInIdList.Count = 0 Then
                Return New TabletSmbCommonClassRelationChipInfoDataDataTable
            End If

            'サービス入庫IDを「svcInId1,svcInId2,…svcInIdN」のstringに変更する
            Dim sbSvcInIdList As New StringBuilder
            For Each stallId As String In svcInIdList
                sbSvcInIdList.Append(stallId)
                sbSvcInIdList.Append(",")
            Next
            Dim strSvcInIdList As String = sbSvcInIdList.ToString()
            '最後のコンマを削除する
            strSvcInIdList = strSvcInIdList.Substring(0, strSvcInIdList.Length - 1)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcInIdList={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, strSvcInIdList))

            '関連チップがある
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_035 */ ")
                .AppendLine("        TJOBDT.SVCIN_ID ")

                '2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない
                .AppendLine("      , TJOBDT.JOB_DTL_ID ")
                '2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない

                .AppendLine("      , TSTAUSE.STALL_USE_ID ")
                .AppendLine("      , ( CASE ")
                .AppendLine("             WHEN ")
                .AppendLine("                TSTAUSE.RSLT_START_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                .AppendLine("             THEN ")
                .AppendLine("                TSTAUSE.SCHE_START_DATETIME ")
                .AppendLine("             ELSE ")
                .AppendLine("                TSTAUSE.RSLT_START_DATETIME ")
                .AppendLine("             END ")
                .AppendLine("        ) AS START_DATETIME ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_DTL TJOBDT ")
                .AppendLine("      , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("  WHERE ")
                .AppendLine("        TJOBDT.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("    AND TJOBDT.CANCEL_FLG = N'0' ")
                .AppendLine("    AND ")
                .AppendLine("        TSTAUSE.STALL_USE_ID IN ")
                .AppendLine("        ( ")
                .AppendLine("            SELECT MAX(T3.STALL_USE_ID) AS STALL_USE_ID ")
                .AppendLine("              FROM TB_T_JOB_DTL T4 ")
                .AppendLine("                 , TB_T_STALL_USE T3 ")
                .AppendLine("             WHERE T4.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("               AND T4.SVCIN_ID IN ")
                .AppendLine("               ( ")
                .AppendLine("                   SELECT T2.SVCIN_ID ")
                .AppendLine("                     FROM ( ")
                .AppendLine("                              SELECT ")
                .AppendLine("                                     COUNT(1) COUNT ")
                .AppendLine("                                   , SVCIN_ID ")
                .AppendLine("                                FROM TB_T_JOB_DTL T1 ")
                .AppendLine("                               WHERE T1.CANCEL_FLG = N'0' ")
                .AppendLine("                                 AND T1.SVCIN_ID IN ( ")
                .AppendLine(strSvcInIdList)
                .AppendLine("                                                    ) ")
                .AppendLine("                            GROUP BY T1.SVCIN_ID ")
                .AppendLine("                          ) T2 ")
                .AppendLine("                    WHERE T2.COUNT > 1 ")
                .AppendLine("               ) ")
                .AppendLine("          GROUP BY T3.JOB_DTL_ID ")
                .AppendLine("       ) ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassRelationChipInfoDataDataTable)("TABLETSMBCOMMONCLASS_035")
                query.CommandText = sql.ToString()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function
#End Region

#End Region

#Region "重複配置チェック"
        ''' <summary>
        ''' チップがほかのチップと重複配置されているチップ数
        ''' </summary>
        ''' <param name="dlrCode">販売店</param>
        ''' <param name="brnCode">店舗</param>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="scheStartDateTime">予定開始日時</param>
        ''' <param name="scheEndDateTime">予定終了日時</param>
        ''' <returns>重複配置されているチップ数</returns>
        ''' <remarks></remarks>
        Public Function GetChipOverlapChipNums(ByVal dlrCode As String, _
                                                 ByVal brnCode As String, _
                                                 ByVal stallUseId As Decimal, _
                                                 ByVal stallId As Decimal, _
                                                 ByVal scheStartDateTime As Date, _
                                                 ByVal scheEndDateTime As Date, _
                                                 ByVal defaultDateTime As Date) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dlrCode={1}, brnCode={2}, stallUseId={3}, stallId={4}, scheStartDateTime={5}, scheEndDateTime={6}, defaultDateTime={7}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, dlrCode, brnCode, stallUseId, stallId, scheStartDateTime, scheEndDateTime, defaultDateTime))

            '引数チェック
            If String.IsNullOrEmpty(dlrCode) Or String.IsNullOrEmpty(brnCode) Then
                Throw New ArgumentNullException()
            End If

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_007 */")
                .AppendLine("        COUNT(1) COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("      , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("  WHERE ")
                .AppendLine("        TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("    AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("    AND TSRVIN.DLR_CD = :DLR_CD ")
                .AppendLine("    AND TSRVIN.BRN_CD = :BRN_CD ")
                .AppendLine("    AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine("    AND TSTAUSE.BRN_CD = :BRN_CD ")
                .AppendLine("    AND TSRVIN.SVC_STATUS <> N'02' ")
                .AppendLine("    AND TJOBDTL.CANCEL_FLG = N'0' ")
                .AppendLine("    AND TSTAUSE.STALL_ID = :STALL_ID ")
                .AppendLine("    AND TSTAUSE.STALL_USE_STATUS <> N'07' ")
                .AppendLine("    AND TSTAUSE.TEMP_FLG <> N'1' ")
                .AppendLine("    AND ( ")
                '実績開始日時が設定ありの条件
                .AppendLine("             ( ")
                .AppendLine("                     TSTAUSE.RSLT_START_DATETIME <> :DEFAULT_DATE")
                .AppendLine("                 AND TSTAUSE.RSLT_START_DATETIME < :END_DATE ")
                .AppendLine("                 AND (  ")
                .AppendLine("                         ( ")
                .AppendLine("                                  TSTAUSE.RSLT_END_DATETIME = :DEFAULT_DATE ")
                .AppendLine("                              AND TSTAUSE.PRMS_END_DATETIME > :START_DATE ")
                .AppendLine("                         )  ")
                .AppendLine("                         OR ( ")
                .AppendLine("                                  TSTAUSE.RSLT_END_DATETIME <> :DEFAULT_DATE ")
                .AppendLine("                              AND TSTAUSE.RSLT_END_DATETIME >  :START_DATE ")
                .AppendLine("                              AND TSTAUSE.RSLT_WORKTIME <> 0 ")
                .AppendLine("                            ) ")
                .AppendLine("                     ) ")
                .AppendLine("             ) ")
                .AppendLine("          OR ")
                '実績開始日時が設定なしの条件
                .AppendLine("             ( ")
                .AppendLine("                     TSTAUSE.RSLT_START_DATETIME = :DEFAULT_DATE ")
                .AppendLine("                 AND TSTAUSE.SCHE_START_DATETIME < :END_DATE ")
                .AppendLine("                 AND TSTAUSE.SCHE_END_DATETIME > :START_DATE ")
                .AppendLine("             ) ")
                .AppendLine("       ) ")

                If stallUseId <> 0 Then
                    .AppendLine("   AND TSTAUSE.STALL_USE_ID <> :STALL_USE_ID ")
                End If
            End With

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_007")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("DEFAULT_DATE", OracleDbType.Date, defaultDateTime)
                query.AddParameterWithTypeValue("START_DATE", OracleDbType.Date, scheStartDateTime)
                query.AddParameterWithTypeValue("END_DATE", OracleDbType.Date, scheEndDateTime)
                If stallUseId <> 0 Then
                    query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)
                End If
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return CType(tblResult(0)(0), Long)
        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' チップがほかの予約チップと重複配置されているチップ数
        ''' </summary>
        ''' <param name="dlrCode">販売店</param>
        ''' <param name="brnCode">店舗</param>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="scheStartDateTime">予定開始日時</param>
        ''' <param name="scheEndDateTime">予定終了日時</param>
        ''' <returns>重複配置されているチップ数</returns>
        ''' <remarks></remarks>
        Public Function GetChipOverlapRezChipNums(ByVal dlrCode As String, _
                                                 ByVal brnCode As String, _
                                                 ByVal stallUseId As Decimal, _
                                                 ByVal stallId As Decimal, _
                                                 ByVal scheStartDateTime As Date, _
                                                 ByVal scheEndDateTime As Date, _
                                                 ByVal defaultDateTime As Date) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dlrCode={1}, brnCode={2}, stallUseId={3}, stallId={4}, scheStartDateTime={5}, scheEndDateTime={6}, defaultDateTime={7}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, dlrCode, brnCode, stallUseId, stallId, scheStartDateTime, scheEndDateTime, defaultDateTime))

            '引数チェック
            If String.IsNullOrEmpty(dlrCode) Or String.IsNullOrEmpty(brnCode) Then
                Throw New ArgumentNullException()
            End If

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_060 */")
                .AppendLine("        COUNT(1) COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("      , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("  WHERE ")
                .AppendLine("        TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("    AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("    AND TSRVIN.DLR_CD = :DLR_CD ")
                .AppendLine("    AND TSRVIN.BRN_CD = :BRN_CD ")
                .AppendLine("    AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine("    AND TSTAUSE.BRN_CD = :BRN_CD ")
                .AppendLine("    AND TSRVIN.SVC_STATUS <> N'02' ")
                .AppendLine("    AND TJOBDTL.CANCEL_FLG = N'0' ")
                .AppendLine("    AND TSTAUSE.STALL_ID = :STALL_ID ")
                .AppendLine("    AND TSTAUSE.STALL_USE_STATUS <> N'07' ")
                .AppendLine("    AND TSTAUSE.TEMP_FLG <> N'1' ")
                .AppendLine("    AND ( ")
                .AppendLine("           TSTAUSE.RSLT_START_DATETIME = :DEFAULT_DATE ")
                .AppendLine("       AND TSTAUSE.SCHE_START_DATETIME < :END_DATE ")
                .AppendLine("       AND TSTAUSE.SCHE_END_DATETIME > :START_DATE ")
                .AppendLine("       ) ")
                If stallUseId <> 0 Then
                    .AppendLine("   AND TSTAUSE.STALL_USE_ID <> :STALL_USE_ID ")
                End If
            End With

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_060")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("DEFAULT_DATE", OracleDbType.Date, defaultDateTime)
                query.AddParameterWithTypeValue("START_DATE", OracleDbType.Date, scheStartDateTime)
                query.AddParameterWithTypeValue("END_DATE", OracleDbType.Date, scheEndDateTime)
                If stallUseId <> 0 Then
                    query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)
                End If
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return CType(tblResult(0)(0), Long)
        End Function
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END


        ''' <summary>
        ''' チェック時間内仮仮チップ数を取得する
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="scheStartDateTime">チェック用開始日時</param>
        ''' <param name="scheEndDateTime">チェック用終了日時</param>
        ''' <returns>重複配置されている仮仮チップ数</returns>
        ''' <remarks></remarks>
        Public Function GetKariKariChipOverlapChipNums(ByVal stallId As Decimal, _
                                                        ByVal scheStartDateTime As Date, _
                                                        ByVal scheEndDateTime As Date, _
                                                        ByVal dtNow As Date) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}, scheStartDateTime={2}, scheEndDateTime={3}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, stallId, scheStartDateTime, scheEndDateTime))

            '仮仮チップが1時間の有効期
            Dim validDate As Date = dtNow.AddHours(-1)

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_039 */")
                .AppendLine("        COUNT(1) COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SVCIN_TEMP_RESV  ")
                .AppendLine("  WHERE ")
                .AppendLine("       ( ")
                .AppendLine("           ( SCHE_END_DATETIME > :START_DATE ")
                .AppendLine("             AND SCHE_END_DATETIME <= :END_DATE) ")
                .AppendLine("           OR  ")
                .AppendLine("           ( SCHE_START_DATETIME >= :START_DATE ")
                .AppendLine("             AND SCHE_START_DATETIME < :END_DATE) ")
                .AppendLine("       ) ")
                .AppendLine("       AND ROW_UPDATE_DATETIME > :KARIVALIDDATE ")
                .AppendLine("       AND STALL_ID = :STALL_ID ")
            End With

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_039")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("START_DATE", OracleDbType.Date, scheStartDateTime)
                query.AddParameterWithTypeValue("END_DATE", OracleDbType.Date, scheEndDateTime)
                query.AddParameterWithTypeValue("KARIVALIDDATE", OracleDbType.Date, validDate)
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return CType(tblResult(0)(0), Long)
        End Function
#End Region

#Region "関連チップの件数取得"

#Region "関連チップが存在するか否か判定"
        ''' <summary>
        ''' 関連チップが存在するか否か判定
        ''' </summary>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <returns>存在する場合<c>true</c>、存在しない場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function IsExistRelationChip(ByVal svcinId As Decimal) As Boolean

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_008 */ ")
                .AppendLine("        COUNT(1) COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_DTL ")
                .AppendLine("  WHERE  ")
                .AppendLine("        SVCIN_ID = :SVCIN_ID ")
                .AppendLine("    AND CANCEL_FLG = N'0' ")
            End With

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_008")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

            If CType(tblResult(0)(0), Long) > 1 Then
                '「true：関連チップがある」を返却する
                Return True
            End If
            '「false：関連チップがなし」を返却する
            Return False
        End Function

        ''' <summary>
        ''' 関連チップが存在するか否か判定
        ''' </summary>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
        ''' <returns>存在する場合<c>true</c>、存在しない場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function IsExistRelationChip(ByVal svcinId As Decimal, _
                                            ByVal prevCancelJobDtlIdList As List(Of Decimal)) As Boolean

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. svcinId={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      svcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_040 */ ")
                .AppendLine("        JOB_DTL_ID COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_DTL ")
                .AppendLine("  WHERE  ")
                .AppendLine("        SVCIN_ID = :SVCIN_ID ")
                .AppendLine("    AND JOB_DTL_ID NOT IN (:CANCELED_JOB_DTL_ID_LIST) ")
            End With

            '予約情報の絞込み文字列を作成する
            Dim selectString As New StringBuilder

            If IsNothing(prevCancelJobDtlIdList) Then
                '元々キャンセルだった作業内容IDのリストがない場合
                selectString.Append("-1")
            Else
                '元々キャンセルだった作業内容IDのリストがある場合、
                'それらの作業内容IDに該当する予約情報は除く
                For Each canceledJobDtlId In prevCancelJobDtlIdList
                    selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
                    selectString.Append(",")
                Next
                '最後のカンマを削除
                selectString.Remove(selectString.Length - 1, 1)
            End If

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_040")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
                query.AddParameterWithTypeValue("CANCELED_JOB_DTL_ID_LIST", OracleDbType.NVarchar2, selectString.ToString())

                tblResult = query.GetData()
            End Using

            Dim returnValue As Boolean
            If 1 < tblResult.Rows.Count Then
                '関連チップ有り
                returnValue = True
            Else
                '関連チップ無し
                returnValue = False
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E returnValue={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      returnValue))

            Return returnValue

        End Function

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>
        ''' 関連チップに作業中チップ存在するか
        ''' </summary>
        ''' <param name="inSvcInId">サービス入庫ID</param>
        ''' <param name="inStallUseId">ストール利用ID
        ''' (入力の場合：該当ストール利用ID以外の関連チップで検索する、
        ''' 入力しない場合：-1　該当サービス入庫IDに全関連チップで検索する)</param>
        ''' <returns>関連チップに作業中チップ存在するか</returns>
        ''' <remarks></remarks>
        Public Function IsExistWorkingRelationChip(ByVal inSvcInId As Decimal, _
                                                   Optional ByVal inStallUseId As Decimal = -1) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_Start. inSvcInId={1}, inStallUseId={2}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inSvcInId, _
                                      inStallUseId))

            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_067 */ ")
                .AppendLine("        T2.STALL_USE_ID AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_DTL T1 ")
                .AppendLine("      , TB_T_STALL_USE T2 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .AppendLine("    AND T1.SVCIN_ID = :SVCIN_ID ")
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                '.AppendLine("    AND T2.STALL_USE_STATUS = N'02' ")      '作業中
                .AppendLine("    AND T2.STALL_USE_STATUS IN (N'02', N'04') ")      '作業中
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
                .AppendLine("    AND T1.CANCEL_FLG = N'0' ")             '削除してない
                .AppendLine("    AND T2.STALL_USE_ID <> :STALL_USE_ID ") '該当ストール利用ID以外
                .AppendLine("    AND ROWNUM <= 1 ")
            End With

            '戻る用テーブル
            Dim tblResult As TabletSmbCommonClassNumberValueDataTable

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_067")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcInId)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStallUseId)

                'SQL実行
                tblResult = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_End", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name))

            '結果を戻す
            Return tblResult

        End Function
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
#End Region

#Region "自分以外のチップで「03：完了」又は「05：中断」のはあるか"
        ''' <summary>
        ''' 自分以外のチップで、ストール利用ステータスが「03：完了」又は「05：中断」の
        ''' ストール利用の件数が1件以上存在するか否かを判定します。
        ''' </summary>
        ''' <param name="dlrCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <returns>存在する場合<c>true</c>、存在しない場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function IsExistOtherFinishOrStop(ByVal dlrCode As String, _
                                                 ByVal brnCode As String, _
                                                 ByVal svcinId As Decimal, _
                                                 ByVal stallUseId As Decimal) As Boolean

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}, stallUseId={2}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinId, stallUseId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_009 */ ")
                .AppendLine("       COUNT(1) COL1 ")
                .AppendLine("   FROM ( ")
                .AppendLine("            SELECT ")                   '同じの作業内容IDの中の最大ストール利用ID
                .AppendLine("                   MAX(TSTAUSE.STALL_USE_ID) AS STALL_USE_ID ")
                .AppendLine("              FROM ")
                .AppendLine("                   TB_T_SERVICEIN TSRVIN ")
                .AppendLine("                 , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("                 , TB_T_STALL_USE TSTAUSE  ")
                .AppendLine("             WHERE ")
                .AppendLine("                   TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID  ")
                .AppendLine("               AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID  ")
                .AppendLine("               AND TJOBDTL.CANCEL_FLG = N'0'  ")
                .AppendLine("               AND TSRVIN.SVCIN_ID=:SVCIN_ID  ")
                .AppendLine("               AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine("               AND TSTAUSE.BRN_CD = :BRN_CD ")
                .AppendLine("          GROUP BY  ")
                .AppendLine("                   TSTAUSE.JOB_DTL_ID ")
                .AppendLine("        ) T1 ")
                .AppendLine("      , TB_T_STALL_USE T2 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T2.STALL_USE_ID = T1.STALL_USE_ID ")
                .AppendLine("    AND T1.STALL_USE_ID <> :STALL_USE_ID ")            '自分以外
                .AppendLine("    AND (    T2.STALL_USE_STATUS = N'03' ")            '「03：完了」又は「05：中断」
                .AppendLine("          OR T2.STALL_USE_STATUS = N'05' ) ")
            End With

            Dim tblResult As DataTable
            Using query As New DBSelectQuery(Of DataTable)("TABLETSMBCOMMONCLASS_009")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

            If CType(tblResult(0)(0), Long) > 0 Then
                Return True
            End If
            Return False
        End Function
#End Region

#Region "未完了の関連チップの件数取得"
        ''' <summary>
        ''' 未完了の関連チップの件数を取得する
        ''' </summary>
        ''' <param name="dlrCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <returns>未完了の関連チップの件数</returns>
        Public Function GetBeforeFinishRelationChipCount(ByVal dlrCode As String, _
                                                         ByVal brnCode As String, _
                                                         ByVal svcInId As Decimal) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcInId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_010 */ ")
                .AppendLine("       COUNT(1) COL1 ")
                .AppendLine("   FROM ( ")
                .AppendLine("            SELECT ")               '同じの作業内容IDの中の最大ストール利用ID
                .AppendLine("                   Max(TSTAUSE.STALL_USE_ID) STALL_USE_ID ")
                .AppendLine("              FROM ")
                .AppendLine("                   TB_T_SERVICEIN TSRVIN ")
                .AppendLine("                 , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("                 , TB_T_STALL_USE TSTAUSE ")
                .AppendLine("             WHERE ")
                .AppendLine("                   TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("               AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("               AND TJOBDTL.CANCEL_FLG = N'0' ")
                .AppendLine("               AND TSRVIN.SVCIN_ID=:SVCIN_ID ")
                .AppendLine("               AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine("               AND TSTAUSE.BRN_CD = :BRN_CD ")
                .AppendLine("          GROUP BY ")
                .AppendLine("                   TSTAUSE.JOB_DTL_ID ")
                .AppendLine("        ) T1 ")
                .AppendLine("      , TB_T_STALL_USE T2 ")
                .AppendLine(" WHERE ")
                .AppendLine("       T2.STALL_USE_ID = T1.STALL_USE_ID ")
                .AppendLine("   AND T2.STALL_USE_STATUS <> N'03' ")   '完了以外の場合
            End With

            Using query As New DBSelectQuery(Of DataTable)("TABLETSMBCOMMONCLASS_010")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)

                Dim dt As DataTable = query.GetData()
                Dim retCount As Long = CType(dt(0)(0), Long)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. retCount = {1}", System.Reflection.MethodBase.GetCurrentMethod.Name, retCount))
                Return retCount
            End Using
        End Function

        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
        ''' <summary>
        ''' 指定サービス入庫ID単位で未完了チップ件数の取得
        ''' </summary>
        ''' <param name="inDlrCode">販売店コード</param>
        ''' <param name="inBrnCode">店舗コード</param>
        ''' <param name="inSvcInIdList">サービス入庫IDリスト</param>
        ''' <returns>未完了の関連チップの件数を含めるデータテーブル</returns>
        Public Function GetNotFinishedJobDtlCount(ByVal inDlrCode As String, _
                                                  ByVal inBrnCode As String, _
                                                  ByVal inSvcInIdList As List(Of Decimal)) As TabletSmbCommonClassNotFinishedChipCountDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} Start inDlrCode={2}, inBrnCode={3} ", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inDlrCode, _
                                      inBrnCode))

            If inSvcInIdList.Count = 0 Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} End [return nothing.]", _
                                           Me.GetType.ToString, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing

            End If

            'サービス入庫ID　条件用文字列
            Dim sqlSvcInIdList As New StringBuilder

            Dim strSvcInIdListParameter As String

            'サービス入庫IDの数
            Dim count As Long = 1

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNotFinishedChipCountDataTable)("TABLETSMBCOMMONCLASS_073")

                'サービス入庫ID分ループ
                For Each svcInId As Decimal In inSvcInIdList

                    ' SQL作成
                    strSvcInIdListParameter = String.Format(CultureInfo.CurrentCulture, "SVCIN_ID{0}", count)

                    '1行目か判定
                    If 1 < count Then
                        '2行目以降

                        'カンマ設定
                        sqlSvcInIdList.AppendLine(String.Format(CultureInfo.CurrentCulture, ", :{0} ", strSvcInIdListParameter))
                    Else
                        '1行目

                        'カンマ無し
                        sqlSvcInIdList.AppendLine(String.Format(CultureInfo.CurrentCulture, "  :{0} ", strSvcInIdListParameter))
                    End If

                    ' パラメータ作成
                    query.AddParameterWithTypeValue(strSvcInIdListParameter, OracleDbType.Decimal, svcInId)

                    count += 1
                Next

                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_073 */ ")
                    .AppendLine("       T4.SVCIN_ID ")
                    .AppendLine("     , NVL(T3.COUNT, 0) AS COUNT ")
                    .AppendLine("   FROM ( ")
                    .AppendLine("             SELECT  ")
                    .AppendLine("                   T1.SVCIN_ID ")
                    .AppendLine("                 , COUNT(1) AS COUNT ")
                    .AppendLine("               FROM ( ")
                    .AppendLine("                        SELECT ")
                    .AppendLine("                               TSRVIN.SVCIN_ID ")                         '同じの作業内容IDと販売店コードの中の最大ストール利用ID
                    .AppendLine("                             , MAX(TSTAUSE.STALL_USE_ID) STALL_USE_ID ")
                    .AppendLine("                          FROM ")
                    .AppendLine("                               TB_T_SERVICEIN TSRVIN ")
                    .AppendLine("                             , TB_T_JOB_DTL TJOBDTL ")
                    .AppendLine("                             , TB_T_STALL_USE TSTAUSE ")
                    .AppendLine("                         WHERE ")
                    .AppendLine("                               TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                    .AppendLine("                           AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                    .AppendLine("                           AND TJOBDTL.CANCEL_FLG = N'0' ")
                    .AppendLine("                           AND TSRVIN.SVCIN_ID IN ( ")
                    .AppendLine(sqlSvcInIdList.ToString)
                    .AppendLine("                                                  ) ")
                    .AppendLine("                           AND TSTAUSE.DLR_CD = :DLR_CD ")
                    .AppendLine("                           AND TSTAUSE.BRN_CD = :BRN_CD ")
                    .AppendLine("                      GROUP BY ")
                    .AppendLine("                               TSTAUSE.JOB_DTL_ID ")
                    .AppendLine("                             , TSRVIN.SVCIN_ID ")
                    .AppendLine("                    ) T1 ")
                    .AppendLine("                  , TB_T_STALL_USE T2 ")
                    .AppendLine("             WHERE ")
                    .AppendLine("                   T2.STALL_USE_ID = T1.STALL_USE_ID ")
                    .AppendLine("               AND T2.STALL_USE_STATUS <> N'03' ")   '完了以外の場合
                    .AppendLine("          GROUP BY ")
                    .AppendLine("                   T1.SVCIN_ID ")
                    .AppendLine("        ) T3 ")
                    .AppendLine("       , TB_T_SERVICEIN T4 ")
                    .AppendLine("  WHERE  ")
                    .AppendLine("        T3.SVCIN_ID(+) =  T4.SVCIN_ID ")
                    .AppendLine("    AND T4.SVCIN_ID IN (  ")
                    .AppendLine(sqlSvcInIdList.ToString)
                    .AppendLine(" ) ")

                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBrnCode)

                Dim retDt As TabletSmbCommonClassNotFinishedChipCountDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                          "{0}.{1} End retCount={2}", _
                                          Me.GetType.ToString, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          retDt.Count))
                Return retDt

            End Using

        End Function
        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END
#End Region

#Region "関連チップに実績チップ存在有無の判定"
        ''' <summary>
        ''' 自分以外のチップで、ストール利用ステータスが「03：完了」又は「05：中断」の
        ''' ストール利用の件数が1件以上存在するか否かを判定します。
        ''' </summary>
        ''' <param name="dlrCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <returns>存在する場合：1</returns>
        ''' <remarks></remarks>
        Public Function IsExistRsltChip(ByVal dlrCode As String, _
                                        ByVal brnCode As String, _
                                        ByVal svcinId As Decimal) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_056 */ ")
                .AppendLine("      COUNT(1) COL1 ")
                .AppendLine(" FROM ")
                .AppendLine("      TB_T_SERVICEIN TSRVIN ")
                .AppendLine("    , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("    , TB_T_STALL_USE TSTAUSE  ")
                .AppendLine(" WHERE ")
                .AppendLine("     TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID  ")
                .AppendLine(" AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID  ")
                .AppendLine(" AND TJOBDTL.CANCEL_FLG = N'0'  ")
                .AppendLine(" AND TSRVIN.SVCIN_ID=:SVCIN_ID  ")
                .AppendLine(" AND TSTAUSE.RSLT_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS')  ")
                .AppendLine(" AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine(" AND TSTAUSE.BRN_CD = :BRN_CD ")
                .AppendLine(" AND ROWNUM <= 1 ")
            End With

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_056")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return tblResult
        End Function
#End Region
#End Region

#Region "非稼働ストールテーブルの各操作"
        ''' <summary>
        ''' 指定された日付、または日時が非稼働エリアにあるか否かを判定します
        ''' </summary>
        ''' <param name="idleStartDate">非稼働開始日付</param>
        ''' <param name="idleEndDate">非稼働終了日付</param>
        ''' <param name="stallId">ストールID</param> 
        ''' <returns>非稼働日の場合<c>true</c>、稼働日の場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function IsStallIdleDay(ByVal idleStartDate As Date, _
                                        ByVal idleEndDate As Date, _
                                        ByVal stallId As Decimal) As Boolean

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. idleStartDate={1}, idleEndDate={2}, stallId={3}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, idleStartDate, idleEndDate, stallId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_011 */ ")
                .AppendLine("        STALL_IDLE_ID COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STALL_IDLE ")
                .AppendLine("  WHERE ")
                .AppendLine("        IDLE_TYPE = N'0' ")
                .AppendLine("    AND SETTING_UNIT_TYPE = N'0' ")
                .AppendLine("    AND CANCEL_FLG = N'0' ")
                .AppendLine("    AND STALL_ID = :STALL_ID ")
                .AppendLine("    AND IDLE_DATE <= :END_DATE ")
                .AppendLine("    AND IDLE_DATE >= :START_DATE ")
                .AppendLine("    AND ROWNUM <= 1 ")
            End With

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_011")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("START_DATE", OracleDbType.Date, idleStartDate)
                query.AddParameterWithTypeValue("END_DATE", OracleDbType.Date, idleEndDate)
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

            If tblResult.Count > 0 Then
                '「true：非稼働日」を返却する
                Return True
            End If
            '「false：稼働日」を返却する
            Return False
        End Function

        ''' <summary>
        ''' 休憩時間情報を取得します
        ''' </summary>
        ''' <param name="stallId">表示対象ストールID</param>
        ''' <returns>ストール非稼働マスタ</returns>
        ''' <remarks></remarks>
        Public Function GetRestTimeInfo(ByVal stallId As Decimal) As TabletSmbCommonClassIdleTimeInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, stallId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_012 */ ")
                .AppendLine("        STALL_IDLE_ID  ")
                .AppendLine("      , IDLE_START_TIME ")
                .AppendLine("      , IDLE_END_TIME ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STALL_IDLE ")
                .AppendLine("  WHERE ")
                .AppendLine("        IDLE_TYPE = N'1' ")
                .AppendLine("    AND SETTING_UNIT_TYPE = N'1' ")
                .AppendLine("    AND CANCEL_FLG = N'0' ")
                .AppendLine("    AND STALL_ID = :STALL_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassIdleTimeInfoDataTable)("TABLETSMBCOMMONCLASS_012")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' ストール使用不可情報を取得します
        ''' </summary>
        ''' <param name="stallId">表示対象ストールID</param>
        ''' <returns>ストール非稼働マスタ</returns>
        ''' <remarks></remarks>
        Public Function GetStallUnavailableInfo(ByVal idleStartDate As Date, _
                                        ByVal idleEndDate As Date, _
                                        ByVal stallId As Decimal) As TabletSmbCommonClassIdleTimeInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, stallId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* TABLETSMBCOMMONCLASS_013 */ ")
                .AppendLine("          STALL_IDLE_ID  ")
                .AppendLine("        , IDLE_START_DATETIME IDLE_START_TIME ")
                .AppendLine("        , IDLE_END_DATETIME IDLE_END_TIME ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_M_STALL_IDLE ")
                .AppendLine("    WHERE ")
                .AppendLine("          IDLE_TYPE = N'2' ")
                .AppendLine("      AND SETTING_UNIT_TYPE = N'2' ")
                .AppendLine("      AND CANCEL_FLG = N'0' ")
                .AppendLine("      AND IDLE_START_DATETIME < :IDLE_END_DATETIME ")
                .AppendLine("      AND IDLE_END_DATETIME > :IDLE_START_DATETIME ")
                .AppendLine("      AND STALL_ID = :STALL_ID ")
                .AppendLine(" ORDER BY ")
                .AppendLine("          IDLE_START_TIME ")
                .AppendLine("        , IDLE_END_TIME ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassIdleTimeInfoDataTable)("TABLETSMBCOMMONCLASS_013")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("IDLE_START_DATETIME", OracleDbType.Date, idleStartDate)
                query.AddParameterWithTypeValue("IDLE_END_DATETIME", OracleDbType.Date, idleEndDate)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' ある時間範囲内指定ストールに非稼働情報を取得する
        ''' </summary>
        ''' <param name="stallIdList">ストールIDリスト</param>
        ''' <param name="idleStartDateTime">比べる用開始日時</param>
        ''' <param name="idleEndDateTime">比べる用終了日時</param>
        ''' <returns>ストール非稼働マスタ情報リスト</returns>
        ''' <remarks></remarks>
        Public Function GetAllIdleDateInfo(ByVal stallIdList As List(Of Decimal), _
                                           ByVal idleStartDateTime As Date, _
                                           ByVal idleEndDateTime As Date) As TabletSmbCommonClassStallIdleInfoDataTable
            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(stallIdList) OrElse stallIdList.Count = 0 Then
                Return New TabletSmbCommonClassStallIdleInfoDataTable
            End If

            'ストールIDを「stallid1,stallid2,…stallidN」のstringに変更する
            Dim sbStallist As New StringBuilder
            For Each stallId As String In stallIdList
                sbStallist.Append(stallId)
                sbStallist.Append(",")
            Next

            Dim strStallList As String = sbStallist.ToString()
            '最後のコンマを削除する
            strStallList = strStallList.Substring(0, strStallList.Length - 1)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallIdList={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, strStallList))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_014 */ ")
                .AppendLine("        STALL_IDLE_ID  ")
                .AppendLine("      , STALL_ID ")
                .AppendLine("      , IDLE_TYPE ")
                .AppendLine("      , IDLE_DATE ")
                .AppendLine("      , IDLE_START_TIME ")
                .AppendLine("      , IDLE_END_TIME ")
                .AppendLine("      , IDLE_START_DATETIME ")
                .AppendLine("      , IDLE_END_DATETIME ")
                .AppendLine("      , ROW_LOCK_VERSION ")
                '2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
                .AppendLine("      , IDLE_MEMO ")
                '2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STALL_IDLE ")
                .AppendLine("  WHERE ")
                .AppendLine("        CANCEL_FLG = N'0' ")
                .AppendLine("    AND STALL_ID IN ( ")
                .AppendLine(strStallList)
                .AppendLine("                    ) ")
                .AppendLine("    AND ( ")
                .AppendLine("            ( ")                                         '非稼働日
                .AppendLine("                 IDLE_TYPE = N'0' ")
                .AppendLine("             AND SETTING_UNIT_TYPE = N'0' ")
                .AppendLine("             AND IDLE_DATE < :END_DATE_TIME ")
                .AppendLine("             AND IDLE_DATE >= :START_DATE ")
                .AppendLine("            ) ")
                .AppendLine("         OR ( ")                                         '休憩エリア
                .AppendLine("                 IDLE_TYPE = N'1' ")
                .AppendLine("             AND SETTING_UNIT_TYPE = N'1' ")
                .AppendLine("             AND TO_CHAR(IDLE_START_TIME, 'HH24MI') < :END_TIME ")
                .AppendLine("             AND TO_CHAR(IDLE_END_TIME, 'HH24MI')  > :START_TIME ")
                .AppendLine("            ) ")
                .AppendLine("         OR ( ")                                         '使用不可エリア
                .AppendLine("                 IDLE_TYPE = N'2' ")
                .AppendLine("             AND SETTING_UNIT_TYPE = N'2' ")
                .AppendLine("             AND IDLE_START_DATETIME < :END_DATE_TIME ")
                .AppendLine("             AND IDLE_END_DATETIME > :START_DATE_TIME ")
                .AppendLine("            ) ")
                .AppendLine("        ) ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStallIdleInfoDataTable)("TABLETSMBCOMMONCLASS_014")
                query.CommandText = sql.ToString()

                '日付だけ
                Dim idleStartDate As Date = New Date(idleStartDateTime.Year, idleStartDateTime.Month, idleStartDateTime.Day, 0, 0, 0)
                query.AddParameterWithTypeValue("START_DATE", OracleDbType.Date, idleStartDate)
                query.AddParameterWithTypeValue("START_DATE_TIME", OracleDbType.Date, idleStartDateTime)
                query.AddParameterWithTypeValue("END_DATE_TIME", OracleDbType.Date, idleEndDateTime)
                query.AddParameterWithTypeValue("START_TIME", OracleDbType.NVarchar2, idleStartDateTime.ToString("HHmm", CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("END_TIME", OracleDbType.NVarchar2, idleEndDateTime.ToString("HHmm", CultureInfo.CurrentCulture))
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 使用不可エリアを更新する
        ''' </summary>
        ''' <param name="drStallUnavailable">更新用のデータセット</param>
        ''' <param name="systemId">更新クラス</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateStallUnavailable(ByVal drStallUnavailable As TabletSmbCommonClassStallIdleInfoRow, _
                                               ByVal systemId As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_201")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_201 */ ")
                    .AppendLine("       TB_M_STALL_IDLE ")
                    .AppendLine("    SET ")
                    .AppendLine("       STALL_ID = :STALL_ID ")
                    .AppendLine("     , IDLE_START_DATETIME = :IDLE_START_DATETIME ")
                    .AppendLine("     , IDLE_END_DATETIME = :IDLE_END_DATETIME ")
                    .AppendLine("     , UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("     , UPDATE_STF_CD = :UPDATE_STF_CD ")
                    If Not String.IsNullOrEmpty(drStallUnavailable.IDLE_MEMO) Then
                        .AppendLine("     , IDLE_MEMO = :IDLE_MEMO ")
                    End If
                    .AppendLine("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("     , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                    .AppendLine("     , ROW_UPDATE_FUNCTION = :SYSTEM ")
                    .AppendLine("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine(" WHERE STALL_IDLE_ID = :STALL_IDLE_ID ")
                    .AppendLine("   AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                    .AppendLine("   AND CANCEL_FLG = N'0' ")
                End With
                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                If Not String.IsNullOrEmpty(drStallUnavailable.IDLE_MEMO) Then
                    query.AddParameterWithTypeValue("IDLE_MEMO", OracleDbType.NVarchar2, drStallUnavailable.IDLE_MEMO)
                End If
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, drStallUnavailable.STALL_ID)
                query.AddParameterWithTypeValue("STALL_IDLE_ID", OracleDbType.Decimal, drStallUnavailable.STALL_IDLE_ID)
                query.AddParameterWithTypeValue("IDLE_START_DATETIME", OracleDbType.Date, drStallUnavailable.IDLE_START_DATETIME)
                query.AddParameterWithTypeValue("IDLE_END_DATETIME", OracleDbType.Date, drStallUnavailable.IDLE_END_DATETIME)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, drStallUnavailable.UPDATE_DATETIME)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, drStallUnavailable.UPDATE_STF_CD)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, drStallUnavailable.ROW_LOCK_VERSION)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, systemId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using

        End Function

#End Region

#Region "同一のストールに既に作業中のステータスが存在するかチェック"
        ''' <summary>
        ''' 同一のストールに既に作業中のステータスが存在するかチェックします
        ''' </summary>
        ''' <param name="dlrCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="workingStartDateTime">営業開始日時</param>
        ''' <returns>作業中のステータスが存在する場合<c>true</c>、それ以外の場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function HasWorkingChipInOneStall(ByVal dlrCode As String, ByVal brnCode As String, _
                                                 ByVal stallId As Decimal, ByVal workingStartDateTime As Date) As Boolean

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}, workingStartDateTime={2}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, stallId, workingStartDateTime))
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_015 */ ")
                .AppendLine("        COUNT(1) COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_STALL_USE TSTAUSE ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("  WHERE ")
                .AppendLine("        TSTAUSE.STALL_ID = :STALL_ID ")
                .AppendLine("    AND TSTAUSE.STALL_USE_STATUS IN (N'02', N'04') ")
                .AppendLine("    AND TSTAUSE.RSLT_START_DATETIME >= :RSLT_START_DATETIME ")
                .AppendLine("    AND TJOBDTL.CANCEL_FLG <> N'1' ")
                .AppendLine("    AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                .AppendLine("    AND TSTAUSE.DLR_CD = :DLR_CD ")
                .AppendLine("    AND TSTAUSE.BRN_CD = :BRN_CD ")
            End With

            Dim dtQuery As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_015")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, workingStartDateTime)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                dtQuery = query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E count={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, CType(dtQuery(0)(0), Long)))
            '存在する場合
            If CType(dtQuery(0)(0), Long) > 0 Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "次のシーケンス値取得"
        ''' <summary>
        ''' 次のシーケンス値取得
        ''' </summary>
        ''' <param name="strSquenceName">シーケンス名前</param>
        ''' <returns>次のシーケンス値</returns>
        ''' <remarks></remarks>
        Public Function GetSequenceNextVal(ByVal strSquenceName As String) As Decimal
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. strSquenceName={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, strSquenceName))
            'シーケンスから連番を取得する
            Dim rtNextSeqNo As Decimal = 0
            Using query As New DBSelectQuery(Of DataTable)("TABLETSMBCOMMONCLASS_016")
                Dim sqlNextVal As New StringBuilder
                sqlNextVal.AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_016 */ ")
                sqlNextVal.AppendLine(strSquenceName)
                sqlNextVal.AppendLine("      .NEXTVAL AS SEQ ")
                sqlNextVal.AppendLine("   FROM DUAL")
                query.CommandText = sqlNextVal.ToString()
                Using dt As DataTable = query.GetData()
                    rtNextSeqNo = CType(dt.Rows(0)("SEQ"), Decimal)      'シーケンス連番
                End Using
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E rtNextSeqNo={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, rtNextSeqNo))
            Return rtNextSeqNo
        End Function
#End Region

#Region "スタッフ作業テーブルの各操作"
#Region "指定ストールの配属テクニシャン"
        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
        ' ''' <summary>
        ' ''' スタッフストール割当テーブルからストールIDにより、スタッフコードを取得します
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="stallIdList">ストールID</param>
        ' ''' <returns>スタッフコード</returns>
        ' ''' <remarks>ストールに配置可能なスタッフ(TC、CHT)を取得対象とする</remarks>
        'Public Function GetStaffCodeByStallId(ByVal dealerCode As String, _
        '                                    ByVal branchCode As String, _
        '                                    ByVal stallIdList As List(Of Decimal)) As TabletSmbCommonClassStringValueDataTable
        ''' <summary>
        ''' スタッフストール割当テーブルからストールIDにより、スタッフコードを取得します
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallIdList">ストールID</param>
        ''' <param name="stfStallDispType">スタッフストール表示区分</param>
        ''' <returns>スタッフコード</returns>
        ''' <remarks>ストールに配置可能なテクニシャン(普通はTC、CHT)を取得対象とする</remarks>
        Public Function GetStaffCodeByStallId(ByVal dealerCode As String, _
                                              ByVal branchCode As String, _
                                              ByVal stallIdList As List(Of Decimal), _
                                              ByVal stfStallDispType As String) As TabletSmbCommonClassStringValueDataTable
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            'ストールIDを「stallid1,stallid2,…stallidN」のstringに変更する
            Dim sbStallist As New StringBuilder
            Dim strStallList As String = ""
            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(stallIdList) OrElse stallIdList.Count = 0 Then
                Return New TabletSmbCommonClassStringValueDataTable
            Else
                For Each stallId As String In stallIdList
                    sbStallist.Append(stallId)
                    sbStallist.Append(",")
                Next
                strStallList = sbStallist.ToString()
                '最後のコンマを削除する
                strStallList = strStallList.Substring(0, strStallList.Length - 1)
            End If

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2},stallIdList={3}" _
            '                          , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strStallList))
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2},stallIdList={3},stfStallDispType={4}" _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strStallList, stfStallDispType))
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_017 */ ")
                .AppendLine("        T2.STF_CD COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STAFF_STALL T1 ")
                .AppendLine("      , TB_M_STAFF T2 ")
                .AppendLine("  WHERE T1.STF_CD = T2.STF_CD ")
                .AppendLine("    AND T1.STALL_ID IN ( ")
                .AppendLine(strStallList)
                .AppendLine("                       ) ")
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
                '.AppendLine("    AND T2.BRN_OPERATOR_FLG = N'1' ")
                'ストール表示区分により、テクニシャン権限が違う
                If ("0").Equals(stfStallDispType) Then
                    '0:店舗M、店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("2").Equals(stfStallDispType) Then
                    '2:店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("3").Equals(stfStallDispType) Then
                    '3:店舗M、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("        )  ")
                Else
                    'ディフォルト(1):店舗O
                    .AppendLine("    AND T2.BRN_OPERATOR_FLG = N'1' ")
                End If
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END
                .AppendLine("    AND T2.INUSE_FLG = N'1' ")
                .AppendLine("    AND EXISTS (  ")
                .AppendLine("        	      SELECT 1  ")
                .AppendLine("                   FROM TB_M_ORGANIZATION T4  ")
                .AppendLine("                  WHERE T2.ORGNZ_ID = T4.ORGNZ_ID  ")
                .AppendLine("                    AND DLR_CD = :DLRCD  ")
                .AppendLine("                    AND BRN_CD = :STRCD  ")
                .AppendLine("                    AND ORGNZ_SA_FLG = N'1'  ")
                .AppendLine("                    AND INUSE_FLG  = N'1'  ")
                .AppendLine("               )  ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_017")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        '2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' スタッフストール割当テーブルからストールIDにより、スタッフコードを取得します(チーフテクニシャン用)
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallIdList">ストールID</param>
        ''' <returns>チーフテクニシャンスタッフコード</returns>
        ''' <remarks></remarks>
        Public Function GetStaffCodeByStallIdForCht(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallIdList As List(Of Decimal)) As TabletSmbCommonClassStringValueDataTable

            'ストールIDを「stallid1,stallid2,…stallidN」のstringに変更する
            Dim sbStallist As New StringBuilder
            Dim strStallList As String = ""
            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(stallIdList) OrElse stallIdList.Count = 0 Then
                Return New TabletSmbCommonClassStringValueDataTable
            Else
                For Each stallId As String In stallIdList
                    sbStallist.Append(stallId)
                    sbStallist.Append(",")
                Next
                strStallList = sbStallist.ToString()
                '最後のコンマを削除する
                strStallList = strStallList.Substring(0, strStallList.Length - 1)
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2},stallIdList={3}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strStallList))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_061 */ ")
                .AppendLine("        D.STF_CD COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STALL_STALL_GROUP A ")
                .AppendLine("      , TB_M_STALL_GROUP B ")
                .AppendLine("      , TB_M_ORGANIZATION C ")
                .AppendLine("      , TB_M_STAFF D ")
                .AppendLine("      , TBL_USERS E ")
                .AppendLine("  WHERE A.STALL_GROUP_ID = B.STALL_GROUP_ID ")
                .AppendLine("    AND B.ORGNZ_ID = C.ORGNZ_ID ")
                .AppendLine("    AND C.ORGNZ_ID = D.ORGNZ_ID ")
                .AppendLine("    AND D.STF_CD = E.ACCOUNT ")
                .AppendLine("    AND A.STALL_ID IN ( ")
                .AppendLine(strStallList)
                .AppendLine("                       ) ")
                .AppendLine("    AND C.DLR_CD = :DLR_CD ")
                .AppendLine("    AND C.BRN_CD = :BRN_CD ")
                .AppendLine("    AND E.OPERATIONCODE = :OPERATIONCODE ")
                .AppendLine("    AND C.ORGNZ_SA_FLG = N'1' ")
                .AppendLine("    AND C.INUSE_FLG = N'1' ")
                .AppendLine("    AND D.BRN_OPERATOR_FLG = N'1' ")
                .AppendLine("    AND D.INUSE_FLG = N'1' ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_061")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OPERATIONCODE_CHT)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' スタッフストール割当テーブルからストールIDにより、スタッフコードを取得します(TC用)
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallIdList">ストールID</param>
        ''' <returns>TCスタッフコード</returns>
        ''' <remarks></remarks>
        Public Function GetStaffCodeByStallIdForTC(ByVal dealerCode As String, _
                                                   ByVal branchCode As String, _
                                                   ByVal stallIdList As List(Of Decimal)) As TabletSmbCommonClassStringValueDataTable

            'ストールIDを「stallid1,stallid2,…stallidN」のstringに変更する
            Dim sbStallist As New StringBuilder
            Dim strStallList As String = ""
            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(stallIdList) OrElse stallIdList.Count = 0 Then
                Return New TabletSmbCommonClassStringValueDataTable
            Else
                For Each stallId As String In stallIdList
                    sbStallist.Append(stallId)
                    sbStallist.Append(",")
                Next
                strStallList = sbStallist.ToString()
                '最後のコンマを削除する
                strStallList = strStallList.Substring(0, strStallList.Length - 1)
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2},stallIdList={3}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, strStallList))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_062 */ ")
                .AppendLine("        T2.STF_CD COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STAFF_STALL T1 ")
                .AppendLine("      , TB_M_STAFF T2 ")
                .AppendLine("      , TBL_USERS T3 ")
                .AppendLine("  WHERE T1.STF_CD = T2.STF_CD ")
                .AppendLine("    AND T2.STF_CD = T3.ACCOUNT ")
                .AppendLine("    AND T1.STALL_ID IN ( ")
                .AppendLine(strStallList)
                .AppendLine("                       ) ")
                .AppendLine("    AND T2.BRN_OPERATOR_FLG = N'1' ")
                .AppendLine("    AND T2.INUSE_FLG = N'1' ")
                .AppendLine("    AND T3.OPERATIONCODE = :OPERATIONCODE ")
                .AppendLine("    AND EXISTS (  ")
                .AppendLine("        	      SELECT 1  ")
                .AppendLine("                   FROM TB_M_ORGANIZATION T4  ")
                .AppendLine("                  WHERE T2.ORGNZ_ID = T4.ORGNZ_ID  ")
                .AppendLine("                    AND DLR_CD = :DLRCD  ")
                .AppendLine("                    AND BRN_CD = :STRCD  ")
                .AppendLine("                    AND ORGNZ_SA_FLG = N'1'  ")
                .AppendLine("                    AND INUSE_FLG  = N'1'  ")
                .AppendLine("               )  ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_062")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OPERATIONCODE_TC)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function
        '2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "スタッフ作業テーブルに1行挿入"
        ''' <summary>
        ''' スタッフ作業テーブルに一行を挿入する
        ''' </summary>
        ''' <param name="drStaffJob">１行データ</param>
        ''' <param name="dtNow">更新日時</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="systemId">更新クラス</param>
        ''' <returns>1:正常終了、その他:更新失敗</returns>
        Public Function InsertTblStaffJob(ByVal drStaffJob As TabletSmbCommonClassStaffJobRow, _
                                          ByVal dtNow As Date, _
                                          ByVal staffCode As String, _
                                          ByVal systemId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S", System.Reflection.MethodBase.GetCurrentMethod.Name))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_304 */ ")
                .AppendLine("   INTO TB_T_STAFF_JOB (")
                .AppendLine("           STF_JOB_ID ")
                .AppendLine("         , STF_CD ")
                .AppendLine("         , JOB_ID ")
                .AppendLine("         , JOB_TYPE ")
                .AppendLine("         , SCHE_START_DATETIME ")
                .AppendLine("         , SCHE_END_DATETIME ")
                .AppendLine("         , RSLT_START_DATETIME ")
                .AppendLine("         , RSLT_END_DATETIME ")
                .AppendLine("         , ROW_CREATE_DATETIME ")
                .AppendLine("         , ROW_UPDATE_DATETIME ")
                .AppendLine("         , ROW_CREATE_ACCOUNT ")
                .AppendLine("         , ROW_UPDATE_ACCOUNT ")
                .AppendLine("         , ROW_CREATE_FUNCTION ")
                .AppendLine("         , ROW_UPDATE_FUNCTION ")
                .AppendLine("         , ROW_LOCK_VERSION ) ")
                .AppendLine(" VALUES ( ")
                .AppendLine("           :STF_JOB_ID ")
                .AppendLine("         , :STF_CD ")
                .AppendLine("         , :JOB_ID ")
                .AppendLine("         , :JOB_TYPE ")
                .AppendLine("         , :SCHE_START_DATETIME ")
                .AppendLine("         , :SCHE_END_DATETIME ")
                .AppendLine("         , :RSLT_START_DATETIME ")
                .AppendLine("         , :RSLT_END_DATETIME ")
                .AppendLine("         , :UPDATE_DATETIME ")
                .AppendLine("         , :UPDATE_DATETIME ")
                .AppendLine("         , :UPDATE_ACCOUNT ")
                .AppendLine("         , :UPDATE_ACCOUNT ")
                .AppendLine("         , :UPDATE_FUNCTION ")
                .AppendLine("         , :UPDATE_FUNCTION ")
                .AppendLine("         , 0 ) ")
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_304")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("STF_JOB_ID", OracleDbType.Decimal, drStaffJob.STF_JOB_ID)
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, drStaffJob.STF_CD)
                query.AddParameterWithTypeValue("JOB_ID", OracleDbType.Decimal, drStaffJob.JOB_ID)
                query.AddParameterWithTypeValue("JOB_TYPE", OracleDbType.NVarchar2, drStaffJob.JOB_TYPE)
                query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, drStaffJob.SCHE_START_DATETIME)
                query.AddParameterWithTypeValue("SCHE_END_DATETIME", OracleDbType.Date, drStaffJob.SCHE_END_DATETIME)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, drStaffJob.RSLT_START_DATETIME)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, drStaffJob.RSLT_END_DATETIME)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, dtNow)
                query.AddParameterWithTypeValue("UPDATE_ACCOUNT", OracleDbType.NVarchar2, staffCode)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Integer = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "スタッフ作業の実績日時更新"
        ''' <summary>
        ''' スタッフ作業テーブル：「実績終了日時」更新
        ''' </summary>
        ''' <param name="jobId">作業内容ID</param>
        ''' <param name="rsltStartDate">実績開始日時</param>
        ''' <param name="rsltEndDate">実績終了日時</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="systemId">更新クラス</param>
        ''' <param name="dtNow">更新日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateStaffJobRsltDatetime(ByVal jobId As Decimal, ByVal rsltStartDate As Date, ByVal rsltEndDate As Date, _
                                                   ByVal staffCode As String, ByVal systemId As String, ByVal dtNow As Date) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. JOB_ID={1}, RSLT_END_DATETIME={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, jobId, rsltEndDate))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_202")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_202 */ ")
                sql.AppendLine("        TB_T_STAFF_JOB ")
                sql.AppendLine("    SET ")
                sql.AppendLine("        ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                sql.AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                If rsltStartDate <> Date.MinValue Then
                    sql.AppendLine("      , RSLT_START_DATETIME = :RSLT_START_DATETIME ")
                End If

                If rsltEndDate <> Date.MinValue Then
                    sql.AppendLine("      , RSLT_END_DATETIME = :RSLT_END_DATETIME ")
                End If

                sql.AppendLine("  WHERE JOB_ID = :JOB_ID ")
                sql.AppendLine("    AND RSLT_END_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                If rsltStartDate <> Date.MinValue Then
                    query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, rsltStartDate)
                End If

                If rsltEndDate <> Date.MinValue Then
                    query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, rsltEndDate)
                End If
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, staffCode)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, dtNow)
                query.AddParameterWithTypeValue("JOB_ID", OracleDbType.Decimal, jobId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
#Region "スタッフストール表示区分の取得"
        ''' <summary>
        ''' スタッフストール表示区分の取得
        ''' </summary>
        ''' <param name="inDlrCode">販売店コード</param>
        ''' <param name="inBrnCode">店舗コード</param>
        ''' <returns>スタッフストール表示区分</returns>
        ''' <remarks></remarks>
        Public Function GetStaffStallDispType(ByVal inDlrCode As String, _
                                              ByVal inBrnCode As String) As TabletSmbCommonClassStringValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inDlrCode={1}, inBrnCode={2}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, inDlrCode, inBrnCode))


            ''SQLの設定
            Dim sql As New StringBuilder
            With sql
                .Append("   SELECT /* TABLETSMBCOMMONCLASS_064 */")
                .Append("          STF_STALL_DISP_TYPE AS COL1 ")
                .Append("     FROM ")
                .Append("          TB_M_SERVICEIN_SETTING T1 ")
                .Append("    WHERE ")
                .Append("          T1.DLR_CD = :DLRCD ")
                .Append("      AND T1.BRN_CD = :STRCD ")
            End With

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_064")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDlrCode)                   ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBrnCode)                   ' 店舗コード

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function
#End Region
        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

#End Region

#Region "ステータス更新処理"
#Region "サービス入庫テーブルにステータス更新処理"
        ''' <summary>
        ''' サービス入庫テーブル：「ステータス」更新
        ''' </summary>
        ''' <param name="svcinId">ストール入庫ID</param>
        ''' <param name="status">ステータス</param>
        ''' <param name="dtNow">更新日時</param>
        ''' <param name="stfCode">スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateSvcinStatus(ByVal svcinId As Decimal, ByVal status As String, ByVal dtNow As Date, ByVal stfCode As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}, status={2}, dtNow={3}, stfCode={4}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinId, status, dtNow, stfCode))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_204")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_204 */ ")
                    .AppendLine("        TB_T_SERVICEIN ")
                    .AppendLine("    SET ")
                    .AppendLine("        SVC_STATUS = :SVC_STATUS ")
                    .AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, status)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "中断処理の更新(ストール利用テーブル)"
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        ' ''' <summary>
        ' ''' 中断処理の更新(ストール利用テーブル)
        ' ''' </summary>
        ' ''' <param name="chipEntity">更新用チップエンティティ</param>
        ' ''' <returns>更新件数</returns>
        ' ''' <remarks></remarks>
        'Public Function UpdateStallUseChipStop(ByVal stallUseId As Decimal, ByVal chipEntity As TabletSmbCommonClassChipEntityRow, ByVal systemId As String) As Long

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallUseId={1}, systemId={2}" _
        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name, stallUseId, systemId))
        ''' <summary>
        ''' 中断処理の更新(ストール利用テーブル)
        ''' </summary>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <param name="chipEntity">更新用チップエンティティ</param>
        ''' <param name="systemId">機能ID</param>
        ''' <param name="restAutoJudgeFlg">休憩取得自動判定フラグ</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateStallUseChipStop(ByVal stallUseId As Decimal, ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                               ByVal systemId As String, ByVal restAutoJudgeFlg As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallUseId={1}, systemId={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, stallUseId, systemId))
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_205")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_205 */ ")
                    .AppendLine("        TB_T_STALL_USE ")
                    .AppendLine("    SET ")
                    .AppendLine("        RSLT_END_DATETIME = :RSLT_END_DATETIME ")
                    .AppendLine("      , RSLT_WORKTIME = :RSLT_WORKTIME ")
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    '休憩を自動判定しない場合
                    If Not restAutoJudgeFlg.Equals("1") Then
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                    .AppendLine("      , REST_FLG = :REST_FLG ")
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    End If
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                    .AppendLine("      , STALL_USE_STATUS = :STALL_USE_STATUS ")
                    .AppendLine("      , STOP_REASON_TYPE = :STOP_REASON_TYPE ")
                    .AppendLine("      , STALL_IDLE_ID = :STALL_IDLE_ID ")
                    .AppendLine("      , UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("      , UPDATE_STF_CD = :UPDATE_STF_CD ")
                    .AppendLine("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                    .AppendLine("      , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                    .AppendLine("      , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                    '中断メモが空文字でなければ中断メモを更新
                    If Not String.IsNullOrEmpty(chipEntity.STOP_MEMO) Then
                        .AppendLine("      , STOP_MEMO = :STOP_MEMO ")
                    End If
                    .AppendLine(" WHERE  STALL_USE_ID = :STALL_USE_ID ")
                End With
                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, chipEntity.RSLT_END_DATETIME)
                query.AddParameterWithTypeValue("RSLT_WORKTIME", OracleDbType.Long, chipEntity.RSLT_WORKTIME)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                '休憩を自動判定しない場合
                If Not restAutoJudgeFlg.Equals("1") Then
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, chipEntity.REST_FLG)
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                End If
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, chipEntity.STALL_USE_STATUS)
                query.AddParameterWithTypeValue("STOP_REASON_TYPE", OracleDbType.NVarchar2, chipEntity.STOP_REASON_TYPE)
                query.AddParameterWithTypeValue("STALL_IDLE_ID", OracleDbType.Decimal, chipEntity.STALL_IDLE_ID)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, chipEntity.UPDATE_DATETIME)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, chipEntity.UPDATE_STF_CD)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)
                If Not String.IsNullOrEmpty(chipEntity.STOP_MEMO) Then
                    query.AddParameterWithTypeValue("STOP_MEMO", OracleDbType.NVarchar2, chipEntity.STOP_MEMO)
                End If

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region
#End Region

#Region "ストール使用不可の追加処理"
        ''' <summary>
        ''' ストール使用不可を追加する
        ''' </summary>
        ''' <param name="stallIdleId">非稼働テーブルのID</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="idleStartDatetime">非稼働開始日時</param>
        ''' <param name="idleEndDatetime">非稼働終了日時</param>
        ''' <param name="idleMemo">非稼働メモ</param>
        ''' <param name="dtNow">更新日時</param>
        ''' <param name="stfCode">スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function InsertStallUnavailable(ByVal stallIdleId As Decimal, ByVal stallId As Decimal, ByVal idleStartDatetime As Date, ByVal idleEndDatetime As Date, _
                                               ByVal idleMemo As String, ByVal dtNow As Date, ByVal stfCode As String, ByVal systemId As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallIdleId={1}, stallId={2},idleStartDatetime={3}, idleEndDatetime={4}, idleMemo={5}, dtNow={6}, stfCode={7}, systemId={8}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, stallIdleId, stallId, idleStartDatetime, idleEndDatetime, idleMemo, dtNow, stfCode, systemId))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_301")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_301 */ ")
                sql.AppendLine("   INTO TB_M_STALL_IDLE ")
                sql.AppendLine("      ( STALL_IDLE_ID ")
                sql.AppendLine("      , STALL_ID ")
                sql.AppendLine("      , IDLE_TYPE ")
                sql.AppendLine("      , SETTING_UNIT_TYPE ")
                sql.AppendLine("      , IDLE_START_DATETIME ")
                sql.AppendLine("      , IDLE_END_DATETIME ")
                If Not String.IsNullOrEmpty(idleMemo) Then
                    sql.AppendLine("      , IDLE_MEMO ")
                End If
                sql.AppendLine("      , CANCEL_FLG ")
                sql.AppendLine("      , CREATE_DATETIME ")
                sql.AppendLine("      , CREATE_STF_CD ")
                sql.AppendLine("      , UPDATE_DATETIME ")
                sql.AppendLine("      , UPDATE_STF_CD ")
                sql.AppendLine("      , ROW_CREATE_DATETIME ")
                sql.AppendLine("      , ROW_CREATE_ACCOUNT ")
                sql.AppendLine("      , ROW_CREATE_FUNCTION ")
                sql.AppendLine("      , ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , ROW_LOCK_VERSION ")
                sql.AppendLine("      ) ")
                sql.AppendLine(" VALUES ")
                sql.AppendLine("      ( :STALL_IDLE_ID ")
                sql.AppendLine("      , :STALL_ID ")
                sql.AppendLine("      , N'2' ")
                sql.AppendLine("      , N'2' ")
                sql.AppendLine("      , :IDLE_START_DATETIME ")
                sql.AppendLine("      , :IDLE_END_DATETIME ")
                If Not String.IsNullOrEmpty(idleMemo) Then
                    sql.AppendLine("      , :IDLE_MEMO ")
                End If
                sql.AppendLine("      , N'0' ")
                sql.AppendLine("      , :UPDATE_DATETIME ")
                sql.AppendLine("      , :UPDATE_STF_CD ")
                sql.AppendLine("      , :UPDATE_DATETIME ")
                sql.AppendLine("      , :UPDATE_STF_CD ")
                sql.AppendLine("      , :UPDATE_DATETIME ")
                sql.AppendLine("      , :UPDATE_STF_CD ")
                sql.AppendLine("      , :ROW_FUNCTION ")
                sql.AppendLine("      , :UPDATE_DATETIME ")
                sql.AppendLine("      , :UPDATE_STF_CD ")
                sql.AppendLine("      , :ROW_FUNCTION ")
                sql.AppendLine("      , 0 ")
                sql.AppendLine("      ) ")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("STALL_IDLE_ID", OracleDbType.Decimal, stallIdleId)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("IDLE_START_DATETIME", OracleDbType.Date, idleStartDatetime)
                query.AddParameterWithTypeValue("IDLE_END_DATETIME", OracleDbType.Date, idleEndDatetime)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, stfCode)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, dtNow)
                query.AddParameterWithTypeValue("ROW_FUNCTION", OracleDbType.NVarchar2, systemId)
                If Not String.IsNullOrEmpty(idleMemo) Then
                    query.AddParameterWithTypeValue("IDLE_MEMO", OracleDbType.NVarchar2, idleMemo)
                End If

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "ストール利用テーブルの「開始実績日時」などを更新"
        ''' <summary>
        ''' ストール利用テーブルの「開始実績日時」などを更新
        ''' </summary>
        ''' <param name="drStallUse">ストール利用テーブルの1行</param>
        ''' <returns></returns>
        Public Function UpdateStallUseRsltStartDate(ByVal drStallUse As TabletSmbCommonClassChipEntityRow, ByVal systemId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. systemId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, systemId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_206 */ ")
                .AppendLine("        TB_T_STALL_USE ")
                .AppendLine("    SET ")
                .AppendLine("        JOB_ID = :JOB_ID ")
                .AppendLine("      , RSLT_START_DATETIME = :RSLT_START_DATETIME ")
                .AppendLine("      , PRMS_END_DATETIME = :PRMS_END_DATETIME ")
                .AppendLine("      , REST_FLG = :REST_FLG ")
                .AppendLine("      , STALL_USE_STATUS = :STALL_USE_STATUS ")
                .AppendLine("      , UPDATE_DATETIME = :UPDATE_DATETIME ")
                .AppendLine("      , UPDATE_STF_CD = :UPDATE_STF_CD ")
                .AppendLine("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                .AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("      , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                .AppendLine("      , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                .AppendLine("  WHERE STALL_USE_ID = :STALL_USE_ID ")
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_206")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("JOB_ID", OracleDbType.Decimal, drStallUse.JOB_ID)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, drStallUse.RSLT_START_DATETIME)
                query.AddParameterWithTypeValue("PRMS_END_DATETIME", OracleDbType.Date, drStallUse.PRMS_END_DATETIME)
                query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, drStallUse.REST_FLG)
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, drStallUse.STALL_USE_STATUS)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, drStallUse.UPDATE_DATETIME)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, drStallUse.UPDATE_STF_CD)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, drStallUse.STALL_USE_ID)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Integer = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "本予約、仮予約処理"
        ''' <summary>
        ''' 本予約、仮予約でサービス入庫テーブルを更新
        ''' </summary>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <param name="rezStatus">予約ステータス</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns></returns>
        Public Function UpdateServiceinResvStatus(ByVal svcInId As Decimal _
                                               , ByVal rezStatus As String _
                                               , ByVal updateAccount As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SVCIN_ID={1}, RESV_STATUS={2}, UPDATE_STF_CD={3}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId, rezStatus, updateAccount))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_207 */ ")
                .AppendLine("        TB_T_SERVICEIN ")
                .AppendLine("    SET ")
                .AppendLine("        RESV_STATUS = :RESV_STATUS ")
                .AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_207")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, rezStatus)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Integer = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "入庫処理"
        ''' <summary>
        ''' サービス作業管理テーブルの[実績入庫日付]を更新
        ''' </summary>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <param name="svcStatus">サービスステータス</param>
        ''' <param name="rsltServiceinDateTimeNoSec">実績入庫日時</param>
        ''' <param name="updateDate">更新日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns></returns>
        Public Function UpdateRsltServiceinDate(ByVal svcInId As Decimal _
                                                     , ByVal svcStatus As String _
                                                     , ByVal rsltServiceinDateTimeNoSec As Date _
                                                     , ByVal updateDate As Date _
                                                     , ByVal updateAccount As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcInId={1}, svcStatus={2}, rsltServiceinDateTimeNoSec={3}, updateDate={4}, updateAccount={5}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId, svcStatus, rsltServiceinDateTimeNoSec, updateDate, updateAccount))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_208 */ ")
                .AppendLine("        TB_T_SERVICEIN ")
                .AppendLine("    SET ")
                .AppendLine("        RSLT_SVCIN_DATETIME = :RSLT_SVCIN_DATETIME ")
                .AppendLine("      , SVC_STATUS = :SVC_STATUS ")
                .AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_208")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("RSLT_SVCIN_DATETIME", OracleDbType.Date, rsltServiceinDateTimeNoSec)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, svcStatus)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
                'SQL実行(影響行数を返却)
                Dim queryCount As Integer = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "移動、リサイズ処理"
        ''' <summary>
        ''' ストール上チップを移動、リサイズ処理
        ''' </summary>
        ''' <param name="drChipInfo">変更後のストールのSTALLID</param>
        ''' <returns>1:正常終了、その他:更新失敗</returns>
        Public Function StallChipMoveResize(ByVal drChipInfo As TabletSmbCommonClassChipEntityRow, _
                                            ByVal systemId As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S systemId={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, systemId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_209 */ ")
                .AppendLine("        TB_T_STALL_USE ")
                .AppendLine("    SET STALL_ID = :STALLID ")
                .AppendLine("      , SCHE_START_DATE = :SCHE_START_DATE ")
                .AppendLine("      , SCHE_START_DATETIME = :SCHE_START_DATETIME ")
                .AppendLine("      , SCHE_END_DATETIME = :SCHE_END_DATETIME ")
                .AppendLine("      , PRMS_END_DATETIME = :PRMS_END_DATETIME ")
                .AppendLine("      , SCHE_WORKTIME = :SCHE_WORKTIME ")
                .AppendLine("      , UPDATE_DATETIME = :UPDATE_DATETIME ")
                .AppendLine("      , UPDATE_STF_CD = :UPDATE_STF_CD ")
                .AppendLine("      , REST_FLG = :REST_FLG ")
                .AppendLine("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                .AppendLine("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
                .AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                If Not IsDBNull(drChipInfo("TEMP_FLG")) Then
                    .AppendLine("      , TEMP_FLG = :TEMP_FLG ")
                End If
                If Not IsDBNull(drChipInfo("PARTS_FLG")) Then
                    .AppendLine("      , PARTS_FLG = :PARTS_FLG ")
                End If
                If Not IsDBNull(drChipInfo("STALL_USE_STATUS")) Then
                    .AppendLine("      , STALL_USE_STATUS = :STALL_USE_STATUS ")
                End If
                .AppendLine("  WHERE STALL_USE_ID = :STALL_USE_ID ")
            End With
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_209")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, drChipInfo.STALL_ID)
                query.AddParameterWithTypeValue("SCHE_START_DATE", OracleDbType.NVarchar2, drChipInfo.SCHE_START_DATETIME.ToString("yyyyMMdd", CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, drChipInfo.SCHE_START_DATETIME)
                query.AddParameterWithTypeValue("SCHE_END_DATETIME", OracleDbType.Date, drChipInfo.SCHE_END_DATETIME)
                query.AddParameterWithTypeValue("PRMS_END_DATETIME", OracleDbType.Date, drChipInfo.PRMS_END_DATETIME)
                query.AddParameterWithTypeValue("SCHE_WORKTIME", OracleDbType.Int64, drChipInfo.SCHE_WORKTIME)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, drChipInfo.UPDATE_DATETIME)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, drChipInfo.UPDATE_STF_CD)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, drChipInfo.STALL_USE_ID)
                query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, drChipInfo.REST_FLG)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, drChipInfo.UPDATE_DATETIME)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, drChipInfo.UPDATE_STF_CD)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)
                If Not IsDBNull(drChipInfo("TEMP_FLG")) Then
                    query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, drChipInfo.TEMP_FLG)
                End If
                If Not IsDBNull(drChipInfo("PARTS_FLG")) Then
                    query.AddParameterWithTypeValue("PARTS_FLG", OracleDbType.NVarchar2, drChipInfo.PARTS_FLG)
                End If
                If Not IsDBNull(drChipInfo("STALL_USE_STATUS")) Then
                    query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, drChipInfo.STALL_USE_STATUS)
                End If
                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "チップ終了処理"
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        ' ''' <summary>
        ' ''' チップ終了処理
        ' ''' </summary>
        ' ''' <param name="chipEntity">チップエンティティ</param>
        ' ''' <param name="systemId">更新クラス</param>
        ' ''' <returns>更新件数</returns>
        ' ''' <remarks></remarks>
        'Public Function UpdateStallUseRsltEndDate(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, ByVal systemId As String) As Long
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
        ''' <summary>
        ''' チップ終了処理
        ''' </summary>
        ''' <param name="chipEntity">チップエンティティ</param>
        ''' <param name="systemId">更新クラス</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateStallUseRsltEndDate(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                                  ByVal systemId As String, ByVal restAutoJudgeFlg As String) As Long
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_210")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_210 */ ")
                sql.AppendLine("       TB_T_STALL_USE  ")
                sql.AppendLine("   SET ")
                sql.AppendLine("       STALL_USE_STATUS = :STALL_USE_STATUS ")
                sql.AppendLine("     , UPDATE_DATETIME = :UPDATE_DATETIME ")
                sql.AppendLine("     , UPDATE_STF_CD = :UPDATE_STF_CD ")
                sql.AppendLine("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                sql.AppendLine("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                sql.AppendLine("     , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                sql.AppendLine("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                If Not chipEntity.IsRSLT_END_DATETIMENull Then
                    sql.AppendLine("     , RSLT_END_DATETIME = :RSLT_END_DATETIME ")
                    sql.AppendLine("     , RSLT_WORKTIME = :RSLT_WORKTIME ")
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    '休憩を自動判定しない場合
                    If Not restAutoJudgeFlg.Equals("1") Then
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                    sql.AppendLine("     , REST_FLG = :REST_FLG ")
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    End If
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                End If
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

                '中断理由区分に値があれば
                If Not chipEntity.IsSTOP_REASON_TYPENull Then

                    '更新する
                    sql.AppendLine("     , STOP_REASON_TYPE = :STOP_REASON_TYPE ")
                    sql.AppendLine("     , STOP_MEMO = :STOP_MEMO ")

                End If

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                sql.AppendLine(" WHERE STALL_USE_ID = :STALL_USE_ID ")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, chipEntity.STALL_USE_ID)
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, chipEntity.STALL_USE_STATUS)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, chipEntity.UPDATE_STF_CD)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, chipEntity.UPDATE_DATETIME)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)
                If Not chipEntity.IsRSLT_END_DATETIMENull Then
                    query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, chipEntity.RSLT_END_DATETIME)
                    query.AddParameterWithTypeValue("RSLT_WORKTIME", OracleDbType.Long, chipEntity.RSLT_WORKTIME)
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    '休憩を自動判定しない場合
                    If Not restAutoJudgeFlg.Equals("1") Then
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                    query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, chipEntity.REST_FLG)
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    End If
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                End If

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                '中断理由区分に値があれば
                If Not chipEntity.IsSTOP_REASON_TYPENull Then

                    query.AddParameterWithTypeValue("STOP_REASON_TYPE", OracleDbType.NVarchar2, chipEntity.STOP_REASON_TYPE)
                    query.AddParameterWithTypeValue("STOP_MEMO", OracleDbType.NVarchar2, chipEntity.STOP_MEMO)

                End If
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "洗車処理/洗車undo処理"

        ''' <summary>
        ''' サービス入庫テーブル：「洗車開始」更新
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        '''  <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        '''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Public Function UpdateServiceinWashCar(ByVal inServiceInId As Decimal, _
                                                 ByVal inPrevStatus As String, _
                                                 ByVal inAfterStatus As String, _
                                                 ByVal inAccount As String, _
                                                 ByVal inNowDate As Date) As Long
            'Public Function UpdateServiceinWashStart(ByVal inServiceInId As Long, _
            '                                   ByVal inAccount As String, _
            '                                   ByVal inNowDate As Date) As Long
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "{0}.{1} START P1:{2} P2:{3} P3:{4}" _
            '            , Me.GetType.ToString _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '            , inServiceInId.ToString(CultureInfo.CurrentCulture) _
            '            , inAccount _
            '            , inNowDate.ToString(CultureInfo.CurrentCulture)))

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} P5:{6}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inPrevStatus _
                        , inAfterStatus _
                        , inAccount _
                        , inNowDate.ToString(CultureInfo.CurrentCulture)))
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_211")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_211 */ ")
                sql.AppendLine("        TB_T_SERVICEIN ")
                sql.AppendLine("    SET SVC_STATUS = :SVC_STATUS_CARWASH ")
                sql.AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                sql.AppendLine("    AND SVC_STATUS = :SVC_STATUS_WASHWAIT ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '値
                'query.AddParameterWithTypeValue("SVC_STATUS_CARWASH", OracleDbType.NVarchar2, "08")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                '条件
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'query.AddParameterWithTypeValue("SVC_STATUS_WASHWAIT", OracleDbType.NVarchar2, "07")
                query.AddParameterWithTypeValue("SVC_STATUS_WASHWAIT", OracleDbType.NVarchar2, inPrevStatus)
                query.AddParameterWithTypeValue("SVC_STATUS_CARWASH", OracleDbType.NVarchar2, inAfterStatus)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' サービス作業管理テーブル：「洗車終了」更新
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inSvcStatus">サービスステータス</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateServiceWorkWashEnd(ByVal inServiceInId As Decimal, _
                                                 ByVal inSvcStatus As String) As Long
            'Public Function UpdateServiceWorkWashEnd(ByVal inServiceInId As Long, _
            '                                 ByVal inAccount As String, _
            '                                 ByVal inNowDate As Date, _
            '                                 ByVal inSvcStatus As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inSvcStatus))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_212")
                'SQL組み立て
                Dim sql As New StringBuilder
                'sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_212 */ ")
                'sql.AppendLine("        TB_T_SERVICEIN ")
                'sql.AppendLine("    SET SVC_STATUS = :SVC_STATUS_CARWASH ")
                'sql.AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                'sql.AppendLine("    AND SVC_STATUS = :SVC_STATUS_WASHING ")
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_212 */ ")
                sql.AppendLine("        TB_T_SERVICEIN ")
                sql.AppendLine("    SET SVC_STATUS = :SVC_STATUS_CARWASH ")
                sql.AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                '値
                query.AddParameterWithTypeValue("SVC_STATUS_CARWASH", OracleDbType.NVarchar2, inSvcStatus)

                '条件
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                'query.AddParameterWithTypeValue("SVC_STATUS_WASHING", OracleDbType.NVarchar2, "08")

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
#Region "洗車実績更新"
        ''' <summary>
        ''' 洗車実績テーブル：「洗車終了」更新
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inDropNowDate">現在日時(秒切り捨て)</param>
        ''' <param name="inNowDate">現在時間</param>
        ''' <param name="inAccount">スタフID</param>
        ''' <param name="inSystemId">プログラムID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateCarWashResult(ByVal inServiceInId As Decimal, _
                                            ByVal inDropNowDate As Date, _
                                            ByVal inNowDate As Date, _
                                            ByVal inAccount As String, _
                                            ByVal inSystemId As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inDropNowDate.ToString(CultureInfo.CurrentCulture)))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_213")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine("UPDATE /* TABLETSMBCOMMONCLASS_213 */ ")
                sql.AppendLine("       TB_T_CARWASH_RESULT ")
                sql.AppendLine("   SET RSLT_END_DATETIME = :RSLT_END_DATETIME ")
                sql.AppendLine("   ,   ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                sql.AppendLine("   ,   ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("   ,   ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                sql.AppendLine("   ,   ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                sql.AppendLine(" WHERE SVCIN_ID = :SVCIN_ID ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, inDropNowDate)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)

                '条件
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function
#End Region

#Region "洗車実績登録"
        ''' <summary>
        ''' 洗車実績テーブル：「洗車開始」新規登録
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        '''  <param name="inCarWashRsltId">洗車実績ID</param>
        ''' <param name="inDropNowDate">現在日時(秒切り捨て)</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inSystemId">プログラムID</param>
        ''' <param name="inAccount">スタフ</param>
        ''' <param name="inDefaultDateTimeValue">デフォルト日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCarWashResult(ByVal inServiceInId As Decimal, _
                                            ByVal inCarWashRsltId As Decimal, _
                                            ByVal inDropNowDate As Date, _
                                            ByVal inNowDate As Date, _
                                            ByVal inSystemId As String, _
                                            ByVal inAccount As String, _
                                            ByVal inDefaultDateTimeValue As Date) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3} P3:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inCarWashRsltId.ToString(CultureInfo.CurrentCulture) _
                        , inDropNowDate.ToString(CultureInfo.CurrentCulture)))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_302")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_302 */ ")
                sql.AppendLine("   INTO TB_T_CARWASH_RESULT ")
                sql.AppendLine("      ( CARWASH_RSLT_ID ")
                sql.AppendLine("      , SVCIN_ID ")
                sql.AppendLine("      , RSLT_START_DATETIME ")
                sql.AppendLine("      , RSLT_END_DATETIME ")
                sql.AppendLine("      , ROW_CREATE_DATETIME ")
                sql.AppendLine("      , ROW_CREATE_ACCOUNT ")
                sql.AppendLine("      , ROW_CREATE_FUNCTION ")
                sql.AppendLine("      , ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , ROW_LOCK_VERSION ")
                sql.AppendLine("      ) ")
                sql.AppendLine(" VALUES ")
                sql.AppendLine("      ( :CARWASH_RSLT_ID ")
                sql.AppendLine("      , :SVCIN_ID ")
                sql.AppendLine("      , :RSLT_START_DATETIME ")
                sql.AppendLine("      , :RSLT_END_DATETIME ")
                sql.AppendLine("      , :ROW_CREATE_DATETIME ")
                sql.AppendLine("      , :ROW_CREATE_ACCOUNT ")
                sql.AppendLine("      , :ROW_CREATE_FUNCTION ")
                sql.AppendLine("      , :ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , :ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , :ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , :ROW_LOCK_VERSION ")
                sql.AppendLine("      ) ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                '値
                query.AddParameterWithTypeValue("CARWASH_RSLT_ID", OracleDbType.Decimal, inCarWashRsltId)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, inDropNowDate)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, inDefaultDateTimeValue)
                query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, 0)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function
#End Region
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
#Region "洗車実績削除テーブルに登録"
        ''' <summary>
        ''' 洗車実績削除テーブル：登録
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inAccount">スタフコード</param>
        ''' <param name="inSystemId">プログラムID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCarWashResultDel(ByVal inServiceInId As Decimal, _
                                            ByVal inNowDate As Date, _
                                            ByVal inAccount As String, _
                                            ByVal inSystemId As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inNowDate.ToString(CultureInfo.CurrentCulture) _
                        , inAccount _
                        , inSystemId))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_305")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_305 */ ")
                sql.AppendLine("   INTO TB_T_CARWASH_RESULT_DEL ")
                sql.AppendLine("      ( CARWASH_RSLT_ID ")
                sql.AppendLine("      , SVCIN_ID ")
                sql.AppendLine("      , RSLT_START_DATETIME ")
                sql.AppendLine("      , RSLT_END_DATETIME ")
                sql.AppendLine("      , ROW_CREATE_DATETIME ")
                sql.AppendLine("      , ROW_CREATE_ACCOUNT ")
                sql.AppendLine("      , ROW_CREATE_FUNCTION ")
                sql.AppendLine("      , ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , ROW_LOCK_VERSION ")
                sql.AppendLine("      ) ")
                sql.AppendLine(" SELECT ")
                sql.AppendLine("      CARWASH_RSLT_ID ")
                sql.AppendLine("      , SVCIN_ID ")
                sql.AppendLine("      , RSLT_START_DATETIME ")
                sql.AppendLine("      , RSLT_END_DATETIME ")
                sql.AppendLine("      , :ROW_CREATE_DATETIME AS ROW_CREATE_DATETIME ")
                sql.AppendLine("      , :ROW_CREATE_ACCOUNT AS ROW_CREATE_ACCOUNT ")
                sql.AppendLine("      , :ROW_CREATE_FUNCTION AS ROW_CREATE_FUNCTION ")
                sql.AppendLine("      , :ROW_UPDATE_DATETIME AS ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , :ROW_UPDATE_ACCOUNT AS ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , :ROW_UPDATE_FUNCTION AS ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , :ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")
                sql.AppendLine(" FROM      ")
                sql.AppendLine("      TB_T_CARWASH_RESULT T1    ")
                sql.AppendLine(" WHERE      ")
                sql.AppendLine("      T1.SVCIN_ID=:SVCIN_ID     ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, 0)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function
#End Region
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "納車処理"

        ''' <summary>
        ''' ササービス入庫テーブル：「納車」更新
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inDropNowDate">現在日時(秒切り捨て)</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateServiceinDelivery(ByVal inServiceInId As Decimal, _
                                                  ByVal inAccount As String, _
                                                  ByVal inNowDate As Date, _
                                                  ByVal inDropNowDate As Date) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inAccount _
                        , inNowDate.ToString(CultureInfo.CurrentCulture) _
                        , inDropNowDate.ToString(CultureInfo.CurrentCulture)))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_214")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_214 */ ")
                sql.AppendLine("        TB_T_SERVICEIN ")
                sql.AppendLine("    SET SVC_STATUS = :SVC_STATUS ")
                sql.AppendLine("      , RSLT_DELI_DATETIME = :RSLT_DELI_DATETIME ")
                sql.AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                sql.AppendLine("    AND SVC_STATUS IN (N'11',N'12') ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                '値
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, "13")
                query.AddParameterWithTypeValue("RSLT_DELI_DATETIME", OracleDbType.Date, inDropNowDate)

                '条件
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function

#End Region
        ' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
#Region "洗車へ移動処理"
        ''' <summary>
        ''' サービス入庫テーブル：「サービスステータス」を「07」に更新
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateServiceinMoveToWash(ByVal inServiceInId As Decimal) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture)))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_228")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_228 */ ")
                sql.AppendLine("        TB_T_SERVICEIN ")
                sql.AppendLine("    SET SVC_STATUS = :SVC_STATUS_WASHWAIT ")
                sql.AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                sql.AppendLine("    AND CARWASH_NEED_FLG = :CARWASH_NEED_FLG ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                '値
                query.AddParameterWithTypeValue("SVC_STATUS_WASHWAIT", OracleDbType.NVarchar2, "07")

                '条件
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG", OracleDbType.NVarchar2, "1")

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function

        ''' <summary>
        ''' 洗車実績テーブル：DELETE
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteCarWashResult(ByVal inServiceInId As Decimal) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture)))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_405")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" DELETE /* TABLETSMBCOMMONCLASS_405 */ ")
                sql.AppendLine(" FROM    TB_T_CARWASH_RESULT ")
                sql.AppendLine(" WHERE SVCIN_ID = :SVCIN_ID ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                '条件
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function

#End Region
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
#Region "キャンセル処理"
        ''' <summary>
        ''' ストール非稼動マスタ テーブル：ストール使用不可を削除に更新
        ''' </summary>
        ''' <param name="stallIdleId">ストール非稼動ID</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateDeleteStallUnavailable(ByVal stallIdleId As Decimal, _
                                                  ByVal inAccount As String, _
                                                  ByVal inNowDate As Date, _
                                                  ByVal rowLockVersion As Long, _
                                                  ByVal systemId As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} START. STALL_IDLE_ID={2}, UPDATE_STF_CD={3}, UPDATE_DATETIME={4}" _
                        , Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name, stallIdleId, inAccount, inNowDate))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_215")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_215 */ ")
                sql.AppendLine("        TB_M_STALL_IDLE ")
                sql.AppendLine("    SET CANCEL_FLG = N'1' ")
                sql.AppendLine("      , UPDATE_DATETIME = :UPDATE_DATETIME ")
                sql.AppendLine("      , UPDATE_STF_CD = :UPDATE_STF_CD ")
                sql.AppendLine("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                sql.AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                sql.AppendLine("      , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                sql.AppendLine("  WHERE STALL_IDLE_ID = :STALL_IDLE_ID ")
                sql.AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                '値
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowLockVersion)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)

                '条件
                query.AddParameterWithTypeValue("STALL_IDLE_ID", OracleDbType.Decimal, stallIdleId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} END " _
                            , Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function

#End Region

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "Undo処理"
        ''' <summary>
        ''' 指定したサービス入庫IDのチップ操作履歴を取得する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <param name="inJobDtlId">作業詳細ID</param>
        ''' <returns>チップ履歴情報テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetChipHis(ByVal inSvcinId As Decimal, _
                                   ByVal inJobDtlId As Decimal) As TabletSmbCommonClassChipHisDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}, jobDtlId={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inSvcinId, inJobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_042 */ ")
                .AppendLine("        CHIP_HIS_ID  ")
                .AppendLine("       ,SVCIN_ID  ")
                .AppendLine("       ,JOB_DTL_ID  ")
                .AppendLine("       ,STALL_USE_ID  ")
                .AppendLine("       ,STALL_ID  ")
                .AppendLine("       ,RESV_STATUS    ")
                .AppendLine("       ,SVC_STATUS    ")
                .AppendLine("       ,SCHE_START_DATETIME    ")
                .AppendLine("       ,SCHE_END_DATETIME    ")
                .AppendLine("       ,SCHE_WORKTIME    ")
                .AppendLine("   FROM  TB_T_CHIP_HIS ")
                .AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                .AppendLine("    AND JOB_DTL_ID = :JOB_DTL_ID ")
                .AppendLine("    AND CANCEL_FLG = N'0'  ")
                .AppendLine("    AND SVC_STATUS <> N'05' ")
                .AppendLine("   ORDER BY CHIP_HIS_ID DESC ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassChipHisDataTable)("TABLETSMBCOMMONCLASS_042")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function


        ''' <summary>
        ''' 作業中チップの履歴を取得する(前回のみ)
        ''' </summary>
        ''' <param name="inStallUseIdList">サービス入庫ID</param>
        ''' <returns>チップ履歴情報テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetWorkingChipHis(ByVal inStallUseIdList As String) As TabletSmbCommonClassChipHisDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallUseIdList={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inStallUseIdList))

            '引数チェック
            If (String.IsNullOrEmpty(inStallUseIdList)) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Count=0", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return New TabletSmbCommonClassChipHisDataTable
            End If

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_044 */ ")
                .AppendLine("        T1.CHIP_HIS_ID  ")
                .AppendLine("       ,T1.SVCIN_ID  ")
                .AppendLine("       ,T1.JOB_DTL_ID  ")
                .AppendLine("       ,T1.STALL_USE_ID  ")
                .AppendLine("       ,T1.STALL_ID  ")
                .AppendLine("       ,T1.RESV_STATUS    ")
                .AppendLine("       ,T1.SVC_STATUS    ")
                .AppendLine("       ,T1.SCHE_START_DATETIME    ")
                .AppendLine("       ,T1.SCHE_END_DATETIME    ")
                .AppendLine("       ,T1.SCHE_WORKTIME    ")
                .AppendLine("   FROM  TB_T_CHIP_HIS T1 ")
                .AppendLine("       , TB_T_STALL_USE T2 ")
                .AppendLine("  WHERE T1.STALL_USE_ID = T2.STALL_USE_ID ")

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                '.AppendLine("    AND T2.STALL_USE_STATUS = N'02'  ")                  '今が作業中の場合
                .AppendLine("    AND T2.STALL_USE_STATUS IN (N'02', N'04')  ")            '今が作業中、一部作業中断の場合
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                .AppendLine("    AND T1.CANCEL_FLG = N'0'  ")
                .AppendLine("    AND T1.CHIP_HIS_ID IN ( ")
                .AppendLine("                           SELECT MAX(S1.CHIP_HIS_ID) ") '該チップの最新の１つ履歴
                .AppendLine("                            FROM  TB_T_CHIP_HIS S1 ")
                .AppendLine("                           WHERE S1.STALL_USE_ID IN ( ")
                .AppendLine(inStallUseIdList)
                .AppendLine("                                                    ) ")
                .AppendLine("                           GROUP BY S1.STALL_USE_ID  ")  'チップごとに最新の１つ履歴
                .AppendLine("                          ) ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassChipHisDataTable)("TABLETSMBCOMMONCLASS_044")
                query.CommandText = sql.ToString()
                Dim returnTable As TabletSmbCommonClassChipHisDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, returnTable.Count))
                Return returnTable
            End Using

        End Function

        '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 START
        ''' <summary>
        ''' 作業中チップの履歴を取得する(前回のみ) ※営業時間より取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallStartTime">稼働時間From</param>
        ''' <param name="stallEndTime">稼働時間To</param>
        ''' <returns>チップ履歴情報テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetWorkingChipHisFromStallTime(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal stallStartTime As Date, _
                                          ByVal stallEndTime As Date _
                                         ) As TabletSmbCommonClassChipHisDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, stallStartTime={3}, stallEndTime={4}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, stallStartTime, stallEndTime))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_044 */ ")
                .AppendLine("         T1.CHIP_HIS_ID ")
                .AppendLine("        ,T1.SVCIN_ID ")
                .AppendLine("        ,T1.JOB_DTL_ID ")
                .AppendLine("        ,T1.STALL_USE_ID ")
                .AppendLine("        ,T1.STALL_ID ")
                .AppendLine("        ,T1.RESV_STATUS ")
                .AppendLine("        ,T1.SVC_STATUS ")
                .AppendLine("        ,T1.SCHE_START_DATETIME ")
                .AppendLine("        ,T1.SCHE_END_DATETIME ")
                .AppendLine("        ,T1.SCHE_WORKTIME ")
                .AppendLine("    FROM TB_T_CHIP_HIS T1 ")
                .AppendLine("   WHERE T1.CHIP_HIS_ID IN ( ")
                .AppendLine("         SELECT MAX(S1.CHIP_HIS_ID) ") '該当チップの最新の１つ履歴
                .AppendLine("           FROM TB_T_CHIP_HIS S1 ")
                .AppendLine("          WHERE S1.STALL_USE_ID IN ( ")
                .AppendLine("             SELECT ")
                .AppendLine("                   S2.STALL_USE_ID ")
                .AppendLine("              FROM TB_T_STALL_USE S2 ")
                .AppendLine("             WHERE S2.DLR_CD = :DLR_CD ")
                .AppendLine("               AND S2.BRN_CD = :BRN_CD ")
                .AppendLine("               AND S2.RSLT_START_DATETIME < TO_DATE(:ENDDATE,'YYYYMMDDHH24MISS') ")
                .AppendLine("               AND S2.PRMS_END_DATETIME > TO_DATE(:STARTDATE,'YYYYMMDDHH24MISS') ")
                .AppendLine("               AND S2.STALL_USE_STATUS IN (N'02', N'04') ")    '今が作業中、一部作業中断の場合
                .AppendLine("           ) ")
                .AppendLine("          GROUP BY ")
                .AppendLine("                S1.STALL_USE_ID ") 'チップごとに最新の１つ履歴
                .AppendLine("     ) ")
                .AppendLine("     AND T1.CANCEL_FLG = N'0' ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassChipHisDataTable)("TABLETSMBCOMMONCLASS_044")
                query.CommandText = sql.ToString()

                'バインド設定
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STARTDATE", OracleDbType.NVarchar2, stallStartTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("ENDDATE", OracleDbType.NVarchar2, stallEndTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture))

                Dim returnTable As TabletSmbCommonClassChipHisDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, returnTable.Count))
                Return returnTable
            End Using

        End Function
        '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 END

        ''' <summary>
        ''' 関連チップ中に実績チップの作業内容IDを取得する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <returns>実績チップの作業内容ID</returns>
        ''' <remarks></remarks>
        Public Function GetRsltStallUses(ByVal inSvcinId As Decimal) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inSvcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_043 */ ")
                .AppendLine("        DISTINCT(T1.JOB_DTL_ID) COL1  ")
                .AppendLine("   FROM  TB_T_JOB_DTL T1  ")
                .AppendLine("       , TB_T_STALL_USE T2  ")
                .AppendLine("  WHERE T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .AppendLine("    AND T1.SVCIN_ID = :SVCIN_ID ")
                .AppendLine("    AND T1.CANCEL_FLG <> N'1'  ")                        'キャンセルしてない
                .AppendLine("    AND T2.STALL_USE_STATUS IN (N'03', N'05', N'06')  ") '中断、終了、日跨ぎ終了
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_043")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' jobidにより、スタッフジョブテーブルからレコードを削除する
        ''' </summary>
        ''' <param name="inJobidList">jobidリスト</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeleteStaffJobByJobid(ByVal inJobidList As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. jobidList={1} " _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inJobidList))

            '引数チェック
            If (String.IsNullOrEmpty(inJobidList)) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. queryCount=0", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return 0
            End If

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" DELETE /* TABLETSMBCOMMONCLASS_401 */ ")
                .AppendLine("   FROM  TB_T_STAFF_JOB  ")
                .AppendLine("  WHERE  ")
                .AppendLine("    JOB_ID IN (  ")
                .AppendLine(inJobidList)
                .AppendLine("              )  ")
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_401")
                query.CommandText = sql.ToString()
                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        ''' <summary>
        ''' Undo操作でサービス入庫テーブルの更新
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <param name="inSvcStatus">サービスステータス</param>
        ''' <param name="inResvStatus">予約ステータス</param>
        ''' <param name="inUpdateDatetime">更新日時</param>
        ''' <param name="inStaffCode">スタッフコード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateSvcinTblForUndo(ByVal inSvcinId As Decimal, _
                                              ByVal inSvcStatus As String, _
                                              ByVal inResvStatus As String, _
                                              ByVal inUpdateDatetime As Date, _
                                              ByVal inStaffCode As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}, svcStatus={2}, resvStatus={3}, updateDatetime={4}, staffCode={5}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inSvcinId, inSvcStatus, inResvStatus, inUpdateDatetime, inStaffCode))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_220")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_220 */ ")
                    .AppendLine("        TB_T_SERVICEIN ")
                    .AppendLine("    SET ")
                    .AppendLine("        SVC_STATUS = :SVC_STATUS ")
                    .AppendLine("       ,RESV_STATUS = :RESV_STATUS ")
                    .AppendLine("       ,UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("       ,UPDATE_STF_CD = :UPDATE_STF_CD ")
                    .AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, inSvcStatus)
                query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, inResvStatus)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDatetime)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inStaffCode)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START

        ' ''' <summary>
        ' ''' Undo操作でストール利用テーブルの更新
        ' ''' </summary>
        ' ''' <param name="inStallUseId">ストール利用ID</param>
        ' ''' <param name="inStallUseStatus">ストール利用ステータス</param>
        ' ''' <param name="inScheWorktime">予定作業時間</param>
        ' ''' <param name="inDefaultDatetime">ディフォルト日時</param>
        ' ''' <param name="inUpdateDatetime">更新日時</param>
        ' ''' <param name="inStaffCode">スタッフコード</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function UpdateStallUseTblForUndo(ByVal inStallUseId As Decimal, _
        '                                         ByVal inStallUseStatus As String, _
        '                                         ByVal inScheWorktime As Long, _
        '                                         ByVal inDefaultDatetime As Date, _
        '                                         ByVal inUpdateDatetime As Date, _
        '                                         ByVal inStaffCode As String) As Long
        ''' <summary>
        ''' Undo操作でストール利用テーブルの更新
        ''' </summary>
        ''' <param name="inStallUseId">ストール利用ID</param>
        ''' <param name="inStallUseStatus">ストール利用ステータス</param>
        ''' <param name="inScheWorktime">予定作業時間</param>
        ''' <param name="inDefaultDatetime">ディフォルト日時</param>
        ''' <param name="inRestFlg">休憩取得フラグ</param>
        ''' <param name="inUpdateDatetime">更新日時</param>
        ''' <param name="inStaffCode">スタッフコード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateStallUseTblForUndo(ByVal inStallUseId As Decimal, _
                                                 ByVal inStallUseStatus As String, _
                                                 ByVal inScheWorktime As Long, _
                                                 ByVal inDefaultDatetime As Date, _
                                                 ByVal inRestFlg As String, _
                                                 ByVal inUpdateDatetime As Date, _
                                                 ByVal inStaffCode As String) As Long

            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallUseId={1}, stallUseStatus={2}, scheWorktime={3}, updateDatetime={4}, staffCode={5}" _
            '                        , System.Reflection.MethodBase.GetCurrentMethod.Name, inStallUseId, inStallUseStatus, inScheWorktime, inUpdateDatetime, inStaffCode))
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallUseId={1}, stallUseStatus={2}, scheWorktime={3}, restFlg={4}, updateDatetime={5}, staffCode={6}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inStallUseId, inStallUseStatus, inScheWorktime, inRestFlg, inUpdateDatetime, inStaffCode))
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_221")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_221 */ ")
                    .AppendLine("        TB_T_STALL_USE ")
                    .AppendLine("    SET ")
                    .AppendLine("        STALL_USE_STATUS = :STALL_USE_STATUS ")
                    .AppendLine("       ,RSLT_START_DATETIME = :RSLT_START_DATETIME ")
                    .AppendLine("       ,PRMS_END_DATETIME = :PRMS_END_DATETIME ")
                    If inScheWorktime > 0 Then
                        .AppendLine("       ,SCHE_WORKTIME = :SCHE_WORKTIME ")
                    End If
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    .AppendLine("       ,REST_FLG = :REST_FLG ")
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                    .AppendLine("       ,JOB_ID = 0 ")
                    .AppendLine("       ,UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("       ,UPDATE_STF_CD = :UPDATE_STF_CD ")
                    .AppendLine("  WHERE STALL_USE_ID = :STALL_USE_ID ")
                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStallUseId)
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, inStallUseStatus)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, inDefaultDatetime)
                query.AddParameterWithTypeValue("PRMS_END_DATETIME", OracleDbType.Date, inDefaultDatetime)
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, inRestFlg)
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDatetime)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inStaffCode)
                If inScheWorktime > 0 Then
                    query.AddParameterWithTypeValue("SCHE_WORKTIME", OracleDbType.Long, inScheWorktime)
                End If

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        ''' <summary>
        ''' 該サービスIDを持てるレコードを削除する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <returns>更新レコード数</returns>
        ''' <remarks></remarks>
        Public Function DeleteCarWashResultByServiceId(ByVal inSvcinId As Decimal) As Long
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1} " _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, inSvcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" DELETE /* TABLETSMBCOMMONCLASS_402 */ ")
                .AppendLine("   FROM  TB_T_CARWASH_RESULT  ")
                .AppendLine("  WHERE  ")
                .AppendLine("         SVCIN_ID = :SVCIN_ID  ")
                .AppendLine("    AND  RSLT_END_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS')  ")
                .AppendLine("    AND  RSLT_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS')  ")
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_402")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.NVarchar2, inSvcinId)
                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using
        End Function

        ''' <summary>
        ''' CarWashResultDelテーブルに一行挿入する
        ''' </summary>
        ''' <param name="inCarWashRsultRow">挿入データ</param>
        ''' <returns>挿入個数</returns>
        ''' <remarks></remarks>
        Public Function InsertCarWashResultDel(ByVal inCarWashRsultRow As TabletSmbCommonClassCarWashRsultRow) As Long

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_305")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_305 */ ")
                sql.AppendLine("   INTO TB_T_CARWASH_RESULT_DEL ")
                sql.AppendLine("      ( CARWASH_RSLT_ID ")
                sql.AppendLine("      , SVCIN_ID ")
                sql.AppendLine("      , RSLT_START_DATETIME ")
                sql.AppendLine("      , RSLT_END_DATETIME ")
                sql.AppendLine("      , ROW_CREATE_DATETIME ")
                sql.AppendLine("      , ROW_CREATE_ACCOUNT ")
                sql.AppendLine("      , ROW_CREATE_FUNCTION ")
                sql.AppendLine("      , ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , ROW_LOCK_VERSION ")
                sql.AppendLine("      ) ")
                sql.AppendLine(" VALUES ")
                sql.AppendLine("      ( :CARWASH_RSLT_ID ")
                sql.AppendLine("      , :SVCIN_ID ")
                sql.AppendLine("      , :RSLT_START_DATETIME ")
                sql.AppendLine("      , :RSLT_END_DATETIME ")
                sql.AppendLine("      , :ROW_CREATE_DATETIME ")
                sql.AppendLine("      , :ROW_CREATE_ACCOUNT ")
                sql.AppendLine("      , :ROW_CREATE_FUNCTION ")
                sql.AppendLine("      , :ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , :ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , :ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , :ROW_LOCK_VERSION ")
                sql.AppendLine("      ) ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                '値
                query.AddParameterWithTypeValue("CARWASH_RSLT_ID", OracleDbType.Decimal, inCarWashRsultRow.CARWASH_RSLT_ID)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inCarWashRsultRow.SVCIN_ID)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, inCarWashRsultRow.RSLT_START_DATETIME)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, inCarWashRsultRow.RSLT_END_DATETIME)
                query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, inCarWashRsultRow.ROW_CREATE_DATETIME)
                query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, inCarWashRsultRow.ROW_CREATE_ACCOUNT)
                query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, inCarWashRsultRow.ROW_CREATE_FUNCTION)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inCarWashRsultRow.ROW_UPDATE_DATETIME)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inCarWashRsultRow.ROW_UPDATE_ACCOUNT)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inCarWashRsultRow.ROW_UPDATE_FUNCTION)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, inCarWashRsultRow.ROW_LOCK_VERSION)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using
        End Function

        ''' <summary>
        ''' CarWashResultテーブルから指定サービス入庫IDの洗車中レコードを取得する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <returns>指定サービス入庫IDの洗車中レコード</returns>
        ''' <remarks></remarks>
        Public Function GetCarWashResultBySvcinId(ByVal inSvcinId As Decimal) As TabletSmbCommonClassCarWashRsultDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inSvcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_045 */ ")
                .AppendLine("        CARWASH_RSLT_ID ")
                .AppendLine("      , SVCIN_ID ")
                .AppendLine("      , RSLT_START_DATETIME ")
                .AppendLine("      , RSLT_END_DATETIME ")
                .AppendLine("      , ROW_CREATE_DATETIME ")
                .AppendLine("      , ROW_CREATE_ACCOUNT ")
                .AppendLine("      , ROW_CREATE_FUNCTION ")
                .AppendLine("      , ROW_UPDATE_DATETIME ")
                .AppendLine("      , ROW_UPDATE_ACCOUNT ")
                .AppendLine("      , ROW_UPDATE_FUNCTION ")
                .AppendLine("      , ROW_LOCK_VERSION ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_CARWASH_RESULT ")
                .AppendLine("  WHERE  ")
                .AppendLine("         SVCIN_ID = :SVCIN_ID  ")
                .AppendLine("    AND  RSLT_END_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS')  ")
                .AppendLine("    AND  RSLT_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS')  ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassCarWashRsultDataTable)("TABLETSMBCOMMONCLASS_045")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)

                'SQL実行(影響行数を返却)
                Dim returnTable As TabletSmbCommonClassCarWashRsultDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, returnTable.Count))
                Return returnTable
            End Using

        End Function

#End Region
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#Region "遅れ見込み時刻を計算する"
        ''' <summary>
        ''' 遅れ見込み時刻計算用の残作業時間と最後終了時間を取得する
        ''' </summary>
        ''' <param name="svcinIdList">サービス入庫ID</param>
        ''' <param name="dlrCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <returns>ストール利用ステータスリスト</returns>
        ''' <remarks></remarks>
        Public Function GetAllMaxEndDateAndRemainingTime(ByVal svcinIdList As List(Of Decimal), ByVal dlrCode As String, ByVal brnCode As String) As TabletSmbCommonClassMaxEndDateInfoDataTable

            'ストールIDを「svcinId1,svcinId2,…svcinIdN」のstringに変更する
            Dim sbSvcinId As New StringBuilder
            Dim strStallList As String = ""
            'ストールIDがない場合、空白テーブルを戻す
            If IsNothing(svcinIdList) OrElse svcinIdList.Count = 0 Then
                Return New TabletSmbCommonClassMaxEndDateInfoDataTable
            Else
                For Each stallId As String In svcinIdList
                    sbSvcinId.Append(stallId)
                    sbSvcinId.Append(",")
                Next
                strStallList = sbSvcinId.ToString()
                '最後のコンマを削除する
                strStallList = strStallList.Substring(0, strStallList.Length - 1)
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinIdList={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinIdList))
            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* TABLETSMBCOMMONCLASS_018 */ ")
                .AppendLine("          T1.SVCIN_ID ")
                .AppendLine("        , T2.JOB_DTL_ID ")
                .AppendLine("        , T3.STALL_USE_ID ")
                .AppendLine("        , T3.RSLT_START_DATETIME ")
                .AppendLine("        , T3.RSLT_END_DATETIME ")
                .AppendLine("        , T3.PRMS_END_DATETIME ")
                .AppendLine("        , T3.SCHE_START_DATETIME ")
                .AppendLine("        , T3.SCHE_END_DATETIME ")
                .AppendLine("        , T3.SCHE_WORKTIME ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("        , TB_T_JOB_DTL T2 ")
                .AppendLine("        , TB_T_STALL_USE T3 ")
                .AppendLine("    WHERE ")
                .AppendLine("          T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("      AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("      AND T2.CANCEL_FLG = N'0' ")
                .AppendLine("      AND T3.STALL_USE_ID IN ( ")
                .AppendLine("                                 SELECT MAX(S3.STALL_USE_ID) ")
                .AppendLine("                                   FROM ")
                .AppendLine("                                        TB_T_SERVICEIN S1 ")
                .AppendLine("                                      , TB_T_JOB_DTL S2 ")
                .AppendLine("                                      , TB_T_STALL_USE S3 ")
                .AppendLine("                                  WHERE ")
                .AppendLine("                                        S1.SVCIN_ID = S2.SVCIN_ID ")
                .AppendLine("                                    AND S2.JOB_DTL_ID = S3.JOB_DTL_ID ")
                .AppendLine("                                    AND S3.DLR_CD = :DLR_CD ")
                .AppendLine("                                    AND S3.BRN_CD = :BRN_CD ")
                .AppendLine("                                    AND S1.RSLT_DELI_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                .AppendLine("                                    AND S1.SVCIN_ID IN ( ")
                .AppendLine(strStallList)
                .AppendLine("                                                       ) ")
                .AppendLine("                               GROUP BY S2.JOB_DTL_ID ")
                .AppendLine("                             ) ")
                .AppendLine(" ORDER BY T1.SVCIN_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassMaxEndDateInfoDataTable)("TABLETSMBCOMMONCLASS_018")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCode)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "履歴登録するか否かを判断する情報取得"

        ''' <summary>
        ''' 履歴登録するか否かを判断する情報取得
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>履歴登録するか否かを判断する情報</returns>
        ''' <remarks></remarks>
        Public Function GetChipChangeInfo(ByVal inServiceInId As Decimal, _
                                          ByVal inDlrCode As String, _
                                          ByVal inBrnCode As String) As TabletSmbCommonClassServiceinChangeInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inDlrCode _
                        , inBrnCode))

            ''SQLの設定
            Dim sql As New StringBuilder
            With sql
                .Append("   SELECT /* TABLETSMBCOMMONCLASS_019 */")
                .Append("          T1.DLR_CD ")
                .Append("        , T1.BRN_CD ")
                .Append("        , T1.SVCIN_ID ")
                .Append("        , T1.PICK_DELI_TYPE ")
                .Append("        , T1.CARWASH_NEED_FLG ")
                .Append("        , T1.RESV_STATUS ")
                .Append("        , T1.SVC_STATUS ")
                .Append("        , T1.SCHE_SVCIN_DATETIME ")
                .Append("        , T1.SCHE_DELI_DATETIME ")
                .Append("        , T1.RSLT_SVCIN_DATETIME  ")
                .Append("        , T2.PICK_PREF_DATETIME ")
                .Append("        , T3.DELI_PREF_DATETIME  ")
                .Append("        , T4.JOB_DTL_ID  ")
                .Append("        , T4.SVC_CLASS_ID ")
                .Append("        , T4.MERC_ID ")
                .Append("        , T4.INSPECTION_NEED_FLG ")
                .Append("        , T4.CANCEL_FLG ")
                .Append("        , T5.STALL_USE_ID ")
                .Append("        , T5.STALL_ID ")
                .Append("        , T5.TEMP_FLG ")
                .Append("        , T5.SCHE_START_DATETIME ")
                .Append("        , T5.SCHE_END_DATETIME ")
                .Append("        , T5.SCHE_WORKTIME ")
                .Append("        , T5.RSLT_START_DATETIME ")
                .Append("        , T5.STALL_USE_STATUS ")
                .Append("     FROM ")
                .Append("          TB_T_SERVICEIN T1 ")
                .Append("        , TB_T_VEHICLE_PICKUP T2 ")
                .Append("        , TB_T_VEHICLE_DELIVERY T3 ")
                .Append("        , TB_T_JOB_DTL T4 ")
                .Append("        , TB_T_STALL_USE T5 ")
                .Append("    WHERE ")
                .Append("          T1.SVCIN_ID = T2.SVCIN_ID(+) ")
                .Append("      AND T1.SVCIN_ID = T3.SVCIN_ID(+) ")
                .Append("      AND T1.SVCIN_ID = T4.SVCIN_ID ")
                .Append("      AND T4.JOB_DTL_ID = T5.JOB_DTL_ID ")
                .Append("      AND T1.SVCIN_ID = :SVCIN_ID ")
                .Append("      AND T1.DLR_CD = :DLRCD ")
                .Append("      AND T1.BRN_CD = :STRCD ")
                .Append("      AND T5.DLR_CD = :DLRCD ")
                .Append("      AND T5.BRN_CD = :STRCD ")
                .Append("      AND T5.STALL_USE_ID = ( ")
                .Append("                                 SELECT ")
                .Append("                                       MAX(STALL_USE_ID) ")
                .Append("                                   FROM ")
                .Append("                                       TB_T_STALL_USE T6 ")
                .Append("                                  WHERE ")
                .Append("                                       T6.JOB_DTL_ID = T4.JOB_DTL_ID ")
                .Append("                            ) ")
                .Append(" ORDER BY ")
                .Append("         JOB_DTL_ID DESC ")
            End With

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of TabletSmbCommonClassServiceinChangeInfoDataTable)("TABLETSMBCOMMONCLASS_019")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)               ' サービス入庫ID
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDlrCode)                   ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBrnCode)                   ' 店舗コード

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "ストール利用テーブルに1行を挿入"
        ''' <summary>
        ''' ストール利用テーブルに1行を挿入する
        ''' </summary>
        ''' <param name="chipEntity">チップエンティティ</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function InsertStallUse(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, ByVal systemId As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} START ." _
                        , Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))


            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_303 */ ")
                .AppendLine("   INTO TB_T_STALL_USE ")
                .AppendLine("      ( STALL_USE_ID ")
                .AppendLine("      , JOB_DTL_ID ")
                .AppendLine("      , DLR_CD ")
                .AppendLine("      , BRN_CD ")
                .AppendLine("      , STALL_ID ")
                .AppendLine("      , TEMP_FLG ")
                .AppendLine("      , PARTS_FLG ")
                .AppendLine("      , STALL_USE_STATUS ")
                .AppendLine("      , SCHE_START_DATE ")
                .AppendLine("      , SCHE_START_DATETIME ")
                .AppendLine("      , SCHE_END_DATETIME ")
                .AppendLine("      , SCHE_WORKTIME ")
                .AppendLine("      , REST_FLG ")
                .AppendLine("      , RSLT_START_DATETIME ")
                .AppendLine("      , PRMS_END_DATETIME ")
                .AppendLine("      , RSLT_END_DATETIME ")
                .AppendLine("      , RSLT_WORKTIME ")
                .AppendLine("      , JOB_ID ")
                .AppendLine("      , STOP_REASON_TYPE ")
                .AppendLine("      , STOP_MEMO ")
                .AppendLine("      , STALL_IDLE_ID ")
                .AppendLine("      , CREATE_DATETIME ")
                .AppendLine("      , CREATE_STF_CD ")
                .AppendLine("      , UPDATE_DATETIME ")
                .AppendLine("      , UPDATE_STF_CD ")
                .AppendLine("      , ROW_CREATE_ACCOUNT ")
                .AppendLine("      , ROW_CREATE_FUNCTION ")
                .AppendLine("      , ROW_UPDATE_DATETIME ")
                .AppendLine("      , ROW_UPDATE_ACCOUNT ")
                .AppendLine("      , ROW_UPDATE_FUNCTION ")
                .AppendLine("      , ROW_LOCK_VERSION ")
                .AppendLine("      ) ")
                .AppendLine(" VALUES ")
                .AppendLine("      ( :STALL_USE_ID ")
                .AppendLine("      , :JOB_DTL_ID ")
                .AppendLine("      , :DLR_CD ")
                .AppendLine("      , :BRN_CD ")
                .AppendLine("      , :STALL_ID ")
                .AppendLine("      , :TEMP_FLG ")
                .AppendLine("      , :PARTS_FLG ")
                .AppendLine("      , :STALL_USE_STATUS ")
                .AppendLine("      , :SCHE_START_DATE ")
                .AppendLine("      , :SCHE_START_DATETIME ")
                .AppendLine("      , :SCHE_END_DATETIME ")
                .AppendLine("      , :SCHE_WORKTIME ")
                .AppendLine("      , :REST_FLG ")
                .AppendLine("      , :RSLT_START_DATETIME ")
                .AppendLine("      , :PRMS_END_DATETIME ")
                .AppendLine("      , :RSLT_END_DATETIME ")
                .AppendLine("      , :RSLT_WORKTIME ")
                .AppendLine("      , :JOB_ID ")
                .AppendLine("      , :STOP_REASON_TYPE ")
                .AppendLine("      , :STOP_MEMO ")
                .AppendLine("      , :STALL_IDLE_ID ")
                .AppendLine("      , :UPDATE_DATETIME ")
                .AppendLine("      , :UPDATE_STF_CD ")
                .AppendLine("      , :UPDATE_DATETIME ")
                .AppendLine("      , :UPDATE_STF_CD ")
                .AppendLine("      , :UPDATE_STF_CD ")
                .AppendLine("      , :ROW_FUNCTION ")
                .AppendLine("      , :UPDATE_DATETIME ")
                .AppendLine("      , :UPDATE_STF_CD ")
                .AppendLine("      , :ROW_FUNCTION ")
                .AppendLine("      , 0 ")
                .AppendLine("      ) ")
            End With

            Dim queryCount As Long
            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_303")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, chipEntity.STALL_USE_ID)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, chipEntity.JOB_DTL_ID)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, chipEntity.DLR_CD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, chipEntity.BRN_CD)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, chipEntity.STALL_ID)
                query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, chipEntity.TEMP_FLG)
                query.AddParameterWithTypeValue("PARTS_FLG", OracleDbType.NVarchar2, chipEntity.PARTS_FLG)
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, chipEntity.STALL_USE_STATUS)
                query.AddParameterWithTypeValue("SCHE_START_DATE", OracleDbType.NVarchar2, chipEntity.SCHE_START_DATETIME.ToString("yyyyMMdd", CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, chipEntity.SCHE_START_DATETIME)
                query.AddParameterWithTypeValue("SCHE_END_DATETIME", OracleDbType.Date, chipEntity.SCHE_END_DATETIME)
                query.AddParameterWithTypeValue("SCHE_WORKTIME", OracleDbType.Long, chipEntity.SCHE_WORKTIME)
                query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, chipEntity.REST_FLG)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, chipEntity.RSLT_START_DATETIME)
                query.AddParameterWithTypeValue("PRMS_END_DATETIME", OracleDbType.Date, chipEntity.PRMS_END_DATETIME)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, chipEntity.RSLT_END_DATETIME)
                query.AddParameterWithTypeValue("RSLT_WORKTIME", OracleDbType.Long, chipEntity.RSLT_WORKTIME)
                query.AddParameterWithTypeValue("JOB_ID", OracleDbType.Decimal, chipEntity.JOB_ID)
                query.AddParameterWithTypeValue("STOP_REASON_TYPE", OracleDbType.NVarchar2, chipEntity.STOP_REASON_TYPE)
                query.AddParameterWithTypeValue("STOP_MEMO", OracleDbType.NVarchar2, chipEntity.STOP_MEMO)
                query.AddParameterWithTypeValue("STALL_IDLE_ID", OracleDbType.Decimal, chipEntity.STALL_IDLE_ID)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, chipEntity.UPDATE_DATETIME)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, chipEntity.UPDATE_STF_CD)
                query.AddParameterWithTypeValue("ROW_FUNCTION", OracleDbType.NVarchar2, systemId)

                'SQL実行(影響行数を返却)
                queryCount = query.Execute
            End Using
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} END ", _
                                      Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return queryCount

        End Function
#End Region

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発　START
        '#Region "RO紐付"
#Region "着工指示、着工指示の紐付きの解除"
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発　END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>
        ' '''作業内容テーブル：RO紐付く
        ' ''' </summary>
        ' ''' <param name="inJobDetailId">作業内容ID</param>
        ' ''' <param name="inworkSeq">顧客承認連番</param>
        ' ''' <param name="inNow">更新日時</param>
        ' ''' <param name="inAccount">スタッフコード</param>
        ' ''' <param name="inSystemId">更新機能</param>
        ' ''' <returns>更新件数</returns>
        ' ''' <remarks></remarks>
        'Public Function UpdateJobDtlAttchment(ByVal inJobDetailId As Decimal, _
        '                             ByVal inWorkSeq As Long, _
        '                             ByVal inNow As Date, _
        '                             ByVal inAccount As String, _
        '                             ByVal inSystemId As String) As Long
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START P1:{2} P2:{3}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inJobDetailId.ToString(CultureInfo.CurrentCulture) _
        '                , inWorkSeq))

        '    'DBUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_216")
        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_216 */ ")
        '        sql.AppendLine("        TB_T_JOB_DTL  ")
        '        sql.AppendLine("    SET RO_JOB_SEQ = :RO_JOB_SEQ ")
        '        sql.AppendLine("      , UPDATE_DATETIME = :UPDATE_DATETIME ")
        '        sql.AppendLine("      , UPDATE_STF_CD = :UPDATE_STF_CD ")
        '        sql.AppendLine("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
        '        sql.AppendLine("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
        '        sql.AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '        sql.AppendLine("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
        '        sql.AppendLine(" WHERE ")
        '        sql.AppendLine("       JOB_DTL_ID = :JOB_DTL_ID ")
        '        query.CommandText = sql.ToString()

        '        'SQLパラメータ設定値
        '        query.AddParameterWithTypeValue("RO_JOB_SEQ", OracleDbType.Long, inWorkSeq)
        '        query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inNow)
        '        query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inAccount)
        '        query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNow)
        '        query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)
        '        query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDetailId)

        '        'SQL実行(影響行数を返却)
        '        Dim queryCount As Long = query.Execute
        '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        '        Return queryCount
        '    End Using
        'End Function
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        ''' <summary>
        ''' サービス入庫テーブル：RO紐付く
        ''' </summary>
        ''' <param name="svcinId">ストール入庫ID</param>
        ''' <param name="scheDeliDatetime">予定納車日時</param>
        ''' <param name="status">ステータス</param>
        ''' <param name="dtNow">更新日時</param>
        ''' <param name="stfCode">スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateSvcinUpdateJobDtlAttchment(ByVal svcinId As Decimal, _
                                                         ByVal scheDeliDatetime As Date, _
                                                         ByVal status As String, _
                                                         ByVal dtNow As Date, _
                                                         ByVal stfCode As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}, status={2}, dtNow={3}, stfCode={4}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinId, status, dtNow, stfCode))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_217")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_217 */ ")
                sql.AppendLine("        TB_T_SERVICEIN ")
                sql.AppendLine("    SET SVC_STATUS = :SVC_STATUS ")
                sql.AppendLine("      , SCHE_DELI_DATETIME = :SCHE_DELI_DATETIME ")
                sql.AppendLine("  WHERE SVCIN_ID = :SVCIN_ID ")
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, status)
                query.AddParameterWithTypeValue("SCHE_DELI_DATETIME", OracleDbType.Date, scheDeliDatetime)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using

        End Function
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発　START
        ''' <summary>
        '''作業指示テーブルINSERT：着工指示
        ''' </summary>
        ''' <param name="inJobInstructRow">作業指示データ行</param>
        ''' <param name="inNow">更新日時</param>
        ''' <param name="inAccount">スタッフコード</param>
        ''' <param name="inSystemId">更新機能</param>
        ''' <returns>反映件数</returns>
        ''' <remarks></remarks>
        Public Function InsertJobInstructBinding(ByVal inJobInstructRow As TabletSmbCommonClassJobInstructRow, _
                                                 ByVal inNow As Date, _
                                                 ByVal inAccount As String, _
                                                 ByVal inSystemId As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inNow={2} inAccount={3} inSystemId={4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inNow _
                        , inAccount _
                        , inSystemId))

            Dim queryCount As Long

            'SQL組み立て
            Dim sql As New StringBuilder
            sql.AppendLine(" INSERT INTO /* TABLETSMBCOMMONCLASS_306 */ ")
            sql.AppendLine("        TB_T_JOB_INSTRUCT  ")
            sql.AppendLine(" (                         ")
            sql.AppendLine("        JOB_DTL_ID         ")
            sql.AppendLine("      , JOB_INSTRUCT_ID    ")
            sql.AppendLine("      , JOB_INSTRUCT_SEQ   ")
            sql.AppendLine("      , RO_NUM             ")
            sql.AppendLine("      , RO_SEQ             ")
            sql.AppendLine("      , JOB_CD             ")
            sql.AppendLine("      , JOB_NAME           ")
            sql.AppendLine("      , STD_WORKTIME       ")
            sql.AppendLine("      , JOB_STF_GROUP_ID   ")
            sql.AppendLine("      , JOB_STF_GROUP_NAME ")
            sql.AppendLine("      , STARTWORK_INSTRUCT_FLG ")
            sql.AppendLine("      , OPERATION_TYPE_ID  ")
            sql.AppendLine("      , OPERATION_TYPE_NAME ")
            sql.AppendLine("      , WORK_PRICE         ")
            sql.AppendLine("      , WORK_UNIT_PRICE          ")
            sql.AppendLine("      , ROW_CREATE_DATETIME ")
            sql.AppendLine("      , ROW_CREATE_ACCOUNT ")
            sql.AppendLine("      , ROW_CREATE_FUNCTION ")
            sql.AppendLine("      , ROW_UPDATE_DATETIME ")
            sql.AppendLine("      , ROW_UPDATE_ACCOUNT ")
            sql.AppendLine("      , ROW_UPDATE_FUNCTION ")
            sql.AppendLine("      , ROW_LOCK_VERSION   ")
            sql.AppendLine(" )                         ")
            sql.AppendLine(" VALUES(                   ")
            sql.AppendLine("          :JOB_DTL_ID        ")
            sql.AppendLine("        , :JOB_INSTRUCT_ID ")
            sql.AppendLine("        , :JOB_INSTRUCT_SEQ ")
            sql.AppendLine("        , :RO_NUM ")
            sql.AppendLine("        , :RO_SEQ ")
            sql.AppendLine("        , :JOB_CD ")
            sql.AppendLine("        , :JOB_NAME ")
            sql.AppendLine("        , :STD_WORKTIME ")
            sql.AppendLine("        , :JOB_STF_GROUP_ID ")
            sql.AppendLine("        , :JOB_STF_GROUP_NAME ")
            sql.AppendLine("        , :STARTWORK_INSTRUCT_FLG ")
            sql.AppendLine("        , :OPERATION_TYPE_ID ")
            sql.AppendLine("        , :OPERATION_TYPE_NAME ")
            sql.AppendLine("        , :WORK_PRICE ")
            sql.AppendLine("        , :WORK_UNIT_PRICE ")
            sql.AppendLine("        , :ROW_CREATE_DATETIME ")
            sql.AppendLine("        , :ROW_CREATE_ACCOUNT ")
            sql.AppendLine("        , :ROW_CREATE_FUNCTION ")
            sql.AppendLine("        , :ROW_UPDATE_DATETIME ")
            sql.AppendLine("        , :ROW_UPDATE_ACCOUNT ")
            sql.AppendLine("        , :ROW_UPDATE_FUNCTION ")
            sql.AppendLine("        , :ROW_LOCK_VERSION ")
            sql.AppendLine("  ) ")

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_306")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobInstructRow.JOB_DTL_ID)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inJobInstructRow.JOB_INSTRUCT_ID)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inJobInstructRow.JOB_INSTRUCT_SEQ)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inJobInstructRow.RO_NUM)
                query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Long, inJobInstructRow.RO_JOB_SEQ)
                query.AddParameterWithTypeValue("JOB_CD", OracleDbType.NVarchar2, inJobInstructRow.JOB_CD)
                query.AddParameterWithTypeValue("JOB_NAME", OracleDbType.NVarchar2, inJobInstructRow.JOB_NAME)
                query.AddParameterWithTypeValue("STD_WORKTIME", OracleDbType.Long, inJobInstructRow.STD_WORKTIME)
                query.AddParameterWithTypeValue("JOB_STF_GROUP_ID", OracleDbType.NVarchar2, inJobInstructRow.JOB_STF_GROUP_ID)
                query.AddParameterWithTypeValue("JOB_STF_GROUP_NAME", OracleDbType.NVarchar2, inJobInstructRow.JOB_STF_GROUP_NAME)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, One.ToString(CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("OPERATION_TYPE_ID", OracleDbType.NVarchar2, inJobInstructRow.OPERATION_TYPE_ID)
                query.AddParameterWithTypeValue("OPERATION_TYPE_NAME", OracleDbType.NVarchar2, inJobInstructRow.OPERATION_TYPE_NAME)
                query.AddParameterWithTypeValue("WORK_PRICE", OracleDbType.Long, inJobInstructRow.WORK_PRICE)
                query.AddParameterWithTypeValue("WORK_UNIT_PRICE", OracleDbType.Long, inJobInstructRow.WORK_UNIT_PRICE)
                query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, inNow)
                query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNow)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, Zero)

                'SQL実行(影響行数を返却)
                queryCount = query.Execute
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
            Return queryCount
        End Function

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        ' ''' <summary>
        ' '''RO紐付くため、旧作業指示テーブルデータをDELETE:着工指示
        ' ''' </summary>
        ' ''' <param name="inROJobSeq">RO作業連番</param>
        ' ''' <param name="inRONum">RO番号</param>
        ' ''' <returns>反映件数</returns>
        ' ''' <remarks></remarks>
        'Public Function DeleteJobInstruct(ByVal inROJobSeq As Long, _
        '                                  ByVal inRONum As String) As Long

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START inROJobSeq={2} inRONum={3}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inROJobSeq _
        '                , inRONum))

        '    'DBUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_403")
        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        sql.AppendLine(" DELETE FROM /* TABLETSMBCOMMONCLASS_403 */ ")
        '        sql.AppendLine("        TB_T_JOB_INSTRUCT T1 ")
        '        sql.AppendLine("  WHERE ")
        '        sql.AppendLine("        T1.RO_NUM = :RO_NUM ")
        '        sql.AppendLine("    AND T1.RO_SEQ = :RO_SEQ ")
        '        sql.AppendLine("    AND T1.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_OFF ")

        '        query.CommandText = sql.ToString()

        '        'SQLパラメータ設定値
        '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
        '        query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Long, inROJobSeq)
        '        query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))


        '        'SQL実行(影響行数を返却)
        '        Dim queryCount As Long = query.Execute
        '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
        '        Return queryCount
        '    End Using
        'End Function
        ''' <summary>
        '''RO紐付くため、旧作業指示テーブルデータをDELETE:着工指示
        ''' </summary>
        ''' <param name="inROJobSeq">RO作業連番</param>
        ''' <param name="inRONum">RO番号</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteJobInstruct(ByVal inROJobSeq As Long, _
                                          ByVal inRONum As String, _
                                          ByVal inDealerCode As String, _
                                          ByVal inBranchCode As String) As Long

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inROJobSeq={2} inRONum={3}, inDealerCode={4}, inBranchCode={5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inROJobSeq _
                        , inRONum _
                        , inDealerCode _
                        , inBranchCode))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_403")
                'SQL組み立て
                Dim sql As New StringBuilder

                With sql
                    .AppendLine(" DELETE FROM /* TABLETSMBCOMMONCLASS_403 */ ")
                    .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
                    .AppendLine("  WHERE ")
                    .AppendLine(" 	   (T1.JOB_DTL_ID, T1.JOB_INSTRUCT_ID, T1.JOB_INSTRUCT_SEQ) IN  ")
                    .AppendLine(" 	   ( SELECT ")
                    .AppendLine(" 				T2.JOB_DTL_ID ")
                    .AppendLine("      		  , T2.JOB_INSTRUCT_ID ")
                    .AppendLine("      		  , T2.JOB_INSTRUCT_SEQ ")
                    .AppendLine("   	   FROM ")
                    .AppendLine("        		TB_T_JOB_INSTRUCT T2 ")
                    .AppendLine("      		  , TB_T_JOB_DTL T3 ")

                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                    .AppendLine("      		  , TB_T_STALL_USE T4 ")
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                    .AppendLine(" 		  WHERE ")
                    .AppendLine(" 		        T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .AppendLine(" 		    AND T2.RO_NUM = :RO_NUM ")
                    .AppendLine(" 		    AND T2.RO_SEQ = :RO_SEQ ")
                    .AppendLine(" 		    AND T2.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_OFF ")
                    .AppendLine(" 		    AND T3.DLR_CD = :DLR_CD ")

                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                    '.AppendLine(" 		    AND T3.BRN_CD = :BRN_CD ) ")
                    .AppendLine(" 		    AND T3.BRN_CD = :BRN_CD ")
                    .AppendLine(" 		    AND T3.JOB_DTL_ID = T4.JOB_DTL_ID ")
                    .AppendLine(" 		    AND EXISTS ( ")
                    .AppendLine(" 		               SELECT ")
                    .AppendLine(" 		                      1 ")
                    .AppendLine(" 		                 FROM ")
                    .AppendLine(" 		                      TB_T_STALL_USE T5 ")
                    .AppendLine(" 		                WHERE ")
                    .AppendLine(" 		                      T5.JOB_DTL_ID = T4.JOB_DTL_ID ")
                    .AppendLine(" 		                HAVING ")
                    .AppendLine(" 		                      MAX(T5.STALL_USE_ID) = T4.STALL_USE_ID ) ")
                    .AppendLine(" 		    AND T4.TEMP_FLG = :TEMP_FLG_OFF ) ")
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                    
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
                query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Long, inROJobSeq)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                query.AddParameterWithTypeValue("TEMP_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using
        End Function
        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発　END
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "着工指示の紐付きの解除"
        ''' <summary>
        ''' 指定ストール利用IDのチップ情報を取得する
        ''' </summary>
        ''' <param name="inStallUseIds">ストール利用ID</param>
        ''' <returns></returns>
        Public Function GetInstructedChipInfo(ByVal inStallUseIds As String) As TabletSmbCommonClassCanceledJobInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inStallUseIds={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inStallUseIds))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_057 */ ")
                .AppendLine("        T1.JOB_DTL_ID  ")
                .AppendLine("      , T1.STALL_USE_ID    ")
                .AppendLine("      , T1.STALL_ID  ")
                .AppendLine("      , T1.SCHE_START_DATETIME  ")
                .AppendLine("      , T1.SCHE_END_DATETIME  ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_STALL_USE T1 ")
                .AppendLine(" WHERE  ")
                .AppendLine("       T1.STALL_USE_ID IN ")
                .AppendLine("            ( ")
                .AppendLine(inStallUseIds)
                .AppendLine("            ) ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassCanceledJobInfoDataTable)("TABLETSMBCOMMONCLASS_057")
                query.CommandText = sql.ToString()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function
#End Region
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
#Region "コメントされたソース"
        '#Region "表示商品IDを取得"
        '        ''' <summary>
        '        ''' 表示商品IDを取得
        '        ''' </summary>
        '        ''' <param name="inDlrCode">販売店コード</param>
        '        ''' <param name="inMainteCode">整備コード</param>
        '        ''' <param name="inVclVin">VIN</param>
        '        ''' <returns>表示商品ID</returns>
        '        ''' <remarks></remarks>
        '        Public Function GetMercId(ByVal inDlrCode As String, _
        '                                  ByVal inMainteCode As String, _
        '                                  ByVal inVclVin As String) As Long
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                       , "{0}.{1} START P1:{2} P2:{3} P3:{4}" _
        '                       , Me.GetType.ToString _
        '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                       , inDlrCode _
        '                       , inMainteCode _
        '                       , inVclVin))
        '            Dim mercId As Long
        '            '商品情報を取得する
        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassMercidDataTable)("TABLETSMBCOMMONCLASS_020")
        '                Dim Sql As New StringBuilder
        '                With Sql
        '                    .Append("   SELECT /* TABLETSMBCOMMONCLASS_020 */ ")
        '                    .Append("         T3.MERCHANDISECD AS MERC_ID ")
        '                    .Append("     FROM ( ")
        '                    .Append("               SELECT ")
        '                    .Append("                      T1.MERCHANDISECD ")
        '                    .Append("                    , CASE WHEN T1.BASETYPE = :BASETYPEALL ")
        '                    .Append("                           THEN 1 ")
        '                    .Append("                           ELSE 2 ")
        '                    .Append("                       END AS SORT_NUM ")
        '                    .Append("                 FROM ")
        '                    .Append("                      TBL_MAINTELINK T1 ")
        '                    .Append("                WHERE ")
        '                    .Append("                      T1.DLRCD = :DLRCD ")
        '                    .Append("                  AND T1.MNTNCD = :MAINTE_CD ")
        '                    .Append("                  AND T1.BASETYPE IN ( :BASETYPEALL, ")
        '                    .Append("                                                   ( SELECT ")
        '                    .Append("                                                           SUBSTR(MAX(VCL_KATASHIKI),0,INSTR(MAX(VCL_KATASHIKI), :HYPHEN) -1) ")
        '                    .Append("                                                       FROM ")
        '                    .Append("                                                           TB_M_VEHICLE T2 ")
        '                    .Append("                                                      WHERE ")
        '                    .Append("                                                           T2.VCL_VIN = :VCL_VIN ")
        '                    .Append("                                                           AND T2.DMS_TAKEIN_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
        '                    .Append("                                                   ) ")
        '                    .Append("                                     ) ")
        '                    .Append("          ) T3 ")
        '                    .Append(" ORDER BY SORT_NUM ")
        '                End With
        '                query.CommandText = Sql.ToString()

        '                'SQLパラメータ設定値
        '                ' BASETYPE「*」
        '                query.AddParameterWithTypeValue("BASETYPEALL", OracleDbType.NVarchar2, BaseTypeAll)
        '                '販売店コード
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDlrCode)
        '                '整備コード
        '                query.AddParameterWithTypeValue("MAINTE_CD", OracleDbType.NVarchar2, inMainteCode)
        '                ' 基本型式検索用(ハイフン)
        '                query.AddParameterWithTypeValue("HYPHEN", OracleDbType.NVarchar2, Hyphen)
        '                ' 店舗コード
        '                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, inVclVin)
        '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        '                Dim dt As TabletSmbCommonClassMercidDataTable = query.GetData()
        '                If dt.Count > 0 Then
        '                    mercId = CType(dt.Rows(0).Item("MERC_ID"), Long)
        '                End If
        '            End Using
        '            Return mercId
        '        End Function
        '#End Region

        '#Region "商品情報を取得"
        '        ''' <summary>
        '        ''' 商品情報を取得
        '        ''' </summary>
        '        ''' <param name="inMercId">商品ID</param>
        '        ''' <returns>商品情報</returns>
        '        ''' <remarks></remarks>
        '        Public Function GetServiceClassId(ByVal inMercId As Long) As TabletSmbCommonClassMercinfoDataTable
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '           , "{0}.{1} START P1:{2}" _
        '           , Me.GetType.ToString _
        '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '           , inMercId.ToString(CultureInfo.CurrentCulture)))

        '            '商品情報を取得する
        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassMercinfoDataTable)("TABLETSMBCOMMONCLASS_021")
        '                Dim Sql As New StringBuilder
        '                With Sql
        '                    .Append(" SELECT /* TABLETSMBCOMMONCLASS_021 */ ")
        '                    .Append("        T1.SVC_CLASS_ID ")
        '                    .Append("      , T1.UPPER_DISP ")
        '                    .Append("      , T1.LOWER_DISP ")
        '                    .Append("      , T2.SVC_CLASS_NAME ")
        '                    .Append("      , T2.SVC_CLASS_NAME_ENG ")
        '                    .Append("   FROM ")
        '                    .Append("        TB_M_MERCHANDISE T1 ")
        '                    .Append("      , TB_M_SERVICE_CLASS T2 ")
        '                    .Append("  WHERE  ")
        '                    .Append("        T1.SVC_CLASS_ID = T2.SVC_CLASS_ID ")
        '                    .Append("    AND T1.MERC_ID = :MERC_ID ")
        '                    .Append("    AND T1.INUSE_FLG = :INUSE_FLG ")
        '                    .Append("    AND T1.SVC_CLASS_ID <> 0 ")
        '                End With
        '                query.CommandText = Sql.ToString()
        '                'SQLパラメータ設定値
        '                '商品ID
        '                query.AddParameterWithTypeValue("MERC_ID", OracleDbType.Long, inMercId)
        '                '使用フラグ
        '                query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.NVarchar2, InUse)

        '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        '                Return query.GetData()
        '            End Using

        '        End Function
        '#End Region
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        '#Region "基幹連携(予約送信)"

        '        ''' <summary>
        '        ''' サービス基幹連携送信設定から送信フラグを取得する
        '        ''' </summary>
        '        ''' <param name="inDealerCD">販売店コード</param>
        '        ''' <param name="inBranchCD">店舗コード</param>
        '        ''' <param name="inAllDealerCD">全販売店を示すコード</param>
        '        ''' <param name="inAllBranchCD">全店舗を示すコード</param>
        '        ''' <param name="inInterfaceType">インターフェース区分(1:予約送信/2:ステータス送信/3:作業実績送信)</param>
        '        ''' <param name="inPrevStatus">更新前サービス連携ステータス</param>
        '        ''' <param name="inCrntStatus">更新後サービス連携ステータス</param>
        '        ''' <returns>0:送信しない/1:送信する/Empty:取得できなかった</returns>
        '        ''' <remarks></remarks>
        '        Public Function GetLinkSettings(ByVal inDealerCD As String, _
        '                                        ByVal inBranchCD As String, _
        '                                        ByVal inAllDealerCD As String, _
        '                                        ByVal inAllBranchCD As String, _
        '                                        ByVal inInterfaceType As String, _
        '                                        ByVal inPrevStatus As String, _
        '                                        ByVal inCrntStatus As String) As String

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_S inDealerCD={1}, inBranchCD={2}, inAllDealerCD={3}, inAllBranchCD={4}, inInterfaceType={5}, inPrevStatus={6}, inCrntStatus={7}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      inDealerCD, _
        '                                      inBranchCD, _
        '                                      inAllDealerCD, _
        '                                      inAllBranchCD, _
        '                                      inInterfaceType, _
        '                                      inPrevStatus, _
        '                                      inCrntStatus))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine("   SELECT /* TABLETSMBCOMMONCLASS_022 */ ")
        '                .AppendLine(" 		   SEND_FLG ")
        '                .AppendLine(" 		 , DLR_CD ")
        '                .AppendLine(" 		 , BRN_CD ")
        '                .AppendLine("     FROM ")
        '                .AppendLine(" 		   TB_M_SVC_LINK_SEND_SETTING ")
        '                .AppendLine("    WHERE ")
        '                .AppendLine(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
        '                .AppendLine(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
        '                .AppendLine(" 	   AND INTERFACE_TYPE = :INTERFACE_TYPE ")
        '                .AppendLine(" 	   AND BEFORE_SVC_LINK_STATUS = :BEFORE_SVC_LINK_STATUS ")
        '                .AppendLine(" 	   AND AFTER_SVC_LINK_STATUS = :AFTER_SVC_LINK_STATUS ")
        '                .AppendLine(" ORDER BY ")
        '                .AppendLine("          DLR_CD ASC, BRN_CD ASC ")
        '            End With

        '            Dim getTable As TabletSmbCommonClassLinkSendSettingsDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassLinkSendSettingsDataTable)("TABLETSMBCOMMONCLASS_022")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCD)
        '                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCD)
        '                query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, inAllDealerCD)
        '                query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, inAllBranchCD)
        '                query.AddParameterWithTypeValue("INTERFACE_TYPE", OracleDbType.NVarchar2, inInterfaceType)
        '                query.AddParameterWithTypeValue("BEFORE_SVC_LINK_STATUS", OracleDbType.NVarchar2, inPrevStatus)
        '                query.AddParameterWithTypeValue("AFTER_SVC_LINK_STATUS", OracleDbType.NVarchar2, inCrntStatus)

        '                getTable = query.GetData()
        '            End Using

        '            '送信フラグ（戻り値）
        '            Dim sendFlg As String = String.Empty

        '            'ログ出力用販売店コード、店舗コード
        '            Dim dealerCode As String = String.Empty
        '            Dim branchCode As String = String.Empty

        '            Dim getFirstRow As TabletSmbCommonClassLinkSendSettingsRow

        '            If 0 < getTable.Count Then

        '                '取得データの1行目（最優先レコード）のみを取得
        '                getFirstRow = getTable.Item(0)
        '                '最優先レコードの送信フラグ、販売店コード、店舗コードを取得
        '                sendFlg = getFirstRow.SEND_FLG
        '                dealerCode = getFirstRow.DLR_CD
        '                branchCode = getFirstRow.BRN_CD

        '            End If

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E SEND_FLG={1}, DLR_CD={2}, BRN_CD={3}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      sendFlg, _
        '                                      dealerCode, _
        '                                      branchCode))

        '            Return sendFlg

        '        End Function

        '        ''' <summary>
        '        ''' 顧客、車両、販売店車両、販売店顧客車両情報を取得する
        '        ''' </summary>
        '        ''' <param name="svcInId">サービス入庫ID</param>
        '        ''' <param name="dealerCD">販売店コード</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Function GetSendDmsCstVclInfo(ByVal svcInId As Long, ByVal dealerCD As String) As TabletSmbCommonClassDmsSendCstVclInfoDataTable

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S svcInId={1}, dealerCD={2}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId, dealerCD))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_023 */ ")
        '                .AppendLine(" 	      B.DMS_CST_CD ")                                               '基幹顧客コード
        '                .AppendLine("       , B.NEWCST_CD ")                                                '未取引客コード
        '                .AppendLine(" 	    , B.CST_NAME ")                                                 '顧客氏名
        '                .AppendLine(" 	    , B.CST_PHONE ")                                                '顧客電話番号
        '                .AppendLine(" 	    , B.CST_MOBILE ")                                               '顧客携帯電話番号
        '                .AppendLine(" 	    , B.CST_EMAIL_1 ")                                              '顧客EMAILアドレス1
        '                .AppendLine(" 		, B.CST_ZIPCD ")                                                '顧客郵便番号
        '                .AppendLine(" 		, B.CST_ADDRESS ")                                              '顧客住所
        '                .AppendLine(" 		, C.VCL_VIN ")                                                  'VIN(車両マスタ)
        '                .AppendLine(" 		, C.VCL_KATASHIKI ")                                            '車両型式
        '                .AppendLine(" 		, C.MODEL_CD ")                                                 'モデルコード
        '                .AppendLine(" 		, D.REG_NUM ")                                                  '車両登録番号
        '                .AppendLine(" 		, E.CST_VCL_TYPE ")                                             '顧客車両区分
        '                .AppendLine(" 		, E.SVC_PIC_STF_CD ")                                           'サービス担当スタッフコード
        '                .AppendLine(" 		, NVL(TRIM(F.MODEL_NAME), C.NEWCST_MODEL_NAME) MODEL_NAME ")    'モデル名
        '                .AppendLine(" 		, G.VCL_VIN VCL_VIN_SALESBOOKING ")                             'VIN(注文マスタ)
        '                .AppendLine("   FROM  ")
        '                .AppendLine(" 		  TB_T_SERVICEIN A ")                                           'サービス入庫
        '                .AppendLine(" 	    , TB_M_CUSTOMER B ")                                            '顧客
        '                .AppendLine(" 	    , TB_M_VEHICLE C ")                                             '車両
        '                .AppendLine(" 	    , TB_M_VEHICLE_DLR D ")                                         '販売店車両
        '                .AppendLine(" 	    , TB_M_CUSTOMER_VCL E ")                                        '販売店顧客車両
        '                .AppendLine(" 	    , TB_M_MODEL F ")                                               'モデルマスタ
        '                .AppendLine(" 	    , TB_T_SALESBOOKING G ")                                        '注文
        '                .AppendLine("  WHERE  A.CST_ID = B.CST_ID(+) ")
        '                .AppendLine("    AND  A.VCL_ID = C.VCL_ID(+) ")
        '                .AppendLine("    AND  A.VCL_ID = D.VCL_ID(+) ")
        '                .AppendLine("    AND  A.CST_ID = E.CST_ID(+) ")
        '                .AppendLine("    AND  A.VCL_ID = E.VCL_ID(+) ")
        '                .AppendLine("    AND  C.MODEL_CD = F.MODEL_CD(+) ")
        '                .AppendLine("    AND  D.DLR_CD = G.DLR_CD(+) ")
        '                .AppendLine("    AND  D.SALESBKG_NUM = G.SALESBKG_NUM(+) ")
        '                .AppendLine("    AND  A.SVCIN_ID = :SVCIN_ID ")
        '                .AppendLine("    AND  D.DLR_CD = :DLR_CD ")
        '                .AppendLine("    AND  E.DLR_CD = :DLR_CD ")
        '            End With

        '            Dim getTable As TabletSmbCommonClassDmsSendCstVclInfoDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassDmsSendCstVclInfoDataTable)("TABLETSMBCOMMONCLASS_023")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)
        '                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)

        '                getTable = query.GetData()
        '            End Using

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E RowCount={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, getTable.Count))

        '            Return getTable

        '        End Function

        '        ''' <summary>
        '        ''' 予約情報、サービス分類情報を取得する
        '        ''' </summary>
        '        ''' <param name="svcInId">サービス入庫ID</param>
        '        ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Function GetSendDmsReserveInfo(ByVal svcInId As Long, _
        '                                              ByVal prevCancelJobDtlIdList As List(Of Long)) As TabletSmbCommonClassDmsSendReserveInfoDataTable

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_S svcInId={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      svcInId))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine("   SELECT /* TABLETSMBCOMMONCLASS_024 */ ")
        '                .AppendLine("   	    A.SVCIN_MILE ")                                                                   '入庫時走行距離
        '                .AppendLine("         , A.CARWASH_NEED_FLG ")                                                             '洗車必要フラグ
        '                .AppendLine("   	  , A.PICK_DELI_TYPE ")                                                               '引取納車区分
        '                .AppendLine("   	  , A.PIC_SA_STF_CD ")                                                                '担当SAスタッフコード
        '                .AppendLine("   	  , A.RESV_STATUS ")                                                                  '予約ステータス
        '                .AppendLine("   	  , A.ACCEPTANCE_TYPE ")                                                              '受付区分
        '                .AppendLine("   	  , A.SMS_TRANSMISSION_FLG ")                                                         'SMS送信可フラグ
        '                .AppendLine("   	  , A.SVCIN_CREATE_TYPE ")                                                            'サービス入庫作成元区分
        '                .AppendLine("   	  , A.ROW_LOCK_VERSION ")                                                             'サービス入庫テーブル.行ロックバージョン
        '                .AppendLine("   	  , B.INSPECTION_NEED_FLG ")                                                          '検査必要フラグ
        '                .AppendLine("   	  , B.MERC_ID ")                                                                      '表示商品ID
        '                .AppendLine("   	  , B.MAINTE_CD ")                                                                    '整備コード
        '                .AppendLine("   	  , B.JOB_DTL_ID ")                                                                   '作業内容ID
        '                .AppendLine("   	  , B.DMS_JOB_DTL_ID ")                                                               '基幹作業内容ID
        '                .AppendLine("   	  , B.JOB_DTL_MEMO ")                                                                 '作業内容メモ
        '                .AppendLine("   	  , B.CREATE_STF_CD  ")                                                               '作成スタッフコード
        '                .AppendLine("   	  , B.UPDATE_STF_CD ")                                                                '更新スタッフコード
        '                .AppendLine("    	  , B.CANCEL_FLG ")                                                                   'キャンセルフラグ
        '                .AppendLine("   	  , B.CREATE_DATETIME ")                                                              '作成日時
        '                .AppendLine("   	  , B.UPDATE_DATETIME ")                                                              '更新日時
        '                .AppendLine("   	  , C.SCHE_START_DATETIME ")                                                          '予定開始日時
        '                .AppendLine("   	  , C.SCHE_END_DATETIME ")                                                            '予定終了日時
        '                .AppendLine("   	  , C.SCHE_WORKTIME ")                                                                '予定作業時間
        '                .AppendLine("   	  , C.STALL_ID ")                                                                     'ストールID
        '                .AppendLine("   	  , C.TEMP_FLG ")                                                                     '仮置きフラグ
        '                .AppendLine("   	  , C.STALL_USE_STATUS ")                                                             'ストール利用ステータス
        '                .AppendLine("   	  , D.SVC_CLASS_CD ")                                                                 'サービス分類コード
        '                .AppendLine("   	  , NVL(TRIM(D.SVC_CLASS_NAME), D.SVC_CLASS_NAME_ENG) SVC_CLASS_NAME")                'サービス分類名称
        '                .AppendLine("   	  , E.PICK_PREF_DATETIME ")                                                           '引取希望日時
        '                .AppendLine("   	  , E.PICK_DESTINATION ")                                                             '引取先
        '                .AppendLine("   	  , E.PICK_WORKTIME ")                                                                '引取作業時間
        '                .AppendLine("   	  , F.DELI_PREF_DATETIME ")                                                           '配送希望日時
        '                .AppendLine("   	  , F.DELI_DESTINATION ")                                                             '配送先
        '                .AppendLine("   	  , F.DELI_WORKTIME ")                                                                '配送作業時間
        '                .AppendLine("     FROM   ")
        '                .AppendLine("   	    TB_T_SERVICEIN A  ")                                                              'サービス入庫
        '                .AppendLine("   	  , TB_T_JOB_DTL B  ")                                                                '作業内容
        '                .AppendLine("   	  , TB_T_STALL_USE C  ")                                                              'ストール利用
        '                .AppendLine("   	  , TB_M_SERVICE_CLASS D  ")                                                          'サービス分類マスタ
        '                .AppendLine("   	  , TB_T_VEHICLE_PICKUP E ")                                                          '車両引取
        '                .AppendLine("   	  , TB_T_VEHICLE_DELIVERY F ")                                                        '車両配送
        '                .AppendLine("    WHERE  A.SVCIN_ID = B.SVCIN_ID(+)  ")
        '                .AppendLine("      AND  B.JOB_DTL_ID = C.JOB_DTL_ID(+) ")
        '                .AppendLine("      AND  B.SVC_CLASS_ID = D.SVC_CLASS_ID(+) ")
        '                .AppendLine("      AND  A.SVCIN_ID = E.SVCIN_ID(+) ")
        '                .AppendLine("      AND  A.SVCIN_ID = F.SVCIN_ID(+)  ")
        '                .AppendLine("      AND  A.SVCIN_ID = :SVCIN_ID ")
        '                .AppendLine("      AND  C.STALL_USE_ID = ( SELECT MAX(STALL_USE_ID) ")
        '                .AppendLine("                                FROM TB_T_STALL_USE ")
        '                .AppendLine("                               WHERE B.JOB_DTL_ID = JOB_DTL_ID ) ")
        '                .AppendLine("      AND  B.JOB_DTL_ID NOT IN (:CANCELED_JOB_DTL_ID_LIST) ")
        '                .AppendLine(" ORDER BY ")
        '                .AppendLine("           B.JOB_DTL_ID ASC ")
        '            End With

        '            '予約情報の絞込み文字列を作成する
        '            Dim selectString As New StringBuilder

        '            If IsNothing(prevCancelJobDtlIdList) Then
        '                '元々キャンセルだった作業内容IDのリストがない場合
        '                selectString.Append("-1")
        '            Else
        '                '元々キャンセルだった作業内容IDのリストがある場合、
        '                'それらの作業内容IDに該当する予約情報は除く
        '                For Each canceledJobDtlId In prevCancelJobDtlIdList
        '                    selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
        '                    selectString.Append(",")
        '                Next
        '                '最後のカンマを削除
        '                selectString.Remove(selectString.Length - 1, 1)
        '            End If

        '            Dim getTable As TabletSmbCommonClassDmsSendReserveInfoDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassDmsSendReserveInfoDataTable)("TABLETSMBCOMMONCLASS_024")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)
        '                query.AddParameterWithTypeValue("CANCELED_JOB_DTL_ID_LIST", OracleDbType.NVarchar2, selectString.ToString())

        '                getTable = query.GetData()
        '            End Using

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E RowCount={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      getTable.Count))

        '            Return getTable

        '        End Function

        '        ''' <summary>
        '        ''' 自分の作業内容IDの子予約連番を取得
        '        ''' </summary>
        '        ''' <param name="svcInId">サービス入庫ID</param>
        '        ''' <param name="jobDtlId">作業内容ID</param>
        '        ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
        '        ''' <returns>子予約連番</returns>
        '        ''' <remarks>
        '        ''' キャンセルでない関連チップを作業内容IDの昇順で抽出し、
        '        ''' 抽出データに対して連番を1から割り当てる。
        '        ''' 自分の作業内容IDに割り当てられた連番を子予約連番とする。
        '        ''' </remarks>
        '        Public Function GetRezChildNo(ByVal svcInId As Long, _
        '                                      ByVal jobDtlId As Long, _
        '                                      ByVal prevCancelJobDtlIdList As List(Of Long)) As Long

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S svcInId={1}, jobDtlId={2}", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId, jobDtlId))

        '            Dim rezChildNo As Long = -1

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_025 */ ")
        '                .AppendLine(" 		 REZCHILDNO COL1 ")
        '                .AppendLine("   FROM ( ")
        '                .AppendLine(" 		     SELECT  ")
        '                .AppendLine(" 			          ROWNUM REZCHILDNO ")
        '                .AppendLine(" 			        , JOB_DTL_ID ")
        '                .AppendLine(" 		       FROM ( ")
        '                .AppendLine(" 					   SELECT JOB_DTL_ID, CANCEL_FLG ")
        '                .AppendLine(" 					     FROM TB_T_JOB_DTL ")
        '                .AppendLine(" 					    WHERE SVCIN_ID = :SVCIN_ID ")
        '                .AppendLine(" 					      AND JOB_DTL_ID NOT IN (:CANCELED_JOB_DTL_ID_LIST) ")
        '                .AppendLine(" 			         ORDER BY JOB_DTL_ID ASC ")
        '                .AppendLine(" 				    )  ")
        '                .AppendLine(" 	     ) A ")
        '                .AppendLine("  WHERE A.JOB_DTL_ID = :JOB_DTL_ID ")
        '            End With

        '            '予約情報の絞込み文字列を作成する
        '            Dim selectString As New StringBuilder

        '            If IsNothing(prevCancelJobDtlIdList) Then
        '                '元々キャンセルだった作業内容IDのリストがない場合
        '                selectString.Append("-1")
        '            Else
        '                '元々キャンセルだった作業内容IDのリストがある場合、
        '                'それらの作業内容IDに該当する予約情報は除く
        '                For Each canceledJobDtlId In prevCancelJobDtlIdList
        '                    selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
        '                    selectString.Append(",")
        '                Next
        '                '最後のカンマを削除
        '                selectString.Remove(selectString.Length - 1, 1)
        '            End If

        '            Dim getTable As TabletSmbCommonClassNumberValueDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_025")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)
        '                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Long, jobDtlId)
        '                query.AddParameterWithTypeValue("CANCELED_JOB_DTL_ID_LIST", OracleDbType.NVarchar2, selectString.ToString())

        '                getTable = query.GetData()
        '            End Using

        '            If 0 < getTable.Count Then
        '                rezChildNo = DirectCast(getTable.Rows(0).Item(0), Long)
        '            End If

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E rezChildNo={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, rezChildNo))

        '            Return rezChildNo

        '        End Function

        '        ''' <summary>
        '        ''' 基幹作業内容IDを更新する
        '        ''' </summary>
        '        ''' <param name="jobDtlId">作業内容ID</param>
        '        ''' <param name="newDmsJobDtlId">基幹作業内容ID</param>
        '        ''' <param name="account">ログインアカウント</param>
        '        ''' <param name="nowDataTime">現在日時</param>
        '        ''' <param name="functionId">機能ID</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Function UpdateDmsJobDtlId(ByVal jobDtlId As Long, ByVal newDmsJobDtlId As String, _
        '                                          ByVal account As String, ByVal nowDataTime As Date, _
        '                                          ByVal functionId As String) As Integer

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. jobDtlId={1}, newDmsJobDtlId={2}, account={3}, nowDataTime={4}, functionId={5}" _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, jobDtlId, newDmsJobDtlId, account, nowDataTime, functionId))

        '            'SQL組み立て
        '            Dim sql As New StringBuilder
        '            With sql
        '                .Append(" UPDATE /* TABLETSMBCOMMONCLASS_218 */ ")
        '                .Append("       TB_T_JOB_DTL ")
        '                .Append("    SET ")
        '                .Append("       DMS_JOB_DTL_ID = :DMS_JOB_DTL_ID ")             '基幹作業内容ID
        '                .Append("     , UPDATE_DATETIME = :UPDATE_DATETIME ")           '更新日時
        '                .Append("     , UPDATE_STF_CD = :ACCOUNT ")                     '更新スタッフコード
        '                .Append("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")       '行更新日時
        '                .Append("     , ROW_UPDATE_ACCOUNT = :ACCOUNT ")                '行更新アカウント
        '                .Append("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")   '行更新機能
        '                .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
        '                .Append("  WHERE ")
        '                .Append("       JOB_DTL_ID = :JOB_DTL_ID ")
        '            End With

        '            'DbUpdateQueryインスタンス生成
        '            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_218")

        '                query.CommandText = sql.ToString()

        '                'SQLパラメータ設定
        '                query.AddParameterWithTypeValue("DMS_JOB_DTL_ID", OracleDbType.NVarchar2, newDmsJobDtlId)
        '                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
        '                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDataTime)
        '                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, functionId)
        '                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Int64, jobDtlId)

        '                'SQL実行
        '                Dim result As Integer = query.Execute()

        '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. result={1}" _
        '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name, result))

        '                Return result

        '            End Using

        '        End Function

        '#End Region

        '#Region "共通"

        '        ''' <summary>
        '        ''' システム設定から設定値を取得する
        '        ''' </summary>
        '        ''' <param name="settingName">システム設定名</param>
        '        ''' <returns>システム設定値</returns>
        '        ''' <remarks></remarks>
        '        Public Function GetSystemSettingValue(ByVal settingName As String) As String

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_S settingName={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      settingName))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_026 */ ")
        '                .AppendLine(" 		 SETTING_VAL COL1 ")
        '                .AppendLine("   FROM ")
        '                .AppendLine(" 		 TB_M_SYSTEM_SETTING ")
        '                .AppendLine("  WHERE ")
        '                .AppendLine(" 		 SETTING_NAME = :SETTING_NAME ")
        '            End With

        '            Dim getTable As TabletSmbCommonClassStringValueDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_026")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

        '                getTable = query.GetData()
        '            End Using

        '            '戻り値
        '            Dim retValue As String = String.Empty

        '            If 0 < getTable.Count Then

        '                '設定値を取得
        '                retValue = getTable.Rows(0).Item(0).ToString().Trim()

        '            End If

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_E retValue={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      retValue))

        '            Return retValue

        '        End Function

        '        ''' <summary>
        '        ''' 販売店システム設定から設定値を取得する
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
        '        ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
        '        ''' <param name="settingName">販売店システム設定名</param>
        '        ''' <returns>販売店システム設定値</returns>
        '        ''' <remarks></remarks>
        '        Public Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
        '                                                 ByVal branchCode As String, _
        '                                                 ByVal allDealerCode As String, _
        '                                                 ByVal allBranchCode As String, _
        '                                                 ByVal settingName As String) As String

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_S dealerCode={1}, branchCode={2}, allDealerCode={3}, allBranchCode={4}, settingName={5}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      dealerCode, _
        '                                      branchCode, _
        '                                      allDealerCode, _
        '                                      allBranchCode, _
        '                                      settingName))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine("   SELECT /* TABLETSMBCOMMONCLASS_027 */ ")
        '                .AppendLine(" 		   SETTING_VAL COL1 ")
        '                .AppendLine("     FROM ")
        '                .AppendLine(" 		   TB_M_SYSTEM_SETTING_DLR ")
        '                .AppendLine("    WHERE ")
        '                .AppendLine(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
        '                .AppendLine(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
        '                .AppendLine("      AND SETTING_NAME = :SETTING_NAME ")
        '                .AppendLine(" ORDER BY ")
        '                .AppendLine("          DLR_CD ASC, BRN_CD ASC ")
        '            End With

        '            Dim getTable As TabletSmbCommonClassStringValueDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_027")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
        '                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
        '                query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCode)
        '                query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, allBranchCode)
        '                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

        '                getTable = query.GetData()
        '            End Using

        '            '戻り値
        '            Dim retValue As String = String.Empty

        '            If 0 < getTable.Count Then

        '                '設定値を取得
        '                retValue = getTable.Rows(0).Item(0).ToString().Trim()

        '            End If

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_E retValue={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      retValue))

        '            Return retValue

        '        End Function

        '        ''' <summary>
        '        ''' 基幹コードマップテーブルから設定値を取得する
        '        ''' </summary>
        '        ''' <param name="allDealerCD">全販売店を意味するワイルドカード販売店コード</param>
        '        ''' <param name="dmsCodeType">基幹コード区分</param>
        '        ''' <param name="icropCD1">iCROPコード1</param>
        '        ''' <param name="icropCD2">iCROPコード2</param>
        '        ''' <param name="icropCD3">iCROPコード3</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Function GetDmsCodeMapValue(ByVal allDealerCD As String, _
        '                                           ByVal dmsCodeType As String, _
        '                                           ByVal icropCD1 As String, _
        '                                           ByVal icropCD2 As String, _
        '                                           ByVal icropCD3 As String) As TabletSmbCommonClassDmsCodeMapDataTable

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_S allDealerCD={1}, dmsCodeType={2}, icropCD1={3}, icropCD2={4}, icropCD3={5}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      allDealerCD, _
        '                                      dmsCodeType, _
        '                                      icropCD1, _
        '                                      icropCD2, _
        '                                      icropCD3))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_028 */ ")
        '                .AppendLine(" 		   DMS_CD_1 ")                      '基幹コード1
        '                .AppendLine(" 		 , DMS_CD_2 ")                      '基幹コード2
        '                .AppendLine(" 		 , DMS_CD_3 ")                      '基幹コード3
        '                .AppendLine("   FROM ")
        '                .AppendLine(" 		   TB_M_DMS_CODE_MAP ")             '基幹コードマップ
        '                .AppendLine("  WHERE ")
        '                .AppendLine(" 		   DLR_CD = :DLR_CD ")
        '                .AppendLine(" 	 AND   DMS_CD_TYPE = :DMS_CD_TYPE ")
        '                .AppendLine(" 	 AND   ICROP_CD_1 = :ICROP_CD_1 ")

        '                If Not String.IsNullOrEmpty(icropCD2) Then
        '                    .AppendLine(" 	 AND   ICROP_CD_2 = :ICROP_CD_2 ")
        '                End If

        '                If Not String.IsNullOrEmpty(icropCD3) Then
        '                    .AppendLine(" 	 AND   ICROP_CD_3 = :ICROP_CD_3 ")
        '                End If

        '            End With

        '            Dim getTable As TabletSmbCommonClassDmsCodeMapDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassDmsCodeMapDataTable)("TABLETSMBCOMMONCLASS_028")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, allDealerCD)
        '                query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, dmsCodeType)
        '                query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, icropCD1)

        '                If Not String.IsNullOrEmpty(icropCD2) Then
        '                    query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.NVarchar2, icropCD2)
        '                End If

        '                If Not String.IsNullOrEmpty(icropCD3) Then
        '                    query.AddParameterWithTypeValue("ICROP_CD_3", OracleDbType.NVarchar2, icropCD3)
        '                End If

        '                getTable = query.GetData()
        '            End Using

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_E Count={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      getTable.Count))

        '            Return getTable

        '        End Function

        '        ''' <summary>
        '        ''' 自分が子チップかどうかの情報を取得する
        '        ''' </summary>
        '        ''' <param name="svcInId">サービス入庫</param>
        '        ''' <param name="jobDtlId">作業内容ID</param>
        '        ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Function GetJudgeChildChip(ByVal svcInId As Long, _
        '                                          ByVal jobDtlId As Long, _
        '                                          ByVal prevCancelJobDtlIdList As List(Of Long)) As TabletSmbCommonClassNumberValueDataTable

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_S svcInId={1}, jobDtlId={2}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      svcInId, _
        '                                      jobDtlId))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_029 */ ")
        '                .AppendLine(" 		 JOB_DTL_ID ")
        '                .AppendLine("   FROM ")
        '                .AppendLine(" 		 TB_T_JOB_DTL ")
        '                .AppendLine("  WHERE ")
        '                .AppendLine(" 		 SVCIN_ID = :SVCIN_ID ")
        '                .AppendLine(" 	 AND JOB_DTL_ID < :JOB_DTL_ID ")
        '                .AppendLine("    AND JOB_DTL_ID NOT IN (:CANCELED_JOB_DTL_ID_LIST) ")
        '            End With

        '            '予約情報の絞込み文字列を作成する
        '            Dim selectString As New StringBuilder

        '            If IsNothing(prevCancelJobDtlIdList) Then
        '                '元々キャンセルだった作業内容IDのリストがない場合
        '                selectString.Append("-1")
        '            Else
        '                '元々キャンセルだった作業内容IDのリストがある場合、
        '                'それらの作業内容IDに該当する予約情報は除く
        '                For Each canceledJobDtlId In prevCancelJobDtlIdList
        '                    selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
        '                    selectString.Append(",")
        '                Next
        '                '最後のカンマを削除
        '                selectString.Remove(selectString.Length - 1, 1)
        '            End If

        '            Dim getTable As TabletSmbCommonClassNumberValueDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_029")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)
        '                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Long, jobDtlId)
        '                query.AddParameterWithTypeValue("CANCELED_JOB_DTL_ID_LIST", OracleDbType.NVarchar2, selectString.ToString())

        '                getTable = query.GetData()
        '            End Using

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_E retValue={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      getTable.Count))

        '            Return getTable

        '        End Function

        '        ''' <summary>
        '        ''' 関連チップ内で最も小さい作業内容ID(管理作業内容ID)を取得
        '        ''' </summary>
        '        ''' <param name="svcInId">サービス入庫ID</param>
        '        ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
        '        ''' <returns>作業内容ID</returns>
        '        ''' <remarks></remarks>
        '        Public Function GetMinimumJobDtlId(ByVal svcInId As Long, _
        '                                           ByVal prevCancelJobDtlIdList As List(Of Long)) As Long

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_S svcInId={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      svcInId))

        '            Dim minJobDetailId As Long = 0

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_030 */ ")
        '                .AppendLine(" 		 MIN(JOB_DTL_ID) COL1 ")
        '                .AppendLine("   FROM ")
        '                .AppendLine(" 		 TB_T_JOB_DTL ")
        '                .AppendLine("  WHERE ")
        '                .AppendLine(" 		 SVCIN_ID = :SVCIN_ID ")
        '                .AppendLine("    AND JOB_DTL_ID NOT IN (:CANCELED_JOB_DTL_ID_LIST) ")
        '            End With

        '            '予約情報の絞込み文字列を作成する
        '            Dim selectString As New StringBuilder

        '            If IsNothing(prevCancelJobDtlIdList) Then
        '                '元々キャンセルだった作業内容IDのリストがない場合
        '                selectString.Append("-1")
        '            Else
        '                '元々キャンセルだった作業内容IDのリストがある場合、
        '                'それらの作業内容IDに該当する予約情報は除く
        '                For Each canceledJobDtlId In prevCancelJobDtlIdList
        '                    selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
        '                    selectString.Append(",")
        '                Next
        '                '最後のカンマを削除
        '                selectString.Remove(selectString.Length - 1, 1)
        '            End If

        '            Dim getTable As TabletSmbCommonClassNumberValueDataTable = Nothing

        '            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_030")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)
        '                query.AddParameterWithTypeValue("CANCELED_JOB_DTL_ID_LIST", OracleDbType.NVarchar2, selectString.ToString())

        '                getTable = query.GetData()
        '            End Using

        '            If 0 < getTable.Count Then
        '                minJobDetailId = DirectCast(getTable.Rows(0).Item(0), Long)
        '            End If

        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                      "{0}_E Count={1}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      getTable.Count))

        '            Return minJobDetailId


        '        End Function

        '#End Region
#End Region
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

#Region "親R/Oが作業開始するかどうかチェック"

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ' ''' <summary>
        ' ''' 親R/Oが作業開始するかどうかチェックする
        ' ''' </summary>
        ' ''' <param name="svcinId">サービス入庫ID</param>
        ' ''' <returns>親ROが作業開始されたチップがある<c>true</c>、それ以外の場合<c>false</c></returns>
        ' ''' <remarks></remarks>
        'Public Function HasParentroStarted(ByVal svcinId As Decimal) As Boolean

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcinId={1}" _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name, svcinId))

        ''' <summary>
        ''' 親R/Oが作業開始するかどうかチェックする
        ''' </summary>
        ''' <param name="inRoNum">RO番号</param>
        ''' <returns>親ROが作業開始されたチップがある<c>true</c>、それ以外の場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function HasParentroStarted(ByVal inRONum As String) As Boolean

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inRONum={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inRONum))
            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            Dim sql As New StringBuilder
            With sql
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                '.AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_031 */ ")
                '.AppendLine("        COUNT(1) COL1 ")
                '.AppendLine("   FROM ")
                '.AppendLine("        TB_T_SERVICEIN TSRVIN ")
                '.AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                '.AppendLine("      , TB_T_STALL_USE TSTAUSE ")
                '.AppendLine("  WHERE ")
                '.AppendLine("        TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                '.AppendLine("    AND TJOBDTL.JOB_DTL_ID = TSTAUSE.JOB_DTL_ID ")
                '.AppendLine("    AND TSRVIN.SVCIN_ID = :SVCIN_ID ")
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''.AppendLine("    AND TJOBDTL.RO_JOB_SEQ = 0 ")                                                      '親チップ
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("    AND TSTAUSE.RSLT_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') ") '開始した場合
                '.AppendLine("    AND ROWNUM <= 1 ")
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("    AND EXISTS ")
                '.AppendLine("           ( ")
                '.AppendLine("              SELECT  ")
                '.AppendLine("                   S1.JOB_DTL_ID  ")
                '.AppendLine("               FROM  ")
                '.AppendLine("                   TB_T_JOB_INSTRUCT S1  ")
                '.AppendLine("               WHERE  ")
                '.AppendLine("                    S1.JOB_DTL_ID = TJOBDTL.JOB_DTL_ID  ")
                '.AppendLine("                AND S1.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_ON  ")
                '.AppendLine("                AND S1.RO_SEQ = 0  ")
                '.AppendLine("           ) ")
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_031 */ ")
                .AppendLine("        COUNT(1) COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
                .AppendLine("      , TB_T_JOB_RESULT T2 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .AppendLine("    AND T1.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID ")
                .AppendLine("    AND T1.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ ")
                .AppendLine("    AND T1.RO_NUM = :RO_NUM   ")
                .AppendLine("    AND T1.RO_SEQ = 0   ")
                .AppendLine("    AND T1.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_ON ")
                .AppendLine("    AND ROWNUM <= 1 ")
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
            End With

            Dim dtQuery As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_031")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                query.CommandText = sql.ToString()

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                'query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                dtQuery = query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E count={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, CType(dtQuery(0)(0), Long)))
            '存在する場合
            If CType(dtQuery(0)(0), Long) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
#End Region

#Region "全て作業が検査済かチェック"
        ''' <summary>
        ''' サービス入庫単位での完成検査ステータスの取得
        ''' </summary>
        ''' <param name="svcinIdList">サービス入庫IDリスト</param>
        ''' <returns>遅れ見込情報データテーブル</returns>
        ''' <remarks>遅れ見込の計算に必要</remarks>
        ''' <history>
        ''' 2015/01/14 TMEJ 明瀬 納車遅れ見込計算の不具合修正
        ''' </history>
        Public Function GetInspectionStatusBySvcinId(ByVal svcinIdList As List(Of Decimal)) As TabletSmbCommonClassDeliDelayDateDataTable

            'サービス入庫IDがない場合、空白テーブルを戻す
            If IsNothing(svcinIdList) OrElse svcinIdList.Count = 0 Then
                Return New TabletSmbCommonClassDeliDelayDateDataTable
            End If

            'サービス入庫IDを「svcinid1,svcinid2,…svcinidN」のstringに変更する
            Dim sbSvcinId As New StringBuilder
            For Each svcinId As String In svcinIdList
                sbSvcinId.Append(svcinId)
                sbSvcinId.Append(",")
            Next

            Dim strSvcinIdList As String = sbSvcinId.ToString()
            '最後のコンマを削除する
            strSvcinIdList = strSvcinIdList.Substring(0, strSvcinIdList.Length - 1)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SvcinIdList={1} " _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, strSvcinIdList))
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_032 */ ")
                .AppendLine("        TSRVIN.SVCIN_ID SVCIN_ID ")
                .AppendLine("      , TJOBDTL.INSPECTION_STATUS INSPECTION_STATUS ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("  WHERE ")
                .AppendLine("        TSRVIN.SVCIN_ID IN ( ")
                .AppendLine(strSvcinIdList)
                .AppendLine("                           ) ")
                .AppendLine("    AND TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                '2015/01/14 TMEJ 明瀬 納車遅れ見込計算の不具合修正 START
                .AppendLine("    AND TJOBDTL.CANCEL_FLG = N'0' ")
                '2015/01/14 TMEJ 明瀬 納車遅れ見込計算の不具合修正 END
            End With

            Dim dtQuery As TabletSmbCommonClassDeliDelayDateDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassDeliDelayDateDataTable)("TABLETSMBCOMMONCLASS_032")
                query.CommandText = sql.ToString()
                dtQuery = query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtQuery
        End Function
#End Region

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START 
#Region "指定作業内容IDに着工済作業は全部作業実績テーブルに存在するか判定チェック"

        ''' <summary>
        ''' 指定作業内容IDに着工済作業は全部作業実績テーブルに持っていない件数を取得
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>持っていない件数</returns>
        ''' <remarks></remarks>
        Public Function GetNoRsltDataCount(ByVal inJobDtlId As Decimal) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inJobDtlId={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inJobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* TABLETSMBCOMMONCLASS_058 */ ")
                .AppendLine(" 		   COUNT(1) COL1 ")
                .AppendLine("     FROM ")
                .AppendLine(" 		   (  ")
                .AppendLine(" 		    SELECT ")
                .AppendLine(" 		           T2.RSLT_START_DATETIME ")
                .AppendLine(" 		      FROM  ")
                .AppendLine(" 		           TB_T_JOB_INSTRUCT T1  ")
                .AppendLine(" 		         , TB_T_JOB_RESULT T2  ")
                .AppendLine(" 		     WHERE ")
                .AppendLine(" 		           T1.JOB_DTL_ID = T2.JOB_DTL_ID(+)  ")
                .AppendLine(" 	           AND T1.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID(+)    ")
                .AppendLine(" 	           AND T1.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ(+)   ")
                .AppendLine(" 	           AND T1.JOB_DTL_ID = :JOB_DTL_ID     ")
                .AppendLine(" 	           AND T1.STARTWORK_INSTRUCT_FLG = N'1'  ")
                .AppendLine(" 		       AND (T1.RO_NUM, T1.RO_SEQ) IN ")
                .AppendLine(" 		                    ( ")
                .AppendLine(" 		                        SELECT ")
                .AppendLine(" 		                                S2.RO_NUM, S2.RO_SEQ  ")
                .AppendLine(" 		                         FROM  ")
                .AppendLine(" 		                                TB_T_JOB_DTL S1  ")
                .AppendLine(" 		                              , TB_T_JOB_INSTRUCT S2   ")
                .AppendLine(" 		                        WHERE ")
                .AppendLine(" 		                                S1.JOB_DTL_ID = S2.JOB_DTL_ID  ")
                .AppendLine(" 		                            AND S1.JOB_DTL_ID = :JOB_DTL_ID     ")
                .AppendLine(" 	                        )  ")
                .AppendLine(" 	        )  ")
                .AppendLine(" 	 WHERE   ")
                .AppendLine(" 		   RSLT_START_DATETIME IS NULL  ")
            End With

            Dim getTable As TabletSmbCommonClassNumberValueDataTable = Nothing

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_058")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E No reslt data count={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      getTable(0).COL1))
            Return getTable

        End Function

#End Region

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#Region "予定納車日時があるサービス入庫IDを絞り込む"
        ''' <summary>
        ''' 予定納車日時があるサービス入庫IDを絞り込む
        ''' </summary>
        ''' <param name="svcinIdList">絞り込む前のサービス入庫IDリスト</param>
        ''' <param name="dtNow">今の日時</param>
        ''' <returns>絞り込んだサービス入庫IDリスト</returns>
        ''' <remarks></remarks>
        Public Function GetHasScheDeliDateSvcinId(ByVal svcinIdList As List(Of Decimal), _
                                                  ByVal dtNow As Date) As List(Of Decimal)

            'サービス入庫IDがない場合、空白テーブルを戻す
            If IsNothing(svcinIdList) OrElse svcinIdList.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. SvcinIdList is nothing or empty. " _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return svcinIdList
            End If

            'サービス入庫IDを「svcinid1,svcinid2,…svcinidN」のstringに変更する
            Dim sbSvcinId As New StringBuilder
            For Each svcinId As String In svcinIdList
                sbSvcinId.Append(svcinId)
                sbSvcinId.Append(",")
            Next

            Dim strSvcinIdList As String = sbSvcinId.ToString()
            '最後のコンマを削除する
            strSvcinIdList = strSvcinIdList.Substring(0, strSvcinIdList.Length - 1)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SvcinIdList={1} " _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, strSvcinIdList))
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_033 */ ")
                .AppendLine("        TSRVIN.SVCIN_ID SVCIN_ID ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN TSRVIN ")
                .AppendLine("      , TB_T_JOB_DTL TJOBDTL ")
                .AppendLine("  WHERE ")
                .AppendLine("        TSRVIN.SVCIN_ID = TJOBDTL.SVCIN_ID ")
                .AppendLine("    AND TSRVIN.SVCIN_ID IN ( ")
                .AppendLine(strSvcinIdList)
                .AppendLine("                           ) ")
                .AppendLine("    AND TJOBDTL.CANCEL_FLG= N'0' ")
                .AppendLine("    AND TSRVIN.SCHE_DELI_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")    '予定納車日がある
                .AppendLine("    AND TSRVIN.RSLT_DELI_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")     '納車してない
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '遅れているチップの残完成検査区分を取得するため削除
                '.AppendLine("    AND TSRVIN.SCHE_DELI_DATETIME >= :DATETIME_NOW ")    '予定納車日がまだ遅れない
                '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            End With

            Dim dtQuery As TabletSmbCommonClassDeliDelayDateDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassDeliDelayDateDataTable)("TABLETSMBCOMMONCLASS_033")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DATETIME_NOW", OracleDbType.Date, dtNow.AddSeconds(-dtNow.Second))
                dtQuery = query.GetData()
            End Using

            '探した結果をリストで返却
            Dim lstReturn As New List(Of Decimal)
            For Each drQuery As TabletSmbCommonClassDeliDelayDateRow In dtQuery
                '重複のサービス入庫IDを入らない
                If Not lstReturn.Contains(drQuery.SVCIN_ID) Then
                    lstReturn.Add(drQuery.SVCIN_ID)
                End If
            Next
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return lstReturn
        End Function
#End Region

#Region "TBL_STALLTIMEのTIMEINTERVAL取得"
        ''' <summary>
        ''' TBL_STALLTIMEのTIMEINTERVAL取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns>TIMEINTERVAL</returns>
        ''' <remarks></remarks>
        Public Function GetIntervalTime(ByVal inDealerCode As String, ByVal inStoreCode As String) As Long

            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} P1:{2} P2:{3}" _
                        , Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name, inDealerCode, inStoreCode))

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_034")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* TABLETSMBCOMMONCLASS_034 */")
                    .Append("        TIMEINTERVAL AS COL1  ")
                    .Append("  FROM  TBL_STALLTIME ")
                    .Append(" WHERE  DLRCD = :DLRCD ")
                    .Append("   AND  STRCD = :STRCD ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)

                Dim dt As TabletSmbCommonClassNumberValueDataTable = query.GetData()
                'ディフォルトが5
                Dim timeInterval As Long = 5
                If dt.Count = 1 Then
                    timeInterval = CType(dt(0)(0), Long)
                End If
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E timeInterval={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, timeInterval))
                Return timeInterval
            End Using
        End Function
#End Region

#Region "部品準備完了フラグ更新"

        ''' <summary>
        ''' ストール利用テーブル：「部品準備完了フラグ」更新
        ''' </summary>
        ''' <param name="inStalluseId">ストール利用ID</param>
        '''  <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inSystemId">プログラムID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdatePartsFlg(ByVal inStalluseId As Decimal, _
                                                   ByVal inAccount As String, _
                                                   ByVal inNowDate As Date, _
                                                   ByVal inSystemId As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inStalluseId.ToString(CultureInfo.CurrentCulture) _
                        , inAccount _
                        , inNowDate.ToString(CultureInfo.CurrentCulture) _
                        , inSystemId))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_219")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine("UPDATE /* TABLETSMBCOMMONCLASS_219 */ ")
                sql.AppendLine("       TB_T_STALL_USE ")
                sql.AppendLine("   SET PARTS_FLG = :PARTS_FLG ")
                sql.AppendLine("      ,UPDATE_DATETIME = :UPDATE_DATETIME ")
                sql.AppendLine("      ,UPDATE_STF_CD = :UPDATE_STF_CD ")
                sql.AppendLine("      ,ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                sql.AppendLine("      ,ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      ,ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                sql.AppendLine(" WHERE STALL_USE_ID = :STALL_USE_ID ")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("PARTS_FLG", OracleDbType.NVarchar2, "1")
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inSystemId)

                '条件
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStalluseId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function
#End Region

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "テクニシャンに関して処理"
        ''' <summary>
        ''' 指定チーフテクニシャンアカウントのストールIDを取得する
        ''' </summary>
        ''' <param name="inAccount">チーフテクニシャンアカウント</param>
        ''' <returns>ストールID</returns>
        Public Function GetStallidByChtAccount(ByVal inAccount As String) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inAccount={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inAccount))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_046 */ ")
                .AppendLine("        T4.STALL_ID AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STAFF T1 ")
                .AppendLine("      , TB_M_ORGANIZATION T2 ")
                .AppendLine("      , TB_M_STALL_GROUP T3 ")
                .AppendLine("      , TB_M_STALL_STALL_GROUP T4 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.ORGNZ_ID = T2.ORGNZ_ID ")
                .AppendLine("    AND T2.ORGNZ_ID = T3.ORGNZ_ID ")
                .AppendLine("    AND T3.STALL_GROUP_ID = T4.STALL_GROUP_ID ")
                .AppendLine("    AND T1.INUSE_FLG='1' ")
                .AppendLine("    AND T2.INUSE_FLG='1' ")
                .AppendLine("    AND T1.STF_CD = :STF_CD ")
            End With

            Dim returnTable As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_046")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, inAccount)
                returnTable = query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E Count={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      returnTable.Count))
            Return returnTable
        End Function
#End Region

        '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 START
#Region "タブレットSMB ストールグループ表示対応"
        ''' <summary>
        ''' アカウントの組織IDを取得する
        ''' </summary>
        ''' <param name="inAccount">アカウント</param>
        ''' <returns>組織ID</returns>
        Public Function GetOrgnzIdByAccount(ByVal inAccount As String) As List(Of Decimal)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inAccount={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inAccount))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_121 */ ")
                .AppendLine("        T2.ORGNZ_ID AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STAFF T1 ")
                .AppendLine("      , TB_M_ORGANIZATION T2 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.ORGNZ_ID = T2.ORGNZ_ID ")
                .AppendLine("    AND T2.INUSE_FLG = N'1' ")
                .AppendLine("    AND T1.STF_CD = :STF_CD ")
            End With

            Dim orgnzIdTable As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_121")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, inAccount)
                orgnzIdTable = query.GetData()
            End Using

            '取得結果をリストで返却
            Dim orgnzIdList As New List(Of Decimal)
            For Each row As TabletSmbCommonClassNumberValueRow In orgnzIdTable
                orgnzIdList.Add(row.COL1)
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E Count={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      orgnzIdTable.Count))

            Return orgnzIdList
        End Function

        ''' <summary>
        ''' 親組織IDから子組織IDを取得する
        ''' </summary>
        ''' <param name="parentOrgnzId">親組織ID</param>
        ''' <returns>子組織ID</returns>
        Public Function GetChildOrgnzIdByParentOrgnzId(ByVal parentOrgnzId As Decimal) As List(Of Decimal)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. parentOrgnzId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, parentOrgnzId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_122 */ ")
                .AppendLine("        T1.ORGNZ_ID AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_ORGANIZATION T1 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.PARENT_ORGNZ_ID = :PARETNT_ORGNZ_ID ")
                .AppendLine("    AND T1.INUSE_FLG  = N'1' ")

            End With

            Dim childOrgnzIdTable As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_122")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("PARETNT_ORGNZ_ID", OracleDbType.Decimal, parentOrgnzId)
                childOrgnzIdTable = query.GetData()
            End Using

            '取得結果をリストで返却
            Dim childOrgnzIdList As New List(Of Decimal)
            For Each row As TabletSmbCommonClassNumberValueRow In childOrgnzIdTable
                childOrgnzIdList.Add(row.COL1)
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E Count={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      childOrgnzIdTable.Count))

            Return childOrgnzIdList
        End Function

        ''' <summary>
        ''' 組織のストールIDを取得する
        ''' </summary>
        ''' <param name="orgnzIdList">組織IDリスト</param>
        ''' <returns>ストールID</returns>
        Public Function GetStallIdByOrgnzId(ByVal orgnzIdList As List(Of Decimal)) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. orgnzIdList={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, String.Join(",", orgnzIdList)))

            '組織IDをコンマ区切りで連結する
            Dim orgnzIds As String = String.Join(",", orgnzIdList)

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_123 */ ")
                .AppendLine("        T2.STALL_ID AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STALL_GROUP T1 ")
                .AppendLine("      , TB_M_STALL_STALL_GROUP T2 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.STALL_GROUP_ID = T2.STALL_GROUP_ID ")
                .AppendLine("    AND T1.ORGNZ_ID IN ( ")
                .AppendLine(orgnzIds)
                .AppendLine("                       ) ")
            End With

            Dim stallIdTable As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_123")
                query.CommandText = sql.ToString()
                stallIdTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E Count={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      stallIdTable.Count))

            Return stallIdTable
        End Function

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        ''' <summary>
        ''' ストール利用を仮置き状態に更新
        ''' </summary>
        ''' <param name="inStallUseId">ストール利用ID</param>
        ''' <param name="inFunctionId">機能ID</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inStaffcode">スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateStallUseForTemp(ByVal inStallUseId As Decimal, _
                                                         ByVal inFunctionId As String, _
                                                         ByVal inStaffCode As String, _
                                                         ByVal inNowDate As Date) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inStallUseId={1}, infunctionId={2}, inUpdateDateTime={3}, inStaffcode={4}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inStallUseId, _
                                      inFunctionId, _
                                      inNowDate, _
                                      inStaffCode))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* TABLETSMBCOMMONCLASS_124 */ ")
                .Append("       TB_T_STALL_USE ")
                .Append("    SET ")
                .Append("       STALL_USE_STATUS = :STALL_USE_STATUS_WAIT ")
                .Append("     , TEMP_FLG  = :TEMP_FLG ")         '仮置きフラグ
                .Append("     , ROW_UPDATE_DATETIME = :NOW_DATE ")       '行更新日時
                .Append("     , ROW_UPDATE_ACCOUNT =  :STF_CODE ")                '行更新アカウント
                .Append("     , ROW_UPDATE_FUNCTION = :FUNCTION_ID ")   '行更新機能
                .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
                .Append("  WHERE ")
                .Append("       STALL_USE_ID = :STALL_USE_ID ")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_124")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("NOW_DATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("STF_CODE", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("FUNCTION_ID", OracleDbType.NVarchar2, inFunctionId)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStallUseId)
                query.AddParameterWithTypeValue("STALL_USE_STATUS_WAIT", OracleDbType.NVarchar2, STALL_USE_STATUS_WAIT)

                'SQL実行
                Dim result As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. result={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result

            End Using

        End Function

        ''' <summary>
        ''' チップに紐づくJobの着工指示フラグを未指示に更新
        ''' </summary>
        ''' <param name="injobDtlId">作業内容ID</param>
        ''' <param name="inFunctionId">機能ID</param>
        ''' <param name="inStaffCode">スタッフコード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateJobInstructForNotInstructByJobDtlId(ByVal injobDtlId As Decimal, _
                                                         ByVal inFunctionId As String, _
                                                         ByVal inStaffCode As String, _
                                                         ByVal inNowDate As Date) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. injobDtlId={1}, inFunctionId={2}, inStaffCode={3}, inNowDate={4}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      injobDtlId, _
                                      inFunctionId, _
                                      inStaffCode, _
                                      inNowDate))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* TABLETSMBCOMMONCLASS_125 */ ")
                .Append("       TB_T_JOB_INSTRUCT ")
                .Append("    SET ")
                .Append("       STARTWORK_INSTRUCT_FLG  = :STARTWORK_INSTRUCT_FLG_OFF ")         '着工指示フラグ
                .Append("     , ROW_UPDATE_DATETIME = :NOW_DATE ")       '行更新日時
                .Append("     , ROW_UPDATE_ACCOUNT = :ACCOUNT ")                '行更新アカウント
                .Append("     , ROW_UPDATE_FUNCTION = :FUNCTION_ID ")   '行更新機能
                .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
                .Append("  WHERE ")
                .Append("       JOB_DTL_ID = :JOB_DTL_ID ")
                .Append("       AND STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_ON ")

            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_125")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("NOW_DATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("FUNCTION_ID", OracleDbType.NVarchar2, inFunctionId)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, injobDtlId)
                'SQL実行
                Dim result As Long = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. result={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result
            End Using
        End Function

        ''' <summary>
        ''' チップに紐づくJobの着工指示フラグを指示済みに更新
        ''' </summary>
        ''' <param name="injobDtlId">作業内容ID</param>
        ''' <param name="inFunctionId">機能ID</param>
        ''' <param name="inStaffCode">スタッフコード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateJobInstructForInstructByJobDtlId(ByVal injobDtlId As Decimal, _
                                                         ByVal inFunctionId As String, _
                                                         ByVal inStaffCode As String, _
                                                         ByVal inNowDate As Date) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. injobDtlId={1}, inFunctionId={2}, inStaffCode={3}, inNowDate={4}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      injobDtlId, _
                                      inFunctionId, _
                                      inStaffCode, _
                                      inNowDate))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* TABLETSMBCOMMONCLASS_126 */ ")
                .Append("       TB_T_JOB_INSTRUCT ")
                .Append("    SET ")
                .Append("       STARTWORK_INSTRUCT_FLG  = :STARTWORK_INSTRUCT_FLG_ON ")         '着工指示フラグ
                .Append("     , ROW_UPDATE_DATETIME = :NOW_DATE ")       '行更新日時
                .Append("     , ROW_UPDATE_ACCOUNT = :ACCOUNT ")                '行更新アカウント
                .Append("     , ROW_UPDATE_FUNCTION = :FUNCTION_ID ")   '行更新機能
                .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
                .Append("  WHERE ")
                .Append("       JOB_DTL_ID = :JOB_DTL_ID ")
                .Append("       AND STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_OFF ")

            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_126")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("NOW_DATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("FUNCTION_ID", OracleDbType.NVarchar2, inFunctionId)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, injobDtlId)

                'SQL実行
                Dim result As Long = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. result={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result
            End Using
        End Function
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
        
#End Region
        '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 END

#Region "作業指示テーブルからデータ取得"

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START

        ' ''' <summary>
        ' ''' 指定RO番号、枝番の未紐付いた作業指示情報を取得
        ' ''' </summary>
        ' ''' <param name="inROJobSeq">RO作業連番</param>
        ' ''' <param name="inRONum">RO番号</param>
        ' ''' <returns>反映件数</returns>
        ' ''' <remarks></remarks>
        'Public Function GetJobInstruct(ByVal inROJobSeq As Long, _
        '                               ByVal inRONum As String) As TabletSmbCommonClassJobInstructDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} START inROJobSeq={2} inRONum={3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , inROJobSeq _
        '               , inRONum))


        '    Dim Sql As New StringBuilder
        '    With Sql
        '        .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_047 */ ")
        '        .AppendLine("        T1.JOB_DTL_ID ")
        '        .AppendLine("      , T1.JOB_INSTRUCT_ID ")
        '        .AppendLine("      , T1.JOB_INSTRUCT_SEQ ")
        '        .AppendLine("      , T1.RO_NUM ")
        '        .AppendLine("      , T1.RO_SEQ AS RO_JOB_SEQ ")
        '        .AppendLine("      , T1.JOB_CD ")
        '        .AppendLine("      , T1.JOB_NAME ")
        '        .AppendLine("      , T1.STD_WORKTIME ")
        '        .AppendLine("      , T1.JOB_STF_GROUP_ID ")
        '        .AppendLine("      , T1.JOB_STF_GROUP_NAME ")
        '        .AppendLine("      , T1.STARTWORK_INSTRUCT_FLG ")
        '        .AppendLine("      , T1.OPERATION_TYPE_ID ")
        '        .AppendLine("      , T1.OPERATION_TYPE_NAME ")
        '        .AppendLine("      , T1.WORK_PRICE ")
        '        .AppendLine("      , T1.WORK_UNIT_PRICE ")
        '        .AppendLine("   FROM ")
        '        .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
        '        .AppendLine("  WHERE  ")
        '        .AppendLine("        T1.RO_NUM=:RO_NUM ")
        '        .AppendLine("   AND  T1.RO_SEQ=:RO_SEQ ")
        '        .AppendLine("   AND  T1.STARTWORK_INSTRUCT_FLG=:STARTWORK_INSTRUCT_FLG_OFF ")
        '    End With

        '    Using query As New DBSelectQuery(Of TabletSmbCommonClassJobInstructDataTable)("TABLETSMBCOMMONCLASS_047")
        '        query.CommandText = Sql.ToString()
        '        'SQLパラメータ設定値
        '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
        '        query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Long, inROJobSeq)
        '        query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))

        '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        '        Dim dt As TabletSmbCommonClassJobInstructDataTable = query.GetData()
        '        Return dt
        '    End Using

        'End Function

        ''' <summary>
        ''' 指定RO番号、枝番の未紐付いた作業指示情報を取得
        ''' </summary>
        ''' <param name="inROJobSeq">RO作業連番</param>
        ''' <param name="inRONum">RO番号</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>TabletSmbCommonClassJobInstructDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetJobInstruct(ByVal inROJobSeq As Long, _
                                       ByVal inRONum As String, _
                                       ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String) As TabletSmbCommonClassJobInstructDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START inROJobSeq={2} inRONum={3}, inDealerCode={4}, inBranchCode={5}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inROJobSeq _
                       , inRONum _
                       , inDealerCode _
                       , inBranchCode))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_047 */ ")
                .AppendLine("        T1.JOB_DTL_ID ")
                .AppendLine("      , T1.JOB_INSTRUCT_ID ")
                .AppendLine("      , T1.JOB_INSTRUCT_SEQ ")
                .AppendLine("      , T1.RO_NUM ")
                .AppendLine("      , T1.RO_SEQ AS RO_JOB_SEQ ")
                .AppendLine("      , T1.JOB_CD ")
                .AppendLine("      , T1.JOB_NAME ")
                .AppendLine("      , T1.STD_WORKTIME ")
                .AppendLine("      , T1.JOB_STF_GROUP_ID ")
                .AppendLine("      , T1.JOB_STF_GROUP_NAME ")
                .AppendLine("      , T1.STARTWORK_INSTRUCT_FLG ")
                .AppendLine("      , T1.OPERATION_TYPE_ID ")
                .AppendLine("      , T1.OPERATION_TYPE_NAME ")
                .AppendLine("      , T1.WORK_PRICE ")
                .AppendLine("      , T1.WORK_UNIT_PRICE ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
                .AppendLine("      , TB_T_JOB_DTL T2 ")

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("      , TB_T_STALL_USE T3 ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .AppendLine("    AND T1.RO_NUM = :RO_NUM ")
                .AppendLine("    AND T1.RO_SEQ = :RO_SEQ ")
                .AppendLine("    AND T1.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_OFF ")
                .AppendLine("    AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("    AND T2.BRN_CD = :BRN_CD ")

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                .AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("    AND EXISTS( ")
                .AppendLine("        SELECT ")
                .AppendLine("               1 ")
                .AppendLine("        FROM ")
                .AppendLine("               TB_T_STALL_USE T4 ")
                .AppendLine("        WHERE ")
                .AppendLine("               T4.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("        HAVING")
                .AppendLine("               MAX(T4.STALL_USE_ID) = T3.STALL_USE_ID) ")
                .AppendLine("    AND T3.TEMP_FLG = :TEMP_FLG_OFF ")
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassJobInstructDataTable)("TABLETSMBCOMMONCLASS_047")
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
                query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Long, inROJobSeq)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))

                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                query.AddParameterWithTypeValue("TEMP_FLG_OFF", OracleDbType.NVarchar2, Zero.ToString(CultureInfo.CurrentCulture))
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Dim dt As TabletSmbCommonClassJobInstructDataTable = query.GetData()
                Return dt
            End Using

        End Function
        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

        ''' <summary>
        ''' 指定作業内容IDの紐付いた作業指示番号と枝番を取得
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>反映件数</returns>
        ''' <remarks></remarks>
        Public Function GetJobInstructIdAndSeqByJobDtlId(ByVal inJobDtlId As Decimal) As TabletSmbCommonClassJobInstructDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START inJobDtlId={2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inJobDtlId))

            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_052 */ ")
                .AppendLine("        T1.JOB_INSTRUCT_ID ")
                .AppendLine("      , T1.JOB_INSTRUCT_SEQ ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_DTL_ID=:JOB_DTL_ID ")
                .AppendLine("   AND  T1.STARTWORK_INSTRUCT_FLG=:STARTWORK_INSTRUCT_FLG_ON ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassJobInstructDataTable)("TABLETSMBCOMMONCLASS_052")
                query.CommandText = Sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Dim dt As TabletSmbCommonClassJobInstructDataTable = query.GetData()
                Return dt
            End Using

        End Function

        ''' <summary>
        ''' 指定作業内容IDの紐付いたRO枝番を取得する
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>反映件数</returns>
        ''' <remarks></remarks>
        Public Function GetROJobSeqByJobDtlId(ByVal inJobDtlId As Decimal) As TabletSmbCommonClassJobInstructDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START inJobDtlId={2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inJobDtlId))

            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_050 */ ")
                .AppendLine("        DISTINCT(T1.RO_SEQ) AS RO_JOB_SEQ ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_DTL_ID=:JOB_DTL_ID ")
                .AppendLine("   AND  T1.STARTWORK_INSTRUCT_FLG=:STARTWORK_INSTRUCT_FLG_ON ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassJobInstructDataTable)("TABLETSMBCOMMONCLASS_050")
                query.CommandText = Sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))

                Dim dt As TabletSmbCommonClassJobInstructDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.E Return count is {1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))
                Return dt
            End Using

        End Function

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        ' ''' <summary>
        ' ''' RO作業番号、RO枝番により、パラinJobDtlId以外の作業内容IDを取得する
        ' ''' </summary>
        ' ''' <param name="inJobDtlId">作業内容ID</param>
        ' ''' <param name="inRONum">RO番号</param>
        ' ''' <param name="inRoJobSeqNum">RO枝番</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetJobDtlIdByROInfo(ByVal inJobDtlId As Decimal, _
        '                                    ByVal inRONum As String, _
        '                                    ByVal inROJobSeqNum As Long) As TabletSmbCommonClassJobInstructDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                             "{0}_S. inRONum={1}, inRoJobSeqNum={2}", _
        '                             System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                             inRONum, _
        '                             inROJobSeqNum))

        '    ' DBSelectQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of TabletSmbCommonClassJobInstructDataTable)("TABLETSMBCOMMONCLASS_051")

        '        Dim sql As New StringBuilder

        '        ' SQL文の作成
        '        With sql
        '            .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_051 */ ")
        '            .AppendLine("      , T1.JOB_DTL_ID ")
        '            .AppendLine("   FROM ")
        '            .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
        '            .AppendLine("  WHERE  ")
        '            .AppendLine("        T1.RO_NUM=:RO_NUM ")
        '            .AppendLine("    AND T1.RO_SEQ=:RO_SEQ ")
        '            .AppendLine("    AND T1.JOB_DTL_ID<>:JOB_DTL_ID ")
        '        End With
        '        query.CommandText = sql.ToString()

        '        'SQLパラメータ設定
        '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
        '        query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Long, inROJobSeqNum)
        '        query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)

        '        Dim dataTable As TabletSmbCommonClassJobInstructDataTable = query.GetData()
        '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                                  "{0}_E Count={1}", _
        '                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                  dataTable.Rows.Count))

        '        ' 検索結果の返却
        '        Return dataTable

        '    End Using

        'End Function
        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>
        ''' 指定JobのRO連番を取得
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <param name="inInstructId">作業指示ID</param>
        ''' <param name="inInstructSeq">作業連番</param>
        ''' <returns>RO連番を含む数値テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetROSeqByJob(ByVal inJobDtlId As Decimal, _
                                      ByVal inInstructId As String, _
                                      ByVal inInstructSeq As Long) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} START inJobDtlId={2} inInstructId={3} inInstructSeq={4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , inJobDtlId _
                                    , inInstructId _
                                    , inInstructSeq))


            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_069 */ ")
                .AppendLine("        T1.RO_SEQ AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_DTL_ID=:JOB_DTL_ID ")
                .AppendLine("   AND  T1.JOB_INSTRUCT_ID=:JOB_INSTRUCT_ID ")
                .AppendLine("   AND  T1.JOB_INSTRUCT_SEQ=:JOB_INSTRUCT_SEQ ")
                .AppendLine("   AND  T1.STARTWORK_INSTRUCT_FLG=:STARTWORK_INSTRUCT_FLG_ON ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_069")

                query.CommandText = Sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.NVarchar2, inJobDtlId)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inInstructId)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inInstructSeq)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))


                Dim dt As TabletSmbCommonClassNumberValueDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                        , "{0}.End Query Count={1}" _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , dt.Count))

                Return dt

            End Using

        End Function

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#End Region

#Region "作業実績テーブルに対する処理"
        ''' <summary>
        ''' 作業単位で作業ステータスを取得する
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>反映件数</returns>
        ''' <remarks></remarks>
        Public Function GetJobStatusByJob(ByVal inJobDtlId As Decimal) As TabletSmbCommonClassJobResultDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} START inJobDtlId={2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , inJobDtlId))

            '2018/11/26 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している START
            'Dim Sql As New StringBuilder
            'With Sql
                '.AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_059 */ ")
                '.AppendLine("        T2.JOB_DTL_ID ")
                '.AppendLine("      , T2.JOB_INSTRUCT_ID ")
                '.AppendLine("      , T2.JOB_INSTRUCT_SEQ ")
                '.AppendLine("      , T3.JOB_STATUS ")
                '.AppendLine(" FROM ")
                '.AppendLine("        TB_T_JOB_DTL T1 ")
                '.AppendLine("      , TB_T_JOB_INSTRUCT T2 ")
                '.AppendLine("      , (   ")
                '.AppendLine("           SELECT ")   '作業単位で最新の作業ステータスを取得
                '.AppendLine("                  W1.JOB_DTL_ID ")
                '.AppendLine("                , W1.JOB_INSTRUCT_ID ")
                '.AppendLine("                , W1.JOB_INSTRUCT_SEQ ")
                '.AppendLine("                , W1.JOB_STATUS ")
                '.AppendLine("            FROM ")
                '.AppendLine("                TB_T_JOB_RESULT W1  ")
                '.AppendLine("           WHERE ")
                '.AppendLine("                W1.JOB_RSLT_ID IN ")
                '.AppendLine("                               ( ")
                '.AppendLine("                                   SELECT ")       '作業単位でデータを取得
                '.AppendLine("                                         MAX(S1.JOB_RSLT_ID) ")
                '.AppendLine("                                    FROM ")
                '.AppendLine("                                         TB_T_JOB_RESULT S1 ")
                '.AppendLine("                                   WHERE  ")
                '.AppendLine("                                        S1.JOB_DTL_ID = :JOB_DTL_ID  ")
                '.AppendLine("                                   GROUP BY ")
                '.AppendLine("                                           ( ")
                '.AppendLine("                                               S1.JOB_DTL_ID ")
                '.AppendLine("                                             , S1.JOB_INSTRUCT_ID ")
                '.AppendLine("                                             , S1.JOB_INSTRUCT_SEQ ")
                '.AppendLine("                                           ) ")
                '.AppendLine("                                ) ")
                '.AppendLine("        ) T3")
                '.AppendLine("  WHERE  ")
                '.AppendLine("        T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                '.AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID(+) ")
                '.AppendLine("    AND T2.JOB_INSTRUCT_ID = T3.JOB_INSTRUCT_ID(+) ")
                '.AppendLine("    AND T2.JOB_INSTRUCT_SEQ = T3.JOB_INSTRUCT_SEQ(+) ")
                '.AppendLine("    AND T1.JOB_DTL_ID = :JOB_DTL_ID ")
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                '.AppendLine("    AND T2.STARTWORK_INSTRUCT_FLG=:STARTWORK_INSTRUCT_FLG_ON ")
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            'End With

            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_059 */ ")
                .AppendLine("        T2.JOB_DTL_ID ")
                .AppendLine("      , T2.JOB_INSTRUCT_ID ")
                .AppendLine("      , T2.JOB_INSTRUCT_SEQ ")
                .AppendLine("      , T4.JOB_STATUS ")
                .AppendLine(" FROM ")
                .AppendLine("         TB_T_JOB_INSTRUCT T2 ")
                .AppendLine("        , (    ")
                .AppendLine("             SELECT ")
                .AppendLine("                     W1.JOB_DTL_ID ")
                .AppendLine("                   , W1.JOB_INSTRUCT_ID ")
                .AppendLine("                   , W1.JOB_INSTRUCT_SEQ ")
                .AppendLine("                   , MAX(W1.JOB_RSLT_ID) AS JOB_RSLT_ID ")
                .AppendLine("             FROM ")
                .AppendLine("                   TB_T_JOB_RESULT W1  ")
                .AppendLine("             GROUP BY ")
                .AppendLine("                     W1.JOB_DTL_ID ")
                .AppendLine("                   , W1.JOB_INSTRUCT_ID ")
                .AppendLine("                   , W1.JOB_INSTRUCT_SEQ ")
                .AppendLine("         ) T3 ")
                .AppendLine("        , TB_T_JOB_RESULT T4  ")
                .AppendLine(" WHERE ")
                .AppendLine("        T2.JOB_DTL_ID = T3.JOB_DTL_ID(+) ")
                .AppendLine("    AND T2.JOB_INSTRUCT_ID = T3.JOB_INSTRUCT_ID(+) ")
                .AppendLine("    AND T2.JOB_INSTRUCT_SEQ = T3.JOB_INSTRUCT_SEQ(+) ")
                .AppendLine("    AND T3.JOB_RSLT_ID = T4.JOB_RSLT_ID(+) ")
                .AppendLine("    AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
                .AppendLine("    AND T2.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_ON ")

            End With
            '2018/11/26 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している END

            Using query As New DBSelectQuery(Of TabletSmbCommonClassJobResultDataTable)("TABLETSMBCOMMONCLASS_059")
                query.CommandText = Sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_ON", OracleDbType.NVarchar2, One.ToString(CultureInfo.CurrentCulture))
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                Dim dt As TabletSmbCommonClassJobResultDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.END Return count is {1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))

                Return dt
            End Using

        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 指定作業の最新作業ステータスを取得
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <param name="inJobInstructId">作業指示ID</param>
        ''' <param name="inJobInstructSeq">作業指示枝番</param>
        ''' <returns>作業実績ステータスDT</returns>
        ''' <remarks></remarks>
        Public Function GetSingleJobStatus(ByVal inJobDtlId As Decimal, _
                                           ByVal inJobInstructId As String, _
                                           ByVal inJobInstructSeq As Long) As TabletSmbCommonClassJobStatusDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} START inJobDtlId={2} inServiceInId={3} inJobInstructSeq={4}" _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                      , inJobDtlId _
                                      , inJobInstructId _
                                      , inJobInstructSeq))

            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_066 */ ")
                .AppendLine("        T1.JOB_RSLT_ID ")
                .AppendLine("     ,  T1.JOB_STATUS ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_RESULT T1 ")
                .AppendLine("     ,  (SELECT ")
                .AppendLine("                MAX(JOB_RSLT_ID) AS MAX_JOB_RSLT_ID ")
                .AppendLine("           FROM ")
                .AppendLine("             　  TB_T_JOB_RESULT T2 ")
                .AppendLine("          WHERE ")
                .AppendLine("                T2.JOB_DTL_ID =:JOB_DTL_ID ")
                .AppendLine("            AND T2.JOB_INSTRUCT_ID =:JOB_INSTRUCT_ID ")
                .AppendLine("            AND T2.JOB_INSTRUCT_SEQ =:JOB_INSTRUCT_SEQ ")
                .AppendLine("              )T3 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_RSLT_ID=T3.MAX_JOB_RSLT_ID ")

            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassJobStatusDataTable)("TABLETSMBCOMMONCLASS_066")
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inJobInstructId)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inJobInstructSeq)

                Dim dt As TabletSmbCommonClassJobStatusDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.E OUT:Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))

                Return dt

            End Using

        End Function

        ''' <summary>
        ''' 指定チップに紐づく全作業の最新作業実績情報を取得
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>最新作業実績情報テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetAllJobRsltInfoByJobDtlId(ByVal inJobDtlId As Decimal) As TabletSmbCommonClassJobStatusDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} START inJobDtlId={2} " _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                      , inJobDtlId))

            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_068 */ ")
                .AppendLine("        T1.JOB_RSLT_ID ")
                .AppendLine("      , T1.JOB_INSTRUCT_ID ")
                .AppendLine("      , T1.JOB_INSTRUCT_SEQ ")
                .AppendLine("      , T1.JOB_STATUS ")
                .AppendLine("      , T1.STOP_REASON_TYPE ")
                .AppendLine("      , T1.STOP_MEMO ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_RESULT T1 ")
                .AppendLine("     ,  (SELECT ")
                .AppendLine("                MAX(JOB_RSLT_ID) AS MAX_JOB_RSLT_ID ")
                .AppendLine("           FROM ")
                .AppendLine("                TB_T_JOB_INSTRUCT T2 ")
                .AppendLine("             ,  TB_T_JOB_RESULT T3 ")
                .AppendLine("          WHERE ")
                .AppendLine("                T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("            AND T2.JOB_INSTRUCT_ID = T3.JOB_INSTRUCT_ID ")
                .AppendLine("            AND T2.JOB_INSTRUCT_SEQ = T3.JOB_INSTRUCT_SEQ ")
                .AppendLine("            AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
                .AppendLine("            AND T2.STARTWORK_INSTRUCT_FLG = N'1' ")
                .AppendLine("       GROUP BY ")
                .AppendLine("                T3.JOB_DTL_ID ")
                .AppendLine("             ,  T3.JOB_INSTRUCT_ID ")
                .AppendLine("             ,  T3.JOB_INSTRUCT_SEQ) T4 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_RSLT_ID = T4.MAX_JOB_RSLT_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassJobStatusDataTable)("TABLETSMBCOMMONCLASS_068")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)

                'SQL実行
                Dim dt As TabletSmbCommonClassJobStatusDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.E OUT:Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))

                '返却
                Return dt

            End Using

        End Function
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ''' <summary>
        ''' 作業実績テーブルに一行を挿入する
        ''' </summary>
        ''' <param name="inJobRsltDataRow">作業実績テーブルの１行データ</param>
        ''' <param name="inUpdateDate">更新日時</param>
        ''' <param name="inStaffCode">スタッフコード</param>
        ''' <param name="inUpdateFunction">更新ファンクション</param>
        ''' <returns>1:正常終了、その他:更新失敗</returns>
        Public Function InsertJobResult(ByVal inJobRsltDataRow As TabletSmbCommonClassJobResultRow, _
                                        ByVal inUpdateDate As Date, _
                                        ByVal inStaffCode As String, _
                                        ByVal inUpdateFunction As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S", System.Reflection.MethodBase.GetCurrentMethod.Name))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_307 */ ")
                .AppendLine("   INTO TB_T_JOB_RESULT (")
                .AppendLine("           JOB_RSLT_ID ")
                .AppendLine("         , JOB_DTL_ID ")
                .AppendLine("         , JOB_INSTRUCT_ID ")
                .AppendLine("         , JOB_INSTRUCT_SEQ ")
                .AppendLine("         , STALL_ID ")
                .AppendLine("         , RSLT_START_DATETIME ")
                .AppendLine("         , RSLT_END_DATETIME ")
                .AppendLine("         , JOB_STATUS ")
                .AppendLine("         , ROW_CREATE_DATETIME ")
                .AppendLine("         , ROW_CREATE_ACCOUNT ")
                .AppendLine("         , ROW_CREATE_FUNCTION ")
                .AppendLine("         , ROW_UPDATE_DATETIME ")
                .AppendLine("         , ROW_UPDATE_ACCOUNT ")
                .AppendLine("         , ROW_UPDATE_FUNCTION ")
                .AppendLine("         , ROW_LOCK_VERSION ) ")
                .AppendLine(" VALUES ( ")
                .AppendLine("           :JOB_RSLT_ID ")
                .AppendLine("         , :JOB_DTL_ID ")
                .AppendLine("         , :JOB_INSTRUCT_ID ")
                .AppendLine("         , :JOB_INSTRUCT_SEQ ")
                .AppendLine("         , :STALL_ID ")
                .AppendLine("         , :RSLT_START_DATETIME ")
                .AppendLine("         , :RSLT_END_DATETIME ")
                .AppendLine("         , :JOB_STATUS ")
                .AppendLine("         , :UPDATE_DATETIME ")
                .AppendLine("         , :UPDATE_ACCOUNT ")
                .AppendLine("         , :UPDATE_FUNCTION ")
                .AppendLine("         , :UPDATE_DATETIME ")
                .AppendLine("         , :UPDATE_ACCOUNT ")
                .AppendLine("         , :UPDATE_FUNCTION ")
                .AppendLine("         , 0 ) ")
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_307")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("JOB_RSLT_ID", OracleDbType.Decimal, inJobRsltDataRow.JOB_RSLT_ID)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobRsltDataRow.JOB_DTL_ID)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inJobRsltDataRow.JOB_INSTRUCT_ID)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inJobRsltDataRow.JOB_INSTRUCT_SEQ)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, inJobRsltDataRow.STALL_ID)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, inJobRsltDataRow.RSLT_START_DATETIME)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, inJobRsltDataRow.RSLT_END_DATETIME)
                query.AddParameterWithTypeValue("JOB_STATUS", OracleDbType.NVarchar2, inJobRsltDataRow.JOB_STATUS)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDate)
                query.AddParameterWithTypeValue("UPDATE_ACCOUNT", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateFunction)

                'SQL実行(影響行数を返却)
                Dim queryCount As Integer = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        ''' <summary>
        ''' 作業実績削除テーブル：登録
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <param name="inUpdateDate">更新日時</param>
        ''' <param name="inAccount">スタフコード</param>
        ''' <param name="inUpdateFunction">更新ファンクション</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function InsertJobResultDel(ByVal inJobDtlId As Decimal, _
                                           ByVal inUpdateDate As Date, _
                                           ByVal inAccount As String, _
                                           ByVal inUpdateFunction As String) As Long

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , inJobDtlId.ToString(CultureInfo.CurrentCulture) _
                                    , inUpdateDate.ToString(CultureInfo.CurrentCulture) _
                                    , inAccount _
                                    , inUpdateFunction))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_308")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" INSERT /* TABLETSMBCOMMONCLASS_308 */ ")
                    .AppendLine("   INTO TB_T_JOB_RESULT_DEL (")
                    .AppendLine("           JOB_RSLT_ID ")
                    .AppendLine("         , JOB_DTL_ID ")
                    .AppendLine("         , JOB_INSTRUCT_ID ")
                    .AppendLine("         , JOB_INSTRUCT_SEQ ")
                    .AppendLine("         , STALL_ID ")
                    .AppendLine("         , RSLT_START_DATETIME ")
                    .AppendLine("         , RSLT_END_DATETIME ")
                    .AppendLine("         , JOB_STATUS ")
                    .AppendLine("         , STOP_REASON_TYPE ")
                    .AppendLine("         , STOP_MEMO ")
                    .AppendLine("         , ROW_CREATE_DATETIME ")
                    .AppendLine("         , ROW_CREATE_ACCOUNT ")
                    .AppendLine("         , ROW_CREATE_FUNCTION ")
                    .AppendLine("         , ROW_UPDATE_DATETIME ")
                    .AppendLine("         , ROW_UPDATE_ACCOUNT ")
                    .AppendLine("         , ROW_UPDATE_FUNCTION ")
                    .AppendLine("         , ROW_LOCK_VERSION ) ")
                    .AppendLine(" SELECT ")
                    .AppendLine("           JOB_RSLT_ID ")
                    .AppendLine("         , JOB_DTL_ID ")
                    .AppendLine("         , JOB_INSTRUCT_ID ")
                    .AppendLine("         , JOB_INSTRUCT_SEQ ")
                    .AppendLine("         , STALL_ID ")
                    .AppendLine("         , RSLT_START_DATETIME ")
                    .AppendLine("         , RSLT_END_DATETIME ")
                    .AppendLine("         , JOB_STATUS ")
                    .AppendLine("         , STOP_REASON_TYPE ")
                    .AppendLine("         , STOP_MEMO ")
                    .AppendLine("         , :UPDATE_DATETIME AS ROW_CREATE_DATETIME ")
                    .AppendLine("         , :UPDATE_ACCOUNT AS ROW_CREATE_ACCOUNT ")
                    .AppendLine("         , :UPDATE_FUNCTION AS ROW_CREATE_FUNCTION ")
                    .AppendLine("         , :UPDATE_DATETIME AS ROW_UPDATE_DATETIME ")
                    .AppendLine("         , :UPDATE_ACCOUNT AS ROW_UPDATE_ACCOUNT ")
                    .AppendLine("         , :UPDATE_FUNCTION AS ROW_UPDATE_FUNCTION ")
                    .AppendLine("         , :ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")
                    .AppendLine(" FROM      ")
                    .AppendLine("      TB_T_JOB_RESULT T1    ")
                    .AppendLine(" WHERE      ")
                    .AppendLine("      T1.JOB_DTL_ID=:JOB_DTL_ID     ")
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                    .AppendLine("  AND JOB_STATUS = N'0' ")
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDate)
                query.AddParameterWithTypeValue("UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateFunction)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, 0)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return queryCount
            End Using
        End Function

        ''' <summary>
        ''' 作業内容IDにより、作業実績テーブルからレコードを削除する
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>削除行数</returns>
        ''' <remarks></remarks>
        Public Function DeleteInsertJobResult(ByVal inJobDtlId As Decimal) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inJobDtlId={1} " _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inJobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" DELETE /* TABLETSMBCOMMONCLASS_404 */ ")
                .AppendLine("   FROM  TB_T_JOB_RESULT  ")
                .AppendLine("  WHERE JOB_DTL_ID = :JOB_DTL_ID ")
                .AppendLine("    AND JOB_STATUS = N'0' ")       '作業中の実績を削除
            End With

            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_404")

                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)

                query.CommandText = sql.ToString()
                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        ''' <summary>
        ''' 中断、終了、日跨ぎ終了時、作業実績テーブルの更新操作
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <param name="inRsltEndDate">実績終了日時</param>
        ''' <param name="inJobStatus">作業ステータス</param>
        ''' <param name="inUpdateDate">更新日時</param>
        ''' <param name="inAccount">スタフコード</param>
        ''' <param name="inUpdateFunction">更新ファンクション</param>
        ''' <param name="inStopReasonType">中断理由区分</param>
        ''' <param name="inStopMemo">中断メモ</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateJobRsltOnFinish(ByVal inJobDtlId As Decimal, _
                                              ByVal inRsltEndDate As Date, _
                                              ByVal inJobStatus As String, _
                                              ByVal inAccount As String, _
                                              ByVal inUpdateFunction As String, _
                                              ByVal inUpdateDate As Date, _
                                              Optional ByVal inStopReasonType As String = Nothing, _
                                              Optional ByVal inStopMemo As String = Nothing) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inJobDtlId={1}, inRsltEndDate={2}, inJobStatus={3}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inJobDtlId, _
                                      inRsltEndDate, _
                                      inJobStatus))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_225")
                'SQL組み立て
                Dim sql As New StringBuilder
                sql.AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_225 */ ")
                sql.AppendLine("        TB_T_JOB_RESULT ")
                sql.AppendLine("    SET ")
                sql.AppendLine("        ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                sql.AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                sql.AppendLine("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
                sql.AppendLine("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                sql.AppendLine("      , RSLT_END_DATETIME = :RSLT_END_DATETIME ")
                sql.AppendLine("      , JOB_STATUS = :JOB_STATUS ")
                '中断の場合、中断理由区分と中断メモを設定する
                If JobStatusStop.Equals(inJobStatus) Then

                    If Not String.IsNullOrEmpty(inStopReasonType) Then
                        sql.AppendLine("      , STOP_REASON_TYPE = :STOP_REASON_TYPE ")
                    End If

                    If Not String.IsNullOrEmpty(inStopMemo) Then
                        sql.AppendLine("      , STOP_MEMO = :STOP_MEMO ")
                    End If

                End If
                sql.AppendLine("  WHERE  ")
                sql.AppendLine("     RSLT_END_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                sql.AppendLine("   AND (JOB_INSTRUCT_ID, JOB_DTL_ID, JOB_INSTRUCT_SEQ) IN  ")
                sql.AppendLine("          (  ")
                sql.AppendLine("            SELECT  ")
                sql.AppendLine("                   T1.JOB_INSTRUCT_ID  ")
                sql.AppendLine("                 , T1.JOB_DTL_ID  ")
                sql.AppendLine("                 , T1.JOB_INSTRUCT_SEQ  ")
                sql.AppendLine("              FROM   ")
                sql.AppendLine("                   TB_T_JOB_INSTRUCT T1  ")
                sql.AppendLine("             WHERE  ")
                sql.AppendLine("                   T1.JOB_DTL_ID = :JOB_DTL_ID  ")
                sql.AppendLine("          )  ")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateFunction)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inUpdateDate)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, inRsltEndDate)
                query.AddParameterWithTypeValue("JOB_STATUS", OracleDbType.NVarchar2, inJobStatus)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                '中断の場合、中断理由区分と中断メモを設定する
                If JobStatusStop.Equals(inJobStatus) Then
                    If Not String.IsNullOrEmpty(inStopReasonType) Then
                        query.AddParameterWithTypeValue("STOP_REASON_TYPE", OracleDbType.NVarchar2, inStopReasonType)
                    End If

                    If Not String.IsNullOrEmpty(inStopMemo) Then
                        query.AddParameterWithTypeValue("STOP_MEMO", OracleDbType.NVarchar2, inStopMemo)
                    End If
                End If

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>
        ''' 単独なJobの作業実績更新(SingleStopとSingleFinish時)
        ''' </summary>
        ''' <param name="inJobStatusDataRow">作業実績ステータスデータ行</param>
        ''' <param name="inRsltEndDateTime">作業実績終了日時</param>
        ''' <param name="inUpdateAccount">更新アカウント</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inUpdateProgramId">更新機能ID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateSingleJobResultByJobStopFinish(ByVal inJobStatusDataRow As TabletSmbCommonClassJobStatusRow, _
                                                             ByVal inRsltEndDateTime As Date, _
                                                             ByVal inUpdateAccount As String, _
                                                             ByVal inUpdateDateTime As Date, _
                                                             ByVal inUpdateProgramId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. JobDetailId={1}, JobStatus={2}, StopReasonType={3}, StopMemo={4}, inUpdateAccount={5}, inUpdateDateTime={6}, inUpdateProgramId={7}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inJobStatusDataRow.JOB_DTL_ID, _
                                      inJobStatusDataRow.JOB_STATUS, _
                                      inJobStatusDataRow.STOP_REASON_TYPE, _
                                      inJobStatusDataRow.STOP_MEMO, _
                                      inUpdateAccount, _
                                      inUpdateDateTime, _
                                      inUpdateProgramId))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_231 */ ")
                .AppendLine("        TB_T_JOB_RESULT ")
                .AppendLine("    SET ")
                .AppendLine("        JOB_STATUS = :JOB_STATUS ")                     '作業ステータス
                .AppendLine("      , STOP_REASON_TYPE = :STOP_REASON_TYPE ")         '中断理由区分
                .AppendLine("      , STOP_MEMO = :STOP_MEMO ")                       '中断MEMO
                .AppendLine("      , RSLT_END_DATETIME = :RSLT_END_DATETIME ")       '作業実績終了日時
                .AppendLine("      , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")       '行更新日時
                .AppendLine("      , ROW_UPDATE_ACCOUNT = :ACCOUNT ")                '行更新アカウント
                .AppendLine("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")   '行更新機能
                .AppendLine("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
                .AppendLine("  WHERE ")
                .AppendLine("        JOB_DTL_ID= :JOB_DTL_ID  ")
                .AppendLine("    AND JOB_INSTRUCT_ID= :JOB_INSTRUCT_ID  ")
                .AppendLine("    AND JOB_INSTRUCT_SEQ= :JOB_INSTRUCT_SEQ  ")
                .AppendLine("    AND RSLT_END_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS')  ")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_231")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("JOB_STATUS", OracleDbType.NVarchar2, inJobStatusDataRow.JOB_STATUS)
                query.AddParameterWithTypeValue("STOP_REASON_TYPE", OracleDbType.NVarchar2, inJobStatusDataRow.STOP_REASON_TYPE)
                query.AddParameterWithTypeValue("STOP_MEMO", OracleDbType.NVarchar2, inJobStatusDataRow.STOP_MEMO)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, inRsltEndDateTime)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inUpdateAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateProgramId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobStatusDataRow.JOB_DTL_ID)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inJobStatusDataRow.JOB_INSTRUCT_ID)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inJobStatusDataRow.JOB_INSTRUCT_SEQ)
                'SQL実行
                Dim result As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. UpdateRecordCount={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result

            End Using

        End Function
        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

#End Region
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発　START
#Region "ROステータスの情報取得、更新"

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        ' ''' <summary>
        ' ''' ROステータスの情報取得
        ' ''' </summary>
        ' ''' <param name="inRONum">RO番号</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetROStatusInfo(ByVal inRONum As String) As TabletSmbCommonClassROStatusDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '     "{0}_S. inRONum={1}", _
        '     System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '     inRONum))
        ''' <summary>
        ''' ROステータスの情報取得
        ''' </summary>
        ''' <param name="inRONum">RO番号</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetROStatusInfo(ByVal inRONum As String, _
                                        ByVal inDealerCode As String, _
                                        ByVal inBranchCode As String) As TabletSmbCommonClassROStatusDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                     "{0}_Start. inRONum={1}, inDealerCode={2}, inBranchCode={3}", _
                                     System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                     inRONum, _
                                     inDealerCode, _
                                     inBranchCode))

            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of TabletSmbCommonClassROStatusDataTable)("TABLETSMBCOMMONCLASS_049")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine(" SELECT ")
                    .AppendLine("  /* TABLETSMBCOMMONCLASS_049 */ ")
                    .AppendLine("      T1.RO_NUM ")
                    .AppendLine("  ,   T1.RO_SEQ AS RO_JOB_SEQ ")
                    .AppendLine("  ,   T1.RO_STATUS ")
                    .AppendLine(" FROM ")
                    .AppendLine("      TB_T_RO_INFO T1 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("      T1.RO_NUM=:RO_NUM ")
                    .AppendLine("      AND T1.RO_STATUS<>N'99' ")
                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                    .AppendLine("      AND DLR_CD = :DLR_CD")   '指定販売店のデータを取得
                    .AppendLine("      AND BRN_CD = :BRN_CD")
                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END
                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End.", System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        ' ''' <summary>
        ' ''' 指定RONUMのROステータスを取得する
        ' ''' </summary>
        ' ''' <param name="roNums">RO NUM</param>
        ' ''' <returns></returns>
        'Public Function GetROStatusByRONum(ByVal roNums As String) As TabletSmbCommonClassROInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                              "{0}_S. roNums={1}", _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              roNums))

        ''' <summary>
        ''' 指定RONUMのROステータスを取得する
        ''' </summary>
        ''' <param name="roNums">RO NUM</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns></returns>
        Public Function GetROStatusByRONum(ByVal roNums As String, _
                                           ByVal inDealerCode As String, _
                                           ByVal inBranchCode As String) As TabletSmbCommonClassROInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_Start. roNums={1}, inDealerCode={2}, inBranchCode={3}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      roNums, _
                                      inDealerCode, _
                                      inBranchCode))
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_048 */ ")
                .AppendLine("        RO_NUM ")
                .AppendLine("      , RO_STATUS ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_RO_INFO ")
                .AppendLine("  WHERE RO_NUM  ")
                .AppendLine("   IN  ( ")
                .AppendLine(roNums)
                .AppendLine("       ) ")
                .AppendLine("   AND RO_STATUS <> N'99' ")   'R/Oキャンセルのレコードは除く
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                .AppendLine("   AND DLR_CD = :DLR_CD")   '指定販売店のデータを取得
                .AppendLine("   AND BRN_CD = :BRN_CD")
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END
            End With

            Dim getTable As TabletSmbCommonClassROInfoDataTable

            Using query As New DBSelectQuery(Of TabletSmbCommonClassROInfoDataTable)("TABLETSMBCOMMONCLASS_048")
                query.CommandText = sql.ToString()

                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                        "{0}_End. RowCount={1}", _
                        System.Reflection.MethodBase.GetCurrentMethod.Name, _
                        getTable.Rows.Count))

            Return getTable

        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 指定作業内容IDの関連ROステータスを更新する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <param name="inROStatus">ROステータス</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inStaffCode">更新スタッフ</param>
        ''' <param name="inUpdateFunction">更新画面ID</param>
        ''' <param name="inROSeqs">RO作業枝番</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateROStatusByJobDtlId(ByVal inSvcinId As Decimal, _
                                                 ByVal inJobDtlId As Decimal, _
                                                 ByVal inROStatus As String, _
                                                 ByVal inUpdateDateTime As Date, _
                                                 ByVal inStaffCode As String, _
                                                 ByVal inUpdateFunction As String, _
                                                 Optional ByVal inROSeqs As String = Nothing) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inJobDtlId={1}, inROStatus={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, inJobDtlId, inROStatus))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_222")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_222 */ ")
                    .AppendLine("        TB_T_RO_INFO ")
                    .AppendLine("    SET ")
                    .AppendLine("        RO_STATUS = :RO_STATUS ")
                    .AppendLine("     ,  ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("     ,  ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                    .AppendLine("     ,  ROW_UPDATE_FUNCTION = :UPDATE_FUNCTION ")
                    .AppendLine("     ,  ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("  WHERE  ")
                    .AppendLine("        (RO_NUM,RO_SEQ) IN    ")
                    .AppendLine("                   ( ")
                    .AppendLine("                        SELECT ")
                    .AppendLine("                               S1.RO_NUM ")
                    .AppendLine("                             , S1.RO_SEQ ")
                    .AppendLine("                        FROM ")
                    .AppendLine("                               TB_T_JOB_INSTRUCT S1 ")
                    .AppendLine("                        WHERE ")
                    .AppendLine("                                   S1.JOB_DTL_ID= :JOB_DTL_ID ")
                    .AppendLine("                               AND S1.STARTWORK_INSTRUCT_FLG = N'1' ")
                    If Not IsNothing(inROSeqs) Then
                        .AppendLine("                           AND S1.RO_SEQ IN ( ")
                        .AppendLine(inROSeqs)
                        .AppendLine("                                            ) ")
                    End If
                    .AppendLine("                   ) ")
                    .AppendLine("        AND SVCIN_ID = :SVCIN_ID ")
                    .AppendLine("        AND RO_STATUS <> :RO_STATUS ")     '同じ値なら、更新しない
                    .AppendLine("        AND RO_STATUS <> N'99' ")          'R/Oキャンセルのレコードは除く
                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, inROStatus)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateFunction)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>
        ''' 指定作業のROステータスを更新する
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <param name="inInstructId">作業指示ID</param>
        ''' <param name="inInstructSeq">作業指示連番</param>
        ''' <param name="inROStatus">ROステータス</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inStaffCode">更新スタッフ</param>
        ''' <param name="inUpdateFunction">更新画面ID</param>
        ''' <returns>更新行数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/06/15 TMEJ 小澤 TR-SVT-TMT-20150612-001「納車済みのチップが突然、SAメイン画面に表示された」対応
        ''' </history>
        Public Function UpdateROStatusByJob(ByVal inJobDtlId As Decimal, _
                                            ByVal inInstructId As String, _
                                            ByVal inInstructSeq As Long, _
                                            ByVal inROStatus As String, _
                                            ByVal inUpdateDateTime As Date, _
                                            ByVal inStaffCode As String, _
                                            ByVal inUpdateFunction As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.Start. inJobDtlId={1}, inInstructId={2}, inInstructSeq={3}, inROStatus={4}, inStaffCode={5}, inUpdateDateTime={6}, inUpdateFunction={7}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inJobDtlId, _
                                      inInstructId, _
                                      inInstructSeq, _
                                      inROStatus, _
                                      inStaffCode, _
                                      inUpdateDateTime, _
                                      inUpdateFunction))

            'SQL組み立て
            Dim sql As New StringBuilder

            With sql

                .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_224 */ ")
                .AppendLine("        TB_T_RO_INFO ")
                .AppendLine("    SET ")
                .AppendLine("        RO_STATUS = :RO_STATUS ")
                .AppendLine("      , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                .AppendLine("      , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                .AppendLine("      , ROW_UPDATE_FUNCTION = :UPDATE_FUNCTION ")
                .AppendLine("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                .AppendLine("  WHERE  ")

                '2015/06/15 TMEJ 小澤 TR-SVT-TMT-20150612-001「納車済みのチップが突然、SAメイン画面に表示された」対応 START

                '.AppendLine("        (RO_NUM,RO_SEQ) =    ")
                '.AppendLine("                   ( ")
                '.AppendLine("                        SELECT ")
                '.AppendLine("                               S1.RO_NUM ")
                '.AppendLine("                             , S1.RO_SEQ ")
                '.AppendLine("                        FROM ")
                '.AppendLine("                               TB_T_JOB_INSTRUCT S1 ")
                '.AppendLine("                        WHERE ")
                '.AppendLine("                               S1.JOB_DTL_ID= :JOB_DTL_ID ")
                '.AppendLine("                          AND  S1.JOB_INSTRUCT_ID = :JOB_INSTRUCT_ID ")
                '.AppendLine("                          AND  S1.JOB_INSTRUCT_SEQ = :JOB_INSTRUCT_SEQ ")
                '.AppendLine("                          AND  S1.STARTWORK_INSTRUCT_FLG = N'1' ")

                .AppendLine("        (DLR_CD, BRN_CD, RO_NUM, RO_SEQ) =    ")
                .AppendLine("                   ( ")
                .AppendLine("                        SELECT ")
                .AppendLine("                               S2.DLR_CD ")
                .AppendLine("                             , S2.BRN_CD ")
                .AppendLine("                             , S1.RO_NUM ")
                .AppendLine("                             , S1.RO_SEQ ")
                .AppendLine("                        FROM ")
                .AppendLine("                               TB_T_JOB_INSTRUCT S1 ")
                .AppendLine("                             , TB_T_JOB_DTL S2 ")
                .AppendLine("                        WHERE ")
                .AppendLine("                               S1.JOB_DTL_ID = S2.JOB_DTL_ID  ")
                .AppendLine("                          AND  S1.JOB_DTL_ID = :JOB_DTL_ID ")
                .AppendLine("                          AND  S1.JOB_INSTRUCT_ID = :JOB_INSTRUCT_ID ")
                .AppendLine("                          AND  S1.JOB_INSTRUCT_SEQ = :JOB_INSTRUCT_SEQ ")
                .AppendLine("                          AND  S1.STARTWORK_INSTRUCT_FLG = N'1' ")
                .AppendLine("                   ) ")

                '2015/06/15 TMEJ 小澤 TR-SVT-TMT-20150612-001「納車済みのチップが突然、SAメイン画面に表示された」対応 END

                .AppendLine("    AND RO_STATUS <> :RO_STATUS ")     '同じ値なら、更新しない
                .AppendLine("    AND RO_STATUS <> N'99' ")          'R/Oキャンセルのレコードは除く

            End With

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_224")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, inROStatus)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.Varchar2, inInstructId)
                query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inInstructSeq)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateFunction)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.E Update count = {1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          queryCount))
                Return queryCount

            End Using

        End Function
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        ''' <summary>
        ''' Undo操作で最初作業のROステータスを60→50に変更する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <param name="inStallUseId">ストール利用ID</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inStaffCode">更新スタッフ</param>
        ''' <param name="inUpdateFunction">更新画面ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UndoROStatus(ByVal inSvcinId As Decimal, _
                                     ByVal inJobDtlId As Decimal, _
                                     ByVal inStallUseId As Decimal, _
                                     ByVal inUpdateDateTime As Date, _
                                     ByVal inStaffCode As String, _
                                     ByVal inUpdateFunction As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inStallUseId={1}, inJobDtlId={2}, inSvcinId={3}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inStallUseId, _
                                      inJobDtlId, _
                                      inSvcinId))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_223")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_223 */ ")
                    .AppendLine("        TB_T_RO_INFO T1 ")
                    .AppendLine("    SET ")
                    .AppendLine("        T1.RO_STATUS = N'50' ")
                    .AppendLine("     ,  T1.ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("     ,  T1.ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                    .AppendLine("     ,  T1.ROW_UPDATE_FUNCTION = :UPDATE_FUNCTION ")
                    .AppendLine("     ,  T1.ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("  WHERE  ")
                    .AppendLine("        (T1.RO_NUM, T1.RO_SEQ) IN  ")      '初めの作業の場合
                    .AppendLine("                (   ")
                    .AppendLine("                   SELECT    ")
                    .AppendLine("                       S1.RO_NUM     ")
                    .AppendLine("                     , S1.RO_SEQ     ")
                    .AppendLine("                     FROM    ")
                    .AppendLine("                       TB_T_JOB_INSTRUCT S1    ")
                    .AppendLine("                   WHERE    ")
                    .AppendLine("                       S1.JOB_DTL_ID= :JOB_DTL_ID     ")
                    .AppendLine("                   AND S1.STARTWORK_INSTRUCT_FLG=N'1'    ")
                    .AppendLine("                   AND  (S1.RO_NUM, S1.RO_SEQ) NOT IN     ")
                    .AppendLine("                               (   ")
                    .AppendLine("                                   SELECT    ")            '関連チップに自分以外の実績チップ(中断、日跨ぎ終了も含める)のRO番号と枝番を取得する
                    .AppendLine("                                       C1.RO_NUM     ")
                    .AppendLine("                                     , C1.RO_SEQ     ")
                    .AppendLine("                                    FROM    ")
                    .AppendLine("                                       TB_T_STALL_USE A1    ")
                    .AppendLine("                                     , TB_T_JOB_DTL B1    ")
                    .AppendLine("                                     , TB_T_JOB_INSTRUCT C1    ")
                    .AppendLine("                                   WHERE    ")
                    .AppendLine("                                       A1.JOB_DTL_ID = B1.JOB_DTL_ID     ")
                    .AppendLine("                                   AND C1.JOB_DTL_ID = A1.JOB_DTL_ID    ")
                    .AppendLine("                                   AND C1.STARTWORK_INSTRUCT_FLG = N'1'    ")
                    .AppendLine("                                   AND B1.SVCIN_ID = :SVCIN_ID    ")
                    .AppendLine("                                   AND A1.STALL_USE_ID <> :STALL_USE_ID     ")
                    .AppendLine("                                   AND A1.RSLT_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS')    ")
                    .AppendLine("                               )   ")
                    .AppendLine("                )   ")
                    .AppendLine("        AND T1.SVCIN_ID = :SVCIN_ID    ")
                    .AppendLine("        AND T1.RO_STATUS = N'60'    ")
                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateFunction)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStallUseId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        ' ''' <summary>
        ' ''' 終了操作より、チップに紐付く作業が最終の作業番号の場合、該当作業連番を取得する(ROステータス変えるため)
        ' ''' </summary>
        ' ''' <param name="inSvcinId">サービス入庫ID</param>
        ' ''' <param name="inRONum">RO番号</param>
        ' ''' <param name="inRoJobSeqs">作業連番</param>
        ' ''' <returns>RO連番テーブル</returns>
        ' ''' <remarks>
        ' ''' 取得する作業連番は下記の条件を満足した場合、取得する
        ' ''' ①操作したチップにある作業連番が最終の場合
        ' ''' ②該当作業連番があるチップは全部検査なしまたは検査完了の場合
        ' ''' </remarks>
        'Public Function GetLastFinishJobSeq(ByVal inSvcinId As Decimal, _
        '                                    ByVal inRONum As String, _
        '                                    ByVal inROJobSeqs As String) As TabletSmbCommonClassNumberValueDataTable


        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inSvcinId={1}, inRONum={2}, inROJobSeqs={3}" _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name, inSvcinId, inRONum, inROJobSeqs))

        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_053 */ ")
        '        .AppendLine("       DISTINCT(T3.RO_SEQ) AS COL1 ")
        '        .AppendLine("   FROM  ")
        '        .AppendLine("       TB_T_SERVICEIN T1 ")
        '        .AppendLine("     , TB_T_JOB_DTL T2  ")
        '        .AppendLine("     , TB_T_JOB_INSTRUCT T3 ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID   ")
        '        .AppendLine("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '        .AppendLine("   AND T3.RO_SEQ IN (  ")
        '        .AppendLine(inROJobSeqs)
        '        .AppendLine("                    )  ")
        '        .AppendLine("   AND T1.SVCIN_ID = :SVCIN_ID  ")
        '        .AppendLine("   AND T3.STARTWORK_INSTRUCT_FLG = N'1'  ")          '着工指示した
        '        .AppendLine("   AND T3.RO_NUM = :RO_NUM  ")
        '        .AppendLine("   AND NOT EXISTS    ")
        '        .AppendLine("           (  ")
        '        .AppendLine("               SELECT  ")
        '        .AppendLine("                     S3.RO_SEQ  ")
        '        .AppendLine("                FROM  ")
        '        .AppendLine("                     TB_T_SERVICEIN S1 ")
        '        .AppendLine("                   , TB_T_JOB_DTL S2  ")
        '        .AppendLine("                   , TB_T_JOB_INSTRUCT S3 ")
        '        .AppendLine("                   , TB_T_STALL_USE S4 ")
        '        .AppendLine("                WHERE ")
        '        .AppendLine("                     S1.SVCIN_ID = S2.SVCIN_ID   ")
        '        .AppendLine("                 AND S2.JOB_DTL_ID = S3.JOB_DTL_ID ")
        '        .AppendLine("                 AND S2.JOB_DTL_ID = S4.JOB_DTL_ID ")
        '        .AppendLine("                 AND S3.RO_SEQ IN (  ")
        '        .AppendLine(inROJobSeqs)
        '        .AppendLine("                                   )  ")
        '        .AppendLine("                 AND S1.SVCIN_ID=:SVCIN_ID  ")
        '        .AppendLine("                 AND S3.STARTWORK_INSTRUCT_FLG = N'1'  ")
        '        .AppendLine("                 AND S3.RO_NUM = :RO_NUM  ")
        '        .AppendLine("                 AND (   ")
        '        .AppendLine("                         (     S2.INSPECTION_NEED_FLG = N'1'       ") '検査が終わってないチップに紐付く作業連番を除く
        '        .AppendLine("                           AND S2.INSPECTION_STATUS <> N'2'    )   ")
        '        .AppendLine("                     OR  S4.STALL_USE_STATUS <> N'03'   ")            '終了してないチップに紐付く作業連番を除く
        '        .AppendLine("                     )  ")
        '        .AppendLine("                 AND S4.STALL_USE_ID = ( ")                           'JOB_DTL_ID単位で最大のストール利用ID
        '        .AppendLine("                                 SELECT MAX(A1.STALL_USE_ID) ")
        '        .AppendLine("                                   FROM ")
        '        .AppendLine("                                        TB_T_STALL_USE A1 ")
        '        .AppendLine("                                  WHERE ")
        '        .AppendLine("                                        S2.JOB_DTL_ID = A1.JOB_DTL_ID ")
        '        .AppendLine("                                        ) ")
        '        .AppendLine("           )  ")
        '        .AppendLine("   AND NOT EXISTS    ")                                               '未紐付いた作業連番を除く
        '        .AppendLine("           (  ")
        '        .AppendLine("              SELECT  ")
        '        .AppendLine("                     S5.RO_SEQ  ")
        '        .AppendLine("                FROM  ")
        '        .AppendLine("                     TB_T_JOB_INSTRUCT S5 ")
        '        .AppendLine("               WHERE ")
        '        .AppendLine("                     S5.RO_SEQ IN (  ")
        '        .AppendLine(inROJobSeqs)
        '        .AppendLine("                                      )  ")
        '        .AppendLine("                 AND S5.STARTWORK_INSTRUCT_FLG = N'0'  ")
        '        .AppendLine("                 AND S5.RO_NUM = :RO_NUM    ")
        '        .AppendLine("           )  ")
        '    End With

        '    Dim tblResult As TabletSmbCommonClassNumberValueDataTable
        '    Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_053")
        '        query.CommandText = sql.ToString()
        '        query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)
        '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Varchar2, inRONum)
        '        tblResult = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Return count={1}", _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              tblResult.Count))

        '    Return tblResult
        'End Function

        ''' <summary>
        ''' 終了操作で指定サービス入庫IDに全部完了したJobのRO枝番を取得
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>RO連番テーブル</returns>
        ''' <remarks>
        ''' ほかの条件:
        ''' 完了したRO Seq単位に全Jobに紐づくチップが
        ''' 検査なし
        ''' または(検査あり且つ検査ステータスが2)の場合のみ
        ''' ROステータスを更新する
        ''' </remarks>
        Public Function GetROSeqForFinish(ByVal inServiceInId As Decimal) As TabletSmbCommonClassNumberValueDataTable


            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inServiceInId={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inServiceInId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_053 */ ")
                .AppendLine("       DISTINCT(T2.RO_SEQ) AS COL1 ")
                .AppendLine("   FROM  ")
                .AppendLine("       ( SELECT  ")
                .AppendLine("                T1.RO_NUM ")
                .AppendLine("        	   , T1.RO_SEQ ")
                .AppendLine("              , CASE  ")
                .AppendLine("                WHEN MIN(T1.UPDATEFLG) = 1 AND MIN(T1.JOB_STATUS) = N'1' AND MAX(T1.JOB_STATUS) = N'1' THEN 1  ")
                .AppendLine("                ELSE 0 ")
                .AppendLine("                END UPDATEFLG  ")
                .AppendLine("          FROM   ")
                .AppendLine("              ( SELECT   ")
                .AppendLine("                       C.RO_NUM  ")
                .AppendLine("                     , C.RO_SEQ  ")
                .AppendLine("                     , NVL(D.JOB_STATUS, N'-1') JOB_STATUS   ")
                .AppendLine("                     , CASE    ")
                .AppendLine("                       WHEN B.INSPECTION_NEED_FLG = N'0' THEN 1     ")
                .AppendLine("                       WHEN B.INSPECTION_NEED_FLG = N'1' AND B.INSPECTION_STATUS = N'2' THEN 1    ")
                .AppendLine("                       ELSE 0   ")
                .AppendLine("                        END UPDATEFLG  ")
                .AppendLine("                FROM  ")
                .AppendLine("                       TB_T_SERVICEIN A   ")
                .AppendLine("                     , TB_T_JOB_DTL B   ")
                .AppendLine("                     , TB_T_JOB_INSTRUCT C   ")
                .AppendLine("                     , TB_T_JOB_RESULT D   ")
                .AppendLine("                WHERE ")
                .AppendLine("                       A.SVCIN_ID = B.SVCIN_ID   ")
                .AppendLine("                   AND B.JOB_DTL_ID = C.JOB_DTL_ID   ")
                .AppendLine("                   AND C.JOB_DTL_ID = D.JOB_DTL_ID(+)   ")
                .AppendLine("                   AND C.JOB_INSTRUCT_ID = D.JOB_INSTRUCT_ID(+)   ")
                .AppendLine("                   AND C.JOB_INSTRUCT_SEQ = D.JOB_INSTRUCT_SEQ(+)   ")
                .AppendLine("                   AND ( D.JOB_RSLT_ID =   ")
                .AppendLine("                               ( SELECT    ")
                .AppendLine("                                       MAX(JOB_RSLT_ID) ")
                .AppendLine("                                   FROM ")
                .AppendLine("                                       TB_T_JOB_RESULT SUB1 ")
                .AppendLine("                                  WHERE ")
                .AppendLine("                                       D.JOB_DTL_ID = SUB1.JOB_DTL_ID  ")
                .AppendLine("                                   AND D.JOB_INSTRUCT_ID = SUB1.JOB_INSTRUCT_ID  ")
                .AppendLine("                                   AND D.JOB_INSTRUCT_SEQ = SUB1.JOB_INSTRUCT_SEQ  ")
                .AppendLine("                              GROUP BY   ")
                .AppendLine("                                       SUB1.JOB_DTL_ID    ")
                .AppendLine("                                     , SUB1.JOB_INSTRUCT_ID     ")
                .AppendLine("                                     , SUB1.JOB_INSTRUCT_SEQ )      ")
                .AppendLine("                          OR D.JOB_RSLT_ID IS NULL )      ")
                .AppendLine("                   AND A.SVCIN_ID = :SVCIN_ID ) T1      ")
                .AppendLine("       GROUP BY       ")
                .AppendLine("                T1.RO_NUM, T1.RO_SEQ ) T2       ")
                .AppendLine("                WHERE ")
                .AppendLine("       T2.UPDATEFLG = 1       ")
            End With

            Dim tblResult As TabletSmbCommonClassNumberValueDataTable
            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_053")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                tblResult = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Return count={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      tblResult.Count))

            Return tblResult
        End Function
        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START

        ' ''' <summary>
        ' ''' 指定RO番号のROステータスを更新する
        ' ''' </summary>
        ' ''' <param name="inROStatus">更新ROステータス</param>
        ' ''' <param name="inRONum">RO番号</param>
        ' ''' <param name="inUpdateDateTime">更新日時</param>
        ' ''' <param name="inStaffCode">更新スタッフ</param>
        ' ''' <param name="inUpdateFunction">更新画面ID</param>
        ' ''' <param name="inExcludeROStatus">更新除外ROステータス</param>
        ' ''' <returns></returns>
        ' ''' <remarks>
        ' ''' ROステータスが99(R/Oキャンセル)、または
        ' ''' 更新除外ROステータスで除外されたレコードは更新しない。
        ' ''' </remarks>
        'Public Function UpdateROStatusByRONum(ByVal inROStatus As String, _
        '                                       ByVal inRONum As String, _
        '                                       ByVal inUpdateDateTime As Date, _
        '                                       ByVal inStaffCode As String, _
        '                                       ByVal inUpdateFunction As String, _
        '                                       Optional ByVal inExcludeROStatus As String = "") As Long

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                              "{0}_S. inROStatus={1}, inRONum={2}, inExcludeROStatus={3}", _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              inROStatus, _
        '                              inRONum, _
        '                              inExcludeROStatus))

        ''' <summary>
        ''' 指定サービス入庫IDのROステータスを更新する
        ''' </summary>
        ''' <param name="inROStatus">更新ROステータス</param>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inStaffCode">更新スタッフ</param>
        ''' <param name="inUpdateFunction">更新画面ID</param>
        ''' <param name="inRoNum">RO番号</param>
        ''' <param name="inExcludeROStatus">更新除外ROステータス</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' ROステータスが99(R/Oキャンセル)、または
        ''' 更新除外ROステータスで除外されたレコードは更新しない。
        ''' </remarks>
        Public Function UpdateROStatusBySvcinId(ByVal inROStatus As String, _
                                                ByVal inServiceInId As Decimal, _
                                                ByVal inUpdateDateTime As Date, _
                                                ByVal inStaffCode As String, _
                                                ByVal inUpdateFunction As String, _
                                                ByVal inRONum As String, _
                                                Optional ByVal inExcludeROStatus As String = "") As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inROStatus={1}, inServiceInId={2}, inExcludeROStatus={3}, inRONum={4}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inROStatus, _
                                      inServiceInId, _
                                      inExcludeROStatus, _
                                      inRONum))

            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_226")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_226 */ ")
                    .AppendLine("        TB_T_RO_INFO ")
                    .AppendLine("    SET ")
                    .AppendLine("        RO_STATUS = :RO_STATUS ")
                    .AppendLine("     ,  ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("     ,  ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                    .AppendLine("     ,  ROW_UPDATE_FUNCTION = :UPDATE_FUNCTION ")
                    .AppendLine("     ,  ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("  WHERE  ")
                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                    '.AppendLine("        RO_NUM = :RO_NUM ")
                    .AppendLine("        SVCIN_ID = :SVCIN_ID ")
                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

                    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
                    .AppendLine("    AND RO_NUM = :RO_NUM ")  'R/O番号
                    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

                    .AppendLine("    AND RO_STATUS <> N'99' ")  'R/Oキャンセルのレコードは除く

                    If Not String.IsNullOrWhiteSpace(inExcludeROStatus) Then
                        .AppendLine("    AND RO_STATUS <> :EXCLUDE_RO_STATUS ")
                    End If

                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, inROStatus)
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                'query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, inStaffCode)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateFunction)

                '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
                '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

                If Not String.IsNullOrWhiteSpace(inExcludeROStatus) Then
                    query.AddParameterWithTypeValue("EXCLUDE_RO_STATUS", OracleDbType.NVarchar2, inExcludeROStatus)
                End If

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#Region "通知用の情報取得"
        ''' <summary>
        ''' 通知用の情報取得
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inDlrCode">販売店コード</param>
        ''' <param name="inBrnCode">店舗コード</param>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>通知用の情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/06/15 TMEJ 小澤 TR-SVT-TMT-20150612-001「納車済みのチップが突然、SAメイン画面に表示された」対応
        ''' </history>
        Public Function GetNoticeInfo(ByVal inServiceInId As Decimal, _
                                      ByVal inDlrCode As String, _
                                      ByVal inBrnCode As String, _
                                      Optional ByVal inJobDtlId As Decimal = 0) As TabletSmbCommonClassNoticeInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START inServiceInId={2} inJobDtlId={3} inDlrCode={4} inBrnCode={5}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inServiceInId _
                       , inJobDtlId _
                       , inDlrCode _
                       , inBrnCode))


            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_055 */ ")
                .AppendLine("        T1.RO_NUM AS R_O ")
                .AppendLine("      , T1.PIC_SA_STF_CD ")
                .AppendLine("      , T2.VISIT_ID AS SAChipID ")
                .AppendLine("      , T3.VCL_VIN AS VIN ")
                .AppendLine("      , T4.REG_NUM AS VCLREGNO ")
                '.AppendLine("      , T5.CST_TYPE AS CUSTSEGMENT ")
                .AppendLine("      , T6.DMS_CST_CD AS DMS_CST_ID ")
                .AppendLine("      , T6.CST_NAME AS CST_NAME ")
                .AppendLine("      , T7.NAMETITLE_NAME ")
                .AppendLine("      , T7.POSITION_TYPE ")
                .AppendLine("      , (CASE WHEN (TRIM(T6.DMS_CST_CD) IS NOT NULL or TRIM(T11.DMSID) IS NOT NULL) THEN '1' ELSE '2' END) AS CUSTSEGMENT ")
                If Not inJobDtlId = 0 Then
                    .AppendLine("      , T8.DMS_JOB_DTL_ID AS BASREZID ")
                    .AppendLine("      , (CASE WHEN T10.UPPER_DISP IS NULL or T10.LOWER_DISP IS NULL THEN NVL(T9.SVC_CLASS_NAME,T9.SVC_CLASS_NAME_ENG) ELSE T10.UPPER_DISP||T10.LOWER_DISP END) AS MAINTE_NAME ")
                End If
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN T1 ")
                .AppendLine("      , TB_T_RO_INFO T2 ")
                .AppendLine("      , TB_M_VEHICLE T3 ")
                .AppendLine("      , TB_M_VEHICLE_DLR T4 ")
                '.AppendLine("      , TB_M_CUSTOMER_DLR T5 ")
                .AppendLine("      , TB_M_CUSTOMER T6 ")
                .AppendLine("      , TB_M_NAMETITLE T7 ")
                .AppendLine("      , TBL_SERVICE_VISIT_MANAGEMENT T11 ")
                If Not inJobDtlId = 0 Then
                    .AppendLine("      , TB_T_JOB_DTL T8 ")
                    .AppendLine("      , TB_M_SERVICE_CLASS T9 ")
                    .AppendLine("      , TB_M_MERCHANDISE T10 ")
                End If
                .AppendLine("  WHERE  ")

                '2015/06/15 TMEJ 小澤 TR-SVT-TMT-20150612-001「納車済みのチップが突然、SAメイン画面に表示された」対応 START

                '.AppendLine("        T1.RO_NUM=T2.RO_NUM ")

                .AppendLine("        T1.SVCIN_ID = T2.SVCIN_ID ")

                '2015/06/15 TMEJ 小澤 TR-SVT-TMT-20150612-001「納車済みのチップが突然、SAメイン画面に表示された」対応 END

                .AppendLine("   AND  T1.VCL_ID=T3.VCL_ID ")
                .AppendLine("   AND  T1.VCL_ID=T4.VCL_ID ")
                'AppendLine("   AND  T1.CST_ID=T5.CST_ID ")
                .AppendLine("   AND  T1.CST_ID=T6.CST_ID ")
                .AppendLine("   AND  T6.NAMETITLE_CD=T7.NAMETITLE_CD(+) ")
                .AppendLine("   AND  T7.INUSE_FLG(+) = N'1' ")
                .AppendLine("   AND  T1.SVCIN_ID=T11.FREZID(+) ")
                If Not inJobDtlId = 0 Then
                    .AppendLine("   AND  T1.SVCIN_ID=T8.SVCIN_ID ")
                    .AppendLine("   AND  T8.SVC_CLASS_ID=T9.SVC_CLASS_ID(+) ")
                    .AppendLine("   AND  T8.MERC_ID=T10.MERC_ID(+) ")
                    .AppendLine("   AND  T8.JOB_DTL_ID =:JOB_DTL_ID ")
                End If
                .AppendLine("   AND  T1.DLR_CD=:DLR_CD ")
                .AppendLine("   AND  T1.BRN_CD=:BRN_CD ")
                .AppendLine("   AND  T1.SVCIN_ID=:SVCIN_ID ")
                .AppendLine("   AND  T2.RO_SEQ= N'0' ")
                .AppendLine("   AND  T2.RO_STATUS<>N'99' ")
                '.AppendLine("   AND  T5.DLR_CD=:DLR_CD ")

            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNoticeInfoDataTable)("TABLETSMBCOMMONCLASS_055")
                query.CommandText = Sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBrnCode)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                If Not inJobDtlId = 0 Then
                    query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                End If

                Dim dt As TabletSmbCommonClassNoticeInfoDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E OUT:Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))

                Return dt
            End Using

        End Function

        ''' <summary>
        ''' 指定SAアカウントの取得
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inDlrCode">販売店コード</param>
        ''' <param name="inBrnCode">店舗コード</param>
        ''' <returns>指定SAアカウント</returns>
        ''' <remarks></remarks>
        Public Function GetSAAcountBySvcinId(ByVal inServiceInId As Decimal, _
                                             ByVal inDlrCode As String, _
                                             ByVal inBrnCode As String) As TabletSmbCommonClassStringValueDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START inServiceInId={2} inDlrCode={3} inBrnCode={4}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inServiceInId _
                       , inDlrCode _
                       , inBrnCode))


            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_054 */ ")
                .AppendLine("        T1.PIC_SA_STF_CD AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN T1 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.DLR_CD=:DLR_CD ")
                .AppendLine("   AND  T1.BRN_CD=:BRN_CD ")
                .AppendLine("   AND  T1.SVCIN_ID=:SVCIN_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassStringValueDataTable)("TABLETSMBCOMMONCLASS_054")
                query.CommandText = Sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDlrCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBrnCode)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                Dim dt As TabletSmbCommonClassStringValueDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E OUT:Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))

                Return dt
            End Using

        End Function
#End Region

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発　END

        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START

        ''' <summary>
        ''' ストール利用更新(検査不合格時)
        ''' </summary>
        ''' <param name="inStallUseId">ストール利用ID</param>
        ''' <param name="inStallUseStatus">ストール利用ステータス</param>
        ''' <param name="inStopReasonType">中断理由区分</param>
        ''' <param name="inUpdateAccount">更新アカウント</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inUpdateProgramId">更新機能ID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateStallUseByFailedInspection(ByVal inStallUseId As Decimal, _
                                                         ByVal inStallUseStatus As String, _
                                                         ByVal inStopReasonType As String, _
                                                         ByVal inUpdateAccount As String, _
                                                         ByVal inUpdateDateTime As Date, _
                                                         ByVal inUpdateProgramId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inStallUseId={1}, inStallUseStatus={2}, inStopReasonType={3}, inUpdateAccount={4}, inUpdateDateTime={5}, inUpdateProgramId={6}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inStallUseId, _
                                      inStallUseStatus, _
                                      inStopReasonType, _
                                      inUpdateAccount, _
                                      inUpdateDateTime, _
                                      inUpdateProgramId))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* TABLETSMBCOMMONCLASS_227 */ ")
                .Append("       TB_T_STALL_USE ")
                .Append("    SET ")
                .Append("       STALL_USE_STATUS = :STALL_USE_STATUS ")         'ストール利用ステータス
                .Append("     , STOP_REASON_TYPE = :STOP_REASON_TYPE ")         '中断理由区分
                .Append("     , UPDATE_DATETIME = :UPDATE_DATETIME ")           '更新日時
                .Append("     , UPDATE_STF_CD = :ACCOUNT ")                     '更新スタッフコード
                .Append("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")       '行更新日時
                .Append("     , ROW_UPDATE_ACCOUNT = :ACCOUNT ")                '行更新アカウント
                .Append("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")   '行更新機能
                .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
                .Append("  WHERE ")
                .Append("       STALL_USE_ID = :STALL_USE_ID ")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_227")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, inStallUseStatus)
                query.AddParameterWithTypeValue("STOP_REASON_TYPE", OracleDbType.NVarchar2, inStopReasonType)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inUpdateAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateProgramId)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStallUseId)

                'SQL実行
                Dim result As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. result={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result

            End Using

        End Function

        ''' <summary>
        ''' 作業実績更新(検査不合格時)
        ''' </summary>
        ''' <param name="inJobDetailId">作業内容ID</param>
        ''' <param name="inJobStatus">作業ステータス</param>
        ''' <param name="inStopReasonType">中断理由区分</param>
        ''' <param name="inUpdateAccount">更新アカウント</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inUpdateProgramId">更新機能ID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateJobResultByFailedInspection(ByVal inJobDetailId As Decimal, _
                                                          ByVal inJobStatus As String, _
                                                          ByVal inStopReasonType As String, _
                                                          ByVal inUpdateAccount As String, _
                                                          ByVal inUpdateDateTime As Date, _
                                                          ByVal inUpdateProgramId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inJobDetailId={1}, inJobStatus={2}, inStopReasonType={3}, inUpdateAccount={4}, inUpdateDateTime={5}, inUpdateProgramId={6}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inJobDetailId, _
                                      inJobStatus, _
                                      inStopReasonType, _
                                      inUpdateAccount, _
                                      inUpdateDateTime, _
                                      inUpdateProgramId))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* TABLETSMBCOMMONCLASS_229 */ ")
                .Append("       TB_T_JOB_RESULT ")
                .Append("    SET ")
                .Append("       JOB_STATUS = :JOB_STATUS ")                     '作業ステータス
                .Append("     , STOP_REASON_TYPE = :STOP_REASON_TYPE ")         '中断理由区分
                .Append("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")       '行更新日時
                .Append("     , ROW_UPDATE_ACCOUNT = :ACCOUNT ")                '行更新アカウント
                .Append("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")   '行更新機能
                .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
                .Append("  WHERE ")
                .Append("        (JOB_INSTRUCT_ID, JOB_DTL_ID, JOB_INSTRUCT_SEQ) IN ")
                .Append("            ( ")
                .Append("                SELECT ")
                .Append("                       JOB_INSTRUCT_ID ")
                .Append("                     , JOB_DTL_ID ")
                .Append("                     , JOB_INSTRUCT_SEQ ")
                .Append("                  FROM ")
                .Append("                       TB_T_JOB_INSTRUCT ")
                .Append("                 WHERE ")
                .Append("                       JOB_DTL_ID = :JOB_DTL_ID ")
                .Append("             ) ")
                .Append("    AND JOB_STATUS = N'1' ")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_229")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("JOB_STATUS", OracleDbType.NVarchar2, inJobStatus)
                query.AddParameterWithTypeValue("STOP_REASON_TYPE", OracleDbType.NVarchar2, inStopReasonType)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inUpdateAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateProgramId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDetailId)

                'SQL実行
                Dim result As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. UpdateRecordCount={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result

            End Using

        End Function

        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
        ''' <summary>
        ''' 作業実績更新(中断→次工程へ移動時)
        ''' </summary>
        ''' <param name="inJobDetailId">作業内容ID</param>
        ''' <param name="inUpdateAccount">更新アカウント</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inUpdateProgramId">更新機能ID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks>
        ''' 中断サブチップを「次工程へ移動」実施時に作業実績テーブルを更新するSQL実行メソッド
        ''' 更新対象は、下記の条件を全て満たすレコード
        ''' 　・作業内容IDに紐づくJobの最新作業実績
        ''' 　・作業ステータスが2(中断)
        ''' </remarks>
        Public Function UpdateJobResultByMoveToNextProcess(ByVal inJobDetailId As Decimal, _
                                                           ByVal inUpdateAccount As String, _
                                                           ByVal inUpdateDateTime As Date, _
                                                           ByVal inUpdateProgramId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.{1} Start inJobDetailId={2}, inUpdateAccount={3}, inUpdateDateTime={4}, inUpdateProgramId={5}", _
                                      Me.GetType().ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inJobDetailId, _
                                      inUpdateAccount, _
                                      inUpdateDateTime, _
                                      inUpdateProgramId))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" UPDATE /* TABLETSMBCOMMONCLASS_232 */ ")
                .AppendLine("        TB_T_JOB_RESULT A ")
                .AppendLine("    SET ")
                .AppendLine("        A.JOB_STATUS = N'1' ")                             '作業ステータスを1(完了)にする
                .AppendLine("      , A.ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")    '行更新日時
                .AppendLine(" 	   , A.ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")      '行更新アカウント
                .AppendLine(" 	   , A.ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")    '行更新機能
                .AppendLine(" 	   , A.ROW_LOCK_VERSION = A.ROW_LOCK_VERSION + 1 ")     '行ロックバージョン
                .AppendLine("  WHERE ")
                .AppendLine(" 	     A.JOB_RSLT_ID IN ( ")
                .AppendLine("        SELECT ")
                .AppendLine(" 			    MAX(JOB_RSLT_ID) ")
                .AppendLine(" 		   FROM ")
                .AppendLine(" 			    TB_T_JOB_RESULT B ")
                .AppendLine(" 		  WHERE ")
                .AppendLine(" 		        B.JOB_INSTRUCT_ID = A.JOB_INSTRUCT_ID ")
                .AppendLine(" 		    AND B.JOB_DTL_ID = A.JOB_DTL_ID ")
                .AppendLine(" 		    AND B.JOB_INSTRUCT_SEQ = A.JOB_INSTRUCT_SEQ ")
                .AppendLine(" 		    AND B.JOB_DTL_ID = :JOB_DTL_ID ")
                .AppendLine(" 	   GROUP BY ")
                .AppendLine(" 			    B.JOB_INSTRUCT_ID ")
                .AppendLine(" 			  , B.JOB_DTL_ID ")
                .AppendLine(" 			  , B.JOB_INSTRUCT_SEQ ) ")
                .AppendLine("    AND A.JOB_STATUS = N'2' ")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_232")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inUpdateAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateProgramId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDetailId)

                'SQL実行
                Dim result As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.{1} End UpdateRecordCount={2}", _
                                          Me.GetType().ToString, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result

            End Using

        End Function
        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        ' ''' <summary>
        ' ''' RO情報更新(検査合格時)
        ' ''' </summary>
        ' ''' <param name="inRONum">RO番号</param>
        ' ''' <param name="inROStatus">更新用ROステータス</param>
        ' ''' <param name="inUpdateAccount">更新アカウント</param>
        ' ''' <param name="inUpdateDateTime">更新日時</param>
        ' ''' <param name="inUpdateProgramId">更新機能ID</param>
        ' ''' <returns>更新件数</returns>
        ' ''' <remarks>該当するRO番号、かつROステータスが60(作業中)のレコードのみを更新対象とする</remarks>
        'Public Function UpdateROInfoByPassedInspection(ByVal inRONum As String, _
        '                                               ByVal inROStatus As String, _
        '                                               ByVal inUpdateAccount As String, _
        '                                               ByVal inUpdateDateTime As Date, _
        '                                               ByVal inUpdateProgramId As String) As Integer

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                              "{0}_S. inRONum={1}, inROStatus={2}, inUpdateAccount={3}, inUpdateDateTime={4}, inUpdateProgramId={5}", _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              inRONum, _
        '                              inROStatus, _
        '                              inUpdateAccount, _
        '                              inUpdateDateTime, _
        '                              inUpdateProgramId))

        ''' <summary>
        ''' RO情報更新(検査合格時)
        ''' </summary>
        ''' <param name="inRONum">RO番号</param>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inROStatus">更新用ROステータス</param>
        ''' <param name="inUpdateAccount">更新アカウント</param>
        ''' <param name="inUpdateDateTime">更新日時</param>
        ''' <param name="inUpdateProgramId">更新機能ID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks>該当するRO番号、かつROステータスが60(作業中)のレコードのみを更新対象とする</remarks>
        Public Function UpdateROInfoByPassedInspection(ByVal inRONum As String, _
                                                       ByVal inServiceInId As Decimal, _
                                                       ByVal inROStatus As String, _
                                                       ByVal inUpdateAccount As String, _
                                                       ByVal inUpdateDateTime As Date, _
                                                       ByVal inUpdateProgramId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. inRONum={1}, inServiceInId={2}, inROStatus={3}, inUpdateAccount={4}, inUpdateDateTime={5}, inUpdateProgramId={6}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inRONum, _
                                      inServiceInId, _
                                      inROStatus, _
                                      inUpdateAccount, _
                                      inUpdateDateTime, _
                                      inUpdateProgramId))
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END


            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* TABLETSMBCOMMONCLASS_230 */ ")
                .Append("        TB_T_RO_INFO ")
                .Append("    SET ")
                .Append("        RO_STATUS = :RO_STATUS ")                       'ROステータス
                .Append("      , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")       '行更新日時
                .Append("      , ROW_UPDATE_ACCOUNT = :UPDATE_ACCOUNT ")         '行更新アカウント
                .Append("      , ROW_UPDATE_FUNCTION = :UPDATE_FUNCTION ")       '行更新機能
                .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
                .Append("  WHERE ")
                .Append("        RO_NUM = :RO_NUM ")
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                .Append("    AND SVCIN_ID = :SVCIN_ID  ")
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END
                .Append("    AND RO_STATUS = N'60' ")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_230")

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, inROStatus)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONum)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, inUpdateDateTime)
                query.AddParameterWithTypeValue("UPDATE_ACCOUNT", OracleDbType.NVarchar2, inUpdateAccount)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, inUpdateProgramId)
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

                'SQL実行
                Dim result As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. UpdateRecordCount={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          result))

                Return result

            End Using

        End Function

        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
#Region "中断Job件数の取得"
        ''' <summary>
        ''' 中断Job件数の取得
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>中断JOBの件数</returns>
        ''' <remarks></remarks>
        Public Function GetStopJobCount(ByVal inJobDtlId As Decimal) As TabletSmbCommonClassNumberValueDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} START inJobDtlId={2} " _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                   , inJobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_065 */ ")
                .AppendLine("        COUNT(1) AS COL1 ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_RESULT T1 ")
                .AppendLine("     ,  (SELECT ")
                .AppendLine("                MAX(JOB_RSLT_ID) AS MAX_JOB_RSLT_ID ")
                .AppendLine("           FROM ")
                .AppendLine("                TB_T_JOB_INSTRUCT T2 ")
                .AppendLine("             ,  TB_T_JOB_RESULT T3 ")
                .AppendLine("          WHERE ")
                .AppendLine("                T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("            AND T2.JOB_INSTRUCT_ID = T3.JOB_INSTRUCT_ID ")
                .AppendLine("            AND T2.JOB_INSTRUCT_SEQ = T3.JOB_INSTRUCT_SEQ ")
                .AppendLine("            AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
                .AppendLine("            AND T2.STARTWORK_INSTRUCT_FLG = N'1' ")
                .AppendLine("       GROUP BY ")
                .AppendLine("                T3.JOB_DTL_ID ")
                .AppendLine("             ,  T3.JOB_INSTRUCT_ID ")
                .AppendLine("             ,  T3.JOB_INSTRUCT_SEQ) T4 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.JOB_RSLT_ID = T4.MAX_JOB_RSLT_ID ")
                .AppendLine("    AND T1.JOB_STATUS = N'2' ")

            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassNumberValueDataTable)("TABLETSMBCOMMONCLASS_065")
                query.CommandText = sql.ToString()
                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)

                Dim dt As TabletSmbCommonClassNumberValueDataTable = query.GetData()
                Dim count As Long = CType(dt(0).COL1, Long)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.E OUT:Count={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          count))

                Return dt
            End Using

        End Function
#End Region
        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

        '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）START
        ''' <summary>
        ''' サービス入庫IDより実績入庫日時を取得
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>実績入庫日時テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetRsltDeliDateTime(ByVal inServiceInId As Decimal) As TabletSmbCommonClassRsltDeliDateTimeDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} START inServiceInId={2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , inServiceInId))


            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_070 */ ")
                .AppendLine("        T1.RSLT_DELI_DATETIME ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN T1 ")
                .AppendLine("  WHERE  ")
                .AppendLine("        T1.SVCIN_ID=:SVCIN_ID ")
            End With

            Using query As New DBSelectQuery(Of TabletSmbCommonClassRsltDeliDateTimeDataTable)("TABLETSMBCOMMONCLASS_070")

                query.CommandText = Sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                Dim dt As TabletSmbCommonClassRsltDeliDateTimeDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                        , "{0}.End Query Count={1}" _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , dt.Count))

                Return dt

            End Using

        End Function

        '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

#Region "強制納車処理"

        ''' <summary>
        ''' サービス入庫テーブル強制納車更新処理
        ''' </summary>
        ''' <param name="inServiceinId">サービス入庫ID</param>
        ''' <param name="inResultDeliveryDate">実績納車日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history></history>
        Public Function UpdateServiceinForceDeliverd(ByVal inServiceinId As Decimal, _
                                                     ByVal inResultDeliveryDate As Date) As Integer
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START inServiceinId:{2} inResultDeliveryDate:{3} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inServiceinId.ToString(CultureInfo.CurrentCulture) _
                , inResultDeliveryDate.ToString(CultureInfo.CurrentCulture)))

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_233")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.Append("UPDATE /* TABLETSMBCOMMONCLASS_233 */ ")
                sql.Append("       TB_T_SERVICEIN ")
                sql.Append("   SET SVC_STATUS = N'13'")
                sql.Append("      ,RSLT_DELI_DATETIME = :RSLT_DELI_DATETIME ")
                sql.Append(" WHERE ")
                sql.Append("       SVCIN_ID = :SVCIN_ID ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("RSLT_DELI_DATETIME", OracleDbType.Date, inResultDeliveryDate)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceinId)

                'SQL実行
                Dim count As Integer = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , count))

                ' 検索結果の返却
                Return count

            End Using

        End Function

        ''' <summary>
        ''' 作業内容テーブル強制納車更新処理
        ''' </summary>
        ''' <param name="inServiceinId">サービス入庫ID</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inSystemId">システムID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history></history>
        Public Function UpdateJobDetailForceDeliverd(ByVal inServiceinId As Decimal, _
                                                     ByVal inAccount As String, _
                                                     ByVal inNowDate As Date, _
                                                     ByVal inSystemId As String) As Integer
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START inServiceinId:{2} inAccount:{3} inNowDate:{4} inSystemId:{5} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inServiceinId.ToString(CultureInfo.CurrentCulture) _
                , inAccount _
                , inNowDate.ToString(CultureInfo.CurrentCulture) _
                , inSystemId))

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_234")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.Append("UPDATE /* TABLETSMBCOMMONCLASS_234 */ ")
                sql.Append("       TB_T_JOB_DTL ")
                sql.Append("   SET INSPECTION_STATUS = N'2' ")
                sql.Append("      ,UPDATE_DATETIME = :NOWDATE ")
                sql.Append("      ,UPDATE_STF_CD = :ACCOUNT ")
                sql.Append("      ,ROW_UPDATE_DATETIME = :NOWDATE ")
                sql.Append("      ,ROW_UPDATE_ACCOUNT = :ACCOUNT ")
                sql.Append("      ,ROW_UPDATE_FUNCTION = :SYSTEM ")
                sql.Append("      ,ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                sql.Append(" WHERE ")
                sql.Append("       SVCIN_ID = :SVCIN_ID ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceinId)

                'SQL実行
                Dim count As Integer = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , count))

                ' 検索結果の返却
                Return count

            End Using

        End Function

        ''' <summary>
        ''' RO情報強制納車更新処理
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inRepairOrderNum">RO番号</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inSystemId">システムID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history></history>
        Public Function UpdateROInfoForceDeliverd(ByVal inDealerCode As String, _
                                                  ByVal inBranchCode As String, _
                                                  ByVal inRepairOrderNum As String, _
                                                  ByVal inAccount As String, _
                                                  ByVal inNowDate As Date, _
                                                  ByVal inSystemId As String) As Integer
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START inDealerCode:{2} inBranchCode:{3} inRepairOrderNum:{4} inAccount:{5} inNowDate:{6} inSystemId:{7} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inBranchCode _
                , inRepairOrderNum _
                , inAccount _
                , inNowDate.ToString(CultureInfo.CurrentCulture) _
                , inSystemId))

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("TABLETSMBCOMMONCLASS_235")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.Append("UPDATE /* TABLETSMBCOMMONCLASS_235 */ ")
                sql.Append("       TB_T_RO_INFO ")
                sql.Append("   SET RO_STATUS = N'90' ")
                sql.Append("      ,ROW_UPDATE_DATETIME = :NOWDATE ")
                sql.Append("      ,ROW_UPDATE_ACCOUNT = :ACCOUNT ")
                sql.Append("      ,ROW_UPDATE_FUNCTION = :SYSTEM ")
                sql.Append("      ,ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                sql.Append(" WHERE ")
                sql.Append("       DLR_CD = :DLR_CD ")
                sql.Append("   AND BRN_CD = :BRN_CD ")
                sql.Append("   AND RO_NUM = :RO_NUM ")
                sql.Append("   AND RO_STATUS <> N'90' ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, inSystemId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRepairOrderNum)

                'SQL実行
                Dim count As Integer = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , count))

                ' 検索結果の返却
                Return count

            End Using

        End Function

#End Region

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
#Region "サービス分類情報取得"

        ''' <summary>
        ''' ストールに紐付くサービス分類情報を取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <returns>サービス分類情報</returns>
        ''' <remarks></remarks>
        ''' <history></history>
        Public Function GetSvcClassInfo(ByVal stallId As Decimal) As TabletSmbCommonClassServiceClassDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START StallId:{2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , stallId.ToString(CultureInfo.CurrentCulture)))

            Dim Sql As New StringBuilder
            With Sql
                .AppendLine(" SELECT /* TABLETSMBCOMMONCLASS_074 */ ")
                .AppendLine("        STALL.DLR_CD ")
                .AppendLine("      , STALL.BRN_CD ")
                .AppendLine("      , SRVCLS.SVC_CLASS_ID ")
                .AppendLine("      , SRVCLS.SVC_CLASS_CD ")
                .AppendLine("      , SRVCLS.SVC_CLASS_TYPE ")
                .AppendLine("      , BRN_SRVCLS.CARWASH_NEED_FLG ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STALL STALL ")
                .AppendLine("      , TB_M_BRANCH_SERVICE_CLASS BRN_SRVCLS ")
                .AppendLine("      , TB_M_SERVICE_CLASS SRVCLS ")
                .AppendLine("  WHERE  ")
                .AppendLine("        STALL.SVC_CLASS_ID = BRN_SRVCLS.SVC_CLASS_ID  ")
                .AppendLine("    AND STALL.DLR_CD = BRN_SRVCLS.DLR_CD ")
                .AppendLine("    AND STALL.BRN_CD = BRN_SRVCLS.BRN_CD ")
                .AppendLine("    AND BRN_SRVCLS.SVC_CLASS_ID = SRVCLS.SVC_CLASS_ID ")
                .AppendLine("    AND STALL.STALL_ID = :STALL_ID ")
            End With

            Dim dtServiceClass As TabletSmbCommonClassServiceClassDataTable = Nothing
            Using query As New DBSelectQuery(Of TabletSmbCommonClassServiceClassDataTable)("TABLETSMBCOMMONCLASS_074")
                query.CommandText = Sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                dtServiceClass = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_End. RowCount={1}", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                          dtServiceClass.Rows.Count))

            Return dtServiceClass

        End Function

#End Region
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

    End Class

End Namespace


Partial Class TabletSMBCommonClassDataSet

End Class
