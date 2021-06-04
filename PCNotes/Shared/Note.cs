using System;
using System.Collections.Generic;

namespace PCNotes.Shared
{
    public class Note
    {
        public Guid NoteId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Content { get; set; }
        public string Creator { get; set; }
        public bool Favorited { get; set; }
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
        public CheckList CheckList { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
