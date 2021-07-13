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

            Try

                ' スタッフコードリスト
                Dim staffCodeList As List(Of String) = New List(Of String)

                ' 処理区分により、処理を分岐させる
                Select Case CType(detailData.ActionType, Integer)

                    Case ActionType.Entry
                        ' 処理区分が登録（１）の場合
                        staffCodeList = EntryDataBase(detailData, staffCodeList)

                    Case ActionType.Update
                        ' 処理区分が更新（２）の場合
                        staffCodeList = UpdateDataBase(detailData, staffCodeList)

                    Case ActionType.AddEvent
                        ' 処理区分がイベント追加（３）の場合
                        staffCodeList = EventDataBase(detailData, staffCodeList)

                End Select

                ' 今回使用したスタッフコードをカレンダーアドレス最終更新日テーブルに反映します
                SetStaffCode(staffCodeList, detailData)

            Catch ex As ApplicationException

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
            GetNodeInnerText(commonXml, XmlNameScheduleId, DataAssignment.ModeMandatory, 10, TypeConversion.IntegerType, ElementName.ScheduleID)
            xmlDetailClass.ActionType = GetNodeInnerText(commonXml, XmlNameActionType, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ActionType)
            xmlDetailClass.ActivityCreateStaff = GetNodeInnerText(commonXml, XmlNameActivityCreateStaff, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.ActivityCreateStaff)

            ' スケジュール区分が規定値以外の場合
            If Not (IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.VisitReservation) Or _
                IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion)) Then

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
                xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 19, TypeConversion.StringType, ElementName.CustomerCode)
                xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsID)
                xmlDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
                xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
                xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
                xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCd, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCd)
                xmlDetailClass.StrStatus = GetNodeInnerText(scheduleInfoClone, XmlNameStrStatus, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.StrStatus)
                xmlDetailClass.RezStatus = GetNodeInnerText(scheduleInfoClone, XmlNameRezStatus, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.RezStatus)
                xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeMandatory, 19, TypeConversion.DateType, ElementName.CompletionDate)
                xmlDetailClass.DeleteDate = GetNodeInnerText(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)
            Else
                ' 完了区分が完了以外の場合
                xmlDetailClass.CustomerDiv = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CustomerDiv)
                xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeMandatory, 19, TypeConversion.StringType, ElementName.CustomerCode)
                xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsID)
                xmlDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeMandatory, 256, TypeConversion.StringType, ElementName.CustomerName)

                ' スケジュール区分により、受付納車区分の項目の扱いが変化
                If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion) Then
                    ' 入庫予約
                    xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
                Else
                    ' 来店予約
                    xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
                End If

                xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
                xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCd, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCd)
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
        ''' ScheduleInfo要素内の、処理区分が「更新」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleInfoClone">XML</param>
        ''' <param name="xmlDetailClass">取得した要素を格納するクラス</param>
        ''' <returns>取得した要素を格納したDetailクラス</returns>
        ''' <remarks></remarks>
        Private Function UpdateGetScheduleInfoElementValue(ByVal scheduleInfoClone As XmlNode, ByVal xmlDetailClass As XmlDetail) As XmlDetail

            xmlDetailClass.CustomerDiv = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.CustomerDiv)
            xmlDetailClass.CustomerCode = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 19, TypeConversion.StringType, ElementName.CustomerCode)
            xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsID)
            xmlDetailClass.CustomerName = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
            ' 受付納車区分はスケジュール区分が入庫予約の場合は空欄は許されない
            If IsFlgEquals(xmlDetailClass.ScheduleDiv, ScheDuleDiv.GRReservattion) Then

                xmlDetailClass.ReceptionDiv = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)

            Else
                ' 入庫予約でない場合は空欄（削除）を許す
                xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
            End If
            xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
            xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCd, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCd)
            xmlDetailClass.StrStatus = GetNodeInnerText(scheduleInfoClone, XmlNameStrStatus, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.StrStatus)
            xmlDetailClass.RezStatus = GetNodeInnerText(scheduleInfoClone, XmlNameRezStatus, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.RezStatus)
            xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.CompletionDate)
            xmlDetailClass.DeleteDate = GetNodeInnerTextNotEmpty(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            Return xmlDetailClass

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
            xmlDetailClass.CustomerCode = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerCode, DataAssignment.ModeOptional, 19, TypeConversion.StringType, ElementName.CustomerCode)
            xmlDetailClass.DmsId = GetNodeInnerText(scheduleInfoClone, XmlNameDmsId, DataAssignment.ModeOptional, 18, TypeConversion.StringType, ElementName.DmsID)
            xmlDetailClass.CustomerName = GetNodeInnerText(scheduleInfoClone, XmlNameCustomerName, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.CustomerName)
            xmlDetailClass.ReceptionDiv = GetNodeInnerText(scheduleInfoClone, XmlNameReceptionDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ReceptionDiv)
            xmlDetailClass.ServiceCode = GetNodeInnerText(scheduleInfoClone, XmlNameServiceCode, DataAssignment.ModeOptional, 2, TypeConversion.StringType, ElementName.ServiceCode)
            xmlDetailClass.MerchandiseCD = GetNodeInnerText(scheduleInfoClone, XmlNameMerchandiseCd, DataAssignment.ModeOptional, 8, TypeConversion.StringType, ElementName.MerchandiseCd)
            xmlDetailClass.StrStatus = GetNodeInnerText(scheduleInfoClone, XmlNameStrStatus, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.StrStatus)
            xmlDetailClass.RezStatus = GetNodeInnerText(scheduleInfoClone, XmlNameRezStatus, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.RezStatus)
            xmlDetailClass.CompletionDate = GetNodeInnerText(scheduleInfoClone, XmlNameCompletionDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.CompletionDate)
            xmlDetailClass.DeleteDate = GetNodeInnerText(scheduleInfoClone, XmlNameDeleteDate, DataAssignment.ModeOptional, 19, TypeConversion.DateType, ElementName.DeleteDate)

            Return xmlDetailClass

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
                xmlScheduleClass = EventAddGetScheduleElementValue(scheduleClone, xmlScheduleClass)

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
                xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeMandatory, 10, TypeConversion.IntegerType, ElementName.ContactNo)
            Else
                xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
            End If
            xmlScheduleClass.Summary = GetNodeInnerText(scheduleClone, XmlNameSummary, DataAssignment.ModeMandatory, 256, TypeConversion.StringType, ElementName.Summary)
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

            xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.TodoID)
            xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.ParentDiv)

            Return xmlScheduleClass

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
                xmlScheduleClass.ContactNo = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
            Else
                ' 来店予約でない場合は空欄（削除）を許す
                xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
            End If
            xmlScheduleClass.Summary = GetNodeInnerTextNotEmpty(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.Summary)
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
            xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.TodoID)
            xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ParentDiv)

            Return xmlScheduleClass

        End Function

        ''' <summary>
        ''' Schedule要素内の、処理区分が「イベント追加」の場合の要素を取得するメソッド
        ''' </summary>
        ''' <param name="scheduleClone">Schedule要素以下の部分のみのXML</param>
        ''' <param name="xmlScheduleClass">取得した要素を格納するクラス</param>
        ''' <returns>値の入ったxmlScheduleClass</returns>
        ''' <remarks></remarks>
        Private Function EventAddGetScheduleElementValue(ByVal scheduleClone As XmlNode, ByVal xmlScheduleClass As XmlSchedule) As XmlSchedule

            xmlScheduleClass.CreateScheduleDiv = GetNodeInnerText(scheduleClone, XmlNameCreateScheduleDiv, DataAssignment.ModeMandatory, 1, TypeConversion.StringType, ElementName.CreateScheduleDiv)
            xmlScheduleClass.ActivityStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ActivityStaffBranchCode)
            xmlScheduleClass.ActivityStaffCode = GetNodeInnerText(scheduleClone, XmlNameActivityStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ActivityStaffCode)
            xmlScheduleClass.ReceptionStaffBranchCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffBranchCode, DataAssignment.ModeOptional, 3, TypeConversion.StringType, ElementName.ReceptionStaffBranchCode)
            xmlScheduleClass.ReceptionStaffCode = GetNodeInnerText(scheduleClone, XmlNameReceptionStaffCode, DataAssignment.ModeOptional, 20, TypeConversion.StringType, ElementName.ReceptionStaffCode)
            xmlScheduleClass.ContactNo = GetNodeInnerText(scheduleClone, XmlNameContactNo, DataAssignment.ModeOptional, 10, TypeConversion.IntegerType, ElementName.ContactNo)
            xmlScheduleClass.Summary = GetNodeInnerText(scheduleClone, XmlNameSummary, DataAssignment.ModeOptional, 256, TypeConversion.StringType, ElementName.Summary)
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
            xmlScheduleClass.TodoId = GetNodeInnerText(scheduleClone, XmlNameTodoId, DataAssignment.ModeMandatory, 20, TypeConversion.StringType, ElementName.TodoID)
            xmlScheduleClass.ParentDiv = GetNodeInnerText(scheduleClone, XmlNameParentDiv, DataAssignment.ModeOptional, 1, TypeConversion.StringType, ElementName.ParentDiv)

            ' スケジュール作成区分が規定されている値で無い場合
            If Not Validation.Equals(xmlScheduleClass.CreateScheduleDiv, ThreeString) Then
                ' エラーをThrowする
                Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.CreateScheduleDiv)

            End If
            Return xmlScheduleClass

        End Function


        ''' <summary>
        ''' 処理区分が「登録」のＤＢ処理を行います
        ''' </summary>
        ''' <param name="detailData">Detail要素</param>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <returns>スタッフコードリスト</returns>
        ''' <remarks></remarks>
        Private Function EntryDataBase(ByVal detailData As XmlDetail, ByVal staffCodeList As List(Of String)) As List(Of String)

            ' カレンダーID
            Dim calenderId As String = Nothing
            ' TodoId
            Dim todoId As String = Nothing
            ' eventId
            Dim eventId As String = Nothing
            'ICropの変数から、紐付くカレンダーIDを取得します。
            calenderId = BizGetCalenderId(detailData)

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
                ' カレンダーTodo情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                ' カレンダーEvent情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff)

                ' 完了区分が"完了"だった場合、処理をこれで終了する
                If IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgActivityCompleted) Then

                    Return staffCodeList

                End If


            End If

            ' 完了フラグが完了なしまたはContinueの場合
            If IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgNotContinue) Or detailData.CompletionDiv Is Nothing Or _
                IsFlgEquals(detailData.CompletionDiv, CompletionFlg.FlgContinue) Then

                ' スケジュール区分が入庫予約の場合
                If IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.GRReservattion) AndAlso _
                    calenderId IsNot Nothing Then
                    ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                    staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)
                    ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                    staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, Nothing, Nothing)
                    ' カレンダーICROP情報管理テーブルの削除フラグを更新します
                    BizUpdateDeleteFlgCalItem(detailData, calenderId)
                    ' カレンダーTodo情報テーブルの削除フラグを更新します
                    BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                    ' カレンダーEvent情報テーブルの削除フラグを更新します
                    BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff)

                End If

                ' スケジュール区分が来店予約であり、カレンダーIDが取得できている場合以外
                If Not (IsFlgEquals(detailData.ScheduleDiv, ScheDuleDiv.VisitReservation) AndAlso calenderId IsNot Nothing) Then

                    ' 新規のカレンダーＩＤで、カレンダーICROP情報管理テーブルを作成する
                    calenderId = BizGetNewCalenderID()
                    BizInsertCalCalender(detailData, calenderId)

                End If

                ' スケジュール要素分、Todo、イベント追加を行います
                For Each scheduleData As XmlSchedule In detailData.ScheduleList

                    ' スタッフコードチェックを行います
                    CheckStaffCode(detailData.ScheduleDiv, scheduleData.ActivityStaffCode, scheduleData.ReceptionStaffCode)

                    ' スケジュール作成区分がTodo+Event又はTodoだった場合、Todoを作成します
                    If IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEventAndTodo) Or _
                       IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgTodo) Then

                        ' 追加なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                        staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                        staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                        ' todoIdを作成します
                        todoId = BizGetNewTodoId()

                        ' カレンダーTodo情報テーブルに登録します
                        BizInsertCalTodoItem(scheduleData, calenderId, todoId, detailData.ActivityCreateStaff)

                        ' アラームの項目が存在する場合、アラームを登録します
                        If scheduleData.AlarmTriggerList.Count > 0 Then

                            BizInsertCalTodoAlarms(scheduleData, todoId, detailData.ActivityCreateStaff)

                        End If

                    End If

                    ' スケジュール作成区分がTodo+Event又はEventだった場合、Eventを作成します
                    If IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEventAndTodo) Or _
                        IsFlgEquals(scheduleData.CreateScheduleDiv, CreateScheduleDiv.FlgEvent) Then

                        ' 追加なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                        staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                        staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                        ' eventIdを作成します
                        eventId = BizGetNewEventId()

                        ' カレンダーevent情報テーブルに登録します
                        BizInsertCalEventItem(scheduleData, calenderId, todoId, eventId, detailData.ActivityCreateStaff)

                        ' アラームの項目が存在する場合、アラームを登録します
                        If scheduleData.AlarmTriggerList.Count > 0 Then

                            BizInsertCalEventAlarms(scheduleData, eventId, detailData.ActivityCreateStaff)

                        End If

                    End If

                Next

            End If
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

            ' カレンダーID
            Dim calenderId As String = Nothing
            ' TodoId
            Dim todoId As String = Nothing
            ' eventId
            Dim eventId As String = Nothing
            ' ICropの変数から、紐付くカレンダーIDを取得します。
            calenderId = BizGetCalenderId(detailData)

            ' カレンダーＩＤが空の場合、更新、削除処理が実行できないので終了する。
            If calenderId Is Nothing Then

                Return staffCodeList

            End If

            If detailData.DeleteDate Is Nothing Or Validation.Equals(detailData.DeleteDate, EmptyString) Then

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

                    ' 更新なので、新しく追加するスタッフコードの値をスタッフコードリストへ追加する
                    staffCodeList = CheckStaffCodeString(scheduleData.ActivityStaffCode, staffCodeList)
                    staffCodeList = CheckStaffCodeString(scheduleData.ReceptionStaffCode, staffCodeList)

                    ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                    staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, scheduleData, detailData.ScheduleDiv)

                    ' Todoテーブルを更新します
                    BizUpdateCalTodoItem(scheduleData, scheduleData.TodoId, detailData.ActivityCreateStaff)

                    ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                    staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, scheduleData, detailData.ScheduleDiv)

                    ' Eventテーブルを更新します
                    BizUpdateCalEventItem(scheduleData, todoId, detailData.ActivityCreateStaff)

                    ' todoIdから、EventIdを取得します
                    eventId = BizGetEventId(scheduleData.TodoId)

                    ' アラームの項目が存在する場合、アラームを登録します
                    If scheduleData.AlarmTriggerList.Count > 0 Then

                        Using adapter As New IC3040403DataSetTableAdapters.CalTodoAlarmDataTable

                            ' アラームを削除し、新しいアラームを入れます。
                            BizDeleteCalTodoAlarm(scheduleData.TodoId)
                            BizInsertCalTodoAlarms(scheduleData, scheduleData.TodoId, detailData.ActivityCreateStaff)

                        End Using

                        Using adapter As New IC3040403DataSetTableAdapters.CalEventAlarmDataTable

                            ' アラームを削除し、新しいアラームを入れます。
                            BizDeleteCalEventAlarm(eventId)
                            BizInsertCalEventAlarms(scheduleData, eventId, detailData.ActivityCreateStaff)

                        End Using

                    End If

                Next

            Else
                ' 削除日に値が入っていた場合、カレンダー、Todo、イベントの全てのテーブルから論理削除する

                ' Todoテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeTodoItem(staffCodeList, calenderId, Nothing, Nothing, Nothing)
                ' イベントテーブルを更新する際に変更するスタッフコードを取得します
                staffCodeList = GetStaffCodeEventItem(staffCodeList, calenderId, todoId, Nothing, Nothing)

                ' カレンダーICROP情報管理テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalItem(detailData, calenderId)
                ' カレンダーTodo情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalTodoItem(calenderId, todoId, detailData.DeleteDate, detailData.ActivityCreateStaff)
                ' カレンダーEvent情報テーブルの削除フラグを更新します
                BizUpdateDeleteFlgCalEventItem(calenderId, eventId, detailData.DeleteDate, detailData.ActivityCreateStaff)

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

            ' EventId
            Dim eventId As String

            ' Schedule要素の数だけ、紐付きイベントを追加します
            For Each scheduleData As XmlSchedule In detailData.ScheduleList

                ' スタッフコードをスタッフコードリストに追加します
                staffCodeList = GetStaffCodeTodoItem(staffCodeList, Nothing, scheduleData.TodoId, Nothing, Nothing)

                ' イベントＩＤを取得します
                eventId = BizGetNewEventId()

                ' 紐付きイベントを追加します
                BizInsertLinkEvent(scheduleData, scheduleData.TodoId, eventId, detailData.ActivityCreateStaff)

            Next
            Return staffCodeList

        End Function

        ''' <summary>
        ''' 今回の処理で使用したスタッフコードをカレンダーアドレス最終更新日テーブルに更新／追加をします
        ''' </summary>
        ''' <param name="staffCodeList">スタッフコードリスト</param>
        ''' <param name="detailData">更新に必要な値（機能ＩＤ，アカウント）</param>
        ''' <remarks></remarks>
        Private Sub SetStaffCode(ByVal staffCodeList As List(Of String), ByVal detailData As XmlDetail)

            For Each staffCode As String In staffCodeList

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
                            Dim dummy As Integer = CType(target, Integer)
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

            ' 活動スタッフコードの要素内が空欄の場合、Nothingとしてチェックを行う
            If Validation.Equals(activityStaffCode, EmptyString) Then

                activityStaffCode = Nothing

            End If

            ' 受付スタッフコードの要素内が空欄の場合、Nothingとしてチェックを行う
            If Validation.Equals(receptionStaffCode, EmptyString) Then

                receptionStaffCode = Nothing

            End If

            ' スケジュール区分が来店予約の場合
            If IsFlgEquals(scheduleDivString, ScheDuleDiv.VisitReservation) Then

                ' 活動スタッフコードが設定されていて、受付担当スタッフコードが未設定の場合、正常
                If activityStaffCode IsNot Nothing And _
                    receptionStaffCode Is Nothing Then

                    Return

                End If
                'それ以外は全てエラーデータとする
                Throw New ApplicationException(ReturnCode.UniqueError + ReturnCode.StaffCodeError)

            End If

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

            ' dataTableを分解する
            For Each dataRow As IC3040403DataSet.StaffCodeDataTableRow In targetDataTable

                If dataRow.ACTSTAFFCD IsNot Nothing Then

                    staffCodeList = CheckStaffCodeString(dataRow.ACTSTAFFCD, staffCodeList)

                End If

                If dataRow.RECSTAFFCD IsNot Nothing Then

                    staffCodeList = CheckStaffCodeString(dataRow.RECSTAFFCD, staffCodeList)
                End If

            Next

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

            If target Is Nothing Then

                Return staffCodeList

            End If

            ' 重複が存在するかチェックする
            For Each staffCode As String In staffCodeList

                ' 重複が存在する場合は、処理を終了する。
                If Validation.Equals(target, staffCode) Then

                    Return staffCodeList

                End If

            Next

            ' 重複が存在しなかった場合、新しくリストに追加する。
            staffCodeList.Add(target)

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
                    .COMPLETIONFLG = 0
                    .COMPLETIONDATE = Nothing
                    .DELFLG = 0
                    .DELDATE = Nothing
                    .CREATEACCOUNT = activityCreateStaff
                    .UPDATEACCOUNT = activityCreateStaff
                    .CREATEID = CreateId
                    .UPDATEID = UpdateId
                    .PARENTDIV = scheduleElement.ParentDiv

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable

                    Dim count As Integer = adapter.InsertCalTodoItem(dataRow)

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

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoItemDataTable

                    Dim count As Integer = adapter.UpdateCalTodoItem(dataRow)

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
        Private Function BizUpdateDeleteFlgCalTodoItem(ByVal calenderId As String, _
                                                        ByVal todoId As String, _
                                                        ByVal delDate As String, _
                                                        ByVal activityCreateStaff As String) As Integer


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
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetDeleteFlgCalTodoItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateDeleteFlgCalTodoItem > GetDeleteFlgCalTodoItemLock:-1")
                    End If
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateDeleteFlgCalTodoItem(dataRow)

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
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCompleteFlgCalTodoItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateCompleteFlgCalTodoItem > GetCompleteFlgCalTodoItemLock:-1")
                    End If
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateCompleteFlgCalTodoItem(dataRow)

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

            Using dataTable As New IC3040403DataSet.CalTodoAlarmDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalTodoAlarmDataTableRow = dataTable.NewCalTodoAlarmDataTableRow()

                dataRow.TODOID = todoId

                Using adapter As New IC3040403DataSetTableAdapters.CalTodoAlarmDataTable
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalTodoAlarmLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizDeleteCalTodoAlarm > GetCalTodoAlarmLock:-1")
                    End If
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.DeleteCalTodoAlarm(dataRow)

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

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable

                    Dim count As Integer = adapter.InsertCalEventItem(dataRow)

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

                End With

                ' Eventテーブルを更新します
                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalEventItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateCalEventItem > GetCalEventItemLock:-1")
                    End If
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateCalEventItem(dataRow)

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
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Private Function BizUpdateDeleteFlgCalEventItem(ByVal calenderId As String, _
                                                        ByVal eventId As String, _
                                                        ByVal delDate As String, _
                                                        ByVal activityCreateStaff As String) As Integer

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
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetDeleteFlgCalEventItemLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizUpdateDeleteFlgCalEventItem > GetDeleteFlgCalEventItemLock:-1")
                    End If
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.UpdateDeleteFlgCalEventItem(dataRow)

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

                End With

                Using adapter As New IC3040403DataSetTableAdapters.CalEventItemDataTable

                    Dim insertCount As Integer = adapter.InsertLinkEvent(dataRow)

                    If insertCount = 0 Then

                        Throw New ApplicationException(ReturnCode.XmlValueCheckError + ElementName.TodoID)

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

            Using dataTable As New IC3040403DataSet.CalEventAlarmDataTableDataTable

                ' DataRowから、DataTableを作成します。
                Dim dataRow As IC3040403DataSet.CalEventAlarmDataTableRow = dataTable.NewCalEventAlarmDataTableRow()

                dataRow.EVENTID = eventId

                Using adapter As New IC3040403DataSetTableAdapters.CalEventAlarmDataTable
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 START
                    If adapter.GetCalEventAlarmLock(dataRow) = -1 Then
                        Throw New ApplicationException("BizDeleteCalEventAlarm > GetCalEventAlarmLock:-1")
                    End If
'2019/03/01 SKFC二村 TR-V4-TMT-20190131-001 END
                    Dim count As Integer = adapter.DeleteCalEventAlarm(dataRow)

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

            Dim dataTable As IC3040403DataSet.StaffCodeDataTableDataTable

            Using adapter As New IC3040403DataSetTableAdapters.StaffCodeDataTable

                dataTable = adapter.SelectStaffCodeTodoItem(calenderId, todoId)

            End Using

            ' 更新処理の場合は、スケジュール区分に値がセットされている
            If scheduleDiv IsNot Nothing Then

                ' 更新した結果、スタッフコードチェックにひっかからないか調べます
                CheckStaffCodeUpdateDataBase(DataTable, scheduleDiv, scheduleElement)

            End If


            ' スタッフコードを重複していない限りはスタッフコードリストに格納します
            staffCodeList = CheckStaffCodeDataTable(DataTable, staffCodeList)

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

            Dim dataTable As IC3040403DataSet.StaffCodeDataTableDataTable

            Using adapter As New IC3040403DataSetTableAdapters.StaffCodeDataTable

                dataTable = adapter.SelectStaffCodeEventItem(calenderId, todoId)

            End Using

            ' 更新処理の場合は、スケジュール区分に値がセットされている
            If scheduleDiv IsNot Nothing Then

                ' 更新した結果、スタッフコードチェックにひっかからないか調べます
                CheckStaffCodeUpdateDataBase(dataTable, scheduleDiv, scheduleElement)

            End If

            ' スタッフコードを重複していない限りはスタッフコードリストに格納します
            staffCodeList = CheckStaffCodeDataTable(dataTable, staffCodeList)

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
