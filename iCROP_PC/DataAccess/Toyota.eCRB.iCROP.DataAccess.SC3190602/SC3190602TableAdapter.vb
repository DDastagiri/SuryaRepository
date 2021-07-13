'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190602TableAdapter.vb
'─────────────────────────────────────
'機能： B/O部品入力 (データ)
'補足： 
'作成： 2014/08/25 TMEJ M.Asano
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public NotInheritable Class SC3190602TableAdapter

#Region "コンストラクタ"

    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

#End Region

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3190602"

    ''' <summary>
    ''' 行ロックバージョン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RowLockVersion As Decimal = 0

#End Region

#Region "公開メソッド"

#Region "B/O情報取得"

    ''' <summary>
    ''' B/O情報取得
    ''' </summary>
    ''' <param name="BoId">B/O Id</param>
    ''' <returns>BoInfoDataTable(B/O情報)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBoPartsInfo(ByVal boId As Decimal) _
                                          As SC3190602DataSet.BoInfoDataTable

        Dim boInfoDataTable As SC3190602DataSet.BoInfoDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3190602DataSet.BoInfoDataTable)("SC3190602_001")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3190602_001 */ ")
                .Append("        SUB.BO_ID ")
                .Append("      , SUB.PO_NUM ")
                .Append("      , SUB.RO_NUM ")
                .Append("      , SUB.VCL_PARTAKE_FLG ")
                .Append("      , SUB.CST_APPOINTMENT_DATE ")
                .Append("      , SUB.BO_JOB_ID ")
                .Append("      , SUB.JOB_NAME ")
                .Append("      , SUB.PARTS_NAME ")
                .Append("      , SUB.PARTS_CD ")
                .Append("      , SUB.PARTS_AMOUNT ")
                .Append("      , SUB.ODR_DATE ")
                .Append("      , SUB.ARRIVAL_SCHE_DATE ")
                .Append("   FROM ( ")
                .Append("         SELECT MGR.BO_ID ")
                .Append("              , BO.BO_JOB_ID ")
                .Append("              , PARTS.BO_PARTS_ID ")
                .Append("              , MGR.PO_NUM ")
                .Append("              , MGR.RO_NUM ")
                .Append("              , BO.JOB_NAME ")
                .Append("              , PARTS.PARTS_NAME ")
                .Append("              , PARTS.PARTS_CD ")
                .Append("              , PARTS.PARTS_AMOUNT ")
                .Append("              , PARTS.ODR_DATE ")
                .Append("              , PARTS.ARRIVAL_SCHE_DATE ")
                .Append("              , MGR.VCL_SVCIN_FLG AS VCL_PARTAKE_FLG ")
                .Append("              , MGR.CST_APPO_DATE AS CST_APPOINTMENT_DATE ")
                .Append("              , MIN(CASE WHEN PARTS.ARRIVAL_SCHE_DATE = :DEFAULT_DATE THEN NULL ELSE PARTS.ARRIVAL_SCHE_DATE END) OVER (PARTITION BY MGR.BO_ID) AS SORT_KEY1 ")
                .Append("              , MIN(CASE WHEN PARTS.ARRIVAL_SCHE_DATE = :DEFAULT_DATE THEN NULL ELSE PARTS.ARRIVAL_SCHE_DATE END) OVER (PARTITION BY MGR.BO_ID, BO.BO_JOB_ID) AS SORT_KEY2 ")
                .Append("              , MIN(CASE WHEN PARTS.ODR_DATE = :DEFAULT_DATE THEN NULL ELSE PARTS.ODR_DATE END) OVER (PARTITION BY MGR.BO_ID, BO.BO_JOB_ID) AS SORT_KEY3  ")
                .Append("           FROM TB_T_BO_MNG_INFO MGR ")
                .Append("              , TB_T_BO_JOB_INFO BO ")
                .Append("              , TB_T_BO_PARTS_INFO PARTS ")
                .Append("          WHERE MGR.BO_ID = BO.BO_ID ")
                .Append("            AND BO.BO_JOB_ID = PARTS.BO_JOB_ID ")
                .Append("            AND MGR.BO_ID = :BO_ID ")
                .Append("         ) SUB ")
                .Append("  ORDER BY SUB.SORT_KEY1 ")
                .Append("         , SUB.RO_NUM ")
                .Append("         , SUB.BO_ID ")
                .Append("         , SUB.SORT_KEY2 ")
                .Append("         , SUB.SORT_KEY3 ")
                .Append("         , SUB.BO_JOB_ID ")
                .Append("         , CASE WHEN SUB.ARRIVAL_SCHE_DATE = :DEFAULT_DATE THEN NULL ELSE SUB.ARRIVAL_SCHE_DATE END ")
                .Append("         , CASE WHEN SUB.ODR_DATE = :DEFAULT_DATE THEN NULL ELSE SUB.ODR_DATE END ")
                .Append("         , SUB.BO_PARTS_ID ")
            End With

            query.CommandText = sql.ToString()
            sql = Nothing

            'バインド変数
            query.AddParameterWithTypeValue("BO_ID", OracleDbType.Decimal, boId)
            query.AddParameterWithTypeValue("DEFAULT_DATE", OracleDbType.Date, New Date(1900, 1, 1, 0, 0, 0))

            'クエリ実行
            boInfoDataTable = query.GetData()
        End Using

        ' 検索結果返却
        Return boInfoDataTable
    End Function

#End Region

#Region "B/O部品管理情報作成"

    ''' <summary>
    ''' B/O部品管理情報作成
    ''' </summary>
    ''' <param name="boMngInfoRow">B/O部品管理DataRow</param>
    ''' <returns>True：成功 / False：失敗</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertBoMngInfo(ByVal boMngInfoRow As SC3190602DataSet.BoMngInfoRow) As Boolean

        ' 更新対象レコード件数
        Dim record As Integer = 0

        Using query As New DBUpdateQuery("SC3190602_002")
            Dim sql As New StringBuilder

            ' SQL文作成
            With sql
                .Append(" INSERT /* SC3190602_002 */ ")
                .Append("   INTO TB_T_BO_MNG_INFO ( ")
                .Append("        BO_ID ")
                .Append("      , DLR_CD ")
                .Append("      , BRN_CD ")
                .Append("      , PO_NUM ")
                .Append("      , RO_NUM ")
                .Append("      , VCL_SVCIN_FLG ")
                .Append("      , CST_APPO_DATE ")
                .Append("      , ROW_CREATE_DATETIME ")
                .Append("      , ROW_CREATE_ACCOUNT ")
                .Append("      , ROW_CREATE_FUNCTION ")
                .Append("      , ROW_UPDATE_DATETIME ")
                .Append("      , ROW_UPDATE_ACCOUNT ")
                .Append("      , ROW_UPDATE_FUNCTION ")
                .Append("      , ROW_LOCK_VERSION ")
                .Append("   ) ")
                .Append(" VALUES ( ")
                .Append("        :BO_ID ")
                .Append("      , :DLR_CD ")
                .Append("      , :BRN_CD ")
                .Append("      , :PO_NUM ")
                .Append("      , :RO_NUM ")
                .Append("      , :VCL_SVCIN_FLG ")
                .Append("      , :CST_APPO_DATE ")
                .Append("      , :NOW_DATE ")
                .Append("      , :ACCOUNT ")
                .Append("      , :FUNCTION ")
                .Append("      , :NOW_DATE ")
                .Append("      , :ACCOUNT ")
                .Append("      , :FUNCTION ")
                .Append("      , :ROW_LOCK_VERSION ")
                .Append("   ) ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            'バインド変数
            query.AddParameterWithTypeValue("BO_ID", OracleDbType.Decimal, boMngInfoRow.BO_ID)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, boMngInfoRow.DLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, boMngInfoRow.BRN_CD)
            query.AddParameterWithTypeValue("PO_NUM", OracleDbType.NVarchar2, boMngInfoRow.PO_NUM)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, boMngInfoRow.RO_NUM)
            query.AddParameterWithTypeValue("VCL_SVCIN_FLG", OracleDbType.NVarchar2, boMngInfoRow.VCL_PARTAKE_FLG)
            query.AddParameterWithTypeValue("CST_APPO_DATE", OracleDbType.Date, boMngInfoRow.CST_APPOINTMENT_DATE)
            query.AddParameterWithTypeValue("NOW_DATE", OracleDbType.Date, boMngInfoRow.NOW_DATE)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, boMngInfoRow.ACCOUNT)
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, ApplicationId)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, RowLockVersion)

            ' SQLの実行
            record = query.Execute()
        End Using

        ' 処理結果
        Dim isSuccess As Boolean = False

        ' 実行結果が0件超過の場合
        If 0 < record Then
            ' 処理結果に成功を設定
            isSuccess = True
        End If

        ' 戻り値に処理結果を設定
        Return isSuccess

    End Function

#End Region

#Region "B/O部品管理情報更新"

    ''' <summary>
    ''' B/O部品管理情報更新
    ''' </summary>
    ''' <param name="boMngInfoRow">B/O部品管理DataRow</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateBoMngInfo(ByVal boMngInfoRow As SC3190602DataSet.BoMngInfoRow) As Integer

        ' 更新対象レコード件数
        Dim record As Integer = 0

        Using query As New DBUpdateQuery("SC3190602_003")
            Dim sql As New StringBuilder

            ' SQL文作成
            With sql
                .Append(" UPDATE /* SC3190602_003 */ ")
                .Append("        TB_T_BO_MNG_INFO ")
                .Append("    SET PO_NUM = :PO_NUM ")
                .Append("      , RO_NUM = :RO_NUM ")
                .Append("      , VCL_SVCIN_FLG = :VCL_SVCIN_FLG ")
                .Append("      , CST_APPO_DATE = :CST_APPO_DATE ")
                .Append("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                .Append("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
                .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
                .Append("  WHERE BO_ID = :BO_ID ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' バインド変数を設定
            query.AddParameterWithTypeValue("BO_ID", OracleDbType.Decimal, boMngInfoRow.BO_ID)
            query.AddParameterWithTypeValue("PO_NUM", OracleDbType.NVarchar2, boMngInfoRow.PO_NUM)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, boMngInfoRow.RO_NUM)
            query.AddParameterWithTypeValue("VCL_SVCIN_FLG", OracleDbType.NVarchar2, boMngInfoRow.VCL_PARTAKE_FLG)
            query.AddParameterWithTypeValue("CST_APPO_DATE", OracleDbType.Date, boMngInfoRow.CST_APPOINTMENT_DATE)
            query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, boMngInfoRow.NOW_DATE)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, boMngInfoRow.ACCOUNT)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, ApplicationId)

            ' SQLの実行
            record = query.Execute()
        End Using

        ' 戻り値に更新対象レコード件数を設定
        Return record

    End Function

#End Region

#Region "B/O部品管理情報削除"

    ''' <summary>
    ''' B/O部品管理情報削除
    ''' </summary>
    ''' <param name="boId"></param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteBoMngInfo(ByVal boId As Decimal) As Integer

        ' 更新対象レコード件数
        Dim record As Integer = 0

        Using query As New DBUpdateQuery("SC3190602_004")
            Dim sql As New StringBuilder

            With sql
                .Append(" DELETE /* SC3190602_004 */ ")
                .Append("   FROM TB_T_BO_MNG_INFO ")
                .Append("  WHERE BO_ID = :BO_ID ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' バインド変数を設定
            query.AddParameterWithTypeValue("BO_ID", OracleDbType.Decimal, boId)

            ' SQLの実行
            record = query.Execute()
        End Using

        ' 戻り値に更新対象レコード件数を設定
        Return record

    End Function

#End Region

#Region "B/O作業情報作成"

    ''' <summary>
    ''' B/O作業情報作成
    ''' </summary>
    ''' <param name="boJobInfoRow">B/O作業情報DataRow</param>
    ''' <returns>True：成功 / False：失敗</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertBoJobInfo(ByVal boJobInfoRow As SC3190602DataSet.BoJobInfoRow) As Boolean

        ' 更新対象レコード件数
        Dim record As Integer = 0

        Using query As New DBUpdateQuery("SC3190602_005")
            Dim sql As New StringBuilder

            ' SQL文作成
            With sql
                .Append(" INSERT /* SC3190602_005 */ ")
                .Append("   INTO TB_T_BO_JOB_INFO ( ")
                .Append("        BO_ID ")
                .Append("      , BO_JOB_ID ")
                .Append("      , JOB_NAME ")
                .Append("      , ROW_CREATE_DATETIME ")
                .Append("      , ROW_CREATE_ACCOUNT ")
                .Append("      , ROW_CREATE_FUNCTION ")
                .Append("      , ROW_UPDATE_DATETIME ")
                .Append("      , ROW_UPDATE_ACCOUNT ")
                .Append("      , ROW_UPDATE_FUNCTION ")
                .Append("      , ROW_LOCK_VERSION ")
                .Append("   ) ")
                .Append(" VALUES ( ")
                .Append("        :BO_ID ")
                .Append("      , :BO_JOB_ID ")
                .Append("      , :JOB_NAME ")
                .Append("      , :NOW_DATE ")
                .Append("      , :ACCOUNT ")
                .Append("      , :FUNCTION ")
                .Append("      , :NOW_DATE ")
                .Append("      , :ACCOUNT ")
                .Append("      , :FUNCTION ")
                .Append("      , :ROW_LOCK_VERSION ")
                .Append("   ) ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            'バインド変数
            query.AddParameterWithTypeValue("BO_ID", OracleDbType.Decimal, boJobInfoRow.BO_ID)
            query.AddParameterWithTypeValue("BO_JOB_ID", OracleDbType.Decimal, boJobInfoRow.BO_JOB_ID)
            query.AddParameterWithTypeValue("JOB_NAME", OracleDbType.NVarchar2, boJobInfoRow.JOB_NAME)
            query.AddParameterWithTypeValue("NOW_DATE", OracleDbType.Date, boJobInfoRow.NOW_DATE)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, boJobInfoRow.ACCOUNT)
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, ApplicationId)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, RowLockVersion)

            ' SQLの実行
            record = query.Execute()
        End Using

        ' 処理結果
        Dim isSuccess As Boolean = False

        ' 実行結果が0件超過の場合
        If 0 < record Then
            ' 処理結果に成功を設定
            isSuccess = True
        End If

        ' 戻り値に処理結果を設定
        Return isSuccess

    End Function

#End Region

#Region "B/O作業情報削除"

    ''' <summary>
    ''' B/O作業情報削除
    ''' </summary>
    ''' <param name="boId">B/O ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteBoJobInfo(ByVal boId As Decimal) As Integer

        ' 更新対象レコード件数
        Dim record As Integer = 0

        Using query As New DBUpdateQuery("SC3190602_006")
            Dim sql As New StringBuilder

            With sql
                .Append(" DELETE /* SC3190602_006 */ ")
                .Append("   FROM TB_T_BO_JOB_INFO ")
                .Append("  WHERE BO_ID = :BO_ID ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' バインド変数を設定
            query.AddParameterWithTypeValue("BO_ID", OracleDbType.Decimal, boId)

            ' SQLの実行
            record = query.Execute()
        End Using

        ' 戻り値に更新対象レコード件数を設定
        Return record

    End Function

#End Region

#Region "B/O部品情報作成"

    ''' <summary>
    ''' B/O部品情報作成
    ''' </summary>
    ''' <param name="boPartsInfoRow">B/O部品情報DataRow</param>
    ''' <returns>True：成功 / False：失敗</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertPartsInfo(ByVal boPartsInfoRow As SC3190602DataSet.BoPartsInfoRow) As Boolean

        ' 更新対象レコード件数
        Dim record As Integer = 0

        Using query As New DBUpdateQuery("SC3190602_007")
            Dim sql As New StringBuilder

            ' SQL文作成
            With sql
                .Append(" INSERT /* SC3190602_007 */ ")
                .Append("   INTO TB_T_BO_PARTS_INFO ( ")
                .Append("        BO_JOB_ID ")
                .Append("      , BO_PARTS_ID ")
                .Append("      , PARTS_NAME ")
                .Append("      , PARTS_CD ")
                .Append("      , PARTS_AMOUNT ")
                .Append("      , ODR_DATE ")
                .Append("      , ARRIVAL_SCHE_DATE ")
                .Append("      , ROW_CREATE_DATETIME ")
                .Append("      , ROW_CREATE_ACCOUNT ")
                .Append("      , ROW_CREATE_FUNCTION ")
                .Append("      , ROW_UPDATE_DATETIME ")
                .Append("      , ROW_UPDATE_ACCOUNT ")
                .Append("      , ROW_UPDATE_FUNCTION ")
                .Append("      , ROW_LOCK_VERSION ")
                .Append("   ) ")
                .Append(" VALUES ( ")
                .Append("        :BO_JOB_ID ")
                .Append("      , SQ_BO_PARTS_INFO_ID.NEXTVAL ")
                .Append("      , :PARTS_NAME ")
                .Append("      , :PARTS_CD ")
                .Append("      , :PARTS_AMOUNT ")
                .Append("      , :ODR_DATE ")
                .Append("      , :ARRIVAL_SCHE_DATE ")
                .Append("      , :NOW_DATE ")
                .Append("      , :ACCOUNT ")
                .Append("      , :FUNCTION ")
                .Append("      , :NOW_DATE ")
                .Append("      , :ACCOUNT ")
                .Append("      , :FUNCTION ")
                .Append("      , :ROW_LOCK_VERSION ")
                .Append("   ) ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            'バインド変数
            query.AddParameterWithTypeValue("BO_JOB_ID", OracleDbType.Decimal, boPartsInfoRow.BO_JOB_ID)
            query.AddParameterWithTypeValue("PARTS_NAME", OracleDbType.NVarchar2, boPartsInfoRow.PARTS_NAME)
            query.AddParameterWithTypeValue("PARTS_CD", OracleDbType.NVarchar2, boPartsInfoRow.PARTS_CD)
            query.AddParameterWithTypeValue("PARTS_AMOUNT", OracleDbType.Decimal, boPartsInfoRow.PARTS_AMOUNT)
            query.AddParameterWithTypeValue("ODR_DATE", OracleDbType.Date, boPartsInfoRow.ODR_DATE)
            query.AddParameterWithTypeValue("ARRIVAL_SCHE_DATE", OracleDbType.Date, boPartsInfoRow.ARRIVAL_SCHE_DATE)
            query.AddParameterWithTypeValue("NOW_DATE", OracleDbType.Date, boPartsInfoRow.NOW_DATE)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, boPartsInfoRow.ACCOUNT)
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, ApplicationId)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, RowLockVersion)

            ' SQLの実行
            record = query.Execute()
        End Using

        ' 処理結果
        Dim isSuccess As Boolean = False

        ' 実行結果が0件超過の場合
        If 0 < record Then
            ' 処理結果に成功を設定
            isSuccess = True
        End If

        ' 戻り値に処理結果を設定
        Return isSuccess

    End Function

#End Region

#Region "B/O部品情報削除"

    ''' <summary>
    ''' B/O作業情報削除
    ''' </summary>
    ''' <param name="boId">B/O ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeletePartsInfo(ByVal boId As Decimal) As Integer

        ' 更新対象レコード件数
        Dim record As Integer = 0

        Using query As New DBUpdateQuery("SC3190602_008")
            Dim sql As New StringBuilder

            With sql
                .Append(" DELETE /* SC3190602_008 */ ")
                .Append("   FROM TB_T_BO_PARTS_INFO ")
                .Append("  WHERE EXISTS ( ")
                .Append("     SELECT ")
                .Append("         1 ")
                .Append("       FROM TB_T_BO_JOB_INFO JOB ")
                .Append("      WHERE JOB.BO_JOB_ID = TB_T_BO_PARTS_INFO.BO_JOB_ID ")
                .Append("        AND JOB.BO_ID = :BO_ID ")
                .Append("  ) ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' バインド変数を設定
            query.AddParameterWithTypeValue("BO_ID", OracleDbType.Decimal, boId)

            ' SQLの実行
            record = query.Execute()
        End Using

        ' 戻り値に更新対象レコード件数を設定
        Return record

    End Function
#End Region

#Region "B/O ID取得"
    ''' <summary>
    ''' B/O ID取得
    ''' </summary>
    ''' <returns>B/O IDの次番号</returns>
    ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
    ''' <remarks></remarks>
    Public Shared Function GetBoIdNextValue() As Decimal

        ' B/O IDの次番号
        Dim boIdNextValue As Decimal = 0

        Using query As New DBSelectQuery(Of SC3190602DataSet.BoIdDataTable)("SC3190601_001")
            Dim sql As New StringBuilder

            ' SQL文作成
            With sql
                .Append(" SELECT /* SC3190602_009 */ ")
                .Append("        SQ_BO_MNG_INFO_ID.NEXTVAL AS BO_ID ")
                .Append("   FROM DUAL ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' SQLを実行
            Using dt As SC3190602DataSet.BoIdDataTable = query.GetData()
                ' レコードが取得できた場合
                If 0 < dt.Count Then
                    ' 来店実績シーケンスの次番号を取得
                    boIdNextValue = dt.Item(0).BO_ID
                End If
            End Using

        End Using

        ' 戻り値にB/O IDの次番号を設定
        Return boIdNextValue
    End Function

#End Region

#Region "B/O Job ID取得"
    ''' <summary>
    ''' B/O Job ID取得
    ''' </summary>
    ''' <returns>B/O Job IDの次番号</returns>
    ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
    ''' <remarks></remarks>
    Public Shared Function GetBoJobIdNextValue() As Decimal

        ' B/O JobIDの次番号
        Dim boJobIdNextValue As Decimal = 0

        Using query As New DBSelectQuery(Of SC3190602DataSet.BoJobIdDataTable)("SC3190601_010")
            Dim sql As New StringBuilder

            ' SQL文作成
            With sql
                .Append(" SELECT /* SC3190602_010 */ ")
                .Append("        SQ_BO_JOB_INFO_ID.NEXTVAL AS BO_JOB_ID ")
                .Append("   FROM DUAL ")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' SQLを実行
            Using dt As SC3190602DataSet.BoJobIdDataTable = query.GetData()
                ' レコードが取得できた場合
                If 0 < dt.Count Then
                    ' B/O JobIDの次番号を取得
                    boJobIdNextValue = dt.Item(0).BO_JOB_ID
                End If
            End Using

        End Using

        ' 戻り値にB/O JobIDの次番号を設定
        Return boJobIdNextValue
    End Function
#End Region

#End Region

End Class
