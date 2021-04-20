Imports System.Collections.Generic

Public Class ExcelModel
    Public rows As List(Of ExcelRow)
    Public columns As List(Of ExcelCol)
    Public columnNames As List(Of String)

    Public Sub New(columns As List(Of ExcelCol))
        Me.columns = columns
        Me.rows = New List(Of ExcelRow)
        Me.columnNames = New List(Of String)
    End Sub
End Class
