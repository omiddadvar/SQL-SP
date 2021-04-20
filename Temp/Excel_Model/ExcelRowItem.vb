Public Class ExcelRowItem
    Public column As ExcelCol
    Public value As Object
    Public Sub New(value As Object, column As ExcelCol)
        'Me.column = New ExcelCol(colName, type, excel)
        Me.column = column
        Me.value = value
    End Sub
    Public Sub New(column As ExcelCol)
        Me.column = column
    End Sub
End Class
