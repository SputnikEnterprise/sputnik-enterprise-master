

Public Class SplashScreen2

  Private m_Translate As TranslateValues


  Sub New()

    InitializeComponent()
    m_Translate = New TranslateValues

    Me.pictureEdit2.Image = My.Resources.Sputnik_SES_Logo_Symbol_OrangeReflection_5_4

    Me.labelControl2.Text = m_Translate.GetSafeTranslationValue("Die Module werden gestartet. Bitte warten Sie einen Augenblick") & "..."
		Me.labelControl1.Text = String.Format("Copyright © 2004-{0}", Now.Year)

  End Sub

  Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)
    MyBase.ProcessCommand(cmd, arg)
  End Sub

  Public Enum SplashScreenCommand
    SomeCommandId
  End Enum
End Class
