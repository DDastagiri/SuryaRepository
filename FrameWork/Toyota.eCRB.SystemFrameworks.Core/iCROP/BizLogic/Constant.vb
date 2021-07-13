'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

#Region "ConstDlrCd"
    ''' <summary>
    ''' 販売店コードに関する定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ConstantDealerCD

#Region "Constant"

        ''' <summary>
        ''' 共通販売店コードです。
        ''' </summary>
        Public Const AllDealerCD As String = "XXXXX"

        ''' <summary>
        ''' ディストリビュータの販売店コードです。
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DistDealerCD As String = "00000"
#End Region

#Region "Constructor"
        Private Sub New()

        End Sub
#End Region

    End Class

#End Region

#Region "ConstStrCd"
    ''' <summary>
    ''' 店舗コードに関する定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ConstantBranchCD

#Region "Constant"

        ''' <summary>
        ''' H/Oの店舗コードです。
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BranchHO As String = "000"

        ''' <summary>
        ''' 共通店舗コードです。
        ''' </summary>
        ''' <remarks></remarks>
        Public Const AllBranchCD As String = "XXX"

#End Region

#Region "Constructor"
        Private Sub New()

        End Sub
#End Region

    End Class

#End Region

#Region "PresenceCategory"
    ''' <summary>
    ''' 在席状態（大分類）に関する定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class PresenceCategory

#Region "Constant"

        ''' <summary>
        ''' スタンバイです。
        ''' </summary>
        Public Const Standby As String = "1"

        ''' <summary>
        ''' 商談中です。
        ''' </summary>
        Public Const OnMeeting As String = "2"

        ''' <summary>
        ''' 退席中です。
        ''' </summary>
        Public Const Suspend As String = "3"

        ''' <summary>
        ''' オフラインです。
        ''' </summary>
        Public Const Offline As String = "4"

        ''' <summary>
        ''' どの状態にも該当しません。
        ''' </summary>
        Public Const None As String = ""
#End Region

#Region "Constructor"
        Private Sub New()

        End Sub
#End Region

    End Class

#End Region

#Region "Constant"

#Region "Operation"
    ''' <summary>
    ''' 権限一覧です。
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Operation
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        None = 0
        ''' <summary>
        ''' Call Centre Manager
        ''' </summary>
        ''' <remarks></remarks>
        CCM = 1
        ''' <summary>
        ''' Call Centre Operator
        ''' </summary>
        ''' <remarks></remarks>
        CCO = 2
        ''' <summary>
        ''' H/O Assistant
        ''' </summary>
        ''' <remarks>[Obsoleted] 代わりにAHOを使用してください</remarks>
        ASH = 3
        ''' <summary>
        ''' H/O Assistant
        ''' </summary>
        ''' <remarks></remarks>
        AHO = 3
        ''' <summary>
        ''' Branch Assistant
        ''' </summary>
        ''' <remarks>[Obsoleted] 代わりにABRを使用してください</remarks>
        ASB = 4
        ''' <summary>
        ''' Branch Assistant
        ''' </summary>
        ''' <remarks></remarks>
        ABR = 4
        ''' <summary>
        ''' Sales General Manager
        ''' </summary>
        ''' <remarks></remarks>
        SGM = 5
        ''' <summary>
        ''' Branch Manager
        ''' </summary>
        ''' <remarks></remarks>
        BM = 6
        ''' <summary>
        ''' Sales Staff Manager
        ''' </summary>
        ''' <remarks></remarks>
        SSM = 7
        ''' <summary>
        ''' Sales Staff
        ''' </summary>
        ''' <remarks>[Obsoleted] 代わりにSS, SLを使用してください</remarks>
        SSF = 8
        ''' <summary>
        ''' Sales Staff
        ''' </summary>
        ''' <remarks></remarks>
        SS = 8
        ''' <summary>
        ''' Sales Staff (Leader)
        ''' </summary>
        ''' <remarks></remarks>
        SL = 8
        ''' <summary>
        ''' Service Adviser
        ''' </summary>
        ''' <remarks></remarks>
        SA = 9
        ''' <summary>
        ''' Service Manager
        ''' </summary>
        ''' <remarks></remarks>
        SM = 10
        ''' <summary>
        ''' Display
        ''' </summary>
        ''' <remarks></remarks>
        DISP = 11
        ''' <summary>
        ''' Distributor Operator
        ''' </summary>
        ''' <remarks>[Obsoleted] 代わりにDOを使用してください</remarks>
        PSO = 12
        ''' <summary>
        ''' Distributor Operator
        ''' </summary>
        ''' <remarks></remarks>
        [DO] = 12
        ''' <summary>
        ''' Distributor Manager
        ''' </summary>
        ''' <remarks>[Obsoleted] 代わりにDMを使用してください</remarks>
        PSM = 13
        ''' <summary>
        ''' Distributor Manager
        ''' </summary>
        ''' <remarks></remarks>
        [DM] = 13
        ''' <summary>
        ''' Service Staff
        ''' </summary>
        ''' <remarks></remarks>
        TEC = 14
        ''' <summary>
        ''' CS Board (DVD)
        ''' </summary>
        ''' <remarks></remarks>
        CSB1 = 30
        ''' <summary>
        ''' CS Board
        ''' </summary>
        ''' <remarks></remarks>
        CSB2 = 31
        ''' <summary>
        ''' SMB
        ''' </summary>
        ''' <remarks></remarks>
        SMB = 32
        ''' <summary>
        ''' SPM(BRANCH)
        ''' </summary>
        ''' <remarks></remarks>
        SPM = 33
        ''' <summary>
        ''' BP-SMB
        ''' </summary>
        ''' <remarks></remarks>
        BP = 40
        ''' <summary>
        ''' BP-Engineer
        ''' </summary>
        ''' <remarks></remarks>
        BPS = 41
        ''' <summary>
        ''' Gate Keeper
        ''' </summary>
        ''' <remarks></remarks>
        GK = 50
        ''' <summary>
        ''' Sales Receptionist
        ''' </summary>
        ''' <remarks></remarks>
        SLR = 51
        ''' <summary>
        ''' Service Receptionist
        ''' </summary>
        ''' <remarks></remarks>
        SVR = 52
        ''' <summary>
        ''' Showroom Status Visualization
        ''' </summary>
        ''' <remarks></remarks>
        SSV = 53
        ''' <summary>
        ''' Parts Staff
        ''' </summary>
        ''' <remarks></remarks>
        PS = 54
        ''' <summary>
        ''' Controller
        ''' </summary>
        ''' <remarks></remarks>
        CT = 55
        ''' <summary>
        ''' Fore Man
        ''' </summary>
        ''' <remarks></remarks>
        FM = 58
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
        ''' <summary>
        ''' Status Monitor
        ''' </summary>
        ''' <remarks></remarks>
        STM = 60
        ''' <summary>
        ''' Chief Technician 
        ''' </summary>
        ''' <remarks></remarks>
        CHT = 62
        ''' <summary>
        ''' Welcome Borad(Service)
        ''' </summary>
        ''' <remarks></remarks>
        WBS = 63
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

        '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START
        ''' <summary>
        ''' Assistant SA(Service)
        ''' </summary>
        ''' <remarks></remarks>
        ASA = 64
        '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

        '2014/12/16 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START
        ''' <summary>
        ''' Car Wash Man
        ''' </summary>
        ''' <remarks></remarks>
        CW = 65
        '2014/12/16 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END
        '2014/03/10 TCS 武田 受注後フォロー機能開発 START
        ''' <summary>
        ''' オフィススタッフ(H/O)
        ''' </summary>
        ''' <remarks></remarks>
        OSH = 71
        ''' <summary>
        ''' オフィススタッフ(ブランチ)
        ''' </summary>
        ''' <remarks></remarks>
        OSB = 72
        '2014/03/10 TCS 武田 受注後フォロー機能開発 END
    End Enum
#End Region

    ''定数を管理するクラスです。
    Public Class Constant



#Region "Constructor"
        Private Sub New()

        End Sub
#End Region

    End Class

#End Region

End Namespace