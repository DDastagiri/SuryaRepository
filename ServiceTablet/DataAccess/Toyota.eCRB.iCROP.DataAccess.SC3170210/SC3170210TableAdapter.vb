'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170210TableAdapter.vb
'─────────────────────────────────────
'機能： RO作成機能グローバル連携処理
'補足： 追加作業サムネイル(追加作業)
'作成： 2013/12/25 SKFC 久代
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' データアクセスクラス
''' </summary>
''' <remarks></remarks>
Public Class SC3170210TableAdapter

#Region "定数"
    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PROGRAM_ID As String = "SC3170210"

    ''' <summary>
    ''' 基幹コード変換販売店コード
    ''' </summary>
    Private Const _DLRCD_XXXXX As String = "XXXXX"

    ''' <summary>
    ''' 基幹コード変換タイプ
    ''' </summary>
    Private Const _DMS_CODE_TYPE As String = "2"

    ''' <summary>
    ''' 表示フラグON
    ''' </summary>
    Private Const _DISP_FLG_ON As String = "1"

    ''' <summary>
    ''' 表示フラグOFF
    ''' </summary>
    Private Const _DISP_FLG_OFF As String = "0"
#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' ROサムネイル画像情報の取得
    ''' </summary>
    ''' <param name="DealerCode">基幹販売店コード</param>
    ''' <param name="BranchCode">基幹店舗コード</param>
    ''' <param name="SAChipId">来店実績連番</param>
    ''' <param name="R_O">RO番号</param>
    ''' <param name="SEQ_NO">RO枝番</param>
    ''' <param name="PictMode">写真モード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRoThumbnail(ByVal DealerCode As String,
                                          ByVal BranchCode As String,
                                          ByVal SAChipId As Long,
                                          ByVal R_O As String,
                                          ByVal SEQ_NO As Long,
                                          ByVal PictMode As String) As SC3170210DataSet.TB_T_RO_THUMBNAILDataTable
        Logger.Info("SC3170210TableAdapter.GetRoThumbnail function Begin.")

        Dim result As SC3170210DataSet.TB_T_RO_THUMBNAILDataTable

        ' SQL文生成
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3170210_001 */ ")
            .Append("       RO_THUMBNAIL_ID, ")
            .Append("       THUMBNAIL_IMG_PATH ")
            .Append("FROM   TB_T_RO_THUMBNAIL ")
            .Append("WHERE  DLR_CD = :DLR_CD ")
            .Append("AND    BRN_CD = :BRN_CD ")
            .Append("AND    DISP_FLG = :DISP_FLG ")

            ' 写真モードが指定されている場合のみ検索条件
            If Not String.IsNullOrEmpty(PictMode) Then
                .Append(" AND PICTURE_GROUP = :PICTUREGROUP ")
            End If

            '値のある項目のみ検索条件
            If 0 <= SAChipId Then
                .Append(" AND VISIT_SEQ = :VISIT_SEQ ")
            ElseIf Not String.IsNullOrEmpty(R_O) Then
                .Append(" AND RO_NUM = :RO_NUM ")
            End If

            ' RO枝番号が0の場合、RO番号のすべてが対象
            If 0 < SEQ_NO Then
                .Append(" AND RO_SEQ_NUM = :RO_SEQ_NUM ")
            End If

            '行作成日付順に降順ソート
            .Append(" ORDER BY CREATE_DATETIME DESC ")
        End With

        ' バインド変数の設定
        Using query As New DBSelectQuery(Of SC3170210DataSet.TB_T_RO_THUMBNAILDataTable)("SC3170210_001")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, DealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, BranchCode)
            query.AddParameterWithTypeValue("DISP_FLG", OracleDbType.NVarchar2, _DISP_FLG_ON)

            ' 写真モードが指定されている場合のみ検索条件
            If Not String.IsNullOrEmpty(PictMode) Then
                query.AddParameterWithTypeValue("PICTUREGROUP", OracleDbType.NVarchar2, PictMode)
            End If

            If 0 <= SAChipId Then
                query.AddParameterWithTypeValue("VISIT_SEQ", OracleDbType.Long, SAChipId)
            ElseIf Not String.IsNullOrEmpty(R_O) Then
                'RO番号
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, R_O)
            End If

            ' RO枝番号が0の場合、RO番号のすべてが対象
            If 0 < SEQ_NO Then
                query.AddParameterWithTypeValue("RO_SEQ_NUM", OracleDbType.Long, SEQ_NO)
            End If

            ' 問合せ実行
            result = query.GetData()
        End Using

        Logger.Info("SC3170210TableAdapter.GetRoThumbnail function End.")

        Return result
    End Function

    ''' <summary>
    ''' 部位文言No取得
    ''' </summary>
    ''' <param name="RO_THUMBNAIL_ID">ROサムネイルID</param>
    ''' <returns>部品文言No.</returns>
    ''' <remarks></remarks>
    Public Shared Function GetPartsTitle(ByVal RO_THUMBNAIL_ID As Decimal) As Decimal
        Logger.Info("SC3170210TableAdapter.GetPartsTitle function Begin.")

        ' SQL文生成
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT     /* SC3170210_005 */ ")
            .Append("           B.PARTS_WORD_NUM AS SETTING_VAL ")
            .Append("FROM       TB_T_RO_EXTERIOR_DAMAGE A ")
            .Append("INNER JOIN TB_M_RO_PARTS_TYPE B ")
            .Append("        ON A.PARTS_TYPE = B.PARTS_TYPE ")
            .Append("WHERE      A.RO_THUMBNAIL_ID = :RO_THUMBNAIL_ID ")
        End With

        Dim result As Decimal = -1
        Dim ds As SC3170210DataSet.TB_M_PROGRAM_SETTINGDataTable

        ' バインド変数の設定
        Using query As New DBSelectQuery(Of SC3170210DataSet.TB_M_PROGRAM_SETTINGDataTable)("SC3170210_005")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("RO_THUMBNAIL_ID", OracleDbType.Decimal, RO_THUMBNAIL_ID)

            ' 問合せ実行
            ds = query.GetData()
            If 1 = ds.Count Then
                If Not Decimal.TryParse(ds.Item(0).SETTING_VAL, result) Then
                    result = -1
                End If
            End If
        End Using

        Logger.Info("SC3170210TableAdapter.GetPartsTitle function End.")
        Return result
    End Function

    ''' <summary>
    ''' ROサムネイル画像情報の削除
    ''' </summary>
    ''' <param name="RO_THUMBNAIL_ID">ROサムネイルID</param>
    ''' <param name="LoginUserID">ログインユーザID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteRoThumbnail(ByVal RO_THUMBNAIL_ID As Decimal,
                                             ByVal LoginUserID As String) As Integer
        Logger.Info("SC3170210TableAdapter.DeleteRoThumbnail function Begin.")
        Dim result As Integer = -1

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3170210_002")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3170210_002 */ ")
                .AppendLine("       TB_T_RO_THUMBNAIL SET ")
                .AppendLine("       DISP_FLG  = :DISP_FLG ")
                .AppendLine("     , ROW_UPDATE_DATETIME = SYSDATE ")
                .AppendLine("     , ROW_UPDATE_ACCOUNT  = :ROW_UPDATE_ACCOUNT ")
                .AppendLine("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  RO_THUMBNAIL_ID     = :RO_THUMBNAIL_ID ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("DISP_FLG", OracleDbType.NVarchar2, _DISP_FLG_OFF)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, LoginUserID)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, _PROGRAM_ID)
            query.AddParameterWithTypeValue("RO_THUMBNAIL_ID", OracleDbType.Decimal, RO_THUMBNAIL_ID)

            '登録実行
            result = query.Execute()
        End Using


        Logger.Info("SC3170210TableAdapter.DeleteRoThumbnail function End.")
        Return result
    End Function

    ''' <summary>
    ''' 販売店/店舗コード変換(DMS->iCROP)
    ''' </summary>
    ''' <param name="orgDlrCD">基幹販売店コード</param>
    ''' <param name="orgStrCd">基幹店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ChangeDlrStrCodeToICROP(ByVal orgDlrCD As String,
                                                   ByVal orgStrCd As String) As SC3170210DataSet.TB_M_DMS_CODE_MAPDataTable
        Logger.Info("SC3170210TableAdapter.ChangeDlrStrCodeToICROP function Begin.")

        Dim result As SC3170210DataSet.TB_M_DMS_CODE_MAPDataTable

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3170210_003 */ ")
            .Append("        ICROP_CD_1 ")
            .Append("      , ICROP_CD_2 ")
            .Append(" FROM   TB_M_DMS_CODE_MAP ")
            .Append(" WHERE  DLR_CD      = :DLRCD ")
            .Append(" AND    DMS_CD_1    = :DMSCD1 ")
            .Append(" AND    DMS_CD_2    = :DMSCD2 ")
            .Append(" AND    DMS_CD_TYPE = :DMS_CD_TYPE ")
        End With

        Using query As New DBSelectQuery(Of SC3170210DataSet.TB_M_DMS_CODE_MAPDataTable)("SC3170210_003")

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, _DLRCD_XXXXX)
            query.AddParameterWithTypeValue("DMSCD1", OracleDbType.NVarchar2, orgDlrCD)
            query.AddParameterWithTypeValue("DMSCD2", OracleDbType.NVarchar2, orgStrCd)
            query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, _DMS_CODE_TYPE)

            result = query.GetData()
        End Using

        Logger.Info("SC3170210TableAdapter.ChangeDlrStrCodeToICROP function End.")

        Return result
    End Function

    ''' <summary>
    ''' プログラム設定取得
    ''' </summary>
    ''' <param name="SETTING_SECTION"></param>
    ''' <param name="SETTING_KEY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetProgramSetting(ByVal SETTING_SECTION As String,
                                             ByVal SETTING_KEY As String) As String
        Logger.Info("SC3170210TableAdapter.GetProgramSetting function Begin.")

        Dim result As String = ""
        Dim ds As SC3170210DataSet.TB_M_PROGRAM_SETTINGDataTable

        Using query As New DBSelectQuery(Of SC3170210DataSet.TB_M_PROGRAM_SETTINGDataTable)("SC3170210_004")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3170210_004 */ ")
                .Append("        SETTING_VAL ")
                .Append(" FROM   TB_M_PROGRAM_SETTING ")
                .Append(" WHERE  PROGRAM_CD      = :PROGRAM_CD ")
                .Append(" AND    SETTING_SECTION = :SETTING_SECTION ")
                .Append(" AND    SETTING_KEY     = :SETTING_KEY ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("PROGRAM_CD", OracleDbType.NVarchar2, _PROGRAM_ID)
            query.AddParameterWithTypeValue("SETTING_SECTION", OracleDbType.NVarchar2, SETTING_SECTION)
            query.AddParameterWithTypeValue("SETTING_KEY", OracleDbType.NVarchar2, SETTING_KEY)

            ds = query.GetData()
            If 1 = ds.Count Then
                result = ds.Item(0).SETTING_VAL
            End If
        End Using

        Logger.Info("SC3170210TableAdapter.GetProgramSetting function End.")

        Return result
    End Function
#End Region

End Class
