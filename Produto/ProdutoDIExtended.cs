using Produto;
using Produto.Impostos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BaseGeral.ItensBD
{
    public sealed class ProdutoDIExtended
    {
        ProdutoDI Root { get; }
        List<ImpSimplesArmazenado> impostosSimples;
        List<ICMSArmazenado> icms;

        public ProdutoDIExtended(ProdutoDI root)
        {
            Root = root;
        }

        public IEnumerable<ImpSimplesArmazenado> GetImpSimplesArmazenados()
        {
            if (impostosSimples == null)
            {
                if (!string.IsNullOrEmpty(Root.ImpostosSimples))
                {
                    var xml = XElement.Parse(Root.ImpostosSimples);
                    impostosSimples = xml.FromXElement<List<ImpSimplesArmazenado>>();
                }
                else
                {
                    impostosSimples = new List<ImpSimplesArmazenado>();
                }
            }
            return impostosSimples;
        }

        public IEnumerable<ICMSArmazenado> GetICMSArmazenados()
        {
            if (icms == null)
            {
                if (!string.IsNullOrEmpty(Root.ICMS))
                {
                    var xml = XElement.Parse(Root.ICMS);
                    icms = xml.FromXElement<List<ICMSArmazenado>>();
                }
                else
                {
                    icms = new List<ICMSArmazenado>();
                }
            }
            return icms;
        }

        public void AdicionarImpostoSimples(ImpSimplesArmazenado imp)
        {
            impostosSimples.Add(imp);
            Root.ImpostosSimples = impostosSimples.ToXElement<List<ImpSimplesArmazenado>>()
                .ToString(SaveOptions.DisableFormatting);
        }

        public void AdicionarICMS(ICMSArmazenado imp)
        {
            icms.Add(imp);
            Root.ICMS = icms.ToXElement<List<ICMSArmazenado>>()
                .ToString(SaveOptions.DisableFormatting);
        }

        public void RemoverImpostoSimples(ImpSimplesArmazenado imp)
        {
            impostosSimples.Remove(imp);
            if (impostosSimples.Count > 0)
            {
                Root.ImpostosSimples = impostosSimples.ToXElement<List<ImpSimplesArmazenado>>()
                    .ToString(SaveOptions.DisableFormatting);
            }
            else
            {
                Root.ImpostosSimples = null;
            }
        }

        public void RemoverICMS(ICMSArmazenado imp)
        {
            icms.Remove(imp);
            if (icms.Count > 0)
            {
                Root.ICMS = icms.ToXElement<List<ICMSArmazenado>>()
                    .ToString(SaveOptions.DisableFormatting);
            }
            else
            {
                Root.ICMS = null;
            }
        }

        public (PrincipaisImpostos Tipo, string NomeTemplate, int CST)[] GetImpostosPadrao()
        {
            return string.IsNullOrEmpty(Root.ImpostosPadrao)
                ? null
                : Root.ImpostosPadrao.Split(new string[1] { "&#&" }, StringSplitOptions.RemoveEmptyEntries).Select(x =>
                {
                    var strs = x.Split(new string[1] { "&|&" }, StringSplitOptions.RemoveEmptyEntries);
                    return ((PrincipaisImpostos)int.Parse(strs[0]), strs[1], int.Parse(strs[2]));
                }).ToArray();
        }

        public void SetImpostosPadrao(IEnumerable<ImpostoArmazenado> impostos)
        {
            var imps = impostos.Select(x => $"{(int)x.Tipo}&|&{x.NomeTemplate}&|&{x.CST}&#&");
            Root.ImpostosPadrao = string.Concat(imps);
        }

        public static implicit operator ProdutoDIExtended(ProdutoDI prod)
        {
            return new ProdutoDIExtended(prod);
        }
    }
}
