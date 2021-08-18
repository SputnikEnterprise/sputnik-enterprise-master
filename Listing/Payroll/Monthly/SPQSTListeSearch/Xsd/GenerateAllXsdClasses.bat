@echo off
@echo Check out files first (readonly files are not overwritten)
@echo.
"..\libs\xsd.exe" -nologo -classes -l:vb -namespace:Xsd.Ge QstExportGe.xsd ISEL_ListeRecapitulative_1_03.xsd
pause