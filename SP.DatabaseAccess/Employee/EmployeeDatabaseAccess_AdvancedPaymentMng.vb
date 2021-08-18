
Imports SP.DatabaseAccess.Employee.DataObjects.AdvancedPaymentMng
Imports System.Text

Namespace Employee

  Partial Public Class EmployeeDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IEmployeeDatabaseAccess

    ''' <summary>
    ''' Loads employee LO (salary) settings.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>The LO settings or nothing in error case.</returns>
    Function LoadEmployeeAdvancedPaymentSettings(ByVal employeeNumber As Integer, ByVal Month As Integer?, ByVal lang As String, ByVal mdnumber As Integer,
                                                 Optional ByVal onlyCurrentYear As Boolean = False) As IEnumerable(Of EmployeeAdvancedPaymentData) Implements IEmployeeDatabaseAccess.LoadEmployeeAdvancedPaymentSettings

      Dim result As List(Of EmployeeAdvancedPaymentData) = Nothing

      Dim translatedLAColumn As String = String.Empty
      Dim sql As String = String.Empty

      Select Case lang.ToLower()
        Case "deutsch"
          translatedLAColumn = "LA.LALoText "
        Case "französisch"
          translatedLAColumn = "LAB.Name_LO_F "
        Case "italienisch"
          translatedLAColumn = "LAB.Name_LO_I"
        Case "englisch"
          translatedLAColumn = "LAB.Name_LO_E"
        Case Else
          translatedLAColumn = "LA.LALoText"
      End Select

      sql = "SELECT ZG.[ID]"
      sql = sql & ",ZG.[RPNr]"
      sql = sql & ",ZG.[ZGNr]"
      sql = sql & ",ZG.[MANr]"
      sql = sql & ",ZG.[LANr]"
      sql = sql & ",ZG.[LONr]"
      sql = sql & ",ZG.[VGNr]"

      sql = sql & String.Format(",{0} as TranslatedLAColumn ", translatedLAColumn)

      sql = sql & ",ZG.[ZGGrund]"
      sql = sql & ",ZG.[Betrag]"
      sql = sql & ",ZG.[Anzahl]"
      sql = sql & ",ZG.[Ansatz]"
      sql = sql & ",ZG.[Basis]"
      sql = sql & ",ZG.[Currency]"
      sql = sql & ",Convert(int, ZG.[LP]) As LP"
      sql = sql & ",Convert(Int, ZG.[Jahr]) As Jahr"
      sql = sql & ",ZG.[Aus_Dat]"
      sql = sql & ",ZG.[ClearingNr]"
      sql = sql & ",ZG.[Bank]"
      sql = sql & ",ZG.[Bank]"
      sql = sql & ",ZG.[BankOrt]"
      sql = sql & ",ZG.[CreatedOn]"
      sql = sql & ",ZG.[CreatedFrom]"

      sql = sql & "FROM [ZG] "
			sql = sql & "LEFT JOIN LA_Translated LAB ON ZG.LANR = LAB.LANR "
			sql = sql & "LEFT JOIN LA ON ZG.LANR = LA.LANr AND Year(ZG.Aus_Dat) = LA.LAJahr "

			sql &= "WHERE LA.LADeactivated = 0 "
			sql &= "And ZG.MANr = @employeeNumber "
			sql &= "And (ZG.LP = @Month Or @Month = 0) "
      sql &= "And (ZG.Jahr = @Jahr Or @Jahr = 0) "
      sql &= "Order By ZG.Jahr Desc, ZG.LP Desc "

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", employeeNumber))
      listOfParams.Add(New SqlClient.SqlParameter("Jahr", If(onlyCurrentYear, Now.Year, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("Month", 0))  ' is to see later !!! If(onlyCurrentYear, ReplaceMissing(Month, 0), 0)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          result = New List(Of EmployeeAdvancedPaymentData)

          While reader.Read

            Dim employeeAdvancedPaymentSettting As New EmployeeAdvancedPaymentData

            employeeAdvancedPaymentSettting.ID = SafeGetInteger(reader, "ID", 0)

            employeeAdvancedPaymentSettting.paymentnumber = SafeGetInteger(reader, "zgnr", Nothing)
            employeeAdvancedPaymentSettting.salarynumber = SafeGetInteger(reader, "lonr", Nothing)
            employeeAdvancedPaymentSettting.reportnumber = SafeGetInteger(reader, "rpnr", Nothing)
            employeeAdvancedPaymentSettting.employeenumber = SafeGetInteger(reader, "MANr", Nothing)
            employeeAdvancedPaymentSettting.translatedLAname = SafeGetString(reader, "TranslatedLAColumn")

            employeeAdvancedPaymentSettting.paymentdate = SafeGetDateTime(reader, "aus_dat", Nothing)
            employeeAdvancedPaymentSettting.paymentmonth = SafeGetInteger(reader, "lp", Nothing)
            employeeAdvancedPaymentSettting.paymentyear = SafeGetInteger(reader, "Jahr", Nothing)
            employeeAdvancedPaymentSettting.Amount = SafeGetDecimal(reader, "Betrag", Nothing)
            employeeAdvancedPaymentSettting.createdon = SafeGetDateTime(reader, "createdon", Nothing)
            employeeAdvancedPaymentSettting.createdfrom = SafeGetString(reader, "createdFrom")
            employeeAdvancedPaymentSettting.paymentreason = SafeGetString(reader, "zggrund")

            result.Add(employeeAdvancedPaymentSettting)

          End While

        End If


      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        result = Nothing

      Finally
        CloseReader(reader)
      End Try

      Return result

    End Function


  End Class

End Namespace

