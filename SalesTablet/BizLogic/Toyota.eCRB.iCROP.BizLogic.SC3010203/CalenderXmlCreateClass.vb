
Imports System.Text
Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration

Public Class CalenderXmlCreateClass

    ''' <summary>
    ''' カレンダーのXMLを生成します。
    ''' </summary>
    ''' <param name="startDate">開始月日</param>
    ''' <param name="endDate">終了月日</param>
    ''' <param name="userAccount">ユーザーアカウント</param>
    ''' <param name="operationCode">権限区分</param>
    ''' <returns>生成したXML（String形）</returns>
    ''' <remarks></remarks>
    Public Function GetCalendarXml(ByVal startDate As Date, ByVal endDate As Date, ByVal userAccount As String, ByVal operationCode As String) As String

        ' XMLドキュメントを生成します。
        Dim calendarXml As XmlDocument = New XmlDocument()
        Dim xmlstring As StringBuilder = New StringBuilder()

        xmlstring.Append("<?xml version=""1.0"" encoding=""UTF-8""?>")
        xmlstring.Append("<Calendar>")
  
        xmlstring.Append(Me.GetNtivEvent())
        xmlstring.Append(Me.GetService())
        xmlstring.Append(Me.GetFll())
        xmlstring.Append("</Calendar>")

        calendarXml.LoadXml(xmlstring.ToString)

        ' XMLドキュメントをString型に置き換えます。
        Dim calendarElement As XmlElement = calendarXml.DocumentElement
        Dim calendarString As String = calendarElement.OuterXml

        Return calendarString
    End Function

    Public Function GetFll() As String
        Dim xmlstring As StringBuilder = New StringBuilder()
        Dim dtToday As Date = Date.Now

        xmlstring.Append("<Detail>")
        xmlstring.Append("<Common>")
        xmlstring.Append("  <CreateLocation>1</CreateLocation>")
        xmlstring.Append("  <DealerCode>44B40</DealerCode>")
        xmlstring.Append("  <BranchCode>01</BranchCode>")
        xmlstring.Append("  <ScheduleID>3267</ScheduleID>")
        xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
        xmlstring.Append("</Common>")
        xmlstring.Append("<ScheduleInfo>")
        xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
        xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
        xmlstring.Append("  <DmsID>11111111</DmsID>")
        xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
        xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
        xmlstring.Append("</ScheduleInfo>")
        xmlstring.Append(Me.CreateVEvent("○○様　来店フォロー", "2", "09:00:00", "10:00:00", False, 1))
        xmlstring.Append(Me.CreateVTodo("○○様　来店フォロー", "2", "09:00:00", "10:00:00", 1, "1"))
        xmlstring.Append("</Detail>")

        xmlstring.Append("<Detail>")
        xmlstring.Append("<Common>")
        xmlstring.Append("  <CreateLocation>1</CreateLocation>")
        xmlstring.Append("  <DealerCode>44B40</DealerCode>")
        xmlstring.Append("  <BranchCode>01</BranchCode>")
        xmlstring.Append("  <ScheduleID>3279</ScheduleID>")
        xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
        xmlstring.Append("</Common>")
        xmlstring.Append("<ScheduleInfo>")
        xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
        xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
        xmlstring.Append("  <DmsID>11111111</DmsID>")
        xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
        xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
        xmlstring.Append("</ScheduleInfo>")
        xmlstring.Append(Me.CreateVTodo("○×様　来店フォロー", "2", "12:00:00", "13:00:00", 1, "0"))
        xmlstring.Append("</Detail>")

        xmlstring.Append("<Detail>")
        xmlstring.Append("<Common>")
        xmlstring.Append("  <CreateLocation>1</CreateLocation>")
        xmlstring.Append("  <DealerCode>44B40</DealerCode>")
        xmlstring.Append("  <BranchCode>01</BranchCode>")
        xmlstring.Append("  <ScheduleID>9000021643</ScheduleID>")
        xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
        xmlstring.Append("</Common>")
        xmlstring.Append("<ScheduleInfo>")
        xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
        xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
        xmlstring.Append("  <DmsID>11111111</DmsID>")
        xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
        xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
        xmlstring.Append("</ScheduleInfo>")
        xmlstring.Append(Me.CreateVTodo("○■様　来店フォロー", "2", "13:00:00", "14:00:00", 1, "0"))
        xmlstring.Append("</Detail>")
        Return xmlstring.ToString()
    End Function

    Public Function GetService() As String
        Dim xmlstring As StringBuilder = New StringBuilder()
        Dim dtToday As Date = Date.Now

        xmlstring.Append("<Detail>")
        xmlstring.Append("<Common>")
        xmlstring.Append("  <CreateLocation>1</CreateLocation>")
        xmlstring.Append("  <DealerCode>44B40</DealerCode>")
        xmlstring.Append("  <BranchCode>01</BranchCode>")
        xmlstring.Append("  <ScheduleID>1</ScheduleID>")
        xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
        xmlstring.Append("</Common>")
        xmlstring.Append("<ScheduleInfo>")
        xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
        xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
        xmlstring.Append("  <DmsID>11111111</DmsID>")
        xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
        xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
        xmlstring.Append("</ScheduleInfo>")
        xmlstring.Append(Me.CreateVEvent("○○様　入庫予約", "1", "15:00:00", "19:00:00", False))
        xmlstring.Append("</Detail>")
        Return xmlstring.ToString()
    End Function

    Public Function GetNtivEvent() As String
        Dim xmlstring As StringBuilder = New StringBuilder()

        xmlstring.Append("<Detail>")
        xmlstring.Append("<Common>")
        xmlstring.Append("   <CreateLocation>2</CreateLocation>")
        xmlstring.Append("</Common>")
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント１", "2", "10:00:00", "12:00:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント２", "2", "10:00:00", "12:00:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント３", "2", "10:00:00", "13:00:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント４", "2", "16:00:00", "17:30:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント１", "2", "10:00:00", "12:00:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント２", "2", "10:00:00", "12:00:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント３", "2", "11:00:00", "13:00:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント４", "2", "16:00:00", "17:30:00", False))
        xmlstring.Append(Me.CreateVEvent("ネイティブイベント終日１", "2", "00:00:00", "00:00:00", True))
        xmlstring.Append("</Detail>")

        Return xmlstring.ToString()
    End Function

    Public Function CreateVTodo(ByVal title As String, ByVal location As String, ByVal stDate As String, ByVal edDate As String, ByVal contactNo As Integer, ByVal eventFlg As String) As String
        Dim xmlstring As StringBuilder = New StringBuilder()
        Dim dtToday As Date = Date.Now
        xmlstring.Append("<VTodo>")
        xmlstring.Append("  <ContactNo>").Append(contactNo).Append("</ContactNo>")
        xmlstring.Append("  <Summary>").Append(title).Append("</Summary>")
        If stDate.Length > 0 Then
            xmlstring.Append("   <DtStart>").Append(dtToday.ToString("yyyy/MM/dd")).Append(" ").Append(stDate).Append("</DtStart>")
        End If
        xmlstring.Append("   <Due>").Append(dtToday.ToString("yyyy/MM/dd")).Append(" ").Append(edDate).Append("</Due>")
        xmlstring.Append("  <TimeFlg>1</TimeFlg>")
        xmlstring.Append("  <AllDayFlg>0</AllDayFlg>")
        xmlstring.Append("  <Description></Description>")
        If location = "1" Then
            xmlstring.Append("   <XiCropColor>""147,186,115,0.7""</XiCropColor>")
        Else
            xmlstring.Append("   <XiCropColor>").Append(GetBackColor(contactNo)).Append("</XiCropColor>")
        End If
        xmlstring.Append("  <VAlarm><Trigger>4</Trigger></VAlarm>")
        xmlstring.Append("  <TodoID>00000000000000000001</TodoID>")
        xmlstring.Append("  <CompFlg>0</CompFlg>")
        xmlstring.Append("  <EventFlg>").Append(eventFlg).Append("</EventFlg>")
        xmlstring.Append("</VTodo>")
        Return xmlstring.ToString()
    End Function

    Public Function CreateVEvent(ByVal title As String, ByVal location As String, ByVal stTime As String, ByVal edTime As String, ByVal dayEvent As Boolean, Optional ByVal contactNo As Integer = 1) As String
        Dim xmlstring As StringBuilder = New StringBuilder()
        Dim dtToday As Date = Date.Now
        xmlstring.Append("<VEvent>")
        If location <> "2" And contactNo <> 0 Then
            xmlstring.Append("<ContactNo>").Append(contactNo).Append("</ContactNo>")
        End If
        xmlstring.Append("   <Summary>").Append(title).Append("</Summary>")
        xmlstring.Append("   <DtStart>").Append(dtToday.ToString("yyyy/MM/dd")).Append(" ").Append(stTime).Append("</DtStart>")
        xmlstring.Append("   <DtEnd>").Append(dtToday.ToString("yyyy/MM/dd")).Append(" ").Append(edTime).Append("</DtEnd>")
        xmlstring.Append("   <TimeFlg>1</TimeFlg>")
        If dayEvent Then
            xmlstring.Append("   <AllDayFlg>1</AllDayFlg>")
        Else
            xmlstring.Append("   <AllDayFlg>0</AllDayFlg>")
        End If

        xmlstring.Append("   <Description></Description>")
        If location = "1" Then
            xmlstring.Append("   <XiCropColor>""147,186,115,0.7""</XiCropColor>")
        Else
            xmlstring.Append("   <XiCropColor>").Append(GetBackColor(contactNo)).Append("</XiCropColor>")
        End If
        xmlstring.Append("   <VAlarm><Trigger>4</Trigger></VAlarm>")
        xmlstring.Append("   <EventID>1000000000000000000</EventID>")
        xmlstring.Append("   <LinkTodoID>00000000000000000001</LinkTodoID>")
        xmlstring.Append("   <UpdateDate>2011/11/15 18:00:00</UpdateDate>")
        xmlstring.Append("</VEvent>")
        Return xmlstring.ToString()
    End Function

    Private Function GetBackColor(ByVal contactNo As Integer) As String
        Select Case contactNo
            Case 1 : Return """128,177,206,0.7"""
            Case 2 : Return """128,177,206,0.7"""
            Case 3 : Return """128,177,206,0.7"""
            Case 4 : Return """128,177,206,0.7"""
            Case 5 : Return """128,177,206,0.7"""
            Case 6 : Return """192,139,212,0.7"""
        End Select
        Return String.Empty
    End Function

End Class
