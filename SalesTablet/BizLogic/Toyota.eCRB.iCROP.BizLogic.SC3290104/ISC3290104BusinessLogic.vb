Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess

''' <summary>
''' フォロー設定 インタフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3290104BusinessLogic

    ''' <summary>
    ''' 異常項目フォローの設定
    ''' </summary>
    ''' <param name="row">異常項目フォローの行</param>
    ''' <param name="account">更新するアカウント</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function SetIrregularFollowInfo(ByVal row As SC3290104DataSet.SC3290104IrregFllwRow, ByVal account As String, ByVal nowDate As Date) As Integer

End Interface
