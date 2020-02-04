# FixMediaDate
Small console utility to restore EXIF and file date/time from file name

Sometimes incorrectly written apps and services can loose EXIF information from your photos and/or change file creation time.
For example, current (02/03/2020) WhatsApp restore action (for example, after Android phone reset) can completely mess up your Google Photos/Gallery - all restored photos and videos will be placed on top in the Google Photos app because EXIF information is removed from jpegs, and all file dates will be set to the date of restoration.

I wrote this small utility to fix that mess, and restore correct media date & time from the file names (fortunately, Android and former Windows phone OSes include date and time information to the file names, even by different and non-consistant way)

This app is pretty simple (I'll show below how to fix WhatsApp mess): 
- copy restored WhatsApp media folders to PC (you may copy whole WhatsApp folder, or sub-folders **WhatsApp Images** & **WhatsApp Video**)
- download release of this utility, and place it to WhatsApp main folder
- run the program: it will process all files in subfolders recursively
- copy media folders (i.e. **WhatsApp Images** & **WhatsApp Video**) with the fixed files back to Android phone
- enjoy
As an extra step, you may need to uninstall "Photos" app (or clear app's data) first, to get your pictures perfectly organized by dates.

But, of course, you should clearly understood what are you doing! Source code is pretty clear an simple; please take a look before using.
Please, note: current code is expecting US date and time format. To get it works with your locale (if differ), please fix the code and issue PR.

**I'M PERSONALLY AND THE APP SHALL HAVE NO RESPONSIBILITY FOR ANY DAMAGE TO YOUR PHONE OR TABLET OR LOSS OF DATA THAT RESULTS FROM YOUR USE OF THE APP OR ITS CONTENT.**
