
Public Class ESRRecord_Camt054

	Public Property GrpHdr As GrpHdrData
	Public Property Ntfctn As NtfctnData

End Class

Public Class NtfctnData

	Public Property Id As String
	Public Property ElctrncSeqNb As Integer?
	Public Property CreDtTm As DateTime?
	Public Property FrDtTm As DateTime?
	Public Property ToDtTm As DateTime?
	Public Property CpyDplctInd As String
	Public Property Acct As AcctData
	Public Property Ntry As List(Of NtryData)

End Class

Public Class NtryData
	Public Property NtryRef As String
	Public Property Amt As Double?
	Public Property CdtDbtInd As String
	Public Property RvslInd As Boolean?
	Public Property Sts As String
	Public Property BookgDt As DateTime?
	Public Property ValDt As DateTime?
	Public Property AcctSvcrRef As String
	Public Property NtryDtls As NtryDtlsData
	Public Property AddtlNtryInf As String

End Class

Public Class NtryDtlsData
	Public Property Btch As Integer?
	Public Property TxDtls As List(Of TxDtlsData)

End Class

Public Class GrpHdrData
	Public Property MsgId As String
	Public Property CreDtTm As DateTime?
	Public Property MsgRcpt As String
	Public Property AddtlInf As String

End Class

Public Class AcctData
	Public Property IBAN As String
	Public Property Nm As String
	Public Property OwnerNm As String
	Public Property OwnerAdrLine As String

End Class

Public Class TxDtlsData
	Public Property Refs As TxDtlsRef
	Public Property Amount As Decimal?
	Public Property CdtDbtInd As String
	Public Property RltdPtiesNm As String
	Public Property RmtInf As TxDtlsRmtInf
	Public Property AccptncDtTm As DateTime?

End Class

Public Class TxDtlsRef
	Public Property AcctSvcrRef As String
	Public Property EndToEndId As String
	Public Property Tp As String
	Public Property Ref As String

End Class

Public Class TxDtlsRmtInf
	Public Property Prtry As String
	Public Property Ref As String

	Public Property InvoiceNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property InvoiceAmouint As Decimal?


End Class




