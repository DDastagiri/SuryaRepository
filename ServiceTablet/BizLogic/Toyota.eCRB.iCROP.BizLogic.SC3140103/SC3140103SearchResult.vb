Imports Toyota.eCRB.iCROP.DataAccess.SC3140103.SC3140103DataSet

Public Class SC3140103SearchResult
    Private _dataTable As SC3140103VisitSearchResultDataTable
    Private _resultStartRow As Long
    Private _resultEndRow As Long
    Private _searchResult As Long
    Private _resultCustomerCount As Long
    Private _standardCount As Long

    Public Property DataTable As SC3140103VisitSearchResultDataTable
        Get
            Return _dataTable
        End Get
        Set(value As SC3140103VisitSearchResultDataTable)
            _dataTable = value
        End Set
    End Property

    Public Property ResultStartRow As Long
        Get
            Return _resultStartRow
        End Get
        Set(value As Long)
            _resultStartRow = value
        End Set
    End Property

    Public Property ResultEndRow As Long
        Get
            Return _resultEndRow
        End Get
        Set(value As Long)
            _resultEndRow = value
        End Set
    End Property

    Public Property SearchResult As Long
        Get
            Return _searchResult
        End Get
        Set(value As Long)
            _searchResult = value
        End Set
    End Property

    Public Property ResultCustomerCount As Long
        Get
            Return _resultCustomerCount
        End Get
        Set(value As Long)
            _resultCustomerCount = value
        End Set
    End Property

    Public Property StandardCount As Long
        Get
            Return _standardCount
        End Get
        Set(value As Long)
            _standardCount = value
        End Set
    End Property

End Class
