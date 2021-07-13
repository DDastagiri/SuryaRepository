Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.DataAccess.IC3040402.IC3040402DataSet
Imports Toyota.eCRB.iCROP.DataAccess.IC3040402.IC3040402DataSetTableAdapters
Imports Toyota.eCRB.iCROP.DataAccess.IC3040402

Public Class IC3040402BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

    Private dtSheduleInfo As IC3040402DataSet.IC3040402ScheduleInfoDataTable

    Private dtXMLCommon As IC3040402DataSet.IC3040402XMLCommonDataTable
    Private dtXMLScheduleInfo As IC3040402DataSet.IC3040402XMLScheduleInfoDataTable
    Private dtXMLSchedule As IC3040402DataSet.IC3040402XMLScheduleDataTable
    Private dtXMLAlarm As IC3040402DataSet.IC3040402XMLAlarmDataTable

    Private drXMLCommon As IC3040402DataSet.IC3040402XMLCommonRow
    Private drXMLScheduleInfo As IC3040402DataSet.IC3040402XMLScheduleInfoRow
    Private drXMLSchedule As IC3040402DataSet.IC3040402XMLScheduleRow
    Private drXMLAlarm As IC3040402DataSet.IC3040402XMLAlarmRow

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM As String = "IC3040402"

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="doc">スケジュール情報(XML)</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal doc As Xml.XmlDocument)

        Dim xDataList As XmlNodeList
        Dim xCommonList As XmlNodeList
        Dim xScheduleInfoList As XmlNodeList
        Dim xScheduleList As XmlNodeList
        Dim xAlarmList As XmlNodeList

        dtSheduleInfo = New IC3040402DataSet.IC3040402ScheduleInfoDataTable
        dtXMLCommon = New IC3040402DataSet.IC3040402XMLCommonDataTable
        dtXMLScheduleInfo = New IC3040402DataSet.IC3040402XMLScheduleInfoDataTable
        dtXMLSchedule = New IC3040402DataSet.IC3040402XMLScheduleDataTable
        dtXMLAlarm = New IC3040402DataSet.IC3040402XMLAlarmDataTable

        If Not doc Is Nothing Then

            'ルート要素から親リストを取得する
            xDataList = doc.GetElementsByTagName("Detail")

            For Each xElement As XmlElement In xDataList

                drXMLCommon = dtXMLCommon.NewIC3040402XMLCommonRow()
                drXMLScheduleInfo = dtXMLScheduleInfo.NewIC3040402XMLScheduleInfoRow()
                drXMLSchedule = dtXMLSchedule.NewIC3040402XMLScheduleRow()
                drXMLAlarm = dtXMLAlarm.NewIC3040402XMLAlarmRow()

                'Common要素リストを取り出す
                xCommonList = xElement.GetElementsByTagName("Common")

                'Common要素の子要素を取り出す
                If xCommonList.Count > 0 Then
                    For Each xCommon As XmlElement In xCommonList
                        drXMLCommon.DealerCode = GetXmlValue(xCommon.GetElementsByTagName("DealerCode"))
                        drXMLCommon.BranchCode = GetXmlValue(xCommon.GetElementsByTagName("BranchCode"))
                        drXMLCommon.ScheduleDiv = GetXmlValue(xCommon.GetElementsByTagName("ScheduleDiv"))
                        drXMLCommon.ScheduleID = GetXmlValue(xCommon.GetElementsByTagName("ScheduleID"))
                        drXMLCommon.ActionType = GetXmlValue(xCommon.GetElementsByTagName("ActionType"))
                        drXMLCommon.ActivityCreateStaff = GetXmlValue(xCommon.GetElementsByTagName("ActivityCreateStaff"))
                    Next xCommon
                End If


                'ScheduleInfo要素リストを取り出す
                xScheduleInfoList = xElement.GetElementsByTagName("ScheduleInfo")

                'ScheduleInfo要素の子要素を取り出す
                If xScheduleInfoList.Count > 0 Then
                    For Each xScheduleInfo As XmlElement In xScheduleInfoList
                        drXMLScheduleInfo.CustomerDiv = GetXmlValue(xScheduleInfo.GetElementsByTagName("CustomerDiv"))
                        drXMLScheduleInfo.CustomerCode = GetXmlValue(xScheduleInfo.GetElementsByTagName("CustomerCode"))
                        drXMLScheduleInfo.DmsID = GetXmlValue(xScheduleInfo.GetElementsByTagName("DmsID"))
                        drXMLScheduleInfo.CustomerName = GetXmlValue(xScheduleInfo.GetElementsByTagName("CustomerName"))
                        drXMLScheduleInfo.ReceptionDiv = GetXmlValue(xScheduleInfo.GetElementsByTagName("ReceptionDiv"))
                        drXMLScheduleInfo.ServiceCode = GetXmlValue(xScheduleInfo.GetElementsByTagName("ServiceCode"))
                        drXMLScheduleInfo.MerchandiseCd = GetXmlValue(xScheduleInfo.GetElementsByTagName("MerchandiseCd"))
                        drXMLScheduleInfo.StrStatus = GetXmlValue(xScheduleInfo.GetElementsByTagName("StrStatus"))
                        drXMLScheduleInfo.RezStatus = GetXmlValue(xScheduleInfo.GetElementsByTagName("RezStatus"))
                        drXMLScheduleInfo.CompletionDiv = GetXmlValue(xScheduleInfo.GetElementsByTagName("CompletionDiv"))
                        drXMLScheduleInfo.CompletionDate = GetXmlValue(xScheduleInfo.GetElementsByTagName("CompletionDate"))
                        drXMLScheduleInfo.DeleteDate = GetXmlValue(xScheduleInfo.GetElementsByTagName("DeleteDate"))
                    Next xScheduleInfo

                End If

                'Schedule要素リストを取り出す
                xScheduleList = xElement.GetElementsByTagName("Schedule")

                'Schedule要素の子要素を取り出す
                If xScheduleList.Count > 0 Then
                    For Each xSchedule As XmlElement In xScheduleList
                        drXMLSchedule.ParentDiv = GetXmlValue(xSchedule.GetElementsByTagName("ParentDiv"))
                        drXMLSchedule.CreateScheduleDiv = GetXmlValue(xSchedule.GetElementsByTagName("CreateScheduleDiv"))
                        drXMLSchedule.ActivityStaffBranchCode = GetXmlValue(xSchedule.GetElementsByTagName("ActivityStaffBranchCode"))
                        drXMLSchedule.ActivityStaffCode = GetXmlValue(xSchedule.GetElementsByTagName("ActivityStaffCode"))
                        drXMLSchedule.ReceptionStaffBranchCode = GetXmlValue(xSchedule.GetElementsByTagName("ReceptionStaffBranchCode"))
                        drXMLSchedule.ReceptionStaffCode = GetXmlValue(xSchedule.GetElementsByTagName("ReceptionStaffCode"))
                        drXMLSchedule.ContactNo = GetXmlValue(xSchedule.GetElementsByTagName("ContactNo"))
                        drXMLSchedule.Summary = GetXmlValue(xSchedule.GetElementsByTagName("Summary"))
                        drXMLSchedule.StartTime = GetXmlValue(xSchedule.GetElementsByTagName("StartTime"))
                        drXMLSchedule.EndTime = GetXmlValue(xSchedule.GetElementsByTagName("EndTime"))
                        drXMLSchedule.Memo = GetXmlValue(xSchedule.GetElementsByTagName("Memo"))
                        drXMLSchedule.XiCropColor = GetXmlValue(xSchedule.GetElementsByTagName("XiCropColor"))
                        drXMLSchedule.TodoID = GetXmlValue(xSchedule.GetElementsByTagName("ToDoID"))

                        'Alarm要素リストを取り出す
                        xAlarmList = xSchedule.GetElementsByTagName("Alarm")

                        If xAlarmList.Count > 0 Then

                            'Alarmリストの子要素を取り出す
                            For Each xAlarm As XmlElement In xAlarmList
                                drXMLAlarm.Trigger = GetXmlValue(xAlarm.GetElementsByTagName("Trigger"))

                                'スケジュール情報を登録
                                RegistScheduleData(drXMLCommon, drXMLScheduleInfo, drXMLSchedule, drXMLAlarm)

                            Next xAlarm
                        Else
                            'スケジュール情報を登録
                            RegistScheduleData(drXMLCommon, drXMLScheduleInfo, drXMLSchedule, drXMLAlarm)
                        End If
                    Next xSchedule
                Else
                    'スケジュール情報を登録
                    If xCommonList.Count > 0 Then
                        RegistScheduleData(drXMLCommon, drXMLScheduleInfo, drXMLSchedule, drXMLAlarm)
                    End If
                End If

            Next xElement

        End If

    End Sub

    ''' <summary>
    ''' Commonタグの値を取得
    ''' </summary>
    ''' <param name="xmlList">Commonタグリスト</param>
    ''' <return>Commonタグ情報</return>
    ''' <remarks></remarks>
    Public Function GetXmlValue(ByVal xmlList As XmlNodeList) As String

        Dim xmlValue As String = ""

        If Not xmlList Is Nothing Then
            If Not xmlList(0) Is Nothing AndAlso Not xmlList(0).FirstChild Is Nothing Then
                xmlValue = xmlList(0).FirstChild.Value
            End If
        End If

        Return xmlValue

    End Function

    ''' <summary>
    ''' 登録、更新エラー情報を一時退避する。
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="schedulediv">スケジュール区分</param>
    ''' <param name="scheduleid">スケジュールID</param>
    ''' <param name="reason">未登録理由(1:セールススタッフ、サービススタッフが未設定 2:登録、更新エラー 3:XML設定値エラー)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EvacuationScheduleInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal scheduleDiv As String, _
                                           ByVal scheduleId As String, _
                                           ByVal reason As String) As Boolean

        '戻り値
        Dim blnRegistResult As Boolean = True

        '登録対象のスケジュール情報を取得する
        Dim registInfo As IC3040402DataSet.IC3040402ScheduleInfoRow() = dtSheduleInfo.Select("DealerCode = '" & dealerCode & "' AND " & _
                                                                                             "BranchCode = '" & branchCode & "' AND " & _
                                                                                             "ScheduleDiv = '" & scheduleDiv & "' AND " & _
                                                                                             "ScheduleID = '" & scheduleId & "'")

        '未登録スケジュール情報を登録
        Using da As New IC3040402ScheduleDataSetTableAdapters
            '既存データの削除（エラーの場合同一のデータが溜まってしまうため
            da.DeleteUnregistScheduleInfo(dealerCode, branchCode, scheduleDiv, scheduleId)
            For Each registInfoRow In registInfo

                'スケジュールID連番を取得
                Dim sequenceIdSeqno As Integer = 0
                Try
                    sequenceIdSeqno = da.SelectScheduleIdSeqnoMax(dealerCode, branchCode, scheduleDiv, scheduleId)

                Catch ex As Exception

                    Logger.Error(C_SYSTEM & " " & "Error SelectScheduleIdSeqnoMax()" & ":" & _
                    "(DLRCD = " & dealerCode & "," & _
                    "STRCD = " & branchCode & "," & _
                    "SCHEDULEDIV = " & scheduleDiv & "," & _
                    "SCHEDULEID = " & registInfoRow.ScheduleID & "," & _
                    "SCHEDULEID_SEQNO = " & scheduleId & ")")
                    Return True

                End Try

                '未登録スケジュールテーブルにレコードを追加
                Try
                    blnRegistResult = da.InsertUnregistScheduleInfo(registInfoRow, sequenceIdSeqno, reason)
                    If blnRegistResult = False Then
                        Return blnRegistResult
                    End If

                Catch ex As Exception

                    'ログ
                    Logger.Error(C_SYSTEM & " " & "Error InsertUnregistScheduleInfo()" & ":" & _
                    "(DLRCD = " & registInfoRow.DealerCode & "," & _
                    "STRCD = " & registInfoRow.BranchCode & "," & _
                    "SCHEDULEDIV = " & registInfoRow.ScheduleDiv & "," & _
                    "SCHEDULEID = " & registInfoRow.ScheduleID & "," & _
                    "SCHEDULEID_SEQNO = " & sequenceIdSeqno & "," & _
                    "UNREGIST_REASON = " & reason & "," & _
                    "ACTIONTYPE = " & registInfoRow.ActionType & "," & _
                    "COMPLETEFLG = " & registInfoRow.CompletionDiv & "," & _
                    "COMPLETEDATE = " & registInfoRow.CompletionDate & "," & _
                    "ACTCREATESTAFFCD = " & registInfoRow.ActivityCreateStaff & "," & _
                    "ACTSTAFFSTRCD = " & registInfoRow.ActivityStaffBranchCode & "," & _
                    "ACTSTAFFCD = " & registInfoRow.ActivityStaffCode & "," & _
                    "RECSTAFFSTRCD = " & registInfoRow.ReceptionStaffBranchCode & "," & _
                    "RECSTAFFCD = " & registInfoRow.ReceptionStaffCode & "," & _
                    "CUSTDIV = " & registInfoRow.CustomerDiv & "," & _
                    "CUSTID = " & registInfoRow.CustomerCode & "," & _
                    "CUSTNAME = " & registInfoRow.CustomerName & "," & _
                    "DMSID = " & registInfoRow.DmsID & "," & _
                    "RECEPTIONDIV = " & registInfoRow.ReceptionDiv & "," & _
                    "SERVICECODE = " & registInfoRow.ServiceCode & "," & _
                    "MERCHANDISECD = " & registInfoRow.MerchandiseCd & "," & _
                    "STRSTATUS = " & registInfoRow.StrStatus & "," & _
                    "REZSTATUS = " & registInfoRow.RezStatus & "," & _
                    "PARENTDIV = " & registInfoRow.ParentDiv & "," & _
                    "REGISTFLG = " & registInfoRow.CreateScheduleDiv & "," & _
                    "CONTACTNO = " & registInfoRow.ContactNo & "," & _
                    "SUMMARY = " & registInfoRow.Summary & "," & _
                    "STARTTIME = " & registInfoRow.StartTime & "," & _
                    "ENDTIME = " & registInfoRow.EndTime & "," & _
                    "MEMO = " & registInfoRow.Memo & "," & _
                    "BACKGROUNDCOLOR = " & registInfoRow.XiCropColor & "," & _
                    "ALARMNO = " & registInfoRow.Trigger & "," & _
                    "TODOID = " & registInfoRow.TodoID & "," & _
                    "DELETEDATE = " & registInfoRow.DeleteDate & ")")
                    Return True
                End Try

            Next registInfoRow

            ' 結果を返却
            Return blnRegistResult

        End Using

    End Function

    ''' <summary>
    ''' Disposeメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        'Dispose(True)
        ' This object will be cleaned up by the Dispose method.
        ' Therefore, you should call GC.SupressFinalize to
        ' take this object off the finalization queue 
        ' and prevent finalization code for this object
        ' from executing a second time.
        Dispose(True)
        GC.SuppressFinalize(Me)

    End Sub

    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

        If disposing Then

            dtSheduleInfo.Dispose()
            dtXMLCommon.Dispose()
            dtXMLScheduleInfo.Dispose()
            dtXMLSchedule.Dispose()
            dtXMLAlarm.Dispose()

            dtSheduleInfo = Nothing
            dtXMLCommon = Nothing
            dtXMLScheduleInfo = Nothing
            dtXMLSchedule = Nothing
            dtXMLAlarm = Nothing

        End If

    End Sub

    ''' <summary>
    ''' XMLデータをクラス内保持用のDataTableに登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Function RegistScheduleData(ByVal xmlCommonRow As IC3040402DataSet.IC3040402XMLCommonRow, _
                                        ByVal xmlScheduleInfoRow As IC3040402DataSet.IC3040402XMLScheduleInfoRow, _
                                        ByVal xmlScheduleRow As IC3040402DataSet.IC3040402XMLScheduleRow, _
                                        ByVal xmlAlarmRow As IC3040402DataSet.IC3040402XMLAlarmRow) As Boolean

        Dim DealerCode As String = ""
        Dim BranchCode As String = ""
        Dim ScheduleID As String = ""
        Dim ScheduleDiv As String = ""

        DealerCode = xmlCommonRow.DealerCode
        BranchCode = xmlCommonRow.BranchCode
        ScheduleID = xmlCommonRow.ScheduleID
        ScheduleDiv = xmlCommonRow.ScheduleDiv

        Dim dr As IC3040402DataSet.IC3040402ScheduleInfoRow = dtSheduleInfo.NewIC3040402ScheduleInfoRow()

        'Commonタグ
        dr.DealerCode = xmlCommonRow.DealerCode                      '販売店コード
        dr.BranchCode = xmlCommonRow.BranchCode                      '店舗コード
        dr.ScheduleDiv = xmlCommonRow.ScheduleDiv                    'スケジュール区分
        dr.ScheduleID = xmlCommonRow.ScheduleID                      'スケジュールID
        dr.ActionType = xmlCommonRow.ActionType                      '処理区分
        dr.ActivityCreateStaff = xmlCommonRow.ActivityCreateStaff    '活動作成スタッフコード

        'ScheduleInfoタグ
        dr.CustomerDiv = xmlScheduleInfoRow.CustomerDiv              '顧客区分
        dr.CustomerCode = xmlScheduleInfoRow.CustomerCode            '顧客コード
        dr.DmsID = xmlScheduleInfoRow.DmsID                          'DMSID
        dr.CustomerName = xmlScheduleInfoRow.CustomerName            '顧客名
        dr.ReceptionDiv = xmlScheduleInfoRow.ReceptionDiv            '受付納車区分
        dr.ServiceCode = xmlScheduleInfoRow.ServiceCode              'サービスコード
        dr.MerchandiseCd = xmlScheduleInfoRow.MerchandiseCd          '商品コード
        dr.StrStatus = xmlScheduleInfoRow.StrStatus                  '入庫ステータス
        dr.RezStatus = xmlScheduleInfoRow.RezStatus                  '予約ステータス
        dr.CompletionDiv = xmlScheduleInfoRow.CompletionDiv          '完了フラグ
        dr.CompletionDate = xmlScheduleInfoRow.CompletionDate        '完了日
        dr.DeleteDate = xmlScheduleInfoRow.DeleteDate                '削除日

        'Scheduleタグ
        dr.ParentDiv = xmlScheduleRow.ParentDiv                                 '親子フラグ(1:親、2:子)
        dr.CreateScheduleDiv = xmlScheduleRow.CreateScheduleDiv                 'スケジュール作成区分
        dr.ActivityStaffBranchCode = xmlScheduleRow.ActivityStaffBranchCode     '活動担当スタッフ店舗コード
        dr.ActivityStaffCode = xmlScheduleRow.ActivityStaffCode                 '活動担当スタッフコード
        dr.ReceptionStaffBranchCode = xmlScheduleRow.ReceptionStaffBranchCode   '受付担当スタッフ店舗コード
        dr.ReceptionStaffCode = xmlScheduleRow.ReceptionStaffCode               '受付担当スタッフコード
        dr.ContactNo = xmlScheduleRow.ContactNo                                 '接触方法No
        dr.Summary = xmlScheduleRow.Summary                                     'タイトル
        dr.StartTime = xmlScheduleRow.StartTime                                 '開始時間
        dr.EndTime = xmlScheduleRow.EndTime                                     '終了時間
        dr.Memo = xmlScheduleRow.Memo                                           'メモ
        dr.XiCropColor = xmlScheduleRow.XiCropColor                             '色設定
        dr.TodoID = xmlScheduleRow.TodoID                                       'ToDoID

        'Alarmタグ
        dr.Trigger = xmlAlarmRow.Trigger                             'アラーム起動タイミング

        'クラス内に保持しておくDataTableにデータを追加する
        dtSheduleInfo.Rows.Add(dr)

        '未登録スケジュール情報にキーが一致するレコードが存在すれば、削除する
        Using da As New IC3040402ScheduleDataSetTableAdapters
            Try
                da.DeleteUnregistScheduleInfo(DealerCode, BranchCode, ScheduleDiv, ScheduleID)
                Return True

            Catch ex As Exception

                Logger.Error(C_SYSTEM & " " & "Error DeleteUnregistScheduleInfo()" & ":" & _
                             "(DLRCD = " & DealerCode & "," & _
                             "STRCD = " & BranchCode & "," & _
                             "SCHEDULEDIV = " & ScheduleDiv & "," & _
                             "SCHEDULEID = " & ScheduleID & ")")
                Return False
            End Try
        End Using

    End Function

End Class