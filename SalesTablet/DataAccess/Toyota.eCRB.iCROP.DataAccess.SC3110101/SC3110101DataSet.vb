
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3060102DataSet.vb
'─────────────────────────────────────
'機能： 試乗入力データアクセス
'補足： 
'作成： 
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization


Namespace SC3110101DataSetTableAdapters

    ''' <summary>
    ''' SC3110101 試乗入力画面 データ層
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3110101TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AppId As String = "SC3110101"

#End Region

#Region "コンストラクタ"

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>処理なし</remarks>
        Public Sub New()

        End Sub

#End Region

#Region "試乗車の取得"

        ''' <summary>
        ''' 試乗車の情報(販売店コード、試乗車ID、グレード名、外板色、シリーズコード、型式コード、外装コード)の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="nowDate">現在日時</param>
        ''' <returns>引数で指定した値に一致した試乗車情報を返却</returns>
        ''' <remarks></remarks>
        Public Function GetTestDriveCar(ByVal dealerCode As String, ByVal storeCode As String, ByVal nowDate As Date) As SC3110101DataSet.SC3110101CarStatusInfoDataTable

            Using query As New DBSelectQuery(Of SC3110101DataSet.SC3110101CarStatusInfoDataTable)("SC3110101_001")

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3110101_001 */ ")
                    .Append("        TEST_DLR.DLR_CD  AS DLRCD ")
                    .Append("      , TEST_DLR.VCL_TESTDRIVE_ID AS TESTDRIVECARID ")
                    .Append("      , STATUS.TESTDRIVECARSTATUS AS TESTDRIVECARSTATUS ")
                    .Append("      , MODEL.MODEL_NAME AS TESTDRIVECARNAME ")
                    .Append("      , GRADE.GRADE_NAME AS GRADENAME ")
                    .Append("      , COL.BODYCLR_NAME AS CORTCOLOR ")
                    .Append("      , TEST.BODYCLR_CD AS BODYCLRCD ")
                    .Append("      , TO_CHAR(STATUS.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS') AS UPDATEDATE ")
                    .Append("      , TEST.MODEL_CD AS VCLSERIESCD ")
                    .Append("      , TEST.VCL_KATASHIKI AS VCLMODELCD ")
                    .Append(" FROM ")
                    .Append("      TB_M_VCL_TESTDRIVE_DLR TEST_DLR ")
                    .Append("    , TB_M_VCL_TESTDRIVE TEST ")
                    .Append("    , TB_M_MODEL MODEL ")
                    .Append("    , TB_M_GRADE GRADE ")
                    .Append("    , TB_M_BODYCOLOR COL ")
                    .Append("    , TBL_TESTDRIVECAR_STATUS STATUS ")
                    .Append(" WHERE ")
                    .Append("       TEST_DLR.VCL_TESTDRIVE_ID = TEST.VCL_TESTDRIVE_ID ")
                    .Append("   AND TEST.MODEL_CD = MODEL.MODEL_CD(+) ")
                    .Append("   AND TEST.MODEL_CD = GRADE.MODEL_CD(+) ")
                    .Append("   AND TEST.GRADE_CD = GRADE.GRADE_CD(+) ")
                    .Append("   AND TEST.MODEL_CD = COL.MODEL_CD(+) ")
                    .Append("   AND TEST.GRADE_CD = COL.GRADE_CD(+) ")
                    .Append("   AND TEST.SUFFIX_CD = COL.SUFFIX_CD(+) ")
                    .Append("   AND TEST.BODYCLR_CD = COL.BODYCLR_CD(+) ")
                    .Append("   AND TO_CHAR(TEST_DLR.DLR_CD) = STATUS.DLRCD(+) ")
                    .Append("   AND TEST_DLR.VCL_TESTDRIVE_ID  = STATUS.TESTDRIVECARID(+) ")
                    .Append("   AND TEST_DLR.DLR_CD = :DLR_CD ")
                    .Append("   AND TEST_DLR.BRN_CD = :BRN_CD ")
                    .Append("   AND TEST_DLR.USE_TYPE = '2' ")
                    .Append("   AND TEST_DLR.ASSIGN_SCHE_FROM_DATETIME <= :CHEXPECTEDDATE ")
                    .Append("   AND TEST_DLR.ASSIGN_SCHE_TO_DATETIME >= :CHEXPECTEDDATE ")
                    .Append("   AND ( ")
                    .Append("           TEST_DLR.ASSIGN_RSLT_TO_DATETIME = TO_DATE('1900-01-01 00:00:00' ,'YYYY-MM-DD HH24:MI:SS') ")
                    .Append("        OR TEST_DLR.ASSIGN_RSLT_TO_DATETIME >= :CHEXPECTEDDATE ")
                    .Append("       ) ")
                    .Append("   AND TEST.INUSE_FLG = '1' ")
                    .Append("  ORDER BY ")
                    .Append("        TEST.ROW_UPDATE_DATETIME ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)         '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, storeCode)          '店舗コード
                query.AddParameterWithTypeValue("CHEXPECTEDDATE", OracleDbType.Date, nowDate)   '配車開始実績日、配車終了実績日

                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 試乗車の情報(イメージ画像)の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="seriesCode">シリーズコード</param>
        ''' <param name="modelCode">モデルコード</param>
        ''' <param name="colorCode">カラーコード</param>
        ''' <returns>引数で指定した値に一致したイメージ画像情報を返却</returns>
        ''' <remarks></remarks>
        Public Function GetModelImageFile(ByVal dealerCode As String, ByVal seriesCode As String, ByVal modelCode As String, ByVal colorCode As String) As SC3110101DataSet.SC3110101ModelPictureInfoDataTable

            Using query As New DBSelectQuery(Of SC3110101DataSet.SC3110101ModelPictureInfoDataTable)("SC3110101_002")

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim pictureSql As New StringBuilder
                With pictureSql
                    .Append(" SELECT /* SC3110101_002 */ ")
                    .Append("        PICTURE.VCL_PICTURE AS IMAGEFILE")
                    .Append("   FROM ( ")
                    .Append("         SELECT ")
                    .Append("                MODEL_DLR.DLR_CD ")
                    .Append("              , MODEL_DLR.MODEL_CD ")
                    .Append("              , KATASHIKI.VCL_KATASHIKI ")
                    .Append("              , KATASHIKI_PIC.BODYCLR_CD ")
                    .Append("              , KATASHIKI_PIC.VCL_PICTURE ")
                    .Append("              , ROW_NUMBER() OVER(ORDER BY CASE WHEN MODEL_DLR.DLR_CD = :DLRCD THEN 0 ELSE 1 END) AS T ")
                    .Append("           FROM ")
                    .Append("                TB_M_MODEL_DLR MODEL_DLR ")
                    .Append("              , TB_M_MODEL MODEL ")
                    .Append("              , TB_M_KATASHIKI KATASHIKI ")
                    .Append("              , TB_M_KATASHIKI_PICTURE KATASHIKI_PIC ")
                    .Append("          WHERE MODEL_DLR.MODEL_CD = MODEL.MODEL_CD ")
                    .Append("            AND MODEL_DLR.MODEL_CD = KATASHIKI.MODEL_CD ")
                    .Append("            AND KATASHIKI.VCL_KATASHIKI = KATASHIKI_PIC.VCL_KATASHIKI ")
                    .Append("            AND MODEL_DLR.DLR_CD IN (:DLRCD, 'XXXXX') ")
                    .Append("            AND MODEL_DLR.MODEL_CD = :SERIESCD ")
                    .Append("            AND KATASHIKI.VCL_KATASHIKI = :MODELCD ")
                    .Append("            AND KATASHIKI_PIC.BODYCLR_CD = :COLORCD ")
                    .Append("            AND MODEL.INUSE_FLG = '1' ")
                    .Append("        ) PICTURE ")
                    .Append(" ORDER BY ")
                    .Append("        PICTURE.T ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = pictureSql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)             '販売店コード
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, seriesCode)     'セールスコード
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.Varchar2, modelCode)        'モデルコード
                query.AddParameterWithTypeValue("COLORCD", OracleDbType.Varchar2, colorCode)        'カラーコード
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 試乗車情報(ロゴ(未選択))の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="seriesCode">シリーズコード</param>
        ''' <returns>引数で指定した値に一致したロゴ情報を返却</returns>
        ''' <remarks></remarks>
        Public Function GetModelLogo(ByVal dealerCode As String, ByVal seriesCode As String) As SC3110101DataSet.SC3110101ModelLogoInfoDataTable

            Using query As New DBSelectQuery(Of SC3110101DataSet.SC3110101ModelLogoInfoDataTable)("SC3110101_003")

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim logoSql As New StringBuilder
                With logoSql
                    .Append(" SELECT /* SC3110101_003 */ ")
                    .Append("        LOGO.LOGO_PICTURE AS LOGO_NOTSELECTED")
                    .Append("   FROM ( ")
                    .Append("         SELECT ")
                    .Append("                MODEL_DLR.DLR_CD ")
                    .Append("              , MODEL_DLR.MODEL_CD ")
                    .Append("              , MODEL.LOGO_PICTURE ")
                    .Append("              , ROW_NUMBER() OVER(ORDER BY CASE WHEN MODEL_DLR.DLR_CD = :DLRCD THEN 0 ELSE 1 END) AS T ")
                    .Append("           FROM ")
                    .Append("                TB_M_MODEL_DLR MODEL_DLR ")
                    .Append("              , TB_M_MODEL MODEL ")
                    .Append("          WHERE MODEL_DLR.MODEL_CD = MODEL.MODEL_CD ")
                    .Append("            AND MODEL_DLR.DLR_CD IN (:DLRCD, 'XXXXX') ")
                    .Append("            AND MODEL_DLR.MODEL_CD = :SERIESCD ")
                    .Append("            AND MODEL.INUSE_FLG = '1' ")
                    .Append("        ) LOGO ")
                    .Append(" ORDER BY ")
                    .Append("        LOGO.T ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = logoSql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.Char, seriesCode)
                Return query.GetData()
            End Using
        End Function

#End Region

#Region "試乗車ステータス情報の存在有無"

        ''' <summary>
        ''' 試乗車ステータス情報の存在有無
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="testDriveCarCode">試乗車ID</param>
        ''' <returns>存在有無(True:存在する / False:存在しない)</returns>
        ''' <remarks>試乗車のステータス情報の有無を確認する</remarks>
        Public Function ExistsTestDriveCar(ByVal dealerCode As String, ByVal testDriveCarCode As Decimal) As Boolean

            Using query As New DBSelectQuery(Of SC3110101DataSet.SC3110101ExistStatusDataTable)("SC3110101_004")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3110101_004 */")
                    .Append("        STATUS.DLRCD")
                    .Append("   FROM TBL_TESTDRIVECAR_STATUS STATUS")
                    .Append("  WHERE STATUS.DLRCD = :DLRCD")
                    .Append("    AND STATUS.TESTDRIVECARID = :TESTDRIVECARID")
                    .Append("    AND ROWNUM <= 1")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)                     '販売店コード

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("TESTDRIVECARID", OracleDbType.Decimal, testDriveCarCode)      '試乗車ID
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                If query.GetData().Count > 0 Then

                    Return True
                Else

                    Return False
                End If
            End Using
        End Function

#End Region

#Region "試乗車ステータスの更新"

        ''' <summary>
        ''' 試乗車ステータスの更新
        ''' </summary>
        ''' <param name="dataRow">試乗車ステータステーブル</param>
        ''' <param name="account">更新するアカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <returns>処理結果(True:成功 / False:失敗)</returns>
        ''' <remarks></remarks>
        Public Function UpdateTestDriveCar(ByVal dataRow As SC3110101DataSet.SC3110101InsertTestDriveCarStatusRow, ByVal account As String, ByVal updateId As String) As Boolean

            'データがない場合
            If dataRow Is Nothing Then

                Return False
            End If

            Using query As New DBUpdateQuery("SC3110101_005")

                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3110101_005 */")
                    .Append("        TBL_TESTDRIVECAR_STATUS")
                    .Append("    SET ACCOUNT = :ACCOUNT")
                    .Append("      , TESTDRIVECARSTATUS = :TESTDRIVECARSTATUS")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE DLRCD = :DLRCD")
                    .Append("    AND TESTDRIVECARID = :TESTDRIVECARID")
                    .Append("    AND UPDATEDATE  = :UPDATEDATE")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, dataRow.ACCOUNT)                      'アカウント
                query.AddParameterWithTypeValue("TESTDRIVECARSTATUS", OracleDbType.Char, dataRow.TESTDRIVECARSTATUS)    '試乗車ステータス
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dataRow.DLRCD)                              '販売店コード

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START                            
                query.AddParameterWithTypeValue("TESTDRIVECARID", OracleDbType.Decimal, dataRow.TESTDRIVECARID)         '試乗車ID
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)                            '更新ID
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)                        '更新アカウント
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, DateTime.Parse(dataRow.UPDATEDATE, CultureInfo.InvariantCulture()))                    '更新日

                If query.Execute() > 0 Then

                    Return True
                Else
                    Return False
                End If
            End Using
        End Function

#End Region

#Region "試乗車ステータスの挿入"

        ''' <summary>
        ''' 試乗車ステータスの挿入
        ''' </summary>
        ''' <param name="datarow">試乗車データロウ</param>
        ''' <param name="createAccount">作成アカウント</param>
        ''' <param name="createId">作成機能ID</param>
        ''' <returns>処理結果(True:成功 / False:失敗)</returns>
        ''' <remarks></remarks>
        Public Function InsertTestDriveCar(ByVal dataRow As SC3110101DataSet.SC3110101InsertTestDriveCarStatusRow, ByVal createAccount As String, ByVal createId As String) As Boolean

            'データがない場合
            If dataRow Is Nothing Then

                Return False
            End If

            Using query As New DBUpdateQuery("SC3110101_006")

                Dim sql As New StringBuilder
                With sql
                    .Append(" INSERT /* SC3110101_006 */ ")
                    .Append("   INTO TBL_TESTDRIVECAR_STATUS ( ")
                    .Append("        DLRCD ")
                    .Append("      , TESTDRIVECARID")
                    .Append("      , TESTDRIVECARSTATUS")
                    .Append("      , ACCOUNT")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append(" ) ")
                    .Append(" VALUES (  ")
                    .Append("        :DLRCD")
                    .Append("      , :TESTDRIVECARID")
                    .Append("      , :TESTDRIVECARSTATUS ")
                    .Append("      , :ACCOUNT ")
                    .Append("      , SYSDATE ")
                    .Append("      , SYSDATE ")
                    .Append("      , :CREATEACCOUNT ")
                    .Append("      , :UPDATEACCOUNT ")
                    .Append("      , :CREATEID ")
                    .Append("      , :UPDATEID )")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dataRow.DLRCD)                              '販売店コード

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START  
                query.AddParameterWithTypeValue("TESTDRIVECARID", OracleDbType.Decimal, dataRow.TESTDRIVECARID)         '試乗車ID
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.AddParameterWithTypeValue("TESTDRIVECARSTATUS", OracleDbType.Char, dataRow.TESTDRIVECARSTATUS)    '試乗車ステータス
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, dataRow.ACCOUNT)                      'アカウント
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, createAccount)                  '作成アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, createAccount)                  '更新アカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, createId)                            '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, createId)                            '更新機能ID

                If query.Execute() > 0 Then

                    Return True
                Else
                    Return False
                End If
            End Using
        End Function

#End Region

    End Class
End Namespace