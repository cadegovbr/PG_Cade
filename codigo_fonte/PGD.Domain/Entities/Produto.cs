using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class Produto
    {
        public Produto()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public int IdProduto { get; set; }
        public int IdGrupoAtividade { get; set; }
        public virtual OS_GrupoAtividade GrupoAtividade { get; set; }
        public int IdAtividade { get; set; }
        public virtual OS_Atividade Atividade { get; set; }
        public int IdTipoAtividade { get; set; }
        public virtual OS_TipoAtividade TipoAtividade { get; set; }
        public int CargaHoraria { get; set; }
        public int QuantidadeProduto { get; set; }
        public double CargaHorariaProduto { get; set; }
        [MaxLength]
        [Column(TypeName = "varchar(MAX)")]
        public string Observacoes { get; set; }
        [MaxLength]
        [Column(TypeName = "varchar(MAX)")]
        public string ObservacoesAdicionais { get; set; }
        public int IdPacto { get; set; }
        [MaxLength]
        [Column(TypeName = "varchar(MAX)")]
        public string Motivo { get; set; }
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public int Avaliacao { get; set; }

        public bool? EntregueNoPrazo { get; set; }

        public int? IdJustificativa { get; set; }

        public DateTime? DataTerminoReal { get; set; }

        public int? IdSituacaoProduto { get; set; }

        public virtual SituacaoProduto SituacaoProduto { get; set; }

        public virtual Justificativa Justificativa {get; set;}

        public virtual ICollection<IniciativaPlanoOperacionalProduto> IniciativasPlanoOperacionalProduto { get; set; }

        public virtual List<AvaliacaoProduto> Avaliacoes { get; set; }

        public virtual List<AnexoProduto> AnexoProduto { get; set; }

        public bool IsValid()
        {
            return true;
        }

        
    }
}

