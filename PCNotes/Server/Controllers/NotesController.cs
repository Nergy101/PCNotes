using Microsoft.AspNetCore.Mvc;
using PCNotes.Services;
using PCNotes.Shared;
using System;
using System.Collections.Generic;
using System.IO;
//using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCNotes.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController
    {
        public static string UploadDir = "Uploads";

        public INoteService NoteService { get; set; }

        public NotesController(INoteService noteService)
        {
            NoteService = noteService;
        }

        [HttpGet]
        public IEnumerable<Note> GetAllNotes()
        {
            return NoteService.GetNotes();
        }

        [HttpGet("{noteId}")]
        public Note GetNote(Guid noteId)
        {

            return NoteService.GetNotes().SingleOrDefault(n => n.NoteId == noteId);
        }

        [HttpGet("search")]
        public IEnumerable<Note> GetNotesBySearch(string searchterm)
        {
            if (searchterm == null) { return GetAllNotes(); }
            searchterm = searchterm.ToLowerInvariant();
            return NoteService.GetNotes().Where(n =>
                n.Content.ToLowerInvariant().Contains(searchterm) ||
                n.Title.ToLowerInvariant().Contains(searchterm) ||
                n.Creator.ToLowerInvariant().Contains(searchterm) ||
                n.NoteId.ToString().ToLowerInvariant().Contains(searchterm) ||
                n.CreationTime.ToString("dd-MM-yyyy").Contains(searchterm) ||
                n.CheckList.Items.Any(i => i.Content.ToLowerInvariant().Contains(searchterm))
            ).ToList();
        }

        [HttpPost]
        public Note AddNote([FromBody] Note note)
        {
            NoteService.AddNote(note);
            return note;
        }

        [HttpDelete]
        public bool DeleteNote(Guid noteId)
        {
            try
            {
                NoteService.DeleteNote(noteId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPut]
        public Note UpdateNote([FromBody] Note updateNote)
        {
            return NoteService.UpdateNote(updateNote);
        }

        [HttpPost("{noteId}/attachments")]
        public async Task<ActionResult> AddAttachment([FromRoute] Guid noteId, [FromBody] Attachment attachment)
        {
            var filePath = GetUploadPath(attachment.ServerFullName);

            using (var stream = File.Create(filePath))
            {
                using var ms = new MemoryStream(attachment.FileContent);
                await ms.CopyToAsync(stream).ConfigureAwait(false);
            }

            NoteService.AddAttachment(noteId, attachment);
            return new OkResult();
        }

        [HttpDelete("{noteId}/attachments/{serverFileName}")]
        public async Task<ActionResult> DeleteAttachment(Guid noteId, Guid serverFileName)
        {
            var note = NoteService.GetNoteById(noteId);
            var attachment = note.Attachments.Single(a => a.ServerFileName == serverFileName);

            NoteService.DeleteAttachment(noteId, serverFileName);

            var filePath = GetUploadPath(attachment.ServerFullName);
            File.Delete(filePath);

            return new OkResult();
        }

        [HttpGet("{noteId}/attachments/{serverFileName}"), DisableRequestSizeLimit]
        public async Task<ActionResult> Download(Guid noteId, Guid serverFileName)
        {
            var note = NoteService.GetNoteById(noteId);
            var attachment = note.Attachments.Single(a => a.ServerFileName == serverFileName);

            var fileName = attachment.OriginalFileName;
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(fileName);
            var filePath = GetUploadPath(attachment.ServerFullName);

            byte[] bytes = File.ReadAllBytes(filePath);

            var ms = new MemoryStream(bytes);

            return new FileStreamResult(ms, mimeType)
            {
                FileDownloadName = fileName
            };
        }

        private static string GetUploadPath(string attachmentServerFullName)
        {
            Directory.CreateDirectory(UploadDir);
            return Path.Combine(UploadDir, attachmentServerFullName);
        }
    }
}
