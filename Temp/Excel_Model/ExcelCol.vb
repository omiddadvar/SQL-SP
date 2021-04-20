Public Class ExcelCol
    Public name As String
    Public type As Type
    Public excelColumn As String

    Public Sub New(name As String, type As Type, excelColumn As String)
        Me.name = name
        Me.type = type
        Me.excelColumn = excelColumn
    End Sub
End Class
