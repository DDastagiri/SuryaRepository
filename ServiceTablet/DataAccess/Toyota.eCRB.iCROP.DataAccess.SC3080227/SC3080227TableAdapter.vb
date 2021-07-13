'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080227TableAdapter.vb
'─────────────────────────────────────
'機能： 顧客イメージ画像取得のデータアクセス
'補足： 
'作成： 2017/01/05 NSK 荒川
'更新： 
'──────────────────────────────────

Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace SC3080227DataSetTableAdapters

    ''' <summary>
    ''' 顧客イメージ画像取得のテーブルアダプタークラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3080227TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 顧客画像ファイルのサイズ：S
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ImageFileSmall As String = "S"

        ''' <summary>
        ''' 顧客画像ファイルのサイズ：M
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ImageFileMedium As String = "M"

        ''' <summary>
        ''' 顧客画像ファイルのサイズ：L
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ImageFileLarge As String = "L"

#End Region

#Region "メソッド"

        ''' <summary>
        ''' 顧客イメージ画像ファイル名取得
        ''' </summary>
        ''' <param name="DealerCode">販売店コード</param>
        ''' <param name="DMSCstCdDisp">基幹顧客コード(表示用)</param>
        ''' <param name="imageSize">画像サイズ</param>
        ''' <returns>顧客イメージ画像ファイル名データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetCustImageFile(ByVal DealerCode As String, ByVal DMSCstCdDisp As String, ByVal imageSize As String) As SC3080227DataSet.CustImageFileDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start DLR_CD:[{1}] DMS_CST_CD_DISP:[{2}] IMAGEFILE_SIZE:[{3}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  DealerCode, DMSCstCdDisp, imageSize))

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080227_001 */ ")
                If String.Equals(imageSize, ImageFileSmall) Then 'イメージ画像(小)
                    .Append("       IMG_FILE_SMALL AS IMAGEFILE_NAME ")
                ElseIf String.Equals(imageSize, ImageFileMedium) Then 'イメージ画像(中)
                    .Append("       IMG_FILE_MEDIUM AS IMAGEFILE_NAME ")
                ElseIf String.Equals(imageSize, ImageFileLarge) Then 'イメージ画像(大)
                    .Append("       IMG_FILE_LARGE AS IMAGEFILE_NAME ")
                End If
                .Append("  FROM TB_M_CUSTOMER_DLR CSTDLR, TB_M_CUSTOMER CST ")
                .Append(" WHERE CST.CST_ID = CSTDLR.CST_ID ")
                .Append("   AND CSTDLR.DLR_CD = :DLR_CD ")
                .Append("   AND CST.DMS_CST_CD_DISP = :DMS_CST_CD_DISP ")
            End With

            Dim dt As SC3080227DataSet.CustImageFileDataTable = Nothing

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080227DataSet.CustImageFileDataTable)("SC3080227_001")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, DealerCode)
                query.AddParameterWithTypeValue("DMS_CST_CD_DISP", OracleDbType.NVarchar2, DMSCstCdDisp)

                ' SQLを実行
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          "{0} End RowCount:[{1}]",
                                          MethodBase.GetCurrentMethod.Name,
                                          dt.Rows.Count))

            ' 結果を返却
            Return dt

        End Function

#End Region

    End Class

End Namespace