'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'TDISFooterButton.vb
'─────────────────────────────────────
'機能： TDISフッターボタン
'補足： 
'作成： 2014/08/29 TCS 武田 Next追加要件
'─────────────────────────────────────

Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' TDISフッターボタン
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public NotInheritable Class TDISFooterButton

#Region "ローカル変数"

        Private _dlrCd As String
        Private _brnCd As String
        Private _contNo As String

#End Region

#Region "Property"

        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        Public Property DlrCD() As String
            Get
                Return _dlrCd
            End Get
            Set(ByVal value As String)
                _dlrCd = value
            End Set
        End Property

        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        Public Property BrnCD() As String
            Get
                Return _brnCd
            End Get
            Set(ByVal value As String)
                _brnCd = value
            End Set
        End Property

        ''' <summary>
        ''' 契約書No.
        ''' </summary>
        Public Property ContNo() As String
            Get
                Return _contNo
            End Get
            Set(ByVal value As String)
                _contNo = value
            End Set
        End Property

#End Region

#Region "Constructor"
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()
        End Sub
#End Region

    End Class

End Namespace