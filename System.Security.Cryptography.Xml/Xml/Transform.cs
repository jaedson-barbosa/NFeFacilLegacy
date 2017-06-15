// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This file contains the classes necessary to represent the Transform processing model used in 
// XMLDSIG. The basic idea is as follows. A Reference object contains within it a TransformChain, which
// is an ordered set of XMLDSIG transforms (represented by <Transform>...</Transform> clauses in the XML).
// A transform in XMLDSIG operates on an input of either an octet stream or a node set and produces
// either an octet stream or a node set. Conversion between the two types is performed by parsing (octet stream->
// node set) or C14N (node set->octet stream). We generalize this slightly to allow a transform to define an array of
// input and output types (because I believe in the future there will be perf gains by being smarter about what goes in & comes out)
// Each XMLDSIG transform is represented by a subclass of the abstract Transform class. We need to use CryptoConfig to
// associate Transform classes with URLs for transform extensibility, but that's a future concern for this code.
// Once the Transform chain is constructed, call TransformToOctetStream to convert some sort of input type to an octet
// stream. (We only bother implementing that much now since every use of transform chains in XmlDsig ultimately yields something to hash).

using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public abstract class Transform
    {
        private string _algorithm;

        //
        // public properties
        //

        public string Algorithm
        {
            get { return _algorithm; }
            set { _algorithm = value; }
        }

        //
        // public methods
        //

        internal XmlElement GetXml(XmlDocument document)
        {
            return GetXml(document, "Transform");
        }

        internal XmlElement GetXml(XmlDocument document, string name)
        {
            XmlElement transformElement = document.CreateElement(name, SignedXml.XmlDsigNamespaceUrl);
            if (!string.IsNullOrEmpty(Algorithm))
                transformElement.SetAttribute("Algorithm", Algorithm);
            XmlNodeList children = GetInnerXml();
            if (children != null)
            {
                foreach (XmlNode node in children)
                {
                    transformElement.AppendChild(document.ImportNode(node, true));
                }
            }
            return transformElement;
        }

        protected abstract XmlNodeList GetInnerXml();

        public abstract void LoadInput(XmlDocument obj);

        public abstract object GetOutput();

        public abstract byte[] GetDigestedOutput(HashAlgorithm hash);
    }
}
