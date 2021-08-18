
Imports System.Runtime.InteropServices


Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPLOLKontiSearch.ClsDataDetail


Public Class MyApi
	<DllImport("user32.dll")> Public Shared Function _
				 FindWindow(ByVal strClassName As String, ByVal strWindowName As String) As Integer
	End Function
End Class


Module FuncLv

	Private m_Logger As ILogger = New Logger()

	'Private _ClsFunc As New ClsDivFunc
	'Private _ClsReg As New SPProgUtility.ClsDivReg
	'Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Function GetMADbData4Lohnkonti(ByVal iYear As Integer) As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strQuery As String = "[List MAData For Search In LOKonti]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter
    Dim param As System.Data.SqlClient.SqlParameter

    param = cmd.Parameters.AddWithValue("@MDYear", iYear)

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "MAData")

    Return ds.Tables(0)
  End Function


#Region "Dropdown-Funktionen für 1. Seite..."

  ' Jahr ---------------------------------------------------------------------------------------------
  Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
    Dim strSqlQuery As String = "Select LO.Jahr From LO Where LO.Jahr > 2006 "
    strSqlQuery += "Group By LO.Jahr Order By LO.Jahr DESC"


    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rFOPrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rFOPrec.Read
        strEntry = rFOPrec("Jahr").ToString
        cbo.Properties.Items.Add(strEntry)

        i += 1
      End While

    Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub


#End Region


End Module
