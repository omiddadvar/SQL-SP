Imports System.Collections.Generic

Public Class ExcelRow
    Public items As List(Of ExcelRowItem)
    Public length As Int32

    Public Sub New()
        Me.items = New List(Of ExcelRowItem)
        Me.length = 0
    End Sub

    Public Sub Add(ByVal item As ExcelRowItem)
        items.Add(item)
        length += 1
    End Sub
End Class
