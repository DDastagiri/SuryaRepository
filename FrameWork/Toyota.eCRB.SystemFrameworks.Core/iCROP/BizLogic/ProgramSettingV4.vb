'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ProgramSettingV4.vb
'─────────────────────────────────────
'機能： ProgramSettingV4
'補足： 
'作成： 2016/04/26 TCS 山口　（トライ店システム評価）他システム連携における複数店舗コード変換対応
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    Public Class ProgramSettingV4
        Inherits BaseBusinessComponent

#Region "GetProgramSettingV4"
        ''' <summary>
        ''' プログラム設定を取得。
        ''' </summary>
        ''' <param name="programCd">プログラムコード</param>
        ''' <param name="settingSection">設定セクション</param>
        ''' <param name="settingKey">設定キー</param>
        ''' <returns>ProgramSettingV4</returns>
        ''' <remarks>
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetProgramSettingV4(ByVal programCd As String, ByVal settingSection As String,
                                            ByVal settingKey As String) As ProgramSettingV4DataSet.PROGRAMSETTINGV4Row

            Dim programDt As ProgramSettingV4DataSet.PROGRAMSETTINGV4DataTable

            programDt = ProgramSettingV4TableAdapter.GetProgramSettingV4DataTable(programCd, settingSection, settingKey)

            If programDt.Rows.Count = 0 Then
                Return Nothing
            End If

            Return DirectCast(programDt.Rows(0), ProgramSettingV4DataSet.PROGRAMSETTINGV4Row)

        End Function
#End Region

    End Class

End Namespace
