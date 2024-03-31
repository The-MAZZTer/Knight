Unicode true
SetCompressor /SOLID lzma
SetCompressorDictSize 32
SetDatablockOptimize On
RequestExecutionLevel highest

!define NAME "Knight"
!define VERSION "1.0.0"
!define DESC "A mod and patch manager for the Dark Forces/Jedi Knight series of games."
!define AUTHOR "The MAZZTer"
!define WEBSITE "https://github.com/The-MAZZTer/Knight"

!define SOURCEDIR ".."
!define ICON "${SOURCEDIR}\${NAME}\Resources\${NAME}.ico"
!define BINARIES "${SOURCEDIR}\${NAME}\bin\Release\net8.0-windows\publish\win-x64\"

VIAddVersionKey "FileDescription" "${NAME}"
ViAddVersionKey "Comments" "${DESC}"
VIAddVersionKey "CompanyName" "${AUTHOR}"
ViAddVersionKey "LegalCopyright" ""

VIProductVersion "${VERSION}.0"
ViAddVersionKey "ProductVersion" "${VERSION}"
ViAddVersionKey "FileVersion" "${VERSION}"

OutFile "..\${NAME} ${VERSION}.exe"
Name "${NAME}"
BrandingText "${NAME}"
InstallDir "$PROGRAMFILES64\${NAME}"

!include "LogicLib.nsh"
!include "FileFunc.nsh"
!include "WordFunc.nsh"
!include "MUI2.nsh"

Icon "${ICON}"
!define MUI_ICON "${ICON}"
#!define MUI_UNICON "${ICON}"

Var UPDATE
Var StartMenuFolder

!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING

!insertmacro MUI_PAGE_WELCOME
!define MUI_DIRECTORYPAGE_VERIFYONLEAVE
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU "Application" $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN "$INSTDIR\${NAME}.exe"
!insertmacro MUI_PAGE_FINISH

!include "locale\en.nsh"

Function .onInit
	ClearErrors
	${GetOptions} $CMDLINE "/update=" $0
	${IfNot} ${Errors}
		StrCpy $UPDATE "True"
		StrCpy $INSTDIR $0
		SetSilent silent

		ClearErrors
		${Do}
			Sleep 500

			nsExec::ExecToStack "tasklist /FO CSV /NH /FI $\"IMAGENAME eq ${NAME}.exe$\"" 
			Pop $R0
			Pop $R0
			${WordFind} "$R0" "," "E/$\"${NAME}.exe$\"" $R0
		${LoopUntil} ${Errors}
	${Else}
		ClearErrors
		StrCpy $UPDATE "False"
	${EndIf}
FunctionEnd

Section "-${NAME}" SecMain
	DetailPrint "$(DETAIL_INSTALLING)"

	SetOutPath "$INSTDIR"
	File "${BINARIES}\${NAME}.exe"
	File "${BINARIES}\*.dll"

	!insertmacro MUI_STARTMENU_WRITE_BEGIN "Application"
	  CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
		CreateShortcut "$SMPROGRAMS\$StartMenuFolder\${NAME}.lnk" "$INSTDIR\${NAME}.exe"
	!insertmacro MUI_STARTMENU_WRITE_END

	WriteUninstaller "Uninstall ${NAME}.exe"

	${If} $UPDATE == "True"
		ClearErrors
		${GetOptions} $CMDLINE "/restart" $0
		${IfNot} ${Errors}
			Exec "$INSTDIR\${NAME}.exe"
		${EndIf}
	${EndIf}
SectionEnd

Section "un.${NAME}"
	!insertmacro MUI_STARTMENU_GETFOLDER "Application" $R0
  Delete "$SMPROGRAMS\$R0\${Name}.lnk"
	RMDir "$SMPROGRAMS\$R0"

	Delete "$INSTDIR\${NAME}.exe"
	Delete "$INSTDIR\*.dll"
  RMDir "$INSTDIR"
SectionEnd
