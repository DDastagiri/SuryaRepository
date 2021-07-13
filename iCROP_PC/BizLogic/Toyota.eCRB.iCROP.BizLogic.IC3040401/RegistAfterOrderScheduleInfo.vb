'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlRegistAfterOrderScheduleInfo.vb
'──────────────────────────────────
'機能： CalDAV連携インターフェース
'補足： 
'作成： 2014/04/24 TMEJ t.mizumoto 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計
'──────────────────────────────────
Namespace Toyota.eCRB.iCROP.BizLogic.IC3040401

    Public Class RegistAfterOrderScheduleInfo

        ''' <summary>
        ''' 顧客区分
        ''' </summary>
        ''' <remarks></remarks>
        Private _customerDiv As String

        ''' <summary>
        ''' 顧客コード
        ''' </summary>
        ''' <remarks></remarks>
        Private _customerCode As String

        ''' <summary>
        ''' DMSID
        ''' </summary>
        ''' <remarks></remarks>
        Private _dmsId As String

        ''' <summary>
        ''' 顧客名
        ''' </summary>
        ''' <remarks></remarks>
        Private _customerName As String

        ''' <summary>
        ''' 削除日
        ''' </summary>
        ''' <remarks></remarks>
        Private _deleteDate As String


        ''' <summary>
        ''' 顧客区分
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CustomerDiv As String
            Get
                Return _customerDiv
            End Get
            Set(value As String)
                _customerDiv = value
            End Set
        End Property

        ''' <summary>
        ''' 顧客コード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CustomerCode As String
            Get
                Return _customerCode
            End Get
            Set(value As String)
                _customerCode = value
            End Set
        End Property

        ''' <summary>
        ''' DMSID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DmsId As String
            Get
                Return _dmsId
            End Get
            Set(value As String)
                _dmsId = value
            End Set
        End Property

        ''' <summary>
        ''' 顧客名
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CustomerName As String
            Get
                Return _customerName
            End Get
            Set(value As String)
                _customerName = value
            End Set
        End Property

        ''' <summary>
        ''' 削除日
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteDate As String
            Get
                Return _deleteDate
            End Get
            Set(value As String)
                _deleteDate = value
            End Set
        End Property


    End Class

End Namespace
