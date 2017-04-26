// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml;
using System.Collections;

namespace System.Security.Cryptography.Xml
{
    // the stack of currently active NamespaceFrame contexts. this
    // object also maintains the inclusive prefix list in a tokenized form.
    internal class C14NAncestralNamespaceContextManager
    {
        internal ArrayList _ancestorStack = new ArrayList();

        internal NamespaceFrame GetScopeAt(int i)
        {
            return (NamespaceFrame)_ancestorStack[i];
        }

        internal NamespaceFrame GetCurrentScope()
        {
            return GetScopeAt(_ancestorStack.Count - 1);
        }

        protected XmlAttribute GetNearestRenderedNamespaceWithMatchingPrefix(string nsPrefix, out int depth)
        {
            XmlAttribute attr = null;
            depth = -1;
            for (int i = _ancestorStack.Count - 1; i >= 0; i--)
            {
                if ((attr = GetScopeAt(i).GetRendered(nsPrefix)) != null)
                {
                    depth = i;
                    return attr;
                }
            }
            return null;
        }

        protected XmlAttribute GetNearestUnrenderedNamespaceWithMatchingPrefix(string nsPrefix, out int depth)
        {
            XmlAttribute attr = null;
            depth = -1;
            for (int i = _ancestorStack.Count - 1; i >= 0; i--)
            {
                if ((attr = GetScopeAt(i).GetUnrendered(nsPrefix)) != null)
                {
                    depth = i;
                    return attr;
                }
            }
            return null;
        }

        internal void EnterElementContext()
        {
            _ancestorStack.Add(new NamespaceFrame());
        }

        internal void ExitElementContext()
        {
            _ancestorStack.RemoveAt(_ancestorStack.Count - 1);
        }

        internal void LoadUnrenderedNamespaces(Hashtable nsLocallyDeclared)
        {
            object[] attrs = new object[nsLocallyDeclared.Count];
            nsLocallyDeclared.Values.CopyTo(attrs, 0);
            foreach (object attr in attrs)
            {
                AddUnrendered((XmlAttribute)attr);
            }
        }

        internal void LoadRenderedNamespaces(SortedList nsRenderedList)
        {
            foreach (object attr in nsRenderedList.GetKeyList())
            {
                AddRendered((XmlAttribute)attr);
            }
        }

        internal void AddRendered(XmlAttribute attr)
        {
            GetCurrentScope().AddRendered(attr);
        }

        internal void AddUnrendered(XmlAttribute attr)
        {
            GetCurrentScope().AddUnrendered(attr);
        }

        private void GetNamespaceToRender(string nsPrefix, SortedList attrListToRender, SortedList nsListToRender, Hashtable nsLocallyDeclared)
        {
            foreach (object a in nsListToRender.GetKeyList())
            {
                if (Utils.HasNamespacePrefix((XmlAttribute)a, nsPrefix))
                    return;
            }
            foreach (object a in attrListToRender.GetKeyList())
            {
                if (((XmlAttribute)a).LocalName.Equals(nsPrefix))
                    return;
            }

            int rDepth;
            XmlAttribute local = (XmlAttribute)nsLocallyDeclared[nsPrefix];
            XmlAttribute rAncestral = GetNearestRenderedNamespaceWithMatchingPrefix(nsPrefix, out rDepth);
            if (local != null)
            {
                if (Utils.IsNonRedundantNamespaceDecl(local, rAncestral))
                {
                    nsLocallyDeclared.Remove(nsPrefix);
                    if (Utils.IsXmlNamespaceNode(local))
                        attrListToRender.Add(local, null);
                    else
                        nsListToRender.Add(local, null);
                }
            }
            else
            {
                int uDepth;
                XmlAttribute uAncestral = GetNearestUnrenderedNamespaceWithMatchingPrefix(nsPrefix, out uDepth);
                if (uAncestral != null && uDepth > rDepth && Utils.IsNonRedundantNamespaceDecl(uAncestral, rAncestral))
                {
                    if (Utils.IsXmlNamespaceNode(uAncestral))
                        attrListToRender.Add(uAncestral, null);
                    else
                        nsListToRender.Add(uAncestral, null);
                }
            }
        }

        internal void GetNamespacesToRender(XmlElement element, SortedList attrListToRender, SortedList nsListToRender, Hashtable nsLocallyDeclared)
        {
            XmlAttribute attrib = null;
            object[] attrs = new object[nsLocallyDeclared.Count];
            nsLocallyDeclared.Values.CopyTo(attrs, 0);
            foreach (object a in attrs)
            {
                attrib = (XmlAttribute)a;
                int rDepth;
                XmlAttribute rAncestral = GetNearestRenderedNamespaceWithMatchingPrefix(Utils.GetNamespacePrefix(attrib), out rDepth);
                if (Utils.IsNonRedundantNamespaceDecl(attrib, rAncestral))
                {
                    nsLocallyDeclared.Remove(Utils.GetNamespacePrefix(attrib));
                    if (Utils.IsXmlNamespaceNode(attrib))
                        attrListToRender.Add(attrib, null);
                    else
                        nsListToRender.Add(attrib, null);
                }
            }

            for (int i = _ancestorStack.Count - 1; i >= 0; i--)
            {
                foreach (object a in GetScopeAt(i).GetUnrendered().Values)
                {
                    attrib = (XmlAttribute)a;
                    if (attrib != null)
                        GetNamespaceToRender(Utils.GetNamespacePrefix(attrib), attrListToRender, nsListToRender, nsLocallyDeclared);
                }
            }
        }

        internal void TrackNamespaceNode(XmlAttribute attr, SortedList nsListToRender, Hashtable nsLocallyDeclared)
        {
            nsLocallyDeclared.Add(Utils.GetNamespacePrefix(attr), attr);
        }

        internal void TrackXmlNamespaceNode(XmlAttribute attr, SortedList nsListToRender, SortedList attrListToRender, Hashtable nsLocallyDeclared)
        {
            nsLocallyDeclared.Add(Utils.GetNamespacePrefix(attr), attr);
        }
    }
}
