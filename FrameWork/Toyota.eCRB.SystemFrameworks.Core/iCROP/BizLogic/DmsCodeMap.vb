'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'DmsCodeMap.vb
'─────────────────────────────────────
'機能： DmsCodeMap
'補足： 
'作成： 2014/08/29 TCS 武田 Next追加要件
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    Public Class DmsCodeMap
        Inherits BaseBusinessComponent

#Region "GetDmsCodeMap"
        ''' <summary>
        ''' 基幹コードを取得。
        ''' </summary>
        ''' <param name="dmsCodeType">基幹コード区分</param>
        ''' <param name="icropCode1">i-CROPコード1</param>
        ''' <param name="icropCode2">i-CROPコード2</param>
        ''' <param name="icropCode3">i-CROPコード3</param>
        ''' <returns>DmsCodeMapRow</returns>
        ''' <remarks>
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetDmsCodeMap(ByVal dmsCodeType As String, ByVal icropCode1 As String,
                                      Optional ByVal icropCode2 As String = "", Optional ByVal icropCode3 As String = "") As DmsCodeMapDataSet.DMSCODEMAPRow
            Dim dmsDt As DmsCodeMapDataSet.DMSCODEMAPDataTable

            dmsDt = DmsCodeMapTableAdapter.GetDmsCodeMapDataTable(dmsCodeType, icropCode1, icropCode2, icropCode3)

            If dmsDt.Rows.Count = 0 Then
                Return Nothing
            End If

            Return DirectCast(dmsDt.Rows(0), DmsCodeMapDataSet.DMSCODEMAPRow)

        End Function
#End Region

    End Class

End Namespace
