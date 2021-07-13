'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3150201BusinessLogic.vb
'─────────────────────────────────────
'機能： TCステータスモニター_ビジネスロジック
'補足： 
'作成： 2013/02/21 TMEJ 成澤
'更新：2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3150201


Public Class SC3150201BusinessLogic
    Inherits BaseBusinessComponent


#Region "定数"

    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2
    ''' <summary>
    ''' DateTimeFuncにて、"yyyyMMdd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDD As Integer = 9
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYY_MM_DD As Integer = 21
   
#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private userContext As StaffContext

#End Region

#Region "ストール情報取得"

    ''' <summary>
    ''' ストール情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>ストール情報データセット</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発
    ''' </History>
    Public ReadOnly Property GetStallData(ByVal stallId As Decimal) As SC3150201DataSet.SC3150201StallInfoDataTable

        Get
            Logger.Info("GetStallData Start")
            userContext = StaffContext.Current
            'アダプター呼び出し
            Using adapter As New SC3150201DataSetTableAdapters.SC3150201StallInfoDataTableAdapter
                'データテーブルを定義
                Dim dt As SC3150201DataSet.SC3150201StallInfoDataTable

                'ストール情報を取得、データセットに格納
                dt = adapter.GetStallInfo(Me.userContext.DlrCD, _
                                          Me.userContext.BrnCD, _
                                          Me.userContext.Account, _
                                          stallId)

                Logger.Info("GetStallData End")
                Return dt
            End Using
        End Get

    End Property

#End Region

#Region "チップ情報（予約・実績）の取得"

    ''' <summary>
    ''' 予定・実績チップ情報の取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼働開始時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWorkData(ByVal stallId As Decimal, _
                                ByVal fromDate As Date) As SC3150201DataSet.SC3150201ChipInfoDataTable

        Logger.Info("GetWorkData Start")

        Dim returnChipInfo As SC3150201DataSet.SC3150201ChipInfoDataTable ' 戻り値用
        userContext = StaffContext.Current

        'アダプター呼び出し
        Using adapter As New SC3150201DataSetTableAdapters.SC3150201StallInfoDataTableAdapter
            '実績チップ情報の取得
            returnChipInfo = adapter.GetResultChipInfo(Me.userContext.DlrCD, Me.userContext.BrnCD, stallId, fromDate)

            '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
            ''実績チップ情報をチップ情報データセットに格納する
            'Using chipInfo As New SC3150201DataSet.SC3150201ChipInfoDataTable
            '    Dim resultItem As SC3150201DataSet.SC3150201ResultChipInfoRow
            '    For Each resultItem In resultData.Rows

            '        '実績チップ情報を格納
            '        Dim chipInfoItem As SC3150201DataSet.SC3150201ChipInfoRow = _
            '                                DirectCast(chipInfo.NewRow(), SC3150201DataSet.SC3150201ChipInfoRow)

            '        '実績チップ情報の取得
            '        chipInfoItem = Me.SetResultChipData(chipInfoItem, resultItem)

            '        '「chipInfo」の行にchipInfoItemを追加
            '        chipInfo.Rows.Add(chipInfoItem)
            '    Next

            'returnChipInfo = DirectCast(chipInfo.Copy, SC3150201DataSet.SC3150201ChipInfoDataTable)

            'End Using
            '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END
        End Using

        Logger.Info("GetWorkData End")
        Return returnChipInfo

    End Function
    '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' ストール実績情報をチップ用に形成
    ' ''' </summary>
    ' ''' <param name="chipInfoItem">チップ情報レコード（設定するテーブルのコピー）</param>
    ' ''' <param name="resultItem">ストール実績レコード</param>
    ' ''' <returns>チップ情報レコード</returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' </History>
    'Private Function SetResultChipData(ByVal chipInfoItem As SC3150201DataSet.SC3150201ChipInfoRow, _
    '                                   ByVal resultItem As SC3150201DataSet.SC3150201ResultChipInfoRow _
    '                                   ) As SC3150201DataSet.SC3150201ChipInfoRow

    '    '実績チップ情報を初期化
    '    chipInfoItem = Me.InitChipInfoItem(chipInfoItem)

    '    '車両登録NO
    '    If (Not resultItem.IsVCLREGNONull()) Then
    '        chipInfoItem.VCLREGNO = resultItem.VCLREGNO
    '    End If

    '    '着工指示区分
    '    If (Not resultItem.IsINSTRUCTNull()) Then
    '        chipInfoItem.INSTRUCT = resultItem.INSTRUCT
    '    End If

    '    '使用開始日時(着工計画)
    '    chipInfoItem.STARTTIME = resultItem.STARTTIME

    '    '使用終了日時(完了計画)
    '    chipInfoItem.ENDTIME = resultItem.ENDTIME

    '    '予約_納車_希望日時時刻(納車予定時刻)
    '    If (Not resultItem.IsREZ_DELI_DATENull()) Then
    '        chipInfoItem.REZ_DELI_DATE = resultItem.REZ_DELI_DATE
    '    End If

    '    '実績_入庫時間(着工実績)
    '    If (Not resultItem.IsRESULT_START_TIMENull()) Then
    '        chipInfoItem.RESULT_START_TIME = resultItem.RESULT_START_TIME
    '    End If



    '    Return chipInfoItem
    'End Function
   
    ' ''' <summary>
    ' ''' 実績チップ情報を初期化
    ' ''' </summary>
    ' ''' <param name="chipInfoItem">実績チップ情報</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' </History>
    'Private Function InitChipInfoItem(ByVal chipInfoItem As SC3150201DataSet.SC3150201ChipInfoRow) As SC3150201DataSet.SC3150201ChipInfoRow
    '    With chipInfoItem

    '        .VCLREGNO = ""
    '        .STARTTIME = Nothing
    '        .ENDTIME = Nothing
    '        .RESULT_START_TIME = Nothing
    '        .REZ_DELI_DATE = Nothing

    '    End With
    '    Return chipInfoItem
    'End Function

    '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END

#End Region

#Region "リフレッシュタイムの取得"
    ''' <summary>
    ''' リフレッシュタイムの取得
    ''' </summary>
    ''' <returns>リフレッシュタイム</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GetRefreshData() As SC3150201DataSet.SC3150201RefreshTimeDataTable

        Get
            Logger.Info("GetRefreshData  Start")
            userContext = StaffContext.Current
            'アダプター呼び出し
            Using adapter As New SC3150201DataSetTableAdapters.SC3150201StallInfoDataTableAdapter
                'データテーブルを定義
                Dim dt As SC3150201DataSet.SC3150201RefreshTimeDataTable

                'リフレッシュタイムを取得、データセットに格納
                dt = adapter.GetRefreshTime(Me.userContext.DlrCD, Me.userContext.BrnCD)

                Logger.Info("GetRefreshData End")

                Return dt
            End Using
        End Get

    End Property

#End Region

End Class

