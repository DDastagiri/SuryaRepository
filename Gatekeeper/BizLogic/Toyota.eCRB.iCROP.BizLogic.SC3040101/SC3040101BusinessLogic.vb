Imports Toyota.eCRB.Tool.Message.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web

''' <summary>
''' SC3040101
''' メインメニューの重要事項に表示する内容を登録する画面のビジネスロジックを実装する。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3040101BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3040101BusinessLogic



#Region "定数"
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM As String = "SC3040101"
#End Region

#Region "デフォルトコンストラクタ処理"
    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub
#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 連絡事項登録処理
    ''' </summary>
    ''' <param name="insMessageDataTable">連絡事項登録データテーブル</param>
    ''' <returns>成功:True/失敗:False</returns>
    ''' <remarks>本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。</remarks>
    ''' <seealso>InsertMessages</seealso>
    <EnableCommit()>
    Public Function InsertPost(ByVal insMessageDataTable As SC3040101DataSet.SC3040101MessageInfoDataTable) As Boolean Implements ISC3040101BusinessLogic.InsertPost

        'データテーブルの検証
        If insMessageDataTable Is Nothing Then
            Return False
        End If

        Dim dr As SC3040101DataSet.SC3040101MessageInfoRow = CType(insMessageDataTable.Rows(0), SC3040101DataSet.SC3040101MessageInfoRow)

        'ログインユーザの情報を格納
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim result As Boolean

        Dim account As String = staffInfo.Account
        Dim dlrcd As String = staffInfo.DlrCD
        Dim brncd As String = staffInfo.BrnCD

        dr.DLRCD = dlrcd
        dr.STRCD = brncd
        dr.ACCOUNT = account
        dr.SYSTEM = C_SYSTEM

        Dim insMessage As Integer = SC3040101TableAdapter.InsertMessages(insMessageDataTable)

        If insMessage <> 1 Then
            result = False
        Else
            result = True
        End If

        Return result

    End Function

#End Region


End Class
