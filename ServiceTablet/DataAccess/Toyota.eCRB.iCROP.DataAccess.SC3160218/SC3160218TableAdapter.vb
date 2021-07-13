'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3160218TableAdapter.vb
'─────────────────────────────────────
'機能： RO作成機能グローバル連携処理
'補足： 
'作成： 2013/11/25 SKFC 久代 
'更新： 
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' SC3160218データアクセスクラス
''' </summary>
''' <remarks></remarks>
Public Class SC3160218TableAdapter

#Region "定数"
    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramId As String = "SC3160218"

    ''' <summary>
    ''' 更新関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateFunction As String = "SC3160218"

    ''' <summary>
    ''' NoDamageフラグ初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InitNoDamageFlg As String = "0"

    ''' <summary>
    ''' Can'tCheckフラグ初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InitCanNotCheckFlg As String = "0"

    ''' <summary>
    ''' ダメージ情報初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InitDamageTypeExitst As String = "-"

    ''' <summary>
    ''' メモ初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InitMemo As String = " "

    ''' <summary>
    ''' サムネイルID初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InitThumbnailId As Decimal = -1

    ''' <summary>
    ''' 基幹コード変換販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChangeDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 基幹コード変換タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChangeCodeType As String = "2"

    ''' <summary>
    ''' 表示フラグOFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispFlg_Off As String = "0"
#End Region

#Region "公開メソッド"
    ''' <summary>
    ''' RO損傷種類情報の取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDamageTypeInfo() As SC3160218DataSet.TB_M_RO_DAMAGE_TYPEDataTable
        Logger.Info("SC3160218TableAdapter.GetDamageTypeInfo function Begin.")

        Dim response As SC3160218DataSet.TB_M_RO_DAMAGE_TYPEDataTable

        Using query As New DBSelectQuery(Of SC3160218DataSet.TB_M_RO_DAMAGE_TYPEDataTable)("SC3160218_001")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3160218_001 */ ")
                .Append("          DAMAGE_TYPE ")
                .Append("        , DAMAGE_WORD_NUM ")
                .Append("        , GRADATION_FROM ")
                .Append("        , GRADATION_TO ")
                .Append(" FROM     TB_M_RO_DAMAGE_TYPE ")
                .Append(" ORDER BY ORDER_NUM ")
            End With
            query.CommandText = sql.ToString()

            response = query.GetData()
        End Using

        Logger.Info("SC3160218TableAdapter.GetDamageTypeInfo function End.")

        Return response
    End Function

    ''' <summary>
    ''' RO外装情報の取得
    ''' </summary>
    ''' <param name="VISIT_SEQ">来店実績連番</param>
    ''' <param name="RO_NUM">RO番号</param>
    ''' <param name="DLR_CD">販売店コード</param>
    ''' <param name="BRN_CD">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetExteriorInfo(ByVal VISIT_SEQ As Decimal,
                                           ByVal RO_NUM As String,
                                           ByVal DLR_CD As String,
                                           ByVal BRN_CD As String) As SC3160218DataSet.RO_EXTERIORDataTable
        Logger.Info("SC3160218TableAdapter.GetExteriorInfo function Begin.")

        Dim result As SC3160218DataSet.RO_EXTERIORDataTable

        Using query As New DBSelectQuery(Of SC3160218DataSet.RO_EXTERIORDataTable)("SC3160218_002")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3160218_002 */ ")
                .Append("        RO_EXTERIOR_ID ")
                .Append("      , NO_DAMAGE_FLG ")
                .Append("      , CANNOT_CHECK_FLG ")
                .Append(" FROM   TB_T_RO_EXTERIOR ")
                .Append(" WHERE  ")
            End With

            ' 検索条件設定
            If 0 <= VISIT_SEQ Then
                '来店実績連番が存在す場合は来店実績連番で検索する
                sql.Append("        VISIT_SEQ = :VISIT_SEQ ")
            Else
                sql.Append("        RO_NUM = :RO_NUM ")
            End If
            sql.Append(" AND    DLR_CD = :DLR_CD ")
            sql.Append(" AND    BRN_CD = :BRN_CD ")

            query.CommandText = sql.ToString()
            'バインド変数
            If 0 <= VISIT_SEQ Then
                query.AddParameterWithTypeValue("VISIT_SEQ", OracleDbType.Decimal, VISIT_SEQ)
            Else
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, RO_NUM)
            End If
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, DLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, BRN_CD)

            result = query.GetData()
        End Using

        Logger.Info("SC3160218TableAdapter.GetExteriorInfo function End.")

        Return result
    End Function

    ''' <summary>
    ''' RO外装情報の取得
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID">RO外装ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetExteriorInfoFromId(ByVal RO_EXTERIOR_ID As Decimal) As SC3160218DataSet.RO_EXTERIORDataTable
        Logger.Info("SC3160218TableAdapter.GetExteriorInfoFromId function Begin.")

        Dim result As SC3160218DataSet.RO_EXTERIORDataTable

        Using query As New DBSelectQuery(Of SC3160218DataSet.RO_EXTERIORDataTable)("SC3160218_003")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3160218_003 */ ")
                .Append("        RO_EXTERIOR_ID ")
                .Append("      , NO_DAMAGE_FLG ")
                .Append("      , CANNOT_CHECK_FLG ")
                .Append(" FROM   TB_T_RO_EXTERIOR ")
                .Append(" WHERE  RO_EXTERIOR_ID = :RO_EXTERIOR_ID ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            result = query.GetData()
        End Using

        Logger.Info("SC3160218TableAdapter.GetExteriorInfoFromId function End.")

        Return result
    End Function

    ''' <summary>
    ''' RO外装損傷情報の取得
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID">外装ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDamageInfo(ByVal RO_EXTERIOR_ID As Decimal) As SC3160218DataSet.TB_T_RO_EXTERIOR_DAMAGEDataTable
        Logger.Info("SC3160218TableAdapter.GetDamageInfo function Begin.")

        Dim result As SC3160218DataSet.TB_T_RO_EXTERIOR_DAMAGEDataTable

        Using query As New DBSelectQuery(Of SC3160218DataSet.TB_T_RO_EXTERIOR_DAMAGEDataTable)("SC3160218_004")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3160218_004 */ ")
                .Append("        RO_EXTERIOR_ID ")
                .Append("      , PARTS_TYPE ")
                .Append("      , DAMAGE_TYPE_EXISTS ")
                .Append("      , RO_THUMBNAIL_ID ")
                .Append(" FROM   TB_T_RO_EXTERIOR_DAMAGE ")
                .Append(" WHERE  RO_EXTERIOR_ID = :RO_EXTERIOR_ID ")

            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            result = query.GetData()
        End Using

        Logger.Info("SC3160218TableAdapter.GetDamageInfo function End.")

        Return result
    End Function


    ''' <summary>
    ''' 新規行データ追加
    ''' </summary>
    ''' <param name="DLR_CD">販売店コード</param>
    ''' <param name="BRN_CD">店舗コード</param>
    ''' <param name="VISIT_SEQ">来店実績連番</param>
    ''' <param name="BASREZ_ID">基幹予約ID</param>
    ''' <param name="RO_NUM">RO番号</param>
    ''' <param name="RO_SEQ_NUM">ROシーケンス番号(RO枝番号)</param>
    ''' <param name="VIN">VIN</param>
    ''' <param name="UserID">eCRB LoginID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddExteriorInfo(ByVal DLR_CD As String,
                                           ByVal BRN_CD As String,
                                           ByVal VISIT_SEQ As Decimal,
                                           ByVal BASREZ_ID As String,
                                           ByVal RO_NUM As String,
                                           ByVal RO_SEQ_NUM As Decimal,
                                           ByVal VIN As String,
                                           ByVal UserID As String) As Decimal
        Logger.Info("SC3160218TableAdapter.AddExteriorInfo function Begin.")

        '新規外装ID
        Dim newExteriorId As Decimal = SC3160218TableAdapter.GetExteriorSeqNextValue()

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160218_005")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("INSERT /* SC3160218_005 */ ")
                .AppendLine("  INTO TB_T_RO_EXTERIOR(")
                .AppendLine("       RO_EXTERIOR_ID")
                .AppendLine("     , DLR_CD")
                .AppendLine("     , BRN_CD")
                .AppendLine("     , VISIT_SEQ")
                .AppendLine("     , BASREZ_ID")
                .AppendLine("     , RO_NUM")
                .AppendLine("     , RO_SEQ_NUM")
                .AppendLine("     , VIN_NUM")
                .AppendLine("     , NO_DAMAGE_FLG")
                .AppendLine("     , CANNOT_CHECK_FLG")
                .AppendLine("     , ROW_CREATE_DATETIME")
                .AppendLine("     , ROW_CREATE_ACCOUNT")
                .AppendLine("     , ROW_CREATE_FUNCTION")
                .AppendLine(") ")
                .AppendLine("VALUES ")
                .AppendLine("( ")
                .AppendLine("       :RO_EXTERIOR_ID")
                .AppendLine("     , :DLR_CD")
                .AppendLine("     , :BRN_CD")
                .AppendLine("     , :VISIT_SEQ")
                .AppendLine("     , :BASREZ_ID")
                .AppendLine("     , :RO_NUM")
                .AppendLine("     , :RO_SEQ_NUM")
                .AppendLine("     , :VIN")
                .AppendLine("     , :NO_DAMAGE_FLG")
                .AppendLine("     , :CANNOT_CHECK_FLG")
                .AppendLine("     , SYSDATE")
                .AppendLine("     , :ROW_CREATE_ACCOUNT")
                .AppendLine("     , :ROW_CREATE_FUNCTION")
                .AppendLine(") ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, newExteriorId)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, DLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, BRN_CD)
            query.AddParameterWithTypeValue("VISIT_SEQ", OracleDbType.Decimal, VISIT_SEQ)
            query.AddParameterWithTypeValue("BASREZ_ID", OracleDbType.NVarchar2, BASREZ_ID)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, RO_NUM)
            query.AddParameterWithTypeValue("RO_SEQ_NUM", OracleDbType.Decimal, RO_SEQ_NUM)
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, VIN)
            query.AddParameterWithTypeValue("NO_DAMAGE_FLG", OracleDbType.NVarchar2, InitNoDamageFlg)
            query.AddParameterWithTypeValue("CANNOT_CHECK_FLG", OracleDbType.NVarchar2, InitCanNotCheckFlg)
            query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, UserID)
            query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, ProgramId)

            '登録実行
            query.Execute()

        End Using

        Logger.Info("SC3160218TableAdapter.AddExteriorInfo function End.")

        Return newExteriorId
    End Function

    ''' <summary>
    ''' NoDamageフラグの更新
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID"></param>
    ''' <param name="NO_DAMAGE_FLG"></param>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateNoDamage(ByVal RO_EXTERIOR_ID As Decimal,
                                          ByVal NO_DAMAGE_FLG As String,
                                          ByVal UserID As String) As Integer
        Logger.Info("SC3160218TableAdapter.UpdateNoDamage function Begin.")

        Dim result As Integer = -1

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160218_006")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3160218_006 */ ")
                .AppendLine("       TB_T_RO_EXTERIOR SET ")
                .AppendLine("       NO_DAMAGE_FLG       = :NO_DAMAGE_FLG")
                .AppendLine("     , ROW_UPDATE_DATETIME = SYSDATE ")
                .AppendLine("     , ROW_UPDATE_ACCOUNT  = :ROW_UPDATE_ACCOUNT ")
                .AppendLine("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  RO_EXTERIOR_ID = :RO_EXTERIOR_ID ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("NO_DAMAGE_FLG", OracleDbType.NVarchar2, NO_DAMAGE_FLG)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, UserID)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, UpdateFunction)
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            '登録実行
            result = query.Execute()
        End Using
        Logger.Info("SC3160218TableAdapter.UpdateNoDamage function End.")

        Return result
    End Function

    ''' <summary>
    ''' Can'ｔCheckフラグの更新
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID"></param>
    ''' <param name="CANNOT_CHECK_FLG"></param>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateCanNotCheck(ByVal RO_EXTERIOR_ID As Decimal,
                                             ByVal CANNOT_CHECK_FLG As String,
                                             ByVal UserID As String) As Integer
        Logger.Info("SC3160218TableAdapter.UpdateCanNotCheck function Begin.")

        Dim result As Integer = -1

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160218_007")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3160218_007 */ ")
                .AppendLine("       TB_T_RO_EXTERIOR SET ")
                .AppendLine("       CANNOT_CHECK_FLG    = :CANNOT_CHECK_FLG ")
                .AppendLine("     , ROW_UPDATE_DATETIME = SYSDATE ")
                .AppendLine("     , ROW_UPDATE_ACCOUNT  = :ROW_UPDATE_ACCOUNT ")
                .AppendLine("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  RO_EXTERIOR_ID = :RO_EXTERIOR_ID ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("CANNOT_CHECK_FLG", OracleDbType.NVarchar2, CANNOT_CHECK_FLG)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, UserID)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, UpdateFunction)
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            '登録実行
            result = query.Execute()
        End Using
        Logger.Info("SC3160218TableAdapter.UpdateCanNotCheck function End.")

        Return result
    End Function

    ''' <summary>
    ''' Can'ｔCheckフラグの更新
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID"></param>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteDamageInfo(ByVal RO_EXTERIOR_ID As Decimal,
                                            ByVal UserID As String) As Integer
        Logger.Info("SC3160218TableAdapter.DeleteDamageInfo function Begin.")

        Dim result As Integer = -1

        ' サムネイル情報削除
        If Not DeleteThumbnail(RO_EXTERIOR_ID, UserID) Then
            Logger.Info("SC3160218TableAdapter.DeleteDamageInfo thumbnail info delete failed.")
            Logger.Info("SC3160218TableAdapter.DeleteDamageInfo function End.")
            Return result
        End If

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160218_008")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3160218_008 */ ")
                .AppendLine("       TB_T_RO_EXTERIOR_DAMAGE SET")
                .AppendLine("       DAMAGE_TYPE_EXISTS  = :DAMAGE_TYPE_EXISTS")
                .AppendLine("     , DAMAGE_MEMO         = :DAMAGE_MEMO")
                .AppendLine("     , RO_THUMBNAIL_ID     = :RO_THUMBNAIL_ID")
                .AppendLine("     , ROW_UPDATE_DATETIME = SYSDATE")
                .AppendLine("     , ROW_UPDATE_ACCOUNT  = :ROW_UPDATE_ACCOUNT")
                .AppendLine("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  RO_EXTERIOR_ID = :RO_EXTERIOR_ID ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("DAMAGE_TYPE_EXISTS", OracleDbType.NVarchar2, InitDamageTypeExitst)
            query.AddParameterWithTypeValue("DAMAGE_MEMO", OracleDbType.NVarchar2, InitMemo)
            query.AddParameterWithTypeValue("RO_THUMBNAIL_ID", OracleDbType.Decimal, InitThumbnailId)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, UserID)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, UpdateFunction)
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            '登録実行
            result = query.Execute()
        End Using

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160218_008-1")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3160218_008-1 */ ")
                .AppendLine("       TB_T_RO_THUMBNAIL SET")
                .AppendLine("       DISP_FLG  = :DISP_FLG")
                .AppendLine("     , ROW_UPDATE_DATETIME = SYSDATE")
                .AppendLine("     , ROW_UPDATE_ACCOUNT  = :ROW_UPDATE_ACCOUNT")
                .AppendLine("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  RO_EXTERIOR_ID = :RO_EXTERIOR_ID ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("DISP_FLG", OracleDbType.NVarchar2, InitNoDamageFlg)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, UserID)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, UpdateFunction)
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            '登録実行
            result = query.Execute()
        End Using


        Logger.Info("SC3160218TableAdapter.DeleteDamageInfo function End.")

        Return result
    End Function


    ''' <summary>
    ''' 外装IDシーケンス
    ''' </summary>
    ''' <returns>外装IDシーケンスの次番号</returns>
    ''' <remarks></remarks>
    Private Shared Function GetExteriorSeqNextValue() As Decimal
        ' 外装IDシーケンスの次番号
        Dim result As Decimal = 0

        Using query As New DBSelectQuery(Of SC3160218DataSet.ExteriorSequenceDataTable)("SC3160218_009")
            Dim sql As New StringBuilder

            ' SQL文作成
            With sql
                .Append("SELECT /* SC3160218_009 */")
                .Append("       SQ_RO_EXTERIOR.NEXTVAL AS RO_EXTERIOR_ID ")
                .Append("FROM DUAL")
            End With

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' SQLを実行
            Using dt As SC3160218DataSet.ExteriorSequenceDataTable = query.GetData()
                ' レコードが取得できた場合
                If 0 < dt.Count Then
                    ' 外装IDシーケンスの次番号を取得
                    result = dt.Item(0).RO_EXTERIOR_ID
                End If
            End Using
        End Using


        ' 戻り値に外装IDシーケンスの次番号を設定
        Return result

    End Function

    ''' <summary>
    ''' 販売店/店舗コード変換(DMS->iCROP)
    ''' </summary>
    ''' <param name="ORG_DLRCD">基幹販売店コード</param>
    ''' <param name="ORG_STRCD">基幹店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ChangeDlrStrCodeToICROP(ByVal ORG_DLRCD As String,
                                                   ByVal ORG_STRCD As String) As SC3160218DataSet.TB_M_DMS_CODE_MAPDataTable
        Logger.Info("SC3160218TableAdapter.ChangeDlrStrCodeToICROP function Begin.")

        Dim result As SC3160218DataSet.TB_M_DMS_CODE_MAPDataTable

        Using query As New DBSelectQuery(Of SC3160218DataSet.TB_M_DMS_CODE_MAPDataTable)("SC3160218_010")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3160218_010 */ ")
                .Append("        ICROP_CD_1 ")
                .Append("      , ICROP_CD_2 ")
                .Append(" FROM   TB_M_DMS_CODE_MAP ")
                .Append(" WHERE  DLR_CD      = :DLR_CD ")
                .Append(" AND    DMS_CD_1    = :DMS_CD_1 ")
                .Append(" AND    DMS_CD_2    = :DMS_CD_2 ")
                .Append(" AND    DMS_CD_TYPE = :DMS_CD_TYPE ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, ChangeDealerCode)
            query.AddParameterWithTypeValue("DMS_CD_1", OracleDbType.NVarchar2, ORG_DLRCD)
            query.AddParameterWithTypeValue("DMS_CD_2", OracleDbType.NVarchar2, ORG_STRCD)
            query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, ChangeCodeType)

            result = query.GetData()
        End Using

        Logger.Info("SC3160218TableAdapter.ChangeDlrStrCodeToICROP function End.")

        Return result
    End Function

    ''' <summary>
    ''' プログラム設定取得
    ''' </summary>
    ''' <param name="PROGRAM_CD"></param>
    ''' <param name="SETTING_SECTION"></param>
    ''' <param name="SETTING_KEY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetProgramSetting(ByVal PROGRAM_CD As String,
                                             ByVal SETTING_SECTION As String,
                                             ByVal SETTING_KEY As String) As String
        Logger.Info("SC3160218TableAdapter.GetProgramSetting function Begin.")

        Dim result As String = ""
        Dim ds As SC3160218DataSet.TB_M_PROGRAM_SETTINGDataTable

        Using query As New DBSelectQuery(Of SC3160218DataSet.TB_M_PROGRAM_SETTINGDataTable)("SC3160218_011")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3160218_011 */ ")
                .Append("        SETTING_VAL ")
                .Append(" FROM   TB_M_PROGRAM_SETTING ")
                .Append(" WHERE  PROGRAM_CD      = :PROGRAM_CD ")
                .Append(" AND    SETTING_SECTION = :SETTING_SECTION ")
                .Append(" AND    SETTING_KEY     = :SETTING_KEY ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("PROGRAM_CD", OracleDbType.NVarchar2, PROGRAM_CD)
            query.AddParameterWithTypeValue("SETTING_SECTION", OracleDbType.NVarchar2, SETTING_SECTION)
            query.AddParameterWithTypeValue("SETTING_KEY", OracleDbType.NVarchar2, SETTING_KEY)

            ds = query.GetData()
            If 1 = ds.Count Then
                result = ds.Item(0).SETTING_VAL
            End If
        End Using

        Logger.Info("SC3160218TableAdapter.GetProgramSetting function End.")

        Return result
    End Function
#End Region

    ''' <summary>
    ''' サムネイルデータ削除(表示フラグOFF処理)
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID"></param>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function DeleteThumbnail(ByVal RO_EXTERIOR_ID As Decimal,
                                            ByVal UserID As String) As Boolean
        Logger.Info("SC3160218TableAdapter.DeleteThumbnail function Begin.")
        Dim result As Boolean = False

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160218_012")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3160218_012 */ ")
                .AppendLine("       TB_T_RO_THUMBNAIL SET")
                .AppendLine("       DISP_FLG  = :DISP_FLG")
                .AppendLine("     , ROW_UPDATE_DATETIME = SYSDATE ")
                .AppendLine("     , ROW_UPDATE_ACCOUNT  = :ROW_UPDATE_ACCOUNT")
                .AppendLine("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  RO_THUMBNAIL_ID IN ( ")
                .AppendLine("       SELECT RO_THUMBNAIL_ID ")
                .AppendLine("       FROM   TB_T_RO_EXTERIOR_DAMAGE ")
                .AppendLine("       WHERE  RO_EXTERIOR_ID = :RO_EXTERIOR_ID) ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("DISP_FLG", OracleDbType.NVarchar2, DispFlg_Off)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, UserID)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, UpdateFunction)
            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            '登録実行
            result = query.Execute()

            result = True
        End Using


        Logger.Info("SC3160218TableAdapter.DeleteThumbnail function End.")
        Return result
    End Function

End Class
