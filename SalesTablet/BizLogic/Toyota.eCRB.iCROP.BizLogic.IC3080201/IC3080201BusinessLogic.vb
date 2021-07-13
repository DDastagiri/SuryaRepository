'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3080201BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細(写真)
'補足： 
'作成： 2012/01/27 KN 佐藤（真）
'更新： 
'─────────────────────────────────────

Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.IC3080201

''' <summary>
''' IC3080201()
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class IC3080201BusinessLogic
    Inherits BaseBusinessComponent

#Region " 定数 "

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstReturnSuccess As Long = 0

    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstReturnNoMatch As Long = 902

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Native向け)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstFacePictureUploadPath As String = "FACEPIC_UPLOADPATH"

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstFacePictureUploadUrl As String = "FACEPIC_UPLOADURL"

#End Region

    ''' <summary>
    ''' 自社客取得（顔写真）
    ''' </summary>
    ''' <param name="inPictureDataTable">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>自社客の顔写真を取得する処理</remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function GetOrgPictureData(ByVal inPictureDataTable As IC3080201DataSet.IC3080201ParameterDataTable) As IC3080201DataSet.IC3080201OrgPictureDataTable

        '引数を編集
        Dim args As New List(Of String)
        For Each dr As IC3080201DataSet.IC3080201ParameterRow In inPictureDataTable.Rows
            For Each column As DataColumn In inPictureDataTable.Columns
                If dr.IsNull(column.ColumnName) = True Then
                    args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = NULL", column.ColumnName))
                Else
                    args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", column.ColumnName, dr(column.ColumnName)))
                End If
            Next
        Next
        '開始ログを出力
        OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

        '検索条件を取得
        Dim originalId As String = Nothing
        Using da As New IC3080201DataSetTableAdapters.IC3080201DataTableTableAdapter
            '検索条件（自社客連番）取得
            originalId = da.GetOriginalId(inPictureDataTable(0).DMSID)
        End Using

        '検索条件の取得判定
        If String.IsNullOrEmpty(originalId) Then
            '該当データなしを出力
            OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnNoMatch)

            Return New IC3080201DataSet.IC3080201OrgPictureDataTable
        End If

        '自社客情報を取得
        Using da As New IC3080201DataSetTableAdapters.IC3080201DataTableTableAdapter
            '自社客情報（顔写真）取得
            Using outPictureDataTable As IC3080201DataSet.IC3080201OrgPictureDataTable = da.GetPicture(originalId)

                '自社客情報の取得判定
                If outPictureDataTable.Count = 0 Then
                    '該当データなしを出力
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnNoMatch)

                    Return outPictureDataTable
                End If

                '「システム環境設定」テーブルのクラスを生成
                Dim sysEnv As New SystemEnvSetting
                Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

                '顔写真の保存先フォルダを取得
                For Each drOutCustomerDataTbl In outPictureDataTable
                    'ローカル
                    sysEnvRow = sysEnv.GetSystemEnvSetting(ConstFacePictureUploadPath)
                    drOutCustomerDataTbl.FACEPIC_UPLOADPATH = sysEnvRow.PARAMVALUE

                    'Web
                    sysEnvRow = sysEnv.GetSystemEnvSetting(ConstFacePictureUploadUrl)
                    drOutCustomerDataTbl.FACEPIC_UPLOADURL = sysEnvRow.PARAMVALUE
                Next

                '終了ログを出力
                OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnSuccess)

                Return outPictureDataTable
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 開始ログ出力
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="args">引数</param>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Sub OutPutStartLog(ByVal methodName As String, ByVal args As List(Of String))

        '引数をログに出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , methodName _
            , String.Join(", ", args.ToArray())))

    End Sub

    ''' <summary>
    ''' 終了ログ出力
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="returnCD">リターンコード</param>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Sub OutPutEndLog(ByVal methodName As String, ByVal returnCD As Long)

        'ログに出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            "{0}.{1} OUT:RETURNCODE = {2}" _
            , Me.GetType.ToString _
            , methodName _
            , returnCD))

    End Sub

End Class
