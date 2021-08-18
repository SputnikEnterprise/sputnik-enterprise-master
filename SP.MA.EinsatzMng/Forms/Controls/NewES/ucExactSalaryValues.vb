Namespace UI

  Public Class ucExactSalaryValues

#Region "Public Methods"

    ''' <summary>
    ''' Sets the values.
    ''' </summary>
    ''' <param name="exactValues">The values.</param>
    Public Sub SetValues(ByVal exactValues As ExaclSalaryValues)

      txtGrundlohn.EditValue = exactValues.GrundLohn

      txtBasisFeiertag.EditValue = exactValues.FeiertagBasis
      txtAnsatzFeiertag.EditValue = exactValues.FeiertagAnsatz
      txtBetragFeiertag.EditValue = exactValues.FeiertagBetrag

      txtBasisFerien.EditValue = exactValues.FerienBasis
      txtAnsatzFerien.EditValue = exactValues.FerienAnsatz
      txtBetragFerien.EditValue = exactValues.FerienBetrag

      txtBasisLohn13.EditValue = exactValues.Lohn13Basis
      txtAnsatzLohn13.EditValue = exactValues.Lohn13Ansatz
      txtBetragLohn13.EditValue = exactValues.Lohn13Betrag

      txtStundenLohn.EditValue = exactValues.StundenLohn
      txtLohnspesen.EditValue = exactValues.Lohnspesen
      txtTagesspesenMA.EditValue = exactValues.Tagespesen

    End Sub

    ''' <summary>
    ''' Translate controls.
    ''' </summary>
    ''' <param name="translate">The translation helper.</param>
    Public Sub TranslateControls(ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper)

			Me.lblGrundlohn.Text = translate.GetSafeTranslationValue(Me.lblGrundlohn.Text)
			Me.lblFeiertag.Text = translate.GetSafeTranslationValue(Me.lblFeiertag.Text)
      Me.lblFerien.Text = translate.GetSafeTranslationValue(Me.lblFerien.Text)
      Me.lblLohn13.Text = translate.GetSafeTranslationValue(Me.lblLohn13.Text)
      Me.lblStundenlohn.Text = translate.GetSafeTranslationValue(Me.lblStundenlohn.Text)
			Me.lblLohnspesen.Text = translate.GetSafeTranslationValue(Me.lblLohnspesen.Text, True)
			Me.lblTagespesen.Text = translate.GetSafeTranslationValue(Me.lblTagespesen.Text, True)

    End Sub

#End Region

#Region "Helper Classes"

    Public Class ExaclSalaryValues

      Public Property GrundLohn As Decimal

      Public Property FeiertagBasis As Decimal
      Public Property FeiertagAnsatz As Decimal
      Public Property FeiertagBetrag As Decimal

      Public Property FerienBasis As Decimal
      Public Property FerienAnsatz As Decimal
      Public Property FerienBetrag As Decimal

      Public Property Lohn13Basis As Decimal
      Public Property Lohn13Ansatz As Decimal
      Public Property Lohn13Betrag As Decimal

      Public Property StundenLohn As Decimal
      Public Property Lohnspesen As Decimal
      Public Property Tagespesen As Decimal

    End Class

#End Region

  End Class

End Namespace