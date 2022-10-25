﻿using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeopleManager.ViewModel.Helpers;

public static class ValidatorExtractor
{
    public static IEnumerable<ValidationResult> Validate(this object source)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        var results = new List<ValidationResult>();
        bool IsValid = Validator.TryValidateObject(source, new ValidationContext(source, null, null), results, true);
        if (IsValid) yield break;
        foreach (var result in results)
        {
            yield return result;
        }

    }
}