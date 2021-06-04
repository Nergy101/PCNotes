using System.Collections.Generic;
using System.Linq;

namespace PCNotes.Shared
{
    public class CheckList
    {
        public List<CheckListItem> Items { get; set; } = new List<CheckListItem>();
        public int TotalItemCount => Items.Count;
        public int CheckedItemCount => Items.Count(i => i.Checked);

        public double GetCheckListProgress()
        {
            return (Items.Count(item => item.Checked)) / ((double)Items.Count);
        }
    }

    public class CheckListItem
    {
        public int Index { get; set; }
        public string Content { get; set; }
        public bool Checked { get; set; }
    }
}