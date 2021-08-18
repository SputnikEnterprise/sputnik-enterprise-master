Startup arguments that can be used for testing.

// Convert a pdf
ProcessStarter CONVERT_PDF <pdfPath> <mdguid>
e.g. ProcessStarter CONVERT_PDF C:\Users\Technik\Desktop\test.pdf A0BB18D4-84EB-42d7-B366-721ED0E296EC


// Analyze a pdf in order to find a DataMatrix code
ProcessStarter ANALYZE_PDF <pdfPath>
e.g. ProcessStarter ANALYZE_PDF C:\Users\Technik\Desktop\test.pdf


// Start the file system watcher.
ProcessStarter START_FILESYSTEMWATCHER