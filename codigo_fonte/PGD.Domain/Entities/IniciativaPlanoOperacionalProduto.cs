using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class IniciativaPlanoOperacionalProduto
    {
        [StringLength(5)]
        [Key, Column(Order = 0)]
        public string IdIniciativaPlanoOperacional { get; set; }
        [Key, Column(Order = 1)]
        public int IdProduto { get; set; }
    }
}
