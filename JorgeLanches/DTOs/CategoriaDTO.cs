﻿using System.ComponentModel.DataAnnotations;

namespace JorgeLanches.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }
    }
}