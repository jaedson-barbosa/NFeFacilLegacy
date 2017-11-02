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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace ServidorCertificacaoConsole.CodigoNet
{
    // This class represents an ordered chain of transforms

    public class TransformChain : IEnumerable<Transform>
    {
        private List<Transform> _transforms;

        public TransformChain()
        {
            _transforms = new List<Transform>();
        }

        public void Add(Transform transform)
        {
            if (transform != null)
                _transforms.Add(transform);
        }

        public IEnumerator GetEnumerator()
        {
            return _transforms.GetEnumerator();
        }

        IEnumerator<Transform> IEnumerable<Transform>.GetEnumerator()
        {
            return _transforms.GetEnumerator();
        }

        public int Count
        {
            get { return _transforms.Count; }
        }

        public Transform this[int index]
        {
            get
            {
                return (Transform)_transforms[index];
            }
        }

        // The goal behind this method is to pump the input stream through the transforms and get back something that
        // can be hashed
        internal Stream TransformToOctetStream(object inputObject, Type inputType)
        {
            object currentInput = inputObject;
            foreach (Transform transform in _transforms)
            {
                if (currentInput == null || currentInput is XmlDocument)
                {
                    //in this case, no translation necessary, pump it through
                    transform.LoadInput(currentInput as XmlDocument);
                    currentInput = transform.GetOutput();
                }
                else
                {
                    throw new CryptographicException();
                }
            }

            // Final processing, either we already have a stream or have to canonicalize
            if (currentInput is Stream)
            {
                return currentInput as Stream;
            }
            throw new CryptographicException();
        }

        internal Stream TransformToOctetStream(XmlDocument document)
        {
            return TransformToOctetStream(document, typeof(XmlDocument));
        }

        internal XmlElement GetXml(XmlDocument document, string ns)
        {
            XmlElement transformsElement = document.CreateElement("Transforms", ns);
            foreach (Transform transform in _transforms)
            {
                if (transform != null)
                {
                    // Construct the individual transform element
                    XmlElement transformElement = transform.GetXml(document);
                    if (transformElement != null)
                        transformsElement.AppendChild(transformElement);
                }
            }
            return transformsElement;
        }
    }
}
