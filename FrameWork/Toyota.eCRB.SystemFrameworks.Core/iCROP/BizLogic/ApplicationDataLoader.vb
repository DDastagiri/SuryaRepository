Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' アプリケーション共通で必要なデータの読み込みを行うクラスです。
    ''' </summary>
    ''' <remarks>このクラスはアプリケーション開始時に使用します。</remarks>
    Public NotInheritable Class ApplicationDataLoader

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ''' <summary>
        ''' ロック待機時間PARAMNAME
        ''' </summary>
        ''' <remarks></remarks>
        Private Const UPDATE_LOCK_TIMEOUT As String = "UPDATE_LOCK_TIMEOUT"
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Private Sub New()

        End Sub

        ''' <summary>
        ''' アプリケーション共通で必要なデータを読み込みます。
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub Load()

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            '文言の読み込み
            WordResourceManager.LoadWord()

            '日付フォーマット読み込み
            DateTimeForm.LoadDateTimeForm()

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            Dim sysenv As New SystemEnvSetting
            sysenv.GetSystemEnvSetting(UPDATE_LOCK_TIMEOUT)

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        End Sub

    End Class

End Namespace