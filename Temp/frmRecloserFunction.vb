Imports System.Data.SqlClient

Public Class frmRecloserFunction

    Private mCnn As SqlConnection = New SqlConnection(GetConnection())
    Private mDs As DataSet

    Private Sub frmRecloserFunction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadAreaData()
    End Sub
    Private Sub cboArea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboArea.SelectedIndexChanged

    End Sub

    Private Sub loadAreaData()
        Dim lSQL As String = "SELECT * FROM Tbl_Area"
        BindingTable(lSQL, mCnn, mDs, "Tbl_Area", cboArea, , , True)
    End Sub

End Class