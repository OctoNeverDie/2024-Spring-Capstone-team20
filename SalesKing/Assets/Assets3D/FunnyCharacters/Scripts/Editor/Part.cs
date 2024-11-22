﻿using System.Collections.Generic;
using System.Linq;

namespace CharacterCustomization
{
    public class Part
    {
        public readonly PartType Type;
        public readonly List<Variant> Variants;

        public Variant SelectedVariant;
        public bool IsEnabled = true;

        public int VariantIndex => Variants.IndexOf(SelectedVariant);

        public Part(PartType type, List<Variant> variants)
        {
            Type = type;
            SelectedVariant = variants.First();
            Variants = variants;
        }

        public void SelectVariant(int index)
        {
            SelectedVariant = Variants[index];
        }
    }
}