'-------------------------------------------------------------------------
'Partial Class SMBCommonClassDataSet.vb
'-------------------------------------------------------------------------
'機能：共通関数API
'補足：
'作成：2012/05/24 KN 河原 【servive_2】
'更新：2012/06/06 KN 小澤 STEP2事前準備対応
'更新：2012/06/19 KN 小澤 STEP2対応(事前準備用の処理削除)
'更新：2012/08/22 TMEJ 日比野【SERVICE_2】チップ詳細の代表整備項目が表示されないv
'更新：2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新：2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新：2014/02/08 TMEJ 小澤 BTS対応
'更新：2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応
'更新：2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加
'更新：2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新：2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001
'更新：2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新：2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新：2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる
'更新：2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある
'更新：
'
Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace SMBCommonClassDataSetTableAdapters

    ''' <summary>
    ''' 共通関数APIデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SMBCommonClassTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '処理なし
        End Sub

#Region "定数"

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        ' ''' <summary>
        ' ''' 新規登録
        ' ''' </summary>
        'Private Const ReserveHisNew As Integer = 0

        ' ''' <summary>
        ' ''' 個別登録
        ' ''' </summary>
        'Private Const ReserveHisIndividual As Integer = 1

        ' ''' <summary>
        ' ''' 全て登録
        ' ''' </summary>
        'Private Const ReserveHisAll As Integer = 2

        ' ''' <summary>
        ' ''' 削除時の登録
        ' ''' </summary>
        'Private Const ReserveHisDelete As Integer = 9

        ''' <summary>
        ''' 販売店コードデフォルト値
        ''' </summary>
        Private Const DealerCodeDefault As String = "XXXXX"

        ''' <summary>
        ''' 店舗コードデフォルト値
        ''' </summary>
        Private Const StoreCodeDefault As String = "XXX"

        ''' <summary>
        ''' サービス入庫テーブル
        ''' </summary>
        Private Const RegisterServiceIn As Integer = 0

        ''' <summary>
        ''' 作業内容テーブル
        ''' </summary>
        Private Const RegisterJobDetail As Integer = 1

        ''' <summary>
        ''' ストール利用テーブル
        ''' </summary>
        Private Const RegisterStallUse As Integer = 2

        ''' <summary>
        ''' キャンセルフラグ（0：有効）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CancelTypeEffective As String = "0"

        ''' <summary>
        ''' サービスステータス（00：未入庫）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusNoneCarIn As String = "00"

        ''' <summary>
        ''' サービスステータス（02：キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusCancel As String = "02"

        ''' <summary>
        ''' サービスステータス（04：作業開始待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusWaitStart As String = "04"

        ''' <summary>
        ''' ストール利用テータス（00：着工指示待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallUseStatusWaitInstruct As String = "00"

        ''' <summary>
        ''' ストール利用テータス（01：作業開始待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallUseStatusWaitActual As String = "01"

        ''' <summary>
        ''' ストール利用テータス（05：中断）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallUseStatusStop As String = "05"

        ''' <summary>
        ''' ストール利用テータス（07：未来店客）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallUseStatusNoVisitor As String = "07"

        ''' <summary>
        ''' 基本型式（共通）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CommonBaseType As String = "X"

        ''' <summary>
        ''' 日付最小値文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DateMinValue As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' 顧客区分（1：自社客）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CusutomerTypeVisitor As String = "1"

        ''' <summary>
        ''' 受付区分（0：予約）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeReserve As String = "0"

        ''' <summary>
        ''' 受付区分（1：WalkIn）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeWalkIn As String = "1"

        ''' <summary>
        ''' 登録機能区分（1：画面）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RegisterFunctionTypeDisplay As String = "1"

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' 配置区分（1：名前の後）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PositionTypeAfter As String = "1"
        ''' <summary>
        ''' 配置区分（2：名前の前）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PositionTypeBefore As String = "2"

        ''' <summary>
        ''' 使用中フラグ（1：使用中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InuseTypeUse As String = "1"

        ''' <summary>
        ''' RO情報有無（0:RO情報なし）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderTypeNone As String = "0"
        ''' <summary>
        ''' RO情報有無（1:RO情報あり）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderTypeExist As String = "1"

        ''' <summary>
        ''' 起票者：1：TC
        ''' </summary>
        Private Const ReissueVouchersTC As String = "1"
        ''' <summary>
        ''' 起票者：2：SA
        ''' </summary>
        Private Const ReissueVouchersSA As String = "2"

        '' 2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる START
        ''' <summary>
        ''' RO情報ステータス（35:SA承認待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderStatusConfirmationWait As String = "35"
        '' 2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる END

        ''' <summary>
        ''' RO情報ステータス（80:納車準備待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderStatusWaitDelivery As String = "80"
        ''' <summary>
        ''' RO情報ステータス（99:R/Oキャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderStatusCancel As String = "99"

        ''' <summary>
        ''' 作業終了有無（0:作業中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const WorkEndTypeWorking As String = "0"
        ''' <summary>
        ''' 作業終了有無（1:作業終了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const WorkEndTypeWorkEnd As String = "1"

        '2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応 START

        ''' <summary>
        ''' 権限コード(62:ChT)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationCodeChT As Long = 62

        '2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応 END

        ''' <summary>
        ''' 権限コード(14:TC)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationCodeTC As Long = 14
        ''' <summary>
        ''' 権限コード(9:SA)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationCodeSA As Long = 9

        ''' <summary>
        ''' 着工指示フラグ(1：指示済み)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SstartWorkInstructTypeOn As String = "1"

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

#End Region

#Region "メソッド"

#Region "SMBCommonClassBusinessLogic.vb"

        ''' <summary>
        ''' SMBCommonClass_001:チップ操作履歴インサート
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inWhereKey">サービス入庫ID or 作業内容ID or ストール利用ID</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <param name="inRegisterType">登録区分</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inSystem">プログラムID</param>
        ''' <returns>登録結果</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function RegisterDBStallReserveHis(ByVal inDealerCode As String, _
                                                  ByVal inStoreCode As String, _
                                                  ByVal inWhereKey As Decimal, _
                                                  ByVal inPresentTime As Date, _
                                                  ByVal inRegisterType As Integer, _
                                                  ByVal inAccount As String, _
                                                  ByVal inSystem As String, _
                                                  ByVal inActionId As Decimal) As Integer
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inWhereKey, inPresentTime _
                        , inRegisterType, inAccount, inSystem, inActionId))
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'Public Function RegistDBStallReserveHis(ByVal inDealerCode As String, _
            '                                        ByVal inStoreCode As String, _
            '                                        ByVal inReserveId As Long, _
            '                                        ByVal inPresentTime As Date, _
            '                                        ByVal inRegisterType As Integer) As Integer
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6}" _
            '            , Me.GetType.ToString _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '            , inDealerCode, inStoreCode, inReserveId, inPresentTime, inRegisterType))
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            ' 2012/05/15 KN 河原 【SERVICE_2】ストール予約履歴テーブルの登録処理(リレーションも含めて登録)の変更 START
            ''SQLの設定
            Dim sql As New StringBuilder
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'sql.AppendLine("INSERT /* SMBCommonClass_001 */")
            'sql.AppendLine("  INTO TBL_STALLREZHIS (")
            'sql.AppendLine("       DLRCD")
            'sql.AppendLine("     , STRCD")
            'sql.AppendLine("     , REZID")
            'sql.AppendLine("     , SEQNO")
            'sql.AppendLine("     , UPDDVSID")
            'sql.AppendLine("     , BASREZID")
            'sql.AppendLine("     , STALLID")
            'sql.AppendLine("     , STARTTIME")
            'sql.AppendLine("     , ENDTIME")
            'sql.AppendLine("     , CUSTCD")
            'sql.AppendLine("     , PERMITID")
            'sql.AppendLine("     , CUSTOMERNAME")
            'sql.AppendLine("     , TELNO")
            'sql.AppendLine("     , MOBILE")
            'sql.AppendLine("     , EMAIL1")
            'sql.AppendLine("     , VEHICLENAME")
            'sql.AppendLine("     , VCLREGNO")
            'sql.AppendLine("     , SERVICECODE")
            'sql.AppendLine("     , REZDATE")
            'sql.AppendLine("     , NETREZID")
            'sql.AppendLine("     , STATUS")
            'sql.AppendLine("     , INSDID")
            'sql.AppendLine("     , VIN")
            'sql.AppendLine("     , CUSTOMERFLAG")
            'sql.AppendLine("     , CUSTVCLRE_SEQNO")
            'sql.AppendLine("     , MERCHANDISECD")
            'sql.AppendLine("     , SERVICEMSTCD")
            'sql.AppendLine("     , ZIPCODE")
            'sql.AppendLine("     , ADDRESS")
            'sql.AppendLine("     , MODELCODE")
            'sql.AppendLine("     , MILEAGE")
            'sql.AppendLine("     , WASHFLG")
            'sql.AppendLine("     , WALKIN")
            'sql.AppendLine("     , SERVICECODE_S")
            'sql.AppendLine("     , REZ_RECEPTION")
            'sql.AppendLine("     , REZ_WORK_TIME")
            'sql.AppendLine("     , REZ_PICK_DATE")
            'sql.AppendLine("     , REZ_PICK_LOC")
            'sql.AppendLine("     , REZ_PICK_TIME")
            'sql.AppendLine("     , REZ_PICK_FIX")
            'sql.AppendLine("     , REZ_DELI_DATE")
            'sql.AppendLine("     , REZ_DELI_LOC")
            'sql.AppendLine("     , REZ_DELI_TIME")
            'sql.AppendLine("     , REZ_DELI_FIX")
            'sql.AppendLine("     , UPDATE_COUNT")
            'sql.AppendLine("     , STOPFLG")
            'sql.AppendLine("     , PREZID")
            'sql.AppendLine("     , REZCHILDNO")
            'sql.AppendLine("     , ACTUAL_STIME")
            'sql.AppendLine("     , ACTUAL_ETIME")
            'sql.AppendLine("     , CRRY_TYPE")
            'sql.AppendLine("     , CRRYINTIME")
            'sql.AppendLine("     , CRRYOUTTIME")
            'sql.AppendLine("     , MEMO")
            'sql.AppendLine("     , STOPMEMO")
            'sql.AppendLine("     , STRDATE")
            'sql.AppendLine("     , NETDEVICESFLG")
            'sql.AppendLine("     , ACCOUNT_PLAN")
            'sql.AppendLine("     , INPUTACCOUNT")
            'sql.AppendLine("     , INFOUPDATEDATE")
            'sql.AppendLine("     , INFOUPDATEACCOUNT")
            'sql.AppendLine("     , CREATEDATE")
            'sql.AppendLine("     , UPDATEDATE")
            'sql.AppendLine("     , HIS_FLG")
            'sql.AppendLine("     , RSSTATUS")
            'sql.AppendLine("     , RSDATE")
            'sql.AppendLine("     , UPDATESERVER")
            'sql.AppendLine("     , SMSFLG")
            'sql.AppendLine("     , REZTYPE")
            'sql.AppendLine("     , TELEMA_CONTRACT_FLG")
            'sql.AppendLine("     , INSPECTIONFLG")
            'sql.AppendLine("     , CRCUSTID")
            'sql.AppendLine("     , CUSTOMERCLASS")
            'sql.AppendLine("     , STALLWAIT_REZID")
            'sql.AppendLine("     , MNTNCD")
            'sql.AppendLine("     , ORDERNO")
            'sql.AppendLine("     , INSTRUCT")
            'sql.AppendLine("     , WORKSEQ")
            'sql.AppendLine("     , MERCHANDISEFLAG")
            'sql.AppendLine(") ")
            'sql.AppendLine("SELECT")
            'sql.AppendLine("       T1.DLRCD")
            'sql.AppendLine("     , T1.STRCD")
            'sql.AppendLine("     , T1.REZID")
            'sql.AppendLine("     , (")
            'sql.AppendLine("       SELECT NVL(MAX(SEQNO) + 1, 1)")
            'sql.AppendLine("         FROM TBL_STALLREZHIS")
            'sql.AppendLine("        WHERE DLRCD = T1.DLRCD")
            'sql.AppendLine("          AND STRCD = T1.STRCD")
            'sql.AppendLine("          AND REZID = T1.REZID")
            'sql.AppendLine("       )")
            'If inRegisterType = ReserveHisDelete Then
            '    sql.AppendLine("     , '1'")
            'Else
            '    sql.AppendLine("     , '0'")
            'End If
            'sql.AppendLine("     , T1.BASREZID")
            'sql.AppendLine("     , T1.STALLID")
            'sql.AppendLine("     , T1.STARTTIME")
            'sql.AppendLine("     , T1.ENDTIME")
            'sql.AppendLine("     , T1.CUSTCD")
            'sql.AppendLine("     , T1.PERMITID")
            'sql.AppendLine("     , T1.CUSTOMERNAME")
            'sql.AppendLine("     , T1.TELNO")
            'sql.AppendLine("     , T1.MOBILE")
            'sql.AppendLine("     , T1.EMAIL1")
            'sql.AppendLine("     , T1.VEHICLENAME")
            'sql.AppendLine("     , T1.VCLREGNO")
            'sql.AppendLine("     , T1.SERVICECODE")
            'sql.AppendLine("     , T1.REZDATE")
            'sql.AppendLine("     , T1.NETREZID")
            'sql.AppendLine("     , T1.STATUS")
            'sql.AppendLine("     , T1.INSDID")
            'sql.AppendLine("     , T1.VIN")
            'sql.AppendLine("     , T1.CUSTOMERFLAG")
            'sql.AppendLine("     , T1.CUSTVCLRE_SEQNO")
            'sql.AppendLine("     , T1.MERCHANDISECD")
            'sql.AppendLine("     , T1.SERVICEMSTCD")
            'sql.AppendLine("     , T1.ZIPCODE")
            'sql.AppendLine("     , T1.ADDRESS")
            'sql.AppendLine("     , T1.MODELCODE")
            'sql.AppendLine("     , T1.MILEAGE")
            'sql.AppendLine("     , T1.WASHFLG")
            'sql.AppendLine("     , T1.WALKIN")
            'sql.AppendLine("     , T1.SERVICECODE_S")
            'sql.AppendLine("     , T1.REZ_RECEPTION")
            'sql.AppendLine("     , T1.REZ_WORK_TIME")
            'sql.AppendLine("     , T1.REZ_PICK_DATE")
            'sql.AppendLine("     , T1.REZ_PICK_LOC")
            'sql.AppendLine("     , T1.REZ_PICK_TIME")
            'sql.AppendLine("     , T1.REZ_PICK_FIX")
            'sql.AppendLine("     , T1.REZ_DELI_DATE")
            'sql.AppendLine("     , T1.REZ_DELI_LOC")
            'sql.AppendLine("     , T1.REZ_DELI_TIME")
            'sql.AppendLine("     , T1.REZ_DELI_FIX")
            'sql.AppendLine("     , T1.UPDATE_COUNT")
            'sql.AppendLine("     , T1.STOPFLG")
            'sql.AppendLine("     , T1.PREZID")
            'sql.AppendLine("     , T1.REZCHILDNO")
            'sql.AppendLine("     , T1.ACTUAL_STIME")
            'sql.AppendLine("     , T1.ACTUAL_ETIME")
            'sql.AppendLine("     , T1.CRRY_TYPE")
            'sql.AppendLine("     , T1.CRRYINTIME")
            'sql.AppendLine("     , T1.CRRYOUTTIME")
            'sql.AppendLine("     , T1.MEMO")
            'sql.AppendLine("     , T1.STOPMEMO")
            'sql.AppendLine("     , T1.STRDATE")
            'sql.AppendLine("     , T1.NETDEVICESFLG")
            'sql.AppendLine("     , T1.ACCOUNT_PLAN")
            'sql.AppendLine("     , T1.INPUTACCOUNT")
            'sql.AppendLine("     , T1.UPDATEDATE")
            'sql.AppendLine("     , T1.UPDATEACCOUNT")
            'sql.AppendLine("     , :PRESENTTIME")
            'sql.AppendLine("     , :PRESENTTIME")
            'If inRegisterType = ReserveHisNew Then
            '    sql.AppendLine("     , '0'")
            'ElseIf inRegisterType = ReserveHisDelete Then
            '    sql.AppendLine("     , '2'")
            'Else
            '    sql.AppendLine("     , '1'")
            'End If
            'sql.AppendLine("     , '99'")
            'sql.AppendLine("     , :PRESENTTIME")
            'sql.AppendLine("     , NULL")
            'sql.AppendLine("     , T1.SMSFLG")
            'sql.AppendLine("     , T1.REZTYPE")
            'sql.AppendLine("     , T1.TELEMA_CONTRACT_FLG")
            'sql.AppendLine("     , T1.INSPECTIONFLG")
            'sql.AppendLine("     , T1.CRCUSTID")
            'sql.AppendLine("     , T1.CUSTOMERCLASS")
            'sql.AppendLine("     , T1.STALLWAIT_REZID")
            'sql.AppendLine("     , T1.MNTNCD")
            'sql.AppendLine("     , T1.ORDERNO")
            'sql.AppendLine("     , T1.INSTRUCT")
            'sql.AppendLine("     , T1.WORKSEQ")
            'sql.AppendLine("     , T1.MERCHANDISEFLAG")
            'sql.AppendLine("  FROM TBL_STALLREZINFO T1")
            'sql.AppendLine(" WHERE DLRCD = :DLRCD")
            'sql.AppendLine("   AND STRCD = :STRCD")
            'If inRegisterType = ReserveHisAll Then
            '    sql.AppendLine("   AND (T1.REZID = :REZID ")
            '    sql.AppendLine("    OR T1.PREZID = (SELECT ")
            '    sql.AppendLine("                           T3.PREZID ")
            '    sql.AppendLine("                      FROM TBL_STALLREZINFO T3 ")
            '    sql.AppendLine("                     WHERE T3.DLRCD = :DLRCD ")
            '    sql.AppendLine("                       AND T3.STRCD = :STRCD ")
            '    sql.AppendLine("                       AND T3.REZID = :REZID))")
            'Else
            '    sql.AppendLine("   AND T1.REZID = :REZID")
            'End If

            'sql.AppendLine("   AND NOT EXISTS (")
            'sql.AppendLine("            SELECT 1")
            'sql.AppendLine("              FROM TBL_STALLREZINFO T2")
            'sql.AppendLine("             WHERE  T2.DLRCD = T1.DLRCD")
            'sql.AppendLine("               AND  T2.STRCD = T1.STRCD")
            'sql.AppendLine("               AND  T2.REZID = T1.REZID")
            'sql.AppendLine("               AND  T2.STOPFLG = '0'")
            'sql.AppendLine("               AND  T2.CANCELFLG = '1'")
            'sql.AppendLine("                  )")
            sql.AppendLine("INSERT /* SMBCOMMONCLASS_001 */ ")
            sql.AppendLine("  INTO TB_T_CHIP_HIS ( ")
            sql.AppendLine("       CHIP_HIS_ID ")
            sql.AppendLine("      ,SVCIN_ID ")
            sql.AppendLine("      ,PICK_DELI_TYPE ")
            sql.AppendLine("      ,CARWASH_NEED_FLG ")
            sql.AppendLine("      ,RESV_STATUS ")
            sql.AppendLine("      ,SVC_STATUS ")
            sql.AppendLine("      ,SCHE_SVCIN_DATETIME ")
            sql.AppendLine("      ,SCHE_DELI_DATETIME ")
            sql.AppendLine("      ,RSLT_SVCIN_DATETIME ")
            sql.AppendLine("      ,PICK_PREF_DATETIME ")
            sql.AppendLine("      ,DELI_PREF_DATETIME ")
            sql.AppendLine("      ,ACT_ID ")
            sql.AppendLine("      ,JOB_DTL_ID ")
            sql.AppendLine("      ,SVC_CLASS_ID ")
            sql.AppendLine("      ,MERC_ID ")
            sql.AppendLine("      ,INSPECTION_NEED_FLG ")
            sql.AppendLine("      ,CANCEL_FLG ")
            sql.AppendLine("      ,STALL_USE_ID ")
            sql.AppendLine("      ,STALL_ID ")
            sql.AppendLine("      ,TEMP_FLG ")
            sql.AppendLine("      ,SCHE_START_DATETIME ")
            sql.AppendLine("      ,SCHE_END_DATETIME ")
            sql.AppendLine("      ,SCHE_WORKTIME ")
            sql.AppendLine("      ,MAINTE_COMPLETE_FLG ")
            sql.AppendLine("      ,CREATE_DATETIME ")
            sql.AppendLine("      ,CREATE_STF_CD ")
            sql.AppendLine("      ,UPDATE_DATETIME ")
            sql.AppendLine("      ,UPDATE_STF_CD ")
            sql.AppendLine("      ,ROW_CREATE_DATETIME ")
            sql.AppendLine("      ,ROW_CREATE_ACCOUNT ")
            sql.AppendLine("      ,ROW_CREATE_FUNCTION ")
            sql.AppendLine("      ,ROW_UPDATE_DATETIME ")
            sql.AppendLine("      ,ROW_UPDATE_ACCOUNT ")
            sql.AppendLine("      ,ROW_UPDATE_FUNCTION ")
            sql.AppendLine("      ,ROW_LOCK_VERSION ")
            sql.AppendLine("      )( ")
            sql.AppendLine("      SELECT SQ_CHIP_HIS_ID.NEXTVAL ")
            sql.AppendLine("            ,T1.SVCIN_ID ")
            sql.AppendLine("            ,T1.PICK_DELI_TYPE ")
            sql.AppendLine("            ,T1.CARWASH_NEED_FLG ")
            sql.AppendLine("            ,T1.RESV_STATUS ")
            sql.AppendLine("            ,T1.SVC_STATUS ")
            sql.AppendLine("            ,T1.SCHE_SVCIN_DATETIME ")
            sql.AppendLine("            ,T1.SCHE_DELI_DATETIME ")
            sql.AppendLine("            ,T1.RSLT_SVCIN_DATETIME ")
            sql.AppendLine("            ,NVL(T2.PICK_PREF_DATETIME, :MINDATE) ")
            sql.AppendLine("            ,NVL(T3.DELI_PREF_DATETIME, :MINDATE) ")
            sql.AppendLine("            ,:ACTIONID ")
            sql.AppendLine("            ,T6.JOB_DTL_ID ")
            sql.AppendLine("            ,T6.SVC_CLASS_ID ")
            sql.AppendLine("            ,T6.MERC_ID ")
            sql.AppendLine("            ,T6.INSPECTION_NEED_FLG ")
            sql.AppendLine("            ,T6.CANCEL_FLG ")
            sql.AppendLine("            ,T7.STALL_USE_ID ")
            sql.AppendLine("            ,T7.STALL_ID ")
            sql.AppendLine("            ,T7.TEMP_FLG ")
            sql.AppendLine("            ,T7.SCHE_START_DATETIME ")
            sql.AppendLine("            ,T7.SCHE_END_DATETIME ")
            sql.AppendLine("            ,T7.SCHE_WORKTIME ")
            sql.AppendLine("            ,'0' ")
            sql.AppendLine("            ,:NOWDATE ")
            sql.AppendLine("            ,:ACCOUNT ")
            sql.AppendLine("            ,:NOWDATE ")
            sql.AppendLine("            ,:ACCOUNT ")
            sql.AppendLine("            ,:NOWDATE ")
            sql.AppendLine("            ,:ACCOUNT ")
            sql.AppendLine("            ,:SYSTEM ")
            sql.AppendLine("            ,:NOWDATE ")
            sql.AppendLine("            ,:ACCOUNT ")
            sql.AppendLine("            ,:SYSTEM ")
            sql.AppendLine("            ,0 ")
            sql.AppendLine("        FROM TB_T_SERVICEIN T1 ")
            sql.AppendLine("            ,TB_T_VEHICLE_PICKUP T2 ")
            sql.AppendLine("            ,TB_T_VEHICLE_DELIVERY T3 ")
            sql.AppendLine("            ,TB_T_JOB_DTL T6 ")
            sql.AppendLine("            ,TB_T_STALL_USE T7 ")
            sql.AppendLine("       WHERE T1.SVCIN_ID = T2.SVCIN_ID(+) ")
            sql.AppendLine("         AND T1.SVCIN_ID = T3.SVCIN_ID(+) ")
            sql.AppendLine("         AND T1.SVCIN_ID = T6.SVCIN_ID ")
            sql.AppendLine("         AND T6.JOB_DTL_ID = T7.JOB_DTL_ID ")
            sql.AppendLine("         AND T1.DLR_CD = :DLR_CD ")
            sql.AppendLine("         AND T1.BRN_CD = :BRN_CD ")
            sql.AppendLine("         AND T6.DLR_CD = :DLR_CD ")
            sql.AppendLine("         AND T6.BRN_CD = :BRN_CD ")
            sql.AppendLine("         AND T7.DLR_CD = :DLR_CD ")
            sql.AppendLine("         AND T7.BRN_CD = :BRN_CD ")
            '条件設定
            If inRegisterType = RegisterServiceIn Then
                '登録区分がサービス入庫の場合
                sql.AppendLine("         AND T1.SVCIN_ID = :WHEREKEY ")
                sql.AppendLine("         AND T6.CANCEL_FLG = :CANCEL_FLG_0 ")
                sql.AppendLine("         AND T7.STALL_USE_STATUS IN (:STALL_USE_STATUS_00, :STALL_USE_STATUS_01, :STALL_USE_STATUS_07) ")
            ElseIf inRegisterType = RegisterJobDetail Then
                '登録区分が作業内容の場合
                sql.AppendLine("         AND T6.JOB_DTL_ID = :WHEREKEY ")
                sql.AppendLine("         AND T7.STALL_USE_STATUS IN (:STALL_USE_STATUS_00, :STALL_USE_STATUS_01, :STALL_USE_STATUS_07) ")
            ElseIf inRegisterType = RegisterStallUse Then
                '登録区分がストール利用の場合
                sql.AppendLine("         AND T7.STALL_USE_ID = :WHEREKEY ")
            End If
            sql.AppendLine("      ) ")
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            Using query As New DBUpdateQuery("SMBCommonClass_001")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inPresentTime)
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inReserveId)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("ACTIONID", OracleDbType.Decimal, inActionId)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inPresentTime)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, inSystem)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inStoreCode)
                query.AddParameterWithTypeValue("WHEREKEY", OracleDbType.Decimal, inWhereKey)
                If inRegisterType = RegisterServiceIn Then
                    '登録区分がサービス入庫の場合
                    query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                    query.AddParameterWithTypeValue("STALL_USE_STATUS_00", OracleDbType.NVarchar2, StallUseStatusWaitInstruct)
                    query.AddParameterWithTypeValue("STALL_USE_STATUS_01", OracleDbType.NVarchar2, StallUseStatusWaitActual)
                    query.AddParameterWithTypeValue("STALL_USE_STATUS_07", OracleDbType.NVarchar2, StallUseStatusNoVisitor)
                ElseIf inRegisterType = RegisterJobDetail Then
                    '登録区分が作業内容の場合
                    query.AddParameterWithTypeValue("STALL_USE_STATUS_00", OracleDbType.NVarchar2, StallUseStatusWaitInstruct)
                    query.AddParameterWithTypeValue("STALL_USE_STATUS_01", OracleDbType.NVarchar2, StallUseStatusWaitActual)
                    query.AddParameterWithTypeValue("STALL_USE_STATUS_07", OracleDbType.NVarchar2, StallUseStatusNoVisitor)
                End If
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                ''SQLの実行
                Dim ret As Integer = query.Execute()
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ret))
                Return ret
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_002:店舗の営業開始・終了時刻取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns>店舗の営業開始・終了時刻</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function GetStallTime(ByVal inDealerCode As String, _
                                     ByVal inStoreCode As String) _
                                     As SMBCommonClassDataSet.StallTimeDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode))

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.StallTimeDataTable)("SMBCommonClass_002")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("   SELECT  /* SMBCommonClass_002 */")
                    .Append("            PSTARTTIME ")
                    .Append("           ,PENDTIME ")
                    .Append("    FROM     TBL_STALLTIME ")
                    .Append("   WHERE       DLRCD = :DLRCD ")
                    .Append("     AND       STRCD = :STRCD ")
                End With

                query.CommandText = sql.ToString()

                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                Dim dt As SMBCommonClassDataSet.StallTimeDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_003:店舗の非稼働日取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inStartDay">取得開始日</param>
        ''' <returns>店舗の非稼働日</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function GetNonWorkingDays(ByVal inDealerCode As String, _
                                          ByVal inStoreCode As String, _
                                          ByVal inStartDay As String) _
                                          As SMBCommonClassDataSet.NonWorkDaysDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inStartDay))

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.NonWorkDaysDataTable)("SMBCommonClass_003")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("   SELECT  /* SMBCommonClass_003 */")
                    .Append("            WORKDATE ")
                    .Append("    FROM     TBL_STALLPLAN ")
                    .Append("   WHERE       DLRCD = :DLRCD ")
                    .Append("     AND       STRCD = :STRCD ")
                    .Append("     AND       STALLID = -1 ")
                    .Append("     AND       WORKDATE >= :WORKDATE ")
                End With

                query.CommandText = sql.ToString()

                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, inStartDay)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.NVarchar2, inStartDay)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                Dim dt As SMBCommonClassDataSet.NonWorkDaysDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_006ストール予約情報入庫日時変更
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inPlaceStorageTime">入庫日時</param>
        ''' <param name="inAccount">更新者</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <param name="inSystem">プログラムID</param>
        ''' <returns>更新件数</returns>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function UpdateReserveStrDate(ByVal inDealerCode As String, _
                                             ByVal inStoreCode As String, _
                                             ByVal inServiceInId As Decimal, _
                                             ByVal inPlaceStorageTime As DateTime, _
                                             ByVal inAccount As String, _
                                             ByVal inPresentTime As DateTime, _
                                             ByVal inSystem As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inServiceInId, inPlaceStorageTime _
                        , inAccount, inPresentTime, inSystem))
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'Public Function UpdateReserveStrDate(ByVal inDealerCode As String, _
            '                                     ByVal inStoreCode As String, _
            '                                     ByVal inReserveId As Long, _
            '                                     ByVal inPlaceStorageTime As DateTime, _
            '                                     ByVal inAccount As String, _
            '                                     ByVal inPresentTime As DateTime) As Long
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7}" _
            '                , Me.GetType.ToString _
            '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                , inDealerCode, inStoreCode, inReserveId, inPlaceStorageTime, inAccount, inPresentTime))
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            Using query As New DBUpdateQuery("SMBCommonClass_006")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'sql.Append("UPDATE /* SMBCommonClass_006 */ ")
                    'sql.Append("       TBL_STALLREZINFO T1 ")
                    'sql.Append("   SET  ")
                    'If inPlaceStorageTime <> DateTime.MinValue Then
                    '    sql.Append("       T1.STRDATE = :STRDATE ")
                    'Else
                    '    sql.Append("       T1.STRDATE = NULL ")
                    'End If
                    'sql.Append("      ,T1.UPDATE_COUNT = T1.UPDATE_COUNT + 1 ")
                    'sql.Append("      ,T1.UPDATEDATE = :UPDATEDATE ")
                    'sql.Append("      ,T1.UPDATEACCOUNT = :UPDATEACCOUNT ")
                    'sql.Append(" WHERE T1.DLRCD = :DLRCD ")
                    'sql.Append("   AND T1.STRCD = :STRCD ")
                    'sql.Append("   AND (T1.REZID = :REZID  ")
                    'sql.Append("    OR T1.PREZID = :REZID)  ")
                    'sql.Append("   AND NOT EXISTS ( ")
                    'sql.Append("            SELECT 1 ")
                    'sql.Append("              FROM TBL_STALLREZINFO T2 ")
                    'sql.Append("             WHERE  T2.DLRCD = T1.DLRCD ")
                    'sql.Append("               AND  T2.STRCD = T1.STRCD ")
                    'sql.Append("               AND  T2.REZID = T1.REZID ")
                    'sql.Append("               AND  T2.STOPFLG = '0' ")
                    'sql.Append("               AND  T2.CANCELFLG = '1' ")
                    'sql.Append("                  ) ")
                    sql.AppendLine("UPDATE /* SMBCommonClass_006 */ ")
                    sql.AppendLine("       TB_T_SERVICEIN T1 ")
                    sql.AppendLine("   SET T1.RSLT_SVCIN_DATETIME = :RSLT_SVCIN_DATETIME ")
                    sql.AppendLine("      ,T1.SVC_STATUS = :SVC_STATUS ")
                    sql.AppendLine(" WHERE T1.SVCIN_ID = :SVCIN_ID ")
                    sql.AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                    sql.AppendLine("   AND T1.BRN_CD = :BRN_CD ")
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                End With

                query.CommandText = sql.ToString()
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'If inPlaceStorageTime <> DateTime.MinValue Then
                '    query.AddParameterWithTypeValue("STRDATE", OracleDbType.Date, inPlaceStorageTime)
                'End If
                'query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inPresentTime)
                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, inReserveId)
                If inPlaceStorageTime <> DateTime.MinValue Then
                    '入庫日時がある場合
                    'サービスステータス(04：作業開始待ち)
                    query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, ServiceStatusWaitStart)
                    '入庫日時
                    query.AddParameterWithTypeValue("RSLT_SVCIN_DATETIME", OracleDbType.Date, inPlaceStorageTime)
                Else
                    '入庫日時がない場合はNULLを入れる
                    'サービスステータス(00：未入庫)
                    query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, ServiceStatusNoneCarIn)
                    '入庫日時(最小値)
                    query.AddParameterWithTypeValue("RSLT_SVCIN_DATETIME", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                End If
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inStoreCode)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                Dim updateCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , updateCount))
                Return updateCount
            End Using

        End Function

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START

        ''' <summary>
        ''' SMBCommonClass_011:サービス入庫テーブル行ロックバージョン更新処理
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inSystem">プログラムID</param>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function UpdateDBServiceInLockVersion(ByVal inServiceInId As Decimal, _
                                                     ByVal inRowLockVersion As Long, _
                                                     ByVal inAccount As String, _
                                                     ByVal inNowDate As Date, _
                                                     ByVal inSystem As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId, inAccount, inNowDate, inSystem))

            Using query As New DBUpdateQuery("SMBCommonClass_011")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("UPDATE /* SMBCommonClass_011 */ ")
                    .AppendLine("       TB_T_SERVICEIN T1 ")
                    .AppendLine("   SET T1.UPDATE_DATETIME = :NOWDATE ")
                    .AppendLine("      ,T1.UPDATE_STF_CD = :ACCOUNT ")
                    .AppendLine("      ,T1.ROW_UPDATE_DATETIME = :NOWDATE ")
                    .AppendLine("      ,T1.ROW_UPDATE_ACCOUNT = :ACCOUNT ")
                    .AppendLine("      ,T1.ROW_UPDATE_FUNCTION = :SYSTEM ")
                    .AppendLine("      ,T1.ROW_LOCK_VERSION = T1.ROW_LOCK_VERSION + 1 ")
                    .AppendLine(" WHERE T1.SVCIN_ID = :SVCIN_ID ")
                    .AppendLine("   AND T1.ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, inSystem)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, inRowLockVersion)

                Dim updateCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , updateCount))
                Return updateCount
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_012:サービス入庫テーブルロック処理
        ''' </summary>
        ''' <param name="inDearlerCode">販売店コード</param>
        ''' <param name="inBrunchCode">店舗コード</param>
        ''' <param name="inServiceInId">条件キー</param>
        ''' <param name="inCancelType">キャンセルフラグ</param>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function LockDBServiceInTable(ByVal inDearlerCode As String, _
                                             ByVal inBrunchCode As String, _
                                             ByVal inServiceInId As Decimal, _
                                             ByVal inCancelType As String) As SMBCommonClassDataSet.LockInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDearlerCode, inBrunchCode, inServiceInId, inCancelType))

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.LockInfoDataTable)("SMBCommonClass_012")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SMBCommonClass_012 */ ")
                    .AppendLine("       T1.ROW_LOCK_VERSION ")
                    .AppendLine("      ,T1.SVC_STATUS ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_T_SERVICEIN T1 ")
                    .AppendLine("      ,TB_T_JOB_DTL T2 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("   AND T2.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T2.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND T1.SVCIN_ID = :SVCIN_ID ")
                    If CancelTypeEffective.Equals(inCancelType) Then
                        'キャンセルフラグを見る場合は条件追加
                        .AppendLine("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                        query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                    End If
                    .AppendLine("   FOR UPDATE OF T1.SVCIN_ID WAIT 1 ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDearlerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBrunchCode)

                Dim dt As SMBCommonClassDataSet.LockInfoDataTable = query.GetData

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_013:ストールロック登録処理
        ''' </summary>
        ''' <param name="inStallId">ストールID</param>
        ''' <param name="inLockDate">対象ロック日付</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inSystem">プログラムID</param>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function RegisterDBStallLock(ByVal inStallId As Decimal, _
                                            ByVal inLockDate As Date, _
                                            ByVal inAccount As String, _
                                            ByVal inNowDate As Date, _
                                            ByVal inSystem As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inStallId, inLockDate, inAccount, inNowDate, inSystem))

            Using query As New DBUpdateQuery("SMBCommonClass_013")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("INSERT /* SMBCommonClass_013 */ ")
                    .AppendLine("  INTO TB_T_STALL_LOCK( ")
                    .AppendLine("       STALL_ID ")
                    .AppendLine("      ,LOCK_DATE ")
                    .AppendLine("      ,ROW_CREATE_DATETIME ")
                    .AppendLine("      ,ROW_CREATE_ACCOUNT ")
                    .AppendLine("      ,ROW_CREATE_FUNCTION ")
                    .AppendLine("      ,ROW_UPDATE_DATETIME ")
                    .AppendLine("      ,ROW_UPDATE_ACCOUNT ")
                    .AppendLine("      ,ROW_UPDATE_FUNCTION ")
                    .AppendLine("      ,ROW_LOCK_VERSION ")
                    .AppendLine(" )VALUES( ")
                    .AppendLine("       :STALL_ID ")
                    .AppendLine("      ,TRUNC(:LOCK_DATE) ")
                    .AppendLine("      ,:NOWDATE ")
                    .AppendLine("      ,:ACCOUNT ")
                    .AppendLine("      ,:SYSTEM ")
                    .AppendLine("      ,:NOWDATE ")
                    .AppendLine("      ,:ACCOUNT ")
                    .AppendLine("      ,:SYSTEM ")
                    .AppendLine("      ,0 ")
                    .AppendLine("      ) ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, inStallId)
                query.AddParameterWithTypeValue("LOCK_DATE", OracleDbType.Date, inLockDate)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, inSystem)

                Dim updateCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , updateCount))
                Return updateCount
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_014:ストールロック削除処理
        ''' </summary>
        ''' <param name="inStallId">サービス入庫ID</param>
        ''' <param name="inLockDate">ロック対象日付</param>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function DeleteDBStallLock(ByVal inStallId As Decimal, _
                                          ByVal inLockDate As Date) As Integer
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inStallId, inLockDate))

            Using query As New DBUpdateQuery("SMBCommonClass_014")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("DELETE /* SMBCommonClass_014 */ ")
                    .AppendLine("       TB_T_STALL_LOCK T1 ")
                    .AppendLine(" WHERE T1.STALL_ID = :STALL_ID ")
                    .AppendLine("   AND T1.LOCK_DATE = TRUNC(:LOCK_DATE) ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, inStallId)
                query.AddParameterWithTypeValue("LOCK_DATE", OracleDbType.Date, inLockDate)

                Dim updateCount As Integer = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , updateCount))
                Return updateCount
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_015:ストールロックテーブル取得処理
        ''' </summary>
        ''' <param name="inStallId">ストールID</param>
        ''' <param name="inLockDate">ロック対象日付</param>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function GetLockTableCount(ByVal inStallId As Decimal, _
                                          ByVal inLockDate As Date) As SMBCommonClassDataSet.StallLockInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inStallId.ToString(CultureInfo.CurrentCulture) _
                        , inLockDate.ToString(CultureInfo.CurrentCulture)))

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.StallLockInfoDataTable)("SMBCommonClass_015")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SMBCommonClass_015 */ ")
                    .AppendLine("       T1.STALL_ID ")
                    .AppendLine("      ,T1.LOCK_DATE ")
                    .AppendLine("      ,T1.ROW_UPDATE_DATETIME ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_T_STALL_LOCK T1 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.STALL_ID = :STALL_ID ")
                    .AppendLine("   AND T1.LOCK_DATE = TRUNC(:LOCK_DATE) ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, inStallId)
                query.AddParameterWithTypeValue("LOCK_DATE", OracleDbType.Date, inLockDate)

                Dim dt As SMBCommonClassDataSet.StallLockInfoDataTable = query.GetData

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_016:顧客テーブル行ロックバージョン更新処理
        ''' </summary>
        ''' <param name="inCustomerId">顧客ID</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inSystem">プログラムID</param>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function UpdateDBCustomerLockVersion(ByVal inCustomerId As Decimal, _
                                                    ByVal inRowLockVersion As Long, _
                                                    ByVal inAccount As String, _
                                                    ByVal inNowDate As Date, _
                                                    ByVal inSystem As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inCustomerId, inAccount, inNowDate, inSystem))

            Using query As New DBUpdateQuery("SMBCommonClass_016")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("UPDATE /* SMBCommonClass_016 */ ")
                    .AppendLine("       TB_M_CUSTOMER T1 ")
                    .AppendLine("   SET T1.ROW_UPDATE_DATETIME = :NOWDATE ")
                    .AppendLine("      ,T1.ROW_UPDATE_ACCOUNT = :ACCOUNT ")
                    .AppendLine("      ,T1.ROW_UPDATE_FUNCTION = :SYSTEM ")
                    .AppendLine("      ,T1.ROW_LOCK_VERSION = T1.ROW_LOCK_VERSION + 1 ")
                    .AppendLine(" WHERE T1.CST_ID = :CST_ID ")
                    .AppendLine("   AND T1.ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, inSystem)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerId)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, inRowLockVersion)

                Dim updateCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , updateCount))
                Return updateCount
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_017:顧客テーブルロック処理
        ''' </summary>
        ''' <param name="inCustomerId">顧客ID</param>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function LockDBCustomerTable(ByVal inCustomerId As Decimal) As SMBCommonClassDataSet.LockInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inCustomerId))

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.LockInfoDataTable)("SMBCommonClass_017")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SMBCommonClass_017 */ ")
                    .AppendLine("       T1.ROW_LOCK_VERSION ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_M_CUSTOMER T1 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.CST_ID = :CST_ID ")
                    .AppendLine("   FOR UPDATE WAIT 1 ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerId)

                Dim dt As SMBCommonClassDataSet.LockInfoDataTable = query.GetData

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_018:ストールロック登録処理
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inRequestId">用件ID</param>
        ''' <param name="inAttraction">誘致ID</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inSystem">プログラムID</param>
        ''' <returns>処理件数</returns>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function RegisterDBServiceInRequestAttraction(ByVal inServiceInId As Decimal, _
                                                             ByVal inRequestId As Decimal, _
                                                             ByVal inAttraction As Decimal, _
                                                             ByVal inNowDate As Date, _
                                                             ByVal inAccount As String, _
                                                             ByVal inSystem As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inRequestId.ToString(CultureInfo.CurrentCulture) _
                        , inAttraction.ToString(CultureInfo.CurrentCulture) _
                        , inNowDate.ToString(CultureInfo.CurrentCulture), inAccount, inSystem))

            Using query As New DBUpdateQuery("SMBCommonClass_018")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("INSERT /* SMBCommonClass_018 */ ")
                    .AppendLine("  INTO TB_T_SERVICEIN_REQ_ATT( ")
                    .AppendLine("       SVCIN_REQ_ATT_ID ")
                    .AppendLine("      ,SVCIN_ID ")
                    .AppendLine("      ,REQ_ID ")
                    .AppendLine("      ,ATT_ID ")
                    .AppendLine("      ,REG_FUNCTION_TYPE ")
                    .AppendLine("      ,ROW_CREATE_DATETIME ")
                    .AppendLine("      ,ROW_CREATE_ACCOUNT ")
                    .AppendLine("      ,ROW_CREATE_FUNCTION ")
                    .AppendLine("      ,ROW_UPDATE_DATETIME ")
                    .AppendLine("      ,ROW_UPDATE_ACCOUNT ")
                    .AppendLine("      ,ROW_UPDATE_FUNCTION ")
                    .AppendLine("      ,ROW_LOCK_VERSION ")
                    .AppendLine(") VALUES ( ")
                    .AppendLine("       SQ_SVCIN_REQ_ATT_ID.NEXTVAL ")
                    .AppendLine("      ,:SVCIN_ID ")
                    .AppendLine("      ,:REQ_ID ")
                    .AppendLine("      ,:ATT_ID ")
                    .AppendLine("      ,:REG_FUNCTION_TYPE_1 ")
                    .AppendLine("      ,:NOWDATE ")
                    .AppendLine("      ,:ACCOUNT ")
                    .AppendLine("      ,:SYSTEM ")
                    .AppendLine("      ,:NOWDATE ")
                    .AppendLine("      ,:ACCOUNT ")
                    .AppendLine("      ,:SYSTEM ")
                    .AppendLine("      ,0 ")
                    .AppendLine(") ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, inRequestId)
                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, inAttraction)
                query.AddParameterWithTypeValue("REG_FUNCTION_TYPE_1", OracleDbType.NVarchar2, RegisterFunctionTypeDisplay)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, inSystem)

                Dim updateCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , updateCount))
                Return updateCount
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_020:サービス入庫IDと作業内容ID(最小値)取得処理
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inJobDetailId">作業内容ID</param>
        ''' <returns>サービス入庫IDと作業内容ID(最小値)</returns>
        ''' <history>
        ''' </history>
        ''' <remarks></remarks>
        Public Function GetServiceinIdJobDetailMinId(ByVal inServiceInId As Decimal, _
                                                     ByVal inJobDetailId As Decimal) As SMBCommonClassDataSet.ServiceinIdJobDetailMinIdDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                        , inJobDetailId.ToString(CultureInfo.CurrentCulture)))

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.ServiceinIdJobDetailMinIdDataTable)("SMBCommonClass_020")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    sql.AppendLine("SELECT /* SMBCommonClass_020 */ ")
                    sql.AppendLine("       T1.SVCIN_ID ")
                    sql.AppendLine("      ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID_MIN ")
                    sql.AppendLine("  FROM ")
                    sql.AppendLine("       TB_T_SERVICEIN T1 ")
                    sql.AppendLine("      ,TB_T_JOB_DTL T2 ")
                    sql.AppendLine(" WHERE ")
                    sql.AppendLine("       T1.SVCIN_ID=T2.SVCIN_ID ")
                    '引数確認
                    If Not (IsNothing(inServiceInId)) AndAlso inServiceInId > 0 Then
                        'サービス入庫IDが存在する場合
                        'サービス入庫IDの条件追加
                        sql.AppendLine("   AND T1.SVCIN_ID = :SVCIN_ID ")
                        query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                    ElseIf Not (IsNothing(inJobDetailId)) AndAlso inJobDetailId > 0 Then
                        '作業内容IDが存在する場合
                        '作業内容IDの条件追加
                        sql.AppendLine("   AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
                        query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDetailId)

                    End If
                    sql.AppendLine(" GROUP BY T1.SVCIN_ID ")
                End With

                query.CommandText = sql.ToString()

                Dim dt As SMBCommonClassDataSet.ServiceinIdJobDetailMinIdDataTable = query.GetData

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))
                Return dt
            End Using
        End Function

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

#End Region

#Region "SMBCommonSAChangeClassBusinessLogic.vb"

        ''' <summary>
        ''' SMBCommonClass_004:担当SA変更
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inServiceInId">サービス入庫</param>
        ''' <param name="inSACode">変更後担当SA</param>
        ''' <param name="inAccount">更新者</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <param name="inSystem">プログラムID</param>
        ''' <returns>登録結果</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function UpdateReserveSACode(ByVal inDealerCode As String, _
                                            ByVal inStoreCode As String, _
                                            ByVal inServiceInId As Decimal, _
                                            ByVal inSACode As String, _
                                            ByVal inAccount As String, _
                                            ByVal inPresentTime As DateTime, _
                                            ByVal inSystem As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inDealerCode, inStoreCode, inServiceInId, inSACode, inAccount, inPresentTime, inSystem))
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'Public Function UpdateReserveSACode(ByVal inDealerCode As String, _
            '                                      ByVal inStoreCode As String, _
            '                                      ByVal inReserveId As Long, _
            '                                      ByVal inSACode As String, _
            '                                      ByVal inAccount As String, _
            '                                      ByVal inPresentTime As DateTime) As Long
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '          , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7}" _
            '          , Me.GetType.ToString _
            '          , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '          , inDealerCode, inStoreCode, inReserveId, inSACode, inAccount, inPresentTime))
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            ''SQLの設定
            Dim sql As New StringBuilder
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'sql.AppendLine("UPDATE /* SMBCommonClass_004 */")
            'sql.AppendLine("       TBL_STALLREZINFO T1")
            'sql.AppendLine("   SET ")
            'sql.AppendLine("       T1.UPDATE_COUNT = T1.UPDATE_COUNT + 1")
            'sql.AppendLine("      ,T1.ACCOUNT_PLAN = :ACCOUNT_PLAN")
            'sql.AppendLine("      ,T1.UPDATEDATE = :UPDATEDATE")
            'sql.AppendLine("      ,T1.UPDATEACCOUNT = :UPDATEACCOUNT")
            'sql.AppendLine(" WHERE T1.DLRCD = :DLRCD")
            'sql.AppendLine("   AND T1.STRCD = :STRCD")
            'sql.AppendLine("   AND (T1.REZID = :REZID ")
            'sql.AppendLine("    OR T1.PREZID = :REZID) ")
            'sql.AppendLine("   AND NOT EXISTS (")
            'sql.AppendLine("            SELECT 1")
            'sql.AppendLine("              FROM TBL_STALLREZINFO T2")
            'sql.AppendLine("             WHERE  T2.DLRCD = T1.DLRCD")
            'sql.AppendLine("               AND  T2.STRCD = T1.STRCD")
            'sql.AppendLine("               AND  T2.REZID = T1.REZID")
            'sql.AppendLine("               AND  T2.STOPFLG = '0'")
            'sql.AppendLine("               AND  T2.CANCELFLG = '1'")
            'sql.AppendLine("                  )")
            sql.AppendLine("UPDATE /* SMBCommonClass_004 */ ")
            sql.AppendLine("       TB_T_SERVICEIN T1 ")
            sql.AppendLine("   SET T1.PIC_SA_STF_CD = :PIC_SA_STF_CD ")
            sql.AppendLine(" WHERE T1.SVCIN_ID = :SVCIN_ID ")
            sql.AppendLine("   AND T1.DLR_CD = :DLR_CD ")
            sql.AppendLine("   AND T1.BRN_CD = :BRN_CD ")
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            Using query As New DBUpdateQuery("SMBCommonClass_004")
                query.CommandText = sql.ToString()
                ''パラメータの設定           
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inReserveId)
                'query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Varchar2, inSACode)
                'query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inPresentTime)
                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
                query.AddParameterWithTypeValue("PIC_SA_STF_CD", OracleDbType.NVarchar2, inSACode)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inStoreCode)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''SQLの実行
                Dim updateCount As Integer = query.Execute()
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , updateCount))
                Return updateCount
            End Using
        End Function

#End Region

#Region "SMBCommonDetailClassBusinessLogic.vb"

        ''' <summary>
        ''' SMBCommonClass_007:チップ詳細情報取得(来店)
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <returns>チップ詳細情報取得(来店)</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' </history>
        Public Function GetChipDetailVisitData(ByVal inDealerCode As String, _
                                               ByVal inStoreCode As String, _
                                               ByVal inVisitSequence As Decimal) As SMBCommonClassDataSet.ChipDetailVisitDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inVisitSequence.ToString(CultureInfo.InvariantCulture)))
            ''SQLの設定
            Dim sql As New StringBuilder

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'sql.Append("SELECT /* SMBCommonClass_007 */ ")
            'sql.Append("       VISITSEQ, ")
            'sql.Append("       DLRCD, ")
            'sql.Append("       STRCD, ")
            'sql.Append("       FREZID, ")
            'sql.Append("       VCLREGNO, ")
            'sql.Append("       VIN, ")
            'sql.Append("       NAME, ")
            'sql.Append("       TELNO, ")
            'sql.Append("       MOBILE, ")
            'sql.Append("       CUSTSEGMENT, ")
            'sql.Append("       ORDERNO, ")
            ''2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
            'sql.Append("       UPDATEDATE, ")
            'sql.Append("       CALLNO, ")
            'sql.Append("       CALLPLACE, ")
            'sql.Append("       CALLSTATUS, ")
            'sql.Append("       VISITNAME, ")
            'sql.Append("       VISITTELNO ")
            ''2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
            'sql.Append("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
            'sql.Append(" WHERE VISITSEQ = :VISITSEQ ")
            'sql.Append("   AND DLRCD = :DLRCD ")
            'sql.Append("   AND STRCD = :STRCD ")

            sql.AppendLine("SELECT /* SMBCommonClass_007 */ ")
            sql.AppendLine("       T1.VISITSEQ ")
            sql.AppendLine("      ,T1.DLRCD ")
            sql.AppendLine("      ,T1.STRCD ")
            sql.AppendLine("      ,T1.FREZID ")
            sql.AppendLine("      ,T1.VCLREGNO ")
            sql.AppendLine("      ,T1.VIN ")
            sql.AppendLine("      ,T1.NAME ")
            sql.AppendLine("      ,T1.TELNO ")
            sql.AppendLine("      ,T1.MOBILE ")
            sql.AppendLine("      ,T1.CUSTSEGMENT ")
            sql.AppendLine("      ,T1.ORDERNO ")
            sql.AppendLine("      ,T1.UPDATEDATE ")
            sql.AppendLine("      ,T1.CALLNO ")
            sql.AppendLine("      ,T1.CALLPLACE ")
            sql.AppendLine("      ,T1.CALLSTATUS ")
            sql.AppendLine("      ,T1.VISITNAME ")
            sql.AppendLine("      ,T1.VISITTELNO ")
            sql.AppendLine("      ,T1.CUSTID ")
            sql.AppendLine("      ,T1.VCL_ID ")
            sql.AppendLine("      ,T1.ASSIGNSTATUS ")
            sql.AppendLine("      ,NVL2(T2.VISIT_ID, :RO_TYPE_1, :RO_TYPE_0) RO_TYPE ")
            sql.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT T1 ")
            sql.AppendLine("      ,(SELECT MIN(R1.VISIT_ID) AS VISIT_ID ")
            sql.AppendLine("          FROM TB_T_RO_INFO R1 ")
            sql.AppendLine("         WHERE R1.VISIT_ID = :VISITSEQ) T2 ")
            sql.AppendLine(" WHERE T1.VISITSEQ = T2.VISIT_ID(+) ")
            sql.AppendLine("   AND T1.VISITSEQ = :VISITSEQ ")
            sql.AppendLine("   AND T1.DLRCD = :DLRCD ")
            sql.AppendLine("   AND T1.STRCD = :STRCD ")
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.ChipDetailVisitDataTable)("SMBCommonClass_007")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, inVisitSequence)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                query.AddParameterWithTypeValue("RO_TYPE_1", OracleDbType.NVarchar2, RepairOrderTypeExist)
                query.AddParameterWithTypeValue("RO_TYPE_0", OracleDbType.NVarchar2, RepairOrderTypeNone)
                '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                ''SQLの実行
                Using dt As SMBCommonClassDataSet.ChipDetailVisitDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_008:チップ詳細情報取得(実績)
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>チップ詳細情報取得(実績)</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function GetChipDetailProcessData(ByVal inDealerCode As String, _
                                                 ByVal inStoreCode As String, _
                                                 ByVal inServiceInId As Decimal) As SMBCommonClassDataSet.ChipDetailProcessDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inServiceInId.ToString(CultureInfo.InvariantCulture)))
            ''SQLの設定
            Dim sql As New StringBuilder
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'sql.Append("SELECT /* SMBCommonClass_008 */ ")
            'sql.Append("       T6.PREZID, ")
            'sql.Append("       MIN(T6.ACTUAL_STIME) AS STARTTIME, ")
            'sql.Append("       MAX(NVL2(T6.RESULT_END_TIME,TO_DATE(T6.RESULT_END_TIME,'YYYYMMDDHH24MI'),T6.ENDTIME)) AS ENDTIME, ")
            'sql.Append("       SUM(DECODE(T6.RESULT_STATUS,'0' ,T6.REZ_WORK_TIME, ")
            'sql.Append("           DECODE(T6.RESULT_STATUS,'00',T6.REZ_WORK_TIME, ")
            'sql.Append("           DECODE(T6.RESULT_STATUS,'10',T6.REZ_WORK_TIME, ")
            'sql.Append("           DECODE(T6.RESULT_STATUS,NULL,T6.REZ_WORK_TIME,0))))) AS WORKTIME, ")
            'sql.Append("       SUM(DECODE(NVL(T6.STOPFLG,0),0,0,1)) AS STOPCOUNT, ")
            'sql.Append("       MAX(DECODE(T6.NUM,1,T6.RESULT_STATUS,NULL)) AS RESULT_STATUS, ")
            'sql.Append("       MAX(T6.WASHFLG) AS WASHFLG, ")
            'sql.Append("       MAX(T6.RESULT_WASH_START) AS RESULT_WASH_START, ")
            'sql.Append("       MAX(T6.RESULT_WASH_END) AS RESULT_WASH_END, ")
            'sql.Append("       MAX(INSTRUCT) AS INSTRUCT, ")
            'sql.Append("       DECODE(MIN(T6.NUM),0,'0','1') AS INDEXNUMBER")
            'sql.Append("  FROM ")
            'sql.Append("    (SELECT NVL(T2.PREZID,T2.REZID) AS PREZID, ")
            'sql.Append("            T2.STARTTIME, ")
            'sql.Append("            T2.ENDTIME, ")
            'sql.Append("            T2.ACTUAL_STIME, ")
            'sql.Append("            T2.ACTUAL_ETIME, ")
            'sql.Append("            T2.REZ_WORK_TIME, ")
            'sql.Append("            T2.WASHFLG, ")
            'sql.Append("            T2.STOPFLG, ")
            'sql.Append("            T2.CANCELFLG, ")
            'sql.Append("            T2.REZCHILDNO, ")
            'sql.Append("            T2.INSTRUCT, ")
            'sql.Append("            T1.RESULT_STATUS, ")
            'sql.Append("            T1.RESULT_WASH_START, ")
            'sql.Append("            T1.RESULT_WASH_END, ")
            'sql.Append("            T1.RESULT_END_TIME, ")
            'sql.Append("            ROW_NUMBER() OVER ( ")
            'sql.Append("                PARTITION BY NVL(T2.PREZID,T2.REZID) ")
            'sql.Append("                ORDER BY ENDTIME DESC, ")
            'sql.Append("                         STARTTIME DESC) NUM ")
            'sql.Append("       FROM TBL_STALLPROCESS T1, ")
            'sql.Append("            TBL_STALLREZINFO T2 ")
            'sql.Append("      WHERE T2.DLRCD = :DLRCD ")
            'sql.Append("        AND T2.STRCD = :STRCD ")
            'sql.Append("        AND (T2.REZID = :REZID ")
            'sql.Append("         OR T2.PREZID = :REZID) ")
            'sql.Append("        AND DECODE(T2.STOPFLG,'0',DECODE(T2.CANCELFLG,'1',1,0),0) + ")
            'sql.Append("            DECODE(T2.REZCHILDNO,0,1,0) + ")
            'sql.Append("            DECODE(T2.REZCHILDNO,999,1,0) = 0 ")
            'sql.Append("        AND T2.DLRCD = T1.DLRCD ")
            'sql.Append("        AND T2.STRCD = T1.STRCD ")
            'sql.Append("        AND T2.REZID = T1.REZID ")
            'sql.Append("        AND T1.SEQNO = ( ")
            'sql.Append("            SELECT MAX(T3.SEQNO) ")
            'sql.Append("              FROM TBL_STALLPROCESS T3 ")
            'sql.Append("             WHERE T1.DLRCD = T3.DLRCD ")
            'sql.Append("               AND T1.STRCD = T3.STRCD ")
            'sql.Append("               AND T1.REZID = T3.REZID ")
            'sql.Append("               AND T1.DSEQNO = T3.DSEQNO) ")
            'sql.Append("        AND T1.DSEQNO = ( ")
            'sql.Append("            SELECT MAX(T4.DSEQNO) ")
            'sql.Append("              FROM TBL_STALLPROCESS T4 ")
            'sql.Append("             WHERE T1.DLRCD = T4.DLRCD ")
            'sql.Append("               AND T1.STRCD = T4.STRCD ")
            'sql.Append("               AND T1.REZID = T4.REZID) ")
            'sql.Append("      UNION ALL ")
            'sql.Append("     SELECT NVL(T4.PREZID,T4.REZID) AS PREZID, ")
            'sql.Append("            T4.STARTTIME, ")
            'sql.Append("            T4.ENDTIME, ")
            'sql.Append("            T4.ACTUAL_STIME, ")
            'sql.Append("            T4.ACTUAL_ETIME, ")
            'sql.Append("            T4.REZ_WORK_TIME, ")
            'sql.Append("            T4.WASHFLG, ")
            'sql.Append("            T4.STOPFLG, ")
            'sql.Append("            T4.CANCELFLG, ")
            'sql.Append("            T4.REZCHILDNO, ")
            'sql.Append("            T4.INSTRUCT, ")
            'sql.Append("            NULL, ")
            'sql.Append("            NULL, ")
            'sql.Append("            NULL, ")
            'sql.Append("            NULL, ")
            'sql.Append("            0 ")
            'sql.Append("       FROM TBL_STALLREZINFO T4 ")
            'sql.Append("      WHERE T4.DLRCD = :DLRCD ")
            'sql.Append("        AND T4.STRCD = :STRCD ")
            'sql.Append("        AND (T4.REZID = :REZID ")
            'sql.Append("         OR T4.PREZID = :REZID) ")
            'sql.Append("        AND DECODE(T4.STOPFLG,'0',DECODE(T4.CANCELFLG,'1',1,0),0) + ")
            'sql.Append("            DECODE(T4.REZCHILDNO,0,1,0) + ")
            'sql.Append("            DECODE(T4.REZCHILDNO,999,1,0) = 0 ")
            'sql.Append("        AND NOT EXISTS ( ")
            'sql.Append("                SELECT '1' ")
            'sql.Append("                  FROM TBL_STALLPROCESS T5 ")
            'sql.Append("                 WHERE T4.DLRCD = T5.DLRCD ")
            'sql.Append("                   AND T4.STRCD = T5.STRCD ")
            'sql.Append("                   AND T4.REZID = T5.REZID)  ")
            'sql.Append("    ) T6 ")
            'sql.Append(" GROUP BY T6.PREZID ")
            sql.AppendLine("SELECT /* SMBCommonClass_008 */ ")
            sql.AppendLine("       T1.SVCIN_ID ")
            sql.AppendLine("      ,MIN(DECODE(T1.RSLT_START_DATETIME, ")
            sql.AppendLine("                  :MINDATE, ")
            sql.AppendLine("                  TO_DATE(NULL), ")
            sql.AppendLine("                  T1.RSLT_START_DATETIME)) AS STARTTIME ")
            sql.AppendLine("      ,MAX(CASE ")
            sql.AppendLine("           WHEN T1.RSLT_END_DATETIME <> :MINDATE THEN T1.RSLT_END_DATETIME ")
            sql.AppendLine("           WHEN T1.PRMS_END_DATETIME <> :MINDATE THEN T1.PRMS_END_DATETIME ")
            sql.AppendLine("           ELSE T1.SCHE_END_DATETIME END) AS ENDTIME ")
            sql.AppendLine("      ,SUM(DECODE(T1.STALL_USE_STATUS, :STALL_USE_STATUS_00, T1.SCHE_WORKTIME, ")
            sql.AppendLine("           DECODE(T1.STALL_USE_STATUS, :STALL_USE_STATUS_01, T1.SCHE_WORKTIME, 0))) AS WORKTIME ")
            sql.AppendLine("      ,SUM(DECODE(T1.STALL_USE_STATUS, :STALL_USE_STATUS_05, 1, 0)) AS STOPCOUNT ")
            sql.AppendLine("      ,MAX(T1.SVC_STATUS) AS RESULT_STATUS ")
            sql.AppendLine("      ,MAX(NVL2(TRIM(T1.CARWASH_NEED_FLG), T1.CARWASH_NEED_FLG, 0)) AS WASHFLG ")
            sql.AppendLine("      ,DECODE(MAX(T2.RSLT_START_DATETIME), ")
            sql.AppendLine("              :MINDATE, ")
            sql.AppendLine("              NULL, ")
            sql.AppendLine("              TO_CHAR(MAX(T2.RSLT_START_DATETIME), 'YYYYMMDDHH24MI')) AS RESULT_WASH_START ")
            sql.AppendLine("      ,DECODE(MAX(T2.RSLT_END_DATETIME), ")
            sql.AppendLine("              :MINDATE, ")
            sql.AppendLine("              NULL, ")
            sql.AppendLine("              TO_CHAR(MAX(T2.RSLT_END_DATETIME), 'YYYYMMDDHH24MI')) AS RESULT_WASH_END ")
            sql.AppendLine("      ,MAX(T1.STALL_USE_STATUS) AS INSTRUCT ")
            sql.AppendLine("      ,MAX(DECODE(T1.RSLT_START_DATETIME, :MINDATE, 0, 1)) AS INDEXNUMBER ")
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            sql.AppendLine("      ,MIN(DECODE(T1.INSPECTION_NEED_FLG, 1, T1.INSPECTION_STATUS, 2)) AS REMAINING_INSPECTION_TYPE")
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            sql.AppendLine("  FROM ")
            sql.AppendLine("       (SELECT M1.SVCIN_ID ")
            sql.AppendLine("              ,M1.CARWASH_NEED_FLG ")
            sql.AppendLine("              ,M1.SVC_STATUS ")
            sql.AppendLine("              ,M3.SCHE_END_DATETIME ")
            sql.AppendLine("              ,M3.JOB_ID ")
            sql.AppendLine("              ,M3.SCHE_WORKTIME ")
            sql.AppendLine("              ,M3.RSLT_START_DATETIME ")
            sql.AppendLine("              ,M3.PRMS_END_DATETIME ")
            sql.AppendLine("              ,M3.RSLT_END_DATETIME ")
            sql.AppendLine("              ,M3.STALL_USE_STATUS ")
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            sql.AppendLine("              ,M2.INSPECTION_NEED_FLG ")
            sql.AppendLine("              ,M2.INSPECTION_STATUS ")
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            sql.AppendLine("              ,ROW_NUMBER() OVER ( ")
            sql.AppendLine("               PARTITION BY M1.SVCIN_ID ")
            sql.AppendLine("               ORDER BY M3.SCHE_END_DATETIME DESC ")
            sql.AppendLine("              ,M3.SCHE_START_DATETIME DESC) NUM ")
            sql.AppendLine("         FROM TB_T_SERVICEIN M1 ")
            sql.AppendLine("             ,TB_T_JOB_DTL M2 ")
            sql.AppendLine("             ,TB_T_STALL_USE M3 ")
            sql.AppendLine("             ,(SELECT Y1.SVCIN_ID ")
            sql.AppendLine("                     ,Y2.JOB_DTL_ID ")
            sql.AppendLine("                     ,MAX(Y3.STALL_USE_ID) AS STALL_USE_ID_MAX ")
            sql.AppendLine("                 FROM TB_T_SERVICEIN Y1 ")
            sql.AppendLine("                     ,TB_T_JOB_DTL Y2 ")
            sql.AppendLine("                     ,TB_T_STALL_USE Y3 ")
            sql.AppendLine("                WHERE Y1.SVCIN_ID = Y2.SVCIN_ID ")
            sql.AppendLine("                  AND Y2.JOB_DTL_ID = Y3.JOB_DTL_ID ")
            sql.AppendLine("                  AND Y1.SVCIN_ID = :SVCIN_ID ")
            sql.AppendLine("                GROUP BY Y1.SVCIN_ID ")
            sql.AppendLine("                        ,Y2.JOB_DTL_ID) M4 ")
            sql.AppendLine("        WHERE M1.SVCIN_ID = M2.SVCIN_ID ")
            sql.AppendLine("          AND M2.JOB_DTL_ID = M3.JOB_DTL_ID ")
            sql.AppendLine("          AND M1.SVCIN_ID = M4.SVCIN_ID ")
            sql.AppendLine("          AND M2.JOB_DTL_ID = M4.JOB_DTL_ID ")
            sql.AppendLine("          AND M3.STALL_USE_ID = M4.STALL_USE_ID_MAX ")
            sql.AppendLine("          AND M1.SVCIN_ID = :SVCIN_ID ")
            sql.AppendLine("          AND M2.DLR_CD = :DLR_CD ")
            sql.AppendLine("          AND M2.BRN_CD = :BRN_CD ")
            sql.AppendLine("          AND M1.SVC_STATUS <> :SVC_STATUS_02 ")
            sql.AppendLine("          AND M2.CANCEL_FLG = :CANCEL_FLG_0) T1 ")
            sql.AppendLine("      ,TB_T_CARWASH_RESULT T2 ")
            sql.AppendLine(" WHERE ")
            sql.AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID(+) ")
            sql.AppendLine(" GROUP BY T1.SVCIN_ID ")
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.ChipDetailProcessDataTable)("SMBCommonClass_008")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, inFirstReserveId)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("STALL_USE_STATUS_00", OracleDbType.NVarchar2, StallUseStatusWaitInstruct)
                query.AddParameterWithTypeValue("STALL_USE_STATUS_01", OracleDbType.NVarchar2, StallUseStatusWaitActual)
                query.AddParameterWithTypeValue("STALL_USE_STATUS_05", OracleDbType.NVarchar2, StallUseStatusStop)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inStoreCode)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''SQLの実行
                Using dt As SMBCommonClassDataSet.ChipDetailProcessDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_009:中断情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>中断情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function GetStopData(ByVal inDealerCode As String, _
                                    ByVal inStoreCode As String, _
                                    ByVal inServiceInId As Decimal) As SMBCommonClassDataSet.StopDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inServiceInId.ToString(CultureInfo.InvariantCulture)))
            ''SQLの設定
            Dim sql As New StringBuilder
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'sql.Append("SELECT /* SMBCommonClass_009 */ ")
            'sql.Append("       T2.RESULT_END_TIME, ")
            'sql.Append("       T2.RESULT_STATUS, ")
            'sql.Append("       T2.STOPMEMO ")
            'sql.Append("  FROM TBL_STALLREZINFO T1, ")
            'sql.Append("       TBL_STALLPROCESS T2 ")
            'sql.Append(" WHERE T1.DLRCD = T2.DLRCD ")
            'sql.Append("   AND T1.STRCD = T2.STRCD ")
            'sql.Append("   AND T1.REZID = T2.REZID ")
            'sql.Append("   AND T1.DLRCD = :DLRCD ")
            'sql.Append("   AND T1.STRCD = :STRCD ")
            'sql.Append("   AND (T1.REZID = :REZID OR T1.PREZID = :REZID) ")
            'sql.Append("   AND T1.STOPFLG = '1' ")
            'sql.Append("   AND T2.DSEQNO = ( ")
            'sql.Append("                    SELECT MAX(T3.DSEQNO) ")
            'sql.Append("                      FROM TBL_STALLPROCESS T3 ")
            'sql.Append("                     WHERE T3.DLRCD = T2.DLRCD ")
            'sql.Append("                       AND T3.STRCD = T2.STRCD ")
            'sql.Append("                       AND T3.REZID = T2.REZID) ")
            'sql.Append("   AND T2.SEQNO = ( ")
            'sql.Append("                    SELECT MAX(T4.SEQNO) ")
            'sql.Append("                      FROM TBL_STALLPROCESS T4 ")
            'sql.Append("                     WHERE T4.DLRCD = T2.DLRCD")
            'sql.Append("                       AND T4.STRCD = T2.STRCD ")
            'sql.Append("                       AND T4.REZID = T2.REZID ")
            'sql.Append("                       AND T4.DSEQNO = T2.DSEQNO) ")
            sql.AppendLine("SELECT /* SMBCommonClass_009 */ ")
            sql.AppendLine("       DECODE(T3.RSLT_END_DATETIME, ")
            sql.AppendLine("              :MINDATE, ")
            sql.AppendLine("              NULL, ")
            sql.AppendLine("              TO_CHAR(T3.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_END_TIME ")
            sql.AppendLine("      ,T3.STOP_REASON_TYPE AS RESULT_STATUS ")
            sql.AppendLine("      ,T3.STOP_MEMO AS STOPMEMO ")
            sql.AppendLine("  FROM ")
            sql.AppendLine("       TB_T_SERVICEIN T1 ")
            sql.AppendLine("      ,TB_T_JOB_DTL T2 ")
            sql.AppendLine("      ,TB_T_STALL_USE T3 ")
            sql.AppendLine("      ,(SELECT Y1.SVCIN_ID ")
            sql.AppendLine("              ,Y2.JOB_DTL_ID ")
            sql.AppendLine("              ,MAX(Y3.STALL_USE_ID) AS STALL_USE_ID_MAX ")
            sql.AppendLine("          FROM TB_T_SERVICEIN Y1  ")
            sql.AppendLine("              ,TB_T_JOB_DTL Y2  ")
            sql.AppendLine("              ,TB_T_STALL_USE Y3  ")
            sql.AppendLine("        WHERE Y1.SVCIN_ID = Y2.SVCIN_ID ")
            sql.AppendLine("          AND Y2.JOB_DTL_ID = Y3.JOB_DTL_ID ")
            sql.AppendLine("          AND Y1.SVCIN_ID = :SVCIN_ID ")
            sql.AppendLine("        GROUP BY Y1.SVCIN_ID ")
            sql.AppendLine("                ,Y2.JOB_DTL_ID ")
            sql.AppendLine("       ) T4 ")
            sql.AppendLine(" WHERE ")
            sql.AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
            sql.AppendLine("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
            sql.AppendLine("   AND T1.SVCIN_ID = T4.SVCIN_ID ")
            sql.AppendLine("   AND T2.JOB_DTL_ID = T4.JOB_DTL_ID ")
            sql.AppendLine("   AND T3.STALL_USE_ID = T4.STALL_USE_ID_MAX ")
            sql.AppendLine("   AND T1.SVCIN_ID = :SVCIN_ID ")
            sql.AppendLine("   AND T1.SVC_STATUS <> :SVC_STATUS_02 ")
            sql.AppendLine("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
            sql.AppendLine("   AND T3.STALL_USE_STATUS = :STALL_USE_STATUS_05 ")
            sql.AppendLine(" ORDER BY RESULT_END_TIME ASC ")
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.StopDataTable)("SMBCommonClass_009")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inReserveId)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("STALL_USE_STATUS_05", OracleDbType.NVarchar2, StallUseStatusStop)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''SQLの実行
                Using dt As SMBCommonClassDataSet.StopDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_005:チップ詳細情報取得(予約)
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>チップ詳細情報取得(予約)</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' 2014/02/08 TMEJ 小澤 BTS対応
        ''' 2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001
        ''' </history>
        Public Function GetChipDetailReserveData(ByVal inDealerCode As String, _
                                                 ByVal inStoreCode As String, _
                                                 ByVal inServiceInId As Decimal) As SMBCommonClassDataSet.ChipDetailReserveDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inServiceInId.ToString(CultureInfo.InvariantCulture)))
            ''SQLの設定
            Dim sql As New StringBuilder

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'sql.Append("SELECT /* SMBCommonClass_005 */ ")
            'sql.Append("       T1.DLRCD, ")
            'sql.Append("       T1.STRCD, ")
            'sql.Append("       T1.REZID, ")
            'sql.Append("       T1.VEHICLENAME, ")
            'sql.Append("       T1.VCLREGNO, ")
            'sql.Append("       T1.CUSTOMERNAME, ")
            'sql.Append("       T1.TELNO, ")
            'sql.Append("       T1.MOBILE, ")
            'sql.Append("       DECODE(T1.CUSTOMERFLAG,'0','1',DECODE(T1.CUSTOMERFLAG,'1','2')) AS CUSTOMERFLAG, ")
            'sql.Append("       T1.MERCHANDISECD, ")
            'sql.Append("       T1.REZ_RECEPTION, ")
            'sql.Append("       T1.REZ_DELI_DATE, ")
            'sql.Append("       DECODE(T1.WALKIN,'0','1',DECODE(T1.WALKIN,'1','0')) AS WALKIN, ")
            'sql.Append("       NVL2(T2.MAINTECD,T2.MAINTENM,T3.MAINTENM) AS MERCHANDISENAME_VISIT, ")
            'sql.Append("       T1.VIN, ")
            'sql.Append("       T1.ORDERNO, ")
            'sql.Append("       T4.MERCHANDISENAME AS MERCHANDISENAME_RESERVE, ")
            'sql.Append("       T1.MILEAGE, ")
            'sql.Append("       NVL(TO_DATE(T1.REZ_PICK_DATE,'YYYYMMDDHH24MI'),T1.STARTTIME) AS REZ_PICK_DATE, ")
            'sql.Append("       T1.STRDATE ")
            'sql.Append("  FROM TBL_STALLREZINFO T1, ")
            'sql.Append("       TBLORG_MAINTEMASTER T2, ")
            'sql.Append("       TBLORG_MAINTEMASTER T3, ")
            'sql.Append("       TBL_MERCHANDISEMST T4 ")
            'sql.Append(" WHERE T1.DLRCD = T2.DLRCD(+) ")
            'sql.Append("   AND T1.MNTNCD = T2.MAINTECD(+) ")
            'sql.Append("   AND :BASETYPE = T2.BASETYPE(+) ")
            'sql.Append("   AND T1.DLRCD = T3.DLRCD(+) ")
            'sql.Append("   AND T1.MNTNCD = T3.MAINTECD(+) ")
            'sql.Append("   AND T1.MODELCODE = T3.BASETYPE(+) ")
            'sql.Append("   AND T1.DLRCD = T4.DLRCD(+) ")
            'sql.Append("   AND T1.MERCHANDISECD = T4.MERCHANDISECD(+) ")
            'sql.Append("   AND T1.DLRCD = :DLRCD ")
            'sql.Append("   AND T1.STRCD = :STRCD ")
            ''2012/08/22 TMEJ 日比野【SERVICE_2】チップ詳細の代表整備項目が表示されない START
            ''sql.Append("   AND (T1.REZID = :REZID OR T1.PREZID = :REZID) ")
            'sql.Append("   AND T1.REZID = :REZID ")
            ''2012/08/22 TMEJ 日比野【SERVICE_2】チップ詳細の代表整備項目が表示されない END
            'sql.Append("   AND DECODE(T1.STOPFLG,'0',DECODE(T1.CANCELFLG, '1', 1, 0), 0) + ")
            'sql.Append("       DECODE(T1.REZCHILDNO, 0, 1, 0) + ")
            'sql.Append("       DECODE(T1.REZCHILDNO, 999, 1, 0) = 0 ")
            ''2012/08/22 TMEJ 日比野【SERVICE_2】チップ詳細の代表整備項目が表示されない START
            ''sql.Append("   AND T1.STARTTIME = ( ")
            ''sql.Append("                       SELECT MIN(T4.STARTTIME) ")
            ''sql.Append("                         FROM TBL_STALLREZINFO T4 ")
            ''sql.Append("                        WHERE T4.DLRCD = :DLRCD ")
            ''sql.Append("                          AND T4.STRCD = :STRCD ")
            ''sql.Append("                          AND (T4.PREZID = :REZID OR T4.REZID = :REZID) ")
            ''sql.Append("                          AND DECODE(T4.STOPFLG,'0',DECODE(T4.CANCELFLG, '1', 1, 0), 0) + ")
            ''sql.Append("                              DECODE(T4.REZCHILDNO, 0, 1, 0) + ")
            ''sql.Append("                              DECODE(T4.REZCHILDNO, 999, 1, 0) = 0) ")
            ''2012/08/22 TMEJ 日比野【SERVICE_2】チップ詳細の代表整備項目が表示されない END

            'sql.AppendLine("SELECT /* SMBCommonClass_005 */ ")
            'sql.AppendLine("       TRIM(M1.DLR_CD) AS DLRCD ")
            'sql.AppendLine("      ,TRIM(M1.BRN_CD) AS STRCD ")
            'sql.AppendLine("      ,TRIM(M1.SVCIN_ID) AS REZID ")
            'sql.AppendLine("      ,TRIM(M1.MODEL_NAME) AS VEHICLENAME ")
            'sql.AppendLine("      ,TRIM(M1.REG_NUM) AS VCLREGNO ")
            'sql.AppendLine("      ,TRIM(M1.CST_NAME) AS CUSTOMERNAME ")
            'sql.AppendLine("      ,TRIM(M1.CST_PHONE) AS TELNO ")
            'sql.AppendLine("      ,TRIM(M1.CST_MOBILE) AS MOBILE ")
            'sql.AppendLine("      ,TRIM(M1.CUSTOMERFLAG) AS CUSTOMERFLAG ")
            'sql.AppendLine("      ,TRIM(M1.MERC_ID) AS MERCHANDISECD ")
            'sql.AppendLine("      ,TRIM(M1.PICK_DELI_TYPE) AS REZ_RECEPTION ")
            'sql.AppendLine("      ,DECODE(M1.SCHE_DELI_DATETIME, ")
            'sql.AppendLine("              :MINDATE, ")
            'sql.AppendLine("              NULL, ")
            'sql.AppendLine("              TO_CHAR(M1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_DELI_DATE ")
            'sql.AppendLine("      ,DECODE(TRIM(M1.ACCEPTANCE_TYPE), ")
            'sql.AppendLine("                   :WALKIN_0, ")
            'sql.AppendLine("                   :WALKIN_1, ")
            'sql.AppendLine("                   DECODE(TRIM(M1.ACCEPTANCE_TYPE), ")
            'sql.AppendLine("                               :WALKIN_1, ")
            'sql.AppendLine("                               :WALKIN_0)) AS WALKIN ")
            'sql.AppendLine("      ,CASE WHEN TRIM(M2.MAINTECD) IS NOT NULL THEN ")
            'sql.AppendLine("                 TRIM(M2.MAINTENM) ELSE ")
            'sql.AppendLine("                 TRIM(M3.MAINTENM) END AS MERCHANDISENAME_VISIT ")
            'sql.AppendLine("      ,TRIM(M1.VCL_VIN) AS VIN ")
            'sql.AppendLine("      ,TRIM(M1.RO_NUM) AS ORDERNO ")
            'sql.AppendLine("      ,TRIM(M4.MERC_NAME) AS MERCHANDISENAME_RESERVE ")
            'sql.AppendLine("      ,M1.SVCIN_MILE AS MILEAGE ")
            'sql.AppendLine("      ,DECODE(M1.SCHE_SVCIN_DATETIME, ")
            'sql.AppendLine("              :MINDATE, ")
            'sql.AppendLine("              TO_DATE(NULL), ")
            'sql.AppendLine("              M1.SCHE_SVCIN_DATETIME) AS REZ_PICK_DATE ")
            'sql.AppendLine("      ,DECODE(M1.RSLT_SVCIN_DATETIME, ")
            'sql.AppendLine("              :MINDATE, ")
            'sql.AppendLine("              TO_DATE(NULL), ")
            'sql.AppendLine("              M1.RSLT_SVCIN_DATETIME) AS STRDATE ")
            '2014/02/08 TMEJ 小澤 BTS対応 START
            'sql.AppendLine("      ,M1.SVC_STATUS ")
            '2014/02/08 TMEJ 小澤 BTS対応 END
            'sql.AppendLine("  FROM( ")
            'sql.AppendLine("      SELECT T1.DLR_CD ")
            'sql.AppendLine("            ,T1.BRN_CD ")
            'sql.AppendLine("            ,T1.SVCIN_ID ")
            'sql.AppendLine("            ,NVL(T7.MODEL_NAME, T5.NEWCST_MODEL_NAME) AS MODEL_NAME ")
            'sql.AppendLine("            ,T6.REG_NUM ")
            'sql.AppendLine("            ,T4.CST_NAME ")
            'sql.AppendLine("            ,T4.CST_PHONE ")
            'sql.AppendLine("            ,T4.CST_MOBILE ")
            'sql.AppendLine("            ,NVL2(T9.CST_ID, :MYCUSTOMER, T8.CST_TYPE) AS CUSTOMERFLAG ")
            'sql.AppendLine("            ,T2.MERC_ID ")
            'sql.AppendLine("            ,T2.MAINTE_CD ")
            'sql.AppendLine("            ,T1.PICK_DELI_TYPE ")
            'sql.AppendLine("            ,T1.SCHE_DELI_DATETIME ")
            'sql.AppendLine("            ,T1.ACCEPTANCE_TYPE ")
            'sql.AppendLine("            ,NVL2(T9.CST_ID, T9.VCL_VIN, T5.VCL_VIN) AS VCL_VIN ")
            'sql.AppendLine("            ,T1.RO_NUM ")
            'sql.AppendLine("            ,T1.SVCIN_MILE ")
            'sql.AppendLine("            ,DECODE(T1.SCHE_SVCIN_DATETIME, ")
            'sql.AppendLine("                    :MINDATE, ")
            'sql.AppendLine("                    T3.SCHE_START_DATETIME_MIN, ")
            'sql.AppendLine("                    T1.SCHE_SVCIN_DATETIME) AS SCHE_SVCIN_DATETIME ")
            'sql.AppendLine("            ,T1.RSLT_SVCIN_DATETIME ")
            'sql.AppendLine("            ,T5.VCL_KATASHIKI ")
            'sql.AppendLine("            ,T1.SVC_STATUS ")
            'sql.AppendLine("       FROM TB_T_SERVICEIN T1 ")
            'sql.AppendLine("           ,TB_T_JOB_DTL T2 ")
            'sql.AppendLine("           ,(SELECT U1.SVCIN_ID ")
            'sql.AppendLine("                   ,MIN(U2.JOB_DTL_ID) AS JOB_DTL_ID_MIN ")
            'sql.AppendLine("                   ,MIN(U3.SCHE_START_DATETIME) AS SCHE_START_DATETIME_MIN ")
            'sql.AppendLine("              FROM TB_T_SERVICEIN U1 ")
            'sql.AppendLine("                  ,TB_T_JOB_DTL U2 ")
            'sql.AppendLine("                  ,TB_T_STALL_USE U3 ")
            'sql.AppendLine("             WHERE U1.SVCIN_ID = U2.SVCIN_ID ")
            'sql.AppendLine("               AND U2.JOB_DTL_ID = U3.JOB_DTL_ID ")
            'sql.AppendLine("               AND U3.DLR_CD = :DLR_CD ")
            'sql.AppendLine("               AND U3.BRN_CD = :BRN_CD ")
            'sql.AppendLine("               AND U2.CANCEL_FLG = :CANCEL_FLG_0 ")
            'sql.AppendLine("             GROUP BY U1.SVCIN_ID) T3 ")
            'sql.AppendLine("           ,TB_M_CUSTOMER T4 ")
            'sql.AppendLine("           ,TB_M_VEHICLE T5 ")
            'sql.AppendLine("           ,TB_M_VEHICLE_DLR T6 ")
            'sql.AppendLine("           ,TB_M_MODEL T7 ")
            'sql.AppendLine("           ,TB_M_CUSTOMER_DLR T8 ")
            'sql.AppendLine("           ,TBL_SERVICEIN_APPEND T9 ")
            'sql.AppendLine("      WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
            'sql.AppendLine("        AND T2.SVCIN_ID = T3.SVCIN_ID ")
            'sql.AppendLine("        AND T2.JOB_DTL_ID = T3.JOB_DTL_ID_MIN ")
            'sql.AppendLine("        AND T1.CST_ID = T4.CST_ID ")
            'sql.AppendLine("        AND T1.VCL_ID = T5.VCL_ID ")
            'sql.AppendLine("        AND T1.DLR_CD = T6.DLR_CD ")
            'sql.AppendLine("        AND T1.VCL_ID = T6.VCL_ID ")
            'sql.AppendLine("        AND T5.MODEL_CD = T7.MODEL_CD(+) ")
            'sql.AppendLine("        AND T1.DLR_CD = T8.DLR_CD ")
            'sql.AppendLine("        AND T1.CST_ID = T8.CST_ID ")
            'sql.AppendLine("        AND T1.CST_ID = T9.CST_ID(+) ")
            'sql.AppendLine("        AND T1.VCL_ID = T9.VCL_ID(+) ")
            'sql.AppendLine("        AND T2.CANCEL_FLG = :CANCEL_FLG_0) M1 ")
            'sql.AppendLine("      ,TBLORG_MAINTEMASTER M2 ")
            'sql.AppendLine("      ,TBLORG_MAINTEMASTER M3 ")
            'sql.AppendLine("      ,TB_M_MERCHANDISE M4 ")
            'sql.AppendLine(" WHERE ")
            'sql.AppendLine("       M1.DLR_CD = M2.DLRCD(+) ")
            'sql.AppendLine("   AND M1.MAINTE_CD = M2.MAINTECD(+) ")
            'sql.AppendLine("   AND :BASETYPE = M2.BASETYPE(+) ")
            'sql.AppendLine("   AND M1.DLR_CD = M3.DLRCD(+) ")
            'sql.AppendLine("   AND M1.MAINTE_CD = M3.MAINTECD(+) ")
            'sql.AppendLine("   AND SUBSTR(M1.VCL_KATASHIKI, 0, INSTR(M1.VCL_KATASHIKI, '-') - 1) = M3.BASETYPE(+) ")
            'sql.AppendLine("   AND M1.MERC_ID = M4.MERC_ID(+) ")
            'sql.AppendLine("   AND M1.SVCIN_ID = :SVCIN_ID ")
            'sql.AppendLine("   AND M1.DLR_CD = :DLR_CD ")
            'sql.AppendLine("   AND M1.BRN_CD = :BRN_CD ")
            'sql.AppendLine("   AND M1.SVC_STATUS <> :SVC_STATUS_02 ")

            sql.AppendLine("SELECT /* SMBCommonClass_005 */  ")
            sql.AppendLine("       TRIM(M1.DLR_CD) AS DLRCD  ")
            sql.AppendLine("      ,TRIM(M1.BRN_CD) AS STRCD  ")
            sql.AppendLine("      ,TRIM(M1.SVCIN_ID) AS REZID  ")
            sql.AppendLine("      ,TRIM(M1.MODEL_NAME) AS VEHICLENAME  ")
            sql.AppendLine("      ,TRIM(M1.REG_NUM) AS VCLREGNO  ")
            sql.AppendLine("      ,TRIM(M1.CST_NAME) AS CUSTOMERNAME  ")
            sql.AppendLine("      ,TRIM(M1.CST_PHONE) AS TELNO  ")
            sql.AppendLine("      ,TRIM(M1.CST_MOBILE) AS MOBILE  ")
            sql.AppendLine("      ,TRIM(M1.CUSTOMERFLAG) AS CUSTOMERFLAG  ")
            sql.AppendLine("      ,TRIM(M1.MERC_ID) AS MERCHANDISECD  ")
            sql.AppendLine("      ,TRIM(M1.PICK_DELI_TYPE) AS REZ_RECEPTION  ")
            sql.AppendLine("      ,DECODE(M1.SCHE_DELI_DATETIME,  ")
            sql.AppendLine("              :MINDATE,  ")
            sql.AppendLine("              NULL,  ")
            sql.AppendLine("              TO_CHAR(M1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_DELI_DATE  ")
            sql.AppendLine("      ,DECODE(TRIM(M1.ACCEPTANCE_TYPE),  ")
            sql.AppendLine("              :WALKIN_0,  ")
            sql.AppendLine("              :WALKIN_1,  ")
            sql.AppendLine("              DECODE(TRIM(M1.ACCEPTANCE_TYPE),  ")
            sql.AppendLine("                     :WALKIN_1,  ")
            sql.AppendLine("                     :WALKIN_0)) AS WALKIN  ")
            sql.AppendLine("      ,TRIM(M1.VCL_VIN) AS VIN  ")
            sql.AppendLine("      ,TRIM(M1.RO_NUM) AS ORDERNO  ")
            sql.AppendLine("      ,M1.SVCIN_MILE AS MILEAGE  ")
            sql.AppendLine("      ,DECODE(M1.SCHE_SVCIN_DATETIME,  ")
            sql.AppendLine("              :MINDATE,  ")
            sql.AppendLine("              TO_DATE(NULL),  ")
            sql.AppendLine("              M1.SCHE_SVCIN_DATETIME) AS REZ_PICK_DATE  ")
            sql.AppendLine("      ,DECODE(M1.RSLT_SVCIN_DATETIME,  ")
            sql.AppendLine("              :MINDATE,  ")
            sql.AppendLine("              TO_DATE(NULL),  ")
            sql.AppendLine("              M1.RSLT_SVCIN_DATETIME) AS STRDATE  ")
            sql.AppendLine("      ,M1.ROW_LOCK_VERSION  ")
            sql.AppendLine("      ,NVL(TRIM(CONCAT(M2.UPPER_DISP, M2.LOWER_DISP)), NVL(TRIM(M3.SVC_CLASS_NAME), M3.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
            sql.AppendLine("      ,M1.SVC_STATUS ")
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            sql.AppendLine("      ,TRIM(M1.CST_VCL_TYPE) AS CST_VCL_TYPE ")
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END
            sql.AppendLine("  FROM(  ")
            sql.AppendLine("      SELECT T1.DLR_CD  ")
            sql.AppendLine("            ,T1.BRN_CD  ")
            sql.AppendLine("            ,T1.SVCIN_ID  ")
            sql.AppendLine("            ,NVL(T7.MODEL_NAME, T5.NEWCST_MODEL_NAME) AS MODEL_NAME  ")
            sql.AppendLine("            ,T6.REG_NUM  ")
            sql.AppendLine("            ,T4.CST_NAME  ")
            sql.AppendLine("            ,T4.CST_PHONE  ")
            sql.AppendLine("            ,T4.CST_MOBILE  ")
            sql.AppendLine("            ,NVL2(T9.CST_ID, :MYCUSTOMER, T8.CST_TYPE) AS CUSTOMERFLAG  ")
            sql.AppendLine("            ,T2.MERC_ID  ")
            sql.AppendLine("            ,T2.MAINTE_CD  ")
            sql.AppendLine("            ,T1.PICK_DELI_TYPE  ")
            sql.AppendLine("            ,T1.SCHE_DELI_DATETIME  ")
            sql.AppendLine("            ,T1.ACCEPTANCE_TYPE  ")
            sql.AppendLine("            ,NVL2(T9.CST_ID, T9.VCL_VIN, T5.VCL_VIN) AS VCL_VIN  ")
            sql.AppendLine("            ,T1.RO_NUM  ")
            sql.AppendLine("            ,T1.SVCIN_MILE  ")
            sql.AppendLine("            ,DECODE(T1.SCHE_SVCIN_DATETIME,  ")
            sql.AppendLine("                    :MINDATE,  ")
            sql.AppendLine("                    T3.SCHE_START_DATETIME_MIN,  ")
            sql.AppendLine("                    T1.SCHE_SVCIN_DATETIME) AS SCHE_SVCIN_DATETIME  ")
            sql.AppendLine("            ,T1.RSLT_SVCIN_DATETIME  ")
            sql.AppendLine("            ,T5.VCL_KATASHIKI  ")
            sql.AppendLine("            ,T1.SVC_STATUS  ")
            sql.AppendLine("            ,T1.ROW_LOCK_VERSION  ")
            sql.AppendLine("            ,T2.SVC_CLASS_ID  ")
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            sql.AppendLine("            ,T1.CST_VCL_TYPE ")
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END
            sql.AppendLine("       FROM TB_T_SERVICEIN T1  ")
            sql.AppendLine("           ,TB_T_JOB_DTL T2  ")
            sql.AppendLine("           ,(SELECT U1.SVCIN_ID  ")
            sql.AppendLine("                   ,MIN(U2.JOB_DTL_ID) AS JOB_DTL_ID_MIN  ")
            sql.AppendLine("                   ,MIN(U3.SCHE_START_DATETIME) AS SCHE_START_DATETIME_MIN  ")
            sql.AppendLine("              FROM TB_T_SERVICEIN U1  ")
            sql.AppendLine("                  ,TB_T_JOB_DTL U2  ")
            sql.AppendLine("                  ,TB_T_STALL_USE U3  ")
            sql.AppendLine("             WHERE U1.SVCIN_ID = U2.SVCIN_ID  ")
            sql.AppendLine("               AND U2.JOB_DTL_ID = U3.JOB_DTL_ID  ")
            sql.AppendLine("               AND U3.DLR_CD = :DLR_CD  ")
            sql.AppendLine("               AND U3.BRN_CD = :BRN_CD  ")
            sql.AppendLine("               AND U2.CANCEL_FLG = :CANCEL_FLG_0  ")
            sql.AppendLine("             GROUP BY U1.SVCIN_ID) T3  ")
            sql.AppendLine("           ,TB_M_CUSTOMER T4  ")
            sql.AppendLine("           ,TB_M_VEHICLE T5  ")
            sql.AppendLine("           ,TB_M_VEHICLE_DLR T6  ")
            sql.AppendLine("           ,TB_M_MODEL T7  ")
            sql.AppendLine("           ,TB_M_CUSTOMER_DLR T8  ")
            sql.AppendLine("           ,TBL_SERVICEIN_APPEND T9  ")
            sql.AppendLine("      WHERE T1.SVCIN_ID = T2.SVCIN_ID  ")
            sql.AppendLine("        AND T2.SVCIN_ID = T3.SVCIN_ID  ")
            sql.AppendLine("        AND T2.JOB_DTL_ID = T3.JOB_DTL_ID_MIN  ")
            sql.AppendLine("        AND T1.CST_ID = T4.CST_ID  ")
            sql.AppendLine("        AND T1.VCL_ID = T5.VCL_ID  ")
            sql.AppendLine("        AND T1.DLR_CD = T6.DLR_CD  ")
            sql.AppendLine("        AND T1.VCL_ID = T6.VCL_ID  ")
            sql.AppendLine("        AND T5.MODEL_CD = T7.MODEL_CD(+)  ")
            sql.AppendLine("        AND T1.DLR_CD = T8.DLR_CD  ")
            sql.AppendLine("        AND T1.CST_ID = T8.CST_ID  ")
            sql.AppendLine("        AND T1.CST_ID = T9.CST_ID(+)  ")
            sql.AppendLine("        AND T1.VCL_ID = T9.VCL_ID(+)  ")
            sql.AppendLine("        AND T2.CANCEL_FLG = :CANCEL_FLG_0) M1  ")
            sql.AppendLine("      ,TB_M_MERCHANDISE M2  ")
            sql.AppendLine("      ,TB_M_SERVICE_CLASS M3 ")
            sql.AppendLine(" WHERE  ")
            sql.AppendLine("       M1.MERC_ID = M2.MERC_ID(+)  ")
            sql.AppendLine("   AND M1.SVC_CLASS_ID = M3.SVC_CLASS_ID(+)  ")
            sql.AppendLine("   AND M1.SVCIN_ID = :SVCIN_ID  ")
            sql.AppendLine("   AND M1.DLR_CD = :DLR_CD  ")
            sql.AppendLine("   AND M1.BRN_CD = :BRN_CD  ")
            sql.AppendLine("   AND M1.SVC_STATUS <> :SVC_STATUS_02  ")

            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.ChipDetailReserveDataTable)("SMBCommonClass_005")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("BASETYPE", OracleDbType.Char, Me.BaseTypeAll(inDealerCode))
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inReserveId)
                query.AddParameterWithTypeValue("MYCUSTOMER", OracleDbType.NVarchar2, CusutomerTypeVisitor)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("BASETYPE", OracleDbType.NVarchar2, CommonBaseType)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inStoreCode)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("WALKIN_0", OracleDbType.NVarchar2, AcceptanceTypeReserve)
                query.AddParameterWithTypeValue("WALKIN_1", OracleDbType.NVarchar2, AcceptanceTypeWalkIn)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''SQLの実行
                Using dt As SMBCommonClassDataSet.ChipDetailReserveDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_019:中断情報取得(SMB)
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inStallUseId">ストール利用ID</param>
        ''' <returns>中断情報(SMB)</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function GetSmbStopData(ByVal inDealerCode As String, _
                                       ByVal inStoreCode As String, _
                                       ByVal inStallUseId As Decimal) As SMBCommonClassDataSet.StopDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inStallUseId.ToString(CultureInfo.InvariantCulture)))
            ''SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine("SELECT /* SMBCommonClass_019 */ ")
            sql.AppendLine("       DECODE(T3.RSLT_END_DATETIME, ")
            sql.AppendLine("              :MINDATE, ")
            sql.AppendLine("              NULL, ")
            sql.AppendLine("              TO_CHAR(T3.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_END_TIME ")
            sql.AppendLine("      ,T3.STOP_REASON_TYPE AS RESULT_STATUS ")
            sql.AppendLine("      ,T3.STOP_MEMO AS STOPMEMO ")
            sql.AppendLine("  FROM ")
            sql.AppendLine("       TB_T_SERVICEIN T1 ")
            sql.AppendLine("      ,TB_T_JOB_DTL T2 ")
            sql.AppendLine("      ,TB_T_STALL_USE T3 ")
            sql.AppendLine(" WHERE ")
            sql.AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
            sql.AppendLine("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
            sql.AppendLine("   AND T3.STALL_USE_ID = :STALL_USE_ID ")
            sql.AppendLine("   AND T1.SVC_STATUS <> :SVC_STATUS_02 ")
            sql.AppendLine("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
            sql.AppendLine("   AND T3.STALL_USE_STATUS = :STALL_USE_STATUS_05 ")

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.StopDataTable)("SMBCommonClass_019")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStallUseId)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("STALL_USE_STATUS_05", OracleDbType.NVarchar2, StallUseStatusStop)
                ''SQLの実行
                Using dt As SMBCommonClassDataSet.StopDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' SMBCommonClass_025:顧客情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inCustomerId">顧客ID</param>
        ''' <param name="inVehicleId">車両ID</param>
        ''' <returns>顧客情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' 2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
        ''' 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </history>
        Public Function GetCustomerInfo(ByVal inDealerCode As String, _
                                        ByVal inCustomerId As Decimal, _
                                        ByVal inVehicleId As Decimal) As SMBCommonClassDataSet.ChipDetailCustomerInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} inDealerCode:{2} inCustomerId:{3} inVehicleId:{4} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inCustomerId.ToString(CultureInfo.InvariantCulture) _
                        , inVehicleId.ToString(CultureInfo.InvariantCulture)))
            ''SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine("SELECT /* SMBCommonClass_025 */ ")
            sql.AppendLine("       TRIM(T3.REG_NUM) AS REG_NUM ")
            sql.AppendLine("      ,TRIM(T5.CST_PHONE) AS CST_PHONE ")
            sql.AppendLine("      ,TRIM(T5.CST_MOBILE) AS CST_MOBILE ")
            sql.AppendLine("      ,TRIM(T8.MODEL_NAME) AS MODEL_NAME ")
            sql.AppendLine("      ,TRIM(T2.MODEL_CD) AS MODEL_CD ")
            sql.AppendLine("      ,TRIM(T3.REG_AREA_CD) AS REG_AREA_CD ")

            '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

            'sql.AppendLine("      ,TRIM(T3.VIP_FLG) AS VIP_FLG ")

            sql.AppendLine("      ,TRIM(T3.IMP_VCL_FLG) AS IMP_VCL_FLG ")

            '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

            sql.AppendLine("      ,TRIM(T2.GRADE_NAME) AS GRADE_NAME ")

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

            sql.AppendLine("      ,TRIM(T4.CST_TYPE) AS CST_TYPE ")
            sql.AppendLine("      ,TRIM(T1.CST_VCL_TYPE) AS CST_VCL_TYPE ")

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

            sql.AppendLine("      ,CASE ")
            sql.AppendLine("            WHEN T6.POSITION_TYPE = :POSITION_TYPE_1 THEN TRIM(T5.CST_NAME) || TRIM(T6.NAMETITLE_NAME) ")
            sql.AppendLine("            WHEN T6.POSITION_TYPE = :POSITION_TYPE_2 THEN TRIM(T6.NAMETITLE_NAME) || TRIM(T5.CST_NAME) ")
            sql.AppendLine("            ELSE TRIM(T5.CST_NAME) ")
            sql.AppendLine("       END AS CST_NAME ")
            sql.AppendLine("      ,TRIM(T7.REG_AREA_NAME) AS REG_AREA_NAME ")
            sql.AppendLine("      ,TRIM(T2.VCL_VIN) AS VCL_VIN ")
            '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            sql.AppendLine("      ,T2.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")
            '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
            sql.AppendLine("  FROM ")
            sql.AppendLine("       TB_M_CUSTOMER_VCL T1 ")
            sql.AppendLine("      ,TB_M_VEHICLE T2 ")
            sql.AppendLine("      ,TB_M_VEHICLE_DLR T3 ")
            sql.AppendLine("      ,TB_M_CUSTOMER_DLR T4 ")
            sql.AppendLine("      ,TB_M_CUSTOMER T5 ")
            sql.AppendLine("      ,TB_M_NAMETITLE T6 ")
            sql.AppendLine("      ,TB_M_REG_AREA T7 ")
            sql.AppendLine("      ,TB_M_MODEL T8 ")
            sql.AppendLine(" WHERE ")
            sql.AppendLine("       T1.VCL_ID = T2.VCL_ID ")
            sql.AppendLine("   AND T1.DLR_CD = T3.DLR_CD ")
            sql.AppendLine("   AND T2.VCL_ID = T3.VCL_ID ")
            sql.AppendLine("   AND T1.DLR_CD = T4.DLR_CD ")
            sql.AppendLine("   AND T1.CST_ID = T4.CST_ID ")
            sql.AppendLine("   AND T1.CST_ID = T5.CST_ID ")
            sql.AppendLine("   AND T5.NAMETITLE_CD = T6.NAMETITLE_CD(+) ")
            sql.AppendLine("   AND T3.REG_AREA_CD = T7.REG_AREA_CD(+) ")
            sql.AppendLine("   AND T2.MODEL_CD = T8.MODEL_CD(+) ")
            sql.AppendLine("   AND T1.DLR_CD = :DLR_CD ")
            sql.AppendLine("   AND T3.DLR_CD = :DLR_CD ")
            sql.AppendLine("   AND T4.DLR_CD = :DLR_CD ")
            sql.AppendLine("   AND T6.INUSE_FLG(+) = :INUSE_FLG_1 ")
            sql.AppendLine("   AND T1.CST_ID = :CST_ID ")
            sql.AppendLine("   AND T1.VCL_ID = :VCL_ID ")


            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.ChipDetailCustomerInfoDataTable)("SMBCommonClass_025")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                query.AddParameterWithTypeValue("POSITION_TYPE_1", OracleDbType.NVarchar2, PositionTypeAfter)
                query.AddParameterWithTypeValue("POSITION_TYPE_2", OracleDbType.NVarchar2, PositionTypeBefore)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, InuseTypeUse)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerId)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVehicleId)

                ''SQLの実行
                Using dt As SMBCommonClassDataSet.ChipDetailCustomerInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_026:RO情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <returns>RO情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' 2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応
        ''' 2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加
        ''' </history>
        Public Function GetRepariOrderInfo(ByVal inDealerCode As String, _
                                           ByVal inBranchCode As String, _
                                           ByVal inVisitSequence As Long) As SMBCommonClassDataSet.ChipDetailRepairOrderInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} inDealerCode:{2} inBranchCode:{3} inVisitSequence:{4} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inBranchCode _
                        , inVisitSequence.ToString(CultureInfo.InvariantCulture)))

            ''SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine("SELECT /* SMBCommonClass_026 */ ")
            sql.AppendLine("       CASE ")
            sql.AppendLine("            WHEN T1.SCHE_DELI_DATETIME = :MINDATE THEN NULL ")
            sql.AppendLine("            ELSE TO_CHAR(T1.SCHE_DELI_DATETIME, 'YYYY/MM/DD HH24:MI:SS') ")
            sql.AppendLine("       END AS SCHE_DELI_DATETIME ")
            sql.AppendLine("      ,T3.RO_STATUS_MAX AS RO_STATUS ")
            sql.AppendLine("      ,T1.INSPECTION_STATUS ")
            sql.AppendLine("      ,T2.RO_STATUS AS ADD_RO_STATUS ")
            sql.AppendLine("      ,T2.DRAWER ")
            sql.AppendLine("      ,T2.RO_CREATE_STF_CD ")
            sql.AppendLine("      ,T2.WORK_END_TYPE ")
            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            sql.AppendLine("      ,T2.RO_STATUS_MIN AS RO_STATUS_MIN ")
            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            sql.AppendLine("      ,CASE ")
            sql.AppendLine("            WHEN T1.INSPECTION_APPROVAL_DATETIME = :MINDATE THEN NULL ")
            sql.AppendLine("            ELSE T1.INSPECTION_APPROVAL_DATETIME ")
            sql.AppendLine("       END AS INSPECTION_APPROVAL_DATETIME ")
            sql.AppendLine("      ,CASE ")
            sql.AppendLine("            WHEN T1.INVOICE_PREP_COMPL_DATETIME = :MINDATE THEN NULL ")
            sql.AppendLine("            ELSE T1.INVOICE_PREP_COMPL_DATETIME ")
            sql.AppendLine("       END AS INVOICE_PRINT_DATETIME ")
            sql.AppendLine("  FROM (SELECT /* 納車予定時刻・完成検査フラグ・完成検査完了時刻・精算書印刷時刻 */ ")
            sql.AppendLine("               A1.VISITSEQ ")
            sql.AppendLine("              ,MAX(A2.SCHE_DELI_DATETIME) AS SCHE_DELI_DATETIME ")
            sql.AppendLine("              ,MIN(A3.INSPECTION_STATUS) AS INSPECTION_STATUS ")
            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            'sql.AppendLine("              ,MIN(A3.INSPECTION_APPROVAL_DATETIME) AS INSPECTION_APPROVAL_DATETIME ")
            sql.AppendLine("              ,MAX(A3.INSPECTION_APPROVAL_DATETIME) AS INSPECTION_APPROVAL_DATETIME ")
            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            sql.AppendLine("              ,MAX(A2.INVOICE_PREP_COMPL_DATETIME) AS INVOICE_PREP_COMPL_DATETIME ")
            sql.AppendLine("          FROM TBL_SERVICE_VISIT_MANAGEMENT A1 ")
            sql.AppendLine("              ,TB_T_SERVICEIN A2 ")
            sql.AppendLine("              ,TB_T_JOB_DTL A3 ")
            sql.AppendLine("         WHERE A1.REZID = A2.SVCIN_ID ")
            sql.AppendLine("           AND A2.SVCIN_ID = A3.SVCIN_ID ")
            sql.AppendLine("           AND A1.VISITSEQ = :VISIT_SEQ ")
            sql.AppendLine("           AND A2.DLR_CD = :DLR_CD ")
            sql.AppendLine("           AND A2.BRN_CD = :BRN_CD ")
            sql.AppendLine("           AND A2.SVC_STATUS <> :SVC_STATUS_02 ")
            sql.AppendLine("           AND A3.CANCEL_FLG = :CANCEL_FLG_0 ")
            sql.AppendLine("         GROUP BY A1.VISITSEQ ")
            sql.AppendLine("       ) T1 ")
            sql.AppendLine("      ,(SELECT /* 追加作業ステータス・起票者・起票者アカウント・全ての作業終了有無 */ ")
            sql.AppendLine("               D1.VISIT_ID ")
            sql.AppendLine("              ,D1.RO_STATUS ")
            sql.AppendLine("              ,CASE ")
            sql.AppendLine("                    WHEN D3.OPERATIONCODE = :OPERATIONCODE_14 THEN :DRAWER_1 ")

            '2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応 START

            sql.AppendLine("                    WHEN D3.OPERATIONCODE = :OPERATIONCODE_62 THEN :DRAWER_1 ")

            '2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応 END

            sql.AppendLine("                    WHEN D3.OPERATIONCODE = :OPERATIONCODE_9 THEN :DRAWER_2 ")
            sql.AppendLine("                    ELSE NULL ")
            sql.AppendLine("               END AS DRAWER ")
            sql.AppendLine("              ,TRIM(D1.RO_CREATE_STF_CD) AS RO_CREATE_STF_CD ")
            sql.AppendLine("              ,CASE ")
            sql.AppendLine("                    WHEN D2.RO_STATUS_MIN < :RO_STATUS_80 THEN :WORK_END_TYPE_0 ")

            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 START

            sql.AppendLine("                    WHEN 0 < NVL(D4.NOT_JOB_COUNT, 0) THEN :WORK_END_TYPE_0 ")

            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 END

            sql.AppendLine("                    ELSE :WORK_END_TYPE_1 ")
            sql.AppendLine("               END AS WORK_END_TYPE ")
            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            sql.AppendLine("              ,D2.RO_STATUS_MIN AS RO_STATUS_MIN ")
            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            sql.AppendLine("          FROM TB_T_RO_INFO D1 ")
            sql.AppendLine("              ,(SELECT B1.VISITSEQ ")
            sql.AppendLine("                      ,MAX(B2.RO_SEQ) AS RO_SEQ_MAX ")
            sql.AppendLine("                      ,MIN(B2.RO_STATUS) AS RO_STATUS_MIN ")
            sql.AppendLine("                  FROM TBL_SERVICE_VISIT_MANAGEMENT B1 ")
            sql.AppendLine("                      ,TB_T_RO_INFO B2 ")
            sql.AppendLine("                 WHERE B1.VISITSEQ = B2.VISIT_ID ")
            sql.AppendLine("                   AND B1.VISITSEQ = :VISIT_SEQ ")
            sql.AppendLine("                   AND B2.RO_STATUS <> :RO_STATUS_99 ")
            sql.AppendLine("                 GROUP BY B1.VISITSEQ ")
            sql.AppendLine("               ) D2 ")
            sql.AppendLine("               ,TBL_USERS D3 ")

            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 START

            sql.AppendLine("               ,(SELECT E1.VISITSEQ ")
            sql.AppendLine("                       ,COUNT(1) AS NOT_JOB_COUNT ")
            sql.AppendLine("                   FROM TBL_SERVICE_VISIT_MANAGEMENT E1 ")
            sql.AppendLine("                       ,TB_T_SERVICEIN E2 ")
            sql.AppendLine("                       ,TB_T_JOB_DTL E3 ")
            sql.AppendLine("                       ,TB_T_JOB_INSTRUCT E4 ")
            sql.AppendLine("                  WHERE E1.REZID = E2.SVCIN_ID ")
            sql.AppendLine("                    AND E2.SVCIN_ID = E3.SVCIN_ID ")
            sql.AppendLine("                    AND E3.JOB_DTL_ID = E4.JOB_DTL_ID ")
            sql.AppendLine("                    AND E1.VISITSEQ = :VISIT_SEQ ")
            sql.AppendLine("                    AND E4.STARTWORK_INSTRUCT_FLG = N'0' ")
            sql.AppendLine("                    AND E2.SVCIN_ID = E3.SVCIN_ID ")
            sql.AppendLine("                  GROUP BY E1.VISITSEQ ")
            sql.AppendLine("               ) D4 ")

            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 END

            sql.AppendLine("         WHERE D1.VISIT_ID = D2.VISITSEQ ")
            sql.AppendLine("           AND D1.RO_SEQ = D2.RO_SEQ_MAX ")
            sql.AppendLine("           AND D1.RO_CREATE_STF_CD = D3.ACCOUNT(+) ")

            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 START

            sql.AppendLine("           AND D1.VISIT_ID = D4.VISITSEQ(+) ")

            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 END

            sql.AppendLine("           AND D3.DLRCD(+) = :DLRCD ")
            sql.AppendLine("           AND D3.STRCD(+) = :BRNCD ")
            sql.AppendLine("           AND D1.RO_STATUS <> :RO_STATUS_99 ")
            sql.AppendLine("       ) T2 ")
            sql.AppendLine("      ,(SELECT /* 親のROステータス */ ")
            sql.AppendLine("              C1.VISIT_ID ")
            sql.AppendLine("             ,MAX(C1.RO_STATUS) AS RO_STATUS_MAX ")
            sql.AppendLine("          FROM TB_T_RO_INFO C1 ")
            sql.AppendLine("         WHERE C1.VISIT_ID = :VISIT_SEQ ")
            sql.AppendLine("           AND C1.RO_STATUS <> :RO_STATUS_99 ")
            sql.AppendLine("         GROUP BY C1.VISIT_ID ")
            sql.AppendLine("       ) T3 ")
            sql.AppendLine(" WHERE T3.VISIT_ID = T2.VISIT_ID(+) ")
            sql.AppendLine("   AND T3.VISIT_ID = T1.VISITSEQ(+) ")

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.ChipDetailRepairOrderInfoDataTable)("SMBCommonClass_026")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("VISIT_SEQ", OracleDbType.Decimal, inVisitSequence)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                query.AddParameterWithTypeValue("BRNCD", OracleDbType.Char, inBranchCode)

                '2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応 START

                query.AddParameterWithTypeValue("OPERATIONCODE_62", OracleDbType.Long, OperationCodeChT)

                '2014/08/22 TMEJ 小澤 ダミーディーラー不具合対応 END

                query.AddParameterWithTypeValue("OPERATIONCODE_14", OracleDbType.Long, OperationCodeTC)
                query.AddParameterWithTypeValue("OPERATIONCODE_9", OracleDbType.Long, OperationCodeSA)
                query.AddParameterWithTypeValue("DRAWER_1", OracleDbType.NVarchar2, ReissueVouchersTC)
                query.AddParameterWithTypeValue("DRAWER_2", OracleDbType.NVarchar2, ReissueVouchersSA)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("RO_STATUS_80", OracleDbType.NVarchar2, RepairOrderStatusWaitDelivery)
                query.AddParameterWithTypeValue("RO_STATUS_99", OracleDbType.NVarchar2, RepairOrderStatusCancel)
                query.AddParameterWithTypeValue("WORK_END_TYPE_0", OracleDbType.NVarchar2, WorkEndTypeWorking)
                query.AddParameterWithTypeValue("WORK_END_TYPE_1", OracleDbType.NVarchar2, WorkEndTypeWorkEnd)

                ''SQLの実行
                Using dt As SMBCommonClassDataSet.ChipDetailRepairOrderInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' SMBCommonClass_027:RO作業連番取得
        ''' </summary>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <param name="inJobDetailSequenceId">作業内容ID</param>
        ''' <param name="inOrderNo">RO番号</param>
        ''' <returns>RO情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' </history>
        Public Function GetJobDetailSequenceInfo(ByVal inVisitSequence As Long, _
                                                 ByVal inServiceInId As Decimal, _
                                                 ByVal inJobDetailSequenceId As Decimal, _
                                                 ByVal inOrderNo As String) As SMBCommonClassDataSet.JobDetailSequenceInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0} P1:{1} P2:{2} P3:{3} P4:{4}" _
                           , String.Concat(Me.GetType.ToString, ".", System.Reflection.MethodBase.GetCurrentMethod.Name) _
                           , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
                           , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                           , inJobDetailSequenceId.ToString(CultureInfo.CurrentCulture) _
                           , inOrderNo))

            ''SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine("SELECT /* SMBCommonClass_027 */ ")
            sql.AppendLine("       T4.RO_SEQ ")
            sql.AppendLine("  FROM ")
            sql.AppendLine("       TB_T_RO_INFO T4 ")
            sql.AppendLine(" WHERE ")
            sql.AppendLine("       T4.VISIT_ID = :VISIT_ID ")
            sql.AppendLine("   AND T4.RO_NUM = :RO_NUM ")
            sql.AppendLine("   AND T4.RO_STATUS <> :RO_STATUS_99 ")
            sql.AppendLine("   AND EXISTS (SELECT 1 ")
            sql.AppendLine("                 FROM TB_T_SERVICEIN T1 ")
            sql.AppendLine("                     ,TB_T_JOB_DTL T2 ")
            sql.AppendLine("                     ,TB_T_JOB_INSTRUCT T3 ")
            sql.AppendLine("                WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
            sql.AppendLine("                  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
            sql.AppendLine("                  AND T1.SVCIN_ID = :SVCIN_ID ")
            sql.AppendLine("                  AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
            sql.AppendLine("                  AND T1.SVC_STATUS <> :SVC_STATUS_02 ")
            sql.AppendLine("                  AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
            sql.AppendLine("                  AND T3.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_1) ")

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.JobDetailSequenceInfoDataTable)("SMBCommonClass_027")
                query.CommandText = sql.ToString()
                ''パラメータの設定
                query.AddParameterWithTypeValue("VISIT_ID", OracleDbType.Long, inVisitSequence)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inOrderNo)
                query.AddParameterWithTypeValue("RO_STATUS_99", OracleDbType.NVarchar2, RepairOrderStatusCancel)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDetailSequenceId)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_1", OracleDbType.NVarchar2, SstartWorkInstructTypeOn)

                ''SQLの実行
                Using dt As SMBCommonClassDataSet.JobDetailSequenceInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))

                    Return dt

                End Using

            End Using

        End Function

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        ' 2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる START
        ''' <summary>
        ''' SMBCommonClass_028:追加作業承認の取得
        ''' </summary>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <returns>追加作業のROステータスと起票者アカウント</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2019/06/14 TKM 追加作業承認ボタンの表示対応
        ''' 2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある
        ''' </history>
        Public Function GetAddRepairOrderInfo(ByVal inVisitSequence As Long) As SMBCommonClassDataSet.ChipDetailRepairOrderInfoDataTable

            ' 開始ログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0} P1:{1}" _
                           , String.Concat(Me.GetType.ToString, ".", System.Reflection.MethodBase.GetCurrentMethod.Name) _
                           , inVisitSequence.ToString(CultureInfo.CurrentCulture)))

            ' SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine("SELECT /* SMBCommonClass_028 */ ")
            sql.AppendLine("       RO_INFO.RO_STATUS AS ADD_RO_STATUS ")
            ' 2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある START
            sql.AppendLine("      ,RO_INFO.RO_CREATE_STF_CD AS RO_CREATE_STF_CD ")
            ' 2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある END
            sql.AppendLine("      ,CASE ")
            sql.AppendLine("           WHEN USERS.OPERATIONCODE = :OPERATIONCODE_14 THEN :DRAWER_1 ")
            sql.AppendLine("           WHEN USERS.OPERATIONCODE = :OPERATIONCODE_62 THEN :DRAWER_1 ")
            sql.AppendLine("           WHEN USERS.OPERATIONCODE = :OPERATIONCODE_9  THEN :DRAWER_2 ")
            sql.AppendLine("           ELSE NULL ")
            sql.AppendLine("       END AS DRAWER ")
            sql.AppendLine("  FROM ")
            sql.AppendLine("       TB_T_RO_INFO RO_INFO ")
            sql.AppendLine("      ,TBL_USERS USERS ")
            sql.AppendLine(" WHERE ")
            sql.AppendLine("       RO_INFO.VISIT_ID = :VISIT_ID ")
            sql.AppendLine("   AND RO_INFO.RO_STATUS = :RO_STATUS_35 ")
            sql.AppendLine("   AND RO_INFO.RO_CREATE_STF_CD = USERS.ACCOUNT(+) ")
            sql.AppendLine(" ORDER BY ")
            sql.AppendLine("       RO_INFO.RO_SEQ ASC ")

            Using query As New DBSelectQuery(Of SMBCommonClassDataSet.ChipDetailRepairOrderInfoDataTable)("SMBCommonClass_028")
                query.CommandText = sql.ToString()
                ' パラメータの設定
                query.AddParameterWithTypeValue("DRAWER_1", OracleDbType.NVarchar2, ReissueVouchersTC)
                query.AddParameterWithTypeValue("DRAWER_2", OracleDbType.NVarchar2, ReissueVouchersSA)
                query.AddParameterWithTypeValue("OPERATIONCODE_9", OracleDbType.Decimal, OperationCodeSA)
                query.AddParameterWithTypeValue("OPERATIONCODE_14", OracleDbType.Decimal, OperationCodeTC)
                query.AddParameterWithTypeValue("OPERATIONCODE_62", OracleDbType.Decimal, OperationCodeChT)
                query.AddParameterWithTypeValue("RO_STATUS_35", OracleDbType.NVarchar2, RepairOrderStatusConfirmationWait)
                query.AddParameterWithTypeValue("VISIT_ID", OracleDbType.Decimal, inVisitSequence)

                ' SQLの実行
                Using dt As SMBCommonClassDataSet.ChipDetailRepairOrderInfoDataTable = query.GetData()
                    '' 終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))

                    Return dt

                End Using

            End Using

        End Function
        ' 2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる END

#End Region

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        ' ''' <summary>
        ' ''' TBL_DLRENVSETTINGから「*」を取得
        ' ''' </summary>
        ' ''' <param name="dealerCD">販売店コード</param>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' </history>
        'Private ReadOnly Property BaseTypeAll(ByVal dealerCD As String) As String
        '    Get
        '        Const BASETYPE_ALL As String = "BASETYPE_ALL"
        '        Static value As String
        '        If String.IsNullOrEmpty(value) = True Then
        '            Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow = (New DealerEnvSetting).GetEnvSetting(dealerCD, BASETYPE_ALL)
        '            value = If(row IsNot Nothing, row.PARAMVALUE, "*")
        '        End If
        '        Return value
        '    End Get
        'End Property
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

#End Region

    End Class
End Namespace
Partial Class SMBCommonClassDataSet
End Class
