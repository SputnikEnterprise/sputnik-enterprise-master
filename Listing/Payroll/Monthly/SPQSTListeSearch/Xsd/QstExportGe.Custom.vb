'------------------------------------------------------------------------------
' Diese Datei enthält Erweiterungen für die automatisch generierten Klassen
' in QstExportGe.vb.
'
' Diese Erweiterungen bleiben auch erhalten wenn QstExportGe.vb
' mit Xsd.exe neu generiert wird.
'------------------------------------------------------------------------------

Namespace Xsd.Ge

  Public Class QstExportGe

#Region "Public Constants"

    ' TODO: in Business Logik- oder Datenzugrisffs-Layer verschieben
    Public Const SP_ZIVILSTAND_LEDIG = "L"
    Public Const SP_ZIVILSTAND_VERHEIRATET = "V"
    Public Const SP_ZIVILSTAND_GESCHIEDEN = "G"
    Public Const SP_ZIVILSTAND_GETRENNT = "T"
    Public Const SP_ZIVILSTAND_VERWITWET = "W"
    Public Const SP_ZIVILSTAND_UNBEKANNT_R = "R"
    Public Const SP_ZIVILSTAND_UNBEKANNT_U = "U"
    Public Const SP_ZIVILSTAND_UNBEKANNT_X = "X"

    ' TODO: in Business Logik- oder Datenzugrisffs-Layer verschieben
    Public Const SP_GESCHLECHT_MAENNLICH = "M"
    Public Const SP_GESCHLECHT_WEIBLICH = "W"

#End Region

#Region "Public Shared Methods"

    ''' <summary>
    ''' Liefert das XSD Land für einen Ländercode.
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPaysOuSuisse(code As String) As PaysOuSuisse_Type
      Dim paysOuSuisse As New PaysOuSuisse_Type()
      Select Case code.ToUpper()
        Case "CH"
          paysOuSuisse.Item = EnumCH_Type.CH
        Case Else
          For Each enumPays_Type In [Enum].GetValues(GetType(EnumPays_Type))
            If enumPays_Type.ToString() = code Then
              paysOuSuisse.Item = CType(enumPays_Type, EnumPays_Type)
              Exit For
            End If
          Next
      End Select
      Return paysOuSuisse
    End Function

    ''' <summary>
    ''' Liefert die XSD Gemeinde für einen Gemeindecode.
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCommuneGE(code As String) As EnumCommuneGE_Type?
      If code IsNot Nothing AndAlso code.Length = 4 Then
        For Each enumCommuneGE_Type In [Enum].GetValues(GetType(EnumCommuneGE_Type))
          If enumCommuneGE_Type.ToString() = "Item" + code Then
            Return CType(enumCommuneGE_Type, EnumCommuneGE_Type)
          End If
        Next
      End If
      Return Nothing
    End Function

		''' <summary>
		''' Liefert die XSD Canton für einen Cantoncode.
		''' </summary>
		''' <param name="code"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCanton(code As String) As EnumCanton_Type?
			If code IsNot Nothing AndAlso code.Length = 2 Then
				For Each enumCanton_Type In [Enum].GetValues(GetType(EnumCanton_Type))
					If enumCanton_Type.ToString() = code Then
						Return CType(enumCanton_Type, EnumCanton_Type)
					End If
				Next
			End If
			Return Nothing
		End Function

    ''' <summary>
    ''' Liefert den XSD Zivilstand für den Sputnik Zivilstandscode.
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEtatCivil(code As String) As EnumEtatCivil_Type
      Select Case code.ToUpper()
        Case SP_ZIVILSTAND_LEDIG
          ' Ledig - Célibataire (1)
          Return EnumEtatCivil_Type.Item1
        Case SP_ZIVILSTAND_VERHEIRATET
          ' Verheiratet - Marié (2)
          Return EnumEtatCivil_Type.Item2
        Case SP_ZIVILSTAND_GESCHIEDEN
          ' Geschieden - Divorcé (3)
          Return EnumEtatCivil_Type.Item3
        Case SP_ZIVILSTAND_GETRENNT
          ' Getrennt lebend - Séparé (4)
          Return EnumEtatCivil_Type.Item4
        Case SP_ZIVILSTAND_VERWITWET
          ' Verwitwet - Veuf (5)
          Return EnumEtatCivil_Type.Item5
        Case ""
          ' TODO: richtiger Code
          ' Parterschaft eingetragen - Partenariat enregistré (6)
          Return EnumEtatCivil_Type.Item6
        Case SP_ZIVILSTAND_UNBEKANNT_R
          ' TODO: richtiger Code
          Return EnumEtatCivil_Type.Item1
        Case SP_ZIVILSTAND_UNBEKANNT_U
          ' TODO: richtiger Code
          Return EnumEtatCivil_Type.Item1
        Case SP_ZIVILSTAND_UNBEKANNT_X
          ' TODO: richtiger Code
          Return EnumEtatCivil_Type.Item1
        Case Else
          ' Default: Ledig - Célibataire (1)
          Return EnumEtatCivil_Type.Item1
      End Select
    End Function

    ''' <summary>
    ''' Liefert das XSD Geschlecht für den Sputnik Geschlechtscode.
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSexe(code As String) As EnumSexe_Type

      Select Case code.ToUpper()
        Case SP_GESCHLECHT_MAENNLICH
          ' Männlich - Masculin (1)
          Return EnumSexe_Type.Item1
        Case SP_ZIVILSTAND_VERHEIRATET
          ' Weiblich - Féminin (2)
          Return EnumSexe_Type.Item2
        Case Else
          ' Default: Männlich - Masculin (1)
          Return EnumSexe_Type.Item1
      End Select
    End Function

#End Region

  End Class

  ''' <summary>
  ''' Allgemeine Klasse mit allen Informationen zum Erstellen der Klassen vom Typ DeclarationContribuableT1...6_Type.
  ''' <author>egle</author>
  ''' </summary>
  ''' <remarks></remarks>
  Public Class DeclarationContribuableCommon_Type

#Region "Public Shared Methods"

    ''' <summary>
    ''' Liefert den typisierten Array von ListeRecapitulativeTypeX_Type Instanzen als Object.
    ''' Type 1 : les salariés, les activités accessoires, les permis 120j, les administrateurs et les effeuilleurs
    ''' Type 2 : les autres cantons
    ''' Type 3 : les bénéficiaires de rente
    ''' Type 4 : les revenus acquis en compensation et le travail au noir
    ''' Type 5 : les artistes, les sportifs et les conférenciers
    ''' Type 6 : les prestations en capital 
    ''' Exemple : un employeur pour fournir 2 fichier LR de type 1 et 3 doit posséder 2 numéros de DPI
    ''' FOLGERUNG: es muss pro Typ eine Liste generiert werden!!!
    ''' </summary>
    ''' <param name="declarationContribuableCommonList"></param>
    ''' <param name="listType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
		Public Shared Function GetListRecapitulativeArray(declarationContribuableCommonList As List(Of DeclarationContribuableCommon_Type), listType As Integer) As Object
			Dim i As Integer = 0
			Select Case listType
				Case 1
					Dim listeRecapitulative As New ListeRecapitulativeType1_Type2014()
					Dim declarationContribuableArray(declarationContribuableCommonList.Count - 1) As DeclarationContribuableType1_Type2014
					For Each declarationContribuableCommon As DeclarationContribuableCommon_Type In declarationContribuableCommonList
						declarationContribuableArray(i) = New DeclarationContribuableType1_Type2014(declarationContribuableCommon)
						i = i + 1
					Next
					listeRecapitulative.declarationContribuable = declarationContribuableArray
					Return listeRecapitulative
				Case 2
					Dim listeRecapitulative As New ListeRecapitulativeType2_Type2014()
					Dim declarationContribuableArray(declarationContribuableCommonList.Count - 1) As DeclarationContribuableType2_Type2014
					For Each declarationContribuableCommon As DeclarationContribuableCommon_Type In declarationContribuableCommonList
						declarationContribuableArray(i) = New DeclarationContribuableType2_Type2014(declarationContribuableCommon)
						i = i + 1
					Next
					listeRecapitulative.declarationContribuable = declarationContribuableArray
					Return listeRecapitulative
				Case 3
					Dim listeRecapitulative As New ListeRecapitulativeType3_Type2014()
					Dim declarationContribuableArray(declarationContribuableCommonList.Count - 1) As DeclarationContribuableType3_Type2014
					For Each declarationContribuableCommon As DeclarationContribuableCommon_Type In declarationContribuableCommonList
						declarationContribuableArray(i) = New DeclarationContribuableType3_Type2014(declarationContribuableCommon)
						i = i + 1
					Next
					listeRecapitulative.declarationContribuable = declarationContribuableArray
					Return listeRecapitulative
				Case 4
					Dim listeRecapitulative As New ListeRecapitulativeType4_Type2014()
					Dim declarationContribuableArray(declarationContribuableCommonList.Count - 1) As DeclarationContribuableType4_Type2014
					For Each declarationContribuableCommon As DeclarationContribuableCommon_Type In declarationContribuableCommonList
						declarationContribuableArray(i) = New DeclarationContribuableType4_Type2014(declarationContribuableCommon)
						i = i + 1
					Next
					listeRecapitulative.declarationContribuable = declarationContribuableArray
					Return listeRecapitulative
				Case 5
					Dim listeRecapitulative As New ListeRecapitulativeType5_Type2014()
					Dim declarationContribuableArray(declarationContribuableCommonList.Count - 1) As DeclarationContribuableType5_Type2014
					For Each declarationContribuableCommon As DeclarationContribuableCommon_Type In declarationContribuableCommonList
						declarationContribuableArray(i) = New DeclarationContribuableType5_Type2014(declarationContribuableCommon)
						i = i + 1
					Next
					listeRecapitulative.declarationContribuable = declarationContribuableArray
					Return listeRecapitulative
				Case 6
					Dim listeRecapitulative As New ListeRecapitulativeType6_Type2014()
					Dim declarationContribuableArray(declarationContribuableCommonList.Count - 1) As DeclarationContribuableType6_Type2014
					For Each declarationContribuableCommon As DeclarationContribuableCommon_Type In declarationContribuableCommonList
						declarationContribuableArray(i) = New DeclarationContribuableType6_Type2014(declarationContribuableCommon)
						i = i + 1
					Next
					listeRecapitulative.declarationContribuable = declarationContribuableArray
					Return listeRecapitulative
				Case Else
					' Nicht unterstützt
					Return Nothing
			End Select
		End Function

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
      MyBase.New()
      Me.m_baremepreferentiel = False
    End Sub

#End Region

#Region "Public Properties"

    Public Property TypeContribuable() As Integer
      Get
        Return Me.m_typeContribuable
      End Get
      Set(value As Integer)
        Me.m_typeContribuable = value
      End Set
    End Property

    Public Property Confession() As EnumConfession_Type
      Get
        Return Me.m_confession
      End Get
      Set(value As EnumConfession_Type)
        Me.m_confession = value
      End Set
    End Property

    Public Property ConfessionSpecified() As Boolean
      Get
        Return Me.m_confessionSpecified
      End Get
      Set(value As Boolean)
        Me.m_confessionSpecified = value
      End Set
    End Property

    Public Property InfoContribuable() As InfoPersonne_Type
      Get
        Return Me.m_infoContribuable
      End Get
      Set(value As InfoPersonne_Type)
        Me.m_infoContribuable = value
      End Set
    End Property

		Public Property Famille() As FamillePersonne_Type2014
			Get
				Return Me.m_famille
			End Get
			Set(value As FamillePersonne_Type2014)
				Me.m_famille = value
			End Set
		End Property

    Public Property AdresseDomicile() As AdresseDomicile_Type
      Get
        Return Me.m_adresseDomicile
      End Get
      Set(value As AdresseDomicile_Type)
        Me.m_adresseDomicile = value
      End Set
    End Property

    Public Property Baremepreferentiel() As Boolean
      Get
        Return Me.m_baremepreferentiel
      End Get
      Set(value As Boolean)
        Me.m_baremepreferentiel = value
      End Set
    End Property

		Public Property AssujettissementContribuable() As Assujettissement_Type2014()
			Get
				Return Me.m_assujettissementContribuable
			End Get
			Set(value As Assujettissement_Type2014())
				Me.m_assujettissementContribuable = value
			End Set
		End Property

    Public Property RetenuePrestationsImpots() As Object
      Get
        Return Me.m_retenuePrestationsImpots
      End Get
      Set(value As Object)
        Me.m_retenuePrestationsImpots = value
      End Set
    End Property

    Public Property AdresseExpedition() As AdresseExpedition_Type
      Get
        Return Me.m_adresseExpedition
      End Get
      Set(value As AdresseExpedition_Type)
        Me.m_adresseExpedition = value
      End Set
    End Property

    Public Property TexteLibre() As TexteLibre_Type
      Get
        Return Me.m_texteLibre
      End Get
      Set(value As TexteLibre_Type)
        Me.m_texteLibre = value
      End Set
    End Property

#End Region

#Region "Private Fields"

    Private m_typeContribuable As Integer
		Private m_confession As EnumConfession_Type
    Private m_confessionSpecified As Boolean
    Private m_infoContribuable As InfoPersonne_Type
		Private m_famille As FamillePersonne_Type2014
		Private m_adresseDomicile As AdresseDomicile_Type
    Private m_baremepreferentiel As Boolean
		Private m_assujettissementContribuable() As Assujettissement_Type2014
    Private m_retenuePrestationsImpots As Object
		Private m_adresseExpedition As AdresseExpedition_Type
		Private m_texteLibre As TexteLibre_Type

#End Region

  End Class

	Partial Public Class DeclarationContribuableType1_Type2014

		Public Sub New(declarationContribuableCommon As DeclarationContribuableCommon_Type)
			Me.New()
			Me.typeContribuable = ContribuableLR1FromInt(declarationContribuableCommon.TypeContribuable)
			Me.confession = declarationContribuableCommon.Confession
			Me.infoContribuable = declarationContribuableCommon.InfoContribuable
			Me.famille = declarationContribuableCommon.Famille
			Me.adresseDomicile = declarationContribuableCommon.AdresseDomicile
			Me.baremepreferentiel = declarationContribuableCommon.Baremepreferentiel
			Me.assujettissementContribuable = declarationContribuableCommon.AssujettissementContribuable
			Me.retenuePrestationsImpots = CType(declarationContribuableCommon.RetenuePrestationsImpots, RetenueType1_2_Type2014)
			Me.adresseExpedition = declarationContribuableCommon.AdresseExpedition
			Me.texteLibre = declarationContribuableCommon.TexteLibre
		End Sub

		Public Shared Function ContribuableLR1FromInt(value As Integer) As EnumTypeContribuableLR1_Type
			Select Case value
				Case 1
					Return EnumTypeContribuableLR1_Type.Item1
				Case 2
					Return EnumTypeContribuableLR1_Type.Item2
				Case 3
					Return EnumTypeContribuableLR1_Type.Item3
				Case 4
					Return EnumTypeContribuableLR1_Type.Item4
				Case 5
					Return EnumTypeContribuableLR1_Type.Item5
				Case Else
					Return EnumTypeContribuableLR1_Type.Item1
			End Select
		End Function

	End Class

	Partial Public Class DeclarationContribuableType2_Type2014

		Public Sub New()
			MyBase.New()
		End Sub

		Public Sub New(declarationContribuableCommon As DeclarationContribuableCommon_Type)
			Me.New()
			Me.typeContribuable = ContribuableLR2FromInt(declarationContribuableCommon.TypeContribuable)
			Me.infoContribuable = declarationContribuableCommon.InfoContribuable
			Me.famille = declarationContribuableCommon.Famille
			Me.adresseDomicile = declarationContribuableCommon.AdresseDomicile
			Me.assujettissementContribuable = declarationContribuableCommon.AssujettissementContribuable
			Me.retenuePrestationsImpots = CType(declarationContribuableCommon.RetenuePrestationsImpots, RetenueType1_2_Type2014)
			' TODO: retenueAutresCantons
			Me.retenueAutresCantons = New RetenueAutreCanton_Type()
			Me.adresseExpedition = declarationContribuableCommon.AdresseExpedition
			Me.texteLibre = declarationContribuableCommon.TexteLibre
		End Sub

		Public Shared Function ContribuableLR2FromInt(value As Integer) As EnumTypeContribuableLR2_Type
			Select Case value
				Case 6
					Return EnumTypeContribuableLR2_Type.Item6
				Case Else
					Return EnumTypeContribuableLR2_Type.Item6
			End Select
		End Function

	End Class

	Partial Public Class DeclarationContribuableType3_Type2014
		Public Sub New()
			MyBase.New()
		End Sub

		Public Sub New(declarationContribuableCommon As DeclarationContribuableCommon_Type)
			Me.New()
			Me.typeContribuable = ContribuableLR3FromInt(declarationContribuableCommon.TypeContribuable)
			Me.infoContribuable = declarationContribuableCommon.InfoContribuable
			Me.adresseDomicile = declarationContribuableCommon.AdresseDomicile
			Me.periodeImposition = Nothing ' TODO
			Me.retenuePrestationsImpots = CType(declarationContribuableCommon.RetenuePrestationsImpots, RetenueType3_4_Type2014)
			Me.adresseExpedition = declarationContribuableCommon.AdresseExpedition
			Me.texteLibre = declarationContribuableCommon.TexteLibre
		End Sub

		Public Shared Function ContribuableLR3FromInt(value As Integer) As EnumTypeContribuableLR3_Type
			Select Case value
				Case 7
					Return EnumTypeContribuableLR3_Type.Item7
				Case Else
					Return EnumTypeContribuableLR3_Type.Item7
			End Select
		End Function

	End Class

	Partial Public Class DeclarationContribuableType4_Type2014
		Public Sub New()
			MyBase.New()
		End Sub

		Public Sub New(declarationContribuableCommon As DeclarationContribuableCommon_Type)
			Me.New()
			Me.typeContribuable = ContribuableLR4FromInt(declarationContribuableCommon.TypeContribuable)
			Me.infoContribuable = declarationContribuableCommon.InfoContribuable
			Me.adresseDomicile = declarationContribuableCommon.AdresseDomicile
			Me.periodeImposition = Nothing ' TODO
			Me.retenuePrestationsImpots = CType(declarationContribuableCommon.RetenuePrestationsImpots, RetenueType3_4_Type2014)
			Me.adresseExpedition = declarationContribuableCommon.AdresseExpedition
			Me.texteLibre = declarationContribuableCommon.TexteLibre
		End Sub

		Public Shared Function ContribuableLR4FromInt(value As Integer) As EnumTypeContribuableLR4_Type
			Select Case value
				Case 8
					Return EnumTypeContribuableLR4_Type.Item8
				Case 9
					Return EnumTypeContribuableLR4_Type.Item9
				Case Else
					Return EnumTypeContribuableLR4_Type.Item8
			End Select
		End Function

	End Class

	Partial Public Class DeclarationContribuableType5_Type2014
		Public Sub New()
			MyBase.New()
		End Sub

		Public Sub New(declarationContribuableCommon As DeclarationContribuableCommon_Type)
			Me.New()
			Me.typeContribuable = ContribuableLR5FromInt(declarationContribuableCommon.TypeContribuable)
			Me.infoContribuable = declarationContribuableCommon.InfoContribuable
			Me.infoArtiste = Nothing ' TODO
			Me.paysArtiste = Nothing ' TODO
			Me.assujettissementContribuable = declarationContribuableCommon.AssujettissementContribuable.FirstOrDefault()
			Me.assujettissementArtiste = Nothing ' TODO
			Me.retenuePrestationsImpots = CType(declarationContribuableCommon.RetenuePrestationsImpots, RetenueType5_Type2014)
			Me.adresseExpedition = declarationContribuableCommon.AdresseExpedition
			Me.texteLibre = declarationContribuableCommon.TexteLibre
		End Sub

		Public Shared Function ContribuableLR5FromInt(value As Integer) As EnumTypeContribuableLR5_Type
			Select Case value
				Case 10
					Return EnumTypeContribuableLR5_Type.Item10
				Case Else
					Return EnumTypeContribuableLR5_Type.Item10
			End Select
		End Function

	End Class

	Partial Public Class DeclarationContribuableType6_Type2014
		Public Sub New()
			MyBase.New()
		End Sub

		Public Sub New(declarationContribuableCommon As DeclarationContribuableCommon_Type)
			Me.New()
			Me.typeContribuable = ContribuableLR6FromInt(declarationContribuableCommon.TypeContribuable)
			Me.infoContribuable = declarationContribuableCommon.InfoContribuable
			Me.adresseDomicile = declarationContribuableCommon.AdresseDomicile
			Me.retenuePrestationsImpots = CType(declarationContribuableCommon.RetenuePrestationsImpots, RetenueType6_Type2014)
			Me.adresseExpedition = declarationContribuableCommon.AdresseExpedition
			Me.texteLibre = declarationContribuableCommon.TexteLibre
		End Sub

		Public Shared Function ContribuableLR6FromInt(value As Integer) As EnumTypeContribuableLR6_Type
			Select Case value
				Case 11
					Return EnumTypeContribuableLR6_Type.Item11
				Case Else
					Return EnumTypeContribuableLR6_Type.Item11
			End Select
		End Function

	End Class

End Namespace

