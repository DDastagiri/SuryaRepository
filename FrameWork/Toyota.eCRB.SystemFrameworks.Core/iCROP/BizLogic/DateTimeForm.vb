Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBL_DATETIMEFORMより日付フォーマットを取得
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class DateTimeForm

        ''' <summary>
        ''' 日付フォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _dataTimeFormTable As Dictionary(Of Integer, String)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>プライベートコンストラクタです。</remarks>
        Private Sub New()

        End Sub


        ''' <summary>
        ''' TBL_DATETIMEFORMより日付フォーマットを取得
        ''' </summary>
        ''' <remarks></remarks>
        Friend Shared Sub LoadDateTimeForm()

            Dim cntcd As String = EnvironmentSetting.CountryCode

            Dim dt As DateTimeFormDataSet.TBL_DATETIMEFORMDataTable = DateTimeFormTableAdapter.GetDateTimeForm(cntcd)

            _dataTimeFormTable = New Dictionary(Of Integer, String)()

            For Each dr As DateTimeFormDataSet.TBL_DATETIMEFORMRow In dt.Rows

                _dataTimeFormTable.Add(CInt(dr.CONVID), dr.FORMAT)

            Next

        End Sub

        ''' <summary>
        ''' 指定された変換IDのフォーマットを返却
        ''' </summary>
        ''' <param name="convid">変換ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Shared Function GetDateTimeForm(convid As Integer) As String

            Dim format As String = Nothing

            If _dataTimeFormTable.ContainsKey(convid) Then

                '指定されたフォーマットが存在するので格納
                format = _dataTimeFormTable(convid)
            End If

            Return format

        End Function

    End Class
End Namespace
