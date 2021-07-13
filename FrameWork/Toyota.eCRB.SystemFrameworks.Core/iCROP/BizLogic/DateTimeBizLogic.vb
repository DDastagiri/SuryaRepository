Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' DBの現在時刻を取得するクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Class DateTimeBizLogic

        Private Sub New()

        End Sub

#Region "定数"
        ''2013/06/30 TCS 坂井 2013/10対応版 既存流用 START ADD
        ''' <summary>
        ''' BLANK判定用定数 VALUE(値:' ')
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_BLANK As String = " "
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
#End Region

        ''' <summary>
        ''' DBの現在時刻を取得します。
        ''' </summary>
        ''' <returns>DBの現在時刻</returns>
        ''' <remarks>DBの現在時刻を取得します。</remarks>
        Friend Shared Function GetNow() As Date

            Dim dt As DateTimeDataSet.DATETIMEDataTable = DateTimeTableAdapter.GetNow()

            Dim dr As DateTimeDataSet.DATETIMERow = DirectCast(dt.Rows(0), DateTimeDataSet.DATETIMERow)

            Return dr.DBDATE

        End Function

        ''' <summary>
        ''' 時差を考慮してDBの現在時刻を取得します。
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <returns>DBの現在時刻(時差考慮)</returns>
        ''' <remarks>時差を考慮してDBの現在時刻を取得します。</remarks>
        Friend Shared Function GetNow(ByVal dlrcd As String, ByVal strcd As String) As Date

            Dim dt As DateTimeDataSet.DATETIMEDataTable = DateTimeTableAdapter.GetNow(dlrcd, strcd)

            If 0 < dt.Rows.Count Then
                Dim dr As DateTimeDataSet.DATETIMERow = DirectCast(dt.Rows(0), DateTimeDataSet.DATETIMERow)

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                Dim time As String = dr.TIME_DIFF

                If C_BLANK.Equals(time) Then
                    Return dr.DBDATE
                Else
                    Dim sign As String
                    sign = time.Substring(0, 1)
                    Dim tmpHour As String
                    tmpHour = time.Substring(1, 2)
                    Dim tmpMinute As String
                    tmpMinute = time.Substring(4, 2)
                    Dim intMinute As Integer
                    intMinute = CInt(tmpMinute)
                    Dim intHour As Integer = CInt(tmpHour)

                    Dim dbTime As Date
                    dbTime = dr.DBDATE
                    If sign.Equals("-") Then
                        Dim retTime As Date
                        retTime = DateAdd("h", intHour * (-1), dbTime)
                        retTime = DateAdd("n", intMinute * (-1), retTime)
                        Return retTime
                    ElseIf sign.Equals("+") Then
                        Dim retTime As Date
                        retTime = DateAdd("h", intHour, dbTime)
                        retTime = DateAdd("n", intMinute, retTime)
                        Return retTime
                    End If

                End If
                    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

                    If Not dr.IsDBDATENull Then
                        Return dr.DBDATE
                    End If
            End If

            Return GetNow()

        End Function

    End Class

End Namespace
