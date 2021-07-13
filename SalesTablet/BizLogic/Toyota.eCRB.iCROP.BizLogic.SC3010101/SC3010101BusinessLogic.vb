Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Common.Login.DataAccess

''' <summary>
''' ログイン認証を行う画面のビジネスロジックを実装する
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010101BusinessLogic
    Inherits BaseBusinessComponent

    Private Sub New()

    End Sub

    ''' <summary>
    ''' MacAddressに対応した販売店コードの取得処理
    ''' </summary>
    ''' <param name="macaddress">マックアドレス</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckDBConnection(ByVal macaddress As String) As SC3010101DataSet.SC3010101MacDataTableDataTable
        Return SC3010101TableAdapter.SelDlrCD(macaddress)
    End Function

End Class
