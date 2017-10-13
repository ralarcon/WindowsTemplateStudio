﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI.Services
{
    public static class ValidationService
    {
        private static Func<IEnumerable<string>> _getNames;

        public static void Initialize(Func<IEnumerable<string>> getNames)
        {
            _getNames = getNames;
        }

        public static ValidationResult ValidateTemplateName(string templateName, bool includesDefaultNamesValidation, bool includesExistingNamesValidation)
        {
            var validators = new List<Validator>() { new ReservedNamesValidator() };

            if (includesExistingNamesValidation)
            {
                validators.Add(new ExistingNamesValidator(_getNames.Invoke()));
            }

            if (includesDefaultNamesValidation)
            {
                validators.Add(new DefaultNamesValidator());
            }

            return Naming.Validate(templateName, validators);
        }

        public static string InferTemplateName(string templateName, bool includesExistingNamesValidation, bool includesDefaultNamesValidation)
        {
            var validators = new List<Validator>() { new ReservedNamesValidator() };
            if (includesDefaultNamesValidation)
            {
                validators.Add(new DefaultNamesValidator());
            }

            if (includesExistingNamesValidation)
            {
                validators.Add(new ExistingNamesValidator(_getNames.Invoke()));
            }

            return Naming.Infer(templateName, validators);
        }
    }
}
