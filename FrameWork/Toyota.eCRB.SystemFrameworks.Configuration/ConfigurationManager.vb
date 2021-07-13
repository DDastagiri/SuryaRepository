'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Toyota.eCRB.SystemFrameworksカスタム構成セクションの内容を取得するためのクラスです。
    ''' このクラスはアプリケーションコードで使用するためのものではありません。
    ''' </summary>
    ''' <remarks>
    ''' このクラスを利用した構成要素へのアクセスは高速ではありません。したがって、このクラスを利用して同一要素への繰り返し
    ''' のアクセスは行わないでください。そのような場合は、最初のアクセスの結果を Shared メンバに待避するなどの工夫を行って
    ''' ください。
    ''' </remarks>
    Public NotInheritable Class ConfigurationManager

        Private _config As System.Xml.XmlElement = Nothing

        ''' <summary>
        ''' インスタンスの生成をできないようにするためのデフォルトのコンストラクタです。
        ''' </summary>
        Public Sub New(ByVal config As System.Xml.XmlElement)
            Me._config = config
        End Sub

        ''' <summary>
        ''' セッションマネージャー設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property ScreenUrl() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("ScreenUrl")
            End Get
        End Property

        ''' <summary>
        ''' セッションマネージャー設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property TopPageUrl() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("TopPageUrl")
            End Get
        End Property

        ''' <summary>
        ''' セッションマネージャー設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property StaffDivision() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("StaffDivision")
            End Get
        End Property

        ''' <summary>
        ''' セッションマネージャー設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property Individual() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("Individual")
            End Get
        End Property

        ''' <summary>
        ''' セッションマネージャー設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property SessionManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("SessionManager")
            End Get
        End Property

        ''' <summary>
        ''' 環境設定取得クラス用設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property EnvironmentSetting() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("EnvironmentSetting")
            End Get
        End Property

        ''' <summary>
        ''' ログマネージャ設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property LogManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("LogManager")
            End Get
        End Property

        ''' <summary>
        ''' ログインマネージャ設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property LoginManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("LoginManager")
            End Get
        End Property

        ''' <summary>
        ''' セッションマネージャー設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property ConnectionManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("ConnectionManager")
            End Get
        End Property

        ''' <summary>
        ''' 暗号化クラス用設定情報を取得します。
        ''' </summary>
        ''' <value>暗号化クラス用設定情報</value>
        Public ReadOnly Property Encryption() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("Encryption")
            End Get
        End Property

        ''' <summary>
        ''' キャッシュクラス用設定情報を取得します。
        ''' </summary>
        ''' <value>キャッシュクラス用設定情報</value>
        Public ReadOnly Property CachingManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CachingManager")
            End Get
        End Property

        ''' <summary>
        ''' バリデーションクラス用設定情報を取得します。
        ''' </summary>
        ''' <value>バリデーションクラス用設定情報</value>
        Public ReadOnly Property Validation() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("Validation")
            End Get
        End Property

        ''' <summary>
        ''' セッションマネージャー設定情報を取得します。
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property BatchCommonSetting() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("BatchCommonSetting")
            End Get
        End Property

        ''' <summary>
        ''' コードテーブルサービスクラス用設定情報を取得します。
        ''' </summary>
        ''' <value>コードテーブルサービスクラス用設定情報</value>
        Public ReadOnly Property CodeTableSerivce() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CodeTableSerivce")
            End Get
        End Property

        ''' <summary>
        ''' ペースページ用の設定情報を取得します。
        ''' </summary>
        ''' <value>ベースページ用設定情報</value>
        Public ReadOnly Property BasePage() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("BasePage")
            End Get
        End Property

        ''' <summary>
        ''' i-CROPのVersion情報を取得します。
        ''' </summary>
        ''' <value>i-CROPのVersion情報</value>
        Public ReadOnly Property VersionInformation() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("VersionInformation")
            End Get
        End Property

        ''' <summary>
        ''' マスターページの情報を取得します。
        ''' </summary>
        ''' <value>マスターページの情報</value>
        Public ReadOnly Property CommonMasterPage() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CommonMasterPage")
            End Get
        End Property

        ''' <summary>
        ''' アップロードサイズ上限の情報を取得します。
        ''' </summary>
        ''' <value>アップロードサイズ上限の情報</value>
        Public ReadOnly Property CustomFileUpload() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CustomFileUpload")
            End Get
        End Property

        ''' <summary>
        ''' CSVファイル上限の情報を取得します。
        ''' </summary>
        ''' <value>アップロードサイズ上限の情報</value>
        Public ReadOnly Property CsvManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CsvManager")
            End Get
        End Property

        ''' <summary>
        ''' メインメニューURLの情報を取得します。
        ''' </summary>
        ''' <value>メインメニューURL</value>
        Public ReadOnly Property MainMenu() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("MainMenu")
            End Get
        End Property

        ''' <summary>
        ''' 印刷の情報を取得します。
        ''' </summary>
        ''' <value></value>
        Public ReadOnly Property ConcurrentProcMgr() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("ConcurrentProcMgr")
            End Get
        End Property

        ''' <summary>
        ''' 業務起動画面の情報を取得します。
        ''' </summary>
        ''' <value></value>
        Public ReadOnly Property S90B0003() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("S90B0003")
            End Get
        End Property

        ''' <summary>
        ''' アプリ基盤固有のシステム設定情報を取得します。
        ''' </summary>
        ''' <value></value>
        Public ReadOnly Property System() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("System")
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value>環境設定取得クラス用設定情報</value>
        Public ReadOnly Property DocumentDomain() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("DocumentDomain")
            End Get
        End Property

        ''' <summary>
        ''' Class要素の読み込みを行います。
        ''' </summary>
        ''' <param name="className">取得するClass要素のName属性</param>
        ''' <returns>Class要素のを表現するClassSectionクラスのインスタンス</returns>
        Public Function GetClassSection(ByVal className As String) As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection

            ''---------- Classセクションの宣言 ----------------------- 
            Dim returnValue As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = Nothing

            ''----- クラスセクションの取得
            'Dim section As System.Xml.XmlElement = DirectCast(System.Web.Configuration.WebConfigurationManager.GetSection(APP_NAMESPACE), System.Xml.XmlElement)

            For Each item As System.Xml.XmlNode In Me._config

                Dim element As System.Xml.XmlElement = TryCast(item, System.Xml.XmlElement)

                If element IsNot Nothing Then
                    If (element.Attributes("Name").Value.Equals(className)) Then
                        returnValue = New Toyota.eCRB.SystemFrameworks.Configuration.ClassSection(item)
                        Exit For
                    End If
                End If
            Next

            '---------- 返り値の設定 --------------------------------
            Return returnValue

        End Function

    End Class


    ''' <summary>
    ''' e-CRB Framework用のカスタム構成セクションを処理するためのクラスです。
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' このクラスは、e-CRB Framework独自の構成要素を処理するためのクラスです。
    ''' 構成ファイルに次の形式で構成セクションを登録することにより、e-CRB Frameworkの構成を行うことが可能となります。
    ''' </para>
    ''' <example>
    ''' <code>
    '''   &lt;configSections&gt;
    '''     &lt;section name="Toyota.eCRB.SystemFrameworks" type="Toyota.eCRB.SystemFrameworks.Configuration.ConfigurationHandler, Toyota.eCRB.SystemFrameworks.Configuration, version=1.1.0.0, Culture=neutral, PublicKeyToken=18229613dc9cad02"/&gt;
    '''   &lt;/configSections&gt;
    ''' </code>
    ''' </example>
    ''' </remarks>
    Friend Class ConfigurationHandler

        Implements System.Configuration.IConfigurationSectionHandler

        ''' <summary>
        ''' 構成セクション ハンドラを作成します。
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <param name="configContext">構成コンテキスト オブジェクト</param>
        ''' <param name="section"></param>
        ''' <returns>作成されたセクション ハンドラ オブジェクト。</returns>
        ''' <remarks>parent および section については、MSDNなどにも明確な記載が無いため説明を割愛しています。</remarks>
        Friend Function Create(ByVal parent As Object, ByVal configContext As Object, ByVal section As System.Xml.XmlNode) As Object Implements System.Configuration.IConfigurationSectionHandler.Create

            Return section

        End Function
    End Class

End Namespace
