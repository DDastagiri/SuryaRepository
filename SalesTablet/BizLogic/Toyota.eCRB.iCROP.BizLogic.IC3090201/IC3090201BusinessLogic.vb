'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3090201BusinessLogic.vb
'──────────────────────────────────
'機能： 来店通知送信I
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/06/04 KN  m.asano    ログイン中のGKのみPushするように修正  $01
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────
Imports Microsoft.VisualBasic

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.GateKeeper.GateNoticeSend.DataAccess
Imports System.Net
Imports System.Text
Imports System.IO
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.Visit.Api.BizLogic

''' <summary>
''' IC3090201 来店通知送信IF ビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class IC3090201BusinessLogic
    Inherits BaseBusinessComponent
    Implements IIC3090201BusinessLogic

#Region "定数"

    ''' <summary>
    ''' 門番の権限ナンバー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GateKeeperNo As Decimal = 50

#Region "メッセージID"

    ''' <summary>
    ''' 正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' 門番のスタッフ情報が0件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdAccountInfoIsNull As Integer = 1101

    ''' <summary>
    ''' 販売店コードに該当するマスタデータが存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdDealerInfoIsNull As Integer = 1102

    ''' <summary>
    ''' 店舗コードに該当するマスタデータが存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdBranchInfoIsNull As Integer = 1103

    ''' <summary>
    ''' Push送信に失敗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdPushSendFailed As Integer = 6001

    ''' <summary>
    ''' システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSystemError As Integer = 9999

#End Region

#End Region

#Region "来店通知送信"

    ''' <summary>
    ''' 来店通知送信
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="vehicleRegNo">車両登録No.</param>
    ''' <returns>終了コード</returns>
    ''' <remarks>
    ''' 門番への来店通知送信は、来店車両実績情報の登録処理がコミットされてから実施する
    ''' </remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Public Function SendGateNotice(ByVal dealerCode As String, ByVal storeCode As String, ByVal vehicleRegNo As String) As Integer Implements IIC3090201BusinessLogic.SendGateNotice

        'SendGateNotice開始ログ出力
        Dim sendGateNoticeStartLogInfo As New StringBuilder
        sendGateNoticeStartLogInfo.Append("SendGateNotice_Start ")
        sendGateNoticeStartLogInfo.Append("param1[" & dealerCode & "]")
        sendGateNoticeStartLogInfo.Append(",param2[" & storeCode & "]")
        sendGateNoticeStartLogInfo.Append(",param3[" & vehicleRegNo & "]")
        Logger.Info(sendGateNoticeStartLogInfo.ToString())

        '終了コード
        Dim resultId As Integer = MessageIdSuccess

        Logger.Info("SendGateNotice_001 " & "MasterCheck_Start")

        'マスターチェック
        resultId = IsVaildMaster(dealerCode, storeCode)

        If resultId <> MessageIdSuccess Then

            Logger.Info("SendGateNotice_002 " & "IsVaildMaster NG")

            'エラー出力
            Logger.Info("ResultId : " & CStr(resultId))

            'チェックに引っかかっていたら返却
            Logger.Info("SendGateNotice_End Ret[" & CStr(resultId) & "]")
            Return resultId
        End If

        Logger.Info("SendGateNotice_003 " & "IsVaildMaster OK")

        'ユーザマスタから販売店コード、店舗コード、権限：門番を条件に、門番のスタッフ情報を取得
        Dim users As Users = New Users
        Dim operationCdList As New List(Of Decimal)

        '門番の権限を格納
        operationCdList.Add(GateKeeperNo)

        'userData取得ログ出力
        Dim userDataStartLogInfo As New StringBuilder
        userDataStartLogInfo.Append("SendGateNotice_004 " & "Call_Start users.GetAllUser ")
        userDataStartLogInfo.Append("param1[" & dealerCode & "]")
        userDataStartLogInfo.Append(",param2[" & storeCode & "]")
        userDataStartLogInfo.Append(",param3[" & operationCdList.Item(0).ToString(CultureInfo.InvariantCulture()) & "]")
        Logger.Info(userDataStartLogInfo.ToString())

        '販売店コード、店舗コード、門番を元にスタッフ情報を取得
        Dim userData As UsersDataSet.USERSDataTable = users.GetAllUser(dealerCode, storeCode, operationCdList)

        'userData取得ログ出力
        Dim userDataEndLogInfo As New StringBuilder
        userDataEndLogInfo.Append("SendGateNotice_004 " & "Call_End users.GetAllUser ")
        userDataEndLogInfo.Append("Ret[" & userData.ToString & "]")
        Logger.Info(userDataEndLogInfo.ToString())

        '門番のスタッフ情報チェック
        If userData.Count = 0 Then

            userDataStartLogInfo.Append("SendGateNotice_005 NotStaffInfo")

            'スタッフ情報が0件
            resultId = MessageIdAccountInfoIsNull

            'エラー出力
            Logger.Info("ResultId : " & CStr(resultId))

            Logger.Info("SendGateNotice_End Ret[" & CStr(resultId) & "]")
            Return resultId
        End If

        'マスタチェック終了
        Logger.Info("SendGateNotice_006 MasterCheck_End")

        'デバッグログ出力(来店日時取得開始)
        Logger.Info("SendGateNotice_007 " & "Call_Start DateTimeFunc.Now Param[" & dealerCode & "]")

        '日付管理機能から来店日時(現在日時)を販売店コードを元に取得
        Dim visitTimeStamp As Date = DateTimeFunc.Now(dealerCode)

        'デバッグログ出力(来店日時取得終了)
        Logger.Info("SendGateNotice_008 " & "Call_End DateTimeFunc.Now Ret[" & visitTimeStamp & "]")

        '来店車両実績シーケンス
        Dim seq As Long = Nothing

        Try

            Using adapter As New IC3090201DataSetTableAdapters.IC3090201DataSetTableAdapter

                '来店車両実績シーケンスの次番号を取得
                seq = adapter.GetSeqNextValue()

                '来店車両実績情報を登録
                adapter.InsertVisitVehicle(seq, dealerCode, storeCode, vehicleRegNo, visitTimeStamp)

            End Using

        Catch ex As OracleException

            'デバッグログ出力(データベースエラー)
            Logger.Error("SendGateNotice_009 OracleException")

            'データベースでエラーがあった場合、ロールバック
            Me.Rollback = True
            resultId = MessageIdSystemError

            'ログ出力
            Logger.Error("ResultId : " & CStr(resultId))
            Logger.Error("ErrorID:" & CStr(ex.Number) & "Exception:" & ex.Message)
            Logger.Error("SendGateNotice_End Ret[" & CStr(resultId) & "]")
            Return resultId

        End Try

        '終了デバッグログ出力
        Dim sendGateNoticeEndLogInfo As New StringBuilder
        sendGateNoticeEndLogInfo.Append("SendGateNotice_End ")
        sendGateNoticeEndLogInfo.Append("Ret[" & CStr(resultId) & "]")
        Logger.Info(sendGateNoticeEndLogInfo.ToString())

        Return resultId
    End Function

    ''' <summary>
    ''' 門番スタッフへゲート通知送信命令を送信
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <remarks></remarks>
    Public Sub PushGateNotice(ByVal dealerCode As String, ByVal storeCode As String)

        'PushGateNotice開始ログ出力
        Dim sendGateNoticeStartLogInfo As New StringBuilder
        sendGateNoticeStartLogInfo.Append("PushGateNotice_Start ")
        sendGateNoticeStartLogInfo.Append("param1[" & dealerCode & "]")
        sendGateNoticeStartLogInfo.Append(",param2[" & storeCode & "]")
        Logger.Info(sendGateNoticeStartLogInfo.ToString())

        ' $01 START ログイン中のGKのみPushするように修正
        'ユーザマスタから販売店コード、店舗コード、権限：門番を条件に、門番のスタッフ情報を取得
        'Dim users As Users = New Users
        'Dim operationCdList As New List(Of Decimal)

        '門番の権限を格納
        'operationCdList.Add(GateKeeperNo)

        '販売店コード、店舗コード、門番を元にスタッフ情報を取得
        'Dim userData As UsersDataSet.USERSDataTable _
        '    = users.GetAllUser(dealerCode, storeCode, operationCdList)

        Dim userData As IC3090201DataSet.IC3090201PushAccountDataTable
        Using adapter As New IC3090201DataSetTableAdapters.IC3090201DataSetTableAdapter

            userData = adapter.GetPushStaff(dealerCode, storeCode)

        End Using

        '門番スタッフのアカウントを取得
        For Each target As IC3090201DataSet.IC3090201PushAccountRow In userData

            'Push機能にて、門番のiPod Touch端末へ、ゲート通知送信命令を送信
            SendGatePush(target.Account, dealerCode)
        Next

        userData.Dispose()
        userData = Nothing
        ' $01 END ログイン中のGKのみPushするように修正

        '終了デバッグログ出力
        Dim sendGateNoticeEndLogInfo As New StringBuilder
        sendGateNoticeEndLogInfo.Append("PushGateNotice_End ")
        Logger.Info(sendGateNoticeEndLogInfo.ToString())
    End Sub

#End Region

#Region "マスターデータチェック"

    ''' <summary>
    ''' マスターデータチェックメソッド
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <returns>チェック結果を終了コードで返却</returns>
    ''' <remarks></remarks>
    Private Function IsVaildMaster(ByVal dlrCd As String, ByVal strCd As String) As Integer

        'マスタチェック開始
        Dim startLogMaster As New StringBuilder
        startLogMaster.Append("IsVaildMaster_Start ")
        startLogMaster.Append("param1[" & dlrCd & "]")
        startLogMaster.Append(",param2[" & strCd & "]")
        Logger.Info(startLogMaster.ToString())

        '販売店コードの存在チェック
        Logger.Info("IsVaildMaster_001 Call_Start dealers.GetDealer Param[" & dlrCd & "]")
        Dim dealers As Dealer = New Dealer
        Dim dealerData As DealerDataSet.DEALERRow = dealers.GetDealer(dlrCd)

        '指定した販売店コードが取れなかった場合
        If dealerData Is Nothing Then

            Logger.Info("IsVaildMaster_002  dealerData Is Nothing")

            '終了ログ
            Logger.Info("IsVaildMaster_End Ret[" & MessageIdDealerInfoIsNull & "]")
            Return MessageIdDealerInfoIsNull
        End If
        Logger.Info("IsVaildMaster_001 Call_End dealers.GetDealer Ret[" & dealerData.ToString & "]")

        '店舗コードの存在チェック
        Dim storesLogMaster As New StringBuilder
        storesLogMaster.Append("IsVaildMaster_003 Call_Start stores.GetBranch ")
        storesLogMaster.Append("param1[" & dlrCd & "]")
        storesLogMaster.Append(",param2[" & strCd & "]")
        Logger.Info(storesLogMaster.ToString())
        Dim stores As Branch = New Branch
        Dim storesData As BranchDataSet.BRANCHRow = stores.GetBranch(dlrCd, strCd)

        '指定した店舗コードが取れなかった場合
        If storesData Is Nothing Then

            Logger.Info("IsVaildMaster_004  storesData Is Nothing")

            '終了ログ
            Logger.Info("IsVaildMaster_End Ret[" & MessageIdBranchInfoIsNull & "]")
            Return MessageIdBranchInfoIsNull
        End If
        Logger.Info("IsVaildMaster_003 Call_End stores.GetBranch Ret[" & storesData.ToString & "]")


        '終了ログ
        Logger.Info("IsVaildMaster_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess
    End Function

#End Region

#Region "push機能"

    ''' <summary>
    ''' 来店通知を門番に送信
    ''' </summary>
    ''' <param name="accountCd">アカウント</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <remarks>push送信を行う</remarks>
    Private Sub SendGatePush(ByVal accountCd As String, ByVal dealerCode As String)

        'デバッグログ出力(PUSH開始)
        Dim sendGatePushStartLogInfo As New StringBuilder
        sendGatePushStartLogInfo.Append("SendGatePush_Start ")
        sendGatePushStartLogInfo.Append("param1[" & accountCd & "]")
        Logger.Info(sendGatePushStartLogInfo.ToString())

        'POST送信する文字列を作成する
        Dim postMsg As New StringBuilder
        With postMsg
            .Append("cat=action")
            .Append("&type=main")
            .Append("&sub=js")
            .Append("&uid=" & accountCd)
            .Append("&time=0")
            .Append("&js1=sc3090301pushRecv()")
        End With

        'Push送信を行う
        Dim util As New VisitUtility
        util.SendPush(postMsg.ToString, dealerCode)

        'デバッグログ出力(PUSH終了)
        Logger.Info("SendGatePush_End")

    End Sub
#End Region

End Class