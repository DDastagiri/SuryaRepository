'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Web

#Region "Enum"
    Public Enum StaffContextPermission
        HeadOffice = 0
        Branch = 1
        Distributor = 2
    End Enum
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
    Public Enum StaffContextUpdateResult
        ''' <summary>
        ''' 更新ロック取得失敗
        ''' </summary>
        ''' <remarks></remarks>
        GetLockError = 3
    End Enum
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
#End Region

    ''' <summary>
    ''' ログインスタッフに関する情報を管理するクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public NotInheritable Class StaffContext

#Region "ローカル変数"
        ''' <summary>
        ''' セッションのキー
        ''' </summary>
        Friend Const SESSION_KEY As String = "Toyota.eCRB.SystemFrameworks.AppService.StaffContext"

        Private _account As String
        Private _userName As String
        Private _dlrCd As String
        Private _dlrName As String
        Private _brnCd As String
        Private _brnName As String
        Private _opeCd As Operation
        Private _opeName As String
        Private _userPermission As StaffContextPermission
        '2013/11/26 TCS 河原 Aカード情報相互連携開発 START
        Private _teamCd As Decimal
        '2013/11/26 TCS 河原 Aカード情報相互連携開発 END
        Private _teamName As String
        Private _teamLeader As Boolean
        Private _timeDiff As Decimal
        Private _presenceCategory As String
        Private _presenceDetail As String
        Private _presenceUpdateDate As DateTime

#End Region

#Region "Property"
        ''' <summary>
        ''' ログインユーザーのアカウントを取得します。
        ''' </summary>
        Public ReadOnly Property Account As String
            Get
                Return _account
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザのユーザー名を取得します。
        ''' </summary>
        Public ReadOnly Property UserName() As String
            Get
                Return _userName
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの販売店コードを取得します。
        ''' </summary>
        Public ReadOnly Property DlrCD() As String
            Get
                Return _dlrCd
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの販売店名を取得します。
        ''' </summary>
        Public ReadOnly Property DlrName() As String
            Get
                Return _dlrName
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの店舗コードを取得します。
        ''' </summary>
        Public ReadOnly Property BrnCD() As String
            Get
                Return _brnCd
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの店舗名を取得します。
        ''' </summary>
        Public ReadOnly Property BrnName() As String
            Get
                Return _brnName
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの権限コードを取得します。
        ''' </summary>
        Public ReadOnly Property OpeCD() As Operation
            Get
                Return _opeCd
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの権限名を取得します。
        ''' </summary>
        Public ReadOnly Property OpeName() As String
            Get
                Return _opeName
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの権限(事業体、販売店、店舗)を取得します。
        ''' </summary>
        Public ReadOnly Property UserPermission() As StaffContextPermission
            Get
                Return _userPermission
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザのチームコードを取得します。
        ''' </summary>
        Public ReadOnly Property TeamCD() As Decimal
            Get
                Return _teamCd
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザのチーム名を取得します。
        ''' </summary>
        Public ReadOnly Property TeamName() As String
            Get
                Return _teamName
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザがチームリーダーか取得します。
        ''' </summary>
        Public ReadOnly Property TeamLeader() As Boolean
            Get
                Return _teamLeader
            End Get
        End Property

        ''' <summary>
        ''' ログインユーザの時差を取得します。
        ''' </summary>
        Public ReadOnly Property TimeDiff() As Decimal
            Get
                Return _timeDiff
            End Get
        End Property

        ''' <summary>
        ''' StaffContextオブジェクトが生成されているかを取得します。
        ''' </summary>
        ''' <returns>
        ''' True:生成されています。<br/>
        ''' False:生成されていません。
        ''' </returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property IsCreated As Boolean
            Get
                If (HttpContext.Current.Session Is Nothing) OrElse (HttpContext.Current.Session(SESSION_KEY) Is Nothing) Then
                    '生成されていない
                    Return False
                Else
                    '生成されている
                    Return True
                End If
            End Get
        End Property

        ''' <summary>
        ''' ステイタス（大分類）を取得します。
        ''' </summary>
        ''' <remarks>設定値の仕様は、tbl_USERS.PRESENCECATEGORYに準じます</remarks>
        Public ReadOnly Property PresenceCategory As String
            Get
                Return _presenceCategory
            End Get
        End Property

        ''' <summary>
        ''' ステイタス（小分類）を取得します。
        ''' </summary>
        ''' <remarks>設定値の仕様は、tbl_USERS. PRESENCEDETAILに準じます</remarks>
        Public ReadOnly Property PresenceDetail As String
            Get
                Return _presenceDetail
            End Get
        End Property

#End Region

#Region "Constructor"
        ''' <summary>
        ''' コンストラクタです。インタンスを生成させないようにするため、修飾子はPrivateです。
        ''' </summary>
        Private Sub New()
        End Sub
#End Region

#Region "Public:Create"
        ''' <summary>
        ''' StaffContextオブジェクトを生成します。オブジェクトはセッションに格納します
        ''' </summary>
        ''' <remarks>
        ''' ログインID、ブランド販売店コード、社員コード、社員名、会社コード、販売店コード、
        ''' 販売店名、所属店舗コード、所属店舗名、集約先店舗コード、集約先店舗名、
        ''' 店舗コード一覧、所属グループコード、レクサス販売店コード、所有権限一覧、運用パターン、
        ''' 本部パターンおよび所属店舗ブランドコードに値を格納します。
        ''' </remarks>
        ''' <param name="account">アカウント</param>
        ''' <returns>生成できた場合はTrue、できなかった場合はFalse</returns> 
        Friend Shared Function Create(ByVal account As String) As Boolean
            If (HttpContext.Current.Session Is Nothing) Then
                Return False
            End If

            Dim dtUser As StaffContextDataSet.USERINFODataTable = StaffContextTableAdapter.GetUserInfo(account)

            ''顧客情報を取得失敗の場合エラー
            If dtUser.Count < 1 Then
                Return False
            End If

            Dim drUser As StaffContextDataSet.USERINFORow = CType(dtUser.Rows(0), StaffContextDataSet.USERINFORow)

            Dim instance As New StaffContext

            instance._account = Trim(drUser.ACCOUNT)
            instance._userName = Trim(drUser.USERNAME)
            instance._dlrCd = Trim(drUser.DLRCD)
            instance._dlrName = Trim(drUser.DLRNM_LOCAL)
            instance._brnCd = Trim(drUser.STRCD)
            instance._brnName = Trim(drUser.STRNM_LOCAL)
            instance._opeCd = CType(drUser.OPERATIONCODE, Operation)
            instance._opeName = Trim(drUser.OPERATIONNAME)

            ''販売店コードが'00000'の時Distributor権限
            If instance._dlrCd = ConstantDealerCD.DistDealerCD Then
                instance._userPermission = StaffContextPermission.Distributor
            Else
                ''店舗コードが'000'の時H/O権限
                If instance._brnCd = ConstantBranchCD.BranchHO Then
                    instance._userPermission = StaffContextPermission.HeadOffice
                Else
                    ''その他は店舗権限
                    instance._userPermission = StaffContextPermission.Branch
                End If
            End If

            instance._teamCd = drUser.TEAMCD
            instance._teamName = Trim(drUser.TEAMNAME)

            If drUser.LEADERFLG.Equals("1") Then
                instance._teamLeader = True
            Else
                instance._teamLeader = False
            End If

            ''時差取得
            Dim timeDiff As Decimal = _GetTimeDiff(instance._dlrCd, instance._brnCd)
            If IsNothing(timeDiff) Then
                Return False
            End If

            instance._timeDiff = timeDiff
            
            If (Toyota.eCRB.SystemFrameworks.Core.VersionInformation.IsEqualOrLaterThan(1, 2, 0)) Then
                ''在席状態取得
                If (drUser.IsPRESENCECATEGORYNull() OrElse drUser.IsPRESENCEDETAILNull()) Then
                    'オフライン
                    instance._presenceCategory = "4"
                    instance._presenceDetail = "0"
                Else
                    instance._presenceCategory = drUser.PRESENCECATEGORY
                    instance._presenceDetail = drUser.PRESENCEDETAIL
                End If

            End If

            HttpContext.Current.Session(SESSION_KEY) = instance

            dtUser.Dispose()
            dtUser = Nothing
            drUser = Nothing

            Return True

        End Function
#End Region

#Region "Public:Current"
        ''' <summary>
        ''' ログイン情報を保持するセッション毎にユニークなインスタンスです。
        ''' セッションからユーザ情報管理機能オブジェクトを取得します。
        ''' </summary>
        ''' <remarks>例外　InvalidOperationException：セッションからユーザ情報管理機能オブジェクトが
        ''' 取得できないと発生します。</remarks>
        ''' <returns>ユーザ情報管理機能オブジェクト</returns>
        Public Shared Function Current() As StaffContext
            'セッション
            Dim instance As StaffContext = DirectCast(HttpContext.Current.Session(SESSION_KEY), StaffContext)

            '2.ローカル変数instanceの値を評価します。
            '2.1.Nothingの場合はInvalidOperationExceptionを発生します。
            '2.2.上記以外の場合はローカル変数instanceを戻します。
            If instance Is Nothing Then
                Throw New InvalidOperationException()
            Else
                Return instance
            End If

        End Function
#End Region

#Region "Public:Clear"
        ''' <summary>
        ''' スタッフコンテキストを削除する。
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub Clear()

            HttpContext.Current.Session(SESSION_KEY) = Nothing

        End Sub
#End Region

#Region "Public:UpdatePresence"
        ''' <summary>
        ''' ユーザーのステイタスを更新します。
        ''' </summary>
        ''' <param name="presenceCategory">在席状態（大分類）</param>
        ''' <param name="presenceDetail">在席状態（小分類）</param>
        ''' <returns>データ更新の影響を受けた行数</returns>
        Public Function UpdatePresence(ByVal presenceCategory As String, ByVal presenceDetail As String) As Integer

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================

            'ユーザマスタロック取得
            StaffContextTableAdapter.GetUsersLock(Account)
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            Dim dtUser As StaffContextDataSet.USERINFODataTable = StaffContextTableAdapter.GetUserInfo(Account)

            Dim catDateUpFlg As Boolean = False
            If dtUser.Count >= 1 Then
                If dtUser.Item(0).PRESENCECATEGORY <> presenceCategory Then
                    catDateUpFlg = True
                End If
            End If

            Dim affected As Integer = StaffContextTableAdapter.UpdatePresence(_account, presenceCategory, presenceDetail, catDateUpFlg)
            If (affected = 1) Then
                _presenceCategory = presenceCategory
                _presenceDetail = presenceDetail
                _presenceUpdateDate = DateTimeFunc.Now()
            End If

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, affected))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
            Return affected
        End Function
#End Region

#Region "Private:時差取得"
        ''' <summary>
        ''' 時差を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <returns>時差</returns>
        ''' <remarks>サーバーと販売店の時差を取得し返却します。</remarks>
        Private Shared Function _GetTimeDiff(ByVal dlrCd As String, ByVal strCd As String) As Decimal


            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            Dim diffTime As Decimal = Decimal.Zero
            Dim dtDiff As StaffContextDataSet.TIMEDIFFDataTable = StaffContextTableAdapter.GetTimeDiffData(dlrCd, strCd)

            If dtDiff.Count < 1 Then
                ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, Decimal.Zero))
                ' ======================== ログ出力 終了 ========================
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return Decimal.Zero
            End If

            Dim drDiff As StaffContextDataSet.TIMEDIFFRow = CType(dtDiff.Rows(0), StaffContextDataSet.TIMEDIFFRow)
            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START

            If Not drDiff.TIMEDIFF.Equals(" ") Then

                Dim tmpTime As String = drDiff.TIMEDIFF

                Dim sign As String = tmpTime.Substring(0, 1)
                Dim tmpHour As String = tmpTime.Substring(1, 2).TrimStart("0"c)
                Dim tmpMinuteDec As Decimal = CDec(tmpTime.Substring(4, 2)) / 60
                Dim tmpMinuteStr As String = CStr(tmpMinuteDec).TrimStart("0"c)

                If tmpHour.Equals(String.Empty) Then
                    tmpHour = "0"
                End If

                diffTime = CDec(sign & tmpHour & tmpMinuteStr)

            End If
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            dtDiff.Dispose()
            dtDiff = Nothing
            drDiff = Nothing

            '2013/06/30 TCS 山田 2013/10対応版　既存流用 START DEL
            '2013/06/30 TCS 山田 2013/10対応版　既存流用 DEL

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, diffTime))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            Return diffTime

        End Function
#End Region

        '2013/11/26 TCS 河原 Aカード情報相互連携開発 START
#Region "Public:上長アカウントコレクション取得"
        ''' <summary>
        ''' 上長アカウントコレクション取得
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="strCD">店舗コード</param>
        ''' <param name="operationcode">権限コード</param>
        ''' <param name="TLFlg">チームリーダーフラグ</param>
        ''' <returns>上長のアカウントリスト</returns>
        ''' <remarks>上長アカウントコレクションを取得する</remarks>
        Public Shared Function GetMySuperiors(ByVal dlrCD As String, ByVal strCD As String, operationcode As Long, ByVal TLFlg As Boolean, ByVal account As String) As List(Of String)

            Dim AccountList As New List(Of String)

            '権限がセールスマネージャの場合、空のリストを返却して終了
            If operationcode = 7 Then
                Return AccountList
            End If

            '店舗の全マネージャ・チームリーダー取得
            Dim BranchSuperiorsDt As StaffContextDataSet.BRANCHSUPERIORSDataTable = StaffContextTableAdapter.GetBranchSuperiors(dlrCD, strCD, account)

            If TLFlg Then
                '自身がチームリーダーか、組織に所属していない場合
                For Each dr As StaffContextDataSet.BRANCHSUPERIORSRow In BranchSuperiorsDt.Rows
                    'セールススタッフマネージャ権限のアカウントコードのみを取得
                    If dr.OPERATIONCODE = 7 Then
                        AccountList.Add(dr.STF_CD)
                    End If
                Next
            Else
                '自身がチームリーダーではなく、かつ組織に所属している場合
                For Each dr As StaffContextDataSet.BRANCHSUPERIORSRow In BranchSuperiorsDt.Rows
                    '店舗の全リーダーとセールススタッフマネージャ権限のアカウントコードを取得
                    AccountList.Add(dr.STF_CD)
                Next
            End If

            Return AccountList

        End Function
#End Region
        '2013/11/26 TCS 河原 Aカード情報相互連携開発 END

    End Class

End Namespace