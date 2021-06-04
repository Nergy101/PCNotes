using System;

namespace PCNotes.Shared
{
    public class Attachment
    {
        public string OriginalFileName { get; set; }
        public Guid ServerFileName { get; set; }

        public string ServerFileFolder { get; set; }
        public string UploadedBy { get; set; }
        public byte[] FileContent { get; set; }

    }
}
