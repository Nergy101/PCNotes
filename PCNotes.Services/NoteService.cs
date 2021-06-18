using JsonFlatFileDataStore;
using PCNotes.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCNotes.Services
{
    public class NoteService : IDisposable
    {
        private DataStore DataStore;
        private IDocumentCollection<Note> Notes;

        public NoteService()
        {
            DataStore = new DataStore("notes.json");
            Notes = DataStore.GetCollection<Note>();
        }

        public List<Note> GetNotes()
        {
            return Notes.AsQueryable().ToList();
        }

        public void AddNote(Note note)
        {
            Notes.InsertOne(note);
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

        public void DeleteNote(Guid noteId)
        {
            Notes.DeleteOne(note => note.NoteId == noteId);
        }

        public void AddAttachment(Guid noteId, Attachment attachment)
        {
            var note = GetNoteById(noteId);

            note.Attachments.Add(attachment);

            UpdateNote(note);
        }

        public void DeleteAttachment(Guid noteId, Guid serverFileName)
        {
            var note = GetNoteById(noteId);
            var attachment = note.Attachments.Single(a => a.ServerFileName == serverFileName);

            note.Attachments.Remove(attachment);

            UpdateNote(note);
        }

        public void Dispose()
        {
            DataStore.Dispose();
        }
    }
}
