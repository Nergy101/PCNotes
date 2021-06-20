using MongoDB.Bson.Serialization.Attributes;
using System;
using System.IO;

namespace PCNotes.Shared
{
    public class Attachment
    {
        public string OriginalFileName { get; set; }
        public Guid ServerFileName { get; set; }

        public string ServerFileFolder { get; set; }
        public string UploadedBy { get; set; }

        [BsonIgnore]
        public byte[] FileContent { get; set; }

        public string ServerFullName => ServerFileName + "---" + OriginalFileName;

    }
}
