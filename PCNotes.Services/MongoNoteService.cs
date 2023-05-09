using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using PCNotes.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCNotes.Services
{
    public class MongoNoteService : INoteService
    {
        private readonly IMongoCollection<Note> Notes;

        public MongoNoteService(IConfiguration config)
        {
            var connectionstring = Environment.GetEnvironmentVariable("MONGO");

            var client = new MongoClient(connectionstring);

            Console.WriteLine(string.Join('-', client.ListDatabaseNames().ToList()));

            var database = client.GetDatabase("pcnotes");
            Notes = database.GetCollection<Note>("notes");
        }

        public Attachment AddAttachment(Guid noteId, Attachment attachment)
        {
            var note = GetNoteById(noteId);
            note.Attachments.Add(attachment);
            UpdateNote(note);
            return attachment;
        }

        public Note AddNote(Note note)
        {
            Notes.InsertOne(note);
            return note;
        }

        public bool DeleteAttachment(Guid noteId, Guid serverFileName)
        {
            var note = GetNoteById(noteId);

            var attachment = note.Attachments.Single(a => a.ServerFileName == serverFileName);

            note.Attachments.Remove(attachment);
            UpdateNote(note);
            return true;
        }

        public bool DeleteNote(Guid noteId)
        {
            var result = Notes.DeleteOne(n => n.NoteId == noteId);
            return result.DeletedCount == 1;
        }

        public Note GetNoteById(Guid noteId)
        {
            return Notes.Find(n => n.NoteId == noteId).First();
        }

        public List<Note> GetNotes()
        {
            return Notes.Find(new BsonDocument()).ToList();
        }

        public Note UpdateNote(Note updateNote)
        {
            Notes.ReplaceOne(n => n.NoteId == updateNote.NoteId, updateNote);
            return updateNote;
        }
    }
}
