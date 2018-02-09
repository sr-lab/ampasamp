using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampasamp
{
    class SearchTreeNode
    {
        private List<SearchTreeNode> children;

        public char Character { get; private set; }

        public SearchTreeNode(char character)
        {
            Character = character;
            children = new List<SearchTreeNode>();
        }

        public SearchTreeNode() : this('\0') { }

        public void AddChild(SearchTreeNode child)
        {
            children.Add(child);
        }

        public void AddString(string str)
        {
            if (str == "")
            {
                children.Add(new SearchTreeNode('\0'));
            }
            else
            {
                var child = children.FirstOrDefault(x => x.Character == str[0]);
                if (child == null) {
                    child = new SearchTreeNode(str[0]);
                    children.Add(child);
                }
                child.AddString(str.Substring(1));
            }
        }
        
        public bool Contains(string search)
        {
            return search == "" ? children.Any(x => x.Character == '\0')
                : children.Any(x => x.Character == search[0] && x.Contains(search.Substring(1)));
        }
    }
}
