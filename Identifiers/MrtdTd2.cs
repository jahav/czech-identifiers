using System;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    /// <summary>
    /// Machine Readable Travel Document - TD2 format.
    /// The TD2 format is a larger card 74x105 mm with two sections:
    /// <ul>
    /// <li>VIZ - Visual zone, where all data is written with labels, used for visual inspection and can differ slightly between various issuers.</li>
    /// <li>MRZ - Machine Readable Zone which consists of two lines of 36 characters.</li>
    /// </ul>
    /// This interface provides access to the MRZ. The data inside MRZ might differ slightly from data 
    /// inside VIZ, becuase of MTZ character limitations (e.g. hypens are omitted in MTZ, names can be truncated...).
    /// </summary>
    /// <remarks>
    /// Specification of MRZ is at <a href="https://www.icao.int/publications/Documents/9303_p6_cons_en.pdf">chapter 4.2</a>.
    /// </remarks>
    public class MrtdTd2 : IIdentifier
    {
        public MrtdTd2(string documentCode, string issuerCode, Td2Name name, string documentNumber,
            char? documentNumberCheckDigit, string nationality, Td2Date dateOfBirth, Td2Sex sex,
            Td2Date dateOfExpiry, string optionalData)
        {
            // TODO: Add some asserts

            DocumentCode = documentCode;
            IssuerCode = issuerCode;
            Name = name;
            DocumentNumber = documentNumber;
            DocumentNumberCheckDigit = documentNumberCheckDigit;
            Nationality = nationality;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            DateOfExpiry = dateOfExpiry;
            OptionalData = optionalData;
        }

        /// <summary>
        /// A two character long code of the document. In the valid documents, the first character shall be A, C or I. 
        /// The second character shall be at the discretion of the issuing State or organization except that V shall not 
        /// be used, and C shall not be used after A except in the crew member certificate.
        /// </summary>
        string DocumentCode { get; }

        /// <summary>
        /// A three character code of the issuing state or organization. The code is specified in 
        /// <a href="https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf">doc 9303-3, section 5.</a>.
        /// </summary>
        string IssuerCode { get; }

        /// <summary>
        /// Name of the holder.
        /// </summary>
        Td2Name Name { get; }

        /// <summary>
        /// Document number, allows [A-Z0-9] characters.
        /// </summary>
        /// <remarks>The document number can be longer than 9 characters, see 
        /// <a href="https://www.icao.int/publications/Documents/9303_p6_cons_en.pdf">
        /// note j) at 4.2.2</a>.</remarks>
        string DocumentNumber { get; }

        /// <summary>
        /// Check digit of the document number.
        /// </summary>
        char? DocumentNumberCheckDigit { get; }

        /// <summary>
        /// Three letter nationality of the document holder.
        /// </summary>
        /// <remarks><a href="https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf">Doc 9303-3, chapter 3.6</a>.</remarks>
        string Nationality { get; set; }

        /// <summary>
        /// Date of birth of the document holder.
        /// </summary>
        Td2Date DateOfBirth { get; }

        /// <summary>
        /// Sex of the document holder.
        /// </summary>
        Td2Sex Sex { get; }

        /// <summary>
        /// Expiration date of the document.
        /// </summary>
        Td2Date DateOfExpiry { get; }

        /// <summary>
        /// Optional data elements. The optional space might be used for document number, if overflowed. If that happens, 
        /// this getter won't return overflowed document number.
        /// </summary>
        string OptionalData { get; }

        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }

        private bool Validate()
        {
            return ValidateDocumentCode()
                && ValidateIssuerCode()
                && ValidateName();/*
            DocumentNumber = documentNumber;
            DocumentNumberCheckDigit = documentNumberCheckDigit;
            Nationality = nationality;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            DateOfExpiry = dateOfExpiry;
            OptionalData = optionalData;*/
        }

        private bool ValidateDocumentCode()
        {
            if (DocumentCode.Length < 1 || DocumentCode.Length > 2)
            {
                return false;
            }

            var firstIsACI = DocumentCode[0] == 'A'
                    || DocumentCode[0] == 'C'
                    || DocumentCode[0] == 'I';

            if (DocumentCode.Length == 1)
            {
                var isValid = firstIsACI;
                return isValid;
            }

            // 2 characters long
            if (!firstIsACI)
            {
                return false;
            }

            if (DocumentCode[1] == 'V' || DocumentCode == "AC") {
                return false;
            }

            char b = DocumentCode[1];
            return Regex.IsMatch(DocumentCode, "^.[A-Z0-9]$");
        }

        private bool ValidateIssuerCode()
        {
            return Regex.IsMatch(IssuerCode, "^[A-Z]{1,3}$");
        }

        private bool ValidateName()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// <a href="">9303-3, section 3.4 and 4.6</a>
    /// </summary>
    public struct Td2Name
    {
        /// <summary>
        /// Primary identifier of a person, as defined by the issuer.
        /// </summary>
        public string[] Primary { get; }

        /// <summary>
        /// Secondary identifier of a person, as defined by the issuer.
        /// </summary>
        public string[] Secondary { get; }
    }

    /// <summary>
    /// The representation of a date.
    /// </summary>
    /// <remarks>For representation of dates, see <a href="">Doc 9303-3, chapter 4.8</a>.</remarks>
    public struct Td2Date
    {
        /// <summary>
        /// A year in a century, the century itself is unknown.
        /// </summary>
        int? CenturyYear { get; }

        /// <summary>
        /// Month in a year, range 01-12. If unknown, null.
        /// </summary>
        int? Month { get; }

        /// <summary>
        /// Day in a month. If unknown, null.
        /// </summary>
        int? Day { get; }

        /// <summary>
        /// Check digit for the date.
        /// </summary>
        char? CheckDigit { get; }
    }

    public enum Td2Sex
    {
        Unspecified,
        Male,
        Female
    }

    public class MrtdTd2Builder
    {
        private string documentCode = "I";
        private string issuerCode = "CZE";
        private Td2Name name = new Td2Name();
        private string documentNumber = string.Empty;
        private char? documentNumberCheckDigit = null;
        private string nationality = string.Empty;
        private Td2Date dateOfBirth = new Td2Date();
        private Td2Sex sex = Td2Sex.Unspecified;
        private Td2Date dateOfExpiry = new Td2Date();
        private string optionalData = string.Empty;

        public MrtdTd2Builder WithDocumentCode(string documentCode)
        {
            this.documentCode = documentCode;
            return this;
        }

        public MrtdTd2Builder WithIssuerCode(string issuerCode)
        {
            this.issuerCode = issuerCode;
            return this;
        }

        public MrtdTd2 Build()
        {
            return new MrtdTd2(documentCode, issuerCode, name, documentNumber,
                       documentNumberCheckDigit, nationality, dateOfBirth, sex,
                       dateOfExpiry, optionalData);
        }
    }
}
