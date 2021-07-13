'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080301BusinessLogic.vb
'─────────────────────────────────────
'機能： 査定依頼
'補足： 
'作成： 2012/01/05 TCS 鈴木(恭)
'更新： 2013/03/25 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 趙 2013/10対応版　既存流用
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.AssesmentRequest.DataAccess
Imports Toyota.eCRB.CustomerInfo.AssesmentRequest.DataAccess.SC3080301TableAdapter
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports System.Globalization

''' <summary>
''' SC3080301(Edit Customer)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3080301BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const OrgCustFlg As String = "1"

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NewCustFlg As String = "2"

    ''' <summary>
    ''' 非保有車両
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RetentionOther As String = "0"

    ''' <summary>
    ''' 保有車両
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RetentionCustomer As String = "1"

    ''' <summary>
    ''' 査定のステータス（依頼）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RequestStatus As String = "1"

    ''' <summary>
    ''' 査定のステータス（依頼受信）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RequestReceiveStatus As String = "3"

    ''' <summary>
    ''' 査定のステータス（査定受付）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const EndStatus As String = "4"

    ''' <summary>
    ''' 査定のステータス（査定キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CancelStatus As String = "2"

    ''' <summary>
    ''' 依頼種別（通知IF）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RequestClass As String = "01"

    ''' <summary>
    ''' カテゴリータイプ（通知IF）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PushCategory As String = "1"

    ''' <summary>
    ''' 表示位置（通知IF）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PositionType As String = "0"

    ''' <summary>
    ''' 表示タイプ（通知IF）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayType As String = "1"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AssessmentDisplayId As String = "SC3080301"

    ''' <summary>
    ''' 比較チェック用値
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CountCheckZero As Integer = 0

    ''' <summary>
    ''' 処理成功時の戻り値
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SuccessIfZero As String = "000000"

    ''' <summary>
    ''' 登録処理エラーの戻り値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InsertAssessmentError As String = "9001"

    ''' <summary>
    ''' 更新処理エラーの戻り値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateAssessmentError As String = "9002"

    ' 2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 更新ロックエラーの戻り値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateAssessmentLockError As String = "9003"
    ' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END

    ''' <summary>
    ''' メッセージID　通知依頼ID登録エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrMessageIdUpdateNotice As Integer = 999999

    ''' <summary>
    ''' メッセージID　登録エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrMessageIdInsertNotice As Integer = -1

    ''' <summary>
    ''' 文言マスタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SC3080301Word09 As Integer = 9
    Public Const SC3080301Word10 As Integer = 10
    Public Const SC3080301Word11 As Integer = 11
    Public Const SC3080301Word12 As Integer = 12
    Public Const SC3080301Word13 As Integer = 13

#End Region

#Region "Publicメソット"

    ''' <summary>
    ''' 端末ID情報取得
    ''' </summary>
    ''' <param name="sessionDLRCD">販売店コード</param>
    ''' <param name="sessionSTRCD">店舗コード</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客情報を取得する。</remarks>
    Public Function GetTerminalList(ByVal sessionDlrcd As String, ByVal sessionStrcd As String) As SC3080301DataSet.SC3080301UcarTerminalDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sessionDlrcd:{0}][sessionStrcd:{1}]", sessionDlrcd, sessionStrcd))

        Dim retTerminalDataTbl As SC3080301DataSet.SC3080301UcarTerminalDataTable
        Dim da As New SC3080301TableAdapter

        retTerminalDataTbl = da.GetUcarTerminal(sessionDlrcd, sessionStrcd)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retTerminalDataTbl:{0}]", retTerminalDataTbl.Rows.Count))

        Return retTerminalDataTbl

    End Function

    ''' <summary>
    ''' 来店情報取得
    ''' </summary>
    ''' <param name="sessionDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>来店情報を取得する。</remarks>
    Public Function GetVisitSalesList(ByVal sessionDataTbl As SC3080301DataSet.SC3080301SessionDataTable) As SC3080301DataSet.SC3080301VisitSalesDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sessionDataTbl:{0}]", sessionDataTbl.Rows.Count))

        Dim retVisitSalesDataTbl As SC3080301DataSet.SC3080301VisitSalesDataTable
        Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow
        Dim da As New SC3080301TableAdapter

        sessionDataRow = sessionDataTbl.Item(0)

        '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
        retVisitSalesDataTbl = da.GetVisitSales(sessionDataRow.FLLWUPBOX_SEQNO)
        '2013/06/30 TCS 趙 2013/10対応版 既存流用 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retVisitSalesDataTbl:{0}]", retVisitSalesDataTbl.Rows.Count))

        Return retVisitSalesDataTbl

    End Function

    ''' <summary>
    ''' 査定情報取得
    ''' </summary>
    ''' <param name="sessionDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>査定情報を取得する</remarks>
    Public Function GetAssessmentList(ByVal sessionDataTbl As SC3080301DataSet.SC3080301SessionDataTable) As SC3080301DataSet.SC3080301UcarAssessmentDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sessionDataTbl:{0}]", sessionDataTbl.Rows.Count))

        Dim retAssessmentDataTbl As SC3080301DataSet.SC3080301UcarAssessmentDataTable
        Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow
        Dim da As New SC3080301TableAdapter

        sessionDataRow = sessionDataTbl.Item(0)

        '査定中を取得
        '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
        retAssessmentDataTbl = da.GetAssessmentInfo(sessionDataRow.FLLWUPBOX_SEQNO, _
                                                                       sessionDataRow.ORIGINALID)
        '2013/06/30 TCS 趙 2013/10対応版 既存流用 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retAssessmentDataTbl:{0}]", retAssessmentDataTbl.Rows.Count))

        Return retAssessmentDataTbl

    End Function

    ''' <summary>
    ''' 保有車両リスト取得
    ''' </summary>
    ''' <param name="sessionDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>保有車両リストを取得する。</remarks>
    Public Function GetVehicleList(ByVal sessionDataTbl As SC3080301DataSet.SC3080301SessionDataTable) As SC3080301DataSet.SC3080301VehicleDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sessionDataTbl:{0}]", sessionDataTbl.Rows.Count))

        Dim retVehicleDataTbl As SC3080301DataSet.SC3080301VehicleDataTable
        Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow

        sessionDataRow = sessionDataTbl.Item(0)

        If (sessionDataRow.CUSTSEGMENT = OrgCustFlg) Then
            '1：自社客
            retVehicleDataTbl = GetOrgVehicleList(sessionDataRow)

        Else
            '2：未取引客
            '未取引客車両情報取得
            retVehicleDataTbl = GetNewVehicleList(sessionDataRow)

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retVehicleDataTbl:{0}]", retVehicleDataTbl.Rows.Count))

        Return retVehicleDataTbl

    End Function

    ''' <summary>
    ''' 自社客の保有車両リスト取得
    ''' </summary>
    ''' <param name="sessionDataRow">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>保有車両リストを取得する。</remarks>
    Private Function GetOrgVehicleList(ByVal sessionDataRow As SC3080301DataSet.SC3080301SessionRow) As SC3080301DataSet.SC3080301VehicleDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sessionDataRow:{0}]", sessionDataRow.ORIGINALID))

        Dim retVehicleDataTbl As SC3080301DataSet.SC3080301VehicleDataTable
        Dim da As New SC3080301TableAdapter

        Dim dataCarNo As String = String.Empty
        Dim dataCarName As String = String.Empty

        ''1：自社客
        'If (sessionDataRow.CUSTCLASS = OrgCustFlg) Then
        '自社客車両情報取得
        retVehicleDataTbl = da.GetOrgVehicle(sessionDataRow.DLRCD, sessionDataRow.ORIGINALID)
        'Else
        '    '副顧客車両情報取得
        '    retVehicleDataTbl = da.GetSubVehicle(sessionDataRow.DLRCD, sessionDataRow.ORIGINALID)
        'End If

        '取得できなかった場合の処理 (例外処理とする)
        If (retVehicleDataTbl.Rows.Count = CountCheckZero) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retVehicleDataTbl:{0}]", retVehicleDataTbl.Rows.Count))

            Return retVehicleDataTbl
        End If

        '検索結果のセット
        For Each dt In retVehicleDataTbl
            If Not dt.IsVCLREGNONull AndAlso Not String.IsNullOrEmpty(Trim(dt.VCLREGNO)) Then
                dataCarNo = dt.VCLREGNO
            End If
            If Not dt.IsMAKERNAMENull AndAlso Not String.IsNullOrEmpty(Trim(dt.MAKERNAME)) Then
                dataCarName = dataCarName & dt.MAKERNAME
            Else
                dataCarName = dataCarName & WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word10)
            End If
            dataCarName = dataCarName & WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word09)
            If Not dt.IsSERIESNAMENull AndAlso Not String.IsNullOrEmpty(Trim(dt.SERIESNAME)) Then
                dataCarName = dataCarName & dt.SERIESNAME
            Else
                dataCarName = dataCarName & WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word10)
            End If
            dt.CARNO = dataCarNo
            dt.CARNAME = dataCarName
            dataCarNo = String.Empty
            dataCarName = String.Empty
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retVehicleDataTbl:{0}]", retVehicleDataTbl.Rows.Count))

        Return retVehicleDataTbl

    End Function

    ''' <summary>
    ''' 未取引客の保有車両リスト取得
    ''' </summary>
    ''' <param name="sessionDataRow">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>保有車両リストを取得する。</remarks>
    Private Function GetNewVehicleList(ByVal sessionDataRow As SC3080301DataSet.SC3080301SessionRow) As SC3080301DataSet.SC3080301VehicleDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sessionDataRow:{0}]", sessionDataRow.ORIGINALID))

        Dim retVehicleDataTbl As SC3080301DataSet.SC3080301VehicleDataTable
        Dim da As New SC3080301TableAdapter

        Dim dataCarNo As String = String.Empty
        Dim dataCarName As String = String.Empty

        '2：未取引客
        '未取引客車両情報取得
        retVehicleDataTbl = da.GetNewVehicle(sessionDataRow.DLRCD, sessionDataRow.ORIGINALID)

        '取得できなかった場合の処理 (例外処理とする)
        If (retVehicleDataTbl.Rows.Count = CountCheckZero) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retVehicleDataTbl:{0}]", retVehicleDataTbl.Rows.Count))

            Return retVehicleDataTbl
        End If

        '検索結果のセット
        For Each dt In retVehicleDataTbl
            If Not dt.IsVCLREGNONull AndAlso Not String.IsNullOrEmpty(Trim(dt.VCLREGNO)) Then
                dataCarNo = dt.VCLREGNO
            End If
            If Not dt.IsMAKERNAMENull AndAlso Not String.IsNullOrEmpty(Trim(dt.MAKERNAME)) Then
                dataCarName = dataCarName & dt.MAKERNAME
            Else
                dataCarName = dataCarName & WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word10)
            End If
            dataCarName = dataCarName & WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word09)
            If Not dt.IsSERIESNAMENull AndAlso Not String.IsNullOrEmpty(Trim(dt.SERIESNAME)) Then
                dataCarName = dataCarName & dt.SERIESNAME
            Else
                dataCarName = dataCarName & WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word10)
            End If
            dt.CARNO = dataCarNo
            dt.CARNAME = dataCarName
            dataCarNo = String.Empty
            dataCarName = String.Empty
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[retVehicleDataTbl:{0}]", retVehicleDataTbl.Rows.Count))

        Return retVehicleDataTbl

    End Function

    ''' <summary>
    ''' 査定登録と通知連携処理
    ''' </summary>
    ''' <param name="status">ステータス(1:査定、2:キャンセル)</param>
    ''' <param name="retention">保有フラグ</param>
    ''' <param name="selectVin">選択車両VIN</param>
    ''' <param name="selectSeqno">選択車両SEQNO</param>
    ''' <param name="selectAssessmentNo">査定No</param>
    ''' <param name="selectNoticeReqId">通知依頼ID</param>
    ''' <param name="sessionDataRow">セッションデータセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>査定情報を登録する処理</remarks>
    <EnableCommit()>
    Public Function RegistUcarAssessment(ByVal status As String, ByVal retention As String, _
                                         ByVal selectVin As String, ByVal selectSeqno As Long, _
                                         ByVal selectAssessmentNo As Long, ByVal selectNoticeReqId As Long, _
                                         ByVal sessionDataRow As SC3080301DataSet.SC3080301SessionRow, _
                                         ByVal ucarTerminalDataTable As SC3080301DataSet.SC3080301UcarTerminalDataTable) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[status:{0}][retention:{1}][selectVin:{2}][selectSeqno:{3}][selectAssessmentNo:{4}][selectNoticeReqId:{5}][sessionDataRow:{6}][ucarTerminalDataTable:{7}]", _
                                  status, retention, selectVin, selectSeqno, selectAssessmentNo, selectNoticeReqId, _
                                  sessionDataRow.ORIGINALID, ucarTerminalDataTable.Rows.Count))

        Dim vinseqno As String = String.Empty
        '連携処理結果
        Dim returnXmlNotice As XmlCommon
        Dim assessmentNo As Long = 0
        Dim returnUpdateResult As Long = 0

        '車両シーケンスNoの設定
        If sessionDataRow.CUSTSEGMENT = OrgCustFlg And retention = RetentionCustomer Then
            vinseqno = selectVin
        ElseIf sessionDataRow.CUSTSEGMENT = NewCustFlg And retention = RetentionCustomer Then
            vinseqno = CStr(selectSeqno)
        End If

        If selectAssessmentNo > CountCheckZero And selectNoticeReqId > CountCheckZero Then

            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            Try
                SC3080301TableAdapter.GetAssesmetLock(selectAssessmentNo)
            Catch ex As OracleExceptionEx
                Return UpdateAssessmentLockError
            End Try
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

            '2013/03/25 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
            Dim da As New SC3080301TableAdapter

            returnUpdateResult = da.UpdateUcarAssessmentInfo(selectAssessmentNo)

            '更新件数が0件の場合、ロールバックしエラーコードを返す
            If returnUpdateResult = 0 Then
                'Updateの失敗
                Me.Rollback = True

                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", UpdateAssessmentError))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

                Return UpdateAssessmentError
            End If
            '2013/03/25 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END

            '通知情報連携
            returnXmlNotice = SetNoticeInfo(status, vinseqno, selectAssessmentNo, selectNoticeReqId, sessionDataRow, ucarTerminalDataTable)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", returnXmlNotice.ResultId))

            Return returnXmlNotice.ResultId

        Else
            '査定情報登録
            assessmentNo = InsertUcarAssessment(retention, selectVin, selectSeqno, sessionDataRow)

            '登録処理が失敗した場合、エラーコードを返す
            If assessmentNo <= CountCheckZero Then
                'Insertの失敗

                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", InsertAssessmentError))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

                Return InsertAssessmentError
            End If

            '通知情報連携
            returnXmlNotice = SetNoticeInfo(status, vinseqno, assessmentNo, 0, sessionDataRow, ucarTerminalDataTable)

            '処理結果が0以外または通知依頼IDが0以下の場合、ロールバックし処理を終了する
            If CLng(returnXmlNotice.ResultId) <> CountCheckZero Or returnXmlNotice.NoticeRequestId <= CountCheckZero Then
                Me.Rollback = True

                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", returnXmlNotice.ResultId))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

                Return returnXmlNotice.ResultId
            End If

            returnUpdateResult = UpdateUcarAssessment(returnXmlNotice.NoticeRequestId, assessmentNo)

            '更新処理が失敗した場合、エラーコードを返す
            If returnUpdateResult = ErrMessageIdUpdateNotice Then
                'Updateの失敗

                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", UpdateAssessmentError))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

                Return UpdateAssessmentError
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", returnXmlNotice.ResultId))

            Return returnXmlNotice.ResultId
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", SuccessIfZero))

        '正常終了
        Return SuccessIfZero

    End Function

    ''' <summary>
    ''' 登録処理
    ''' </summary>
    ''' <param name="retention">保有フラグ</param>
    ''' <param name="selectVin">選択車両VIN</param>
    ''' <param name="selectSeqno">選択車両SEQNO</param>
    ''' <param name="sessionDataRow">セッションデータセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>査定情報を登録する処理</remarks>
    Private Function InsertUcarAssessment(ByVal retention As String, ByVal selectVin As String, _
                                         ByVal selectSeqno As Long, ByVal sessionDataRow As SC3080301DataSet.SC3080301SessionRow) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[retention:{0}][selectVin:{1}][selectSeqno:{2}][sessionDataRow:{3}]", _
                                  retention, selectVin, selectSeqno, sessionDataRow.ORIGINALID))

        Dim da As New SC3080301TableAdapter

        '処理件数確認用
        Dim cnt As Integer = 0

        '査定Noの取得
        Dim assessmentNo As Long
        assessmentNo = da.GetUcarAssessmentseq()
        '査定情報登録
        cnt = da.InsertUcarAssessment(sessionDataRow.FLLWUPBOX_DLRCD, _
                                          sessionDataRow.FLLWUPBOX_STRCD, _
                                          sessionDataRow.FLLWUPBOX_SEQNO, _
                                          sessionDataRow.ORIGINALID, _
                                          sessionDataRow.CUSTCLASS, _
                                          sessionDataRow.CUSTSEGMENT, _
                                          retention, _
                                          selectVin, _
                                          selectSeqno, _
                                          assessmentNo, _
                                          sessionDataRow.UPDATEACCOUNT, _
                                          sessionDataRow.DISPID)

        '登録件数が0件の場合、ロールバックし処理を終了する
        If cnt = CountCheckZero Or assessmentNo <= CountCheckZero Then
            Me.Rollback = True

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", ErrMessageIdInsertNotice))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

            Return ErrMessageIdInsertNotice
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[assessmentNo:{0}]", assessmentNo))

        '正常終了
        Return assessmentNo

    End Function

    ''' <summary>
    ''' 更新処理
    ''' </summary>
    ''' <param name="noticeReqId">査定依頼ID</param>
    ''' <param name="assessmentNo">査定No</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>査定情報を登録する処理</remarks>
    Private Function UpdateUcarAssessment(ByVal noticeReqId As Long, ByVal assessmentNo As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[noticeReqId:{0}][assessmentNo:{1}]", noticeReqId, assessmentNo))

        Dim da As New SC3080301TableAdapter

        '処理件数確認用
        Dim cnt As Integer = 0

        '査定依頼No登録
        cnt = da.UpdateUcarAssessment(noticeReqId, assessmentNo)

        '登録件数が0件の場合、ロールバックし処理を終了する
        If cnt = CountCheckZero Then
            Me.Rollback = True

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", ErrMessageIdUpdateNotice))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

            Return ErrMessageIdUpdateNotice
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[ResultId:{0}]", CountCheckZero))

        '正常終了
        Return CountCheckZero

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Function

    ''' <summary>
    ''' 査定済み端末ID取得
    ''' </summary>
    ''' <param name="noticereqid">査定依頼ID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>端末IDを取得する。</remarks>
    Private Function GetFromClientId(ByVal noticereqid As Long) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                          "_Start[noticeReqId:{0}]", noticereqid))

        Dim retNoticeInfoDataTbl As SC3080301DataSet.SC3080301NoticeInfoDataTable
        Dim da As New SC3080301TableAdapter

        retNoticeInfoDataTbl = da.GetNoticeFromClient(noticereqid)


        '取得できなかった場合の処理 (例外処理とする)
        If (retNoticeInfoDataTbl.Rows.Count = CountCheckZero) OrElse retNoticeInfoDataTbl.Item(0).IsFROMCLIENTIDNull Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[FromClientId:{0}]", String.Empty))

            Return String.Empty
        End If

        Dim noticeInfoRow As SC3080301DataSet.SC3080301NoticeInfoRow = retNoticeInfoDataTbl.Item(0)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[FromClientId:{0}]", noticeInfoRow.FROMCLIENTID))

        Return noticeInfoRow.FROMCLIENTID

    End Function

#End Region

#Region " 通知登録API呼び出し "
    ''' <summary>
    ''' 通知登録
    ''' </summary>
    ''' <param name="status">ステータス(1:査定、2:キャンセル)</param>
    ''' <param name="vinseqno">車両シーケンスNo</param>
    ''' <param name="assessmentno">査定No</param>
    ''' <param name="noticereqid">査定依頼ID</param>
    ''' <param name="sessionDataRow">セッションデータセット (インプット)</param>
    ''' <remarks></remarks>
    Public Function SetNoticeInfo(ByVal status As String, ByVal vinseqno As String, _
                                         ByVal assessmentno As Long, ByVal noticereqid As Long, _
                                         ByVal sessionDataRow As SC3080301DataSet.SC3080301SessionRow, _
                                         ByVal ucarTerminalDataTable As SC3080301DataSet.SC3080301UcarTerminalDataTable) As XmlCommon

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                  "_Start[status:{0}][vinseqno:{1}][assessmentno:{2}][noticereqid:{3}][sessionDataRow:{4}][ucarTerminalDataTable:{5}]", _
                  status, vinseqno, assessmentno, noticereqid, sessionDataRow.ORIGINALID, ucarTerminalDataTable.Rows.Count))

        Dim returnXmlNotice As XmlCommon

        Using noticeData As New XmlNoticeData
            noticeData.TransmissionDate = DateTimeFunc.Now(sessionDataRow.DLRCD)

            Dim noticeFromClientId As String = String.Empty

            If noticereqid > CountCheckZero Then
                noticeFromClientId = GetFromClientId(noticereqid)
                If Not String.IsNullOrEmpty(Trim(noticeFromClientId)) Then
                    For Each dtucarterminal In ucarTerminalDataTable
                        If dtucarterminal.TERMINALID = noticeFromClientId Then
                            noticeFromClientId = dtucarterminal.TERMINALID
                        End If
                    Next
                End If
            End If

            If Not String.IsNullOrEmpty(Trim(noticeFromClientId)) Then
                Using account As New XmlAccount
                    'accountにデータを格納
                    account.ToClientId = noticeFromClientId
                    '格納したデータを親クラスに格納
                    noticeData.AccountList.Add(account)
                End Using
            Else
                For Each dtucarterminal In ucarTerminalDataTable
                    Using account As New XmlAccount
                        'accountにデータを格納
                        account.ToClientId = dtucarterminal.TERMINALID             '受信先の端末ID（受信先）
                        '格納したデータを親クラスに格納
                        noticeData.AccountList.Add(account)
                    End Using
                Next
            End If

            Using requestNotice As New XmlRequestNotice
                'requestNoticeにデータを格納
                '汎用（i-CROPへ送信する場合）
                requestNotice.DealerCode = sessionDataRow.DLRCD                         '販売店コード
                requestNotice.StoreCode = sessionDataRow.STRCD                          '店舗コード
                requestNotice.RequestClass = RequestClass                               '依頼種別
                requestNotice.Status = status                                           'ステータス
                requestNotice.RequestId = noticereqid                                   '依頼種別ID
                requestNotice.RequestClassId = assessmentno                             '依頼ID
                requestNotice.FromAccount = sessionDataRow.STAFFCD                      'スタッフコード（送信元）
                requestNotice.FromAccountName = sessionDataRow.STAFFNAME                'スタッフ名（送信元）
                requestNotice.CustomId = sessionDataRow.ORIGINALID                      '顧客コード
                requestNotice.CustomName = sessionDataRow.CUSTOMERNAME                  '顧客名
                requestNotice.CustomerClass = sessionDataRow.CUSTCLASS                  '顧客分類
                requestNotice.CustomerKind = sessionDataRow.CUSTSEGMENT                 '顧客種別
                requestNotice.SalesStaffCode = sessionDataRow.STAFFCD                   '顧客担当スタッフコード
                requestNotice.VehicleSequenceNumber = vinseqno                          '車両シーケンスNo
                requestNotice.FollowUpBoxStoreCode = sessionDataRow.FLLWUPBOX_STRCD     'Follow-up Box販売店コード
                requestNotice.FollowUpBoxNumber = sessionDataRow.FLLWUPBOX_SEQNO        'Follow-up Box内連番

                noticeData.RequestNotice = requestNotice
            End Using

            Using pushInfo As New XmlPushInfo
                Dim wordDispContents As String
                Dim wordTableNo As String
                'pushInfoにデータを格納
                pushInfo.PushCategory = PushCategory                           'カテゴリータイプ
                pushInfo.PositionType = PositionType                           '表示位置
                pushInfo.DisplayType = DisplayType                             '表示タイプ
                If status = CancelStatus Then
                    wordDispContents = WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word12).Replace("{0}", sessionDataRow.STAFFNAME)
                    wordDispContents = wordDispContents.Replace("{1}", sessionDataRow.CUSTOMERNAME)
                Else
                    wordDispContents = WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word11).Replace("{0}", sessionDataRow.STAFFNAME)
                    wordDispContents = wordDispContents.Replace("{1}", sessionDataRow.CUSTOMERNAME)
                End If
                If Not sessionDataRow.IsSALESTABLENONull AndAlso sessionDataRow.SALESTABLENO > 0 Then
                    wordTableNo = WebWordUtility.GetWord(AssessmentDisplayId, SC3080301Word13).Replace("{0}", CStr(sessionDataRow.SALESTABLENO))
                    wordDispContents = wordDispContents.Replace("{2}", wordTableNo)
                Else
                    wordDispContents = wordDispContents.Replace("{2}", "")
                End If
                pushInfo.DisplayContents = wordDispContents                          '表示内容

                noticeData.PushInfo = pushInfo
            End Using

            'ロジックを呼ぶ
            Using noticeInfo As New IC3040801BusinessLogic

                'i-CROPへ送信
                returnXmlNotice = noticeInfo.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & _
                  "_End[Message:{0}][NoticeRequestId:{1}][ResultId:{2}]", returnXmlNotice.Message, returnXmlNotice.NoticeRequestId.ToString(CultureInfo.CurrentCulture), returnXmlNotice.ResultId))

            Return returnXmlNotice
        End Using

    End Function

#End Region

End Class
