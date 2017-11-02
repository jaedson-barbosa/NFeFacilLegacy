// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace ServidorCertificacao.CodigoNet
{
    public class Reference
    {
        private string _uri;
        string tag;
        private TransformChain _transformChain;
        private string _digestMethod;
        private byte[] _digestValue;
        private HashAlgorithm _hashAlgorithm;
        private SignedXml _signedXml = null;
        internal CanonicalXmlNodeList _namespaces = null;

        //
        // public constructors
        //

        public Reference(string uri, string tagASerAplicada, SignedXml main)
        {
            _transformChain = new TransformChain();
            _uri = uri;
            tag = tagASerAplicada;
            _digestMethod = SignedXml.XmlDsigSHA1Url;
            _signedXml = main;
        }

        //
        // public properties
        //

        public string Uri
        {
            get { return _uri; }
            set
            {
                _uri = value;
            }
        }

        public string DigestMethod
        {
            get { return _digestMethod; }
            set
            {
                _digestMethod = value;
            }
        }

        public byte[] DigestValue
        {
            get { return _digestValue; }
            set
            {
                _digestValue = value;
            }
        }

        public TransformChain TransformChain
        {
            get
            {
                if (_transformChain == null)
                    _transformChain = new TransformChain();
                return _transformChain;
            }
            set
            {
                _transformChain = value;
            }
        }

        internal SignedXml SignedXml
        {
            get { return _signedXml; }
            set { _signedXml = value; }
        }

        //
        // public methods
        //

        internal XmlElement GetXml(XmlDocument document)
        {
            // Create the Reference
            XmlElement referenceElement = document.CreateElement("Reference", SignedXml.XmlDsigNamespaceUrl);

            if (_uri != null)
                referenceElement.SetAttribute("URI", _uri);

            // Add the transforms to the Reference
            if (TransformChain.Count != 0)
                referenceElement.AppendChild(TransformChain.GetXml(document, SignedXml.XmlDsigNamespaceUrl));

            // Add the DigestMethod
            XmlElement digestMethodElement = document.CreateElement("DigestMethod", SignedXml.XmlDsigNamespaceUrl);
            digestMethodElement.SetAttribute("Algorithm", _digestMethod);
            referenceElement.AppendChild(digestMethodElement);

            XmlElement digestValueElement = document.CreateElement("DigestValue", SignedXml.XmlDsigNamespaceUrl);
            digestValueElement.AppendChild(document.CreateTextNode(Convert.ToBase64String(_digestValue)));
            referenceElement.AppendChild(digestValueElement);

            return referenceElement;
        }

        public void AddTransform(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            TransformChain.Add(transform);
        }

        // What we want to do is pump the input throug the TransformChain and then 
        // hash the output of the chain document is the document context for resolving relative references
        internal void UpdateHashValue(XmlDocument document, CanonicalXmlNodeList refList)
        {
            // refList is a list of elements that might be targets of references
            // Now's the time to create our hashing algorithm
            _hashAlgorithm = SHA1.Create();

            // Let's go get the target.
            string baseUri = document.BaseURI;
            Stream hashInputStream = null;
            byte[] hashval = null;

            try
            {
                // Second-easiest case -- dereference the URI & pump through the TransformChain
                // handle the special cases where the URI is null (meaning whole doc)
                // or the URI is just a fragment (meaning a reference to an embedded Object)
                if (_uri[0] == '#')
                {
                    // If we get here, then we are constructing a Reference to an embedded DataObject
                    // referenced by an Id = attribute. Go find the relevant object
                    bool discardComments = true;
                    string idref = Utils.GetIdFromLocalUri(_uri, out discardComments);
                    XmlElement elem = document.GetElementsByTagName(tag)[0] as XmlElement;
                    if (elem != null)
                        _namespaces = Utils.GetPropagatedAttributes(elem.ParentNode as XmlElement);

                    if (elem == null)
                    {
                        // Go throw the referenced items passed in
                        if (refList != null)
                        {
                            foreach (XmlNode node in refList)
                            {
                                XmlElement tempElem = node as XmlElement;
                                if ((tempElem != null) && (Utils.HasAttribute(tempElem, "Id", SignedXml.XmlDsigNamespaceUrl))
                                    && (Utils.GetAttribute(tempElem, "Id", SignedXml.XmlDsigNamespaceUrl).Equals(idref)))
                                {
                                    elem = tempElem;
                                    if (_signedXml._context != null)
                                        _namespaces = Utils.GetPropagatedAttributes(_signedXml._context);
                                    break;
                                }
                            }
                        }
                    }

                    XmlDocument normDocument = Utils.PreProcessElementInput(elem);
                    // Add the propagated attributes
                    Utils.AddNamespaces(normDocument.DocumentElement, _namespaces);

                    if (discardComments)
                    {
                        // We should discard comments before going into the transform chain
                        hashInputStream = TransformChain.TransformToOctetStream(normDocument);
                    }
                    else
                    {
                        // This is an XPointer reference, do not discard comments!!!
                        hashInputStream = TransformChain.TransformToOctetStream(normDocument);
                    }
                }

                // Compute the new hash value
                hashval = _hashAlgorithm.ComputeHash(hashInputStream);
            }
            finally
            {
                if (hashInputStream != null)
                    hashInputStream.Dispose();
            }

            DigestValue = hashval;
        }
    }
}
