Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.IC3040403
Imports Toyota.eCRB.iCROP.DataAccess.IC3040403.ConstCode
Imports Toyota.eCRB.iCROP.BizLogic.IC3040402
Imports System.Xml

'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040403BusinessLogic.vb
'─────────────────────────────────────
'機能： ｶﾚﾝﾀﾞｰ情報登録ｲﾝﾀｰﾌｪｰｽ
'補足： 
'更新： $01 2016/09/02 SKFC二村  TR-V4-TMT-20150516-131  ログ追加対応
'更新：     2019/02/28 SKFC二村  TR-V4-TMT-20190131-001
'更新：     2019/02/14 SKFC上田  TKM UAT-0182
'更新：     2019/05/08 SKFC上田  （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 (TR-SLT-FTMS-20181219-001)
'更新：     2020/06/17 SKFC二村  TR-SLT-TKM-20200206-001横展(カレンダーIDの特定から販売店コード、店舗コードを除外)
'─────────────────────────────────────

Namespace IC3040403BizLogic
    ''' <summary>
    ''' プログラムID:IC3040403
    ''' 処理概要:カレンダー情報登録インターフェース
    ''' 英名:Calender Register Interface
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3040403BusinessLogic
        Inherits BaseBusinessComponent

        Implements IDisposable

#Region "定数"
        ' XML宣言
        Private Const Xml_Version As String = "1.0"
        Private Const Xml_Encoding As String = "UTF-8"

        ' 値チェック用
        Private Const EmptyString As String = ""
        Private Const ZeroString As String = "0"
        Private Const OneString As String = "1"
        Private Const TwoString As String = "2"
        Private Const ThreeString As String = "3"
        Private Const FourString As String = "4"
        Private Const FiveString As String = "5"
        Private Const SixString As String = "6"
        Private Const SevenString As String = "7"
        Private Const EightString As String = "8"
        Private Const NineString As String = "9"
        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
        Private Const BlankString As String = " "
        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END

#End Region

#Region "Public関数"

        ''' <summary>
        ''' Head要素内にある要素を専用のクラスに格納します。
        ''' </summary>
        ''' <param name="registScheduleClone">RegistSchedule要素以下の要素</param>
        ''' <param name="xmlDataClass">取得した要素を格納したクラス</param>
        ''' <returns>取得した要素を格納したクラス</returns>
        ''' <remarks></remarks>
        Public Function GetHeadElementValue(ByVal registScheduleClone As XmlNode, ByVal xmlDataClass As XmlRegistSchedule) As XmlRegistSchedule

            Dim headXml As XmlNode = GetChildNode(registScheduleClone, XmlNameHead, DataAssignment.ModeMandatory, ElementName.Head).CloneNode(True)
            xmlDataClass.MessageId = GetNodeInnerText(headXml, XmlNameMessageId, DataAssignment.ModeMandatory, 0, TypeConversion.StringType, ElementName.MessageID)
            xmlDataClass.CountryCode = GetNodeInnerText(headXml, XmlNameCountryCode, DataAssignment.ModeMandatory, 0, TypeConversion.StringType, ElementName.CountryCode)
            xmlDataClass.LinkSystemCode = GetNodeInnerText(headXml, XmlNameLinkSystemCode, DataAssignment.ModeMandatory, 0, TypeConversion.StringType, ElementName.LinkSystemCode)
            xmlDataClass.TransmissionDate = GetNodeInnerText(headXml, XmlNameTransmissionDate, DataAssignment.ModeMandatory, 0, TypeConversion.DateType, ElementName.TransmissionDate)

            GetNodeInnerText(headXml, XmlNameMessageId, DataAssignment.ModeMandatory, 9, TypeConversion.StringType, ElementName.MessageID)
            GetNodeInnerText(headXml, XmlNameCountryCode, DataAssignment.ModeMandatory, 2, TypeConversion.StringType, ElementName.CountryCode)
            GetNodeInnerText(headXml, XmlNameLinkSystemCode, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.LinkSystemCode)
            GetNodeInnerText(headXml, XmlNameTransmissionDate, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.TransmissionDate)

            ' メッセージＩＤが別の場合、エラーとする
            If Not Validation.Equals(xmlDataClass.MessageId, TrueMessageId) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.MessageID)

            End If

            Return xmlDataClass

        End Function

        ''' <summary>
        ''' Detail要素内にある要素を専用のクラスに格納します。
        ''' </summary>
        ''' <param name="detailClone">Detail要素以下の要素</param>
        ''' <param name="xmlDetailClass">取得した要素を格納したクラス</param>
        ''' <returns>取得した要素を格納したクラス</returns>
        ''' <remarks></remarks>
        Public Function GetDetailElementValue(ByVal detailClone As XmlNode, ByVal xmlDetailClass As XmlDetail)

            ' Common要素内を取得します。
            xmlDetailClass = GetCommonElementValue(detailClone, xmlDetailClass)

            ' ScheduleInfo要素内を取得します。
            xmlDetailClass = GetScheduleInfoElementValue(detailClone, xmlDetailClass)

            ' XMLScheduleクラスをNewする。
            xmlDetailClass.InitialScheduleList()

            ' 処理区分が登録であり、完了区分が無い時または、イベント追加の場合、scheduleは必須項目となる
            If (IsFlgEquals(xmlDetailClass.ActionType, ActionType.Entry) AndAlso _
                           (xmlDetailClass.CompletionDiv Is Nothing Or _
                            IsFlgEquals(xmlDetailClass.CompletionDiv, CompletionFlg.FlgNotContinue))) Or _
                           IsFlgEquals(xmlDetailClass.ActionType, ActionType.AddEvent) Then

                For Each scheduleXml As XmlNode In GetChildNode(detailClone, XmlNameScheduleName, DataAssignment.ModeMandatory, True, ElementName.Schedule)

                    Dim xmlDataScheduleClassMan As New XmlSchedule()

                    ' Schedule要素内を取得します。
                    xmlDataScheduleClassMan = GetScheduleElementValue(scheduleXml.CloneNode(True), xmlDataScheduleClassMan, xmlDetailClass)

                    xmlDetailClass.ScheduleList.Add(xmlDataScheduleClassMan)

                Next

            Else
                ' それ以外は全てオプションとなる
                For Each scheduleXml As XmlNode In GetChildNode(detailClone, XmlNameScheduleName, DataAssignment.ModeOptional, True, ElementName.Schedule)

                    Dim xmlDataScheduleClassOption As New XmlSchedule()

                    ' Schedule要素内を取得します。
                    xmlDataScheduleClassOption = GetScheduleElementValue(scheduleXml.CloneNode(True), xmlDataScheduleClassOption, xmlDetailClass)

                    xmlDetailClass.ScheduleList.Add(xmlDataScheduleClassOption)

                Next

            End If

            Return xmlDetailClass

        End Function

        ''' <summary>
        ''' 受注後工程Head要素内にある要素を専用のクラスに格納します。
        ''' </summary>
        ''' <param name="RegistAfterOrderScheduleClone">RegistAfterOrderSchedule要素以下の要素</param>
        ''' <param name="xmlDataClass">取得した要素を格納したクラス</param>
        ''' <returns>取得した要素を格納したクラス</returns>
        ''' <remarks></remarks>
        ''' <history>2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START</history>
        Public Function GetAfterOrderHeadElementValue(ByVal RegistAfterOrderScheduleClone As XmlNode, _
                                                      ByVal xmlDataClass As XmlRegistAfterOrderSchedule) As XmlRegistAfterOrderSchedule

            Dim headXml As XmlNode = GetChildNode(RegistAfterOrderScheduleClone, XmlNameHead, DataAssignment.ModeMandatory, ElementName.Head).CloneNode(True)
            xmlDataClass.MessageId = GetNodeInnerText(headXml, XmlNameMessageId, DataAssignment.ModeMandatory, 0, TypeConversion.StringType, ElementName.MessageId)
            xmlDataClass.CountryCode = GetNodeInnerText(headXml, XmlNameCountryCode, DataAssignment.ModeMandatory, 0, TypeConversion.StringType, ElementName.CountryCode)
            xmlDataClass.LinkSystemCode = GetNodeInnerText(headXml, XmlNameLinkSystemCode, DataAssignment.ModeMandatory, 0, TypeConversion.StringType, ElementName.LinkSystemCode)
            xmlDataClass.TransmissionDate = GetNodeInnerText(headXml, XmlNameTransmissionDate, DataAssignment.ModeMandatory, 0, TypeConversion.DateType, ElementName.TransmissionDate)

            GetNodeInnerText(headXml, XmlNameMessageId, DataAssignment.ModeMandatory, 9, TypeConversion.StringType, ElementName.MessageId)
            GetNodeInnerText(headXml, XmlNameCountryCode, DataAssignment.ModeMandatory, 2, TypeConversion.StringType, ElementName.CountryCode)
            GetNodeInnerText(headXml, XmlNameLinkSystemCode, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.LinkSystemCode)
            GetNodeInnerText(headXml, XmlNameTransmissionDate, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.TransmissionDate)

            ' メッセージＩＤが別の場合、エラーとする
            If Not Validation.Equals(xmlDataClass.MessageId, TrueMessageId) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.MessageId)

            End If

            Return xmlDataClass

        End Function

        ''' <summary>
        ''' 受注後工程Detail要素内にある要素を専用のクラスに格納します。
        ''' </summary>
        ''' <param name="detailClone">Detail要素以下の要素</param>
        ''' <param name="xmlAfterOrderDetailClass">取得した要素を格納したクラス</param>
        ''' <returns>取得した要素を格納したクラス</returns>
        ''' <remarks></remarks>
        ''' <history>2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START</history>
        Public Function GetAfterOrderDetailElementValue(ByVal detailClone As XmlNode, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail)

            ' Common要素内を取得する
            xmlAfterOrderDetailClass = GetAfterOrderCommonElementValue(detailClone, xmlAfterOrderDetailClass)

            ' ScheduleInfo要素内を取得する
            xmlAfterOrderDetailClass = GetAfterOrderScheduleInfoElementValue(detailClone, xmlAfterOrderDetailClass)

            ' XMLScheduleクラスをNewする
            xmlAfterOrderDetailClass.InitialScheduleList()

            ' 処理区分が登録である場合、scheduleは必須項目となる
            If IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Entry) Then

                For Each scheduleXml As XmlNode In GetChildNode(detailClone, XmlNameScheduleName, DataAssignment.ModeMandatory, True, ElementName.Schedule)

                    Dim xmlAfterOrderDataScheduleClassMan As New XmlAfterOrderSchedule()

                    ' Schedule要素内を取得する
                    xmlAfterOrderDataScheduleClassMan = GetAfterOrderScheduleElementValue(scheduleXml.CloneNode(True), xmlAfterOrderDataScheduleClassMan, xmlAfterOrderDetailClass)

                    xmlAfterOrderDetailClass.ScheduleList.Add(xmlAfterOrderDataScheduleClassMan)

                Next

            Else
                ' それ以外は全てオプションとなる
                For Each scheduleXml As XmlNode In GetChildNode(detailClone, XmlNameScheduleName, DataAssignment.ModeOptional, True, ElementName.Schedule)

                    Dim xmlAfterOrderDataScheduleClassOption As New XmlAfterOrderSchedule()

                    ' Schedule要素内を取得する
                    xmlAfterOrderDataScheduleClassOption = GetAfterOrderScheduleElementValue(scheduleXml.CloneNode(True), xmlAfterOrderDataScheduleClassOption, xmlAfterOrderDetailClass)

                    xmlAfterOrderDetailClass.ScheduleList.Add(xmlAfterOrderDataScheduleClassOption)

                Next

            End If

            Return xmlAfterOrderDetailClass

        End Function



        ''' <summary>
        ''' 子ノードを取得します
        ''' </summary>
        ''' <param name="parentsNode">親ノード</param>
        ''' <param name="childNodeName">子ノード名</param>
        ''' <param name="dataAssignmentMode">要素の割り当て状態</param>
        ''' <param name="elementCode">エラー出力用の要素コード</param>
        ''' <returns>子ノードのXMLNodeList</returns>
        ''' <remarks></remarks>
        Public Function GetChildNode(ByVal parentsNode As XmlNode, ByVal childNodeName As String, ByVal dataAssignmentMode As Integer, ByVal elementCode As Integer) As XmlNode

            Dim getChildNodes As XmlNodeList = GetChildNode(parentsNode, childNodeName, dataAssignmentMode, False, elementCode)

            Return getChildNodes.Item(0)

        End Function

        ''' <summary>
        ''' 子ノードを取得します
        ''' </summary>
        ''' <param name="parentsNode">親ノード</param>
        ''' <param name="childNodeName">子ノード名</param>
        ''' <param name="dataAssignmentMode">要素の割り当て状態</param>
        ''' <param name="canMultiple">同名の子ノードを複数を許す場合はTrueとします</param>
        ''' <param name="elementCode">エラー出力用の要素コード</param>
        ''' <returns>子ノードのXMLNodeList</returns>
        ''' <remarks></remarks>
        Public Function GetChildNode(ByVal parentsNode As XmlNode, ByVal childNodeName As String, ByVal dataAssignmentMode As Integer, ByVal canMultiple As Boolean, ByVal elementCode As Integer) As XmlNodeList

            ' 子ノードが存在するか確認します。
            Dim childNodesCount As Integer = parentsNode.SelectNodes(XmlRootDirectry + childNodeName).Count

            If childNodesCount = 0 Then
                'ノードが存在しなかった場合

                Select Case dataAssignmentMode

                    Case DataAssignment.ModeMandatory

                        ' 必須項目に対してノードが存在しないのでエラー
                        Throw New ApplicationException(CType(ReturnCode.NotXmlElementError + elementCode, String))

                    Case DataAssignment.ModeOptional

                        ' オプション項目なのでノードが存在しなくても問題ない
                        Return parentsNode.SelectNodes(XmlRootDirectry + childNodeName)

                    Case Else

                        ' 想定外の値が設定された場合、オプション項目として扱います。
                        Return parentsNode.SelectNodes(XmlRootDirectry + childNodeName)

                End Select

            ElseIf childNodesCount = 1 Then
                ' ノードが存在する場合、要素を取得します。
                Select Case dataAssignmentMode

                    Case DataAssignment.ModeMandatory

                        ' 必須項目なので存在しなければならない
                        Return parentsNode.SelectNodes(XmlRootDirectry + childNodeName)


                    Case DataAssignment.ModeOptional

                        ' オプション項目は存在して構わない
                        Return parentsNode.SelectNodes(XmlRootDirectry + childNodeName)

                    Case Else

                        ' 想定外の値が設定された場合、オプション項目として扱います。
                        Return parentsNode.SelectNodes(XmlRootDirectry + childNodeName)

                End Select

            Else
                ' ノードが複数あるのが許される場合
                If canMultiple Then

                    Return parentsNode.SelectNodes(XmlRootDirectry + childNodeName)

                End If

                Throw New ApplicationException(CType(ReturnCode.ManyXmlElementError + elementCode, String))

            End If

        End Function

        ''' <summary>
        ''' 処理区分、完了フラグから、DBに掛ける処理を判別します。
        ''' </summary>
        ''' <param name="detailData"></param>
        ''' <remarks></remarks>
        <EnableCommit()>
        Public Sub ProcessingDataBase(ByVal detailData As XmlDetail)
            '$01 Add Start
            Logger.Info("IC3040403 ProcessingDataBase() Start")
            '$01 Add End
            Try

                ' スタッフコードリスト
                Dim staffCodeList As List(Of String) = New List(Of String)
                '$01 Add Start
                Logger.Info("IC3040403 detailData.ActionType:" + detailData.ActionType)
                '$01 Add End
                ' 処理区分により、処理を分岐させる
                Select Case CType(detailData.ActionType, Integer)

                    Case ActionType.Entry
                        '$01 Add Start
                        Logger.Info("IC3040403 ActionType.Entry")
                        '$01 Add End
                        ' 処理区分が登録（１）の場合
                        staffCodeList = EntryDataBase(detailData, staffCodeList)

                    Case ActionType.Update
                        '$01 Add Start
                        Logger.Info("IC3040403 ActionType.Update")
                        '$01 Add End
                        ' 処理区分が更新（２）の場合
                        staffCodeList = UpdateDataBase(detailData, staffCodeList)

                    Case ActionType.AddEvent
                        '$01 Add Start
                        Logger.Info("IC3040403 ActionType.AddEvent")
                        '$01 Add End
                        ' 処理区分がイベント追加（３）の場合
                        staffCodeList = EventDataBase(detailData, staffCodeList)

                End Select

                ' 今回使用したスタッフコードをカレンダーアドレス最終更新日テーブルに反映します
                SetStaffCode(staffCodeList, detailData)
                '$01 Add Start
                Logger.Info("IC3040403 ProcessingDataBase() Normal End")
                '$01 Add End
            Catch ex As ApplicationException
                '$01 Add Start
                Logger.Info("IC3040403 ProcessingDataBase() Error End")
                '$01 Add End
                Rollback = True
                Throw

            End Try

        End Sub
        ''' <summary>
        ''' 処理区分から、DBに掛ける処理を判別します。(受注後工程)
        ''' </summary>
        ''' <param name="detailData"></param>
        ''' <remarks></remarks>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        <EnableCommit()>
        Public Sub ProcessingAfterOrderDataBase(ByVal detailData As XmlAfterOrderDetail)
            '$01 Add Start
            Logger.Info("IC3040403 ProcessingAfterOrderDataBase() Start")
            '$01 Add End
            Try

                ' スタッフコードリスト
                Dim staffCodeList As List(Of String) = New List(Of String)

                '$01 Add Start
                Logger.Info("IC3040403 detailData.ActionType:" + detailData.ActionType)
                '$01 Add End
                ' 処理区分により、処理を分岐させる
                Select Case CType(detailData.ActionType, Integer)

                    Case ActionType.Entry
                        '$01 Add Start
                        Logger.Info("IC3040403 ActionType.Entry")
                        '$01 Add End
                        ' 処理区分が登録（１）の場合
                        staffCodeList = EntryAfterOrderDataBase(detailData, staffCodeList)

                    Case ActionType.Update
                        '$01 Add Start
                        Logger.Info("IC3040403 ActionType.Update")
                        '$01 Add End
                        ' 処理区分が更新（２）の場合
                        staffCodeList = UpdateAfterOrderDataBase(detailData, staffCodeList)

                        ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 START
                    Case ActionType.Delete
                        '$01 Add Start
                        Logger.Info("IC3040403 ActionType.Delete")
                        '$01 Add End
                        ' 処理区分が削除（４）の場合
                        staffCodeList = DeleteAfterOrderDataBase(detailData, staffCodeList)
                        ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 END
                End Select

                ' 今回使用したスタッフコードをカレンダーアドレス最終更新日テーブルに反映する
                SetAfterOrderStaffCode(staffCodeList, detailData)
                '$01 Add Start
                Logger.Info("IC3040403 ProcessingAfterOrderDataBase() Normal End")
                '$01 Add End
            Catch ex As ApplicationException
                '$01 Add Start
                Logger.Info("IC3040403 ProcessingAfterOrderDataBase() Error End")
                '$01 Add End
                Rollback = True
                Throw

            End Try

        End Sub


        ''' <summary>
        ''' Disposeメソッド
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

            If disposing Then

            End If

        End Sub

#End Region
#Region "Private関数"

        ''' <summary>
        ''' Common要素内にある要素を専用のクラスに格納します。
        ''' </summary>
        ''' <param name="detailXml">Detail要素以下の部分のみのXML</param>
        ''' <param name="xmlDetailClass">取得した要素を格納するクラス</param>
        ''' <remarks></remarks>
        Private Function GetCommonElementValue(ByVal detailXml As XmlNode, ByVal xmlDetailClass As XmlDetail) As XmlDetail

            Dim commonXml As XmlNode = GetChildNode(detailXml, XmlNameCommonName, DataAssignment.ModeMandatory, ElementName.Head).CloneNode(True)
            xmlDetailClass.DealerCode = GetNodeInnerText(commonXml, XmlNameDealerCode, DataAssignment.ModeOptional, 0, TypeConversion.StringType, ElementName.DealerCode)
            xmlDetailClass.BranchCode = GetNodeInnerText(commonXml, XmlNameBranchCode, DataAssignment.ModeOptional, 0, TypeConversion.StringType, ElementName.BranchCode)
            xmlDetailClass.ScheduleDiv = GetNodeInnerText(commonXml, XmlNameScheduleDiv, DataAssignment.ModeOptional, 0, TypeConversion.StringType, ElementName.ScheduleDiv)
            xmlDetailClass.ScheduleId = GetNodeInnerText(commonXml, XmlNameScheduleId, DataAssignment.ModeOptional, 0, TypeConversion.StringType, ElementName.ScheduleID)

            GetNodeInnerText(commonXml, XmlNameDealerCode, DataAssignment.ModeMandatory, 5, TypeConversion.StringType, ElementName.DealerCode)
            GetNodeInnerText(commonXml, XmlNameBranchCode, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.BranchCode)
            GetNodeInnerText(commonXml, XmlNameScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ScheduleDiv)
            '2013/07/17 SKFC 森 既存流用 Start
            'GetNodeInnerText(commonXml, XmlNameScheduleId, DataAssignment.ModeMandatory, 10, TypeConversion.IntegerType, ElementName.ScheduleId)
            GetNodeInnerText(commonXml, XmlNameScheduleId, DataAssignment.ModeMandatory, 20, TypeConversion.IntegerType, ElementName.ScheduleId)
            '2013/07/17 SKFC 森 既存流用 End
            xmlDetailClass.ActionType = GetNodeInnerText(commonXml, XmlNameActionType, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ActionType)
            xmlDetailClass.ActivityCreateStaff = GetNodeInnerText(commonXml, XmlNameActivityCreateStaff, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.ActivityCreateStaff)

            '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
            '' スケジュール区分が規定値以外の場合
            ''2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
            ''If Not (IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Or _
            ''       IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion) _
            ''        ) Then
            'If Not (IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Or _
            'IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion) Or _
            'IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.ReceivedProcess) _
            ') Then
            '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
            If Not (IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Or _
            IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion)) Then
                '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.ScheduleDiv)

            End If

            ' 処理区分が規定値以外の場合
            If Not (IsFlgEquals(xmlDetailClass.ActionType, ActionType.Entry) Or _
                IsFlgEquals(xmlDetailClass.ActionType, ActionType.Update) Or _
                    IsFlgEquals(xmlDetailClass.ActionType, ActionType.AddEvent)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.ActionType)

            End If

            Return xmlDetailClass

        End Function

        ''' <summary>
        ''' 受注後工程Common要素内にある要素を専用のクラスに格納します。(受注後工程)
        ''' </summary>
        ''' <param name="detailXml">Detail要素以下の部分のみのXML</param>
        ''' <param name="xmlAfterOrderDetailClass">取得した要素を格納するクラス</param>
        '''<history>2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function GetAfterOrderCommonElementValue(ByVal detailXml As XmlNode, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderDetail

            Dim commonXml As XmlNode = GetChildNode(detailXml, XmlNameCommonName, DataAssignment.ModeMandatory, ElementName.Head).CloneNode(True)
            xmlAfterOrderDetailClass.DealerCode = GetNodeInnerText(commonXml, XmlNameDealerCode, DataAssignment.ModeOptional, 0, TypeConversion.StringType, ElementName.DealerCode)
            xmlAfterOrderDetailClass.BranchCode = GetNodeInnerText(commonXml, XmlNameBranchCode, DataAssignment.ModeOptional, 0, TypeConversion.StringType, ElementName.BranchCode)
            xmlAfterOrderDetailClass.ScheduleId = GetNodeInnerText(commonXml, XmlNameScheduleId, DataAssignment.ModeOptional, 0, TypeConversion.StringType, ElementName.ScheduleId)

            GetNodeInnerText(commonXml, XmlNameDealerCode, DataAssignment.ModeMandatory, 5, TypeConversion.StringType, ElementName.DealerCode)
            GetNodeInnerText(commonXml, XmlNameBranchCode, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.BranchCode)
            GetNodeInnerText(commonXml, XmlNameScheduleId, DataAssignment.ModeMandatory, 20, TypeConversion.IntegerType, ElementName.ScheduleId)
            xmlAfterOrderDetailClass.ActionType = GetNodeInnerText(commonXml, XmlNameActionType, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ActionType)
            xmlAfterOrderDetailClass.ActivityCreateStaff = GetNodeInnerText(commonXml, XmlNameActivityCreateStaff, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.ActivityCreateStaff)


            ' 処理区分が規定値以外の場合
            ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 START
            '            If Not (IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Entry) Or _
            '    IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Update)) Then

            If Not (IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Entry) Or _
                IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Update) Or _
                IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Delete)) Then
                ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 END
                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.ActionType)

            End If


            Return xmlAfterOrderDetailClass

        End Function


        ''' <summary>
        ''' ScheduleInfo要素内にある要素を専用のクラスに格納します。
        ''' </summary>
        ''' <param name="detailXml">XML</param>
        ''' <param name="xmlDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <remarks></remarks>
        Private Function GetScheduleInfoElementValue(ByVal detailXml As XmlNode, ByVal xmlDetailClass As XmlDetail) As XmlDetail

            Dim scheduleInfoClone As XmlNode = Nothing

            ' 処理区分によって、ScheduleInfo要素の条件を変更する
            If IsFlgEquals(xmlDetailClass.ActionType, ActionType.Entry) Then
                ' 登録処理であれば、ScheduleInfo要素は必須項目となる
                scheduleInfoClone = GetChildNode(detailXml, XmlNameScheduleInfoName, DataAssignment.ModeMandatory, ElementName.ScheduleInfo).CloneNode(True)
            Else
                ' 更新、またはEvent追加の場合、オプション設定となる。
                Dim scheduleInfoXml As XmlNode = GetChildNode(detailXml, XmlNameScheduleInfoName, DataAssignment.ModeOptional, ElementName.ScheduleInfo)

                If scheduleInfoXml IsNot Nothing Then

                    scheduleInfoClone = scheduleInfoXml.CloneNode(True)

                End If

            End If

            ' スケジュール要素が存在しなかった場合
            If scheduleInfoClone Is Nothing Then
                ' 値がない場合はフラグをFlaseにし、終了する。
                xmlDetailClass.ScheduleInfoFlg = False
                Return xmlDetailClass
            Else

                xmlDetailClass.ScheduleInfoFlg = True

            End If

            ' 完了区分の取得
            xmlDetailClass.CompletionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.CompletionDiv)

            ' 完了区分が空文字である場合、Nothingに置き換える
            If Validation.Equals(xmlDetailClass.CompletionDiv, EmptyString) Then

                xmlDetailClass.CompletionDiv = Nothing

            End If

            ' 処理区分によって分岐
            If IsFlgEquals(xmlDetailClass.ActionType, ActionType.Entry) Then
                ' 処理区分が登録の場合
                xmlDetailClass = EntryGetScheduleInfoElementValue(scheduleInfoClone, xmlDetailClass)

            ElseIf IsFlgEquals(xmlDetailClass.ActionType, ActionType.Update) Then

                ' 更新の場合
                xmlDetailClass = UpdateGetScheduleInfoElementValue(scheduleInfoClone, xmlDetailClass)
            Else
                ' イベント追加の場合、全てがオプション扱いとなる
                xmlDetailClass = EventAddGetScheduleInfoElementValue(scheduleInfoClone, xmlDetailClass)

            End If

            ' 取得した値の入力チェックを行います
            CheckScheduleInfoElementValue(xmlDetailClass)

            Return xmlDetailClass

        End Function

        ''' <summary>
        ''' ScheduleInfo要素内にある要素を専用のクラスに格納します。(受注後工程)
        ''' </summary>
        ''' <param name="detailXml">XML</param>
        ''' <param name="xmlAfterOrderDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <history>2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function GetAfterOrderScheduleInfoElementValue(ByVal detailXml As XmlNode, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderDetail

            Dim scheduleInfoClone As XmlNode = Nothing

            ' 処理区分によって、ScheduleInfo要素の条件を変更する
            If IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Entry) Then
                ' 登録処理であれば、ScheduleInfo要素は必須項目となる
                scheduleInfoClone = GetChildNode(detailXml, XmlNameScheduleInfoName, DataAssignment.ModeMandatory, ElementName.ScheduleInfo).CloneNode(True)
                ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 START
                'Else
            ElseIf IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Update) Or _
                   IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Delete) Then
                '' 更新の場合、オプション設定となる
                ' 更新または削除の場合、オプション設定となる
                Dim scheduleInfoXml As XmlNode = GetChildNode(detailXml, XmlNameScheduleInfoName, DataAssignment.ModeOptional, ElementName.ScheduleInfo)

                If scheduleInfoXml IsNot Nothing Then

                    scheduleInfoClone = scheduleInfoXml.CloneNode(True)

                End If

            End If

            ' スケジュール要素が存在しなかった場合
            If scheduleInfoClone Is Nothing Then
                ' 値がない場合はフラグをFlaseにし、終了する
                xmlAfterOrderDetailClass.ScheduleInfoFlg = False
                Return xmlAfterOrderDetailClass
            Else

                xmlAfterOrderDetailClass.ScheduleInfoFlg = True

            End If

            ' 処理区分によって分岐
            If IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Entry) Then
                ' 処理区分が登録の場合
                xmlAfterOrderDetailClass = EntryGetAfterOrderScheduleInfoElementValue(scheduleInfoClone, xmlAfterOrderDetailClass)

            ElseIf IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Update) Then

                ' 処理区分が更新の場合
                xmlAfterOrderDetailClass = UpdateGetAfterOrderScheduleInfoElementValue(scheduleInfoClone, xmlAfterOrderDetailClass)

                ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 START
                '処理区分が削除の場合
            Else
                xmlAfterOrderDetailClass = DeleteGetAfterOrderScheduleInfoElementValue(scheduleInfoClone, xmlAfterOrderDetailClass)
                ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 END
            End If

            ' 取得した値の入力チェックを行う
            CheckAfterOrderScheduleInfoElementValue(xmlAfterOrderDetailClass)

            Return xmlAfterOrderDetailClass

        End Function



        ''' <summary>
        ''' ScheduleInfo要素内の、処理区分が「登録」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleInfoClone">XML</param>
        ''' <param name="xmlDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <remarks></remarks>
        Private Function EntryGetScheduleInfoElementValue(ByVal scheduleInfoClone As XmlNode, ByVal xmlDetailClass As XmlDetail) As XmlDetail

            If IsFlgEquals(xmlDetailClass.CompletionDiv, CompletionFlg.FlgActivityCompleted) Then
                ' 完了区分が完了の場合
                xmlDetailClass.CustomerDiv = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.CustomerDiv)
                '2013/07/17 SKFC 森 既存流用 Start
                'xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 19, TypeConversion.StringType, ElementName.CustomerCode)
                xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.CustomerCode)
                '2013/07/17 SKFC 森 既存流用 End
                xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsId)
                xmlDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
                xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
                '2014/07/11 SKFC 渡邊 NextStep CalDAV Start
                '2013/07/17 SKFC 森 既存流用 Start
                'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
                'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 20, TypeConversion.IntegerType, ElementName.ServiceCode)
                xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.IntegerType, ElementName.ServiceCode)
                '2013/07/17 SKFC 森 既存流用 End
                '2014/07/11 SKFC 渡邊 NextStep CalDAV End
                xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCD, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCD)
                xmlDetailClass.StrStatus = GetNodeInnerText(scheduleInfoClone, XmlNameStrStatus, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.StrStatus)
                xmlDetailClass.RezStatus = GetNodeInnerText(scheduleInfoClone, XmlNameRezStatus, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.RezStatus)
                xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.CompletionDate)
                xmlDetailClass.DeleteDate = GetNodeInnerText(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)
            Else
                ' 完了区分が完了以外の場合
                xmlDetailClass.CustomerDiv = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CustomerDiv)
                '2013/07/17 SKFC 森 既存流用 Start
                'xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeMandatory, 19, TypeConversion.StringType, ElementName.CustomerCode)
                xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.CustomerCode)
                '2013/07/17 SKFC 森 既存流用 End
                xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsId)
                xmlDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeMandatory, 256, TypeConversion.StringType, ElementName.CustomerName)

                ' スケジュール区分により、受付納車区分の項目の扱いが変化
                If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion) Then
                    ' 入庫予約
                    xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
                Else
                    ' 来店予約
                    xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
                End If

                '2014/07/11 SKFC 渡邊 NextStep CalDAV Start
                '2013/07/17 SKFC 森 既存流用 Start
                'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
                'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 20, TypeConversion.IntegerType, ElementName.ServiceCode)
                xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.IntegerType, ElementName.ServiceCode)
                '2013/07/17 SKFC 森 既存流用 End
                '2014/07/11 SKFC 渡邊 NextStep CalDAV End
                xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCD, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCD)
                xmlDetailClass.StrStatus = GetNodeInnerText(scheduleInfoClone, XmlNameStrStatus, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.StrStatus)
                xmlDetailClass.RezStatus = GetNodeInnerText(scheduleInfoClone, XmlNameRezStatus, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.RezStatus)
                ' 完了区分がContinueの場合、完了日は必須項目
                If IsFlgEquals(xmlDetailClass.CompletionDiv, CompletionFlg.FlgContinue) Then
                    xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.CompletionDate)
                Else
                    xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.CompletionDate)
                End If
                xmlDetailClass.DeleteDate = GetNodeInnerText(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            End If

            Return xmlDetailClass

        End Function

        ''' <summary>
        ''' ScheduleInfo要素内の、処理区分が「登録」の場合の要素を取得するメソッド(受注後工程)
        ''' </summary>
        ''' <param name="scheduleInfoClone">XML</param>
        ''' <param name="xmlAfterOrderDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <history>2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function EntryGetAfterOrderScheduleInfoElementValue(ByVal scheduleInfoClone As XmlNode, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderDetail

            xmlAfterOrderDetailClass.CustomerDiv = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CustomerDiv)
            xmlAfterOrderDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.CustomerCode)
            xmlAfterOrderDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsId)
            '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START
            xmlAfterOrderDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeMandatory, 256, TypeConversion.StringType, ElementName.CustomerName)
            '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 END
            xmlAfterOrderDetailClass.DeleteDate = GetNodeInnerText(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            Return xmlAfterOrderDetailClass

        End Function


        ''' <summary>
        ''' ScheduleInfo要素内の、処理区分が「更新」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleInfoClone">XML</param>
        ''' <param name="xmlDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <remarks></remarks>
        Private Function UpdateGetScheduleInfoElementValue(ByVal scheduleInfoClone As XmlNode, ByVal xmlDetailClass As XmlDetail) As XmlDetail

            xmlDetailClass.CustomerDiv = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.CustomerDiv)
            '2013/07/17 SKFC 森 既存流用 Start
            'xmlDetailClass.CustomerCode = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 19, TypeConversion.StringType, ElementName.CustomerCode)
            xmlDetailClass.CustomerCode = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.CustomerCode)
            '2013/07/17 SKFC 森 既存流用 End
            xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsId)
            xmlDetailClass.CustomerName = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
            ' 受付納車区分はスケジュール区分が入庫予約の場合は空欄は許されない
            If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion) Then

                xmlDetailClass.ReceptionDiv = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)

            Else
                ' 入庫予約でない場合は空欄（削除）を許す
                xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
            End If
            '2014/07/11 SKFC 渡邊 NextStep CalDAV Start
            '2013/07/17 SKFC 森 既存流用 Start
            'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
            'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 20, TypeConversion.IntegerType, ElementName.ServiceCode)
            xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.IntegerType, ElementName.ServiceCode)
            '2013/07/17 SKFC 森 既存流用 End
            '2014/07/11 SKFC 渡邊 NextStep CalDAV End
            xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCD, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCD)
            xmlDetailClass.StrStatus = GetNodeInnerText(scheduleInfoClone, XmlNameStrStatus, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.StrStatus)
            xmlDetailClass.RezStatus = GetNodeInnerText(scheduleInfoClone, XmlNameRezStatus, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.RezStatus)
            xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.CompletionDate)
            xmlDetailClass.DeleteDate = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            Return xmlDetailClass

        End Function

        ''' <summary>
        ''' ScheduleInfo要素内の、処理区分が「更新」の場合の要素を取得するメソッド(受注後工程)
        ''' </summary>
        ''' <param name="scheduleInfoClone">XML</param>
        ''' <param name="xmlAfterOrderDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <history>2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function UpdateGetAfterOrderScheduleInfoElementValue(ByVal scheduleInfoClone As XmlNode, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderDetail

            '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 START
            'xmlAfterOrderDetailClass.CustomerDiv = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CustomerDiv)
            'xmlAfterOrderDetailClass.CustomerCode = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.CustomerCode)
            xmlAfterOrderDetailClass.CustomerDiv = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.CustomerDiv)
            xmlAfterOrderDetailClass.CustomerCode = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.CustomerCode)
            '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 END
            '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START
            xmlAfterOrderDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
            '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 END
            xmlAfterOrderDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsId)
            xmlAfterOrderDetailClass.DeleteDate = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            Return xmlAfterOrderDetailClass

        End Function


        ''' <summary>
        ''' ScheduleInfo要素内の、処理区分が「イベント追加」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleInfoClone">XML</param>
        ''' <param name="xmlDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <remarks></remarks>
        Private Function EventAddGetScheduleInfoElementValue(ByVal scheduleInfoClone As XmlNode, ByVal xmlDetailClass As XmlDetail) As XmlDetail

            xmlDetailClass.CustomerDiv = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.CustomerDiv)
            '2013/07/17 SKFC 森 既存流用 Start
            'xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 19, TypeConversion.StringType, ElementName.CustomerCode)
            xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.CustomerCode)
            '2013/07/17 SKFC 森 既存流用 End
            xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsId)
            xmlDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
            xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
            '2014/07/11 SKFC 渡邊 NextStep CalDAV Start
            '2013/07/17 SKFC 森 既存流用 Start
            'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
            'xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 20, TypeConversion.IntegerType, ElementName.ServiceCode)
            xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.IntegerType, ElementName.ServiceCode)
            '2013/07/17 SKFC 森 既存流用 End
            '2014/07/11 SKFC 渡邊 NextStep CalDAV End
            xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCD, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCD)
            xmlDetailClass.StrStatus = GetNodeInnerText(scheduleInfoClone, XmlNameStrStatus, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.StrStatus)
            xmlDetailClass.RezStatus = GetNodeInnerText(scheduleInfoClone, XmlNameRezStatus, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.RezStatus)
            xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.CompletionDate)
            xmlDetailClass.DeleteDate = GetNodeInnerText(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            Return xmlDetailClass

        End Function
        ''' <summary>
        ''' ScheduleInfo要素内の、処理区分が「削除」の場合の要素を取得するメソッド(受注後工程)
        ''' </summary>
        ''' <param name="scheduleInfoClone">XML</param>
        ''' <param name="xmlAfterOrderDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <history>2014/07/23 SKFC渡邊 NEXTSTEP_CALDAV 仕様変更 START</history>
        ''' <remarks></remarks>
        Private Function DeleteGetAfterOrderScheduleInfoElementValue(ByVal scheduleInfoClone As XmlNode, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderDetail

            xmlAfterOrderDetailClass.CustomerDiv = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.CustomerDiv)
            xmlAfterOrderDetailClass.CustomerCode = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.CustomerCode)
            xmlAfterOrderDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
            xmlAfterOrderDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsId)
            xmlAfterOrderDetailClass.DeleteDate = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            Return xmlAfterOrderDetailClass

        End Function

        ''' <summary>
        ''' ScheduleInfo要素内から取得した値が、正常値であるかチェックします
        ''' </summary>
        ''' <param name="xmlDetailClass">取得した値が格納されているデータ格納クラス</param>
        ''' <remarks></remarks>
        Public Sub CheckScheduleInfoElementValue(ByVal xmlDetailClass As XmlDetail)

            ' 顧客区分が規定されている値で無い場合
            If Not (xmlDetailClass.CustomerDiv Is Nothing Or _
                Validation.Equals(xmlDetailClass.CustomerDiv, EmptyString) Or _
                Validation.Equals(xmlDetailClass.CustomerDiv, ZeroString) Or _
                Validation.Equals(xmlDetailClass.CustomerDiv, OneString) Or _
                Validation.Equals(xmlDetailClass.CustomerDiv, TwoString)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.CustomerDiv)

            End If

            ' 受付納車区分が規定されている値で無い場合
            If Not (xmlDetailClass.ReceptionDiv Is Nothing Or _
                Validation.Equals(xmlDetailClass.ReceptionDiv, EmptyString) Or _
                Validation.Equals(xmlDetailClass.ReceptionDiv, ZeroString) Or _
                Validation.Equals(xmlDetailClass.ReceptionDiv, OneString) Or _
                Validation.Equals(xmlDetailClass.ReceptionDiv, TwoString) Or _
                Validation.Equals(xmlDetailClass.ReceptionDiv, ThreeString) Or _
                Validation.Equals(xmlDetailClass.ReceptionDiv, FourString)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.ReceptionDiv)

            End If

            ' 入庫ステータスが規定されている値でない場合
            If Not (xmlDetailClass.StrStatus Is Nothing Or _
                Validation.Equals(xmlDetailClass.StrStatus, EmptyString) Or _
                Validation.Equals(xmlDetailClass.StrStatus, ZeroString) Or _
                Validation.Equals(xmlDetailClass.StrStatus, OneString)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.StrStatus)

            End If

            ' 予約ステータスが規定されている値でない場合
            If Not (xmlDetailClass.RezStatus Is Nothing Or _
                Validation.Equals(xmlDetailClass.RezStatus, EmptyString) Or _
                Validation.Equals(xmlDetailClass.RezStatus, OneString) Or _
                Validation.Equals(xmlDetailClass.RezStatus, TwoString) Or _
                Validation.Equals(xmlDetailClass.RezStatus, ThreeString) Or _
                Validation.Equals(xmlDetailClass.RezStatus, FourString)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.RezStatus)

            End If

            ' 完了区分が規定されている値で無い場合
            If Not (xmlDetailClass.CompletionDiv Is Nothing Or _
                    IsFlgEquals(xmlDetailClass.CompletionDiv, CompletionFlg.FlgNotContinue) Or _
                     IsFlgEquals(xmlDetailClass.CompletionDiv, CompletionFlg.FlgContinue) Or _
                      IsFlgEquals(xmlDetailClass.CompletionDiv, CompletionFlg.FlgActivityCompleted)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.CompletionDiv)
            End If

        End Sub

        ''' <summary>
        ''' ScheduleInfo要素内から取得した値が、正常値であるかチェックします(受注後工程)
        ''' </summary>
        ''' <param name="xmlAFterOrderDetailClass">取得した値が格納されているデータ格納クラス</param>
        ''' <history>2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Public Sub CheckAfterOrderScheduleInfoElementValue(ByVal xmlAFterOrderDetailClass As XmlAfterOrderDetail)

            ' 顧客区分が規定されている値で無い場合
            If Not (xmlAFterOrderDetailClass.CustomerDiv Is Nothing Or _
                Validation.Equals(xmlAFterOrderDetailClass.CustomerDiv, EmptyString) Or _
                Validation.Equals(xmlAFterOrderDetailClass.CustomerDiv, ZeroString) Or _
                Validation.Equals(xmlAFterOrderDetailClass.CustomerDiv, OneString) Or _
                Validation.Equals(xmlAFterOrderDetailClass.CustomerDiv, TwoString)) Then
                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.CustomerDiv)

            End If

        End Sub


        ''' <summary>
        ''' Schedule要素内にある要素を専用のクラスに格納します。
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlScheduleClass">取得した要素を格納するクラス</param>
        ''' <param name="xmlDetailClass">Detail要素以下のデータが格納された変数</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <remarks></remarks>
        Private Function GetScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlScheduleClass As XmlSchedule, ByVal xmlDetailClass As XmlDetail) As XmlSchedule

            xmlScheduleClass.InitialAlarmTriggerList()

            If IsFlgEquals(xmlDetailClass.ActionType, ActionType.Entry) And (Not IsFlgEquals(xmlDetailClass.CompletionDiv, CompletionFlg.FlgActivityCompleted)) Then
                ' 登録の場合
                xmlScheduleClass = EntryGetScheduleElementValue(scheduleClone, xmlScheduleClass, xmlDetailClass)

            ElseIf IsFlgEquals(xmlDetailClass.ActionType, ActionType.Update) And (xmlDetailClass.DeleteDate Is Nothing Or Validation.Equals(xmlDetailClass.DeleteDate, EmptyString)) Then
                ' 更新処理
                xmlScheduleClass = UpdateGetScheduleElementValue(scheduleClone, xmlScheduleClass, xmlDetailClass)

            ElseIf IsFlgEquals(xmlDetailClass.ActionType, ActionType.AddEvent) Then
                ' Event追加
                '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
                'xmlScheduleClass = EventAddGetScheduleElementValue(scheduleClone, xmlScheduleClass)
                xmlScheduleClass = EventAddGetScheduleElementValue(scheduleClone, xmlScheduleClass, xmlDetailClass)
                '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START

            End If

            ' スケジュール作成区分が規定されている値で無い場合
            If Not (xmlScheduleClass.CreateScheduleDiv Is Nothing Or _
                     Validation.Equals(xmlScheduleClass.CreateScheduleDiv, EmptyString) Or _
                     Validation.Equals(xmlScheduleClass.CreateScheduleDiv, OneString) Or _
                     Validation.Equals(xmlScheduleClass.CreateScheduleDiv, TwoString) Or _
                      Validation.Equals(xmlScheduleClass.CreateScheduleDiv, ThreeString)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.CreateScheduleDiv)

            End If

            For Each alarm As String In xmlScheduleClass.AlarmTriggerList

                ' アラーム起動タイミングが規定されている値で無い場合
                If Not (Validation.Equals(alarm, EmptyString) Or _
                            Validation.Equals(alarm, OneString) Or _
                            Validation.Equals(alarm, TwoString) Or _
                            Validation.Equals(alarm, ThreeString) Or _
                            Validation.Equals(alarm, FourString) Or _
                            Validation.Equals(alarm, FiveString) Or _
                            Validation.Equals(alarm, SixString) Or _
                            Validation.Equals(alarm, SevenString) Or _
                            Validation.Equals(alarm, EightString) Or _
                            Validation.Equals(alarm, NineString)) Then

                    ' エラーをThrowする
                    Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.Alarm)

                End If

            Next

            Return xmlScheduleClass

        End Function

        ''' <summary>
        ''' Schedule要素内にある要素を専用のクラスに格納します。(受注後工程)
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlAfterOrderScheduleClass">取得した要素を格納するクラス</param>
        ''' <param name="xmlAfterOrderDetailClass">Detail要素以下のデータが格納された変数</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <history>2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function GetAfterOrderScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlAfterOrderScheduleClass As XmlAfterOrderSchedule, _
                                                           ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderSchedule

            xmlAfterOrderScheduleClass.InitialAlarmTriggerList()

            If IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Entry) Then
                ' 登録の場合
                xmlAfterOrderScheduleClass = EntryGetAfterOrderScheduleElementValue(scheduleClone, xmlAfterOrderScheduleClass, xmlAfterOrderDetailClass)
            ElseIf IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Update) And (xmlAfterOrderDetailClass.DeleteDate Is Nothing Or Validation.Equals(xmlAfterOrderDetailClass.DeleteDate, EmptyString)) Then
                ' 更新処理
                xmlAfterOrderScheduleClass = UpdateGetAfterOrderScheduleElementValue(scheduleClone, xmlAfterOrderScheduleClass, xmlAfterOrderDetailClass)
                ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 START
                ' 削除処理
            ElseIf IsFlgEquals(xmlAfterOrderDetailClass.ActionType, ActionType.Delete) Then
                xmlAfterOrderScheduleClass = DeleteGetAfterOrderScheduleElementValue(scheduleClone, xmlAfterOrderScheduleClass, xmlAfterOrderDetailClass)
                ' 2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 END
            End If

            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
            ' スケジュール作成区分が規定されている値で無い場合
            'If Not (xmlAfterOrderScheduleClass.CreateScheduleDiv Is Nothing Or _
            '   Validation.Equals(xmlAfterOrderScheduleClass.CreateScheduleDiv, TwoString)) Then
            If Not (xmlAfterOrderScheduleClass.CreateScheduleDiv Is Nothing Or _
                Validation.Equals(xmlAfterOrderScheduleClass.CreateScheduleDiv, OneString) Or _
                Validation.Equals(xmlAfterOrderScheduleClass.CreateScheduleDiv, TwoString)) Then

                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.CreateScheduleDiv)

            End If

            For Each alarm As String In xmlAfterOrderScheduleClass.AlarmTriggerList

                ' アラーム起動タイミングが規定されている値で無い場合
                If Not (Validation.Equals(alarm, EmptyString) Or _
                            Validation.Equals(alarm, OneString) Or _
                            Validation.Equals(alarm, TwoString) Or _
                            Validation.Equals(alarm, ThreeString) Or _
                            Validation.Equals(alarm, FourString) Or _
                            Validation.Equals(alarm, FiveString) Or _
                            Validation.Equals(alarm, SixString) Or _
                            Validation.Equals(alarm, SevenString) Or _
                            Validation.Equals(alarm, EightString) Or _
                            Validation.Equals(alarm, NineString)) Then

                    ' エラーをThrowする
                    Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.Alarm)

                End If

            Next

            Return xmlAfterOrderScheduleClass

        End Function


        ''' <summary>
        ''' Schedule要素内の、処理区分が「登録」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlScheduleClass">取得した要素を格納するクラス</param>
        ''' <param name="xmlDetailClass">Detail要素以下のデータが格納された変数</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <remarks></remarks>
        Private Function EntryGetScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlScheduleClass As XmlSchedule, ByVal xmlDetailClass As XmlDetail) As XmlSchedule

            xmlScheduleClass.CreateScheduleDiv = GetNodeInnerText(scheduleClone, XmlNameCreateScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CreateScheduleDiv)
            xmlScheduleClass.ActivityStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ActivityStaffBranchCode)
            xmlScheduleClass.ActivityStaffCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActivityStaffCode)
            xmlScheduleClass.ReceptionStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ReceptionStaffBranchCode)
            xmlScheduleClass.ReceptionStaffCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ReceptionStaffCode)
            ' スケジュール区分が来店予約の場合は必須項目、そうでなければオプション
            If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Then
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 START
                'xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeMandatory, 10, TypeConversion.IntegerType, ElementName.ContactNo)
                xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeMandatory, 10, TypeConversion.StringType, ElementName.ContactNo)
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 END
                '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
                xmlScheduleClass.ContactName = GetNodeInnerText(scheduleClone, XmlNameContactName, DataAssignment.ModeMandatory, 64, TypeConversion.StringType, ElementName.ContactName)
                '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV END
            Else
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 START
                'xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
                xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.StringType, ElementName.ContactNo)
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 END
                '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
                xmlScheduleClass.ContactName = GetNodeInnerText(scheduleClone, XmlNameContactName, DataAssignment.ModeOptional, 64, TypeConversion.StringType, ElementName.ContactName)
                '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV END
            End If
            '2013/07/17 SKFC 森 既存流用 Start
            'xmlScheduleClass.Summary = GetNodeInnerText(scheduleClone, XmlNameSummary, DataAssignment.ModeMandatory, 256, TypeConversion.StringType, ElementName.Summary)
            xmlScheduleClass.Summary = GetNodeInnerText(scheduleClone, XmlNameSummary, DataAssignment.ModeMandatory, 1000, TypeConversion.StringType, ElementName.Summary)
            '2013/07/17 SKFC 森 既存流用 End
            ' スケジュール作成区分がTodoのみの場合、開始日時はオプション設定
            If IsFlgEquals(xmlScheduleClass.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then
                xmlScheduleClass.StartTime = GetNodeInnerText(scheduleClone, XmlNameStartTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.StartTime)
            Else
                xmlScheduleClass.StartTime = GetNodeInnerText(scheduleClone, XmlNameStartTime, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.StartTime)
            End If
            xmlScheduleClass.EndTime = GetNodeInnerText(scheduleClone, XmlNameEndTime, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.EndTime)
            xmlScheduleClass.Memo = GetNodeInnerText(scheduleClone, XmlNameMemo, DataAssignment.ModeOptional, 2000, TypeConversion.StringType, ElementName.Memo)
            xmlScheduleClass.XIcropColor = GetNodeInnerText(scheduleClone, XmlNameXICropColor, DataAssignment.ModeMandatory, 30, TypeConversion.StringType, ElementName.XICropColor)

            ' Schedule要素内のAlert要素は0-2個存在する
            For Each alarmXml As XmlNode In GetChildNode(scheduleClone, XmlNameAlarm, DataAssignment.ModeOptional, True, ElementName.Alarm)

                Dim alarmClone As XmlNode = alarmXml.CloneNode(True)
                Dim alarm As String = GetNodeInnerText(alarmClone, XmlNameTrigger, DataAssignment.ModeOptional, 1, TypeConversion.IntegerType, ElementName.Trigger)
                If alarm IsNot Nothing Then
                    xmlScheduleClass.AlarmTriggerList.Add(alarm)
                End If
            Next

            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
            ' スケジュール区分が来店予約の場合は必須項目、そうでなければオプション
            If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Then
                xmlScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.OdrDiv)
            Else
                xmlScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.OdrDiv)
            End If
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV END


            xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.TodoId)
            xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ParentDiv)

            '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
            ''2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
            'If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.ReceivedProcess) Then
            'xmlScheduleClass.ProcessDiv = GetNodeInnerText(scheduleClone, XmlNameProcessDiv, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.ProcessDiv)
            'xmlScheduleClass.ResultDate = GetNodeInnerText(scheduleClone, XmlNameResultDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.ResultDate)
            'End If
            ''2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 END
            '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END

            Return xmlScheduleClass

        End Function

        ''' <summary>
        ''' Schedule要素内の、処理区分が「登録」の場合の要素を取得するメソッド(受注後工程)
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlAfterOrderScheduleClass">取得した要素を格納するクラス</param>
        ''' <param name="xmlAfterOrderDetailClass">Detail要素以下のデータが格納された変数</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <history>2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function EntryGetAfterOrderScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlAfterOrderScheduleClass As XmlAfterOrderSchedule, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderSchedule

            xmlAfterOrderScheduleClass.CreateScheduleDiv = GetNodeInnerText(scheduleClone, XmlNameCreateScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CreateScheduleDiv)
            xmlAfterOrderScheduleClass.ActivityStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ActivityStaffBranchCode)
            xmlAfterOrderScheduleClass.ActivityStaffCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActivityStaffCode)
            xmlAfterOrderScheduleClass.ReceptionStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ReceptionStaffBranchCode)
            xmlAfterOrderScheduleClass.ReceptionStaffCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ReceptionStaffCode)
            xmlAfterOrderScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeMandatory, 10, TypeConversion.StringType, ElementName.ContactNo)
            xmlAfterOrderScheduleClass.ContactName = GetNodeInnerText(scheduleClone, XmlNameContactName, DataAssignment.ModeMandatory, 64, TypeConversion.StringType, ElementName.ContactName)
            '2014/06/20 SKFC渡邊 NEXTSTEP_CALDAV 桁数修正 START
            'xmlAfterOrderScheduleClass.ActOdrName = GetNodeInnerText(scheduleClone, XmlNameActOdrName, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.ActOdrName)
            xmlAfterOrderScheduleClass.ActOdrName = GetNodeInnerText(scheduleClone, XmlNameActOdrName, DataAssignment.ModeMandatory, 512, TypeConversion.StringType, ElementName.ActOdrName)
            '2014/06/20 SKFC渡邊 NEXTSTEP_CALDAV 桁数修正 START
            xmlAfterOrderScheduleClass.Summary = GetNodeInnerText(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 1000, TypeConversion.StringType, ElementName.Summary)
            xmlAfterOrderScheduleClass.StartTime = GetNodeInnerText(scheduleClone, XmlNameStartTime, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.StartTime)
            xmlAfterOrderScheduleClass.EndTime = GetNodeInnerText(scheduleClone, XmlNameEndTime, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.EndTime)
            xmlAfterOrderScheduleClass.Memo = GetNodeInnerText(scheduleClone, XmlNameMemo, DataAssignment.ModeOptional, 2000, TypeConversion.StringType, ElementName.Memo)
            xmlAfterOrderScheduleClass.XIcropColor = GetNodeInnerText(scheduleClone, XmlNameXICropColor, DataAssignment.ModeOptional, 30, TypeConversion.StringType, ElementName.XICropColor)

            ' Schedule要素内のAlert要素は0-2個存在する
            For Each alarmXml As XmlNode In GetChildNode(scheduleClone, XmlNameAlarm, DataAssignment.ModeOptional, True, ElementName.Alarm)

                Dim alarmClone As XmlNode = alarmXml.CloneNode(True)
                Dim alarm As String = GetNodeInnerText(alarmClone, XmlNameTrigger, DataAssignment.ModeMandatory, 1, TypeConversion.IntegerType, ElementName.Trigger)
                If alarm IsNot Nothing Then
                    xmlAfterOrderScheduleClass.AlarmTriggerList.Add(alarm)
                End If
            Next

            xmlAfterOrderScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.OdrDiv)
            xmlAfterOrderScheduleClass.AfterOdrActID = GetNodeInnerText(scheduleClone, XmlNameAfterOdrActID, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.AfterOdrActID)
            xmlAfterOrderScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.TodoId)
            xmlAfterOrderScheduleClass.ProcessDiv = GetNodeInnerText(scheduleClone, XmlNameProcessDiv, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.ProcessDiv)
            xmlAfterOrderScheduleClass.ResultDate = GetNodeInnerText(scheduleClone, XmlNameResultDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.ResultDate)

            Return xmlAfterOrderScheduleClass

        End Function



        ''' <summary>
        ''' Schedule要素内の、処理区分が「更新」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlScheduleClass">取得した要素を格納するクラス</param>
        ''' <param name="xmlDetailClass">Detail要素以下のデータが格納された変数</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <remarks></remarks>
        Private Function UpdateGetScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlScheduleClass As XmlSchedule, ByVal xmlDetailClass As XmlDetail) As XmlSchedule

            xmlScheduleClass.CreateScheduleDiv = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameCreateScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CreateScheduleDiv)
            xmlScheduleClass.ActivityStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ActivityStaffBranchCode)
            xmlScheduleClass.ActivityStaffCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActivityStaffCode)
            xmlScheduleClass.ReceptionStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ReceptionStaffBranchCode)
            xmlScheduleClass.ReceptionStaffCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ReceptionStaffCode)
            ' 接触方法Ｎｏはスケジュール区分が来店予約の場合は空欄は許されない
            If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Then
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 START
                'xmlScheduleClass.ContactNo = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
                xmlScheduleClass.ContactNo = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.StringType, ElementName.ContactNo)
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 END
            Else
                ' 来店予約でない場合は空欄（削除）を許す
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 START
                'xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
                xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.StringType, ElementName.ContactNo)
                '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 END
            End If

            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
            xmlScheduleClass.ContactName = GetNodeInnerText(scheduleClone, XmlNameContactName, DataAssignment.ModeOptional, 64, TypeConversion.StringType, ElementName.ContactName)
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START

            '2013/07/17 SKFC 森 既存流用 Start
            'xmlScheduleClass.Summary = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.Summary)
            xmlScheduleClass.Summary = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 1000, TypeConversion.StringType, ElementName.Summary)
            '2013/07/17 SKFC 森 既存流用 End
            ' 開始時間はスケジュール作成区分がTodoのみのとき意外は許されない
            If IsFlgEquals(xmlScheduleClass.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then
                ' Todoのときのみの時は空欄を許す
                xmlScheduleClass.StartTime = GetNodeInnerText(scheduleClone, XmlNameStartTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.StartTime)
            Else

                xmlScheduleClass.StartTime = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameStartTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.StartTime)
            End If
            xmlScheduleClass.EndTime = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameEndTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.EndTime)
            xmlScheduleClass.Memo = GetNodeInnerText(scheduleClone, XmlNameMemo, DataAssignment.ModeOptional, 2000, TypeConversion.StringType, ElementName.Memo)
            xmlScheduleClass.XIcropColor = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameXICropColor, DataAssignment.ModeOptional, 30, TypeConversion.StringType, ElementName.XICropColor)
            ' Schedule要素内のAlert要素は0-2個存在する
            For Each alarmXml As XmlNode In GetChildNode(scheduleClone, XmlNameAlarm, DataAssignment.ModeOptional, True, ElementName.Alarm)

                Dim alarmClone As XmlNode = alarmXml.CloneNode(True)
                Dim alarm As String = GetNodeInnerText(alarmClone, XmlNameTrigger, DataAssignment.ModeOptional, 1, TypeConversion.IntegerType, ElementName.Trigger)
                If alarm IsNot Nothing Then
                    xmlScheduleClass.AlarmTriggerList.Add(alarm)
                End If
            Next

            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
            'スケジュール区分が来店予約の場合
            If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Then
                xmlScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.OdrDiv)
            Else
                xmlScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.OdrDiv)
            End If
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV END

            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV IF追加に伴い削除　START
            ''2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
            ''xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.TodoId)
            'If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.ReceivedProcess) Then
            ''2012/03/23 SKFC 加藤 【SALES_2】受注後工程(不具合対応) START
            ''mlScheduleClass.ProcessDiv = GetNodeInnerText(scheduleClone, XmlNameProcessDiv, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.ProcessDiv)
            'xmlScheduleClass.ProcessDiv = GetNodeInnerText(scheduleClone, XmlNameProcessDiv, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ProcessDiv)
            'xmlScheduleClass.ResultDate = GetNodeInnerText(scheduleClone, XmlNameResultDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.ResultDate)
            'xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeOptional, 20, TypeConversion.None, ElementName.TodoId)
            ''2012/03/23 SKFC 加藤 【SALES_2】受注後工程(不具合対応) END
            'Else
            'xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.None, ElementName.TodoId)
            'xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ParentDiv)
            'End If
            ''2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START

            xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.None, ElementName.TodoId)
            xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ParentDiv)
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV IF追加に伴い削除　END

            Return xmlScheduleClass

        End Function

        ''' <summary>
        ''' Schedule要素内の、処理区分が「更新」の場合の要素を取得するメソッド(受注後工程)
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlAfterOrderScheduleClass">取得した要素を格納するクラス</param>
        ''' <param name="xmlAfterOrderDetailClass">Detail要素以下のデータが格納された変数</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <history>2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function UpdateGetAfterOrderScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlAfterOrderScheduleClass As XmlAfterOrderSchedule, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderSchedule

            xmlAfterOrderScheduleClass.CreateScheduleDiv = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameCreateScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CreateScheduleDiv)
            xmlAfterOrderScheduleClass.ActivityStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ActivityStaffBranchCode)
            xmlAfterOrderScheduleClass.ActivityStaffCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActivityStaffCode)
            xmlAfterOrderScheduleClass.ReceptionStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ReceptionStaffBranchCode)
            xmlAfterOrderScheduleClass.ReceptionStaffCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ReceptionStaffCode)
            xmlAfterOrderScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.StringType, ElementName.ContactNo)
            xmlAfterOrderScheduleClass.ContactName = GetNodeInnerText(scheduleClone, XmlNameContactName, DataAssignment.ModeOptional, 64, TypeConversion.StringType, ElementName.ContactName)
            '2014/06/20 SKFC 渡邊 NEXTSTEP_CALDAV 桁数修正 START
            ''2014/06/16 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START
            ''xmlAfterOrderScheduleClass.ActOdrName = GetNodeInnerText(scheduleClone, XmlNameActOdrName, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ContactName)
            'xmlAfterOrderScheduleClass.ActOdrName = GetNodeInnerText(scheduleClone, XmlNameActOdrName, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActOdrName)
            xmlAfterOrderScheduleClass.ActOdrName = GetNodeInnerText(scheduleClone, XmlNameActOdrName, DataAssignment.ModeOptional, 512, TypeConversion.StringType, ElementName.ActOdrName)
            ''2014/06/16 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 END
            '2014/06/20 SKFC 渡邊 NEXTSTEP_CALDAV 桁数修正 END

            xmlAfterOrderScheduleClass.Summary = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 1000, TypeConversion.StringType, ElementName.Summary)
            ' 開始時間はスケジュール作成区分がTodoのみのとき意外は許されない
            If IsFlgEquals(xmlAfterOrderScheduleClass.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then
                ' Todoのときのみの時は空欄を許す
                xmlAfterOrderScheduleClass.StartTime = GetNodeInnerText(scheduleClone, XmlNameStartTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.StartTime)
            Else

                xmlAfterOrderScheduleClass.StartTime = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameStartTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.StartTime)
            End If
            xmlAfterOrderScheduleClass.EndTime = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameEndTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.EndTime)
            xmlAfterOrderScheduleClass.Memo = GetNodeInnerText(scheduleClone, XmlNameMemo, DataAssignment.ModeOptional, 2000, TypeConversion.StringType, ElementName.Memo)
            'xmlAfterOrderScheduleClass.XIcropColor = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameXICropColor, DataAssignment.ModeOptional, 30, TypeConversion.StringType, ElementName.XICropColor)
            xmlAfterOrderScheduleClass.XIcropColor = GetNodeInnerText(scheduleClone, XmlNameXICropColor, DataAssignment.ModeOptional, 30, TypeConversion.StringType, ElementName.XICropColor)
            ' Schedule要素内のAlert要素は0-2個存在する
            For Each alarmXml As XmlNode In GetChildNode(scheduleClone, XmlNameAlarm, DataAssignment.ModeOptional, True, ElementName.Alarm)

                Dim alarmClone As XmlNode = alarmXml.CloneNode(True)
                Dim alarm As String = GetNodeInnerText(alarmClone, XmlNameTrigger, DataAssignment.ModeOptional, 1, TypeConversion.IntegerType, ElementName.Trigger)
                If alarm IsNot Nothing Then
                    xmlAfterOrderScheduleClass.AlarmTriggerList.Add(alarm)
                End If
            Next

            xmlAfterOrderScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.OdrDiv)
            '2014/06/16 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START
            'xmlAfterOrderScheduleClass.AfterOdrActID = GetNodeInnerText(scheduleClone, XmlNameAfterOdrActID, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.OdrDiv)
            xmlAfterOrderScheduleClass.AfterOdrActID = GetNodeInnerText(scheduleClone, XmlNameAfterOdrActID, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.AfterOdrActID)
            '2014/06/16 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 END
            xmlAfterOrderScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeOptional, 20, TypeConversion.None, ElementName.TodoId)
            xmlAfterOrderScheduleClass.ProcessDiv = GetNodeInnerText(scheduleClone, XmlNameProcessDiv, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.ProcessDiv)
            xmlAfterOrderScheduleClass.ResultDate = GetNodeInnerText(scheduleClone, XmlNameResultDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.ResultDate)


            Return xmlAfterOrderScheduleClass

        End Function


        ''' <summary>
        ''' Schedule要素内の、処理区分が「イベント追加」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlScheduleClass">取得した要素を格納するクラス</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <remarks></remarks>
        Private Function EventAddGetScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlScheduleClass As XmlSchedule, ByVal xmlDetailClass As XmlDetail) As XmlSchedule
            '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
            'Private Function EventAddGetScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlScheduleClass As XmlSchedule, ByVal xmlDetailClass As XmlDetail) As XmlSchedule
            '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START

            xmlScheduleClass.CreateScheduleDiv = GetNodeInnerText(scheduleClone, XmlNameCreateScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CreateScheduleDiv)
            xmlScheduleClass.ActivityStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ActivityStaffBranchCode)
            xmlScheduleClass.ActivityStaffCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActivityStaffCode)
            xmlScheduleClass.ReceptionStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ReceptionStaffBranchCode)
            xmlScheduleClass.ReceptionStaffCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ReceptionStaffCode)
            '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 START
            'xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
            xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.StringType, ElementName.ContactNo)
            '2014/01/26 SKFC 加藤 【既存流用】CONTACTNOの属性NUMBER→nVarcha2の対応 END
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
            xmlScheduleClass.ContactName = GetNodeInnerText(scheduleClone, XmlNameContactName, DataAssignment.ModeOptional, 64, TypeConversion.StringType, ElementName.ContactName)
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV END
            '2013/07/17 SKFC 森 既存流用 Start
            'xmlScheduleClass.Summary = GetNodeInnerText(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.Summary)
            xmlScheduleClass.Summary = GetNodeInnerText(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 1000, TypeConversion.StringType, ElementName.Summary)
            '2013/07/17 SKFC 森 既存流用 End
            xmlScheduleClass.StartTime = GetNodeInnerText(scheduleClone, XmlNameStartTime, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.StartTime)
            xmlScheduleClass.EndTime = GetNodeInnerText(scheduleClone, XmlNameEndTime, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.EndTime)
            xmlScheduleClass.Memo = GetNodeInnerText(scheduleClone, XmlNameMemo, DataAssignment.ModeOptional, 2000, TypeConversion.StringType, ElementName.Memo)
            xmlScheduleClass.XIcropColor = GetNodeInnerText(scheduleClone, XmlNameXICropColor, DataAssignment.ModeOptional, 30, TypeConversion.StringType, ElementName.XICropColor)

            xmlScheduleClass.InitialAlarmTriggerList()

            ' Schedule要素内のAlert要素は0-2個存在する
            For Each alarmXml As XmlNode In GetChildNode(scheduleClone, XmlNameAlarm, DataAssignment.ModeOptional, True, ElementName.Alarm)
                Dim alarmClone As XmlNode = alarmXml.CloneNode(True)
                Dim alarm As String = GetNodeInnerText(alarmClone, XmlNameTrigger, DataAssignment.ModeOptional, 1, TypeConversion.IntegerType, ElementName.Trigger)
                If alarm IsNot Nothing Then
                    xmlScheduleClass.AlarmTriggerList.Add(alarm)
                End If

            Next

            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
            'スケジュール区分が来店予約の場合
            If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Then
                xmlScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.OdrDiv)
            Else
                xmlScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.OdrDiv)
            End If
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV END


            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV IF追加に伴い削除　START
            ''2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
            ''xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.TodoId)
            ''xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ParentDiv)
            'If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.ReceivedProcess) Then
            ''2012/03/27 SKFC 上田 【SALES_2】受注後工程の対応 START
            ''xmlScheduleClass.ProcessDiv = GetNodeInnerText(scheduleClone, XmlNameProcessDiv, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.ProcessDiv)
            'xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.None, ElementName.TodoId)
            ''2012/03/27 SKFC 上田 【SALES_2】受注後工程の対応 START
            'xmlScheduleClass.ResultDate = GetNodeInnerText(scheduleClone, XmlNameResultDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.ResultDate)
            'Else
            'xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.None, ElementName.TodoId)
            'xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ParentDiv)
            'End If
            ''2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV IF追加に伴い削除　END

            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV START
            xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.None, ElementName.TodoId)
            xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ParentDiv)
            '2014/04/03 SKFC渡邊 NEXTSTEP_CALDAV END


            ' スケジュール作成区分が規定されている値で無い場合
            If Not Validation.Equals(xmlScheduleClass.CreateScheduleDiv, ThreeString) Then
                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.CreateScheduleDiv)

            End If
            Return xmlScheduleClass

        End Function
        ''' <summary>
        ''' Schedule要素内の、処理区分が「削除」の場合の要素を取得するメソッド(受注後工程)
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlAfterOrderScheduleClass">取得した要素を格納するクラス</param>
        ''' <param name="xmlAfterOrderDetailClass">Detail要素以下のデータが格納された変数</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <history>2014/07/23 SKFC渡邊 NEXTSTEP_CALDAV 仕様変更 START</history>
        ''' <remarks></remarks>
        Private Function DeleteGetAfterOrderScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlAfterOrderScheduleClass As XmlAfterOrderSchedule, ByVal xmlAfterOrderDetailClass As XmlAfterOrderDetail) As XmlAfterOrderSchedule

            xmlAfterOrderScheduleClass.CreateScheduleDiv = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameCreateScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CreateScheduleDiv)
            xmlAfterOrderScheduleClass.ActivityStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ActivityStaffBranchCode)
            xmlAfterOrderScheduleClass.ActivityStaffCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActivityStaffCode)
            xmlAfterOrderScheduleClass.ReceptionStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ReceptionStaffBranchCode)
            xmlAfterOrderScheduleClass.ReceptionStaffCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ReceptionStaffCode)
            xmlAfterOrderScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.StringType, ElementName.ContactNo)
            xmlAfterOrderScheduleClass.ContactName = GetNodeInnerText(scheduleClone, XmlNameContactName, DataAssignment.ModeOptional, 64, TypeConversion.StringType, ElementName.ContactName)
            xmlAfterOrderScheduleClass.ActOdrName = GetNodeInnerText(scheduleClone, XmlNameActOdrName, DataAssignment.ModeOptional, 512, TypeConversion.StringType, ElementName.ActOdrName)

            xmlAfterOrderScheduleClass.Summary = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 1000, TypeConversion.StringType, ElementName.Summary)
            ' 開始時間はスケジュール作成区分がTodoのみのとき意外は許されない
            If IsFlgEquals(xmlAfterOrderScheduleClass.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then
                ' Todoのときのみの時は空欄を許す
                xmlAfterOrderScheduleClass.StartTime = GetNodeInnerText(scheduleClone, XmlNameStartTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.StartTime)
            Else

                xmlAfterOrderScheduleClass.StartTime = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameStartTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.StartTime)
            End If
            xmlAfterOrderScheduleClass.EndTime = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameEndTime, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.EndTime)
            xmlAfterOrderScheduleClass.Memo = GetNodeInnerText(scheduleClone, XmlNameMemo, DataAssignment.ModeOptional, 2000, TypeConversion.StringType, ElementName.Memo)
            xmlAfterOrderScheduleClass.XIcropColor = GetNodeInnerText(scheduleClone, XmlNameXICropColor, DataAssignment.ModeOptional, 30, TypeConversion.StringType, ElementName.XICropColor)
            ' Schedule要素内のAlert要素は0-2個存在する
            For Each alarmXml As XmlNode In GetChildNode(scheduleClone, XmlNameAlarm, DataAssignment.ModeOptional, True, ElementName.Alarm)

                Dim alarmClone As XmlNode = alarmXml.CloneNode(True)
                Dim alarm As String = GetNodeInnerText(alarmClone, XmlNameTrigger, DataAssignment.ModeOptional, 1, TypeConversion.IntegerType, ElementName.Trigger)
                If alarm IsNot Nothing Then
                    xmlAfterOrderScheduleClass.AlarmTriggerList.Add(alarm)
                End If
            Next

            xmlAfterOrderScheduleClass.OdrDiv = GetNodeInnerText(scheduleClone, XmlNameOdrDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.OdrDiv)
            xmlAfterOrderScheduleClass.AfterOdrActID = GetNodeInnerText(scheduleClone, XmlNameAfterOdrActID, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.AfterOdrActID)
            xmlAfterOrderScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeOptional, 20, TypeConversion.None, ElementName.TodoId)
            xmlAfterOrderScheduleClass.ProcessDiv = GetNodeInnerText(scheduleClone, XmlNameProcessDiv, DataAssignment.ModeMandatory, 3, TypeConversion.StringType, ElementName.ProcessDiv)
            xmlAfterOrderScheduleClass.ResultDate = GetNodeInnerText(scheduleClone, XmlNameResultDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.ResultDate)


            Return xmlAfterOrderScheduleClass

        End Function


        ''' <summary>
        ''' 処理区分が「登録」のＤＢ処理を行います
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function EntryDataBase(ByVal detailData As XmlDetail, ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 EntryDataBase() Start")
            '$01 Add End
            ' カレンダーID
            Dim calenderId As String = Nothing
            ' TodoId
            Dim todoId As String = Nothing
            ' eventId
            Dim eventId As String = Nothing
            'ICropの変数から、紐付くカレンダーIDを取得します。
            calenderId = BizGetCalenderId(detailData)
            '$01 Add Start
            Logger.Info("IC3040403 BizGetCalenderId:" + calenderId)
            Logger.Info("IC3040403 detailData.CompletionDiv:" + detailData.CompletionDiv)
            Logger.Info("IC3040403 CompletionFlg.FlgContinue:" + CompletionFlg.FlgContinue.ToString())
            Logger.Info("IC3040403 CompletionFlg.FlgContinue:" + CompletionFlg.FlgActivityCompleted.ToString())
            '$01 Add End
            ' 完了フラグがContinue又は完了の場合であり、カレンダーＩＤが紐付く値の場合
            If (IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgContinue) Or _
                 IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgActivityCompleted)) AndAlso _
                  calenderId IsNot Nothing Then
                ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)
                ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, Nothing, Nothing)
                ' カレンダーTodo情報テーブルの完了フラグを更新します
                BizUpdateCompleteFlgCalTodoItem(calenderId, detailData.CompletionDate, detailData.ActivityCreateStaff)

                ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                '' カレンダーTodo情報テーブルの削除フラグを更新します 
                'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                '' カレンダーEvent情報テーブルの削除フラグを更新します
                'BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                ' カレンダーTodo情報テーブルの削除フラグを更新します 
                ' (来店予約特有処理、「来店フォローを完了、来店予約を削除」の為、完了していないTODOを削除する)
                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
                'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff, CompletionFlg.None)
                BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, Nothing, detailData.DeleteDate, detailData.ActivityCreateStaff, CompletionFlg.None)
                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END
                ' カレンダーEvent情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff, CompletionFlg.None)
                ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

                '$01 Add Start
                Logger.Info("IC3040403 detailData.CompletionDiv:" + detailData.CompletionDiv)
                Logger.Info("IC3040403 CompletionFlg.FlgContinue:" + CompletionFlg.FlgActivityCompleted.ToString())
                '$01 Add End
                ' 完了区分が"完了"だった場合、処理をこれで終了する
                If IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgActivityCompleted) Then

                    Return staffCodeList

                End If


            End If
            '$01 Add Start
            Logger.Info("IC3040403 detailData.CompletionDiv:" + detailData.CompletionDiv)
            Logger.Info("IC3040403 CompletionFlg.FlgNotContinue:" + CompletionFlg.FlgNotContinue.ToString())
            Logger.Info("IC3040403 CompletionFlg.FlgContinue:" + CompletionFlg.FlgContinue.ToString())
            '$01 Add End
            ' 完了フラグが完了なしまたはContinueの場合
            If detailData.CompletionDiv Is Nothing Or _
                IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgNotContinue) Or _
                IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgContinue) Then

                ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                ' スケジュール区分が「入庫予約」の場合
                'If IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.GRReservattion) AndAlso _
                '    calenderId IsNot Nothing Then

                '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV IF追加に伴い削除　START
                ' スケジュール区分が「入庫予約」もしくは「受注後工程」でCalendarIDが取得できている場合
                'If (IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.GRReservattion) Or _
                '    IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.ReceivedProcess)) AndAlso _
                '   calenderId IsNot Nothing Then
                ''2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                'スケジュール区分が「入庫予約」でCalendarIDが取得できている場合
                '$01 Add Start
                Logger.Info("IC3040403 calenderId:" + calenderId)
                Logger.Info("IC3040403 detailData.ScheduleDiv:" + detailData.ScheduleDiv)
                Logger.Info("IC3040403 ScheDuleDiv.GRReservattion:" + ScheDuleDiv.GRReservattion.ToString())
                '$01 Add End
                If IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.GRReservattion) AndAlso _
                    calenderId IsNot Nothing Then
                    '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END


                    ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                    staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)
                    ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                    staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, Nothing, Nothing)
                    ' カレンダーICROP情報管理テーブルの削除フラグを更新します
                    BizUpdateDeleteFlgCalItem(detailData, calenderId)

                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    '' カレンダーTodo情報テーブルの削除フラグを更新します
                    'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                    '' カレンダーEvent情報テーブルの削除フラグを更新します
                    'BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff)

                    '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
                    ' カレンダーTodo情報テーブルの削除フラグを更新します
                    'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                    BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, Nothing, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                    '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END

                    ' カレンダーEvent情報テーブルの削除フラグを更新します
                    BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

                End If
                '$01 Add Start
                Logger.Info("IC3040403 calenderId:" + calenderId)
                Logger.Info("IC3040403 detailData.ScheduleDiv:" + detailData.ScheduleDiv)
                Logger.Info("IC3040403 ScheDuleDiv.VisitReservation:" + ScheDuleDiv.VisitReservation.ToString())
                '$01 Add End
                ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                ' 「来店予約」でCalendarIdが取得できている場合以外
                ' 分かりにくいのでNotをElseへ置き換え
                'If Not (IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.VisitReservation) AndAlso calenderId IsNot Nothing) Then
                If (IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.VisitReservation) AndAlso calenderId IsNot Nothing) Then
                Else
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

                    ' 新規のカレンダーＩＤで、カレンダーICROP情報管理テーブルを作成する
                    calenderId = BizGetNewCalenderID()
                    '$01 Add Start
                    Logger.Info("IC3040403 New calenderId:" + calenderId)
                    '$01 Add End
                    BizInsertCalCalender(detailData, calenderId)

                End If

                ' スケジュール要素分、Todo、イベント追加を行います
                For Each scheduleData As XmlSchedule In detailData.ScheduleList

                    ' スタッフコードチェックを行います
                    CheckStaffCode(detailData.ScheduleDiv, scheduleData.ActivityStaffCode, scheduleData.ReceptionStaffCode)
                    '$01 Add Start
                    Logger.Info("IC3040403 scheduleData.CreateScheduleDiv:" + scheduleData.CreateScheduleDiv)
                    Logger.Info("IC3040403 CreateScheduleDiv.FlgEventAndTodo:" + CreateScheduleDiv.FlgEventAndTodo.ToString())
                    Logger.Info("IC3040403 CreateScheduleDiv.FlgTodo:" + CreateScheduleDiv.FlgTodo.ToString())
                    '$01 Add End
                    ' スケジュール作成区分がTodo+Event又はTodoだった場合、Todoを作成します
                    If IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEventAndTodo) Or _
                       IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then

                        ' 追加なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                        staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                        staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                        ' todoIdを作成します
                        todoId = BizGetNewTodoId()
                        '$01 Add Start
                        Logger.Info("IC3040403 New todoId:" + todoId)
                        '$01 Add End
                        ' カレンダーTodo情報テーブルに登録します
                        BizInsertCalTodoItem(scheduleData, calenderId, todoId, detailData.ActivityCreateStaff)
                        '$01 Add Start
                        Logger.Info("IC3040403 scheduleData.AlarmTriggerList.Count:" + scheduleData.AlarmTriggerList.Count.ToString())
                        '$01 Add End
                        ' アラームの項目が存在する場合、アラームを登録します
                        If scheduleData.AlarmTriggerList.Count > 0 Then

                            BizInsertCalTodoAlarms(scheduleData, todoId, detailData.ActivityCreateStaff)

                        End If

                    End If
                    '$01 Add Start
                    Logger.Info("IC3040403 scheduleData.CreateScheduleDiv:" + scheduleData.CreateScheduleDiv)
                    Logger.Info("IC3040403 CreateScheduleDiv.FlgEventAndTodo:" + CreateScheduleDiv.FlgEventAndTodo.ToString())
                    Logger.Info("IC3040403 CreateScheduleDiv.FlgEvent:" + CreateScheduleDiv.FlgEvent.ToString())
                    '$01 Add End
                    ' スケジュール作成区分がTodo+Event又はEventだった場合、Eventを作成します
                    If IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEventAndTodo) Or _
                        IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEvent) Then

                        ' 追加なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                        staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                        staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                        ' eventIdを作成します
                        eventId = BizGetNewEventId()
                        '$01 Add Start
                        Logger.Info("IC3040403 New eventId:" + eventId)
                        '$01 Add End
                        ' カレンダーevent情報テーブルに登録します
                        BizInsertCalEventItem(scheduleData, calenderId, todoId, eventId, detailData.ActivityCreateStaff)
                        '$01 Add Start
                        Logger.Info("IC3040403 scheduleData.AlarmTriggerList.Count:" + scheduleData.AlarmTriggerList.Count.ToString())
                        '$01 Add End
                        ' アラームの項目が存在する場合、アラームを登録します
                        If scheduleData.AlarmTriggerList.Count > 0 Then

                            BizInsertCalEventAlarms(scheduleData, eventId, detailData.ActivityCreateStaff)

                        End If

                    End If

                Next

            End If
            '$01 Add Start
            Logger.Info("IC3040403 EntryDataBase() End")
            '$01 Add End
            Return staffCodeList

        End Function

        ''' <summary>
        ''' 処理区分が「登録」のＤＢ処理を行います(受注後工程)
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        Private Function EntryAfterOrderDataBase(ByVal detailData As XmlAfterOrderDetail, ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 EntryAfterOrderDataBase() Start")
            '$01 Add End
            ' カレンダーID
            Dim calenderId As String = Nothing
                        ' TodoId
            Dim todoId As String = Nothing

            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
            Dim eventId As String = Nothing
            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START

            'ICropの変数から、紐付くカレンダーIDを取得します
            calenderId = BizGetAfterOrderCalenderId(detailData)
            '$01 Add Start
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            ' カレンダーＩＤが紐付く値の場合

            ' calenderIdが取得された場合
            If calenderId IsNot Nothing Then
                ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                'staffCodeList = GetAfterOrderStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing)
                staffCodeList = GetAfterOrderStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)

                ' カレンダーTodo情報テーブルの削除フラグを更新します
                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
                'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff, CompletionFlg.None)
                BizUpdateDeleteFlgCalTodoItem(calenderId, Nothing, Nothing, detailData.DeleteDate, detailData.ActivityCreateStaff, CompletionFlg.None)
                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END

                ' ★2014/07/23 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                ' カレンダーEvent情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff, CompletionFlg.None)
                ' 2014/07/23 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END

            Else
                ' 新規のカレンダーＩＤで、カレンダーICROP情報管理テーブルを作成します
                calenderId = BizGetNewCalenderID()
                '$01 Add Start
                Logger.Info("IC3040403 New calenderId:" + calenderId)
                '$01 Add End
                BizInsertAfterOrderCalCalender(detailData, calenderId)

            End If



            ' スケジュール要素分、Todo追加を行います
            For Each scheduleData As XmlAfterOrderSchedule In detailData.ScheduleList

                ' スタッフコードチェックを行います
                CheckStaffCode(Nothing, scheduleData.ActivityStaffCode, scheduleData.ReceptionStaffCode)
                ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                '' スケジュール作成区分がTodoだった場合、Todoを作成します
                'If IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then
                '$01 Add Start
                Logger.Info("IC3040403 scheduleData.CreateScheduleDiv:" + scheduleData.CreateScheduleDiv)
                Logger.Info("IC3040403 CreateScheduleDiv.FlgEventAndTodo:" + CreateScheduleDiv.FlgEventAndTodo.ToString())
                Logger.Info("IC3040403 CreateScheduleDiv.FlgTodo:" + CreateScheduleDiv.FlgTodo.ToString())
                '$01 Add End
                ' スケジュール作成区分がTodo+Event又はTodoだった場合、Todoを作成します
                If IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEventAndTodo) Or _
                    IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then

                    ' 追加なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                    staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                    staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)


                    ' todoIdを作成します
                    todoId = BizGetNewTodoId()
                    '$01 Add Start
                    Logger.Info("IC3040403 New todoId:" + todoId)
                    '$01 Add End
                    ' カレンダーTodo情報テーブルに登録します
                    BizInsertAfterOrderCalTodoItem(scheduleData, calenderId, todoId, detailData.ActivityCreateStaff)
                    '$01 Add Start
                    Logger.Info("IC3040403 scheduleData.AlarmTriggerList.Count:" + scheduleData.AlarmTriggerList.Count.ToString())
                    '$01 Add End
                    ' アラームの項目が存在する場合、アラームを登録します
                    If scheduleData.AlarmTriggerList.Count > 0 Then

                        BizInsertAfterOrderCalTodoAlarms(scheduleData, todoId, detailData.ActivityCreateStaff)

                    End If

                End If
                '$01 Add Start
                Logger.Info("IC3040403 scheduleData.CreateScheduleDiv:" + scheduleData.CreateScheduleDiv)
                Logger.Info("IC3040403 CreateScheduleDiv.FlgEventAndTodo:" + CreateScheduleDiv.FlgEventAndTodo.ToString())
                '$01 Add End
                ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                ' スケジュール作成区分がTodo+Eventだった場合、Eventを作成します
                If IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEventAndTodo) Then

                    ' 追加なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                    staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                    staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                    ' eventIdを作成します
                    eventId = BizGetNewEventId()
                    '$01 Add Start
                    Logger.Info("IC3040403 New eventId:" + eventId)
                    '$01 Add End
                    ' カレンダーevent情報テーブルに登録します
                    BizInsertAfterOrderCalEventItem(scheduleData, calenderId, todoId, eventId, detailData.ActivityCreateStaff)
                    '$01 Add Start
                    Logger.Info("IC3040403 scheduleData.AlarmTriggerList.Count:" + scheduleData.AlarmTriggerList.Count.ToString())
                    '$01 Add End
                    ' アラームの項目が存在する場合、アラームを登録します
                    If scheduleData.AlarmTriggerList.Count > 0 Then

                        BizInsertAfterOrderCalEventAlarms(scheduleData, eventId, detailData.ActivityCreateStaff)

                    End If

                End If
                ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END



            Next
            '$01 Add Start
            Logger.Info("IC3040403 EntryAfterOrderDataBase() End")
            '$01 Add End
            Return staffCodeList

        End Function


        ''' <summary>
        ''' 処理区分が「更新」のＤＢ処理を行います
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function UpdateDataBase(ByVal detailData As XmlDetail, ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 UpdateDataBase() Start")
            '$01 Add End
            ' カレンダーID
            Dim calenderId As String = Nothing
            ' TodoId
            Dim todoId As String = Nothing
            ' eventId
            Dim eventId As String = Nothing
            ' ICropの変数から、紐付くカレンダーIDを取得します。
            calenderId = BizGetCalenderId(detailData)
            '$01 Add Start
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            ' カレンダーＩＤが空の場合、更新、削除処理が実行できないので終了する。
            If calenderId Is Nothing Then
                '$01 Add Start
                Logger.Info("IC3040403 UpdateDataBase() End")
                '$01 Add End
                Return staffCodeList
            End If

            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
            '削除日に値がNullか入っていない場合
            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
            '$01 Add Start
            Logger.Info("IC3040403 detailData.DeleteDate:" + detailData.DeleteDate)
            '$01 Add End
            If detailData.DeleteDate Is Nothing Or Validation.Equals(detailData.DeleteDate, EmptyString) Then
                '$01 Add Start
                Logger.Info("IC3040403 detailData.ScheduleInfoFlg:" + detailData.ScheduleInfoFlg.ToString())
                '$01 Add End
                ' scheduleInfoが存在する場合、カレンダーを更新する。
                If detailData.ScheduleInfoFlg = True Then

                    ' 紐付くカレンダーIdが存在するのであれば、更新します
                    If calenderId IsNot Nothing Then

                        ' カレンダーICROP情報管理テーブルを更新します
                        BizUpdateCalCalender(detailData, calenderId)

                    End If

                End If

                ' Schedule要素の数だけ、更新処理を行います
                For Each scheduleData As XmlSchedule In detailData.ScheduleList

                    todoId = scheduleData.TodoId
                    '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV IF追加に伴い削除　START
                    '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    'If todoId Is Nothing Then
                    'If IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.ReceivedProcess) Then
                    '' 受注後工程の場合、工程からTodoIdを取得します
                    '' バッチからの更新の場合、i-CROP側でTodoIDを取得できない為WebServiceで吸収
                    'todoId = BizGetTodoId(calenderId, scheduleData.ProcessDiv)
                    'End If
                    'End If
                    '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV IF追加に伴い削除　END
                    '$01 Add Start
                    Logger.Info("IC3040403 todoId:" + todoId)
                    '$01 Add End
                    If todoId Is Nothing Then
                        'TODOIDが取得できていない場合
                        Logger.Info("Don't Sherch TodoID CalendarID=[" & calenderId & "] ProsessDiv=[" & scheduleData.ProcessDiv & "]")

                    Else
                        '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 END

                        ' 更新なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                        staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                        staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                        ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                        staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, scheduleData, detailData.ScheduleDiv)

                        ' Todoテーブルを更新します
                        '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        'BizUpdateCalTodoItem(scheduleData, scheduleData.TodoId, detailData.ActivityCreateStaff)
                        BizUpdateCalTodoItem(scheduleData, todoId, detailData.ActivityCreateStaff)
                        '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 END

                        ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                        staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, scheduleData, detailData.ScheduleDiv)

                        ' Eventテーブルを更新します
                        BizUpdateCalEventItem(scheduleData, todoId, detailData.ActivityCreateStaff)

                        ' todoIdから、EventIdを取得します
                        '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        'eventId = BizGetEventId(scheduleData.todoId)
                        eventId = BizGetEventId(todoId)
                        '$01 Add Start
                        Logger.Info("IC3040403 eventId:" + eventId)
                        '$01 Add End
                        '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 END

                        '$01 Add Start
                        Logger.Info("IC3040403 scheduleData.AlarmTriggerList.Count:" + scheduleData.AlarmTriggerList.Count.ToString())
                        '$01 Add End
                        ' アラームの項目が存在する場合、アラームを登録します
                        If scheduleData.AlarmTriggerList.Count > 0 Then
                            Using adapter As New IC3040403DataSetTableAdapters.CalTodoAlarmDataTable
                                ' アラームを削除し、新しいアラームを入れます。
                                '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
                                'BizDeleteCalTodoAlarm(scheduleData.TodoId)
                                'BizInsertCalTodoAlarms(scheduleData, scheduleData.TodoId, detailData.ActivityCreateStaff)
                                BizDeleteCalTodoAlarm(todoId)
                                BizInsertCalTodoAlarms(scheduleData, todoId, detailData.ActivityCreateStaff)
                                '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 END
                            End Using

                            Using adapter As New IC3040403DataSetTableAdapters.CalEventAlarmDataTable
                                ' アラームを削除し、新しいアラームを入れます。
                                BizDeleteCalEventAlarm(eventId)
                                BizInsertCalEventAlarms(scheduleData, eventId, detailData.ActivityCreateStaff)
                            End Using
                        End If

                        '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    End If
                    '2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 END

                Next

            Else
                ' 削除日に値が入っていた場合、カレンダー、Todo、イベントの全てのテーブルから論理削除する

                ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)
                ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, Nothing, Nothing)
                ' カレンダーICROP情報管理テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalItem(detailData, calenderId)

                ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                '' カレンダーTodo情報テーブルの削除フラグを更新します
                'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                '' カレンダーEvent情報テーブルの削除フラグを更新します
                'BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                ' カレンダーTodo情報テーブルの削除フラグを更新します
                'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, Nothing, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                ' カレンダーEvent情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

            End If
            Return staffCodeList

        End Function

        ''' <summary>
        ''' 処理区分が「更新」のＤＢ処理を行います(受注後工程)
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        '''<history>2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START</history>
        Private Function UpdateAfterOrderDataBase(ByVal detailData As XmlAfterOrderDetail, ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 UpdateAfterOrderDataBase() Start")
            '$01 Add End
            ' カレンダーID
            Dim calenderId As String = Nothing
            '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 START 
            ' TodoId
            Dim todoId As String = Nothing
            ' AfterOdrActID
            Dim AfterOdrActID As String = Nothing
            '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 END 

            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
            Dim eventId As String = Nothing
            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END



            ' ICropの変数から、紐付くカレンダーIDを取得します。
            calenderId = BizGetAfterOrderCalenderId(detailData)
            '$01 Add Start
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            ' カレンダーＩＤが空の場合、更新、削除処理が実行できないので終了します。
            If calenderId Is Nothing Then
                '$01 Add Start
                Logger.Info("IC3040403 UpdateAfterOrderDataBase() End")
                '$01 Add End
                Return staffCodeList
            End If

            '$01 Add Start
            Logger.Info("IC3040403 detailData.DeleteDate:" + detailData.DeleteDate)
            '$01 Add End
            '削除日の値がNullか入っていない場合。
            If detailData.DeleteDate Is Nothing Or Validation.Equals(detailData.DeleteDate, EmptyString) Then
                '$01 Add Start
                Logger.Info("IC3040403 detailData.ScheduleInfoFlg:" + detailData.ScheduleInfoFlg.ToString())
                '$01 Add End
                ' scheduleInfoが存在する場合、カレンダーを更新する。
                If detailData.ScheduleInfoFlg = True Then

                    ' 紐付くカレンダーIdが存在するのであれば、更新します。
                    If calenderId IsNot Nothing Then

                        ' カレンダーICROP情報管理テーブルを更新します
                        BizUpdateAfterOrderCalCalender(detailData, calenderId)

                    End If

                End If

                ' Schedule要素の数だけ、更新処理を行います
                For Each scheduleData As XmlAfterOrderSchedule In detailData.ScheduleList
                    '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 START
                    'todoId = scheduleData.TodoId

                    '2014/07/28 SKFC NEXTSTEP CalDAV 不具合修正 START

                    AfterOdrActID = scheduleData.AfterOdrActID
                    '$01 Add Start
                    Logger.Info("IC3040403 AfterOdrActID:" + AfterOdrActID)
                    '$01 Add End
                    If AfterOdrActID Is Nothing Then
                        AfterOdrActID = scheduleData.AfterOdrActID
                    End If

                    '2014/07/28 SKFC NEXTSTEP CalDAV 不具合修正 END
                    If todoId Is Nothing Then

                        todoId = BizGetTodoId(calenderId, scheduleData.ProcessDiv)

                    End If
                    '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 END
                    '$01 Add Start
                    Logger.Info("IC3040403 todoId:" + todoId)
                    '$01 Add End
                    '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 START
                    'if todoId Is Nothing Then
                    If AfterOdrActID Is Nothing Then

                        ''TODOIDが取得できていない場合
                        'AfterOdrActIDが取得できていない場合
                        'Logger.Info("Don't Sherch TodoID CalendarID=[" & calenderId & "] ProsessDiv=[" & scheduleData.ProcessDiv & "]")
                        Logger.Info("Don't Sherch AfterOdrActID CalendarID=[" & calenderId & "] ProsessDiv=[" & scheduleData.ProcessDiv & "]")
                        '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 END
                    Else

                        ' 更新なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                        staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                        staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                        ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                        '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 START
                        'staffCodeList = GetAfterOrderStaffCodeTodoItem(staffCodeList, calenderId, Nothing,  scheduleData)
                        staffCodeList = GetAfterOrderStaffCodeTodoItem(staffCodeList, calenderId, Nothing, AfterOdrActID, scheduleData)
                        ' Todoテーブルを更新します
                        'BizUpdateAfterOrderCalTodoItem(scheduleData, todoId, detailData.ActivityCreateStaff)
                        BizUpdateAfterOrderCalTodoItem(scheduleData, AfterOdrActID, detailData.ActivityCreateStaff)
                        '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 END

                        ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                        ' Eventテーブルを更新します
                        BizUpdateAfterOrderCalEventItem(scheduleData, todoId, detailData.ActivityCreateStaff)
                        ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END


                        '$01 Add Start
                        Logger.Info("IC3040403 scheduleData.AlarmTriggerList.Count:" + scheduleData.AlarmTriggerList.Count.ToString())
                        '$01 Add End
                        ' アラームの項目が存在する場合、アラームを登録します
                        If scheduleData.AlarmTriggerList.Count > 0 Then
                            Using adapter As New IC3040403DataSetTableAdapters.CalTodoAlarmDataTable
                                ' アラームを削除し、新しいアラームを入れます。
                                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 START
                                'BizDeleteCalTodoAlarm(todoId)
                                'BizInsertAfterOrderCalTodoAlarms(scheduleData, todoId, detailData.ActivityCreateStaff)
                                BizDeleteCalTodoAlarm(Nothing)
                                BizInsertAfterOrderCalTodoAlarms(scheduleData, Nothing, detailData.ActivityCreateStaff)
                                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 END
                            End Using

                            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                            Using adapter As New IC3040403DataSetTableAdapters.CalEventAlarmDataTable
                                ' アラームを削除し、新しいアラームを入れます。
                                BizDeleteCalEventAlarm(eventId)
                                BizInsertAfterOrderCalEventAlarms(scheduleData, eventId, detailData.ActivityCreateStaff)
                            End Using
                            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END

                        End If

                    End If

                Next

            Else
                ' 削除日に値が入っていた場合、カレンダー、Todo、イベントの全てのテーブルから論理削除する

                ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)
                ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, Nothing, Nothing)
                ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END

                ' カレンダーICROP情報管理テーブルの削除フラグを更新します
                BizUpdateAfterOrderDeleteFlgCalItem(detailData, calenderId)
                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 START
                ' カレンダーTodo情報テーブルの削除フラグを更新します
                'BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                BizUpdateDeleteFlgCalTodoItem(calenderId, Nothing, AfterOdrActID, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正 END
                ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                ' カレンダーEvent情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff, Nothing)
                ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END

            End If
            Return staffCodeList

        End Function


        ''' <summary>
        ''' 処理区分が「イベント更新」のＤＢ処理を行います
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function EventDataBase(ByVal detailData As XmlDetail, ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 EventDataBase() Start")
            '$01 Add End
            ' EventId
            Dim eventId As String

            ' Schedule要素の数だけ、紐付きイベントを追加します
            For Each scheduleData As XmlSchedule In detailData.ScheduleList

                ' スタッフコードをスタッフコードリストに追加します
                staffCodeList = GetStaffCodeTodoItem(staffCodeList, Nothing, scheduleData.TodoId, Nothing, Nothing)

                ' イベントＩＤを取得します
                eventId = BizGetNewEventId()
                '$01 Add Start
                Logger.Info("IC3040403 New eventId" + eventId)
                '$01 Add End
                ' 紐付きイベントを追加します
                BizInsertLinkEvent(scheduleData, scheduleData.TodoId, eventId, detailData.ActivityCreateStaff)

            Next
            '$01 Add Start
            Logger.Info("IC3040403 EventDataBase() End")
            '$01 Add End
            Return staffCodeList

        End Function

        ''' <summary>
        ''' 処理区分が「削除」のＤＢ処理を行います(受注後工程)
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        '''<history>2014/07/23 SKFC 渡邊 NEXTSTEP_CALDAV 仕様変更 START</history>
        Private Function DeleteAfterOrderDataBase(ByVal detailData As XmlAfterOrderDetail, _
                                                  ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 DeleteAfterOrderDataBase() Start")
            '$01 Add End
            ' カレンダーID
            Dim calenderId As String = Nothing

            ' ICropの変数から、紐付くカレンダーIDを取得します
            calenderId = BizGetAfterOrderCalenderId(detailData)
            '$01 Add Start
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            ' カレンダーＩＤが空の場合、削除処理が実行できないので終了します
            If calenderId Is Nothing Then
                '$01 Add Start
                Logger.Info("IC3040403 DeleteAfterOrderDataBase() End")
                '$01 Add End
                Return staffCodeList
            End If
            '$01 Add Start
            Logger.Info("IC3040403 detailData.DeleteDate:" + detailData.DeleteDate)
            '$01 Add End
            ' 削除日に値が入っていた場合、カレンダー、Todo、イベントの全てのテーブルから論理削除します
            If detailData.DeleteDate IsNot Nothing Then

                ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)

                ' カレンダーEvent情報テーブルの削除フラグを更新します
                BizUpdateAfterOrderDeleteFlgCalEventItem(calenderId, Nothing, detailData.DeleteDate, detailData.ActivityCreateStaff)

                ' 2014/07/25 SKFC渡邊 NextStep CalDAV 不具合修正 START
                '' カレンダーICROP情報管理テーブルの削除フラグを更新します
                'BizUpdateAfterOrderDeleteFlgCalItem(detailData, calenderId)
                ' 2014/07/25 SKFC渡邊 NextStep CalDAV 不具合修正 END

                ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)

                ' カレンダーTodo情報テーブルの削除フラグを更新します
                BizUpdateAfterOrderDeleteFlgCalTodoItem(calenderId, Nothing, detailData.DeleteDate, detailData.ActivityCreateStaff)


                '削除日が設定されていない場合、処理を終了します
            Else
                '$01 Add Start
                Logger.Info("IC3040403 DeleteAfterOrderDataBase() End")
                '$01 Add End
                Return staffCodeList

            End If
            Return staffCodeList

        End Function

        ''' <summary>
        ''' 今回の処理で使用したスタッフコードをカレンダーアドレス最終更新日テーブルに更新／追加をします
        ''' </summary>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <param name="detailData">更新に必要な値（機能ＩＤ，アカウント）</param>
        ''' <remarks></remarks>
        Private Sub SetStaffCode(ByVal staffCodeList As List(Of String), ByVal detailData As XmlDetail)
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertLinkEvent() Start")
            '$01 Add End
            For Each staffCode As String In staffCodeList
                '$01 Add Start
                Logger.Info("IC3040403 staffCode:" + staffCode)
                '$01 Add End
                If staffCode Is Nothing Or Validation.Equals(staffCode, EmptyString) Then

                    ' 空文字や、Nothingの場合は処理を行わない
                Else

                    ' 更新処理をします
                    Dim updateCount As Integer = BizUpdateCalCardLastModify(staffCode, detailData.ActivityCreateStaff)

                    ' １件も更新できなかった→新規登録
                    If updateCount = 0 Then

                        BizInsertCalCardLastModify(staffCode, detailData.ActivityCreateStaff)

                    End If

                End If

            Next
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertLinkEvent() End")
            '$01 Add End
        End Sub

        ''' <summary>
        ''' 今回の処理で使用したスタッフコードをカレンダーアドレス最終更新日テーブルに更新／追加をします
        ''' </summary>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <param name="detailData">更新に必要な値（機能ＩＤ，アカウント）</param>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        ''' <remarks></remarks>
        Private Sub SetAfterOrderStaffCode(ByVal staffCodeList As List(Of String), ByVal detailData As XmlAfterOrderDetail)
            '$01 Add Start
            Logger.Info("IC3040403 SetAfterOrderStaffCode() Start")
            '$01 Add End
            For Each staffCode As String In staffCodeList
                '$01 Add Start
                Logger.Info("IC3040403 staffCode:" + staffCode)
                '$01 Add End
                If staffCode Is Nothing Or Validation.Equals(staffCode, EmptyString) Then

                    ' 空文字や、Nothingの場合は処理を行わない
                Else

                    ' 更新処理をします
                    Dim updateCount As Integer = BizUpdateCalCardLastModify(staffCode, detailData.ActivityCreateStaff)
                    '$01 Add Start
                    Logger.Info("IC3040403 updateCount:" + updateCount.ToString())
                    '$01 Add End
                    ' １件も更新できなかった→新規登録
                    If updateCount = 0 Then

                        BizInsertCalCardLastModify(staffCode, detailData.ActivityCreateStaff)

                    End If

                End If

            Next
            '$01 Add Start
            Logger.Info("IC3040403 SetAfterOrderStaffCode() End")
            '$01 Add End
        End Sub



        ''' <summary>
        ''' 子ノードの要素を取得します、但し、中身が空文字の場合はエラーとします
        ''' </summary>
        ''' <param name="parentsNode">親ノード</param>
        ''' <param name="childNodeName">子ノード名</param>
        ''' <param name="maximumOfDigit">子ノードの要素の最大桁数</param>
        ''' <param name="dataAssignmentMode">要素の割り当て状態</param>
        ''' <param name="elementCode">エラー出力用の要素コード</param>
        ''' <param name="type">入力チェックの形式</param>
        ''' <returns>要素内の値</returns>
        ''' <remarks></remarks>
        Private Function GetNodeInnerTextNotEmpty(ByVal parentsNode As XmlNode, _
                                                  ByVal childNodeName As String, _
                                                  ByVal dataAssignmentMode As Integer, _
                                                  ByVal maximumOfDigit As Integer, _
                                                  ByVal type As Integer, _
                                                  ByVal elementCode As Integer) As String

            Dim innerText As String = GetNodeInnerText(parentsNode, childNodeName, dataAssignmentMode, maximumOfDigit, type, elementCode)

            If Validation.Equals(innerText, EmptyString) Then

                Throw New ApplicationException(CType(ReturnCode.NotXmlElementError + elementCode, String))

            End If

            Return innerText

        End Function



        ''' <summary>
        ''' 子ノードの要素を取得します。
        ''' </summary>
        ''' <param name="parentsNode">親ノード</param>
        ''' <param name="childNodeName">子ノード名</param>
        ''' <param name="maximumOfDigit">子ノードの要素の最大桁数</param>
        ''' <param name="dataAssignmentMode">要素の割り当て状態</param>
        ''' <param name="elementCode">エラー出力用の要素コード</param>
        ''' <param name="type">入力チェックの形式</param>
        ''' <returns>要素内の値</returns>
        ''' <remarks></remarks>
        Private Function GetNodeInnerText(ByVal parentsNode As XmlNode, ByVal childNodeName As String, ByVal dataAssignmentMode As Integer, ByVal maximumOfDigit As Integer, ByVal type As Integer, ByVal elementCode As Integer) As String


            ' 要素を取得します。
            Dim childNode As XmlNode = GetChildNode(parentsNode, childNodeName, dataAssignmentMode, elementCode)

            If childNode IsNot Nothing Then

                ' 必須項目で、尚且つ要素内が空の場合は必須項目がないのでエラーとなる
                If Validation.Equals(childNode.InnerText, EmptyString) And dataAssignmentMode = DataAssignment.ModeMandatory Then

                    ' 必須項目に対してノードが存在しないのでエラー
                    Throw New ApplicationException(CType(ReturnCode.NotXmlElementError + elementCode, String))

                End If

                ' 取得した要素をチェックします。
                If Validation.Equals(childNode.InnerText, EmptyString) Then
                    ' 要素内が空の場合、空文字列を返します
                    Return EmptyString

                End If

                Dim isCheck As Boolean = IsCheckElement(childNode.InnerText, maximumOfDigit, type, elementCode)

                If isCheck Then

                    ' 正常な場合、要素を返却します。
                    Return childNode.InnerText

                End If

            End If

            Return Nothing

        End Function


        ''' <summary>
        ''' 国コードを返します
        ''' </summary>
        ''' <remarks>WebConfigから取得する方法が不明な為、現在はマジックコードで処理。（12/4現在）</remarks>
        Private Function GetCountryCode() As String

            Return EnvironmentSetting.CountryCode

        End Function


#End Region

#Region "入力チェック"

        ''' <summary>
        ''' 文字列型の値と、数値型の値（Enumの中の値）が同一かチェックをします。
        ''' </summary>
        ''' <param name="stringColum">文字列型の値</param>
        ''' <param name="integerColum">Enumの値</param>
        ''' <returns>True or False</returns>
        ''' <remarks></remarks>
        Private Function IsFlgEquals(ByVal stringColum As String, ByVal integerColum As Integer)

            If Validation.Equals(stringColum, CType(integerColum, String)) Then

                Return True

            End If

            Return False

        End Function

        ''' <summary>
        ''' 最大桁数チェックと、型チェックを行います
        ''' </summary>
        ''' <param name="target">チェック対象の文字列</param>
        ''' <param name="maximumOfDigit">最大桁数</param>
        ''' <param name="type">型チェックを行う型</param>
        ''' <param name="elementCode">エラー出力用の要素コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsCheckElement(ByVal target As String, ByVal maximumOfDigit As Integer, ByVal type As Integer, ByVal elementCode As Integer) As Boolean

            If maximumOfDigit = 0 Then

                Return True
            End If

            ' 最大桁数チェックを行います
            If Validation.IsCorrectDigit(target, maximumOfDigit) Then

                Dim isCheck As Boolean = False

                Select Case type
                    Case TypeConversion.None
                        ' noneはチェックしない
                        isCheck = True

                    Case TypeConversion.StringType
                        ' 元々文字列型なのでチェックの必要性はない
                        isCheck = True

                    Case TypeConversion.IntegerType
                        ' 整数型のチェックをします

                        Try
                            '2013/07/17 SKFC 森 既存流用 Start
                            'Dim dummy As Integer = CType(target, Integer)
                            Dim dummy As Decimal = CType(target, Decimal)
                            '2013/07/17 SKFC 森 既存流用 End

                            dummy = dummy + 0

                            isCheck = True

                        Catch ex As SystemException
                            Throw New ApplicationException(CType(ReturnCode.XmlParseError + elementCode, String))
                        End Try

                    Case TypeConversion.DateType
                        ' 日付型のチェックをします
                        Try

                            Dim rtnDate As Date = Nothing

                            If Len(target) = 10 Then
                                rtnDate = DateTimeFunc.FormatString("yyyy/MM/dd", target)
                            Else
                                rtnDate = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm:ss", target)
                            End If
                            isCheck = True
                        Catch ex As FormatException
                            Throw New ApplicationException(CType(ReturnCode.XmlParseError + elementCode, String))
                        End Try
                End Select

                If isCheck Then
                    Return isCheck
                End If
                ' 値チェックに失敗した場合、エラーとします。
                Throw New ApplicationException(CType(ReturnCode.XmlParseError + elementCode, String))
            Else
                ' 値チェックに失敗した場合、エラーとします。
                Throw New ApplicationException(CType(ReturnCode.XmlMaximumOfDigitError + elementCode, String))
            End If

        End Function

#End Region

#Region "入力値判別"

        ''' <summary>
        ''' 開始日指定フラグがあるかないか判別します
        ''' </summary>
        ''' <param name="startTime">開始日</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function checkStartTimeFlg(ByVal startTime As String) As String

            If startTime Is Nothing Or Validation.Equals(startTime, EmptyString) Then

                Return StartTimeFlgNo

            End If

            Return StartTimeFlgYes

        End Function

        ''' <summary>
        ''' 時刻指定フラグがあるかないか判別します
        ''' </summary>
        ''' <param name="endTime">終了日</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function checkTimeFlg(ByVal endTime As String) As String

            If endTime Is Nothing Then

                Return TimeFlgNo

            End If

            If endTime.Length = DateLength Then

                Return TimeFlgNo

            End If

            Return TimeFlgYes

        End Function

        ''' <summary>
        ''' 終日フラグがあるかないか判別します
        ''' </summary>
        ''' <param name="startTime">開始日</param>
        ''' <param name="endTime">終了日</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function checkAlldayFlg(ByVal startTime As String,
                                           ByVal endTime As String) As String

            If Validation.Equals(startTime, EmptyString) Then

                Return AllDayFlgNo
            End If

            If Validation.Equals(endTime, EmptyString) Then

                Return AllDayFlgNo
            End If

            If startTime Is Nothing Or endTime Is Nothing Then

                Return AllDayFlgNo

            End If

            If startTime.Length = DateLength AndAlso endTime.Length = DateLength Then

                Return AllDayFlgYes

            End If

            Return AllDayFlgNo

        End Function

        ''' <summary>
        ''' スタッフコードチェックを行います。
        ''' </summary>
        ''' <param name="scheduleDivString">スケジュール区分</param>
        ''' <param name="activityStaffCode">活動スタッフコード</param>
        ''' <param name="receptionStaffCode">受付担当スタッフコード</param>
        ''' <remarks></remarks>
        Private Sub CheckStaffCode(ByVal scheduleDivString As String, _
                                        ByVal activityStaffCode As String, _
                                        ByVal receptionStaffCode As String)
            '$01 Add Start
            Logger.Info("IC3040403 CheckStaffCode() Start")
            Logger.Info("IC3040403 scheduleDivString:" + scheduleDivString)
            Logger.Info("IC3040403 activityStaffCode:" + activityStaffCode)
            Logger.Info("IC3040403 receptionStaffCode:" + receptionStaffCode)
            '$01 Add End
            ' 活動スタッフコードの要素内が空欄の場合、Nothingとしてチェックを行う
            If Validation.Equals(activityStaffCode, EmptyString) Then

                activityStaffCode = Nothing

            End If

            ' 受付スタッフコードの要素内が空欄の場合、Nothingとしてチェックを行う
            If Validation.Equals(receptionStaffCode, EmptyString) Then

                receptionStaffCode = Nothing

            End If

            '$01 Add Start
            Logger.Info("IC3040403 ScheDuleDiv.VisitReservation:" + ScheDuleDiv.VisitReservation.ToString())
            '$01 Add End
            ' スケジュール区分が来店予約の場合
            If IsFlgEquals(scheduleDivString, ScheDuleDiv.VisitReservation) Then

                ' 活動スタッフコードが設定されていて、受付担当スタッフコードが未設定の場合、正常
                If activityStaffCode IsNot Nothing And _
                    receptionStaffCode Is Nothing Then
                    '$01 Add Start
                    Logger.Info("IC3040403 CheckStaffCode() Normal End")
                    '$01 Add End
                    Return

                End If
                '$01 Add Start
                Logger.Info("IC3040403 CheckStaffCode() Error End")
                '$01 Add End
                'それ以外は全てエラーデータとする
                Throw New ApplicationException(ReturnCode.UniqueError + ReturnCode.StaffCodeError)

            End If
            '$01 Add Start
            Logger.Info("IC3040403 ScheDuleDiv.GRReservattion:" + ScheDuleDiv.GRReservattion.ToString())
            '$01 Add End
            ' スケジュール区分が入庫予約の場合
            If IsFlgEquals(scheduleDivString, ScheDuleDiv.GRReservattion) Then

                '20120124 Modify Start
                '' 活動スタッフコードが設定されていて、受付担当スタッフコードが未設定の場合、正常
                'If activityStaffCode Is Nothing And _
                '    receptionStaffCode Is Nothing Then

                '    'それ以外は全てエラーデータとする
                '    Throw New ApplicationException(ReturnCode.UniqueError + ReturnCode.StaffCodeError)

                'Else

                '    Return

                'End If
                '$01 Add Start
                Logger.Info("IC3040403 CheckStaffCode() Normal End")
                '$01 Add End
                Return
                '20120124 Modify End

            End If


        End Sub

        ''' <summary>
        ''' データテーブル内のスタッフコードをスタッフコードリストに追加します。（但し、重複分は追加しない）
        ''' </summary>
        ''' <param name="targetDataTable">DataTable</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>データテーブル内のスタッフコードを追加したスタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function CheckStaffCodeDataTable(ByVal targetDataTable As IC3040403DataSet.StaffCodeDataTableDataTable, _
                                                ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 CheckStaffCodeDataTable() Start")
            '$01 Add End
            ' dataTableを分解する
            For Each dataRow As IC3040403DataSet.StaffCodeDataTableRow In targetDataTable

                If dataRow.ACTSTAFFCD IsNot Nothing Then

                    staffCodeList = CheckStaffCodeString(dataRow.ACTSTAFFCD, staffCodeList)

                End If

                If dataRow.RECSTAFFCD IsNot Nothing Then

                    staffCodeList = CheckStaffCodeString(dataRow.RECSTAFFCD, staffCodeList)
                End If

            Next
            '$01 Add Start
            Logger.Info("IC3040403 CheckStaffCodeDataTable() End")
            '$01 Add End
            Return staffCodeList

        End Function


        ''' <summary>
        ''' 取得したスタッフコードをスタッフコードリストに追加します（但し、重複分は追加しない）
        ''' </summary>
        ''' <param name="target">対象のスタッフコード</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードを追加したスタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function CheckStaffCodeString(ByVal target As String, ByVal staffCodeList As List(Of String)) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 CheckStaffCodeString() Start")
            Logger.Info("IC3040403 target:" + target)
            '$01 Add End
            If target Is Nothing Then
                '$01 Add Start
                Logger.Info("IC3040403 CheckStaffCodeString() End")
                '$01 Add End
                Return staffCodeList

            End If

            ' 重複が存在するかチェックする
            For Each staffCode As String In staffCodeList

                ' 重複が存在する場合は、処理を終了する。
                If Validation.Equals(target, staffCode) Then
                    '$01 Add Start
                    Logger.Info("IC3040403 CheckStaffCodeString() End")
                    '$01 Add End
                    Return staffCodeList

                End If

            Next

            ' 重複が存在しなかった場合、新しくリストに追加する。
            staffCodeList.Add(target)
            '$01 Add Start
            Logger.Info("IC3040403 CheckStaffCodeString() End")
            '$01 Add End
            Return staffCodeList

        End Function

        ''' <summary>
        ''' データベースを更新することによって、スタッフコードチェックにひっかからないかチェックします
        ''' </summary>
        ''' <param name="dataTable"></param>
        ''' <param name="scheduleDiv"></param>
        ''' <param name="scheduleElement"></param>
        ''' <remarks></remarks>
        Private Sub CheckStaffCodeUpdateDataBase(ByVal dataTable As IC3040403DataSet.StaffCodeDataTableDataTable, _
                                                 ByVal scheduleDiv As String, _
                                                 ByVal scheduleElement As XmlSchedule)
            '$01 Add Start
            Logger.Info("IC3040403 CheckStaffCodeUpdateDataBase() Start")
            Logger.Info("IC3040403 scheduleDiv:" + scheduleDiv)
            '$01 Add End

            For Each dataRow As IC3040403DataSet.StaffCodeDataTableRow In dataTable

                Dim actStaffCode As String = dataRow.ACTSTAFFCD

                Dim recStaffCode As String = dataRow.RECSTAFFCD

                If scheduleElement.ActivityStaffCode IsNot Nothing Then

                    actStaffCode = scheduleElement.ActivityStaffCode

                End If

                If scheduleElement.ReceptionStaffCode IsNot Nothing Then

                    recStaffCode = scheduleElement.ReceptionStaffCode

                End If

                ' スタッフコードチェックを行います
                CheckStaffCode(scheduleDiv, actStaffCode, recStaffCode)

            Next
            '$01 Add Start
            Logger.Info("IC3040403 CheckStaffCodeUpdateDataBase() End")
            '$01 Add End

        End Sub

        ''' <summary>
        ''' データベースを更新することによって、スタッフコードチェックにひっかからないかチェックします(受注後工程)
        ''' </summary>
        ''' <param name="dataTable"></param>
        ''' <param name="scheduleDiv"></param>
        ''' <param name="scheduleElement"></param>
        '''<history>2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Sub CheckAfterOrderStaffCodeUpdateDataBase(ByVal dataTable As IC3040403DataSet.StaffCodeDataTableDataTable, _
                                                           ByVal scheduleDiv As String, _
                                                 ByVal scheduleElement As XmlAfterOrderSchedule)


            For Each dataRow As IC3040403DataSet.StaffCodeDataTableRow In dataTable

                Dim actStaffCode As String = dataRow.ACTSTAFFCD

                Dim recStaffCode As String = dataRow.RECSTAFFCD

                If scheduleElement.ActivityStaffCode IsNot Nothing Then

                    actStaffCode = scheduleElement.ActivityStaffCode

                End If

                If scheduleElement.ReceptionStaffCode IsNot Nothing Then

                    recStaffCode = scheduleElement.ReceptionStaffCode

                End If

                ' スタッフコードチェックを行います
                CheckStaffCode(Nothing, actStaffCode, recStaffCode)

            Next


        End Sub


#End Region

#Region "DataRow格納＆DataAccessに接続"

        ''' <summary>
        ''' カレンダーICROP情報管理テーブルに追加します。
        ''' </summary>
        ''' <param name="detailElement">Detail要素以下のXml</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <returns>追加件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertCalCalender(ByVal detailElement As XmlDetail, ByVal calenderId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertCalCalender() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCalenderDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCalenderDataTableRow = dataTable.NewCalCalenderDataTableRow()

                With dataRow

                    .CALID = calenderId
                    .DLRCD = detailElement.DealerCode
                    .STRCD = detailElement.BranchCode
                    .SCHEDULEID = detailElement.ScheduleId
                    .SCHEDULEDIV = detailElement.ScheduleDiv
                    .CUSTOMERDIV = detailElement.CustomerDiv
                    .CUSTCODE = detailElement.CustomerCode
                    .CUSTNAME = detailElement.CustomerName
                    .DMSID = detailElement.DmsId
                    .RECEPTIONDIV = detailElement.ReceptionDiv
                    .SERVICECODE = detailElement.ServiceCode
                    .MERCHANDISECD = detailElement.MerchandiseCD
                    .STRSTATUS = detailElement.StrStatus
                    .REZSTATUS = detailElement.RezStatus
                    .DELFLG = Delflg.NotDel
                    .DELDATE = Nothing
                    .CREATEACCOUNT = detailElement.ActivityCreateStaff
                    .UPDATEACCOUNT = detailElement.ActivityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalCalenderDataTable

                    Dim count As Integer = adapter.InsertCalCalender(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertCalCalender() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーICROP情報管理テーブルに追加します。(受注後工程)
        ''' </summary>
        ''' <param name="detailElement">Detail要素以下のXml</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <returns>追加件数</returns>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        ''' <remarks></remarks>
        Private Function BizInsertAfterOrderCalCalender(ByVal detailElement As XmlAfterOrderDetail, ByVal calenderId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertAfterOrderCalCalender() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCalenderDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCalenderDataTableRow = dataTable.NewCalCalenderDataTableRow()

                With dataRow

                    .CALID = calenderId
                    .DLRCD = detailElement.DealerCode
                    .STRCD = detailElement.BranchCode
                    .SCHEDULEID = detailElement.ScheduleId
                    .SCHEDULEDIV = TwoString
                    .CUSTOMERDIV = detailElement.CustomerDiv
                    .CUSTCODE = detailElement.CustomerCode
                    '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START
                    '.CUSTNAME = BlankString
                    .CUSTNAME = detailElement.CustomerName
                    '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 END
                    .DMSID = detailElement.DmsId
                    .DELFLG = Delflg.NotDel
                    .DELDATE = Nothing
                    .CREATEACCOUNT = detailElement.ActivityCreateStaff
                    .UPDATEACCOUNT = detailElement.ActivityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalCalenderDataTable

                    Dim count As Integer = adapter.InsertCalCalender(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertAfterOrderCalCalender() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーICROP情報管理を更新します
        ''' </summary>
        ''' <param name="detailElement">Detail要素以下のXml</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateCalCalender(ByVal detailElement As XmlDetail, ByVal calenderId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateCalCalender() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCalenderDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCalenderDataTableRow = dataTable.NewCalCalenderDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = detailElement.ActivityCreateStaff
                    .UPDATEID = UpdateId
                    .CUSTOMERDIV = detailElement.CustomerDiv
                    .CUSTCODE = detailElement.CustomerCode
                    .CUSTNAME = detailElement.CustomerName
                    .DMSID = detailElement.DmsId
                    .RECEPTIONDIV = detailElement.ReceptionDiv
                    .SERVICECODE = detailElement.ServiceCode
                    .MERCHANDISECD = detailElement.MerchandiseCD
                    .STRSTATUS = detailElement.StrStatus
                    .REZSTATUS = detailElement.RezStatus
                    .DELDATE = detailElement.DeleteDate
                    .CALID = calenderId

                End With

                ' カレンダーICROP情報管理テーブルを更新します
                Using adapter As New IC3040403DataSetTableAdapters.CalCalenderDataTable

                    Dim count As Integer = adapter.UpdateCalCalender(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateCalCalender() Start")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーICROP情報管理を更新します(受注後工程)
        ''' </summary>
        ''' <param name="detailElement">Detail要素以下のXml</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <returns>更新件数</returns>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        ''' <remarks></remarks>
        Private Function BizUpdateAfterOrderCalCalender(ByVal detailElement As XmlAfterOrderDetail, ByVal calenderId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateAfterOrderCalCalender() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCalenderDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCalenderDataTableRow = dataTable.NewCalCalenderDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = detailElement.ActivityCreateStaff
                    .UPDATEID = UpdateId
                    .CUSTOMERDIV = detailElement.CustomerDiv
                    .CUSTCODE = detailElement.CustomerCode
                    '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START
                    '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 START
                    ''.CUSTNAME = BlankString
                    '.CUSTNAME = Nothing
                    ''2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 END
                    .CUSTNAME = detailElement.CustomerName
                    '2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 END
                    .DMSID = detailElement.DmsId
                    .RECEPTIONDIV = Nothing
                    .SERVICECODE = Nothing
                    .MERCHANDISECD = Nothing
                    .STRSTATUS = Nothing
                    .REZSTATUS = Nothing
                    .DELDATE = detailElement.DeleteDate
                    .CALID = calenderId


                End With

                ' カレンダーICROP情報管理テーブルを更新します
                Using adapter As New IC3040403DataSetTableAdapters.CalCalenderDataTable

                    Dim count As Integer = adapter.UpdateCalCalender(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateAfterOrderCalCalender() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーICROP情報管理の削除フラグを更新します
        ''' </summary>
        ''' <param name="detailElement">Detail要素以下のXml</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateDeleteFlgCalItem(ByVal detailElement As XmlDetail, ByVal calenderId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateDeleteFlgCalItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCalenderDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCalenderDataTableRow = dataTable.NewCalCalenderDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = detailElement.ActivityCreateStaff
                    .UPDATEID = UpdateId
                    .DELFLG = CType(Delflg.Del + 0, String)
                    .DELDATE = detailElement.DeleteDate
                    .CALID = calenderId


                End With

                ' カレンダーICROP情報管理テーブルの削除フラグを更新します
                Using adapter As New IC3040403DataSetTableAdapters.CalCalenderDataTable

                    Dim count As Integer = adapter.UpdateDeleteFlgCalItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateDeleteFlgCalItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーICROP情報管理の削除フラグを更新します
        ''' </summary>
        ''' <param name="detailElement">Detail要素以下のXml</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <returns>更新件数</returns>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        ''' <remarks></remarks>
        Private Function BizUpdateAfterOrderDeleteFlgCalItem(ByVal detailElement As XmlAfterOrderDetail, ByVal calenderId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateAfterOrderDeleteFlgCalItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCalenderDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCalenderDataTableRow = dataTable.NewCalCalenderDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = detailElement.ActivityCreateStaff
                    .UPDATEID = UpdateId
                    .DELFLG = CType(Delflg.Del + 0, String)
                    .DELDATE = detailElement.DeleteDate
                    .CALID = calenderId


                End With

                ' カレンダーICROP情報管理テーブルの削除フラグを更新します
                Using adapter As New IC3040403DataSetTableAdapters.CalCalenderDataTable

                    Dim count As Integer = adapter.UpdateDeleteFlgCalItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateAfterOrderDeleteFlgCalItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーTodo情報テーブルに登録します
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素以下の値</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="activityCreateStaff">作成／更新アカウント用のスタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertCalTodoItem(ByVal scheduleElement As XmlSchedule, _
                                          ByVal calenderId As String, _
                                          ByVal todoId As String, _
                                          ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertCalTodoItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoItemDataTableRow = dataTable.NewCalTodoItemDataTableRow()

                With dataRow

                    .TODOID = todoId
                    .CALID = calenderId
                    Using adapter As New IC3040403DataSetTableAdapters.SequenceTable
                        .UNIQUEID = adapter.GetNewUniqueId(GetCountryCode())
                    End Using
                    .RECURRENCEID = RecurrenceIdElement
                    .CHGSEQNO = 0
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    .STARTTIMEFLG = checkStartTimeFlg(scheduleElement.StartTime)
                    .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .RRULE_FREQ = RruleNone
                    .RRULE_INTERVAL = Nothing
                    .RRULE_UNTIL = Nothing
                    .RRULE_TEXT = Nothing
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    '.COMPLETIONFLG = 0
                    '.COMPLETIONDATE = Nothing
                    If (scheduleElement.ResultDate Is Nothing Or Validation.Equals(scheduleElement.ResultDate, EmptyString)) Then
                        .COMPLETIONFLG = CType(DBCompletionFlg.CheckOff, String)
                        .COMPLETIONDATE = Nothing
                    Else
                        .COMPLETIONFLG = CType(DBCompletionFlg.CheckOn, String)
                        .COMPLETIONDATE = scheduleElement.ResultDate
                    End If
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .DELFLG = 0
                    .DELDATE = Nothing
                    .CREATEACCOUNT = activityCreateStaff
                    .UPDATEACCOUNT = activityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId
                    .PARENTDIV = scheduleElement.ParentDiv
                    '2014/04/03 SKFC 渡邊 NEXT_STEP START
                    '' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    '.PROCESSDIV = scheduleElement.ProcessDiv
                    '' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    If (scheduleElement.ContactName IsNot Nothing) Then
                        .CONTACT_NAME = scheduleElement.ContactName
                    Else
                        .CONTACT_NAME = BlankString
                    End If
                    .ACT_ODR_NAME = BlankString
                    If (scheduleElement.OdrDiv IsNot Nothing) Then
                        .ODR_DIV = scheduleElement.OdrDiv
                    Else
                        .ODR_DIV = BlankString
                    End If
                    .AFTER_ODR_ACT_ID = BlankString
                    '2014/04/03 SKFC 渡邊 NEXT_STEP START

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable
                    Dim count As Integer = adapter.InsertCalTodoItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertCalTodoItem() End")
                    '$01 Add End
                    Return count
                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーTodo情報テーブルに登録します
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素以下の値</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="activityCreateStaff">作成／更新アカウント用のスタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        ''' <remarks></remarks>
        Private Function BizInsertAfterOrderCalTodoItem(ByVal scheduleElement As XmlAfterOrderSchedule, _
                                          ByVal calenderId As String, _
                                          ByVal todoId As String, _
                                          ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertAfterOrderCalTodoItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoItemDataTableRow = dataTable.NewCalTodoItemDataTableRow()

                With dataRow

                    .TODOID = todoId
                    .CALID = calenderId
                    Using adapter As New IC3040403DataSetTableAdapters.SequenceTable
                        .UNIQUEID = adapter.GetNewUniqueId(GetCountryCode())
                    End Using
                    .RECURRENCEID = RecurrenceIdElement
                    .CHGSEQNO = 0
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    .STARTTIMEFLG = checkStartTimeFlg(scheduleElement.StartTime)
                    .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .RRULE_FREQ = RruleNone
                    .RRULE_INTERVAL = Nothing
                    .RRULE_UNTIL = Nothing
                    .RRULE_TEXT = Nothing
                    .COMPLETIONDATE = Nothing
                    .COMPLETIONFLG = ZeroString
                    .DELFLG = 0
                    .DELDATE = Nothing
                    .CREATEACCOUNT = activityCreateStaff
                    .UPDATEACCOUNT = activityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId
                    .PARENTDIV = BlankString
                    .PROCESSDIV = scheduleElement.ProcessDiv
                    .CONTACT_NAME = scheduleElement.ContactName
                    .ACT_ODR_NAME = scheduleElement.ActOdrName
                    .ODR_DIV = scheduleElement.OdrDiv
                    .AFTER_ODR_ACT_ID = scheduleElement.AfterOdrActID
                    '2014/06/27 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 START
                    If (scheduleElement.ResultDate Is Nothing Or Validation.Equals(scheduleElement.ResultDate, EmptyString)) Then
                        .COMPLETIONFLG = CType(DBCompletionFlg.CheckOff, String)
                        .COMPLETIONDATE = Nothing
                    Else
                        .COMPLETIONFLG = CType(DBCompletionFlg.CheckOn, String)
                        .COMPLETIONDATE = scheduleElement.ResultDate
                    End If
                    '2014/06/27 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 END

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable
                    Dim count As Integer = adapter.InsertCalTodoItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertAfterOrderCalTodoItem() End")
                    '$01 Add End
                    Return count
                End Using

            End Using

        End Function


        ''' <summary>
        ''' カレンダーTodo情報テーブルを更新する
        ''' </summary>
        ''' <param name="scheduleElement">スケジュール要素</param>
        ''' <param name="todoId">todoId</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateCalTodoItem(ByVal scheduleElement As XmlSchedule, _
                                                        ByVal todoId As String, _
                                                        ByVal activityCreateStaff As String) As Integer

            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateCalTodoItem() Start")
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoItemDataTableRow = dataTable.NewCalTodoItemDataTableRow

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    If scheduleElement.StartTime Is Nothing Then
                        .STARTTIMEFLG = Nothing
                    Else
                        .STARTTIMEFLG = checkStartTimeFlg(scheduleElement.StartTime)
                    End If
                    If scheduleElement.EndTime Is Nothing Then
                        .TIMEFLG = Nothing
                    Else
                        .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    End If
                    If scheduleElement.StartTime IsNot Nothing And scheduleElement.EndTime IsNot Nothing Then
                        .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    Else
                        .ALLDAYFLG = Nothing
                    End If
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .TODOID = todoId
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    If scheduleElement.ProcessDiv IsNot Nothing Then

                        .PROCESSDIV = scheduleElement.ProcessDiv

                        '2012/03/12 SKFC 加藤 【SALES_2】<ResultDate>タグなしが入ってくる対応 START
                        'If scheduleElement.ResultDate Is Nothing Then
                        '2012/04/09 SKFC 上田 【SALES_2】受注後工程の対応 START
                        'If (scheduleElement.ResultDate Is Nothing Or Validation.Equals(scheduleElement.ResultDate, EmptyString)) Then
                        If (scheduleElement.ResultDate IsNot Nothing) Then
                            If (Validation.Equals(scheduleElement.ResultDate, EmptyString)) Then
                                '2012/04/09 SKFC 上田 【SALES_2】受注後工程の対応 END
                                '2012/03/12 SKFC 加藤 【SALES_2】なしタグが入ってくる対応 END

                                '実績日がセットされていない場合は、実績のキャンセルを行う。
                                .COMPLETIONFLG = CType(DBCompletionFlg.CheckOff, String)
                                '2012/04/09 SKFC 上田 【SALES_2】受注後工程の対応 START
                                '.COMPLETIONDATE = Nothing
                                .COMPLETIONDATE = EmptyString
                                '2012/04/09 SKFC 上田 【SALES_2】受注後工程の対応 END
                            Else
                                '実績日がセットされている場合は、実績の登録を行う。
                                .COMPLETIONFLG = CType(DBCompletionFlg.CheckOn, String)
                                .COMPLETIONDATE = scheduleElement.ResultDate
                            End If
                            '2012/04/09 SKFC 上田 【SALES_2】受注後工程の対応 START
                        End If
                        '2012/04/09 SKFC 上田 【SALES_2】受注後工程の対応 END
                    End If
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 START
                    ''2014/04/03 SKFC 渡邊 NEXT_STEP START
                    'If (scheduleElement.ContactName IsNot Nothing) Then
                    '    .CONTACT_NAME = scheduleElement.ContactName
                    'Else
                    '    .CONTACT_NAME = BlankString
                    'End If
                    '.ACT_ODR_NAME = BlankString
                    'If (scheduleElement.OdrDiv IsNot Nothing) Then
                    '    .ODR_DIV = scheduleElement.OdrDiv
                    'Else
                    '    .ODR_DIV = BlankString
                    'End If
                    '.AFTER_ODR_ACT_ID = BlankString
                    ''2014/04/03 SKFC 渡邊 NEXT_STEP END
                    .CONTACT_NAME = scheduleElement.ContactName
                    .ACT_ODR_NAME = Nothing
                    .ODR_DIV = scheduleElement.OdrDiv
                    .AFTER_ODR_ACT_ID = Nothing
                    '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 END



                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalTodoItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateCalTodoItem > GetCalTodoItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateCalTodoItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateCalTodoItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーTodo情報テーブルを更新する
        ''' </summary>
        ''' <param name="scheduleElement">スケジュール要素</param>
        ''' <param name="afterodractid">afterodractid</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <history>2014/04/03 SKFC 渡邊 NEXT_STEP START</history>
        ''' <remarks></remarks>
        Private Function BizUpdateAfterOrderCalTodoItem(ByVal scheduleElement As XmlAfterOrderSchedule, _
                                                        ByVal afterodractId As String, _
                                                        ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateAfterOrderCalTodoItem() Start")
            Logger.Info("IC3040403 afterodractId:" + afterodractId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End

            Using dataTable As New IC3040403DataSet.CalTodoItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoItemDataTableRow = dataTable.NewCalTodoItemDataTableRow

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    If scheduleElement.StartTime Is Nothing Then
                        .STARTTIMEFLG = Nothing
                    Else
                        .STARTTIMEFLG = checkStartTimeFlg(scheduleElement.StartTime)
                    End If
                    If scheduleElement.EndTime Is Nothing Then
                        .TIMEFLG = Nothing
                    Else
                        .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    End If
                    If scheduleElement.StartTime IsNot Nothing And scheduleElement.EndTime IsNot Nothing Then
                        .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    Else
                        .ALLDAYFLG = Nothing
                    End If
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .TODOID = scheduleElement.TodoId
                    '2014/07/28 SKFC渡邊 NEXTSTEP CalDAV 不具合修正 START
                    '.PROCESSDIV = scheduleElement.ProcessDiv
                    '.COMPLETIONFLG = ZeroString
                    '.COMPLETIONDATE = Nothing
                    '2014/07/28 SKFC渡邊 NEXTSTEP CalDAV 不具合修正 END
                    '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 START
                    'If (scheduleElement.ContactName IsNot Nothing) Then
                    '    .CONTACT_NAME = scheduleElement.ContactName
                    'Else
                    '    .CONTACT_NAME = BlankString
                    'End If
                    'If (scheduleElement.ActOdrName IsNot Nothing) Then
                    '    .ACT_ODR_NAME = scheduleElement.ActOdrName
                    'Else
                    '    .ACT_ODR_NAME = BlankString
                    'End If
                    .CONTACT_NAME = scheduleElement.ContactName
                    .ACT_ODR_NAME = scheduleElement.ActOdrName
                    '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 END
                    .ODR_DIV = scheduleElement.OdrDiv
                    .AFTER_ODR_ACT_ID = afterodractId
                    '2014/07/28 SKFC渡邊 NEXTSTEP CalDAV 不具合修正 START
                    If scheduleElement.ProcessDiv IsNot Nothing Then

                        .PROCESSDIV = scheduleElement.ProcessDiv

                        If (scheduleElement.ResultDate IsNot Nothing) Then
                            If (Validation.Equals(scheduleElement.ResultDate, EmptyString)) Then

                                '実績日がセットされていない場合は、実績のキャンセルを行う。
                                .COMPLETIONFLG = CType(DBCompletionFlg.CheckOff, String)
                                .COMPLETIONDATE = EmptyString
                            Else
                                '実績日がセットされている場合は、実績の登録を行う。
                                .COMPLETIONFLG = CType(DBCompletionFlg.CheckOn, String)
                                .COMPLETIONDATE = scheduleElement.ResultDate
                            End If
                        End If
                    End If

                    ''2014/06/27 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 START
                    'If (scheduleElement.ResultDate IsNot Nothing) Then
                    'If (Validation.Equals(scheduleElement.ResultDate, EmptyString)) Then
                    ''実績日がセットされていない場合は、実績のキャンセルを行う。
                    '.COMPLETIONFLG = CType(DBCompletionFlg.CheckOff, String)
                    '.COMPLETIONDATE = EmptyString
                    'Else
                    ''実績日がセットされている場合は、実績の登録を行う。
                    '.COMPLETIONFLG = CType(DBCompletionFlg.CheckOn, String)
                    '.COMPLETIONDATE = scheduleElement.ResultDate
                    'End If
                    'End If
                    ''2014/06/27 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 END
                    '2014/07/28 SKFC渡邊 NEXTSTEP CalDAV 不具合修正 START

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalTodoItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateAfterOrderCalTodoItem > GetCalTodoItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateCalTodoItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateAfterOrderCalTodoItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function


        ''' <summary>
        ''' カレンダーTodo情報テーブルの削除フラグを更新する
        ''' </summary>
        ''' <param name="calenderId">calenderId</param>
        ''' <param name="todoId">todoId</param>
        ''' <param name="delDate">削除日</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <param name="CompletionFlg">完了フラグ</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateDeleteFlgCalTodoItem(ByVal calenderId As String, _
                                                        ByVal todoId As String, _
                                                        ByVal afterodractId As String, _
                                                        ByVal delDate As String, _
                                                        ByVal activityCreateStaff As String, _
                                                        ByVal CompletionFlg As String) As Integer
            '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
            'Private Function BizUpdateDeleteFlgCalTodoItem(ByVal calenderId As String, _
            '                                    ByVal todoId As String, _
            '                                   ByVal delDate As String, _
            '                                  ByVal activityCreateStaff As String, _
            '                                 ByVal CompletionFlg As String) As Integer
            '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END
            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
            'Private Function BizUpdateDeleteFlgCalTodoItem(ByVal calenderId As String, _
            '                                                ByVal todoId As String, _
            '                                                ByVal delDate As String, _
            '                                                ByVal activityCreateStaff As String) As Integer
            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateCompleteFlgCalTodoItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 afterodractId:" + afterodractId)
            Logger.Info("IC3040403 delDate:" + delDate)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            Logger.Info("IC3040403 CompletionFlg:" + CompletionFlg)
            '$01 Add End

            Using dataTable As New IC3040403DataSet.CalTodoItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoItemDataTableRow = dataTable.NewCalTodoItemDataTableRow

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .DELFLG = CType(Delflg.Del + 0, String)
                    .DELDATE = delDate
                    .CALID = calenderId
                    .TODOID = todoId
                    '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
                    .AFTER_ODR_ACT_ID = afterodractId
                    '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END

                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    ' 更新条件の完了フラグを指定できるよう変更
                    .COMPLETIONFLG = CompletionFlg
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '$01 Add Start
                    Logger.Info("IC3040403 .UPDATEACCOUNT:" + .UPDATEACCOUNT)
                    Logger.Info("IC3040403 .UPDATEID:" + .UPDATEID)
                    Logger.Info("IC3040403 .DELFLG:" + .DELFLG)
                    Logger.Info("IC3040403 .DELDATE:" + .DELDATE)
                    Logger.Info("IC3040403 .CALID:" + .CALID)
                    Logger.Info("IC3040403 .TODOID:" + .TODOID)
                    Logger.Info("IC3040403 .AFTER_ODR_ACT_ID:" + .AFTER_ODR_ACT_ID)
                    Logger.Info("IC3040403 .COMPLETIONFLG:" + .COMPLETIONFLG)
                    '$01 Add End
                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetDeleteFlgCalTodoItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateDeleteFlgCalTodoItem > GetDeleteFlgCalTodoItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateDeleteFlgCalTodoItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateCompleteFlgCalTodoItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function



        ''' <summary>
        ''' カレンダーTodo情報テーブルの削除フラグを更新する
        ''' </summary>
        ''' <param name="calenderId">calenderId</param>
        ''' <param name="todoId">todoId</param>
        ''' <param name="delDate">削除日</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateAfterOrderDeleteFlgCalTodoItem(ByVal calenderId As String, _
                                                        ByVal todoId As String, _
                                                        ByVal delDate As String, _
                                                        ByVal activityCreateStaff As String) As Integer

            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateAfterOrderDeleteFlgCalTodoItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 delDate:" + delDate)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoItemDataTableRow = dataTable.NewCalTodoItemDataTableRow

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .DELFLG = CType(Delflg.Del + 0, String)
                    .DELDATE = delDate
                    .CALID = calenderId
                    .TODOID = todoId

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetAfterOrderDeleteFlgCalTodoItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateAfterOrderDeleteFlgCalTodoItem > GetAfterOrderDeleteFlgCalTodoItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateAfterOrderDeleteFlgCalTodoItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateAfterOrderDeleteFlgCalTodoItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーTodo情報テーブルの完了フラグを更新する
        ''' </summary>
        ''' <param name="calenderId">calenderId</param>
        ''' <param name="completeDate">完了日</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateCompleteFlgCalTodoItem(ByVal calenderId As String, _
                                                                   ByVal completeDate As String, _
                                                                   ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateCompleteFlgCalTodoItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 completeDate:" + completeDate)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoItemDataTableRow = dataTable.NewCalTodoItemDataTableRow

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .COMPLETIONFLG = CompleteFlgYes
                    .COMPLETIONDATE = completeDate
                    .CALID = calenderId

                End With

                ' 完了フラグ更新に必要な値を設定します
                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCompleteFlgCalTodoItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateCompleteFlgCalTodoItem > GetCompleteFlgCalTodoItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateCompleteFlgCalTodoItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateCompleteFlgCalTodoItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function


        ''' <summary>
        '''  カレンダーTodoアラームテーブルに登録する
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素以下の値</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="activityCreateStaff">作成／更新アカウント用のスタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertCalTodoAlarms(ByVal scheduleElement As XmlSchedule, _
                                          ByVal todoId As String, _
                                          ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertCalTodoAlarms() Start")
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoAlarmDataTableDataTable

                Dim sequenseNo As Integer = 1

                For Each alarmTrigger As String In scheduleElement.AlarmTriggerList

                    If Not Validation.Equals(alarmTrigger, EmptyString) Then

                        ' DataRowから、DataTableを作成します。
                        Dim dataRow As IC3040403DataSet.CalTodoAlarmDataTableRow = dataTable.NewCalTodoAlarmDataTableRow()

                        With dataRow

                            .TODOID = todoId
                            .SEQNO = sequenseNo
                            .STARTTRIGGER = alarmTrigger
                            .CREATEACCOUNT = activityCreateStaff
                            .UPDATEACCOUNT = activityCreateStaff
                            .CREATEID = CreateId
                            .UPDATEID = UpdateId

                        End With

                        sequenseNo = sequenseNo + 1

                        dataTable.AddCalTodoAlarmDataTableRow(dataRow)

                    End If

                Next

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoAlarmDataTable

                    Dim count As Integer = adapter.InsertCalTodoAlarms(dataTable)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertCalTodoAlarms() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        '''  カレンダーTodoアラームテーブルに登録する
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素以下の値</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="activityCreateStaff">作成／更新アカウント用のスタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <history>2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START</history>
        ''' <remarks></remarks>
        Private Function BizInsertAfterOrderCalTodoAlarms(ByVal scheduleElement As XmlAfterOrderSchedule, _
                                          ByVal todoId As String, _
                                          ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertAfterOrderCalTodoAlarms() Start")
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoAlarmDataTableDataTable

                Dim sequenseNo As Integer = 1

                For Each alarmTrigger As String In scheduleElement.AlarmTriggerList

                    If Not Validation.Equals(alarmTrigger, EmptyString) Then

                        ' DataRowから、DataTableを作成します。
                        Dim dataRow As IC3040403DataSet.CalTodoAlarmDataTableRow = dataTable.NewCalTodoAlarmDataTableRow()

                        With dataRow

                            .TODOID = todoId
                            .SEQNO = sequenseNo
                            .STARTTRIGGER = alarmTrigger
                            .CREATEACCOUNT = activityCreateStaff
                            .UPDATEACCOUNT = activityCreateStaff
                            .CREATEID = CreateId
                            .UPDATEID = UpdateId

                        End With

                        sequenseNo = sequenseNo + 1

                        dataTable.AddCalTodoAlarmDataTableRow(dataRow)

                    End If

                Next

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoAlarmDataTable

                    Dim count As Integer = adapter.InsertCalTodoAlarms(dataTable)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertAfterOrderCalTodoAlarms() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        '''  カレンダーTodoアラームテーブルの値を削除する
        ''' </summary>
        ''' <param name="todoId">TodoId</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Private Function BizDeleteCalTodoAlarm(ByVal todoId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizDeleteCalTodoAlarm() Start")
            Logger.Info("IC3040403 todoId:" + todoId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalTodoAlarmDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoAlarmDataTableRow = dataTable.NewCalTodoAlarmDataTableRow()

                dataRow.TODOID = todoId

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoAlarmDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalTodoAlarmLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizDeleteCalTodoAlarm > GetCalTodoAlarmLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.DeleteCalTodoAlarm(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizDeleteCalTodoAlarm() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function
        ''' <summary>
        ''' カレンダーイベント情報テーブルに登録する
        ''' </summary>
        ''' <param name="scheduleElement">Scdule要素</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="eventId">EventId</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertCalEventItem(ByVal scheduleElement As XmlSchedule, _
                                                           ByVal calenderId As String, _
                                                           ByVal todoId As String, _
                                                           ByVal eventId As String, _
                                                           ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertCalEventItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 eventId:" + eventId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventItemDataTableRow = dataTable.NewCalEventItemDataTableRow()

                With dataRow

                    .EVENTID = eventId
                    .CALID = calenderId
                    .TODOID = todoId
                    Using adapter As New IC3040403DataSetTableAdapters.SequenceTable

                        .UNIQUEID = adapter.GetNewUniqueId(GetCountryCode())

                    End Using
                    .RECURRENCEID = RecurrenceIdElement
                    .CHGSEQNO = 0
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .RRULE_FREQ = RruleNone
                    .RRULE_INTERVAL = Nothing
                    .RRULE_UNTIL = Nothing
                    .RRULE_TEXT = Nothing
                    .LOCATION = Nothing
                    .ATTENDEE = Nothing
                    .TRANSP = Nothing
                    .URL = Nothing
                    .DELFLG = 0
                    .DELDATE = Nothing
                    .CREATEACCOUNT = activityCreateStaff
                    .UPDATEACCOUNT = activityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .PROCESSDIV = scheduleElement.ProcessDiv
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                    If (scheduleElement.ContactName IsNot Nothing) Then
                        .CONTACT_NAME = scheduleElement.ContactName
                    Else
                        .CONTACT_NAME = BlankString
                    End If
                    .ACT_ODR_NAME = BlankString
                    If (scheduleElement.OdrDiv IsNot Nothing) Then
                        .ODR_DIV = scheduleElement.OdrDiv
                    Else
                        .ODR_DIV = BlankString
                    End If
                    .AFTER_ODR_ACT_ID = BlankString
                    '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END


                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable

                    Dim count As Integer = adapter.InsertCalEventItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertCalEventItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function
        ''' <summary>
        ''' カレンダーイベント情報テーブルに登録する
        ''' </summary>
        ''' <param name="scheduleElement">Scdule要素</param>
        ''' <param name="calenderId">カレンダーID</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="eventId">EventId</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        ''' <history>2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　STARTT</history>

        Private Function BizInsertAfterOrderCalEventItem(ByVal scheduleElement As XmlAfterOrderSchedule, _
                                                           ByVal calenderId As String, _
                                                           ByVal todoId As String, _
                                                           ByVal eventId As String, _
                                                           ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertAfterOrderCalEventItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 eventId:" + eventId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventItemDataTableRow = dataTable.NewCalEventItemDataTableRow()

                With dataRow

                    .EVENTID = eventId
                    .CALID = calenderId
                    .TODOID = todoId
                    Using adapter As New IC3040403DataSetTableAdapters.SequenceTable

                        .UNIQUEID = adapter.GetNewUniqueId(GetCountryCode())

                    End Using
                    .RECURRENCEID = RecurrenceIdElement
                    .CHGSEQNO = 0
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .RRULE_FREQ = RruleNone
                    .RRULE_INTERVAL = Nothing
                    .RRULE_UNTIL = Nothing
                    .RRULE_TEXT = Nothing
                    .LOCATION = Nothing
                    .ATTENDEE = Nothing
                    .TRANSP = Nothing
                    .URL = Nothing
                    .DELFLG = 0
                    .DELDATE = Nothing
                    .CREATEACCOUNT = activityCreateStaff
                    .UPDATEACCOUNT = activityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId
                    .PROCESSDIV = scheduleElement.ProcessDiv
                    .CONTACT_NAME = scheduleElement.ContactName
                    .ACT_ODR_NAME = scheduleElement.ActOdrName
                    .ODR_DIV = scheduleElement.OdrDiv
                    .AFTER_ODR_ACT_ID = scheduleElement.AfterOdrActID


                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable

                    Dim count As Integer = adapter.InsertCalEventItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertAfterOrderCalEventItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント情報テーブルを更新する
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素</param>
        ''' <param name="todoId">EventId</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateCalEventItem(ByVal scheduleElement As XmlSchedule, _
                                                           ByVal todoId As String, _
                                                           ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateCalEventItem() Start")
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventItemDataTableRow = dataTable.NewCalEventItemDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .TODOID = todoId
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    If scheduleElement.EndTime Is Nothing Then
                        .TIMEFLG = Nothing
                    Else
                        .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    End If
                    If scheduleElement.StartTime IsNot Nothing And scheduleElement.EndTime IsNot Nothing Then
                        .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    Else
                        .ALLDAYFLG = Nothing
                    End If
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .DELFLG = 0
                    .DELDATE = Nothing
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .PROCESSDIV = scheduleElement.ProcessDiv
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 START
                    ''2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                    'If (scheduleElement.ContactName IsNot Nothing) Then
                    '    .CONTACT_NAME = scheduleElement.ContactName
                    'Else
                    '    .CONTACT_NAME = BlankString
                    'End If
                    '.ACT_ODR_NAME = BlankString
                    'If (scheduleElement.OdrDiv IsNot Nothing) Then
                    '    .ODR_DIV = scheduleElement.OdrDiv
                    'Else
                    '    .ODR_DIV = BlankString
                    'End If
                    '.AFTER_ODR_ACT_ID = BlankString
                    ''2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                    .CONTACT_NAME = scheduleElement.ContactName
                    .ACT_ODR_NAME = Nothing
                    .ODR_DIV = scheduleElement.OdrDiv
                    .AFTER_ODR_ACT_ID = Nothing
                    '2014/06/14 SKFC 森 NEXTSTEP_CALDAV 不具合修正 END


                End With

                ' Eventテーブルを更新します
                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalEventItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateCalEventItem > GetCalEventItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateCalEventItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateCalEventItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント情報テーブルを更新する(受注後工程)
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素</param>
        ''' <param name="todoId">todoId</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　STARTT</history>
        Private Function BizUpdateAfterOrderCalEventItem(ByVal scheduleElement As XmlAfterOrderSchedule, _
                                                           ByVal todoId As String, _
                                                           ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateAfterOrderCalEventItem() Start")
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventItemDataTableRow = dataTable.NewCalEventItemDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .TODOID = todoId
                    .ACTSTAFFSTRCD = scheduleElement.ActivityStaffBranchCode
                    .ACTSTAFFCD = scheduleElement.ActivityStaffCode
                    .RECSTAFFSTRCD = scheduleElement.ReceptionStaffBranchCode
                    .RECSTAFFCD = scheduleElement.ReceptionStaffCode
                    .CONTACTNO = scheduleElement.ContactNo
                    .SUMMARY = scheduleElement.Summary
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    If scheduleElement.EndTime Is Nothing Then
                        .TIMEFLG = Nothing
                    Else
                        .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    End If
                    If scheduleElement.StartTime IsNot Nothing And scheduleElement.EndTime IsNot Nothing Then
                        .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    Else
                        .ALLDAYFLG = Nothing
                    End If
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .DELFLG = 0
                    .DELDATE = Nothing
                    .PROCESSDIV = scheduleElement.ProcessDiv
                    .CONTACT_NAME = scheduleElement.ContactName
                    .ACT_ODR_NAME = scheduleElement.ActOdrName
                    .ODR_DIV = scheduleElement.OdrDiv
                    .AFTER_ODR_ACT_ID = scheduleElement.AfterOdrActID


                End With

                ' Eventテーブルを更新します
                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalEventItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateAfterOrderCalEventItem > GetCalEventItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateCalEventItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateAfterOrderCalEventItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント情報テーブルの削除フラグを更新する
        ''' </summary>
        ''' <param name="calenderId">calenderId</param>
        ''' <param name="eventId">EventId</param>
        ''' <param name="delDate">削除日</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <param name="CompletionFlg">完了フラグ</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateDeleteFlgCalEventItem(ByVal calenderId As String, _
                                                        ByVal eventId As String, _
                                                        ByVal delDate As String, _
                                                        ByVal activityCreateStaff As String, _
                                                        ByVal CompletionFlg As String) As Integer
            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
            'Private Function BizUpdateDeleteFlgCalEventItem(ByVal calenderId As String, _
            '                                                ByVal eventId As String, _
            '                                                ByVal delDate As String, _
            '                                                ByVal activityCreateStaff As String) As Integer
            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateDeleteFlgCalEventItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 eventId:" + eventId)
            Logger.Info("IC3040403 delDate:" + delDate)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            Logger.Info("IC3040403 CompletionFlg:" + CompletionFlg)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventItemDataTableRow = dataTable.NewCalEventItemDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .DELFLG = CType(Delflg.Del + 0, String)
                    .DELDATE = delDate
                    .CALID = calenderId
                    .EVENTID = eventId

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetDeleteFlgCalEventItemLock(dataRow, CompletionFlg) = -1 Then
                        Throw New ApplicationException("BizUpdateDeleteFlgCalEventItem > GetDeleteFlgCalEventItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    'Dim count As Integer = adapter.UpdateDeleteFlgCalEventItem(dataRow)
                    Dim count As Integer = adapter.UpdateDeleteFlgCalEventItem(dataRow, CompletionFlg)
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateDeleteFlgCalEventItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function
        ''' <summary>
        ''' カレンダーイベント情報テーブルの削除フラグを更新する
        ''' </summary>
        ''' <param name="calenderId">calenderId</param>
        ''' <param name="todoId">todoId</param>
        ''' <param name="delDate">削除日</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>2014/07/23 SKFC 渡邊 NEXT_STEP CalDAV 仕様変更 START</history>
        Private Function BizUpdateAfterOrderDeleteFlgCalEventItem(ByVal calenderId As String, _
                                                        ByVal todoId As String, _
                                                        ByVal delDate As String, _
                                                        ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateAfterOrderDeleteFlgCalEventItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 delDate:" + delDate)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End

            Using dataTable As New IC3040403DataSet.CalEventItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventItemDataTableRow = dataTable.NewCalEventItemDataTableRow()

                With dataRow

                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId
                    .DELFLG = CType(Delflg.Del + 0, String)
                    .DELDATE = delDate
                    .CALID = calenderId
                    .TODOID = todoId

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetAfterOrderDeleteFlgCalEventItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateAfterOrderDeleteFlgCalEventItem > GetAfterOrderDeleteFlgCalEventItemLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateAfterOrderDeleteFlgCalEventItem(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateAfterOrderDeleteFlgCalEventItem() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント情報テーブルに登録する
        ''' </summary>
        ''' <param name="scheduleElement">Scdule要素</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="eventId">EventId</param>
        ''' <param name="activityCreateStaff">活動生成スタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertLinkEvent(ByVal scheduleElement As XmlSchedule, _
                                                           ByVal todoId As String, _
                                                           ByVal eventId As String, _
                                                           ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertLinkEvent() Start")
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 eventId:" + eventId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventItemDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventItemDataTableRow = dataTable.NewCalEventItemDataTableRow()

                With dataRow

                    .EVENTID = eventId
                    .TODOID = todoId
                    Using adapter As New IC3040403DataSetTableAdapters.SequenceTable

                        .UNIQUEID = adapter.GetNewUniqueId(GetCountryCode())

                    End Using
                    .RECURRENCEID = RecurrenceIdElement
                    .CHGSEQNO = 0
                    .STARTTIME = scheduleElement.StartTime
                    .ENDTIME = scheduleElement.EndTime
                    .TIMEFLG = checkTimeFlg(scheduleElement.EndTime)
                    .ALLDAYFLG = checkAlldayFlg(scheduleElement.StartTime, scheduleElement.EndTime)
                    .MEMO = scheduleElement.Memo
                    .ICROPCOLOR = scheduleElement.XIcropColor
                    .LOCATION = Nothing
                    .ATTENDEE = Nothing
                    .TRANSP = Nothing
                    .URL = Nothing
                    .DELFLG = 0
                    .DELDATE = Nothing
                    .CREATEACCOUNT = activityCreateStaff
                    .UPDATEACCOUNT = activityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .PROCESSDIV = scheduleElement.ParentDiv
                    ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable

                    Dim insertCount As Integer = adapter.InsertLinkEvent(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 insertCount:" + insertCount.ToString())
                    Logger.Info("IC3040403 BizInsertLinkEvent() End")
                    '$01 Add End
                    If insertCount = 0 Then

                        Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.TodoId)

                    End If

                    Return insertCount

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーEventアラームテーブルに登録する
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素以下の値</param>
        ''' <param name="eventId">EventId</param>
        ''' <param name="activityCreateStaff">作成／更新アカウント用のスタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertCalEventAlarms(ByVal scheduleElement As XmlSchedule, _
                                          ByVal eventId As String, _
                                          ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertCalEventAlarms() Start")
            Logger.Info("IC3040403 eventId:" + eventId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventAlarmDataTableDataTable

                Dim sequenseNo As Integer = 1

                For Each alarmTrigger As String In scheduleElement.AlarmTriggerList


                    If Not Validation.Equals(alarmTrigger, EmptyString) Then

                        ' DataRowから、DataTableを作成します。
                        Dim dataRow As IC3040403DataSet.CalEventAlarmDataTableRow = dataTable.NewCalEventAlarmDataTableRow()

                        With dataRow

                            .EVENTID = eventId
                            .SEQNO = sequenseNo
                            .STARTTRIGGER = alarmTrigger
                            .CREATEACCOUNT = activityCreateStaff
                            .UPDATEACCOUNT = activityCreateStaff
                            .CREATEID = CreateId
                            .UPDATEID = UpdateId

                        End With

                        sequenseNo = sequenseNo + 1

                        dataTable.AddCalEventAlarmDataTableRow(dataRow)

                    End If

                Next

                Using adapter As New IC3040403DataSetTableAdapters.CalEventAlarmDataTable

                    Dim count As Integer = adapter.InsertCalEventAlarms(dataTable)
                    '$01 Add Start
                    '2019/02/14 TKM UAT-0182 START
                    Logger.Info("IC3040403 count:" + count.ToString())
                    '2019/02/14 TKM UAT-0182 END
                    Logger.Info("IC3040403 BizInsertCalEventAlarms() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function


        ''' <summary>
        ''' カレンダーEventアラームテーブルに登録する(受注後工程)
        ''' </summary>
        ''' <param name="scheduleElement">Schedule要素以下の値</param>
        ''' <param name="eventId">EventId</param>
        ''' <param name="activityCreateStaff">作成／更新アカウント用のスタッフコード</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertAfterOrderCalEventAlarms(ByVal scheduleElement As XmlAfterOrderSchedule, _
                                          ByVal eventId As String, _
                                          ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertAfterOrderCalEventAlarms() Start")
            Logger.Info("IC3040403 eventId:" + eventId)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventAlarmDataTableDataTable

                Dim sequenseNo As Integer = 1

                For Each alarmTrigger As String In scheduleElement.AlarmTriggerList


                    If Not Validation.Equals(alarmTrigger, EmptyString) Then

                        ' DataRowから、DataTableを作成します。
                        Dim dataRow As IC3040403DataSet.CalEventAlarmDataTableRow = dataTable.NewCalEventAlarmDataTableRow()

                        With dataRow

                            .EVENTID = eventId
                            .SEQNO = sequenseNo
                            .STARTTRIGGER = alarmTrigger
                            .CREATEACCOUNT = activityCreateStaff
                            .UPDATEACCOUNT = activityCreateStaff
                            .CREATEID = CreateId
                            .UPDATEID = UpdateId

                        End With

                        sequenseNo = sequenseNo + 1

                        dataTable.AddCalEventAlarmDataTableRow(dataRow)

                    End If

                Next

                Using adapter As New IC3040403DataSetTableAdapters.CalEventAlarmDataTable

                    Dim count As Integer = adapter.InsertCalEventAlarms(dataTable)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertAfterOrderCalEventAlarms() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function


        ''' <summary>
        '''  カレンダーTodoアラームテーブルの値を削除する
        ''' </summary>
        ''' <param name="eventId">EventId</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Private Function BizDeleteCalEventAlarm(ByVal eventId As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizDeleteCalEventAlarm() Start")
            Logger.Info("IC3040403 eventId:" + eventId)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalEventAlarmDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventAlarmDataTableRow = dataTable.NewCalEventAlarmDataTableRow()

                dataRow.EVENTID = eventId

                Using adapter As New IC3040403DataSetTableAdapters.CalEventAlarmDataTable
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalEventAlarmLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizDeleteCalEventAlarm > GetCalEventAlarmLock:-1")
                    End If
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.DeleteCalEventAlarm(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizDeleteCalEventAlarm() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーアドレス最終更新日テーブルに、新しいスタッフを追加する
        ''' </summary>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="activityCreateStaff">作成／更新アカウント</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Private Function BizInsertCalCardLastModify(ByVal staffCode As String, _
                                                       ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizInsertCalCardLastModify() Start")
            Logger.Info("IC3040403 staffCode:" + staffCode)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCardLastModifyDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCardLastModifyDataTableRow = dataTable.NewCalCardLastModifyDataTableRow()

                With dataRow

                    .STAFFCD = staffCode
                    .CREATEACCOUNT = activityCreateStaff
                    .UPDATEACCOUNT = activityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalCardLastModifyDataTable

                    Dim count As Integer = adapter.InsertCalCardLastModify(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizInsertCalCardLastModify() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' カレンダーアドレス最終更新日テーブルのスタッフの更新日付を更新する
        ''' </summary>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="activityCreateStaff">更新アカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateCalCardLastModify(ByVal staffCode As String, _
                                                       ByVal activityCreateStaff As String) As Integer
            '$01 Add Start
            Logger.Info("IC3040403 BizUpdateCalCardLastModify() Start")
            Logger.Info("IC3040403 staffCode:" + staffCode)
            Logger.Info("IC3040403 activityCreateStaff:" + activityCreateStaff)
            '$01 Add End
            Using dataTable As New IC3040403DataSet.CalCardLastModifyDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalCardLastModifyDataTableRow = dataTable.NewCalCardLastModifyDataTableRow()

                With dataRow

                    .STAFFCD = staffCode
                    .UPDATEACCOUNT = activityCreateStaff
                    .UPDATEID = UpdateId

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalCardLastModifyDataTable

                    Dim count As Integer = adapter.UpdateCalCardLastModify(dataRow)
                    '$01 Add Start
                    Logger.Info("IC3040403 count:" + count.ToString())
                    Logger.Info("IC3040403 BizUpdateCalCardLastModify() End")
                    '$01 Add End
                    Return count

                End Using

            End Using

        End Function

        ''' <summary>
        ''' TODO情報テーブルから、カレンダーＩＤに紐付くスタッフコードを取得します
        ''' </summary>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <param name="calenderId">カレンダーＩＤ</param>
        ''' <param name="scheduleElement">スケジュール要素</param>
        ''' <param name="scheduleDiv">スケジュール区分</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function GetStaffCodeTodoItem(ByVal staffCodeList As List(Of String), _
                                              ByVal calenderId As String, _
                                              ByVal todoId As String, _
                                              ByVal scheduleElement As XmlSchedule, _
                                              ByVal scheduleDiv As String) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 GetStaffCodeTodoItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 scheduleDiv:" + scheduleDiv)
            '$01 Add End
            Dim dataTable As IC3040403DataSet.StaffCodeDataTableDataTable

            Using adapter As New IC3040403DataSetTableAdapters.StaffCodeDataTable

                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
                'dataTable = adapter.SelectStaffCodeTodoItem(calenderId, todoId)
                dataTable = adapter.SelectStaffCodeTodoItem(calenderId, todoId, Nothing)
                '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END

            End Using

            '2019/05/08 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 (TR-SLT-FTMS-20181219-001) START
            '2019/05/08 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 (TR-SLT-FTMS-20181219-001) END


            ' スタッフコードを重複していない限りはスタッフコードリストに格納します
            staffCodeList = CheckStaffCodeDataTable(dataTable, staffCodeList)
            '$01 Add Start
            Logger.Info("IC3040403 GetStaffCodeTodoItem() End")
            '$01 Add End
            Return staffCodeList

        End Function

        ''' <summary>
        ''' TODO情報テーブルから、カレンダーＩＤに紐付くスタッフコードを取得します(受注後工程)
        ''' </summary>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <param name="calenderId">カレンダーＩＤ</param>
        ''' <param name="scheduleElement">スケジュール要素</param>
        ''' <returns>スタッフコードリスト</returns>
        '''<history>2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START</history>
        ''' ''' <remarks></remarks>
        Private Function GetAfterOrderStaffCodeTodoItem(ByVal staffCodeList As List(Of String), _
                                              ByVal calenderId As String, _
                                              ByVal todoId As String, _
                                              ByRef afterodractId As String, _
                                              ByVal scheduleElement As XmlAfterOrderSchedule) As List(Of String)
            'Private Function GetAfterOrderStaffCodeTodoItem(ByVal staffCodeList As List(Of String), _
            '                                      ByVal calenderId As String, _
            '                                     ByVal todoId As String, _
            '                                    ByVal scheduleElement As XmlAfterOrderSchedule) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 GetAfterOrderStaffCodeTodoItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 afterodractId:" + afterodractId)
            '$01 Add End
            Dim dataTable As IC3040403DataSet.StaffCodeDataTableDataTable

            Using adapter As New IC3040403DataSetTableAdapters.StaffCodeDataTable

                dataTable = adapter.SelectStaffCodeTodoItem(calenderId, todoId, afterodractId)

            End Using

            '2014/06/27 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
            ' 更新した結果、スタッフコードチェックにひっかからないか調べます
            'CheckAfterOrderStaffCodeUpdateDataBase(dataTable, TwoString, scheduleElement)
            '2014/06/27 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END

            ' スタッフコードを重複していない限りはスタッフコードリストに格納します
            staffCodeList = CheckStaffCodeDataTable(dataTable, staffCodeList)
            '$01 Add Start
            Logger.Info("IC3040403 GetAfterOrderStaffCodeTodoItem() Start")
            '$01 Add End
            Return staffCodeList

        End Function


        ''' <summary>
        ''' イベント情報テーブルから、カレンダーＩＤ又はTodoIDに紐付くスタッフコードを取得します
        ''' </summary>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <param name="calenderId">カレンダーＩＤ</param>
        ''' <param name="todoId">TodoId</param>
        ''' <param name="scheduleElement">スケジュール要素</param>
        ''' <param name="scheduleDiv">スケジュール区分</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function GetStaffCodeEventItem(ByVal staffCodeList As List(Of String), _
                                               ByVal calenderId As String, _
                                               ByVal todoId As String, _
                                               ByVal scheduleElement As XmlSchedule, _
                                               ByVal scheduleDiv As String) As List(Of String)
            '$01 Add Start
            Logger.Info("IC3040403 GetStaffCodeEventItem() Start")
            Logger.Info("IC3040403 calenderId:" + calenderId)
            Logger.Info("IC3040403 todoId:" + todoId)
            Logger.Info("IC3040403 scheduleDiv:" + scheduleDiv)
            '$01 Add End
            Dim dataTable As IC3040403DataSet.StaffCodeDataTableDataTable

            Using adapter As New IC3040403DataSetTableAdapters.StaffCodeDataTable

                dataTable = adapter.SelectStaffCodeEventItem(calenderId, todoId)

            End Using

            '2019/05/08 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 (TR-SLT-FTMS-20181219-001) START
            '2019/05/08 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 (TR-SLT-FTMS-20181219-001) END

            ' スタッフコードを重複していない限りはスタッフコードリストに格納します
            staffCodeList = CheckStaffCodeDataTable(dataTable, staffCodeList)
            '$01 Add Start
            Logger.Info("IC3040403 GetStaffCodeEventItem() End")
            '$01 Add End
            Return staffCodeList

        End Function


        ''' <summary>
        ''' ICROPのキーから、カレンダーIDを取得します
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <returns>カレンダーＩＤ</returns>
        ''' <remarks></remarks>
        Private Function BizGetCalenderId(ByVal detailData As XmlDetail) As String

            Using adapter As New IC3040403DataSetTableAdapters.IdTable

                Dim calendarId As String = adapter.GetCalenderId(detailData.ScheduleId, detailData.ScheduleDiv)

                Return calendarId

            End Using

        End Function

        ''' <summary>
        ''' ICROPのキーから、カレンダーIDを取得します
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <returns>カレンダーＩＤ</returns>
        ''' <remarks></remarks>
        Private Function BizGetAfterOrderCalenderId(ByVal detailData As XmlAfterOrderDetail) As String

            Using adapter As New IC3040403DataSetTableAdapters.IdTable

                Dim calendarId As String = adapter.GetCalenderId(detailData.ScheduleId, TwoString)

                Return calendarId

            End Using

        End Function


        ''' <summary>
        ''' TodoIdから、EventIdを取得します
        ''' </summary>
        ''' <param name="todoId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BizGetEventId(ByVal todoId As String) As String

            Using adapter As New IC3040403DataSetTableAdapters.IdTable

                Dim eventId As String = adapter.GetEventId(todoId)

                Return eventId

            End Using

        End Function

        ''' <summary>
        ''' TodoIdから、EventIdを取得します
        ''' </summary>
        ''' <param name="ProcessDiv"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Private Function BizGetTodoId(ByVal calenderId As String, ByVal ProcessDiv As String) As String

            Using adapter As New IC3040403DataSetTableAdapters.IdTable

                Dim toDoId As String = adapter.GetToDoId(calenderId, ProcessDiv)

                Return toDoId

            End Using

        End Function



        ''' <summary>
        ''' 新しいカレンダーIDを取得します
        ''' </summary>
        ''' <returns>カレンダーＩＤ</returns>
        ''' <remarks></remarks>
        Private Function BizGetNewCalenderID() As String

            Using adapter As New IC3040403DataSetTableAdapters.SequenceTable

                Dim calendarId As String = adapter.GetNewCalenderId()

                Return calendarId

            End Using

        End Function

        ''' <summary>
        ''' 新しいTODOIDを取得します
        ''' </summary>
        ''' <returns>新規TODOID</returns>
        ''' <remarks></remarks>
        Private Function BizGetNewTodoId() As String

            ' todoIdを作成します
            Using adapter As New IC3040403DataSetTableAdapters.SequenceTable

                Dim todoId As String = adapter.GetNewTodoId()

                Return todoId

            End Using

        End Function

        ''' <summary>
        ''' 新しいEventIdを取得します
        ''' </summary>
        ''' <returns>新規EventId</returns>
        ''' <remarks></remarks>
        Private Function BizGetNewEventId() As String

            ' eventIdを作成します
            Using adapter As New IC3040403DataSetTableAdapters.SequenceTable

                Dim eventId As String = adapter.GetNewEventId()

                Return eventId

            End Using

        End Function

#End Region


    End Class

End Namespace
