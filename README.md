# Neatify
A C# File Sorting Utility that automatically organizes your media and document files by type and year, detects duplicates using SHA-256 hashing, and maintains a detailed log file for all actions.

📋 Features

/- Automatic Categorization

Sorts files into structured folders such as:

🎵 Music (mp3, flac, wav, etc.)

🎬 Videos (mp4, mkv, avi, etc.)

🖼️ Images (jpg, png, gif, etc.)

📄 Documents (pdf, docx, txt, etc.)

🗜️ Archives (zip, rar, etc.)

🧩 Code / Scripts (cs, py, html, bat, etc.)

And more...

/- Duplicate Detection
Compares files using SHA-256 hash and removes exact duplicates automatically.

/- Date-Based Sorting
Creates subfolders for each year based on file creation date.

/- Conflict Handling
If a file with the same name exists, it appends a version number (e.g., photo_v1.jpg).

/- Detailed Logging
All operations (created folders, moved files, errors, duplicates) are recorded in:

MediaOrganizer.log

/- Statistics Summary
Displays a summary at the end:

Moved: 123
Duplicates: 4
Errors: 2
Skipped: 0

⚙️ How It Works

The program asks for the source folder that contains your unsorted media files.

It creates a new folder named:

Media-Organized

inside your source directory.

Each file is analyzed, categorized, and moved to the appropriate subfolder:

Media-Organized/
├── Music/
│ ├── 2023/
│ └── 2024/
├── Videos/
│ ├── 2021/
│ └── 2025/
├── Documents/
│ ├── 2022/
│ └── 2023/
└── Other/

Duplicate files are deleted and logged.

🚀 How to Run

Locate the compiled file:

MediaOrganizer.exe

(It’s usually inside the bin/Release/net6.0/ or bin/Debug/net6.0/ folder.)

Copy the executable to any folder — for example, your Desktop or Downloads.

Double-click the .exe file, or run it from Command Prompt:

MediaOrganizer.exe

When prompted, enter the full path to the folder you want to organize:

Please enter a valid source folder for your media: C:\Users\John\Downloads

The program will:

Create a new folder named Media-Organized inside the source folder

Sort all files by type and year

Delete duplicates

Write all operations to MediaOrganizer.log

When finished, you’ll see a summary like:

Moved : 124
Duplicates : 3
Errors : 0
Skipped : 0

🪪 Logging Example

Example from MediaOrganizer.log:

[2025-11-07 18:32:14] Created target folder: C:\Users\Name\Downloads\Media-Organized
[2025-11-07 18:32:15] Created folder: C:\Users\Name\Downloads\Media-Organized\Music\2024
[2025-11-07 18:32:16] Moved: song.mp3 -> ...\Music\2024\song.mp3
[2025-11-07 18:32:17] Duplicate removed: copy_of_song.mp3 (duplicate of song.mp3)
[2025-11-07 18:32:18] === Sorting completed ===

⚠️ Notes

Do not place the program inside the same folder you are organizing.

Deleted duplicates cannot be recovered — backup important data before running.

The program ignores system and hidden files.
