'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3150101DataSet.vb
'─────────────────────────────────────
'機能： TCメインメニューデータセット
'補足： 
'作成： 2012/01/26 KN 鶴田
'更新： 2012/02/27 KN 佐藤 DevPartner 1回目の指摘事項を修正
'更新： 2012/02/27 KN 佐藤 スタッフストール割当の抽出条件を追加
'更新： 2012/02/28 KN 渡辺 関連チップの順不同開始を抑制するように修正
'更新： 2012/02/28 KN 上田 SQLインスペクション対応
'更新： 2012/03/09 KN 日比野 SQLインスペクション対応
'更新： 2012/03/19 KN 西田 プレユーザーテスト課題・不具合対応 No.22 開始処理は15分前以前は開始不可とする
'更新： 2012/05/24 KN 西田 TCメイン 号口不具合対応 作業が開始出来なかった
'更新： 2012/06/01 KN 西田 STEP1 重要課題対応
'更新： 2012/06/05 KN 彭健 コード分析対応
'更新： 2012/06/14 KN 西田 STEP1 重要課題対応 DevPartner指摘対応
'更新： 2012/07/26 KN 彭健 STEP1 仕分け課題対応
'更新： 2012/11/05 TMEJ彭健  問連修正(GTMC121025029、GTMC121029047)
'更新： 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応
'更新： 2013/02/26 TMEJ 成澤 【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成(TCステータスモニター起動待機時間の取得)
'更新： 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計
'更新： 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新： 2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応
'更新： 2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発
'更新： 2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
'更新： 2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発
'更新： 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新： 2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新： 2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization

Namespace SC3150101DataSetTableAdapters
    Public Class SC3150101StallInfoDataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        ''' <summary>
        ''' 画面ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const APPLICATION_ID As String = "SC3150101"
        ''' <summary>
        ''' 最小日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MINDATE As String = "1900/01/01 00:00:00"
        ''' <summary>
        ''' 入庫日時最小日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const strMinDate As String = "0001/01/01 0:00:00"
        ''' <summary>
        ''' 行ロックバージョン初期値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DEFAULT_ROW_LOCK_VERSION As Long = 0
        ''' <summary>
        ''' サービスステータス"00":未入庫
        ''' </summary>
        ''' <remarks></remarks>
        Private Const sarviceStatus00 As String = "00"
        ''' <summary>
        ''' サービスステータス"01":未来店客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const sarviceStatus01 As String = "01"
        ''' <summary>
        ''' サービスステータス"03":着工指示待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const sarviceStatus03 As String = "03"
        ''' <summary>
        ''' サービスステータス"04":作業開始待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const sarviceStatus04 As String = "04"
        ''' <summary>
        ''' サービスステータス"13":納車済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const sarviceStatus13 As String = "13"

        ''' <summary>
        ''' ストール利用ステータス"00":着工指示待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus00 As String = "00"
        ''' <summary>
        ''' ストール利用ステータス"01":作業開始待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus01 As String = "01"
        ''' <summary>
        ''' ストール利用ステータス"02":作業中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus02 As String = "02"
        ''' <summary>
        ''' ストール利用ステータス"03":完了
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus03 As String = "03"
        ''' <summary>
        ''' ストール利用ステータス"04":作業指示の一部の作業が中断
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus04 As String = "04"
        ''' <summary>
        ''' ストール利用ステータス"05":中断
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus05 As String = "05"
        ''' <summary>
        ''' ストール利用ステータス"07":未来店客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus07 As String = "07"

        ''' <summary>
        ''' 着工指示区分 "0":未着工 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CN_INSTRUCT_0 As String = "0"

        ''' <summary>
        ''' 着工指示区分 "1":着工指示 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CN_INSTRUCT_1 As String = "1"

        ''' <summary>
        ''' 着工指示区分 "2":着工準備 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CN_INSTRUCT_2 As String = "2"

        ''' <summary>
        ''' サービス分類区分"1":EM
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_CLASS_TYPE_1 As String = "1"

        ''' <summary>
        ''' サービス分類区分"2":PM
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_CLASS_TYPE_2 As String = "2"

        ''' <summary>
        ''' サービス分類区分"3":GR
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_CLASS_TYPE_3 As String = "3"
        ''' <summary>
        ''' サービス分類区分"4":PDS
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_CLASS_TYPE_4 As String = "4"

        ''' <summary>
        ''' サービスコード"1":車検
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SERVICECODE_INSPECTION As String = "10"

        ''' <summary>
        ''' サービスコード"2":定期点検
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SERVICECODE_PERIODIC As String = "20"

        ''' <summary>
        ''' サービスコード"3":一般点検
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SERVICECODE_GENERAL As String = "30"
        ''' <summary>
        ''' サービスコード"4":新規点検
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SERVICECODE_NEWCAR As String = "40"


        ''' <summary>
        ''' 作業区分0RSLT_WORKTIME
        ''' </summary>
        ''' <remarks></remarks>
        Private Const JOB_TYPE_0 As String = "0"

        ''' <summary>
        ''' キャンセルフラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CANCEL_FLG_0 As String = "0"
        ''' <summary>
        ''' キャンセルフラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CANCEL_FLG_1 As String = "1"
        ''' <summary>
        '''  仮置フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TEMP_FLG As String = "1"
        ''' <summary>
        '''  非稼動区分"1":休憩時間 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IDLE_TYPE_1 As String = "1"
        ''' <summary>
        '''  非稼動区分"2":使用不可
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IDLE_TYPE_2 As String = "2"

        ''' <summary>
        '''  ストップフラグ"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const STOPFLG_0 As String = "0"
        ''' <summary>
        '''  ストップフラグ"1"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const STOPFLG_1 As String = "1"
        ''' <summary>
        ''' 実績作業時間省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RSLT_WORKTIME_0 As String = "0"
        ''' <summary>
        '''  休憩区分"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BREAKKBN_0 As String = "0"
        ''' <summary>
        '''  休憩区分"1"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BREAKKBN_1 As String = "1"
        ''' <summary>
        '''  RO作業連番"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_SEQ_0 As String = "0"
        ''' <summary>
        '''  洗車必要フラグ"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CARWASH_NEED_FLG_0 As String = "0"
        ''' <summary>
        '''  検査必要フラグ"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INSPECTION_NEED_FLG_0 As String = "0"
        ''' <summary>
        '''  納車済フラグ"0":未納車
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DELIVERY_FLG_0 As String = "0"
        ''' <summary>
        '''  納車済フラグ"1":納車済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DELIVERY_FLG_1 As String = "1"
        ''' <summary>
        ''' 省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DEFAULT_VALUE As String = " "
        ''' <summary>
        ''' ロウナンバー:1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ROW_NUM As Integer = 1
        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
        ''' <summary>
        ''' オペレーションコード：チーフテクニシャン
        ''' </summary>
        ''' <remarks></remarks>
        Private OPERATIONCODE_CHIEF_TECHNICIAN As Integer = 62
        ''' <summary>
        ''' 着工指示フラグ:着工済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const STARTWORK_INSTRUCT_FLG As String = "1"
        ''' <summary>
        ''' 業務権限フラグ（サービス）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ORGNZ_SA_FLG As String = "1"
        ''' <summary>
        ''' 使用中フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INUSE_FLG As String = "1"
        ''' <summary>
        ''' RO連番：省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_SEQ_DEFAULT As Integer = -1
        ''' <summary>
        ''' ROステータス：キャンセル
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_STATUS_CANCEL As String = "99"
        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

        '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        ''' <summary>
        ''' P/Lアイコンフラグ（0：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ICON_OFF_FLAG As String = "0"
        '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

#End Region


        ''' <summary>
        ''' ストール予約情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        ''' <history>2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計</history>
        Public Function GetStallReserveInfo(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal reserveId As Decimal) As SC3150101DataSet.SC3150101StallReserveInfoDataTable

            Logger.Info("[S]GetStallReserveInfo()")


            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                '.Append(" SELECT /* SC3150101_001 */ ")
                '.Append("        DLRCD, ")                ' 01 販売店コード
                '.Append("        STRCD, ")                ' 02 店舗コード
                '.Append("        REZID, ")                ' 03 予約ID
                '.Append("        STALLID, ")              ' 05 ストールID
                '.Append("        STARTTIME, ")            ' 06 使用開始日時
                '.Append("        ENDTIME, ")              ' 07 使用終了日時
                '.Append("        STATUS, ")               ' 19 ステータス
                '.Append("        WASHFLG, ")              ' 30 洗車フラグ
                '.Append("        REZ_RECEPTION, ")        ' 33 予約_受付納車区分
                '.Append("        REZ_WORK_TIME, ")        ' 34 予定_作業時間
                '.Append("        REZ_PICK_DATE, ")        ' 35 予約_引取_希望日時時刻
                '.Append("        REZ_PICK_LOC, ")         ' 36 予約_引取_場所
                '.Append("        REZ_PICK_TIME, ")        ' 37 予約_引取_所要時間
                '.Append("        REZ_DELI_DATE, ")        ' 39 予約_納車_希望日時時刻
                '.Append("        REZ_DELI_LOC, ")         ' 40 予約_納車_場所
                '.Append("        REZ_DELI_TIME, ")        ' 41 予約_納車_所要時間
                '.Append("        UPDATE_COUNT, ")         ' 43 更新カウント           '2012/11/05 TMEJ彭健  問連修正(GTMC121029047) ADD
                '.Append("        STOPFLG, ")              ' 44 中断フラグ
                '.Append("        PREZID, ")               ' 45 管理予約ID
                '.Append("        REZCHILDNO, ")           ' 46 子予約連番
                '.Append("        STRDATE, ")              ' 54 入庫時間
                '.Append("        CANCELFLG, ")            ' 58 キャンセルフラグ
                '.Append("        INSPECTIONFLG, ")        ' 67 検査フラグ
                '.Append("        DELIVERY_FLG, ")         ' 66 納車済フラグ
                '.Append("        INSTRUCT,")              ' 74 着工指示区分
                '.Append("        WORKSEQ")                ' 75 作業連番
                '.Append("   FROM TBL_STALLREZINFO ")      ' [ストール予約]
                '.Append("  WHERE DLRCD = :DLRCD ")        ' 01 販売店コード
                '.Append("    AND STRCD = :STRCD ")        ' 02 店舗コード
                '.Append("    AND REZID = :REZID")         ' 03 予約ID

                .Append(" SELECT /* SC3150101_001 */ ")
                .Append("        T1.DLR_CD AS DLRCD ")
                .Append("      , T1.BRN_CD AS STRCD ")
                .Append("      , T3.STALL_USE_ID AS REZID ")
                .Append("      , T3.STALL_ID AS STALLID ")
                .Append("      , DECODE(T3.SCHE_START_DATETIME,:MINDATE,TO_DATE(NULL),T3.SCHE_START_DATETIME) AS STARTTIME ")
                .Append("      , DECODE(T3.SCHE_END_DATETIME,:MINDATE,TO_DATE(NULL),T3.SCHE_END_DATETIME) AS ENDTIME ")
                .Append("      , T1.RESV_STATUS AS STATUS ")
                .Append("      , T1.CARWASH_NEED_FLG AS WASHFLG ")
                .Append("      , T1.PICK_DELI_TYPE AS REZ_RECEPTION ")
                .Append("      , T3.SCHE_WORKTIME AS REZ_WORK_TIME ")
                .Append("      , DECODE(T1.SCHE_SVCIN_DATETIME,:MINDATE,NULL,TO_CHAR(T1.SCHE_SVCIN_DATETIME,'YYYYMMDDHH24MI')) AS REZ_PICK_DATE ")
                .Append("      , T4.PICK_DESTINATION AS REZ_PICK_LOC ")
                .Append("      , TO_CHAR(T4.PICK_WORKTIME) AS REZ_PICK_TIME ")
                .Append("      , DECODE(T1.SCHE_DELI_DATETIME,:MINDATE,NULL,TO_CHAR(T1.SCHE_DELI_DATETIME,'YYYYMMDDHH24MI')) AS REZ_DELI_DATE ")
                .Append("      , T5.DELI_DESTINATION AS REZ_DELI_LOC ")
                .Append("      , T5.DELI_WORKTIME AS REZ_DELI_TIME ")
                .Append("      , T1.ROW_LOCK_VERSION AS UPDATE_COUNT ")
                .Append("      , DECODE(T3.STALL_USE_STATUS, :SUS05, :STOPFLG_1, :STOPFLG_0) AS STOPFLG ")
                .Append("      , DECODE(T1.RSLT_SVCIN_DATETIME,:MINDATE,:STRMINDATE,T1.RSLT_SVCIN_DATETIME) AS STRDATE ")
                .Append("      , ROW_NUMBER() OVER (PARTITION BY T2.JOB_DTL_ID ")
                .Append("                           ORDER BY T1.SVCIN_ID ASC ")
                .Append("                                  , T2.JOB_DTL_ID ASC ")
                .Append("                          ) AS REZCHILDNO ")
                .Append("      , T2.CANCEL_FLG AS CANCELFLG ")
                .Append("      , DECODE(T1.SVC_STATUS, :SS13, :DELIVERY_FLG_1, :DELIVERY_FLG_0) AS DELIVERY_FLG ")
                .Append("      , T2.INSPECTION_NEED_FLG AS INSPECTIONFLG ")
                .Append("      , DECODE(T3.STALL_USE_STATUS,:SUS00 ,:CN_INSTRUCT_0, :SUS07, :CN_INSTRUCT_0, :CN_INSTRUCT_2) AS INSTRUCT ")
                .Append("      , 0 AS WORKSEQ ")
                .Append("      , T2.SVCIN_ID AS PREZID ")
                .Append("      , T1.SVC_STATUS ")
                .Append("      , T3.STALL_USE_STATUS ")
                .Append("      , T2.JOB_DTL_ID ")
                .Append("      , TRIM(T1.PIC_SA_STF_CD) ")
                .Append("      , T1.RO_NUM ")
                .Append("   FROM TB_T_SERVICEIN T1 ")
                .Append("      , TB_T_JOB_DTL T2 ")
                .Append("      , TB_T_STALL_USE T3 ")
                .Append("      , TB_T_VEHICLE_PICKUP T4 ")
                .Append("      , TB_T_VEHICLE_DELIVERY T5 ")
                .Append("  WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .Append("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .Append("    AND T1.SVCIN_ID = T4.SVCIN_ID (+) ")
                .Append("    AND T1.SVCIN_ID = T5.SVCIN_ID (+) ")
                .Append("    AND T3.DLR_CD = :DLR_CD ")
                .Append("    AND T3.BRN_CD = :BRN_CD ")
                .Append("    AND T3.STALL_USE_ID = :STALL_USE_ID ")

                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
            End With

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallReserveInfoDataTable)("SC3150101_001")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("STRMINDATE", OracleDbType.Date, Date.Parse(strMinDate, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("CN_INSTRUCT_2", OracleDbType.NVarchar2, CN_INSTRUCT_2)
                query.AddParameterWithTypeValue("CN_INSTRUCT_0", OracleDbType.NVarchar2, CN_INSTRUCT_0)
                query.AddParameterWithTypeValue("DELIVERY_FLG_1", OracleDbType.NVarchar2, DELIVERY_FLG_1)
                query.AddParameterWithTypeValue("DELIVERY_FLG_0", OracleDbType.NVarchar2, DELIVERY_FLG_0)
                query.AddParameterWithTypeValue("SS13", OracleDbType.NVarchar2, sarviceStatus13)
                query.AddParameterWithTypeValue("SUS00", OracleDbType.NVarchar2, stallUseStetus00)
                query.AddParameterWithTypeValue("SUS05", OracleDbType.NVarchar2, stallUseStetus05)
                query.AddParameterWithTypeValue("SUS07", OracleDbType.NVarchar2, stallUseStetus07)
                query.AddParameterWithTypeValue("STOPFLG_0", OracleDbType.NVarchar2, STOPFLG_0)
                query.AddParameterWithTypeValue("STOPFLG_1", OracleDbType.NVarchar2, STOPFLG_1)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, reserveId)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                Logger.Info("[E]GetStallReserveInfo()")

                ' 検索結果の返却
                Return query.GetData()
            End Using


        End Function

        ' 2012/06/05 KN 彭 コード分析対応 START

        ''' <summary>
        ''' ストール実績情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetStallProcessInfo(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal reserveId As Decimal) As SC3150101DataSet.SC3150101StallProcessInfoDataTable

            Logger.Info("[S]GetStallProcessInfo()")

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                '.Append("    SELECT /* SC3150101_002 */ ")
                '.Append("           T1.DLRCD AS DLRCD, ")                                        ' 01 販売店コード
                '.Append("           T1.STRCD AS STRCD, ")                                        ' 02 店舗コード
                '.Append("           T1.REZID AS REZID, ")                                        ' 03 予約ID
                ''.Append("           T2.DSEQNO AS DSEQNO, ")                                      ' 04 日跨ぎシーケンス番号
                ''.Append("           T2.SEQNO AS SEQNO, ")                                        ' 05 シーケンス番号
                '.Append("           NVL(T2.DSEQNO, 0) AS DSEQNO, ")                                      ' 04 日跨ぎシーケンス番号
                '.Append("           NVL(T2.SEQNO, 0) AS SEQNO, ")                                        ' 05 シーケンス番号
                '.Append("           T2.RESULT_STATUS AS RESULT_STATUS, ")                        ' 16 実績_ステータス
                ''.Append("           T2.RESULT_STALLID AS RESULT_STALLID, ")                      ' 17 実績_ストールID
                '.Append("           NVL(T2.RESULT_STALLID, 0) AS RESULT_STALLID, ")                      ' 17 実績_ストールID
                '.Append("           T2.RESULT_START_TIME AS RESULT_START_TIME, ")                ' 18 実績_ストール開始日時時刻
                '.Append("           T2.RESULT_END_TIME AS RESULT_END_TIME, ")                    ' 19 実績_ストール終了日時時刻
                ''.Append("           T2.RESULT_WORK_TIME AS RESULT_WORK_TIME, ")                  ' 21 実績_実績時間
                '.Append("           NVL(T2.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME, ")                  ' 21 実績_実績時間
                '.Append("           T2.RESULT_IN_TIME AS RESULT_IN_TIME, ")                      ' 20 実績_入庫時間
                '.Append("           T2.REZ_START_TIME AS REZ_START_TIME, ")                      ' 23 予定_ストール開始日時時刻
                '.Append("           T2.REZ_END_TIME AS REZ_END_TIME, ")                          ' 24 予定_ストール終了日時時刻
                '.Append("           NVL(T2.REZ_WORK_TIME, T1.REZ_WORK_TIME) AS REZ_WORK_TIME, ") ' 25 予定_作業時間
                '.Append("           T2.RESULT_WASH_START AS RESULT_WASH_START, ")                ' 34 洗車開始時刻
                '.Append("           T2.RESULT_WASH_END AS RESULT_WASH_END, ")                    ' 35 洗車終了時刻
                '.Append("           T2.RESULT_WAIT_START AS RESULT_WAIT_START, ")                ' 36 納車待ち開始時刻
                '.Append("           T2.RESULT_WAIT_END AS RESULT_WAIT_END, ")                    ' 37 納車待ち終了時刻
                '.Append("           T2.RESULT_INSPECTION_START AS RESULT_INSPECTION_START, ")    ' 51 実績検査開始時刻
                '.Append("           T2.RESULT_INSPECTION_END AS RESULT_INSPECTION_END ")         ' 52 実績検査終了時刻
                '.Append("      FROM TBL_STALLREZINFO T1, ")                                      ' [ストール予約]
                '.Append("           TBL_STALLPROCESS T2 ")                                       ' [ストール実績]
                '.Append("     WHERE T1.DLRCD = T2.DLRCD (+) ")                                       ' 01 販売店コード
                '.Append("       AND T1.STRCD = T2.STRCD (+) ")                                       ' 02 店舗コード
                '.Append("       AND T1.REZID = T2.REZID (+) ")                                       ' 03 予約ID
                '.Append("       AND T1.DLRCD = :DLRCD ")                                         ' 01 販売店コード
                '.Append("       AND T1.STRCD = :STRCD ")                                         ' 02 店舗コード
                '.Append("       AND T1.REZID = :REZID ")                                         ' 03 予約ID
                '.Append("       AND (T2.SEQNO IS NULL ")                                         ' 05 シーケンス番号
                '.Append("           OR (T2.DSEQNO = (SELECT MAX(T3.DSEQNO) ")                    ' 04 日跨ぎシーケンス番号
                '.Append("                              FROM TBL_STALLPROCESS T3 ")               ' [ストール実績]
                '.Append("                             WHERE T3.DLRCD = T2.DLRCD ")               ' 01 販売店コード
                '.Append("                               AND T3.STRCD = T2.STRCD ")               ' 02 店舗コード
                '.Append("                               AND T3.REZID = T2.REZID ")               ' 03 予約ID
                '.Append("                          GROUP BY T3.DLRCD, T3.STRCD, T3.REZID) ")
                '.Append("          AND T2.SEQNO = (SELECT MAX(T4.SEQNO) ")                       ' 05 シーケンス番号
                '.Append("                            FROM TBL_STALLPROCESS T4 ")                 ' [ストール実績]
                '.Append("                           WHERE T4.DLRCD = T2.DLRCD ")                 ' 01 販売店コード
                '.Append("                             AND T4.STRCD = T2.STRCD ")                 ' 02 店舗コード
                '.Append("                             AND T4.REZID = T2.REZID ")                 ' 03 予約ID
                '.Append("                             AND T4.DSEQNO = T2.DSEQNO) ")              ' 04 日跨ぎシーケンス番号
                '.Append("              ) ")
                '.Append("           )")

                .Append("SELECT /* SC3150101_002 */ ")
                .Append("       T1.DLR_CD AS DLRCD ")
                .Append("     , T1.BRN_CD AS STRCD ")
                .Append("     , T3.STALL_USE_ID AS REZID ")
                .Append("     , 0 AS DSEQNO ")
                .Append("     , T2.JOB_DTL_ID AS SEQNO ")
                .Append("     , T1.SVC_STATUS AS RESULT_STATUS ")
                .Append("     , T3.STALL_ID AS RESULT_STALLID ")
                .Append("     , DECODE(T3.RSLT_START_DATETIME,:MINDATE, NULL, TO_CHAR(T3.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_START_TIME ")
                .Append("     , DECODE(T3.PRMS_END_DATETIME, :MINDATE, NULL, TO_CHAR(T3.PRMS_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_END_TIME ")
                .Append("     , T3.TEMP_FLG AS TEMP_FLG ")
                .Append("     , 0 AS PARTS_FLG ")
                .Append("     , T3.REST_FLG AS REST_FLG ")
                .Append("     , NVL(T3.RSLT_WORKTIME, 0) AS RESULT_WORK_TIME ")
                .Append("     , T1.RSLT_SVCIN_DATETIME AS RESULT_IN_TIME ")
                .Append("     , DECODE(T3.SCHE_START_DATETIME, :MINDATE, NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_START_TIME ")
                .Append("     , DECODE(T3.SCHE_END_DATETIME, :MINDATE, NULL, TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_END_TIME ")
                .Append("     , T3.SCHE_WORKTIME AS REZ_WORK_TIME ")
                .Append("     , DECODE(T4.RSLT_START_DATETIME,:MINDATE, NULL, TO_CHAR(T4.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WASH_START ")
                .Append("     , DECODE(T4.RSLT_END_DATETIME, :MINDATE, NULL, TO_CHAR(T4.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WASH_END ")
                .Append("     , DECODE(T1.RSLT_DELI_DATETIME, :MINDATE, NULL, TO_CHAR(T1.RSLT_DELI_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WAIT_END ")
                .Append("     , DECODE(T5.RSLT_START_DATETIME, :MINDATE, NULL, TO_CHAR(T5.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_INSPECTION_START ")
                .Append("     , DECODE(T5.RSLT_END_DATETIME, :MINDATE, NULL, TO_CHAR(T5.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_INSPECTION_END ")
                .Append("     , T3.STALL_USE_STATUS ")
                .Append("     , T3.JOB_ID ")
                .Append("     , T2.INSPECTION_STATUS ")
                .Append("     , T1.ROW_LOCK_VERSION AS UPDATE_COUNT ")
                .Append("     , TRIM(T1.PIC_SA_STF_CD) AS PIC_SA_STF_CD ")
                .Append("     , T1.RO_NUM ")
                .Append("  FROM TB_T_SERVICEIN T1 ")
                .Append("     , TB_T_JOB_DTL T2 ")
                .Append("     , TB_T_STALL_USE T3 ")
                .Append("     , TB_T_CARWASH_RESULT T4 ")
                .Append("     , TB_T_INSPECTION_RESULT T5 ")
                .Append(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .Append("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .Append("   AND T1.SVCIN_ID = T4.SVCIN_ID(+) ")
                .Append("   AND T2.JOB_DTL_ID = T5.JOB_DTL_ID(+) ")
                .Append("   AND T1.DLR_CD = T2.DLR_CD(+) ")
                .Append("   AND T2.DLR_CD = T3.DLR_CD(+) ")
                .Append("   AND T1.BRN_CD = T2.BRN_CD(+) ")
                .Append("   AND T2.BRN_CD = T3.BRN_CD(+) ")
                .Append("   AND T3.DLR_CD = :DLR_CD ")
                .Append("   AND T3.BRN_CD = :BRN_CD ")
                .Append("   AND T3.STALL_USE_ID = :STALL_USE_ID ")
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
            End With

            Dim stallProcessInfoTable As SC3150101DataSet.SC3150101StallProcessInfoDataTable

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallProcessInfoDataTable)("SC3150101_002")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, reserveId)
                'query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int64, ROW_NUM)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                ' SQLの実行
                stallProcessInfoTable = query.GetData()
            End Using

            If stallProcessInfoTable.Rows.Count <> 0 Then
                stallProcessInfoTable.Rows.Item(0).Item("DLRCD") = SetData(stallProcessInfoTable.Rows.Item(0).Item("DLRCD"), "")
                stallProcessInfoTable.Rows.Item(0).Item("STRCD") = SetData(stallProcessInfoTable.Rows.Item(0).Item("STRCD"), "")
                stallProcessInfoTable.Rows.Item(0).Item("REZID") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZID"), 0)
                stallProcessInfoTable.Rows.Item(0).Item("DSEQNO") = SetData(stallProcessInfoTable.Rows.Item(0).Item("DSEQNO"), 0)
                stallProcessInfoTable.Rows.Item(0).Item("SEQNO") = SetData(stallProcessInfoTable.Rows.Item(0).Item("SEQNO"), 0)
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_STATUS") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_STATUS"), "0")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_STALLID") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_STALLID"), 0)
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_START_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_START_TIME"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_END_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_END_TIME"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_WORK_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WORK_TIME"), 0)
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_IN_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_IN_TIME"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_START") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_START"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_END") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_END"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_START") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_START"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_END") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_END"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_START") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_START"), "")
                stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_END") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_END"), "")
                stallProcessInfoTable.Rows.Item(0).Item("REZ_START_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZ_START_TIME"), "")
                stallProcessInfoTable.Rows.Item(0).Item("REZ_END_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZ_END_TIME"), "")
                stallProcessInfoTable.Rows.Item(0).Item("REZ_WORK_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZ_WORK_TIME"), 0)
            Else
                Logger.Info("[E]GetStallProcessInfo()")
                Return Nothing
            End If

            Logger.Info("[E]GetStallProcessInfo()")
            Return (stallProcessInfoTable)

        End Function

        '2013/12/10 TMEJ 成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        ' ''' <summary>
        ' ''' 予約ストール利用情報更新
        ' ''' </summary>
        ' ''' <param name="reserveInfo">ストール予約情報</param>
        ' ''' <param name="updateAccount">アカウント</param>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function UpdateReserveStallUse(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable,
        '                                      ByVal updateAccount As String, _
        '                                      ByVal updateDate As Date) As Integer

        '    Logger.Info("[S]UpdateReserveStallUse()")

        '    ' 引数チェック
        '    If reserveInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:UpdateReserveStallUse()]")
        '        Logger.Info("[E]UpdateReserveStallUse()")
        '        Return (-1)
        '    End If


        '    '-----------------
        '    ' データセットを展開
        '    Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
        '    '-----------------


        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        '.Append(" UPDATE /* SC3150101_003 */ ")
        '        '.Append("        TBL_STALLREZINFO ")
        '        '.Append("    SET STALLID = :STALLID, ")               ' 05 ストールID
        '        '.Append("        STARTTIME = :STARTTIME, ")           ' 06 使用開始日時
        '        '.Append("        ENDTIME = :ENDTIME, ")               ' 07 使用終了日時
        '        '.Append("        REZ_WORK_TIME = :REZ_WORK_TIME, ")   ' 34 予定_作業時間
        '        '.Append("        STATUS = :STATUS, ")                 ' 19 ステータス
        '        'If drReserveInfo.STRDATE = DateTime.MinValue Then
        '        '    .Append("        STRDATE = NULL, ")                   ' 54 入庫日時
        '        'Else
        '        '    .Append("        STRDATE = :STRDATE, ")               ' 54 入庫日時
        '        'End If
        '        '.Append("        WASHFLG = :WASHFLG, ")               ' 30 洗車フラグ
        '        '.Append("        INSPECTIONFLG = :INSPECTIONFLG, ")   ' 67 検査フラグ
        '        '.Append("        STOPFLG = :STOPFLG, ")               ' 44 中断フラグ
        '        'If CType(drReserveInfo.STOPFLG, Integer) = 0 Then
        '        '    .Append("        CANCELFLG = '0', ")                  ' 58 キャンセルフラグ
        '        'Else
        '        '    .Append("        CANCELFLG = '1', ")                  ' 58 キャンセルフラグ
        '        'End If
        '        '.Append("        DELIVERY_FLG = :DELIVERY_FLG, ")     ' 66 納車済フラグ
        '        '.Append("        UPDATE_COUNT = UPDATE_COUNT + 1, ")  ' 43 更新カウント
        '        '.Append("        UPDATEACCOUNT = :UPDATEACCOUNT, ")   ' 61 更新ユーザーアカウント
        '        '.Append("        UPDATEDATE = SYSDATE ")
        '        'If updateStartTime = 0 Then
        '        '    .Append("      , ACTUAL_STIME = NULL ")             ' 47 作業開始時間
        '        'ElseIf updateStartTime = 1 Then
        '        '    .Append("      , ACTUAL_STIME = :ACTUAL_STIME ")    ' 47 作業開始時間
        '        'End If
        '        'If updateEndTime = 0 Then
        '        '    .Append("      , ACTUAL_ETIME = NULL ")             ' 48 作業終了時間
        '        'ElseIf updateEndTime = 1 Then
        '        '    .Append("      , ACTUAL_ETIME = :ACTUAL_ETIME ")    ' 48 作業終了時間
        '        'End If
        '        'If newChildNo > 0 Then
        '        '    .Append("      , REZCHILDNO = :REZCHILDNO ")        ' 46 子予約連番
        '        'End If
        '        '.Append("  WHERE DLRCD = :DLRCD ")                    ' 01 販売店コード
        '        '.Append("    AND STRCD = :STRCD ")                    ' 02 店舗コード
        '        '.Append("    AND REZID = :REZID ")                    ' 03 予約ID

        '        .Append(" UPDATE /* SC3150101_003 */ ")
        '        .Append("        TB_T_STALL_USE ")
        '        .Append("    SET STALL_ID = :STALL_ID ")
        '        .Append("      , STALL_USE_STATUS = :STALL_USE_STATUS ")
        '        .Append("      , UPDATE_DATETIME = :UPDATEDATE ")
        '        .Append("      , UPDATE_STF_CD = :STF_CD ")
        '        .Append("      , ROW_UPDATE_DATETIME = :UPDATEDATE ")
        '        .Append("      , ROW_UPDATE_ACCOUNT = :STF_CD ")
        '        .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '        .Append("      , ROW_LOCK_VERSION = (ROW_LOCK_VERSION + 1) ")
        '        .Append("  WHERE DLR_CD = :DLR_CD ")
        '        .Append("    AND BRN_CD = :BRN_CD ")
        '        .Append("    AND STALL_USE_ID = :STALL_USE_ID ")
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_003")

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, drReserveInfo.STALLID)             ' 05 ストールID
        '        'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, drReserveInfo.STARTTIME)          ' 06 使用開始日時
        '        'query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, drReserveInfo.ENDTIME)              ' 07 使用終了日時
        '        'query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, drReserveInfo.REZ_WORK_TIME)      ' 34 予定_作業時間
        '        'query.AddParameterWithTypeValue("STATUS", OracleDbType.Int64, drReserveInfo.STATUS)            ' 19 ステータス
        '        'If drReserveInfo.STRDATE <> DateTime.MinValue Then
        '        '    query.AddParameterWithTypeValue("STRDATE", OracleDbType.Date, drReserveInfo.STRDATE)            ' 54 入庫日時
        '        'End If
        '        'query.AddParameterWithTypeValue("WASHFLG", OracleDbType.Char, drReserveInfo.WASHFLG)              ' 30 洗車フラグ
        '        'query.AddParameterWithTypeValue("INSPECTIONFLG", OracleDbType.Char, drReserveInfo.INSPECTIONFLG)  ' 67 検査フラグ
        '        'query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, drReserveInfo.STOPFLG)              ' 44 中断フラグ
        '        'query.AddParameterWithTypeValue("DELIVERY_FLG", OracleDbType.Char, drReserveInfo.DELIVERY_FLG)     ' 66 納車済フラグ
        '        'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)  ' 61 更新ユーザーアカウント
        '        'If updateStartTime = 1 Then
        '        '    query.AddParameterWithTypeValue("ACTUAL_STIME", OracleDbType.Date, actualStartTime)  ' 47 作業開始時間
        '        'End If
        '        'If updateEndTime = 1 Then
        '        '    query.AddParameterWithTypeValue("ACTUAL_ETIME", OracleDbType.Date, actualEndTime)  ' 48 作業終了時間
        '        'End If
        '        'If newChildNo > 0 Then
        '        '    query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, newChildNo)           ' 46 子予約連番
        '        'End If
        '        ''query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)                  ' 販売店コード
        '        ''query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)                  ' 店舗コード
        '        ''query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)                     ' 予約ID
        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drReserveInfo.DLRCD)                  ' 販売店コード
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drReserveInfo.STRCD)                  ' 店舗コード
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drReserveInfo.REZID)                 ' 予約ID

        '        'Dim strStartTime As String = drReserveInfo.STARTTIME.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())                          ' 稼働時間From
        '        'Dim strEndTime As String = drReserveInfo.ENDTIME.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())
        '        query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, drReserveInfo.STALLID)                 ' 05 ストールID
        '        query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, drReserveInfo.STALL_USE_STATUS)        ' 19 ステータス
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, drReserveInfo.DLRCD)                      ' 販売店コード
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, drReserveInfo.STRCD)                      ' 店舗コード
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, drReserveInfo.REZID)               ' 作業内容ID
        '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, updateAccount)                        ' 61 更新スタッフコード
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)          ' 行更新機能
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)                    ' 更新日時
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        '        Logger.Info("[E]UpdateReserveStallUse()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function

        ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        ' ''' <summary>
        ' ''' 予約サービス入庫情報更新
        ' ''' </summary>
        ' ''' <param name="reserveInfo">ストール予約情報</param>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history> 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function UpdateReserveServiceIn(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable) As Integer

        '    Logger.Info("[S]UpdateReserveServiceIn()")

        '    ' 引数チェック
        '    If reserveInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:UpdateReserveServiceIn()]")
        '        Logger.Info("[E]UpdateReserveServiceIn()")
        '        Return (-1)
        '    End If

        '    ' データセットを展開
        '    '-----------------
        '    'ストール予約データセット
        '    Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
        '    '-----------------

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append(" UPDATE  /* SC3150101_037 */ ")
        '        .Append("        TB_T_SERVICEIN ")
        '        .Append("    SET  ")
        '        If drReserveInfo.STRDATE = Date.Parse(MINDATE, CultureInfo.InvariantCulture) Then
        '            .Append("    RSLT_SVCIN_DATETIME = :MINDATE ")
        '        Else
        '            .Append("    RSLT_SVCIN_DATETIME = :RSLT_SVCIN_DATETIME ")
        '        End If
        '        .Append("      , SVC_STATUS = :SVC_STATUS ")
        '        .Append("      , CARWASH_NEED_FLG = :CARWASH_NEED_FLG ")
        '        .Append("  WHERE DLR_CD = :DLR_CD ")
        '        .Append("    AND BRN_CD = :BRN_CD ")
        '        .Append("    AND SVCIN_ID = :SVCIN_ID ")
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_037")

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        If drReserveInfo.STRDATE = Date.Parse(MINDATE, CultureInfo.InvariantCulture) Then
        '            query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
        '        Else
        '            Dim strStrDate As String = drReserveInfo.STRDATE.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())
        '            query.AddParameterWithTypeValue("RSLT_SVCIN_DATETIME", OracleDbType.Date, Date.Parse(strStrDate, CultureInfo.InvariantCulture))       ' 実績入庫日時
        '        End If
        '        query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, drReserveInfo.SVC_STATUS)              ' サービスステータス
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, drReserveInfo.DLRCD)                  ' 販売店コード
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, drReserveInfo.STRCD)                  ' 店舗コード
        '        query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, drReserveInfo.PREZID)                  ' サービス入庫ID
        '        query.AddParameterWithTypeValue("CARWASH_NEED_FLG", OracleDbType.Int64, drReserveInfo.WASHFLG)                ' 洗車必要フラグ
        '        Logger.Info("[E]UpdateReserveServiceIn()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using


        'End Function


        ' ''' <summary>
        ' ''' 作業内容情報更新
        ' ''' </summary>
        ' ''' <param name="reserveInfo">ストール予約情報</param>
        ' ''' <param name="updateAccount">アカウント</param>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history> 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function UpdateJobDetail(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
        '                                ByVal updateAccount As String, _
        '                                ByVal updateDate As Date) As Integer

        '    Logger.Info("[S]UpdateJobDetail()")

        '    ' 引数チェック
        '    If reserveInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:UpdateJobDetail()]")
        '        Logger.Info("[E]UpdateJobDetail()")
        '        Return (-1)
        '    End If

        '    ' データセットを展開
        '    '-----------------
        '    'ストール予約データセット
        '    Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
        '    '-----------------

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append(" UPDATE /* SC3150101_038 */ ")
        '        .Append("        TB_T_JOB_DTL ")
        '        .Append("    SET  ")
        '        If drReserveInfo.STOPFLG.Equals("0") Then
        '            .Append("        CANCEL_FLG = :CANCEL_FLG_0 ")
        '        Else
        '            .Append("        CANCEL_FLG = :CANCEL_FLG_1 ")
        '        End If
        '        .Append("      , INSPECTION_NEED_FLG = :INSPECTION_NEED_FLG ")
        '        .Append("      , UPDATE_DATETIME = :UPDATEDATE ")
        '        .Append("      , UPDATE_STF_CD = :STF_CD ")
        '        .Append("      , ROW_UPDATE_DATETIME = :UPDATEDATE ")
        '        .Append("      , ROW_UPDATE_ACCOUNT = :STF_CD ")
        '        .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '        .Append("      , ROW_LOCK_VERSION = (ROW_LOCK_VERSION + 1) ")
        '        .Append("  WHERE DLR_CD = :DLR_CD ")
        '        .Append("    AND BRN_CD = :BRN_CD ")
        '        .Append("    AND JOB_DTL_ID = (   ")
        '        .Append("                      SELECT JOB_DTL_ID ")
        '        .Append("                        FROM TB_T_STALL_USE ")
        '        .Append("                       WHERE DLR_CD = :DLR_CD ")
        '        .Append("                         AND BRN_CD = :BRN_CD ")
        '        .Append("                         AND STALL_USE_ID = :STALL_USE_ID ")
        '        .Append("                      ) ")

        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_038")
        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("INSPECTION_NEED_FLG", OracleDbType.NVarchar2, drReserveInfo.INSPECTIONFLG)  ' 検査必要フラグ
        '        If drReserveInfo.STOPFLG.Equals("0") Then
        '            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)            ' キャンセルフラグ
        '        Else
        '            query.AddParameterWithTypeValue("CANCEL_FLG_1", OracleDbType.NVarchar2, CANCEL_FLG_1)            ' キャンセルフラグ
        '        End If
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, drReserveInfo.DLRCD)            ' 販売店コード
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, drReserveInfo.STRCD)            ' 店舗コード
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, drReserveInfo.REZID)         ' ストール利用ID
        '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, updateAccount)                   ' 更新スタッフコード
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)     ' 行更新機能
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)          ' 更新日時
        '        Logger.Info("[E]UpdateJobDetail()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using


        'End Function

        '2013/12/10 TMEJ 成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        ' ''' <summary>
        ' ''' ストール予約情報を更新する。
        ' ''' </summary>
        ' ''' <param name="reserveInfo">ストール予約情報</param>
        ' ''' <param name="actualStartTime">販売点コード</param>
        ' ''' <param name="actualEndTime">店舗コード</param>
        ' ''' <param name="updateStartTime">作業開始時間の更新方法(0:Nullで上書き, 1:指定値で上書き, 2:変更しない)</param>
        ' ''' <param name="updateEndTime">作業終了時間の更新方法(0:Nullで上書き, 1:指定値で上書き, 2:変更しない)</param>
        ' ''' <param name="updateAccount">アカウント</param>
        ' ''' <param name="newChildNo"></param>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history> 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function UpdateStallReserveInfo(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
        '                                       ByVal actualStartTime As Date, _
        '                                       ByVal actualEndTime As Date, _
        '                                       ByVal updateStartTime As Integer, _
        '                                       ByVal updateEndTime As Integer, _
        '                                       ByVal updateAccount As String, _
        '                                       Optional ByVal newChildNo As Integer = -1) As Integer 'UpdateStallRezInfo
        '    Logger.Info("[S]UpdateStallReserveInfo()")

        '    ' 引数チェック
        '    If reserveInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:UpdateStallReserveInfo()]")
        '        Logger.Info("[E]UpdateStallReserveInfo()")
        '        Return (-1)
        '    End If


        '    '-----------------
        '    ' データセットを展開
        '    Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
        '    '-----------------

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_003")

        '        Dim sql As New StringBuilder


        '        ' SQL文の作成
        '        With sql
        '            .Append(" UPDATE /* SC3150101_003 */ ")
        '            .Append("        TBL_STALLREZINFO ")
        '            .Append("    SET STALLID = :STALLID, ")               ' 05 ストールID
        '            .Append("        STARTTIME = :STARTTIME, ")           ' 06 使用開始日時
        '            .Append("        ENDTIME = :ENDTIME, ")               ' 07 使用終了日時
        '            .Append("        REZ_WORK_TIME = :REZ_WORK_TIME, ")   ' 34 予定_作業時間
        '            .Append("        STATUS = :STATUS, ")                 ' 19 ステータス
        '            If drReserveInfo.STRDATE = DateTime.MinValue Then
        '                .Append("        STRDATE = NULL, ")                   ' 54 入庫日時
        '            Else
        '                .Append("        STRDATE = :STRDATE, ")               ' 54 入庫日時
        '            End If
        '            .Append("        WASHFLG = :WASHFLG, ")               ' 30 洗車フラグ
        '            .Append("        INSPECTIONFLG = :INSPECTIONFLG, ")   ' 67 検査フラグ
        '            .Append("        STOPFLG = :STOPFLG, ")               ' 44 中断フラグ
        '            If CType(drReserveInfo.STOPFLG, Integer) = 0 Then
        '                .Append("        CANCELFLG = '0', ")                  ' 58 キャンセルフラグ
        '            Else
        '                .Append("        CANCELFLG = '1', ")                  ' 58 キャンセルフラグ
        '            End If
        '            .Append("        DELIVERY_FLG = :DELIVERY_FLG, ")     ' 66 納車済フラグ
        '            .Append("        UPDATE_COUNT = UPDATE_COUNT + 1, ")  ' 43 更新カウント
        '            .Append("        UPDATEACCOUNT = :UPDATEACCOUNT, ")   ' 61 更新ユーザーアカウント
        '            .Append("        UPDATEDATE = SYSDATE ")
        '            If updateStartTime = 0 Then
        '                .Append("      , ACTUAL_STIME = NULL ")             ' 47 作業開始時間
        '            ElseIf updateStartTime = 1 Then
        '                .Append("      , ACTUAL_STIME = :ACTUAL_STIME ")    ' 47 作業開始時間
        '            End If
        '            If updateEndTime = 0 Then
        '                .Append("      , ACTUAL_ETIME = NULL ")             ' 48 作業終了時間
        '            ElseIf updateEndTime = 1 Then
        '                .Append("      , ACTUAL_ETIME = :ACTUAL_ETIME ")    ' 48 作業終了時間
        '            End If
        '            If newChildNo > 0 Then
        '                .Append("      , REZCHILDNO = :REZCHILDNO ")        ' 46 子予約連番
        '            End If
        '            .Append("  WHERE DLRCD = :DLRCD ")                    ' 01 販売店コード
        '            .Append("    AND STRCD = :STRCD ")                    ' 02 店舗コード
        '            .Append("    AND REZID = :REZID ")                    ' 03 予約ID
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, drReserveInfo.STALLID)             ' 05 ストールID
        '        query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, drReserveInfo.STARTTIME)          ' 06 使用開始日時
        '        query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, drReserveInfo.ENDTIME)              ' 07 使用終了日時
        '        query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, drReserveInfo.REZ_WORK_TIME)      ' 34 予定_作業時間
        '        query.AddParameterWithTypeValue("STATUS", OracleDbType.Int64, drReserveInfo.STATUS)            ' 19 ステータス
        '        If drReserveInfo.STRDATE <> DateTime.MinValue Then
        '            query.AddParameterWithTypeValue("STRDATE", OracleDbType.Date, drReserveInfo.STRDATE)            ' 54 入庫日時
        '        End If
        '        query.AddParameterWithTypeValue("WASHFLG", OracleDbType.Char, drReserveInfo.WASHFLG)              ' 30 洗車フラグ
        '        query.AddParameterWithTypeValue("INSPECTIONFLG", OracleDbType.Char, drReserveInfo.INSPECTIONFLG)  ' 67 検査フラグ
        '        query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, drReserveInfo.STOPFLG)              ' 44 中断フラグ
        '        query.AddParameterWithTypeValue("DELIVERY_FLG", OracleDbType.Char, drReserveInfo.DELIVERY_FLG)     ' 66 納車済フラグ
        '        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)  ' 61 更新ユーザーアカウント
        '        If updateStartTime = 1 Then
        '            query.AddParameterWithTypeValue("ACTUAL_STIME", OracleDbType.Date, actualStartTime)  ' 47 作業開始時間
        '        End If
        '        If updateEndTime = 1 Then
        '            query.AddParameterWithTypeValue("ACTUAL_ETIME", OracleDbType.Date, actualEndTime)  ' 48 作業終了時間
        '        End If
        '        If newChildNo > 0 Then
        '            query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, newChildNo)           ' 46 子予約連番
        '        End If

        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)                  ' 販売店コード
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)                  ' 店舗コード
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)                     ' 予約ID
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drReserveInfo.DLRCD)                  ' 販売店コード
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drReserveInfo.STRCD)                  ' 店舗コード
        '        query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drReserveInfo.REZID)                 ' 予約ID

        '        Logger.Info("[E]UpdateStallReserveInfo()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function

        ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

        ' ''' <summary>
        ' ''' 実績ストール利用情報更新
        ' ''' </summary>
        ' ''' <param name="procInfo">ストール実績情報</param>
        ' ''' <param name="updateAccount">アカウント</param>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history> 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function UpdateProcessStallUse(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable,
        '                                      ByVal updateAccount As String, _
        '                                      ByVal updateDate As Date) As Integer

        '    Logger.Info("[S]UpdateProcessStallUse()")

        '    ' 引数チェック
        '    If procInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:UpdateProcessStallUse()]")
        '        Logger.Info("[E]UpdateProcessStallUse()")
        '        Return (-1)
        '    End If

        '    ' データセットを展開
        '    '-----------------
        '    'ストール予約データセット
        '    'Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    'If reserveInfo IsNot Nothing Then
        '    '    drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
        '    'Else
        '    '    drReserveInfo = Nothing
        '    'End If
        '    'ストール実績データセット
        '    Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
        '    drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
        '    '-----------------

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append(" UPDATE /* SC3150101_039 */ ")
        '        .Append("        TB_T_STALL_USE ")
        '        .Append("    SET STALL_USE_STATUS = :STALL_USE_STATUS ")
        '        .Append("      , STALL_ID = :STALL_ID ")
        '        .Append("      , RSLT_START_DATETIME = :RSLT_START_DATETIME ")
        '        If drProcInfo.STALL_USE_STATUS.Equals("02") Then
        '            .Append("      , PRMS_END_DATETIME = :RSLT_END_DATETIME ")
        '            .Append("      , JOB_ID = :JOB_ID ")
        '        Else
        '            .Append("      , RSLT_END_DATETIME = :RSLT_END_DATETIME ")
        '        End If
        '        If drProcInfo.RESULT_WORK_TIME >= 0 And CType(drProcInfo.STALL_USE_STATUS, Integer) > 2 Then
        '            .Append("  , RSLT_WORKTIME = :RSLT_WORKTIME ")
        '        Else
        '            .Append("  , RSLT_WORKTIME = :RSLT_WORKTIME_0 ")
        '        End If
        '        .Append("      , REST_FLG = :REST_FLG ")
        '        .Append("      , UPDATE_DATETIME = :UPDATEDATE ")
        '        .Append("      , UPDATE_STF_CD = :STF_CD ")
        '        .Append("      , ROW_UPDATE_DATETIME = :UPDATEDATE ")
        '        .Append("      , ROW_UPDATE_ACCOUNT = :STF_CD ")
        '        .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '        .Append("      , ROW_LOCK_VERSION = (ROW_LOCK_VERSION + 1) ")
        '        .Append("  WHERE DLR_CD = :DLR_CD ")
        '        .Append("    AND BRN_CD = :BRN_CD ")
        '        .Append("    AND STALL_USE_ID =:STALL_USE_ID ")
        '    End With


        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_039")

        '        query.CommandText = sql.ToString()


        '        ' バインド変数定義
        '        Dim resultStartTime As Date = Date.ParseExact(drProcInfo.RESULT_START_TIME, "yyyyMMddHHmm", Nothing)
        '        Dim resultEndTime As Date = Date.ParseExact(drProcInfo.RESULT_END_TIME, "yyyyMMddHHmm", Nothing)
        '        query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, drProcInfo.STALL_USE_STATUS)            ' ストール利用ステータス
        '        query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, drProcInfo.RESULT_STALLID)                          ' ストールID
        '        query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, resultStartTime)                          ' 実績開始日時
        '        query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, resultEndTime)                              ' 実績終了日時
        '        If drProcInfo.STALL_USE_STATUS.Equals("02") Then
        '            query.AddParameterWithTypeValue("JOB_ID", OracleDbType.Decimal, drProcInfo.JOB_ID)                                '作業ID
        '        End If
        '        If drProcInfo.RESULT_WORK_TIME >= 0 And CType(drProcInfo.STALL_USE_STATUS, Integer) > 2 Then                        ' 作業時間が0以上 かつ ストール利用ステータスが02(作業中)より大きい 時
        '            query.AddParameterWithTypeValue("RSLT_WORKTIME", OracleDbType.Int64, drProcInfo.RESULT_WORK_TIME)               ' 実績作業時間
        '        Else
        '            query.AddParameterWithTypeValue("RSLT_WORKTIME_0", OracleDbType.Int64, RSLT_WORKTIME_0)
        '        End If
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, drProcInfo.DLRCD)                                 ' 販売店コード
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, drProcInfo.STRCD)                                 ' 店舗コード
        '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, updateAccount)                                     ' 更新スタッフコード
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)                       ' 行更新機能
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Int64, drProcInfo.REZID)                               ' ストール利用ID
        '        query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, drProcInfo.REST_FLG)                               ' 休憩取得フラグ
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)                                      ' 更新日時
        '        Logger.Info("[E]UpdateProcessStallUse()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function


        ' ''' <summary>
        ' ''' 実績サービス入庫情報更新
        ' ''' </summary>
        ' ''' <param name="reserveInfo">ストール予約情報</param>
        ' ''' <param name="procInfo">ストール実績情報</param>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history> 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function UpdateProcessServiceIn(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
        '                                       ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable _
        '                                       ) As Integer

        '    Logger.Info("[S]UpdateProcessServiceIn()")

        '    ' 引数チェック
        '    If procInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:UpdateProcessServiceIn()]")
        '        Logger.Info("[E]UpdateProcessServiceIn()")
        '        Return (-1)
        '    End If

        '    ' データセットを展開
        '    '-----------------
        '    'ストール予約データセット
        '    Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    If reserveInfo IsNot Nothing Then
        '        drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
        '    Else
        '        drReserveInfo = Nothing
        '    End If
        '    'ストール実績データセット
        '    Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
        '    drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
        '    '-----------------

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append("UPDATE /* SC3150101_040 */ ")
        '        .Append("        TB_T_SERVICEIN ")
        '        .Append("    SET RSLT_SVCIN_DATETIME = :RSLT_SVCIN_DATETIME ")
        '        .Append("      , SVC_STATUS = :SVC_STATUS ")
        '        If reserveInfo IsNot Nothing Then
        '            .Append("  , PICK_DELI_TYPE = :PICK_DELI_TYPE ")
        '        End If
        '        .Append("  WHERE DLR_CD = :DLR_CD ")
        '        .Append("    AND BRN_CD = :BRN_CD ")
        '        .Append("    AND SVCIN_ID = ( ")
        '        .Append("                     SELECT SVCIN_ID ")
        '        .Append("                       FROM TB_T_JOB_DTL ")
        '        .Append("                     WHERE DLR_CD = :DLR_CD ")
        '        .Append("                        AND BRN_CD = :BRN_CD ")
        '        .Append("                        AND JOB_DTL_ID = :JOB_DTL_ID ")
        '        .Append("                     ) ")
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_040")
        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("RSLT_SVCIN_DATETIME", OracleDbType.Date, drProcInfo.RESULT_IN_TIME)               ' 実績入庫日時
        '        If reserveInfo IsNot Nothing Then
        '            query.AddParameterWithTypeValue("PICK_DELI_TYPE", OracleDbType.NVarchar2, drReserveInfo.REZ_RECEPTION)         ' 予約_受付納車区分
        '        End If
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, drProcInfo.DLRCD)                                ' 販売店コード
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, drProcInfo.STRCD)                                ' 店舗コード
        '        query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, drProcInfo.SEQNO)                                ' 作業内容ID
        '        query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, drProcInfo.RESULT_STATUS)                    ' サービスステータス
        '        Logger.Info("[E]UpdateProcessServiceIn()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using


        'End Function


        ' ''' <summary>
        ' ''' ストール実績情報の更新
        ' ''' </summary>
        ' ''' <param name="procInfo">ストール実績情報</param>
        ' ''' <param name="reserveInfo">ストール予約情報</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history> 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function UpdateStallProcessInfo(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
        '                                       ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable) As Integer

        '    Logger.Info("[S]UpdateStallProcessInfo()")

        '    ' 引数チェック
        '    If procInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:UpdateStallProcessInfo()]")
        '        Logger.Info("[E]UpdateStallProcessInfo()")
        '        Return (-1)
        '    End If

        '    '-----------------
        '    Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
        '    drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
        '    Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    If reserveInfo IsNot Nothing Then
        '        drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
        '    Else
        '        drReserveInfo = Nothing
        '    End If
        '    '-----------------

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append(" UPDATE /* SC3150101_004 */ ")
        '        .Append("        tbl_STALLPROCESS ")
        '        .Append("    SET RESULT_STATUS = :RESULT_STATUS, ")                        ' 16 実績_ステータス
        '        .Append("        RESULT_STALLID = :RESULT_STALLID, ")                      ' 17 実績_ストールID
        '        .Append("        RESULT_START_TIME = :RESULT_START_TIME, ")                ' 18 実績_ストール開始日時時刻
        '        .Append("        RESULT_END_TIME = :RESULT_END_TIME, ")                    ' 19 実績_ストール終了日時時刻
        '        If drProcInfo.RESULT_WORK_TIME >= 0 And CType(drProcInfo.RESULT_STATUS, Integer) > 20 Then     ' 作業時間が0以上 かつ 実績_ステータスが20(作業中)より大きい 時
        '            .Append("        RESULT_WORK_TIME = :RESULT_WORK_TIME, ")                  ' 21 実績_実績時間
        '        Else
        '            .Append("        RESULT_WORK_TIME = NULL, ")                               ' 21 実績_実績時間
        '        End If
        '        .Append("        RESULT_IN_TIME = :RESULT_IN_TIME, ")                      ' 20 実績_入庫時間
        '        .Append("        RESULT_WASH_START = :RESULT_WASH_START, ")                ' 34 洗車開始時刻
        '        .Append("        RESULT_WASH_END = :RESULT_WASH_END, ")                    ' 35 洗車終了時刻
        '        .Append("        RESULT_INSPECTION_START = :RESULT_INSPECTION_START, ")    ' 51 実績検査開始時刻
        '        .Append("        RESULT_INSPECTION_END = :RESULT_INSPECTION_END, ")        ' 52 実績検査終了時刻
        '        .Append("        RESULT_WAIT_START = :RESULT_WAIT_START, ")                ' 36 納車待ち開始時刻
        '        .Append("        RESULT_WAIT_END = :RESULT_WAIT_END, ")                    ' 37 納車待ち終了時刻
        '        If reserveInfo IsNot Nothing Then ' 予約情報が取得できた場合
        '            .Append("        REZ_Reception = :REZ_Reception, ")                        ' 22 予約_受付納車区分
        '            .Append("        REZ_START_TIME = :REZ_START_TIME, ")                      ' 23 予定_ストール開始日時時刻
        '            .Append("        REZ_END_TIME = :REZ_END_TIME, ")                          ' 24 予定_ストール終了日時時刻
        '            .Append("        REZ_WORK_TIME = :REZ_WORK_TIME, ")                        ' 25 予定_作業時間
        '            .Append("        REZ_PICK_DATE = :REZ_PICK_DATE, ")                        ' 26 予約_引取_希望日時時刻
        '            .Append("        REZ_PICK_LOC = :REZ_PICK_LOC, ")                          ' 27 予約_引取_場所
        '            .Append("        REZ_PICK_TIME = :REZ_PICK_TIME, ")                        ' 28 予約_引取_所要時間
        '            .Append("        REZ_DELI_DATE = :REZ_DELI_DATE, ")                        ' 30 予約_納車_希望日時時刻
        '            .Append("        REZ_DELI_LOC = :REZ_DELI_LOC, ")                          ' 31 予約_納車_場所
        '            .Append("        REZ_DELI_TIME = :REZ_DELI_TIME, ")                        ' 32 予約_納車_所要時間
        '            .Append("        RESULT_CARRY_IN = :RESULT_CARRY_IN, ")                    ' 38 預かり日時時刻
        '            .Append("        RESULT_CARRY_OUT = :RESULT_CARRY_OUT, ")                  ' 39 引渡し日時時刻
        '        End If
        '        .Append("        UPDATE_COUNT = UPDATE_COUNT + 1, ")                       ' 40 更新カウント
        '        .Append("        UPDATEDATE = SYSDATE ")                                   ' 47 更新日
        '        .Append("  WHERE DLRCD = :DLRCD ")                                         ' 01 販売店コード
        '        .Append("    AND STRCD = :STRCD ")                                         ' 02 店舗コード
        '        .Append("    AND REZID = :REZID ")                                         ' 03 予約ID
        '        .Append("    AND DSEQNO = :DSEQNO ")                                       ' 04 日跨ぎシーケンス番号
        '        .Append("    AND SEQNO = :SEQNO ")                                         ' 05 シーケンス番号
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_004")

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, drProcInfo.RESULT_STATUS)                        ' 16 実績_ステータス
        '        query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, drProcInfo.RESULT_STALLID)                     ' 17 実績_ストールID
        '        query.AddParameterWithTypeValue("RESULT_START_TIME", OracleDbType.Char, drProcInfo.RESULT_START_TIME)                ' 18 実績_ストール開始日時時刻
        '        query.AddParameterWithTypeValue("RESULT_END_TIME", OracleDbType.Char, drProcInfo.RESULT_END_TIME)                    ' 19 実績_ストール終了日時時刻
        '        If drProcInfo.RESULT_WORK_TIME >= 0 And CType(drProcInfo.RESULT_STATUS, Integer) > 20 Then
        '            query.AddParameterWithTypeValue("RESULT_WORK_TIME", OracleDbType.Int64, drProcInfo.RESULT_WORK_TIME)                 ' 21 実績_実績時間
        '        End If
        '        query.AddParameterWithTypeValue("RESULT_IN_TIME", OracleDbType.Char, drProcInfo.RESULT_IN_TIME)                      ' 20 実績_入庫時間
        '        query.AddParameterWithTypeValue("RESULT_WASH_START", OracleDbType.Char, drProcInfo.RESULT_WASH_START)                ' 34 洗車開始時刻
        '        query.AddParameterWithTypeValue("RESULT_WASH_END", OracleDbType.Char, drProcInfo.RESULT_WASH_END)                    ' 35 洗車終了時刻
        '        query.AddParameterWithTypeValue("RESULT_INSPECTION_START", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_START)    ' 51 実績検査開始時刻
        '        query.AddParameterWithTypeValue("RESULT_INSPECTION_END", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_END)        ' 52 実績検査終了時刻
        '        query.AddParameterWithTypeValue("RESULT_WAIT_START", OracleDbType.Char, drProcInfo.RESULT_WAIT_START)                ' 36 納車待ち開始時刻
        '        query.AddParameterWithTypeValue("RESULT_WAIT_END", OracleDbType.Char, drProcInfo.RESULT_WAIT_END)                    ' 37 納車待ち終了時刻
        '        'If IsNothing(rez) = False Then
        '        If reserveInfo IsNot Nothing Then
        '            query.AddParameterWithTypeValue("REZ_Reception", OracleDbType.Char, drReserveInfo.REZ_RECEPTION)                        ' 22 予約_受付納車区分
        '            query.AddParameterWithTypeValue("REZ_START_TIME", OracleDbType.Char, drReserveInfo.STARTTIME.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))                      ' 23 予定_ストール開始日時時刻
        '            query.AddParameterWithTypeValue("REZ_END_TIME", OracleDbType.Char, drReserveInfo.ENDTIME.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))                          ' 24 予定_ストール終了日時時刻
        '            query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, drReserveInfo.REZ_WORK_TIME)                       ' 25 予定_作業時間
        '            query.AddParameterWithTypeValue("REZ_PICK_DATE", OracleDbType.Char, drReserveInfo.REZ_PICK_DATE)                        ' 26 予約_引取_希望日時時刻
        '            query.AddParameterWithTypeValue("REZ_PICK_LOC", OracleDbType.Char, drReserveInfo.REZ_PICK_LOC)                          ' 27 予約_引取_場所
        '            query.AddParameterWithTypeValue("REZ_PICK_TIME", OracleDbType.Int64, drReserveInfo.REZ_PICK_TIME)                       ' 28 予約_引取_所要時間
        '            query.AddParameterWithTypeValue("REZ_DELI_DATE", OracleDbType.Char, drReserveInfo.REZ_DELI_DATE)                        ' 30 予約_納車_希望日時時刻
        '            query.AddParameterWithTypeValue("REZ_DELI_LOC", OracleDbType.Char, drReserveInfo.REZ_DELI_LOC)                          ' 31 予約_納車_場所
        '            query.AddParameterWithTypeValue("REZ_DELI_TIME", OracleDbType.Int64, drReserveInfo.REZ_DELI_TIME)                       ' 32 予約_納車_所要時間
        '            query.AddParameterWithTypeValue("RESULT_CARRY_IN", OracleDbType.Char, drReserveInfo.REZ_PICK_DATE)                      ' 38 預かり日時時刻
        '            query.AddParameterWithTypeValue("RESULT_CARRY_OUT", OracleDbType.Char, drReserveInfo.REZ_DELI_DATE)                     ' 39 引渡し日時時刻
        '        End If
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drProcInfo.DLRCD)                                        ' 01 販売店コード
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drProcInfo.STRCD)                                        ' 02 店舗コード
        '        query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drProcInfo.REZID)                                       ' 03 予約ID
        '        query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, drProcInfo.DSEQNO)                                     ' 04 日跨ぎシーケンス番号
        '        query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, drProcInfo.SEQNO)                                       ' 05 シーケンス番号

        '        Logger.Info("[E]UpdateStallProcessInfo()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function



        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END


        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START

        ' ''' <summary>
        ' ''' ストール実績情報の登録
        ' ''' </summary>
        ' ''' <param name="procInfo">ストール実績情報</param>
        ' ''' <param name="updateAccount">更新アカウント</param>
        ' ''' <param name="middleFinish">当日処理でのInsertか否か</param>
        ' ''' <param name="relocate"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history> 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 </history>
        'Public Function InsertStallProcessInfo(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
        '                                       ByVal updateAccount As String, _
        '                                       ByVal middleFinish As Boolean, _
        '                                       ByVal relocate As Boolean) As Integer

        '    Logger.Info("[S]InsertStallProcessInfo()")

        '    ' 引数チェック
        '    If procInfo Is Nothing Then
        '        'Argument is nothing
        '        Logger.Error("Argument is nothing [FUNC:InsertStallProcessInfo()]")
        '        Logger.Info("[E]InsertStallProcessInfo()")
        '        Return (-1)
        '    End If

        '    '-----------------
        '    Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
        '    drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
        '    '-----------------

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append("INSERT /* SC3150101_005 */ ")
        '        .Append("  INTO TBL_STALLPROCESS (DLRCD, ")
        '        .Append("                         STRCD, ")
        '        .Append("                         REZID, ")
        '        .Append("                         DSEQNO, ")
        '        .Append("                         SEQNO, ")
        '        .Append("                         ORIGINALID, ")
        '        .Append("                         VIN, ")
        '        .Append("                         SERVICEMSTCD, ")
        '        .Append("                         NAME, ")
        '        .Append("                         MODELCODE, ")
        '        .Append("                         VCLREGNO, ")
        '        .Append("                         SERVICECODE, ")
        '        .Append("                         WASHFLG, ")
        '        .Append("                         INSPECTIONFLG, ")
        '        .Append("                         MILEAGE, ")
        '        .Append("                         RESULT_STATUS, ")
        '        .Append("                         RESULT_STALLID, ")
        '        .Append("                         RESULT_START_TIME, ")
        '        .Append("                         RESULT_END_TIME, ")
        '        .Append("                         RESULT_IN_TIME, ")
        '        .Append("                         RESULT_WORK_TIME, ")
        '        .Append("                         REZ_Reception, ")
        '        .Append("                         REZ_START_TIME, ")
        '        .Append("                         REZ_END_TIME, ")
        '        .Append("                         REZ_WORK_TIME, ")
        '        .Append("                         RESULT_WASH_START, ")
        '        .Append("                         RESULT_WASH_END, ")
        '        .Append("                         RESULT_INSPECTION_START, ")
        '        .Append("                         RESULT_INSPECTION_END, ")
        '        .Append("                         RESULT_WAIT_START, ")
        '        .Append("                         RESULT_WAIT_END, ")
        '        .Append("                         RESULT_CARRY_IN, ")
        '        .Append("                         RESULT_CARRY_OUT, ")
        '        .Append("                         UPDATE_COUNT, ")
        '        .Append("                         MEMO, ")
        '        .Append("                         PREZID, ")
        '        .Append("                         REZ_PICK_DATE, ")
        '        .Append("                         REZ_PICK_LOC, ")
        '        .Append("                         REZ_PICK_TIME, ")
        '        .Append("                         REZ_DELI_DATE, ")
        '        .Append("                         REZ_DELI_LOC, ")
        '        .Append("                         REZ_DELI_TIME, ")
        '        .Append("                         MERCHANDISECD, ")
        '        .Append("                         RSSTATUS, ")
        '        .Append("                         RSDATE, ")
        '        .Append("                         UPDATESERVER, ")
        '        .Append("                         INPUTACCOUNT, ")
        '        .Append("                         CREATEDATE, ")
        '        .Append("                         UPDATEDATE ")
        '        .Append("                        ) ")
        '        If drProcInfo.SEQNO <= 1 Then
        '            .Append("SELECT T1.DLRCD, ")                                     ' 01 販売店コード
        '            .Append("       T1.STRCD, ")                                     ' 02 店舗コード
        '            .Append("       T1.REZID, ")                                     ' 03 予約ID
        '            .Append("       :DSEQNO, ")                                      ' 04 日跨ぎシーケンス番号
        '            .Append("       1, ")                                            ' 05 シーケンス番号
        '            .Append("       NVL(T1.INSDID, ''), ")                           ' 06 連番
        '            .Append("       T1.VIN, ")                                       ' 07 VIN
        '            .Append("       T1.SERVICEMSTCD, ")                              ' 09 サービスマスタコード
        '            .Append("       T1.CUSTOMERNAME, ")                              ' 10 氏名
        '            .Append("       T1.MODELCODE, ")                                 ' 11 モデルコード
        '            .Append("       T1.VCLREGNO, ")                                  ' 12 車両登録No.
        '            .Append("       T1.SERVICECODE_S, ")                             ' 13 サービスコード
        '            .Append("       T1.WASHFLG, ")                                   ' 14 洗車フラグ
        '            .Append("       T1.INSPECTIONFLG, ")                             ' 53 検査フラグ
        '            .Append("       NVL(T1.MILEAGE,0), ")                            ' 15 走行距離
        '            .Append("       :RESULT_STATUS, ")                               ' 16 実績_ステータス
        '            .Append("       T1.STALLID, ")                                   ' 17 実績_ストールID
        '            .Append("       :RESULT_START_TIME, ")                           ' 18 実績_ストール開始日時時刻
        '            .Append("       :RESULT_END_TIME, ")                             ' 19 実績_ストール終了日時時刻
        '            .Append("       :RESULT_IN_TIME, ")                              ' 20 実績_入庫時間
        '            .Append("       0, ")                                            ' 21 実績_実績時間
        '            .Append("       T1.REZ_Reception, ")                             ' 22 予約_受付納車区分
        '            .Append("       TO_CHAR(T1.STARTTIME, 'YYYYMMDDHH24MI'), ")      ' 23 予定_ストール開始日時時刻
        '            .Append("       TO_CHAR(T1.ENDTIME, 'YYYYMMDDHH24MI'), ")        ' 24 予定_ストール終了日時時刻
        '            .Append("       T1.REZ_WORK_TIME, ")                             ' 25 予定_作業時間
        '            .Append("       :RESULT_WASH_START, ")                           ' 34 洗車開始時刻
        '            .Append("       :RESULT_WASH_END, ")                             ' 35 洗車終了時刻
        '            .Append("       :RESULT_INSPECTION_START, ")                     ' 51 実績検査開始時刻
        '            .Append("       :RESULT_INSPECTION_END, ")                       ' 52 実績検査終了時刻
        '            .Append("       :RESULT_WAIT_START, ")                           ' 36 納車待ち開始時刻
        '            .Append("       :RESULT_WAIT_END, ")                             ' 37 納車待ち終了時刻
        '            .Append("       TO_CHAR(T1.CRRYINTIME, 'YYYYMMDDHH24MI'), ")     ' 38 預かり日時時刻
        '            .Append("       TO_CHAR(T1.CRRYOUTTIME, 'YYYYMMDDHH24MI'), ")    ' 39 引渡し日時時刻
        '            If middleFinish Then
        '                .Append("       T1.UPDATE_COUNT, ")                              ' 40 更新カウント
        '            Else
        '                .Append("       T1.UPDATE_COUNT + 1, ")                          ' 40 更新カウント
        '            End If
        '            .Append("       T1.MEMO, ")                                      ' 41 メモ
        '            .Append("       T1.PREZID, ")                                    ' 43 管理予約ID
        '            .Append("       T1.REZ_PICK_DATE, ")                             ' 26 予約_引取_希望日時時刻
        '            .Append("       T1.REZ_PICK_LOC, ")                              ' 27 予約_引取_場所
        '            .Append("       T1.REZ_PICK_TIME, ")                             ' 28 予約_引取_所要時間
        '            .Append("       T1.REZ_DELI_DATE, ")                             ' 30 予約_納車_希望日時時刻
        '            .Append("       T1.REZ_DELI_LOC, ")                              ' 31 予約_納車_場所
        '            .Append("       T1.REZ_DELI_TIME, ")                             ' 32 予約_納車_所要時間
        '            .Append("       T1.MERCHANDISECD, ")                             ' 08 商品コード
        '            .Append("       '99', ")                                         ' 48 送受信完了フラグ
        '            .Append("       SYSDATE, ")                                      ' 49 送受信日時
        '            .Append("       '', ")                                           ' 50 データ発生サーバ
        '            .Append("       :INPUTACCOUNT, ")                                ' 45 入力オペレータ
        '            .Append("       SYSDATE, ")                                      ' 46 作成日
        '            .Append("       SYSDATE ")                                       ' 47 更新日
        '            .Append("  FROM TBL_STALLREZINFO T1 ")
        '            .Append(" WHERE T1.DLRCD = :DLRCD ")
        '            .Append("   AND T1.STRCD = :STRCD ")
        '            .Append("   AND T1.REZID = :REZID ")
        '        Else
        '            .Append("SELECT T1.DLRCD, ")                                     ' 01 販売店コード
        '            .Append("       T1.STRCD, ")                                     ' 02 店舗コード
        '            .Append("       T1.REZID, ")                                     ' 03 予約ID
        '            .Append("       T1.DSEQNO, ")                                    ' 04 日跨ぎシーケンス番号
        '            .Append("       T1.SEQNO + 1, ")                                 ' 05 シーケンス番号
        '            .Append("       T1.ORIGINALID, ")                                ' 06 連番
        '            .Append("       T1.VIN, ")                                       ' 07 VIN
        '            .Append("       T1.SERVICEMSTCD, ")                              ' 09 サービスマスタコード
        '            .Append("       T1.NAME, ")                                      ' 10 氏名
        '            .Append("       T1.MODELCODE, ")                                 ' 11 モデルコード
        '            .Append("       T1.VCLREGNO, ")                                  ' 12 車両登録No.
        '            .Append("       T1.SERVICECODE, ")                               ' 13 サービスコード
        '            .Append("       T1.WASHFLG, ")                                   ' 14 洗車フラグ
        '            .Append("       T1.INSPECTIONFLG, ")                             ' 53 検査フラグ
        '            .Append("       T1.MILEAGE, ")                                   ' 15 走行距離
        '            .Append("       :RESULT_STATUS, ")                               ' 16 実績_ステータス
        '            .Append("       :RESULT_STALLID, ")                              ' 17 実績_ストールID
        '            .Append("       :RESULT_START_TIME, ")                           ' 18 実績_ストール開始日時時刻
        '            .Append("       :RESULT_END_TIME, ")                             ' 19 実績_ストール終了日時時刻
        '            .Append("       :RESULT_IN_TIME, ")                              ' 20 実績_入庫時間
        '            .Append("       0, ")                                            ' 21 実績_実績時間
        '            .Append("       T1.REZ_Reception, ")                             ' 22 予約_受付納車区分
        '            .Append("       :REZ_START_TIME, ")                              ' 23 予定_ストール開始日時時刻
        '            .Append("       :REZ_END_TIME, ")                                ' 24 予定_ストール終了日時時刻
        '            If relocate Then
        '                .Append("       :REZ_WORK_TIME, ")                               ' 25 予定_作業時間
        '            Else
        '                .Append("       T1.REZ_WORK_TIME, ")                             ' 25 予定_作業時間
        '            End If
        '            .Append("       :RESULT_WASH_START, ")                           ' 34 洗車開始時刻
        '            .Append("       :RESULT_WASH_END, ")                             ' 35 洗車終了時刻
        '            .Append("       :RESULT_INSPECTION_START, ")                     ' 51 実績検査開始時刻
        '            .Append("       :RESULT_INSPECTION_END, ")                       ' 52 実績検査終了時刻
        '            .Append("       :RESULT_WAIT_START, ")                           ' 36 納車待ち開始時刻
        '            .Append("       :RESULT_WAIT_END, ")                             ' 37 納車待ち終了自国
        '            .Append("       T1.RESULT_CARRY_IN, ")                           ' 38 預かり日時時刻
        '            .Append("       T1.RESULT_CARRY_OUT, ")                          ' 39 引渡し日時時刻
        '            .Append("       T1.UPDATE_COUNT + 1, ")                          ' 40 更新カウント
        '            .Append("       T1.MEMO, ")                                      ' 41 メモ
        '            .Append("       T1.PREZID, ")                                    ' 43 管理予約ID
        '            .Append("       T1.REZ_PICK_DATE, ")                             ' 26 予約_引取_希望日時時刻
        '            .Append("       T1.REZ_PICK_LOC, ")                              ' 27 予約_引取_場所
        '            .Append("       T1.REZ_PICK_TIME, ")                             ' 28 予約_引取_所要時間
        '            .Append("       T1.REZ_DELI_DATE, ")                             ' 30 予約_納車_希望日時時刻
        '            .Append("       T1.REZ_DELI_LOC, ")                              ' 31 予約_納車_場所
        '            .Append("       T1.REZ_DELI_TIME, ")                             ' 32 予約_納車_所要時間
        '            .Append("       T1.MERCHANDISECD, ")                             ' 08 商品コード
        '            .Append("       '99', ")                                         ' 48 送受信完了フラグ
        '            .Append("       SYSDATE, ")                                      ' 49 送受信日時
        '            .Append("       '', ")                                           ' 50 データ発生サーバ
        '            .Append("       :INPUTACCOUNT, ")                                ' 45 入力オペレータ
        '            .Append("       SYSDATE, ")                                      ' 46 作成日
        '            .Append("       SYSDATE ")                                       ' 47 更新日
        '            .Append("  FROM TBL_STALLPROCESS T1 ")
        '            .Append(" WHERE T1.DLRCD = :DLRCD ")
        '            .Append("   AND T1.STRCD = :STRCD ")
        '            .Append("   AND T1.REZID = :REZID ")
        '            .Append("   AND T1.DSEQNO = :DSEQNO ")
        '            .Append("   AND T1.SEQNO = (SELECT MAX(T2.SEQNO) ")
        '            .Append("                     FROM TBL_STALLPROCESS T2 ")
        '            .Append("                    WHERE T1.DLRCD = T2.DLRCD ")
        '            .Append("                      AND T1.STRCD = T2.STRCD ")
        '            .Append("                      AND T1.REZID = T2.REZID ")
        '            .Append("                      AND T1.DSEQNO = T2.DSEQNO ")
        '            .Append("                   ) ")
        '        End If
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_005")

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        If drProcInfo.SEQNO <= 1 Then
        '            query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, drProcInfo.DSEQNO)                                  ' 04 日跨ぎシーケンス番号
        '            query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, drProcInfo.RESULT_STATUS)                     ' 16 実績_ステータス
        '            query.AddParameterWithTypeValue("RESULT_START_TIME", OracleDbType.Char, drProcInfo.RESULT_START_TIME)             ' 18 実績_ストール開始日時時刻
        '            query.AddParameterWithTypeValue("RESULT_END_TIME", OracleDbType.Char, drProcInfo.RESULT_END_TIME)                 ' 19 実績_ストール終了日時時刻
        '            query.AddParameterWithTypeValue("RESULT_IN_TIME", OracleDbType.Char, drProcInfo.RESULT_IN_TIME)                   ' 20 実績_入庫時間
        '            query.AddParameterWithTypeValue("RESULT_WASH_START", OracleDbType.Char, drProcInfo.RESULT_WASH_START)             ' 34 洗車開始時刻
        '            query.AddParameterWithTypeValue("RESULT_WASH_END", OracleDbType.Char, drProcInfo.RESULT_WASH_END)                 ' 35 洗車終了時刻
        '            query.AddParameterWithTypeValue("RESULT_INSPECTION_START", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_START) ' 51 実績検査開始時刻
        '            query.AddParameterWithTypeValue("RESULT_INSPECTION_END", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_END)     ' 52 実績検査終了時刻
        '            query.AddParameterWithTypeValue("RESULT_WAIT_START", OracleDbType.Char, drProcInfo.RESULT_WAIT_START)             ' 36 納車待ち開始時刻
        '            query.AddParameterWithTypeValue("RESULT_WAIT_END", OracleDbType.Char, drProcInfo.RESULT_WAIT_END)                 ' 37 納車待ち終了時刻
        '            query.AddParameterWithTypeValue("INPUTACCOUNT", OracleDbType.Varchar2, updateAccount)                        ' 45 入力オペレータ
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drProcInfo.DLRCD)                                     ' 01 販売店コード
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drProcInfo.STRCD)                                     ' 02 店舗コード
        '            query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drProcInfo.REZID)                                    ' 03 予約ID
        '        Else
        '            query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, drProcInfo.RESULT_STATUS)                     ' 16 実績_ステータス
        '            'query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, recDr("StallID"))                      ' 17 実績_ストールID
        '            query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, drProcInfo.RESULT_STALLID)
        '            query.AddParameterWithTypeValue("RESULT_START_TIME", OracleDbType.Char, drProcInfo.RESULT_START_TIME)             ' 18 実績_ストール開始日時時刻
        '            query.AddParameterWithTypeValue("RESULT_END_TIME", OracleDbType.Char, drProcInfo.RESULT_END_TIME)                 ' 19 実績_ストール終了日時時刻
        '            query.AddParameterWithTypeValue("RESULT_IN_TIME", OracleDbType.Char, drProcInfo.RESULT_IN_TIME)                   ' 20 実績_入庫時間
        '            'query.AddParameterWithTypeValue("REZ_START_TIME", OracleDbType.Char, recDr("RezStartTime"))                  ' 23 予定_ストール開始日時時刻
        '            query.AddParameterWithTypeValue("REZ_START_TIME", OracleDbType.Char, drProcInfo.REZ_START_TIME)
        '            'query.AddParameterWithTypeValue("REZ_END_TIME", OracleDbType.Char, recDr("RezEndTime"))                      ' 24 予定_ストール終了日時時刻
        '            query.AddParameterWithTypeValue("REZ_END_TIME", OracleDbType.Char, drProcInfo.REZ_END_TIME)
        '            If relocate Then
        '                'query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, recDr("RezWorkTime"))               ' 25 予定作業時間
        '                query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, drProcInfo.REZ_WORK_TIME)
        '            End If
        '            query.AddParameterWithTypeValue("RESULT_WASH_START", OracleDbType.Char, drProcInfo.RESULT_WASH_START)             ' 34 洗車開始時刻
        '            query.AddParameterWithTypeValue("RESULT_WASH_END", OracleDbType.Char, drProcInfo.RESULT_WASH_END)                 ' 35 洗車終了時刻
        '            query.AddParameterWithTypeValue("RESULT_INSPECTION_START", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_START) ' 51 実績検査開始時刻
        '            query.AddParameterWithTypeValue("RESULT_INSPECTION_END", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_END)     ' 52 実績検査終了時刻
        '            query.AddParameterWithTypeValue("RESULT_WAIT_START", OracleDbType.Char, drProcInfo.RESULT_WAIT_START)             ' 36 納車待ち開始時刻
        '            query.AddParameterWithTypeValue("RESULT_WAIT_END", OracleDbType.Char, drProcInfo.RESULT_WAIT_END)                 ' 37 納車待ち終了時刻
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drProcInfo.DLRCD)                                     ' 01 販売店コード
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drProcInfo.STRCD)                                     ' 02 店舗コード
        '            query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drProcInfo.REZID)                                    ' 03 予約ID
        '            query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, drProcInfo.DSEQNO)                                  ' 04 日跨ぎシーケンス番号
        '        End If

        '        Logger.Info("[E]InsertStallProcessInfo()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function

        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START

        ' ''' <summary>
        ' ''' ストール利用ID取得
        ' ''' </summary>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public ReadOnly Property GetSequenceStallUseId() As SC3150101DataSet.SC3150101StallUseIdDataTable

        '    Get
        '        ' DBSelectQueryインスタンス生成
        '        Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallUseIdDataTable)("SC3150101_005_1")

        '            Logger.Info("[S]GetSequenceStallUseId()")

        '            Dim sql_1 As New StringBuilder

        '            ' SQL文の作成
        '            With sql_1
        '                .Append(" SELECT  /*SC3150101_041*/ ")
        '                .Append(" SQ_STALL_USE_ID.NEXTVAL AS STALL_USE_ID FROM DUAL ")
        '            End With

        '            query.CommandText = sql_1.ToString()

        '            Logger.Info("[E]GetSequenceStallUseId()")

        '            'SQL実行
        '            Return query.GetData()

        '        End Using
        '    End Get
        'End Property

        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        ' ''' <summary>
        ' ''' 日跨ぎストール利用登録
        ' ''' </summary>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' '''  <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function InsertStallUseMidFinish(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
        '                                        ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
        '                                        ByVal updateAccount As String, _
        '                                        ByVal stallUseId As SC3150101DataSet.SC3150101StallUseIdDataTable, _
        '                                        ByVal updateDate As Date) As Integer

        '    Logger.Info("[S]InsertStallUseMidFinish()")


        '    ' データセットを展開
        '    '-----------------
        '    'ストール実績データセット
        '    Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
        '    drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)

        '    'ストール予約データセット
        '    Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        '    drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

        '    'ストール利用IDデータセット
        '    Dim drStallUseId As SC3150101DataSet.SC3150101StallUseIdRow
        '    drStallUseId = CType(stallUseId.Rows(0), SC3150101DataSet.SC3150101StallUseIdRow)

        '    '-----------------

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append(" INSERT /* SC3150101_005 */ ")
        '        .Append("   INTO TB_T_STALL_USE( ")
        '        .Append("        STALL_USE_ID ")
        '        .Append("       ,JOB_DTL_ID ")
        '        .Append("       ,DLR_CD ")
        '        .Append("       ,BRN_CD ")
        '        .Append("       ,STALL_ID ")
        '        .Append("       ,TEMP_FLG ")
        '        .Append("       ,PARTS_FLG ")
        '        .Append("       ,STALL_USE_STATUS ")
        '        .Append("       ,SCHE_START_DATETIME ")
        '        .Append("       ,SCHE_END_DATETIME ")
        '        .Append("       ,SCHE_WORKTIME ")
        '        .Append("       ,REST_FLG ")
        '        .Append("       ,RSLT_START_DATETIME ")
        '        .Append("       ,PRMS_END_DATETIME ")
        '        .Append("       ,RSLT_END_DATETIME ")
        '        .Append("       ,RSLT_WORKTIME ")
        '        .Append("       ,JOB_ID ")
        '        .Append("       ,STOP_REASON_TYPE ")
        '        .Append("       ,STOP_MEMO ")
        '        .Append("       ,STALL_IDLE_ID ")
        '        .Append("       ,ROW_CREATE_DATETIME ")
        '        .Append("       ,ROW_CREATE_ACCOUNT ")
        '        .Append("       ,ROW_CREATE_FUNCTION ")
        '        .Append("       ,ROW_UPDATE_DATETIME ")
        '        .Append("       ,ROW_UPDATE_ACCOUNT ")
        '        .Append("       ,ROW_UPDATE_FUNCTION ")
        '        .Append("       ,ROW_LOCK_VERSION ")
        '        .Append("       ,CREATE_DATETIME ")
        '        .Append("       ,CREATE_STF_CD ")
        '        .Append("       ,UPDATE_DATETIME ")
        '        .Append("       ,UPDATE_STF_CD ")
        '        .Append(" )VALUES( ")
        '        .Append("        :STALL_USE_ID ")
        '        .Append("       ,:JOB_DTL_ID ")
        '        .Append("       ,:DLR_CD ")
        '        .Append("       ,:BRN_CD ")
        '        .Append("       ,:STALL_ID ")
        '        .Append("       ,:TEMP_FLG ")
        '        .Append("       ,:PARTS_FLG ")
        '        .Append("       ,:STALL_USE_STATUS ")
        '        .Append("       ,:SCHE_START_DATETIME ")
        '        .Append("       ,:SCHE_END_DATETIME ")
        '        .Append("       ,:SCHE_WORKTIME ")
        '        .Append("       ,:REST_FLG ")
        '        .Append("       ,:MINDATE ")
        '        .Append("       ,:MINDATE ")
        '        .Append("       ,:MINDATE ")
        '        .Append("       ,0 ")
        '        .Append("       ,0 ")
        '        .Append("       ,' ' ")
        '        .Append("       ,' ' ")
        '        .Append("       ,0 ")
        '        .Append("       ,:UPDATEDATE ")
        '        .Append("       ,:ROW_CREATE_ACCOUNT ")
        '        .Append("       ,:ROW_CREATE_FUNCTION ")
        '        .Append("       ,:UPDATEDATE ")
        '        .Append("       ,:ROW_UPDATE_ACCOUNT ")
        '        .Append("       ,:ROW_UPDATE_FUNCTION ")
        '        .Append("       ,:ROW_LOCK_VERSION ")
        '        .Append("       ,:UPDATEDATE ")
        '        .Append("       ,:CREATE_STF_CD ")
        '        .Append("       ,:UPDATEDATE ")
        '        .Append("       ,:UPDATE_STF_CD ")
        '        .Append(" ) ")
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_005")
        '        query.CommandText = sql.ToString()


        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, drStallUseId.STALL_USE_ID)                    ' ストール利用ID
        '        query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, drProcInfo.SEQNO)                               ' 作業内容ID
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, drProcInfo.DLRCD)                               ' 販売店コード
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, drProcInfo.STRCD)                               ' 店舗コード
        '        query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, drProcInfo.RESULT_STALLID)                        ' ストールID
        '        query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, drProcInfo.TEMP_FLG)                              ' 仮置きフラグ
        '        query.AddParameterWithTypeValue("PARTS_FLG", OracleDbType.NVarchar2, drProcInfo.PARTS_FLG)                            ' 部品準備完了フラグ
        '        query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, drProcInfo.STALL_USE_STATUS)             ' ストール利用ステータス
        '        query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, drReserveInfo.STARTTIME)                           ' 予定開始日時
        '        query.AddParameterWithTypeValue("SCHE_END_DATETIME", OracleDbType.Date, drReserveInfo.ENDTIME)                               ' 予定終了日時
        '        query.AddParameterWithTypeValue("SCHE_WORKTIME", OracleDbType.Int64, drReserveInfo.REZ_WORK_TIME)                     ' 予定作業時間
        '        query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, drProcInfo.REST_FLG)                               ' 休憩取得フラグ
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))                                            ' 最小日付
        '        query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, updateAccount)                           ' 行作成アカウント
        '        query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)                         ' 行作成機能
        '        query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, updateAccount)                           ' 行更新アカウント
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)                         ' 行更新機能
        '        query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, DEFAULT_ROW_LOCK_VERSION)                 ' 行ロックバージョン
        '        query.AddParameterWithTypeValue("CREATE_STF_CD", OracleDbType.NVarchar2, updateAccount)                                ' 作成スタッフコード
        '        query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, updateAccount)                                ' 更新スタッフコード
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)                                       ' 更新日時
        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

        ' ''' <summary>
        ' ''' ストール予約履歴の登録
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="reserveId">予約ID</param>
        ' ''' <param name="insertType"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function InsertReserveHistory(ByVal dealerCode As String, _
        '                                     ByVal branchCode As String, _
        '                                     ByVal reserveId As Long, _
        '                                     ByVal insertType As Integer) As Integer 'InsertRezHistory

        '    Logger.Info("[S]InsertReserveHistory()")

        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        .Append("INSERT /* SC3150101_006 */ ")
        '        .Append("  INTO TBL_STALLREZHIS ( ")
        '        .Append("       DLRCD, ")
        '        .Append("       STRCD, ")
        '        .Append("       REZID, ")
        '        .Append("       SEQNO, ")
        '        .Append("       UPDDVSID, ")
        '        .Append("       STALLID, ")
        '        .Append("       STARTTIME, ")
        '        .Append("       ENDTIME, ")
        '        .Append("       CUSTCD, ")
        '        .Append("       PERMITID, ")
        '        .Append("       CUSTOMERNAME, ")
        '        .Append("       TELNO, ")
        '        .Append("       MOBILE, ")
        '        .Append("       EMAIL1, ")
        '        .Append("       VEHICLENAME, ")
        '        .Append("       VCLREGNO, ")
        '        .Append("       SERVICECODE, ")
        '        .Append("       SERVICECODE_S, ")
        '        .Append("       REZDATE, ")
        '        .Append("       NETREZID, ")
        '        .Append("       STATUS, ")
        '        .Append("       INSDID, ")
        '        .Append("       VIN, ")
        '        .Append("       CUSTOMERFLAG, ")
        '        .Append("       CUSTVCLRE_SEQNO, ")
        '        .Append("       SERVICEMSTCD, ")
        '        .Append("       ZIPCODE, ")
        '        .Append("       ADDRESS, ")
        '        .Append("       MODELCODE, ")
        '        .Append("       MILEAGE, ")
        '        .Append("       WASHFLG, ")
        '        .Append("       INSPECTIONFLG, ")
        '        .Append("       WALKIN, ")
        '        .Append("       REZ_RECEPTION, ")
        '        .Append("       REZ_WORK_TIME, ")
        '        .Append("       REZ_PICK_DATE, ")
        '        .Append("       REZ_PICK_LOC, ")
        '        .Append("       REZ_PICK_TIME, ")
        '        .Append("       REZ_DELI_DATE, ")
        '        .Append("       REZ_DELI_LOC, ")
        '        .Append("       REZ_DELI_TIME, ")
        '        .Append("       UPDATE_COUNT, ")
        '        .Append("       STOPFLG, ")
        '        .Append("       PREZID, ")
        '        .Append("       REZCHILDNO, ")
        '        .Append("       ACTUAL_STIME, ")
        '        .Append("       ACTUAL_ETIME, ")
        '        .Append("       CRRY_TYPE, ")
        '        .Append("       CRRYINTIME, ")
        '        .Append("       CRRYOUTTIME, ")
        '        .Append("       MEMO, ")
        '        .Append("       STRDATE, ")
        '        .Append("       NETDEVICESFLG, ")
        '        .Append("       INPUTACCOUNT, ")
        '        .Append("       INFOUPDATEDATE, ")
        '        .Append("       INFOUPDATEACCOUNT, ")
        '        .Append("       CREATEDATE, ")
        '        .Append("       UPDATEDATE, ")
        '        .Append("       HIS_FLG, ")
        '        .Append("       MERCHANDISECD, ")
        '        .Append("       BASREZID, ")
        '        .Append("       ACCOUNT_PLAN, ")
        '        .Append("       RSSTATUS, ")
        '        .Append("       RSDATE, ")
        '        .Append("       UPDATESERVER, ")
        '        .Append("       REZTYPE, ")
        '        .Append("       CRCUSTID, ")
        '        .Append("       CUSTOMERCLASS, ")
        '        .Append("       STALLWAIT_REZID, ")
        '        .Append("       ORDERNO, ")
        '        ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        '        .Append("       INSTRUCT, ")
        '        .Append("       WORKSEQ, ")
        '        .Append("       MERCHANDISEFLAG")
        '        ' 2012/06/01 KN 西田 STEP1 重要課題対応 END
        '        .Append("       ) ")
        '        .Append("SELECT T1.DLRCD, ")
        '        .Append("       T1.STRCD, ")
        '        .Append("       T1.REZID, ")
        '        If insertType = 0 Then
        '            .Append("       1, ")
        '        Else
        '            .Append("       T4.NEXT_SEQNO,")
        '        End If
        '        If insertType = 2 Then
        '            .Append("       '1', ")
        '        Else
        '            .Append("       '0', ")
        '        End If
        '        .Append("       T1.STALLID, ")
        '        .Append("       T1.STARTTIME, ")
        '        .Append("       T1.ENDTIME, ")
        '        .Append("       T1.CUSTCD, ")
        '        .Append("       T1.PERMITID, ")
        '        .Append("       T1.CUSTOMERNAME, ")
        '        .Append("       T1.TELNO, ")
        '        .Append("       T1.MOBILE, ")
        '        .Append("       T1.EMAIL1, ")
        '        .Append("       T1.VEHICLENAME, ")
        '        .Append("       T1.VCLREGNO, ")
        '        .Append("       T1.SERVICECODE, ")
        '        .Append("       T1.SERVICECODE_S, ")
        '        .Append("       T1.REZDATE, ")
        '        .Append("       T1.NETREZID, ")
        '        .Append("       T1.STATUS, ")
        '        .Append("       T1.INSDID, ")
        '        .Append("       T1.VIN, ")
        '        .Append("       T1.CUSTOMERFLAG, ")
        '        .Append("       T1.CUSTVCLRE_SEQNO, ")
        '        .Append("       T1.SERVICEMSTCD, ")
        '        .Append("       T1.ZIPCODE, ")
        '        .Append("       T1.ADDRESS, ")
        '        .Append("       T1.MODELCODE, ")
        '        .Append("       T1.MILEAGE, ")
        '        .Append("       T1.WASHFLG, ")
        '        .Append("       T1.INSPECTIONFLG, ")
        '        .Append("       T1.WALKIN, ")
        '        .Append("       T1.REZ_Reception, ")
        '        .Append("       T1.REZ_WORK_TIME, ")
        '        .Append("       T1.REZ_PICK_DATE, ")
        '        .Append("       T1.REZ_PICK_LOC, ")
        '        .Append("       T1.REZ_PICK_TIME, ")
        '        .Append("       T1.REZ_DELI_DATE, ")
        '        .Append("       T1.REZ_DELI_LOC, ")
        '        .Append("       T1.REZ_DELI_TIME, ")
        '        .Append("       T1.UPDATE_COUNT, ")
        '        .Append("       T1.STOPFLG, ")
        '        .Append("       T1.PREZID, ")
        '        .Append("       T1.REZCHILDNO, ")
        '        .Append("       T1.ACTUAL_STIME, ")
        '        .Append("       T1.ACTUAL_ETIME, ")
        '        .Append("       T1.CRRY_TYPE, ")
        '        .Append("       T1.CRRYINTIME, ")
        '        .Append("       T1.CRRYOUTTIME, ")
        '        .Append("       T1.MEMO, ")
        '        .Append("       T1.STRDATE, ")
        '        .Append("       T1.NETDEVICESFLG, ")
        '        .Append("       T1.INPUTACCOUNT, ")
        '        .Append("       T1.UPDATEDATE, ")
        '        .Append("       T1.UPDATEACCOUNT, ")
        '        .Append("       SYSDATE, ")
        '        .Append("       SYSDATE, ")
        '        If insertType = 0 Then
        '            .Append("       '0', ")
        '        ElseIf insertType = 2 Then
        '            .Append("       '2', ")
        '        Else
        '            .Append("       '1', ")
        '        End If
        '        .Append("       T1.MERCHANDISECD, ")
        '        .Append("       T1.BASREZID, ")
        '        .Append("       T1.ACCOUNT_PLAN, ")
        '        .Append("       '99', ")
        '        .Append("       SYSDATE, ")
        '        .Append("       '', ")
        '        .Append("       T1.REZTYPE, ")
        '        .Append("       T1.CRCUSTID, ")
        '        .Append("       T1.CUSTOMERCLASS, ")
        '        .Append("       T1.STALLWAIT_REZID, ")
        '        .Append("       T1.ORDERNO, ")
        '        ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        '        .Append("       T1.INSTRUCT, ")
        '        .Append("       T1.WORKSEQ, ")
        '        .Append("       T1.MERCHANDISEFLAG")
        '        ' 2012/06/01 KN 西田 STEP1 重要課題対応 END
        '        .Append("  FROM TBL_STALLREZINFO T1 ")
        '        If insertType <> 0 Then
        '            .Append("       ,(SELECT NVL(MAX(T2.SEQNO) + 1, 1)  AS NEXT_SEQNO ")
        '            .Append("          FROM TBL_STALLREZHIS  T2 ")
        '            .Append("         WHERE T2.DLRCD = :DLRCD1 ")
        '            .Append("           AND T2.STRCD = :STRCD1 ")
        '            If insertType <> 3 Then
        '                .Append("       AND T2.REZID = :REZID1 ")
        '            Else
        '                .Append("       AND T2.REZID <> :REZID3 ")
        '            End If
        '            .Append("        ) T4 ")
        '        End If
        '        .Append(" WHERE T1.DLRCD = :DLRCD1 ")
        '        .Append("   AND T1.STRCD = :STRCD1 ")
        '        If insertType <> 3 Then
        '            .Append("   AND T1.REZID = :REZID1 ")
        '        Else
        '            .Append("   AND T1.PREZID = ( ")
        '            .Append("                 SELECT T3.PREZID ")
        '            .Append("                   FROM TBL_STALLREZINFO T3 ")
        '            .Append("                  WHERE T3.DLRCD = :DLRCD2 ")
        '            .Append("                    AND T3.STRCD = :STRCD2 ")
        '            .Append("                    AND T3.REZID = :REZID2 ")
        '            .Append("                ) ")
        '            .Append("   AND T1.REZID <> :REZID3 ")
        '            .Append("   AND T1.CANCELFLG = '0' ")
        '        End If

        '    End With
        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_006")

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("DLRCD1", OracleDbType.Char, dealerCode)
        '        query.AddParameterWithTypeValue("STRCD1", OracleDbType.Char, branchCode)
        '        If insertType <> 3 Then
        '            query.AddParameterWithTypeValue("REZID1", OracleDbType.Int64, reserveId)
        '        Else
        '            query.AddParameterWithTypeValue("DLRCD2", OracleDbType.Char, dealerCode)
        '            query.AddParameterWithTypeValue("STRCD2", OracleDbType.Char, branchCode)
        '            query.AddParameterWithTypeValue("REZID2", OracleDbType.Int64, reserveId)
        '            query.AddParameterWithTypeValue("REZID3", OracleDbType.Int64, reserveId)
        '        End If

        '        Logger.Info("[E]InsertReserveHistory()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function

        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        ''' <summary>
        ''' ストール時間情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetStallTimeInfo(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal stallId As Decimal) As SC3150101DataSet.SC3150101StallTimeInfoDataTable

            Logger.Info("[S]GetStallTimeInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallTimeInfoDataTable)("SC3150101_007")
                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("    SELECT /* SC3150101_007 */ ")
                    .Append("           T1.DLRCD AS DLRCD, ")                ' 販売店コード
                    .Append("           T1.STRCD AS STRCD, ")                ' 店舗コード
                    .Append("           T1.STALLID AS STALLID, ")            ' ストールID
                    .Append("           T1.STALLNAME AS STALLNAME, ")        ' ストール名称
                    .Append("           T1.STALLNAME_S AS STALLNAME_S, ")    ' ストール省略名称
                    .Append("           T2.STARTTIME AS STARTTIME, ")        ' 開始時間
                    .Append("           T2.ENDTIME AS ENDTIME, ")            ' 終了時間
                    .Append("           T2.TIMEINTERVAL AS TIMEINTERVAL, ")  ' 時間間隔
                    .Append("           T2.PSTARTTIME AS PSTARTTIME, ")      ' プログレス開始時間
                    .Append("           T2.PENDTIME AS PENDTIME ")           ' プログレス終了時間
                    .Append("      FROM TBL_STALL T1, ")                     ' [ストールマスタ]
                    .Append("           TBL_STALLTIME T2 ")                  ' [ストール時間]
                    .Append("     WHERE T1.DLRCD = T2.DLRCD ")               ' 販売店コード
                    .Append("       AND T1.STRCD = T2.STRCD ")               ' 店舗コード
                    .Append("       AND T1.DLRCD = :DLRCD ")                 ' 販売店コード
                    .Append("       AND T1.STRCD = :STRCD ")                 ' 店舗コード
                    .Append("       AND T1.STALLID = :STALLID")              ' ストールID
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, stallId)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                Logger.Info("[E]GetStallTimeInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 指定範囲内のストール予約情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="fromDate">範囲時間(FROM)</param>
        ''' <param name="toDate">範囲時間(TO)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetStallReserveList(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallId As Decimal, _
                                            ByVal reserveId As Decimal, _
                                            ByVal fromDate As Date, _
                                            ByVal toDate As Date) As SC3150101DataSet.SC3150101StallReserveListDataTable

            Logger.Info("[S]GetStallReserveList()")

            ' DBSelectQueryインスタンス生成


            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                '.Append(" SELECT /* SC3150101_008 */ ")
                '.Append("        DLRCD, ")                ' 01 販売店コード
                '.Append("        STRCD, ")                ' 02 店舗コード
                '.Append("        REZID, ")                ' 03 予約ID
                '.Append("        STALLID, ")              ' 05 ストールID
                '.Append("        STARTTIME, ")            ' 06 使用開始日時
                '.Append("        ENDTIME, ")              ' 07 使用終了日時
                '.Append("        STATUS, ")               ' 19 ステータス
                '.Append("        WASHFLG, ")              ' 30 洗車フラグ
                '.Append("        REZ_RECEPTION, ")        ' 33 予約_受付納車区分
                '.Append("        REZ_WORK_TIME, ")        ' 34 予定_作業時間
                '.Append("        REZ_PICK_DATE, ")        ' 35 予約_引取_希望日時時刻
                '.Append("        REZ_DELI_DATE, ")        ' 39 予約_納車_希望日時時刻
                '.Append("        STOPFLG, ")              ' 44 中断フラグ
                '.Append("        STRDATE, ")              ' 54 入庫時間
                '.Append("        CANCELFLG, ")            ' 58 キャンセルフラグ
                '.Append("        INSPECTIONFLG ")         ' 67 検査フラグ
                '.Append("   FROM TBL_STALLREZINFO ")      ' [ストール予約]
                '.Append("  WHERE DLRCD = :DLRCD ")        ' 01 販売店コード
                '.Append("    AND STRCD = :STRCD ")        ' 02 店舗コード
                '.Append("    AND STALLID = :STALLID ")
                '.Append("    AND ( ")
                '.Append("         ( ")
                ''.Append("          STARTTIME < :STARTTIME ")
                ''.Append("      AND ENDTIME > :ENDTIME ")
                '.Append("          STARTTIME < TO_DATE(:STARTTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                '.Append("      AND ENDTIME > TO_DATE(:ENDTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                '.Append("          ) ")
                '.Append("       OR REZID = :REZID ")
                '.Append("         ) ")
                '.Append("    AND STATUS < 3 ")
                '.Append("    AND ( ")
                '.Append("         CANCELFLG = '0' ")
                '.Append("     OR ( ")
                '.Append("         CANCELFLG = '1' ")
                '.Append("     AND STOPFLG = '1' ")
                '.Append("         ) ")
                '.Append("        )")

                .Append(" SELECT /* SC3150101_008 */ ")
                .Append("        TRIM(T1.DLR_CD) AS  DLRCD ")                                                                                     ' 販売店コード
                .Append("      , TRIM(T1.BRN_CD) AS STRCD ")                                                                                      ' 店舗コード
                .Append("      , T1.STALL_USE_ID AS REZID ")                                                                                        ' 作業内容ID
                .Append("      , T1.STALL_ID AS STALLID ")                                                                                         ' ストールID
                .Append("      , DECODE(T1.SCHE_START_DATETIME,:MINDATE,TO_DATE(NULL),T1.SCHE_START_DATETIME) AS STARTTIME ")                               ' 予約開始日時
                .Append("      , DECODE(T1.SCHE_END_DATETIME,:MINDATE,TO_DATE(NULL),T1.SCHE_END_DATETIME) AS ENDTIME ")                                                                                ' 予約終了日時
                .Append("      , TRIM(T3.RESV_STATUS) AS STATUS ")                                                                                 ' ステータス
                .Append("      , NVL(TRIM(T3.CARWASH_NEED_FLG), :CARWASH_NEED_FLG_0) AS WASHFLG ")                                                                 ' 洗車必要フラグ
                .Append("      , TRIM(T3.PICK_DELI_TYPE) AS REZ_RECEPTION ")                                                                       ' 引取納車区分
                .Append("      , T1.SCHE_WORKTIME AS REZ_WORK_TIME ")                                                                              ' 予定作業時間
                .Append("      , DECODE(T3.SCHE_SVCIN_DATETIME,:MINDATE,NULL,TO_CHAR(T3.SCHE_SVCIN_DATETIME,'YYYYMMDDHH24MI')) AS REZ_PICK_DATE ") ' 予定入庫日時
                .Append("      , DECODE(T3.SCHE_DELI_DATETIME,:MINDATE,NULL,TO_CHAR(T3.SCHE_DELI_DATETIME,'YYYYMMDDHH24MI')) AS REZ_DELI_DATE ")                   ' 予定納車日時
                .Append("      , DECODE(T1.STALL_USE_STATUS,:SUS05,:STOPFLG_1,:STOPFLG_0) AS STOPFLG")                                                             ' 中断フラグ
                .Append("      , DECODE(T3.RSLT_SVCIN_DATETIME,:MINDATE,:STRMINDATE,T3.RSLT_SVCIN_DATETIME) AS STRDATE ")                                   ' 実績納車日時
                .Append("      , T2.CANCEL_FLG AS CANCELFLG")                                                                                      ' キャンセルフラグ
                .Append("      , NVL(TRIM(T2.INSPECTION_NEED_FLG), :INSPECTION_NEED_FLG_0) AS INSPECTIONFLG ")                                                        ' 検査必要フラグ
                .Append("      , T3.SVC_STATUS AS RESULT_STATUS ")
                .Append("      , T1.STALL_USE_STATUS ")
                .Append("   FROM TB_T_STALL_USE T1 ")
                .Append("      , TB_T_JOB_DTL T2 ")
                .Append("      , TB_T_SERVICEIN T3 ")
                .Append("  WHERE T1.DLR_CD = T2.DLR_CD ")
                .Append("    AND T1.BRN_CD = T2.BRN_CD ")
                .Append("    AND T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .Append("    AND T2.DLR_CD = T3.DLR_CD ")
                .Append("    AND T2.BRN_CD = T3.BRN_CD ")
                .Append("    AND T2.SVCIN_ID = T3.SVCIN_ID ")
                .Append("    AND T1.DLR_CD = :DLRCD")
                .Append("    AND T1.BRN_CD = :STRCD ")
                .Append("    AND T1.STALL_ID = :STALLID ")
                .Append("    AND  ( (T1.SCHE_START_DATETIME < :STARTTIME ")
                .Append("            AND T1.SCHE_END_DATETIME > :ENDTIME) ")
                .Append("             OR T1.JOB_DTL_ID = :REZID ) ")
                '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                '.Append("    AND T1.STALL_USE_STATUS IN (:SUS00,:SUS01,:SUS02) ")
                .Append("    AND T1.STALL_USE_STATUS IN (:SUS00,:SUS01,:SUS02,:SUS04) ")
                '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                .Append("    AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
            End With

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallReserveListDataTable)("SC3150101_008")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                ''query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, CType(toDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                ''query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, CType(fromDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                'query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Dim workTimeFromString As String = fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())
                Dim workTimeToString As String = toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, Date.Parse(workTimeToString, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, Date.Parse(workTimeFromString, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, reserveId)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
                query.AddParameterWithTypeValue("SUS00", OracleDbType.NVarchar2, stallUseStetus00)
                query.AddParameterWithTypeValue("SUS01", OracleDbType.NVarchar2, stallUseStetus01)
                query.AddParameterWithTypeValue("SUS02", OracleDbType.NVarchar2, stallUseStetus02)
                query.AddParameterWithTypeValue("SUS05", OracleDbType.NVarchar2, stallUseStetus05)
                query.AddParameterWithTypeValue("STOPFLG_0", OracleDbType.NVarchar2, STOPFLG_0)
                query.AddParameterWithTypeValue("STOPFLG_1", OracleDbType.NVarchar2, STOPFLG_1)
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG_0", OracleDbType.NVarchar2, CARWASH_NEED_FLG_0)
                query.AddParameterWithTypeValue("INSPECTION_NEED_FLG_0", OracleDbType.NVarchar2, INSPECTION_NEED_FLG_0)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("STRMINDATE", OracleDbType.Date, Date.Parse(strMinDate, CultureInfo.InvariantCulture))
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                query.AddParameterWithTypeValue("SUS04", OracleDbType.NVarchar2, stallUseStetus04)
                '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

                Logger.Info("[E]GetStallReserveList()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 指定範囲内のストール実績情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="fromDate">範囲時間(FROM)</param>
        ''' <param name="toDate">範囲時間(TO)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetStallProcessList(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallId As Decimal, _
                                            ByVal fromDate As Date, _
                                            ByVal toDate As Date) As SC3150101DataSet.SC3150101StallProcessListDataTable

            Logger.Info("[S]GetStallProcessList()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallProcessListDataTable)("SC3150101_009")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("    SELECT /* SC3150101_009 */ ")
                    '.Append("           T1.DLRCD AS DLRCD, ")                                        ' 01 販売店コード
                    '.Append("           T1.STRCD AS STRCD, ")                                        ' 02 店舗コード
                    '.Append("           T1.REZID AS REZID, ")                                        ' 03 予約ID
                    '.Append("           NVL(T2.DSEQNO, 0) AS DSEQNO, ")                              ' 04 日跨ぎシーケンス番号
                    '.Append("           NVL(T2.SEQNO, 0) AS SEQNO, ")                                ' 05 シーケンス番号
                    '.Append("           T2.RESULT_STATUS AS RESULT_STATUS, ")                        ' 16 実績_ステータス
                    '.Append("           NVL(T2.RESULT_STALLID, 0) AS RESULT_STALLID, ")              ' 17 実績_ストールID
                    '.Append("           T2.RESULT_START_TIME AS RESULT_START_TIME, ")                ' 18 実績_ストール開始日時時刻
                    '.Append("           T2.RESULT_END_TIME AS RESULT_END_TIME, ")                    ' 19 実績_ストール終了日時時刻
                    '.Append("           NVL(T2.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME, ")          ' 21 実績_実績時間
                    '.Append("           T2.RESULT_IN_TIME AS RESULT_IN_TIME, ")                      ' 20 実績_入庫時間
                    '.Append("           T2.RESULT_WASH_START AS RESULT_WASH_START, ")                ' 34 洗車開始時刻
                    '.Append("           T2.RESULT_WASH_END AS RESULT_WASH_END, ")                    ' 35 洗車終了時刻
                    '.Append("           T2.RESULT_WAIT_START AS RESULT_WAIT_START, ")                ' 36 納車待ち開始時刻
                    '.Append("           T2.RESULT_WAIT_END AS RESULT_WAIT_END, ")                    ' 37 納車待ち終了時刻
                    '.Append("           T2.RESULT_INSPECTION_START AS RESULT_INSPECTION_START, ")    ' 51 実績検査開始時刻
                    '.Append("           T2.RESULT_INSPECTION_END AS RESULT_INSPECTION_END ")         ' 52 実績検査終了時刻
                    '.Append("      FROM TBL_STALLREZINFO T1, ")                                      ' [ストール予約]
                    '.Append("           TBL_STALLPROCESS T2 ")                                       ' [ストール実績]
                    '.Append("     WHERE T1.DLRCD = T2.DLRCD (+) ")                                       ' 01 販売店コード
                    '.Append("       AND T1.STRCD = T2.STRCD (+) ")                                       ' 02 店舗コード
                    '.Append("       AND T1.REZID = T2.REZID (+) ")                                       ' 03 予約ID
                    '.Append("       AND T1.DLRCD = :DLRCD ")                                         ' 01 販売店コード
                    '.Append("       AND T1.STRCD = :STRCD ")                                         ' 02 店舗コード
                    '.Append("       AND T1.STALLID = :STALLID ")                                     ' 05 ストールID
                    ''.Append("       AND T1.STARTTIME < :STARTTIME ")                                 ' 06 使用開始日時
                    ''.Append("       AND T1.ENDTIME > :ENDTIME ")                                     ' 07 使用終了日時
                    '.Append("       AND T1.STARTTIME < TO_DATE(:STARTTIME, 'YYYY/MM/DD HH24:MI:SS') ") ' 06 使用開始日時
                    '.Append("       AND T1.ENDTIME > TO_DATE(:ENDTIME, 'YYYY/MM/DD HH24:MI:SS') ")     ' 07 使用終了日時
                    '.Append("       AND T1.STATUS < 3 ")                                             ' 19 ステータス
                    '' 2012/05/24 KN 西田 TCメイン 号口不具合対応 作業が開始出来なかった START
                    '.Append("       AND (")
                    '.Append("           T1.CANCELFLG = '0' ")
                    '.Append("           OR (")
                    '.Append("               T1.CANCELFLG = '1' ")
                    '.Append("           AND T1.STOPFLG = '1' ")
                    '.Append("              )")
                    '.Append("           )")
                    ''.Append("       AND T1.CANCELFLG = '0' ")                                        ' 58 キャンセルフラグ
                    '' 2012/05/24 KN 西田 TCメイン 号口不具合対応 作業が開始出来なかった END
                    '.Append("       AND (T2.SEQNO IS NULL ")                                         ' 05 シーケンス番号
                    '.Append("           OR (T2.DSEQNO = (SELECT MAX(T3.DSEQNO) ")                    ' 04 日跨ぎシーケンス番号
                    '.Append("                              FROM TBL_STALLPROCESS T3 ")               ' [ストール実績]
                    '.Append("                             WHERE T3.DLRCD = T2.DLRCD ")               ' 01 販売店コード
                    '.Append("                               AND T3.STRCD = T2.STRCD ")               ' 02 店舗コード
                    '.Append("                               AND T3.REZID = T2.REZID ")               ' 03 予約ID
                    '.Append("                          GROUP BY T3.DLRCD, T3.STRCD, T3.REZID) ")
                    '.Append("          AND T2.SEQNO = (SELECT MAX(T4.SEQNO) ")                       ' 05 シーケンス番号
                    '.Append("                            FROM TBL_STALLPROCESS T4 ")                 ' [ストール実績]
                    '.Append("                           WHERE T4.DLRCD = T2.DLRCD ")                 ' 01 販売店コード
                    '.Append("                             AND T4.STRCD = T2.STRCD ")                 ' 02 店舗コード
                    '.Append("                             AND T4.REZID = T2.REZID ")                 ' 03 予約ID
                    '.Append("                             AND T4.DSEQNO = T2.DSEQNO) ")              ' 04 日跨ぎシーケンス番号
                    '.Append("              ) ")
                    '.Append("           )")

                    .Append("      SELECT  /* SC3150101_009 */ ")
                    .Append("              TRIM(T1.DLR_CD) AS  DLRCD ") ' 販売店コード
                    .Append("             ,TRIM(T1.BRN_CD) AS STRCD ")  ' 店舗コード
                    .Append("             ,T1.STALL_ID AS RESULT_STALLID ") 'ストールID
                    .Append("             ,0 AS DSEQNO ")                       ' 日跨ぎシーケンス番号
                    .Append("             ,T2.JOB_DTL_ID  AS SEQNO ")           'シーケンス番号
                    .Append("             ,DECODE(T1.RSLT_START_DATETIME,:MINDATE, NULL, TO_CHAR(T1.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_START_TIME ") '実績開始日時
                    .Append("             ,DECODE(T1.PRMS_END_DATETIME,:MINDATE, NULL, TO_CHAR(T1.PRMS_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_END_TIME ") ' 19 実績_ストール終了日時時刻
                    .Append("             ,T1.RSLT_WORKTIME AS RESULT_WORK_TIME ") ' 21 実績_実績時間
                    .Append("             ,T1.STALL_USE_ID AS REZID ") ' 03 予約ID
                    .Append("             ,TRIM(T3.SVC_STATUS) AS RESULT_STATUS ") ' 16 実績_ステータス
                    .Append("             ,DECODE(T3.RSLT_SVCIN_DATETIME,:MINDATE, NULL, TO_CHAR(T3.RSLT_SVCIN_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_IN_TIME ") ' 20 実績_入庫時間
                    .Append("             ,DECODE(T4.RSLT_START_DATETIME,:MINDATE, NULL, TO_CHAR(T4.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WASH_START ") ' 34 洗車開始時刻
                    .Append("             ,DECODE(T4.RSLT_END_DATETIME, :MINDATE, NULL, TO_CHAR(T4.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WASH_END ") ' 35 洗車終了時刻
                    .Append("             ,DECODE(T3.RSLT_DELI_DATETIME,:MINDATE, NULL,TO_CHAR(T3.RSLT_DELI_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WAIT_END ") '  37 納車待ち終了時刻
                    .Append("             ,DECODE(T5.RSLT_START_DATETIME, :MINDATE, NULL, TO_CHAR(T5.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_INSPECTION_START ") ' 51 実績検査開始時刻
                    .Append("             ,DECODE(T5.RSLT_END_DATETIME, :MINDATE, NULL, TO_CHAR(T5.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_INSPECTION_END ") ' 52 実績検査終了時刻
                    .Append("        FROM  TB_T_STALL_USE T1 ")
                    .Append("             ,TB_T_JOB_DTL T2 ")
                    .Append("             ,TB_T_SERVICEIN T3 ")
                    .Append("             ,TB_T_CARWASH_RESULT T4 ")
                    .Append("             ,TB_T_INSPECTION_RESULT T5 ")
                    .Append("       WHERE  T1.DLR_CD = T2.DLR_CD ")
                    .Append("         AND  T1.BRN_CD = T2.BRN_CD ")
                    .Append("         AND  T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                    .Append("         AND  T2.DLR_CD = T3.DLR_CD ")
                    .Append("         AND  T2.BRN_CD = T3.BRN_CD ")
                    .Append("         AND  T2.SVCIN_ID = T3.SVCIN_ID ")
                    .Append("         AND  T3.SVCIN_ID = T4.SVCIN_ID (+) ")
                    .Append("         AND  T2.JOB_DTL_ID = T5.JOB_DTL_ID (+) ")
                    .Append("         AND  T1.DLR_CD = :DLRCD ")
                    .Append("         AND  T1.BRN_CD = :STRCD ")
                    .Append("         AND  T1.STALL_ID = :STALLID ")
                    .Append("         AND  ( (T1.SCHE_START_DATETIME < :STARTTIME ")
                    .Append("              AND T1.SCHE_END_DATETIME > :ENDTIME)) ")
                    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                    '.Append("         AND T1.STALL_USE_STATUS = :SUS02 ")
                    .Append("         AND T1.STALL_USE_STATUS IN (:SUS02 ,:SUS04) ")
                    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                    .Append("         AND T1.STALL_USE_STATUS = :SUS02 ")
                    .Append("         AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                    .Append("    ORDER BY T1.RSLT_START_DATETIME ASC ")
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                ''query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, CType(toDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                ''query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, CType(fromDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                'query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))

                Dim workTimeFromString As String = fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())
                Dim workTimeToString As String = toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, Date.Parse(workTimeToString, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, Date.Parse(workTimeFromString, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
                query.AddParameterWithTypeValue("SUS02", OracleDbType.NVarchar2, stallUseStetus02)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                query.AddParameterWithTypeValue("SUS04", OracleDbType.NVarchar2, stallUseStetus04)
                '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

                Logger.Info("[E]GetStallProcessList()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 担当者実績情報の取得
        ''' </summary>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="workDate">作業日付</param>
        ''' <param name="account">更新者アカウント</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetStaffResultInfo(ByVal reserveId As Decimal, _
                                           ByVal workDate As DateTime, _
                                           ByVal account As String) As SC3150101DataSet.SC3150101StaffResultInfoDataTable

            Logger.Info("[S]GetStaffResultInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StaffResultInfoDataTable)("SC3150101_010")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("    SELECT /* SC3150101_010 */ ")
                    '.Append("           T2.DSEQNO AS DSEQNO, ")
                    '.Append("           T2.SEQNO AS SEQNO, ")
                    '.Append("           T2.RESULT_STATUS AS RESULT_STATUS, ")
                    '.Append("           T2.RESULT_END_TIME AS RESULT_END_TIME ")
                    '.Append("      FROM TBL_TSTAFFSTALL T1, ")
                    '.Append("           TBL_STALLPROCESS T2 ")
                    '.Append("     WHERE T1.DLRCD = T2.DLRCD ")
                    '.Append("       AND T1.STRCD = T2.STRCD ")
                    '.Append("       AND T1.REZID = T2.REZID ")
                    '.Append("       AND T1.STALLID = :STALLID ")
                    '.Append("       AND T1.REZID = :REZID ")
                    ''.Append("       AND T1.WORKDATE = :WORKDATE ")
                    '.Append("       AND T1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")
                    'If midFinish Then
                    '    .Append("       AND T2.DSEQNO = (SELECT MAX(T3.DSEQNO) - 1 ")
                    'Else
                    '    .Append("       AND T2.DSEQNO = (SELECT MAX(T3.DSEQNO) ")
                    'End If
                    '.Append("                          FROM TBL_STALLPROCESS T3 ")
                    '.Append("                         WHERE T3.DLRCD = T2.DLRCD ")
                    '.Append("                           AND T3.STRCD = T2.STRCD ")
                    '.Append("                           AND T3.REZID = T2.REZID ")
                    '.Append("                      GROUP BY T3.DLRCD, T3.STRCD, T3.REZID) ")
                    '.Append("       AND T2.SEQNO = (SELECT MAX(T4.SEQNO) ")
                    '.Append("                         FROM TBL_STALLPROCESS T4 ")
                    '.Append("                        WHERE T4.DLRCD = T2.DLRCD ")
                    '.Append("                          AND T4.STRCD = T2.STRCD ")
                    '.Append("                          AND T4.REZID = T2.REZID ")
                    '.Append("                          AND T4.DSEQNO = T2.DSEQNO ")
                    '.Append("                       ) ")

                    .Append("SELECT /* SC3150101_010 */ ")
                    .Append("       0 AS DSEQNO ")
                    .Append("     , T1.STALL_USE_ID AS SEQNO ")
                    .Append("     , DECODE(T1.RSLT_END_DATETIME, :MINDATE, NULL, TO_CHAR(T1.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_END_TIME ")
                    .Append("     , TRIM(T1.STALL_USE_STATUS) AS RESULT_STATUS ")
                    .Append(" FROM  TB_T_STALL_USE T1 ")
                    .Append("	  , TB_T_STAFF_JOB T2 ")
                    .Append(" WHERE T2.JOB_ID = T1.JOB_ID ")
                    .Append("   AND T1.STALL_USE_ID = :STALL_USE_ID ")
                    .Append("   AND T1.RSLT_START_DATETIME = :RSLT_START_DATETIME ")
                    .Append("   AND T2.STF_CD = :STF_CD ")
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                ''query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Date, CType(workDate.ToString("yyyy/MM/dd"), Date))
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, reserveId)
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, workDate)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                Logger.Info("[E]GetStaffResultInfo()")

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START

        ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        ' ''' <summary>
        ' ''' 作業ID取得
        ' ''' </summary>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public ReadOnly Property GetSequenceJobId() As SC3150101DataSet.SC3150101JobIDDataTable

        '    Get
        '        ' DBSelectQueryインスタンス生成
        '        Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101JobIDDataTable)("SC3150101_011_1")

        '            Logger.Info("[S]GetSequenceJobId()")

        '            Dim sql_1 As New StringBuilder

        '            ' SQL文の作成
        '            With sql_1
        '                .Append(" SELECT /* SC3150101_042 */")
        '                .Append(" SQ_JOB_ID.NEXTVAL AS JOB_ID FROM DUAL ")
        '            End With

        '            query.CommandText = sql_1.ToString()

        '            Logger.Info("[E]GetSequenceJobId()")

        '            'SQL実行
        '            Return query.GetData()

        '        End Using
        '    End Get
        'End Property

        ' ''' <summary>
        ' ''' スタッフ作業ID取得
        ' ''' </summary>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public ReadOnly Property GetSequenceStaffJobId() As SC3150101DataSet.SC3150101StaffJobIdDataTable

        '    Get
        '        ' DBSelectQueryインスタンス生成
        '        Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StaffJobIdDataTable)("SC3150101_011_2")

        '            Logger.Info("[S]GetSequenceStaffJobId()")

        '            Dim sql_1 As New StringBuilder

        '            ' SQL文の作成
        '            With sql_1
        '                .Append(" SELECT /* SC3150101_043 */")
        '                .Append(" SQ_STF_JOB_ID.NEXTVAL AS STF_JOB_ID FROM DUAL ")
        '            End With

        '            query.CommandText = sql_1.ToString()

        '            Logger.Info("[E]GetSequenceStaffJobId()")

        '            'SQL実行
        '            Return query.GetData()

        '        End Using
        '    End Get
        'End Property
        ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        ' ''' <summary>
        ' ''' 担当者実績情報の作成
        ' ''' </summary>
        ' ''' <param name="procInfo">ストール実績情報</param>
        ' ''' <param name="updateAccount">更新アカウント</param>
        ' ''' <param name="updateDate"> 更新日付</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function InsertStaffStall(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
        '                                 ByVal updateAccount As String, _
        '                                 ByVal staffCode As String, _
        '                                 ByVal updateDate As Date) As Integer

        '    Logger.Info("[S]InsertStaffStall()")


        '    'ストール実績データセットを展開
        '    Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
        '    drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)


        '    Dim sql As New StringBuilder

        '    ' SQL文の作成
        '    With sql
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        '.Append("    INSERT /* SC3150101_011 */ ")
        '        '.Append("      INTO TBL_TSTAFFSTALL ( ")
        '        '.Append("                            DLRCD, ")
        '        '.Append("                            STRCD, ")
        '        '.Append("                            REZID, ")
        '        '.Append("                            DSEQNO, ")
        '        '.Append("                            SEQNO, ")
        '        '.Append("                            SSEQNO, ")
        '        '.Append("                            STAFFCD, ")
        '        '.Append("                            WORKDATE, ")
        '        '.Append("                            STALLID, ")
        '        '.Append("                            CREATEDATE, ")
        '        '.Append("                            RSSTATUS, ")
        '        '.Append("                            RSDATE, ")
        '        '.Append("                            UPDATESERVER, ")
        '        '.Append("                            WORK_START, ")
        '        '.Append("                            WORK_END ")
        '        '.Append("                           ) ")
        '        '.Append("    SELECT T2.DLRCD, ")
        '        '.Append("           T2.STRCD, ")
        '        '.Append("           T2.REZID, ")
        '        '.Append("           T2.DSEQNO, ")
        '        '.Append("           T2.SEQNO, ")
        '        '.Append("           0, ")
        '        '.Append("           T1.STAFFCD, ")
        '        '.Append("           TO_DATE(:WORKDATE1, 'YYYY/MM/DD'), ")
        '        '.Append("           T1.STALLID, ")
        '        '.Append("           SYSDATE, ")
        '        '.Append("           '00', ")
        '        '.Append("           NULL, ")
        '        '.Append("           '', ")
        '        '.Append("           T2.RESULT_START_TIME, ")
        '        '.Append("           NULL ")
        '        '.Append("      FROM TBL_WSTAFFSTALL T1, ")
        '        '.Append("           TBL_STALLPROCESS T2 ")
        '        '.Append("     WHERE T1.DLRCD = T2.DLRCD ")
        '        '.Append("       AND T1.STRCD = T2.STRCD ")
        '        '.Append("       AND T1.STALLID = :STALLID ")
        '        '.Append("       AND T1.WORKDATE = :WORKDATE2 ")
        '        '.Append("       AND T2.REZID = :REZID ")
        '        '.Append("       AND T2.DSEQNO = (SELECT MAX(T3.DSEQNO) ")
        '        '.Append("                          FROM TBL_STALLPROCESS T3 ")
        '        '.Append("                         WHERE T3.DLRCD = T2.DLRCD ")
        '        '.Append("                           AND T3.STRCD = T2.STRCD ")
        '        '.Append("                           AND T3.REZID = T2.REZID ")
        '        '.Append("                      GROUP BY T3.DLRCD, T3.STRCD, T3.REZID) ")
        '        '.Append("       AND T2.SEQNO = (SELECT MAX(T4.SEQNO) ")
        '        '.Append("                         FROM TBL_STALLPROCESS T4 ")
        '        '.Append("                        WHERE T4.DLRCD = T2.DLRCD ")
        '        '.Append("                          AND T4.STRCD = T2.STRCD ")
        '        '.Append("                          AND T4.REZID = T2.REZID ")
        '        '.Append("                          AND T4.DSEQNO = T2.DSEQNO ")
        '        '.Append("                       ) ")

        '        .Append("    INSERT /* SC3150101_011 */ ")
        '        .Append("      INTO TB_T_STAFF_JOB( ")
        '        .Append("                           STF_JOB_ID ")
        '        .Append("                         , STF_CD ")
        '        .Append("                         , JOB_ID ")
        '        .Append("                         , JOB_TYPE ")
        '        .Append("                         , SCHE_START_DATETIME ")
        '        .Append("                         , SCHE_END_DATETIME ")
        '        .Append("                         , RSLT_START_DATETIME ")
        '        .Append("                         , RSLT_END_DATETIME ")
        '        .Append("                         , ROW_CREATE_DATETIME ")
        '        .Append("                         , ROW_CREATE_ACCOUNT ")
        '        .Append("                         , ROW_CREATE_FUNCTION ")
        '        .Append("                         , ROW_UPDATE_DATETIME ")
        '        .Append("                         , ROW_UPDATE_ACCOUNT ")
        '        .Append("                         , ROW_UPDATE_FUNCTION ")
        '        .Append("                         , ROW_LOCK_VERSION ")
        '        .Append("                         ) ")
        '        .Append("      VALUES ( ")
        '        .Append("              :STF_JOB_ID ")
        '        .Append("            , :STF_CD ")
        '        .Append("            , :JOB_ID ")
        '        .Append("            , :JOB_TYPE ")
        '        .Append("            , :SCHE_START_DATETIME ")
        '        .Append("            , :SCHE_END_DATETIME ")
        '        .Append("            , :RSLT_START_DATETIME ")
        '        .Append("            , :RSLT_END_DATETIME ")
        '        .Append("            , :UPDATEDATE ")
        '        .Append("            , :ROW_CREATE_ACCOUNT ")
        '        .Append("            , :ROW_CREATE_FUNCTION ")
        '        .Append("            , :UPDATEDATE ")
        '        .Append("            , :ROW_CREATE_ACCOUNT  ")
        '        .Append("            , :ROW_CREATE_FUNCTION ")
        '        .Append("            , :ROW_LOCK_VERSION ")
        '        .Append("            ) ")
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_011")

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("WORKDATE1", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))
        '        'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
        '        'query.AddParameterWithTypeValue("WORKDATE2", OracleDbType.Char, SetSqlValue(workDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture())))
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

        '        Dim rezStartTime As Date = Date.ParseExact(drProcInfo.REZ_START_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
        '        Dim rezEndTime As Date = Date.ParseExact(drProcInfo.REZ_END_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
        '        Dim resultStartTime As Date = Date.ParseExact(drProcInfo.RESULT_START_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
        '        Dim strRezStartTime As String = rezStartTime.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())
        '        Dim strRezEndTime As String = rezEndTime.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())
        '        Dim strResultStartTime As String = resultStartTime.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture())

        '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, staffCode)
        '        query.AddParameterWithTypeValue("STF_JOB_ID", OracleDbType.Decimal, drProcInfo.STF_JOB_ID)
        '        query.AddParameterWithTypeValue("JOB_ID", OracleDbType.Decimal, drProcInfo.JOB_ID)
        '        query.AddParameterWithTypeValue("JOB_TYPE", OracleDbType.NVarchar2, JOB_TYPE_0)
        '        query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, Date.Parse(strRezStartTime, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("SCHE_END_DATETIME", OracleDbType.Date, Date.Parse(strRezEndTime, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Date, Date.Parse(strResultStartTime, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, updateAccount)                           ' 行作成アカウント
        '        query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)                         ' 行作成機能
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)
        '        query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, DEFAULT_ROW_LOCK_VERSION)                 ' 行ロックバージョン
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        '        Logger.Info("[E]InsertStaffStall()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function


        ' ''' <summary>
        ' ''' 担当者実績情報の更新(実績ステータス："10"=入庫)
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店ID</param>
        ' ''' <param name="branchCode">店舗ID</param>
        ' ''' <param name="stallId">ストールID</param>
        ' ''' <param name="reserveId">予約ID</param>
        ' ''' <param name="updateAccount">更新者アカウント</param>
        ' ''' <param name="workDate">作業日付</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function DeleteStaffStall(ByVal dealerCode As String, _
        '                                 ByVal branchCode As String, _
        '                                 ByVal stallId As Integer, _
        '                                 ByVal reserveId As Decimal, _
        '                                 ByVal updateAccount As String, _
        '                                 ByVal workDate As Date) As Integer

        '    Logger.Info("[S]DeleteStaffStall()")

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_012")

        '        Dim sql As New StringBuilder

        '        ' SQL文の作成
        '        With sql
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '            '.Append("DELETE /* SC3150101_012 */ ")
        '            '.Append("  FROM TBL_TSTAFFSTALL t1 ")
        '            '.Append(" WHERE t1.DLRCD = :DLRCD ")
        '            '.Append("   AND t1.STRCD = :STRCD ")
        '            '.Append("   AND t1.REZID = :REZID ")
        '            '.Append("   AND t1.DSEQNO = :DSEQNO ")
        '            '.Append("   AND t1.SEQNO = :SEQNO ")
        '            '.Append("   AND t1.STALLID = :STALLID ")
        '            '.Append("   AND t1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")

        '            .Append("DELETE /* SC3150101_012 */ ")
        '            .Append("  FROM TB_T_STAFF_JOB T1 ")
        '            .Append(" WHERE T1.STF_CD = :STF_CD ")
        '            .Append("   AND T1.RSLT_END_DATETIME = :MINDATE")
        '            .Append("   AND T1.RSLT_START_DATETIME >= :RSLT_START_DATETIME ")
        '            .Append("   AND T1.JOB_ID = ( SELECT T2.JOB_ID ")
        '            .Append("                     FROM TB_T_STALL_USE T2 ")
        '            .Append("                     WHERE T2.DLR_CD = :DLR_CD ")
        '            .Append("                       AND T2.BRN_CD = :BRN_CD ")
        '            .Append("                       AND T2.STALL_USE_ID = :STALL_USE_ID ")
        '            .Append("                       AND T2.STALL_ID = :STALL_ID ) ")
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
        '        'query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, daySeqNo)
        '        'query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)
        '        'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
        '        'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))

        '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, updateAccount)
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, reserveId)
        '        query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
        '        query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.Char, workDate)
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        '        Logger.Info("[E]DeleteStaffStall()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function


        ' ''' <summary>
        ' ''' 担当者実績情報の更新(実績ステータス："20"=作業中)
        ' ''' </summary>
        ' ''' <param name="reserveId">予約ID</param>
        ' ''' <param name="updateAccount">更新者アカウント</param>
        ' ''' <param name="updateDate">更新日付</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function UpdateStaffStallAtWork(ByVal reserveId As Decimal, _
        '                                       ByVal updateAccount As String,
        '                                       ByVal updateDate As Date) As Integer

        '    Logger.Info("[S]UpdateStaffStallAtWork()")

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_013")

        '        Dim sql As New StringBuilder

        '        ' SQL文の作成
        '        With sql
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '            '.Append("UPDATE /* SC3150101_013 */ ")
        '            '.Append("       TBL_TSTAFFSTALL T1 ")
        '            '.Append("   SET T1.WORK_END = NULL ")
        '            '.Append(" WHERE T1.DLRCD = :DLRCD ")
        '            '.Append("   AND T1.STRCD = :STRCD ")
        '            '.Append("   AND T1.REZID = :REZID ")
        '            '.Append("   AND T1.DSEQNO = :DSEQNO ")
        '            '.Append("   AND T1.SEQNO = :SEQNO ")
        '            '.Append("   AND T1.SSEQNO = (SELECT MAX(T2.SSEQNO) ")
        '            '.Append("                      FROM TBL_TSTAFFSTALL T2 ")
        '            '.Append("                     WHERE T2.DLRCD = T1.DLRCD ")
        '            '.Append("                       AND T2.STRCD = T1.STRCD ")
        '            '.Append("                       AND T2.REZID = T1.REZID ")
        '            '.Append("                       AND T2.DSEQNO = T1.DSEQNO ")
        '            '.Append("                       AND T2.SEQNO = T1.SEQNO ) ")
        '            '.Append("   AND T1.STALLID = :STALLID ")
        '            '.Append("   AND T1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")
        '            '.Append("   AND T1.WORK_END IS NOT NULL ")

        '            .Append("UPDATE /* SC3150101_013 */ ")
        '            .Append("       TB_T_STAFF_JOB T1 ")
        '            .Append("   SET T1.RSLT_END_DATETIME = :MINDATE ")
        '            .Append("     , T1.ROW_UPDATE_DATETIME = :UPDATEDATE ")
        '            .Append("     , T1.ROW_UPDATE_ACCOUNT = :STF_CD ")
        '            .Append("     , T1.ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '            .Append("     , T1.ROW_LOCK_VERSION = (ROW_LOCK_VERSION + 1) ")
        '            .Append(" WHERE T1.RSLT_END_DATETIME <>:MINDATE ")
        '            .Append("   AND T1.JOB_ID = ( SELECT T2.JOB_ID ")
        '            .Append("                      FROM TB_T_STALL_USE T2 ")
        '            .Append("                     WHERE T2.STALL_USE_ID = :STALL_USE_ID ) ")
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
        '        'query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, daySeqNo)
        '        'query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)
        '        'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
        '        'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))

        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, updateAccount)
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, reserveId)
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)          ' 行更新機能
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)          ' 更新日時
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        '        Logger.Info("[E]UpdateStaffStallAtWork()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function


        ' ''' <summary>
        ' ''' 担当者実績情報の更新(実績ステータス：実績ステータス："10"=入庫、"20"=作業中 以外)
        ' ''' </summary>
        ' ''' <param name="reserveId">予約ID</param>
        ' ''' <param name="endTime">実績終了日時</param>
        ' ''' <param name="account">更新者アカウント</param>
        ' ''' <param name="updateDate">更新日付</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function UpdateStaffStall(ByVal reserveId As Decimal, _
        '                                 ByVal endTime As String, _
        '                                 ByVal account As String, _
        '                                 ByVal updateDate As Date) As Integer

        '    Logger.Info("[S]UpdateStaffStall()")

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_014")

        '        Dim sql As New StringBuilder

        '        ' SQL文の作成
        '        With sql
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '            'etc
        '            '.Append("UPDATE /* SC3150101_014 */ ")
        '            '.Append("       TBL_TSTAFFSTALL T1 ")
        '            '.Append("   SET T1.WORK_END = :WORK_END ")
        '            '.Append(" WHERE T1.DLRCD = :DLRCD ")
        '            '.Append("   AND T1.STRCD = :STRCD ")
        '            '.Append("   AND T1.REZID = :REZID ")
        '            '.Append("   AND T1.DSEQNO = :DSEQNO ")
        '            '.Append("   AND T1.SEQNO = :SEQNO ")
        '            '.Append("   AND T1.STALLID = :STALLID ")
        '            '.Append("   AND T1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")
        '            '.Append("   AND T1.WORK_END IS NULL ")

        '            .Append("UPDATE /* SC3150101_014 */ ")
        '            .Append("       TB_T_STAFF_JOB T1 ")
        '            .Append("   SET T1.RSLT_END_DATETIME = :RSLT_END_DATETIME ")
        '            .Append("     , T1.ROW_UPDATE_DATETIME = :UPDATEDATE ")
        '            .Append("     , T1.ROW_UPDATE_ACCOUNT = :STF_CD ")
        '            .Append("     , T1.ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '            .Append("     , T1.ROW_LOCK_VERSION = (ROW_LOCK_VERSION + 1) ")
        '            .Append(" WHERE T1.RSLT_END_DATETIME = :MINDATE")
        '            .Append("   AND T1.JOB_ID = ( SELECT T2.JOB_ID ")
        '            .Append("                       FROM TB_T_STALL_USE T2 ")
        '            .Append("                      WHERE T2.STALL_USE_ID = :STALL_USE_ID ) ")
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
        '        'query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, daySeqNo)
        '        'query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)
        '        'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
        '        'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))
        '        'query.AddParameterWithTypeValue("WORK_END", OracleDbType.Char, endTime)

        '        Dim workTimeTo As Date = DateTime.ParseExact(endTime, "yyyyMMddHHmm", CultureInfo.InvariantCulture())
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, reserveId)
        '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, account)
        '        query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.Date, workTimeTo)
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)  ' 行更新機能
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)                    ' 更新日時
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        '        Logger.Info("[E]UpdateStaffStall()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' 休憩時間帯、使用不可時間帯取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="fromDate">取得対象時刻範囲(FROM)</param>
        ''' <param name="toDate">取得対象時刻範囲(TO)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetBreakSlot(ByVal stallId As Decimal, _
                                     ByVal fromDate As Date, _
                                     ByVal toDate As Date) As SC3150101DataSet.SC3150101StallBreakInfoDataTable

            Logger.Info("[S]GetBreakSlot()")

            Dim dt As SC3150101DataSet.SC3150101StallBreakInfoDataTable

            ' DBSelectQueryインスタンス生成(休憩時間帯)
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallBreakInfoDataTable)("SC3150101_015")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150101_015 */ ")
                    .Append("         T1.STALLID AS STALLID ")
                    .Append("       , T1.STARTTIME AS STARTTIME ")
                    .Append("       , T1.ENDTIME AS ENDTIME ")
                    .Append("    FROM TBL_STALLBREAK T1 ")
                    .Append("   WHERE T1.STALLID = :STALLID1 ")
                    .Append("     AND T1.BREAKKBN = :BREAKKBN_1 ")
                    .Append("ORDER BY T1.STARTTIME, T1.ENDTIME")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("STALLID1", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("BREAKKBN_1", OracleDbType.NVarchar2, BREAKKBN_1)

                dt = query.GetData()

            End Using

            ' 2012/06/14 KN 西田 STEP1 重要課題対応 DevPartner指摘対応 START
            ' DBSelectQueryインスタンス生成(使用不可時間帯)
            Using query2 As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallBreakInfoDataTable)("SC3150101_032")

                Dim sql2 As New StringBuilder

                With sql2
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("  SELECT /* SC3150101_032 */ ")
                    '.Append("         T2.STALLID AS STALLID, ")
                    '.Append("         TO_CHAR(T2.STARTTIME, 'HH24MI') AS STARTTIME, ")
                    '.Append("         TO_CHAR(T2.ENDTIME, 'HH24MI') AS ENDTIME ")
                    '.Append("    FROM TBL_STALLREZINFO T2 ")
                    '.Append("   WHERE T2.STALLID = :STALLID2 ")
                    '.Append("     AND T2.ENDTIME < TO_DATE(:ENDTIME1, 'YYYY/MM/DD HH24:MI:SS') ")
                    '.Append("     AND T2.ENDTIME > TO_DATE(:ENDTIME2, 'YYYY/MM/DD HH24:MI:SS') ")
                    '.Append("     AND T2.CANCELFLG <> '1' ")
                    '.Append("     AND T2.STATUS = '3' ")
                    '.Append("ORDER BY T2.STARTTIME, T2.ENDTIME")

                    .Append("  SELECT /* SC3150101_032 */ ")
                    .Append("         T3.STALL_ID AS STALLID ")
                    .Append("       , TO_CHAR(T3.IDLE_START_DATETIME,'HH24MI') AS STARTTIME ")
                    .Append("       , TO_CHAR(T3.IDLE_END_DATETIME, 'HH24MI') AS ENDTIME ")
                    .Append("    FROM TB_M_STALL_IDLE T3 ")
                    .Append("   WHERE T3.STALL_ID = :STALL_ID ")
                    .Append("     AND T3.IDLE_END_DATETIME > :SCHE_END_DATETIME2 ")
                    .Append("     AND T3.IDLE_END_DATETIME < :SCHE_END_DATETIME1 ")
                    .Append("     AND T3.CANCEL_FLG <> :CANCEL_FLG_1 ")
                    .Append("     AND T3.IDLE_TYPE = :IDLE_TYPE_2 ")
                    .Append("ORDER BY T3.IDLE_START_DATETIME,T3.IDLE_END_DATETIME ")
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query2.CommandText = sql2.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query2.AddParameterWithTypeValue("STALLID2", OracleDbType.Int64, stallId)
                'query2.AddParameterWithTypeValue("ENDTIME1", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                'query2.AddParameterWithTypeValue("ENDTIME2", OracleDbType.Char, fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                '' 2012/06/14 KN 西田 STEP1 重要課題対応 DevPartner指摘対応 END

                query2.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query2.AddParameterWithTypeValue("IDLE_TYPE_2", OracleDbType.NVarchar2, IDLE_TYPE_2)
                query2.AddParameterWithTypeValue("SCHE_END_DATETIME1", OracleDbType.Date, toDate)
                query2.AddParameterWithTypeValue("SCHE_END_DATETIME2", OracleDbType.Date, fromDate)
                query2.AddParameterWithTypeValue("CANCEL_FLG_1", OracleDbType.NVarchar2, CANCEL_FLG_1)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                '休憩時間帯と使用不可時間帯をマージ
                dt.Merge(query2.GetData())

            End Using

            'ソート結果を格納する変数
            Dim dtClone As SC3150101DataSet.SC3150101StallBreakInfoDataTable = CType(dt.Clone(), SC3150101DataSet.SC3150101StallBreakInfoDataTable)

            ' ソートされたデータビューの作成
            Using dv As New DataView(dt)
                dv.Sort = "STARTTIME, ENDTIME"

                ' ソートされたレコードのコピー
                For Each drv As DataRowView In dv
                    dtClone.ImportRow(drv.Row)
                Next
            End Using

            Logger.Info("[E]GetBreakSlot()")

            'SQL実行
            Return dtClone

        End Function


        ''' <summary>
        ''' 販売店環境設定値取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="parameterName">パラメータ名</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetDealerEnvironmentSettingValue(ByVal dealerCode As String, _
                                                         ByVal branchCode As String, _
                                                         ByVal parameterName As String) As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable

            Logger.Info("[S]GetDealerEnvironmentSettingValue()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable)("SC3150101_016")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("   SELECT /* SC3150101_016 */ ")
                    '.Append("          PARAMVALUE ")
                    '.Append("     FROM TBL_DLRENVSETTING ")
                    '.Append("    WHERE DLRCD IN (:DLRCD, '00000') ")
                    '.Append("      AND STRCD IN (:STRCD, '000') ")
                    '.Append("      AND PARAMNAME = :PARAMNAME ")
                    '.Append(" ORDER BY DLRCD DESC, STRCD DESC ")

                    .Append("   SELECT /* SC3150101_016 */ ")
                    .Append("          SETTING_VAL AS PARAMVALUE  ")
                    .Append("     FROM TB_M_SYSTEM_SETTING_DLR ")
                    .Append("    WHERE DLR_CD IN (:DLR_CD, N'XXXXX') ")
                    .Append("      AND BRN_CD IN (:BRN_CD, N'XXX') ")
                    .Append("      AND SETTING_NAME = :SETTING_NAME ")
                    .Append(" ORDER BY DLR_CD ASC, BRN_CD ASC ")
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("PARAMNAME", OracleDbType.Varchar2, parameterName)

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)              '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)              '店舗コード
                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, parameterName) '設定名
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                Logger.Info("[E]GetDealerEnvironmentSettingValue()")

                'SQL実行
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 処理対象日のUnavailableチップのリストを取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="targetDayStart">営業開始日時</param>
        ''' <param name="targetDayEnd">営業終了日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetUnavailableList(ByVal stallId As Decimal, _
                                           ByVal targetDayStart As Date, _
                                           ByVal targetDayEnd As Date) As SC3150101DataSet.SC3150101UnavailableChipListDataTable

            Logger.Info("[S]GetUnavailableList()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101UnavailableChipListDataTable)("SC3150101_017")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("SELECT /* SC3150101_017 */ " & vbCrLf)
                    '.Append("       TO_CHAR(STARTTIME, 'yyyyMMdd') AS STARTTIME_DAY, ")
                    '.Append("       TO_CHAR(STARTTIME, 'HH24MI') AS STARTTIME_TIME, ")
                    '.Append("       TO_CHAR(ENDTIME, 'yyyyMMdd') AS ENDTIME_DAY, ")
                    '.Append("       TO_CHAR(ENDTIME, 'HH24MI') AS ENDTIME_TIME ")
                    '.Append("  FROM TBL_STALLREZINFO ")
                    '.Append(" WHERE DLRCD = :DLRCD ")
                    '.Append("   AND STRCD = :STRCD ")
                    '.Append("   AND STALLID = :STALLID ")
                    '.Append("   AND STARTTIME < TO_DATE(:STARTTIME, 'YYYYMMDDHH24MI') ")
                    '.Append("   AND ENDTIME > TO_DATE(:ENDTIME, 'YYYYMMDDHH24MI') ")
                    '.Append("   AND STATUS = 3 ")
                    '.Append("   AND CANCELFLG <> '1'")

                    .Append("SELECT /* SC3150101_017 */ ")
                    .Append("       TO_CHAR(T2.IDLE_START_DATETIME, 'yyyyMMdd') AS STARTTIME_DAY ")
                    .Append("     , TO_CHAR(T2.IDLE_START_DATETIME, 'HH24MI') AS STARTTIME_TIME ")
                    .Append("     , TO_CHAR(T2.IDLE_END_DATETIME, 'yyyyMMdd') AS ENDTIME_DAY ")
                    .Append("     , TO_CHAR(T2.IDLE_END_DATETIME, 'HH24MI') AS ENDTIME_TIME ")
                    .Append("  FROM TB_M_STALL_IDLE T2 ")
                    .Append(" WHERE T2.STALL_ID = :STALL_ID")
                    .Append("   AND T2.IDLE_START_DATETIME > TO_DATE(:IDLE_START_DATETIME, 'YYYYMMDDHH24MI')")
                    .Append("   AND T2.IDLE_END_DATETIME < TO_DATE(:IDLE_END_DATETIME,'YYYYMMDDHH24MI') ")
                    .Append("   AND T2.IDLE_TYPE = :IDLE_TYPE_2 ")
                    .Append("   AND T2.CANCEL_FLG <> :CANCEL_FLG_1")
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, targetDayEnd.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))
                'query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, targetDayStart.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))

                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("IDLE_START_DATETIME", OracleDbType.NVarchar2, targetDayStart.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("IDLE_END_DATETIME", OracleDbType.NVarchar2, targetDayEnd.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("IDLE_TYPE_2", OracleDbType.NVarchar2, IDLE_TYPE_2)
                query.AddParameterWithTypeValue("CANCEL_FLG_1", OracleDbType.NVarchar2, CANCEL_FLG_1)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                Logger.Info("[E]GetUnavailableList()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 次の非稼動日の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetNextNonworkingDate(ByVal dealerCode As String, _
                                              ByVal branchCode As String, _
                                              ByVal stallId As Decimal, _
                                              ByVal targetDate As Date) As SC3150101DataSet.SC3150101NextNonworkingDateDataTable

            Logger.Info("[S]GetNextNonworkingDate()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101NextNonworkingDateDataTable)("SC3150101_018")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_018 */ ")
                    .Append("       WORKDATE ")
                    .Append("  FROM (SELECT WORKDATE ")
                    .Append("          FROM TBL_STALLPLAN ")
                    .Append("         WHERE DLRCD = :DLRCD ")
                    .Append("           AND STRCD = :STRCD ")
                    .Append("           AND STALLID IN(-1, :STALLID) ")
                    .Append("           AND WORKDATE > :WORKDATE ")
                    .Append("      ORDER BY WORKDATE) ")
                    .Append(" WHERE ROWNUM <= 1")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, targetDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture()))

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.NVarchar2, targetDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture()))
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                Logger.Info("[E]GetNextNonworkingDate()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 予約チップ情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <param name="dateTo">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetReserveChipInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal stallId As Decimal, _
                                           ByVal dateFrom As Date, _
                                           ByVal dateTo As Date) As SC3150101DataSet.SC3150101ReserveChipInfoDataTable

            Logger.Info("[S]GetReserveChipInfo()")

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                ' 2012/02/28 上田 SQLインスペクション対応 Start
                '.Append("with vREZINFO AS (SELECT /* SC3150101_019 */ ")
                '.Append("          T1.STARTTIME, ")
                '.Append("          T1.ENDTIME, ")
                '.Append("          T1.STALLID, ")
                '.Append("          T1.REZID, ")
                '.Append("          T1.INSDID, ")
                '.Append("          T1.STATUS, ")
                '.Append("          T1.CUSTOMERNAME, ")
                '.Append("          T1.REZ_RECEPTION, ")
                '.Append("          T1.CRRYINTIME, ")
                '.Append("          T1.CRRYOUTTIME, ")
                '.Append("          T1.VCLREGNO, ")
                '.Append("          T1.SERVICECODE_S, ")
                '.Append("          T1.STRDATE, ")
                '.Append("          T4.SVCORGNMCT, ")
                '.Append("          T4.SVCORGNMCB, ")
                '.Append("          NVL(T1.UPDATE_COUNT, 0) AS UPDATE_COUNT, ")
                '.Append("          T1.STOPFLG As STOPFLG, ")
                ''.Append("          (CASE T1.STOPFLG ")
                ''.Append("               WHEN '2' THEN '32' ")
                ''.Append("               WHEN '5' THEN '34' ")
                ''.Append("               WHEN '6' THEN '33' ")
                ''.Append("               ELSE '00' ")
                ''.Append("           END) AS STOPFLG, ")
                '.Append("          T2.RESULT_STATUS, ")
                '.Append("          T1.REZ_WORK_TIME, ")
                '.Append("          T3.SERVICECODE, ")
                '.Append("          T1.UPDATEACCOUNT, ")
                '.Append("          T1.VEHICLENAME, ")
                '.Append("          T1.CANCELFLG, ")
                '.Append("          T1.UPDATEDATE, ")
                '.Append("          T1.INPUTACCOUNT, ")
                '.Append("          T1.MERCHANDISECD, ")
                '.Append("          T4.SERVICECODE AS SERVICECODE_2, ")
                '.Append("          T1.WALKIN, ")
                '.Append("          T2.UPDATE_COUNT AS UPDATE_COUNT_2, ")
                '.Append("          T1.STOPFLG AS STOPFLG_2, ")
                '.Append("          NVL(T2.SEQNO, 0) AS SEQNO, ")
                '.Append("          NVL(T2.DSEQNO,0) AS DSEQNO , ")
                '.Append("          NVL(T1.PREZID,'') AS PREZID, ")
                '.Append("          NVL(T1.REZCHILDNO,'') AS REZCHILDNO, ")
                '.Append("          T2.REZ_END_TIME, ")
                '.Append("          T3.DLRCD, ")
                '.Append("          T3.STRCD, ")
                '.Append("          T1.DLRCD AS DLRCD_2, ")
                '.Append("          T1.STRCD AS STRCD_2, ")
                '.Append("          T1.VIN, ")
                '.Append("          T2.REZ_START_TIME, ")
                '.Append("          T1.ACCOUNT_PLAN ") ' SAコード
                ''.Append("          /* USERNAME */ ") ' SA名 tbl_USERSより
                '.Append("        , T1.ORDERNO ") ' R/O No.
                '.Append("     FROM TBL_STALLREZINFO T1 ")
                '.Append("LEFT JOIN (SELECT T5.DLRCD, ")
                '.Append("                  T5.STRCD, ")
                '.Append("                  T5.REZID, ")
                '.Append("                  T5.DSEQNO, ")
                '.Append("                  T5.SEQNO, ")
                '.Append("                  T5.RESULT_STATUS, ")
                '.Append("                  T5.UPDATE_COUNT, ")
                '.Append("                  T5.REZ_END_TIME, ")
                '.Append("                  T5.REZ_START_TIME ")
                '.Append("             FROM TBL_STALLPROCESS T5 ")
                '.Append("            WHERE T5.DLRCD = :DLRCD1 ") '''''販売店コード
                '.Append("              AND T5.STRCD = :STRCD1 ") '''''店舗コード
                '.Append("              AND ( T5.RESULT_START_TIME < :RESULT_START_TIME1 ") '''''稼働時間To
                '.Append("                   AND T5.RESULT_START_TIME >= :RESULT_START_TIME2 ") '''''稼働時間From
                '.Append("                    OR T5.RESULT_STATUS IN ('0', '00', '10', '11')) ")
                '.Append("          ) T2 ")
                '.Append("       ON T2.DLRCD = T1.DLRCD ")
                '.Append("      AND T2.STRCD = T1.STRCD ")
                '.Append("      AND T2.REZID = T1.REZID ")
                '.Append("LEFT JOIN TBL_STALL T3 ")
                '.Append("       ON T3.STALLID = T1.STALLID ")
                '.Append("LEFT JOIN tbl_MERCHANDISEMST T4 ")
                '.Append("       ON T4.MERCHANDISECD = T1.MERCHANDISECD ")
                '.Append("      AND T4.DLRCD = T1.DLRCD ")
                '.Append("    WHERE T1.DLRCD = :DLRCD2 ") '''''販売店コード
                '.Append("      AND T1.STRCD = :STRCD2 ") '''''店舗コード
                '.Append("      AND T1.STALLID = :STALLID1 ") '''''ストールID
                '.Append("      AND T1.STATUS <> 3 ")
                '.Append("      AND ( T2.RESULT_STATUS IN ('0', '00', '10') OR T2.RESULT_STATUS IS NULL ) ")
                '.Append("      AND ( T1.ENDTIME >= TO_DATE( :ENDTIME1 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間Fromの日付部分＋00:00:00
                '.Append("           AND T1.STARTTIME < TO_DATE( :STARTTIME1 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間To
                '.Append("           AND T1.STOPFLG NOT IN ('2', '5', '6') ")
                '.Append("           AND T1.CANCELFLG <> '1' ")
                '.Append("           AND ( T1.ENDTIME >= TO_DATE( :ENDTIME2 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間From
                '.Append("                OR ( T1.STARTTIME = TO_DATE( :STARTTIME2 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間Fromの日付部分＋00:00:00
                '.Append("                    AND T1.ENDTIME = TO_DATE( :ENDTIME3 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間Fromの日付部分＋00:00:00
                '.Append("                   ) ")
                '.Append("               ) ")
                '.Append("          ) ")
                '.Append("      AND ( TO_CHAR(T1.STARTTIME, 'YYYYMMDD') = TO_CHAR(T1.ENDTIME, 'YYYYMMDD') ") '''''
                '.Append("           OR (T2.RESULT_STATUS IS NULL OR T2.RESULT_STATUS < 20) ")
                '.Append("           OR NOT EXISTS ( SELECT T6.REZID ")
                '.Append("                             FROM TBL_STALLPROCESS T6 ")
                '.Append("                            WHERE T6.DLRCD = T2.DLRCD ")
                '.Append("                              AND T6.STRCD = T2.STRCD ")
                '.Append("                              AND T6.REZID = T2.REZID ")
                '.Append("                              AND T6.DSEQNO = T2.DSEQNO ")
                '.Append("                              AND T6.RESULT_START_TIME < :RESULT_START_TIME3 ") '''''稼働時間To
                '.Append("                              AND T6.RESULT_START_TIME >= :RESULT_START_TIME4 ) ") '''''稼働時間From
                '.Append("          ) ")
                '.Append(") ")
                '.Append("   SELECT MT.STARTTIME AS STARTTIME, ")
                '.Append("          MT.ENDTIME AS ENDTIME, ")
                ''.Append("          MT.STALLID AS STALLID, ")
                '.Append("          NVL(MT.STALLID, 0) AS STALLID, ")
                ''.Append("          MT.REZID AS REZID, ")
                '.Append("          NVL(MT.REZID, 0) AS REZID, ")
                '.Append("          MT.INSDID AS INSDID, ")
                ''.Append("          MT.STATUS AS STATUS, ")
                '.Append("          NVL(MT.STATUS, 0) AS STATUS, ")
                '.Append("          MT.CUSTOMERNAME AS CUSTOMERNAME, ")
                '.Append("          MT.REZ_RECEPTION AS REZ_RECEPTION, ")
                '.Append("          MT.CRRYINTIME AS CRRYINTIME, ")
                '.Append("          MT.CRRYOUTTIME AS CRRYOUTTIME, ")
                '.Append("          MT.VCLREGNO AS VCLREGNO, ")
                '.Append("          MT.SERVICECODE_S AS SERVICECODE_S, ")
                ''.Append("          MT.STRDATE AS STRDATE, ")
                '.Append("          NVL(MT.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE, ")
                '.Append("          MT.SVCORGNMCT AS SVCORGNMCT, ")
                '.Append("          MT.SVCORGNMCB AS SVCORGNMCB, ")
                ''.Append("          MT.UPDATE_COUNT AS UPDATE_COUNT, ")
                '.Append("          NVL(MT.UPDATE_COUNT, 0) AS UPDATE_COUNT, ")
                '.Append("          MT.STOPFLG AS STOPFLG, ")
                ''.Append("          MT.RESULT_STATUS AS RESULT_STATUS, ")
                '.Append("          NVL(MT.RESULT_STATUS, '  ') AS RESULT_STATUS, ")
                ''.Append("          MT.REZ_WORK_TIME AS REZ_WORK_TIME, ")
                '.Append("          NVL(MT.REZ_WORK_TIME, 0) AS REZ_WORK_TIME, ")
                '.Append("          MT.SERVICECODE AS SERVICECODE, ")
                '.Append("          MT.UPDATEACCOUNT AS UPDATEACCOUNT, ")
                '.Append("          MT.VEHICLENAME AS VEHICLENAME, ")
                '.Append("          MT.CANCELFLG AS CANCELFLG, ")
                '.Append("          MT.UPDATEDATE AS UPDATEDATE, ")
                '.Append("          MT.INPUTACCOUNT AS INPUTACCOUNT, ")
                '.Append("          MT.MERCHANDISECD AS MERCHANDISECD, ")
                '.Append("          MT.SERVICECODE_2 AS SERVICECODE_2, ")
                '.Append("          MT.WALKIN AS WALKIN, ")
                ''.Append("          MT.UPDATE_COUNT_2 AS UPDATE_COUNT_2, ")
                '.Append("          NVL(MT.UPDATE_COUNT_2, 0) AS UPDATE_COUNT_2, ")
                '.Append("          MT.STOPFLG_2 AS STOPFLG_2, ")
                ''.Append("          MT.SEQNO AS SEQNO, ")
                '.Append("          NVL(MT.SEQNO, 0) AS SEQNO, ")
                ''.Append("          MT.DSEQNO AS DSEQNO, ")
                '.Append("          NVL(MT.DSEQNO, 0) AS DSEQNO, ")
                ''.Append("          MT.PREZID AS PREZID, ")
                ''.Append("          MT.REZCHILDNO AS REZCHILDNO, ")
                ''.Append("          MT.REZ_END_TIME AS REZ_END_TIME, ")
                '.Append("          NVL(MT.PREZID, -1) AS PREZID, ")
                '.Append("          NVL(MT.REZCHILDNO, -1) AS REZCHILDNO, ")
                '.Append("          NVL(MT.REZ_END_TIME, '') AS REZ_END_TIME, ")
                '.Append("          MT.DLRCD AS DLRCD, ")
                '.Append("          MT.STRCD AS STRCD, ")
                ''.Append("          MT.REZ_START_TIME AS REZ_START_TIME, ")
                '.Append("          NVL(MT.REZ_START_TIME, '            ') AS REZ_START_TIME, ")
                '.Append("          MT.ACCOUNT_PLAN AS ACCOUNT_PLAN, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT T20.RESULT_STATUS ")
                '.Append("                                                      FROM TBL_STALLREZINFO T10 ")
                '.Append("                                                INNER JOIN TBL_STALLPROCESS T20 ")
                '.Append("                                                        ON T10.DLRCD = T20.DLRCD ")
                '.Append("                                                       AND T10.STRCD = T20.STRCD ")
                '.Append("                                                       AND T10.REZID = T20.REZID ")
                '.Append("                                                     WHERE T10.DLRCD = MT.DLRCD_2 ")
                '.Append("                                                       AND T10.STRCD = MT.STRCD_2 ")
                '.Append("                                                       AND T10.PREZID = MT.PREZID ")
                '.Append("                                                       AND T10.REZCHILDNO > 0 ")
                '.Append("                                                       AND T10.REZCHILDNO < 999 ")
                '.Append("                                                       AND NOT T20.RESULT_STATUS IS NULL ")
                '.Append("                                                       AND T20.RESULT_STATUS NOT IN('00','01','10','11','32','33','34') ")
                '.Append("                                                       AND ROWNUM = 1 ),'0') ")
                '.Append("               ELSE '0' ")
                '.Append("           END) AS RELATIONSTATUS, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT COUNT(1) ")
                '.Append("                                                      FROM TBL_STALLREZINFO T11 ")
                '.Append("                                           LEFT OUTER JOIN TBL_STALLPROCESS T21 ")
                '.Append("                                                        ON T11.DLRCD = T21.DLRCD ")
                '.Append("                                                       AND T11.STRCD = T21.STRCD ")
                '.Append("                                                       AND T11.REZID = T21.REZID ")
                '.Append("                                                     WHERE T11.DLRCD = MT.DLRCD ")
                '.Append("                                                       AND T11.STRCD = MT.STRCD ")
                '.Append("                                                       AND T11.PREZID = MT.PREZID ")
                '.Append("                                                       AND T11.REZCHILDNO > 0 ")
                '.Append("                                                       AND T11.REZCHILDNO < 999 ")
                ''.Append("                                                       AND NOT (T11.CANCELFLG = '1' AND T11.STOPFLG = '0') ")
                '.Append("                                                       AND NOT (T11.CANCELFLG = '1' AND T11.STOPFLG IN ('0', '2', '5', '6')) ")
                '.Append("                                                       AND (T21.RESULT_STATUS IS NULL OR T21.RESULT_STATUS NOT IN ('97','99')) ")
                '.Append("                                                       AND (T21.DSEQNO IS NULL ")
                '.Append("                                                            OR T21.DSEQNO = (SELECT MAX(T22.DSEQNO) ")
                '.Append("                                                                               FROM TBL_STALLPROCESS T22 ")
                '.Append("                                                                              WHERE T22.DLRCD = T21.DLRCD ")
                '.Append("                                                                                AND T22.STRCD = T21.STRCD ")
                '.Append("                                                                                AND T22.REZID = T21.REZID)) ")
                '.Append("                                                                                AND (T21.SEQNO IS NULL ")
                '.Append("                                                                                     OR T21.SEQNO = (SELECT MAX(T23.SEQNO) ")
                '.Append("                                                                                                       FROM TBL_STALLPROCESS T23 ")
                '.Append("                                                                                                      WHERE T23.DLRCD = T21.DLRCD ")
                '.Append("                                                                                                        AND T23.STRCD = T21.STRCD ")
                '.Append("                                                                                                        AND T23.REZID = T21.REZID ")
                '.Append("                                                                                                        AND T23.DSEQNO = T21.DSEQNO) ")
                '.Append("                                                                                    ) ")
                '.Append("                                                   ), 0) ")
                '.Append("               ELSE 0 ")
                '.Append("           END) AS RELATION_UNFINISHED_COUNT ")
                '.Append("         , MT.ORDERNO AS ORDERNO ")
                '.Append("     FROM vREZINFO MT ")
                '.Append("LEFT JOIN TBL_SMBVCLINFO OV ")
                '.Append("       ON OV.DLRCD = :DLRCD3 ") '''''販売店コード
                '.Append("      AND MT.INSDID = OV.ORIGINALID ")
                '.Append("      AND MT.VIN = OV.VIN ")
                '.Append("      AND MT.VCLREGNO = OV.VCLREGNO ")
                '.Append("LEFT JOIN TBL_SMBCUSTOMER NC ")
                '.Append("       ON NC.DLRCD = :DLRCD4 ") '''''販売店コード
                '.Append("      AND MT.INSDID = NC.ORIGINALID ")

                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                '.Append("SELECT /* SC3150101_019 */")
                '.Append("       T1.DLRCD AS DLRCD")
                '.Append("     , T1.STRCD AS STRCD")
                '.Append("     , NVL(T1.STALLID, 0) AS STALLID")
                '.Append("     , NVL(T1.REZID, 0) AS REZID")
                '.Append("     , T1.ORDERNO AS ORDERNO")
                '.Append("     , T1.STARTTIME AS STARTTIME")
                '.Append("     , T1.ENDTIME AS ENDTIME")
                '.Append("     , T1.INSDID AS INSDID")
                '.Append("     , NVL(T1.STATUS, 0) AS STATUS")
                '.Append("     , T1.CUSTOMERNAME AS CUSTOMERNAME")
                '.Append("     , T1.REZ_RECEPTION AS REZ_RECEPTION")
                '.Append("     , T1.CRRYINTIME AS CRRYINTIME")
                '.Append("     , T1.CRRYOUTTIME AS CRRYOUTTIME")
                '.Append("     , T1.VCLREGNO AS VCLREGNO")
                '.Append("     , T1.SERVICECODE_S AS SERVICECODE_S")
                '.Append("     , NVL(T1.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE")
                '.Append("     , T4.SVCORGNMCT AS SVCORGNMCT")
                '.Append("     , T4.SVCORGNMCB AS SVCORGNMCB")
                '.Append("     , NVL(T1.UPDATE_COUNT, 0) AS UPDATE_COUNT")
                '.Append("     , T1.STOPFLG AS STOPFLG")
                '.Append("     , NVL(T2.RESULT_STATUS, '  ') AS RESULT_STATUS")
                '.Append("     , NVL(T1.REZ_WORK_TIME, 0) AS REZ_WORK_TIME")
                '.Append("     , T3.SERVICECODE AS SERVICECODE")
                '.Append("     , T1.UPDATEACCOUNT AS UPDATEACCOUNT")
                '.Append("     , T1.VEHICLENAME AS VEHICLENAME")
                '.Append("     , T1.CANCELFLG AS CANCELFLG")
                '.Append("     , T1.UPDATEDATE AS UPDATEDATE")
                '.Append("     , T1.INPUTACCOUNT AS INPUTACCOUNT")
                '.Append("     , T1.MERCHANDISECD AS MERCHANDISECD")
                '.Append("     , T4.SERVICECODE AS SERVICECODE_2")
                '.Append("     , T1.WALKIN AS WALKIN")
                '.Append("     , NVL(T2.UPDATE_COUNT, 0) AS UPDATE_COUNT_2")
                '.Append("     , T1.STOPFLG AS STOPFLG_2")
                '.Append("     , NVL(T2.SEQNO, 0) AS SEQNO")
                '.Append("     , NVL(T2.DSEQNO, 0) AS DSEQNO")
                '.Append("     , NVL(T1.PREZID, -1) AS PREZID")
                '.Append("     , NVL(T1.REZCHILDNO, -1) AS REZCHILDNO")
                '.Append("     , NVL(T2.REZ_END_TIME, '') AS REZ_END_TIME")
                '.Append("     , NVL(T2.REZ_START_TIME, '            ') AS REZ_START_TIME")
                '.Append("     , T1.ACCOUNT_PLAN AS ACCOUNT_PLAN")
                '' 2012/06/01 KN 西田 STEP1 重要課題対応 START
                '.Append("     , T1.INSTRUCT AS INSTRUCT")
                '.Append("     , T1.WORKSEQ AS WORKSEQ")
                '.Append("     , T1.MERCHANDISEFLAG AS MERCHANDISEFLAG")
                '' 2012/06/01 KN 西田 STEP1 重要課題対応 END
                '.Append("  FROM TBL_STALLREZINFO T1")
                '.Append("     , ( SELECT T6.DLRCD")
                '.Append("              , T6.STRCD")
                '.Append("              , T6.REZID")
                '.Append("              , T6.DSEQNO")
                '.Append("              , T6.SEQNO")
                '.Append("              , T6.RESULT_STATUS")
                '.Append("              , T6.UPDATE_COUNT")
                '.Append("              , T6.REZ_END_TIME")
                '.Append("              , T6.REZ_START_TIME")
                '.Append("           FROM TBL_STALLPROCESS T6")
                '.Append("          WHERE T6.DLRCD = :DLRCD1")
                '.Append("            AND T6.STRCD = :STRCD1")
                '.Append("            AND (")
                '.Append("                     T6.RESULT_START_TIME < :RESULT_START_TIME1")
                '.Append("                 AND T6.RESULT_START_TIME >= :RESULT_START_TIME2")
                '.Append("                  OR T6.RESULT_STATUS IN ('0', '00', '10', '11')")
                '.Append("                )")
                '.Append("       ) T2")
                '.Append("     , TBL_STALL T3")
                '.Append("     , TBL_MERCHANDISEMST T4")
                '.Append(" WHERE T1.DLRCD = T2.DLRCD (+)")
                '.Append("   AND T1.STRCD = T2.STRCD (+)")
                '.Append("   AND T1.REZID = T2.REZID (+)")
                '.Append("   AND T1.STALLID  =T3.STALLID (+)")
                '.Append("   AND T1.DLRCD = T4.DLRCD (+)")
                '.Append("   AND T1.MERCHANDISECD = T4.MERCHANDISECD (+)")
                '.Append("   AND T1.DLRCD = :DLRCD2")
                '.Append("   AND T1.STRCD = :STRCD2")
                '.Append("   AND T1.STALLID = :STALLID1")
                '.Append("   AND T1.STATUS <> 3")
                '.Append("   AND ( T2.RESULT_STATUS IN ('0', '00', '10') OR T2.RESULT_STATUS IS NULL ) ")
                '.Append("   AND (")
                '.Append("           T1.ENDTIME >= TO_DATE( :ENDTIME1 , 'YYYY/MM/DD HH24:MI:SS')")
                '.Append("       AND T1.STARTTIME < TO_DATE( :STARTTIME1 , 'YYYY/MM/DD HH24:MI:SS')")
                '.Append("       AND T1.STOPFLG NOT IN ('2', '5', '6')")
                '.Append("       AND T1.CANCELFLG <> '1'")
                '.Append("       AND (")
                '.Append("                T1.ENDTIME >= TO_DATE( :ENDTIME2 , 'YYYY/MM/DD HH24:MI:SS')")
                '.Append("             OR (")
                '.Append("                      T1.STARTTIME = TO_DATE( :STARTTIME2 , 'YYYY/MM/DD HH24:MI:SS')")
                '.Append("                  AND T1.ENDTIME = TO_DATE( :ENDTIME3 , 'YYYY/MM/DD HH24:MI:SS')")
                '.Append("                )")
                '.Append("           )")
                '.Append("       )")
                '.Append("   AND (")
                '.Append("            TO_CHAR(T1.STARTTIME, 'YYYYMMDD') = TO_CHAR(T1.ENDTIME, 'YYYYMMDD')")
                '.Append("         OR (T2.RESULT_STATUS IS NULL OR T2.RESULT_STATUS < 20)")
                '.Append("         OR NOT EXISTS (SELECT T5.REZID")
                '.Append("                          FROM TBL_STALLPROCESS T5")
                '.Append("                WHERE(T5.DLRCD = T2.DLRCD)")
                '.Append("                           AND T5.STRCD = T2.STRCD")
                '.Append("                           AND T5.REZID = T2.REZID")
                '.Append("                           AND T5.DSEQNO = T2.DSEQNO")
                '.Append("                           AND T5.RESULT_START_TIME < :RESULT_START_TIME3")
                '.Append("                           AND T5.RESULT_START_TIME >= :RESULT_START_TIME4)")
                '.Append("       )")
                '' 2012/02/28 上田 SQLインスペクション対応 End

                .Append("SELECT /* SC3150101_019 */ ")
                .Append("       TRIM(T1.DLR_CD) AS DLRCD ")
                .Append("     , TRIM(T1.BRN_CD) AS STRCD ")
                .Append("     , T3.STALL_ID AS STALLID ")
                .Append("     , T3.STALL_USE_ID AS REZID ")
                .Append("     , TRIM(T1.RO_NUM) AS ORDERNO ")
                .Append("     , DECODE(T3.SCHE_START_DATETIME,:MINDATE,TO_DATE(NULL),T3.SCHE_START_DATETIME) AS STARTTIME ")
                .Append("     , DECODE(T3.SCHE_END_DATETIME,:MINDATE,TO_DATE(NULL),T3.SCHE_END_DATETIME)  AS ENDTIME ")
                .Append("     , T1.CST_ID AS INSDID ")
                .Append("     , T1.RESV_STATUS AS STATUS ")
                .Append("     , TRIM(T4.CST_NAME) AS CUSTOMERNAME ")
                .Append("     , TRIM(T1.PICK_DELI_TYPE) AS REZ_RECEPTION ")
                .Append("     , DECODE(T1.SCHE_SVCIN_DATETIME,:MINDATE,TO_DATE(NULL),T1.SCHE_SVCIN_DATETIME) AS CRRYINTIME ")
                .Append("     , DECODE(T1.SCHE_DELI_DATETIME,:MINDATE,TO_DATE(NULL),T1.SCHE_DELI_DATETIME) AS CRRYOUTTIME ")
                .Append("     , T5.REG_NUM AS VCLREGNO ")
                .Append("     , T9.SVC_CLASS_TYPE")
                .Append("     , CASE T9.SVC_CLASS_TYPE")
                .Append("            WHEN :SVC_CLASS_TYPE_1 THEN :C_SERVICECODE_INSPECTION ")
                .Append("            WHEN :SVC_CLASS_TYPE_2 THEN :C_SERVICECODE_PERIODIC ")
                .Append("            WHEN :SVC_CLASS_TYPE_3 THEN :C_SERVICECODE_GENERAL ")
                .Append("            WHEN :SVC_CLASS_TYPE_4 THEN :C_SERVICECODE_NEWCAR ")
                .Append("            ELSE :DEFAULT_VALUE ")
                .Append("       END AS SERVICECODE_S ")
                .Append("     , DECODE(T1.RSLT_SVCIN_DATETIME,:MINDATE,:STRMINDATE,T1.RSLT_SVCIN_DATETIME) AS STRDATE ")
                .Append("     , DECODE(T6.UPPER_DISP,' ',NULL,T6.UPPER_DISP)AS SVCORGNMCT ")
                .Append("     , DECODE(T6.LOWER_DISP,' ',NULL,T6.LOWER_DISP)AS SVCORGNMCB ")
                .Append("     , T1.ROW_LOCK_VERSION AS UPDATE_COUNT ")
                .Append("     , DECODE(T3.STALL_USE_STATUS,:SUS05,:STOPFLG_1,:STOPFLG_0) AS STOPFLG ")
                .Append("     , DECODE(T3.STALL_USE_STATUS,:SUS05,:STOPFLG_1,:STOPFLG_0) AS STOPFLG_2 ")
                .Append("     , T3.STALL_USE_STATUS AS RESULT_STATUS ")
                .Append("     , T3.SCHE_WORKTIME  AS REZ_WORK_TIME ")
                .Append("     , TRIM(T8.SERVICECODE) AS SERVICECODE ")
                .Append("     , T1.ROW_UPDATE_ACCOUNT AS UPDATEACCOUNT ")
                .Append("     , NVL(TRIM(T7.MODEL_NAME),TRIM(T10.NEWCST_MODEL_NAME)) AS VEHICLENAME ")
                .Append("     , TRIM(T2.CANCEL_FLG) AS CANCELFLG ")
                .Append("     , DECODE(T1.UPDATE_DATETIME,:MINDATE,TO_DATE(NULL),T1.UPDATE_DATETIME) AS UPDATEDATE ")
                .Append("     , DECODE(T1.UPDATE_DATETIME,:MINDATE,TO_DATE(NULL),T1.UPDATE_DATETIME) AS UPDATEDATE2 ")
                .Append("     , TRIM(T1.UPDATE_STF_CD) AS INPUTACCOUNT ")
                .Append("     , TRIM(T2.MERC_ID) AS MERCHANDISECD ")
                .Append("     , T6.SVC_CD AS SERVICECODE_2 ")
                .Append("     , TRIM(T1.ACCEPTANCE_TYPE) AS WALKIN ")
                .Append("     , T1.ROW_LOCK_VERSION AS UPDATE_COUNT_2 ")
                .Append("     , 0 AS DSEQNO ")
                .Append("     , T2.JOB_DTL_ID AS SEQNO ")
                .Append("     , T1.SVCIN_ID AS PREZID ")
                .Append("     , ROW_NUMBER() OVER (PARTITION BY T2.JOB_DTL_ID ")
                .Append("                               ORDER BY T1.SVCIN_ID ASC ")
                .Append("                                      ,T2.JOB_DTL_ID ASC ")
                .Append("                           ) AS REZCHILDNO ")
                .Append("     , DECODE(T3.SCHE_END_DATETIME, :MINDATE,NULL, TO_CHAR(T3.SCHE_END_DATETIME, 'YYYY/MM/DD HH24:MI:SS')) AS REZ_END_TIME ")
                .Append("     , DECODE(T3.SCHE_START_DATETIME, :MINDATE,NULL,TO_CHAR(T3.SCHE_START_DATETIME, 'YYYY/MM/DD HH24:MI:SS')) AS REZ_START_TIME ")
                .Append("     , TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                .Append("     , DECODE(T3.STALL_USE_STATUS,:SUS00,:CN_INSTRUCT_0,:SUS07,:CN_INSTRUCT_0,:CN_INSTRUCT_2) AS INSTRUCT ")
                .Append("     , 0 AS WORKSEQ ")
                .Append("     , 0 AS MERCHANDISEFLAG ")
                '2014/08/29 TMEJ 成澤 【IT9745】NextSTEPサービス サービス業務向け評価用アプリのシステムテスト  START
                .Append("     , T11.PARKINGCODE ")
                '2014/08/29 TMEJ 成澤 【IT9745】NextSTEPサービス サービス業務向け評価用アプリのシステムテスト  END
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .Append("     , NVL(TRIM(T5.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                .Append("  FROM TB_T_SERVICEIN T1 ")
                .Append("     , TB_T_JOB_DTL T2 ")
                .Append("     , TB_T_STALL_USE T3 ")
                .Append("     , TB_M_CUSTOMER T4 ")
                .Append("     , TB_M_VEHICLE_DLR T5 ")
                .Append("     , TB_M_MERCHANDISE T6 ")
                .Append("     , TB_M_MODEL T7 ")
                .Append("     , TBL_STALL T8 ")
                .Append("     , TB_M_SERVICE_CLASS T9 ")
                .Append("     , TB_M_VEHICLE T10 ")
                '2014/08/29 TMEJ 成澤 【IT9745】NextSTEPサービス サービス業務向け評価用アプリのシステムテスト  START
                .Append("     , TBL_SERVICE_VISIT_MANAGEMENT T11 ")
                '2014/08/29 TMEJ 成澤 【IT9745】NextSTEPサービス サービス業務向け評価用アプリのシステムテスト  END
                .Append(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .Append("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .Append("   AND T1.CST_ID = T4.CST_ID (+) ")
                .Append("   AND T1.VCL_ID = T5.VCL_ID (+) ")
                .Append("   AND T2.MERC_ID = T6.MERC_ID (+) ")
                .Append("   AND T8.STALLID = T3.STALL_ID (+) ")
                .Append("   AND T2.SVC_CLASS_ID = T9.SVC_CLASS_ID (+)")
                .Append("   AND T1.VCL_ID = T10.VCL_ID (+) ")
                .Append("   AND T10.MODEL_CD = T7.MODEL_CD (+) ")
                '2014/08/29 TMEJ 成澤 【IT9745】NextSTEPサービス サービス業務向け評価用アプリのシステムテスト  START
                .Append("   AND T1.SVCIN_ID = T11.FREZID (+) ")
                .Append("   AND T1.DLR_CD = T11.DLRCD (+) ")
                .Append("   AND T1.BRN_CD = T11.STRCD (+)")
                '2014/08/29 TMEJ 成澤 【IT9745】NextSTEPサービス サービス業務向け評価用アプリのシステムテスト  END
                .Append("   AND T1.DLR_CD =:DLR_CD ")
                .Append("   AND T1.BRN_CD = :BRN_CD ")
                .Append("   AND T3.DLR_CD =:DLR_CD ")
                .Append("   AND T3.BRN_CD = :BRN_CD ")
                .Append("   AND T3.STALL_ID = :STALL_ID ")
                .Append("   AND T5.DLR_CD =:DLR_CD ")
                .Append("   AND T3.STALL_USE_STATUS IN (:SUS00,:SUS01) ")
                .Append("   AND ( ")
                .Append("          T3.SCHE_END_DATETIME >= :SCHE_END_DATETIME1 ")
                .Append("      AND T3.SCHE_START_DATETIME < :SCHE_START_DATETIME1 ")
                .Append("      AND T3.STALL_USE_STATUS <> :SUS05 ")
                .Append("      AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .Append("      AND T3.TEMP_FLG <> :TEMP_FLG ")
                .Append("      AND ( ")
                .Append("            T3.SCHE_END_DATETIME >= :SCHE_END_DATETIME2 ")
                .Append("            OR ( ")
                .Append("                    T3.SCHE_START_DATETIME = :SCHE_START_DATETIME2 ")
                .Append("                AND T3.SCHE_END_DATETIME = :SCHE_END_DATETIME3 ")
                .Append("                ) ")
                .Append("           ) ")
                .Append("      ) ")
                .Append("   AND (   TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDD') = TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDD') ")
                .Append("       OR T3.STALL_USE_STATUS IN (:SUS00,:SUS01) ")
                .Append("       OR NOT EXISTS (SELECT T11.JOB_DTL_ID ")
                .Append("                        FROM TB_T_STALL_USE T11 ")
                .Append("                       WHERE T11.DLR_CD = T3.DLR_CD ")
                .Append("                         AND T11.BRN_CD = T3.BRN_CD ")
                .Append("                         AND T11.RSLT_START_DATETIME < :RSLT_START_DATETIME1 ")
                .Append("                         AND T11.RSLT_START_DATETIME >= :RSLT_START_DATETIME2 ")
                .Append("                     ) ")
                .Append("      ) ")
            End With
            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ReserveChipInfoDataTable)("SC3150101_019")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                ''Dim workTimeFromString As String = dateFrom.ToString("yyyyMMddHHmmss") ' 稼働時間From
                ''Dim workTimeToString As String = dateTo.ToString("yyyyMMddHHmmss")     ' 稼働時間To
                'Dim workTimeFromString As String = dateFrom.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())   ' 稼働時間From
                'Dim workTimeToString As String = dateTo.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())       ' 稼働時間To
                'Dim workTimeFrom As String = dateFrom.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())  ' 稼働時間From
                'Dim workTimeTo As String = dateTo.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())      ' 稼働時間To
                'Dim workTimeZeroFrom As String = SetSearchDate(dateFrom)               ' 稼働時間Fromの日付部分＋00:00:00
                'query.AddParameterWithTypeValue("DLRCD1", OracleDbType.Char, dealerCode)                     ' 販売店コード
                'query.AddParameterWithTypeValue("STRCD1", OracleDbType.Char, branchCode)                     ' 店舗コード
                'query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, workTimeToString)   ' 稼働時間To
                'query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, workTimeFromString) ' 稼働時間From
                'query.AddParameterWithTypeValue("DLRCD2", OracleDbType.Char, dealerCode)                     ' 販売店コード
                'query.AddParameterWithTypeValue("STRCD2", OracleDbType.Char, branchCode)                     ' 店舗コード
                'query.AddParameterWithTypeValue("STALLID1", OracleDbType.Int64, stallId)                     ' ストールID
                'query.AddParameterWithTypeValue("ENDTIME1", OracleDbType.Char, workTimeZeroFrom)             ' 稼働時間Fromの日付部分＋00:00:00
                'query.AddParameterWithTypeValue("STARTTIME1", OracleDbType.Char, workTimeTo)                 ' 稼働時間To
                'query.AddParameterWithTypeValue("ENDTIME2", OracleDbType.Char, workTimeFrom)                 ' 稼働時間From
                'query.AddParameterWithTypeValue("STARTTIME2", OracleDbType.Char, workTimeZeroFrom)           ' 稼働時間Fromの日付部分＋00:00:00
                'query.AddParameterWithTypeValue("ENDTIME3", OracleDbType.Char, workTimeZeroFrom)             ' 稼働時間Fromの日付部分＋00:00:00
                'query.AddParameterWithTypeValue("RESULT_START_TIME3", OracleDbType.Char, workTimeToString)   ' 稼働時間To
                'query.AddParameterWithTypeValue("RESULT_START_TIME4", OracleDbType.Char, workTimeFromString) ' 稼働時間From
                '' 2012/02/28 上田 SQLインスペクション対応 Start 
                ''query.AddParameterWithTypeValue("DLRCD3", OracleDbType.Char, dealerCode)                     ' 販売店コード
                ''query.AddParameterWithTypeValue("DLRCD4", OracleDbType.Char, dealerCode)                     ' 販売店コード
                '' 2012/02/28 上田 SQLインスペクション対応 End 
                '' 入庫日時の仮デフォルト値として設定
                'query.AddParameterWithTypeValue("MINDATE1", OracleDbType.Char, DateTime.MinValue.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                Dim workTimeFromString As String = dateFrom.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) ' 稼働時間From
                Dim workTimeToString As String = dateTo.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)   ' 稼働時間To
                Dim workTimeFrom As String = dateFrom.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)   ' 稼働時間From
                Dim workTimeTo As String = dateTo.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)     ' 稼働時間To
                Dim workTimeZeroFrom As String = SetSearchDate(dateFrom)               ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)                     ' ストールID
                query.AddParameterWithTypeValue("STOPFLG_0", OracleDbType.NVarchar2, STOPFLG_0)
                query.AddParameterWithTypeValue("STOPFLG_1", OracleDbType.NVarchar2, STOPFLG_1)
                query.AddParameterWithTypeValue("CN_INSTRUCT_2", OracleDbType.NVarchar2, CN_INSTRUCT_2)
                query.AddParameterWithTypeValue("CN_INSTRUCT_0", OracleDbType.NVarchar2, CN_INSTRUCT_0)
                query.AddParameterWithTypeValue("SUS00", OracleDbType.NVarchar2, stallUseStetus00)
                query.AddParameterWithTypeValue("SUS01", OracleDbType.NVarchar2, stallUseStetus01)
                query.AddParameterWithTypeValue("SUS05", OracleDbType.NVarchar2, stallUseStetus05)
                query.AddParameterWithTypeValue("SUS07", OracleDbType.NVarchar2, stallUseStetus07)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_1", OracleDbType.NVarchar2, SVC_CLASS_TYPE_1)
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_2", OracleDbType.NVarchar2, SVC_CLASS_TYPE_2)
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_3", OracleDbType.NVarchar2, SVC_CLASS_TYPE_3)
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_4", OracleDbType.NVarchar2, SVC_CLASS_TYPE_4)
                query.AddParameterWithTypeValue("C_SERVICECODE_INSPECTION", OracleDbType.NVarchar2, C_SERVICECODE_INSPECTION)
                query.AddParameterWithTypeValue("C_SERVICECODE_PERIODIC", OracleDbType.NVarchar2, C_SERVICECODE_PERIODIC)
                query.AddParameterWithTypeValue("C_SERVICECODE_GENERAL", OracleDbType.NVarchar2, C_SERVICECODE_GENERAL)
                query.AddParameterWithTypeValue("C_SERVICECODE_NEWCAR", OracleDbType.NVarchar2, C_SERVICECODE_NEWCAR)
                query.AddParameterWithTypeValue("SCHE_END_DATETIME1", OracleDbType.Date, Date.Parse(workTimeZeroFrom, CultureInfo.InvariantCulture))             ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("SCHE_START_DATETIME1", OracleDbType.Date, Date.Parse(workTimeTo, CultureInfo.InvariantCulture))                 ' 稼働時間To
                query.AddParameterWithTypeValue("SCHE_END_DATETIME2", OracleDbType.Date, Date.Parse(workTimeFrom, CultureInfo.InvariantCulture))                 ' 稼働時間From
                query.AddParameterWithTypeValue("SCHE_START_DATETIME2", OracleDbType.Date, Date.Parse(workTimeZeroFrom, CultureInfo.InvariantCulture))           ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("SCHE_END_DATETIME3", OracleDbType.Date, Date.Parse(workTimeZeroFrom, CultureInfo.InvariantCulture))             ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("RSLT_START_DATETIME1", OracleDbType.Date, Date.Parse(workTimeToString, CultureInfo.InvariantCulture))   ' 稼働時間To
                query.AddParameterWithTypeValue("RSLT_START_DATETIME2", OracleDbType.Date, Date.Parse(workTimeFromString, CultureInfo.InvariantCulture)) ' 稼働時間From
                query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, TEMP_FLG)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("STRMINDATE", OracleDbType.Date, Date.Parse(strMinDate, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("DEFAULT_VALUE", OracleDbType.NVarchar2, DEFAULT_VALUE)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, ICON_OFF_FLAG)
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                Logger.Info("[E]GetReserveChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 実績チップ情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <param name="dateTo">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応
        ''' </History>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetResultChipInfo(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal stallId As Decimal, _
                                          ByVal dateFrom As Date, _
                                          ByVal dateTo As Date) As SC3150101DataSet.SC3150101ResultChipInfoDataTable

            Logger.Info("[S]GetResultChipInfo()")

            Dim sql As New StringBuilder


            ' SQL文の作成
            With sql
                ' 2012/02/28 上田 SQLインスペクション対応 Start 
                '.Append("WITH vREZINFO as ( /* SC3150101_020 */ ")
                '.Append("    SELECT T1.DLRCD, ")
                '.Append("           T1.STRCD, ")
                '.Append("           T1.REZID, ")
                '.Append("           T1.SEQNO, ")
                '.Append("           T1.DSEQNO , ")
                '.Append("           NVL(T2.PREZID,'') AS PREZID, ")
                '.Append("           NVL(T2.REZCHILDNO,'') AS REZCHILDNO, ")
                '.Append("           T1.REZ_START_TIME,  ")
                '.Append("           T1.REZ_END_TIME, ")
                '.Append("           T1.RESULT_STALLID, ")
                '.Append("           T2.INSDID, ")
                '.Append("           T2.STATUS, ")
                '.Append("           T2.CUSTOMERNAME, ")
                '.Append("           T1.REZ_RECEPTION, ")
                '.Append("           T1.REZ_PICK_DATE, ")
                '.Append("           T1.REZ_DELI_DATE, ")
                '.Append("           T1.MODELCODE, ")
                '.Append("           T1.VCLREGNO, ")
                '.Append("           T1.SERVICECODE, ")
                '.Append("           T2.STRDATE, ")
                '.Append("           T4.SVCORGNMCT, ")
                '.Append("           T4.SVCORGNMCB, ")
                '.Append("           T1.UPDATE_COUNT, ")
                '.Append("           T1.RESULT_STATUS, ")
                '.Append("           T1.REZ_WORK_TIME, ")
                '.Append("           NVL(T1.RESULT_START_TIME, ' ') AS RESULT_START_TIME, ")
                '.Append("           T1.RESULT_END_TIME, ")
                '.Append("           T1.REZ_PICK_TIME, ")
                '.Append("           T1.REZ_DELI_TIME, ")
                '.Append("           T1.INPUTACCOUNT, ")
                '.Append("           T1.RESULT_IN_TIME, ")
                '.Append("           T1.RESULT_WAIT_END, ")
                '.Append("           T2.VEHICLENAME, ")
                '.Append("           T1.UPDATEDATE, ")
                '.Append("           T2.CANCELFLG, ")
                '.Append("           T2.STOPFLG, ")
                '.Append("           T2.STARTTIME, ")
                '.Append("           T2.ENDTIME, ")
                '.Append("           T2.REZ_WORK_TIME AS REZ_WORK_TIME_2, ")
                '.Append("           T2.UPDATEACCOUNT, ")
                '.Append("           T1.RESULT_WORK_TIME, ")
                '.Append("           T1.VIN, ")
                '.Append("           T4.SERVICECODE AS SERVICECODE_MST, ")
                '.Append("           T2.WALKIN, ")
                '.Append("           T2.ACCOUNT_PLAN ")
                '.Append("         , T2.ORDERNO ")
                '.Append("      FROM tbl_STALLPROCESS T1 ")
                '.Append("INNER JOIN (SELECT TT.DLRCD, ")
                '.Append("                   TT.STRCD, ")
                '.Append("                   TT.REZID, ")
                '.Append("                   TT.INSDID, ")
                '.Append("                   TT.STATUS, ")
                '.Append("                   TT.CUSTOMERNAME, ")
                '.Append("                   TT.STRDATE, ")
                '.Append("                   TT.VEHICLENAME, ")
                '.Append("                   TT.CANCELFLG, ")
                '.Append("                   TT.STOPFLG, ")
                '.Append("                   TT.STARTTIME, ")
                '.Append("                   TT.ENDTIME, ")
                '.Append("                   TT.REZ_WORK_TIME, ")
                '.Append("                   TT.UPDATEACCOUNT, ")
                '.Append("                   TT.PREZID, ")
                '.Append("                   TT.REZCHILDNO, ")
                '.Append("                   TT.VIN, ")
                '.Append("                   TT.WALKIN, ")
                '.Append("                   TT.ACCOUNT_PLAN ")
                '.Append("                 , TT.ORDERNO ")
                '.Append("              FROM TBL_STALLREZINFO TT ")
                '.Append("             WHERE TT.DLRCD = :DLRCD1 ") '''''販売店コード
                '.Append("               AND TT.STRCD = :STRCD1 ") '''''店舗コード
                '.Append("           ) T2 ")
                '.Append("        ON T2.DLRCD = T1.DLRCD ")
                '.Append("       AND T2.STRCD = T1.STRCD ")
                '.Append("       AND T2.REZID = T1.REZID ")
                '.Append(" LEFT JOIN TBL_MERCHANDISEMST T4 ")
                '.Append("        ON T4.DLRCD = T1.DLRCD ")
                '.Append("       AND T4.MERCHANDISECD = T1.MERCHANDISECD ")
                '.Append("     WHERE T1.DLRCD = :DLRCD2 ") '''''販売店コード
                '.Append("       AND T1.STRCD = :STRCD2 ") '''''店舗コード
                '.Append("       AND T1.RESULT_STALLID = :RESULT_STALLID1 ") '''''ストールID
                '.Append("       AND T1.RESULT_STATUS NOT IN ('0', '00', '10', '32', '33') ")
                '.Append("       AND ( ( T2.STATUS <> '0' ")
                '.Append("              AND T1.RESULT_START_TIME >= :RESULT_START_TIME1 ") '''''稼働時間From
                '.Append("              AND T1.RESULT_START_TIME < :RESULT_START_TIME2 ") '''''稼働時間To
                '.Append("             ) ")
                '.Append("            OR ( T1.RESULT_START_TIME < :RESULT_START_TIME3 ") '''''稼働時間To
                '.Append("                AND T1.RESULT_STATUS IN ('30', '31', '38', '39' , '42', '43', '44') ")
                '.Append("                AND T1.SEQNO = ( SELECT MAX(T6.SEQNO) ")
                '.Append("                                   FROM TBL_STALLPROCESS T6 ")
                '.Append("                                  WHERE T6.DLRCD = T1.DLRCD ")
                '.Append("                                    AND T6.STRCD = T1.STRCD ")
                '.Append("                                    AND T6.REZID = T1.REZID ")
                '.Append("                                    AND T6.DSEQNO = T1.DSEQNO ")
                '.Append("                               ) ")
                '.Append("               ) ")
                '.Append("           ) ")
                '.Append("       AND ( T2.CANCELFLG <> '1' ")
                '.Append("            OR T2.STOPFLG IN ('1', '2', '5', '6') ")
                '.Append("           ) ")
                '.Append("        OR (T1.RESULT_STATUS = '11' ")
                '.Append("            AND T1.REZ_START_TIME = :REZ_START_TIME1 ") '''''稼働時間Fromの日付部分＋0000
                '.Append("            AND ( T2.CANCELFLG <> '1' ")
                '.Append("                 OR T2.STOPFLG IN ('1', '2', '5', '6') ")
                '.Append("                ) ")
                '.Append("           ) ")
                '.Append(") ")
                '.Append("   SELECT MT.REZID AS REZID, ")
                '.Append("          MT.SEQNO AS SEQNO, ")
                '.Append("          MT.DSEQNO AS DSEQNO, ")
                ''.Append("          MT.PREZID AS PREZID, ")
                '.Append("          NVL(MT.PREZID, -1) AS PREZID, ")
                ''.Append("          MT.REZCHILDNO AS REZCHILDNO, ")
                '.Append("          NVL(MT.REZCHILDNO, -1) AS REZCHILDNO, ")
                '.Append("          MT.REZ_START_TIME AS REZ_START_TIME, ")
                '.Append("          MT.REZ_END_TIME AS REZ_END_TIME, ")
                '.Append("          MT.RESULT_STALLID AS RESULT_STALLID, ")
                '.Append("          MT.INSDID AS INSDID, ")
                '.Append("          MT.STATUS AS STATUS, ")
                '.Append("          MT.CUSTOMERNAME AS CUSTOMERNAME, ")
                '.Append("          MT.REZ_RECEPTION AS REZ_RECEPTION, ")
                '.Append("          MT.REZ_PICK_DATE AS REZ_PICK_DATE, ")
                '.Append("          MT.REZ_DELI_DATE AS REZ_DELI_DATE, ")
                '.Append("          MT.MODELCODE AS MODELCODE, ")
                '.Append("          MT.VCLREGNO AS VCLREGNO, ")
                '.Append("          MT.SERVICECODE AS SERVICECODE, ")
                ''.Append("          MT.STRDATE AS STRDATE, ")
                '.Append("          NVL(MT.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE, ")
                '.Append("          MT.SVCORGNMCT AS SVCORGNMCT, ")
                '.Append("          MT.SVCORGNMCB AS SVCORGNMCB, ")
                '.Append("          MT.UPDATE_COUNT AS UPDATE_COUNT, ")
                '.Append("          MT.RESULT_STATUS AS RESULT_STATUS, ")
                ''.Append("          MT.REZ_WORK_TIME AS REZ_WORK_TIME, ")
                '.Append("          NVL(MT.REZ_WORK_TIME, 0) AS REZ_WORK_TIME, ")
                '.Append("          MT.RESULT_START_TIME AS RESULT_START_TIME, ")
                '.Append("          MT.RESULT_END_TIME AS RESULT_END_TIME, ")
                '.Append("          MT.REZ_PICK_TIME AS REZ_PICK_TIME, ")
                '.Append("          MT.REZ_DELI_TIME AS REZ_DELI_TIME, ")
                '.Append("          MT.INPUTACCOUNT AS INPUTACCOUNT, ")
                '.Append("          MT.RESULT_IN_TIME AS RESULT_IN_TIME, ")
                '.Append("          MT.RESULT_WAIT_END AS RESULT_WAIT_END, ")
                '.Append("          MT.VEHICLENAME AS VEHICLENAME, ")
                '.Append("          MT.UPDATEDATE AS UPDATEDATE, ")
                '.Append("          MT.CANCELFLG AS CANCELFLG, ")
                '.Append("          MT.STOPFLG AS STOPFLG, ")
                '.Append("          MT.STARTTIME AS STARTTIME, ")
                '.Append("          MT.ENDTIME AS ENDTIME, ")
                ''.Append("          MT.REZ_WORK_TIME_2 AS REZ_WORK_TIME_2, ")
                '.Append("          NVL(MT.REZ_WORK_TIME_2, 0) AS REZ_WORK_TIME_2, ")
                '.Append("          MT.UPDATEACCOUNT AS UPDATEACCOUNT, ")
                ''.Append("          MT.RESULT_WORK_TIME AS RESULT_WORK_TIME, ")
                '.Append("          NVL(MT.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME, ")
                '.Append("          MT.SERVICECODE_MST AS SERVICECODE_MST, ")
                '.Append("          MT.WALKIN AS WALKIN, ")
                '.Append("          MT.ACCOUNT_PLAN AS ACCOUNT_PLAN, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT T20.RESULT_STATUS ")
                '.Append("                                                      FROM TBL_STALLREZINFO T10 ")
                '.Append("                                                INNER JOIN TBL_STALLPROCESS T20 ")
                '.Append("                                                        ON T10.DLRCD = T20.DLRCD ")
                '.Append("                                                       AND T10.STRCD = T20.STRCD ")
                '.Append("                                                       AND T10.REZID = T20.REZID ")
                '.Append("                                                     WHERE T10.DLRCD = MT.DLRCD ")
                '.Append("                                                       AND T10.STRCD = MT.STRCD ")
                '.Append("                                                       AND T10.PREZID = MT.PREZID ")
                '.Append("                                                       AND T10.REZCHILDNO > 0 ")
                '.Append("                                                       AND T10.REZCHILDNO < 999 ")
                '.Append("                                                       AND NOT T20.RESULT_STATUS IS NULL ")
                '.Append("                                                       AND T20.RESULT_STATUS NOT IN('00','01','10','11','32','33','34') ")
                '.Append("                                                       AND ROWNUM = 1 ")
                '.Append("                                                   ),'0') ")
                '.Append("               ELSE '0' ")
                '.Append("           END) AS RELATIONSTATUS, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT COUNT(1) ")
                '.Append("                                                      FROM TBL_STALLREZINFO T11 ")
                '.Append("                                           LEFT OUTER JOIN TBL_STALLPROCESS T21 ")
                '.Append("                                                        ON T11.DLRCD = T21.DLRCD ")
                '.Append("                                                       AND T11.STRCD = T21.STRCD ")
                '.Append("                                                       AND T11.REZID = T21.REZID ")
                '.Append("                                                     WHERE T11.DLRCD = MT.DLRCD ")
                '.Append("                                                       AND T11.STRCD = MT.STRCD ")
                '.Append("                                                       AND T11.PREZID = MT.PREZID ")
                '.Append("                                                       AND T11.REZCHILDNO > 0 ")
                '.Append("                                                       AND T11.REZCHILDNO < 999 ")
                '.Append("                                                       AND NOT (T11.CANCELFLG = '1' AND T11.STOPFLG = '0') ")
                '.Append("                                                       AND (T21.RESULT_STATUS IS NULL ")
                '.Append("                                                            OR T21.RESULT_STATUS NOT IN ('97','99') ")
                '.Append("                                                           ) ")
                '.Append("                                                       AND (T21.DSEQNO IS NULL ")
                '.Append("                                                            OR T21.DSEQNO = (SELECT MAX(T22.DSEQNO) ")
                '.Append("                                                                               FROM TBL_STALLPROCESS T22 ")
                '.Append("                                                                              WHERE T22.DLRCD = T21.DLRCD ")
                '.Append("                                                                                AND T22.STRCD = T21.STRCD ")
                '.Append("                                                                                AND T22.REZID = T21.REZID ")
                '.Append("                                                                            ) ")
                '.Append("                                                           ) ")
                '.Append("                                                       AND (T21.SEQNO IS NULL ")
                '.Append("                                                            OR T21.SEQNO = (SELECT MAX(T23.SEQNO) ")
                '.Append("                                                                              FROM TBL_STALLPROCESS T23 ")
                '.Append("                                                                             WHERE T23.DLRCD = T21.DLRCD ")
                '.Append("                                                                               AND T23.STRCD = T21.STRCD ")
                '.Append("                                                                               AND T23.REZID = T21.REZID ")
                '.Append("                                                                               AND T23.DSEQNO = T21.DSEQNO ")
                '.Append("                                                                           )")
                '.Append("                                                           ) ")
                '.Append("                                                   ), 0) ")
                '.Append("               ELSE 0 ")
                '.Append("           END) AS RELATION_UNFINISHED_COUNT ")
                '.Append("        , MT.ORDERNO AS ORDERNO ")
                '.Append("     FROM vREZINFO MT ")
                '.Append("LEFT JOIN TBL_SMBVCLINFO OV ")
                '.Append("       ON OV.DLRCD = :DLRCD5 ") '''''販売店コード
                '.Append("      AND MT.INSDID = OV.ORIGINALID ")
                '.Append("      AND MT.VIN = OV.VIN ")
                '.Append("      AND MT.VCLREGNO = OV.VCLREGNO ")
                '.Append("LEFT JOIN TBL_SMBCUSTOMER NC ")
                '.Append("       ON NC.DLRCD = :DLRCD6 ") '''''販売店コード
                '.Append("      AND MT.INSDID = NC.ORIGINALID ")

                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                '.Append("SELECT /* SC3150101_020 */")
                '.Append("       T1.REZID AS REZID")
                '.Append("     , NVL(T1.SEQNO, 0) AS SEQNO")
                '.Append("     , NVL(T1.DSEQNO, 0) AS DSEQNO")
                '.Append("     , NVL(T2.PREZID, -1) AS PREZID")
                '.Append("     , NVL(T2.REZCHILDNO, -1) AS REZCHILDNO")
                '.Append("     , T1.REZ_START_TIME AS REZ_START_TIME")
                '.Append("     , T1.REZ_END_TIME AS REZ_END_TIME")
                '.Append("     , T1.RESULT_STALLID AS RESULT_STALLID")
                '.Append("     , T2.INSDID AS INSDID")
                '.Append("     , T2.STATUS AS STATUS")
                '.Append("     , T2.CUSTOMERNAME AS CUSTOMERNAME")
                '.Append("     , T1.REZ_RECEPTION AS REZ_RECEPTION")
                '.Append("     , T1.REZ_PICK_DATE AS REZ_PICK_DATE")
                '.Append("     , T1.REZ_DELI_DATE AS REZ_DELI_DATE")
                '.Append("     , T1.MODELCODE AS MODELCODE")
                '.Append("     , T1.VCLREGNO AS VCLREGNO")
                '.Append("     , T1.SERVICECODE AS SERVICECODE")
                '.Append("     , NVL(T2.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE")
                '.Append("     , T3.SVCORGNMCT AS SVCORGNMCT")
                '.Append("     , T3.SVCORGNMCB AS SVCORGNMCB")
                '.Append("     , T1.UPDATE_COUNT AS UPDATE_COUNT")
                '.Append("     , T1.RESULT_STATUS AS RESULT_STATUS")
                '.Append("     , NVL(T1.REZ_WORK_TIME, 0) AS REZ_WORK_TIME")
                '.Append("     , T1.RESULT_START_TIME AS RESULT_START_TIME")
                '.Append("     , T1.RESULT_END_TIME AS RESULT_END_TIME")
                '.Append("     , T1.REZ_PICK_TIME AS REZ_PICK_TIME")
                '.Append("     , T1.REZ_DELI_TIME AS REZ_DELI_TIME")
                '.Append("     , T1.INPUTACCOUNT AS INPUTACCOUNT")
                '.Append("     , T1.RESULT_IN_TIME AS RESULT_IN_TIME")
                '.Append("     , T1.RESULT_WAIT_END AS RESULT_WAIT_END")
                '.Append("     , T2.VEHICLENAME AS VEHICLENAME")
                '.Append("     , T1.UPDATEDATE AS UPDATEDATE")
                '.Append("     , T2.CANCELFLG AS CANCELFLG")
                '.Append("     , T2.STOPFLG AS STOPFLG")
                '.Append("     , T2.STARTTIME AS STARTTIME")
                '.Append("     , T2.ENDTIME AS ENDTIME")
                '.Append("     , NVL(T2.REZ_WORK_TIME, 0) AS REZ_WORK_TIME_2")
                '.Append("     , T2.UPDATEACCOUNT AS UPDATEACCOUNT")
                '.Append("     , NVL(T1.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME")
                '.Append("     , T3.SERVICECODE AS SERVICECODE_MST")
                '.Append("     , T2.WALKIN AS WALKIN")
                '.Append("     , T2.ACCOUNT_PLAN AS ACCOUNT_PLAN")
                '.Append("     , T2.ORDERNO AS ORDERNO")
                '' 2012/06/01 KN 西田 STEP1 重要課題対応 START
                '.Append("     , T2.INSTRUCT AS INSTRUCT")
                '.Append("     , T2.WORKSEQ AS WORKSEQ")
                '.Append("     , T2.MERCHANDISEFLAG AS MERCHANDISEFLAG")
                '' 2012/06/01 KN 西田 STEP1 重要課題対応 END

                '' 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 START
                '.Append("     , NVL(T2.INSPECTIONREQFLG, '0') AS INSPECTIONREQFLG ")
                '' 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 END

                '.Append("  FROM TBL_STALLPROCESS T1")
                '.Append("     , TBL_STALLREZINFO T2")
                '.Append("     , TBL_MERCHANDISEMST T3")
                '.Append(" WHERE T1.DLRCD = T2.DLRCD")
                '.Append("   AND T1.STRCD = T2.STRCD")
                '.Append("   AND T1.REZID = T2.REZID")
                '.Append("   AND T1.DLRCD = T3.DLRCD (+)")
                '.Append("   AND T1.MERCHANDISECD = T3.MERCHANDISECD (+)")
                '.Append("   AND T1.DLRCD = :DLRCD1")
                '.Append("   AND T1.STRCD = :STRCD1")
                '.Append("   AND T1.RESULT_STALLID = :RESULT_STALLID1")
                '.Append("   AND T1.RESULT_STATUS NOT IN ('0', '00', '10', '32', '33')")
                ''2012/03/09 上田 サブエリア表示分取得削除 START
                '.Append("   AND T2.STATUS <> '0'")
                '.Append("   AND T1.RESULT_START_TIME >= :RESULT_START_TIME1")
                '.Append("   AND T1.RESULT_START_TIME < :RESULT_START_TIME2")
                ''.Append("   AND (")
                ''.Append("         (")
                ''.Append("             T2.STATUS <> '0'")
                ''.Append("         AND T1.RESULT_START_TIME >= :RESULT_START_TIME1")
                ''.Append("         AND T1.RESULT_START_TIME < :RESULT_START_TIME2")
                ''.Append("         )")
                ''.Append("         OR")
                ''.Append("         (")
                ''.Append("             T1.RESULT_START_TIME < :RESULT_START_TIME3")
                ''.Append("         AND T1.RESULT_STATUS IN ('30', '31', '38', '39' , '42', '43', '44')")
                ''.Append("         AND T1.SEQNO = ( SELECT MAX(T4.SEQNO)")
                ''.Append("                            FROM TBL_STALLPROCESS T4")
                ''.Append("                WHERE(T4.DLRCD = T1.DLRCD)")
                ''.Append("                             AND T4.STRCD = T1.STRCD")
                ''.Append("                             AND T4.REZID = T1.REZID")
                ''.Append("                             AND T4.DSEQNO = T1.DSEQNO")
                ''.Append("                        )")
                ''.Append("         )")
                ''.Append("       )")
                ''2012/03/09 上田 サブエリア表示分取得削除 END
                '.Append("   AND ")
                '.Append("       (")
                '.Append("        (")
                '.Append("            T2.CANCELFLG <> '1'")
                '.Append("         OR T2.STOPFLG IN ('1', '2', '5', '6')")
                '.Append("        )")
                '.Append("       OR ")
                '.Append("        (")
                '.Append("            T1.RESULT_STATUS = '11'")
                '.Append("        AND T1.REZ_START_TIME = :REZ_START_TIME1")
                '.Append("        AND (")
                '.Append("                 T2.CANCELFLG <> '1'")
                '.Append("              OR T2.STOPFLG IN ('1', '2', '5', '6')")
                '.Append("            )")
                '.Append("        )")
                '.Append("       )")

                .Append("SELECT /* SC3150101_020 */ ")
                .Append("       T2.JOB_DTL_ID AS SEQNO ")
                .Append("     , 0 AS DSEQNO ")
                .Append("     , T1.STALL_ID AS RESULT_STALLID ")
                .Append("     , 0 AS MERCHANDISEFLAG ")
                .Append("     , TRIM(T1.STALL_USE_STATUS) AS STALL_USE_STATUS ")
                .Append("     , DECODE(T1.STALL_USE_STATUS,:SUS00,:CN_INSTRUCT_0,:SUS07,:CN_INSTRUCT_0,:CN_INSTRUCT_2) AS INSTRUCT  ")
                .Append("     , DECODE(T1.SCHE_START_DATETIME, :MINDATE, NULL, TO_CHAR(T1.SCHE_START_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_START_TIME ")
                .Append("     , DECODE(T1.PRMS_END_DATETIME, :MINDATE, NULL, TO_CHAR(T1.PRMS_END_DATETIME, 'YYYYMMDDHH24MI')) AS PRMS_END_DATETIME ")
                .Append("     , DECODE(T1.SCHE_END_DATETIME, :MINDATE, NULL, TO_CHAR(T1.SCHE_END_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_END_TIME ")
                .Append("     , DECODE(T1.SCHE_START_DATETIME,:MINDATE,TO_DATE(NULL),T1.SCHE_START_DATETIME) AS STARTTIME ")
                .Append("     , DECODE(T1.SCHE_END_DATETIME,:MINDATE,TO_DATE(NULL),T1.SCHE_END_DATETIME)  AS ENDTIME ")
                .Append("     , T1.SCHE_WORKTIME AS REZ_WORK_TIME ")
                .Append("     , T1.SCHE_WORKTIME AS REZ_WORK_TIME_2 ")
                .Append("     , DECODE(T1.RSLT_START_DATETIME, :MINDATE, NULL, TO_CHAR(T1.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_START_TIME ")
                .Append("     , DECODE(T1.RSLT_END_DATETIME, :MINDATE, NULL, TO_CHAR(T1.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_END_TIME ")
                .Append("     , T1.RSLT_WORKTIME AS RESULT_WORK_TIME ")
                .Append("     , TRIM(T1.CREATE_STF_CD) AS INPUTACCOUNT ")
                .Append("     , T3.ROW_LOCK_VERSION AS UPDATE_COUNT ")
                .Append("     , 0 AS WORKSEQ ")
                .Append("     , T1.STALL_USE_ID AS REZID ")
                .Append("     , T3.SVCIN_ID AS PREZID ")
                .Append("     , T2.MERC_ID AS SERVICECODE_MST ")
                .Append("     , ROW_NUMBER() OVER (PARTITION BY T2.JOB_DTL_ID ")
                .Append("                          ORDER BY T3.SVCIN_ID ASC ")
                .Append("                        , T2.JOB_DTL_ID ASC ")
                .Append("                          ) AS REZCHILDNO ")
                .Append("     , DECODE(T2.UPDATE_DATETIME, :MINDATE, TO_DATE(NULL), T2.UPDATE_DATETIME) AS UPDATEDATE ")
                .Append("     , TRIM(T2.UPDATE_STF_CD) AS UPDATEACCOUNT ")
                .Append("     , TRIM(T3.RO_NUM) AS ORDERNO ")
                .Append("     , TO_CHAR(T3.CST_ID) AS INSDID ")
                .Append("     , TRIM(T3.RESV_STATUS) AS STATUS ")
                .Append("     , TRIM(T3.PICK_DELI_TYPE) AS REZ_RECEPTION ")
                .Append("     , TRIM(T3.ACCEPTANCE_TYPE) AS WALKIN ")
                .Append("     , DECODE(T10.PICK_PREF_DATETIME, :MINDATE, NULL, TO_CHAR(T10.PICK_PREF_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_PICK_DATE ")
                .Append("     , DECODE(T11.DELI_PREF_DATETIME, :MINDATE, NULL, TO_CHAR(T11.DELI_PREF_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_DELI_DATE ")
                .Append("     , TRIM(T3.SVC_STATUS) AS RESULT_STATUS ")
                .Append("     , DECODE(T3.RSLT_SVCIN_DATETIME, :MINDATE, NULL, TO_CHAR(T3.RSLT_SVCIN_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_IN_TIME ")
                .Append("     , DECODE(T3.RSLT_SVCIN_DATETIME, :MINDATE, :STRMINDATE, RSLT_SVCIN_DATETIME) AS STRDATE ")
                .Append("     , NULL AS RESULT_WAIT_END ")
                .Append("     , TRIM(T3.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                .Append("     , TRIM(T4.UPPER_DISP) AS SVCORGNMCT ")
                .Append("     , TRIM(T4.LOWER_DISP) AS SVCORGNMCB ")
                .Append("     , CASE T5.SVC_CLASS_TYPE")
                .Append("            WHEN :SVC_CLASS_TYPE_1 THEN :C_SERVICECODE_INSPECTION ")
                .Append("            WHEN :SVC_CLASS_TYPE_2 THEN :C_SERVICECODE_PERIODIC")
                .Append("            WHEN :SVC_CLASS_TYPE_3 THEN :C_SERVICECODE_GENERAL ")
                .Append("            WHEN :SVC_CLASS_TYPE_4 THEN :C_SERVICECODE_NEWCAR ")
                .Append("            ELSE :DEFAULT_VALUE  ")
                .Append("       END AS SERVICECODE ")
                .Append("     , TRIM(T6.VCL_KATASHIKI) AS MODELCODE ")
                .Append("     , TRIM(T7.REG_NUM) AS VCLREGNO ")
                .Append("     , T2.INSPECTION_STATUS AS INSPECTIONREQFLG ")
                .Append("     , NVL(TRIM(T8.MODEL_NAME),TRIM(T6.NEWCST_MODEL_NAME)) AS VEHICLENAME ")
                .Append("     , TRIM(T9.CST_NAME) AS CUSTOMERNAME ")
                .Append("     , T10.PICK_WORKTIME AS REZ_PICK_TIME ")
                .Append("     , T11.DELI_WORKTIME AS REZ_DELI_TIME ")
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .Append("     , NVL(TRIM(T7.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                .Append("  FROM TB_T_STALL_USE T1 ")
                .Append("     , TB_T_JOB_DTL T2 ")
                .Append("     , TB_T_SERVICEIN T3 ")
                .Append("     , TB_M_MERCHANDISE T4 ")
                .Append("     , TB_M_SERVICE_CLASS T5 ")
                .Append("     , TB_M_VEHICLE T6 ")
                .Append("     , TB_M_VEHICLE_DLR T7 ")
                .Append("     , TB_M_MODEL T8 ")
                .Append("     , TB_M_CUSTOMER T9 ")
                .Append("     , TB_T_VEHICLE_PICKUP T10 ")
                .Append("     , TB_T_VEHICLE_DELIVERY T11 ")
                .Append("     , TB_T_RO_INFO T12 ")
                .Append(" WHERE T1.DLR_CD = T2.DLR_CD ")
                .Append("   AND T1.BRN_CD = T2.BRN_CD ")
                .Append("   AND T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .Append("   AND T2.DLR_CD = T3.DLR_CD ")
                .Append("   AND T2.BRN_CD = T3.BRN_CD ")
                .Append("   AND T2.SVCIN_ID = T3.SVCIN_ID ")
                .Append("   AND T2.MERC_ID = T4.MERC_ID (+) ")
                .Append("   AND T2.SVC_CLASS_ID = T5.SVC_CLASS_ID(+) ")
                .Append("   AND T3.VCL_ID = T6.VCL_ID (+) ")
                .Append("   AND T3.DLR_CD = T7.DLR_CD (+) ")
                .Append("   AND T3.VCL_ID = T7.VCL_ID (+) ")
                .Append("   AND T6.MODEL_CD = T8.MODEL_CD (+) ")
                .Append("   AND T3.CST_ID = T9.CST_ID (+) ")
                .Append("   AND T3.SVCIN_ID = T10.SVCIN_ID (+) ")
                .Append("   AND T3.SVCIN_ID = T11.SVCIN_ID (+) ")
                .Append("   AND T3.SVCIN_ID = T12.SVCIN_ID (+) ")
                .Append("   AND T1.DLR_CD = :DLR_CD ")
                .Append("   AND T1.BRN_CD = :BRN_CD ")
                .Append("   AND T1.STALL_ID = :STALL_ID ")
                .Append("   AND T1.STALL_USE_STATUS <> :SUS01 ")
                .Append("   AND T1.RSLT_START_DATETIME  >= :RESULT_START_TIME1 ")
                .Append("   AND T1.RSLT_START_DATETIME  <  :RESULT_START_TIME2 ")
                .Append("   AND NOT(T1.STALL_USE_STATUS = :SUS07 OR T1.TEMP_FLG = :TEMP_FLG) ")
                .Append("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .Append("   AND T3.SVC_STATUS NOT IN (:SS00, :SS01, :SS03) ")

            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ResultChipInfoDataTable)("SC3150101_020")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                ''Dim workTimeFrom As String = dateFrom.ToString("yyyyMMddHHmmss")                        ' 稼働時間From
                ''Dim workTimeTo As String = dateTo.ToString("yyyyMMddHHmmss")                            ' 稼働時間To
                'Dim workTimeFrom As String = dateFrom.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())                          ' 稼働時間From
                'Dim workTimeTo As String = dateTo.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())                              ' 稼働時間To
                'Dim workTimeZeroFrom As String = dateFrom.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture()) & "0000"            ' 稼働時間Fromの日付部分＋00:00:00
                'query.AddParameterWithTypeValue("DLRCD1", OracleDbType.Char, dealerCode)                ' 販売店コード
                'query.AddParameterWithTypeValue("STRCD1", OracleDbType.Char, branchCode)                ' 店舗コード
                '' 2012/02/28 上田 SQLインスペクション対応 Start
                ''query.AddParameterWithTypeValue("DLRCD2", OracleDbType.Char, dealerCode)                ' 販売店コード
                ''query.AddParameterWithTypeValue("STRCD2", OracleDbType.Char, branchCode)                ' 店舗コード
                '' 2012/02/28 上田 SQLインスペクション対応 End
                'query.AddParameterWithTypeValue("RESULT_STALLID1", OracleDbType.Int64, stallId)         ' ストールID
                'query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, workTimeFrom)  ' 稼働時間From
                'query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, workTimeTo)    ' 稼働時間To
                ''2012/03/09 上田 サブエリア表示分取得削除 START
                ''query.AddParameterWithTypeValue("RESULT_START_TIME3", OracleDbType.Char, workTimeTo)    ' 稼働時間To
                ''2012/03/09 上田 サブエリア表示分取得削除 END
                'query.AddParameterWithTypeValue("REZ_START_TIME1", OracleDbType.Char, workTimeZeroFrom) ' 稼働時間Fromの日付部分＋0000
                '' 2012/02/28 上田 SQLインスペクション対応 Start
                ''query.AddParameterWithTypeValue("DLRCD5", OracleDbType.Char, dealerCode)                ' 販売店コード
                ''query.AddParameterWithTypeValue("DLRCD6", OracleDbType.Char, dealerCode)                ' 販売店コード
                '' 2012/02/28 上田 SQLインスペクション対応 End
                '' 入庫日時の仮デフォルト値として設定
                'query.AddParameterWithTypeValue("MINDATE1", OracleDbType.Char, DateTime.MinValue.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                Dim workTimeFrom As String = dateFrom.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())                          ' 稼働時間From
                Dim workTimeTo As String = dateTo.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())                              ' 稼働時間To
                'Dim workTimeZeroFrom As String = dateFrom.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture()) & "0000"            ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("STRMINDATE", OracleDbType.Date, Date.Parse(strMinDate, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_1", OracleDbType.NVarchar2, SVC_CLASS_TYPE_1)
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_2", OracleDbType.NVarchar2, SVC_CLASS_TYPE_2)
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_3", OracleDbType.NVarchar2, SVC_CLASS_TYPE_3)
                query.AddParameterWithTypeValue("SVC_CLASS_TYPE_4", OracleDbType.NVarchar2, SVC_CLASS_TYPE_4)
                query.AddParameterWithTypeValue("C_SERVICECODE_INSPECTION", OracleDbType.NVarchar2, C_SERVICECODE_INSPECTION)
                query.AddParameterWithTypeValue("C_SERVICECODE_PERIODIC", OracleDbType.NVarchar2, C_SERVICECODE_PERIODIC)
                query.AddParameterWithTypeValue("C_SERVICECODE_GENERAL", OracleDbType.NVarchar2, C_SERVICECODE_GENERAL)
                query.AddParameterWithTypeValue("C_SERVICECODE_NEWCAR", OracleDbType.NVarchar2, C_SERVICECODE_NEWCAR)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                ' 店舗コード
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)                     ' ストールID
                query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Date, Date.Parse(workTimeFrom, CultureInfo.InvariantCulture))       ' 稼働時間From
                query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Date, Date.Parse(workTimeTo, CultureInfo.InvariantCulture))         ' 稼働時間To
                query.AddParameterWithTypeValue("CN_INSTRUCT_2", OracleDbType.NVarchar2, CN_INSTRUCT_2)
                query.AddParameterWithTypeValue("CN_INSTRUCT_0", OracleDbType.NVarchar2, CN_INSTRUCT_0)
                query.AddParameterWithTypeValue("SS00", OracleDbType.NVarchar2, sarviceStatus00)
                query.AddParameterWithTypeValue("SS01", OracleDbType.NVarchar2, sarviceStatus01)
                query.AddParameterWithTypeValue("SS03", OracleDbType.NVarchar2, sarviceStatus03)
                query.AddParameterWithTypeValue("SUS00", OracleDbType.NVarchar2, stallUseStetus00)
                query.AddParameterWithTypeValue("SUS01", OracleDbType.NVarchar2, stallUseStetus01)
                query.AddParameterWithTypeValue("SUS07", OracleDbType.NVarchar2, stallUseStetus07)
                query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, TEMP_FLG)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
                query.AddParameterWithTypeValue("DEFAULT_VALUE", OracleDbType.NVarchar2, DEFAULT_VALUE)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, ICON_OFF_FLAG)
                '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                Logger.Info("[E]GetResultChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 使用不可チップ情報の取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="fromDate">稼働時間From</param>
        ''' <param name="toDate">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetUnavailableChipInfo(ByVal stallId As Decimal, _
                                               ByVal fromDate As Date, _
                                               ByVal toDate As Date) As SC3150101DataSet.SC3150101UnavailableChipInfoDataTable

            Logger.Info("[S]GetUnavailableChipInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101UnavailableChipInfoDataTable)("SC3150101_021")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("  SELECT /* SC3150101_021 */")
                    '.Append("         STARTTIME, ")
                    '.Append("         ENDTIME ")
                    '.Append("    FROM TBL_STALLREZINFO")
                    '.Append("   WHERE DLRCD = :DLRCD ")
                    '.Append("     AND STRCD = :STRCD ")
                    '.Append("     AND STALLID = :STALLID ")
                    '.Append("     AND STATUS = 3 ")
                    '.Append("     AND CANCELFLG = '0' ")
                    '.Append("     AND STOPFLG = '0' ")
                    '.Append("     AND ENDTIME >= TO_DATE(:ENDTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                    '.Append("     AND STARTTIME < TO_DATE(:STARTTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                    '.Append("ORDER BY STARTTIME")

                    .Append("SELECT /* SC3150101_021 */ ")
                    .Append("         IDLE_START_DATETIME AS STARTTIME  ")
                    .Append("       , IDLE_END_DATETIME AS ENDTIME ")
                    .Append("    FROM TB_M_STALL_IDLE  ")
                    .Append("   WHERE STALL_ID = :STALL_ID ")
                    .Append("     AND CANCEL_FLG = :CANCEL_FLG_0 ")
                    .Append("     AND IDLE_TYPE = :IDLE_TYPE_2 ")
                    .Append("     AND IDLE_START_DATETIME >= :STARTTIME ")
                    .Append("     AND IDLE_END_DATETIME < :ENDTIME ")
                    .Append("ORDER BY IDLE_START_DATETIME ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, SetSearchDate(fromDate))                  ' 稼働時間Fromの日付部分＋00:00:00
                'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())) ' 稼働時間To


                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId) ' ストールID
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
                query.AddParameterWithTypeValue("IDLE_TYPE_2", OracleDbType.NVarchar2, IDLE_TYPE_2)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, fromDate)                 ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, toDate) ' 稼働時間To
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                Logger.Info("[E]GetUnavailableChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 休憩情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetBreakChipInfo(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal stallId As Decimal) As SC3150101DataSet.SC3150101BreakChipInfoDataTable

            Logger.Info("[S]GetBreakChipInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101BreakChipInfoDataTable)("SC3150101_022")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150101_022 */ ")
                    .Append("         STARTTIME ")
                    .Append("       , ENDTIME ")
                    .Append("    FROM TBL_STALLBREAK ")
                    .Append("   WHERE DLRCD = :DLRCD ")
                    .Append("     AND STRCD = :STRCD ")
                    .Append("     AND STALLID = :STALLID ")
                    .Append("     AND BREAKKBN = :BREAKKBN_1 ")
                    .Append("ORDER BY STARTTIME")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("BREAKKBN_1", OracleDbType.NVarchar2, BREAKKBN_1)
                Logger.Info("[E]GetBreakChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' ログインアカウントが所属するストール情報の取得
        ''' </summary>
        ''' <param name="account">ログインアカウント</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetBelongStallInfo(ByVal account As String, ByVal stallId As Decimal) As SC3150101DataSet.SC3150101BelongStallInfoDataTable

            Logger.Info("[S]GetBelongStallInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101BelongStallInfoDataTable)("SC3150101_023")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("SELECT /* SC3150101_023 */ ")
                    '.Append("       T3.STALLID AS STALLID, ")
                    '.Append("       T3.STALLNAME AS STALLNAME, ")
                    '.Append("       T3.STALLNAME_S AS STALLNAME_S, ")
                    '.Append("       T4.PSTARTTIME AS PSTARTTIME, ")
                    '.Append("       T4.PENDTIME AS PENDTIME ")
                    '.Append("  FROM TBL_SSTAFF      T1, ")
                    '.Append("       TBL_WSTAFFSTALL T2, ")
                    '.Append("       TBL_STALL       T3, ")
                    '.Append("       TBL_STALLTIME   T4 ")
                    '.Append(" WHERE T2.DLRCD    = T1.DLRCD ")
                    '.Append("   AND T2.STRCD    = T1.STRCD ")
                    '.Append("   AND T2.STAFFCD  = T1.STAFFCD ")
                    '.Append("   AND T3.STALLID  = T2.STALLID ")
                    '.Append("   AND T4.DLRCD    = T2.DLRCD ")
                    '.Append("   AND T4.STRCD    = T2.STRCD")
                    '.Append("   AND T1.ACCOUNT  = :ACCOUNT ")
                    '.Append("   AND T2.WORKDATE = :WORKDATE ")

                    .Append("SELECT /* SC3150101_023 */ ")
                    .Append("       T3.STALLID AS STALLID ")
                    .Append("     , T3.STALLNAME AS STALLNAME ")
                    .Append("     , T3.STALLNAME_S AS STALLNAME_S ")
                    .Append("     , TO_CHAR(T4.PSTARTTIME) AS PSTARTTIME ")
                    .Append("     , TO_CHAR(T4.PENDTIME) AS PENDTIME ")
                    .Append("  FROM TB_M_STAFF T1 ")
                    .Append("     , TB_M_STAFF_STALL T2 ")
                    .Append("     , TBL_STALL T3 ")
                    .Append("     , TBL_STALLTIME T4 ")
                    .Append(" WHERE T3.STALLID = T2.STALL_ID (+) ")
                    .Append("   AND T4.DLRCD = T3.DLRCD ")
                    .Append("   AND T4.STRCD = T3.STRCD ")
                    .Append("   AND T2.STF_CD =  T1.STF_CD(+) ")
                    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
                    If stallId = 0 Then
                        .Append("   AND T1.STF_CD = T2.STF_CD")
                        .Append("   AND T1.STF_CD = :STF_CD ")
                        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, account) 'ログインアカウント
                    Else
                        .Append("   AND T3.STALLID = :STALLID ")
                        query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, stallId) 'ストールID
                    End If
                    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

                End With
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate)


                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                Logger.Info("[E]GetBelongStallInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
        ' ''' <summary>
        ' ''' 指定日の指定ストールに所属するテクニシャン名の取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="stallId">ストールID</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function GetBelongStallStaff(ByVal dealerCode As String, _
        '                                    ByVal branchCode As String, _
        '                                    ByVal stallId As Decimal) As SC3150101DataSet.SC3150101BelongStallStaffDataTable

        ''' <summary>
        ''' 指定日の指定ストールに所属するテクニシャン名の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="stfStallDispType">スタッフストール表示区分</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBelongStallStaff(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallId As Decimal, _
                                            ByVal stfStallDispType As String) As SC3150101DataSet.SC3150101BelongStallStaffDataTable
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            Logger.Info("[S]GetBelongStallStaff()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101BelongStallStaffDataTable)("SC3150101_024")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("SELECT /* SC3150101_024 */ ")
                    '.Append("       T3.USERNAME AS USERNAME ")
                    '.Append("  FROM TBL_WSTAFFSTALL T1,")
                    '.Append("       TBL_SSTAFF      T2,")
                    '.Append("       TBL_USERS       T3 ")
                    '.Append(" WHERE T2.DLRCD    = T1.DLRCD ")
                    '.Append("   AND T2.STRCD    = T1.STRCD ")
                    '.Append("   AND T2.STAFFCD  = T1.STAFFCD ")
                    '.Append("   AND T3.ACCOUNT  = T2.ACCOUNT")
                    '.Append("   AND T1.DLRCD    = :DLRCD ")
                    '.Append("   AND T1.STRCD    = :STRCD ")
                    '.Append("   AND T1.WORKDATE = :WORKDATE ")
                    '.Append("   AND T1.STALLID  = :STALLID ")

                    .Append("SELECT /* SC3150101_024 */ ")
                    .Append("       T1.STF_NAME AS USERNAME ")
                    .Append("     , T1.STF_CD AS STF_CD ")
                    .Append("  FROM TB_M_STAFF T1")
                    .Append("     , TB_M_STAFF_STALL T2")
                    .Append(" WHERE T1.STF_CD = T2.STF_CD ")
                    .Append("   AND T2.STALL_ID = :STALL_ID ")
                    .Append("   AND DLR_CD = :DLR_CD ")
                    .Append("   AND BRN_CD = :BRN_CD ")


                    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
                    'ストール表示区分により、テクニシャン権限が違う
                    If ("0").Equals(stfStallDispType) Then
                        '0:店舗M、店舗SA、店舗O
                        .AppendLine("    AND (  ")
                        .AppendLine("               T1.BRN_MANAGER_FLG = N'1' ")
                        .AppendLine("           OR  T1.BRN_OPERATOR_FLG = N'1' ")
                        .AppendLine("           OR  T1.BRN_SA_FLG = N'1' ")
                        .AppendLine("        )  ")
                    ElseIf ("2").Equals(stfStallDispType) Then
                        '2:店舗SA、店舗O
                        .AppendLine("    AND (  ")
                        .AppendLine("               T1.BRN_OPERATOR_FLG = N'1' ")
                        .AppendLine("           OR  T1.BRN_SA_FLG = N'1' ")
                        .AppendLine("        )  ")
                    ElseIf ("3").Equals(stfStallDispType) Then
                        '3:店舗M、店舗O
                        .AppendLine("    AND (  ")
                        .AppendLine("               T1.BRN_MANAGER_FLG = N'1' ")
                        .AppendLine("           OR  T1.BRN_OPERATOR_FLG = N'1' ")
                        .AppendLine("        )  ")
                    Else
                        'ディフォルト(1):店舗O
                        .AppendLine("    AND T1.BRN_OPERATOR_FLG = N'1' ")
                    End If

                    .AppendLine("    AND T1.INUSE_FLG = N'1' ")
                    .AppendLine("    AND EXISTS (  ")
                    .AppendLine("        	      SELECT 1  ")
                    .AppendLine("                   FROM TB_M_ORGANIZATION T3  ")
                    .AppendLine("                  WHERE T1.ORGNZ_ID = T3.ORGNZ_ID  ")
                    .AppendLine("                    AND DLR_CD = :DLR_CD  ")
                    .AppendLine("                    AND BRN_CD = :BRN_CD  ")
                    .AppendLine("                    AND ORGNZ_SA_FLG = N'1'  ")
                    .AppendLine("                    AND INUSE_FLG  = N'1'  ")
                    .AppendLine("               )  ")
                    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate)
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)  ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)  ' 店舗コード
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)  ' ストールID
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                Logger.Info("[E]GetBelongStallStaff()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START

        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
        ' ''' <summary>
        ' ''' ストールの作業担当者数の取得
        ' ''' </summary>
        ' ''' <param name="stallId">ストールID</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function GetStaffCount(ByVal stallId As Decimal) As SC3150101DataSet.SC3150101StallStaffCountDataTable

        ''' <summary>
        ''' ストールの作業担当者数の取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetStaffCount(ByVal stallId As Decimal, _
                                      ByVal dealerCode As String, _
                                      ByVal branchCode As String, _
                                      ByVal stfStallDispType As String) As SC3150101DataSet.SC3150101StallStaffCountDataTable
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            'Public Function GetStaffCount(ByVal processDate As Date, _
            '                              ByVal stallId As Integer) As SC3150101DataSet.SC3150101StallStaffCountDataTable

            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END

            Logger.Info("[S]GetStaffCount()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallStaffCountDataTable)("SC3150101_025")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("SELECT /* SC3150101_025 */ ")
                    '.Append("       COUNT(1) AS COUNT ")
                    '.Append("  FROM TBL_WSTAFFSTALL ")
                    '' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
                    ''.Append(" WHERE STALLID  = :STALLID ")
                    ''.Append("   AND WORKDATE = :WORKDATE")
                    '.Append(" WHERE DLRCD = :DLRCD ")
                    '.Append("   AND STRCD = :STRCD")
                    '.Append("   AND WORKDATE = :WORKDATE")
                    '.Append("   AND STALLID = :STALLID")
                    '' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END

                    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
                    '.Append("SELECT /* SC3150101_025 */ ")
                    '.Append("       COUNT(1) AS COUNT ")
                    '.Append("  FROM TB_M_STAFF_STALL ")
                    '.Append(" WHERE STALL_ID = :STALL_ID ")

                    .AppendLine(" SELECT /* SC3150101_025 */ ")
                    .AppendLine("        COUNT(1) AS COUNT")
                    .AppendLine("   FROM ")
                    .AppendLine("        TB_M_STAFF_STALL T1 ")
                    .AppendLine("      , TB_M_STAFF T2 ")
                    .AppendLine("  WHERE T1.STF_CD = T2.STF_CD ")
                    .AppendLine("    AND T1.STALL_ID = :STALL_ID  ")
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
                    .AppendLine("    AND T2.INUSE_FLG = N'1' ")
                    .AppendLine("    AND EXISTS (  ")
                    .AppendLine("        	      SELECT 1  ")
                    .AppendLine("                   FROM TB_M_ORGANIZATION T3  ")
                    .AppendLine("                  WHERE T2.ORGNZ_ID = T3.ORGNZ_ID  ")
                    .AppendLine("                    AND DLR_CD = :DLRCD  ")
                    .AppendLine("                    AND BRN_CD = :STRCD  ")
                    .AppendLine("                    AND ORGNZ_SA_FLG = N'1'  ")
                    .AppendLine("                    AND INUSE_FLG  = N'1'  ")
                    .AppendLine("               )  ")
                    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                '' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                '' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, processDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture()))
                'query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)

                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

                Logger.Info("[E]GetStaffCount()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 作業中の数の取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="startTime">稼動開始時間</param>
        ''' <param name="endTime">稼動終了時間</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetWorkingStateCount(ByVal dealerCode As String, _
                                             ByVal branchCode As String, _
                                             ByVal stallId As Decimal, _
                                             ByVal startTime As Date, _
                                             ByVal endTime As Date) As SC3150101DataSet.SC3150101WorkingStateCountDataTable

            Logger.Info("[S]GetWorkingStateCount()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101WorkingStateCountDataTable)("SC3150101_026")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("    SELECT /* SC3150101_026 */ ")
                    '.Append("           COUNT(1) AS COUNT ")
                    '.Append("      FROM TBL_STALLPROCESS T1, ")
                    '.Append("           TBL_STALLREZINFO T2 ")
                    '.Append("     WHERE T2.DLRCD = T1.DLRCD ")
                    '.Append("       AND T2.STRCD = T1.STRCD ")
                    '.Append("       AND T2.REZID = T1.REZID ")
                    '.Append("       AND T1.SEQNO =( SELECT MAX(T3.SEQNO) ")
                    '.Append("                         FROM TBL_STALLPROCESS T3 ")
                    '.Append("                        WHERE T3.DLRCD = T1.DLRCD ")
                    '.Append("                          AND T3.STRCD = T1.STRCD ")
                    '.Append("                          AND T3.REZID = T1.REZID ")
                    '.Append("                     GROUP BY T3.DLRCD, T3.STRCD, T3.REZID ) ")
                    '.Append("       AND T1.RESULT_STALLID = :RESULT_STALLID ")
                    '.Append("       AND T1.RESULT_STATUS = '20' ")
                    '.Append("       AND T2.CANCELFLG <> '1' ")
                    '.Append("       AND T1.RESULT_START_TIME >= :RESULT_START_TIME1 ")
                    '.Append("       AND T1.RESULT_START_TIME < :RESULT_START_TIME2")

                    .Append("SELECT /* SC3150101_026 */ ")
                    .Append("       COUNT(1) AS COUNT ")
                    .Append("  FROM TB_T_JOB_DTL T1 ")
                    .Append("     , TB_T_STALL_USE T2 ")
                    .Append(" WHERE T2.DLR_CD = T1.DLR_CD ")
                    .Append("   AND T2.BRN_CD = T1.BRN_CD  ")
                    .Append("   AND T2.JOB_DTL_ID = T1.JOB_DTL_ID ")
                    .Append("   AND T2.DLR_CD = :DLR_CD ")
                    .Append("   AND T2.BRN_CD = :BRN_CD ")
                    .Append("   AND T2.STALL_ID = :STALL_ID ")
                    '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
                    '.Append("   AND T2.STALL_USE_STATUS = :SUS02 ")
                    .Append("   AND T2.STALL_USE_STATUS IN (:SUS02, :SUS04) ")
                    '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
                    .Append("   AND T1.CANCEL_FLG <> :CANCEL_FLG_1 ")
                    .Append("   AND T2.STALL_USE_ID = (SELECT MAX(T3.STALL_USE_ID) ")
                    .Append("                            FROM TB_T_STALL_USE T3 ")
                    .Append("                           WHERE T3.DLR_CD = T1.DLR_CD ")
                    .Append("                             AND T3.BRN_CD = T1.BRN_CD  ")
                    .Append("                             AND T3.JOB_DTL_ID = T1.JOB_DTL_ID ")
                    .Append("                        GROUP BY T3.DLR_CD, T3.BRN_CD, T3.JOB_DTL_ID ) ")
                    .Append("   AND T2.RSLT_START_DATETIME >= :RESULT_START_TIME1 ")
                    .Append("   AND T2.RSLT_START_DATETIME < :RESULT_START_TIME2 ")
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, stallId)
                ''query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, startTime.ToString("yyyyMMddHHmmss"))
                ''query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, endTime.ToString("yyyyMMddHHmmss"))
                'query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, startTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))
                'query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, endTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)                                                                          'ストールID
                query.AddParameterWithTypeValue("SUS02", OracleDbType.NVarchar2, stallUseStetus02)
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
                query.AddParameterWithTypeValue("SUS04", OracleDbType.NVarchar2, stallUseStetus04)
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
                query.AddParameterWithTypeValue("CANCEL_FLG_1", OracleDbType.NVarchar2, CANCEL_FLG_1)
                query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Date, endTime)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                Logger.Info("[E]GetWorkingStateCount()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ' ''' <summary>
        ' ''' リレーション内の作業終了(実績ステータス：97)チップの最大REZCHILDNOを取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="parentsReserveId">管理予約ID</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ' ''' </history>
        'Public Function GetRelationLastChildNo(ByVal dealerCode As String, _
        '                                       ByVal branchCode As String, _
        '                                       ByVal parentsReserveId As Long) As SC3150101DataSet.SC3150101RelationLastChildNoDataTable

        '    Logger.Info("[S]GetRelationLastChildNo()")

        '    ' DBSelectQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101RelationLastChildNoDataTable)("SC3150101_028")

        '        Dim sql As New StringBuilder

        '        ' SQL文の作成
        '        With sql
        '            .Append("   SELECT /* SC3150101_028 */ ")
        '            .Append("          MAX(T1.REZCHILDNO) REZCHILDNO ")
        '            .Append("     FROM TBL_STALLREZINFO T1, ")
        '            .Append("          TBL_STALLPROCESS T2 ")
        '            .Append("    WHERE T1.DLRCD = T2.DLRCD (+) ")
        '            .Append("      AND T1.STRCD = T2.STRCD (+) ")
        '            .Append("      AND T1.REZID = T2.REZID (+) ")
        '            .Append("      AND T1.DLRCD = :DLRCD ")
        '            .Append("      AND T1.STRCD = :STRCD ")
        '            .Append("      AND T1.PREZID = :PREZID ")
        '            .Append("      AND T2.RESULT_STATUS = '97'")
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '        query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, parentsReserveId)

        '        Logger.Info("[E]GetRelationLastChildNo()")

        '        ' 検索結果の返却
        '        Return query.GetData()

        '    End Using

        'End Function


        ' ''' <summary>
        ' ''' リレーション内のREZCHILDNO更新対象を取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="parentsReserveId">管理予約ID</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetChildNoUpdateTarget(ByVal dealerCode As String, _
        '                                       ByVal branchCode As String, _
        '                                       ByVal parentsReserveId As Long, _
        '                                       ByVal childNo As Integer, _
        '                                       ByVal reserveId As Long) As SC3150101DataSet.SC3150101TargetChildNoInfoDataTable

        '    Logger.Info("[S]GetChildNoUpdateTarget()")

        '    ' DBSelectQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101TargetChildNoInfoDataTable)("SC3150101_029")

        '        Dim sql As New StringBuilder

        '        ' SQL文の作成
        '        With sql
        '            .Append("SELECT /* SC3150101_029 */ ")
        '            .Append("       DLRCD, ")
        '            .Append("       STRCD, ")
        '            ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        '            '.Append("       REZID ")
        '            .Append("       REZID, ")
        '            .Append("       REZCHILDNO, ")
        '            .Append("       INSTRUCT ")
        '            ' 2012/06/01 KN 西田 STEP1 重要課題対応 END
        '            .Append("  FROM TBL_STALLREZINFO ")
        '            .Append(" WHERE DLRCD = :DLRCD ")
        '            .Append("   AND STRCD = :STRCD ")
        '            .Append("   AND PREZID = :PREZID ")
        '            .Append("   AND REZCHILDNO > :REZCHILDNO ")
        '            ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        '            .Append("   AND REZCHILDNO < 999 ")             '納車チップは除外
        '            .Append(" ORDER BY DECODE(REZID, :REZID, NULL, REZCHILDNO) NULLS FIRST")    'NULLS FIRST指定しないと対象のREZIDが一番下になる
        '            '.Append("ORDER BY DECODE(REZID, :REZID, DLRCD, STRCD) ASC, ")
        '            '.Append("         REZCHILDNO ASC ")
        '            ' 2012/06/01 KN 西田 STEP1 重要課題対応 END
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '        query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, parentsReserveId)
        '        query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, childNo)
        '        query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

        '        Logger.Info("[E]GetChildNoUpdateTarget()")

        '        ' 検索結果の返却
        '        Return query.GetData()

        '    End Using

        'End Function


        ' ''' <summary>
        ' ''' 指定チップのREZCHILDNOを指定値で更新
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="reserveId">予約ID</param>
        ' ''' <param name="childNo">子予約連番</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function UpdateChildNo(ByVal dealerCode As String, _
        '                              ByVal branchCode As String, _
        '                              ByVal reserveId As Long, _
        '                              ByVal childNo As Integer) As Integer

        '    Logger.Info("[S]UpdateChildNo()")

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3150101_030")

        '        Dim sql As New StringBuilder

        '        ' SQL文の作成
        '        With sql
        '            'etc
        '            .Append("UPDATE /* SC3150101_030 */ ")
        '            .Append("       TBL_STALLREZINFO ")
        '            .Append("   SET REZCHILDNO = :REZCHILDNO ")
        '            .Append(" WHERE DLRCD = :DLRCD ")
        '            .Append("   AND STRCD = :STRCD ")
        '            .Append("   AND REZID = :REZID")
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, childNo)
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '        query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

        '        Logger.Info("[E]UpdateChildNo()")

        '        'SQL実行
        '        Return query.Execute()

        '    End Using

        'End Function

        ''' <summary>
        ''' 子チップのORDERNOを取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="parentsReserveId">管理予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetChildOrderNo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal parentsReserveId As Decimal) As SC3150101DataSet.SC3150101ChildChipOrderNoDataTable

            Logger.Info("[S]GetChildOrderNo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ChildChipOrderNoDataTable)("SC3150101_031")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("SELECT /* SC3150101_031 */ ")
                    '.Append("       ORDERNO ")            ' R/O No.
                    '.Append("  FROM TBL_STALLREZINFO ")
                    '.Append(" WHERE DLRCD = :DLRCD ")     ' 販売店コード
                    '.Append("   AND STRCD = :STRCD ")     ' 店舗コード
                    '.Append("   AND REZID = :PREZID ")    ' 予約ID

                    .Append("SELECT /* SC3150101_031 */ ")
                    .Append("       TRIM(RO_NUM) AS ORDERNO ")    ' R/O No.
                    .Append("  FROM TB_T_SERVICEIN ")
                    .Append(" WHERE DLR_CD = :DLR_CD ")     ' 販売店コード
                    .Append("   AND BRN_CD = :BRN_CD ")     ' 店舗コード
                    .Append("   AND SVCIN_ID = :SVCIN_ID ") ' 予約ID

                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                ''query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                'query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, parentsReserveId)
                ''query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, childNo)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)          ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)          ' 店舗コード
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, parentsReserveId)      ' サービス入庫ID
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                Logger.Info("[E]GetChildOrderNo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START

        ' ''' <summary>
        ' ''' 親ROの開始済みのChip数を取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="pRezId">管理予約ID</param>
        ' ''' <returns>親ROの開始済みのChip数</returns>
        ' ''' <remarks>親ROの開始済みのChip数が0の場合、親ROが未着工であることを意味する。この場合、追加作業の各Chipの作業開始を許さない</remarks>
        'Public Function GetStartedChipCountOfInitialRO(ByVal dealerCode As String, ByVal branchCode As String, ByVal pRezId As Decimal) As Long
        '    Logger.Info("[S]GetStartedChipCountOfInitialRO()")

        '    Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ChipCountDataTable)("SC3150101_035")
        '        Dim sql As New StringBuilder

        '        With sql
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '            '.Append("SELECT /* SC3150101_035 */ ")
        '            '.Append("       COUNT(1) AS CNT ")
        '            '.Append("  FROM TBL_STALLREZINFO T1 ")
        '            '.Append(" WHERE DLRCD = :DLRCD ")
        '            '.Append("   AND STRCD = :STRCD ")
        '            '.Append("   AND PREZID = :PREZID ")
        '            '.Append("   AND WORKSEQ = 0 ")
        '            '.Append("   AND ACTUAL_STIME IS NOT NULL")

        '            .Append("SELECT /* SC3150101_035 */ ")
        '            .Append("       COUNT(1) AS CNT ")
        '            .Append("  FROM TB_T_JOB_DTL T1 ")
        '            .Append("     , TB_T_STALL_USE T2 ")
        '            .Append(" WHERE T1.JOB_DTL_ID = T2.JOB_DTL_ID ")
        '            .Append("   AND T1.DLR_CD = :DLR_CD ")
        '            .Append("   AND T1.BRN_CD = :BRN_CD ")
        '            .Append("   AND T1.SVCIN_ID = :SVCIN_ID ")
        '            '.Append("   AND T1.RO_SEQ = :RO_SEQ_0 ")
        '            .Append("   AND T2.RSLT_START_DATETIME <> :MINDATE ")
        '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
        '        End With

        '        query.CommandText = sql.ToString()

        '        ' バインド変数定義
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '        'query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, pRezId)

        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode) '販売店コード
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode) '店舗コード
        '        query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, pRezId)  'サービス入庫ID
        '        'query.AddParameterWithTypeValue("RO_SEQ_0", OracleDbType.NVarchar2, RO_SEQ_0)
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.CurrentCulture()))
        '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

        '        ' 検索結果の返却
        '        Dim dt As DataTable = query.GetData()

        '        Logger.Info("[E]GetStartedChipCountOfInitialRO()")

        '        If dt.Equals(Nothing) Then
        '            Return 0
        '        Else
        '            Return CLng(dt.Rows(0).Item("CNT"))
        '        End If

        '    End Using
        'End Function

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' 作業中のチップの数を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="pRezId">管理予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetWorkingChipCount(ByVal dealerCode As String, ByVal branchCode As String, ByVal pRezId As Decimal) As Long
            Logger.Info("[S]GetWorkingChipCount()")

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ChipCountDataTable)("SC3150101_034")
                Dim sql As New StringBuilder

                With sql
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                    '.Append("SELECT /* SC3150101_034 */ ")
                    '.Append("       COUNT(1) AS CNT ")
                    '.Append("  FROM TBL_STALLREZINFO T1, ")
                    '.Append("       TBL_STALLPROCESS T2 ")
                    '.Append(" WHERE T1.DLRCD = T2.DLRCD ")
                    '.Append("   AND T1.STRCD = T2.STRCD ")
                    '.Append("   AND T1.REZID = T2.REZID ")
                    '.Append("   AND T1.DLRCD = :DLRCD ")
                    '.Append("   AND T1.STRCD = :STRCD ")
                    '.Append("   AND T1.PREZID = :PREZID ")
                    '.Append("   AND T2.RESULT_STATUS = :RESULT_STATUS")

                    .Append("SELECT /* SC3150101_034 */ ")
                    .Append("       COUNT(1) AS CNT ")
                    .Append("  FROM TB_T_SERVICEIN T1 ")
                    .Append("     , TB_T_JOB_DTL T2 ")
                    .Append("     , TB_T_STALL_USE T3 ")
                    .Append(" WHERE T1.DLR_CD = T2.DLR_CD ")
                    .Append("   AND T1.BRN_CD = T2.BRN_CD ")
                    .Append("   AND T1.SVCIN_ID = T2.SVCIN_ID ")
                    .Append("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .Append("   AND T1.DLR_CD = :DLR_CD ")
                    .Append("   AND T1.BRN_CD = :BRN_CD ")
                    .Append("   AND T1.SVCIN_ID = :SVCIN_ID ")
                    '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
                    '.Append("   AND T3.STALL_USE_STATUS = :STALL_USE_STATUS")
                    .Append("   AND T3.STALL_USE_STATUS IN (:STALL_USE_STATUS02, :STALL_USE_STATUS04)")
                    '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, pRezId)
                'query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, "20")

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)     '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)     '店舗コード
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, pRezId)      'サービス入庫ID
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
                'query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, stallUseStetus02) 'ストール利用ステータス
                query.AddParameterWithTypeValue("STALL_USE_STATUS02", OracleDbType.NVarchar2, stallUseStetus02) 'ストール利用ステータス:作業中
                query.AddParameterWithTypeValue("STALL_USE_STATUS04", OracleDbType.NVarchar2, stallUseStetus04) 'ストール利用ステータス:作業指示の一部の作業が中断
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END

                ' 検索結果の返却
                Dim dt As DataTable = query.GetData()

                Logger.Info("[E]GetWorkingChipCount()")

                If dt.Equals(Nothing) Then
                    Return 0
                Else
                    Return CLng(dt.Rows(0).Item("CNT"))
                End If

            End Using
        End Function
        ' 2012/06/05 KN 彭 コード分析対応 END

        '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 START
        ''' <summary>
        '''TCステータスモニター起動までの待機時間の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' '更新：2013/02/26 TMEJ 成澤 【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成(TCステータスモニター起動待機時間の取得)
        ''' </history>
        Public Function GetTcStatusStandTime(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3150101DataSet.SC3150101TcStatusStandTimeDataTable

            Logger.Info("[S]GetTcStatusStandTime()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101TcStatusStandTimeDataTable)("SC3150101_036")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150101_036 */ ")
                    .Append("         TCSTATUS_STANDBY_TIME")
                    .Append("    FROM TBL_SERVICEINI ")
                    .Append("   WHERE DLRCD = :DLRCD ")
                    .Append("     AND STRCD = :STRCD ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END
                Logger.Info("[E]GetTcStatusStandTime()")

                ' 検索結果の返却
                Return query.GetData()
            End Using

        End Function
        '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 END

        ''' <summary>
        ''' 実績リレーションチップの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="serviceInId">サービス入庫ID</param>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <returns>存在する場合<c>true</c>、存在しない場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function GetResultRelationChip(ByVal dealerCode As String, _
                                              ByVal branchCode As String, _
                                              ByVal serviceInId As Decimal, _
                                              ByVal stallUseId As Decimal) As Boolean

            Logger.Info("[S]GetResultRelationChip()")

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* SC3150101_044 */ ")
                .AppendLine("       COUNT(1) COUNT ")
                .AppendLine("   FROM ( ")
                .AppendLine("            SELECT ")
                .AppendLine("                   T5.STALL_USE_ID ")
                .AppendLine("              FROM ")
                .AppendLine("                   TB_T_SERVICEIN T3 ")
                .AppendLine("                 , TB_T_JOB_DTL T4 ")
                .AppendLine("                 , TB_T_STALL_USE T5  ")
                .AppendLine("             WHERE ")
                .AppendLine("                   T3.SVCIN_ID = T4.SVCIN_ID  ")
                .AppendLine("               AND T4.JOB_DTL_ID = T5.JOB_DTL_ID  ")
                .AppendLine("               AND T5.DLR_CD = :DLR_CD ")
                .AppendLine("               AND T5.BRN_CD = :BRN_CD ")
                .AppendLine("               AND T3.SVCIN_ID=:SVCIN_ID  ")
                .AppendLine("               AND T4.CANCEL_FLG = :CANCEL_FLG  ")
                .AppendLine("        ) T1 ")
                .AppendLine("      , TB_T_STALL_USE T2 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T2.STALL_USE_ID = T1.STALL_USE_ID ")
                .AppendLine("    AND T1.STALL_USE_ID <> :STALL_USE_ID ")
                .AppendLine("    AND (    T2.STALL_USE_STATUS = :SUS03 ")
                .AppendLine("          OR T2.STALL_USE_STATUS = :SUS05 ) ")
            End With


            Dim tblResult As SC3150101DataSet.SC3150101GetResultRelationChipDataTable

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101GetResultRelationChipDataTable)("SC3150101_044")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, serviceInId)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("SUS03", OracleDbType.NVarchar2, stallUseStetus03)
                query.AddParameterWithTypeValue("SUS05", OracleDbType.NVarchar2, stallUseStetus05)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CANCEL_FLG_0)
                tblResult = query.GetData()
            End Using

            Logger.Info("[E]GetResultRelationChip()")

            '取得したリレーションチップが0件以上場合
            If CType(tblResult.Item(0).Item("COUNT"), Long) > 0 Then
                Return True
            End If

            Return False

        End Function

        ''' <summary>
        ''' 予約リレーションチップの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="serviceInId">サービス入庫ID</param>
        ''' <returns>存在する場合<c>true</c>、存在しない場合<c>false</c></returns>
        ''' <remarks></remarks>
        Public Function GetReserveRelationChip(ByVal dealerCode As String, _
                                               ByVal branchCode As String, _
                                               ByVal serviceInId As Decimal) As Boolean

            Logger.Info("[S]GetReserveRelationChip")

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .AppendLine("SELECT /* SC3150101_045 */ ")
                .AppendLine("       COUNT(1) COUNT ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_SERVICEIN T1 ")
                .AppendLine("     , TB_T_JOB_DTL T2 ")
                .AppendLine("    , TB_T_STALL_USE T3 ")
                .AppendLine(" WHERE ")
                .AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("   AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("   AND T1.SVCIN_ID= :SVCIN_ID ")
                .AppendLine("   AND T2.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("  AND T3.STALL_USE_STATUS IN(:SUS00,:SUS01) ")
            End With


            Dim tblResult As SC3150101DataSet.SC3150101GetReserveRelationChipDataTable

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101GetReserveRelationChipDataTable)("SC3150101_045")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, serviceInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("SUS00", OracleDbType.NVarchar2, stallUseStetus00)
                query.AddParameterWithTypeValue("SUS01", OracleDbType.NVarchar2, stallUseStetus01)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CANCEL_FLG_0)
                tblResult = query.GetData()
            End Using

            Logger.Info("[E]GetReserveRelationChip")

            '取得したリレーションチップが0件以上場合
            If CType(tblResult.Item(0).Item("COUNT"), Long) > 0 Then
                Return True
            End If

            Return False

        End Function

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' 現ストール担当のChtアカウント取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetChtTechnicianAccount(ByVal stallId As Decimal) _
                                                As SC3150101DataSet.SC3150101ChtStaffCodeDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} STALL_ID:{2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , stallId))

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ChtStaffCodeDataTable)("SC3150101_047")

                Dim sql As New StringBuilder      ' SQL文格納
                With sql
                    .AppendLine("SELECT /* SC3150101_047 */ ")
                    .AppendLine("       T4.STF_CD ")
                    .AppendLine("  FROM TB_M_STALL_STALL_GROUP T1 ")
                    .AppendLine("     , TB_M_STALL_GROUP T2 ")
                    .AppendLine("	  , TB_M_ORGANIZATION T3 ")
                    .AppendLine("     , TB_M_STAFF T4 ")
                    .AppendLine("	  , TBL_USERS T5 ")
                    .AppendLine(" WHERE T1.STALL_GROUP_ID = T2.STALL_GROUP_ID ")
                    .AppendLine("   AND T2.ORGNZ_ID = T3.ORGNZ_ID ")
                    .AppendLine("   AND T3.ORGNZ_ID = T4.ORGNZ_ID ")
                    .AppendLine("   AND T4.STF_CD = T5.ACCOUNT ")
                    .AppendLine("   AND T1.STALL_ID = :STALL_ID ")
                    .AppendLine("   AND T3.ORGNZ_SA_FLG = :ORGNZ_SA_FLG ")
                    .AppendLine("   AND T3.INUSE_FLG = :INUSE_FLG ")
                    .AppendLine("   AND T5.OPERATIONCODE = :OPERATIONCODE ")

                End With
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Int32, OPERATIONCODE_CHIEF_TECHNICIAN)
                query.AddParameterWithTypeValue("ORGNZ_SA_FLG", OracleDbType.NVarchar2, ORGNZ_SA_FLG)
                query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.NVarchar2, INUSE_FLG)

                '実行
                Dim dt As SC3150101DataSet.SC3150101ChtStaffCodeDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        ''' <summary>
        ''' ROステータスの取得
        ''' </summary>
        ''' <param name="jobDetailId">サービス入庫ID</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetRepairOrderStatus(ByVal jobDetailId As Decimal,
                                             ByVal dealerCode As String, _
                                             ByVal branchCode As String) _
                                             As SC3150101DataSet.SC3150101RepairOrderStatusDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} SVCIN_ID:{2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , jobDetailId.ToString(CultureInfo.CurrentCulture())))

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101RepairOrderStatusDataTable)("SC3150101_048")

                Dim sql As New StringBuilder      ' SQL文格納
                With sql
                    .AppendLine("SELECT /* SC3150101_048 */ ")
                    .AppendLine("       T2.RO_STATUS ")
                    .AppendLine("  FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("     , TB_T_RO_INFO T2 ")
                    .AppendLine(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T1.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND T1.SVCIN_ID = ( SELECT T1.SVCIN_ID ")
                    .AppendLine("                         FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("                            , TB_T_JOB_DTL T2  ")
                    .AppendLine("                        WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("                          AND T2.DLR_CD = :DLR_CD ")
                    .AppendLine("                          AND T2.BRN_CD = :BRN_CD ")
                    .AppendLine("                          AND JOB_DTL_ID = :JOB_DTL_ID ) ")

                End With
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDetailId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

                '実行
                Dim dt As SC3150101DataSet.SC3150101RepairOrderStatusDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        ''' <summary>
        ''' TCからの画面連携に必要な引数取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="jobDatilId">作業内容</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetTechnicianScreenLinkageInfo(ByVal dealerCode As String, _
                                                       ByVal branchCode As String, _
                                                       ByVal jobDatilId As Decimal) As SC3150101DataSet.SC3150101ScreenLinkageInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, JOB_DTL_ID:{4}. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dealerCode _
                  , branchCode _
                  , dealerCode.ToString(CultureInfo.CurrentCulture())))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ScreenLinkageInfoDataTable)("SC3150101_049")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150101_049 */  ")
                    .AppendLine("       T1.RO_NUM AS RO_NUM  ")
                    .AppendLine("     , T2.DMS_JOB_DTL_ID AS DMS_JOB_DTL_ID  ")
                    .AppendLine("     , T3.VISITSEQ AS VISITSEQ  ")
                    .AppendLine("     , T3.VIN AS VIN  ")
                    .AppendLine("     , TRIM(T3.VCLREGNO) AS VCLREGNO ")
                    .AppendLine("     , TRIM(T3.SACODE) AS SACODE ")
                    .AppendLine("     , 0 AS RO_SEQ  ")
                    .AppendLine("     , T4.CST_NAME  ")
                    .AppendLine("     , NVL(TRIM(T4.DMS_CST_CD) , T3.DMSID) AS DMS_CST_CD ")
                    .AppendLine("     , T5.NAMETITLE_NAME  ")
                    .AppendLine("     , T5.POSITION_TYPE  ")
                    .AppendLine("  FROM TB_T_SERVICEIN T1  ")
                    .AppendLine("     , TB_T_JOB_DTL T2  ")
                    .AppendLine("     , TBL_SERVICE_VISIT_MANAGEMENT T3 ")
                    .AppendLine("     , TB_M_CUSTOMER T4 ")
                    .AppendLine("     , TB_M_NAMETITLE T5 ")
                    .AppendLine(" WHERE T1.SVCIN_ID = T2.SVCIN_ID  ")
                    .AppendLine("   AND T1.RO_NUM = T3.ORDERNO (+)  ")
                    .AppendLine("   AND T1.CST_ID = T4.CST_ID (+)  ")
                    .AppendLine("   AND T4.NAMETITLE_CD = T5.NAMETITLE_CD (+)  ")
                    .AppendLine("   AND T2.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T2.BRN_CD = :BRN_CD  ")
                    .AppendLine("   AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
                    .AppendLine("   AND T3.DLRCD = :DLR_CD  ")
                    .AppendLine("   AND T3.STRCD = :BRN_CD  ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                             '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                             '店舗コード
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDatilId)                               '作業内容ID
                'ロウナンバー

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 最後の作業チップと着工指示フラグのない整備数を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="OrderRepiarNumber">RO番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
        ''' </history>
        Public Function GetLastWorkChip(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal orderRepiarNumber As String) As SC3150101DataSet.SC3150101GetLastWorkChipDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, RO_NUM:{4}. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dealerCode _
                  , branchCode _
                  , orderRepiarNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101GetLastWorkChipDataTable)("SC3150101_050")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150101_050 */  ")
                    .AppendLine("       T1.NO_FLG_COUNT ")
                    .AppendLine("     , T5.JOB_DTL_ID ")
                    .AppendLine("     , T5.PIC_SA_STF_CD ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

                    .AppendLine("     , T5.SVC_STATUS ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

                    .AppendLine("  FROM (SELECT COUNT(1) AS NO_FLG_COUNT ")
                    .AppendLine("          FROM TB_T_JOB_INSTRUCT T6 ")
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発
                    '.AppendLine("         WHERE T6.RO_NUM = :RO_NUM ")
                    .AppendLine("             , TB_T_SERVICEIN T7 ")
                    .AppendLine("             , TB_T_JOB_DTL T8 ")
                    .AppendLine("         WHERE T7.SVCIN_ID = T8.SVCIN_ID (+)  ")
                    .AppendLine("           AND T8.JOB_DTL_ID = T6.JOB_DTL_ID ")
                    .AppendLine("           AND T7.DLR_CD = :DLR_CD ")
                    .AppendLine("           AND T7.BRN_CD = :BRN_CD  ")
                    .AppendLine("           AND T7.RO_NUM = :RO_NUM ")
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発
                    .AppendLine("           AND NOT T6.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG ")
                    .AppendLine("        ) T1  ")
                    .AppendLine("     , (SELECT T3.JOB_DTL_ID  ")
                    .AppendLine("             , T2.PIC_SA_STF_CD  ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

                    .AppendLine("             , T2.SVC_STATUS  ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

                    .AppendLine("             , ROW_NUMBER() OVER (PARTITION BY T2.SVCIN_ID  ")
                    .AppendLine("                                  ORDER BY T4.SCHE_END_DATETIME DESC  ")
                    .AppendLine("                                ) AS RNUM  ")
                    .AppendLine("         FROM TB_T_SERVICEIN T2  ")
                    .AppendLine("            , TB_T_JOB_DTL T3  ")
                    .AppendLine("            , TB_T_STALL_USE T4  ")
                    .AppendLine("        WHERE T2.SVCIN_ID = T3.SVCIN_ID (+)  ")
                    .AppendLine("          AND T3.JOB_DTL_ID = T4.JOB_DTL_ID (+) ")
                    .AppendLine("          AND T2.DLR_CD = :DLR_CD ")
                    .AppendLine("          AND T3.BRN_CD = :BRN_CD ")
                    .AppendLine("          AND T2.RO_NUM = :RO_NUM ")
                    .AppendLine("        ) T5  ")
                    .AppendLine(" WHERE T5.RNUM = :ROW_NUM ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                             '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                             '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, orderRepiarNumber)                      'RO番号
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, STARTWORK_INSTRUCT_FLG) '着工指示フラグ
                query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int32, ROW_NUM)                                   'ロウナンバー

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 最初の作業チップと担当SAコードの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="jobDetailId">作業内容ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetFirstWorkChip(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal jobDetailId As Decimal) As SC3150101DataSet.SC3150101FirstWorkChipDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, RO_NUM:{4}. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dealerCode _
                  , branchCode _
                  , jobDetailId.ToString(CultureInfo.CurrentCulture)))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101FirstWorkChipDataTable)("SC3150101_051")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150101_051 */ ")
                    .AppendLine("       T6.PIC_SA_STF_CD ")
                    .AppendLine("     , T6.JOB_DTL_ID ")
                    .AppendLine("  FROM ( SELECT T1.PIC_SA_STF_CD ")
                    .AppendLine("              , T2.JOB_DTL_ID  ")
                    .AppendLine("              , ROW_NUMBER() OVER(  ")
                    .AppendLine("                                   PARTITION BY T1.SVCIN_ID ")
                    .AppendLine("                                   ORDER BY T3.SCHE_START_DATETIME ASC ")
                    .AppendLine("                                 ) AS RNUM ")
                    .AppendLine("           FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("              , TB_T_JOB_DTL T2 ")
                    .AppendLine("              , TB_T_STALL_USE T3 ")
                    .AppendLine("          WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("            AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .AppendLine("            AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("            AND T1.BRN_CD = :BRN_CD  ")
                    .AppendLine("            AND T1.SVCIN_ID = ( SELECT T4.SVCIN_ID ")
                    .AppendLine("                                  FROM TB_T_SERVICEIN T4 ")
                    .AppendLine("                                     , TB_T_JOB_DTL T5 ")
                    .AppendLine("                                 WHERE T4.SVCIN_ID = T5.SVCIN_ID ")
                    .AppendLine("                                   AND T5.DLR_CD = :DLR_CD ")
                    .AppendLine("                                   AND T5.BRN_CD = :BRN_CD ")
                    .AppendLine("                                   AND T5.JOB_DTL_ID = :JOB_DTL_ID ) ")
                    .AppendLine("        ) T6 ")
                    .AppendLine(" WHERE T6.RNUM = :ROW_NUM ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                             '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                             '店舗コード
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDetailId)                          '作業内容ID
                query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int32, ROW_NUM)                                   'ロウナンバー

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 現ストールの全てのチップのRO番号とRO連番を取得する
        ''' </summary>
        ''' <param name="dealerCode"></param>
        ''' <param name="branchCode"></param>
        ''' <param name="stallId"></param>
        ''' <param name="todayDateTime"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRepairOrderSequence(ByVal dealerCode As String, _
                                               ByVal branchCode As String, _
                                               ByVal stallId As Decimal, _
                                               ByVal todayDateTime As Date) As SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} STALL_ID:{2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , stallId.ToString(CultureInfo.CurrentCulture())))



            Dim sql As New StringBuilder      ' SQL文格納
            With sql
                .AppendLine("SELECT /* SC3150101_052 */ ")
                .AppendLine("       T7.RO_NUM ")
                .AppendLine("     , T7.JOB_DTL_ID ")
                .AppendLine("     , T7.RO_SEQ ")
                .AppendLine("     , 0 AS PARTS_ISSUE_STATUS ")
                .AppendLine("  FROM ( SELECT /* SC3150101_048 */ ")
                .AppendLine("                T6.JOB_DTL_ID ")
                .AppendLine("              , T5.RO_NUM ")
                .AppendLine("              , T5.RO_SEQ ")
                .AppendLine("              , ROW_NUMBER() OVER (PARTITION BY  T6.RO_NUM, T6.RO_SEQ  ")
                .AppendLine("                                   ORDER BY T6.JOB_DTL_ID ASC  ")
                .AppendLine("                                   ) AS RNUM  ")
                .AppendLine("           FROM TB_T_RO_INFO T5  ")
                .AppendLine("              , TB_T_JOB_INSTRUCT T6 ")
                .AppendLine("              , ( SELECT T1.RO_NUM ")
                '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                .AppendLine("                       , T2.JOB_DTL_ID ")
                '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 END
                .AppendLine("                       , ROW_NUMBER() OVER (PARTITION BY T1.SVCIN_ID ")
                .AppendLine("                                            ORDER BY T2.JOB_DTL_ID ASC ")
                .AppendLine("                                            ) AS RNUM ")
                .AppendLine("                    FROM TB_T_SERVICEIN T1")
                .AppendLine("                       , TB_T_JOB_DTL T2 ")
                .AppendLine("                       , TB_T_STALL_USE T3 ")
                .AppendLine("                   WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("                     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID")
                .AppendLine("                     AND T3.DLR_CD = :DLR_CD ")
                .AppendLine("                     AND T3.BRN_CD = :BRN_CD ")
                .AppendLine("                     AND T3.STALL_ID = :STALL_ID ")
                .AppendLine("                     AND NOT T1.RO_NUM = :DEFAULT_VALUE ")
                .AppendLine("             AND T1.SVC_STATUS <> :SVCS01 ")
                .AppendLine("             AND T3.STALL_USE_STATUS <> :SUS07 ")
                .AppendLine("                     AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("                     AND T3.TEMP_FLG <> :TEMP_FLG ")
                .AppendLine("                     AND TRUNC(T3.SCHE_START_DATETIME) = TRUNC(:TODAY_DATETIME) ")
                .AppendLine("                ) T4  ")
                .AppendLine("          WHERE T4.RO_NUM = T5.RO_NUM ")
                '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                .AppendLine("            AND T4.JOB_DTL_ID = T6.JOB_DTL_ID ")
                '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 END
                .AppendLine("            AND T5.RO_NUM = T6.RO_NUM ")
                .AppendLine("            AND T5.RO_SEQ = T6.RO_SEQ ")
                .AppendLine("            AND T4.RNUM = :ROW_NUM ")
                '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                .AppendLine("            AND T5.DLR_CD = :DLR_CD ")
                .AppendLine("            AND T5.BRN_CD = :BRN_CD ")
                '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 END
                .AppendLine("            AND NOT T5.RO_SEQ = :RO_SEQ_DEFAULT ")
                .AppendLine("            AND NOT T5.RO_STATUS = :RO_STATUS_CANCEL ")
                .AppendLine("            AND T6.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG ")
                .AppendLine("        ) T7 ")
                .AppendLine(" WHERE T7.RNUM = :ROW_NUM ")
            End With

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable)("SC3150101_052")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("TODAY_DATETIME", OracleDbType.Date, todayDateTime)
                query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int32, ROW_NUM)                                   'ロウナンバー
                query.AddParameterWithTypeValue("DEFAULT_VALUE", OracleDbType.NVarchar2, DEFAULT_VALUE)
                query.AddParameterWithTypeValue("RO_SEQ_DEFAULT", OracleDbType.Int32, RO_SEQ_DEFAULT)
                query.AddParameterWithTypeValue("RO_STATUS_CANCEL", OracleDbType.NVarchar2, RO_STATUS_CANCEL)
                query.AddParameterWithTypeValue("SVCS01", OracleDbType.NVarchar2, sarviceStatus01)
                query.AddParameterWithTypeValue("SUS07", OracleDbType.NVarchar2, stallUseStetus07)
                query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, TEMP_FLG)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, STARTWORK_INSTRUCT_FLG)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                          , "{0}.{1} END" _
                                          , Me.GetType.ToString _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '実行
                Return query.GetData()

            End Using
        End Function

        '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END


        '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

        ''' <summary>
        ''' 中断メモテンプレートを取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns>中断メモテンプレートテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetStopMemoTemplate(ByVal dealerCode As String, ByVal branchCode As String) As SC3150101DataSet.SC3150101StopMemoTempDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                    , "{0}_S. DLR_CD={1}, BRN_CD={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , dealerCode _
                                    , branchCode))

            '関連チップがある
            Dim sql As New StringBuilder
            With sql
                .AppendLine("    SELECT /* SC3150101_053 */ ")
                .AppendLine("           STOP_MEMO_TEMPLATE  ")
                .AppendLine("      FROM TB_M_STOP_MEMO_TEMPLATE")
                .AppendLine("     WHERE DLR_CD=:DLR_CD")
                .AppendLine("       AND BRN_CD=:BRN_CD ")
                .AppendLine("  ORDER BY SORT_ORDER  ")

            End With

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StopMemoTempDataTable)("SC3150101_053")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E.", System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()
            End Using
        End Function

        '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="defult">デフォルト値</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SetData(ByVal src As Object, ByVal defult As Object) As Object

            If IsDBNull(src) = True Then
                Return defult
            End If

            Return src

        End Function

        ''' <summary>
        ''' SQL用の値を設定
        ''' </summary>
        ''' <param name="Value">対象文字列</param>
        ''' <returns>SQLに設定する文字列</returns>
        ''' <remarks></remarks>
        Protected Function SetSqlValue(ByVal value As String) As String
            ' 2012/02/27 KN 佐藤 【SERVICE_1】DevPartner 1回目の指摘事項を修正（処理修正） START
            'If IsNothing(value) OrElse value.Trim() = "" Then
            '    '値がない場合、半角スペースを設定
            '    value = " "
            'End If
            If String.IsNullOrEmpty(value) OrElse value.Trim.Length = 0 Then
                '値がない場合、半角スペースを設定
                value = " "
            End If
            ' 2012/02/27 KN 佐藤 【SERVICE_1】DevPartner 1回目の指摘事項を修正（処理修正） END
            Return value
        End Function

        ''' <summary>
        ''' 日付+00:00:00を返す
        ''' </summary>
        ''' <param name="Value">対象文字列</param>
        ''' <returns>SQLに設定する文字列</returns>
        ''' <remarks></remarks>
        Private Function SetSearchDate(ByVal value As Date) As String

            Dim retValue As String

            retValue = DateSerial(value.Year, value.Month, value.Day).ToString("yyyy/MM/dd HH:mm:ss", Globalization.CultureInfo.CurrentCulture())

            Return retValue

        End Function

    End Class

End Namespace
Partial Class SC3150101DataSet
End Class
