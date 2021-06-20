using JsonFlatFileDataStore;
using PCNotes.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCNotes.Services
{
    public class FlatNoteService : INoteService, IDisposable
    {
        private DataStore DataStore;
        private IDocumentCollection<Note> Notes;

        public FlatNoteService()
        {
            DataStore = new DataStore("notes.json");
            Notes = DataStore.GetCollection<Note>();
        }

        public List<Note> GetNotes()
        {
            return Notes.AsQueryable().ToList();
        }

        public Note AddNote(Note note)
        {
            Notes.InsertOne(note);
            return note;
        }

        public Note GetNoteById(Guid noteId)
        {
            return Notes.AsQueryable().SingleOrDefault(n => n.NoteId == noteId);
        }

        public Note UpdateNote(Note updateNote)
        {
            DeleteNote(updateNote.NoteId);
            updateNote.Attachments.ForEach(a => a.FileContent = null);
            AddNote(updateNote);
            return updateNote;
        }

        public bool DeleteNote(Guid noteId)
        {
            Notes.DeleteOne(n => n.NoteId == noteId);
            return true;
        }

        public Attachment AddAttachment(Guid noteId, Attachment attachment)
        {
            var note = GetNoteById(noteId);

            note.Attachments.Add(attachment);

            UpdateNote(note);
            return attachment;
        }

        public bool DeleteAttachment(Guid noteId, Guid serverFileName)
        {
            var note = GetNoteById(noteId);
            var attachment = note.Attachments.Single(a => a.ServerFileName == serverFileName);

            note.Attachments.Remove(attachment);

            UpdateNote(note);
            return true;
        }

        public void Dispose()
        {
            DataStore.Dispose();
        }
    }
}
