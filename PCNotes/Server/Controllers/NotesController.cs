using Microsoft.AspNetCore.Mvc;
using PCNotes.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCNotes.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController
    {
        public List<Note> Notes { get; set; }
        public NotesController()
        {
            Notes = new List<Note> {
            new Note {
                NoteId = new Guid("6798895e-ec91-475b-a465-00f2e6b79719"),
                Content = "Een uur geen Avril, is een uur niet geleefd",
                Creator = "Christian",
                Title = "Feitje",
                CreationTime = DateTime.Now.AddDays(-2),
                CheckList = new CheckList {
                    Items = new List<CheckListItem> {
                        new CheckListItem { Index = 0, Content = "Avril Luisteren", Checked=true },
                        new CheckListItem { Index = 1, Content = "Metal Luisteren", Checked=false }
                    }
                }
            },
             new Note
             {
                 Content = "Boodschap",
                 Creator = "Noah",
                 Title = "Test Title",
                 CreationTime = DateTime.Now.AddDays(-1),
                 CheckList = new CheckList { Items = new List<CheckListItem>() }
             },
              new Note
              {
                  Content = "6x sensor, 3x arduino, 1x brein",
                  Creator = "Peter",
                  Title = "Ding bouwen",
                  CreationTime = DateTime.Now.AddMonths(-1).AddDays(-3),
                  CheckList = new CheckList
                  {
                      Items = new List<CheckListItem> {
                        new CheckListItem { Index = 0, Content = "6x sensor", Checked=true },
                        new CheckListItem { Index = 1, Content = "3x arduino", Checked=false },
                        new CheckListItem { Index = 2, Content = "1x brein", Checked=false },
                        new CheckListItem { Index = 2, Content = "20x schroef", Checked=false }
                    }
                  }
              }
            };
        }

        [HttpGet]
        public IEnumerable<Note> GetAllNotes()
        {
            return Notes;
        }

        [HttpGet("{noteId}")]
        public Note GetNote(Guid noteId)
        {

            return Notes.SingleOrDefault(n => n.NoteId == noteId);
        }

        [HttpGet("search")]
        public IEnumerable<Note> GetNotesBySearch(string searchterm)
        {
            if (searchterm == null) { return GetAllNotes(); }
            searchterm = searchterm.ToLowerInvariant();
            return Notes.Where(n =>
                n.Content.ToLowerInvariant().Contains(searchterm) ||
                n.Title.ToLowerInvariant().Contains(searchterm) ||
                n.Creator.ToLowerInvariant().Contains(searchterm) ||
                n.NoteId.ToString().ToLowerInvariant().Contains(searchterm) ||
                n.CreationTime.ToString("dd-MM-yyyy").Contains(searchterm)||
                n.CheckList.Items.Any(i => i.Content.ToLowerInvariant().Contains(searchterm)) 
            ).ToList();
        }

        [HttpPost]
        public Note AddNote([FromBody] Note note)
        {
            Notes.Add(note);
            return note;
        }

        [HttpDelete]
        public bool DeleteNote(Guid noteId)
        {
            return Notes.Remove(Notes.SingleOrDefault(n => n.NoteId == noteId));
        }

        [HttpPut]
        public Note UpdateNote([FromBody] Note updateNote)
        {
            var noteToUpdate = Notes.Single(n => n.NoteId == updateNote.NoteId);
            var indexOfNoteToUpdate = Notes.IndexOf(noteToUpdate);

            Notes.Remove(noteToUpdate);
            Notes.Insert(indexOfNoteToUpdate, updateNote);

            return updateNote;
        }
    }
}
