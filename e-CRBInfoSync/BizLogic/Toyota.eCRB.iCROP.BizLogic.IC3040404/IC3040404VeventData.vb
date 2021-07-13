
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Xml
Imports System.Text
Imports Toyota.eCRB.iCROP.DataAccess.IC3040404
Imports Toyota.eCRB.iCROP.DataAccess

Namespace IC3040404.BizLogic

    ''' <summary>
    ''' VEVENTのデータ   2012/12/12
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VeventData
        Private InIcsName As String         'ICSファイル名（拡張子なし）パスも削除しファイル名のみ=UID
        Private InTitle As String           'タイトル
        Private InPlace As String           '場所
        Private InStartTime As String       '開始
        Private InEndTime As String         '終了
        Private InRrule As String           '繰り返しの原文の文字列
        Private InNotify(1) As String       '通知0,1
        Private InExDate As StringBuilder   '除外日
        Private InUrlnfo As String             'URL
        Private InMemo As String            'メモ
        Private InUniqueId As String        'Unique ID
        Private InNotifyCount As Integer    'Notify数
        Private InExDateCount As Integer    'ExDate数
        Private InAllDay As String          '終日フラグ ADD 2011/12/12
        Private InTimeLag As Double         'タイムラグ
        Private InAttendee As String        '招待者
        Private InRecur As String           'リカレンスデータ（RRULEで分割時の新設データ）

        'コンストラクタ
        Sub New()
            Refresh()
        End Sub

        ''' <summary>
        ''' Getter Setter群
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property IcsName As String
            Get
                Return InIcsName
            End Get
            Set(ByVal Value As String)
                InIcsName = Value
            End Set
        End Property

        Property Title As String
            Get
                Return InTitle
            End Get
            Set(ByVal Value As String)
                InTitle = Value
            End Set
        End Property

        Property Place As String
            Get
                Return InPlace
            End Get
            Set(ByVal Value As String)
                InPlace = Value
            End Set
        End Property

        Property StartTime As String
            Get
                Return InStartTime
            End Get
            Set(ByVal Value As String)
                InStartTime = Value
            End Set
        End Property

        Property EndTime As String
            Get
                Return InEndTime
            End Get
            Set(ByVal Value As String)
                InEndTime = Value
            End Set
        End Property

        Property Rrule As String
            Get
                Return InRrule
            End Get
            Set(ByVal Value As String)
                InRrule = Value
            End Set
        End Property

        Property Notify(ByVal index) As String
            Get
                Return InNotify(index)
            End Get
            Set(ByVal Value As String)
                InNotify(index) = Value
            End Set
        End Property

        Property ExDate As StringBuilder
            Get
                Return InExDate
            End Get
            Set(ByVal Value As StringBuilder)
                InExDate = Value
            End Set
        End Property

        Property URInfo As String
            Get
                Return InUrlnfo
            End Get
            Set(ByVal Value As String)
                InUrlnfo = Value
            End Set
        End Property

        Property Memo As String
            Get
                Return InMemo
            End Get
            Set(ByVal Value As String)
                InMemo = Value
            End Set
        End Property

        Property UniqueId As String
            Get
                Return InUniqueId
            End Get
            Set(ByVal Value As String)
                InUniqueId = Value
            End Set
        End Property

        Property NotifyCount As Integer
            Get
                Return InNotifyCount
            End Get
            Set(ByVal Value As Integer)
                InNotifyCount = Value
            End Set
        End Property

        Property ExDateCount As Integer
            Get
                Return InExDateCount
            End Get
            Set(ByVal Value As Integer)
                InExDateCount = Value
            End Set
        End Property

        Property AllDay As String
            Get
                Return InAllDay
            End Get
            Set(ByVal Value As String)
                InAllDay = Value
            End Set
        End Property

        Property TimeLag As Double
            Get
                Return InTimeLag
            End Get
            Set(ByVal Value As Double)
                InTimeLag = Value
            End Set
        End Property

        Property Attendee As String
            Get
                Return InAttendee
            End Get
            Set(ByVal Value As String)
                InAttendee = Value
            End Set
        End Property

        Property Recur As String
            Get
                Return InRecur
            End Get
            Set(ByVal Value As String)
                InRecur = Value
            End Set
        End Property

        ''' <summary>
        ''' 再初期化
        ''' </summary>
        ''' <remarks></remarks>
        Sub Refresh()
            InIcsName = ""
            InTitle = ""
            InPlace = ""
            InStartTime = ""
            InEndTime = ""
            InRrule = ""
            InNotify(0) = ""
            InNotify(1) = ""
            InExDate = New StringBuilder
            InExDate.Length = 0
            InUrlnfo = ""
            InMemo = ""
            InUniqueId = ""
            InNotifyCount = 0
            InExDateCount = 0   'Add 2011/12/17
            InAllDay = "0"      'Add 2011/12/12
            InAttendee = ""     'Add 2011/12/15
            InRecur = " "       'リカレンスの初期値はスペース1文字 " "Add 2011/12/18
            InTimeLag = 0
        End Sub

    End Class

End Namespace
