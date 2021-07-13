Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace IC3040404.BizLogic

    ''' <summary>
    ''' グローバル変数   2011/12/12
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/10/11 SKFC 浦野【iOS6対応】サーバーのタイムゾーンをDBまたはConfigから取得する
    '''                                 方法に変更
    ''' </History>
    Public Class GlobalValue

        'カレンダー名称の既定値(DBから取れなかった場合。識別のため大文字にしている）
        Private Const DefaultDisplayName As String = "e-CRB CALENDAR"
        Private Const DefaultReferenceName As String = "e-CRB REFERENCE"

        '実行パス
        Private InRootPath As String

        'パス情報はDBから取得
        'InCalDavRootUrl　"/e-CRBInfoSync/DAV/CalDAV/IC3040404.aspx/"
        Private InCalDavRootPath As String
        Private InDisplayName As String
        Private InReferenceName As String

        ' サーバーのロカール
        Private InDefaultLocal As String = "Asia/Shanghai"
        '2012/10/11 SKFC 浦野【iOS6対応】サーバーのタイムゾーン名 START
        Private InDefaultLocalTime As String = "GMT+08:00"
        'Private InTokyoLocal As String = "Tokyo"   '廃止
        '2012/10/11 SKFC 浦野【iOS6対応】サーバーのタイムゾーン名 End

        ''' <summary>
        ''' Setter Getter
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property RootPath As String
            Get
                Return InRootPath
            End Get
            Set(ByVal Value As String)
                InRootPath = Value
            End Set
        End Property

        Property CalDavRootPath As String
            Get
                Return InCalDavRootPath
            End Get
            Set(ByVal Value As String)
                InCalDavRootPath = Value
            End Set
        End Property

        Property DisplayName As String
            Get
                Return InDisplayName
            End Get
            Set(ByVal Value As String)
                InDisplayName = Value
            End Set
        End Property

        Property ReferenceName As String
            Get
                Return InReferenceName
            End Get
            Set(ByVal Value As String)
                InReferenceName = Value
            End Set
        End Property

        ' 2012/10/11 SKFC 浦野【iOS6対応】自動取得するのでSetter削除 START       
        ''' <summary>
        ''' タイムゾーン名を返す　
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' 次のような文字列　"Asia/Shanghai"
        ''' </remarks>
        ReadOnly Property DefaultLocal As String
            Get
                Return InDefaultLocal
            End Get
        End Property
        ' 2012/10/11 SKFC 浦野【iOS6対応】自動取得するのでSetter削除 END

        ' 2012/10/11 SKFC 浦野【iOS6対応】プロパティ変更 START
        'プロパティ追加
        ''' <summary>
        ''' タイムゾーン文字列を返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' 次のような文字列　"GMT+08:00"
        ''' </remarks>
        ReadOnly Property DefaultLocalTime As String
            Get
                Return InDefaultLocalTime
            End Get
        End Property

        'プロパティ廃止
        'Property TokyoLocal As String
        '    Get
        '        Return InTokyoLocal
        '    End Get
        '    Set(ByVal Value As String)
        '        InTokyoLocal = Value
        '    End Set
        'End Property
        ' 2012/10/11 SKFC 浦野【iOS6対応】プロパティ変更 END

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            RootPath = ""

            Try
                Dim SystemEnv As New Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.SystemEnvSetting
                CalDavRootPath = SystemEnv.GetSystemEnvSetting("CALDAV_ROOT_URL").Item("PARAMVALUE")
                DisplayName = WebWordUtility.GetWord("IC3040404", 1)       'カレンダー名
                ReferenceName = WebWordUtility.GetWord("IC3040404", 2)     '照会名

                '既定値の設定（テーブルにデータが無い場合）
                If String.IsNullOrEmpty(DisplayName) Then
                    DisplayName = DefaultDisplayName
                End If
                If String.IsNullOrEmpty(ReferenceName) Then
                    ReferenceName = DefaultReferenceName
                End If

            Catch ex As ApplicationException
                Logger.Error("@@@ ERROR CALDAV_ROOT_URLが取得できません")
                Logger.Error("TBL_SYSTEMENVSETTING の PARAMNAME に CALDAV_ROOT_URLがありますか？")

            End Try

            'サーバーのタイムゾーン名を得る
            InDefaultLocalTime = GetTimeZone(InDefaultLocal)

        End Sub

        '2012/10/24 SKFC 浦野【iOS6対応】タイムゾーンとタイムゾーン名を得る START
        ''' <summary>
        ''' タイムゾーンとタイムゾーン名を得る
        ''' </summary>
        ''' <param name="StrTimeZoneName"></param>
        ''' <returns>StrTimeZone  タイムゾーンをあらわす文字列 </returns>
        ''' <remarks>
        ''' 返り値は次のような文字列　"GMT+09:00"
        ''' また引数StrTimeZoneNameはByRefで値が返る "Asia/Shanghai" など
        ''' 設定が読めない場合は、EROORログを出力する
        ''' </remarks>
        Public Function GetTimeZone(ByRef StrTimeZoneName As String) As String
            Const CALDAV_TIMEZONE_NAME As String = "CALDAV_TIMEZONE_NAME"
            Const CALDAV_TIMEZONE As String = "CALDAV_TIMEZONE"

            Dim StrTimeZone As String = ""

            '====== DBの「TBL_SYSTEMENVSETTING」テーブルから読む場合  ======
            'Try
            '    Dim s As New SystemEnvSetting
            '    Dim row As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            '    row = s.GetSystemEnvSetting(CALDAV_TIMEZONE_NAME)
            '    StrTimeZoneName = row.Item("PARAMVALUE").ToString

            '    row = s.GetSystemEnvSetting(CALDAV_TIMEZONE)
            '    StrTimeZone = row.Item("PARAMVALUE")
            '    s = Nothing

            'Catch ex As ApplicationException
            '    Logger.Error("[GetTimeZone] Not Found TimeZone Settings. Check DAV\CalDAV\web.config")
            'End Try

            '====== Web.Configから読む場合  ======
            Try
                StrTimeZoneName = System.Web.Configuration.WebConfigurationManager.AppSettings(CALDAV_TIMEZONE_NAME)
                StrTimeZone = System.Web.Configuration.WebConfigurationManager.AppSettings(CALDAV_TIMEZONE)

            Catch ex As ApplicationException
                Logger.Error("[GetTimeZone] Not Found TimeZone Settings. Check DAV\CalDAV\web.config")
            End Try

            'Add 2012/10/24 【エラーログを出力します】
            If String.IsNullOrEmpty(StrTimeZoneName) OrElse _
                String.IsNullOrEmpty(StrTimeZone) Then
                Logger.Error("[GetTimeZone] Not Found TimeZone Settings. Check DAV\CalDAV\web.config")
            End If

            Logger.Info("[GetTimeZone] TimeZone:" & StrTimeZone & "[" & StrTimeZoneName & "]")

            Return StrTimeZone

        End Function
        '2012/10/24 SKFC 浦野【iOS6対応】タイムゾーンとタイムゾーン名を得る END

    End Class

End Namespace