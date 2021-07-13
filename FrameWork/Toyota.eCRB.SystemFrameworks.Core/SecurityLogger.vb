'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' セキュリティログの出力機能を提供するクラスです。
    ''' ログの出力設定は、外部ファイルとして定義されます。
    ''' </summary>
    ''' <remarks>
    ''' メンバにアクセスするために、静的クラスのインスタンスを宣言する必要はありません。
    ''' このクラスはアセンブリ外に公開します。
    ''' このクラスは継承できません。
    ''' </remarks>

    Public NotInheritable Class SecurityLogger

#Region "定数"
        ''' <summary>
        ''' ※メッセージ取得元が決まっていないため、固定文言出力
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SECURITY_LOGIN_MESSAGE As String = "login error"

        Private Const DefaultLogDateTimeFormat = "yyyy/MM/dd_HH:mm:ss.fff"

#End Region

#Region "変数"
        ''' <summary>
        ''' TraceLoggerインスタンスを格納します。
        ''' </summary>
        Private Shared _SecurityLogger As New TraceLogger("SecurityLog")

#End Region

#Region "SecurityLoggerInstance"
        ''' <summary>
        ''' TraceLoggerインスタンスを取得します。
        ''' </summary>
        Private Shared ReadOnly Property SecurityLoggerInstance() As TraceLogger
            Get
                Return _SecurityLogger
            End Get
        End Property
#End Region

#Region "New"
        ''' <summary>
        ''' コンストラクタです。インスタンスを生成させないようにするため、修飾子はPrivateです。
        ''' </summary>
        Private Sub New()

        End Sub
#End Region

#Region "Security"
        Private Shared SecurityLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' セキュリティログを出力する、オンライン用API。
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        Public Shared Sub Security(ByVal msg As String)

            Dim now As DateTime = DateTime.Now

            ''出力設定ではないので終了
            If Not LoggerUtility.IsEnableSecurityLogSetting Then
                Return
            End If

            If SecurityLogDateTimeFormat Is Nothing Then
                Dim listener = SecurityLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    SecurityLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    SecurityLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If


            Dim log As New StringBuilder  ''ログ文字列
            log.Append(LoggerUtility.CreateWebHeader())
            log.Append(now.ToString(SecurityLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            SecurityLoggerInstance.TraceInformation(log.ToString)

        End Sub
#End Region

    End Class
End Namespace