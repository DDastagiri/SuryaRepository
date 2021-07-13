'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlRegistAfterOrder.vb
'──────────────────────────────────
'機能： CalDAV連携インターフェース
'補足： 
'作成： 2014/04/24 TMEJ t.mizumoto 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計
'──────────────────────────────────
Namespace Toyota.eCRB.iCROP.BizLogic.IC3040401

    Public Class RegistAfterOrder

        ''' <summary>
        ''' Detail要素のリスト
        ''' </summary>
        ''' <remarks></remarks>
        Private _detailList As List(Of RegistAfterOrderDetail)


        ''' <summary>
        ''' Detail要素のリスト
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DetailList As List(Of RegistAfterOrderDetail)
            Get
                Return _detailList
            End Get
        End Property


        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            Me._detailList = New List(Of RegistAfterOrderDetail)

        End Sub

    End Class

End Namespace
