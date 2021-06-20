using PCNotes.Shared;
using System;
using System.Collections.Generic;

namespace PCNotes.Services
{
    public interface INoteService
    {
        List<Note> GetNotes();
        Note GetNoteById(Guid noteId);

        Note AddNote(Note note);
        Note UpdateNote(Note updateNote);
        bool DeleteNote(Guid noteId);

        Attachment AddAttachment(Guid noteId, Attachment attachment);
        bool DeleteAttachment(Guid noteId, Guid serverFileName);
    }
}
