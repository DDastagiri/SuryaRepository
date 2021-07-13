'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090301BusinessLogic.vb
'──────────────────────────────────
'機能： ゲートキーパーメイン
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/02/13 KN  y.nakamura STEP2開発 　　　　　$01
'更新： 2012/05/23 KN  m.asano    性能改善  　　　　　$02
'更新： 2012/05/23 KN  m.asano    号口課題NO.126対応  $03
'更新： 2012/11/11 TMEJ t.shimamura  $04
'更新： 2013/04/16 TMEJ m.asano   ウェルカムボード仕様変更対応 $05
'更新： 2013/06/14 TMEJ t.shimamura 再構築DB対応 $06
'更新： 2013/10/16 TMEJ m.asano   次世代e-CRBセールス機能 新DB適応に向けた機能開発 $07
'更新： 2013/12/02 TMEJ t.shimamura   次世代e-CRBサービス 店舗展開に向けた標準作業確立 $08
'更新： 2015/02/18 TMEJ y.nakamura UAT課題#158 $09
'更新： 2015/12/17 TM y.nakamura ゲートキーパーのユーザ表示対応 $10
'──────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace SC3090301DataSetTableAdapters

    ''' <summary>
    ''' SC3090301(ゲートキーパーメイン)
    ''' データ層
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3090301TableAdapter
        Inherits Global.System.ComponentModel.Component

'$09 start UAT課題#158
#Region "定数"
        ''' <summary>
        ''' 受付区分(予約客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeRez As String = "0"

        ''' <summary>
        ''' 予約ステータス(仮予約)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RezStatusTentative As String = "0"
        
        ''' <summary>
        ''' 予約ステータス(本予約)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RezStatus As String = "1"
        
        ''' <summary>
        ''' サービスステータス(未入庫)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoIn As String = "00"

        ''' <summary>
        ''' サービスステータス(未来店)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoVisit As String = "01"
        
        ''' <summary>
        ''' キャンセルフラグ(有効)
        ''' </summary>
        Private Const CancelFlagEffective As String = "0"

        ' $10 start ゲートキーパーのユーザ表示対応
        ''' <summary>
        ''' 顧客車両区分(保険)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VehicleTypeInsurance As String = "4"
        
        ''' <summary>
        ''' オーナーチェンジフラグ(未設定)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OwnerChangeFlagNot As String = "0"

        ''' <summary>
        ''' サービス来店未取引客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustSegmentNewCustomer As String = "2"
        ' $10 end   ゲートキーパーのユーザ表示対応

#End Region
'$09 end UAT課題#158

#Region "コンストラクタ"

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>静的メソッドのみなので呼び出しなし</remarks>
        Public Sub New()

        End Sub

#End Region

#Region "来店通知未送信データ件数取得"

        ''' <summary>
        ''' 来店通知未送信データ件数取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="visitDateStart">来店日時開始</param>
        ''' <param name="visitDateEnd">来店日時終了</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetVisitUnsentTotalCount(ByVal dealerCode As String, _
                                                 ByVal storeCode As String, _
                                                 ByVal visitDateStart As Date, _
                                                 ByVal visitDateEnd As Date) As SC3090301DataSet.SC3090301VisitUnsentTotalCountDataTable

            Logger.Info("GetVisitUnsentTotalCount_Start Pram[" & dealerCode & "," & storeCode & "," & visitDateStart & "," & visitDateEnd & "]")

            Dim dt As SC3090301DataSet.SC3090301VisitUnsentTotalCountDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3090301DataSet.SC3090301VisitUnsentTotalCountDataTable)("SC3090301_001")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3090301_001 */")
                    .Append(" 	     COUNT(1) AS TOTALCOUNT")
                    .Append("   FROM TBL_VISIT_VEHICLE")
                    .Append("  WHERE DLRCD = :DLRCD")
                    .Append("    AND STRCD = :STRCD")
                    .Append("    AND VISITTIMESTAMP BETWEEN :DATEST AND :DATEED")
                    .Append("    AND DEALFLG = :DEALFLG")
                    ' $08 start 削除フラグ追加
                    .Append("    AND DELFLG = :DELFLG")
                    ' $08 end 削除フラグ追加

                End With
                query.CommandText = sql.ToString()
                sql = Nothing

                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("DATEST", OracleDbType.Date, visitDateStart)
                query.AddParameterWithTypeValue("DATEED", OracleDbType.Date, visitDateEnd)
                query.AddParameterWithTypeValue("DEALFLG", OracleDbType.Char, "0")
                ' $08 start 削除フラグ追加
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "0")
                ' $08 end 削除フラグ追加

                dt = query.GetData()
            End Using

            '検索結果返却
            Logger.Info("GetVisitUnsentTotalCount_End Ret[" & (dt IsNot Nothing) & "]")
            Return dt

        End Function
#End Region

#Region "来店通知未送信データの取得"
        ' 08 start 引数追加(行)
        ''' <summary>
        ''' 来店通知未送信データの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="visitDateStart">来店日時開始</param>
        ''' <param name="visitDateEnd">来店日時終了</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetVisitUnsentData(ByVal dealerCode As String, _
                                           ByVal storeCode As String, _
                                           ByVal visitDateStart As Date, _
                                           ByVal visitDateEnd As Date, _
                                           ByVal startRowNum As Integer, _
                                           ByVal endRowNum As Integer) As SC3090301DataSet.SC3090301VisitVehicleUnsentDataDataTable
            ' 08 end 引数追加(行)
            Logger.Info("GetVisitUnsentData_Start Pram[" & dealerCode & "," & storeCode & "," & visitDateStart & "," & visitDateEnd & "," & startRowNum & "," & endRowNum & "]")

            Dim dt As SC3090301DataSet.SC3090301VisitVehicleUnsentDataDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3090301DataSet.SC3090301VisitVehicleUnsentDataDataTable)("SC3090301_002")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    ' $08 start 削除フラグ、行指定追加
                    .Append(" SELECT /* SC3090301_002 */ ")
                    .Append("        VISITVCLSEQ ")
                    .Append("        , VISITTIMESTAMP ")
                    .Append("        , VCLREGNO ")
                    .Append("   FROM ( ")
                    .Append("        SELECT ")
                    .Append("               VISITVCLSEQ ")
                    .Append("             , VISITTIMESTAMP ")
                    .Append("             , VCLREGNO ")
                    .Append("             , ROWNUM AS ROWNUMBER ")
                    .Append("         FROM ")
                    .Append("               TBL_VISIT_VEHICLE ")
                    .Append("        WHERE DLRCD = :DLRCD ")
                    .Append("          AND STRCD = :STRCD ")
                    .Append("          AND VISITTIMESTAMP BETWEEN :DATEST AND :DATEED ")
                    .Append("          AND DEALFLG = :DEALFLG ")
                    .Append("          AND DELFLG = :DELFLG ")
                    .Append("        ORDER BY VISITTIMESTAMP ")
                    .Append("       ) ")
                    .Append(" WHERE ROWNUMBER BETWEEN :STARTROW and :ENDROW ")
                    ' $08 end 削除フラグ、行指定追加

                End With
                query.CommandText = sql.ToString()
                sql = Nothing

                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("DATEST", OracleDbType.Date, visitDateStart)
                query.AddParameterWithTypeValue("DATEED", OracleDbType.Date, visitDateEnd)
                query.AddParameterWithTypeValue("DEALFLG", OracleDbType.Char, "0")
                ' $08 start  削除フラグ、行指定追加
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "0")
                query.AddParameterWithTypeValue("STARTROW", OracleDbType.Int32, startRowNum)
                query.AddParameterWithTypeValue("ENDROW", OracleDbType.Int32, endRowNum)

                ' $08 end  削除フラグ、行指定追加

                '検索結果返却
                dt = query.GetData()

            End Using

            '検索結果返却
            Logger.Info("GetVisitUnsentData_End Ret[" & (dt IsNot Nothing) & "]")
            Return dt

        End Function

#End Region

#Region "来店車両実績テーブル更新"

        ''' <summary>
        ''' 来店車両実績テーブル更新
        ''' </summary>
        ''' <param name="visitVehicleSeq">来店車両実績連番</param>
        ''' <param name="dealFig">対応フラグ</param>
        ''' <param name="account">アカウント</param>
        ''' <param name="functionId">機能ID</param>
        ''' <returns>影響行数</returns>
        ''' <remarks></remarks>
        Public Function UpdateVisitVehicle(ByVal visitVehicleSeq As String, _
                                                  ByVal dealFig As String, _
                                                  ByVal account As String, _
                                                  ByVal functionId As String) As Integer

            Logger.Info("UpdateVisitVehicle_Start Pram[" & visitVehicleSeq & "," & dealFig & "," & _
                                                          account & "," & functionId & "," & "]")

            Dim returnValue As Integer = 0

            Using query As New DBUpdateQuery("SC3090301_006")

                'SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3090301_006 */")
                    .Append(" TBL_VISIT_VEHICLE")
                    .Append("    SET   DEALFLG = :DEALFLG")
                    .Append("        , UPDATEDATE = SYSDATE")
                    .Append("        , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("        , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITVCLSEQ = :VISITVCLSEQ")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DEALFLG", OracleDbType.Char, dealFig)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, functionId)
                query.AddParameterWithTypeValue("VISITVCLSEQ", OracleDbType.Int64, visitVehicleSeq)

                'SQL実行(影響行数を返却)
                returnValue = query.Execute()

            End Using

            Logger.Info("UpdateVisitVehicle_End Ret[" & returnValue & "]")
            Return returnValue

        End Function

#End Region

#Region "登録車両送信情報取得"

        ''' <summary>
        ''' 登録車両送信情報取得
        ''' </summary>
        ''' <param name="visitVehicleSeq">来店車両実績連番</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetDealType(ByVal visitVehicleSeq As String) As SC3090301DataSet.SC3090301VisitVehicleDealFlgDataTable

            Logger.Info("GetDealType_Start Pram[" & visitVehicleSeq & "]")

            Dim dt As SC3090301DataSet.SC3090301VisitVehicleDealFlgDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3090301DataSet.SC3090301VisitVehicleDealFlgDataTable)("SC3090301_009")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3090301_009 */")
                    .Append(" 	     DEALFLG")
                    ' $08 start 削除フラグ追加
                    .Append(" 	   , DELFLG")
                    ' $08 end 削除フラグ追加
                    .Append("   FROM TBL_VISIT_VEHICLE")
                    .Append("  WHERE VISITVCLSEQ = :VISITVCLSEQ")

                End With
                query.CommandText = sql.ToString()
                sql = Nothing

                'バインド変数
                query.AddParameterWithTypeValue("VISITVCLSEQ", OracleDbType.Int64, visitVehicleSeq)
                dt = query.GetData()

            End Using

            '検索結果返却
            Logger.Info("GetDealType_End Ret[" & (dt IsNot Nothing) & "]")
            Return dt
        End Function
#End Region

#Region "来店実績シーケンス取得"

        ''' <summary>
        ''' 来店実績シーケンス取得
        ''' </summary>
        ''' <returns>来店実績シーケンスの次番号</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function GetVisitSalesSeqNextValue() As Long

            Logger.Info("GetVisitSalesSeqNextValue_Start")

            ' 来店実績シーケンスの次番号
            Dim visitSeqNextValue As Long = 0L

            Using query As New DBSelectQuery( _
                    Of SC3090301DataSet.SC3090301VisitSequenceValueDataTable)("SC3090301_010")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" SELECT /* SC3090301_010 */")
                    .Append("        SEQ_VISIT_SALES_VISITSEQ.NEXTVAL AS VISITSEQ")
                    .Append("   FROM DUAL")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' SQLを実行
                Using dt As SC3090301DataSet.SC3090301VisitSequenceValueDataTable = query.GetData()
                    ' レコードが取得できた場合
                    If 0 < dt.Count Then
                        ' 来店実績シーケンスの次番号を取得
                        visitSeqNextValue = dt.Item(0).VISITSEQ
                    End If
                End Using
            End Using

            ' 戻り値に来店実績シーケンスの次番号を設定
            Logger.Info("GetVisitSalesSeqNextValue_End Ret[" & visitSeqNextValue & "]")
            Return visitSeqNextValue

        End Function
#End Region

        ' $08 start 削除処理追加
#Region "来店車両実績テーブル削除"

        ''' <summary>
        ''' 来店車両実績テーブル削除
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="visitDateStart">来店開始日時</param>
        ''' <param name="visitDateEnd">来店終了日時</param>
        ''' <returns>削除結果件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteVisitVehicl(ByVal dealerCode As String, _
                                          ByVal storeCode As String, _
                                          ByVal visitDateStart As Date, _
                                          ByVal visitDateEnd As Date) As Integer

            Logger.Info("DeleteVisitVehicl Pram[" & dealerCode & "," & storeCode & "," & _
                                                      visitDateStart & "," & visitDateEnd & "," & "]")

            Dim returnValue As Integer = 0

            Using query As New DBUpdateQuery("SC3090301_005")

                'SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3090301_005 */ ")
                    .Append("        TBL_VISIT_VEHICLE ")
                    .Append("    SET DELFLG = '1'  ")
                    .Append("      , UPDATEDATE = SYSDATE  ")
                    .Append("  WHERE DLRCD = :DLRCD ")
                    .Append("    AND STRCD = :STRCD ")
                    .Append("    AND VISITTIMESTAMP BETWEEN :DATEST AND :DATEED ")
                    .Append("    AND DEALFLG = :DEALFLG ")
                    .Append("    AND DEALFLG = :DELFLG ")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("DATEST", OracleDbType.Date, visitDateStart)
                query.AddParameterWithTypeValue("DATEED", OracleDbType.Date, visitDateEnd)
                query.AddParameterWithTypeValue("DEALFLG", OracleDbType.Char, "0")
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "0")
                'SQL実行(影響行数を返却)
                returnValue = query.Execute()

            End Using

            Logger.Info("DeleteVisitVehicl_End Ret[" & returnValue & "]")
            Return returnValue
        End Function

#End Region
        ' $08 end 削除処理追加

        '$09 start UAT課題#158
#Region "予約あり未送信データの取得"
        ''' <summary>
        ''' 予約あり未送信データの取得
        ''' </summary>
        ''' <param name="cstId">顧客ID</param>
        ''' <param name="vclId">車両ID</param>
        ''' <param name="startDate">予定開始日時</param>
        ''' <param name="days">日数</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetReservData(ByVal cstId As Collection, _
                                      ByVal vclId As Collection, _
                                      ByVal startDate As Date, _
                                      ByVal days As Long, _
                                      ByVal dealerCode As String, _
                                      ByVal storeCode As String) As SC3090301DataSet.SC3090301ReservDataDataTable
            ' 08 end 引数追加(行)
            Logger.Info("GetReservData_Start Pram[" & cstId.ToString & "," & vclId.ToString & "," & "]")

            Dim dt As SC3090301DataSet.SC3090301ReservDataDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3090301DataSet.SC3090301ReservDataDataTable)("SC3090301_007")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT DISTINCT /* SC3090301_007 */ ")
                    .Append("        SVC.CST_ID AS CUSTCD ")
                    .Append("      , SVC.VCL_ID AS SEQNO ")
                    .Append("   FROM TB_T_SERVICEIN SVC ")
                    .Append("      , TB_T_JOB_DTL DTL ")
                    .Append("      , TB_T_STALL_USE STU ")
                    .Append("  WHERE SVC.SVCIN_ID = DTL.SVCIN_ID ")
                    .Append("    AND DTL.JOB_DTL_ID = STU.JOB_DTL_ID ")
                    .Append("    AND SVC.DLR_CD = :DLR_CD ")
                    .Append("    AND SVC.BRN_CD = :BRN_CD ")
                    .Append("    AND SVC.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                    .Append("    AND SVC.RESV_STATUS IN (:RESV_STATUS_0, :RESV_STATUS_1) ")
                    .Append("    AND SVC.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                    .Append("    AND DTL.CANCEL_FLG = :CAMCEL_FLG_0 ")
                    .Append("    AND TRUNC(STU.SCHE_START_DATETIME) BETWEEN TRUNC(:STARTTIME) AND TRUNC(:STARTTIME + :DAYS) ")
                    .Append("    AND (SVC.CST_ID, SVC.VCL_ID) IN(")
                        For i As Decimal = 1 To cstId.Count()
                            .Append("(")
                            .Append(" :CST_ID" & CStr(i))
                            .Append(", ")
                            .Append(" :VCL_ID" & CStr(i))
                            .Append(")")
                            If Not i = cstId.Count() Then
                                .Append(",")
                            End If
                        Next
                    .Append(")")
                End With
                query.CommandText = sql.ToString()
                sql = Nothing

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, storeCode)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                query.AddParameterWithTypeValue("RESV_STATUS_0", OracleDbType.NVarchar2, RezStatusTentative)
                query.AddParameterWithTypeValue("RESV_STATUS_1", OracleDbType.NVarchar2, RezStatus)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)
                query.AddParameterWithTypeValue("CAMCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startDate)
                query.AddParameterWithTypeValue("DAYS", OracleDbType.Long, days)
                For j As Decimal = 1 To cstId.Count()
                    query.AddParameterWithTypeValue("CST_ID" & CStr(j), OracleDbType.NVarchar2, cstId.Item(j))
                    query.AddParameterWithTypeValue("VCL_ID" & CStr(j), OracleDbType.NVarchar2, vclId.Item(j))
                Next

                '検索結果返却
                dt = query.GetData()

            End Using

            '検索結果返却
            Logger.Info("GetReservData_End Ret[" & (dt IsNot Nothing) & "]")
            Return dt

        End Function

#End Region
        '$09 end UAT課題#158


        ' $10 start ゲートキーパーのユーザ表示対応
#Region "予約あり未送信データの取得"
        ''' <summary>
        ''' 予約あり未送信データの取得(全て)
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="regNumList">車両登録番号リスト</param>
        ''' <param name="startDate">予定開始日時</param>
        ''' <param name="days">日数</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetAllReservData(ByVal dealerCode As String, _
                                          ByVal storeCode As String, _
                                          ByVal regNumList As Collection, _
                                          ByVal startDate As Date, _
                                          ByVal days As Long ) As SC3090301DataSet.SC3090301AllReservDataDataTable

            Logger.Info("GetAllReservData_Start Pram[" & regNumList.ToString & "]")

            Dim dt As SC3090301DataSet.SC3090301AllReservDataDataTable = Nothing

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3090301_008 */   ")
                .Append("        T1.REG_NUM AS VCLREGNO ")
                .Append("      , T6.CST_NAME AS NAME ")
                .Append("      , T6.NAMETITLE_NAME AS NAMETITLE ")
                .Append("      , T5.SCHE_START_DATETIME AS SCHESTARTDATETIME ")
                .Append("      , T7.CST_TYPE AS CUSTOMERFLAG  ")
                .Append("      , T2.CST_VCL_TYPE AS CUSTVCLTYPE ")
                .Append("   FROM TB_M_VEHICLE_DLR T1  ")
                .Append("      , TB_M_CUSTOMER_VCL T2  ")
                .Append("      , TB_T_SERVICEIN T3  ")
                .Append("      , TB_T_JOB_DTL T4  ")
                .Append("      , TB_T_STALL_USE T5  ")
                .Append("      , TB_M_CUSTOMER T6  ")
                .Append("      , TB_M_CUSTOMER_DLR T7  ")
                .Append("  WHERE T1.DLR_CD = T2.DLR_CD  ")
                .Append("    AND T1.VCL_ID = T2.VCL_ID  ")
                .Append("    AND T2.CST_ID = T3.CST_ID  ")
                .Append("    AND T2.VCL_ID = T3.VCL_ID  ")
                .Append("    AND T3.SVCIN_ID = T4.SVCIN_ID  ")
                .Append("    AND T4.JOB_DTL_ID = T5.JOB_DTL_ID  ")
                .Append("    AND T3.CST_ID = T6.CST_ID  ")
                .Append("    AND T6.CST_ID = T7.CST_ID  ")
                .Append("    AND T1.REG_NUM_SEARCH IN(")
                For i As Decimal = 1 To regNumList.Count()
                    .Append(" :REG_NUM" & CStr(i))
                    If Not i = regNumList.Count() Then
                        .Append(",")
                    End If
                Next
                .Append(")")
                .Append("    AND T1.DLR_CD = :DLR_CD  ")
                .Append("    AND T2.CST_VCL_TYPE <> :CST_VCL_TYPE_INS   ")
                .Append("    AND T2.OWNER_CHG_FLG = :OWNER_CHG_FLG_NOT  ")
                .Append("    AND T3.DLR_CD = :DLR_CD  ")
                .Append("    AND T3.BRN_CD = :BRN_CD  ")
                .Append("    AND T3.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_REZ  ")
                .Append("    AND T3.RESV_STATUS IN (:RESV_STATUS_TEN, :RESV_STATUS)  ")
                .Append("    AND T3.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01)  ")
                .Append("    AND T4.DLR_CD = :DLR_CD  ")
                .Append("    AND T4.BRN_CD = :BRN_CD  ")
                .Append("    AND T4.CANCEL_FLG = :CANCEL_FLG  ")
                .Append("    AND T5.DLR_CD = :DLR_CD  ")
                .Append("    AND T5.BRN_CD = :BRN_CD  ")
                .Append("    AND TRUNC(T5.SCHE_START_DATETIME) BETWEEN TRUNC(:VISIT_DATE) AND TRUNC(:VISIT_DATE + :BOOK_BY_DATES)  ")
                .Append("    AND T7.DLR_CD = :DLR_CD  ")
            End With

            Using query As New DBSelectQuery(Of SC3090301DataSet.SC3090301AllReservDataDataTable)("SC3090301_008")

                query.CommandText = sql.ToString()
                sql = Nothing

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, storeCode)
                query.AddParameterWithTypeValue("VISIT_DATE", OracleDbType.Date, startDate)
                query.AddParameterWithTypeValue("BOOK_BY_DATES", OracleDbType.Long, days)
                query.AddParameterWithTypeValue("RESV_STATUS_TEN", OracleDbType.NVarchar2, RezStatusTentative)
                query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, RezStatus)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_REZ", OracleDbType.NVarchar2, AcceptanceTypeRez)
                query.AddParameterWithTypeValue("CST_VCL_TYPE_INS", OracleDbType.NVarchar2, VehicleTypeInsurance)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_NOT", OracleDbType.NVarchar2, OwnerChangeFlagNot)
                For i As Decimal = 1 To regNumList.Count()
                    query.AddParameterWithTypeValue("REG_NUM" & CStr(i), OracleDbType.NVarchar2, regNumList.Item(i))
                Next

                '検索結果返却
                dt = query.GetData()

            End Using

            '検索結果返却
            Logger.Info("GetAllReservData_End Ret[" & (dt IsNot Nothing) & "]")
            Return dt

        End Function

#End Region
        ' $10 end   ゲートキーパーのユーザ表示対応

    End Class
End Namespace

Partial Class SC3090301DataSet
End Class
