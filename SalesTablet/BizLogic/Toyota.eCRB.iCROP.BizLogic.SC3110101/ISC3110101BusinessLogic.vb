Imports Toyota.eCRB.TrialRide.TrialRidePreparation.DataAccess

''' <summary>
''' 試乗入力 インタフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3110101BusinessLogic

    ''' <summary>
    ''' 試乗車ステータスの更新/挿入を行う
    ''' </summary>
    ''' <param name="updateCarStatus">更新対象データ</param>
    ''' <param name="account">更新者(アカウント)</param>
    ''' <remarks></remarks>
    Function UpdateTestDriveCarStatus(ByVal updateCarStatus As SC3110101DataSet.SC3110101InsertTestDriveCarStatusDataTable, ByVal account As String) As Integer

End Interface
