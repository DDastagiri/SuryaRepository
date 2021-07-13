'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080401BussinessLogic.vb
'─────────────────────────────────────
'機能： ヘルプ依頼画面 ビジネスロジック
'補足： 
'作成： 2012/01/30 TCS 鈴木(健)
'更新： 2013/06/30 TCS 趙 2013/10対応版
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CustomerInfo.Help.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
'2013/06/30 TCS 趙 2013/10対応版 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/06/30 TCS 趙 2013/10対応版　既存流用 END 

''' <summary>
''' ヘルプ依頼画面
''' ビジネスロジック層クラス
''' </summary>
''' <remarks></remarks>
Public Class SC3080401BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' 依頼種別：ヘルプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RequestClassHelp As String = "03"

    ''' <summary>
    ''' ステータス：依頼
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusRequest As String = "1"

    ''' <summary>
    ''' ステータス：キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusCancel As String = "2"

    ''' <summary>
    ''' カテゴリータイプ：Popup
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushCategoryPopup As String = "1"

    ''' <summary>
    ''' 表示位置：header
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeHeader As String = "1"

    ''' <summary>
    ''' 表示時間：3秒
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayTime As Long = 3

    ''' <summary>
    ''' 表示タイプ：Text
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayTypeText As String = "1"

    ''' <summary>
    ''' 色：薄い黄色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayColor As String = "1"

    ''' <summary>
    ''' 表示時関数：JavaScriptのメソッド名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayFunction As String = "icropScript.ui.openNoticeList()"

    ''' <summary>
    ''' 処理結果：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultIdSuccess As Long = 0

    ''' <summary>
    ''' 処理結果：異常終了（DBタイムアウト）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultIdDbTimeout As Long = 6000

    ''' <summary>
    ''' メッセージID：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpMessageIdSuccess As Long = 0

    ''' <summary>
    ''' メッセージID：異常終了（DBタイムアウト）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpMessageIdDBTimeout As Long = 9001

    ''' <summary>
    ''' メッセージID：異常終了（その他エラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpMessageIdSys As Long = 9999
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' メッセージID：正常終了
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>メッセージID：正常終了</remarks>
    Public Shared ReadOnly Property MessageIdSuccess() As Long
        Get
            Return prpMessageIdSuccess
        End Get
    End Property

    ''' <summary>
    ''' メッセージID：異常終了（DBタイムアウト）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>メッセージID：異常終了（DBタイムアウト）</remarks>
    Public Shared ReadOnly Property MessageIdDBTimeout() As Long
        Get
            Return prpMessageIdDBTimeout
        End Get
    End Property

    ''' <summary>
    ''' メッセージID：異常終了（その他エラー）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>メッセージID：異常終了（その他エラー）</remarks>
    Public Shared ReadOnly Property MessageIdSys() As Long
        Get
            Return prpMessageIdSys
        End Get
    End Property
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' 初期データを取得します。
    ''' </summary>
    ''' <param name="ds">ヘルプ依頼画面データセット</param>
    ''' <remarks></remarks>
    Public Sub GetInitialData(ByVal ds As SC3080401DataSet)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' ヘルプ依頼画面テーブルアダプタ
        Dim adapter As SC3080401TableAdapter

        Try
            ' パラメータ情報の取得
            Dim dr As SC3080401DataSet.SC3080401ParameterRow = ds.SC3080401Parameter.Item(0)

            ' テーブルアダプタのインスタンス生成
            adapter = New SC3080401TableAdapter

            ' ヘルプ情報の取得
            Using dtGetHelp As SC3080401DataSet.SC3080401GetHelpInfoDataTable = adapter.GetHelpInfo(dr)
                ds.SC3080401GetHelpInfo.Merge(dtGetHelp)
            End Using

            ' 依頼先情報の取得
            Using dtGetSendAccount As SC3080401DataSet.SC3080401GetSendAccountDataTable = adapter.GetSendAccount(dr)
                ds.SC3080401GetSendAccount.Merge(dtGetSendAccount)
            End Using

            ' ヘルプマスタの取得
            Using dtGetHelpMst As SC3080401DataSet.SC3080401GetHelpMstDataTable = adapter.GetHelpMst(dr)
                ds.SC3080401GetHelpMst.Merge(dtGetHelpMst)
            End Using

        Finally
            adapter = Nothing
        End Try

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End, GetHelpInfo RowCount:[{2}], GetSendAccount RowCount:[{3}], GetHelpMst RowCount:[{4}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name,
                                  ds.SC3080401GetHelpInfo.Rows.Count.ToString(CultureInfo.InvariantCulture),
                                  ds.SC3080401GetSendAccount.Rows.Count.ToString(CultureInfo.InvariantCulture),
                                  ds.SC3080401GetHelpMst.Rows.Count.ToString(CultureInfo.InvariantCulture)))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' ヘルプ依頼を登録します。
    ''' 　１．ヘルプ情報の登録
    ''' 　２．通知登録I/Fの呼び出し
    ''' 　３．ヘルプ情報の更新（通知依頼ID）
    ''' </summary>
    ''' <param name="dr">パラメータ情報データテーブル行</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function RegistHelpRequest(ByVal dr As SC3080401DataSet.SC3080401ParameterRow) As Long

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dr):[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dr)))
        ' ======================== ログ出力 終了 ========================

        ' ヘルプ依頼画面テーブルアダプタ
        Dim adapter As SC3080401TableAdapter
        ' メッセージID
        Dim messageID As Long

        Try
            ' ビジネスロジックのインスタンス生成
            adapter = New SC3080401TableAdapter

            ' ヘルプNoの取得
            dr.HELPNO = adapter.GetHelpNo()

            ' ヘルプ情報の登録
            adapter.SetHelpInfo(dr)

            ' 通知登録I/Fの呼び出し
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            Dim resultID As Decimal = Convert.ToDecimal(Me.CallRequestRegist(dr, StatusRequest), CultureInfo.InvariantCulture)
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

            ' 処理結果判定
            Select Case resultID
                Case ResultIdSuccess
                    ' 正常終了の場合

                    ' ヘルプ情報の更新
                    adapter.UpdateHelpInfo(dr)

                    ' メッセージIDの設定
                    messageID = prpMessageIdSuccess

                Case ResultIdDbTimeout
                    ' 異常終了（DBタイムアウト）の場合

                    ' ロールバック
                    Me.Rollback = True

                    ' メッセージIDの設定
                    messageID = prpMessageIdDBTimeout
            End Select

        Finally
            adapter = Nothing
        End Try

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End, messageID:[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, messageID.ToString(CultureInfo.InvariantCulture)))
        ' ======================== ログ出力 終了 ========================

        ' 結果を返却
        Return messageID

    End Function

    ''' <summary>
    ''' ヘルプ依頼をキャンセルします。
    ''' 　１．通知登録I/Fの呼び出し
    ''' </summary>
    ''' <param name="dr">パラメータ情報データテーブル行</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelHelpRequest(ByVal dr As SC3080401DataSet.SC3080401ParameterRow) As Long

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dr):[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dr)))
        ' ======================== ログ出力 終了 ========================

        ' メッセージID
        Dim messageID As Long

        ' 通知登録I/Fの呼び出し
        Dim resultID As Long = Convert.ToInt64(Me.CallRequestRegist(dr, StatusCancel), CultureInfo.InvariantCulture)

        ' 処理結果判定
        If resultID = ResultIdDbTimeout Then
            messageID = prpMessageIdDBTimeout
        Else
            messageID = prpMessageIdSuccess
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End, messageID:[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, messageID.ToString(CultureInfo.InvariantCulture)))
        ' ======================== ログ出力 終了 ========================

        ' 結果を返却
        Return messageID

    End Function
#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' 通知情報登録I/Fを呼び出します。
    ''' </summary>
    ''' <param name="dr">パラメータ情報データテーブル行</param>
    ''' <param name="status">ステータス（1：依頼 / 2：キャンセル）</param>
    ''' <returns>I/Fの終了コード</returns>
    ''' <remarks></remarks>
    Private Function CallRequestRegist(ByVal dr As SC3080401DataSet.SC3080401ParameterRow, ByVal status As String) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dr):[{2}], status:[{3}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dr), status))
        ' ======================== ログ出力 終了 ========================

        ' NoticeDataのインスタンス生成
        Using requestData As New XmlNoticeData

            ' Headタグのデータ格納
            requestData.TransmissionDate = DateTimeFunc.Now(dr.DLRCD)       '送信日付

            ' ReceiveAccountのインスタンス生成
            Using ReceiveAccount As New XmlAccount

                ' ReceiveAccountタグのデータ格納
                With ReceiveAccount
                    .ToAccount = dr.TOACCOUNT                               'スタッフコード（受信先）
                    .ToAccountName = dr.TOACCOUNTNAME                       'スタッフ名（受信先）
                End With

                ' ReceiveAccountを親クラスに格納
                requestData.AccountList.Add(ReceiveAccount)
            End Using

            ' RequestNoticeのインスタンス生成
            Using requestNotice As New XmlRequestNotice

                ' RequestNoticeタグのデータ格納
                With requestNotice
                    .DealerCode = dr.DLRCD                                  '販売店コード
                    .StoreCode = dr.STRCD                                   '店舗コード
                    .RequestClass = RequestClassHelp                        '依頼種別
                    .Status = status                                        'ステータス
                    If Not dr.IsNOTICEREQIDNull Then
                        .RequestId = dr.NOTICEREQID                         '依頼ID
                    End If
                    .RequestClassId = dr.HELPNO                             '依頼種別ID
                    .FromAccount = dr.FROMACCOUNT                           'スタッフコード（送信元）
                    .FromAccountName = dr.FROMACCOUNTNAME                   'スタッフ名（送信元）
                    .CustomId = dr.CRCUSTID                                 'お客様ID
                    .CustomName = dr.CUSTOMNAME                             'お客様名
                    .CustomerClass = dr.CUSTOMERCLASS                       '顧客分類
                    .CustomerKind = dr.CSTKIND                              '顧客種別
                    .SalesStaffCode = dr.SALESSTAFFCODE                     '顧客担当セールススタッフコード
                    .FollowUpBoxStoreCode = dr.FLLWUPBOX_STRCD              'Follow-up Box店舗コード
                    .FollowUpBoxNumber = dr.FLLWUPBOX_SEQNO                 'Follow-up Box内連番
                    '.VehicleSequenceNumber = dr.VIN                         '車両シーケンスNo
                End With

                ' RequestNoticeを親クラスに格納
                requestData.RequestNotice = requestNotice
            End Using

            ' PushInfoのインスタンス生成
            Using pushInfo As New XmlPushInfo

                ' PushInfoタグのデータ格納
                With pushInfo
                    .PushCategory = PushCategoryPopup                                                           'カテゴリータイプ
                    .PositionType = PositionTypeHeader                                                          '表示位置
                    .Time = DisplayTime                                                                         '表示時間
                    .DisplayType = DisplayTypeText                                                              '表示タイプ
                    If status.Equals(StatusRequest) Then
                        .DisplayContents = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9002)       '表示内容（依頼時）
                    Else
                        .DisplayContents = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9003)       '表示内容（キャンセル時）
                    End If
                    .Color = DisplayColor                                                                       '色
                    .DisplayFunction = DisplayFunction                                                          '表示時関数
                    .ActionFunction = DisplayFunction                                                           'アクション時関数
                End With

                ' PushInfoを親クラスに格納
                requestData.PushInfo = pushInfo
            End Using

            ' 通知情報登録I/Fのインスタンス生成
            Using bizLogic As IC3040801BusinessLogic = New IC3040801BusinessLogic

                ' 通知情報登録I/Fの呼び出し
                Dim responseData As XmlCommon = bizLogic.NoticeDisplay(requestData, ConstCode.NoticeDisposal.Peculiar)

                ' 通知依頼情報.通知依頼IDを格納
                dr.NOTICEREQID = responseData.NoticeRequestId

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          "{0}.ascx {1}_End, ResultId:[{2}]",
                                          SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, responseData.ResultId))
                ' ======================== ログ出力 終了 ========================

                ' 結果を返却
                Return responseData.ResultId
            End Using
        End Using

    End Function

#End Region

End Class

