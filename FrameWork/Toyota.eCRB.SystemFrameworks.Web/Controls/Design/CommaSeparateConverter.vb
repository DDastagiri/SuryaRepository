Namespace Toyota.eCRB.SystemFrameworks.Web.Controls.Design
    ''' <summary>
    ''' プロパティエディタで、String配列の値を編集する際のカスタムコンバートクラスを提供します。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommaSeparateConverter
        Inherits System.ComponentModel.StringConverter

        ''' <summary>
        ''' 指定したコンテキストとカルチャ情報を使用して、指定した値オブジェクトを、指定した型に変換します。
        ''' </summary>
        ''' <param name="context">書式指定コンテキストを提供する System.ComponentModel.ITypeDescriptorContext。</param>
        ''' <param name="culture">System.Globalization.CultureInfo。null が渡された場合は、現在のカルチャが使用されます。</param>
        ''' <param name="value">変換対象の System.Object。</param>
        ''' <param name="destinationType">value パラメーターの変換後の System.Type。</param>
        ''' <returns>変換後の値を表す System.Object。</returns>
        ''' <remarks></remarks>
        Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As System.Type) As Object

            Dim castValue As String() = TryCast(value, String())
            If (destinationType Is GetType(String)) AndAlso (castValue IsNot Nothing) Then
                Return String.Join(",", castValue)
            Else
                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End If
        End Function

        ''' <summary>
        ''' 指定した値オブジェクトを System.String オブジェクトに変換します。
        ''' </summary>
        ''' <param name="context">書式指定コンテキストを提供する System.ComponentModel.ITypeDescriptorContext。</param>
        ''' <param name="culture">使用する System.Globalization.CultureInfo。</param>
        ''' <param name="value">変換対象の System.Object。</param>
        ''' <returns>変換後の値を表す System.Object。</returns>
        ''' <remarks></remarks>
        Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
            If (TypeOf value Is String) Then
                Return CType(value, String).Split(",".ToCharArray)
            Else
                Return MyBase.ConvertFrom(context, culture, value)
            End If
        End Function
    End Class
End Namespace