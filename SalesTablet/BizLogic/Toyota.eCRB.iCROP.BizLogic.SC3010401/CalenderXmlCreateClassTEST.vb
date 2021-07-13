Imports System.Text
Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration


<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="TEST")>
Public Class ClassLibraryBusinessLogicTest


	''' <summary>
	''' カレンダーのXMLを生成します。
	''' </summary>
	''' <param name="startDate">開始月日</param>
	''' <param name="endDate">終了月日</param>
	''' <param name="userAccount">ユーザーアカウント</param>
	''' <param name="operationCode">権限区分</param>
	''' <returns>生成したXML（String形）</returns>
	''' <remarks></remarks>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="userAccount")>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="startDate")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="operationCode")> Public Function GetCalender(ByVal startDate As Date, ByVal endDate As Date, ByVal userAccount As String, ByVal operationCode As String) As String

		' XMLドキュメントを生成します。
		Dim calendarXml As XmlDocument = New XmlDocument()
		Dim xmlstring As StringBuilder = New StringBuilder()

		xmlstring.Append("<?xml version=""1.0"" encoding=""UTF-8""?>")
		xmlstring.Append("<Calendar>")

		'xmlstring.Append(Me.GetNtivEvent())
		xmlstring.Append(Me.GetService(endDate))
		'xmlstring.Append(Me.GetFll())
		xmlstring.Append("</Calendar>")

		calendarXml.LoadXml(xmlstring.ToString)

		' XMLドキュメントをString型に置き換えます。
		Dim calendarElement As XmlElement = calendarXml.DocumentElement
		Dim calendarString As String = calendarElement.OuterXml

		Return calendarString
	End Function

	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId:="System.DateTime.ToString(System.String)")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId:="dtToday")> Public Function GetFll() As String
		Dim xmlstring As StringBuilder = New StringBuilder()
		Dim dtToday As Date = Date.Now

		Dim func As Func(Of String) = _
		 Function() As String
			 Dim xmlstring2 As StringBuilder = New StringBuilder()
			 xmlstring2.Append("<Common>")
			 xmlstring2.Append("  <CreateLocation>1</CreateLocation>")
			 xmlstring2.Append("  <DealerCode>55555</DealerCode>")
			 xmlstring2.Append("  <BranchCode>555</BranchCode>")
			 'xmlstring2.Append("  <ScheduleID>9000024569</ScheduleID>")
			 xmlstring2.Append("  <ScheduleID>1234567890</ScheduleID>")
			 xmlstring2.Append("  <ScheduleDiv>0</ScheduleDiv>")
			 xmlstring2.Append("</Common>")
			 xmlstring2.Append("<ScheduleInfo>")
			 xmlstring2.Append("  <CustomerDiv>0</CustomerDiv>")
			 xmlstring2.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
			 xmlstring2.Append("  <DmsID>11111111</DmsID>")
			 xmlstring2.Append("  <CustomerName>顧客名</CustomerName>")
			 xmlstring2.Append("  <ReceptionDiv></ReceptionDiv>")
			 xmlstring2.Append("</ScheduleInfo>")
			 Return xmlstring2.ToString()
		 End Function

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("C1様", "2", "", "21:00:00", 1, "0", "", "000"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("C2様", "2", "", "21:00:00", 2, "0", "0", "000"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("C3様", "2", "", "21:00:00", 3, "0", "0", "000"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("C4様", "2", "", "21:00:00", 4, "0", "0", "000"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("C5様", "2", "", "21:00:00", 5, "0", "0", "000"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("C6様", "2", "", "21:00:00", 6, "0", "0", "000"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("C6様 時間なし", "2", "", "12:10:00", 4, "0", "0", "0"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("完了 ", "2", "", Date.Now.AddDays(-2).ToString("yyyy/MM/dd") & " 12:00:00", 6, "0", "1", "1"))
		xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append(func.Invoke())
		xmlstring.Append(Me.CreateVTodo("未完了 ", "2", "", Date.Now.AddYears(-1).ToString("yyyy/MM/dd") & " 12:00:00", 6, "0", "0", "1"))
		xmlstring.Append("</Detail>")
		Return xmlstring.ToString()
	End Function

	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId:="System.DateTime.ToString(System.String)")> Public Function GetService(edDate As Date) As String
		Dim xmlstring As StringBuilder = New StringBuilder()
		Dim dtToday As Date = Date.Now
		Dim eddatewk As Date = New Date(edDate.Year, edDate.Month, edDate.Day)

		'xmlstring.Append("<Detail>")
		'xmlstring.Append("<Common>")
		'xmlstring.Append("  <CreateLocation>1</CreateLocation>")
		'xmlstring.Append("  <DealerCode>44B40</DealerCode>")
		'xmlstring.Append("  <BranchCode>01 </BranchCode>")
		'xmlstring.Append("  <ScheduleID>1234567891</ScheduleID>")
		'xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
		'xmlstring.Append("</Common>")
		'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称００１００００００００００００ ", "2", "", Date.Now.AddDays(-2).ToString("yyyy/MM/dd") & " 10:00:00", 1, "0", "0", "001", ))
		'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称００２ ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 11:00:01", 2, "0", "0", "001"))
		'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称００３ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 00:00:00", 3, "0", "0", "001", "1", "1"))
		'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称００４ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 00:00:00", 4, "0", "0", "001", "0", "0"))
		'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称００５ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 22:00:00", 4, "0", "0", "001", "1", "0"))
		'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称００６ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 00:00:00", 4, "0", "0", "001", "0", "1"))
		'If Date.Compare(eddatewk, dtToday) = 1 Then
		'	xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称００７ ", "2", "", Date.Now.AddDays(+1).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "001"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１１ ", "2", "", Date.Now.AddDays(+7).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１２ ", "2", "", Date.Now.AddDays(+8).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１３ ", "2", "", Date.Now.AddDays(+9).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１４ ", "2", "", Date.Now.AddDays(+10).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１５ ", "2", "", Date.Now.AddDays(+11).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１６ ", "2", "", Date.Now.AddDays(+12).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１７ ", "2", "", Date.Now.AddDays(+13).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１８ ", "2", "", Date.Now.AddDays(+14).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１９ ", "2", "", Date.Now.AddDays(+15).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２０ ", "2", "", Date.Now.AddDays(+16).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２１ ", "2", "", Date.Now.AddDays(+17).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２２ ", "2", "", Date.Now.AddDays(+18).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２３ ", "2", "", Date.Now.AddDays(+19).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２４ ", "2", "", Date.Now.AddDays(+20).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２５ ", "2", "", Date.Now.AddDays(+21).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２６ ", "2", "", Date.Now.AddDays(+22).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２７ ", "2", "", Date.Now.AddDays(+23).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２８ ", "2", "", Date.Now.AddDays(+24).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０２９ ", "2", "", Date.Now.AddDays(+25).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３０ ", "2", "", Date.Now.AddDays(+26).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３１ ", "2", "", Date.Now.AddDays(+27).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３２ ", "2", "", Date.Now.AddDays(+28).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３３ ", "2", "", Date.Now.AddDays(+29).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３４ ", "2", "", Date.Now.AddDays(+30).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３５ ", "2", "", Date.Now.AddDays(+31).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３６ ", "2", "", Date.Now.AddDays(+32).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３７ ", "2", "", Date.Now.AddDays(+33).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３８ ", "2", "", Date.Now.AddDays(+34).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０３９ ", "2", "", Date.Now.AddDays(+35).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４０ ", "2", "", Date.Now.AddDays(+36).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４１ ", "2", "", Date.Now.AddDays(+37).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４２ ", "2", "", Date.Now.AddDays(+38).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４３ ", "2", "", Date.Now.AddDays(+39).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４４ ", "2", "", Date.Now.AddDays(+40).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４５ ", "2", "", Date.Now.AddDays(+41).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４６ ", "2", "", Date.Now.AddDays(+42).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４７ ", "2", "", Date.Now.AddDays(+43).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４８ ", "2", "", Date.Now.AddDays(+44).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０４９ ", "2", "", Date.Now.AddDays(+45).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５０ ", "2", "", Date.Now.AddDays(+46).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５１ ", "2", "", Date.Now.AddDays(+47).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５２ ", "2", "", Date.Now.AddDays(+48).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５３ ", "2", "", Date.Now.AddDays(+49).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５４ ", "2", "", Date.Now.AddDays(+50).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５５ ", "2", "", Date.Now.AddDays(+51).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５６ ", "2", "", Date.Now.AddDays(+52).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５７ ", "2", "", Date.Now.AddDays(+53).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５８ ", "2", "", Date.Now.AddDays(+54).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０５９ ", "2", "", Date.Now.AddDays(+55).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６０ ", "2", "", Date.Now.AddDays(+66).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６１ ", "2", "", Date.Now.AddDays(+57).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６２ ", "2", "", Date.Now.AddDays(+58).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６３ ", "2", "", Date.Now.AddDays(+59).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６４ ", "2", "", Date.Now.AddDays(+60).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６５ ", "2", "", Date.Now.AddDays(+61).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６６ ", "2", "", Date.Now.AddDays(+62).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６７ ", "2", "", Date.Now.AddDays(+63).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６８ ", "2", "", Date.Now.AddDays(+64).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０６９ ", "2", "", Date.Now.AddDays(+65).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７０ ", "2", "", Date.Now.AddDays(+66).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７１ ", "2", "", Date.Now.AddDays(+67).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７２ ", "2", "", Date.Now.AddDays(+68).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７３ ", "2", "", Date.Now.AddDays(+69).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７４ ", "2", "", Date.Now.AddDays(+70).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７５ ", "2", "", Date.Now.AddDays(+71).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７６ ", "2", "", Date.Now.AddDays(+72).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７７ ", "2", "", Date.Now.AddDays(+73).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７８ ", "2", "", Date.Now.AddDays(+74).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０７９ ", "2", "", Date.Now.AddDays(+75).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８０ ", "2", "", Date.Now.AddDays(+76).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８１ ", "2", "", Date.Now.AddDays(+77).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８２ ", "2", "", Date.Now.AddDays(+78).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８３ ", "2", "", Date.Now.AddDays(+79).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８４ ", "2", "", Date.Now.AddDays(+80).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８５ ", "2", "", Date.Now.AddDays(+81).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８６ ", "2", "", Date.Now.AddDays(+82).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８７ ", "2", "", Date.Now.AddDays(+83).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８８ ", "2", "", Date.Now.AddDays(+84).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０８９ ", "2", "", Date.Now.AddDays(+85).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９０ ", "2", "", Date.Now.AddDays(+86).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９１ ", "2", "", Date.Now.AddDays(+87).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９２ ", "2", "", Date.Now.AddDays(+88).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９３ ", "2", "", Date.Now.AddDays(+89).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９４ ", "2", "", Date.Now.AddDays(+90).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９５ ", "2", "", Date.Now.AddDays(+91).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９６ ", "2", "", Date.Now.AddDays(+92).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９７ ", "2", "", Date.Now.AddDays(+93).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９８ ", "2", "", Date.Now.AddDays(+94).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０９９ ", "2", "", Date.Now.AddDays(+95).ToString("yyyy/MM/dd") & " 14:00:04", 5, "0", "0", "1"))
		'          'xmlstring.Append(Me.CreateVTodo("客１ ＴＯＤＯ名称０１００ ", "2", "", Date.Now.AddDays(+96).ToString("yyyy/MM/dd") & " 15:00:05", 6, "0", "0", "1"))
		'End If
		'xmlstring.Append("<ScheduleInfo>")
		'xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
		'xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
		'xmlstring.Append("  <DmsID>11111111</DmsID>")
		'xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
		'xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
		'xmlstring.Append("</ScheduleInfo>")
		'xmlstring.Append(Me.CreateVEvent("○○様　入庫予約", "1", "21:00:00", "23:00:00", False))
		'xmlstring.Append("</Detail>")

		'xmlstring.Append("<Detail>")
		'xmlstring.Append("<Common>")
		'xmlstring.Append("  <CreateLocation>1</CreateLocation>")
		'xmlstring.Append("  <DealerCode>44B40</DealerCode>")
		'xmlstring.Append("  <BranchCode>01 </BranchCode>")
		'xmlstring.Append("  <ScheduleID>1234567892</ScheduleID>")
		'xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
		'xmlstring.Append("</Common>")
		'xmlstring.Append(Me.CreateVTodo("客２ ＴＯＤＯ名称１ ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", 1, "0", "0", "001"))
		'xmlstring.Append(Me.CreateVTodo("客２ ＴＯＤＯ名称２ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 19:00:01", 2, "0", "0", "002"))
		'If Date.Compare(eddatewk, dtToday) = 1 Then
		'	xmlstring.Append(Me.CreateVTodo("客２ ＴＯＤＯ名称３ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
		'	xmlstring.Append(Me.CreateVTodo("客２ ＴＯＤＯ名称４ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
		'End If
		'xmlstring.Append("<ScheduleInfo>")
		'xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
		'xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
		'xmlstring.Append("  <DmsID>1234567891</DmsID>")
		'xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
		'xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
		'xmlstring.Append("</ScheduleInfo>")
		'xmlstring.Append("</Detail>")

		'      xmlstring.Append("<Detail>")
		'      xmlstring.Append("<Common>")
		'      xmlstring.Append("  <CreateLocation>1</CreateLocation>")
		'      xmlstring.Append("  <DealerCode>44B40</DealerCode>")
		'      xmlstring.Append("  <BranchCode>01 </BranchCode>")
		'      xmlstring.Append("  <ScheduleID>1234567893</ScheduleID>")
		'      xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
		'      xmlstring.Append("</Common>")
		'      xmlstring.Append(Me.CreateVTodo("客３ ＴＯＤＯ名称１ ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", 1, "0", "0", "001"))
		'      xmlstring.Append(Me.CreateVTodo("客３ ＴＯＤＯ名称２ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 11:00:01", 2, "0", "0", "002"))
		'      If Date.Compare(eddatewk, dtToday) = 1 Then
		'	xmlstring.Append(Me.CreateVTodo("客３ ＴＯＤＯ名称３ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
		'End If
		'      xmlstring.Append("<ScheduleInfo>")
		'      xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
		'      xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
		'      xmlstring.Append("  <DmsID>11111111</DmsID>")
		'      xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
		'      xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
		'      xmlstring.Append("</ScheduleInfo>")
		'      xmlstring.Append("</Detail>")

		'      xmlstring.Append("<Detail>")
		'      xmlstring.Append("<Common>")
		'      xmlstring.Append("  <CreateLocation>1</CreateLocation>")
		'      xmlstring.Append("  <DealerCode>44B40</DealerCode>")
		'      xmlstring.Append("  <BranchCode>01 </BranchCode>")
		'      xmlstring.Append("  <ScheduleID>1234567894</ScheduleID>")
		'      xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
		'      xmlstring.Append("</Common>")
		'      xmlstring.Append(Me.CreateVTodo("客４ ＴＯＤＯ名称１ ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", 1, "0", "0", "001"))
		'      xmlstring.Append(Me.CreateVTodo("客４ ＴＯＤＯ名称２ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 11:00:01", 2, "0", "0", "002"))
		'      If Date.Compare(eddatewk, dtToday) = 1 Then
		'	xmlstring.Append(Me.CreateVTodo("客４ ＴＯＤＯ名称３ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
		'End If
		'      xmlstring.Append("<ScheduleInfo>")
		'      xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
		'      xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
		'      xmlstring.Append("  <DmsID>11111111</DmsID>")
		'      xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
		'      xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
		'      xmlstring.Append("</ScheduleInfo>")
		'      xmlstring.Append("</Detail>")

		'xmlstring.Append("<Detail>")
		'xmlstring.Append("<Common>")
		'xmlstring.Append("  <CreateLocation>1</CreateLocation>")
		'xmlstring.Append("  <DealerCode>44B40</DealerCode>")
		'xmlstring.Append("  <BranchCode>01 </BranchCode>")
		'xmlstring.Append("  <ScheduleID>1234567895</ScheduleID>")
		'xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
		'xmlstring.Append("</Common>")
		'xmlstring.Append(Me.CreateVTodo("客５ ＴＯＤＯ名称１ ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", 1, "0", "0", "001"))
		'xmlstring.Append(Me.CreateVTodo("客５ ＴＯＤＯ名称２ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 11:00:01", 2, "0", "0", "002"))
		'If Date.Compare(eddatewk, dtToday) = 1 Then
		'	xmlstring.Append(Me.CreateVTodo("客５ ＴＯＤＯ名称３ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
		'End If
		'xmlstring.Append("<ScheduleInfo>")
		'xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
		'xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
		'xmlstring.Append("  <DmsID>11111111</DmsID>")
		'xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
		'xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
		'xmlstring.Append("</ScheduleInfo>")
		'xmlstring.Append("</Detail>")

		'xmlstring.Append("<Detail>")
		'xmlstring.Append("<Common>")
		'xmlstring.Append("  <CreateLocation>1</CreateLocation>")
		'xmlstring.Append("  <DealerCode>44B40</DealerCode>")
		'xmlstring.Append("  <BranchCode>01 </BranchCode>")
		'xmlstring.Append("  <ScheduleID>1234567896</ScheduleID>")
		'xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
		'xmlstring.Append("</Common>")
		'xmlstring.Append(Me.CreateVTodo("客６ ＴＯＤＯ名称１ ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", 1, "0", "0", "001"))
		'xmlstring.Append(Me.CreateVTodo("客６ ＴＯＤＯ名称２ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 11:00:01", 2, "0", "0", "002"))
		'If Date.Compare(eddatewk, dtToday) = 1 Then
		'	xmlstring.Append(Me.CreateVTodo("客６ ＴＯＤＯ名称３ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
		'End If
		'xmlstring.Append("<ScheduleInfo>")
		'xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
		'xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
		'xmlstring.Append("  <DmsID>11111111</DmsID>")
		'xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
		'xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
		'xmlstring.Append("</ScheduleInfo>")
		'xmlstring.Append("</Detail>")

        xmlstring.Append("<Detail>")
        xmlstring.Append("<Common>")
        xmlstring.Append("  <CreateLocation>1</CreateLocation>")
        xmlstring.Append("  <DealerCode>44B40</DealerCode>")
        xmlstring.Append("  <BranchCode>01 </BranchCode>")
        xmlstring.Append("  <ScheduleID>1234567897</ScheduleID>")
        xmlstring.Append("  <ScheduleDiv>2</ScheduleDiv>")
        xmlstring.Append("</Common>")
        xmlstring.Append(Me.CreateVTodo("客７ ＴＯＤＯ名称１ ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", 1, "0", "0", "001"))
        xmlstring.Append(Me.CreateVTodo("客７ ＴＯＤＯ名称２ ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 11:00:01", 2, "0", "0", "002"))
        If Date.Compare(eddatewk, dtToday) = 1 Then
            xmlstring.Append(Me.CreateVTodo("客７ ＴＯＤＯ名称３ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
        End If
        xmlstring.Append("<ScheduleInfo>")
        xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
        xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
        xmlstring.Append("  <DmsID>11111111</DmsID>")
        xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
        xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
        xmlstring.Append("</ScheduleInfo>")
        xmlstring.Append("</Detail>")

		xmlstring.Append("<Detail>")
		xmlstring.Append("<Common>")
		xmlstring.Append("  <CreateLocation>1</CreateLocation>")
		xmlstring.Append("  <DealerCode>44B40</DealerCode>")
		xmlstring.Append("  <BranchCode>01 </BranchCode>")
		xmlstring.Append("  <ScheduleID>1234567898</ScheduleID>")
		xmlstring.Append("  <ScheduleDiv>2</ScheduleDiv>")
		xmlstring.Append("</Common>")
        xmlstring.Append(Me.CreateVTodo("客８ ＴＯＤＯ名称１昨日 ", "2", "", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", 1, "0", "0", "001"))
		xmlstring.Append(Me.CreateVTodo("客８ ＴＯＤＯ名称２今日時刻あり ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 22:00:01", 2, "0", "0", "002", "1", "0"))
		xmlstring.Append(Me.CreateVTodo("客８ ＴＯＤＯ名称３今日時刻なし ", "2", "", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 22:00:01", 2, "0", "0", "002", "0", "0"))
		If Date.Compare(eddatewk, dtToday) = 1 Then
			xmlstring.Append(Me.CreateVTodo("客８ ＴＯＤＯ名称３明日 ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
		End If
		xmlstring.Append("<ScheduleInfo>")
		xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
		xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
		xmlstring.Append("  <DmsID>11111111</DmsID>")
		xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
		xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
		xmlstring.Append("</ScheduleInfo>")
		xmlstring.Append("</Detail>")

        xmlstring.Append("<Detail>")
        xmlstring.Append("<Common>")
        xmlstring.Append("  <CreateLocation>1</CreateLocation>")
        xmlstring.Append("  <DealerCode>44B40</DealerCode>")
        xmlstring.Append("  <BranchCode>01 </BranchCode>")
        xmlstring.Append("  <ScheduleID>1234567899</ScheduleID>")
        xmlstring.Append("  <ScheduleDiv>0</ScheduleDiv>")
        xmlstring.Append("</Common>")
        xmlstring.Append(Me.CreateVTodo("客９ 日またぎデータＦ過去Ｔ過去 ", "2", Date.Now.AddDays(-2).ToString("yyyy/MM/dd") & " 10:00:01", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 11:00:01", 6, "0", "0", "002"))
        xmlstring.Append(Me.CreateVTodo("客９ 日またぎデータＦ過去Ｔ今日 ", "2", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 22:00:01", 6, "0", "0", "002"))
        xmlstring.Append(Me.CreateVTodo("客９ 日またぎデータＦ過去Ｔ未来 ", "2", Date.Now.AddDays(-1).ToString("yyyy/MM/dd") & " 10:00:01", Date.Now.AddDays(1).ToString("yyyy/MM/dd") & " 10:00:01", 6, "0", "0", "001"))
        xmlstring.Append(Me.CreateVTodo("客９ 日またぎデータＦ今日Ｔ未来 ", "2", Date.Now.AddDays(0).ToString("yyyy/MM/dd") & " 10:00:01", Date.Now.AddDays(2).ToString("yyyy/MM/dd") & " 11:00:01", 6, "0", "0", "002"))
        If Date.Compare(eddatewk, dtToday) = 1 Then
            xmlstring.Append(Me.CreateVTodo("客９ ＴＯＤＯ名称３ ", "2", "", Date.Now.AddDays(+2).ToString("yyyy/MM/dd") & " 12:00:02", 3, "0", "0", "005"))
        End If
        xmlstring.Append("<ScheduleInfo>")
        xmlstring.Append("  <CustomerDiv>0</CustomerDiv>")
        xmlstring.Append("  <CustomerCode>0000000000000000001</CustomerCode>")
        xmlstring.Append("  <DmsID>11111111</DmsID>")
        xmlstring.Append("  <CustomerName>顧客名</CustomerName>")
        xmlstring.Append("  <ReceptionDiv></ReceptionDiv>")
        xmlstring.Append("</ScheduleInfo>")
        xmlstring.Append("</Detail>")

		Return xmlstring.ToString()
	End Function

	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")> Public Function GetNtivEvent() As String
		Dim xmlstring As StringBuilder = New StringBuilder()

		xmlstring.Append("<Detail>")
		xmlstring.Append("<Common>")
		xmlstring.Append("   <CreateLocation>2</CreateLocation>")
		xmlstring.Append("</Common>")
		xmlstring.Append(Me.CreateVEvent("ネイティブイベント１", "2", "02:00:00", "04:00:00", False))
		xmlstring.Append(Me.CreateVEvent("ネイティブイベント２", "2", "10:00:00", "12:00:00", False))
		xmlstring.Append(Me.CreateVEvent("ネイティブイベント３", "2", "10:00:00", "13:00:00", False))




		xmlstring.Append(Me.CreateVEvent("チップ１", "2", "15:00:00", "16:00:00", False))
		xmlstring.Append(Me.CreateVEvent("チップ２", "2", "15:01:00", "16:00:00", False))
		xmlstring.Append(Me.CreateVEvent("チップ３", "2", "15:02:00", "16:00:00", False))


		xmlstring.Append(Me.CreateVEvent("ネイティブイベント終日１ななななななななななな", "2", "00:00:00", "00:00:00", True))
		xmlstring.Append(Me.CreateVEvent("ネイティブイベント終日２", "2", "00:00:00", "00:00:00", True))
		xmlstring.Append(Me.CreateVEvent("ネイティブイベント終日３&lt;br/&gt;FBCD", "2", "00:00:00", "00:00:00", True))
		xmlstring.Append("</Detail>")

		Return xmlstring.ToString()
	End Function

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId:="System.DateTime.ToString(System.String)")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Process")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="ID")> Public Function CreateVTodo(ByVal title As String, ByVal location As String, ByVal stDate As String, ByVal edDate As String, ByVal contactNo As Integer, ByVal eventFlg As String, ByVal compFlg As String, ByVal ProcessID As String, Optional ByVal timeFlg As String = "1", Optional ByVal allDayFlg As String = "0") As String
        Dim xmlstring As StringBuilder = New StringBuilder()
        Dim dtToday As Date = Date.Now
        xmlstring.Append("<VTodo>")
        xmlstring.Append("  <ContactNo>").Append(contactNo).Append("</ContactNo>")
        xmlstring.Append("  <Summary>").Append(title).Append("</Summary>")
        If stDate.Length > 0 Then
            If stDate.Length <> 8 Then
                xmlstring.Append("   <DtStart>").Append(stDate).Append("</DtStart>")
            Else
                xmlstring.Append("   <DtStart>").Append(dtToday.ToString("yyyy/MM/dd")).Append(" ").Append(stDate).Append("</DtStart>")
            End If
        End If
        If edDate.Length <> 8 Then
            xmlstring.Append("   <Due>").Append(edDate).Append("</Due>")
        Else
            xmlstring.Append("   <Due>").Append(dtToday.ToString("yyyy/MM/dd")).Append(" ").Append(edDate).Append("</Due>")
        End If
        xmlstring.Append("  <TimeFlg>").Append(timeFlg).Append("</TimeFlg>")
        xmlstring.Append("  <AllDayFlg>").Append(allDayFlg).Append("</AllDayFlg>")
        xmlstring.Append("  <Description></Description>")
        If location = "1" Then
            xmlstring.Append("   <XiCropColor>""147,186,115,0.7""</XiCropColor>")
        Else
            xmlstring.Append("   <XiCropColor>").Append(GetBackColor(contactNo)).Append("</XiCropColor>")
        End If
        xmlstring.Append("  <VAlarm><Trigger>4</Trigger></VAlarm>")
        xmlstring.Append("  <TodoID>00000000000000000001</TodoID>")
        xmlstring.Append("  <CompFlg>").Append(compFlg).Append("</CompFlg>")
        xmlstring.Append("  <EventFlg>").Append(eventFlg).Append("</EventFlg>")
        xmlstring.Append("  <ProcessDiv>").Append(ProcessID).Append("</ProcessDiv>")
        xmlstring.Append("</VTodo>")
        Return xmlstring.ToString()
    End Function

	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId:="System.DateTime.ToString(System.String)")> Public Function CreateVEvent(ByVal title As String, ByVal location As String, ByVal stTime As String, ByVal edTime As String, ByVal dayEvent As Boolean, Optional ByVal contactNo As Integer = 1) As String
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

