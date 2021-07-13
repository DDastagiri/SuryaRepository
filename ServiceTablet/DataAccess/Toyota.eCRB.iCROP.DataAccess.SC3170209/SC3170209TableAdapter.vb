'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3160218TableAdapter.vb
'─────────────────────────────────────
'機能： RO作成機能グローバル連携処理
'補足： 追加作業サムネイル(追加作業)
'作成： 2013/12/30 SKFC 久代 橋本
'更新： 
'─────────────────────────────────────
Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' SC3170209データ
''' </summary>
''' <remarks></remarks>
Public Class SC3170209TableAdapter

#Region "定数"

    ''' <summary>
    ''' プログラムID
    ''' </summary>
    Private Const C_FUNCTION_ID As String = "SC3170209"

    ''' <summary>
    ''' 基幹コード変換販売店コード
    ''' </summary>
    Private Const C_DLRCD_XXXXX As String = "XXXXX"

    ''' <summary>
    ''' 基幹コード変換タイプ
    ''' </summary>
    Private Const C_DMS_CODE_TYPE As String = "2"

    ''' <summary>
    ''' 基幹コード変換タイプ
    ''' </summary>
    Private Const C_DISP_FLG_ON As String = "1"

#End Region

#Region "取得"

    ''' <summary>
    ''' ROサムネイル画像取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="basrezId">基幹予約ID</param>
    ''' <param name="roNum">R/ONO</param>
    ''' <param name="roSeqNum">R/O枝番</param>
    ''' <param name="vinNum">VIN</param>
    ''' <param name="pictureGroup">写真区分</param>
    ''' <returns>処理結果  成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    Public Shared Function getRoThumbnailImgInfo(ByVal dlrCd As String,
                                                 ByVal brnCd As String,
                                                 ByVal visitSeq As Long,
                                                 ByVal basrezId As String,
                                                 ByVal roNum As String,
                                                 ByVal roSeqNum As Long,
                                                 ByVal vinNum As String,
                                                 ByVal pictureGroup As String) As SC3170209DataSet.TB_T_RO_THUMBNAILDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [dlrCd:{1}][brnCd:{2}][visitSeq:{3}][basrezId:{4}][roNum:{5}]" & _
                                  "[roSeqNum:{6}][vinNum:{7}][pictureGroup:{8}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dlrCd, brnCd, visitSeq, basrezId, roNum, roSeqNum, vinNum, pictureGroup))

        Dim response As SC3170209DataSet.TB_T_RO_THUMBNAILDataTable

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3170209_001 */ ")
            .Append("       THUMBNAIL_IMG_PATH ")
            .Append("  FROM (SELECT THUMBNAIL_IMG_PATH ")
            .Append("          FROM TB_T_RO_THUMBNAIL ")
            .Append("         WHERE DLR_CD = :DLRCD ")
            .Append("           AND BRN_CD = :BRNCD ")

            '値のある項目のみ検索条件
            If 0 <= visitSeq Then
                .Append(" AND VISIT_SEQ = :VISITSEQ ")
            ElseIf Not String.IsNullOrEmpty(roNum) Then
                .Append(" AND RO_NUM = :RONUM ")
            End If

            ' 2014/12/19 SKFC山口 検索条件として不要な基幹予約IDをWHERE句から削除 START
            'If Not String.IsNullOrEmpty(basrezId) Then
            '    .Append(" AND BASREZ_ID = :BASREZID ")
            'End If
            ' 2014/12/19 SKFC山口 検索条件として不要な基幹予約IDをWHERE句から削除 END

            If 0 < roSeqNum Then
                .Append(" AND RO_SEQ_NUM = :ROSEQNUM ")
            End If

            If Not String.IsNullOrEmpty(vinNum) Then
                .Append(" AND VIN_NUM = :VINNUM ")
            End If

            If Not String.IsNullOrEmpty(pictureGroup) Then
                .Append(" AND PICTURE_GROUP = :PICTUREGROUP ")
            End If

            .Append(" AND DISP_FLG = '" & C_DISP_FLG_ON & "'")
            '行作成日付順に降順ソート
            .Append(" ORDER BY CREATE_DATETIME DESC ")
            .Append("       )")
            '20件まで
            .Append(" WHERE ROWNUM <= 20")

        End With

        Using query As New DBSelectQuery(Of SC3170209DataSet.TB_T_RO_THUMBNAILDataTable)("SC3170209_001")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCd)
            query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brnCd)

            If 0 <= visitSeq Then
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)
            ElseIf Not String.IsNullOrEmpty(roNum) Then
                'RO番号
                query.AddParameterWithTypeValue("RONUM", OracleDbType.NVarchar2, roNum)
            End If

            ' 2014/12/19 SKFC山口 検索条件として不要な基幹予約IDをWHERE句から削除 START
            '基幹予約ID
            'If Not String.IsNullOrEmpty(basrezId) Then
            '    query.AddParameterWithTypeValue("BASREZID", OracleDbType.NVarchar2, basrezId)
            'End If
            ' 2014/12/19 SKFC山口 検索条件として不要な基幹予約IDをWHERE句から削除 END

            'RO作業連番
            If 0 < roSeqNum Then
                query.AddParameterWithTypeValue("ROSEQNUM", OracleDbType.Long, roSeqNum)
            End If

            'VIN
            If Not String.IsNullOrEmpty(vinNum) Then
                query.AddParameterWithTypeValue("VINNUM", OracleDbType.NVarchar2, vinNum)
            End If

            If Not String.IsNullOrEmpty(pictureGroup) Then
                query.AddParameterWithTypeValue("PICTUREGROUP", OracleDbType.NVarchar2, pictureGroup)
            End If

            response = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [RowCount:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  response.Rows.Count))

        Return response

    End Function


    ''' <summary>
    ''' ROサムネイルID取得
    ''' </summary>
    ''' <returns>シーケンス採番ROサムネイルID</returns>
    ''' <remarks></remarks>
    Public Shared Function getRoThumbnailId() As Decimal

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  MethodBase.GetCurrentMethod.Name))

        Dim resultId As Long = -1L

        Dim sql As New StringBuilder

        ' SQL文作成
        With sql
            .Append("SELECT /* SC3170209_002 */ ")
            .Append("       SQ_RO_THUMBNAIL.NEXTVAL AS RO_THUMBNAIL_ID ")
            .Append("  FROM DUAL")
        End With

        Using query As New DBSelectQuery(Of SC3170209DataSet.ThumbnailSequenceDataTable)("SC3170209_002")

            ' SQL文を設定
            query.CommandText = sql.ToString()
            sql = Nothing

            ' SQLを実行
            Using dt As SC3170209DataSet.ThumbnailSequenceDataTable = query.GetData()

                ' レコードが取得できた場合
                If 0 < dt.Count Then
                    ' シーケンスの次番号を取得
                    resultId = dt.Item(0).RO_THUMBNAIL_ID
                End If

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [RO_THUMBNAIL_ID:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  resultId.ToString(CultureInfo.InvariantCulture)))

        Return resultId

    End Function

#End Region

#Region "登録"

    ''' <summary>
    ''' ROサムネイル画像登録
    ''' </summary>
    ''' <param name="roThumbnailId"></param>
    ''' <param name="dlrCd"></param>
    ''' <param name="brnCd"></param>
    ''' <param name="visitSeq"></param>
    ''' <param name="basrezId"></param>
    ''' <param name="roNum"></param>
    ''' <param name="roSeqNum"></param>
    ''' <param name="vinNum"></param>
    ''' <param name="pictureGroup"></param>
    ''' <param name="thumbnailImgPath"></param>
    ''' <param name="loginUserId"></param>
    ''' <returns>処理結果  成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    Public Shared Function setRoThumbnailImgInfo(ByVal roThumbnailId As Decimal,
                                                 ByVal dlrCd As String,
                                                 ByVal brnCd As String,
                                                 ByVal visitSeq As Long,
                                                 ByVal basrezId As String,
                                                 ByVal roNum As String,
                                                 ByVal roSeqNum As Long,
                                                 ByVal vinNum As String,
                                                 ByVal pictureGroup As String,
                                                 ByVal thumbnailImgPath As String,
                                                 ByVal loginUserId As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [roThumbnailId:{1}][dlrCd:{2}][brnCd:{3}][visitSeq:{4}][basrezId:{5}][roNum:{6}]" & _
                                  "[roSeqNum:{7}][vinNum:{8}][pictureGroup:{9}][loginUserId:{10}][thumbnailImgPath:{11}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roThumbnailId, dlrCd, brnCd, visitSeq, basrezId, roNum,
                                  roSeqNum, vinNum, pictureGroup, loginUserId, thumbnailImgPath))

        Dim result As Boolean = False

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .AppendLine("INSERT /* SC3170209_003 */ ")
            .AppendLine("  INTO TB_T_RO_THUMBNAIL ")
            .AppendLine("( ")
            .AppendLine("       RO_THUMBNAIL_ID")
            .AppendLine("     , DLR_CD")
            .AppendLine("     , BRN_CD")

            '値のある項目のみ登録（値無しは初期値）
            If 0 <= visitSeq Then .AppendLine("     , VISIT_SEQ")
            If Not String.IsNullOrEmpty(basrezId) Then .AppendLine("     , BASREZ_ID")
            If Not String.IsNullOrEmpty(roNum) Then .AppendLine("     , RO_NUM")
            If 0 <= roSeqNum Then .AppendLine("     , RO_SEQ_NUM")
            If Not String.IsNullOrEmpty(vinNum) Then .AppendLine("     , VIN_NUM")
            If Not String.IsNullOrEmpty(pictureGroup) Then .AppendLine("     , PICTURE_GROUP")

            .AppendLine("     , THUMBNAIL_IMG_PATH")
            .AppendLine("     , CREATE_DATETIME")
            .AppendLine("     , ROW_CREATE_DATETIME")
            .AppendLine("     , ROW_CREATE_ACCOUNT")
            .AppendLine("     , ROW_CREATE_FUNCTION")
            .AppendLine(") ")
            .AppendLine("VALUES ")
            .AppendLine("( ")
            .AppendLine("       :ROTHUMBNAILID")
            .AppendLine("     , :DLRCD")
            .AppendLine("     , :BRNCD")

            If 0 <= visitSeq Then .AppendLine("     , :VISITSEQ")
            If Not String.IsNullOrEmpty(basrezId) Then .AppendLine("     , :BASREZID")
            If Not String.IsNullOrEmpty(roNum) Then .AppendLine("     , :RONUM")
            If 0 <= roSeqNum Then .AppendLine("     , :ROSEQNUM")
            If Not String.IsNullOrEmpty(vinNum) Then .AppendLine("     , :VINNUM")
            If Not String.IsNullOrEmpty(pictureGroup) Then .AppendLine("     , :PICTUREGROUP")

            .AppendLine("     , :THUMBNAILIMGPATH")
            .AppendLine("     , SYSDATE")
            .AppendLine("     , SYSDATE")
            .AppendLine("     , :ROWCREATEACCOUNT")
            .AppendLine("     , :ROWCREATEFUNCTION")
            .AppendLine(") ")
        End With

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3170209_003")



            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("ROTHUMBNAILID", OracleDbType.Decimal, roThumbnailId)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCd)
            query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brnCd)
            If 0 <= visitSeq Then
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)
            End If
            If Not String.IsNullOrEmpty(basrezId) Then
                query.AddParameterWithTypeValue("BASREZID", OracleDbType.NVarchar2, basrezId)
            End If
            If Not String.IsNullOrEmpty(roNum) Then
                query.AddParameterWithTypeValue("RONUM", OracleDbType.NVarchar2, roNum)
            End If
            If 0 <= roSeqNum Then
                query.AddParameterWithTypeValue("ROSEQNUM", OracleDbType.Long, roSeqNum)
            End If
            If Not String.IsNullOrEmpty(vinNum) Then
                query.AddParameterWithTypeValue("VINNUM", OracleDbType.NVarchar2, vinNum)
            End If
            If Not String.IsNullOrEmpty(pictureGroup) Then
                query.AddParameterWithTypeValue("PICTUREGROUP", OracleDbType.NVarchar2, pictureGroup)
            End If
            query.AddParameterWithTypeValue("THUMBNAILIMGPATH", OracleDbType.NVarchar2, thumbnailImgPath)
            query.AddParameterWithTypeValue("ROWCREATEACCOUNT", OracleDbType.NVarchar2, loginUserId)
            query.AddParameterWithTypeValue("ROWCREATEFUNCTION", OracleDbType.NVarchar2, C_FUNCTION_ID)

            '登録実行
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                result = True
            Else
                result = False
            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [IsSuccess:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  result))

        Return result

    End Function

#End Region

#Region "DMSコード変換"

    ''' <summary>
    ''' 販売店/店舗コード変換(DMS->iCROP)
    ''' </summary>
    ''' <param name="orgDlrCD">基幹販売店コード</param>
    ''' <param name="orgStrCd">基幹店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ChangeDlrStrCodeToICROP(ByVal orgDlrCD As String,
                                            ByVal orgStrCd As String) As SC3170209DataSet.TB_M_DMS_CODE_MAPDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [ORG_DLRCD:{1}][ORG_STRCD:{2}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  orgDlrCD, orgStrCd))

        Dim result As SC3170209DataSet.TB_M_DMS_CODE_MAPDataTable

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3170209_004 */ ")
            .Append("        ICROP_CD_1 ")
            .Append("      , ICROP_CD_2 ")
            .Append(" FROM   TB_M_DMS_CODE_MAP ")
            .Append(" WHERE  DLR_CD      = :DLRCD ")
            .Append(" AND    DMS_CD_1    = :DMSCD1 ")
            .Append(" AND    DMS_CD_2    = :DMSCD2 ")
            .Append(" AND    DMS_CD_TYPE = '" & C_DMS_CODE_TYPE & "'")
        End With

        Using query As New DBSelectQuery(Of SC3170209DataSet.TB_M_DMS_CODE_MAPDataTable)("SC3170209_004")

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, C_DLRCD_XXXXX)
            query.AddParameterWithTypeValue("DMSCD1", OracleDbType.NVarchar2, orgDlrCD)
            query.AddParameterWithTypeValue("DMSCD2", OracleDbType.NVarchar2, orgStrCd)

            result = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [rowCount:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  result.Rows.Count))

        Return result

    End Function

#End Region

#Region "TB_M_PROGRAM_SETTING取得"

    ''' <summary>
    ''' プログラムセッティング値郡取得
    ''' </summary>
    ''' <param name="programCd">画面ID</param>
    ''' <param name="settingSection">セクション（省略可）</param>
    ''' <param name="settingKey">キー（省略時可）</param>
    ''' <returns>セッティング値データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetProgramSettingValues(ByVal programCd As String,
                                            Optional ByVal settingSection As String = "",
                                            Optional ByVal settingKey As String = "") As DataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [programCd:{1}][settingSection:{2}][settingKey:{3}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  programCd, settingSection, settingKey))

        ' SQL組み立て
        Dim sql As New StringBuilder

        With sql
            .Append("SELECT /* SC3170209_005 */ ")
            .Append("       SETTING_KEY ")
            .Append("     , SETTING_VAL ")
            .Append("  FROM TB_M_PROGRAM_SETTING ")
            .Append(" WHERE PROGRAM_CD = :PROGRAMCD ")
            '以下省略可
            If Not String.IsNullOrEmpty(settingSection) Then .Append("   AND SETTING_SECTION = :SETTINGSECTION ")
            If Not String.IsNullOrEmpty(settingKey) Then .Append("   AND SETTING_KEY = :SETTINGKEY ")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of DataTable)("SC3170209_005")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("PROGRAMCD", OracleDbType.NVarchar2, programCd)
            If Not settingSection.Equals(String.Empty) Then query.AddParameterWithTypeValue("SETTINGSECTION", OracleDbType.NVarchar2, settingSection)
            If Not settingKey.Equals(String.Empty) Then query.AddParameterWithTypeValue("SETTINGKEY", OracleDbType.NVarchar2, settingKey)

            ' SQL実行
            Dim dt As DataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [rowCount:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dt.Rows.Count))

            ' 結果を返却
            Return dt

        End Using
    End Function


    ''' <summary>
    ''' プログラムセッティング値取得
    ''' </summary>
    ''' <param name="programCd">画面ID</param>
    ''' <param name="settingSection">セクション（省略可）</param>
    ''' <param name="settingKey">キー（省略時可）</param>
    ''' <returns>セッティング値</returns>
    ''' <remarks></remarks>
    Public Function GetProgramSettingValue(ByVal programCd As String,
                                            Optional ByVal settingSection As String = "",
                                            Optional ByVal settingKey As String = "") As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [programCd:{1}][settingSection:{2}][settingKey:{3}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  programCd, settingSection, settingKey))

        Dim result As String = String.Empty

        Dim dt As DataTable = Me.GetProgramSettingValues(programCd, settingSection, settingKey)

        If Not IsDBNull(dt) AndAlso dt.Rows.Count > 0 Then
            result = dt.Rows(0).Item("SETTING_VAL").ToString.Trim()

        Else
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      "{0} TB_M_PROGRAM_SETTING value is null or empty",
                                      MethodBase.GetCurrentMethod.Name,
                                      result))

            Throw New ApplicationException

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [result:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  result))

        Return result

    End Function

#End Region

#Region "TB_M_SYSTEM_SETTING取得"

    ''' <summary>
    ''' システム設定値郡取得
    ''' </summary>
    ''' <param name="settingName">設定名</param>
    ''' <returns>セッティング値データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetSystemSettingValues(ByVal settingName As String) As DataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [settingName:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  settingName))

        ' SQL組み立て
        Dim sql As New StringBuilder

        With sql
            .Append("SELECT /* SC3170209_006 */ ")
            .Append("       SETTING_NAME ")
            .Append("     , SETTING_VAL ")
            .Append("  FROM TB_M_SYSTEM_SETTING ")
            .Append(" WHERE SETTING_NAME = :SETTINGNAME ")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of DataTable)("SC3170209_006")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("SETTINGNAME", OracleDbType.NVarchar2, settingName)

            ' SQL実行
            Dim dt As DataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [rowCount:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dt.Rows.Count))

            ' 結果を返却
            Return dt

        End Using
    End Function


    ''' <summary>
    ''' システム設定値取得
    ''' </summary>
    ''' <param name="settingName">設定名</param>
    ''' <returns>セッティング値</returns>
    ''' <remarks></remarks>
    Public Function GetSystemSettingValue(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [settingName:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  settingName))

        Dim result As String = String.Empty

        Dim dt As DataTable = Me.GetSystemSettingValues(settingName)

        If Not IsDBNull(dt) AndAlso dt.Rows.Count > 0 Then
            result = dt.Rows(0).Item("SETTING_VAL").ToString.Trim()

        Else
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      "{0} TB_M_SYSTEM_SETTING value is null or empty",
                                      MethodBase.GetCurrentMethod.Name,
                                      result))

            Throw New ApplicationException

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [result:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  result))

        Return result

    End Function

#End Region

End Class
