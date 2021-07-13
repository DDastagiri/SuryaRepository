'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Web
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.AuthenticationManagerDataSet
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Web

#Region "Enum"

    ''' <summary>
    ''' ログインの結果です。
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LoginResult
        ''' <summary>
        ''' 正常
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0
        ''' <summary>
        ''' 端末認証によるエラー
        ''' </summary>
        ''' <remarks></remarks>
        MachineCertificationError = 1
        ''' <summary>
        ''' 入力アカウント形式エラー
        ''' </summary>
        ''' <remarks></remarks>
        AccountFormatError = 2
        ''' <summary>
        ''' 販売店コード存在エラー
        ''' </summary>
        ''' <remarks></remarks>
        NotExistDLRCDError = 3
        ''' <summary>
        ''' GHDユーザMacAddress登録完了
        ''' </summary>
        ''' <remarks></remarks>
        GHDEditComplete = 4
        ''' <summary>
        ''' GHDユーザMacAddress登録済みエラー
        ''' </summary>
        ''' <remarks></remarks>
        GHDExistError = 5
        ''' <summary>
        ''' MacAddressと販売店コードの不整合エラー
        ''' </summary>
        ''' <remarks></remarks>
        MacAddressError = 6
        ''' <summary>
        ''' 一致するユーザーが存在しない
        ''' </summary>
        ''' <remarks></remarks>
        LoginError = 7
        ''' <summary>
        ''' 利用時間制限によるエラー
        ''' </summary>
        ''' <remarks></remarks>
        LoginTimeError = 8
        ''' <summary>
        ''' セッション作成エラー
        ''' </summary>
        ''' <remarks></remarks>
        CreateSessionError = 9
        ''' <summary>
        ''' 同一アカウントエラー
        ''' </summary>
        ''' <remarks></remarks>
        DuplicateError = 10
        ''' <summary>
        ''' 更新ロック取得失敗
        ''' </summary>
        ''' <remarks></remarks>
        GetLockError = 11
    End Enum

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class AuthenticationManager
        Inherits BaseBusinessComponent

#Region "Constant"
        Private Const C_USED_FLG_CLIP As String = "USED_FLG_CLIP"
        Private Const C_ADJUST_TIME As UInteger = 2400
        Private Const C_MACHINE_CERTIFICATION_ENABLE As String = "1"
        Private Const C_MACHINE_CERTIFICATION_ENABLE_TMCI As String = "2"
        Private Const C_MACHINE_CERTIFICATION_TARGET As String = "0"
        Private Const C_MACHINE_CERTIFICATION_NOT_TARGET As String = "1"
#End Region

#Region "Public:認証処理"
        ''' <summary>
        ''' 引数で指定されたアカウント、パスワードを元に認証処理を行い、その認証結果を取得します。
        ''' </summary>
        ''' <param name="account">アカウント</param>
        ''' <param name="password">パスワード</param>
        ''' <param name="deviceId">デバイス識別文字列（iPadの場合はMACアドレス）</param>
        ''' <returns>
        ''' 認証が成功した場合、Trueを返します。<br/>
        ''' 認証に失敗した場合は、Falseを返します。
        ''' 販売店コードを補完したアカウントで返却します（PushServer登録用）
        ''' </returns>
        ''' <remarks></remarks>
        <EnableCommit()> _
        Public Function Auth(ByRef account As String, ByVal password As String, ByVal deviceId As String) As LoginResult

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, account:[{1}], password:[{2}], deviceId:[{3}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      account,
                                      password,
                                      deviceId))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Dim config As ConfigurationManager = SystemConfiguration.Current.Manager

            '引数チェック
            If String.IsNullOrEmpty(account) OrElse String.IsNullOrEmpty(password) Then
                Return LoginResult.LoginError
            End If

            'アカウントをパース
            Dim accountTokens As String() = account.Split(New Char() {"@"c}, 2)
            Dim accountName As String = accountTokens(0)
            Dim accountDomain As String = Nothing
            If (accountTokens.Length = 2) Then
                accountDomain = accountTokens(1)
                If String.IsNullOrEmpty(accountDomain) Then
                    accountDomain = Nothing
                End If
            End If

            'User-Agentチェック
            Dim userAgentType As String = "UNKNOWN"
            Dim passUserAgentCheck As Boolean = False
            For Each userAgent As Item In config.LoginManager.GetSetting("UserAgent").Item
                Dim userAgentRegEx As New Regex(userAgent.Value)
                If (userAgentRegEx.IsMatch(HttpContext.Current.Request.UserAgent)) Then
                    userAgentType = userAgent.Name
                    passUserAgentCheck = True
                    Exit For
                End If
            Next
            If (Not passUserAgentCheck) Then
                Return LoginResult.MachineCertificationError
            End If

            '端末認証
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START
            If (userAgentType.Equals("iPad") OrElse userAgentType.Equals("iPod") OrElse userAgentType.Equals("iPhone")) Then
                'If (userAgentType.Equals("iPad")) Then
                '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                '業務端末マスタロック取得
                Try
                    AuthenticationManagerTableAdapter.GetClientLock(deviceId, account)
                Catch ex As OracleExceptionEx
                    Return LoginResult.GetLockError
                End Try
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

                'デバイス識別文字列（iPadの場合はMACアドレス）存在確認
                If String.IsNullOrEmpty(deviceId) Then
                    Return LoginResult.MacAddressError
                End If

                'GHDユーザー判定コード取得
                Dim ghdAbbreviation As String = CStr(config.EnvironmentSetting.GetSetting(String.Empty).GetValue("GhdAbbreviation"))

                '[TB_M_CLIENT]データ取得
                Dim result As TBL_CLIENT_MACADDRESSDataTable = AuthenticationManagerTableAdapter.GetClientMacAddressData(deviceId)

                'MACアドレスチェック
                If (result.Count = 0) Then
                    '----------------------------------------------------MACアドレス未登録
                    'GHDユーザ判定
                    Dim isGhdUser As Boolean = (ghdAbbreviation.Equals(accountDomain))

                    '初回アクセス時で「ユーザアカウント@販売店コード」の形式でない場合にエラー
                    If (accountDomain = Nothing) Then
                        Return LoginResult.AccountFormatError
                    End If

                    '販売店存在確認
                    If (Not isGhdUser) Then
                        Dim dlrDt As DealerDataSet.DEALERDataTable = DealerTableAdapter.GetDealerDataTable(accountDomain, CStr(0))
                        If (dlrDt.Count = 0) Then
                            Return LoginResult.NotExistDLRCDError
                        End If
                    End If

                    '[TB_M_CLIENT]登録処理
                    AuthenticationManagerTableAdapter.InsertClientMacAddress(account, deviceId, accountDomain)

                    'GHDユーザの場合登録完了で処理終了のためメッセージを返す
                    If (isGhdUser) Then
                        Return LoginResult.GHDEditComplete
                    End If
                Else
                    '----------------------------------------------------MACアドレス登録済
                    '[TB_M_CLIENT]の販売店コード取得
                    Dim getDomain = Trim(result(0).DLRCD)

                    'GHDユーザ判定
                    Dim isGhdUser As Boolean = (ghdAbbreviation.Equals(getDomain))

                    If (isGhdUser) Then
                        'GHDユーザとして登録済みの場合にメッセージを返す
                        If (ghdAbbreviation.Equals(accountDomain)) Then
                            Return LoginResult.GHDExistError
                        End If
                    Else
                        '販売店コードを補完する
                        If (accountDomain = Nothing) Then
                            accountDomain = getDomain
                            account = accountName & "@" & accountDomain
                        End If

                        '[TB_M_CLIENT]の販売店コードと不一致の場合にエラーとする
                        If (Not accountDomain.Equals(getDomain)) Then
                            Return LoginResult.MacAddressError
                        End If
                    End If

                End If
            ElseIf (userAgentType.Equals("PC")) Then
                'IPアドレスチェック
                If Not AuthenticationManager._IsEnableMachineCertification(account) Then
                    Return LoginResult.MachineCertificationError
                End If
            End If

            'ユーザー認証
            If Not AuthenticationManager._IsLogin(account, password) Then
                Return LoginResult.LoginError
            End If

            '利用時間帯チェック
            If Not _IsEnableLoginTime(account) Then
                Return LoginResult.LoginTimeError
            End If

            'ログイン重複チェック
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START
            If (userAgentType.Equals("iPad") OrElse userAgentType.Equals("iPod") OrElse userAgentType.Equals("iPhone")) Then
                'If (userAgentType.Equals("iPad")) Then
                '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

                If Not _IsDuplicateLogin(account, deviceId) Then
                    Return LoginResult.DuplicateError
                End If
            Else
                If Not _IsDuplicateLogin(account) Then
                    Return LoginResult.DuplicateError
                End If
            End If

            'StaffContextを生成()
            If Not StaffContext.Create(account) Then
                Return LoginResult.CreateSessionError
            End If

            'ステイタス更新
            If (VersionInformation.IsEqualOrLaterThan(1, 2, 0)) Then
                StaffContext.Current.UpdatePresence("1", "0")
            End If

            '使用中ユーザアカウント更新
            If (userAgentType.Equals("iPad") OrElse userAgentType.Equals("iPod") OrElse userAgentType.Equals("iPhone")) Then
                '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START
                'If (userAgentType.Equals("iPad")) Then
                '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

                AuthenticationManagerTableAdapter.ClearClientMacAddress(account)
                AuthenticationManagerTableAdapter.UpdateClientMacAddress(account, deviceId)
            End If

            'ILoginHook起動
            For Each hook As Object In SystemConfiguration.Current.Hooks
                If (TypeOf hook Is ILoginHook) Then
                    CType(hook, ILoginHook).HookAfterLogin()
                End If
            Next

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, LoginResult.Success))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Return LoginResult.Success

        End Function
#End Region

#Region "Private:端末認証"
        ''' <summary>
        ''' 端末認証
        ''' </summary>
        ''' <param name="account">ログインアカウント</param>
        ''' <returns>True:利用可 False:利用不可</returns>
        ''' <remarks>
        ''' 引数がない、データが取得できない場合も利用不可
        ''' </remarks>
        Private Shared Function _IsEnableMachineCertification(ByVal account As String) As Boolean

            ''端末認証を行うか確認し、戻り値によってIPの取得方法を変更
            Dim sys As New SystemEnvSetting
            Dim sysDr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(C_USED_FLG_CLIP)

            ''SystemEnvSettingに登録されていない場合不可
            If (sysDr Is Nothing) Then
                Return True
            End If

            Dim flgClip As String = sysDr.PARAMVALUE

            Dim clip As String
            If flgClip.Equals(C_MACHINE_CERTIFICATION_ENABLE) Then
                clip = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            ElseIf flgClip.Equals(C_MACHINE_CERTIFICATION_ENABLE_TMCI) Then
                clip = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            Else
                ''取得できない場合は無条件で利用可能
                Return True
            End If

            ''Cクラスまで取得
            Dim clip_C As String = Mid(clip, 1, InStrRev(clip, ".") - 1)

            Dim dt As AuthenticationManagerDataSet.TBL_CLIENT_IPDataTable = AuthenticationManagerTableAdapter.GetClientIPData(clip, clip_C)

            ''レコードの存在確認
            If dt.Count < 1 Then
                Return False
            End If

            ''Cクラスが存在する時はCクラス優先(データ取得時に1行目が優先となる)
            Dim dr As AuthenticationManagerDataSet.TBL_CLIENT_IPRow = CType(dt.Rows(0), AuthenticationManagerDataSet.TBL_CLIENT_IPRow)

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            If dr.DLRCD.Equals("XXXXX") Then
                ''販売店コードが指定されていない場合は利用可
            Else
                ''販売店コードがアカウントの販売店コードと一致する場合利用可
                Dim dlrcd As String = account.Split("@"c)(1)
                If Not dlrcd = RTrim(dr.DLRCD) Then
                    Return False
                End If
            End If
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            dt.Dispose()
            dt = Nothing
            dr = Nothing

            Return True

        End Function
#End Region

#Region "Private:ユーザー確認"
        ''' <summary>
        ''' ユーザー確認
        ''' </summary>
        ''' <param name="account">アカウント</param>
        ''' <param name="password">パスワード</param>
        ''' <returns>
        ''' True:ログイン可<br/>
        ''' False:ログイン不可
        ''' </returns>
        ''' <remarks></remarks>
        Private Shared Function _IsLogin(ByVal account As String, ByVal password As String) As Boolean


            If AuthenticationManagerTableAdapter.IsUser(account, password) < 1 Then
                Return False
            End If

            Return True

        End Function
#End Region

#Region "Private:利用時間制限"
        ''' <summary>
        ''' 利用時間制限
        ''' </summary>
        ''' <param name="account">アカウント</param>
        ''' <returns>
        ''' True:ログイン可<br/>
        ''' False:ログイン不可
        ''' </returns>
        ''' <remarks>利用時間制限の確認をします。</remarks>
        Private Shared Function _IsEnableLoginTime(ByVal account As String) As Boolean

            Dim dt As AuthenticationManagerDataSet.LOGINTIMEDataTable = AuthenticationManagerTableAdapter.GetLoginTimeData(account, ConstantBranchCD.BranchHO)

            ''レコードが取得できなかった場合は制限無し
            If dt.Count < 1 Then
                Return True
            End If

            Dim dr As AuthenticationManagerDataSet.LOGINTIMERow = CType(dt.Rows(0), AuthenticationManagerDataSet.LOGINTIMERow)

            ''利用時間制限開始時刻が数値ではない場合ログイン可
            Dim tmpStart As String = Trim(dr.LOGIN_STARTTIME)
            If String.IsNullOrEmpty(tmpStart) OrElse Not IsNumeric(tmpStart) Then
                Return True
            End If

            ''利用時間制限開始時刻を取得
            Dim startTime As UInteger = CUInt(tmpStart)
            If C_ADJUST_TIME < startTime Then
                startTime = startTime - C_ADJUST_TIME
            End If

            ''利用時間制限終了時刻が数値ではない場合ログイン可
            Dim tmpEnd As String = Trim(dr.LOGIN_ENDTIME)
            If String.IsNullOrEmpty(tmpEnd) OrElse Not IsNumeric(dr.LOGIN_ENDTIME) Then
                Return True
            End If

            ''利用時間制限終了時刻を取得
            Dim endTime As UInteger = CUInt(tmpEnd)
            If C_ADJUST_TIME < endTime Then
                endTime = endTime - C_ADJUST_TIME
            End If

            Dim sysTime As Date = Now()
            Dim nowTime As UInteger = CUInt(sysTime.ToString("HHmm", CultureInfo.InvariantCulture))

            '利用終了時刻 <= 利用開始時刻且つ、現在時刻 <= 利用開始時刻の場合は現在時刻にC_ADJUST_TIMEを加算する
            If endTime <= startTime And nowTime < startTime Then
                nowTime = nowTime + C_ADJUST_TIME
            End If

            '利用終了時刻 <= 利用開始時刻の場合は利用終了時刻にC_ADJUST_TIMEを加算する
            If endTime <= startTime Then
                endTime = endTime + C_ADJUST_TIME
            End If

            '利用可能判定を行う
            If startTime <= nowTime And nowTime <= endTime Then
                Return True
            Else
                Return False
            End If

            dt.Dispose()
            dt = Nothing
            dr = Nothing

        End Function
#End Region

#Region "Private:ログイン状況確認"
        ''' <summary>
        ''' 指定アカウントのログイン状況確認
        ''' </summary>
        ''' <param name="account">アカウント</param>
        ''' <returns>ログイン可能:True ログイン不可：False</returns>
        ''' <remarks></remarks>
        Private Shared Function _IsDuplicateLogin(ByVal account As String, ByVal maccaddress As String) As Boolean
            Dim dt As AuthenticationManagerDataSet.STATUS_MACADDRESSDataTable = AuthenticationManagerTableAdapter.SelectStatusMacaddress(account)

            'レコードが取得できない場合はログインユーザがいないので許可
            If dt.Count = 0 Then
                Return True
            End If

            'レコードが複数取得できた場合はエラー
            If dt.Count > 1 Then
                Return False
            End If

            Dim dr As AuthenticationManagerDataSet.STATUS_MACADDRESSRow = CType(dt.Rows(0), AuthenticationManagerDataSet.STATUS_MACADDRESSRow)

            '-----------------------------------------ステータス確認
            If (dr.PRESENCECATEGORY = 0 And dr.PRESENCEDETAIL = 0) Then
                '未ログインユーザなので許可
                Return True
            End If

            If dr.PRESENCECATEGORY = 4 And dr.PRESENCEDETAIL = 0 Then
                'オフラインなので許可
                Return True
            Else
                '同一Macaddressであれば再ログインを許可
                If dr.MACADDRESS = maccaddress Then
                    Return True
                End If

                '既に別iPadにてログイン中のため不許可とする
                Return False
            End If

        End Function

        ''' <summary>
        ''' 指定アカウントのログイン状況確認
        ''' </summary>
        ''' <param name="account"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function _IsDuplicateLogin(ByVal account As String) As Boolean
            If (AuthenticationManagerTableAdapter.CheckStatus(account) = 1) Then
                '未使用アカウント：許可
                Return True
            Else
                '使用中アカウント：不許可
                Return False
            End If
        End Function

#End Region

    End Class

End Namespace