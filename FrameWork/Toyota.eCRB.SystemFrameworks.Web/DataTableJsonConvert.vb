Imports System.Web.Script.Serialization

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' <see cref="DataTable"/>をJSON形式の文字列に変換するための変換クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DataTableJsonConvert
        Inherits JavaScriptConverter

        ''' <summary>
        ''' デシリアライズメソッドは使用予定がないため実装していません。<br/>
        ''' 呼び出しても変換されません。
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function Deserialize(dictionary As System.Collections.Generic.IDictionary(Of String, Object), type As System.Type, serializer As System.Web.Script.Serialization.JavaScriptSerializer) As Object
            Return New Object
        End Function

        ''' <summary>
        ''' 名前/値ペアのディクショナリを構築します。
        ''' </summary>
        ''' <param name="obj">シリアル化するオブジェクト。</param>
        ''' <param name="serializer">シリアル化を処理するオブジェクト。</param>
        ''' <returns>オブジェクトのデータを表すキー/値ペアを含むオブジェクト。</returns>
        ''' <remarks></remarks>
        Public Overrides Function Serialize(obj As Object, serializer As System.Web.Script.Serialization.JavaScriptSerializer) As System.Collections.Generic.IDictionary(Of String, Object)

            Dim table As DataTable = TryCast(obj, DataTable)

            If table Is Nothing Then
                Return New Dictionary(Of String, Object)
            End If

            '変換
            Dim result As New Dictionary(Of String, Object)
            Dim list As New List(Of Dictionary(Of String, Object))

            '格納
            For Each dr As DataRow In table.Rows
                'レコード別の連想配列初期化
                Dim listDict As New Dictionary(Of String, Object)

                '列ループ
                For Each dc As DataColumn In table.Columns
                    listDict.Add(dc.ColumnName, dr(dc.ColumnName))
                Next

                '変換リストに登録
                list.Add(listDict)
            Next

            result("DataTable") = list
            Return result
        End Function

        ''' <summary>
        ''' サポートされている型のコレクションを取得します。
        ''' </summary>
        ''' <returns>コンバーターによってサポートされている型を表す <see cref="System.Collections.Generic.IEnumerable(Of T)"/>を実装するオブジェクト。</returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property SupportedTypes As System.Collections.Generic.IEnumerable(Of System.Type)
            Get
                Dim lstTypes As New List(Of System.Type)
                lstTypes.Add(GetType(DataTable))
                Return lstTypes
            End Get
        End Property
    End Class

End Namespace