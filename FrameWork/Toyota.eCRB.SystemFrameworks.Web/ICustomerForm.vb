Namespace Toyota.eCRB.SystemFrameworks.Web
    ''' <summary>
    ''' マスターページのロックボタンと連携して、お客様操作に対応するページを実装する際に使用するAPIのインターフェイスです。
    ''' </summary>
    Public Interface ICustomerForm

        ''' <summary>
        ''' ロック状態の初期値を取得します。
        ''' </summary>
        ''' <returns>True:ロック状態、False：未ロック状態</returns>
        ''' <remarks></remarks>
        ReadOnly Property DefaultOperationLocked As Boolean


    End Interface
End Namespace