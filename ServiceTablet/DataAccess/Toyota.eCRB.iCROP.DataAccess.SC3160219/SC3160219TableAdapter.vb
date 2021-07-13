'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3160219TableAdapter.vb
'─────────────────────────────────────
'機能： RO損傷登録画面
'補足： 
'作成： 2013/11/19 SKFC 橋本
'更新： 2018/03/29 SKFC 横田　REQ-SVT-TMT-20170809-001　損傷写真複数対応
'──────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' RO損傷登録画面のテーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class SC3160219TableAdapter

#Region "定数"

    ''' <summary>
    ''' 機能ID：RO損傷登録画面
    ''' </summary>
    Private Const C_FUNCTION_ID As String = "SC3160219"

    ''' <summary>
    ''' 写真区分：外観チェック
    ''' </summary>
    Private Const C_PICTURE_GROUP_EXTERIORCHECK As String = "2"

    ''' <summary>
    ''' ダメージ有無：ノーダメージ
    ''' </summary>
    Private Const C_DBVAL_DAMAGEEXISTS_NODAMAGE As String = "-"

    ''' <summary>
    ''' DBのCHAR型の空文字
    ''' </summary>
    Private Const C_DBVAL_CHAR_EMPTY As String = " "

#End Region

#Region "メソッド"

#Region "取得クエリ"

    ''' <summary>
    ''' RO外装ダメージマスタ情報取得
    ''' </summary>
    ''' <param name="partsType">部位種別</param>
    ''' <returns>RO外装ダメージマスタ情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRoExteriorDamageMaster(ByVal partsType As String) As SC3160219DataSet.RoExteriorDamageMasterDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [PARTS_TYPE:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  partsType))

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3160219_001 */ ")
            .Append("       PARTS_WORD_NUM ")
            .Append("     , DAMAGE_TYPE ")
            .Append("     , DAMAGE_WORD_NUM ")
            .Append("     , GRADATION_FROM ")
            .Append("     , GRADATION_TO ")
            .Append("  FROM (SELECT MPT.PARTS_WORD_NUM ")
            .Append("             , MDT.DAMAGE_TYPE ")
            .Append("             , MDT.DAMAGE_WORD_NUM ")
            .Append("             , MDT.GRADATION_FROM ")
            .Append("             , MDT.GRADATION_TO ")
            .Append("             , 1 AS ORDER_NUM ")
            .Append("          FROM TB_M_RO_PARTS_TYPE MPT ")
            .Append("          LEFT JOIN TB_M_RO_DAMAGE_TYPE MDT ")
            .Append("            ON SUBSTR(MPT.DAMAGE_TYPE_LINE,1,1) = MDT.DAMAGE_TYPE ")
            .Append("         WHERE MPT.PARTS_TYPE = :PARTSTYPE ")
            .Append("        UNION ")
            .Append("        SELECT MPT.PARTS_WORD_NUM ")
            .Append("             , MDT.DAMAGE_TYPE ")
            .Append("             , MDT.DAMAGE_WORD_NUM ")
            .Append("             , MDT.GRADATION_FROM ")
            .Append("             , MDT.GRADATION_TO ")
            .Append("             , 2 AS ORDER_NUM ")
            .Append("         FROM TB_M_RO_PARTS_TYPE MPT ")
            .Append("          LEFT JOIN TB_M_RO_DAMAGE_TYPE MDT ")
            .Append("            ON SUBSTR(MPT.DAMAGE_TYPE_LINE,2,1) = MDT.DAMAGE_TYPE ")
            .Append("         WHERE MPT.PARTS_TYPE = :PARTSTYPE ")
            .Append("        UNION ")
            .Append("        SELECT MPT.PARTS_WORD_NUM ")
            .Append("             , MDT.DAMAGE_TYPE ")
            .Append("             , MDT.DAMAGE_WORD_NUM ")
            .Append("             , MDT.GRADATION_FROM ")
            .Append("             , MDT.GRADATION_TO ")
            .Append("             , 3 AS ORDER_NUM ")
            .Append("          FROM TB_M_RO_PARTS_TYPE MPT ")
            .Append("          LEFT JOIN TB_M_RO_DAMAGE_TYPE MDT ")
            .Append("            ON SUBSTR(MPT.DAMAGE_TYPE_LINE,3,1) = MDT.DAMAGE_TYPE ")
            .Append("         WHERE MPT.PARTS_TYPE = :PARTSTYPE ")
            .Append("        UNION ")
            .Append("        SELECT MPT.PARTS_WORD_NUM ")
            .Append("             , MDT.DAMAGE_TYPE ")
            .Append("             , MDT.DAMAGE_WORD_NUM ")
            .Append("             , MDT.GRADATION_FROM ")
            .Append("             , MDT.GRADATION_TO ")
            .Append("             , 4 AS ORDER_NUM ")
            .Append("          FROM TB_M_RO_PARTS_TYPE MPT ")
            .Append("          LEFT JOIN TB_M_RO_DAMAGE_TYPE MDT ")
            .Append("            ON SUBSTR(MPT.DAMAGE_TYPE_LINE,4,1) = MDT.DAMAGE_TYPE ")
            .Append("         WHERE MPT.PARTS_TYPE = :PARTSTYPE ")
            .Append("        UNION ")
            .Append("        SELECT MPT.PARTS_WORD_NUM ")
            .Append("             , MDT.DAMAGE_TYPE ")
            .Append("             , MDT.DAMAGE_WORD_NUM ")
            .Append("             , MDT.GRADATION_FROM ")
            .Append("             , MDT.GRADATION_TO ")
            .Append("             , 5 AS ORDER_NUM ")
            .Append("          FROM TB_M_RO_PARTS_TYPE MPT ")
            .Append("          LEFT JOIN TB_M_RO_DAMAGE_TYPE MDT ")
            .Append("            ON SUBSTR(MPT.DAMAGE_TYPE_LINE,5,1) = MDT.DAMAGE_TYPE ")
            .Append("         WHERE MPT.PARTS_TYPE = :PARTSTYPE ")
            .Append("        UNION ")
            .Append("        SELECT MPT.PARTS_WORD_NUM ")
            .Append("             , N'-' DAMAGE_TYPE ")
            .Append("             , N' ' DAMAGE_WORD_NUM ")
            .Append("             , N' ' GRADATION_FROM ")
            .Append("             , N' ' GRADATION_TO ")
            .Append("             , 6 AS ORDER_NUM ")
            .Append("          FROM TB_M_RO_PARTS_TYPE MPT ")
            .Append("         WHERE MPT.PARTS_TYPE = :PARTSTYPE ")
            .Append("           AND MPT.DAMAGE_TYPE_LINE = '-' ")
            .Append("       ) ")
            .Append(" WHERE DAMAGE_TYPE IS NOT NULL ")
            .Append(" ORDER BY ORDER_NUM")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3160219DataSet.RoExteriorDamageMasterDataTable)("SC3160219_001")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("PARTSTYPE", OracleDbType.NVarchar2, partsType)

            ' SQLを実行
            Dim dt As SC3160219DataSet.RoExteriorDamageMasterDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [RowCount:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dt.Rows.Count))

            ' 結果を返却
            Return dt
        End Using

    End Function


    ''' <summary>
    ''' RO外装ダメージ情報取得
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <returns>RO外装ダメージ情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRoExteriorDamageInfo(ByVal roExteriorId As Decimal, ByVal partsType As String) As SC3160219DataSet.RoExteriorDamageInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [RO_EXTERIOR_ID:{1}][PARTS_TYPE:{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  roExteriorId.ToString(CultureInfo.InvariantCulture), partsType))

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3160219_002 */ ")
            .Append("       TED.DAMAGE_TYPE_EXISTS ")
            .Append("     , TED.DAMAGE_MEMO ")
            '.Append("     , TED.RO_THUMBNAIL_ID ")
            .Append("     , TTN.RO_THUMBNAIL_ID ")
            .Append("     , TTN.THUMBNAIL_IMG_PATH ")
            .Append("  FROM TB_T_RO_EXTERIOR_DAMAGE TED ")
            .Append("  LEFT OUTER JOIN TB_T_RO_THUMBNAIL TTN")
            '            .Append("    ON TED.RO_THUMBNAIL_ID = TTN.RO_THUMBNAIL_ID ")
            .Append("    ON TED.RO_EXTERIOR_ID = TTN.RO_EXTERIOR_ID ")
            .Append("    AND TED.PARTS_TYPE = TTN.PARTS_TYPE ")
            .Append("   AND TTN.DISP_FLG = '1' ")
            .Append(" WHERE TED.RO_EXTERIOR_ID = :ROEXTERIORID ")
            .Append("   AND TED.PARTS_TYPE = :PARTSTYPE ")
        End With

        'With sql
        '    .Append("SELECT /* SC3160219_002 */ ")
        '    .Append("       DAMAGE_TYPE_EXISTS ")
        '    .Append("     , DAMAGE_MEMO ")
        '    .Append("     , RO_THUMBNAIL_ID ")
        '    .Append("     , THUMBNAIL_IMG_PATH ")
        '    .Append("  FROM TB_T_RO_EXTERIOR_DAMAGE  ")
        '    .Append(" WHERE RO_EXTERIOR_ID = :ROEXTERIORID ")
        '    .Append("   AND PARTS_TYPE = :PARTSTYPE ")
        'End With




        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3160219DataSet.RoExteriorDamageInfoDataTable)("SC3160219_002")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("ROEXTERIORID", OracleDbType.Decimal, roExteriorId)
            query.AddParameterWithTypeValue("PARTSTYPE", OracleDbType.NVarchar2, partsType)

            ' SQLを実行
            Dim dt As SC3160219DataSet.RoExteriorDamageInfoDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [RowCount:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dt.Rows.Count))

            ' 結果を返却
            Return dt
        End Using

    End Function

    ''' <summary>
    ''' RO外装ダメージ情報取得
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <returns>RO外装ダメージ情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRothumbnailInfo(ByVal roExteriorId As Decimal, ByVal partsType As String) As SC3160219DataSet.RoExteriorDamageInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [RO_EXTERIOR_ID:{1}][PARTS_TYPE:{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  roExteriorId.ToString(CultureInfo.InvariantCulture), partsType))

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3160219_099 */ ")
            .Append("     THUMBNAIL_IMG_PATH ")
            .Append("     ,RO_THUMBNAIL_ID ")
            .Append("  FROM TB_T_RO_THUMBNAIL ")
            .Append(" WHERE RO_EXTERIOR_ID = :ROEXTERIORID ")
            .Append("   AND PARTS_TYPE = :PARTSTYPE ")
            .Append("   AND DISP_FLG = '1' ")

        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3160219DataSet.RoExteriorDamageInfoDataTable)("SC3160219_002")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("ROEXTERIORID", OracleDbType.Decimal, roExteriorId)
            query.AddParameterWithTypeValue("PARTSTYPE", OracleDbType.NVarchar2, partsType)

            ' SQLを実行
            Dim dt As SC3160219DataSet.RoExteriorDamageInfoDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [RowCount:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dt.Rows.Count))

            ' 結果を返却
            Return dt
        End Using

    End Function




    ''' <summary>
    ''' ROサムネイルID取得
    ''' </summary>
    ''' <returns>ROサムネイルID</returns>
    ''' <remarks>取得不可の場合は-1を返します。</remarks>
    Public Function GetRoThumbnailId() As Decimal

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  MethodBase.GetCurrentMethod.Name))

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3160219_003 */ ")
            .Append("       SQ_RO_THUMBNAIL.NEXTVAL AS RO_THUMBNAIL_ID ")
            .Append("FROM DUAL ")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3160219DataSet.RoThumbnailIdDataTable)("SC3160219_003")

            query.CommandText = sql.ToString()

            ' SQL実行
            Dim dt As SC3160219DataSet.RoThumbnailIdDataTable = query.GetData()

            ' ROサムネイルID
            Dim roThumbnailId As Decimal = -1

            ' データが取得時のみ、ROサムネイルIDを取得
            If dt.Rows.Count > 0 Then
                roThumbnailId = Convert.ToDecimal(dt.Item(0).RO_THUMBNAIL_ID)
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [RO_THUMBNAIL_ID:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      roThumbnailId.ToString(CultureInfo.InvariantCulture)))

            ' 結果を返却
            Return roThumbnailId

        End Using
    End Function

    ''' <summary>
    ''' システム設定値取得
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
            .Append("SELECT /* SC3160219_011 */ ")
            .Append("       SETTING_NAME ")
            .Append("     , SETTING_VAL ")
            .Append("  FROM TB_M_SYSTEM_SETTING ")
            .Append(" WHERE SETTING_NAME = :SETTINGNAME ")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of DataTable)("SC3160219_011")

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
    ''' プログラムセッティング値取得
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
            .Append("SELECT /* SC3160219_009 */ ")
            .Append("       SETTING_KEY ")
            .Append("     , SETTING_VAL ")
            .Append("  FROM TB_M_PROGRAM_SETTING ")
            .Append(" WHERE PROGRAM_CD = :PROGRAMCD ")
            '以下省略可
            If Not settingSection.Equals(String.Empty) Then .Append("   AND SETTING_SECTION = :SETTINGSECTION ")
            If Not settingKey.Equals(String.Empty) Then .Append("   AND SETTING_KEY = :SETTINGKEY ")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of DataTable)("SC3160219_009")

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
    ''' 販売店コード取得
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID">シーケンスID</param>
    ''' <returns>販売店コード</returns>
    ''' <remarks></remarks>
    Public Function GetDealerCode(ByVal RO_EXTERIOR_ID As Decimal) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [RO_EXTERIOR_ID:{1}]",
                                  MethodBase.GetCurrentMethod.Name, RO_EXTERIOR_ID))
        Dim dealerCode As String = ""

        ' SQL組み立て
        Dim sql As New StringBuilder

        With sql
            .Append("SELECT /* SC3160219_010 */ ")
            .Append("       DLR_CD ")
            .Append("  FROM TB_T_RO_EXTERIOR ")
            .Append(" WHERE RO_EXTERIOR_ID = :RO_EXTERIOR_ID ")
        End With

        Dim result As Decimal = -1
        Dim ds As DataTable


        ' バインド変数の設定
        Using query As New DBSelectQuery(Of DataTable)("SC3160219_010")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("RO_EXTERIOR_ID", OracleDbType.Decimal, RO_EXTERIOR_ID)

            ' 問合せ実行
            ds = query.GetData()
            If 1 = ds.Rows.Count Then
                dealerCode = ds.Rows(0).Item("DLR_CD").ToString()
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} End [dealerCode:{1}]",
                          MethodBase.GetCurrentMethod.Name,
                          dealerCode))
        Return dealerCode
    End Function

#End Region

#Region "登録クエリ"

    ''' <summary>
    ''' ROサムネイル画像登録
    ''' </summary>
    ''' <param name="roThumbnailId">ROサムネイルID</param>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="thumbnailImgPath">サムネイル画像ファイルパス</param>
    ''' <param name="account">アカウントID</param>
    ''' <returns>処理結果  成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    Public Function SetROThumbnailImgPath(ByVal roThumbnailId As Decimal, ByVal roExteriorId As Decimal, ByVal thumbnailImgPath As String,
                                          ByVal account As String, ByVal partsType As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [RO_THUMBNAIL_ID:{1}][RO_EXTERIOR_ID:{2}][THUMBNAIL_IMG_PATH:{3}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  roThumbnailId.ToString(CultureInfo.InvariantCulture), roExteriorId.ToString(CultureInfo.InvariantCulture),
                                  thumbnailImgPath))

        '値なしは初期値に変更
        If String.IsNullOrEmpty(thumbnailImgPath) Then
            thumbnailImgPath = C_DBVAL_CHAR_EMPTY
        End If
        If String.IsNullOrEmpty(account) Then
            account = C_DBVAL_CHAR_EMPTY
        End If


        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("INSERT /* SC3160219_004 */ ")
            .Append("  INTO TB_T_RO_THUMBNAIL ")
            .Append("(")
            .Append(" RO_THUMBNAIL_ID")
            .Append(",DLR_CD")
            .Append(",BRN_CD")
            .Append(",VISIT_SEQ")
            .Append(",BASREZ_ID")
            .Append(",RO_NUM")
            .Append(",RO_SEQ_NUM")
            .Append(",VIN_NUM")
            .Append(",PICTURE_GROUP")
            .Append(",THUMBNAIL_IMG_PATH")
            .Append(",ROW_CREATE_DATETIME")
            .Append(",ROW_CREATE_ACCOUNT")
            .Append(",ROW_CREATE_FUNCTION")
            .Append(",ROW_UPDATE_DATETIME")
            .Append(",ROW_UPDATE_ACCOUNT")
            .Append(",ROW_UPDATE_FUNCTION")
            .Append(",ROW_LOCK_VERSION")
            .Append(",RO_EXTERIOR_ID")
            .Append(",PARTS_TYPE")
            .Append(")")
            .Append("SELECT :ROTHUMBNAILID ")
            .Append("     , DLR_CD ")
            .Append("     , BRN_CD ")
            .Append("     , VISIT_SEQ ")
            .Append("     , BASREZ_ID ")
            .Append("     , RO_NUM ")
            .Append("     , RO_SEQ_NUM ")
            .Append("     , VIN_NUM ")
            .Append("     , '" & C_PICTURE_GROUP_EXTERIORCHECK & "' ")
            .Append("     , :THUMBNAILIMGPATH ")
            .Append("     , SYSDATE ")
            .Append("     , :CREATEACCOUNT ")
            .Append("     , :CREATEFUNCTION ")
            .Append("     , SYSDATE ")
            .Append("     , :UPDATEACCOUNT ")
            .Append("     , :UPDATEFUNCTION ")
            .Append("     , 1 ")
            .Append("     , :ROEXTERIORID ")
            .Append("     , :PARTSTYPE ")
            .Append("  FROM TB_T_RO_EXTERIOR ")
            .Append(" WHERE RO_EXTERIOR_ID = :ROEXTERIORID ")



        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160219_004")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("ROTHUMBNAILID", OracleDbType.Decimal, roThumbnailId)
            query.AddParameterWithTypeValue("THUMBNAILIMGPATH", OracleDbType.NVarchar2, thumbnailImgPath)
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("CREATEFUNCTION", OracleDbType.NVarchar2, C_FUNCTION_ID)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("UPDATEFUNCTION", OracleDbType.NVarchar2, C_FUNCTION_ID)
            query.AddParameterWithTypeValue("ROEXTERIORID", OracleDbType.Decimal, roExteriorId)
            query.AddParameterWithTypeValue("PARTSTYPE", OracleDbType.NVarchar2, partsType)

            Dim ret As Boolean

            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                ret = True
            Else
                ret = False
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [IsSuccess:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      ret))

            Return ret

        End Using
    End Function


    ''' <summary>
    ''' RO外装ダメージ情報登録
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <param name="damageTypeExists">ダメージ有無</param>
    ''' <param name="memo">メモ</param>
    ''' <param name="roThumbnailId">ROサムネイルID</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>処理結果  成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    Public Function SetROExteriorDamageInfo(ByVal roExteriorId As Decimal,
                                            ByVal partsType As String,
                                            ByVal damageTypeExists As String,
                                            ByVal memo As String,
                                            ByVal roThumbnailId As Decimal,
                                            ByVal account As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [RO_EXTERIOR_ID:{1}][PARTS_TYPE:{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  roExteriorId.ToString(CultureInfo.InvariantCulture), partsType))

        '値なしは初期値に変更
        If String.IsNullOrEmpty(damageTypeExists) Then
            damageTypeExists = C_DBVAL_DAMAGEEXISTS_NODAMAGE
        End If
        If String.IsNullOrEmpty(memo) Then
            memo = C_DBVAL_CHAR_EMPTY
        End If

        '戻り値
        Dim ret As Boolean = False

        'RO外装ダメージ情報の存在有無
        Dim isExistsRow As Boolean

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3160219_005 */ ")
            .Append("       1 AS COUNT ")
            .Append("  FROM TB_T_RO_EXTERIOR_DAMAGE ")
            .Append(" WHERE RO_EXTERIOR_ID = :ROEXTERIORID ")
            .Append("   AND PARTS_TYPE = :PARTSTYPE ")
        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3160219DataSet.RoExteriorDamageCountDataTable)("SC3160219_005")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("ROEXTERIORID", OracleDbType.Decimal, roExteriorId)
            query.AddParameterWithTypeValue("PARTSTYPE", OracleDbType.NVarchar2, partsType)

            ' SQL実行
            Dim dt As SC3160219DataSet.RoExteriorDamageCountDataTable = query.GetData()

            ' データの存在判定
            If dt.Rows.Count > 0 Then
                isExistsRow = True
            Else
                isExistsRow = False
            End If

        End Using

        'INSERTかUPDATEか分岐
        If isExistsRow Then

            ' UPDATESQL組み立て
            Dim sqlUpdate As New StringBuilder
            With sqlUpdate
                .Append("UPDATE /* SC3160219_006 */ ")
                .Append("       TB_T_RO_EXTERIOR_DAMAGE ")
                .Append("   SET DAMAGE_TYPE_EXISTS = :DAMAGETYPEEXISTS ")
                .Append("     , DAMAGE_MEMO = :DAMAGEMEMO ")
                .Append("     , RO_THUMBNAIL_ID = :ROTHUMBNAILID ")
                .Append("     , ROW_UPDATE_DATETIME = SYSDATE ")
                .Append("     , ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT ")
                .Append("     , ROW_UPDATE_FUNCTION = :UPDATEFUNCTION ")
                .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                .Append(" WHERE RO_EXTERIOR_ID = :ROEXTERIORID ")
                .Append("   AND PARTS_TYPE = :PARTSTYPE ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3160219_006")

                query.CommandText = sqlUpdate.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ROEXTERIORID", OracleDbType.Decimal, roExteriorId)
                query.AddParameterWithTypeValue("PARTSTYPE", OracleDbType.NVarchar2, partsType)
                query.AddParameterWithTypeValue("DAMAGETYPEEXISTS", OracleDbType.NVarchar2, damageTypeExists)
                query.AddParameterWithTypeValue("DAMAGEMEMO", OracleDbType.NVarchar2, memo)
                query.AddParameterWithTypeValue("ROTHUMBNAILID", OracleDbType.Decimal, roThumbnailId)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("UPDATEFUNCTION", OracleDbType.NVarchar2, C_FUNCTION_ID)

                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    ret = True
                Else
                    ret = False
                End If

            End Using

        Else
            ' INSERTSQL組み立て
            Dim sqlInsert As New StringBuilder
            With sqlInsert
                .Append("INSERT /* SC3160219_007 */ ")
                .Append("  INTO TB_T_RO_EXTERIOR_DAMAGE ")
                .Append("(")
                .Append(" RO_EXTERIOR_ID")
                .Append(",PARTS_TYPE")
                .Append(",DAMAGE_TYPE_EXISTS")
                .Append(",DAMAGE_MEMO")
                .Append(",RO_THUMBNAIL_ID")
                .Append(",ROW_CREATE_DATETIME")
                .Append(",ROW_CREATE_ACCOUNT")
                .Append(",ROW_CREATE_FUNCTION")
                .Append(",ROW_UPDATE_DATETIME")
                .Append(",ROW_UPDATE_ACCOUNT")
                .Append(",ROW_UPDATE_FUNCTION")
                .Append(",ROW_LOCK_VERSION")
                .Append(")")
                .Append("VALUES")
                .Append("(")
                .Append(" :ROEXTERIORID ")
                .Append(",:PARTSTYPE ")
                .Append(",:DAMAGETYPEEXISTS ")
                .Append(",:DAMAGEMEMO ")
                .Append(",:ROTHUMBNAILID ")
                .Append(",SYSDATE ")
                .Append(",:CREATEACCOUNT ")
                .Append(",:CREATEFUNCTION ")
                .Append(",SYSDATE ")
                .Append(",:UPDATEACCOUNT ")
                .Append(",:UPDATEFUNCTION ")
                .Append(",1 ")
                .Append(")")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3160219_007")

                query.CommandText = sqlInsert.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ROEXTERIORID", OracleDbType.Decimal, roExteriorId)
                query.AddParameterWithTypeValue("PARTSTYPE", OracleDbType.NVarchar2, partsType)
                query.AddParameterWithTypeValue("DAMAGETYPEEXISTS", OracleDbType.NVarchar2, damageTypeExists)
                query.AddParameterWithTypeValue("DAMAGEMEMO", OracleDbType.NVarchar2, memo)
                query.AddParameterWithTypeValue("ROTHUMBNAILID", OracleDbType.Decimal, roThumbnailId)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("CREATEFUNCTION", OracleDbType.NVarchar2, C_FUNCTION_ID)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("UPDATEFUNCTION", OracleDbType.NVarchar2, C_FUNCTION_ID)

                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    ret = True
                Else
                    ret = False
                End If

            End Using
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [IsSuccess:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  ret))

        Return ret

    End Function


    ''' <summary>
    ''' ROサムネイル画像論理削除
    ''' </summary>
    ''' <param name="roThumbnailId">ROサムネイルID</param>
    ''' <returns>処理結果  成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    Public Function SetROThumbnailInfo(ByVal roThumbnailId As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [RO_THUMBNAIL_ID:{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  roThumbnailId.ToString(CultureInfo.InvariantCulture)))

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE /* SC3160219_008 */ ")
            .Append("       TB_T_RO_THUMBNAIL ")
            .Append("   SET DISP_FLG = '0' ")
            .Append(" WHERE RO_THUMBNAIL_ID = :ROTHUMBNAILID ")
        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3160219_008")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("ROTHUMBNAILID", OracleDbType.Long, roThumbnailId)

            Dim ret As Boolean

            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                ret = True
            Else
                ret = False
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End, [IsSuccess:{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      ret))

            Return ret

        End Using
    End Function

#End Region

#End Region

End Class
